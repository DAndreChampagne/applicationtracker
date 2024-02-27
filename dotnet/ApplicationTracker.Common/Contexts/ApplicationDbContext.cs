namespace ApplicationTracker.Common.Contexts;

using ApplicationTracker.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


public class ApplicationDbContext : DbContext {

    public ApplicationDbContext(): base() {}
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

    public virtual DbSet<Application> Applications { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Contact> Contacts { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        if (!optionsBuilder.IsConfigured) {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseInMemoryDatabase("ApplicationTracker");
        }
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Seed();
    }
}



public static class ApplicationDbContextExtensions {

    public static void Seed(this ModelBuilder modelBuilder) {
    
        modelBuilder.Entity<Company>().HasData(
            new Company { Id = 1, Name = "GitHub" },
            new Company { Id = 2, Name = "Healthcare Company" },
            new Company { Id = 3, Name = "State Of The-State-Where-I-Currently-Live" }
        );

        modelBuilder.Entity<Contact>().HasData(
            new Contact {
                Id = 1,
                Name = "Brian Recruiter", 
                Email = "brian@company.com", 
                Phone = "860-555-1234"
            }
        );



        modelBuilder.Entity<Application>().HasData(
            new Application {
                Id = 1,
                Link = new Uri("https://www.github.careers/jobs/2433?lang=en-us"),
                CompanyId = 1,
                ContactId = 1,
                Title = "Senior Software Engineer",
                Type = JobType.FullTime,
                Location = "Remote",
                MatchPercent = 60,
                SalaryMin = 104400,
                SalaryMax = 277000,
                DateApplied = new DateTime(2024, 01, 15),
                FollowUps = new() {
                    new DateTime(2024, 01, 18),
                    new DateTime(2024, 01, 20),
                },
                Status = ApplicationStatus.RejectedWithoutInterview,
                ApplicationStatusReason = "None given",
                Notes = new() {
                    "email rejection without reason given"
                }
            }
        );



    }
}