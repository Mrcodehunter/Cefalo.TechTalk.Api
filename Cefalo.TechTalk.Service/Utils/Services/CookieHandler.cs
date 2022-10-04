using Cefalo.TechTalk.Service.Utils.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechTalk.Service.Utils.Services
{
    public class CookieHandler : ICookieHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CookieHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
       
        public string Get(string key)
        {
            return _httpContextAccessor.HttpContext.Request.Cookies["key"];
        }
        public void Set(string key, string value/*, int? expireTime*/)
        {
            /*CookieOptions option = new CookieOptions();
            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);*/
            _httpContextAccessor.HttpContext.Response.Cookies.Append(key, $"bearer {value}"/*, option*/);
        }
        public void Remove(string key)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(key);
        }

    }
}
