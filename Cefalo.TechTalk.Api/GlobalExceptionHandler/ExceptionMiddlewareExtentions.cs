using Cefalo.TechTalk.Service.Utils.Contracts;
using Cefalo.TechTalk.Service.Utils.CustomErrorHandler;
using Cefalo.TechTalk.Service.Utils.Models;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace Cefalo.TechTalk.Api.GlobalExceptionHandler
{
    public static class ExceptionMiddlewareExtentions
    {

        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    //context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature == null)
                    {
                       // logger.LogError($"Something went wrong: {contextFeature.Error}");
                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal Server Error."
                        }.ToString());
                    }
                    else
                    {
                        context.Response.StatusCode = GetStatusCode(contextFeature.Error.GetType());
                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = contextFeature.Error.Message,
                        }.ToString());

                    }
                });
            });
        }

        public static int GetStatusCode(Type type)
        {
            if (type == typeof(BadRequestException)) return (int)HttpStatusCode.BadRequest;
            if (type == typeof(ForbiddenExceptoin)) return (int)HttpStatusCode.Forbidden;
            if (type == typeof(NotFoundException)) return (int)HttpStatusCode.NotFound;
            if (type == typeof(UnAuthorizedException)) return (int)HttpStatusCode.Unauthorized;
            return (int)HttpStatusCode.InternalServerError;

        }
    }
}
