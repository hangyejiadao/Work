using Helper;
using Model.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    public class BaseDal<T> where T : Entity, new()
    { 
        public virtual async Task<bool> Add(T t)
        {
            return await SqlHelper.Add<T>(t); 
        } 
    }
}
