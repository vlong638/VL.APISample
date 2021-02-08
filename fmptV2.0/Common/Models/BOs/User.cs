using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace fmptV2.Common
{
    /// <summary>
    /// 
    /// </summary>
    [Table(TableName)]
    public class User
    {
        /// <summary>
        /// 
        /// </summary>
        public const string TableName = "User";

        public long Id { set; get; }
        public string Name { set; get; }
        public string Password { set; get; }
        public string NickName { set; get; }
        public bool IsDeleted { set; get; }
        public Sex Sex { set; get; }
        public string Phone { set; get; }
        public DateTime CreatedAt { set; get; }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum Sex
    {
        /// <summary>
        /// 
        /// </summary>
        None= 0,
        /// <summary>
        /// 
        /// </summary>
        [Description("男")]
        Man = 1,
        /// <summary>
        /// 
        /// </summary>
        [Description("女")]
        Woman = 2,
    }
}
