using System;
using System.Collections.Generic;
using System.Text;
using Botwave.Commons;

namespace Botwave.XQP.Service.WS
{
    /// <summary>
    /// ActionResult 辅助类.
    /// </summary>
    public static class ActionResultHelper
    {
        internal static void SetActionResultWhenTimeException(string lastModifiedTime, ActionResult result)
        {
            result.ReturnValue = false;
            result.ReturnMessage = String.Format("the format of lastModifiedTime [{0}] is not valid.", lastModifiedTime);
        }

        internal static void HandleException(ActionResult result, Exception ex, log4net.ILog log)
        {
            result.ReturnValue = false;

            if (GlobalSettings.Instance.IsDebugMode)
            {
                result.ReturnMessage = ex.ToString();
            }
            else
            {
                result.ReturnMessage = GlobalSettings.Instance.FailMessage;
            }

            if (log != null)
                log.Warn(ex);
        }

        internal static void SetLoginErrorInfo(string sysAccount, string sysPassword, ActionResult result)
        {
            result.ReturnValue = false;
            switch (result.AppAuth)
            {
                case AppAuthConstants.AccountError:
                    result.ReturnMessage = String.Format("account [{0}] not found", sysAccount);
                    break;
                case AppAuthConstants.UnMatch:
                    result.ReturnMessage = String.Format("account/password unmatch, [sysId:{0}], [sysPassword:{1}]", sysAccount, sysPassword);
                    break;
                default:
                    result.ReturnMessage = String.Format("account [{0}] is disabled", sysAccount);
                    break;
            }
        }
    }
}