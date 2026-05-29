using System;
using System.Collections.Generic;
using System.Text;

namespace BackendVerificationCode.Application.Interfaces;

public interface IVerificationCodeService
{
    Task<string> CreateCodeAsync(string email);                 // SKAPA KODEN 
    Task SendCodeAsync(string email, string code);              // sKICKA KODEN TILL ANVÄNDARENS MAIL
    Task<bool> ValidateCodeAsync(string code);                  // Validera att koden som skrivs in är RÄTT
}
