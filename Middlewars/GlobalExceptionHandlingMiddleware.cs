using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace C_.Middlewars
{
    public class GlobalExceptionHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {

    try
    {
        await next(context);
    }
    catch (Exception ex)
    {
        // Create a new response with the error message
        var response = new ServiceResponse<object>()
        {
            Success = false,
            Message = "Serveris Errori Gaarkviet Lashastan!",
        };

        // Set the response status code to 500
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        // Set the response content type to JSON
        context.Response.ContentType = "application/json";

        // Serialize the response to JSON
        var json = JsonSerializer.Serialize(response);

        // Write the JSON response to the response body
        await context.Response.WriteAsync(json);
    }
        }
    }
}