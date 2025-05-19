using Microsoft.EntityFrameworkCore;
using Blog.Models;

namespace Blog.Context
{
    public class BlogDbContext : DbContext
    {
        // db bagnlantı ayarlarımız.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB; database=BlogV1; Integrated Security=True; TrustServerCertificate=True;");
        }

        public DbSet<Blog.Models.Blog> Blogs { get; set; }
        public DbSet<Blog.Models.Comment> Comments { get; set; }
        public DbSet<Blog.Models.Contact> Contacts { get; set; }
    }
}
