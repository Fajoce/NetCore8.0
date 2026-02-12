using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Application.DTOs
{
    public class CreateBookDto
    {
        [Required]
        public string Title { get; set; } = default!;

        [Required]
        public int Year { get; set; }

        [Required]
        public string Genre { get; set; } = default!;

        [Required]
        public int Pages { get; set; }
       
        public int? AuthorId { get; set; }
    }
}
