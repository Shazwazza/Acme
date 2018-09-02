using System;
using System.Threading;
using System.Threading.Tasks;

namespace Acme.Application.Interfaces
{
    public interface IDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
        void NotifySuccessHandlers();
        void CancelSuccessHandlers();
        void OnChangesSaved(Action action);
        Task MigrateAsync();
    }
}