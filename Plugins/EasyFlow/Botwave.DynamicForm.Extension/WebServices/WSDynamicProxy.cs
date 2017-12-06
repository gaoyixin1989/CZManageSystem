 using System;
 using System.IO;
 using System.Net;
 using System.CodeDom;
 using System.CodeDom.Compiler;
 using System.Web.Services;
 using System.Web.Services.Description;
 using System.Web.Services.Protocols;
 using System.Xml.Serialization;
 using System.Web.Services.Discovery;
 using System.Xml.Schema;
 using System.Text;
 using System.Security.Cryptography;
 using System.Reflection;

namespace Botwave.DynamicForm.Extension.WebServices
{
    /// <summary>
    /// web服务代理类,兼容java发布的webserive
    /// </summary>
    public class WSDynamicProxy
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(WSDynamicProxy));
        #region Variables and Properties

        /// <summary>
        /// web服务名称
        /// </summary>
        private string _wsName = string.Empty;

        /// <summary>
        /// 代理类类型名称
        /// </summary>
        private Type _typeName = null;

        /// <summary>
        /// 程序集名称
        /// </summary>
        private string _assName = string.Empty;

        /// <summary>
        /// web服务地址
        /// </summary>
        private string _wsdlUrl = string.Empty;

        /// <summary>
        /// 代理类所在程序集路径
        /// </summary>
        private string _assPath = string.Empty;

        /// <summary>
        /// 代理类的实例
        /// </summary>
        private object _instance = null;

        /// <summary>
        /// 代理类的实例
        /// </summary>
        private object Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Activator.CreateInstance(_typeName);
                    return _instance;

                }
                else
                    return _instance;
            }
        }
        #endregion

        public WSDynamicProxy(string wsdlUrl, string wsName)
        {
            this._wsdlUrl = wsdlUrl;
            this._wsName = wsName;
            this._assName = string.Format("ZTENetNumenU36.WebServices.{0}", wsName);
            this._assPath = Path.GetTempPath() + this._assName + getMd5Sum(this._wsdlUrl) + ".dll";
            CreateUSSServiceAssembly();
        }

        #region public method
        /// <summary>
        /// 得到WSDL信息，生成本地代理类并编译为DLL
        /// </summary>
        public void CreateUSSServiceAssembly()
        {
            try
            {
                if (this.checkCache())
                {
                    this.initTypeName();
                    return;
                }

                if (string.IsNullOrEmpty(this._wsdlUrl))
                {
                    return;
                }
            }
            catch (Exception exa)
            {
                log.Error(exa.ToString());
            }

            try
            {
                // 使用WebClient下载 WSDL 信息
                WebClient web = new WebClient();
                Stream stream = web.OpenRead(this._wsdlUrl);

                // 创建和格式化WSDL文档
                ServiceDescription description = ServiceDescription.Read(stream);
                // 创建客户端代理代理类
                ServiceDescriptionImporter importer = new ServiceDescriptionImporter();

                importer.ProtocolName = "Soap";
                importer.Style = ServiceDescriptionImportStyle.Client; //生成客户端代理
                importer.CodeGenerationOptions = CodeGenerationOptions.GenerateProperties | CodeGenerationOptions.GenerateNewAsync;
                //添加WSDL文档
                importer.AddServiceDescription(description, null, null);

                // 使用 CodeDom 编译客户端代理类
                CodeNamespace nmspace = new CodeNamespace(_assName); //为代理类添加命名空间
                CodeCompileUnit unit = new CodeCompileUnit();

                unit.Namespaces.Add(nmspace);

                this.checkForImports(this._wsdlUrl, importer);

                ServiceDescriptionImportWarnings warning = importer.Import(nmspace, unit);

                CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");

                CompilerParameters parameter = new CompilerParameters();
                parameter.ReferencedAssemblies.Add("System.dll");
                parameter.ReferencedAssemblies.Add("System.XML.dll");
                parameter.ReferencedAssemblies.Add("System.Web.Services.dll");
                parameter.ReferencedAssemblies.Add("System.Data.dll");
                parameter.GenerateExecutable = false;
                parameter.GenerateInMemory = false;
                parameter.IncludeDebugInformation = false;

                CompilerResults result = provider.CompileAssemblyFromDom(parameter, unit);
                provider.Dispose();
                if (result.Errors.HasErrors)
                {
                    string errors = string.Format(@"Build failed: {0} errors", result.Errors.Count);
                    foreach (CompilerError error in result.Errors)
                    {
                        errors += error.ErrorText;
                    }
                    log.Error(errors);
                    throw new Exception(errors);
                }

                this.copyTempAssembly(result.PathToAssembly);
                this.initTypeName();
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// 执行代理类指定方法，有返回值
        /// </summary>
        /// <param name="methodName">方法名称</param>
        /// <param name="param">参数</param>
        /// <returns>返回值</returns>
        public object ExecuteQuery(string methodName, object[] param)
        {
            try
            {
                object rtnObj = null;
                if (this._typeName != null)
                {
                    MethodInfo method = this._typeName.GetMethod(methodName);
                    rtnObj = method.Invoke(Instance, param);
                }
                else
                {
                    log.Error(string.Format("The Web Service '{0}' is not available!", this._wsName));
                    throw new Exception(string.Format("The Web Service '{0}' is not available!", this._wsName));
                }
                return rtnObj;
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// 执行代理类指定方法，无返回值
        /// </summary>
        /// <param name="methodName">方法名称</param>
        /// <param name="param">参数</param>
        public void ExecuteNoQuery(string methodName, object[] param)
        {
            try
            {
                if (this._typeName != null)
                {
                    MethodInfo method = this._typeName.GetMethod(methodName);
                    method.Invoke(Instance, param);
                }
                else
                {
                    log.Error(string.Format("The Web Service '{0}' is not available!", this._wsName));
                    throw new Exception(string.Format("The Web Service '{0}' is not available!", this._wsName));
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
        }
        #endregion

        #region private method
        /// <summary>
        /// 得到代理类类型名称
        /// </summary>
        private void initTypeName()
        {
            Assembly serviceAsm = Assembly.LoadFrom(this._assPath);
            Type[] types = serviceAsm.GetTypes();
            string objTypeName = "";
            foreach (Type t in types)
            {
                if (t.BaseType == typeof(SoapHttpClientProtocol))
                {
                    objTypeName = t.Name;
                    break;
                }
            }
            _typeName = serviceAsm.GetType(this._assName + "." + objTypeName);
        }


        /// <summary>
        /// 根据web service文档架构向代理类添加ServiceDescription和XmlSchema
        /// </summary>
        /// <param name="baseWSDLUrl">web服务地址</param>
        /// <param name="importer">代理类</param>
        private void checkForImports(string baseWSDLUrl, ServiceDescriptionImporter importer)
        {
            DiscoveryClientProtocol dcp = new DiscoveryClientProtocol();
            dcp.DiscoverAny(baseWSDLUrl);
            dcp.ResolveAll();
            foreach (object osd in dcp.Documents.Values)
            {
                if (osd is ServiceDescription) importer.AddServiceDescription((ServiceDescription)osd, null, null); ;
                if (osd is XmlSchema) importer.Schemas.Add((XmlSchema)osd);
            }
        }

        /// <summary>
        /// 复制程序集到指定路径
        /// </summary>
        /// <param name="pathToAssembly">程序集路径</param>
        private void copyTempAssembly(string pathToAssembly)
        {
            File.Copy(pathToAssembly, this._assPath);
        }

        private string getMd5Sum(string str)
        {
            Encoder enc = System.Text.Encoding.Unicode.GetEncoder();

            byte[] unicodeText = new byte[str.Length * 2];
            enc.GetBytes(str.ToCharArray(), 0, str.Length, unicodeText, 0, true);

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(unicodeText);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                sb.Append(result[i].ToString("X2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 是否已经存在该程序集
        /// </summary>
        /// <returns>false:不存在该程序集,true:已经存在该程序集</returns>
        private bool checkCache()
        {
            if (File.Exists(this._assPath))
            {
                return true;
            }

            return false;
        }


        #endregion
    }
}