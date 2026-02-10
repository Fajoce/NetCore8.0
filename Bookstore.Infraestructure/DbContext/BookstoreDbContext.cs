using Bookstore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Infraestructure
{
    public class BookstoreDbContext: DbContext
    {
        public BookstoreDbContext(DbContextOptions options) : base(options)
        {
        }

       public DbSet<Author> authors { get; set; }
       public DbSet<Book> books { get; set; } 
    }
}
