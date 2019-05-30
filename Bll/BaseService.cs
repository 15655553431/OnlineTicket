using DalFactory;
using IDal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bll
{
    /// <summary>
    /// 父类要逼迫子类给父类的一个属性赋值
    /// 赋值操作必须在父类方法调用之前先执行
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseService<T> where T : class,new()
    {
        public IBaseDal<T> CurrentDal { get; set; }

        public IDbSession DbSession
        {
            get { return DbSessionFactory.GetCurrentDbSession(); }
        }

        public BaseService()//基类给个构造函数
        {
            SetCurrentDal();//抽象方法
        }



        /// <summary>
        /// 抽象方法，要求子类必须重写这个方法
        /// </summary>
        public abstract void SetCurrentDal();


        public IQueryable<T> GetEntities(Expression<Func<T, bool>> whereLambda)
        {
            return CurrentDal.GetEntities(whereLambda);
        }

        //分页
        public IQueryable<T> GetPageEntities<S>(int pageSize, int pageIndex, out int total, Expression<Func<T, bool>> whereLambda, Expression<Func<T, S>> orderLambda, bool isAsc)
        {
            return CurrentDal.GetPageEntities<S>(pageSize, pageIndex, out total, whereLambda, orderLambda, isAsc);
        }

        public bool Add(T entity)
        {
            CurrentDal.Add(entity);
            return DbSession.SaveChanges() > 0;
        }

        public bool Update(T entity)
        {
            CurrentDal.Update(entity);
            return DbSession.SaveChanges() > 0;
        }

        public bool Delete(T entity)
        {
            CurrentDal.Delete(entity);
            return DbSession.SaveChanges() > 0;
        }
    }
}
