﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ConvocatoriaI.Interfaz
{
    public interface IServi<T> where T : class
    {
        void Create(T t);
        int Update(T t);
        bool Delete(T t);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> where);
    }
}
