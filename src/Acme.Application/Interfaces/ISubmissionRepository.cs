using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Acme.Domain.Models;

namespace Acme.Application.Interfaces
{
    public interface ISubmissionRepository
    {
        Task<PagingResult<Submission>> ListAsync(
            PagingInformation pagingInformation,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<Submission> CreateAsync(
            IDbContext context, Submission submission,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<bool> EnsureNumberOfUsagesLessThanToAsync(
            Guid serialNumberCode,
            int value,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}