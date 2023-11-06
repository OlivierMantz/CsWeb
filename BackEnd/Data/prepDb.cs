using System;
using System.Linq;
using BackEnd.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BackEnd.Data;

public static class PrepDb
{
    // for testing
    public static void PrepPopulation(IApplicationBuilder app)
    {
        using (var serviceScope = app.ApplicationServices.CreateScope())
        {
            SeedData(serviceScope.ServiceProvider.GetService<UserContext>());
        }
    }

    private static void SeedData(UserContext context)
    {
        if (!context.User.Any())
        {
            Console.WriteLine("--> Seeding data...");

            context.User.AddRange(
                new User()
                {
                    Id = 1, Name = "John", Email = "John@gmail.com", Password = "1234", Role = "User"
                },
                new User()
                {
                    Id = 2, Name = "Jane", Email = "Jane@gmail.com", Password = "1234", Role = "User"
                },
                new User()
                {
                    Id = 3, Name = "Bob", Email = "Bob@gmail.com", Password = "1234", Role = "User"
                }
            );
            context.SaveChanges();
        }
        else
        {
            Console.WriteLine("--> Data already seeded.");
        }
    }
}