using fmptV2.Common.Cache;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fmptV2.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class APIContext
    {
        /// <summary>
        /// 
        /// </summary>
        public IHttpContextAccessor HttpContextAccessor { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public HttpContext HttpContext { set { HttpContextAccessor.HttpContext = value; } get { return HttpContextAccessor.HttpContext; } }

        /// <summary>
        /// 
        /// </summary>
        public RedisCache RedisCache { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public DBConfig DBConfig { set; get; }

        
        
        /// <summary>
        /// 
        /// </summary>
        public APIContext(IHttpContextAccessor httpContext, RedisCache redisCache, IOptions<DBConfig> dbConfig) : base()
        {
            HttpContextAccessor = httpContext;
            RedisCache = redisCache;
            DBConfig = dbConfig.Value;
        }

        #region Common

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetWebPath()
        {
            var request = HttpContext.Request;
            return new StringBuilder()
                .Append(request.Scheme)
                .Append("://")
                .Append(request.Host)
                .Append(request.PathBase)
                .ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public DbContext GetDBContext(string source)
        {
            var connectionString = DBConfig.ConnectionStrings.FirstOrDefault(c => c.Key == source);
            if (connectionString == null)
            {
                throw new NotImplementedException("尚未支持该类型的dbContext构建");
            }
            return DBHelper.GetDbContext(connectionString.Value);
        }

        internal CurrentUser GetCurrentUser()
        {
            return CurrentUser.GetCurrentUser(HttpContext, RedisCache);
        }

        internal string SetCurrentUser(CurrentUser currentUser)
        {
            return CurrentUser.SetCurrentUser(RedisCache, currentUser);
        }

        #endregion
    }
    public class CurrentUser
    {
        public CurrentUser()
        {
        }
        public CurrentUser(User data)
        {
            UserId = data.Id;
            UserName = data.Name;
        }

        public long UserId { set; get; }
        public string UserName { set; get; }
        public List<long> UserAuthorityIds { get; set; }

        public string GetSessionId()
        {
            return UserId + "_" + (UserId + DateTime.Now.ToString()).ToMD5();
        }

        internal static CurrentUser GetCurrentUser(HttpContext httpContext, RedisCache redisCache)
        {
            StringValues sessionId = StringValues.Empty;
            httpContext.Request.Headers.TryGetValue("VLSession", out sessionId);
            if (sessionId.FirstOrDefault().IsNullOrEmpty())
                return null;
            var currentUser = redisCache.Get<CurrentUser>(sessionId);
            if (currentUser == null)
                throw new NotImplementedException("当前用户不存在");
            return currentUser;
        }

        internal static string SetCurrentUser(RedisCache redisCache, CurrentUser currentUser)
        {
            var sessionId = currentUser.GetSessionId();
            redisCache.Set(sessionId, currentUser, DateTime.Now.AddHours(24));//TODO 这里时效应该是30分钟 根据用户操作来更新
            //var test = redisCache.Get<CurrentUser>(sessionId);
            return sessionId;
        }
    }
}
