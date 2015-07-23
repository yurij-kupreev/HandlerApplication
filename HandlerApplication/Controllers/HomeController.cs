using HandlerApplication.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Caching;
using System.Web.Script.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using HandlerApplication.DAL.Security;

namespace HandlerApplication.Controllers
{
    public class HomeController : Controller
    {
        //static List<string> comments = new List<string>();
        //static List<HumanInfo> staticData;
        //static int numPage;
        //static int numPages;
        static PageData pageData = new PageData();

        public ActionResult Index()
        {
            return View();
        }

        [CustomAuthorize(RolesConfigKey = "AdminRoleName")]
        public ActionResult HomeTask()
        {

            return View(pageData);
        }

        public async Task<ActionResult> GetJson(int? pageNum)
        {
            string filePath = @"~/App_Data/data.json";
            string jsonData;
            List<HumanInfo> data;
            if (HttpContext.Cache["JsonData"] == null)
            {
                using (TextReader tr = System.IO.File.OpenText(System.Web.HttpContext.Current.Server.MapPath(filePath)))
                {
                    jsonData = await tr.ReadToEndAsync();
                }

                if (jsonData == null) { return HttpNotFound(); }
                try
                {
                    data = new JavaScriptSerializer().Deserialize<List<HumanInfo>>(jsonData);
                    double number = (double)data.Count / 10;
                    pageData.numOfPages = (int)Math.Ceiling(number);
                }
                catch { return HttpNotFound(); }
                HttpContext.Cache.Insert("JsonData", data, new CacheDependency(Server.MapPath(filePath)));
            }
            data = HttpContext.Cache["JsonData"] as List<HumanInfo>;
            //List<HumanInfo> newData = data.Skip((pageNum.Value - 1) * 10).Take(10).ToList();
            //staticData = data.Skip((pageNum.Value - 1) * 10).Take(10).ToList();
            pageData.humansInfo = data.Skip((pageNum.Value - 1) * 10).Take(10).ToList();
            pageData.numOfPage = pageNum.Value;
            

            if (Request.IsAjaxRequest())
            {
                //staticData.Add(new HumanInfo() { id = data.Count, name = pageNum.ToString() });
                return Json(pageData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                
                //numPage = pageNum.Value;
                //ViewBag.Comments = comments;
                //ViewBag.NumOfPage = numPage;
                //ViewBag.NumOfPages = numPages;
                ViewBag.GracefulDegradation = true;
                return View("HomeTask", pageData);
            }
        }

        //public ActionResult WatchComments()
        //{
        //    return PartialView("Comments", comments);
        //}

        [HttpPost]
        public ActionResult AddComment(string comment)
        {
            if (pageData.comments == null) pageData.comments = new List<string>();
            if (comment != String.Empty)
                pageData.comments.Add(comment);
            //ViewBag.Comments = comments;
            if (Request.IsAjaxRequest())
                //return PartialView("Comments", pageData);
                return Json(comment);
            else
            {
                //ViewBag.NumOfPage = numPage;
                //ViewBag.NumOfPages = numPages;
                ViewBag.GracefulDegradation = true;
                return View("HomeTask", pageData);
            }
        }
    }
}
