using System;
using System.Threading.Tasks;
using Acme.Application.Interfaces;

namespace Acme.Infrastructure.EntityFrameworkCore
{
    public class ContextFactory : IContextFactory, IEntityFrameworkCoreContextFactory
    {
        private readonly string _connectionString;

        public ContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        IDbContext IContextFactory.CreateReadOnly()
        {
            return CreateReadOnly();
        }

        public async Task<T> Write<T>(Func<IDbContext, Task<T>> action)
        {
            using (var context = Create())
            {
                var result = await action(context);

                try
                {
                    await context.SaveChangesAsync();
                }
                catch
                {
                    context.CancelSuccessHandlers();
                    throw;
                }

                context.NotifySuccessHandlers();

                return result;
            }
        }

        public Task Write(Func<IDbContext, Task> func)
        {
            return Write<bool>(ctx =>
            {
                func(ctx);
                return Task.FromResult(true);
            });
        }

        public EntityFrameworkCoreContext Create()
        {
            return new EntityFrameworkCoreContext(_connectionString);
        }

        public EntityFrameworkCoreContext CreateReadOnly()
        {
            var context = new EntityFrameworkCoreContext(_connectionString);

            context.ChangeTracker.AutoDetectChangesEnabled = false;

            return context;
        }

        public EntityFrameworkCoreContext Concretize(IDbContext ctx)
        {
            return ctx as EntityFrameworkCoreContext;
        }
    }
}