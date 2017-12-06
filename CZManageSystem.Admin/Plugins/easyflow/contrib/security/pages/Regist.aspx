<%@ Page Language="C#" AutoEventWireup="true" Inherits="Botwave.Web.PageBase" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>用户注册-阅读注册协议</title>
</head>
<body style="font-family: 微软雅黑, Arial, 新宋体">
    <div class="topheader_wrapper">
        <div class="topHeader">
            <!--顶部导航 / TopNav-->
            <div class="topNav">
                广州市博汇数码科技有限公司
            </div>
            <!--头部 / Header-->
            <div id="header">
                <h1>
                    <span>广州市博汇数码科技有限公司</span></h1>
                <h2 class="xqp">
                </h2>
            </div>
        </div>
    </div>
    <div class="mainContent" style="margin: 5px 10px 5px 10px; background: none !important;">
        <div class="content2" style="padding: 10px">
            <div style="padding: 10px; height: 460px; margin: 0 auto; text-align: left; overflow: auto;
                background: #F0F6FC; color: #8096AA; border: 1px solid #74A8D6;">
                <p>
                    1、 服务条款的确立和接纳<br />
                    本网站业务受理系统的所有权和运作权,以及所受理具体业务的经营权归博汇数码科技有限公司，客户必须完全同意所有服务条款，才可以办理本网站的各类业务。</p>
                <p>
                    2、 会员必须遵循：<br />
                    (1)中国关于网络的相关法规。<br />
                    (2)使用网络服务不作非法用途。<br />
                    (3)不干扰或混乱网络服务。<br />
                    (4)遵守所有使用网络服务的网络协议、规定、程序和惯例。</p>
                <p>
                    3、 会员承诺：<br />
                    (1)不传输任何非法的、骚扰性的、中伤他人的、辱骂性的、恐吓性的、伤害性的、庸俗的，淫秽等信息资料；<br>
                    (2)不传输任何教唆他人构成犯罪行为的资料；<br />
                    (3)不传输任何不符合当地法规、国家法律和国际法律的资料；<br />
                    (4)未经许可而非法进入其它电脑系统是禁止的；<br />
                    (5)法律规定的其他义务</p>
                <p>
                    4、 请按规定准确填写相关的申请表格。<br />
                    客户填写完毕后按提交键确认，即构成客户对所提交资料内容真实性、准确性、合法性的承诺。 如果您完全接受以上的所有条款，请按"同意"进行受理。</p>
            </div>
            <p style="text-align: center; padding: 10px">
                <input name="Submit2" disabled="disabled" type="button" id="btnagree" class="btn"
                    onclick="javascript:location.href='register.aspx';" value=" 同 意 " />&nbsp;&nbsp;&nbsp;&nbsp;
                <input name="Submit" type="button" class="btn" onclick="javascript:window.close();" value=" 不同意 " />
            </p>
        </div>
    </div>
    
    <div class="footer_wrapper">
        <div class="footer">
            版权所有 <a href="http://www.botwave.com">广州博汇数码科技有限公司</a>  <span >版本号：vEasyFlow.3.081226.1619</span>
        </div>
    </div>
    <script type="text/javascript">
      	    var secs = 8;
      	    var wait = secs * 1000;
      	    document.getElementById("btnagree").value = "同 意(" + secs + ")";
      	    document.getElementById("btnagree").disabled = true;
      	    for (i = 1; i <= secs; i++) {
      	        window.setTimeout("update(" + i + ")", i * 1000);
      	    }
      	    window.setTimeout("timer()", wait);
      	    function update(num, value) {
      	        if (num == (wait / 1000)) {
      	            document.getElementById("btnagree").value = "同 意";
      	        } else {
      	            printnr = (wait / 1000) - num;
      	            document.getElementById("btnagree").value = "同 意(" + printnr + ")";
      	        }
      	    }
      	    function timer() {
      	        document.getElementById("btnagree").disabled = false;
      	        document.getElementById("btnagree").value = "同 意";
      	    }
    </script>
</body>
</html>
