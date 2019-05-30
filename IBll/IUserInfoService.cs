using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBll
{
    public partial interface IUserInfoService : IBaseService<UserInfo>
    {
        /// <summary>
        /// 判断给定的登陆名是否已经存在，存在返回true，不存在返回false
        /// </summary>
        /// <param name="login">登录名</param>
        /// <returns></returns>
        bool LoginExist(string login);

        /// <summary>
        /// 判断给定的登陆名除了已有的这个ID记录，还是否存在其他的，存在返回true
        /// </summary>
        /// <param name="id">已存在的ID</param>
        /// <param name="login">登陆名</param>
        /// <returns></returns>
        bool LoginExist(int id, string login);

        /// <summary>
        /// 修改密码接口，给一个id和新密码。成功true,密码给明文即可
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="newpwd">明文密码</param>
        /// <returns></returns>
        bool EditPwd(int id, string newpwd);

        /// <summary>
        /// 乘客账户校验通过接口，给一个id，对该账号设置通过
        /// </summary>
        /// <param name="id">账户ID</param>
        /// <returns></returns>
        bool GuestPass(int id);
    }
}
