using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using BackEnd;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using BackEnd.Data;
using BackEnd.Services;
using BackEnd.Repositories;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddMemoryCache();

        var secKey = builder.Configuration.GetValue<string>("Security:SecurityKey");


        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IUserService, UserService>();

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}";
            options.Audience = builder.Configuration["Auth0:Audience"];
            options.TokenValidationParameters = new TokenValidationParameters
            {
                NameClaimType = ClaimTypes.NameIdentifier
            };
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        //if (app.Environment.IsDevelopment())
        //{
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "UserAPI V1");
        });

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();
        //builder.Services.AddAuthorization(options =>
        //{
        //    options.AddPolicy("read:messages", policy => policy.Requirements.Add(new
        //    HasScopeRequirement("read:messages", "Auth0:Domain")));
        //});

        //builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

        app.MapControllers();

        app.Run();

        var cache = app.Services.GetService<IMemoryCache>();
        cache.Set("SecurityKey", secKey);
    }
}