using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Cache
{
    public class CacheHelper
    {

        public static ICacheWriter CacheWriter = new HttpRuntimeCacheWriter();//{ get; set; }//

        //这里可以使用spring.net属性注入方式

        static CacheHelper()
        {
            //方案二：在CacheHelper构造函数里面，通过容器创建对象
            //第一步，初始化容器
            //创建spring容器上下文

            //IApplicationContext ctx = ContextRegistry.GetContext();

            //通过容器创建对象,给静态属性注入
            //ctx.GetObject("CacheHelper");

            //或者直接赋值,通过容器获取( 这种方案行不通，还不清楚原因)
            //CacheHelper.CacheWriter = ctx.GetObject("CacheHelper") as ICacheWriter;

        }

        /// <summary>
        /// 添加一个缓存
        /// </summary>
        /// <param name="key">关键值</param>
        /// <param name="value">缓存值</param>
        /// <param name="expDate">过期时间</param>
        public static void AddCache(string key, object value, DateTime expDate)
        {
            CacheWriter.AddCache(key, value, expDate);
        }

        /// <summary>
        /// 添加一个缓存（过期时间为默认值：20分钟）
        /// </summary>
        /// <param name="key">关键值</param>
        /// <param name="value">缓存值</param>
        public static void AddCache(string key, object value)
        {
            CacheWriter.AddCache(key, value);
        }

        /// <summary>
        /// 根据关键值获得一个缓存值
        /// </summary>
        /// <typeparam name="T">缓存值类型</typeparam>
        /// <param name="key">缓存值</param>
        /// <returns></returns>
        public static T GetCache<T>(string key)
        {
            return (T)CacheWriter.GetCache(key);
        }

        /// <summary>
        /// 根据关键值获得一个缓存值
        /// </summary>
        /// <param name="key">缓存值</param>
        /// <returns></returns>
        public static object GetCache(string key)
        {
            return CacheWriter.GetCache(key);
        }


        /// <summary>
        /// 给一个已有的关键值设置新的缓存值，并设置新的过期时间
        /// </summary>
        /// <param name="key">已有关键值</param>
        /// <param name="value">新的缓存值</param>
        /// <param name="expDate">新的过期时间</param>
        public static void SetCache(string key, object value, DateTime expDate)
        {
            CacheWriter.SetCache(key, value, expDate);
        }

        /// <summary>
        /// 给一个已有的关键值设置新的缓存值，过期时间为默认值（20分钟）
        /// </summary>
        /// <param name="key">已有关键值</param>
        /// <param name="value">新的缓存值</param>
        public static void SetCache(string key, object value)
        {
            CacheWriter.SetCache(key, value);
        }


        /// <summary>
        /// 根据已有的关键值删除缓存值
        /// </summary>
        /// <param name="key">已有关键值</param>
        public static void RemCache(string key)
        {
            CacheWriter.RemCache(key);
        }
    }
}
