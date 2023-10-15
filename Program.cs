using System.Text;
using Web3Ai.Service.Services;
using Web3Ai.Service.Data.Context;
using Microsoft.EntityFrameworkCore;
using Web3Ai.Service.Authorization;
using Web3Ai.Service;
using Web3Ai.Service.Utils;
using Swashbuckle.AspNetCore.Annotations;

var localPolicyName = "_myAllowLocalLogins";
var livePolicyName = "_liveAzureStatic";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//For Local Development
builder.Services.AddCors(options => {
    options.AddPolicy(name: localPolicyName,
                        builder => 
                        {
                            builder
                                .WithOrigins("http://localhost:3000")
                                .WithMethods("GET", "POST", "PUT", "DELETE")
                                .AllowAnyHeader();
                        });
        options.AddPolicy(name: livePolicyName,
                        builder => 
                        {
                            builder
                                .WithOrigins("https://gray-sky-0c7072c1e.3.azurestaticapps.net")
                                .WithMethods("GET", "POST", "PUT", "DELETE")
                                .AllowAnyHeader();
                        });
});

//DI
builder.Services.AddHttpClient();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtUtils, JwtUtils>();
builder.Services.AddScoped<IUserDataService, UserDataService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ITextToArtService, TextToArtService>();

builder.Services.AddSingleton<UTF8Encoding>();

builder.Services.AddControllers();

// Db Context (Need to swap out for real db before too long)
builder.Services.AddDbContext<UserContext>(opt => opt.UseInMemoryDatabase("Users"));

// strongly typed settings object
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(localPolicyName);

app.UseAuthorization();

app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

app.Run();
