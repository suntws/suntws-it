<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="frmexporderprepare.aspx.cs" Inherits="COTS.frmexporderprepare" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="styles/datemonthyear.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        th
        {
            line-height: 15px;
            text-align: left;
            font-weight: normal;
        }
        td
        {
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div align="center" class="pageTitleHead">
        Order Master Entry
    </div>
    <div class="contPage">
        <table cellspacing="0" rules="all" border="1" style="background-color: #ffffff; width: 100%;
            border-color: #cccbfb; border-collapse: separate; line-height: 30px;">
            <tr>
                <th>
                    ORDER NO.
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtOrderRefNo" ClientIDMode="Static" Text="" Width="400px"
                        MaxLength="350" onkeypress="return splCharNotAllowed(event)" CssClass="form-control"></asp:TextBox>
                    <div style="width: 417px; float: left; font-size: 10px; line-height: 9px;">
                        <span style="color: #f00;">NOTE: </span><span>Special Characters <b>( , / \ : * ? "
                            < > | ; & .(dot) )</b> Not Allowed in Order Ref No.</span>
                    </div>
                </td>
                <th>
                    DESIRED SHIP DATE
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtDesiredDate" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    BILL TO
                </th>
                <td colspan="3">
                    <asp:DataList runat="server" ID="dlBillAddress" ClientIDMode="Static" RepeatColumns="3"
                        RepeatDirection="Horizontal" Width="100%" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Left">
                        <ItemStyle CssClass="" BorderStyle="Solid" BorderColor="#b2b4fd" BorderWidth="1px" />
                        <ItemTemplate>
                            <table cellspacing="5" border="0" style="border-collapse: collapse; width: 99%; line-height: 15px;
                                vertical-align: top;" class="form-table">
                                <tr style="vertical-align: top;">
                                    <td rowspan="5">
                                        <asp:RadioButton runat="server" ID="rdbBill" Text="" AutoPostBack="true" OnCheckedChanged="rdbBill_IndexChaged"
                                            GroupName="Bill" />
                                        <asp:HiddenField runat="server" ID="hdnBill" ClientIDMode="Static" Value='<%# Eval("ID") %>' />
                                    </td>
                                    <td>
                                        <%# Eval("Name") %>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <%# Eval("AddressDetails").ToString().Replace("~", "<br/>")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <%# Eval("StateName")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <%# Eval("Contact")%>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:DataList>
                </td>
            </tr>
            <tr>
                <th>
                    DISPATCH TO
                </th>
                <td colspan="3">
                    <asp:DataList runat="server" ID="dlShipAddress" ClientIDMode="Static" RepeatColumns="3"
                        RepeatDirection="Horizontal" Width="100%" ItemStyle-VerticalAlign="Top">
                        <ItemStyle CssClass="" BorderStyle="Solid" BorderColor="#b2b4fd" BorderWidth="1px" />
                        <ItemTemplate>
                            <table cellspacing="5" border="0" style="border-collapse: collapse; width: 99%; line-height: 15px;
                                vertical-align: top;" class="form-table">
                                <tr style="vertical-align: top;">
                                    <td rowspan="5">
                                        <asp:RadioButton runat="server" ID="rdbShip" Text="" AutoPostBack="true" OnCheckedChanged="rdbShip_IndexChaged"
                                            GroupName="Ship" />
                                        <asp:HiddenField runat="server" ID="hdnShip" ClientIDMode="Static" Value='<%# Eval("ID") %>' />
                                    </td>
                                    <td>
                                        <%# Eval("Name") %>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <%# Eval("AddressDetails").ToString().Replace("~", "<br/>")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <%# Eval("StateName")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <%# Eval("Contact")%>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:DataList>
                </td>
            </tr>
            <tr>
                <th>
                    PAYMENT TERMS
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtPayTerms" ClientIDMode="Static" TextMode="MultiLine"
                        Width="500px" Height="120px" Text="" CssClass="form-control" Enabled="false"></asp:TextBox>
                </td>
                <th>
                    SPECIAL INSTRUCTION
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtSplIns" ClientIDMode="Static" TextMode="MultiLine"
                        Width="500px" Height="120px" CssClass="form-control" MaxLength="3999"></asp:TextBox>
                </td>
            </tr>
            <tr style="text-align: center; vertical-align: bottom;">
                <td colspan="2" style="line-height: 15px;">
                    <asp:Label runat="server" ID="lblErrMsg" Text="" ClientIDMode="Static" ForeColor="Red"
                        Font-Bold="true"></asp:Label>
                </td>
                <td>
                    <asp:Button runat="server" ID="btnNext" ClientIDMode="Static" Text="NEXT" CssClass="btn btn-success"
                        OnClientClick="javascript:return ctrlValidation();" OnClick="btnNext_Click" />
                </td>
                <td>
                    <span class="btn btn-warning" onclick="ctrlClear();">CLEAR</span>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnPriceSheet" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnRatesID" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnBillID" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnShipID" ClientIDMode="Static" Value="" />
    <script src="scripts/datemonthyear.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("input:radio[id*=rdbPackingMethod_]").click(function () {
                $('.divMethodOthers').css({ "display": "none" });
                $('#txtPackingOthers').val('');
                if ($(this).val() == 'OTHERS')
                    $('.divMethodOthers').css({ "display": "block" });
            });
            $("input:radio[id*=rdbDelivery_]").click(function () {
                $('.divDeliveryGodown').css({ "display": "none" });
                $('#txtGodownName').val('');
                if ($(this).val() == 'TRANSPORTERS GODOWN')
                    $('.divDeliveryGodown').css({ "display": "block" });
            });
            $("#txtDesiredDate").datepicker({ minDate: "+1D", maxDate: "+90D" }).keydown(function (e) { e.keyWhich = 0; e.keyCode = 0; e.preventDefault(); });
            $('#txtOrderRefNo').blur(function () {
                $('#lblErrMsg').html('');
                $.ajax({ type: "POST", url: "bindvalues.aspx?type=ChkOrderNo&chkrefno=" + $('#txtOrderRefNo').val() + "", context: document.body,
                    success: function (data) { if (data != '') { $('#lblErrMsg').html(data).css({ "color": "#f00" }); } }
                });
            });
        });

        function ctrlValidation() {
            var errmsg = '';
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
            if ($('#hdnBillID').val().length == 0)
                errmsg += 'Choose billing address<br/>';
            if ($('#hdnShipID').val().length == 0)
                errmsg += 'Choose dispatch address<br/>';
            if (errmsg.length > 0) {
                $('#lblErrMsg').html(errmsg).css({ "color": "#f00" });
                return false;
            }
            else {
                var orderno = $('#txtOrderRefNo').val();
                $('#lblErrMsg').html('');
                $.ajax({ type: "POST", url: "bindvalues.aspx?type=ChkOrderNo&chkrefno=" + orderno + "", context: document.body,
                    success: function (data) { if (data != '') { $('#lblErrMsg').html(data).css({ "color": "#f00" }); return false; } else { return true; } }
                });
            }
        }

        function ctrlClear() {
            window.location.href = window.location.href;
        }
    </script>
</asp:Content>
