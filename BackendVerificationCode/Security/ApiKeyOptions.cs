namespace BackendVerificationCode.Security;

public class ApiKeyOptions
{
    public const string SectionName = "ApiKeyOptions";
    public string ApiKey { get; set; } = string.Empty;
}

// dENNA gör om JSON/User Secrets till ett C#-objekt, så att koden kan läsa in mitt lösenord.
// Lösenordet har jag lagt i "manage user secrets"