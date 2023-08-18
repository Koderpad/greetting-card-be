using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class PaginationData
    {
        public Object Data { get; set; } = null!;
        public Pagable pagable { get; set; } = null!;
    }

}
