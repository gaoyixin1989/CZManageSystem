<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="contrib_dynamicform_pages_TemplateCreate" Title="表单设计" ResponseEncoding="utf-8" UICulture="zh-CN" EnableEventValidation="false" Codebehind="TemplateCreate.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <script type="text/javascript" language="javascript" src="scripts/dynamic-form.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div class="titleContent">
        <h3><span><asp:Literal ID="ltlTitle" runat="server"></asp:Literal>表单模板设计</span></h3>
    </div>
    <div class="btnControl">
        <div class="btnLeft">
            <input type="button" value="表单管理" class="btnNewwin" onclick="window.location.href='list.aspx?wid=<%=WorkflowId%>';" />
        </div>
    </div>
    <div class="dataList"  style="padding-top:5px">   
        <div class="showControl">
            <h4>表单基本属性</h4>
            
        </div>
        <table class="tblGrayClass grayBackTable" cellpadding="4" cellspacing="1">
            <tr>
            <th width="15%">&nbsp;表单名称：</th>
            <td width="35%"> <asp:TextBox ID="txtFormName" runat="server" Width="250px" CssClass="inputbox"></asp:TextBox></td>
            </tr>
            <tr>
            <th width="15%">&nbsp;备注：</th>
            <td width="35%"><asp:TextBox ID="txtFormRemark" runat="server" Width="250px" CssClass="inputbox"></asp:TextBox></td>
            </tr>
            </table>
		<div class="showControl">
            <h4>表单库字段基本属性</h4>
        </div>
        <table class="tblGrayClass grayBackTable" cellpadding="4" cellspacing="1">
                <tr>
                    <th width="15%">&nbsp;字段名称：</th>
                    <td width="35%">
                        <span hint="字段名称"><asp:TextBox ID="txtFName" runat="server" Width="148px" CssClass="inputbox"></asp:TextBox></span>&nbsp;<span
                            id="spFNameMsg" style="color: Red"></span><asp:RequiredFieldValidator ID="RequiredFieldValidator2"
                                runat="server" ControlToValidate="txtFName" ErrorMessage="字段名称"></asp:RequiredFieldValidator>
                    </td>
                    <th width="15%"> &nbsp;字段意义： </th>
                    <td width="35%">
                        <span hint="字段意义"><asp:TextBox ID="txtName" runat="server" Width="148px" CssClass="inputbox"></asp:TextBox></span><asp:RequiredFieldValidator
                            ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtName" ErrorMessage="字段意义"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <th width="15%">&nbsp;字段注释：</th>
                    <td width="35%">
                        <span  hint="字段注释"><asp:TextBox ID="txtComment" runat="server" Width="148px" CssClass="inputbox"></asp:TextBox></span>
                    </td>
                    <th width="15%">&nbsp;默认值：</th>
                    <td width="35%">
                        <span hint="默认值"><asp:TextBox ID="txtDefaultValue" runat="server" CssClass="inputbox"></asp:TextBox></span>
                    </td>
                </tr>
                <tr>
                    <th width="15%">&nbsp;控件类型：</th>
                    <td colspan="3">
                        <span hint="控件类型"><asp:RadioButtonList ID="rbItemType" runat="server" RepeatDirection="Horizontal"
                            RepeatColumns="9">
                            <asp:ListItem Value="0" Selected="True">单行输入</asp:ListItem>
                            <asp:ListItem Value="1">多行输入</asp:ListItem>
                            <asp:ListItem Value="2">下拉框</asp:ListItem>                            
                            <asp:ListItem Value="4">多选框</asp:ListItem>
                            <asp:ListItem Value="5">单选框</asp:ListItem>
                            <asp:ListItem Value="6">日期</asp:ListItem>
                            <asp:ListItem Value="7">文件</asp:ListItem>                            
                            <asp:ListItem Value="9">复杂HTML</asp:ListItem>
                            <%--<asp:ListItem Value="3">标签</asp:ListItem>
                            <asp:ListItem Value="8">自增多行</asp:ListItem>
                            <asp:ListItem Value="10">隐藏</asp:ListItem>--%>
                        </asp:RadioButtonList></span>
                    </td>
                </tr>
                <tr>
                    <th width="15%">&nbsp;数据类型：</th>
                    <td colspan="3">
                        <span hint="数据类型"><asp:RadioButtonList ID="rbItemDataType" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="0" Selected="True">字符串</asp:ListItem>
                            <asp:ListItem Value="1">数字</asp:ListItem>
                            <asp:ListItem Value="2">文本</asp:ListItem>
                            <asp:ListItem Value="3">文件</asp:ListItem>
                        </asp:RadioButtonList></span>
                    </td>
                </tr>
                <tr id="trSelect" style="display: none">
                    <th width="15%">&nbsp;选项内容：</th>
                    <td colspan="3">
                        <span hint="选项内容"><asp:TextBox ID="txtDataSource" runat="server" Width="250px" CssClass="inputbox"></asp:TextBox></span>
                    </td>
                </tr>
                </table> 
		<div class="showControl">
            <h4>高级设置</h4>
            <button onclick="return showContent(this,'dataDiv1');" title="收缩"><span>折叠</span></button>
        </div>
		    <div class="dataTable" id="dataDiv1" style="display:none;">
		    <fieldset>
                            <legend>行列与高宽</legend>
                            <table class="tblGrayClass grayBackTable" cellpadding="4" cellspacing="1">
                                <tr>
                    <th width="15%">&nbsp;所在行数：</th>
                    <td width="35%">
                        <span hint="所在行数"><asp:TextBox ID="txtTop" runat="server" CssClass="inputbox"></asp:TextBox></span><asp:RegularExpressionValidator
                            ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtTop" ErrorMessage="*"
                            ValidationExpression="\d+"></asp:RegularExpressionValidator>
                    </td>
                    <th width="15%">&nbsp;所在列数：</th>
                    <td width="35%">
                        <span hint="所在列数"><asp:TextBox ID="txtLeft" runat="server" Width="148px" CssClass="inputbox"></asp:TextBox></span><asp:RegularExpressionValidator
                            ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtLeft" ErrorMessage="*"
                            ValidationExpression="\d+"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr id="trPos">
                    <th width="15%">&nbsp;宽度：</th>
                    <td width="35%">
                        <span hint="宽度"><asp:TextBox ID="txtWidth" runat="server" CssClass="inputbox"></asp:TextBox></span><asp:RegularExpressionValidator
                            ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtWidth"
                            ErrorMessage="*" ValidationExpression="\d+"></asp:RegularExpressionValidator>
                    </td>
                    <th width="15%" nowrap>&nbsp;高度：</th>
                    <td width="35%" nowrap>
                        <span hint="高度"><asp:TextBox ID="txtHeight" runat="server" CssClass="inputbox"></asp:TextBox></span><asp:RegularExpressionValidator
                            ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtHeight"
                            ErrorMessage="*" ValidationExpression="\d+"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <th width="15%">&nbsp;独占一行：</th>
                    <td colspan="3">
                        <span hint="独占一行"><asp:CheckBox ID="chkRowExclusive" runat="server" Text="是"></asp:CheckBox></span>
                    </td>
                </tr>
                            </table>
                 </fieldset>
            <fieldset>
                            <legend>读写权限</legend>
                            <table class="tblGrayClass grayBackTable" cellpadding="4" cellspacing="1">
                                <tr>
                                    <th width="15%">&nbsp;是否显示：</th>
                                    <td colspan="3">
                                        <span hint="是否显示"><input type="checkbox" id="cbShowSet" value="设置" />不显示</span>

                                        <div id="divShowActivities" style="display: none">
                                            <hr />
                                            设置以下步骤不显示：
                                            <input type="checkbox" id="cbCheckAll4ShowSet" value="" />全选

                                            <asp:CheckBoxList ID="cblActivities4Show" DataTextField="ActivityName" RepeatColumns="5" RepeatDirection="Horizontal" runat="server">
                                            </asp:CheckBoxList>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <th width="15%"> &nbsp;是否可编辑：</th>
                                    <td colspan="3">
                                        <span hint="是否可编辑"><input type="checkbox" id="cbReadonlySet" value="" />可编辑</span>

                                        <div id="divReadonlyActivities" style="display: none">
                                            <hr />
                                            设置以下步骤可编辑：
                                             <input type="checkbox" id="cbCheckAll4ReadonlySet" value="" />全选

                                            <asp:CheckBoxList ID="cblActivities4Read" DataTextField="ActivityName" RepeatColumns="5" RepeatDirection="Horizontal" runat="server">
                                            </asp:CheckBoxList>
                                        </div>
                                    </td>
                                </tr>                               
                            </table>
                        </fieldset>
            <fieldset>
                            <legend>输入验证</legend>
                            <table class="tblGrayClass grayBackTable" cellpadding="4" cellspacing="1">
                                <tr>
                                    <th width="15%">&nbsp;是否允许为空：</th>
                                    <td width="35%">
                                        <span hint="是否允许为空"><asp:CheckBox ID="chkRequire" runat="server" Text="不允许"></asp:CheckBox></span>
                                    </td>
                                    <th width="15%"> &nbsp;验证类型：</th>
                                    <td width="35%">
                                        <span hint="验证类型"><asp:DropDownList ID="ddlValidateType" runat="server">
                                            <asp:ListItem Value="" Selected="True">不需要</asp:ListItem>
                                            <asp:ListItem Value="require">非空</asp:ListItem>
                                            <asp:ListItem Value="group">必选</asp:ListItem>
                                            <asp:ListItem Value="compare">数值比较</asp:ListItem>
                                            <asp:ListItem Value="match">内容匹配</asp:ListItem>
                                            <asp:ListItem Value="date">日期验证</asp:ListItem>
                                            <asp:ListItem Value="custom">正则式</asp:ListItem>
                                        </asp:DropDownList></span>
                                    </td>
                                </tr>
                                <tr id="trCompare" style="display: none">
                                    <th width="15%">&nbsp;比较操作符：</th>
                                    <td width="35%">
                                        <asp:DropDownList ID="ddlOp" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <th width="15%">&nbsp;比较对象：</th>
                                    <td width="35%">
                                        <asp:TextBox ID="txtOpTarget" runat="server" CssClass="inputbox"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="trRange" style="display: none">
                                    <th width="15%">&nbsp;最大值(数字)：</th>
                                    <td width="35%">
                                        <asp:TextBox ID="txtMaxVal" runat="server" CssClass="inputbox"></asp:TextBox><asp:RegularExpressionValidator
                                            ID="rfvMaxVal" runat="server" ControlToValidate="txtMaxVal" ErrorMessage="*"></asp:RegularExpressionValidator>
                                    </td>
                                    <th width="15%">&nbsp;最小值(数字)：</th>
                                    <td width="35%">
                                        <asp:TextBox ID="txtMinVal" runat="server" CssClass="inputbox"></asp:TextBox><asp:RegularExpressionValidator
                                            ID="rfvMinVal" runat="server" ControlToValidate="txtMinVal" ErrorMessage="*"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th width="15%">&nbsp;错误信息：</th>
                                    <td colspan="3">
                                        <span hint="错误信息"><asp:TextBox ID="txtErrorMessage" runat="server" Width="520px" CssClass="inputbox"
                                            MaxLength="100"></asp:TextBox></span>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
		    </div>     
            <p style="text-align:center;margin-top:5px;">
                <asp:Button ID="btnAddItem" runat="server" CssClass="btn_add" Text="添 加" OnClick="btnAddItem_Click">
                        </asp:Button>&nbsp;
                        <asp:Button ID="btnCreateTemplate" runat="server" CssClass="btnClass2" Text="生成模板" OnClientClick="return confirm('确定要生成模板吗?');"
                            CausesValidation="False" OnClick="btnCreateTemplate_Click"></asp:Button>&nbsp;
                        <input class="btnClass2" onclick="javascript:location.href ='Template.aspx?fdid=<%=FormDefinitionId %>&wfid=<%=WorkflowId %>&EntityType=<%=EntityType%>'"
                            type="button" value="编辑模板"/>&nbsp;
                        <input class="btnClass2l" onclick="javascript:location.href ='WapTemplate.aspx?fdid=<%=FormDefinitionId %>&wfid=<%=WorkflowId %>&EntityType=<%=EntityType%>'"
                            type="button" value="编辑Wap模板"/>&nbsp;
                        <input class="btnReturnClass" onclick="javascript:location.href ='<%=ReturnURL %>'"
                            type="button" value="返回"/>
            </p>
            <p style="text-align:center;margin-top:5px;">
                已添加字段列表：
                        <asp:DropDownList ID="ddlItems" DataTextField="Name" DataValueField="Id" runat="server">
                        </asp:DropDownList>
                        <asp:Button ID="btnUpdateItem" runat="server" Width="67px" CssClass="btn_edit" Text="修 改"
                            OnClick="btnUpdateItem_Click"></asp:Button>&nbsp;
                        <input type="button" id="btnAdvancedConfig" style="width:80px" class="btn_ext" value="   扩展设置"
                          onclick="return OpenSelectionDialog()"  />&nbsp;
                        <asp:Button ID="btnDeleteItem" runat="server" Width="65px" CssClass="btn_del" Text="删 除"
                            CausesValidation="False" OnClick="btnDeleteItem_Click"></asp:Button>
            </p>
    </div>
    <div class="dataList">
        <div class="showControl">
            <h4>表单预览</h4>
            <button onclick="return showContent(this,'divFormPreview');" title="收缩">
                <span>折叠</span></button>
        </div>
        <div class="dataTable" id="divFormPreview">
            <asp:Label ID="lblPreview" runat="server" Text=""></asp:Label>
            <asp:HiddenField ID="hdFormDefinitionId" runat="server" />
        </div>
        <div class="showControl">
            <h4>Wap表单预览</h4>
            <button onclick="return showContent(this,'divwap');" title="收缩">
                <span>折叠</span></button>
        </div>
        <div class="dataTable" id="divWapPreview" style="display:none">
            <asp:Label ID="lblWapReview" runat="server" Text=""></asp:Label>
        </div>
        <div class="dataTable" id="divwap">
            <asp:Label ID="ltlIframe" runat="server" Text=""></asp:Label>
        </div>
    </div>
    <script language="javascript" type="text/javascript">
        $(function() {
            $("#<%=rbItemType.ClientID %>").each(function() {
                this.onclick = inputTypeChanged;
            });
            $("#<%=ddlValidateType.ClientID %>").each(function() {
                this.onchange = validateTypeChanged;
            });
            $("#<%=ddlItems.ClientID %>").each(function() {
                this.onchange = ddlItems_Changed;
            });
            $("#<%=txtFName.ClientID %>").change(function() {
                CheckItemName();
            });
            $("#<%=btnDeleteItem.ClientID %>").click(function() {
                if ($.trim($("#<%=ddlItems.ClientID %>")[0].value) == "")
                    return false;
                else return true;
            });
            $("input[@name=<%=rbItemType.UniqueID %>]").get(0).checked = true;
            $("input[@name=<%=rbItemDataType.UniqueID %>]").get(0).checked = true;

            for (var i = 0, icount = __selectionItems__.length; i < icount; i++) {
                bindSelectionItems(__selectionItems__[i].name, __selectionItems__[i].value);
            }
            $("#cbReadonlySet").click(function() {
                var readDis = $("#divReadonlyActivities")[0].style.display;
                if (readDis == "none") {
                    $("#divReadonlyActivities")[0].style.display = "";
                }
                else {
                    $("#divReadonlyActivities")[0].style.display = "none";
                }
                $("#<%=cblActivities4Read.ClientID %>").find("input").each(function() {
                    $(this)[0].checked = false;
                });
                $("#cbCheckAll4ReadonlySet").checked = false;
            });
            $("#cbCheckAll4ReadonlySet").click(function() {
                var flag = $("#cbCheckAll4ReadonlySet").attr("checked");
                $("#<%=cblActivities4Read.ClientID %>").find("input").each(function() {
                    $(this)[0].checked = flag;
                });
            });

            $("#cbShowSet").click(function() {
                var showDis = $("#divShowActivities")[0].style.display;
                if (showDis == "none") {
                    $("#divShowActivities")[0].style.display = "";
                }
                else {
                    $("#divShowActivities")[0].style.display = "none";
                }
                $("#<%=cblActivities4Show.ClientID %>").find("input").each(function() {
                    $(this)[0].checked = false;
                });
                $("#cbCheckAll4ShowSet").checked = false;
            });
            $("#cbCheckAll4ShowSet").click(function() {
                var flag = $("#cbCheckAll4ShowSet").attr("checked");
                $("#<%=cblActivities4Show.ClientID %>").find("input").each(function() {
                    $(this)[0].checked = flag;
                });
            });
            //输入向导提示
            $("span").each(function() {
                if ($(this).attr("hint") != undefined) {
                    var hint = $(this).attr("hint");
                    $(this).mouseover(function() {
                        showHint(this, hint, '<%=AppPath %>');
                    }).mouseout(function() {
                        hideHint();
                    });
                }
            });
        });
				
		//定义验证类型的数组.
		var dataType = new Array(6);
		for(var i=0; i<6; i++)
			dataType[i] = new Array(2);
			
		dataType[0][0] = "require";
		dataType[0][1] = "非空";
		dataType[1][0] = "compare";
		dataType[1][1] = "数值比较";
		dataType[2][0] = "match";
		dataType[2][1] = "内容匹配";
		dataType[3][0] = "custom";
		dataType[3][1] = "正则式";
		
		dataType[4][0] = "group";
		dataType[4][1] = "必选";
		dataType[5][0] = "date";
		dataType[5][1] = "日期验证";
		
		function validateTypeChanged() {
			var ddlSrc = $("#<%=ddlValidateType.ClientID %>");
			var ddlDest = document.getElementById("<%=ddlOp.ClientID%>");
			var elCompare = $("#trCompare");
			switch (ddlSrc[0].value) {
				case "compare":
					removeOptions(ddlDest, 0);
					addOption2Select(ddlDest, "等于", "Equal");
					addOption2Select(ddlDest, "不等于", "NotEqual");
					addOption2Select(ddlDest, "大于", "GreaterThan");
					addOption2Select(ddlDest, "大于等于", "GreaterThanEqual");
					addOption2Select(ddlDest, "小于", "LessThan");
					addOption2Select(ddlDest, "小于等于", "LessThanEqual");
					ddlDest.selectedIndex = 0;
					elCompare[0].style.display = "block";
					break;
				case "match":
					removeOptions(ddlDest, 0);
					addOption2Select(ddlDest, "包含", "contain");
					addOption2Select(ddlDest, "不包含", "notcontain");
					addOption2Select(ddlDest, "匹配开头", "beginwith");
					addOption2Select(ddlDest, "不匹配开头", "notbeginwith");
					addOption2Select(ddlDest, "匹配结尾", "endwith");
					addOption2Select(ddlDest, "不匹配结尾", "notendwith");
					ddlDest.selectedIndex = 0;
					elCompare[0].style.display = "block";
					break;
				case "custom":
					removeOptions(ddlDest, 0);
					addOption2Select(ddlDest, "匹配", "match");
					ddlDest.selectedIndex = 0;
					elCompare[0].style.display = "block";
					break;
				default:
					elCompare[0].style.display = "none";
					break;
			}
		}
		
		function removeOptions(ddl, startPos) {
			while (ddl.length > startPos) {
				ddl.options.remove(ddl.length - 1);
			}
		}
		function addOption2Select(ddl, oText, oValue) {
			var item = document.createElement("option");
			item.text = oText;
			item.value = oValue;
			ddl.options.add(item);
		}
		function initValidateType(i) 
		{
			var ddl = document.getElementById("<%=ddlValidateType.ClientID %>");
			for(var z=ddl.options.length;z>0;z--)
			{
				ddl.remove(z);
			}
			if(i == 1)
				for(var j=0;j<4;j++)
				{
					var no1 = new Option();
					no1.value = dataType[j][0];
					no1.text = dataType[j][1];
					ddl.options[ddl.options.length] = no1;
				}
			else
			{
				var no1 = new Option();
				no1.value = dataType[i][0];
				no1.text = dataType[i][1];
				ddl.options[ddl.options.length] = no1;
			}
		}
		function initDataType(i) {
		    $("input[@name=<%=rbItemDataType.UniqueID %>]").get(i).checked = true;
		}
		function initInputDisplay(Width,Height,DisplayV,NoneV)
		{
			var elWidth = $("#<%=txtWidth.ClientID %>");
			var elHeight = $("#<%=txtHeight.ClientID %>");
			elWidth[0].value = Width;
			elHeight[0].value = Height;
			DisplayV[0].style.display = "block";
			NoneV[0].style.display = "none";
		}
		function inputTypeChanged() {
			selectInputType(event.srcElement.value);
		}
		function selectInputType(theType) {
		    var elSelect = $("#trSelect");
		    var elPos = $("#trPos");
		    switch (theType) {
		        case "0":
		            initInputDisplay("", "", elPos, elSelect);
		            initValidateType(1);
		            initDataType(0);
		            break;
		        case "1":
		            initInputDisplay("80", "3", elPos, elSelect);
		            initValidateType(1);
		            initDataType(2);
		            break;
		        case "2":
		            initInputDisplay("", "", elSelect, elPos);
		            initValidateType(0);
		            initDataType(0);
		            break;
		        case "3":
		            initInputDisplay("", "", elPos, elSelect);
		            initValidateType(1);
		            initDataType(0);
		            break;
		        case "4":
		        case "5":
		            initInputDisplay("", "", elSelect, elPos);
		            initValidateType(4);
		            initDataType(0);
		            break;
		        case "6":
		            initInputDisplay("", "", elPos, elSelect);
		            initValidateType(5);
		            initDataType(0);
		            break;
		        case "7":
		            initDataType(3);
		            break;
		        case "9":
		            initDataType(2);
		            break;
		        default:
		            break;
		    }
		}
		//验证字段名是否已存在
	    function CheckItemName()
        {           
            var formid = $("#<%=hdFormDefinitionId.ClientID %>")[0].value;
            var formitemname = $("#<%=txtFName.ClientID %>")[0].value;
            $.get("async/CheckItemName.aspx",{fid:formid,fname:formitemname},fnameback);
        }
        function fnameback(result)
        {
            if (result == "true") {
                $("#spFNameMsg").html($("#<%=txtFName.ClientID %>")[0].value+"字段已存在!");
                $("#<%=txtFName.ClientID %>")[0].value = "";
                $("#<%=txtFName.ClientID %>")[0].focus();
                }
            else $("#spFNameMsg").html("");
        }
        
        function ddlItems_Changed() {
			if (this.value != "") {
				var itemid = $("select[@name='<%=ddlItems.UniqueID %>'] option[@selected]").val(); 				
				$.get("async/GetItemDefintionById.aspx",{id:itemid},BindItems);
			}	
			
			 $("#<%=cblActivities4Read.ClientID %>").find("input").each(function(){
                   $(this)[0].checked = false;
                });
            $("#<%=cblActivities4Show.ClientID %>").find("input").each(function(){
                   $(this)[0].checked = false;
                });		
            $("#cbCheckAll4ShowSet").checked = false;
            $("#cbCheckAll4ReadonlySet").checked = false;
		}
		
		function BindItems(xml) {
			var item = xml.getElementsByTagName("Table")[0];
			var FName = item.getElementsByTagName("FName")[0].firstChild.data;
			var Name = item.getElementsByTagName("Name")[0].firstChild.data;
			var itemC = item.getElementsByTagName("Comment")[0].firstChild;
			var Comment = itemC!=null?itemC.data:"";
			var Left = parseInt(item.getElementsByTagName("Left")[0].firstChild.data);
			var Top = parseInt(item.getElementsByTagName("Top")[0].firstChild.data);
			var Width = item.getElementsByTagName("Width")[0].firstChild.data;
			var Height = item.getElementsByTagName("Height")[0].firstChild.data;
			var ItemType = item.getElementsByTagName("ItemType")[0].firstChild.data;
			var ItemDataType = item.getElementsByTagName("ItemDataType")[0].firstChild.data;
	
			$("input[name='<%= rbItemType.UniqueID%>']").each(function(){
			    if(this.value == ItemType)
			        this.checked = true;
			});
			selectInputType(ItemType);

			$("input[name='<%= rbItemDataType.UniqueID%>']").each(function(){
			    if(this.value == ItemDataType)
			        this.checked = true;
			});
			
			$("#<%= txtFName.ClientID%>")[0].value = FName;
			$("#<%= txtName.ClientID%>")[0].value = Name;
			$("#<%= txtComment.ClientID%>")[0].value = Comment;
			
			if (Left != 0 && Top != 0) {				
				$("#<%=txtLeft.ClientID%>")[0].value = Left;
				$("#<%=txtTop.ClientID%>")[0].value = Top;
			} else {
				$("#<%=txtLeft.ClientID%>")[0].value = "";
				$("#<%=txtTop.ClientID%>")[0].value = "";
			}		
			
			if (Width != 0) {
				$("#<%=txtWidth.ClientID%>")[0].value = Width;
			} else {
				$("#<%=txtWidth.ClientID%>")[0].value = "";
			}
			
			if (Height != 0) {
				$("#<%=txtHeight.ClientID%>")[0].value = Height;
			} else {
				$("#<%=txtHeight.ClientID%>")[0].value = "";
			}						
			if (null != item.getElementsByTagName("DataSource")[0].firstChild) {
				var DataSource = item.getElementsByTagName("DataSource")[0].firstChild.data;
				$("#<%=txtDataSource.ClientID%>")[0].value = DataSource;			
			} else {
				$("#<%=txtDataSource.ClientID%>")[0].value = "";	
			}
			
			if (null != item.getElementsByTagName("DefaultValue")[0] 
				&& null != item.getElementsByTagName("DefaultValue")[0].firstChild) {
				var DefalueValue = item.getElementsByTagName("DefaultValue")[0].firstChild.data;
				$("#<%=txtDefaultValue.ClientID%>")[0].value = DefalueValue;		
			} else {
				$("#<%=txtDefaultValue.ClientID%>")[0].value = "";	
			}			
			
			if (null != item.getElementsByTagName("Require")[0].firstChild) {
				var Require = item.getElementsByTagName("Require")[0].firstChild.data.toLowerCase();
				if (Require == "true") {
					$("#<%=chkRequire.ClientID %>").attr("checked",true);			
				} else {
					$("#<%=chkRequire.ClientID %>").attr("checked","");	
				}				
			}			
			
			if (null != item.getElementsByTagName("ValidateType")[0].firstChild) {
				var ValidateType = item.getElementsByTagName("ValidateType")[0].firstChild.data;
		        $("#<%=ddlValidateType.ClientID %>")[0].value=ValidateType;
			}		
			
			validateTypeChanged();
			
			if (null != item.getElementsByTagName("Op")[0].firstChild) {
				var Operator = item.getElementsByTagName("Op")[0].firstChild.data;
				$("#<%=ddlOp.ClientID %>")[0].value=Operator;
			}		
	
			if (null != item.getElementsByTagName("MaxVal")[0].firstChild) {
				var MaxVal = item.getElementsByTagName("MaxVal")[0].firstChild.data;
				$("#<%=txtMaxVal.ClientID%>")[0].value = MaxVal;
			} else {
				$("#<%=txtMaxVal.ClientID%>")[0].value = "";
			}
			
			if (null != item.getElementsByTagName("MinVal")[0].firstChild) {
				var MinVal = item.getElementsByTagName("MinVal")[0].firstChild.data;
				$("#<%=txtMinVal.ClientID%>")[0].value = MinVal;
			} else {
				$("#<%=txtMinVal.ClientID%>")[0].value = "";
			}
			
			if (null != item.getElementsByTagName("OpTarget")[0].firstChild) {
				var Target = item.getElementsByTagName("OpTarget")[0].firstChild.data;
				$("#<%=txtOpTarget.ClientID%>")[0].value = Target;
			} else {
				$("#<%=txtOpTarget.ClientID%>")[0].value = "";
			}
			
			if (null != item.getElementsByTagName("ErrorMessage")[0].firstChild) {
				var ErrorMessage = item.getElementsByTagName("ErrorMessage")[0].firstChild.data;
				$("#<%=txtErrorMessage.ClientID%>")[0].value = ErrorMessage;
			} else {
				$("#<%=txtErrorMessage.ClientID%>")[0].value = "";
			}	
			
			if (null != item.getElementsByTagName("RowExclusive")[0].firstChild) {
				var EngrossRow = item.getElementsByTagName("RowExclusive")[0].firstChild.data.toLowerCase();
				if (EngrossRow == "true") {
					$("#<%=chkRowExclusive.ClientID %>").attr("checked","checked");			
				} else {
					$("#<%=chkRowExclusive.ClientID %>").attr("checked","");			
				}
			}	
			
			if(null != item.getElementsByTagName("ShowSet")[0].firstChild){
			    var showSet = item.getElementsByTagName("ShowSet")[0].firstChild.data;
			    var arr = showSet.split("|");
			    for(var i=0;i<arr.length - 1;i++){
			         $("#<%=cblActivities4Show.ClientID %>").find("input").each(function(){
                         if($(this)[0].nextSibling.firstChild.data == arr[i]){                           
                            $(this)[0].checked=true;
                        }
                });
                 $("#cbShowSet")[0].checked=true;           
                 $("#divShowActivities")[0].style.display = "";  
			    }
			}
			else{
			    $("#cbShowSet")[0].checked=false;           
                $("#divShowActivities")[0].style.display = "none";  
             }
             
			if(null != item.getElementsByTagName("ReadonlySet")[0].firstChild){
			    var ReadonlySet = item.getElementsByTagName("ReadonlySet")[0].firstChild.data;
			    var arr = ReadonlySet.split("|");
			    for(var i=0;i<arr.length - 1;i++){
			         $("#<%=cblActivities4Read.ClientID %>").find("input").each(function(){
                         if($(this)[0].nextSibling.firstChild.data == arr[i]){                           
                            $(this)[0].checked=true;
                        }
                });
                 $("#cbReadonlySet")[0].checked=true;           
                 $("#divReadonlyActivities")[0].style.display = "";  
			    }
			}
			else{
			    $("#cbReadonlySet")[0].checked=false;           
                $("#divReadonlyActivities")[0].style.display = "none";  
             }
         }

         //弹出对话框选择内容
         function OpenSelectionDialog() {
             var arr = new Array();
             var url = "config/AdvancedConfig.aspx?fdid=<%=FormDefinitionId %>&wfid=<%=WorkflowId %>&EntityType=<%=EntityType%>&fid=" + $("#<%=ddlItems.ClientID %>").val() + "&t=" + Math.random();
             //alert(url);
             if ($("#<%=ddlItems.ClientID %>").val().length == 0) {
                 alert("请选择字段");
                 return false;
             }
             //window.showModalDialog(url, arr, "dialogHeight:600px;dialogWidth:800px;status:no;resizable:no;scroll:y;");
             //return true;
             window.open(url, "x", "height=600px,width=800px,toolbar=no,menubar=no,scrollbars=yes, resizable=yes,location=no, status=n, oresizable=no;");
         }
    </script>
</asp:Content>
