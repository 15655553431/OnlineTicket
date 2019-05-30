
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDal
{


	public partial interface IFlightInfoDal:IBaseDal<FlightInfo>
    {
    }



	public partial interface IPathInfoDal:IBaseDal<PathInfo>
    {
    }



	public partial interface ITicketinfoDal:IBaseDal<Ticketinfo>
    {
    }



	public partial interface IUserInfoDal:IBaseDal<UserInfo>
    {
    }



}