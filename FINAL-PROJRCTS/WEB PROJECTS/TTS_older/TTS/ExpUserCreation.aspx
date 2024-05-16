<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExpUserCreation.aspx.cs"
    MasterPageFile="~/master.Master" Inherits="TTS.ExpUserCreation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        <asp:Label ID="lblPageHeading" runat="server" Text="User-ID Creation"></asp:Label>
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="background-color: #ecf6ff; width: 100%;
            border-color: White;">
            <tr>
                <th>
                    <span class="spanCss">Customer</span>
                </th>
                <td colspan="3">
                    <asp:DropDownList ID="ddl_CustomerSelection" Width="550px" ClientIDMode="Static"
                        AutoPostBack="true" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddl_CustomerSelection_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    <span class="spanCss">User ID</span>
                </th>
                <td>
                    <asp:TextBox ID="txt_UserID" runat="server" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                    <asp:DropDownList ID="ddl_UserIDSelection" ClientIDMode="Static" AutoPostBack="true"
                        runat="server" CssClass="form-control" OnSelectedIndexChanged="ddl_UserIDSelection_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <th>
                    <span class="spanCss">Password</span>
                </th>
                <td>
                    <asp:TextBox ID="txt_Password" ClientIDMode="Static" runat="server" CssClass="form-control"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    <span class="spanCss">Email ID</span>
                </th>
                <td>
                    <asp:TextBox ID="txt_EmailID" runat="server" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                </td>
                <th>
                    <span class="spanCss">Currency</span>
                </th>
                <td>
                    <asp:TextBox ID="txt_Currency" runat="server" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    <span class="spanCss">Country</span>
                </th>
                <td>
                    <asp:DropDownList ID="ddl_CountrySelection" ClientIDMode="Static" AutoPostBack="true"
                        runat="server" CssClass="form-control" OnSelectedIndexChanged="ddl_CountrySelection_SelectedIndexChanged">
                    </asp:DropDownList>
                    <div id="div_AddNewCountry" style="display: none;">
                        <span class="spanCss" style="color: #ff0000;">Enter Country</span>
                        <asp:TextBox ID="txt_NewCountry" runat="server" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                    </div>
                </td>
                <th>
                    <span class="spanCss">City</span>
                </th>
                <td>
                    <asp:DropDownList ID="ddl_CitySelection" ClientIDMode="Static" AutoPostBack="true"
                        runat="server" CssClass="form-control" OnSelectedIndexChanged="ddl_CitySelection_SelectedIndexChanged">
                    </asp:DropDownList>
                    <div id="div_AddNewCity" style="display: none;">
                        <span class="spanCss" style="color: #ff0000;">Enter City</span>
                        <asp:TextBox ID="txt_NewCity" runat="server" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <th>
                    <span class="spanCss">Category</span>
                </th>
                <td>
                    <asp:XmlDataSource ID="xml_CategorySelection" DataFile="~/XML/exportcontent.xml"
                        runat="server" XPath="export/Category"></asp:XmlDataSource>
                    <asp:DropDownList ID="ddl_CategorySelection" ClientIDMode="Static" runat="server"
                        CssClass="form-control" DataSourceID="xml_CategorySelection" DataTextField="item"
                        DataValueField="id">
                    </asp:DropDownList>
                </td>
                <th>
                    <span class="spanCss">Lead</span>
                </th>
                <td>
                    <asp:DropDownList ID="ddl_LeadSelection" ClientIDMode="Static" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    <span class="spanCss">Supervisor</span>
                </th>
                <td>
                    <asp:DropDownList ID="ddl_SupervisorSelection" ClientIDMode="Static" runat="server"
                        CssClass="form-control">
                    </asp:DropDownList>
                </td>
                <th>
                    <span class="spanCss">Manager</span>
                </th>
                <td>
                    <asp:DropDownList ID="ddl_ManagerSelection" ClientIDMode="Static" runat="server"
                        CssClass="form-control">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    <span class="spanCss">Payment Mode</span>
                </th>
                <td colspan="3">
                    <div>
                        <asp:XmlDataSource ID="xml_PaymemntMode" DataFile="~/XML/exportcontent.xml" runat="server"
                            XPath="export/paymode"></asp:XmlDataSource>
                        <asp:RadioButtonList ID="rdo_PaymentSelection" ClientIDMode="Static" runat="server"
                            CellPadding="5" RepeatDirection="Horizontal" DataSourceID="xml_PaymemntMode"
                            DataTextField="item" DataValueField="id" AutoPostBack="true" OnSelectedIndexChanged="rdo_PaymentSelection_SelectedIndexChanged">
                        </asp:RadioButtonList>
                    </div>
                    <div>
                        <table style="background-color: #bed1e2; border-color: Blue;">
                            <tr>
                                <td style="text-align: center;">
                                    <asp:RadioButtonList ID="rdo_PayTimeSelection" ClientIDMode="Static" runat="server"
                                        CellPadding="5" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rdo_PayTimeSelection_SelectedIndexChanged">
                                    </asp:RadioButtonList>
                                </td>
                                <td id="div_PaymentSelection" style="display: none;">
                                    <asp:Label CssClass="spanCss" runat="server" ID="lblNoofDays" Text=""></asp:Label>
                                    <asp:TextBox ID="txt_NoofDays" runat="server" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <th>
                    <span class="spanCss">Payment Terms</span>
                </th>
                <td>
                    <asp:TextBox ID="txt_PaymentTerms" runat="server" ClientIDMode="Static" Width="400px"
                        Height="150px" TextMode="MultiLine" onKeyUp="javascript:CheckMaxLength(this, 1999);"
                        onChange="javascript:CheckMaxLength(this, 1999);" CssClass="form-control"></asp:TextBox>
                </td>
                <th>
                    <span class="spanCss">Instructions</span>
                </th>
                <td>
                    <asp:TextBox ID="txt_Instructions" runat="server" ClientIDMode="Static" Width="400px"
                        Height="150px" TextMode="MultiLine" onKeyUp="javascript:CheckMaxLength(this, 999);"
                        onChange="javascript:CheckMaxLength(this, 999);" CssClass="form-control"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Label ID="lblErrMsg" runat="server" ClientIDMode="Static" ForeColor="Red" Font-Bold="true"></asp:Label>
                </td>
            </tr>
        </table>
        <br />
        <hr />
        <div style="text-align: center;">
            <asp:Button ID="btn_SaveRecord" runat="server" Text="Save Record" CssClass="btn btn-success"
                OnClick="btn_SaveRecord_Click" OnClientClick="javascript:return cntrlSaveCheck();" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btn_ClearRecord" runat="server" Text="Clear Selection" CssClass="btn btn-info"
                OnClick="btn_ClearRecord_Click" />
        </div>
        <hr />
    </div>
    <asp:HiddenField ID="hdnType" runat="server" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        $(document).ready(function () {
            $('#txt_UserID').blur(function () {
                $('#lblErrMsg').html('');
                if ($('#txt_UserID').val().length > 0) {
                    $.ajax({ type: "POST", url: "BindRecords.aspx?type=ChkCotsUserName&userId=" + $('#txt_UserID').val() + "", context: document.body,
                        success: function (data) { if (data != '') { $('#lblErrMsg').html(data); $('#txt_UserID').focus(); } }
                    });
                }
            });
        });
        function NewCountryCity(displayval, strmethod) {
            $('#txt_NewCountry').val(''); $('#txt_NewCity').val('');
            $('#div_AddNewCountry').css({ 'display': 'none' });
            $('#div_AddNewCity').css({ 'display': 'none' });
            $('#ddl_CitySelection').prop('disabled', false);
            if (displayval == 'block') {
                $('#div_AddNewCity').css({ 'display': 'block' });
                $('#ddl_CitySelection').prop('disabled', true);
                if (strmethod == 'country')
                    $('#div_AddNewCountry').css({ 'display': 'block' });
            }
        }
        function cntrlSaveCheck() {
            var errMsg = "";
            var emailpattern = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
            $('#lblErrMsg').html("");
            if ($('#ddl_CustomerSelection option:selected').text() == "CHOOSE")
                errMsg += 'Choose Customer <br/>';
            if ($('#hdnType').val() == "new" && $('#txt_UserID').val() == "")
                errMsg += 'Enter UserId<br/>';
            else if ($('#hdnType').val() == "modify" && $('#ddl_UserIDSelection option:selected').text() == "CHOOSE")
                errMsg += 'Choose UserId<br/>';
            if ($('#txt_Password').val() == "")
                errMsg += 'Enter Password<br/>';
            if ($('#txt_EmailID').val() == "")
                errMsg += 'Enter Email ID<br/>';
            else if (emailpattern.test($('#txt_EmailID').val()) == false)
                errMsg += 'Enter Valid Email <br />';
            if ($('#txt_Currency').val() == "")
                errMsg += 'Enter Currency<br/>';

            if ($('#ddl_CountrySelection option:selected').text() == "CHOOSE")
                errMsg += "Choose Country <br/>";
            else if ($('#ddl_CountrySelection option:selected').text() == "ADD NEW") {
                if ($('#txt_NewCountry').val() == "")
                    errMsg += 'Enter Country<br/>';
                if ($('#txt_NewCity').val() == "")
                    errMsg += 'Enter City<br/>';
            }
            if ($('#ddl_CitySelection option:selected').text() == "CHOOSE")
                errMsg += "Choose City <br/>";
            else if ($('#ddl_CitySelection option:selected').text() == "ADD NEW") {
                if ($('#txt_NewCity').val() == "")
                    errMsg += 'Enter City<br/>';
            }
            if ($('#ddl_CategorySelection option:selected').text() == "CHOOSE")
                errMsg += "Choose Category <br/>";
            if ($('#ddl_LeadSelection option:selected').text() == "CHOOSE" && $('#ddl_SupervisorSelection option:selected').text() == "CHOOSE" && $('#ddl_ManagerSelection option:selected').text() == "CHOOSE")
                errMsg += "Choose atleast any one Lead/Supervisor/Manager <br/>";
            if ($('input:radio[id*=rdo_PaymentSelection_]:checked').length == 0)
                errMsg += 'Choose Payment mode<br/>';
            else if ($('input:radio[id*=rdo_PaymentSelection_]:checked').val() != "payagainst") {
                if ($('input:radio[id*=rdo_PayTimeSelection_]:checked').length == 0)
                    errMsg += "Choose PayTime <br/>";
                else if ($('input:radio[id*=rdo_PaymentSelection_]:checked').val() == "advance" && $('#txt_NoofDays').val() == "")
                    errMsg += 'Enter Percentage<br/>';
                else if ($('#txt_NoofDays').val() == "")
                    errMsg += 'Enter days<br/>';
            }
            if ($('#txt_PaymentTerms').val() == "")
                errMsg += 'Enter Payment Terms<br/>';
            if (errMsg.length > 0) {
                $('#lblErrMsg').html(errMsg);
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
