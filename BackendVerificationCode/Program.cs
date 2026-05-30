using BackendVerificationCode.API.Swagger;
using BackendVerificationCode.Application.Interfaces;
using BackendVerificationCode.Application.Services;
using BackendVerificationCode.Infrastructure.Data;
using BackendVerificationCode.Security;
using BackendVerificationCode.Swagger;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// LÅSER SÅ MAN MÅSTE SKRIVA IN SÄKERHETSNYCKEL FÖR SWAGGER
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiKeyAuthFilter>();
});

// Hämtar Azure verification code email Connection String från User Secrets
var azureConnectionString = builder.Configuration["AzureVerificationEmailCode"];
builder.Services.AddSingleton(new VerificationCodeEmailService(azureConnectionString!));




// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000", "https://lms-shiko.vercel.app") //BÅDE TEST-LOKALHOST OCH RIKTIGA LÄNKEN
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
// DÖRRVAKT FÖR API ANROP (t.ex. swagger authorization samt alla andra API anrop som ber om tillåtelse att komma in i detta projekt)
builder.Services.Configure<ApiKeyOptions>(builder.Configuration.GetSection("ApiKeyOptions"));
builder.Services.AddScoped<ApiKeyAuthFilter>();




builder.Services.AddScoped<IVerificationCodeService, VerificationCodeService>();



// SWAGGER
builder.Services.AddSwagger();





builder.Services.AddDbContext <IVerificationDbContext, VerificationDbContext> (options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLAzure")));



var app = builder.Build();




// SWAGGER
app.MapSwagger(app.Environment);


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
