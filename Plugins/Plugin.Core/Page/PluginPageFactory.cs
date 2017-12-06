using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Compilation;
using System.Web.Routing;
using System.Web.WebPages.Razor;

namespace Plugin.Core.Page
{
    public class PluginPageFactory
    {
        public PluginPageFactory(string pluginName)
        {
            CodeGeneration(pluginName);
        }
        /// <summary>
        /// 给运行时编译的页面加了引用程序集。
        /// </summary>
        /// <param name="pluginName"></param>
        private void CodeGeneration(string pluginName)
        {
            var plugin = PluginManager.GetPlugin(pluginName);
            if (plugin != null)
            {
                var name = plugin.Assembly.FullName;
                //var assembly= AssemblyName.GetAssemblyName(name);
                var load = Assembly.Load(name);
                  
                 //add the reference to the build manager
                 BuildManager.AddReferencedAssembly(load);





                //var shadowCopyPlugFolder = new DirectoryInfo(AppDomain.CurrentDomain.DynamicDirectory);
                //var shadowCopiedPlug = new FileInfo(Path.Combine(shadowCopyPlugFolder.FullName, plug.Name));
                //File.Copy(plug.FullName, shadowCopiedPlug.FullName, true); //待办事项:异常处理……
                //var shadowCopiedAssembly = Assembly.Load(AssemblyName.GetAssemblyName(shadowCopiedPlug.FullName));

                //添加引用到构建管理
                //BuildManager.AddReferencedAssembly(shadowCopiedAssembly);











                //var s = Assembly.Load(name);
                //var ss = new AssemblyBuilder;
                //new AssemblyBuilder().AddAssemblyReference(plugin.Assembly); //var s =Assembly.LoadFrom(name);
            }
            //BuildProvider.RegisterBuildProvider("plugin",);
            ////BuildProvider.RegisterBuildProvider
            //BuildManager.AddReferencedAssembly(plugin.Assembly);
            //BuildManager.+= (object sender, EventArgs e)
            //RazorBuildProvider.CodeGenerationStarted += (object sender, EventArgs e) =>
            //{
            //    RazorBuildProvider provider = (RazorBuildProvider)sender;

            //    plugin = PluginManager.GetPlugin(pluginName);

            //    if (plugin != null)
            //    {
            //        provider.AssemblyBuilder.AddAssemblyReference(plugin.Assembly);
            //    }
            //};
        }
        protected Type GetPageType(RequestContext requestContext)
        {
            var controllerName = requestContext.HttpContext.Request.Path;
            Type controllerType = this.GetPageType(controllerName);

            //if (controllerType == null)
            //{
            //    controllerType = base.GetPageType(requestContext, controllerName);
            //}

            return controllerType;
        }
        private Type GetPageType(string controllerName)
        {
            foreach (var plugin in PluginManager.GetPlugins())
            {
                var type = plugin.GetControllerType(controllerName + "Controller");

                if (type != null)
                {
                    return type;
                }
            }

            return null;
        }
    }
}
