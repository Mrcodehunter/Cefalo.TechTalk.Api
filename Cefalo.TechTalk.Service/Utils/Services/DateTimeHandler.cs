using Cefalo.TechTalk.Service.Utils.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechTalk.Service.Utils.Services
{
    public class DateTimeHandler : IDateTimeHandler
    {
        public  DateTime GetDateTimeInUtcNow() { return DateTime.UtcNow; }
        public  DateTime GetDateTime() { return DateTime.Now; }
    }
}
