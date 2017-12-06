<%@ Page Language="C#" AutoEventWireup="true" Inherits="apps_xqp2_pages_workflows_designer_Flowdesign_ModalDialog_attrPower" Codebehind="attrPower.aspx.cs" %>

<div class="tab-pane active" id="attrPower">
                    <div class="control-group">
                        <label class="control-label">
                            自动选人</label>
                        <div class="controls">
                            <select name="auto_person" id="auto_person_id">
                                <option value="0">不自动选人</option>
                                <option value="1">发起人</option>
                                <option value="2">发起人的部门主管</option>
                                <option value="3">处理人的部门主管</option>
                                <option value="5">指定角色</option>
                                <option value="4">指定人员</option>
                            </select>
                            <span class="help-inline">预先设置自动选人，更方便转交工作</span>
                        </div>
                        <div class="controls hide" id="auto_unlock_id">
                            <label class="checkbox">
                                <input type="checkbox" name="auto_unlock" value="1" checked="checked">允许更改
                            </label>
                        </div>
                        <div id="auto_person_4" class="hide">
                            <div class="control-group">
                                <label class="control-label">
                                    指定主办人</label>
                                <div class="controls">
                                    <input type="hidden" name="auto_sponsor_ids" id="auto_sponsor_ids" value="">
                                    <input class="input-xlarge" readonly="readonly" type="text" placeholder="指定主办人" name="auto_sponsor_text"
                                        id="auto_user_text" value="">
                                    <a href="javascript:void(0);" class="btn" onclick="superDialog('/demo/super_dialog/op/user.html','auto_user_text','auto_sponsor_ids');">
                                        选择</a>
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label">
                                    指定经办人</label>
                                <div class="controls">
                                    <input type="hidden" name="auto_respon_ids" id="auto_respon_ids" value="">
                                    <input class="input-xlarge" readonly="readonly" type="text" placeholder="指定经办人" name="auto_respon_text"
                                        id="auto_userop_text" value="">
                                    <a href="javascript:void(0);" class="btn" onclick="superDialog('/demo/super_dialog/op/user.html','auto_userop_text','auto_respon_ids');">
                                        选择</a>
                                </div>
                            </div>
                        </div>
                        <div id="auto_person_5" class="hide">
                            <div class="control-group">
                                <label class="control-label">
                                    指定角色</label>
                                <div class="controls">
                                    <input type="hidden" name="auto_role_ids" id="auto_role_value" value="">
                                    <input class="input-xlarge" readonly="readonly" type="text" placeholder="指定角色" name="auto_role_text"
                                        id="auto_role_text" value="">
                                    <a href="javascript:void(0);" class="btn" onclick="superDialog('/demo/super_dialog/op/role.html','auto_role_text','auto_role_value');">
                                        选择</a>
                                </div>
                            </div>
                        </div>
                    </div>
                    <hr>
                    <h4>
                        授权范围</h4>
                    <div class="control-group">
                        <label class="control-label">
                            授权人员</label>
                        <div class="controls">
                            <input type="hidden" name="range_user_ids" id="range_user_ids" value="">
                            <input class="input-xlarge" readonly="readonly" type="text" placeholder="选择人员" name="range_user_text"
                                id="range_user_text" value="">
                            <a href="javascript:void(0);" class="btn" onclick="superDialog('/demo/super_dialog/op/user.html','range_user_text','range_user_ids');">
                                选择</a>
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label">
                            授权部门</label>
                        <div class="controls">
                            <input type="hidden" name="range_dept_ids" id="range_dept_ids" value="">
                            <input class="input-xlarge" readonly="readonly" type="text" placeholder="选择部门" name="range_dept_text"
                                id="range_dept_text" value="">
                            <a href="javascript:void(0);" class="btn" onclick="superDialog('/demo/super_dialog/op/dept.html','range_dept_text','range_dept_ids');">
                                选择</a>
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label">
                            授权角色</label>
                        <div class="controls">
                            <input type="hidden" name="range_role_ids" id="range_role_ids" value="">
                            <input class="input-xlarge" readonly="readonly" type="text" placeholder="选择角色" name="range_role_text"
                                id="range_role_text" value="">
                            <a href="javascript:void(0);" class="btn" onclick="superDialog('/demo/super_dialog/op/role.html','range_role_text','range_role_ids');">
                                选择</a>
                        </div>
                    </div>
                    <div class="control-group">
                        <div class="controls">
                            <span class="help-block">当需要手动选人时，则授权范围生效</span>
                        </div>
                    </div>
                </div>
                <!-- attrPower end -->
