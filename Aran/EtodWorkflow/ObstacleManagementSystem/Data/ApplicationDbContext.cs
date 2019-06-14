using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ObstacleManagementSystem.Models;

namespace ObstacleManagementSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, long>
    {

        public DbSet<UserAudit> UserAuditEvents { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ObstacleManagementSystem.Models.ApplicationUser> ApplicationUser { get; set; }
    }
}
