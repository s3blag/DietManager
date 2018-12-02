using AutoMapper;
using DM.Database;
using DM.Logic.Interfaces;
using DM.Logic.Services;
using DM.Models.Config;
using DM.Repositories;
using DM.Repositories.Interfaces;
using LinqToDB.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace Diet_Manager
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("config.json", optional: false, reloadOnChange: true)
                .AddJsonFile("achievementsConfig.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddAutoMapper();
            
            DataConnection.DefaultSettings = new DBConnectionSettings(Configuration["ConnectionStrings:PostgreSQLBaseConnection"]);

            services.AddAuthentication(cfg =>
            {
                cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
               .AddCookie()
               .AddJwtBearer(cfg =>
               {
                   cfg.RequireHttpsMetadata = false;
                   cfg.SaveToken = true;
                   cfg.TokenValidationParameters = new TokenValidationParameters()
                   {
                       ValidateIssuerSigningKey = true,
                       ValidIssuer = Configuration["SecuritySettings:Issuer"],
                       ValidAudience = Configuration["SecuritySettings:Audience"],
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecuritySettings:Key"]))
                   };
               });

            services.Configure<ImageServiceConfig>(options => Configuration.GetSection("ImageServiceConfig").Bind(options));
            services.Configure<AchievementsConfig>(options => Configuration.GetSection("AchievementsConfig").Bind(options));
            services.Configure<SecuritySettings>(options => Configuration.GetSection("SecuritySettings").Bind(options));
            
            services.AddScoped<IMealRepository, MealRepository>();
            services.AddScoped<IMealIngredientRepository, MealIngredientRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IFavouriteRepository, FavouriteRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserAchievementRepository, UserAchievementRepository>();
            services.AddScoped<IAchievementRepository, AchievementRepository>();
            services.AddScoped<IFriendRepository, FriendRepository>();
            services.AddScoped<IMealScheduleRepository, MealScheduleRepository>();
            services.AddScoped<IActivityRepository, ActivityRepository>();
            services.AddScoped<ISecurityService, SecurityService>();

            services.AddScoped<IMealService, MealService>();
            services.AddScoped<IMealIngredientService, MealIngredientService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<ISearchService, SearchService>();
            services.AddScoped<IMealIngredientsApiCaller, MealIngredientsApiCaller>();
            services.AddScoped<IAchievementService, AchievementService>();
            services.AddScoped<IMealScheduleService, MealScheduleService>();
            services.AddScoped<IActivityService, ActivityService>();
            services.AddScoped<IFavouritesService, FavouritesService>();
            services.AddScoped<IFriendService, FriendService>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IContentTypeProvider, FileExtensionContentTypeProvider>();

            services.AddSingleton<ICacheContainer, AchievementsCacheContainer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            AchievementsSetup.SetupAchievements(
                serviceProvider.GetRequiredService<IOptions<AchievementsConfig>>().Value,
                serviceProvider.GetRequiredService<IAchievementRepository>()
                ).Wait();

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
