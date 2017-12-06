<%@ Page Language="C#" AutoEventWireup="true" Inherits="apps_xqp2_pages_workflows_designer_Flowdesign_ModalDialog_attrOperate" Codebehind="attrOperate.aspx.cs" %>

 <div class="tab-pane active" id="attrOperate">
                    <div class="control-group">
                        <label class="control-label">
                            交接方式</label>
                        <div class="controls">
                            <select name="receive_type">
                                <option value="0" selected="selected">明确指定主办人</option>
                                <option value="1">先接收为主办人</option>
                            </select>
                        </div>
                    </div>
                    <div class="control-group">
                        <div class="controls">
                            <label class="checkbox">
                                <input type="checkbox" name="is_user_end" value="1">允许主办人办结流程(最后步骤默认允许)
                            </label>
                            <label class="checkbox">
                                <input type="checkbox" name="is_userop_pass" value="1">经办人可以转交下一步
                            </label>
                        </div>
                    </div>
                    <hr>
                    <div class="control-group">
                        <label class="control-label">
                            会签方式</label>
                        <div class="controls">
                            <select name="is_sing">
                                <option value="1" selected="selected">允许会签</option>
                                <option value="2">禁止会签</option>
                                <option value="3">强制会签</option>
                            </select>
                            <span class="help-inline">如果设置强制会签，则本步骤全部人都会签后才能转交或办结</span>
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label">
                            可见性</label>
                        <div class="controls">
                            <select name="sign_look">
                                <option value="1" selected="selected">总是可见</option>
                                <option value="2">本步骤之间经办人不可见</option>
                                <option value="3">其它步骤不可见</option>
                            </select>
                        </div>
                    </div>
                    <hr>
                    <div class="control-group">
                        <label class="control-label">
                            回退方式</label>
                        <div class="controls">
                            <select name="is_back">
                                <option value="1" selected="selected">不允许</option>
                                <option value="2">允许回退上一步</option>
                                <option value="3">允许回退之前步骤</option>
                            </select>
                        </div>
                    </div>
                </div>
                <!-- attrOperate end -->
