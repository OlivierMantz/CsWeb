using BackEnd.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using System;

public class Program
{
    public static void Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureServices((context, services) =>
                {
                    var configuration = context.Configuration;

                    // Configure HttpClient with Auth0 Management API base address
                    services.AddHttpClient<Auth0ManagementService>(client =>
                    {
                        client.BaseAddress = new Uri($"https://{configuration["Auth0:Domain"]}/");
                    });

                    // Register Auth0ManagementService as a singleton
                    services.AddSingleton(new Auth0ManagementService(
                        new HttpClient(),
                        configuration["Auth0:Domain"],
                        configuration["Auth0:ClientId"],
                        configuration["Auth0:ClientSecret"]
                    ));

                    // CORS policy configuration
                    services.AddCors(options =>
                    {
                        options.AddPolicy("AllowSpecificOrigin", builder =>
                        {
                            builder.WithOrigins("http://localhost:5173")
                                   .AllowAnyHeader()
                                   .AllowAnyMethod();
                        });
                    });

                    // MVC and Swagger configurations
                    services.AddControllersWithViews();
                    services.AddSwaggerGen();

                    // JWT Authentication configuration
                    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                        .AddJwtBearer(options =>
                        {
                            options.Authority = $"https://{configuration["Auth0:Domain"]}/";
                            options.Audience = configuration["Auth0:Audience"];
                            options.TokenValidationParameters = new TokenValidationParameters
                            {
                                NameClaimType = ClaimTypes.Name,
                                RoleClaimType = "https://sublimewebapp.me/roles",
                            };
                        });

                });

                webBuilder.Configure(app =>
                {
                    var env = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
                    if (env.IsDevelopment())
                    {
                        app.UseSwagger();
                        app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1"));
                    }

                    app.UseCors("AllowSpecificOrigin")
                        .UseHttpsRedirection()
                        .UseRouting()
                        .UseAuthentication()
                        .UseAuthorization()
                        .UseEndpoints(endpoints => endpoints.MapControllers());
                });
            })
                    .Build();
        host.Run();
    }
}