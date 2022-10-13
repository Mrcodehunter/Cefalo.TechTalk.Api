using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechTalk.Service.Utils.Contracts
{
    public interface IDateTimeHandler
    {
        public DateTime GetDateTimeInUtcNow();
        public DateTime GetDateTime();
    }
}
