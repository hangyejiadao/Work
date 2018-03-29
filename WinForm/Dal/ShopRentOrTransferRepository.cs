using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dal
{
    public class ShopRentOrTransferRepository : Repository<ShopRentOrTransfer>
    {
        public override object Add(ShopRentOrTransfer t)
        {
            if (GetEntity(u => u.Phone == t.Phone).Count() > 0)
                return false;
            else
            {
               return   base.AddBool(t);
              
            }
        }

    }
}
