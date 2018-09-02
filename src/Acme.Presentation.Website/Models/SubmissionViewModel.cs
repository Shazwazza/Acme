using System;
using System.ComponentModel.DataAnnotations;
using Acme.Domain.Interfaces;

namespace Acme.Presentation.Website.Models
{
    public class SubmissionViewModel : ISubmission
    {
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Display(Name = "Birthday")]
        public DateTimeOffset Birthday { get; set; }

        [Display(Name = "Email address")]
        public string EmailAddress { get; set; }

        [Display(Name = "Serial number")]
        public Guid SerialNumberCode { get; set; }

        [Display(Name = "Age")]
        public int Age => CalculateAge(Birthday.DateTime, DateTimeOffset.Now.DateTime);

        [Display(Name = "Submission time")]
        public DateTimeOffset SubmissionTime { get; set; }
        
        private int CalculateAge(DateTime birthDate, DateTime now)
        {
            int age = now.Year - birthDate.Year;

            if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day))
                age--;

            return age;
        }
    }
}