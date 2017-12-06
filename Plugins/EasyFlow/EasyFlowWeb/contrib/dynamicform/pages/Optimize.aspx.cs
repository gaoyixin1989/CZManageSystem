using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave.DynamicForm.Services;

public partial class contrib_dynamicform_pages_Optimize : Botwave.Web.PageBase
{
    private IPartTableService partTableService = (IPartTableService)Ctx.GetObject("partTableService");

    public IPartTableService PartTableService
    {
        get { return partTableService; }
        set { partTableService = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnTablePart_Click(object sender, EventArgs e)
    {
        string numberValue = txtNumber.Text.Trim();
        string topValue = txtTopValue.Text.Trim();
        if (string.IsNullOrEmpty(numberValue) || string.IsNullOrEmpty(topValue))
        {
            ltlMessage.Text = "请输入序号或查询数据数.";
        }
        else
        {
            int tableIndex = Convert.ToInt32(numberValue);
            int topCount = Convert.ToInt32(topValue);

            if (tableIndex <= 0 || topCount <= 0)
            {
                ltlMessage.Text = "请输入序号或查询数据数都必须大于 0.";
            }
            else
            {
                try
                {
                    if (partTableService.MigrateData(topCount, tableIndex))
                    {
                        ltlMessage.Text = "分表成功.";
                    }
                    else
                    {
                        ltlMessage.Text = "分表失败.";
                    }
                }
                catch (Exception ex)
                {
                    ltlMessage.Text = ex.ToString();
                }
            }
        }
    }
}
