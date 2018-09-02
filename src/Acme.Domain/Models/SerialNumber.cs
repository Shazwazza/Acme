using System;
using System.ComponentModel.DataAnnotations;

namespace Acme.Domain.Models
{
    public class SerialNumber : EntityBase
    {
        [Key]
        [Required]
        public Guid Code { get; set; }
    }
}