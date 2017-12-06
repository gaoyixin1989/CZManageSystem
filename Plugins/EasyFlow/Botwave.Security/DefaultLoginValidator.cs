using System;
using System.Collections.Generic;
using System.Text;

using Botwave.Commons;

namespace Botwave.Security
{
    public class DefaultLoginValidator : ILoginValidator
    {
        private static ILoginValidator loginValidator;

        public static ILoginValidator GetInstance()
        {
            if (null == loginValidator)
            {
                lock (typeof(DefaultLoginValidator))
                {
                    if (null == loginValidator)
                    {
                        loginValidator = new DefaultLoginValidator();
                    }
                }
            }
            return loginValidator;
        }

        #region ILoginValidator 成员

        public LoginStatus Validate(string username, string password, Botwave.Security.Domain.UserInfo targetUser)
        {
            LoginStatus status = LoginStatus.Success;

            //加密方式存在一些问题,明码可能对应多个密码.
            string decryptedPassword = TripleDESHelper.Decrypt(targetUser.Password);
            if (password != decryptedPassword)
            {
                status = LoginStatus.InvalidPassword;
            }                

            return status;
        }

        #endregion
    }
}
