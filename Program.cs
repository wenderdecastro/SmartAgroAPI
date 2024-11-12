using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SmartAgroAPI.Contexts;
using SmartAgroAPI.Interfaces;
using SmartAgroAPI.Repositories;
using SmartAgroAPI.Services.EmailService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(x => x.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey("SmartAgroSecurityKey"u8.ToArray()),
        ValidIssuer = "SmartAgro",
        ValidAudience = "SmartAgroAudience"
    });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Smart Agro API",
        Description = "An ASP.NET Core Web API for managing the Smart Agro system, which is an application for agriculture sensor data tracking",
        Contact = new OpenApiContact
        {
            Name = "Wender de Castro (API ADMIN)",
            Url = new Uri("https://github.com/wenderdecastro")
        },
        License = new OpenApiLicense
        {
            Name = "MIT LICENSE",
            Url = new Uri("https://github.com/wenderdecastro/SmartAgroAPI/blob/main/LICENSE")
        }

    });
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    options.IncludeXmlComments(xmlPath);
});

//Dependency Injections

builder.Services.Configure<IEmailService>(builder.Configuration.GetSection(nameof(EmailService)));
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddScoped<EmailSendingService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddDbContext<SmartAgroDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SmartAgroDB")));



builder.Environment.IsDevelopment();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Smart Agro API v1");
    //options.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
