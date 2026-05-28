using BackendVerificationCode.Application.Interfaces;
using BackendVerificationCode.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BackendVerificationCode.Application.Services;
public class VerificationCodeService : IVerificationCodeService
{

    private readonly IVerificationDbContext _context;
    private readonly VerificationCodeEmailService _verificationCodeEmailService;

    public VerificationCodeService(IVerificationDbContext context, VerificationCodeEmailService verificationCodeEmailService)
    {
        _context = context;
        _verificationCodeEmailService = verificationCodeEmailService;
    }





    // SKAPA OCH SPARA TILL DATABASEN
    public async Task<string> CreateCodeAsync(string email)
    {

        // 1. Skapar den slumpmässiga 7-siffriga genererade koden.
        var random = new Random();
        string generatedCode = random.Next(1000000, 9999999).ToString();

        // fyller i alla värden (t.ex emailen som användare skrev in, koden som genererats etc. Innan dess fanns inget värde)
        var verificationCode = new VerificationCodeEntity
        {
            Email = email,
            Code = generatedCode,
            ExpiresAt = DateTime.UtcNow.AddMinutes(2), //koden håller i 2 minuter
            IsUsed = false
        };

        // Sparar nu allt till databasen så det hamnar i tabell som vi sen kan se
        _context.VerificationCodes.Add(verificationCode);
        await _context.SaveChangesAsync();

        // Skickar koden till CONTROLLERN
        return generatedCode;
    }
    

  

    // SKICKA IVÄG KODEN VIA EMAIL TILL ANVÄNDARENS EMAIL
    public async Task SendCodeAsync(string email, string code)
    {
        // 3. Skicka koden till användaren via AzureEmail (VerificationCodeEmail.cs)
        await _verificationCodeEmailService.SendEmailCodeAsync(email, code);
    }




















    public Task<bool> ValidateCodeAsync(string email, string code)
    {
        throw new NotImplementedException();
    }
}
