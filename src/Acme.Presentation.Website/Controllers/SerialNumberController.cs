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
    public class SerialNumberController : Controller
    {
        private readonly ISerialNumberService _serialNumberService;

        public SerialNumberController(ISerialNumberService serialNumberService)
        {
            _serialNumberService = serialNumberService;
        }

        [Authorize]
        // GET
        public async Task<IActionResult> List(PagingInformation pagingInformation, CancellationToken cancellationToken)
        {

            var model = await _serialNumberService.ListAsync(pagingInformation, cancellationToken);
            return View(MaoToViewModel(model));
        }

        private PagingResult<SerialNumberViewModel> MaoToViewModel(PagingResult<SerialNumber> model)
        {
            return new PagingResult<SerialNumberViewModel>()
            {
                TotalHits = model.TotalHits,
                PagingInformation = model.PagingInformation,
                Hits = model.Hits.Select(MapToViewModel).ToList(),
            };
        }

        private SerialNumberViewModel MapToViewModel(SerialNumber model)
        {
            if (model == null)
            {
                return null;
            }
            return new SerialNumberViewModel()
            {
                SerialNumberCode = model.Code,
                CreatedAt = model.CreatedAt,
            };
        }
    }
}