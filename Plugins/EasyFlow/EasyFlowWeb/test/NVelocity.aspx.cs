using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using NVelocity;
using NVelocity.App;

public partial class test_NVelocity : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    private void Evalte()
    {
        string template = @"
#if($u.DpID.StartsWith(""32976813181322""))
TRUE
#else
FALSE
#end";
        string dpID = "329768131813226335";
        Users u = new Users();
        u.DpID = dpID;

        VelocityEngine engine = Botwave.Workflow.IBatisNet.VelocityEngineFactory.GetVelocityEngine();
        VelocityContext vc = new VelocityContext();
        vc.Put("u", u);
        StringWriter sw = new StringWriter();

        try
        {
            engine.Evaluate(vc, sw, "rules tag", template);
        }
        catch (NVelocity.Exception.MethodInvocationException ex)
        {
            Response.Write(ex.ToString());
        }
        Response.Write(sw.ToString());
    }

    public class Users
    {
        private string _dpID;

        public string DpID
        {
            get { return _dpID; }
            set { _dpID = value; }
        }
    }
}
