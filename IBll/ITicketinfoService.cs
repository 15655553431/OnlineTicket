using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBll
{
    public partial interface ITicketinfoService : IBaseService<Ticketinfo>
    {
        /// <summary>
        /// 更新一张车票信息，同时插入一张车票信息，事务操作
        /// </summary>
        /// <param name="AnewTi">需要更新的车票信息</param>
        /// <param name="NewTi">需要插入的车票信息</param>
        /// <returns></returns>
        bool AnewSellTicket(Ticketinfo AnewTi, Ticketinfo NewTi);

        /// <summary>
        /// 插入一张车票，同时更新两张车票信息
        /// </summary>
        /// <param name="newti">需要插入的车票</param>
        /// <param name="tempti">更新的车票</param>
        /// <param name="oldti">更新的车票</param>
        /// <returns></returns>
        bool ChangeSellTicket(Ticketinfo newti,Ticketinfo tempti, Ticketinfo oldti);
    }
}
