namespace Acme.Domain.Models
{
    public class PagingInformation
    {
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
    }
}