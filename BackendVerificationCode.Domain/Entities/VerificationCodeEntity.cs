using System;
using System.Collections.Generic;
using System.Text;

namespace BackendVerificationCode.Domain.Entities;

public class VerificationCodeEntity
{
    public int Id { get; set; }                         //ID för den specifika raden i databasen
    public string Code { get; set; } = string.Empty;    // Den 7-siffriga koden
    public string Email { get; set; } = string.Empty;   // Kopplar koden till rätt användare 
    public bool IsUsed { get; set; } = false;           // en säkerhetsspärr som bara ser till att koden används EN gång. Scenario: om en person ska skriva in koden och en obehörig sen ser koden, så KAN båda sktiva in koden samtidigt UTAN denna.

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Exakt tid näe användaren tryclte in på sidan.
    public DateTime ExpiresAt { get; set; }
    // ExpiresAt: Om personen klickar in på min sida klockan 12:00 (CreatedAt) så kommer ExpiresAt att skriva ut 12.02 om koden är aktiv i 2 minuter. (eller om countdown är på 1 min, så kommer det stå 12.01)
    // Ska sen skapa en javascript countdown timer i frontend som tajmar in sig med ExpiresAt.
    // KOD: ExpiresAt = DateTime.UtcNow.AddMinutes(2); // I koden sen kan jag t.ex. använda denna där jag skriver in exakt antal minuter för nedräkningen. 

}