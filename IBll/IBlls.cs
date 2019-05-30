
 using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBll
{

	public partial interface IFlightInfoService : IBaseService<FlightInfo>
    {
    }
	public partial interface IPathInfoService : IBaseService<PathInfo>
    {
    }
	public partial interface ITicketinfoService : IBaseService<Ticketinfo>
    {
    }
	public partial interface IUserInfoService : IBaseService<UserInfo>
    {
    }

}