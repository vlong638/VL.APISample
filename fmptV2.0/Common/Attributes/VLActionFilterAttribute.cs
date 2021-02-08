using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace fmptV2.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class VLAuthenticationAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        public VLAuthenticationAttribute()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var endpoint = context.HttpContext.Features.Get<IEndpointFeature>()?.Endpoint;
            if (endpoint != null && (endpoint.Metadata.GetMetadata<AllowAnonymousAttribute>() != null || endpoint.Metadata.GetMetadata<VLAuthorizeAttribute>() != null))
            {
                base.OnActionExecuting(context);
                return;
            }
            var currentUser = CurrentUser.GetCurrentUser(context.HttpContext, APIContraints.RedisCache);
            if (currentUser == null)
            {
                context.Result = new UnauthorizedObjectResult("Unauthorized");
            }
            base.OnActionExecuting(context);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class VLAuthorizeAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        public VLAuthorizeAttribute(params SystemAuthority[] authorities)
        {
            Authorities.AddRange(authorities.Select(c => (long)c));
        }

        /// <summary>
        /// 
        /// </summary>
        public List<long> Authorities { set; get; } = new List<long>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var endpoint = context.HttpContext.Features.Get<IEndpointFeature>()?.Endpoint;
            if (endpoint != null && endpoint.Metadata.GetMetadata<AllowAnonymousAttribute>() != null)
            {
                base.OnActionExecuting(context);
                return;
            }
            var currentUser = CurrentUser.GetCurrentUser(context.HttpContext, APIContraints.RedisCache);
            if (currentUser == null)
            {
                context.Result = new UnauthorizedObjectResult("Unauthorized");
                return;
            }
            if (!currentUser.UserAuthorityIds.Any(c=>Authorities.Contains(c)))
            {
                context.Result = new UnauthorizedObjectResult("Unauthorized Access To Action");
                return;
            }
            base.OnActionExecuting(context);
        }
    }

    /// <summary>
    /// 权限 
    /// 前三位代表业务系统,后三位表示模块内功能模块,后三位表示功能模块内细分功能
    /// 通用功能为000
    /// </summary>
    public enum SystemAuthority
    {
        //None = 0,

        #region 项目管理 101
        查看项目列表 = 101001001,

        #endregion

        #region 账户系统 999

        查看用户列表 = 999001001,
        新增用户 = 999001002,
        修改用户信息 = 999001003,
        锁定用户状态 = 999001004,

        查看角色列表 = 999002001,
        新增角色 = 999002002,
        删除角色 = 999002003,
        修改角色信息 = 999002004,
        编辑角色权限 = 999002005,

        #endregion
    }
}
