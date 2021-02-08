using System.Data;

namespace fmptV2.Common
{
    /// <summary>
    /// 数据库访问单元:支持事务及跨库协作
    /// </summary>
    public class DbGroup
    {
        /// <summary>
        /// 
        /// </summary>
        public IDbConnection Connection;
        /// <summary>
        /// 
        /// </summary>
        public IDbCommand Command;
        /// <summary>
        /// 
        /// </summary>
        public IDbTransaction Transaction;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbConnection"></param>
        public DbGroup(IDbConnection dbConnection)
        {
            this.Connection = dbConnection;
            this.Command = Connection.CreateCommand();
        }

        ~DbGroup()
        {
            Command.Dispose();
            Connection.Dispose();
        }
    }
}
