using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class test_filetest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        HttpPostedFile file = this.FileUpload1.PostedFile;
        string fileName = "upload_" + Guid.NewGuid().ToString() + Botwave.FileManager.FileManagerHelper.GetFileExtensionName(file.FileName);

        Botwave.FileManager.FileManagerHelper.FileService.Upload(file.InputStream, fileName);
    }
}
