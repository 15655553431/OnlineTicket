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
    public class PathInfoController : Controller
    {
        //
        // GET: /PathInfo/
        private IPathInfoService pathinfoService = new PathInfoService();

        private IFlightInfoService flightinfoService = new FlightInfoService();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(PagingInfo pi, PathInfo path)
        {
            pi.PageIndex = pi.PageIndex == 0 ? 1 : pi.PageIndex;
            pi.PageSize = pi.PageSize == 0 ? 5 : pi.PageSize;

            path.Number = String.IsNullOrWhiteSpace(path.Number) ? "" : path.Number;
            path.Origin = String.IsNullOrWhiteSpace(path.Origin) ? "" : path.Origin;
            path.Destination = String.IsNullOrWhiteSpace(path.Destination) ? "" : path.Destination;

            Expression<Func<PathInfo, bool>> wherelambda = u => (u.Origin.Contains(path.Origin) && u.Destination.Contains(path.Destination) && u.Number.Contains(path.Number));

            int count = 0;
            var pathinfolist = pathinfoService.GetPageEntities<int>(pi.PageSize, pi.PageIndex, out count, wherelambda, u => u.ID, true) as IEnumerable<PathInfo>;
            pi.TotalItems = count;

            return Json(Tuple.Create(pathinfolist, pi));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(PathInfo path)
        {
            path.Status = (int)PathStatusEnum.Stop;
            path.AddDate = DateTime.Now;
            if (pathinfoService.Add(path))
            {
                return Json("ok");
            }
            return Json("err");
        }


        public ActionResult Edit(int id = 0)
        {
            PathInfo path = pathinfoService.GetEntities(u => u.ID == id).FirstOrDefault() as PathInfo;
            if (path == null)
            {
                return HttpNotFound();
            }
            return View(path);
        }

        [HttpPost]
        public ActionResult Edit(PathInfo path)
        {
            PathInfo pathedit = pathinfoService.GetEntities(u => u.ID == path.ID).FirstOrDefault() as PathInfo;

            pathedit.Number = path.Number;
            pathedit.Origin = path.Origin;
            pathedit.Destination = path.Destination;
            pathedit.Distance = path.Distance;
            pathedit.Time = path.Time;
            pathedit.Other = path.Other;
            if (pathinfoService.Update(pathedit))
            {
                return Json("ok");
            }
            return Json("err");
        }

        public ActionResult Details(int id = 0)
        {
            PathInfo path = pathinfoService.GetEntities(u => u.ID == id).FirstOrDefault() as PathInfo;
            if (path == null)
            {
                return HttpNotFound();
            }
            return View(path);
        }

        [HttpPost]
        public ActionResult Delete(int id = 0)
        {
            PathInfo path = pathinfoService.GetEntities(u => u.ID == id).FirstOrDefault() as PathInfo;
            if (path != null)
            {
                //检测该路线下是否有班车信息
                if (flightinfoService.GetEntities(u => u.PathInfoID == path.ID).Count() == 0)
                {
                    if (pathinfoService.Delete(path))
                    {
                        return Json("ok");
                    }
                }
            }
            return Json("err");
        }


        [HttpPost]
        public ActionResult SetStatus(int id = 0, int status = 0)
        {
            if (status == 1 || status == 2)
            {
                if (pathinfoService.SetStatus(id, status))
                {
                    return Json("ok");
                }
                return Json("err");
            }
            return Json("err");
        }

        [HttpPost]
        public ActionResult GetPath(string Search = "")
        {
            List<PathInfo> pathlist = pathinfoService.GetEntities(u => u.Number.Contains(Search) || u.Origin.Contains(Search) || u.Destination.Contains(Search)).Take(8).ToList();
            return Json(pathlist);
        }

    }
}
