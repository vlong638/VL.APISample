﻿using Dapper;
using fmptV2.Common;
using fmptV2.Common.RepositorySolution;
using System.Collections.Generic;
using System.Linq;

namespace fmptV2.Repositories
{
    internal class UserRepository : Repository<User>
    {
        internal UserRepository(DbContext context) : base(context)
        {
        }

        internal long InsertOne(User user)
        {
            return _connection.ExecuteScalar<long>(@"INSERT INTO [User]([Name], [Password], [NickName], [Phone], [Sex], [IsDeleted], [CreatedAt]) 
VALUES (@name, @password, @nickname, @phone, @sex, @isdeleted, @CreatedAt);SELECT @@IDENTITY;"
                , user, transaction: _transaction);
        }

        internal User GetByName(string userName)
        {
            return _connection.Query<User>("select * from [User] where Name = @userName;"
                , new { userName }, transaction: _transaction)
                .FirstOrDefault();
        }

        internal User GetById(long userId)
        {
            return _connection.Query<User>("select * from [User] where Id = @userId;"
                , new { userId }, transaction: _transaction)
                .FirstOrDefault();
        }

        internal List<User> GetAllUsers()
        {
            return _connection.Query<User>("select * from [User] ;"
                , transaction: _transaction)
                .ToList();
        }

        internal List<User> GetAllUsersIdAndName()
        {
            return _connection.Query<User>("select Id,Name from [User] ;" 
                , transaction: _transaction)
                .ToList();
        }

        internal User GetByUserNameAndPassword(string userName, string password)
        {
            return _connection.Query<User>("select * from [User] where Name = @UserName and Password = @Password and IsDeleted = 0;"
                , new { userName, password }
                , transaction: _transaction)
                .FirstOrDefault();
        }

        internal List<User> GetPagedUsers(int page, int limit, string username, string nickname)
        {
            var sql = @$"
select *
from [User]
where 1=1
{ (username.IsNullOrEmpty()?"":" and name like @name")}
{ (nickname.IsNullOrEmpty()?"": " and nickname like @nickname")}
order by id desc
offset {(page - 1) * limit} rows fetch next {limit} rows only
";
            return _connection.Query<User>(sql
                , new { page, limit, name=$"%{username}%" }
                , transaction: _transaction)
                .ToList();
        }

        internal int GetPagedUserCount(string username, string nickname)
        {
            var sql = @$"
select count(*) 
from [User]
where 1=1
{ (username.IsNullOrEmpty() ? "" : " and name like @name")}
{ (nickname.IsNullOrEmpty() ? "" : " and nickname like @nickname")}
";
            return _connection.ExecuteScalar<int>(sql
                , new { name = $"%{username}%" }
                , transaction: _transaction);
        }

        internal int UpdateUserStatus(long userId, bool fromStatus, bool toStatus)
        {
            return _connection.Execute("update [User] set IsDeleted = @toStatus where id = @userId and IsDeleted =@fromStatus;"
                , new { userId, fromStatus, toStatus }
                , transaction: _transaction);
        }

        internal int UpdateUserInfo(User user)
        {
            return _connection.Execute("update [User] set NickName = @NickName,Sex = @Sex,Phone=@Phone where id = @id;"
                , user
                , transaction: _transaction);
        }
    }
}