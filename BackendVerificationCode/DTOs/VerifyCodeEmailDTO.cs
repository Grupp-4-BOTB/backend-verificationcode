namespace BackendVerificationCode.DTOs;

public class VerifyCodeEmailDTO
{
    public string Code { get; set; } = string.Empty;        // KODEN som användaren skriver in
    public string Email { get; set; } = string.Empty;       // Lagt till Email i DTO igen, just för resend funktionen i min frontend för verifications koden, där användaren kan be om ny kod.
}


// La tillbaka Email så systemet kopplar rätt email till rätt kod. 