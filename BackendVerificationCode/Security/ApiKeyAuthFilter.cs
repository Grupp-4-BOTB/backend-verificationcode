using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace BackendVerificationCode.Security;



// dETTA är typ som en dörrvakt som stoppar eller släpper igenom olika anrop baserat på lösenordet.
// Eller typ att Ge olika ANROP behörighet till just koden här.
// I frontend projektet, när jag anropar mitt backend API, så är det denna delen som säkerställer
// att lösenordet som frontend skickar mer, stämmer. Och om det inte stämmer överrens så kommer det att stoppas
// och inte få tillgång till koden här.
// (Lösenordet står skrivet i "manage user secrets".)


//Här nedan är det basically vad dörrvakten tittar efter, när det kommer in en ny "förfrågan" om att få bli insläppt.

public class ApiKeyAuthFilter : IAuthorizationFilter
{
    private readonly ApiKeyOptions _options;

    public ApiKeyAuthFilter(IOptions<ApiKeyOptions> options)
    {
        _options = options.Value;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // 1. Kolla om headern överhuvudtaget finns med
        if (!context.HttpContext.Request.Headers.TryGetValue("X-API-KEY", out var extractedApiKey))
        {
            context.Result = new UnauthorizedObjectResult("API Key missing!");
            return;
        }




        // 2. Kolla om nyckeln matchar den vi har i appsettings.json
        if (!_options.ApiKey.Equals(extractedApiKey))
        {
            context.Result = new UnauthorizedObjectResult("Felaktig API Key.");
            return;
        }
    }
}