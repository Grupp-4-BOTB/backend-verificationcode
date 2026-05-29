using BackendVerificationCode.Application.Interfaces;
using BackendVerificationCode.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackendVerificationCode.Infrastructure.Data;

public class VerificationDbContext : DbContext, IVerificationDbContext
{
    public VerificationDbContext(DbContextOptions<VerificationDbContext> options) : base(options)
    {
    }

    //Skapar mina tabellet i databasen från min entitys klass
    public DbSet<VerificationCodeEntity> VerificationCodes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("verification"); //SCHEMA SOM SEPARERAR I DATABASEN (för att se schema >>> SQL server > Database > tables så ser du sen verification.*namnet*)

        base.OnModelCreating(modelBuilder);
    }
}
// lägg till schema