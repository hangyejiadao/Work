using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal;
using Model.BaseModel;

namespace Bll
{
    public class BaseBll<T> where T : Entity, new()
    {
        private BaseDal<T> dal = new BaseDal<T>();

        public virtual async Task<bool> Add(T t)
        {
            return await dal.Add(t);
        }
    }
}
