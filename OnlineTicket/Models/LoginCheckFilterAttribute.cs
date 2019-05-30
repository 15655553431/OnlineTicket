using Common.Cache;
using Model;
using Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineTicket.Models
{
    public class LoginCheckFilterAttribute : ActionFilterAttribute
    {
        //使用拦截器校验用户是否登录

        public bool IsCheck { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (IsCheck)
            {
                //校验用户是否已经登录
                //使用memcached+cookie代替session

                //如果没有记录的id，直接跳转到登陆界面
                if (filterContext.HttpContext.Request.Cookies["userLoginId"] == null)
                {
                    filterContext.HttpContext.Response.Redirect("/UserInfo/Login");
                    return;
                }

                //从缓存里面拿到userLoginId信息
                string userLoginId = filterContext.HttpContext.Request.Cookies["userLoginId"].Value.ToString();

                UserInfo userInfo = CacheHelper.GetCache(userLoginId) as UserInfo;

                if (userInfo == null)
                {
                    //用户长时间不操作，超时了
                    filterContext.HttpContext.Response.Redirect("/UserInfo/Login");
                    return;
                }
                else
                {
                    if (userInfo.Type == (int)UiTypeEnum.Admin)
                    {
                        //滑动窗口机制（就是延长缓存时间,重新延长20分钟）
                        CacheHelper.SetCache(userLoginId, userInfo, DateTime.Now.AddMinutes(20));
                    }
                    else
                    {
                        //用户账户类型不符合要求
                        filterContext.HttpContext.Response.Redirect("/UserInfo/Login");
                        return;
                    }
                }

            }

        }
    }
}