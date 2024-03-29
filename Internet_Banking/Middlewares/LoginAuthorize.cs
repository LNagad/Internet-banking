using Internet_Banking.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SocialMedia.Middlewares;

public class LoginAuthorizeNoUser : IAsyncActionFilter
{
    private readonly ValidateUserSession _userSession;

    public LoginAuthorizeNoUser(ValidateUserSession userSession)
    {
        _userSession = userSession;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!_userSession.HasUser())
        {

            var controller = (UserController)context.Controller;
            context.Result = controller.RedirectToAction("index", "user");
   
        }
        else
        {
            await next();
        }
    }
}