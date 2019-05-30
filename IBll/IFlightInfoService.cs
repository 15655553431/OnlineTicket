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
        /// <summary>
        /// 设置班车的状态信息
        /// </summary>
        /// <param name="id">班车id</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        bool SetStatus(int id, int status);
    }
}
