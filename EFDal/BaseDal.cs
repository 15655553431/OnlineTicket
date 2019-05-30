using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EFDal
{
    /// <summary>
    /// 职责：封装所有的Dal的公共的crud的方法
    /// 类的职责要单一
    /// </summary>
    public class BaseDal<T> where T : class ,new()
    {
        //curd
        //不能每次都new一个，那太浪费了
        //DataModelContainer Db = new DataModelContainer();
        //这里返回值用基类，依赖抽象编程，好处多多，应对变化的时候，改变的最小
        public DbContext Db
        {
            get { return DbContextFactory.GetCurrentDbContext(); }
        }

        public IQueryable<T> GetEntities(Expression<Func<T, bool>> whereLambda)
        {
            return Db.Set<T>().Where(whereLambda).AsQueryable();
        }

        //分页
        public IQueryable<T> GetPageEntities<S>(int pageSize, int pageIndex, out int total, Expression<Func<T, bool>> whereLambda, Expression<Func<T, S>> orderLambda, bool isAsc)
        {
            total = Db.Set<T>().Where(whereLambda).Count();
            if (isAsc)
            {
                var temp = Db.Set<T>().Where(whereLambda)
                    .OrderBy<T, S>(orderLambda)
                    .Skip(pageSize * (pageIndex - 1))
                    .Take(pageSize).AsQueryable();
                return temp;
            }
            else
            {
                var temp = Db.Set<T>().Where(whereLambda)
                    .OrderByDescending<T, S>(orderLambda)
                    .Skip(pageSize * (pageIndex - 1))
                    .Take(pageSize).AsQueryable();
                return temp;
            }
        }
        public T Add(T Entity)
        {
            Db.Set<T>().Add(Entity);
            //放到dbsession里面提交
            //Db.SaveChanges();
            return Entity;
        }

        public bool Update(T Entity)
        {
            Db.Entry(Entity).State = EntityState.Modified;
            //放到dbsession里面提交
            //return Db.SaveChanges() > 0;
            return true;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public bool Delete(T Entity)
        {
            Db.Entry(Entity).State = EntityState.Deleted;
            //放到dbsession里面提交
            //return Db.SaveChanges() > 0;
            return true;
        }

    }
}
