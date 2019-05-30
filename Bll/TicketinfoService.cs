using IBll;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll
{
    public partial class TicketinfoService : BaseService<Ticketinfo>, ITicketinfoService
    {

        public bool AnewSellTicket(Ticketinfo AnewTi, Ticketinfo NewTi)
        {
            CurrentDal.Update(AnewTi);
            CurrentDal.Add(NewTi);
            return DbSession.SaveChanges() > 0;
        }


        public bool ChangeSellTicket(Ticketinfo newti, Ticketinfo tempti, Ticketinfo oldti)
        {
            CurrentDal.Add(newti);
            CurrentDal.Update(tempti);
            CurrentDal.Update(oldti);
            return DbSession.SaveChanges() > 0;
        }
    }
}
