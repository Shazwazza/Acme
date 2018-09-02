using System.Threading;
using Acme.Domain.Models;
using Acme.Presentation.Website.Controllers;
using Acme.UnitTests.Helpers;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace Acme.UnitTests.Controllers
{
    public class SubmissionControllerUT
    {
        [Test]
        [AutoMoqData]
        public void List_Returns_paging_result(
            SubmissionController sut,
            PagingInformation pagingInformation)
        {
            var response = sut.List(pagingInformation, CancellationToken.None);

            Assert.IsInstanceOf<ActionResult<PagingResult<Submission>>>(response.Result);
        }
    }
}