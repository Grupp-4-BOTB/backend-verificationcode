using BackendVerificationCode.API.Swagger;
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
// 2 SWAGGER KEY FÖR SÄKERHET
builder.Services.Configure<ApiKeyOptions>(builder.Configuration.GetSection("ApiKeyOptions"));
builder.Services.AddScoped<ApiKeyAuthFilter>();








// SWAGGER
builder.Services.AddSwagger();





builder.Services.AddDbContext<VerificationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLAzure")));



var app = builder.Build();




// SWAGGER
app.MapSwagger(app.Environment);


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
