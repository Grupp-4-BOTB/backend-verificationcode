using BackendVerificationCode.Application.Interfaces;
using BackendVerificationCode.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendVerificationCode.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VerificationCodeController : ControllerBase
{
    private readonly IVerificationCodeService _codeService;

    public VerificationCodeController(IVerificationCodeService codeService)
    {
        _codeService = codeService;
    }





    //NÄR ANVÄNDAREN SKRIVIT IN KODEN, KÖRS NEDAN
    [HttpPost("verify")]
    public async Task<IActionResult> ValidateCode([FromBody] VerifyCodeEmailDTO request) 
    {


        // 1. (request.Code)                    = Controllern tar koden som användaren skrev in.
        // 2. _codeService.ValidateCodeAsync    = Controllern skickar vidare den datan till din Service.
        // 3. var isValid = await               = Servicen kollar i databasen och svaret (JA ELLER NEJ) sparas i variabeln: isValid.
        var isValid = await _codeService.ValidateCodeAsync(request.Code);




        // OM KODEN ÄR FEL/OGILTIG/UTGÅNGEN 
        if (!isValid)
        {
            return BadRequest("Code is not valid.");
        }




        // OM KODEN ÄR RÄTT
        // (if (isValid)
        return Ok("Code has been confirmed!");
    }













    // NÄR ANVÄNDAREN KLICKAR PÅ "RESEND VERIFICATION CODE" PÅ FRONTEND AKTIVERAS DENNA
    [HttpPost("resend")]
    public async Task<IActionResult> ResendCode([FromBody] VerifyCodeEmailDTO request)
    {

        // Genererar den NYA koden i await, sparar den i databasen (_codeservice) + anropar min service GenerateAndSendNewCodeAsync + skickar det till mailen som användaren skrev in på gabriels sida
        var newCodeSentToEmail = await _codeService.GenerateAndSendNewCodeAsync(request.Email);


        // NY KOD SKICKADES INTE
        if (!newCodeSentToEmail)
        {
            return BadRequest("Could not resend verification code.");
        }


        //NY KOD SKICKADES
        return Ok("A new verification code has been sent.");
    }


}



