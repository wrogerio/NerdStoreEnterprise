using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Http;

namespace NSE.WebApp.MVC.Extensions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (CustomHttpRequestExceptions e)
            {
                HandleRequestExceptionAsync(httpContext, e);
            }
        }

        private static void HandleRequestExceptionAsync(HttpContext context, CustomHttpRequestExceptions httpRequestExceptions)
        {
            if (httpRequestExceptions.StatusCode == HttpStatusCode.Unauthorized)
            {
                context.Response.Redirect($"/login?ReturnUrl={context.Request.Path}");
                return;
            }

            context.Response.StatusCode = (int) httpRequestExceptions.StatusCode;
        }
    }
}