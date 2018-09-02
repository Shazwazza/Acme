using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Acme.Domain.Interfaces;

namespace Acme.Domain.Models
{
    public class Submission : EntityBase, ISubmission
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; internal set; }

        [Required]
        [MinLength(ValidationConstants.Submission.FirstName.MinLength)]
        [MaxLength(ValidationConstants.Submission.LastName.MaxLength)]
        public string FirstName { get; set; }
        
        [Required]
        [MinLength(ValidationConstants.Submission.LastName.MinLength)]
        [MaxLength(ValidationConstants.Submission.LastName.MaxLength)]
        public string LastName { get; set; }
        
        [Required]
        public DateTimeOffset Birthday { get; set; }
        
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
        
        [Required]
        [ForeignKey("FK_Submission_SerialNumber")]
        public Guid SerialNumberCode { get; set; }
        
    }
}