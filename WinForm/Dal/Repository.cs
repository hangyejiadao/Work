﻿using Helper;
using Model.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    public class Repository<T> where T : Entity, new()
    { 
        public virtual async Task<object> Add(T t)
        {
            return await SqlHelper.Add<T>(t); 
        }

        public virtual async Task<List<T>> GetEntity(Expression<Func<T, bool>> where)
        {
            return await SqlHelper.GetEntity<T>(where);
        }
    }
}
