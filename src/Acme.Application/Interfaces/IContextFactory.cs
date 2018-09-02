using System;
using System.Threading.Tasks;

namespace Acme.Application.Interfaces
{
    public interface IContextFactory
    {
        IDbContext CreateReadOnly();
        Task<T> Write<T>(Func<IDbContext, Task<T>> func);
        Task Write(Func<IDbContext, Task> func);
    }
}