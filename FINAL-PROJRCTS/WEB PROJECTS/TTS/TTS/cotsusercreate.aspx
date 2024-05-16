<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="cotsusercreate.aspx.cs" Inherits="TTS.cotsusercreate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        <asp:Label runat="server" ID="lblPageHeading" ClientIDMode="Static" Text=""></asp:Label>
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="background-color: #ecf6ff; width: 100%;
            border-color: White;">
            <tr>
                <th class="spanCss">
                    Customer
                </th>
                <td colspan="3">
                    <asp:TextBox runat="server" ID="txtDomesticCustomerName" ClientIDMode="Static" Width="555px"
                        CssClass="form-control" MaxLength="100"></asp:TextBox>
                    <asp:DropDownList runat="server" ID="ddlCustomerName" ClientIDMode="Static" AutoPostBack="true"
                        Width="555px" CssClass="form-control" OnSelectedIndexChanged="ddlCustomerName_IndexChange"
                        AppendDataBoundItems="true">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th class="spanCss">
                    User ID
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtUserName" Text="" CssClass="form-control" ClientIDMode="Static"
                        Width="315px"></asp:TextBox>
                    <asp:DropDownList runat="server" ID="ddlUserID" ClientIDMode="Static" AutoPostBack="true"
                        Width="315px" CssClass="form-control" OnSelectedIndexChanged="ddlUserID_IndexChange">
                    </asp:DropDownList>
                </td>
                <th class="spanCss">
                    Password
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtPassword" Text="" CssClass="form-control" ClientIDMode="Static"
                        Width="315px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th class="spanCss">
                    E-Mail ID
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtEmail" Text="" CssClass="form-control" ClientIDMode="Static"
                        Width="315px"></asp:TextBox>
                </td>
                <th class="spanCss">
                    Currency
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtCotsCurrency" Text="" CssClass="form-control"
                        ClientIDMode="Static" Width="315px" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th class="spanCss">
                    Country
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtCotsUserNewCountry" ClientIDMode="Static" Width="315px"
                        Text="" CssClass="form-control" Enabled="false"></asp:TextBox>
                </td>
                <th class="spanCss">
                    City
                </th>
                <td>
                    <span style="display: block;" id="divCotsExistCity">
                        <asp:DropDownList runat="server" ID="ddlCotsUserCity" ClientIDMode="Static" Width="315px"
                            CssClass="form-control">
                        </asp:DropDownList>
                    </span><span style="display: none; line-height: 20px;" id="divCotsNewCity"><span
                        style="color: #ff0000;">Enter New City:</span>
                        <asp:TextBox runat="server" ID="txtCotsUserNewCity" ClientIDMode="Static" Width="315px"
                            Text="" CssClass="form-control"></asp:TextBox>
                    </span>
                </td>
            </tr>
            <tr>
                <th class="spanCss">
                    Category
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="ddlCustCategory" ClientIDMode="Static" Width="315px"
                        CssClass="form-control">
                        <asp:ListItem Text="CHOOSE" Value="CHOOSE"></asp:ListItem>
                        <asp:ListItem Text="OEM" Value="OEM"></asp:ListItem>
                        <asp:ListItem Text="END USER" Value="END USER"></asp:ListItem>
                        <asp:ListItem Text="DEALER" Value="DEALER"></asp:ListItem>
                        <asp:ListItem Text="PSU" Value="PSU"></asp:ListItem>
                        <asp:ListItem Text="RAILWAY" Value="RAILWAY"></asp:ListItem>
                        <asp:ListItem Text="RENTAL FLEET" Value="RENTAL FLEET"></asp:ListItem>
                        <asp:ListItem Text="ME" Value="ME"></asp:ListItem>
                        <asp:ListItem Text="ST" Value="ST"></asp:ListItem>
                        <asp:ListItem Text="ARC" Value="ARC"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <th class="spanCss">
                    Region
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="ddlRegion" ClientIDMode="Static" Width="100px"
                        CssClass="form-control">
                        <asp:ListItem Text="CHOOSE" Value="CHOOSE"></asp:ListItem>
                        <asp:ListItem Text="EAST" Value="EAST"></asp:ListItem>
                        <asp:ListItem Text="WEST" Value="WEST"></asp:ListItem>
                        <asp:ListItem Text="NORTH" Value="NORTH"></asp:ListItem>
                        <asp:ListItem Text="SOUTH" Value="SOUTH"></asp:ListItem>
                        <asp:ListItem Text="CENTRAL" Value="CENTRAL"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th class="spanCss">
                    Type
                </th>
                <td>
                    <asp:RadioButtonList runat="server" ID="rdbCustType" ClientIDMode="Static" RepeatColumns="4"
                        RepeatDirection="Horizontal" Width="200px">
                        <asp:ListItem Text="A" Value="A"></asp:ListItem>
                        <asp:ListItem Text="B1" Value="B1"></asp:ListItem>
                        <asp:ListItem Text="B2" Value="B2"></asp:ListItem>
                        <asp:ListItem Text="C" Value="C"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <th class="spanCss">
                    Payment Mode
                </th>
                <td>
                    <div style="width: 160px; float: left;">
                        <asp:RadioButtonList runat="server" ID="rdbCreditNote" ClientIDMode="Static" RepeatColumns="2"
                            Width="155px">
                            <asp:ListItem Text="Probation" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Establish" Value="0"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                    <div style="background-color: #D5C9F5; display: none; width: 75px; float: left;"
                        id="divdaysdom">
                        <span style="width: 35px; float: left; line-height: 35px;">Days :</span>
                        <asp:TextBox ID="txtdaysdom" CssClass="form-control" runat="server" ClientIDMode="Static"
                            Text="" Width="30px" onkeypress="return isNumberWithoutDecimal(event)" MaxLength="3"></asp:TextBox>
                    </div>
                    <div style="width: 200px; float: left; line-height: 25px; background-color: #7bfda9;
                        margin-left: 15px;">
                        <span style="width: 70px; float: left; line-height: 35px; font-weight: bold;">Supply
                            Limit:</span>
                        <asp:TextBox runat="server" ID="txtSalesLimit" CssClass="form-control" ClientIDMode="Static"
                            Text="" MaxLength="13" onkeypress="return isNumberKey(event)" Width="100px"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <th class="spanCss">
                    Payment Terms
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtPayTerms" ClientIDMode="Static" TextMode="MultiLine"
                        Width="400px" Height="150px" Text="" onKeyUp="javascript:CheckMaxLength(this, 1999);"
                        onChange="javascript:CheckMaxLength(this, 1999);" CssClass="form-control"></asp:TextBox>
                </td>
                <th class="spanCss">
                    Instructions
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtCustInstructions" ClientIDMode="Static" TextMode="MultiLine"
                        Width="400px" Height="150px" Text="" onKeyUp="javascript:CheckMaxLength(this, 999);"
                        onChange="javascript:CheckMaxLength(this, 999);" CssClass="txtCss"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th class="spanCss">
                    Lead
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="ddlCustLead" ClientIDMode="Static" Width="315px"
                        CssClass="form-control">
                    </asp:DropDownList>
                </td>
                <th class="spanCss">
                    Manager
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="ddlCustManger" ClientIDMode="Static" Width="315px"
                        CssClass="form-control">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div style="float: left; color: Red; line-height: 15px;" id="bindErrmsg" runat="server"
                        clientidmode="Static">
                    </div>
                </td>
                <td colspan="2">
                    <asp:Button runat="server" ID="btnCotsSave" ClientIDMode="Static" Text="SAVE" CssClass="btn btn-success"
                        OnClientClick="javascript:return ctrlCotsNewUserValidate();" OnClick="btnCotsSave_click" />
                    <span class="btn btn-info" onclick="ctrlClearPage();">CLEAR</span>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnFullName" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnUserId" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnMethod" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        $(document).ready(function () {
            $('#txtUserName').blur(function () {
                $('#bindErrmsg').html('');
                if ($('#txtUserName').val().length > 0) {
                    $.ajax({ type: "POST", url: "BindRecords.aspx?type=ChkCotsUserName&userId=" + $('#txtUserName').val() + "", context: document.body,
                        success: function (data) { if (data != '') { $('#bindErrmsg').html(data); $('#txtUserName').focus(); } }
                    });
                }
            });
            $('#ddlCustomerName').change(function () { $('#hdnFullName').val($('#ddlCustomerName option:selected').text()); });
            $('#ddlUserID').change(function () { $('#hdnUserId').val($('#ddlUserID option:selected').text()); });
            $('#ddlCotsUserCity').change(function () {
                $('#divCotsNewCity').css({ 'display': 'none' });
                if ($('#ddlCotsUserCity option:selected').text() == 'ADD NEW CITY')
                    $('#divCotsNewCity').css({ 'display': 'block' });
            });
            $('#rdbCreditNote tr').change(function (e) { PayMode(); });
        });
        function PayMode() {
            $('#divdaysdom').css({ 'display': 'none' });
            if ($('#hdnMethod').val() == "new")
                $('#txtdaysdom').val('0');
            if ($("#rdbCreditNote input[type='radio']:checked").val() == "0")
                $('#divdaysdom').css({ 'display': 'block' });
        }
        function ctrlCotsNewUserValidate() {
            var errmsg = ''; $('#bindErrmsg').html('');
            var emailpattern = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
            if ($('#hdnMethod').val() == "new") {
                if ($('#txtDomesticCustomerName').val().length == 0)
                    errmsg += 'Enter customer full name<br />';
                if ($('#txtUserName').val().length == 0)
                    errmsg += 'Enter user id <br />';
                if ($('#txtPassword').val().length == 0)
                    errmsg += 'Enter password <br />';
            }
            else if ($('#hdnMethod').val() == "modify") {
                if ($("#ddlCustomerName option:selected").text() == "CHOOSE")
                    errmsg += 'Choose customer name <br />';
                if ($("#ddlUserID option:selected").text() == "CHOOSE")
                    errmsg += 'Choose user-id <br />';
            }
            if ($('#txtEmail').val().length == 0)
                errmsg += 'Enter email <br />';
            else if (emailpattern.test($('#txtEmail').val()) == false)
                errmsg += 'Enter vaild email <br />';
            if ($("#ddlCotsUserCity option:selected").text() == "CHOOSE")
                errmsg += 'Choose city <br />';
            else if ($("#ddlCotsUserCity option:selected").text() == 'ADD NEW CITY') {
                if ($('#txtCotsUserNewCity').val().length == 0)
                    errmsg += 'Enter new city<br />';
            }
            if ($("#ddlCustCategory option:selected").text() == 'CHOOSE')
                errmsg += 'Choose customer category <br/>';
            if ($("#ddlRegion option:selected").text() == 'CHOOSE')
                errmsg += 'Choose customer region <br/>';
            if ($('input:radio[id*=rdbCustType_]:checked').length == 0)
                errmsg += 'Choose customer type <br/>';
            if ($('input:radio[id*=rdbCreditNote_]:checked').length == 0)
                errmsg += 'Select Establish/Probation <br />';
            else if ($('input:radio[id*=rdbCreditNote_]:checked').val() == "0" && $('#txtdaysdom').val().length == 0)
                errmsg += 'Enter days <br/>';
            else if ($('input:radio[id*=rdbCreditNote_]:checked').val() == "1")
                $('#txtdaysdom').val('0');
            if ($('#txtSalesLimit').val().length == 0)
                errmsg += 'Enter Sales Limit <br/>';
            if ($('#txtPayTerms').val().length == 0)
                errmsg += 'Enter payment terms <br/>';
            if ($("#ddlCustLead option:selected").text() == 'CHOOSE')
                errmsg += 'Choose Lead <br/>';
            if ($("#ddlCustManger option:selected").text() == 'CHOOSE')
                errmsg += 'Choose manager <br/>';
            if (errmsg.length > 0) {
                $('#bindErrmsg').html(errmsg);
                return false;
            }
            else
                return true;
        }

        function ctrlClearPage() {
            window.location.href = window.location.href;
        }
    </script>
</asp:Content>
