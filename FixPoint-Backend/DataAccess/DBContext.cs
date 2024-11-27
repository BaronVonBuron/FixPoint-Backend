using FixPoint_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace FixPoint_Backend.DataAccess;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Case> Cases { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Technician> Technicians { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.ID);

            entity.Property(e => e.Name)
                .IsRequired();

            entity.Property(e => e.Email)
                .IsRequired();

            entity.Property(e => e.Phonenumber)
                .IsRequired();

            entity.Property(e => e.CPRCVR)
                .IsRequired();
        });
    }
    
}