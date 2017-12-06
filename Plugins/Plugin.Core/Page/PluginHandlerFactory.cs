using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Plugin.Core.Page
{
  public   class HandlerFactory : IHttpHandlerFactory
    {
        public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            string path = context.Request.PhysicalPath;
            if (Path.GetExtension(path) == ".rss")
            {
                return null;// new RSSHandler();
            }

            if (Path.GetExtension(path) == ".atom")
            {
                return null;//ATOMHandler();
            }
            return null;
        }

        public void ReleaseHandler(IHttpHandler handler)
        {
        }
    }
}
