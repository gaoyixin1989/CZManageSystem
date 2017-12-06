<%@ Page Language="C#" AutoEventWireup="true" Inherits="apps_xqp2_pages_workflows_designer_Flowdesign_ModalDialog_attrBasic" Codebehind="attrBasic.aspx.cs" %>
<form runat="server">
<div class="tab-pane active" id="attrBasic">
    <div class="control-group">
        <span class="help-block">说明： 输入用户时，只输入用户名(字符、数字以及下划线的组合)，各用户名以逗号隔开(",")； </span>
    </div>
    <hr>
    <div class="control-group">
        <label class="control-label" for="process_name">
            步骤名称</label>
        <div class="controls">
            <asp:TextBox runat="server" ID="process_name" placeholder="步骤名称" Text="新建步骤"></asp:TextBox>
        </div>
    </div>
    <div class="control-group">
        <label class="control-label">
            打印设置</label>
        <div class="controls">
            <label class="radio inline">
                <input type="radio" name="process_type" value="is_step">开启
            </label>
            <label class="radio inline">
                <input type="radio" name="process_type" value="is_child">关闭
            </label>
            <label class="radio inline">
                - 默认开启打印。
            </label>
        </div>
    </div>
    <div class="control-group">
        <label class="control-label" for="process_name">
            打印次数</label>
        <div class="controls">
            <input type="text" id="Text1" placeholder="打印次数" name="process_name" value="-1">
            <span class="help-block">- 打印设置选 开启 时此设置才生效，-1表示无限制打印次数。</span>
        </div>
    </div>
    <div class="control-group">
        <label class="control-label">
            允许编辑处理意见</label>
        <div class="controls">
            <label class="checkbox inline">
                <input type="checkbox" name="process_type" value="is_step">允许编辑 <span class="help-block">
                    - 选中则在处理工单的时候允许编辑处理意见。</span>
            </label>
        </div>
    </div>
    <div class="control-group">
        <label class="control-label">
            允许退回再提交</label>
        <div class="controls">
            <label class="checkbox inline">
                <input type="checkbox" name="process_type" value="is_step">允许 <span class="help-block">
                    - 选中则在此步骤退还时可以直接提交到此步骤。</span>
            </label>
        </div>
    </div>
    <div class="control-group">
        <label class="control-label">
            允许手机审批</label>
        <div class="controls">
            <label class="checkbox inline">
                <input type="checkbox" name="process_type" value="is_step">允许 <span class="help-block">
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
</form>
