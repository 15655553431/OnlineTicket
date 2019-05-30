using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Common.Md5;
using IBll;
using Bll;
using OnlineTicket.Models;
using Common.VCode;
using Common.Cache;
using Model.Enum;
using Model.other;
using System.Linq.Expressions;

namespace OnlineTicket.Controllers
{
    public class UserInfoController : Controller
    {

        private IUserInfoService userinfoService = new UserInfoService();

        [LoginCheckFilterAttribute(IsCheck = false)]
        public ActionResult Login()
        {
            return View();
        }

        [LoginCheckFilterAttribute(IsCheck = false)]
        public ActionResult ShowVCode()
        {
            VCodeHelper validateCode = new VCodeHelper();
            string strCode = validateCode.RandomCode(4);

            //把验证码放到session中
            Session["VCode"] = strCode;

            byte[] imgBytes = validateCode.CreatVCodeImgs(strCode);

            return File(imgBytes, "image/jpeg");
        }

        /// <summary>
        /// 登陆信息验证
        /// </summary>
        /// <returns></returns>
        [LoginCheckFilterAttribute(IsCheck = false)]
        public ActionResult ProcessLogin()
        {
            //1 处理验证码

            string strCode = Request["vcode"];
            //拿到session中的验证码做对比
            string sessionCode = Session["VCode"] as string;

            //取过验证码，马上置空
            Session["VCode"] = null;

            //在测试阶段可以注释这段，就不需要填写验证码了
            //if (string.IsNullOrEmpty(sessionCode) || strCode != sessionCode)
            //{
            //    // "alert('ok！');", "text/javascript"
            //    return Json("验证码错误！");
            //}


            //2 处理用户名和密码
            string login = Request["login"];
            string pwd = Request["pwd"].GetMd5();

            var userinfo = userinfoService.GetEntities(u => u.Login == login && u.Pwd == pwd && u.Type == (int)UiTypeEnum.Admin).FirstOrDefault();

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


            //正确跳转到首页
            //return RedirectToAction("Test","Index");

            return Json("ok");

        }

        /// <summary>
        /// 登陆成功后，获取跳转的页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ActionUrl()
        {
            return RedirectToAction("Index", "Home");
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
        /// 已通过核验的旅客账户管理显示页面
        /// </summary>
        /// <returns></returns>
        public ActionResult GuestIndex()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GuestIndex(PagingInfo pi, UserInfo ui)
        {
            pi.PageIndex = pi.PageIndex == 0 ? 1 : pi.PageIndex;
            pi.PageSize = pi.PageSize == 0 ? 5 : pi.PageSize;

            ui.IdCard = String.IsNullOrWhiteSpace(ui.IdCard) ? "" : ui.IdCard;
            ui.UName = String.IsNullOrWhiteSpace(ui.UName) ? "" : ui.UName;
            ui.Login = String.IsNullOrWhiteSpace(ui.Login) ? "" : ui.Login;

            Expression<Func<UserInfo, bool>> wherelambda = u => (u.Type == (int)UiTypeEnum.Guest && u.Status == (int)UiStatusEnum.Pass && u.IdCard != null && u.UName != null && u.Login != null && u.IdCard.Contains(ui.IdCard) && u.UName.Contains(ui.UName) && u.Login.Contains(ui.Login));

            int count = 0;
            var guestlist = userinfoService.GetPageEntities<int>(pi.PageSize, pi.PageIndex, out count, wherelambda, u => u.ID, true) as IEnumerable<UserInfo>;
            pi.TotalItems = count;

            return Json(Tuple.Create(guestlist, pi));
        }


        /// <summary>
        /// 没有通过核验的旅客账户管理显示页面,等待检验
        /// </summary>
        /// <returns></returns>
        public ActionResult GuestAwaitIndex()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GuestAwaitIndex(PagingInfo pi, UserInfo ui)
        {
            pi.PageIndex = pi.PageIndex == 0 ? 1 : pi.PageIndex;
            pi.PageSize = pi.PageSize == 0 ? 5 : pi.PageSize;

            ui.IdCard = String.IsNullOrWhiteSpace(ui.IdCard) ? "" : ui.IdCard;
            ui.UName = String.IsNullOrWhiteSpace(ui.UName) ? "" : ui.UName;
            ui.Login = String.IsNullOrWhiteSpace(ui.Login) ? "" : ui.Login;

            Expression<Func<UserInfo, bool>> wherelambda = u => (u.Type == (int)UiTypeEnum.Guest && u.Status == (int)UiStatusEnum.Block && u.IdCard != null && u.UName != null && u.Login != null && u.IdCard.Contains(ui.IdCard) && u.UName.Contains(ui.UName) && u.Login.Contains(ui.Login));

            int count = 0;
            var guestlist = userinfoService.GetPageEntities<int>(pi.PageSize, pi.PageIndex, out count, wherelambda, u => u.ID, true) as IEnumerable<UserInfo>;
            pi.TotalItems = count;

            return Json(Tuple.Create(guestlist, pi));
        }

        /// <summary>
        /// 展示乘客详情，注意将一些私密信息加密
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GuestDetails(int id = 0)
        {
            UserInfo userinfo = userinfoService.GetEntities(u => u.ID == id).First() as UserInfo;
            if (userinfo == null)
            {
                return HttpNotFound();
            }
            if (userinfo.IdCard.Length >= 16)
            {
                userinfo.IdCard = userinfo.IdCard.Substring(0, 8) + "*******" + userinfo.IdCard.Substring(12, 3);
            }

            return View(userinfo);
        }



        /// <summary>
        /// 修改密码界面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditPwd(int id = 0)
        {
            UserInfo userinfo = userinfoService.GetEntities(u => u.ID == id).First() as UserInfo;
            if (userinfo == null)
            {
                return HttpNotFound();
            }
            return View(userinfo);
        }


        [HttpPost]
        public ActionResult EditPwd(UserInfo userinfo)
        {
            if (userinfoService.EditPwd(userinfo.ID, userinfo.Pwd))
            {
                return Json("ok");
            }
            return View(userinfo);
        }

        /// <summary>
        /// 校验通过申请
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GuestPass(int id = 0)
        {
            if (userinfoService.GuestPass(id))
            {
                return Json("ok");
            }
            return HttpNotFound();
        }


        [HttpPost]
        public ActionResult UserInfoDelete(int id = 0)
        {
            UserInfo userinfo = userinfoService.GetEntities(u => u.ID == id).FirstOrDefault();
            if (userinfo == null)
            {
                return HttpNotFound();
            }
            if (userinfoService.Delete(userinfo))
            {
                return Json("ok");
            }
            return Json("ok");
        }




        /// <summary>
        /// 窗口账户
        /// </summary>
        /// <returns></returns>
        public ActionResult AdminIndex()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdminIndex(PagingInfo pi, UserInfo ui)
        {
            pi.PageIndex = pi.PageIndex == 0 ? 1 : pi.PageIndex;
            pi.PageSize = pi.PageSize == 0 ? 5 : pi.PageSize;

            ui.IdCard = String.IsNullOrWhiteSpace(ui.IdCard) ? "" : ui.IdCard;
            ui.UName = String.IsNullOrWhiteSpace(ui.UName) ? "" : ui.UName;
            ui.Login = String.IsNullOrWhiteSpace(ui.Login) ? "" : ui.Login;

            Expression<Func<UserInfo, bool>> wherelambda = u => (u.Type == (int)UiTypeEnum.Admin && u.IdCard != null && u.UName != null && u.Login != null && u.IdCard.Contains(ui.IdCard) && u.UName.Contains(ui.UName) && u.Login.Contains(ui.Login));

            int count = 0;
            var adminlist = userinfoService.GetPageEntities<int>(pi.PageSize, pi.PageIndex, out count, wherelambda, u => u.ID, true) as IEnumerable<UserInfo>;
            pi.TotalItems = count;

            return Json(Tuple.Create(adminlist, pi));
        }



        /// <summary>
        /// 添加管理员账户
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateAdmin()
        {
            return View();
        }

        /// <summary>
        /// 添加管理员账户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateAdmin(UserInfo ui)
        {
            if (!userinfoService.LoginExist(ui.Login))
            {
                ui.SignDate = DateTime.Now;
                ui.Pwd = "123456".GetMd5();
                ui.Type = (int)UiTypeEnum.Admin;
                ui.Status = (int)UiStatusEnum.Pass;
                ui.GUID = Guid.NewGuid().ToString();
                if (userinfoService.Add(ui))
                {
                    return Json("ok");
                }
            }
            return Json("err");
        }


        /// <summary>
        /// 账户编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(int id = 0)
        {

            return View(userinfoService.GetEntities(u => u.ID == id).FirstOrDefault());
        }

        [HttpPost]
        public ActionResult Edit(UserInfo ui)
        {
            UserInfo uiedit = userinfoService.GetEntities(u => u.ID == ui.ID).FirstOrDefault();
            uiedit.UName = ui.UName;
            uiedit.Phone = ui.Phone;
            uiedit.Address = ui.Address;
            uiedit.IdCard = ui.IdCard;

            if (userinfoService.Update(uiedit))
            {
                return Json("ok");
            }


            return Json("err");
        }

        /// <summary>
        /// 窗口账户
        /// </summary>
        /// <returns></returns>
        public ActionResult TellerIndex()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TellerIndex(PagingInfo pi, UserInfo ui)
        {
            pi.PageIndex = pi.PageIndex == 0 ? 1 : pi.PageIndex;
            pi.PageSize = pi.PageSize == 0 ? 5 : pi.PageSize;

            ui.IdCard = String.IsNullOrWhiteSpace(ui.IdCard) ? "" : ui.IdCard;
            ui.UName = String.IsNullOrWhiteSpace(ui.UName) ? "" : ui.UName;
            ui.Login = String.IsNullOrWhiteSpace(ui.Login) ? "" : ui.Login;

            Expression<Func<UserInfo, bool>> wherelambda = u => (u.Type == (int)UiTypeEnum.Teller && u.IdCard != null && u.UName != null && u.Login != null && u.IdCard.Contains(ui.IdCard) && u.UName.Contains(ui.UName) && u.Login.Contains(ui.Login));

            int count = 0;
            var tellerlist = userinfoService.GetPageEntities<int>(pi.PageSize, pi.PageIndex, out count, wherelambda, u => u.ID, true) as IEnumerable<UserInfo>;
            pi.TotalItems = count;

            return Json(Tuple.Create(tellerlist, pi));
        }



        /// <summary>
        /// 添加检票账户
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateTeller()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateTeller(UserInfo ui)
        {
            if (!userinfoService.LoginExist(ui.Login))
            {
                ui.SignDate = DateTime.Now;
                ui.Pwd = "123456".GetMd5();
                ui.Type = (int)UiTypeEnum.Teller;
                ui.Status = (int)UiStatusEnum.Pass;
                ui.GUID = Guid.NewGuid().ToString();
                if (userinfoService.Add(ui))
                {
                    return Json("ok");
                }
            }
            return Json("err");
        }

        
        
       
    }
}