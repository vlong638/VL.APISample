using System;
using System.Data;

namespace fmptV2.Common
{
    /// <summary>
    /// 数据库访问上下文
    /// </summary>
    public class DbContext: UnitOfWork
    {
        /// <summary>
        /// 
        /// </summary>
        public DbGroup DbGroup { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public DbContext()
        { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        public DbContext(IDbConnection connection)
        {
            DbGroup = new DbGroup(connection);
        }
    }

    /// <summary>
    /// 工作单元模式
    /// 负责组合事务的定义
    /// </summary>
    public interface UnitOfWork
    {


    }

    /// <summary>
    /// 
    /// </summary>
    public static class DbContextEX
    {
        /// <summary>
        /// 扩展事务(服务层)通用处理
        /// </summary>
        public static ServiceResult<T> DelegateTransaction<T>(this DbContext context, Func<DbGroup, T> exec)
        {
            try
            {

                context.DbGroup.Connection.Open();
                context.DbGroup.Transaction = context.DbGroup.Connection.BeginTransaction();
                context.DbGroup.Command.Transaction = context.DbGroup.Transaction;
                try
                {
                    var result = exec(context.DbGroup);
                    context.DbGroup.Transaction.Commit();
                    context.DbGroup.Connection.Close();
                    return new ServiceResult<T>(result);
                }
                catch (Exception ex)
                {
                    context.DbGroup.Transaction.Rollback();
                    context.DbGroup.Connection.Close();

                    //集成Log4Net
                    Log4NetLogger.Error("DelegateTransaction Exception", ex);

                    return new ServiceResult<T>(default(T), ex.Message);
                }
            }
            catch (Exception e)
            {
                //集成Log4Net
                Log4NetLogger.Error("打开数据库连接配置失败,当前数据库连接," + context.DbGroup.Connection.ConnectionString);
                return new ServiceResult<T>(default(T), e.Message);
            }
        }
        /// <summary>
        /// 扩展事务(服务层)通用处理
        /// </summary>
        public static ServiceResult<T> DelegateNonTransaction<T>(this DbContext context, Func<DbGroup, T> exec)
        {
            try
            {
                context.DbGroup.Connection.Open();
                try
                {
                    var result = exec(context.DbGroup);
                    context.DbGroup.Connection.Close();
                    return new ServiceResult<T>(result);
                }
                catch (Exception ex)
                {
                    context.DbGroup.Connection.Close();

                    Log4NetLogger.Error("DelegateTransaction Exception", ex);

                    return new ServiceResult<T>(default(T), ex.Message);
                }
            }
            catch (Exception e)
            {
                //集成Log4Net
                Log4NetLogger.Error("打开数据库连接配置失败,当前数据库连接," + context.DbGroup.Connection.ConnectionString);
                return new ServiceResult<T>(default(T), e.Message);
            }
        }
    }
}
