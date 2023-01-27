using FintranetTest.Presentation.Server.Infrastructures;
using FluentResults;
using Microsoft.AspNetCore.Http;
using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;

namespace FintranetTest.Presentation.Server.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"The following error happened: {ex.Message}");

                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                var stream = context.Response.Body;
                await JsonSerializer.SerializeAsync(stream, Result.Fail(ex.Message).ToAPIResponse());
            }
        }
    }
}
