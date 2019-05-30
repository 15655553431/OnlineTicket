using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IBll
{
    public interface IBaseService<T> where T : class ,new()
    {
        IQueryable<T> GetEntities(Expression<Func<T, bool>> whereLambda);

        //分页
        IQueryable<T> GetPageEntities<S>(int pageSize, int pageIndex, out int total, Expression<Func<T, bool>> whereLambda, Expression<Func<T, S>> orderLambda, bool isAsc);

        bool Add(T Entity);

        bool Update(T Entity);

        bool Delete(T Entity);
    }
}
