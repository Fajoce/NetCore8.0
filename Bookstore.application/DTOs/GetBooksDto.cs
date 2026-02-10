using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Application.DTOs
{
    public class GetBooksDto
    {
        [Required]
        public string Title { get; set; } = default!;

        public int Id { get;set ; } 
        public int Year { get; set; }

        public string Genre { get; set; } = default!;

        public int Pages { get; set; }

       public int AuthorId { get; set; }
        public string AuthorName { get; set; }
    }
}
