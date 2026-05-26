using Microsoft.EntityFrameworkCore;

namespace BackendVerificationCode.Infrastructure.Data;

public class VerificationDbContext : DbContext
{
    public VerificationDbContext(DbContextOptions<VerificationDbContext> options) : base(options)
    {
    }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("verification"); //SCHEMA SOM SEPARERAR I DATABASEN

        base.OnModelCreating(modelBuilder);
    }
}