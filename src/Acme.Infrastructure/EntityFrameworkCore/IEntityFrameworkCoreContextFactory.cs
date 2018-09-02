using Acme.Application.Interfaces;

namespace Acme.Infrastructure.EntityFrameworkCore
{
    public interface IEntityFrameworkCoreContextFactory
    {
        EntityFrameworkCoreContext Create();
        EntityFrameworkCoreContext CreateReadOnly();
        EntityFrameworkCoreContext Concretize(IDbContext ctx);
    }
}