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
    public class FlightInfoController : Controller
    {
        //
        // GET: /FlightInfo/

        private IFlightInfoService flightinfoService = new FlightInfoService();


        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(PagingInfo pi, FlightInfo fi, string Origin = "", string Destination="")
        {
            pi.PageIndex = pi.PageIndex == 0 ? 1 : pi.PageIndex;
            pi.PageSize = pi.PageSize == 0 ? 5 : pi.PageSize;

            fi.Number = String.IsNullOrWhiteSpace(fi.Number) ? "" : fi.Number;


            Expression<Func<FlightInfo, bool>> wherelambda = u => (u.Type == (int)FiTypeEnum.Daily && u.PathInfo.Origin.Contains(Origin) && u.PathInfo.Destination.Contains(Destination) && u.Number.Contains(fi.Number));

            int count = 0;
            var flightinfolist = flightinfoService.GetPageEntities<int>(pi.PageSize, pi.PageIndex, out count, wherelambda, u => u.ID, true) as IEnumerable<FlightInfo>;
            pi.TotalItems = count;

            return Json(Tuple.Create(flightinfolist, pi));
        }


        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(FlightInfo fi)
        {
            fi.Status = (int)FiStatusEnum.Stop;
            fi.Type = (int)FiTypeEnum.Daily;
            fi.GUID = Guid.NewGuid().ToString();
            fi.GoTime = Convert.ToDateTime("2000-01-01").Date + fi.GoTime.TimeOfDay;
            fi.OneDate = Convert.ToDateTime("1999-01-01");
            if (flightinfoService.Add(fi))
            {
                return Json("ok");
            }
            return Json("err");
        }


        public ActionResult Details(int id = 0)
        {
            return View(flightinfoService.GetEntities(u=>u.ID==id).FirstOrDefault());
        }


        public ActionResult Edit(int id = 0)
        {
            return View(flightinfoService.GetEntities(u => u.ID == id).FirstOrDefault());
        }

        [HttpPost]
        public ActionResult Edit(FlightInfo fi)
        {
            FlightInfo fiedit = flightinfoService.GetEntities(u => u.ID == fi.ID).FirstOrDefault();
            fiedit.Number = fi.Number;
            fiedit.Driver = fi.Driver;
            fiedit.BusType = fi.BusType;
            fiedit.GoTime = Convert.ToDateTime("2000-01-01").Date + fi.GoTime.TimeOfDay; ;
            fiedit.LicenceTag = fi.LicenceTag;
            fiedit.OverTime = fi.OverTime;
            fiedit.Passengers = fi.Passengers;
            fiedit.PathInfoID = fi.PathInfoID;
            fiedit.Price = fi.Price;

            if (flightinfoService.Update(fiedit))
            {
                return Json("ok");
            }
            return Json("err");
        }


        [HttpPost]
        public ActionResult Delete(int id = 0)
        {
            FlightInfo fidel = flightinfoService.GetEntities(u => u.ID == id).FirstOrDefault();
            if (flightinfoService.Delete(fidel))
            {
                return Json("ok");
            }
            return Json("err");
        }

        [HttpPost]
        public ActionResult SetStatus(int id = 0, int status = 0)
        {
            if (status == 1 || status == 2)
            {
                if (flightinfoService.SetStatus(id, status))
                {
                    return Json("ok");
                }
                return Json("err");
            }
            return Json("err");
        }


        /// <summary>
        /// 临时加班车
        /// </summary>
        /// <returns></returns>
        public ActionResult TempFlightIndex()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TempFlightIndex(PagingInfo pi, FlightInfo fi, string Origin = "", string Destination = "")
        {
            pi.PageIndex = pi.PageIndex == 0 ? 1 : pi.PageIndex;
            pi.PageSize = pi.PageSize == 0 ? 5 : pi.PageSize;

            fi.Number = String.IsNullOrWhiteSpace(fi.Number) ? "" : fi.Number;

            DateTime dt = DateTime.Now;
            Expression<Func<FlightInfo, bool>> wherelambda = u => (u.Type == (int)FiTypeEnum.Temp && u.GoTime >= dt && u.PathInfo.Origin.Contains(Origin) && u.PathInfo.Destination.Contains(Destination) && u.Number.Contains(fi.Number));

            int count = 0;
            var flightinfolist = flightinfoService.GetPageEntities<int>(pi.PageSize, pi.PageIndex, out count, wherelambda, u => u.ID, true) as IEnumerable<FlightInfo>;
            pi.TotalItems = count;

            return Json(Tuple.Create(flightinfolist, pi));
        }

        /// <summary>
        /// 新增加班车页面
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateFlight()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateFlight(FlightInfo fi)
        {
            fi.Status = (int)FiStatusEnum.Run;
            fi.Type = (int)FiTypeEnum.Temp;
            fi.GUID = Guid.NewGuid().ToString();
            fi.OneDate = fi.GoTime;
            if (flightinfoService.Add(fi))
            {
                return Json("ok");
            }
            return Json("err");
        }


        public ActionResult TempFlightDetails(int id = 0)
        {
            return View(flightinfoService.GetEntities(u => u.ID == id).FirstOrDefault());
        }


        public ActionResult TempFlightEdit(int id = 0)
        {
            return View(flightinfoService.GetEntities(u => u.ID == id).FirstOrDefault());
        }

        [HttpPost]
        public ActionResult TempFlightEdit(FlightInfo fi)
        {
            FlightInfo fiedit = flightinfoService.GetEntities(u => u.ID == fi.ID).FirstOrDefault();
            fiedit.Number = fi.Number;
            fiedit.Driver = fi.Driver;
            fiedit.BusType = fi.BusType;
            fi.OneDate = fi.GoTime;
            fiedit.GoTime = Convert.ToDateTime("2000-01-01").Date + fi.GoTime.TimeOfDay; ;
            fiedit.LicenceTag = fi.LicenceTag;
            fiedit.OverTime = fi.OverTime;
            fiedit.Passengers = fi.Passengers;
            fiedit.PathInfoID = fi.PathInfoID;
            fiedit.Price = fi.Price;

            if (flightinfoService.Update(fiedit))
            {
                return Json("ok");
            }
            return Json("err");
        }

        /// <summary>
        /// 历史加班车
        /// </summary>
        /// <returns></returns>
        public ActionResult HistoryFlightIndex()
        {
            return View();
        }

        [HttpPost]
        public ActionResult HistoryFlightIndex(PagingInfo pi, FlightInfo fi, string Origin = "", string Destination = "")
        {
            pi.PageIndex = pi.PageIndex == 0 ? 1 : pi.PageIndex;
            pi.PageSize = pi.PageSize == 0 ? 5 : pi.PageSize;

            fi.Number = String.IsNullOrWhiteSpace(fi.Number) ? "" : fi.Number;

            DateTime dt = DateTime.Now;
            Expression<Func<FlightInfo, bool>> wherelambda = u => (u.Type == (int)FiTypeEnum.Temp && u.GoTime < dt && u.PathInfo.Origin.Contains(Origin) && u.PathInfo.Destination.Contains(Destination) && u.Number.Contains(fi.Number));

            int count = 0;
            var flightinfolist = flightinfoService.GetPageEntities<int>(pi.PageSize, pi.PageIndex, out count, wherelambda, u => u.ID, true) as IEnumerable<FlightInfo>;
            pi.TotalItems = count;

            return Json(Tuple.Create(flightinfolist, pi));
        }

    }
}
