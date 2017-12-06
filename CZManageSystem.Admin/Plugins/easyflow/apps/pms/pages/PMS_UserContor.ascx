<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="apps_pms_pages_PMS_UserContor" Codebehind="PMS_UserContor.ascx.cs" %>
<script type="text/javascript">
    $(document).ready(function () {
        $("#btnclear").click(function () {
            //            $("#ctl00_cphBody_con_Pms_txt_StartData").val("");
            //            $("#ctl00_cphBody_con_Pms_txt_EndData").val("");
            //            $("#ctl00_cphBody_con_Pms_select_Brnd").val("");
            //            $("#ctl00_cphBody_con_Pms_select_Area").val("");
            //            $("#ctl00_cphBody_con_Pms_select_city").val("");
            //            $("#ctl00_cphBody_con_Pms_select_Brnd2").val("");
            //            $("#ctl00_cphBody_con_Pms_select_Area2").val("");
            //            $("#ctl00_cphBody_con_Pms_select_city2").val("");
            //            $("#ctl00_cphBody_con_Pms_txt_Code").val("");
            //            $("#ctl00_cphBody_con_Pms_txt_Name").val("");
            //            $("#ctl00_cphBody_con_Pms_txt_StartData2").val("");
            //            $("#ctl00_cphBody_con_Pms_txt_EndData2").val("");
            //            $("#ctl00_cphBody_con_Pms_select_Num").val("");

            var inputText = $(".toolBlock input[type=text]");
            var inputHidden = $(".toolBlock input[type=hidden]");
            var inputHight = $(".div_high_serch input[type=text]");
            if (inputText.length > 0) { inputText.val(""); }
            if (inputHidden.length > 0) { inputHidden.val(""); }
            if (inputHight.length > 0) { inputHight.val(""); }
        });
    }); 
</script>
<div class="toolBlock" style="text-align: left; background-color: #DDEEFF; margin-left: 5px">
    <span style="margin-right: 5px" runat="server">
        <table width="99%">
            <tr>
                <td colspan="2">
                    <span id="span_1" runat="server"><span id="span_2" runat="server">
                        <panel id="plan_Data" runat="server"> 
            时间:<input type="text" runat="server" style="width:70px" onclick="WdatePicker({dateFmt:'yyyy-MM'});" id="txt_StartData" /><span id="endTiem" runat="server"> 至<input runat="server" type="text" style="width:70px"  id="txt_EndData" onclick="WdatePicker({dateFmt:'yyyy-MM'});"/></span>
          </panel>
                        <input style="width: 70px;" readonly="readonly" runat="server" type="hidden" id="select_Brnd" />
                        <input  style="width: 70px;" readonly="readonly" runat="server" type="hidden" id="select_Area" />
                            <input style="width: 70px;" readonly="readonly" runat="server" type="hidden" id="select_city" />
                        <span id="plan_Data2" runat="server" visible="false">时间:<input type="text" runat="server"
                            style="width: 70px" onclick="WdatePicker({dateFmt:'yyyy-MM-dd'});" id="txt_StartData2" />至<input
                                runat="server" type="text" style="width: 70px" id="txt_EndData2" onclick="WdatePicker({dateFmt:'yyyy-MM-dd'});" />
                        </span><span id="span_b_a_d" runat="server"><span id="pan_Brnd" runat="server">品牌:<input
                            style="width: 70px;" readonly="readonly" runat="server" type="text" id="select_Brnd2" /><input
                                type="button" style="cursor: pointer; background: url(../../../res/img/btnse01.jpg);
                                border-style: none; height: 21px; width: 19px; background-repeat: no-repeat;"
                                onclick="showDiv({tableName2:'UCS_Brand_Name',text:'name',value:'id',ishide:'1'},'GetArea.aspx',{hide:'<%=select_Brnd.ClientID%>',text:'<%=select_Brnd2.ClientID%>'})" />
                        </span><span id="pan_Area" runat="server">分公司:<input style="width: 70px;" readonly="readonly"
                            runat="server" type="text" id="select_Area2" /><input type="button" style="cursor: pointer;
                                background: url(../../../res/img/btnse01.jpg); border-style: none; height: 21px;
                                width: 19px; background-repeat: no-repeat;" onclick="showDiv({tableName2:'Ucs_Area_name',text:'name',value:'id',ishide:'1'},'GetArea.aspx',{hide:'<%=select_Area.ClientID%>',text:'<%=select_Area2.ClientID%>'})" />
                            <span id="pan_City" runat="server">区域:<input style="width: 70px;" readonly="readonly"
                                runat="server" type="text" id="select_city2" /><input type="button" style="cursor: pointer;
                                    background: url(../../../res/img/btnse01.jpg); border-style: none; height: 21px;
                                    width: 19px; background-repeat: no-repeat;" onclick="showDiv({tableName2:'UCS_DISCD',text:'name',value:'id',ishide:'1'},'GetArea.aspx',{hide:'<%=select_city.ClientID%>',text:'<%=select_city2.ClientID%>'})" />
                            </span></span></span><span id="span_b_a_d2" runat="server" visible="false"><span
                                id="pan_Brnd2" runat="server">品牌:<input style="width: 70px;" readonly="readonly"
                                    runat="server" type="text" id="Brnd2" /><input type="button" style="cursor: pointer;
                                        background: url(../../../res/img/btnse01.jpg); border-style: none; height: 21px;
                                        width: 19px; background-repeat: no-repeat;" onclick="showDiv({tableName2:'UCS_Brand_Name',text:'name',value:'name',ishide:'1'},'GetArea.aspx','<%=Brnd2.ClientID%>')" />
                            </span><span id="pan_Area2" runat="server">分公司:<input style="width: 70px;" readonly="readonly"
                                runat="server" type="text" id="Area2" /><input type="button" style="cursor: pointer;
                                    background: url(../../../res/img/btnse01.jpg); border-style: none; height: 21px;
                                    width: 19px; background-repeat: no-repeat;" onclick="showDiv({tableName2:'Ucs_Area_name',text:'name',value:'name',ishide:'1'},'GetArea.aspx','<%=Area2.ClientID%>')" />
                            </span></span></span>
                            <span id="sp_show_call" runat="server"  visible="false">
                            归属地:
                            <input style="width: 70px;" readonly="readonly"
                            runat="server" type="text" id="Text1" /><input type="button" style="cursor: pointer;
                                background: url(../../../res/img/btnse01.jpg); border-style: none; height: 21px;
                                width: 19px; background-repeat: no-repeat;" onclick="showDiv({tableName2:'Ucs_Area_name',text:'name',value:'id',ishide:'1'},'GetArea.aspx',{hide:'<%=select_Area.ClientID%>',text:'<%=Text1.ClientID%>'})" />
                                品牌:<input style="width: 70px;" readonly="readonly" name="BRND_NAM" value="<%=Request.Form["BRND_NAM"] %>"
                                    type="text" id="BRND_NAM" /><input type="button" style="cursor: pointer;
                                        background: url(../../../res/img/btnse01.jpg); border-style: none; height: 21px;
                                        width: 19px; background-repeat: no-repeat;" onclick="showDiv({tableName2:'UCS_Brand_Name',text:'name',value:'name',ishide:'1'},'GetArea.aspx','BRND_NAM')" />
                            </span>
                            </span>
                </td>
            </tr>
            <tr>
                <td><span id="sp_show_call2" runat="server" visible="false">
                号码:<input style="width: 70px;"
                        readonly="readonly" name="ORDER_NBR" value="<%=Request.Form["ORDER_NBR"] %>" type="text" id="ORDER_NBR" /><input type="button"
                            style="cursor: pointer; background: url(../../../res/img/btnse01.jpg); border-style: none;
                            height: 21px; width: 19px; background-repeat: no-repeat;" onclick="showDiv({text:'ORDER_NBR',value:'ORDER_NBR'},'GetMarkData.aspx','ORDER_NBR')" />
               <span id="Span1">产品编码:</span>
                        <input style="width: 70px;" readonly="readonly" type="text" value="<%=Request.Form["BASS_PRDCT_CD"] %>" id="BASS_PRDCT_CD" name="BASS_PRDCT_CD" /><input
                            type="button" style="cursor: pointer; background: url(../../../res/img/btnse01.jpg);
                            border-style: none; height: 21px; width: 19px; background-repeat: no-repeat;"
                            onclick="showDiv({text:$(this).attr('field')},'GetMarkData.aspx','BASS_PRDCT_CD')"
                            field="BASS_PRDCT_CD" />
                            产品名称:<input runat="server" style="width: 70px"
                          type="text" id="BASS_PRDCT_NAM" name="BASS_PRDCT_NAM" /><input type="button" style="cursor: pointer;
                                background: url(../../../res/img/btnse01.jpg); border-style: none; height: 21px;
                                width: 19px; background-repeat: no-repeat;" onclick="showDiv({text:$(this).attr('field')},'GetMarkData.aspx','BASS_PRDCT_NAM')"
                                field="BASS_PRDCT_NAM" />

                </span>
                    <span id="pan_Num" runat="server" visible="false">号码:<input style="width: 70px;"
                        readonly="readonly" runat="server" type="text" id="select_Num" /><input type="button"
                            style="cursor: pointer; background: url(../../../res/img/btnse01.jpg); border-style: none;
                            height: 21px; width: 19px; background-repeat: no-repeat;" onclick="showDiv({text:'USR_NBR',value:'USR_NBR'},'GetMarkData.aspx','<%=select_Num.ClientID%>')" />
                    </span><span runat="server" id="pan_CD_NAM"><span id="pan_nam_text">营销方案编码:</span>
                        <input style="width: 70px;" readonly="readonly" runat="server" type="text" id="txt_Code" /><input
                            type="button" style="cursor: pointer; background: url(../../../res/img/btnse01.jpg);
                            border-style: none; height: 21px; width: 19px; background-repeat: no-repeat;"
                            onclick="showDiv({text:$(this).attr('field')},'GetMarkData.aspx','<%=txt_Code.ClientID%>')"
                            field="MARK_PLAN_CD" />
                        <span runat="server" id="pan_nam">营销方案名称:<input runat="server" style="width: 70px"
                            readonly="readonly" type="text" id="txt_Name" /><input type="button" style="cursor: pointer;
                                background: url(../../../res/img/btnse01.jpg); border-style: none; height: 21px;
                                width: 19px; background-repeat: no-repeat;" onclick="showDiv({text:$(this).attr('field')},'GetMarkData.aspx','<%=txt_Name.ClientID%>')"
                                field="MARK_PLAN_NAM" /></span> </span><span id="span_onlin" runat="server" visible="false">
                                    充值渠道编码:<input runat="server" style="width: 70px" readonly="readonly" type="text"
                                        id="txt_Chnl_cd" /><input type="button" style="cursor: pointer; background: url(../../../res/img/btnse01.jpg);
                                            border-style: none; height: 21px; width: 19px; background-repeat: no-repeat;"
                                            onclick="showDiv({text:$(this).attr('field')},'GetMarkData.aspx','<%=txt_Chnl_cd.ClientID%>')"
                                            field="Chnl_cd" />
                                    充值渠道名称:<input runat="server" style="width: 70px" readonly="readonly" type="text"
                                        id="txt_Chnl_nam" /><input type="button" style="cursor: pointer; background: url(../../../res/img/btnse01.jpg);
                                            border-style: none; height: 21px; width: 19px; background-repeat: no-repeat;"
                                            onclick="showDiv({text:$(this).attr('field')},'GetMarkData.aspx','<%=txt_Chnl_nam.ClientID%>')"
                                            field="Chnl_nam" />
                                </span><span id="span_thres" runat="server" visible="false">预警情况：<select runat="server"
                                    id="thres"><option value="">全部</option>
                                    <option value="1">是</option>
                                    <option value="0">否</option>
                                </select>
                                </span><span id="span_thres2" runat="server" visible="false">预警情况：
                                    <select name="thres_st">
                                        <option value="">全部</option>
                                        <option value="1">预警</option>
                                        <option value="2">未预警</option>
                                    </select>
                                </span><span id="span_ebox_typ" runat="server" visible="false">账户类型：
                                    <input runat="server" style="width: 70px" readonly="readonly" type="text" id="ebox_typ" /><input
                                        type="button" style="cursor: pointer; background: url(../../../res/img/btnse01.jpg);
                                        border-style: none; height: 21px; width: 19px; background-repeat: no-repeat;"
                                        onclick="showDiv({text:$(this).attr('field'),tableName:'biee_chrg_log_day'},'GetMarkData.aspx','<%=ebox_typ.ClientID%>')"
                                        field="ebox_typ" />
                                </span><span id="span_bitchname" runat="server" visible="false">批次：
                                    <input runat="server" style="width: 70px" readonly="readonly" type="text" id="bitchname" /><input
                                        type="button" style="cursor: pointer; background: url(../../../res/img/btnse01.jpg);
                                        border-style: none; height: 21px; width: 19px; background-repeat: no-repeat;"
                                        onclick="showDiv({text:$(this).attr('field'),tableName:'RESOURCEC_CONSUME_VIEW'},'GetMarkData.aspx','<%=bitchname.ClientID%>')"
                                        field="batchname" /></span>
                </td>
                <td style="text-align: right">
                    <input id="btn_high_Seach" type="button" class="btn_query" value="高级搜索" style="display: none"
                        onclick="high_Seach()" />
                    <asp:Button ID="btn_Seach" CssClass="btn_query" runat="server" Text="搜索" OnClientClick="putin()"
                        OnClick="btn_Seach_Click"></asp:Button>
                    <input id="btnclear" type="button" class="btn_query" value="清空条件" />
                    <input id="btn_goback" type="button" class="btn_query" value="返回" onclick="history.back();" />
                </td>
            </tr>
        </table>
    </span>
</div>
