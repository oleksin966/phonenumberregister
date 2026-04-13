using Microsoft.EntityFrameworkCore;
using PhoneNumberRegister.Models;

namespace PhoneNumberRegister.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<PhoneNumber> PhoneNumbers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PhoneNumber>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Number)
                  .IsRequired()
                  .HasMaxLength(20);

            entity.HasIndex(e => e.Number)
                  .IsUnique();
        });
    }
}