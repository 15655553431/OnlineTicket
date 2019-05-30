
 using IBll;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Bll
{


	public partial class FlightInfoService : BaseService<FlightInfo>, IFlightInfoService
    {
        public override void SetCurrentDal()
        {
            CurrentDal = this.DbSession.FlightInfoDal;
        }
    }


	public partial class PathInfoService : BaseService<PathInfo>, IPathInfoService
    {
        public override void SetCurrentDal()
        {
            CurrentDal = this.DbSession.PathInfoDal;
        }
    }


	public partial class TicketinfoService : BaseService<Ticketinfo>, ITicketinfoService
    {
        public override void SetCurrentDal()
        {
            CurrentDal = this.DbSession.TicketinfoDal;
        }
    }


	public partial class UserInfoService : BaseService<UserInfo>, IUserInfoService
    {
        public override void SetCurrentDal()
        {
            CurrentDal = this.DbSession.UserInfoDal;
        }
    }


}