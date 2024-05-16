<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="userprivilege.aspx.cs" Inherits="TTS.userprivilege" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .tablePrivilege
        {
            border-collapse: collapse;
            border-color: #868282;
            width: 100%;
            line-height: 20px;
        }
        .tablePrivilege th
        {
            background-color: #096b6b;
            color: #fff;
            text-align: left;
            padding-left: 10px;
            width: 80px;
            font-weight: bold;
        }
        .tablePrivilege td
        {
            background-color: #ccf9f9;
            color: #000;
            text-align: left;
        }
        .RdbListCssClass
        {
            font-family: Courier New;
            color: #bf3a09;
            font-style: italic;
            font-size: large;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div align="center" class="pageTitleHead">
        USER PRIVILEGE</div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table style="width: 100%;">
            <tr>
                <td>
                    <asp:RadioButtonList runat="server" ID="rdbUserType" RepeatDirection="Horizontal"
                        AutoPostBack="true" OnSelectedIndexChanged="rdbUserType_IndexChange">
                        <asp:ListItem Value="New" Text="New User" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="Exist" Text="Existing User"> </asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td style="width: 100%; display: block;" id="tdNewUser" runat="server">
                    <table cellspacing="0" rules="all" border="1" class="tablePrivilege">
                        <tr>
                            <th>
                                USERNAME
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtUserName" Text="" CssClass="txtCss" ClientIDMode="Static"
                                    Width="245px"></asp:TextBox>
                            </td>
                            <th>
                                PASSWORD
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtPassword" Text="" CssClass="txtCss" ClientIDMode="Static"
                                    TextMode="Password" Width="245px"></asp:TextBox>
                            </td>
                            <th style="line-height: 12px;">
                                CONFIRM PASSWORD
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtConfirmPass" Text="" CssClass="txtCss" ClientIDMode="Static"
                                    TextMode="Password" Width="245px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="width: 100%; display: none;" id="tdExistUser" runat="server">
                    <asp:RadioButtonList runat="server" ID="rdbExistUserList" RepeatDirection="Vertical"
                        AutoPostBack="true" OnSelectedIndexChanged="rdbExistUserList_IndexChange" Width="100%"
                        CellPadding="1" CellSpacing="1" RepeatColumns="4" Font-Bold="true" BackColor="#f3fbae"
                        CssClass="RdbListCssClass">
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                    <table cellspacing="0" rules="all" border="1" class="tablePrivilege">
                        <tr>
                            <th>
                                E-MAIL
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtEmail" Text="" CssClass="txtCss" ClientIDMode="Static"
                                    Width="315px"></asp:TextBox>
                            </td>
                            <th>
                                USER TYPE
                            </th>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlUserLoginType" ClientIDMode="Static" Width="100px">
                                    <asp:ListItem Text="CHOOSE" Value="CHOOSE"></asp:ListItem>
                                    <asp:ListItem Text="Lead" Value="Lead"></asp:ListItem>
                                    <asp:ListItem Text="Supervisor" Value="Supervisor"></asp:ListItem>
                                    <asp:ListItem Text="Manager" Value="Manager"></asp:ListItem>
                                    <asp:ListItem Text="Support" Value="Support"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <th>
                                DEPARTMENT
                            </th>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlUserDepartment" ClientIDMode="Static" Width="80px">
                                </asp:DropDownList>
                                <asp:TextBox runat="server" ID="txtUserDepartment" ClientIDMode="Static" Width="80px"></asp:TextBox>
                            </td>
                            <th>
                                CHANNEL
                            </th>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlUserChannel" ClientIDMode="Static" Width="80px">
                                    <asp:ListItem Text="CHOOSE" Value="CHOOSE"></asp:ListItem>
                                    <asp:ListItem Text="DIRECT" Value="DIRECT"></asp:ListItem>
                                    <asp:ListItem Text="AGENT" Value="AGENT"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table cellspacing="0" rules="all" border="1" style="padding-top: 5px; font-weight: bold;
                        line-height: 15px; width: 100%; background-color: #e4f2f3;" id="chkusermenulevel">
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblMasterMenu" ClientIDMode="Static" Text="" Font-Size="16px"
                                    BackColor="#fdc9e5" Width="100%"></asp:Label>
                                <asp:CheckBoxList runat="server" ID="chkMasterUserLevel" ClientIDMode="Static" RepeatColumns="6"
                                    RepeatDirection="Vertical" RepeatLayout="Table" Width="100%" BackColor="#fdc9e5">
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblDashboardMenu" ClientIDMode="Static" Text="" Font-Size="16px"
                                    BackColor="#dec483" Width="100%"></asp:Label>
                                <asp:CheckBoxList runat="server" ID="chkDashboardUserLevel" ClientIDMode="Static"
                                    RepeatColumns="6" RepeatDirection="Vertical" RepeatLayout="Table" Width="100%"
                                    BackColor="#dec483">
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblProspectMenu" ClientIDMode="Static" Text="" Font-Size="16px"
                                    BackColor="#92f7e0" Width="100%"></asp:Label>
                                <asp:CheckBoxList runat="server" ID="chkProspectUserLevel" ClientIDMode="Static"
                                    RepeatColumns="6" RepeatDirection="Vertical" RepeatLayout="Table" Width="100%"
                                    BackColor="#92f7e0">
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblScotsDomesticMenu" ClientIDMode="Static" Text=""
                                    Font-Size="16px" BackColor="#92ec8c" Width="100%"></asp:Label>
                                <asp:CheckBoxList runat="server" ID="chkScotsDomesticUserLevel" ClientIDMode="Static"
                                    RepeatColumns="6" RepeatDirection="Vertical" RepeatLayout="Table" Width="100%"
                                    BackColor="#92ec8c" 
                                    onselectedindexchanged="chkScotsDomesticUserLevel_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblScotsExportMenu" ClientIDMode="Static" Text="" Font-Size="16px"
                                    BackColor="#b6d1ec" Width="100%"></asp:Label>
                                <asp:CheckBoxList runat="server" ID="chkScotsExportUserLevel" ClientIDMode="Static"
                                    RepeatColumns="6" RepeatDirection="Vertical" RepeatLayout="Table" Width="100%"
                                    BackColor="#b6d1ec">
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblClaimMenu" ClientIDMode="Static" Text="" Font-Size="16px"
                                    BackColor="#c4e87b" Width="100%"></asp:Label>
                                <asp:CheckBoxList runat="server" ID="chkClaimUserLevel" ClientIDMode="Static" RepeatColumns="5"
                                    RepeatDirection="Vertical" RepeatLayout="Table" Width="100%" BackColor="#c4e87b">
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblOrderTrackingMenu" ClientIDMode="Static" Text=""
                                    Font-Size="16px" BackColor="#f9cdcd" Width="100%"></asp:Label>
                                <asp:CheckBoxList runat="server" ID="chkOrderTrackingUserLevel" ClientIDMode="Static"
                                    RepeatColumns="6" RepeatDirection="Vertical" RepeatLayout="Table" Width="100%"
                                    BackColor="#f9cdcd">
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblEBidMenu" ClientIDMode="Static" Text="" Font-Size="16px"
                                    BackColor="#e9ef9b" Width="100%"></asp:Label>
                                <asp:CheckBoxList runat="server" ID="chkEBidUserLevel" ClientIDMode="Static" RepeatColumns="6"
                                    RepeatDirection="Vertical" RepeatLayout="Table" Width="100%" BackColor="#e9ef9b">
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" ForeColor="Red" Text=""
                        CssClass="lblErrCss" Font-Bold="true" Font-Size="14px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <table cellspacing="0" rules="all" border="1" style="width: 100%; text-align: center;">
                        <tr>
                            <td>
                                <asp:LinkButton runat="server" ID="lnkCreate" ClientIDMode="Static" CssClass="btnsave"
                                    Text="Save" OnClientClick="javascript:return CreateUserCtrlValidate();" OnClick="lnkCreate_click"></asp:LinkButton>
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnPageClear" ClientIDMode="Static" Text="Clear" CssClass="btnclear"
                                    OnClick="btnPageClear_Click" />
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnSuspendUser" ClientIDMode="Static" CssClass="btnactive"
                                    Text="Suspend" OnClientClick="javascript:return CtrlSubspendChk();" OnClick="btnSuspendUser_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $("input:text").attr("autocomplete", "off");
            $('#txtUserDepartment').css({ 'display': 'none' });
            $('#ddlUserDepartment').change(function () {
                $('#txtUserDepartment').css({ 'display': 'none' });
                if ($('#ddlUserDepartment option:selected').text() == "ADD NEW DEPT")
                    $('#txtUserDepartment').css({ 'display': 'block' });
            });
        });
        function CreateUserCtrlValidate() {
            $('#lblErrMsg').html(''); var msg = '';
            if ($("input:radio[id*=MainContent_rdbUserType_0]").attr('checked') == "checked") {
                var txtUserName = $('#txtUserName').val();
                var txtPassword = $('#txtPassword').val();
                var txtConfirmPass = $('#txtConfirmPass').val();
                var txtEmail = $('#txtEmail').val();
                var emailpattern = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;

                if (txtUserName.length == 0)
                    msg += 'Enter username <br />';
                if (txtPassword.length == 0)
                    msg += 'Enter password <br />';
                if (txtConfirmPass.length == 0)
                    msg += 'Enter confirm password <br />';
                if (txtEmail.length == 0)
                    msg += 'Enter E-Mail ID <br />';
                else if (emailpattern.test(txtEmail) == false)
                    msg += 'Enter valid E-Mail ID <br />';
                if (txtPassword.length > 0 && txtConfirmPass.length > 0) {
                    if (txtPassword.toString() != txtConfirmPass.toString())
                        msg += 'Password mismatch <br />';
                }
            }
            else if ($("input:radio[id*=MainContent_rdbUserType_1]").attr('checked') == "checked") {
                if ($('input:radio[id*=MainContent_rdbExistUserList_]:checked').length == 0)
                    msg += 'Choose anyone username <br />';
            }
            if ($('#ddlUserLoginType option:selected').text() == "CHOOSE")
                msg += 'Choose user type <br />';
            if ($('#ddlUserChannel option:selected').text() == "CHOOSE")
                msg += 'Choose user channel<br/>';
            if ($('#ddlUserDepartment option:selected').text() == "CHOOSE")
                msg += 'Choose user department<br/>';
            else if ($('#ddlUserDepartment option:selected').text() == "ADD NEW DEPT") {
                if ($('#txtUserDepartment').val().length == 0)
                    msg += 'Enter user department<br/>';
            }
            if ($('#chkusermenulevel input:checked').length == 0)
                msg += 'Choose anyone menu list <br />';

            if (msg.length > 0) {
                $('#lblErrMsg').html(msg).attr("style", "color:red");
                gotoErrMsg('lblErrMsg');
                return false;
            }
            else
                return true;
        }

        function CtrlSubspendChk() {
            $('#lblErrMsg').html('');
            if ($('input:radio[id*=MainContent_rdbExistUserList_]:checked').length == 0) {
                $('#lblErrMsg').html('Choose anyone username <br />').attr("style", "color:red");
                gotoErrMsg('lblErrMsg');
                return false;
            }
            else
                return true;
        }
        function gotoErrMsg(ctrlID) {
            $('#' + ctrlID).css({ 'display': 'block' });
            $("html, body").stop().animate({
                scrollLeft: $('#' + ctrlID).offset().left,
                scrollTop: $('#' + ctrlID).offset().top
            }, 1200);
        }
    </script>
</asp:Content>
