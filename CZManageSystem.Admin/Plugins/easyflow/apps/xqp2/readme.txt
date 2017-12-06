[XQP2 Demo]
----------------------------------
BIZSDK 变动记录：

1. 移除 contrib/attachment 文件夹；
2. 移除 contrib/cms 文件夹；
3. 移除 contrib/usabilityTesting 文件夹；

4. 移除 web.config 中 HttpModule: <add name="UploadHttpModule" type="Botwave.Upload.UploadHttpModule, Botwave.Upload"/>
5. 移除 web.config 中 LeftMenu: <value>~/contrib/cms/controls/LeftMenu.ascx</value>

6. 修改 default.aspx 页：
	重定向链接改为：apps/xqp2/pages/extension/default.aspx.
	
7. 修改 contrib/themes/page/redirect.htm 页.
	重定向链接为 ../../../apps/xqp2/pages/extension/default.aspx .
	
8. 修改 contrib/workflow/pages/download.ashx 的 Class 为 
	Botwave.XQP.Web.HttpHandler.DownloadHandler, Botwave.XQP

9. 修改 contrib/workflow/web.config 文件.

10. 修改 contrib/security/pages/login.aspx
	修改 contrib/security/pages/loginIndex.aspx
	修改 masters/Default.master 页面 title 为 "广州博汇数码科技有限公司 - EasyFlow流程易平台".
	
----------------------------------
XQP2 变动记录

1. 移除 xqp_WorkflowInMenuGroup 中字段：WorkflowAlias, AliasImageUrl

----------------------------------