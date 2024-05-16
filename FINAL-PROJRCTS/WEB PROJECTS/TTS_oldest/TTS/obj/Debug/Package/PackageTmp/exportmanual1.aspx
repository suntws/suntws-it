<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="exportmanual1.aspx.cs" Inherits="TTS.exportmanual1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="styles/datemonthyear.css" rel="stylesheet" type="text/css" />
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: center;" class="pageTitleHead">
        EXPORT ORDER ENTRY
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="background-color: #dcecfb; width: 1106px;
            border-color: White; border-collapse: separate;">
            <tr>
                <th class="spanCss">
                    CUSTOMER
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="ddlCotsCustName" ClientIDMode="Static" Width="430px"
                        OnSelectedIndexChanged="ddlCotsCustName_IndexChange" AutoPostBack="true" CssClass="form-control">
                    </asp:DropDownList>
                </td>
                <th class="spanCss">
                    USER ID
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="ddlLoginUserName" ClientIDMode="Static" Width="200px"
                        OnSelectedIndexChanged="ddlLoginUserName_IndexChange" AutoPostBack="true" CssClass="form-control">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th class="spanCss">
                    BILLING
                </th>
                <td colspan="3">
                    <asp:DropDownList runat="server" ID="ddlBillingAddress" ClientIDMode="Static" Width="800px"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlBillingAddress_IndexChange" CssClass="form-control">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th class="spanCss">
                    SHIPPING
                </th>
                <td colspan="3">
                    <asp:DropDownList runat="server" ID="ddlShippingAddress" ClientIDMode="Static" Width="800px"
                        CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlShippingAddress_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th class="spanCss">
                    BILL TO
                </th>
                <td>
                    <asp:Label ID="lblBillAddress" Text="" ClientIDMode="Static" runat="server"></asp:Label>
                </td>
                <th class="spanCss">
                    SHIP TO
                </th>
                <td>
                    <asp:Label ID="lblShipDetails" Text="" ClientIDMode="Static" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <th class="spanCss">
                    ORDER REF NO
                </th>
                <td>
                    <asp:TextBox ID="txtOrderRefNo" CssClass="form-control" ClientIDMode="Static" runat="server"
                        Width="415px" Height="30px" ToolTip="Enter Order Reference No" MaxLength="100"
                        onkeypress="return splCharNotAllowed(event)"></asp:TextBox>
                    <asp:Label ID="lbl_duplicaterefno" ClientIDMode="Static" runat="server" ForeColor="Red"
                        Font-Size="12px" Text=""></asp:Label>
                </td>
                <th class="spanCss">
                    DESIRED SHIPPING DATE
                </th>
                <td>
                    <asp:TextBox ID="txtDesiredDate" CssClass="form-control" ClientIDMode="Static" runat="server"
                        Width="250px" Height="30px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th class="spanCss">
                    SPECIAL INSTRUCTION
                </th>
                <td>
                    <asp:TextBox ID="txtSplIns" CssClass="form-control" ClientIDMode="Static" runat="server"
                        Width="350px" Height="80px" ToolTip="Enter Special Instructions" TextMode="MultiLine"
                        onKeyUp="javascript:CheckMaxLength(this, 3999);" onChange="javascript:CheckMaxLength(this, 3999);"></asp:TextBox>
                </td>
                <th class="spanCss">
                    SPECIAL NOTES
                </th>
                <td>
                    <asp:TextBox ID="txtSplReq" CssClass="form-control" runat="server" ClientIDMode="Static"
                        Width="350px" Height="80px" ToolTip="Enter Special Notes" TextMode="MultiLine"
                        onKeyUp="javascript:CheckMaxLength(this, 3999);" onChange="javascript:CheckMaxLength(this, 3999);"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblErrMsg" runat="server" ClientIDMode="Static" ForeColor="Red"></asp:Label>
                </td>
                <td>
                    <asp:Button ID="btnNext" runat="server" Text="SAVE & ADD ITEMS" ClientIDMode="Static"
                        CssClass="btn btn-success" Font-Bold="true" OnClientClick="javascript:return ctrlValidation();"
                        OnClick="btnNext_Click" />
                </td>
                <td>
                    <asp:Button ID="btnClearOrder" runat="server" Text="CLEAR SELECTION" ClientIDMode="Static"
                        CssClass="btn btn-info" Font-Bold="true" OnClientClick="javascript:return ctrlClear();" />
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnCotsCustID" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnLoginName" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnFullName" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnBillID" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnShipID" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdncustcodeStd" ClientIDMode="Static" Value="" />
    <script src="scripts/datemonthyear.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#ddlCotsCustName").change(function () {
                $('#hdnFullName').val($('#ddlCotsCustName option:selected').text());
                $('#hdncustcodeStd').val($('#ddlCotsCustName option:selected').val());
            });
            $('#ddlLoginUserName').change(function () {
                $('#hdnLoginName').val($('#ddlLoginUserName option:selected').text());
                $('#hdnCotsCustID').val($('#ddlLoginUserName option:selected').val());
            });
            $('#ddlBillingAddress').change(function () {
                $('#hdnBillID').val($('#ddlBillingAddress option:selected').val());
                $('#lblBillAddress').html('');
            });
            $('#ddlShippingAddress').change(function () {
                $('#hdnShipID').val($('#ddlShippingAddress option:selected').val());
                $('#lblShipDetails').html('');
                if ($('#ddlShippingAddress option:selected').text() != 'Choose' && $('#lblShipDetails').html().length > 0)
                    $('#txtOrderRefNo').focus();
            });

            $("#txtDesiredDate").datepicker({ minDate: "+1D", maxDate: "+360D" }).keydown(function (e) { e.keyWhich = 0; e.keyCode = 0; e.preventDefault(); });

            $('#txtOrderRefNo').blur(function () {
                $('#lblErrMsg').html('');
                var orderno = $('#txtOrderRefNo').val();
                var custcode = $('#hdnCotsCustID').val();
                $.ajax({ type: "POST", url: "BindRecords.aspx?type=ChkOrderNo&chkrefno=" + orderno + "&cotscode=" + custcode + "", context: document.body,
                    success: function (data) { if (data != '') { $('#txtOrderRefNo').focus(); $('#lbl_duplicaterefno').html(data).css({ "color": "#f00" }); } }
                });
            });
        });

        function ctrlValidation() {
            $('#lblErrMsg').html('');
            var errmsg = '';
            if ($('#ddlCotsCustName option:selected').text() == 'Choose')
                errmsg += 'Choose customer name </br>';
            if ($('#ddlLoginUserName option:selected').text() == 'Choose')
                errmsg += 'Choose user name </br>';
            if ($('#ddlBillingAddress option:selected').text() == 'Choose')
                errmsg += 'Choose billing address </br>';
            if ($('#ddlShippingAddress option:selected').text() == 'Choose')
                errmsg += 'Choose shipping address </br>';

            if ($('#txtOrderRefNo').val().length == 0)
                errmsg += 'Enter order reference No.</br>';
            else if ($('#txtOrderRefNo').val().length > 0) {
                var refvalue = $('#txtOrderRefNo').val();
                if (~refvalue.indexOf(",") || ~refvalue.indexOf("/") || ~refvalue.indexOf("\\") || ~refvalue.indexOf(":") || ~refvalue.indexOf("*") || ~refvalue.indexOf("?") || ~refvalue.indexOf("<")
                    || ~refvalue.indexOf(">") || ~refvalue.indexOf("|") || ~refvalue.indexOf(";") || ~refvalue.indexOf("&") || ~refvalue.indexOf(".")) {
                    errmsg += "Special characters not allowed in order reference No.</br>";
                }
            }
            if ($('#txtDesiredDate').val().length == 0)
                errmsg += 'Enter desired shipping date</br>';

            if (errmsg.length > 0) {
                $('#lblErrMsg').html(errmsg);
                return false;
            }
            else {
                var orderno = $('#txtOrderRefNo').val();
                var custcode = $('#hdnCotsCustID').val();
                $('#lblErrMsg').html('');
                $.ajax({ type: "POST", url: "BindRecords.aspx?type=ChkOrderNo&chkrefno=" + orderno + "&cotscode=" + custcode + "", context: document.body,
                    success: function (data) { if (data != '') { $('#lblErrMsg').html(data).css({ "color": "#f00" }); return false; } else { return true; } }
                });
            }
        }
        function ctrlClear() { window.location.href = window.location.href; return false; }

        function splCharNotAllowed(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode == 34 || charCode == 38 || charCode == 39 || charCode == 42 || charCode == 44 || charCode == 46 || charCode == 47 || charCode == 58 || charCode == 59 ||
            charCode == 60 || charCode == 62 || charCode == 63 || charCode == 91 || charCode == 92 || charCode == 93 || charCode == 94 || charCode == 96 || charCode == 123 ||
            charCode == 124 || charCode == 125 || charCode == 126)
                return false;

            return true;
        }
    </script>
</asp:Content>
