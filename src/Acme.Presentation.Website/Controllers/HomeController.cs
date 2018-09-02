using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Acme.Application.Interfaces;
using Acme.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Acme.Presentation.Website.Models;

namespace Acme.Presentation.Website.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISubmissionService _submissionService;

        public HomeController(ISubmissionService submissionService)
        {
            _submissionService = submissionService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        
        
        [HttpPost]
        public async Task<IActionResult> Index(SubmissionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var created = await _submissionService.CreateAsync(new Submission()
            {
                SerialNumberCode = model.SerialNumberCode,
                Birthday = model.Birthday,
                EmailAddress = model.EmailAddress,
                FirstName = model.FirstName,
                LastName = model.LastName,
            });
            
            return RedirectToAction(nameof(SubmissionReceived));
        }

        public IActionResult Privacy()
        {
            return View();
        }
        
        
        public IActionResult SubmissionReceived()
        {
            return View();
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}