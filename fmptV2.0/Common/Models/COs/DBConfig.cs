using System;
using System.Collections.Generic;
using System.Linq;

namespace fmptV2.Common
{
    /// <summary>
    /// 配置样例
    /// </summary>
    public class DBConfig
    {
        /// <summary>
        /// 
        /// </summary>
        public List<VLKeyValue> ConnectionStrings { set; get; }

        internal string GetConnectingString(string v)
        {
            var connectionString =  this.ConnectionStrings.FirstOrDefault(c => c.Key == v);
            if (connectionString==null)
            {
                throw new NotImplementedException("缺少的数据库配置:" + v);
            }
            return connectionString.Value;
        }
    }
}