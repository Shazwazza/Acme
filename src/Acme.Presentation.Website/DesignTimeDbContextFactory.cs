using Acme.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Acme.Presentation.Website
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<EntityFrameworkCoreContext>
    {
        private readonly string _connectionString;

        public DesignTimeDbContextFactory()
        {
            _connectionString = @"Data Source=127.0.0.1;Database=Acme;Integrated Security=False;User ID=sa;Password=<YourStrong!Passw0rd>;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        }
        public DesignTimeDbContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }
        
        public EntityFrameworkCoreContext CreateDbContext(string[] args)
        {
            return new EntityFrameworkCoreContext(_connectionString);
        }
    }
}