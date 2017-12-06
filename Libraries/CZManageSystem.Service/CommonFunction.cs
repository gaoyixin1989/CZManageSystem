using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CZManageSystem.Service
{
    public class CommonFunction
    {

        //判断是否为手机号码
        public static bool isMobilePhone(string value)
        {
            //电信手机号正则表达式
            string dianxin = @"^1[3578][01379]\d{8}$";
            Regex dReg = new Regex(dianxin);
            //移动手机号正则表达式
            string yidong = @"^(134[012345678]\d{7}|1[34578][012356789]\d{8})$";
            Regex yReg = new Regex(yidong);
            //联通手机号正则表达式
            string liantong = @"^1[34578][01256]\d{8}$";
            Regex lReg = new Regex(liantong);

            value = value.Trim();
            if (dReg.IsMatch(value) || yReg.IsMatch(value) || lReg.IsMatch(value))
                return true;
            else
                return false;

        }

    }
}
