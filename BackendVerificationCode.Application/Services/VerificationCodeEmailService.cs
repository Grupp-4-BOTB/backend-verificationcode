using System;
using System.Collections.Generic;
using Azure;
using Azure.Communication.Email;
using System.Threading.Tasks;

namespace BackendVerificationCode.Application.Services;

public class VerificationCodeEmailService
{
    private readonly string _connectionString;

    public VerificationCodeEmailService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task SendEmailCodeAsync(string toEmail, string code)
    {
        var emailClient = new EmailClient(_connectionString);


        var emailMessage = new EmailMessage(
            senderAddress: "DoNotReply@a24f5e62-a1a0-486e-8d6b-ade05acb33c3.azurecomm.net",
            content: new EmailContent("Verification Code")
            {
                PlainText = $"Hi! Your code is: {code}",
                Html = $@"
                <html>
                    <body>
                        <h1>Hi!</h1>
                        <p>This is your verification code for LMS-Shiko:</p>
                        <h2 style='color:blue;'>{code}</h2>
                        <p>The code is valid for 2 minutes.</p>
                    </body>
                </html>"
            },
            recipients: new EmailRecipients(new List<EmailAddress>
            {
                new EmailAddress(toEmail)
            })
        );


        await emailClient.SendAsync(
            WaitUntil.Completed,
            emailMessage
        );
    }
}