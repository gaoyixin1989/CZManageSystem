using System;
namespace Botwave.Security
{
    /// <summary>
    /// 登录返回状态.
    /// </summary>
    [Serializable]
    public enum LoginStatus
    {
        /// <summary>
        /// 未知.
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// 成功.
        /// </summary>
        Success = 1,
        /// <summary>
        /// 帐号不存在.
        /// </summary>
        AccountNotFound = 2,
        /// <summary>
        /// 帐号被禁用.
        /// </summary>
        AccountDisabled = 3,
        /// <summary>
        /// 密码无效.
        /// </summary>
        InvalidPassword = 4,
        /// <summary>
        /// 验证令牌不合法.
        /// </summary>
        InvalidValidateToken = 5
    }

    /// <summary>
    /// 登录状态辅助类.
    /// </summary>
    public static class LoginStatusHelper
    {
        /// <summary>
        /// 将登录状态转换为文字描述.
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string ToDescription(LoginStatus status)
        {
            string desc = "未知";
            switch (status)
            {
                case LoginStatus.Success:
                    desc = "成功";
                    break;
                case LoginStatus.AccountNotFound:
                    desc = "帐号不存在";
                    break;
                case LoginStatus.AccountDisabled:
                    desc = "帐号被禁用";
                    break;
                case LoginStatus.InvalidPassword:
                    desc = "密码无效";
                    break;
                case LoginStatus.InvalidValidateToken:
                    desc = "验证令牌不合法";
                    break;
            }
            return desc;
        }
    }
}
