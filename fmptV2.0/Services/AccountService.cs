using fmptV2.Common;
using fmptV2.Repositories;
using Microsoft.Extensions.Options;
using System;

namespace fmptV2.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class AccountService
    {
        APIContext APIContext { get; set; }
        DbContext FMPTDbContext { set; get; }

        //UserDepartmentRepository UserDepartmentRepository { set; get; }
        UserRepository UserRepository { set; get; }
        //UserRoleRepository UserRoleRepository { set; get; }
        //RoleAuthorityRepository RoleAuthorityRepository { set; get; }
        //RoleRepository RoleRepository { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public AccountService(APIContext apiContext)
        {
            APIContext = apiContext;
            FMPTDbContext = APIContext?.GetDBContext("FMPT");
            Init(FMPTDbContext);
        }

        private void Init(DbContext DbContext)
        {
            //repositories
            //UserDepartmentRepository = new UserDepartmentRepository(DbContext);
            UserRepository = new UserRepository(DbContext);
            //UserRoleRepository = new UserRoleRepository(DbContext);
            //RoleAuthorityRepository = new RoleAuthorityRepository(DbContext);
            //RoleRepository = new RoleRepository(DbContext);
        }

        internal ServiceResult<User> GetUserInfo(long userId)
        {
            return FMPTDbContext.DelegateNonTransaction(c =>
            {
                var user = UserRepository.GetById(userId);
                if (user == null)
                {
                    throw new NotImplementedException("用户不存在");
                }
                return user;
            });
        }

        internal ServiceResult<User> PasswordSignIn(string userName, string password)
        {
            return FMPTDbContext.DelegateNonTransaction(c =>
            {
                var user = UserRepository.GetByUserNameAndPassword(userName, password.ToMD5());
                if (user == null)
                {
                    throw new NotImplementedException("用户不存在或密码错误");
                }
                return user;
            });
        }
    }
}
