using System;
using System.Threading.Tasks;
using Acme.Application;
using Acme.Application.Interfaces;
using Acme.Domain.Interfaces;
using Acme.Infrastructure.EntityFrameworkCore;
using Acme.Infrastructure.EntityFrameworkCore.Repositories;
using Acme.Infrastructure.FluentValidation.Validators;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Acme.Infrastructure.Bootstrapping
{
    public static class ServiceCollectionExtensions
    {
        public static async Task AddAcmeApplicationAsync(
            this IServiceCollection serviceCollection,
            string connectionString,
            bool migrateDoingStartup)
        {
            //Services
            serviceCollection.AddSingleton<ISubmissionService, SubmissionService>();
            serviceCollection.AddSingleton<ISerialNumberService, SerialNumberService>();

            //Repositories
            serviceCollection.AddSingleton<ISubmissionRepository, SubmissionRepository>();
            serviceCollection.AddSingleton<ISerialNumberRepository, SerialNumberRepository>();

            //Validators
            serviceCollection.AddSingleton<IValidator<ISubmission>, SubmissionValidator>();

            //Db context
            var contextFactory = new ContextFactory(connectionString);
            serviceCollection.AddSingleton<IContextFactory>(contextFactory);
            serviceCollection.AddSingleton<IEntityFrameworkCoreContextFactory>(contextFactory);
            serviceCollection.AddSingleton<IDbContext, EntityFrameworkCoreContext>();

            //Bootstrapping
            using (var scope = serviceCollection.BuildServiceProvider().CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                try
                {
                    await DbInitializer.Initialize(contextFactory.Create(), migrateDoingStartup);
                }
                catch (Exception ex)
                {
                    var logger = serviceProvider.GetRequiredService<ILogger>();

                    logger.LogError(ex, message: "An error occurred while seeding the database.");
                    throw;
                }
            }
        }
    }
}