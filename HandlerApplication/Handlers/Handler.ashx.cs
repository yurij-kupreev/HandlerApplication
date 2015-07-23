using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HandlerApplication.Handlers
{
    /// <summary>
    /// Summary description for Handler
    /// </summary>
    public class Handler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            try
            {
                var json = System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/data.json"));
                context.Response.Write(json);
            }
            catch
            {
                context.Response.Write("No data.");
            }

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

    }
}
