namespace Plugin.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Web.Compilation;
    using System.Web.Hosting;

    /// <summary>
    /// 插件加载器。
    /// </summary>
    public static class PluginLoader
    {
        /// <summary>
        /// 插件目录。
        /// </summary>
        private static readonly DirectoryInfo PluginFolder;
        private static readonly string  folder;
        /// <summary>
        /// 插件临时目录。
        /// </summary>
        private static readonly DirectoryInfo TempPluginFolder;

        /// <summary>
        /// 初始化。
        /// </summary>
        static PluginLoader()
        {
            folder = "~/Plugins";
            PluginFolder = new DirectoryInfo(HostingEnvironment.MapPath(folder));
            TempPluginFolder = new DirectoryInfo(HostingEnvironment.MapPath("~/App_Data/Dependencies"));
        }

        /// <summary>
        /// 加载插件。
        /// </summary>
        public static IEnumerable<PluginDescriptor> Load()
        {
            List<PluginDescriptor> plugins = new List<PluginDescriptor>();

            //程序集复制到临时目录。
            FileCopyTo();

            IEnumerable<Assembly> assemblies = null;

            //加载 bin 目录下的所有程序集。
            assemblies = AppDomain.CurrentDomain.GetAssemblies();
            AddReferencedAssembly(assemblies);
            //plugins.AddRange(GetAssemblies(assemblies));

            //加载临时目录下的所有程序集。
            assemblies = TempPluginFolder.GetFiles("*.dll", SearchOption.AllDirectories).Select(x => Assembly.LoadFile(x.FullName));
            AddReferencedAssembly(assemblies);
            //plugins.AddRange(GetAssemblies(assemblies));
            //LoadConfig(folder);
            return plugins;
        }
        private static void AddReferencedAssembly(IEnumerable<Assembly> assemblies)
        {
            foreach (var item in assemblies)
            {
                //add the reference to the build manager
                try
                {
                    BuildManager.AddReferencedAssembly(item);

                }
                catch (Exception ex)
                {

                    var s = ex.ToString();
                }

            }
        }
        //private static void LoadConfig(string folder, string defaultConfigName = "*.config")
        //{
        //    var directory = new DirectoryInfo(HostingEnvironment.MapPath(folder));
        //    var configFiles = directory.GetFiles(defaultConfigName, SearchOption.AllDirectories).ToList();
        //    if (configFiles.Count == 0) return;

        //    foreach (var configFile in configFiles.OrderBy(s => s.Name))
        //    {
        //        ModuleConfigContainer.Register(new ModuleConfiguration(configFile.FullName));
        //    }
        //}
        /// <summary>
        /// 获得插件信息。
        /// </summary>
        /// <param name="pluginType"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        private static PluginDescriptor GetPluginInstance(Type pluginType, Assembly assembly)
        {
            if (pluginType != null)
            {
                var plugin = (IPlugins)Activator.CreateInstance(pluginType);

                if (plugin != null)
                {
                    return new PluginDescriptor(plugin, assembly, assembly.GetTypes());
                }
            }

            return null;
        }

        /// <summary>
        /// 程序集复制到临时目录。
        /// </summary>
        private static void FileCopyTo()
        {
            Directory.CreateDirectory(PluginFolder.FullName);
            Directory.CreateDirectory(TempPluginFolder.FullName);

            //清理临时文件。
            foreach (var file in TempPluginFolder.GetFiles("*.dll", SearchOption.AllDirectories))
            {
                try
                {
                    file.Delete();
                }
                catch (Exception)
                {

                }

            }

            //复制插件进临时文件夹。
            foreach (var plugin in PluginFolder.GetFiles("*.dll", SearchOption.AllDirectories))
            {
                try
                {
                    var di = Directory.CreateDirectory(TempPluginFolder.FullName);
                    File.Copy(plugin.FullName, Path.Combine(di.FullName, plugin.Name), true);
                }
                catch (Exception)
                {

                }
            }
        }

        /// <summary>
        /// 根据程序集列表获得该列表下的所有插件信息。
        /// </summary>
        /// <param name="assemblies">程序集列表</param>
        /// <returns>插件信息集合。</returns>
        private static IEnumerable<PluginDescriptor> GetAssemblies(IEnumerable<Assembly> assemblies)
        {
            IList<PluginDescriptor> plugins = new List<PluginDescriptor>();

            int i = 0;

            foreach (var assembly in assemblies)
            {
                try
                {
                    var assemblys = assembly.GetTypes();
                    //InterfaceMapping map = assembly.GetType().GetInterfaceMap(typeof(IPlugin));
                    var pluginTypes = assemblys.Where(type => type.GetInterface(typeof(IPlugins).Name) != null && type.IsClass && !type.IsAbstract);


                    foreach (var pluginType in pluginTypes)
                    {
                        var plugin = GetPluginInstance(pluginType, assembly);

                        if (plugin != null)
                        {
                            plugins.Add(plugin);
                        }
                    }
                }
                catch (Exception ex)
                {

                    var s = ex.ToString();
                }
            }

            return plugins;
        }
        public static void LoadAssemblies()
        {
            var directory = PluginFolder;// TempPluginFolder;//  new DirectoryInfo(HostingEnvironment.MapPath(folder));
            var binFiles = directory.GetFiles("*.dll", SearchOption.AllDirectories).ToList();
            if (binFiles.Count == 0)
                return;

            foreach (var plug in binFiles)
            {
                //运行在完全信任
                //************
                //if (GetCurrentTrustLevel() != AspNetHostingPermissionLevel.Unrestricted)
                //在网络设置。配置,探索插件\ temp和复制所有文件夹
                //************************
                var shadowCopyPlugFolder = new DirectoryInfo(AppDomain.CurrentDomain.DynamicDirectory);
                var shadowCopiedPlug = new FileInfo(Path.Combine(shadowCopyPlugFolder.FullName, plug.Name));
                File.Copy(plug.FullName, shadowCopiedPlug.FullName, true); //待办事项:异常处理……
                var shadowCopiedAssembly = Assembly.Load(AssemblyName.GetAssemblyName(shadowCopiedPlug.FullName));

                //添加引用到构建管理
                BuildManager.AddReferencedAssembly(shadowCopiedAssembly);
            }
        }
    }
}