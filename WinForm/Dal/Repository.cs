using Helper;
using Model.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dal
{
    public class Repository<T> where T : Entity, new()
    { 


        public virtual async Task<object> AddAsync(T t)
        {
            return await SqlHelper.AddAsync<T>(t); 
        }

        public virtual     object  Add (T t)
        {
            return   SqlHelper.Add<T>(t);
        }

        public virtual bool AddBool(T t)
        {
            return SqlHelper.AddBool<T>(t);
        }


        public virtual   List<T>  GetEntity(Expression<Func<T, bool>> where)
        {
            return   new Context().Set<T>().Where(where).ToList();
        }
    }
}
