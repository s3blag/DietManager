using AutoMapper;
using DM.Database;
using DM.Repositories;
using DM.Repositories.Interfaces;
using LinqToDB.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
//using IdentityRole = AspNetCore.Identity.PostgreSQL.IdentityRole;
//using User = DM.Logic.Models.User.User;

namespace Diet_Manager
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddIdentity<User, IdentityRole>()
            //    .AddUserStore<UserStore<User>>()
            //    .AddRoleStore<RoleStore<IdentityRole>>()
            //    .AddRoleManager<RoleManager<IdentityRole>>()
            //    .AddDefaultTokenProviders();

            services.AddMvc();

            services.AddAutoMapper();
            
            DataConnection.DefaultSettings = new DBConnectionSettings();

            services.AddScoped<IMealRepository, MealRepository>();


            services.AddSingleton(_ => Configuration);
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
