using Cefalo.TechTalk.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechTalk.Service.DTOs
{
    public class UserDetailsDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? UserName { get; set; }

        public string? Email { get; set; }

        public string? Token { get; set; }

    }
}
