using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.BaseModel
{
    /// <summary>
    /// 网站类型
    /// </summary>
    public enum UrlType
    {
        /// <summary>
        /// 每页
        /// </summary>
        Page = 0,
        /// <summary>
        /// 每条
        /// </summary>
        Item = 1,

        /// <summary>
        /// 总的
        /// </summary>
        All = 2,
    }
}
