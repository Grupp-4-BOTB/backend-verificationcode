using Microsoft.EntityFrameworkCore;
using BackendVerificationCode.Domain.Entities;

namespace BackendVerificationCode.Infrastructure.Data;

public class VerificationDbContext : DbContext
{
    public VerificationDbContext(DbContextOptions<VerificationDbContext> options) : base(options)
    {
    }

    //Skapar mina tabellet i databasen från min entitys klass
    public DbSet<VerificationCode> VerificationCodes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("verification"); //SCHEMA SOM SEPARERAR I DATABASEN (för att se schema >>> SQL server > Database > tables så ser du sen verification.*namnet*)

        base.OnModelCreating(modelBuilder);
    }
}