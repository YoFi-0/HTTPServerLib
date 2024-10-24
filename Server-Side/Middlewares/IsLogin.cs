using Domain.Model;
using Domain.ReturnsModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Server_Side.Services.Interfaces;
using System.Data;

namespace Server_Side.Middlewares
{
    public class IsLogin : Attribute, IAsyncActionFilter
    {
        public IsLogin()
        {

        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var jwt = context.HttpContext.Request.Headers["jwt"].ToString();
            var _jwtService = context.HttpContext.RequestServices.GetService<IJwtService<JWTDataModel>>();
            if(_jwtService == null)
            {
                context.Result = new BadRequestObjectResult(new ReturnModel()
                {
                    Comment = "Missing Service",
                    IsSucceeded = false
                });
                return;
            }
            var session = _jwtService.Get(jwt);
            if (jwt is null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            if(!session.IsValid)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            await next();
        }
    }
}
