using Bookstore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookstore.Infraestructure
{
    public class BookstoreDbContext: DbContext
    {
        public BookstoreDbContext(DbContextOptions options) : base(options)
        {
        }

       public DbSet<Author> authors { get; set; }
       public DbSet<Book> books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =========================
            // AUTHOR CONFIGURATION
            // =========================
            modelBuilder.Entity<Author>(entity =>
            {
                
                entity.ToTable("Authors");

                entity.HasKey(a => a.Id);

                entity.Property(a => a.FullName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(a => a.City)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(a => a.Email)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(a => a.BirthDate)
                    .IsRequired();

                // Evita correos duplicados
                entity.HasIndex(a => a.Email)
                    .IsUnique();
            });
            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("Books");

                entity.HasKey(b => b.Id);

                entity.Property(b => b.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(b => b.Genre)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(b => b.Year)
                    .IsRequired();

                entity.Property(b => b.Pages)
                    .IsRequired();

                entity.Property(b => b.AuthorId)
                    .IsRequired();
                entity.HasOne(b => b.Author)
                    .WithMany() // Author no tiene colección Books
                    .HasForeignKey(b => b.AuthorId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
