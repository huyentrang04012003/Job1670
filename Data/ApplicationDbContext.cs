using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using JobWebsite.Models;

namespace JobWebsite.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<JobWebsite.Models.Candidate> Candidates { get; set; } = default!;
        public DbSet<JobWebsite.Models.Job> Jobs { get; set; } = default!;
        public DbSet<JobWebsite.Models.CV> CVs { get; set; } = default!;
    }
}
