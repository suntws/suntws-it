<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExpCustAddressCreation.aspx.cs"
    MasterPageFile="~/master.Master" Inherits="TTS.ExpCustAddressCreation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .tableCss
        {
            background-color: #ecf6ff;
            width: 100%;
            border-color: White;
        }
        .spanCss
        {
            color: Green;
            font-weight: bold;
            font-family: Times New Roman;
            font-size: 14px;
        }
        .form-control
        {
            display: block;
            width: 300px;
            height: 30px;
            font-size: 14px;
            font-weight: bold;
            background-color: #fff;
            border: 1px solid #000;
            border-radius: 4px;
        }
        .ul
        {
            list-style-type: none;
            margin: 0;
            padding: 0;
            overflow: hidden;
            background-color: #1cb5e2;
            font-weight: bold;
        }
        li a
        {
            color: white;
            text-align: center;
            padding: 8px 8px;
            margin: 5px;
            text-decoration: none;
            cursor: pointer;
            float: left;
        }
        li a:hover
        {
            background-color: #071580;
            border-radius: 5px;
        }
        input[type="checkbox"]
        {
            cursor: pointer;
            position: relative;
            width: 25px;
            height: 25px;
        }
        .aTagSelect
        {
            background-color: #91e621;
            border-radius: 5px;
        }
    </style>
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
        <table cellspacing="0" rules="all" border="1" class="tableCss">
            <tr>
                <th class="spanCss">
                    Customer
                </th>
                <td>
                    <asp:DropDownList ID="ddl_CustomerSelection" Width="550px" ClientIDMode="Static"
                        AutoPostBack="true" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddl_CustomerSelection_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <th class="spanCss">
                    User ID
                </th>
                <td>
                    <asp:DropDownList ID="ddl_UserID" ClientIDMode="Static" AutoPostBack="true" runat="server"
                        CssClass="form-control" OnSelectedIndexChanged="ddl_UserID_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th class="spanCss">
                    Billing Address
                </th>
                <td colspan="3">
                    <asp:DropDownList ID="ddl_BillingAddress" Width="700px" ClientIDMode="Static" AutoPostBack="true"
                        runat="server" CssClass="form-control" OnSelectedIndexChanged="ddl_BillingAddress_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th class="spanCss">
                    Shiping Address
                </th>
                <td colspan="3">
                    <asp:DropDownList ID="ddl_ShippingAddress" Width="700px" ClientIDMode="Static" AutoPostBack="true"
                        runat="server" CssClass="form-control" OnSelectedIndexChanged="ddl_ShippingAddress_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <hr />
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <ul class="ul">
                        <li><a id="a_BillAddress">BILLING ADDRESS</a></li>
                        <li><a id="a_ShipAddress">SHIPPING ADDRESS</a></li>
                        <li id="li_BillasShip" style="float: right;">
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chk_BillasShip" ClientIDMode="Static" Height="20px" Width="20px"
                                            runat="server" AutoPostBack="true" Text="" OnCheckedChanged="chk_BillasShip_CheckedChanged" />
                                    </td>
                                    <td>
                                        <span style="padding: 10px 12px; color: White; font-size: 12px; text-align: left;
                                            vertical-align: middle">Make Bill and Ship Address Same</span>
                                    </td>
                                </tr>
                            </table>
                        </li>
                        <li id="li_AddNewShipAddress" style="float: right;">
                            <div style="vertical-align: middle; padding-top: 8px;">
                                <asp:Button ID="btn_AddNewShipAddress" runat="server" Text="+ ADD NEW SHIP ADDRESS"
                                    CssClass="btn btn-success btn-xs" OnClick="btn_AddNewShipAddress_Click" />
                            </div>
                        </li>
                    </ul>
                </td>
            </tr>
            <tr id="div_BillAddress" style="display: none;">
                <td colspan="4">
                    <table cellspacing="0" rules="all" border="1" class="tableCss">
                        <tr>
                            <th class="spanCss">
                                <span>Company Name</span><br />
                                <span style="font-size: 10px;">For Invoice/Documents</span>
                            </th>
                            <td>
                                <asp:TextBox ID="txt_CompanyName_Bill" Enabled="false" runat="server" ClientIDMode="Static"
                                    CssClass="form-control"></asp:TextBox>
                            </td>
                            <th class="spanCss">
                                Email ID
                            </th>
                            <td>
                                <asp:TextBox ID="txt_EmailID_Bill" runat="server" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th class="spanCss" rowspan="2">
                                Address
                            </th>
                            <td rowspan="2">
                                <asp:TextBox ID="txt_Address_Bill" runat="server" ClientIDMode="Static" Height="90px"
                                    Width="350px" CssClass="form-control"></asp:TextBox>
                            </td>
                            <th class="spanCss">
                                Pincode
                            </th>
                            <td>
                                <asp:TextBox ID="txt_pincode_Bill" runat="server" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th class="spanCss">
                                Contact
                            </th>
                            <td>
                                <asp:TextBox ID="txt_Contact_Bill" runat="server" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th class="spanCss">
                                Country
                            </th>
                            <td>
                                <asp:TextBox ID="txt_Country_Bill" runat="server" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                            </td>
                            <th class="spanCss">
                                State
                            </th>
                            <td>
                                <asp:TextBox ID="txt_State_Bill" runat="server" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th class="spanCss">
                                City
                            </th>
                            <td>
                                <asp:TextBox ID="txt_City_Bill" runat="server" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                            </td>
                            <th class="spanCss">
                                Fax No
                            </th>
                            <td>
                                <asp:TextBox ID="txt_FaxNo_Bill" runat="server" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th class="spanCss">
                                Phone No
                            </th>
                            <td>
                                <asp:TextBox ID="txt_PhoneNo_Bill" runat="server" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                            </td>
                            <th class="spanCss">
                                Mobile No
                            </th>
                            <td>
                                <asp:TextBox ID="txt_MobileNo_Bill" runat="server" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="div_ShipAddress" style="display: none;">
                <td colspan="4">
                    <table cellspacing="0" rules="all" border="1" class="tableCss">
                        <tr>
                            <th class="spanCss">
                                <span class="spanCss">Company Name</span><br />
                                <span style="font-size: 10px;">For Invoice/Documents</span>
                            </th>
                            <td>
                                <asp:TextBox ID="txt_CompanyName_Ship" Enabled="false" runat="server" ClientIDMode="Static"
                                    CssClass="form-control"></asp:TextBox>
                            </td>
                            <th class="spanCss">
                                Email ID
                            </th>
                            <td>
                                <asp:TextBox ID="txt_EmailID_Ship" runat="server" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th class="spanCss" rowspan="2">
                                Address
                            </th>
                            <td rowspan="2">
                                <asp:TextBox ID="txt_Address_Ship" runat="server" Height="90px" Width="350px" CssClass="form-control"></asp:TextBox>
                            </td>
                            <th class="spanCss">
                                Pincode
                            </th>
                            <td>
                                <asp:TextBox ID="txt_pincode_Ship" runat="server" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th class="spanCss">
                                Contact
                            </th>
                            <td>
                                <asp:TextBox ID="txt_Contact_Ship" runat="server" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th class="spanCss">
                                Country
                            </th>
                            <td>
                                <asp:TextBox ID="txt_Country_Ship" runat="server" CssClass="form-control"></asp:TextBox>
                            </td>
                            <th class="spanCss">
                                State
                            </th>
                            <td>
                                <asp:TextBox ID="txt_State_Ship" runat="server" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th class="spanCss">
                                City
                            </th>
                            <td>
                                <asp:TextBox ID="txt_City_Ship" runat="server" CssClass="form-control"></asp:TextBox>
                            </td>
                            <th class="spanCss">
                                Fax No
                            </th>
                            <td>
                                <asp:TextBox ID="txt_FaxNo_Ship" runat="server" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th class="spanCss">
                                Phone No
                            </th>
                            <td>
                                <asp:TextBox ID="txt_PhoneNo_Ship" runat="server" CssClass="form-control"></asp:TextBox>
                            </td>
                            <th class="spanCss">
                                Mobile No
                            </th>
                            <td>
                                <asp:TextBox ID="txt_MobileNo_Ship" runat="server" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblErrMsg" runat="server" ClientIDMode="Static" Font-Size="14px" ForeColor="Red"
                        Font-Bold="true"></asp:Label>
                </td>
                <td>
                    <asp:Button ID="btn_SaveRecord" runat="server" Text="Save Record" CssClass="btn btn-success"
                        OnClientClick="javascript:return cntrlSaveCheck();" OnClick="btn_SaveRecord_Click" />
                </td>
                <td>
                    <asp:Button ID="btn_ClearRecord" runat="server" Text="Clear Selection" CssClass="btn btn-info" />
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField ID="hdnType" runat="server" Value="" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnIDforBill" runat="server" Value="" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnIDforShip" runat="server" Value="" ClientIDMode="Static" />
    <script type="text/javascript">
        $(function () {
            if ($('#hdnType').val() == "new") {
                $('#txt_CompanyName_Ship').prop('disabled', false);
                $('#txt_CompanyName_Bill').prop('disabled', false);
                $('li#li_BillasShip').show();
                $('li#li_AddNewShipAddress').hide();
                EnableDisableDiv("#div_BillAddress");
                $('#a_BillAddress').addClass('aTagSelect');
            }
            else if ($('#hdnType').val() == "modify") {
                $('#txt_CompanyName_Ship').prop('disabled', true);
                $('#txt_CompanyName_Bill').prop('disabled', true);
                $('li#li_BillasShip').hide();
                $('li#li_AddNewShipAddress').show();
            }
            $('a#a_BillAddress').click(function () {
                EnableDisableDiv("#div_BillAddress");
                $(this).addClass('aTagSelect');
            });
            $('a#a_ShipAddress').click(function () {
                if ($('#txt_CompanyName_Bill').val() == "")
                    alert('Please Fill Bill Address first');
                else {
                    EnableDisableDiv("#div_ShipAddress");
                    $(this).addClass('aTagSelect');
                }
            });
        });
        function EnableDisableDiv(CntrlID) {
            $('#div_BillAddress').hide();
            $('#div_ShipAddress').hide();
            $(CntrlID).css({ 'display': 'block' }); ;
        }
        function DisableTypeDiv(type) {
            if (type == "block") {
                $('#div_ShipAddress *').prop('disabled', false);
                $('#div_ShipAddress *').css({ 'cursor': 'pointer' });
            }
            else if (type == "none") {
                $('#div_ShipAddress *').prop('disabled', true);
                $('#div_ShipAddress *').css({ 'cursor': 'no-drop' });
            }
            $('#div_ShipAddress').show();
        }
        function DisableBillAddressDiv() {
            $('#div_BillAddress *').prop('disabled', true);
            $('#div_BillAddress *').css({ 'cursor': 'no-drop' });
            $('#div_ShipAddress').show();
        }
        function cntrlSaveCheck() {
            var errMsg = "";
            var emailpattern = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
            $('#lblErrMsg').html("");
            if ($('#ddl_CustomerSelection option:selected').text() == "CHOOSE")
                errMsg += "Choose Customer <br/>";
            if ($('#ddl_UserID option:selected').text() == "CHOOSE")
                errMsg += "Choose User ID <br/>";
            if ($('#hdnType').val() == "modify") {
                if ($('#ddl_BillingAddress option:selected').text() == "CHOOSE")
                    errMsg += "Choose Billing Address <br/>";
                if ($('#ddl_ShippingAddress option:selected').text() == "CHOOSE")
                    errMsg += "Choose Shipping Address <br/>";
            }
            if ($('#txt_CompanyName_Bill').val() == "")
                errMsg += 'Enter Company Name<br/>';
            if (emailpattern.test($('#txt_EmailID_Bill').val()) == false)
                errMsg += 'Enter Valid Email <br />';
            if (emailpattern.test($('#txt_EmailID_Ship').val()) == false)
                errMsg += 'Enter Valid Email <br />';
            if ($('#txt_Contact_Bill').val() == "")
                errMsg += 'Enter Attention Person<br/>';
            if ($('#txt_State_Bill').val() == "")
                errMsg += 'Enter State<br/>';
            if ($('#txt_City_Bill').val() == "")
                errMsg += 'Enter City<br/>';
            if ($('#txt_PhoneNo_Bill').val() == "" && $('#txt_MobileNo_Bill').val() == "")
                errMsg += 'Enter Phone No or Mobile No<br/>';
            if (errMsg.length > 0) {
                $('#lblErrMsg').html(errMsg);
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
