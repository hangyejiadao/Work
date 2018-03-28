using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.BaseModel;


namespace Model
{
    public class Image : IntEntity
    {
        /// <summary>
        /// Guid主键
        /// </summary>
        public Guid FkId { get; set; }

        /// <summary>
        ///商铺类型
        /// </summary>
        public TableType InfoType { get; set; }

        /// <summary>
        /// 图片地址
        /// </summary>
        public string ImageUrl { get; set; }
    }
}
