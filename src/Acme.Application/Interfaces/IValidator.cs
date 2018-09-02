using System.Threading;
using System.Threading.Tasks;

namespace Acme.Application.Interfaces
{
    public interface IValidator<T>
    {
        Task<bool> ValidateIsValidAsync(T instance, CancellationToken cancellationToken = default(CancellationToken));
    }
}