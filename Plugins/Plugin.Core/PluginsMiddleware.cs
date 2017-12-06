using Microsoft.Owin;
using System.Threading.Tasks;
using System;

namespace Plugin.Core
{
    public class PluginsMiddleware : OwinMiddleware
    {
        public PluginsMiddleware(OwinMiddleware next) : base(next)
        {
            //构造函数
        }
        public override Task Invoke(IOwinContext context)
        {
            //中间件的实现代码
            PathString tickPath = new PathString("/plugins");
            //PathString endPath = new PathString(".aspx");
            var s = context.Request.Path.Value ; 
            var path=context.Request.Path;
            //判断Request路径为/tick开头
            if (!path.StartsWithSegments(tickPath)&& path.Value .Contains(".aspx"))
            {
                string content = DateTime.Now.Ticks.ToString();
                //输出答案--当前的Tick数字
                context.Response.ContentType = "text/plain";
                context.Response.ContentLength = content.Length;
                context.Response.StatusCode = 200;
                context.Response.Expires = DateTimeOffset.Now;
                context.Response.Write(content); 
                
            }
            return Next.Invoke(context);
        }
    }
}
