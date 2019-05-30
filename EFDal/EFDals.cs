
 using IDal;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDal
{

	public partial class FlightInfoDal:BaseDal<FlightInfo>,IFlightInfoDal
    {
       
    }

	public partial class PathInfoDal:BaseDal<PathInfo>,IPathInfoDal
    {
       
    }

	public partial class TicketinfoDal:BaseDal<Ticketinfo>,ITicketinfoDal
    {
       
    }

	public partial class UserInfoDal:BaseDal<UserInfo>,IUserInfoDal
    {
       
    }


}