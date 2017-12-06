<%@ Page Language="C#" AutoEventWireup="true" Inherits="apps_xqp2_pages_workflows_designer_Flowdesign_ModalDialog_attrJudge" Codebehind="attrJudge.aspx.cs" %>

<div class="tab-pane  active" id="attrJudge">
    <table class="table">
        <thead>
            <tr>
                <th style="width: 100px;">
                    转出步骤
                </th>
                <th>
                    转出条件设置
                </th>
            </tr>
        </thead>
        <tbody>
            <!--模板-->
            <tr id="tpl" class="hide">
                <td style="width: 100px;">
                    @text
                </td>
                <td>
                    <table class="table table-condensed">
                        <tbody>
                            <tr>
                                <td>
                                    <select id="field_@a" class="input-medium">
                                        <option value="">选择字段</option>
                                        <!-- 表单字段 start -->
                                        <option value="checkboxs_0">是</option>
                                        <!-- 表单字段 end -->
                                    </select>
                                    <select id="condition_@a" class="input-small">
                                        <%--<option value="=">等于</option>
        <option value="<>">不等于</option>
        <option value=">">大于</option>
        <option value="<">小于</option>
        <option value=">=">大于等于</option>
        <option value="<=">小于等于</option>
        <option value="include">包含</option>
        <option value="exclude">不包含</option>--%>
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
                                    <input type="text" id="item_value_@a" class="input-small">
                                    <select id="relation_@a" class="input-small">
                                        <option value="AND">与</option>
                                        <option value="OR">或者</option>
                                    </select>
                                </td>
                                <td>
                                    <div class="btn-group">
                                        <button type="button" class="btn btn-small" onclick="fnAddLeftParenthesis('@a')">
                                            （</button>
                                        <button type="button" class="btn btn-small" onclick="fnAddRightParenthesis('@a')">
                                            ）</button>
                                        <button type="button" onclick="fnAddConditions('@a')" class="btn btn-small">
                                            新增</button>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <select id="conList_@a" multiple="" style="width: 100%; height: 80px;">
                                    </select>
                                </td>
                                <td>
                                    <div class="btn-group">
                                        <button type="button" onclick="fnDelCon('@a')" class="btn btn-small">
                                            删行</button>
                                        <button type="button" onclick="fnClearCon('@a')" class="btn btn-small">
                                            清空</button>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input id="process_in_desc_@a" type="text" name="process_in_desc_@a" style="width: 98%;">
                                    <input name="process_in_set_@a" id="process_in_set_@a" type="hidden">
                                </td>
                                <td>
                                    <span class="xc1">不符合条件时的提示</span>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </tbody>
        <tbody id="ctbody">
            <tr>
                <td style="width: 100px;">
                    <span class="badge badge-inverse">298</span><br>
                    新建步骤
                </td>
                <td>
                    <table class="table table-condensed">
                        <tbody>
                            <tr>
                                <td>
                                    <select id="field_298" class="input-medium">
                                        <option value="">选择字段</option>
                                        <!-- 表单字段 start -->
                                        <option value="checkboxs_0">是</option>
                                        <!-- 表单字段 end -->
                                    </select>
                                    <select id="condition_298" class="input-small">
                                        <%--<option value="=">等于</option>
        <option value="<>">不等于</option>
        <option value=">">大于</option>
        <option value="<">小于</option>
        <option value=">=">大于等于</option>
        <option value="<=">小于等于</option>
        <option value="include">包含</option>
        <option value="exclude">不包含</option>--%>
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
                                    <input type="text" id="item_value_298" class="input-small">
                                    <select id="relation_298" class="input-small">
                                        <option value="AND">与</option>
                                        <option value="OR">或者</option>
                                    </select>
                                </td>
                                <td>
                                    <div class="btn-group">
                                        <button type="button" class="btn btn-small" onclick="fnAddLeftParenthesis('298')">
                                            （</button>
                                        <button type="button" class="btn btn-small" onclick="fnAddRightParenthesis('298')">
                                            ）</button>
                                        <button type="button" onclick="fnAddConditions('298')" class="btn btn-small">
                                            新增</button>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <select id="conList_298" multiple="" style="width: 100%; height: 80px;">
                                    </select>
                                </td>
                                <td>
                                    <div class="btn-group">
                                        <button type="button" onclick="fnDelCon('298')" class="btn btn-small">
                                            删行</button>
                                        <button type="button" onclick="fnClearCon('298')" class="btn btn-small">
                                            清空</button>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input id="process_in_desc_298" type="text" name="process_in_desc_298" style="width: 98%;">
                                    <input name="process_in_set_298" id="process_in_set_298" type="hidden">
                                </td>
                                <td>
                                    <span class="xc1">不符合条件时的提示</span>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
            <tr>
            </tr>
        </tbody>
    </table>
    <input type="hidden" name="process_condition" id="process_condition" value="298,">
</div>
<!-- attrJudge end -->
