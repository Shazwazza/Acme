using System.Threading;
using System.Threading.Tasks;
using Acme.Application.Interfaces;
using Acme.Domain.Models;
using Acme.Presentation.Website.Controllers;
using Acme.UnitTests.Helpers;
using AutoFixture.NUnit3;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace Acme.UnitTests.Controllers
{
    public class SubmissionControllerUT
    {
        [Test]
        [AutoMoqData]
        public async Task List_Returns_paging_result(
            [Frozen] Mock<ISubmissionService> submissionServiceMock,
            SubmissionController sut,
            PagingInformation pagingInformation,
            PagingResult<Submission> submissions)
        {
            submissionServiceMock
                .Setup(x => x.ListAsync(pagingInformation, CancellationToken.None))
                .Returns(Task.FromResult(submissions));
            
            var response = await sut.List(pagingInformation, CancellationToken.None);

            Assert.IsInstanceOf<ViewResult>(response);
            
            submissionServiceMock.VerifyAll();
        }
    }
}