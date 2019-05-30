using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace EFDal
{
    public class DbContextFactory
    {
        /// <summary>
        /// 保证一次请求共用一个实例
        /// </summary>
        /// 返回值使用基类的好处是如果有多个上下文实例都可以作为返回值，而不用修改代码
        /// 上下文可以做到切换
        /// <returns></returns>
        public static DbContext GetCurrentDbContext()
        {
            //这里使用前面学过的代码
            //一个线程共用一个上下文实例，一次请求就用一个线程
            DbContext dbContext = CallContext.GetData("DbContext") as DbContext;
            if (dbContext == null)
            {
                dbContext = new DataModelContainer();
                CallContext.SetData("DbContext", dbContext);
            }
            return dbContext;

        }
    }
}
