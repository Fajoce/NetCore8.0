using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Domain.Entities
{
    public class Author
    {        
        public int Id { get; set; }
        public string FullName { get; set; } = default!;
        public DateTime BirthDate { get; set; }
        public string City { get; set; } = default!;
        public string Email { get; set; } = default!;
    }
}
