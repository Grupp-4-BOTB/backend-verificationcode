using BackendVerificationCode.Application.Interfaces;
using BackendVerificationCode.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
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




    // CreateCodeAsync
    // SKAPA OCH SPARA TILL DATABASEN (Kommwr sen gås igenom på sista Asyncen)
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







    // SendCodeAsync
    // SKICKA IVÄG KODEN VIA EMAIL TILL ANVÄNDARENS EMAIL
    public async Task SendCodeAsync(string email, string code)
    {
        // 3. Skicka koden till användaren via AzureEmail (VerificationCodeEmail.cs)
        await _verificationCodeEmailService.SendEmailCodeAsync(email, code);
    }










    // ValidateCodeAsync
    public async Task<bool> ValidateCodeAsync(string code)
    {

        // GABRIELS API ANROP
        string email = ""; //Här ska mailen som användaren skrev på Gabriels sida sen sparas

        using (var client = new HttpClient())
        {
            // Vi lägger till koden i slutet av URL:en (kolla med Gabriel exakt hur hans GET-url ser ut!)
            var gabrielGetEmailUrl = "https://shiko-identity-webbapi.azurewebsites.net/api/auth/check-email";

            try
            {
                var apiResponse = await client.GetAsync(gabrielGetEmailUrl);

                if (apiResponse.IsSuccessStatusCode)
                {
                    // Vi hämtar e-postadressen som Gabriel skickar tillbaka
                    var responseString = await apiResponse.Content.ReadAsStringAsync(); // HÄMTAR ePOSTEN som användare skrev på Gabriels logga in del, och gör om den till en VANLIG sträng
                    email = responseString; // Sparar här mailen i den tomma "string email" som jag skrev längst upp
                }


                else
                {
                    return false; // oM ingen mail finns, skickas false tillbaka
                }
            }
                // FÅNGAR UPP OM SYSTEMET SKULLE KRASHCA
                catch (Exception)
                {
                    return false; 
                }
            }

        // GABRIELS API ANROP AVSLUT










        // Går in i databasen och hittar det som sparades i CreateCodeAsync,
        // och kontrollerar så allt stämmer. (Att koden inte är använd redan etc)
        var ConfirmedMatch = await _context.VerificationCodes            //Gå till databasen och jämför CreateCodeAsync - med det nya som användaren skrev in
            .FirstOrDefaultAsync(x => x.Email == email
                                   && x.Code == code                    // Matchar siffrorna som användaren skrev in i CreateCodeAsync?
                                   && x.ExpiresAt > DateTime.UtcNow     // Är dom 2 minuterna fortfarande giltiga? (och inte passerat än) Denna utgår helt från tiden i databasen, där den tyo jämför tiden för NÄR koden skickades ut (kanske 12:00) och då funkar den alltså i 2 minuter fram till 12:02. Om klockan åandra sidan är 12:05 när denna check görs så är koden nu ogiltig. (Det är så jag uppfattar det)
                                   && !x.IsUsed);                       // är koden (false) oanvänd?
        // allt ovan har nu stoppats in i variabeln "ConfirmedMatch"
        // och jämförts med databasens _context.VerificationCodes



        // OM KODEN ÄR OGILTIG = RETURNERA FALSE
        if (ConfirmedMatch == null)
            return false;



        // OM ANVÄNDAREN SKRIVIT IN RÄTT OCH GILTIG KOD,
        // SÅ MARKERAS KODEN HÄR SOM ANVÄND (Från false till true) SÅ DEN INTE KAN ANVÄNDAS FLER GÅNGER
        // OCH ANVÄNDAREN GÅR VIDARE
        ConfirmedMatch.IsUsed = true;               //IsUsed är namnet på den boolen för koden i entities.cs

        await _context.SaveChangesAsync();


        return true;
    }
}
