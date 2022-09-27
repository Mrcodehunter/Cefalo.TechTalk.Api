using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechTalk.Service.Utils.CustomErrorHandler
{
    public class ForbiddenExceptoin : Exception
    {
        public ForbiddenExceptoin() { }
        public ForbiddenExceptoin(string message) : base(message) { }
    }
}
