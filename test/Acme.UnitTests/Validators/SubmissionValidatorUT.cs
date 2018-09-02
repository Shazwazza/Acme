using System;
using System.Threading;
using System.Threading.Tasks;
using Acme.Application.Interfaces;
using Acme.Domain;
using Acme.Domain.Models;
using Acme.Infrastructure.FluentValidation.Validators;
using Acme.UnitTests.Helpers;
using AutoFixture.NUnit3;
using FluentValidation.TestHelper;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Acme.UnitTests.Validators
{
    [TestFixture]
    public class SubmissionValidatorUT
    {
        [SetUp]
        public void SetUp()
        {
            _object = new Submission
            {
                Birthday = DateTimeOffset.Now.AddYears(years: -20),
                LastName = "LastName",
                FirstName = "FirstName",
                EmailAddress = "firstname@lastname.com",
                SerialNumberCode = Guid.Empty
            };
        }

        private Submission _object;


        private void SetupNumberOfUsagesAs(Mock<ISubmissionRepository> submissionRepositoryMock, bool valid)
        {
            submissionRepositoryMock
                .Setup(x => x.EnsureNumberOfUsagesLessThanToAsync(
                                                                         _object.SerialNumberCode,
                                                                         ValidationConstants.Submission.SerialNumberCode
                                                                                            .MaxNumberOfSerialNumberUsages,
                                                                         CancellationToken.None))
                .Returns(Task.FromResult(valid));
        }

        private void SetupSerialNumberAs(Mock<ISerialNumberRepository> serialNumberRepositoryMock, bool valid)
        {
            serialNumberRepositoryMock.Setup(x => x.ExistsAsync(_object.SerialNumberCode, CancellationToken.None))
                                      .Returns(Task.FromResult(valid));
        }

        [Test]
        [AutoMoqData]
        public void Email_invaid(
            [Frozen] Mock<ISubmissionRepository> submissionRepositoryMock,
            [Frozen] Mock<ISerialNumberRepository> serialNumberRepositoryMock,
            SubmissionValidator sut)
        {
            var invalidEmails = new[]
            {
                "",
                null,
                "invalid@email..com",
                "invalid@emailcom",
                "invalid(at)email.com",
                "invalid email.com",
            };

            foreach (var invalidEmail in invalidEmails)
            {
                _object.EmailAddress = invalidEmail;

                sut.ShouldHaveValidationErrorFor(x => x.EmailAddress, _object);
            }

            
        }
        
        [Test]
        [AutoMoqData]
        public void BirthDay_Not_Meet(
            [Frozen] Mock<ISubmissionRepository> submissionRepositoryMock,
            [Frozen] Mock<ISerialNumberRepository> serialNumberRepositoryMock,
            SubmissionValidator sut)
        {
            _object.Birthday = DateTimeOffset.Now;

            sut.ShouldHaveValidationErrorFor(x => x.Birthday, _object);
        }

        [Test]
        [AutoMoqData]
        public void SerialNumberCode_Invalid(
            [Frozen] Mock<ISubmissionRepository> submissionRepositoryMock,
            [Frozen] Mock<ISerialNumberRepository> serialNumberRepositoryMock,
            SubmissionValidator sut)
        {
            SetupSerialNumberAs(serialNumberRepositoryMock, valid: false);
            SetupNumberOfUsagesAs(submissionRepositoryMock, valid: true);

            sut.ShouldHaveValidationErrorFor(x => x.SerialNumberCode, _object);
        }
        
        [Test]
        [AutoMoqData]
        public void SerialNumberCode_used_too_many_times(
            [Frozen] Mock<ISubmissionRepository> submissionRepositoryMock,
            [Frozen] Mock<ISerialNumberRepository> serialNumberRepositoryMock,
            SubmissionValidator sut)
        {
            SetupSerialNumberAs(serialNumberRepositoryMock, valid: true);
            SetupNumberOfUsagesAs(submissionRepositoryMock, valid: false);

            sut.ShouldHaveValidationErrorFor(x => x.SerialNumberCode, _object);
        }

        [Test]
        [AutoMoqData]
        public void Valid(
            [Frozen] Mock<ISubmissionRepository> submissionRepositoryMock,
            [Frozen] Mock<ISerialNumberRepository> serialNumberRepositoryMock,
            SubmissionValidator sut)
        {
            SetupSerialNumberAs(serialNumberRepositoryMock, valid: true);
            SetupNumberOfUsagesAs(submissionRepositoryMock, valid: true);

            var result = sut.Validate(_object);

            Assert.IsTrue(result.IsValid, JsonConvert.SerializeObject(result, Formatting.Indented));
        }
        
    }
}