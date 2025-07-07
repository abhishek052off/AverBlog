using AverBlog.Data.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace AverBlog.Api.CustomMiddlewares
{
    public class GlobalExceptionHandlerMiddleWare
    {
        private RequestDelegate _next;

        public GlobalExceptionHandlerMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //Here 

            try
            {
                await _next(context);
            }
            catch (UnauthorisedException ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(GetProblemDetails("Unauthorised User", ex, context));
            }
            catch (NotFoundException ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsJsonAsync( GetProblemDetails("Resource Not Found" ,ex ,context));
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                await context.Response.WriteAsJsonAsync(GetProblemDetails("Some Thing went wrong", ex, context));
            }
            
            //After or in return 
        }

        private ProblemDetails GetProblemDetails( string message,Exception ex  , HttpContext context  )
        {
            return new ProblemDetails()
            {
                Title = message,
                Status = context.Response.StatusCode,
                Detail = ex.Message,
                Instance = context.Request.Path,
                Extensions =
                    {
                        ["ErrorMessage"] = ex.Message
                    }
            };
        }
    }
}
