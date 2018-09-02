using System;
using System.ComponentModel.DataAnnotations;

namespace Acme.Presentation.Website.Models
{
    public class SerialNumberViewModel
    {
        [Display(Name = "Serial number")]
        public Guid SerialNumberCode { get; set; }

        [Display(Name = "Creation time")]
        public DateTimeOffset CreatedAt { get; set; }
    }
}