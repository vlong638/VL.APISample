using fmptV2.Common;
using fmptV2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace fmptV2.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]/[action]")]
    public class HomeController : ControllerBase
    {
        /// <summary>
        /// 登陆
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public APIResult<string> Login([FromServices] APIContext apiContext, [FromServices] AccountService service, [FromBody] LoginRequest request)
        {
            var result = service.PasswordSignIn(request.UserName, request.Password);
            if (result.IsSuccess)
            {
                var currentUser = new CurrentUser(result.Data);
                //TODO
                //var userAuthorityIds = service.GetSystemAuthorityIds(currentUser.UserId);
                //currentUser.UserAuthorityIds = userAuthorityIds.Data;
                var token = apiContext.SetCurrentUser(currentUser);
                return new APIResult<string>(token, result.Messages);
            }
            return new APIResult<string>(null, result.Messages);
        }
    }
}
