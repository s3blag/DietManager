using AutoMapper;
using DM.Database;
using DM.Logic.Interfaces;
using DM.Logic.Services;
using DM.Models.Config;
using DM.Repositories;
using DM.Repositories.Interfaces;
using LinqToDB.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
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
                .AddJsonFile("config.json", optional: true, reloadOnChange: true)
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

            services.AddLinq2Identity<Guid>();

            services.AddAuthentication()
               .AddCookie()
               .AddJwtBearer(cfg =>
               {
                   cfg.TokenValidationParameters = new TokenValidationParameters()
                   {
                       ValidIssuer = Configuration["Tokens:Issuer"],
                       ValidAudience = Configuration["Tokens:Audience"],
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"]))
                   };
               });

            services.Configure<ImageServiceConfig>(options => Configuration.GetSection("ImageServiceConfig").Bind(options));

            services.AddScoped<IMealRepository, MealRepository>();
            services.AddScoped<IMealIngredientRepository, MealIngredientRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IFavouriteRepository, FavouriteRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IMealService, MealService>();
            services.AddScoped<IMealIngredientService, MealIngredientService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<ISearchService, SearchService>();
            services.AddScoped<IMealIngredientsApiCaller, MealIngredientsApiCaller>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
