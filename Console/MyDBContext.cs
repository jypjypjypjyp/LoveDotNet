using Microsoft.EntityFrameworkCore;
using LoveDotNet.Models;

namespace Test
{
    public class MyDBContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\jypjy\source\repos\LoveDotNet.Blazor\Data\lovedotnet.mdf;Integrated Security=True;Connect Timeout=30");
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Paragraph> Paragraphs { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Comment> Comments { get; set; }

    }
}