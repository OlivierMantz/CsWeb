using BackEnd.Models;
using Microsoft.EntityFrameworkCore;
using BackEnd.Models.DTOs;

namespace BackEnd.Data
{
    public class PostContext : DbContext
    {
        public PostContext(DbContextOptions<PostContext> options) : base(options)
        {
        }

        public DbSet<Post> Post { get; set; } = null!;
    }
}
