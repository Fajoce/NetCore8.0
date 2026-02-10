using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Api.DTOs
{
    public class CreateAuthorDto
    {
        [Required]
        public string FullName { get; set; } = default!;

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public string City { get; set; } = default!;

        [Required, EmailAddress]
        public string Email { get; set; } = default!;
    }
}
