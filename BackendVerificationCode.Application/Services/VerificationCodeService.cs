using BackendVerificationCode.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BackendVerificationCode.Application.Services;
public class VerificationCodeService : IVerificationCodeService
{

    private readonly IVerificationDbContext _context;

    public VerificationCodeService(IVerificationDbContext context)
    {
        _context = context;
    }

    public Task<string> CreateCodeAsync(string email)
    {
        throw new NotImplementedException();
    }
    // 1. Generera en slumpmässig kod.

    // 2. Spara koden i databasen via _context.VerificationCodes.Add().






    public Task SendCodeAsync(string email, string code)
    {
        throw new NotImplementedException();
    }
    // 3. Skicka koden till användaren.








    public Task<bool> ValidateCodeAsync(string email, string code)
    {
        throw new NotImplementedException();
    }

    // 3. Kontrollera om koden som användaren skrivit in på hemsidan är giltig


}
