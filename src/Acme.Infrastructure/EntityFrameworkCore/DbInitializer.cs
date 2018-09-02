using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acme.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Acme.Infrastructure.EntityFrameworkCore
{
    public static class DbInitializer
    {
        public static async Task Initialize(EntityFrameworkCoreContext context, bool migrateDoingStartup)
        {
            if (migrateDoingStartup)
            {
                context.Database.Migrate();
            }
            

            if (!await context.ValidSerialNumbers.AnyAsync())
            {
                var submissions = GenerateRandomSerialNumbers();
                await context.ValidSerialNumbers.AddRangeAsync(submissions);

                await context.SaveChangesAsync();
            }
        }

        private static IEnumerable<SerialNumber> GenerateRandomSerialNumbers()
        {
            return Enumerable.Range(start: 1, count: 100).Select(x => new SerialNumber
            {
                Code = Guid.NewGuid()
            });
        }
    }
}