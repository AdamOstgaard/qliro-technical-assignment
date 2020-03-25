using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using qliro.bookshelf.Models;

namespace qliro.bookshelf.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Loan> Loans { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Loan>()
                .HasOne(l => l.User)
                .WithMany(u => u.Loans).HasForeignKey(l => l.ID);
            
            modelBuilder.Entity<Loan>()
                .HasOne(l => l.Book)
                .WithOne(b => b.Loan);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Loans)
                .WithOne(l => l.User).HasForeignKey(l => l.UserID);

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Loan)
                .WithOne(l => l.Book);


            modelBuilder.Entity<Book>().HasData(
                    new List<Book>
                    {
                        new Book("Cats Are Animals", "Kurt Larsson", "Cats"){ID = -1},
                        new Book("Cats Are Animals", "Kurt Larsson", "Cats"){ID = -2},
                        new Book("Flying Birds Are Fun", "Lisa Fredrikssson", "Birds"){ID = -3}
                    }
            );
    }
    }
}
