using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class UserViewDTO
    {
        public string? Id { get; set; } 

        public string? Email { get; set; }

        public string? FullName { get; set; }

        public bool? IsAdmin { get; set; }
    }
}
