using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OmsApi.Dto;
using OmsApi.Entity;

namespace OmsApi.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, long>
    {
        public DbSet<UserAudit> UserAuditEvents { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<RequestReport> RequestReports { get; set; }
        private DbSet<Slot> _slots { get; set; }

        public Task<Slot> GetSelectedSlot()
        {
            return _slots.FirstOrDefaultAsync();
        }

        public async Task SetSelectedSlot(Slot value)
        {
            if (await _slots.CountAsync() > 0)
                await _slots.ForEachAsync(t => _slots.Remove(t));            
            _slots.Add(value);
            try
            {
                await SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var k = ex.Message;
                throw;
            }
            
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            AddTimestamps();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            AddTimestamps();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            AddTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void AddTimestamps()
        {
            var entities = ChangeTracker.Entries().Where(x => x.Entity is IBaseEntity && x.State == EntityState.Added);

            foreach (var entity in entities)
            {
                ((IBaseEntity)entity.Entity).CreatedAt = DateTime.UtcNow;
                ((IBaseEntity)entity.Entity).LastModifiedAt = DateTime.UtcNow;
            }

            entities = ChangeTracker.Entries().Where(x => x.Entity is IBaseEntity && x.State == EntityState.Modified);

            foreach (var entity in entities)
            {
                ((IBaseEntity)entity.Entity).LastModifiedAt = DateTime.UtcNow;
            }
        }
    }
}