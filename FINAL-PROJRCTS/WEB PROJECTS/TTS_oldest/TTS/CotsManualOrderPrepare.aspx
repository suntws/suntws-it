<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CotsManualOrderPrepare.aspx.cs"
    MasterPageFile="~/master.Master" Inherits="TTS.CotsManualOrderPrepare" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/datemonthyear.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/cotsScript.js" type="text/javascript"></script>
    <style type="text/css">
        .Clrbutton
        {
            background-color: #4CAF50;
            border: none;
            color: white;
            padding: 10px 25px;
            text-align: center;
            text-decoration: none;
            display: inline-block;
            font-size: 16px;
            margin: 4px 2px;
            cursor: pointer;
        }
        .deletebutton
        {
            background-color: #E83D19;
            border: none;
            color: white;
            text-align: center;
            cursor: pointer;
            height: 25px;
            font-weight: bold;
            font-family: Times New Roman;
        }
        .tableCss
        {
            background-color: #dcecfb;
            width: 100%;
            border-color: White;
        }
        .tableCss th
        {
            width: 120px;
            color: #008000;
            font-weight: normal;
            text-align: right;
        }
        .tableCss td
        {
            font-weight: bold;
            text-align: left;
        }
        .tableItems
        {
            background-color: #dcecfb;
            width: 100%;
            border-color: White;
        }
        .tableItems th
        {
            width: 120px;
            color: #008000;
            font-weight: normal;
            text-align: left;
        }
        .tableItems td
        {
            font-weight: bold;
            text-align: left;
        }
        .spanCss
        {
            font-size: 12px;
            font-style: italic;
            width: 101px;
            float: left;
            text-align: right;
        }
        .lblCss
        {
            font-size: 12px;
            font-family: Times New Roman;
            font-weight: bold;
        }
        .itemheadercss
        {
            font-size: 12px;
            color: #614126;
            font-weight: 700;
            font-style: italic;
        }
        input[type=text]:disabled
        {
            background: #ffffff;
            cursor: no-drop;
        }
        select:disabled, textarea:disabled
        {
            background: #ffffff;
            cursor: no-drop;
        }
        input[type=checkbox]:disabled
        {
            cursor: no-drop;
        }
        input[type=radio]:disabled
        {
            cursor: no-drop;
        }
        input[type=button]:disabled
        {
            cursor: no-drop;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        DOMESTIC ORDER ENTRY
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" class="tableCss">
            <tr>
                <th>
                    CUSTOMER
                </th>
                <td>
                    <asp:DropDownList ID="ddl_CustomerName" ClientIDMode="Static" runat="server" Width="500px"
                        Height="30px" ToolTip="Select Customer name" AutoPostBack="true" OnSelectedIndexChanged="ddl_CustomerName_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <th>
                    USER-ID
                </th>
                <td>
                    <asp:DropDownList ID="ddl_UserId" ClientIDMode="Static" AutoPostBack="true" runat="server"
                        Width="200px" Height="30px" ToolTip="Select User Id" OnSelectedIndexChanged="ddl_UserId_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    BILLING
                </th>
                <td colspan="3">
                    <asp:DropDownList ID="ddl_BillingAddress" ClientIDMode="Static" AutoPostBack="true"
                        runat="server" Width="800px" Height="30px" ToolTip="Select Billing Address" OnSelectedIndexChanged="ddl_BillingAddress_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    SHIPPING
                </th>
                <td colspan="3">
                    <asp:DropDownList ID="ddl_ShippingAddress" AutoPostBack="true" ClientIDMode="Static"
                        runat="server" Width="800px" Height="30px" ToolTip="Select Shipping Address"
                        OnSelectedIndexChanged="ddl_ShippingAddress_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <table cellspacing="0" rules="all" border="1" class="tableCss">
                        <tr style="border-bottom: none !important;">
                            <th>
                                BILL TO:
                            </th>
                            <td style="vertical-align: top;">
                                <asp:Label ID="lbl_SelectedBillAddress" Text="" ClientIDMode="Static" runat="server"></asp:Label>
                            </td>
                            <th>
                                SHIP TO:
                            </th>
                            <td style="vertical-align: top;">
                                <asp:Label ID="lbl_SelectedShipAddress" Text="" ClientIDMode="Static" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label runat="server" ID="lbl_BillAddressErr" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:Label runat="server" ID="lbl_ShipAddressErr" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <table cellspacing="0" rules="all" border="1" style="width: 100%; border-color: White;">
                        <tr>
                            <th>
                                ORDER REF NO
                            </th>
                            <td>
                                <asp:TextBox ID="txt_OrderRefNo" ClientIDMode="Static" runat="server" Width="415px"
                                    Height="30px" ToolTip="Enter Order Reference No" MaxLength="100" onkeypress="return splCharNotAllowed(event)"></asp:TextBox>
                                <asp:Label ID="lbl_duplicaterefno" ClientIDMode="Static" runat="server" ForeColor="Red"
                                    Font-Size="12px" CssClass="lblCss" Text=""></asp:Label>
                            </td>
                            <th>
                                DESIRED SHIPPING DATE
                            </th>
                            <td>
                                <asp:TextBox ID="txt_DesiredShippingDate" ClientIDMode="Static" runat="server" Width="250px"
                                    Height="30px" ToolTip="Select Desired Shipping Date"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                FREIGHT CHARGES
                            </th>
                            <td>
                                <asp:RadioButtonList runat="server" ID="rdo_FreightCharges" ClientIDMode="Static"
                                    RepeatColumns="2" RepeatDirection="Horizontal" ToolTip="Select Freight charges">
                                </asp:RadioButtonList>
                            </td>
                            <th>
                                PACKING METHOD
                            </th>
                            <td>
                                <div>
                                    <asp:RadioButtonList runat="server" ID="rdo_PackingMethod" ClientIDMode="Static"
                                        RepeatColumns="3" RepeatDirection="Horizontal" ToolTip="Select Packing Method">
                                    </asp:RadioButtonList>
                                </div>
                                <div id="div_packingMethodOthers" style="display: none; text-align: center;">
                                    <span class="spanCss" style="color: #614126;">ENTER PACKING METHOD:</span>
                                    <asp:TextBox ID="txt_PackingMethodOthers" runat="server" ClientIDMode="Static" Width="285px"
                                        Height="20px" ToolTip="Enter Packing Method"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                DELIVERY METHOD
                            </th>
                            <td>
                                <div>
                                    <asp:RadioButtonList runat="server" ID="rdo_DeliveryMethod" ClientIDMode="Static"
                                        RepeatColumns="3" RepeatDirection="Horizontal">
                                    </asp:RadioButtonList>
                                </div>
                                <div id="div_DeliveryMethodGodownname" style="display: none; text-align: center;">
                                    <span class="spanCss" style="color: #614126;">ENTER GODOWN NAME:</span>
                                    <asp:TextBox ID="txt_transporterGodownName" ClientIDMode="Static" runat="server"
                                        Width="300px" Height="20px" ToolTip="Enter Transporter Godown name"></asp:TextBox>
                                </div>
                            </td>
                            <th>
                                PREPARED TRANSPORTER
                            </th>
                            <td>
                                <asp:TextBox ID="txt_PreparedTransporter" ClientIDMode="Static" runat="server" Width="250px"
                                    Height="30px" ToolTip="Enter Prefered Transporter name"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                SPECIAL INSTRUCTION
                            </th>
                            <td>
                                <asp:TextBox ID="txt_SpecialInstruction" ClientIDMode="Static" runat="server" Width="350px"
                                    Height="80px" ToolTip="Enter Special Instructions" TextMode="MultiLine"></asp:TextBox>
                            </td>
                            <th>
                                SPECIAL NOTES
                            </th>
                            <td>
                                <asp:TextBox ID="txt_SpecialRequest" runat="server" ClientIDMode="Static" Width="350px"
                                    Height="80px" ToolTip="Enter Special Notes" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                GST VALUE
                            </th>
                            <td colspan="3">
                                <table id="tblGST">
                                    <tr>
                                        <td style="background-color: #F1CED5;">
                                            <div style="float: left; background-color: #ccc; width: 75px;">
                                                <asp:CheckBox runat="server" ID="chkCGST" ClientIDMode="Static" Checked="false" Text="CGST %" />
                                            </div>
                                            <div id="divCGST" style="display: none; float: left;">
                                                <asp:TextBox runat="server" ID="txtCGST" ClientIDMode="Static" Text="" Width="60px"
                                                    onkeypress="return isNumberKey(event)" MaxLength="5" CssClass="txtCss"></asp:TextBox></div>
                                        </td>
                                        <td style="background-color: #BCE0B8;">
                                            <div style="float: left; background-color: #ccc; width: 75px;">
                                                <asp:CheckBox runat="server" ID="chkSGST" ClientIDMode="Static" Checked="false" Text="SGST %" /></div>
                                            <div id="divSGST" style="display: none; float: left;">
                                                <asp:TextBox runat="server" ID="txtSGST" ClientIDMode="Static" Text="" Width="60px"
                                                    onkeypress="return isNumberKey(event)" MaxLength="5" CssClass="txtCss"></asp:TextBox></div>
                                        </td>
                                        <td style="background-color: #D2C9E8;">
                                            <div style="float: left; background-color: #ccc; width: 75px;">
                                                <asp:CheckBox runat="server" ID="chkIGST" ClientIDMode="Static" Checked="false" Text="IGST %" /></div>
                                            <div id="divIGST" style="display: none; width: 100px; float: left;">
                                                <asp:TextBox runat="server" ID="txtIGST" ClientIDMode="Static" Text="" Width="60px"
                                                    onkeypress="return isNumberKey(event)" MaxLength="5" CssClass="txtCss"></asp:TextBox>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblErrMsg1" runat="server" ClientIDMode="Static" ForeColor="Red" CssClass="lblCss"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: right; padding-right: 20px;">
                                <asp:Button ID="btnSaveCustDetails" runat="server" Text="SAVE & ADD ITEMS" ClientIDMode="Static"
                                    CssClass="Clrbutton" OnClientClick="javascript:return CntrlSave();" OnClick="btnSaveCustDetails_Click" />
                            </td>
                            <td colspan="2" style="text-align: left; padding-left: 20px;">
                                <span class="Clrbutton" style="background-color: #ff000c;" onclick="fnPageReload()">
                                    CLEAR SELECTION</span>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnCustomerName" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnMajorDataVerified" ClientIDMode="Static" Value="" />
    <script src="Scripts/datemonthyear.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#txt_DesiredShippingDate").datepicker({ minDate: "+0D", maxDate: "+360D" }).keydown(function (e) { e.keyWhich = 0; e.keyCode = 0; e.preventDefault(); });
            //Show and  Hide packing method other option
            $('#rdo_PackingMethod').change(function () {
                if ($('#rdo_PackingMethod :checked').val() == 'OTHERS')
                    $('#div_packingMethodOthers').fadeIn('slow');
                else
                    $('#div_packingMethodOthers').fadeOut('slow');
            });
            //Show and  Hide Delivery method transporter godown option
            $('#rdo_DeliveryMethod').change(function () {
                if ($('#rdo_DeliveryMethod :checked').val() == 'TRANSPORTERS GODOWN')
                    $('#div_DeliveryMethodGodownname').fadeIn('slow');
                else
                    $('#div_DeliveryMethodGodownname').fadeOut('slow');
            });
            //Check Duplicate OrderrefNo
            $('#txt_OrderRefNo').blur(function () {
                var orderno = $('#txt_OrderRefNo').val();
                var custcode = $('#ddl_UserId option:selected').val();
                $('#lbl_duplicaterefno').html('');
                $.ajax({ type: "POST", url: "BindRecords.aspx?type=ChkOrderNo&chkrefno=" + orderno + "&cotscode=" + custcode + "", context: document.body,
                    success: function (data) {
                        if (data != '') { $('#lbl_duplicaterefno').html(data); $('#txt_OrderRefNo').focus(); return false; }
                        else { $('#lbl_duplicaterefno').html(''); return true; }
                    }
                });
            });

            //Enable checkboxes for GST Value
            $("input:checkbox[id*=chk]").click(function (e) {
                var ctrlID = e.target.id;
                BindTextboxesGST(ctrlID)
            });

            //Enable checkboxes for GST Value
            $("input:checkbox[id*=chk]").bind(function (e) {
                var ctrlID = e.target.id;
                BindTextboxesGST(ctrlID)
            });

            //unCheck all checkboxes by default
            $("input:checkbox[id*=chk]").each(function () {
                $(this).removeAttr("checked");
            });

            $('#ddl_CustomerName').change(function () {
                $('#hdnCustomerName').val($('#ddl_CustomerName option:selected').text());
            });
        });
        //function to Bind checkboxes GST
        function BindTextboxesGST(ctrlID) {
            if (ctrlID == "chkCGST")
                chktxtEnableDisable('chkCGST', 'divCGST', 'txtCGST');
            if (ctrlID == "chkSGST")
                chktxtEnableDisable('chkSGST', 'divSGST', 'txtSGST');
            if (ctrlID == "chkIGST")
                chktxtEnableDisable('chkIGST', 'divIGST', 'txtIGST');
        }
        //disable special characters for orderrefno textbox
        function splCharNotAllowed(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode == 34 || charCode == 38 || charCode == 39 || charCode == 42 || charCode == 44 || charCode == 46 || charCode == 47 || charCode == 58 || charCode == 59 ||
            charCode == 60 || charCode == 62 || charCode == 63 || charCode == 91 || charCode == 92 || charCode == 93 || charCode == 94 || charCode == 96 || charCode == 123 ||
            charCode == 124 || charCode == 125 || charCode == 126)
                return false;

            return true;
        }

        //enable and disable GST textboxes based on checkbox
        function chktxtEnableDisable(chkID, divID, txtID) {
            $('#' + divID).css({ 'display': 'none' });
            if ($('#' + chkID).attr('checked') == "checked") {
                $('#' + divID).css({ 'display': 'block' });
                $('#' + txtID).focus();
            }
        }
        //function to check save button
        function CntrlSave() {
            var ErrMsg = "";
            $('#lblErrMsg1').html('');
            //check for Customer Address Details
            if ($('#ddl_CustomerName option:selected').val() == "" || $('#ddl_CustomerName option:selected').val() == "CHOOSE")
                ErrMsg += "Select Customer Name <br/>"
            if ($('#ddl_UserId option:selected').val() == "" || $('#ddl_UserId option:selected').val() == "Select")
                ErrMsg += "Select Customer User Id <br/>"
            if ($('#ddl_BillingAddress option:selected').val() == "" || $('#ddl_BillingAddress option:selected').val() == "CHOOSE")
                ErrMsg += "Select Customer Billing Address <br/>"
            if ($('#ddl_ShippingAddress option:selected').val() == "" || $('#ddl_ShippingAddress option:selected').val() == "CHOOSE")
                ErrMsg += "Select Customer Shipping Address <br/>"

            //check for orderref no
            if ($('#txt_OrderRefNo').val().length == 0)
                ErrMsg += "Enter Order Reference No <br/>"
            else if ($('#txt_OrderRefNo').val().length > 0) {
                var refvalue = $('#txt_OrderRefNo').val();
                if (~refvalue.indexOf(",") || ~refvalue.indexOf("/") || ~refvalue.indexOf("\\") || ~refvalue.indexOf(":")
                    || ~refvalue.indexOf("*") || ~refvalue.indexOf("?") || ~refvalue.indexOf("<") || ~refvalue.indexOf(">")
                    || ~refvalue.indexOf("|") || ~refvalue.indexOf(";") || ~refvalue.indexOf("&") || ~refvalue.indexOf(".")
                    || ~refvalue.indexOf("'"))
                    ErrMsg += "Special characters are not allowed in order reference No.</br>";
            }
            //check shipping date,packing,delivermethod,prefered transporter
            if ($('#txt_DesiredShippingDate').val().length == 0)
                ErrMsg += "Select Desired Shipping date <br/>"
            if ($('input:radio[id*=rdo_FreightCharges ]:checked').length == 0)
                ErrMsg += 'Choose Freight charges <br>';
            if ($('#rdo_PackingMethod :checked').length == 0)
                ErrMsg += 'Choose Packing method <br>';
            if ($('#rdo_PackingMethod :checked').val() == 'OTHERS') {
                if ($('#txt_PackingMethodOthers').val().length == 0)
                    ErrMsg += 'Enter Packing method <br>';
            }
            if ($('#rdo_DeliveryMethod :checked').length == 0)
                ErrMsg += 'Choose Delivery method <br>';
            if ($('#rdo_DeliveryMethod :checked').val() == 'TRANSPORTERS GODOWN') {
                if ($('#txt_transporterGodownName').val().length == 0)
                    ErrMsg += 'Enter Delivery method <br>';
            }
            if ($('#txt_PreparedTransporter').val().length == 0)
                ErrMsg += "Enter Prefered Transporter <br/>"

            //check for GST related values
            var Shipid = $('#ddl_ShippingAddress option:selected').val();
            if (Shipid != 189 && Shipid != 960 && Shipid != 1071 && Shipid != 1076 && Shipid != 1168 && Shipid != 2340 && Shipid != 4594 &&
            Shipid != 4616 && Shipid != 4824 && Shipid != 8016 && Shipid != 8033 && Shipid != 8044 && Shipid != 11193 && Shipid != 11262 &&
            Shipid != 11429 && Shipid != 11432 && Shipid != 4892 && Shipid != 12620 && Shipid != 12728) {
                if ($("input:checkbox[id*=chk]:checked").length == 0)
                    ErrMsg += "Check atleast one GST Value<br/>";
                if ($('#chkCGST').attr('checked') == 'checked' && $('#txtCGST').val().length == 0)
                    ErrMsg += "Enter CGST %<br/>";
                else if ($('#chkCGST').attr('checked') == 'checked' && parseFloat($('#txtCGST').val()) == 0)
                    ErrMsg += "CGST Value must greater than 0<br/>";
                if ($('#chkSGST').attr('checked') == 'checked' && $('#txtSGST').val().trim().length == 0)
                    ErrMsg += "Enter SGST %<br/>";
                else if ($('#chkSGST').attr('checked') == 'checked' && parseFloat($('#txtSGST').val()) == 0)
                    ErrMsg += "SGST Value must be greater than 0<br/>";
                if ($('#chkIGST').attr('checked') == 'checked' && $('#txtIGST').val().length == 0)
                    ErrMsg += "Enter IGST %<br/>";
                else if ($('#chkIGST').attr('checked') == 'checked' && parseFloat($('#txtIGST').val()) == 0)
                    ErrMsg += "IGST Value must be greater than 0 <br/>";
            }
            if ($('#lbl_BillAddressErr').html().length > 0)
                ErrMsg += 'Confirm the billing address Mobile/Phone/Pincode/Gst No<br/>';
            if ($('#lbl_ShipAddressErr').html().length > 0)
                ErrMsg += 'Confirm the shipping address Mobile/Phone/Pincode/Gst No<br/>';

            if (ErrMsg.length > 0) {
                $('#lblErrMsg1').html(ErrMsg);
                return false;
            }
            else
                return true;
        }
        function fnPageReload() {
            window.location.href = window.location.href;
        }
        function save_MajorDataVerified(addrID, lblID) {
            $.ajax({ url: "cotsmanualorderprepare.aspx/Update_MajorData_Confirmation", data: "{addrID:\"" + addrID + "\"}",
                type: "POST", contentType: "application/json; charset=utf-8", dataType: "JSON",
                success: function (result) {
                    if (result.d == "SUCCESS") { $('#' + lblID).html(''); }
                    else if (result.d == "NOT UPDATE") {
                        alert(result.d + ". KINDLY UPDATE THE DATA FROM ADDRESS CREATION MASTER");
                        var pathname = window.location.href.toLowerCase();
                        var splitval = pathname.split('/cotsmanualorderprepare.aspx');
                        if (lblID == "lbl_BillAddressErr")
                            window.location.href = splitval[0].toString() + "/cotsdomesticaddress.aspx?qid=2&cid=";
                        else if (lblID == "lbl_ShipAddressErr")
                            window.location.href = splitval[0].toString() + "/cotsdomesticaddress.aspx?qid=1&cid=";
                    }
                }
            });
        }
    </script>
</asp:Content>
