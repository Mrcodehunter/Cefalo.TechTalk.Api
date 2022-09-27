using Cefalo.TechTalk.Service.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechTalk.Service.Utils.CustomFormatters.OutputFormatters
{
    public class HtmlOutputFormatter : TextOutputFormatter
    {
        public HtmlOutputFormatter()
        {
            SupportedMediaTypes.Add("text/html");
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }
       

        protected override bool CanWriteType(Type type)
        {
            if (typeof(BlogDetailsDto).IsAssignableFrom(type) || typeof(IEnumerable<BlogDetailsDto>).IsAssignableFrom(type))
                return base.CanWriteType(type);
            return false;

        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {

            var response = context.HttpContext.Response;
            var buffer = new StringBuilder();

            if (context.Object is IEnumerable<BlogDetailsDto>)
            {
                IEnumerable<BlogDetailsDto> blogs = (IEnumerable<BlogDetailsDto>)context.Object;
                foreach (BlogDetailsDto blog in blogs)
                    ConvertToHtml(buffer, blog);
            }
            else
            {
                ConvertToHtml(buffer, (BlogDetailsDto)context.Object);
            }

            await response.WriteAsync(buffer.ToString());
        }

        private static void ConvertToHtml(StringBuilder buffer, BlogDetailsDto blog)
        {
            buffer.AppendLine($"<p><h4>Id: {blog.Id}</h4></p>");
            buffer.AppendLine($"<p><h4>Title: {blog.Title}</h4></p>");
            buffer.AppendLine($"<p><h2>Authorname: {blog.AuthorName}</h2></p>");
            buffer.AppendLine($"<p>Description: {blog.Body}</p>");
            buffer.AppendLine($"<p><small>Created At: {blog.CreatedAt}</small></p>");
            buffer.AppendLine($"<p><small>Modified At: {blog.ModifiedAt}</small></p>");
        }
    }
}
