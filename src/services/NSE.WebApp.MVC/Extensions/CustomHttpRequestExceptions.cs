using System;
using System.Net;

namespace NSE.WebApp.MVC.Extensions
{
    public class CustomHttpRequestExceptions : Exception
    {
        public HttpStatusCode StatusCode;

        public CustomHttpRequestExceptions() { }

        public CustomHttpRequestExceptions(string message, Exception innException) : base(message, innException) { }

        public CustomHttpRequestExceptions(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }
    }
}