//using AuthService.Models.DBModels;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;

//[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
//public class AuthorizeAttribute : Attribute, IAuthorizationFilter
//{
//    public void OnAuthorization(AuthorizationFilterContext context)
//    {
//        var endpoint = context.HttpContext.GetEndpoint();
//        if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() is object)
//        {
//            return;
//        }
//        var user = (ApplicationUser)context.HttpContext.Items["User"];
//        if (user == null)
//        {
//            // not logged in
//            context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
//        }
//        else
//            context.HttpContext.Items["User"] = user;
//    }
//}
