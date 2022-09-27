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
    public class PlainTextOutputFormatter : TextOutputFormatter
    {
        public PlainTextOutputFormatter()
        {
            SupportedMediaTypes.Add("text/plain");
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
                    ConvertToPlainText(buffer, blog);
            }
            else
            {
                ConvertToPlainText(buffer, (BlogDetailsDto)context.Object);
            }

            await response.WriteAsync(buffer.ToString());
        }

        private static void ConvertToPlainText(StringBuilder buffer, BlogDetailsDto blog)
        {
            buffer.AppendLine($"Id : {blog.Id}");
            buffer.AppendLine($"Title : {blog.Title}");
            buffer.AppendLine($"Authorname : {blog.AuthorName}");
            buffer.AppendLine($"Description : {blog.Body}");
            buffer.AppendLine($"Created At : {blog.CreatedAt}");
            buffer.AppendLine($"Modified At : {blog.ModifiedAt}");
        }
    }
}
