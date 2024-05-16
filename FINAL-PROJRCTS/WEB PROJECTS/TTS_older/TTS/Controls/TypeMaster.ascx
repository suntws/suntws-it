<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TypeMaster.ascx.cs"
    Inherits="TTS.Controls.TypeMaster" %>
<div style="width: 1080px;">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div align="center" class="pageTitleHead">
        Type Master
    </div>
</div>
<div>
    <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
        Font-Size="20px" ForeColor="Red"></asp:Label>
</div>
<div id="displaycontent" style="width: 1080px;">
    <div align="left">
        <table>
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"
                        Font-Bold="true"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView runat="server" ID="gvTypeDetails" AutoGenerateColumns="false" AllowPaging="true"
                        OnPageIndexChanging="gvTypeDetails_PageIndex" PageSize="12" AlternatingRowStyle-BackColor="#f5f5f5"
                        OnRowEditing="gvTypeDetails_RowEditing" OnRowUpdating="gvTypeDetails_RowUpdating"
                        OnRowCancelingEdit="gvTypeDetails_RowCanceling" OnRowDataBound="gvTypeDetails_RowDataBound"
                        PagerStyle-Height="30px" PagerStyle-Font-Bold="true" PagerStyle-Font-Size="15px"
                        PagerStyle-HorizontalAlign="Center" PagerStyle-VerticalAlign="Middle" Width="1075px">
                        <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" Height="22px" HorizontalAlign="Center"
                            VerticalAlign="Middle" />
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="70px">
                                <HeaderTemplate>
                                    TYPE</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblType" CssClass="rptLbl" Text='<%# DataBinder.Eval(Container.DataItem, "type") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="70px">
                                <HeaderTemplate>
                                    LIP/GUM</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblLip" CssClass="rptLbl" Text='<%# DataBinder.Eval(Container.DataItem, "lipgum") %>'></asp:Label></ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList runat="server" ID="ddlLip" ClientIDMode="Static" CssClass="rptDdl">
                                    </asp:DropDownList>
                                    <asp:TextBox runat="server" ID="txtLip" ClientIDMode="Static" CssClass="rptTxt" Text='<%# DataBinder.Eval(Container.DataItem, "lipgum") %>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="50px">
                                <HeaderTemplate>
                                    LIP %</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblLipPer" CssClass="rptLbl" Text='<%# DataBinder.Eval(Container.DataItem, "lipgumper") %>'></asp:Label></ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox runat="server" ID="txtLipPer" ClientIDMode="Static" CssClass="rptTxtNum"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "lipgumper") %>' onkeypress="return isNumberKey(event)"
                                        MaxLength="3"></asp:TextBox></EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="70px">
                                <HeaderTemplate>
                                    BASE</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblBase" CssClass="rptLbl" Text='<%# DataBinder.Eval(Container.DataItem, "base") %>'></asp:Label></ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList runat="server" ID="ddlBase" ClientIDMode="Static" CssClass="rptDdl">
                                    </asp:DropDownList>
                                    <asp:TextBox runat="server" ID="txtBase" ClientIDMode="Static" CssClass="rptTxt"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "base") %>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="50px">
                                <HeaderTemplate>
                                    BASE %</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblBasePer" CssClass="rptLbl" Text='<%# DataBinder.Eval(Container.DataItem, "baseper") %>'></asp:Label></ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox runat="server" ID="txtBasePer" ClientIDMode="Static" CssClass="rptTxtNum"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "baseper") %>' onkeypress="return isNumberKey(event)"
                                        MaxLength="3"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="70px">
                                <HeaderTemplate>
                                    CENTER</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCenter" CssClass="rptLbl" Text='<%# DataBinder.Eval(Container.DataItem, "center") %>'></asp:Label></ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList runat="server" ID="ddlCenter" ClientIDMode="Static" CssClass="rptDdl">
                                    </asp:DropDownList>
                                    <asp:TextBox runat="server" ID="txtCenter" ClientIDMode="Static" CssClass="rptTxt"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "center") %>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="60px">
                                <HeaderTemplate>
                                    CENTER %</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCenterPer" CssClass="rptLbl" Text='<%# DataBinder.Eval(Container.DataItem, "centerper") %>'></asp:Label></ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox runat="server" ID="txtCenterPer" ClientIDMode="Static" CssClass="rptTxtNum"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "centerper") %>' onkeypress="return isNumberKey(event)"
                                        MaxLength="3"></asp:TextBox></EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="70px">
                                <HeaderTemplate>
                                    TREAD</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTread" CssClass="rptLbl" Text='<%# DataBinder.Eval(Container.DataItem, "tread") %>'></asp:Label></ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList runat="server" ID="ddlTread" ClientIDMode="Static" CssClass="rptDdl">
                                    </asp:DropDownList>
                                    <asp:TextBox runat="server" ID="txtTread" ClientIDMode="Static" CssClass="rptTxt"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "tread") %>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="55px">
                                <HeaderTemplate>
                                    TREAD %</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTreadPer" CssClass="rptLbl" Text='<%# DataBinder.Eval(Container.DataItem, "treadper") %>'></asp:Label></ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox runat="server" ID="txtTreadPer" ClientIDMode="Static" CssClass="rptTxtNum"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "treadper") %>' onkeypress="return isNumberKey(event)"
                                        MaxLength="3"></asp:TextBox></EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="280px">
                                <HeaderTemplate>
                                    DESCRIPTION</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTypeDesc" Text='<%# DataBinder.Eval(Container.DataItem, "TypeDesc") %>'></asp:Label></ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox runat="server" ID="txtTypeDesc" ClientIDMode="Static" CssClass="rptTxtDesc"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "TypeDesc") %>' MaxLength="100"></asp:TextBox></EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="35px">
                                <HeaderTemplate>
                                    BANDBEAD</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="chkBead" Enabled="false" Checked='<%# DataBinder.Eval(Container.DataItem,"beadband").ToString()=="Yes"?true:false %>' /></ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox runat="server" ID="chkBead" Enabled="true" Checked='<%# DataBinder.Eval(Container.DataItem,"beadband").ToString()=="Yes"?true:false %>' /></EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="140px">
                                <HeaderTemplate>
                                    ACTION</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Button runat="server" ID="lnkEdit" Text="Edit" CommandName="Edit" /></ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Button runat="server" ID="lnkUpdate" Text="Update" CommandName="Update" OnClientClick="javascript:return calcPer(this);" />
                                    <asp:Button runat="server" ID="lnkCancel" Text="Cancel" CommandName="Cancel" /></EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td style="width: 100%; padding-top: 10px;">
                    <span class="headCss">Empty/Null Values Type</span>
                    <div style="border: 1px solid #757272; padding-left: 15px; line-height: 25px;">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <asp:RadioButtonList runat="server" ID="rdb_EmptyTypes" RepeatDirection="Horizontal"
                                    RepeatColumns="8" Width="1050px">
                                </asp:RadioButtonList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="popup_box_EmptyType">
                        <div style="width: 1070px;">
                            <div style="padding-top: 5px; font-weight: bold; color: #ffffff;">
                                <div style="width: 70px; float: left; text-align: center;">
                                    <span>Type</span>
                                    <asp:Label runat="server" ID="lblType1" ClientIDMode="Static" Text="" Width="80px"></asp:Label></div>
                                <div style="width: 70px; float: left; text-align: center;">
                                    <span>Lip</span>
                                    <asp:TextBox runat="server" ID="txtLip1" ClientIDMode="Static" CssClass="rptTxt"
                                        Text=""></asp:TextBox></div>
                                <div style="width: 50px; float: left; text-align: center">
                                    <span>Lip %</span>
                                    <asp:TextBox runat="server" ID="txtLipPer1" ClientIDMode="Static" Text="" CssClass="rptTxtNum"
                                        onkeypress="return isNumberKey(event)" MaxLength="3"></asp:TextBox>
                                </div>
                                <div style="width: 70px; float: left; text-align: center">
                                    <span>Base</span>
                                    <asp:TextBox runat="server" ID="txtBase1" ClientIDMode="Static" CssClass="rptTxt"
                                        Text=""></asp:TextBox></div>
                                <div style="width: 50px; float: left; text-align: center">
                                    <span>Base %</span>
                                    <asp:TextBox runat="server" ID="txtBasePer1" ClientIDMode="Static" Text="" CssClass="rptTxtNum"
                                        onkeypress="return isNumberKey(event)" MaxLength="3"></asp:TextBox>
                                </div>
                                <div style="width: 70px; float: left; text-align: center">
                                    <span>Center</span>
                                    <asp:TextBox runat="server" ID="txtCenter1" ClientIDMode="Static" CssClass="rptTxt"
                                        Text=""></asp:TextBox></div>
                                <div style="width: 60px; float: left; text-align: center">
                                    <span>Center %</span>
                                    <asp:TextBox runat="server" ID="txtCenterPer1" ClientIDMode="Static" Text="" CssClass="rptTxtNum"
                                        onkeypress="return isNumberKey(event)" MaxLength="3"></asp:TextBox>
                                </div>
                                <div style="width: 70px; float: left; text-align: center">
                                    <span>Tread</span>
                                    <asp:TextBox runat="server" ID="txtTread1" ClientIDMode="Static" CssClass="rptTxt"
                                        Text=""></asp:TextBox></div>
                                <div style="width: 60px; float: left; text-align: center">
                                    <span>Tread %</span>
                                    <asp:TextBox runat="server" ID="txtTreadPer1" ClientIDMode="Static" Text="" CssClass="rptTxtNum"
                                        onkeypress="return isNumberKey(event)" MaxLength="3"></asp:TextBox>
                                </div>
                                <div style="width: 280px; float: left; text-align: center">
                                    <span>Description</span>
                                    <asp:TextBox runat="server" ID="txtTypeDesc1" ClientIDMode="Static" Text="" CssClass="rptTxtDesc"
                                        MaxLength="100"></asp:TextBox>
                                </div>
                                <div style="width: 60px; float: left; text-align: center">
                                    <span>Beadband</span>
                                    <asp:CheckBox runat="server" ID="chkBead1" ClientIDMode="Static" /></div>
                                <div style="width: 160px; float: left; padding-top: 20px;">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                            <div style="float: left; cursor: pointer; width: 80px;">
                                                <asp:Button runat="server" ID="btnEdit1" ClientIDMode="Static" Text="Update" OnClientClick="javascript:return calcPer1();"
                                                    OnClick="btnEdit1_click" CssClass="btnsave" /></div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <div style="width: 80px; float: left; line-height: 22px;">
                                        <span onclick="btnPopupCancel();" class="btnclear">Cancel</span>
                                    </div>
                                </div>
                            </div>
                            <span style="width: 1050px; float: left; padding-left: 10px; color: Red; font-weight: bold;
                                text-align: center; font-size: 20px;" id="emptyTypeErrMsg"></span>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</div>
<asp:HiddenField runat="server" ID="hdnEmptyType" ClientIDMode="Static" Value="" />
<script type="text/javascript">
    var $ = jQuery.noConflict();
    $(document).ready(function () {
        $("input:radio[id*=rdb_EmptyTypes_]").click(function () {
            $('#lblType1').html($(this).val());
            $('#hdnEmptyType').val($(this).val());
            $('#popup_box_EmptyType').show();
            $('#txtLip1').val(''); $('#txtLipPer1').val(''); $('#txtBase1').val(''); $('#txtBasePer1').val(''); $('#txtCenter1').val(''); $('#txtCenterPer1').val('');
            $('#txtTread1').val(''); $('#txtTreadPer1').val(''); $('#chkBead1').attr('checked', false); $('#emptyTypeErrMsg').html(''); $('#txtTypeDesc1').val('');
        });

        $('#ddlLip').change(function () {
            $('#txtLip').css('display', 'none').val('');
            if ($('#ddlLip option:selected').text() != "CHOOSE" && $('#ddlLip option:selected').text() == "ADD NEW")
                $('#txtLip').css('display', 'block');
            else if ($('#ddlLip option:selected').text() != "CHOOSE" && $('#ddlLip option:selected').text() != "ADD NEW")
                $('#txtLip').val($('#ddlLip option:selected').text());
        });
        $('#ddlBase').change(function () {
            $('#txtBase').css('display', 'none').val('');
            if ($('#ddlBase option:selected').text() != "CHOOSE" && $('#ddlBase option:selected').text() == "ADD NEW")
                $('#txtBase').css('display', 'block');
            else if ($('#ddlBase option:selected').text() != "CHOOSE" && $('#ddlBase option:selected').text() != "ADD NEW")
                $('#txtBase').val($('#ddlBase option:selected').text());
        });
        $('#ddlCenter').change(function () {
            $('#txtCenter').css('display', 'none').val('');
            if ($('#ddlCenter option:selected').text() != "CHOOSE" && $('#ddlCenter option:selected').text() == "ADD NEW")
                $('#txtCenter').css('display', 'block');
            else if ($('#ddlCenter option:selected').text() != "CHOOSE" && $('#ddlCenter option:selected').text() != "ADD NEW")
                $('#txtCenter').val($('#ddlCenter option:selected').text());
        });
        $('#ddlTread').change(function () {
            $('#txtTread').css('display', 'none').val('');
            if ($('#ddlTread option:selected').text() != "CHOOSE" && $('#ddlTread option:selected').text() == "ADD NEW")
                $('#txtTread').css('display', 'block');
            else if ($('#ddlTread option:selected').text() != "CHOOSE" && $('#ddlTread option:selected').text() != "ADD NEW")
                $('#txtTread').val($('#ddlTread option:selected').text());
        });
    });

    function isNumberKey(evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode;
        if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57))
            return false;
        return true;
    }

    function calcPer() {
        var ErrMsg = '';
        if ($('#ddlLip option:selected').text() == "CHOOSE" && $('#txtLipPer').val().length > 0)
            ErrMsg += 'choose lip code<br/>';
        else if ($('#ddlLip option:selected').text() != "CHOOSE" && $('#ddlLip option:selected').text() != "ADD NEW" && $('#txtLipPer').val().length == 0)
            ErrMsg += 'Enter lip %<br/>';
        else if ($('#ddlLip option:selected').text() == "ADD NEW" && $('#txtLip').val().length == 0)
            ErrMsg += 'Enter lip code<br/>';
        if ($('#ddlBase option:selected').text() == "CHOOSE" && $('#txtBasePer').val().length > 0)
            ErrMsg += 'choose base code<br/>';
        else if ($('#ddlBase option:selected').text() != "CHOOSE" && $('#ddlBase option:selected').text() != "ADD NEW" && $('#txtBasePer').val().length == 0)
            ErrMsg += 'Enter base %<br/>';
        else if ($('#ddlBase option:selected').text() == "ADD NEW" && $('#txtBase').val().length == 0)
            ErrMsg += 'Enter base code<br/>';
        if ($('#ddlCenter option:selected').text() == "CHOOSE" && $('#txtCenterPer').val().length > 0)
            ErrMsg += 'choose center code<br/>';
        else if ($('#ddlCenter option:selected').text() != "CHOOSE" && $('#ddlCenter option:selected').text() != "ADD NEW" && $('#txtCenterPer').val().length == 0)
            ErrMsg += 'Enter center %<br/>';
        else if ($('#ddlCenter option:selected').text() == "ADD NEW" && $('#txtCenter').val().length == 0)
            ErrMsg += 'Enter center code<br/>';
        if ($('#ddlTread option:selected').text() == "CHOOSE" && $('#txtTreadPer').val().length > 0)
            ErrMsg += 'choose tread code<br/>';
        else if ($('#ddlTread option:selected').text() != "CHOOSE" && $('#ddlTread option:selected').text() != "ADD NEW" && $('#txtTreadPer').val().length == 0)
            ErrMsg += 'Enter tread %<br/>';
        else if ($('#ddlTread option:selected').text() == "ADD NEW" && $('#txtTread').val().length == 0)
            ErrMsg += 'Enter tread code<br/>';
        var strlip = $("input[id*='txtLipPer']").val();
        var strbase = $("input[id*='txtBasePer']").val();
        var strcenter = $("input[id*='txtCenterPer']").val();
        var strtread = $("input[id*='txtTreadPer']").val();
        strlip = strlip.length == 0 ? 0 : strlip;
        strbase = strbase.length == 0 ? 0 : strbase;
        strcenter = strcenter.length == 0 ? 0 : strcenter;
        strtread = strtread.length == 0 ? 0 : strtread;
        var totPer = parseInt(strlip) + parseInt(strbase) + parseInt(strcenter) + parseInt(strtread);
        if (parseInt(totPer) < 100 || parseInt(totPer) > 100) {
            if (parseInt(totPer) != 0)
                ErrMsg += 'Total percentage should be 100. Enter Correct Values'
        }
        if (ErrMsg.length > 0) {
            $('#lblErrMsg').html(ErrMsg);
            return false;
        }
        else
            return true;
    }

    function calcPer1() {
        if ($('#txtLipPer1').val().length > 0 || $('#txtBasePer1').val().length > 0 || $('#txtCenterPer1').val().length > 0 || $('#txtTreadPer1').val().length > 0) {
            var strlip1 = $('#txtLipPer1').val();
            var strbase1 = $('#txtBasePer1').val();
            var strcenter1 = $('#txtCenterPer1').val();
            var strtread1 = $('#txtTreadPer1').val();
            strlip1 = strlip1.length == 0 ? 0 : strlip1;
            strbase1 = strbase1.length == 0 ? 0 : strbase1;
            strcenter1 = strcenter1.length == 0 ? 0 : strcenter1;
            strtread1 = strtread1.length == 0 ? 0 : strtread1;
            var totPer1 = parseInt(strlip1) + parseInt(strbase1) + parseInt(strcenter1) + parseInt(strtread1);
            if (parseInt(totPer1) < 100 || parseInt(totPer1) > 100) {
                if (parseInt(totPer1) != 0) {
                    alert('Total percentage should be 100. Enter Correct Values');
                    return false;
                }
            }
            else {
                $('#popup_box_EmptyType').hide();
                $("input:radio[id*=rdb_EmptyTypes_]").attr('checked', false);
                return true;
            }
        }
        else {
            $('#emptyTypeErrMsg').html('Please fill the deatils.');
            return false;
        }
    }

    function btnPopupCancel() {
        $('#popup_box_EmptyType').hide();
        $("input:radio[id*=rdb_EmptyTypes_]").attr('checked', false);
    }
</script>
