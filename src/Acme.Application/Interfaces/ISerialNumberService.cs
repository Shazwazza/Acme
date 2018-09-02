using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Acme.Domain.Models;

namespace Acme.Application.Interfaces
{
    public interface ISerialNumberService
    {
        Task<PagingResult<SerialNumber>> ListAsync(PagingInformation pagingInformation, CancellationToken cancellationToken);
    }
}