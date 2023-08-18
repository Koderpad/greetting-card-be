using Domain.ExceptionModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI.Middlewares
{
    public class AdminAuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context) { 
            if (!context.HttpContext.User.IsInRole("Admin"))
            {
                throw new AccessDeniedException();
            }
            return;
        }
    }
}
