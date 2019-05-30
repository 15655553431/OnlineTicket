using OnlineGuestTicket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Md5;
using IBll;
using Bll;
using Model.Enum;
using Common.Cache;
using Model;

namespace OnlineGuestTicket.Controllers
{
    [LoginCheckFilterAttribute(IsCheck = false)]
    public class UserInfoController : Controller
    {
        //
        // GET: /UserInfo/
        private IUserInfoService userinfoService = new UserInfoService();

        /// <summary>
        /// 登陆页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 登陆信息验证
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ProcessLogin()
        {
            //处理用户名和密码
            string login = Request["username"];
            string pwd = Request["password"].GetMd5();

            var userinfo = userinfoService.GetEntities(u => u.Login == login && u.Pwd == pwd && u.Type == (int)UiTypeEnum.Guest).FirstOrDefault();

            if (userinfo == null)
            {
                return Json("用户名或密码错误!");
            }

            //登录成功，把用户信息放到Cache里,记录登陆时间
            userinfo.LoginDate = DateTime.Now;
            userinfoService.Update(userinfo);
            //1,立即分配一个标志，guid，作为key存入mm，同时把这个key存入客户端cookie
            string userLoginId = Guid.NewGuid().ToString();

            //把用户数据写入缓存
            CacheHelper.AddCache(userLoginId, userinfo, DateTime.Now.AddMinutes(20));

            //往客户端写入cookie
            Response.Cookies["userLoginId"].Value = userLoginId;

            return Json("ok");

        }


        /// <summary>
        /// 退出登陆请求
        /// </summary>
        /// <returns></returns>
        public ActionResult ExitLogin()
        {
            string userloginid = Request.Cookies["userLoginId"].Value.ToString();
            //登陆信息置空
            if (!string.IsNullOrWhiteSpace(userloginid))
            {
                CacheHelper.RemCache(userloginid);
            }

            return RedirectToAction("Login");
        }

        /// <summary>
        /// 注册页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ZhuCe()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ZhuCe(UserInfo ui)
        {
            ui.GUID = Guid.NewGuid().ToString();
            ui.Type = (int)UiTypeEnum.Guest;
            ui.Status = (int)UiStatusEnum.Block;
            ui.SignDate = DateTime.Now;
            ui.Pwd = ui.Pwd.GetMd5();
            if (userinfoService.LoginExist(ui.Login))
            {
                return Json("err");
            }
            if (userinfoService.Add(ui))
            {
                return Json("ok");
            }
            else
            {
                return Json("err");
            }
        }


        /// <summary>
        /// 获取当前登陆人信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetNow()
        {
            if (HttpContext.Request.Cookies["userLoginId"] == null)
            {
                return HttpNotFound();
            }
            //从缓存里面拿到userLoginId信息
            string userLoginId = HttpContext.Request.Cookies["userLoginId"].Value.ToString();
            UserInfo userInfo = CacheHelper.GetCache(userLoginId) as UserInfo;
            return Json(userInfo);
        }


        /// <summary>
        /// 对当前登陆人设置新密码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetNewPwd(string newpwd = "")
        {
            if (HttpContext.Request.Cookies["userLoginId"] == null)
            {
                return HttpNotFound();
            }
            //从缓存里面拿到userLoginId信息
            string userLoginId = HttpContext.Request.Cookies["userLoginId"].Value.ToString();
            UserInfo userInfo = CacheHelper.GetCache(userLoginId) as UserInfo;
            if (newpwd.Length > 2 && userInfo != null)
            {
                if (userinfoService.EditPwd(userInfo.ID, newpwd))
                {
                    return Json("ok");
                }
            }

            return Json("err");
        }


    }
}
