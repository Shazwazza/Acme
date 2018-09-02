using System;
using System.ComponentModel.DataAnnotations;

namespace Acme.Domain.Models
{
    public abstract class EntityBase
    {
        [Required]
        public DateTimeOffset CreatedAt { get; internal set; } = DateTimeOffset.Now;
    }
}