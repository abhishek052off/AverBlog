using AverBlog.Api.SessionModels;
using System.Security.Claims;

namespace AverBlog.Api.CustomMiddlewares
{
    public class UserSessionMiddleWare
    {
        private readonly RequestDelegate _next;

        public UserSessionMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context , UserSession userSession)
        {
            
            var contextUser = context?.User;

            if (contextUser != null && contextUser.Identities.Any())
            {

                userSession.Id = Convert.ToInt32(contextUser.FindFirstValue(ClaimTypes.NameIdentifier))  ;
                userSession.FullName = contextUser.FindFirstValue("full-name");
                userSession.Email = contextUser.FindFirstValue(ClaimTypes.Email);
                userSession.Username = contextUser.FindFirstValue(ClaimTypes.Name);
            }
            await _next(context);
        }
    }
}
