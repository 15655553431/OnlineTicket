using Bll;
using Common.Cache;
using IBll;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineTicket.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            //从缓存里面拿到userLoginId信息
            string userLoginId = HttpContext.Request.Cookies["userLoginId"].Value.ToString();
            UserInfo userInfo = CacheHelper.GetCache(userLoginId) as UserInfo;
            return View(userInfo);
        }


        /// <summary>
        /// 图表一，展示每日售票数量
        /// </summary>
        /// <returns></returns>
        public ActionResult Chartone()
        {
            return View();
        }


        /// <summary>
        /// 图表二，展示每日收入统计
        /// </summary>
        /// <returns></returns>
        public ActionResult Charttwo()
        {
            return View();
        }

        /// <summary>
        /// 欢迎界面
        /// </summary>
        /// <returns></returns>
        public ActionResult Welcome()
        {
            return View();
        }
    }
}
