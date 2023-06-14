using Microsoft.EntityFrameworkCore;
using BackEnd.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Models.Context
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options)
        : base(options)
        {
        }

        public DbSet<User> User { get; set; } = null!;
    }
}
