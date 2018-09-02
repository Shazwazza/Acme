using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Acme.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Acme.Infrastructure.EntityFrameworkCore
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> SkipAndTake<T>(this IQueryable<T> source, PagingInformation pagingInformation)
        {
            return source.Skip(pagingInformation.PageSize * (pagingInformation.PageNumber - 1))
                         .Take(pagingInformation.PageSize);
        }
        
        public static async Task<PagingResult<T>> ToPagingResultAsync<T>(
            this IQueryable<T> source,
            PagingInformation pagingInformation,
            CancellationToken cancellationToken = default(CancellationToken))
        {

            var countTask = source.CountAsync(cancellationToken);
            var listTask = source.Skip(pagingInformation.PageSize * (pagingInformation.PageNumber - 1))
                                 .Take(pagingInformation.PageSize).ToListAsync(cancellationToken);

            await Task.WhenAll(countTask, listTask);
            
            return new PagingResult<T>()
            {
                Hits = listTask.Result,
                TotalHits = countTask.Result,
                PagingInformation = pagingInformation
            };

        }
    }
}