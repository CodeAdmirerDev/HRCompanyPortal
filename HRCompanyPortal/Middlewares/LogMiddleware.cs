using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HRCompanyPortal.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class LogMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger _logger;

        public LogMiddleware(RequestDelegate next,ILoggerFactory logFact)
        {
            _next = next;
            _logger = logFact.CreateLogger<LogMiddleware>();
        }


        public async Task InvokeAsync(HttpContext httpContext)
        {


            using ( var reader =new StreamReader(httpContext.Request.Body))
            {

                var reqbody = await reader.ReadToEndAsync();

                _logger.LogInformation(reqbody);
                _logger.LogWarning(reqbody);
                _logger.LogError(reqbody);
                _logger.LogDebug(reqbody);
                _logger.LogInformation("GET Pages.PrivacyModel called.");

            }

                await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class LogMiddlewareExtensions
    {
        public static IApplicationBuilder UseLogMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LogMiddleware>();
        }
    }
}
