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
        public bool SetStatus(int id, int status)
        {
            FlightInfo fi = CurrentDal.GetEntities(u => u.ID == id).FirstOrDefault();
            fi.Status = status;
            if (Update(fi))
            {
                return true;
            }
            return false;
        }
    }
}
