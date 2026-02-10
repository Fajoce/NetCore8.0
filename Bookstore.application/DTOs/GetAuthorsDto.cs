using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Application.DTOs
{
    public class GetAuthorsDto
    {
     
        public string FullName { get; set; } = default!;

        public DateTime BirthDate { get; set; }

       
        public string City { get; set; } = default!;

     
        public string Email { get; set; } = default!;
    }
}
