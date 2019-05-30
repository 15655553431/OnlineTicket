
 using IDal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DalFactory
{
    public class StaticDalFactory
    {
		public static string assemblyName = System.Configuration.ConfigurationManager.AppSettings["DalAssemblyName"];
		public static IFlightInfoDal GetFlightInfoDal()
        {
            return Assembly.Load(assemblyName).CreateInstance(assemblyName+".FlightInfoDal") as IFlightInfoDal;
        }
	

		public static IPathInfoDal GetPathInfoDal()
        {
            return Assembly.Load(assemblyName).CreateInstance(assemblyName+".PathInfoDal") as IPathInfoDal;
        }
	

		public static ITicketinfoDal GetTicketinfoDal()
        {
            return Assembly.Load(assemblyName).CreateInstance(assemblyName+".TicketinfoDal") as ITicketinfoDal;
        }
	

		public static IUserInfoDal GetUserInfoDal()
        {
            return Assembly.Load(assemblyName).CreateInstance(assemblyName+".UserInfoDal") as IUserInfoDal;
        }
	

	}
}


