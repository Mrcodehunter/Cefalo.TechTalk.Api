﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechTalk.Service.DTOs
{
    public class UserUpdateDto
    {
        public string? Name { get; set; }

        public string? UserName { get; set; }

        public string? Password { get; set; }

        public string? Email { get; set; }
    }
}
