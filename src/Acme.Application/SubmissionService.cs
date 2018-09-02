using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Acme.Application.Interfaces;
using Acme.Domain.Interfaces;
using Acme.Domain.Models;

namespace Acme.Application
{
    public class SubmissionService : ISubmissionService
    {
        private readonly IContextFactory _contextFactory;
        private readonly ISubmissionRepository _submissionRepository;
        private readonly IValidator<ISubmission> _validator;

        public SubmissionService(
            IContextFactory contextFactory,
            ISubmissionRepository submissionRepository,
            IValidator<ISubmission> validator)
        {
            _contextFactory = contextFactory;
            _submissionRepository = submissionRepository;
            _validator = validator;
        }

        public Task<PagingResult<Submission>> ListAsync(
            PagingInformation pagingInformation,
            CancellationToken cancellationToken = default(CancellationToken)
        )
        {
            return _submissionRepository.ListAsync(pagingInformation, cancellationToken);
        }

        public async Task<Submission> CreateAsync(
            Submission submission,
            CancellationToken cancellationToken = default(CancellationToken)
        )
        {
            if (!await _validator.ValidateIsValidAsync(submission, cancellationToken)
                                .ConfigureAwait(continueOnCapturedContext: false))
            {
                throw new ValidationException(message: "Invalid submission");
            }

            return await _contextFactory.Write(async context =>
            {
                var result = await _submissionRepository.CreateAsync(context, submission, cancellationToken);

                await context.SaveChangesAsync(cancellationToken);

                return result;
            }).ConfigureAwait(continueOnCapturedContext: false);
        }
    }
}