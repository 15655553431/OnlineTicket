using IBll;
using Common.Md5;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Enum;

namespace Bll
{
    public partial class UserInfoService : BaseService<UserInfo>, IUserInfoService
    {
        public bool LoginExist(string login)
        {
            if (CurrentDal.GetEntities(u => u.Login == login).Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool LoginExist(int id, string login)
        {
            throw new NotImplementedException();
        }

        public bool EditPwd(int id, string newpwd)
        {
            UserInfo ui = CurrentDal.GetEntities(u => u.ID == id).First();
            if (ui != null)
            {
                ui.Pwd = newpwd.GetMd5();
                if (Update(ui))
                {
                    return true;
                }
                return false;
            }
            return false;
        }


        public bool GuestPass(int id)
        {
            UserInfo ui = CurrentDal.GetEntities(u => u.ID == id).First();
            if (ui != null)
            {
                ui.Status = (int)UiStatusEnum.Pass;
                if (Update(ui))
                {
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}
