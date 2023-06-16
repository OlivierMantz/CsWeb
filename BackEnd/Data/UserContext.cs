using System.Collections.Generic;
using BackEnd.Models;
using Microsoft.EntityFrameworkCore;
using BackEnd.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }
        public DbSet<User> User { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "John", Email = "John@gmail.com", Password = "1234", Role = "User", IsBanned = false },
                new User { Id = 2, Name = "Jane", Email = "Jane@gmail.com", Password = "1234", Role = "User", IsBanned = false },
                new User { Id = 3, Name = "Bob", Email = "Bob@gmail.com", Password = "1234", Role = "User", IsBanned = false }
            );
        }
    }
}