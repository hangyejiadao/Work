using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.BaseModel;

namespace Model
{
    /// <summary>
    /// 店铺求租
    /// </summary>
    public class ShopBegRent : GuidEntity
    {

        /// <summary>
        /// 标题
        /// </summary>
        public string InfoTitle { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string InfoContent { get; set; }

        /// <summary>
        /// 客户名字
        /// </summary>
        public string Customer { get; set; }


        /// <summary>
        /// 电话号码
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 最小面积
        /// </summary>
        public double MinArea { get; set; }

        /// <summary>
        /// 最大面积
        /// </summary>
        public double MaxArea { get; set; }

        /// <summary>
        /// 最小面积
        /// </summary>
        public double MinRentMoney { get; set; }

        /// <summary>
        /// 最大面积
        /// </summary>
        public double MaxRentMoney { get; set; }

        /// <summary>
        /// 对应的城市id
        /// </summary>
        public string AreaId { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 区域名字
        /// </summary>
        public string AreaName { get; set; }
    }
}
