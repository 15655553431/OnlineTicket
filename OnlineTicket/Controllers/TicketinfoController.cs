using Bll;
using IBll;
using Model;
using Model.Enum;
using Model.other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace OnlineTicket.Controllers
{
    public class TicketinfoController : Controller
    {
        //
        // GET: /Ticketinfo/

        private ITicketinfoService ticketinfoService = new TicketinfoService();

        private IFlightInfoService flightinfoService = new FlightInfoService();

        private IPathInfoService pathinfoService = new PathInfoService();

        public ActionResult Index()
        {
            return View();
        }



        /// <summary>
        /// 窗口售票系统 显示余票信息，展示余票的界面
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowTicket()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ShowTicket(PagingInfo pi, DateTime dtstart = default(DateTime), string Origin = "", string Destination = "", string Number = "")
        {
            //只给查询30分钟后的车次信息，
            if (dtstart > DateTime.Now.AddMinutes(30))
            {
                pi.PageIndex = pi.PageIndex == 0 ? 1 : pi.PageIndex;
                pi.PageSize = pi.PageSize == 0 ? 5 : pi.PageSize;

                DateTime seacherdt = Convert.ToDateTime("2000-01-01").Date + dtstart.TimeOfDay;
                DateTime maxdt = dtstart.Date.AddDays(1);

                //根据条件查询，还要求线路正常，班车正常，这里注意时间筛选不能漏了加班车
                Expression<Func<FlightInfo, bool>> wherelambda = u => u.PathInfo.Status == (int)PathStatusEnum.Run && u.Status == (int)FiStatusEnum.Run && ((u.GoTime >= seacherdt && u.Type == (int)FiTypeEnum.Daily) || (u.OneDate >= dtstart && u.OneDate < maxdt && u.Type == (int)FiTypeEnum.Temp)) && u.PathInfo.Origin.Contains(Origin) && u.PathInfo.Destination.Contains(Destination) && u.Number.Contains(Number);

                int count = 0;
                List<FlightInfo> filist = flightinfoService.GetPageEntities<DateTime>(pi.PageSize, pi.PageIndex, out count, wherelambda, u => u.GoTime, true).ToList();
                pi.TotalItems = count;


                List<FlightInfo> firesultlist = new List<FlightInfo>();
                //查询余票信息，存入 Passengers中，将发车日期存入GoTime中
                foreach (FlightInfo fitemp in filist)
                {
                    DateTime newgotime = dtstart.Date + fitemp.GoTime.TimeOfDay;
                    Expression<Func<Ticketinfo, bool>> templambda = u => u.FGUID == fitemp.GUID && u.Gotime ==newgotime && u.Status == (int)TiStatusEnum.Done;// u.Gotime > dtstart && u.Gotime < maxdt

                    //已经销售的票数
                    int sale = ticketinfoService.GetEntities(templambda).Count();

                    fitemp.GoTime = newgotime;
                    fitemp.Passengers = fitemp.Passengers - sale;

                    firesultlist.Add(fitemp);
                }

                return Json(Tuple.Create(firesultlist, pi));
            }
            return Json(Tuple.Create(new List<FlightInfo>(), pi));
        }



        public ActionResult SellTicket(int id = 0, DateTime dt = default(DateTime))
        {
            FlightInfo fi = flightinfoService.GetEntities(u => u.ID == id).FirstOrDefault();
            if (fi != null)
            {
                fi.GoTime = dt;
                return View(fi);
            }
            return HttpNotFound();
        }

        /// <summary>
        /// 售票请求，当用户点击购买时
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SellTicket(Ticketinfo ti)
        {
            List<string> rmsg = new List<string>();
            rmsg.Add("err");

            //购票信息提交过来，首先要检查该车是否有退票座位，如有退票座位，优先售卖退票座位
            //检查车次信息，是否路线停运，班车停运
            //1、检查是否有退票座位
            //2、生成订单，
            //3、检查付款情况，如果已经付款，生成车票
            FlightInfo fi = flightinfoService.GetEntities(u => u.GUID == ti.FGUID).FirstOrDefault();
            if (fi == null||fi.PathInfo.Status==(int)PathStatusEnum.Stop||fi.Status==(int)FiStatusEnum.Stop)
            {
                rmsg.Add("路线或班车出错，无法订票！");
                return Json(rmsg);
            }

            ti.GUID = Guid.NewGuid().ToString();
            ti.Origin = fi.PathInfo.Origin;
            ti.Destination = fi.PathInfo.Destination;
            ti.Money = fi.Price;
            ti.Number = fi.Number;
            ti.Arrtime = ti.Gotime.AddMinutes(fi.OverTime);
            ti.Overtime = fi.OverTime;
            ti.Phone = "10000000000";
            ti.BuyDate = DateTime.Now;
            ti.OutDate = DateTime.Now;//退票日期，默认和买票日期相同
            ti.BuyName = "车站窗口售票";
            ti.BuyIdCard = "000000000000000000";
            ti.BuyPhone = "400-1234-123";
            ti.BuyType = (int)TiBuyTypeEnum.Station;
            ti.LicenceTag = fi.LicenceTag;
            ti.Driver = fi.Driver;
            ti.BusType = fi.BusType;
            ti.Passengers = fi.Passengers;
            ti.Status = (int)TiStatusEnum.Done;


            //ti.SeatNumber = 1;//座位号要注意

            //1、检查是否有退票座位
            List<Ticketinfo> ticketlist = ticketinfoService.GetEntities(u => u.FGUID == fi.GUID && u.Status == (int)TiStatusEnum.Quit && u.Gotime == ti.Gotime).ToList();

            //有退票，使用退票的座位号,优先使用最小的座位号
            //购票和将退票状态改为已经售出，需要同时完成，这是一个事务操作
            if (ticketlist.Count > 0)
            {
                Ticketinfo tempti = ticketlist.OrderBy(u => u.SeatNumber).FirstOrDefault();
                //更改这条退票信息的状态，同时加入新的售票信息，事务操作
                ti.SeatNumber = tempti.SeatNumber;//座位号要注意
                tempti.Status = (int)TiStatusEnum.Over;

                if (ticketinfoService.AnewSellTicket(tempti, ti))
                {
                    return Json(ti);
                }
                else
                {
                    rmsg.Add("出票失败！请稍后重试！");
                    return Json(rmsg);
                }

            }
            else
            {
                //2、没有退票，取当前已经售出的车票数量，再加1
                int tempSeat = ticketinfoService.GetEntities(u => u.FGUID == fi.GUID && (u.Status == (int)TiStatusEnum.Done || u.Status == (int)TiStatusEnum.NoPay) && u.Gotime == ti.Gotime).Count();
                if (tempSeat >= fi.Passengers)
                {
                    rmsg.Add("十分抱歉！座位已经全部售完！");
                    return Json(rmsg);
                }
                ti.SeatNumber = tempSeat + 1;

                if (ticketinfoService.Add(ti))
                {
                    return Json(ti);
                }
                else
                {
                    rmsg.Add("出票失败！请稍后重试！");
                    return Json(rmsg);
                }

            }

        }

        public ActionResult TicketDetails(int id = 0)
        {
            Ticketinfo ti = ticketinfoService.GetEntities(u => u.ID == id).FirstOrDefault();

            if (ti != null)
            {
                return View(ti);
            }
            return HttpNotFound();
        }

        /// <summary>
        /// 查看已经售票了，但是还没有发车的车票（30分钟后发车的，可以进行退票）
        /// </summary>
        /// <returns></returns>
        public ActionResult DoneIndex()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DoneIndex(PagingInfo pi,Ticketinfo ti)
        {
            pi.PageIndex = pi.PageIndex == 0 ? 1 : pi.PageIndex;
            pi.PageSize = pi.PageSize == 0 ? 5 : pi.PageSize;

            ti.Number = String.IsNullOrWhiteSpace(ti.Number) ? "" : ti.Number;
            ti.Origin = String.IsNullOrWhiteSpace(ti.Origin) ? "" : ti.Origin;
            ti.Destination = String.IsNullOrWhiteSpace(ti.Destination) ? "" : ti.Destination;
            ti.Name = String.IsNullOrWhiteSpace(ti.Name) ? "" : ti.Name;
            ti.IdCard = String.IsNullOrWhiteSpace(ti.IdCard) ? "" : ti.IdCard;

            DateTime lastdt = DateTime.Now.AddMinutes(30);
            Expression<Func<Ticketinfo, bool>> wherelambda = u => (u.Gotime >= lastdt && u.Status == (int)TiStatusEnum.Done && u.Origin.Contains(ti.Origin) && u.Destination.Contains(ti.Destination) && u.Number.Contains(ti.Number) && u.Name.Contains(ti.Name) && u.IdCard.Contains(ti.IdCard));

            int count = 0;
            var tilist = ticketinfoService.GetPageEntities<int>(pi.PageSize, pi.PageIndex, out count, wherelambda, u => u.ID, true) as IEnumerable<Ticketinfo>;
            pi.TotalItems = count;

            return Json(Tuple.Create(tilist, pi));
        }



        /// <summary>
        /// 退票申请,将状态改为退票
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult QuitTicket(string guid="")
        {
            Ticketinfo ti = ticketinfoService.GetEntities(u=>u.GUID==guid).FirstOrDefault();
            if (ti != null)
            {
                if (ti.Status != (int)TiStatusEnum.Done)
                {
                    return Json("yet");
                }
                ti.Status = (int)TiStatusEnum.Quit;
                ti.OutDate = DateTime.Now;
                if (ticketinfoService.Update(ti))
                {
                    return Json("ok");
                }
            }
            return Json("err");
        }



        /// <summary>
        /// 展示所有车票
        /// </summary>
        /// <returns></returns>
        public ActionResult HistroyTicket()
        {
            return View();
        }

        [HttpPost]
        public ActionResult HistroyTicket(PagingInfo pi, Ticketinfo ti)
        {
            pi.PageIndex = pi.PageIndex == 0 ? 1 : pi.PageIndex;
            pi.PageSize = pi.PageSize == 0 ? 5 : pi.PageSize;

            ti.Number = String.IsNullOrWhiteSpace(ti.Number) ? "" : ti.Number;
            ti.Origin = String.IsNullOrWhiteSpace(ti.Origin) ? "" : ti.Origin;
            ti.Destination = String.IsNullOrWhiteSpace(ti.Destination) ? "" : ti.Destination;
            ti.Name = String.IsNullOrWhiteSpace(ti.Name) ? "" : ti.Name;
            ti.IdCard = String.IsNullOrWhiteSpace(ti.IdCard) ? "" : ti.IdCard;

       
            Expression<Func<Ticketinfo, bool>> wherelambda = u => (u.Origin.Contains(ti.Origin) && u.Destination.Contains(ti.Destination) && u.Number.Contains(ti.Number) && u.Name.Contains(ti.Name) && u.IdCard.Contains(ti.IdCard));

            int count = 0;
            var tilist = ticketinfoService.GetPageEntities<int>(pi.PageSize, pi.PageIndex, out count, wherelambda, u => u.ID, true) as IEnumerable<Ticketinfo>;
            pi.TotalItems = count;

            return Json(Tuple.Create(tilist, pi));
        }


        /// <summary>
        /// 改签车票
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangeTicket(string guid = "")
        {
            return View(ticketinfoService.GetEntities(u => u.GUID == guid && u.Status == (int)TiStatusEnum.Done).FirstOrDefault());
        }

        [HttpPost]
        public ActionResult ChangeTicket(DateTime changedate = default(DateTime), string guid = "", string changeflight = "")
        {
            //当前车票
            Ticketinfo oldti = ticketinfoService.GetEntities(u => u.GUID == guid && u.Status == (int)TiStatusEnum.Done).FirstOrDefault();

            //申请改签的班次
            FlightInfo fi = flightinfoService.GetEntities(u => u.GUID ==changeflight).FirstOrDefault();

            //申请改签的时间
            DateTime godt = changedate.Date + fi.GoTime.TimeOfDay;

            if (godt <= DateTime.Now.AddMinutes(10))
            {
                return Json("无法改签十分钟以内班车！");
            }
            //if (godt >= oldti.Gotime)
            //{
            //    return Json("改签车票不可延迟！");
            //}

            if (oldti == null)
            {
                return Json("该车票已经失效,无法改签！");
            }
            if (fi == null || fi.PathInfo.Status == (int)PathStatusEnum.Stop || fi.Status == (int)FiStatusEnum.Stop)
            {
                return Json("路线或班车出错，无法订票！");
            }
            //改签操作  事务操作
            //1、原车票变为退票状态
            //2、购买新的车票(优先购买退票座位)

            Ticketinfo newti = new Ticketinfo();
            newti.Name = oldti.Name;
            newti.IdCard = oldti.IdCard;
            newti.FGUID = fi.GUID;
            newti.GUID = Guid.NewGuid().ToString();
            newti.Gotime = godt;
            newti.Origin = fi.PathInfo.Origin;
            newti.Destination = fi.PathInfo.Destination;
            newti.Money = fi.Price;
            newti.Number = fi.Number;
            newti.Arrtime = godt.AddMinutes(fi.OverTime);
            newti.Overtime = fi.OverTime;
            newti.Phone = "10000000000";
            newti.BuyDate = DateTime.Now;
            newti.OutDate = DateTime.Now;//退票日期，默认和买票日期相同
            newti.BuyName = "车站窗口售票";
            newti.BuyIdCard = "000000000000000000";
            newti.BuyPhone = "400-1234-123";
            newti.BuyType = (int)TiBuyTypeEnum.Station;
            newti.LicenceTag = fi.LicenceTag;
            newti.Driver = fi.Driver;
            newti.BusType = fi.BusType;
            newti.Passengers = fi.Passengers;
            newti.Status = (int)TiStatusEnum.Done;

            //1、检查申请改签的班次是否有退票座位
            List<Ticketinfo> ticketlist = ticketinfoService.GetEntities(u => u.FGUID == fi.GUID && u.Status == (int)TiStatusEnum.Quit && u.Gotime == newti.Gotime).ToList();
            if (ticketlist.Count() > 0)
            {
                //申请改签的班次，有退票的座位
                //事务操作
                //1、插入，新座位，座位号和退票的座位号相同
                //2、更新，将这个退票的车票状态改为座位已经售出
                //3、更新，将老车票状态改为退票

                Ticketinfo tempti = ticketlist.FirstOrDefault();
                tempti.Status = (int)TiStatusEnum.Over;

                newti.SeatNumber = tempti.SeatNumber;

                oldti.Status = (int)TiStatusEnum.Quit;
                oldti.OutDate = DateTime.Now;

                if (ticketinfoService.ChangeSellTicket(newti, tempti, oldti))
                {
                    return Json("ok");
                }

            }
            else
            {
                //2、没有退票，取当前已经售出的车票数量，再加1
                int tempSeat = ticketinfoService.GetEntities(u => u.FGUID == fi.GUID && (u.Status == (int)TiStatusEnum.Done || u.Status == (int)TiStatusEnum.NoPay) && u.Gotime == newti.Gotime).Count();
                if (tempSeat >= fi.Passengers)
                {
                    return Json("当前班车车票已经售完，改签失败！");
                }

                //事务操作
                //1、插入，插入新车票
                //2、更新，更新老车票信息
                newti.SeatNumber = tempSeat + 1;

                oldti.Status = (int)TiStatusEnum.Quit;
                oldti.OutDate = DateTime.Now;

                if (ticketinfoService.AnewSellTicket(oldti, newti))
                {
                    return Json("ok");
                }
                
            }


            return Json("改签失败！请稍后重试");
        }

        /// <summary>
        /// 改签车票，获取某日可改签班次信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangeGetFilght(DateTime changedate = default(DateTime), string guid = "")
        {
            List<string> result = new List<string>();
            if (changedate >= DateTime.Now.Date)
            {
                Ticketinfo ti = ticketinfoService.GetEntities(u => u.GUID == guid).FirstOrDefault();
                //如果查询出的车票为空，或者该车票状态已经不是购买完成状态，
                if (ti == null || ti.Status != (int)TiStatusEnum.Done)
                {
                    result.Add("该车票已经失效！,");
                    return Json(result);
                }

                ////如果发车日期小于改签日期，驳回
                //if (ti.Gotime.Date < changedate.Date)
                //{
                //    result.Add("改签日期不可延后！,");
                //    return Json(result);
                //}


                //查询当前车票所属班车
                FlightInfo fi = flightinfoService.GetEntities(u => u.GUID == ti.FGUID).FirstOrDefault();


                DateTime maxdt = changedate.Date.AddDays(1);
                //条件查询所有当前车票的班车
                //根据条件查询，还要求线路正常，班车正常，这里注意时间筛选不能漏了加班车
                Expression<Func<FlightInfo, bool>> wherelambda = u => u.PathInfoID == fi.PathInfoID && u.PathInfo.Status == (int)PathStatusEnum.Run && u.Status == (int)FiStatusEnum.Run && ((u.Type == (int)FiTypeEnum.Daily) || (u.OneDate >= changedate && u.OneDate < maxdt && u.Type == (int)FiTypeEnum.Temp));

                List<FlightInfo> filist = flightinfoService.GetEntities(wherelambda).OrderBy(u=>u.GoTime).ToList();


                foreach (FlightInfo fitemp in filist)
                {
                    DateTime gotime = changedate.Date + fitemp.GoTime.TimeOfDay;
                    Expression<Func<Ticketinfo, bool>> templambda = u => u.FGUID == fitemp.GUID && u.Gotime==gotime && u.Status == (int)TiStatusEnum.Done;
                    //已经销售的票数
                    int sale = ticketinfoService.GetEntities(templambda).Count();
                    int Passengers = fitemp.Passengers - sale;

                    result.Add(fitemp.GoTime.ToString("HH时mm分") + "(余票" + Passengers.ToString() + "张)," + fitemp.GUID);
                }
                return Json(result);


            }
            result.Add("改签日期不得早于当前日期！,");
            return Json(result);
        }
        

    }
}
