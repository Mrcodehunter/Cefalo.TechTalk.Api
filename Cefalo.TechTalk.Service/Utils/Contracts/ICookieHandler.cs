using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechTalk.Service.Utils.Contracts
{
    public interface ICookieHandler
    {
        public string Get(string key);

        public void Set(string key, string value/*, int? expireTime*/);

        public void Remove(string key);
        
    }
}
