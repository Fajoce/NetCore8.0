using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Domain.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public int Year { get; set; }
        public string Genre { get; set; } = default!;
        public int Pages { get; set; }
        public int AuthorId { get; set; }

        public Author Author { get; set; } = default!;
    }
}
