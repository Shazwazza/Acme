using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Acme.Application.Interfaces;
using Acme.Domain.Models;
using Acme.Presentation.Web.Controllers;
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
        public void List_Returns_paging_result(
            SubmissionController sut, 
            PagingInformation pagingInformation)
        {
            var response = sut.List(pagingInformation, CancellationToken.None);
     
            Assert.IsInstanceOf<ActionResult<PagingResult<Submission>>>(response.Result);
        }
    }
}