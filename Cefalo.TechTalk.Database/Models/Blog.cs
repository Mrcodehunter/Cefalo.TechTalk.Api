using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechTalk.Database.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string? AuthorName { get; set; }

        public int AuthorId { get; set; }  

        public string? Title { get; set; }

        public string? Body { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public User User { get; set; }

    }
}
