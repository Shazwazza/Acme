using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Acme.Application.Interfaces;
using Acme.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Acme.Infrastructure.EntityFrameworkCore.Repositories
{
    public class SubmissionRepository : ISubmissionRepository
    {
        private readonly IEntityFrameworkCoreContextFactory _contextFactory;

        public SubmissionRepository(IEntityFrameworkCoreContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<PagingResult<Submission>> ListAsync(
            PagingInformation pagingInformation,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var ctx = _contextFactory.CreateReadOnly())
            {
                return await ctx.Submissions.ToPagingResultAsync(pagingInformation, cancellationToken);
            }
        }

        public async Task<Submission> CreateAsync(IDbContext context, Submission submission,
                                                  CancellationToken cancellationToken =
                                                      default(CancellationToken))
        {
 
            var ctx = _contextFactory.Concretize(context);

            var result = await ctx.Submissions.AddAsync(submission, cancellationToken);

            return result.Entity;
        }

        public async Task<bool> EnsureNumberOfUsagesLessThanToAsync(Guid serialNumberCode, int value,
                                                                           CancellationToken cancellationToken =
                                                                               default(CancellationToken))
        {
            using (var ctx = _contextFactory.CreateReadOnly())
            {
                var numberOfUsages = await ctx.Submissions
                                              .Where(x => x.SerialNumberCode.Equals(serialNumberCode))
                                              .CountAsync(cancellationToken);

                return numberOfUsages < value;
            }
        }
    }
}