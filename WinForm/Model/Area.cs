using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.BaseModel;


namespace Model
{
    public class Area : IntEntity
    {
        /// <summary>
        /// 区域名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 网址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 父Id
        /// </summary>
        public string ParentId { get; set; }
    }
}
