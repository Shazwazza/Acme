using System;
using Acme.Infrastructure.Bootstrapping;
using Acme.Infrastructure.FluentValidation.Validators;
using Acme.Presentation.Website.Data;
using Acme.Presentation.Website.Models;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;

namespace Acme.Presentation.Website
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            Log.Logger = new LoggerConfiguration()
                         .Enrich.FromLogContext()
                         .WriteTo
                         .Elasticsearch(new ElasticsearchSinkOptions(new Uri(Configuration
                                                                                 .GetConnectionString(name:
                                                                                                      "Elasticsearch")))
                         {
                             MinimumLogEventLevel = LogEventLevel.Verbose,
                             AutoRegisterTemplate = true
                         })
                         .CreateLogger();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                                                            options.UseSqlServer(
                                                                              Configuration
                                                                                  .GetConnectionString(name:
                                                                                                       "MsSql")));
            services.AddDefaultIdentity<IdentityUser>()
                    .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                    .AddFluentValidation(x =>
                    {
                        x.ImplicitlyValidateChildProperties = true;
               
                        x.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                        x.RegisterValidatorsFromAssembly( typeof(SubmissionValidator).Assembly);
                        
   
                    });;

            var migrateDoingStartup = Configuration.GetValue<bool>("Migrations:ExecuteDoingStartup");
            services.AddAcmeApplicationAsync(Configuration.GetConnectionString("MsSql"), migrateDoingStartup);
            services.AddTransient<IValidator<SubmissionViewModel>, SubmissionValidator>();

            if (migrateDoingStartup)
            {
                using (var serviceScope = services.BuildServiceProvider().CreateScope())
                {
                    var dbContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                    dbContext.Database.Migrate();
                }
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler(errorHandlingPath: "/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

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