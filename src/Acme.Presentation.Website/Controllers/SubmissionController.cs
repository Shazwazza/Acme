using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Acme.Application.Interfaces;
using Acme.Domain.Models;
using Acme.Presentation.Website.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Acme.Presentation.Website.Controllers
{
    public class SubmissionController : Controller
    {
        private readonly ISubmissionService _submissionService;

        public SubmissionController(ISubmissionService submissionService)
        {
            _submissionService = submissionService;
        }

        [Authorize]
        // GET
        public async Task<IActionResult> List(PagingInformation pagingInformation, CancellationToken cancellationToken)
        {

            var model = await _submissionService.ListAsync(pagingInformation, cancellationToken);
            return View(MaoToViewModel(model));
        }

        private PagingResult<SubmissionViewModel> MaoToViewModel(PagingResult<Submission> model)
        {
            return new PagingResult<SubmissionViewModel>()
            {
                TotalHits = model.TotalHits,
                PagingInformation = model.PagingInformation,
                Hits = model.Hits.Select(MapToViewModel).ToList(),
            };
        }

        private SubmissionViewModel MapToViewModel(Submission model)
        {
            if (model == null)
            {
                return null;
            }
            return new SubmissionViewModel()
            {
                SerialNumberCode = model.SerialNumberCode,
                Birthday = model.Birthday,
                EmailAddress = model.EmailAddress,
                FirstName = model.FirstName,
                LastName = model.LastName,
                SubmissionTime = model.CreatedAt,
            };
        }
    }
}