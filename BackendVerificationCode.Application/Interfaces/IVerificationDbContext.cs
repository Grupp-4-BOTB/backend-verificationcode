using BackendVerificationCode.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BackendVerificationCode.Application.Interfaces;


public interface IVerificationDbContext
{
    // Eftersom jag följer clean architecture och application layert inte har dependency till infrastucture layert,
    // så skapar jag ist en interface som fungerar som en "mellanhand" mellan service och dbcontext
    // vilket gör att Application layer KAN prata med databasen (DbContext) via detta interfacet.
    DbSet<VerificationCodeEntity> VerificationCodes { get; set; }







    // Jag skapar en "SaveChangesAsync" här så att Servicen har tillåtelse att faktiskt spara 
    // sina ändringar TILL databasen VIA interfacet.
    //Utan denna nedan kommer ingenting att kunna sparas.
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
