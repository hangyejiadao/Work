using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.BaseModel;

namespace Model
{
    /// <summary>
    /// 抓取错误的网址
    /// </summary>
    public class ErrorUrl : IntEntity
    {
        /// <summary>
        /// 网址名字
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 网址类型
        /// </summary>
        public UrlType UrlType { get; set; }

    }
}
