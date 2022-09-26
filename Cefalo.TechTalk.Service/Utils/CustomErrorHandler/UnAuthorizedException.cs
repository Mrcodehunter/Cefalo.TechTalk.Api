using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechTalk.Service.Utils.CustomErrorHandler
{
    public class UnAuthorizedException : Exception
    {
        public UnAuthorizedException() { }
        public UnAuthorizedException(string message) : base(message) { }
    }
}
