using System;

namespace Acme.Domain.Interfaces
{
    public interface ISubmission
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        DateTimeOffset Birthday { get; set; }
        string EmailAddress { get; set; }
        Guid SerialNumberCode { get; set; }
    }
}