﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class ChangePasswordDTO
    {
        public string? Id { get; set; }
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }

    }
}
