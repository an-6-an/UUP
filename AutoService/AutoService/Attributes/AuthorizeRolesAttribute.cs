using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using AutoService.Services;

namespace AutoService.Attributes
{
    public class AuthorizeRolesAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string[] _roles;

        public AuthorizeRolesAttribute(params string[] roles)
        {
            _roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var authService = context.HttpContext.RequestServices.GetService<AuthService>();

            if (authService == null || !authService.IsAuthenticated())
            {
                context.Result = new RedirectToActionResult("Login", "Account", new { returnUrl = context.HttpContext.Request.Path });
                return;
            }

            if (_roles.Length > 0)
            {
                var userType = authService.GetCurrentUser()?.Type;
                if (string.IsNullOrEmpty(userType) || !_roles.Contains(userType))
                {
                    context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
                    return;
                }
            }
        }
    }
}