using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace HandlerApplication.Handlers
{
    public class CustomHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            try
            {
                var json = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/data.json"));
                context.Response.Write(json);
            }
            catch
            {
                context.Response.Write("No data.");
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}