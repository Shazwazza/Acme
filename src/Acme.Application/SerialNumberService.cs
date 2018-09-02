using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Acme.Application.Interfaces;
using Acme.Domain.Models;

namespace Acme.Application
{
    public class SerialNumberService : ISerialNumberService
    {
        private readonly ISerialNumberRepository _serialNumberRepository;

        public SerialNumberService(ISerialNumberRepository serialNumberRepository)
        {
            _serialNumberRepository = serialNumberRepository;
        }

        public Task<PagingResult<SerialNumber>> ListAsync(PagingInformation pagingInformation, CancellationToken cancellationToken)
        {
            return _serialNumberRepository.ListAsync(pagingInformation, cancellationToken);
        }
    }
}