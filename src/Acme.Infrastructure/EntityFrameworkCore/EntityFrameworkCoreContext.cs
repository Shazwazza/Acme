using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Acme.Application.Interfaces;
using Acme.Domain;
using Acme.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Acme.Infrastructure.EntityFrameworkCore
{
    public class EntityFrameworkCoreContext : DbContext, IDbContext
    {
        private readonly string _connectionString;
        private readonly ConcurrentQueue<Action> _onSuccessActions = new ConcurrentQueue<Action>();

        public DbSet<SerialNumber> ValidSerialNumbers { get; set; }
        public DbSet<Submission> Submissions { get; set; }

        public EntityFrameworkCoreContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void OnChangesSaved(Action action)
        {
            _onSuccessActions.Enqueue(action);
        }

        public Task MigrateAsync()
        {
            return this.Database.MigrateAsync();
        }

        public void NotifySuccessHandlers()
        {
            while (_onSuccessActions.TryDequeue(out var action))
            {
                try
                {
                    action();
                }
                catch (Exception)
                {
                    // Ignored
                }
            }
        }

        public void CancelSuccessHandlers()
        {
            while (_onSuccessActions.TryDequeue(out _))
            {
                // Do nothing
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
        }
        
        
    }
}