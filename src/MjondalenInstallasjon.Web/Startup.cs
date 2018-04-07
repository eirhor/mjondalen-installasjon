using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MjondalenInstallasjon.Data;
using MjondalenInstallasjon.Data.Data;
using MjondalenInstallasjon.Identity;
using MjondalenInstallasjon.Identity.Data;
using MjondalenInstallasjon.Identity.Services;
using React.AspNet;

namespace MjondalenInstallasjon.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IIdentityService, IdentityService>();
            services.AddReact();

            var connectionString = Configuration.GetConnectionString("MjondalenInstallasjon");
            services.ConfigureData(connectionString);
            services.ConfigureIdentity(connectionString);
            
            return services.BuildServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetService<ApplicationContext>().Database.EnsureCreated();
                scope.ServiceProvider.GetService<ApplicationIdentityContext>().Database.EnsureCreated();
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseReact(config =>
            {
                config
                    .SetReuseJavaScriptEngines(true)
                    .SetLoadBabel(false)
                    .SetLoadReact(false)
                    .AddScriptWithoutTransform("~/public/server.js");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}