using System.Collections.Generic;

namespace fmptV2.Common
{
    /// <summary>
    /// 配置样例
    /// </summary>
    public class CORSConfig
    {
        /// <summary>
        /// 
        /// </summary>
        public List<VLKeyValue> Origins { set; get; }
    }
    /// <summary>
    /// 键值对
    /// </summary>
    public class VLKeyValue
    {
        public VLKeyValue()
        {
            Key = "";
            Value = "";
        }
        public VLKeyValue(string key, string value)
        {
            Key = key;
            Value = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Key { set; get; }
        /// <summary>
        /// /
        /// </summary>
        public string Value { set; get; }
    }

}