<%@ Page Language="C#" AutoEventWireup="true" Inherits="apps_xqp2_pages_workflows_designer_Flowdesign_ModalDialog_attribute" Codebehind="attribute.aspx.cs" %>

<form runat="server">
<div class="tab-pane <%=Request.QueryString["type"]!="judge"?"active":"" %>" id="attrBasic">
    <div class="control-group">
        <span class="help-block">说明： 输入用户时，只输入用户名(字符、数字以及下划线的组合)，各用户名以逗号隔开(",")； </span>
    </div>
    <hr>
    <div class="control-group">
        <label class="control-label" for="process_name">
            步骤名称</label>
        <div class="controls">
            <asp:textbox runat="server" id="txtActivityName" placeholder="步骤名称" value="" />
        </div>
    </div>
    <div class="control-group">
        <label class="control-label">
            打印设置</label>
        <div class="controls">
            <label class="radio inline">
                <input runat="server" type="radio" id="radOpenPrint" name="CanPrint" value="1" />开启
            </label>
            <label class="radio inline">
                <input runat="server" type="radio" id="radClosePrint" name="CanPrint" value="0" />关闭
            </label>
            <label class="radio inline">
                - 默认开启打印。
            </label>
        </div>
    </div>
    <div class="control-group">
        <label class="control-label" for="PrintAmount">
            打印次数</label>
        <div class="controls">
            <asp:Textbox runat="server" id="txtPrintAmount" placeholder="打印次数" Text="-1" onblur="this.value=this.value.replace(/[^\d]/g,'')"  />
            <span class="help-block">- 打印设置选 开启 时此设置才生效，-1表示无限制打印次数。</span>
        </div>
    </div>
    <div class="control-group">
        <label class="control-label">
            允许编辑处理意见</label>
        <div class="controls">
            <label class="checkbox inline">
                <input runat="server" type="checkbox" id="chkOption" value="1" />允许编辑 <span class="help-block">
                    - 选中则在处理工单的时候允许编辑处理意见。</span>
            </label>
        </div>
    </div>
    <div class="control-group">
        <label class="control-label">
            允许退回再提交</label>
        <div class="controls">
            <label class="checkbox inline">
                <input runat="server" type="checkbox" id="chkReturn" value="1" />允许 <span class="help-block">
                    - 选中则在此步骤退还时可以直接提交到此步骤。</span>
            </label>
        </div>
    </div>
    <div class="control-group">
        <label class="control-label">
            允许手机审批</label>
        <div class="controls">
            <label class="checkbox inline">
                <input runat="server" type="checkbox" id="chkIsMobile" value="1" />允许 <span class="help-block">
                    - 勾选后允许该步骤在手机版的页面上审批。</span>
            </label>
        </div>
    </div>
    <!--<div id="current_flow">
                        <div class="offset1">
                            <select multiple="multiple" size="7" name="process_to[]" id="process_multiple" style="display: none;">
                                <option value="295">新建步骤</option>
                            </select>
                            
                        </div>
                    </div>-->
    <!-- current_flow end -->
    <div id="child_flow" class="hide">
        <div class="control-group">
            <label class="control-label">
                子流程</label>
            <div class="controls">
                <select name="child_id">
                    <option value="0">--请选择--</option>
                    <option value="1">123</option>
                    <option value="2">报表</option>
                    <option value="3">as</option>
                    <option value="4">是的</option>
                    <option value="5">1111</option>
                    <option value="6">xxxx</option>
                    <option value="7">请假</option>
                    <option value="8">阿萨德</option>
                    <option value="9">grg</option>
                    <option value="10">爱爱爱</option>
                    <option value="11">12312</option>
                    <option value="12">gggg</option>
                    <option value="13">1</option>
                    <option value="14">234</option>
                    <option value="15">test</option>
                    <option value="16">333</option>
                    <option value="17">xx</option>
                    <option value="18">testss</option>
                    <option value="19">111</option>
                    <option value="20">www</option>
                    <option value="21">121</option>
                    <option value="22">546456</option>
                    <option value="23">11</option>
                    <option value="24">111</option>
                    <option value="25">流程</option>
                    <option value="26">a'f</option>
                    <option value="27">test</option>
                    <option value="28">请教</option>
                    <option value="29">test</option>
                    <option value="30">测试一下，行吧</option>
                    <option value="31">ww</option>
                    <option value="32">我的测试</option>
                    <option value="33">测试</option>
                    <option value="34">fgfg</option>
                    <option value="35">dfasdf</option>
                    <option value="36">ddd</option>
                    <option value="37">312</option>
                    <option value="38">12441</option>
                    <option value="39">test</option>
                    <option value="40">ahhh</option>
                    <option value="41">11</option>
                    <option value="42">地方</option>
                    <option value="43">test</option>
                    <option value="44">公司流程</option>
                    <option value="45">对对对</option>
                    <option value="46">xxxx</option>
                    <option value="47">dasda</option>
                    <option value="48">hgj</option>
                    <option value="49">yyyyyyy</option>
                    <option value="50">1</option>
                    <option value="51">发送到</option>
                    <option value="52">阿斯蒂芬</option>
                    <option value="53">wddd</option>
                    <option value="54">5</option>
                    <option value="55">765</option>
                    <option value="56">合同审批</option>
                    <option value="57">123123</option>
                    <option value="58">111</option>
                    <option value="59">测试流程1</option>
                    <option value="60">测试请假</option>
                    <option value="61">1212</option>
                    <option value="62">试试</option>
                    <option value="63">请假流程</option>
                    <option value="64">1111</option>
                    <option value="65">1</option>
                    <option value="66">qw</option>
                    <option value="67">测试流程</option>
                    <option value="68">55</option>
                    <option value="69">errrrrrrrrrw</option>
                    <option value="70">test by oiut</option>
                    <option value="71">45654</option>
                    <option value="72">xx流程</option>
                    <option value="73">aaa</option>
                    <option value="74">1212</option>
                    <option value="75">test111</option>
                    <option value="76">测试license</option>
                    <option value="77">test</option>
                    <option value="78">111</option>
                    <option value="79">34444444444</option>
                    <option value="80">测试流程1</option>
                    <option value="81">test0001</option>
                    <option value="82">年假流程</option>
                    <option value="83">请假单测试</option>
                    <option value="84">撒旦</option>
                    <option value="85">流程以</option>
                    <option value="86">轻轻巧巧、</option>
                </select>
            </div>
        </div>
        <div class="control-group">
            <label class="control-label">
                子流程结束后动作</label>
            <div class="controls">
                <label class="radio inline">
                    <input type="radio" name="child_after" value="1" checked="checked">
                    同时结束父流程
                </label>
                <label class="radio inline">
                    <input type="radio" name="child_after" value="2">
                    返回父流程步骤
                </label>
            </div>
        </div>
        <div class="control-group hide" id="child_back_id">
            <label class="control-label">
                返回步骤</label>
            <div class="controls">
                <select name="child_back_process">
                    <option value="0">--默认--</option>
                    <option value="294">新建步骤</option>
                    <option value="295">新建步骤</option>
                    <!--option value="1">步骤1</option>
                <option value="2">步骤2</option>
                <option value="3">步骤3</option-->
                </select>
                <span class="help-inline">默认为当前步骤下一步</span>
            </div>
        </div>
    </div>
    <!-- child_flow end -->
</div>
<!-- attrBasic end -->
<div class="tab-pane" id="attrPower" style="display:none">
    <div class="control-group">
        <table class="tblGrayClass grayBackTable" cellpadding="4" cellspacing="1" style="margin-top: 6px;">
            <tr>
                <th style="width: 60px">
                    下行分派控制类型：
                </th>
                <td style="padding: 5px 0 5px 0">
                    <div id="divControlTypes" style="margin-left: 10px">
                        <label for="chkUsers" class="checkbox inline">
                            <input runat="server" name="chkUsers" type="checkbox" id="chkUsers" />用户控制</label>
                        <label for="chkOrg" class="checkbox inline">
                            <input runat="server" name="chkOrg" type="checkbox" id="chkOrg" />组织控制</label>
                        <label for="chkRes" class="checkbox inline">
                            <input runat="server" name="chkRes" type="checkbox" id="chkRes" />权限控制</label>
                        <label for="chkRole" class="checkbox inline">
                            <input runat="server" name="chkRole" type="checkbox" id="chkRole" />角色控制</label>
                        <label for="chkControl" class="checkbox inline">
                            <input runat="server" name="chkControl" type="checkbox" id="chkControl" />历史步骤处理人</label>
                        <label for="chkPssor" class="checkbox inline">
                            <input runat="server" name="chkPssor" type="checkbox" id="chkPssor" />以前处理人</label>
                        <label for="chkPssctl" class="checkbox inline">
                            <input runat="server" name="chkPssctl" type="checkbox" id="chkPssctl" />过程控制</label>
                        <label for="chkStarter" class="checkbox inline">
                            <input runat="server" name="chkStarter" type="checkbox" id="chkStarter" />发起人</label>
                    </div>
                </td>
            </tr>
            <tr>
                <th>
                    平转分派控制类型：
                </th>
                <td style="padding: 5px 0 5px 0">
                    <div id="divAssignControlTypes" style="margin-left: 10px">
                        <label for="chkUsersAssign" class="checkbox inline">
                            <input runat="server" name="chkUsersAssign" type="checkbox" id="chkUsersAssign" />用户控制</label>
                        <label for="chkOrgAssign" class="checkbox inline">
                            <input runat="server" name="chkOrgAssign" type="checkbox" id="chkOrgAssign" />组织控制</label>
                        <label for="chkResAssign" class="checkbox inline">
                            <input runat="server" name="chkResAssign" type="checkbox" id="chkResAssign" />权限控制</label>
                        <label for="chkRoleAssign" class="checkbox inline">
                            <input runat="server" name="chkRoleAssign" type="checkbox" id="chkRoleAssign" />角色控制</label>
                        <label for="chkControlAssign" class="checkbox inline">
                            <input runat="server" name="chkControlAssign" type="checkbox" id="chkControlAssign" />历史步骤处理人</label>
                        <label for="chkPssorAssign" class="checkbox inline">
                            <input runat="server" name="chkPssorAssign" type="checkbox" id="chkPssorAssign" />以前处理人</label>
                        <label for="chkPssctlAssign" class="checkbox inline">
                            <input runat="server" name="chkPssctlAssign" type="checkbox" id="chkPssctlAssign" />过程控制</label>
                        <label for="chkStarterAssign" class="checkbox inline">
                            <input runat="server" name="chkStarterAssign" type="checkbox" id="chkStarterAssign" />发起人</label>
                    </div>
                </td>
            </tr>
            <tr id="trUsers" runat="server" style="padding: 5px 0 5px 0; display: none">
                <th>
                    下行用户设置：
                </th>
                <td>
                    <textarea name="txtUsers" rows="2" cols="20" id="txtUsers" runat="server" style="height: 35px;
                        width: 520px;"></textarea>
                    <a href="javascript:void(0);" onclick="javascrpt:return openUserSelector('<%=AppPath%>','txtUsers');">
                        选择用户</a>
                </td>
            </tr>
            <tr id="trUsersAssign" runat="server" style="padding: 5px 0 5px 0; display: none">
                <th>
                    平转用户设置：
                </th>
                <td>
                    <textarea name="txtUsersAssign" rows="2" cols="20" runat="server" id="txtUsersAssign"
                        style="height: 35px; width: 520px;"></textarea>
                    <a href="javascript:void(0);" onclick="javascrpt:return openUserSelector('<%=AppPath%>','txtUsersAssign');">
                        选择用户</a>
                </td>
            </tr>
            <tr id="trOrg" runat="server" style="display: none">
                <th>
                    下行组织设置：
                </th>
                <td>
                    <table border="0" cellspacing="0" style="margin: 0; padding: 0">
                        <tr>
                            <td>
                                <asp:literal id="ltlOrg" runat="server"></asp:literal>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="trOrgAssign" runat="server" style="display: none">
                <th>
                    平转组织设置：
                </th>
                <td>
                    <table border="0" cellspacing="0" style="margin: 0; padding: 0">
                        <tr>
                            <td>
                                <asp:literal id="ltlOrgAssign" runat="server"></asp:literal>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="trRole" runat="server" style="display: none">
                <th>
                    下行角色设置：
                </th>
                <td>
                    <asp:literal id="ltlRole" runat="server"></asp:literal>
                </td>
            </tr>
            <tr id="trRoleAssign" runat="server" style="display: none">
                <th>
                    平转角色设置：
                </th>
                <td>
                    <asp:literal id="ltlRoleAssign" runat="server"></asp:literal>
                </td>
            </tr>
            <tr id="orgType" runat="server" style="display: none">
                <th>
                    下行分派历史步骤处理人：
                </th>
                <td style="padding: 5px 0 5px 0">
                    <div style="margin-left: 10px" id="div1">
                        <asp:dropdownlist id="drdlActivities" runat="server"></asp:dropdownlist>
                        &nbsp;步骤的处理人
                    </div>
                </td>
            </tr>
            <tr id="orgTypeAssign" runat="server" style="display: none">
                <th>
                    平转分派历史步骤处理人：
                </th>
                <td style="padding: 5px 0 5px 0">
                    <div style="margin-left: 10px" id="div2">
                        <asp:dropdownlist id="drdlActivitiesAssign" runat="server"></asp:dropdownlist>
                        &nbsp;步骤的处理人
                    </div>
                </td>
            </tr>
            <tr id="orgPssor" runat="server" style="display: none">
                <th>
                    下行分派过程控制：
                </th>
                <td style="padding: 5px 0 5px 0">
                    <div style="margin-left: 10px" id="div3">
                        <asp:dropdownlist id="drdlPssor" runat="server"></asp:dropdownlist>
                        &nbsp;步骤的组织控制类型
                    </div>
                </td>
            </tr>
            <tr id="orgPssorAssign" runat="server" style="display: none">
                <th>
                    平转分派过程控制：
                </th>
                <td style="padding: 5px 0 5px 0">
                    <div style="margin-left: 10px" id="div4">
                        <asp:dropdownlist id="drdlPssorAssign" runat="server"></asp:dropdownlist>
                        &nbsp;步骤的组织控制类型
                    </div>
                </td>
            </tr>
            <tr style="display: none;">
                <th>
                    下行默认控制类型：
                </th>
                <td>
                    <select name="selDefaultTypes" id="selDefaultTypes" runat="server" style="width: 120px">
                        <option value="">无</option>
                        <option value="users">用户控制</option>
                        <option value="superior">组织控制</option>
                        <option value="resource">权限控制</option>
                        <option value="resource">以前处理人</option>
                        <option value="starter">发起人</option>
                    </select>
                </td>
            </tr>
            <tr style="display: none;">
                <th>
                    平转默认控制类型：
                </th>
                <td>
                    <select name="selAssignDefaultTypes" id="selAssignDefaultTypes" runat="server" style="width: 120px">
                        <option value="">无</option>
                        <option value="users">用户控制</option>
                        <option value="superior">组织控制</option>
                        <option value="resource">权限控制</option>
                        <option value="resource">以前处理人</option>
                        <option value="starter">发起人</option>
                    </select>
                </td>
            </tr>
        </table>
    </div>
</div>
<!-- attrPower end -->
<div class="tab-pane" id="attrOperate" style="display:none">
    <table class="table">
        <tbody id="Tbody1">
            <tr>
                <th>
                    退回选项：
                </th>
                <td>
                    <select name="ddlrejectOption" id="ddlrejectOption" runat="server">
                        <option value="" ></option>
                        <option value="initial" >退回起始步骤</option>
                        <option value="previous">退回上一步骤</option>
                        <option value="none">不允许退回</option>
                        <option value="customize">自定义退回步骤</option>
                    </select>
                </td>
                <th class="customize" style="display:none">
                    选择退回步骤：
                </th>
                <td class="customize" style="display:none">
                    <asp:dropdownlist id="ddlcustomize" runat="server">
                        
                    </asp:dropdownlist>
                </td>
            </tr>
        </tbody>
    </table>
    <div class="control-group">
        <label class="control-label">
            合并条件</label>
        <div class="controls">
            <input type="text" id="txtjoincondition" runat="server" class="input-small" style="width:95%"/>
            <span class="help-inline">所有分支走完才能合并：type:XqpJoinConditionHandler;args:all；</span>
        </div>
    </div>
    <div class="control-group">
        <label class="control-label">
            分支条件</label>
        <div class="controls">
            <input type="text" runat="server" runat="server" id="txtsplitcondition" class="input-small" style="width:95%"/>
            <span class="help-inline">所有分支：all</span>
        </div>
    </div>
    <div class="control-group">
        <label class="control-label">
            会签条件</label>
        <div class="controls">
            <input type="text" runat="server" id="txtcountersignedcondition" class="input-small" style="width:95%"/>
            <span class="help-inline">all：开启该步骤的会签，该步骤所有处理人处理之后才会产生下一步骤待办</span>
        </div>
    </div>
</div>
<!-- attrOperate end -->
<div class="tab-pane <%=Request.QueryString["type"]=="judge"?"active":"" %>" id="attrJudge" style="display:none">
    <div class="control-group">
        <span class="help-block">规则命令可手动配置也可手动填写，命令格式遵循SQL语句的where条件；<br />
            比较条件为 大于、小于、大于等于、小于等于 的字段类型必须为数字类型，例如：<br />
            F1的比较条件为 大于、小于、大于等于、小于等于，则F1的类型必须为数字类型；<br />
            规则示例：其中F1、F2、....为流程表单字段
            <br />
            路由规则设置中的字段可根据需要自己在字段集中添加，以英文分号隔开，如F1;F2;F3;...
            <br />
            单条件示例：F1='Test'、并条件示例：F1='Test' AND F2='Test'、或条件示例：F1='Test' OR F2='Test'；<br />
            复杂条件示例：F1='Test' AND (F2='Test' OR F2='Test1') 等价于如下判断逻辑：<br />
            if(F1=Test){if(F2=Test || F2=Test1)} </span>
    </div>
    <hr>
    <table class="table">
        <tbody id="ctbody">
            <tr>
                <th>
                    下行步骤：
                </th>
                <td>
                    <asp:dropdownlist id="drdlNextActivity" runat="server">
                        
                    </asp:dropdownlist>
                </td>
            </tr>
            <tr>
                <th>
                    字段集：
                </th>
                <td>
                    <input type="text" id="txtFieldsAssemble" class="input-small" style="width:95%"/>
                </td>
            </tr>
            <tr>
                <th>
                    路由规则设置：
                </th>
                <td>
                    <table cellpadding="0" cellspacing="0" style="text-align: center;">
                        <tr>
                            <td>
                                <asp:DropDownList ID="drdlFName" runat="server" ></asp:DropDownList>
                            </td>
                            <td>
                                <select id="drdlCondition" class="input-small">
                                    <option value="=" selected="selected">等于</option>
                                    <option value="&lt;&gt;">不等于</option>
                                    <option value="&gt;">大于</option>
                                    <option value="&lt;">小于</option>
                                    <option value="&gt;=">大于等于</option>
                                    <option value="&lt;=">小于等于</option>
                                    <option value="startwith">向后匹配</option>
                                    <option value="endwith">向前匹配</option>
                                    <option value="like">模糊匹配</option>
                                    <option value="notlike">不包含</option>
                                </select>
                            </td>
                            <td>
                                <input type="text" id="txtVal" class="input-small"/>
                            </td>
                            <td>
                                <select id="ddlrelation" class="input-small">
                                    <option value="" selected="selected">- 关联条件 -</option>
                                    <option value="AND">与</option>
                                    <option value="OR">或者</option>
                                </select>
                            </td>
                            <td>
                                <div class="btn-group">
                                    <button type="button" onclick="return fnAddConditions()" class="btn btn-small">
                                        新增</button>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <th>
                    路由规则描述：
                </th>
                <td>
                    <asp:textbox id="txtDescription" textmode="MultiLine" width="450px" height="70px"
                        runat="server"></asp:textbox>
                </td>
            </tr>
            <tr>
                <th>
                    路由规则代码：
                </th>
                <td style="padding: 5px 0 5px 0">
                    <asp:textbox id="txtCommandRules" textmode="MultiLine" width="450px" height="70px"
                        runat="server"></asp:textbox>
                </td>
                <%--<th>父规则:</th><td><asp:DropDownList ID="drdlPrerequest" runat="server"></asp:DropDownList></td>--%>
            </tr>

            <tr>
                <td colspan="2" style="text-align:center">
                    <div class="btn-group">
                        <button type="button" id="btn_Add" class="btn btn-small">
                            保存</button><button type="button" id="btnClear" class="btn btn-small">
                            清空</button>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
    <table class="table table-condensed table-bordered table-hover" id="tbResults" >
    <thead>
    <tr>
      <th style="text-align:center">标题</th>
      <th style="text-align:center">描述</th>
      <th style="text-align:center">规则代码</th>
      <th style="text-align:center">字段集</th>
      <th style="text-align:center">下行步骤</th>
      <th style="text-align:center">操作</th>
    </tr>
    </thead>
    <tbody>
    <asp:Repeater ID="listResults" runat="server"  >
				        <ItemTemplate>
    				        <tr style="text-align:center;">
    				            <td style="text-align:left;"><%# Eval("title") %></td>
    				            <td style="text-align:left;"><%# Eval("description") %></td>
    				            <td style="text-align:left;"><%# Eval("conditions") %></td>
                                <td style="text-align:left;"><%# Eval("FieldsAssemble")%></td>
    				            <td><%#Eval("NextActivityName")%></td>
    				            <td>
                                    <button type="button" name="btnEdit" class="btn btn-small">
                            编辑</button><button type="button" name="btnDel" class="btn btn-small">
                            删除</button>
    				            </td>
    				        </tr>				            
				        </ItemTemplate>
				    </asp:Repeater>
    </tbody>
    </table>
    <input type="hidden" name="process_condition" id="process_condition" value="298,"/>

</div>
<!-- attrJudge end -->
</form>
