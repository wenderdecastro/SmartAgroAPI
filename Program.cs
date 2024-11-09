using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SmartAgroAPI.Contexts;
using SmartAgroAPI.Interfaces;
using SmartAgroAPI.Repositories;

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
builder.Services.AddSwaggerGen();

//Dependency Injections

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddDbContext<SmartAgroDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SmartAgroDB")));

builder.Environment.IsDevelopment();


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
