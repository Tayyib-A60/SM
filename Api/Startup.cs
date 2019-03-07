using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SkineroMotors.Core;
using SkineroMotors.Core.Models;
using SkineroMotors.Extensions;
using SkineroMotors.Persistence;

namespace SkineroMotors
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var appSettingsSection = Configuration.GetSection("AppSettings");
            // services.BuildServiceProvider().GetService<SMDbContext>().Database.Migrate();
            services.AddDbContext<SMDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Default")));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            // services.BuildServiceProvider().GetService<SMDbContext>().Database.Migrate();(production)
            // services.AddCors();
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.Configure<PhotoSettings>(Configuration.GetSection("PhotoSettings"));
            services.AddScoped<IContactUsRepository, ContactUsRepository>();
            services.AddScoped<IPhotoRepository,PhotoRepository>();

            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddAutoMapper();
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(auth => {
               auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
               auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x => {
               x.RequireHttpsMetadata = false;
               x.SaveToken = true;
               x.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(key),
                   ValidateIssuer = false,
                   ValidateAudience = false
               };
            });
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // else
            // {
            //     app.UseExceptionHandler("/Error");
            //     app.UseHsts();
            // }

            app.Use(async (context, next) => {
                await next();

                if(context.Response.StatusCode == 204 )
                {
                    context.Response.ContentLength = 0;
                    await next();
                }

                if(context.Response.StatusCode == 404 ) // && !Path.HasExtension(context.Request.Path.Value
                {
                    context.Request.Path = "/index.html";
                    await next();
                }
            });

            app.UseAuthentication();
            // app.UseHttpsRedirection();
            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials());
            // app.UseDefaultFiles();(production)
            // app.UseStaticFiles();(production)
            app.UseMvc();

            // app.UseSpaStaticFiles();

            // app.UseMvc(routes =>
            // {
            //     routes.MapRoute(
            //         name: "default",
            //         template: "{controller}/{action=Index}/{id?}");
            // });

            // app.UseSpa(spa =>
            // {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                // spa.Options.SourcePath = "ClientApp";
                // spa.UseSpaPrerendering(options =>
                // {
                //     options.BootModulePath = $"{spa.Options.SourcePath}/dist-server/main.bundle.js";
                //     options.BootModuleBuilder = env.IsDevelopment()
                //         ? new AngularCliBuilder(npmScript: "build:ssr")
                //         : null;
                //     options.ExcludeUrls = new[] { "/sockjs-node" };
                // });

            //     if (env.IsDevelopment())
            //     {
            //         // --extract--css
            //         spa.UseAngularCliServer(npmScript: "start");
            //         // spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
            //     }
            // });
        }
    }
}
