using OnlineGuestTicket.Models;
using System.Web;
using System.Web.Mvc;

namespace OnlineGuestTicket
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //使用拦截器校验用户是否登录
            //用来做用户校验，放到了Models文件夹里
            filters.Add(new LoginCheckFilterAttribute() { IsCheck = true });
        }
    }
}