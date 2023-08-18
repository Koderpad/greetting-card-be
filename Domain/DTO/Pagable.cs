using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class Pagable
    {
        public int offset { get; set; }
        public int pageSize { get; set; }
        public int? total { get; set; }
    }
}
