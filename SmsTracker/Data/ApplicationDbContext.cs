using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmsTracker.Models;

namespace SmsTracker.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        
    }
    
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Number> Numbers { get; set; }
    public DbSet<TrackedItem> TrackedItems { get; set; }
}