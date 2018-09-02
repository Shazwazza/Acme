using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Acme.Domain.Models;

namespace Acme.Application.Interfaces
{
    public interface ISubmissionService
    {
        Task<PagingResult<Submission>> ListAsync(PagingInformation pagingInformation,
                                                             CancellationToken cancellationToken =
                                                                 default(CancellationToken));

        Task<Submission> CreateAsync(Submission submission,
                                               CancellationToken cancellationToken = default(CancellationToken));
    }
}