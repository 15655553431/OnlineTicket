
 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDal
{
    public interface IDbSession
    {
    	IFlightInfoDal FlightInfoDal { get;}
    
    	IPathInfoDal PathInfoDal { get;}
    
    	ITicketinfoDal TicketinfoDal { get;}
    
    	IUserInfoDal UserInfoDal { get;}
    
	    int SaveChanges();
    }
}