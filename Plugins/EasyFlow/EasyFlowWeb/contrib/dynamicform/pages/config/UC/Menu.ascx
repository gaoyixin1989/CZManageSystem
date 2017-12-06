<%@ Control Language="C#" AutoEventWireup="true" Inherits="contrib_dynamicform_pages_config_UC_Menu" Codebehind="Menu.ascx.cs" %>
<ul>
<li class="c">
<a href="javascript:void(0)" onclick="toUrl(this,'UC/GetOuterData.aspx?fdid=<%=FormDefinitionId %>&wfid=<%=WorkflowId %>&EntityType=<%=EntityType%>&fid=<%=FormItemDefinitionId %>&t=' + Math.random())" target="_self">数据获取</a>
</li>
<li>
<a href="javascript:void(0)" onclick="toUrl(this,'UC/FormItemLinkage.aspx?fdid=<%=FormDefinitionId %>&wfid=<%=WorkflowId %>&EntityType=<%=EntityType%>&fid=<%=FormItemDefinitionId %>&t=' + Math.random())" target="_self">字段联动</a>
</li>
<li>
<a href="javascript:void(0)" onclick="toUrl(this,'UC/ValidateDataType.aspx?fdid=<%=FormDefinitionId %>&wfid=<%=WorkflowId %>&EntityType=<%=EntityType%>&fid=<%=FormItemDefinitionId %>&t=' + Math.random())" target="_self">格式校验</a>
</li>
<li>
<a href="javascript:void(0)" onclick="toUrl(this,'UC/FormItemsRules.aspx?fdid=<%=FormDefinitionId %>&wfid=<%=WorkflowId %>&EntityType=<%=EntityType%>&fid=<%=FormItemDefinitionId %>&t=' + Math.random())" target="_self">字段规则</a>
</li>
<li>
<a href="javascript:void(0)" onclick="toUrl(this,'UC/FormItemIFrames.aspx?fdid=<%=FormDefinitionId %>&wfid=<%=WorkflowId %>&EntityType=<%=EntityType%>&fid=<%=FormItemDefinitionId %>&t=' + Math.random())" target="_self">IFrame嵌入</a>
</li>
<li>
<a href="javascript:void(0)" onclick="toUrl(this,'UC/FormItemDataList.aspx?fdid=<%=FormDefinitionId %>&wfid=<%=WorkflowId %>&EntityType=<%=EntityType%>&fid=<%=FormItemDefinitionId %>&t=' + Math.random())" target="_self">DataList设置</a>
</li>
</ul>

<script type="text/javascript" language="javascript">
    function toUrl(obj,url) {
        document.getElementById("IframeEF").src = url;
        $("ul li").click(function() {
            $("ul li").not($(this)[0]).each(function() {
                $(this).removeAttr("class", "c");
            });
            $(this).attr("class", "c");
        });
    }
</script>
