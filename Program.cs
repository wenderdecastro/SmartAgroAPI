using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SmartAgroAPI.Contexts;
using SmartAgroAPI.Interfaces;
using SmartAgroAPI.Mappings;
using SmartAgroAPI.Repositories;
using SmartAgroAPI.Services;
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

//AutoMapper



//Api Services

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

//Swagger
builder.Services.AddSwaggerGen(options =>
{

    //Swagger documentation

    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Smart Agro API",
        Description = "An ASP.NET Core Web API for managing the Smart Agro system, which is an application for agriculture sensor data tracking",
        Contact = new OpenApiContact
        {
            Name = "Wender de Castro (API admin)",
            Url = new Uri("https://github.com/wenderdecastro")
        },
        License = new OpenApiLicense
        {
            Name = "MIT LICENSE",
            Url = new Uri("https://github.com/wenderdecastro/SmartAgroAPI/blob/main/LICENSE.txt")
        }

    }
    );

    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    options.IncludeXmlComments(xmlPath);

    //Jwt Authorization and Authentication for Swagger with Bearer Auth
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization Header - set up with Bearer Authentication.\r\n\r\n" +
                       "Use 'Bearer' [space] <yourtoken> in the field below.\r\n\r\n",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },

            Array.Empty<string>()
        }
    });


});

//Dependency Injections

builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddScoped<EmailSendingService>();


builder.Services.AddHostedService<DataGenerationService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<ISensorRepository, SensorRepository>();

builder.Services.AddDbContext<SmartAgroDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SmartAgroDB"))
);

//Configurations


builder.Services.AddAutoMapper(typeof(SensorProfile));
builder.Services.AddAutoMapper(typeof(NotificationProfile));
builder.Services.AddAutoMapper(typeof(UserProfile));


builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireClaim("IsAdmin", "true"));
});

builder.Environment.IsDevelopment();

//azure
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Smart Agro API v1");
    options.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

//azure
app.UseDeveloperExceptionPage();

//auth
app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
