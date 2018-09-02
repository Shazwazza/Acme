using System;
using System.Threading;
using System.Threading.Tasks;
using Acme.Application.Interfaces;
using Acme.Domain;
using Acme.Domain.Interfaces;
using Acme.Domain.Models;
using FluentValidation;

namespace Acme.Infrastructure.FluentValidation.Validators
{
    public class SubmissionValidator : AbstractValidator<ISubmission>, Application.Interfaces.IValidator<ISubmission>
    {
        public SubmissionValidator(
            ISerialNumberRepository serialNumberRepository,
            ISubmissionRepository submissionRepository)
        {
            RuleFor(x => x.Birthday)
                .NotNull()
                .LessThanOrEqualTo(
                                      DateTimeOffset.Now.AddYears(-ValidationConstants
                                                                   .Submission.Birthday.MinimumAgeNumberOfInYears));
            RuleFor(x => x.EmailAddress).NotNull().EmailAddress();
            RuleFor(x => x.FirstName)
                .NotNull()
                .MaximumLength(ValidationConstants.Submission.FirstName.MaxLength)
                .MinimumLength(ValidationConstants.Submission.FirstName.MinLength);
            RuleFor(x => x.LastName)
                .NotNull()
                .MaximumLength(ValidationConstants.Submission.LastName.MaxLength)
                .MinimumLength(ValidationConstants.Submission.LastName.MinLength);

            RuleFor(x => x.SerialNumberCode).NotNull()
                .MustAsync(
                           (code, cancellationToken) => serialNumberRepository
                               .ExistsAsync(code, cancellationToken))    
                .WithMessage(errorMessage: "The serial number is not valid.")
                .MustAsync(
                           (code, cancellationToken) => submissionRepository
                               .EnsureNumberOfUsagesLessThanToAsync(code,
                                                                           ValidationConstants
                                                                               .Submission.SerialNumberCode
                                                                               .MaxNumberOfSerialNumberUsages,
                                                                           cancellationToken))
                .WithMessage($"The serial number cannot be used more than {ValidationConstants.Submission.SerialNumberCode.MaxNumberOfSerialNumberUsages} times");
        }

        public async Task<bool> ValidateIsValidAsync(ISubmission instance,
                                              CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await base.ValidateAsync(instance, cancellationToken);

            return result.IsValid;
        }
    }
}