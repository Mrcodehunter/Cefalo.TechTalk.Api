using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechTalk.Service.DTOs
{
    public class BlogDetailsDto
    {
        public int Id { get; set; }
        public string? AuthorName { get; set; }

        public string? Title { get; set; }

        public string? Body { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }
    }
}
