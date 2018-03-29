using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dal
{
    public class ShopBegRentRepository : Repository<ShopBegRent>
    {
        public override object Add(ShopBegRent t)
        {
            if (GetEntity(u => u.Phone == t.Phone).Count() > 0)
                return false;
            else
            {
                return base.Add(t);
                
            } 
        }
    }
}
