﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.BaseModel;


namespace Model
{
    public class ShopTransfer : GuidEntity
    {
        /// <summary>
        /// 信息类型
        /// </summary>
        public InfoType InfoType { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string InfoTitle { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string InfoContent { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string DetailAddress { get; set; }

        /// <summary>
        /// 商铺面积
        /// </summary>
        public decimal ShopArea { get; set; }

        /// <summary>
        /// 转让费
        /// </summary>
        public decimal TransFerMoney { get; set; }

        /// <summary>
        /// 客户名字
        /// </summary>
        public string Customer { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }


    }


}
