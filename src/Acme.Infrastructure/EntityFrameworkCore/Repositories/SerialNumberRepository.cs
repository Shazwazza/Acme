using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Acme.Application.Interfaces;
using Acme.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Acme.Infrastructure.EntityFrameworkCore.Repositories
{
    public class SerialNumberRepository : ISerialNumberRepository
    {
        private readonly IEntityFrameworkCoreContextFactory _contextFactory;

        public SerialNumberRepository(IEntityFrameworkCoreContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<PagingResult<SerialNumber>> ListAsync(
            PagingInformation pagingInformation,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var ctx = _contextFactory.CreateReadOnly())
            {
                return await ctx.ValidSerialNumbers.ToPagingResultAsync(pagingInformation, cancellationToken);
            }
        }

        public async Task<bool> ExistsAsync(Guid code, CancellationToken cancellationToken)
        {
            using (var ctx = _contextFactory.CreateReadOnly())
            {
                return await ctx.ValidSerialNumbers.AnyAsync(x => x.Code.Equals(code), cancellationToken);
            }
        }
    }
}