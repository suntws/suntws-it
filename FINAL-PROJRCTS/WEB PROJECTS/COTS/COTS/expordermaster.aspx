<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="expordermaster.aspx.cs" Inherits="COTS.expordermaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="styles/datemonthyear.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        th
        {
            line-height: 15px;
            text-align: left;
            font-weight: normal;
            background-color: #f1f1f1;
        }
        td
        {
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div align="center" class="pageTitleHead">
        Order Master Details
    </div>
    <div class="contPage">
        <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #07A714;
            width: 100%;">
            <tr>
                <th>
                    ORDER NO
                </th>
                <td>
                    <asp:Label runat="server" ID="lblOrderRefNo" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                </td>
                <th>
                    DESIRED SHIPPING DATE
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtDesiredDate" ClientIDMode="Static" CssClass="form-control"
                        Font-Bold="true"></asp:TextBox>
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
                    PACKING METHOD
                </th>
                <td>
                    <asp:RadioButtonList runat="server" ID="rdbPackingMethod" ClientIDMode="Static" RepeatColumns="4"
                        RepeatDirection="Horizontal">
                    </asp:RadioButtonList>
                    <span style="display: none; width: 450px; float: right;" class="divMethodOthers"><span
                        style="color: #ff0000; width: 60px; float: left; line-height: 35px;">METHOD:</span>
                        <asp:TextBox runat="server" ID="txtPackingOthers" ClientIDMode="Static" Text="" CssClass="form-control"></asp:TextBox>
                    </span>
                </td>
                <th rowspan="2">
                    FREIGHT ADDRESS
                </th>
                <td rowspan="2">
                    <asp:TextBox runat="server" ID="txtDeliveryAddress" ClientIDMode="Static" Text=""
                        Width="450px" Height="80px" TextMode="MultiLine" onKeyUp="javascript:CheckMaxLength(this, 999);"
                        onChange="javascript:CheckMaxLength(this, 999);" CssClass="form-control"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    DELIVERY METHOD
                </th>
                <td>
                    <asp:RadioButtonList runat="server" ID="rdbDeliveryTerms" ClientIDMode="Static" RepeatColumns="2"
                        RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rdbDeliveryTerms_IndexChange">
                        <asp:ListItem Text="OCEAN FREIGHT" Value="OCEAN FREIGHT"></asp:ListItem>
                        <asp:ListItem Text="AIR FREIGHT" Value="AIR FREIGHT"></asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:RadioButtonList runat="server" ID="rdbDeliveryMethod" ClientIDMode="Static"
                        RepeatColumns="5" RepeatDirection="Horizontal" BackColor="#f0ff71">
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <th>
                    COUNTRY OF FINAL DESTINATION
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtCoutryDestination" ClientIDMode="Static" Text=""
                        Width="265px" MaxLength="20" CssClass="form-control"></asp:TextBox>
                </td>
                <th>
                    FINAL DESTINATION
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtFinalDestination" ClientIDMode="Static" Text=""
                        Width="300px" MaxLength="50" CssClass="form-control"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    STATIONARY REQUIREMENTS
                </th>
                <td>
                    <asp:GridView runat="server" ID="gv_DocList" AutoGenerateColumns="false" AlternatingRowStyle-BackColor="#f5f5f5"
                        Width="210px">
                        <HeaderStyle CssClass="headerNone" />
                        <Columns>
                            <asp:BoundField DataField="DocName" />
                        </Columns>
                    </asp:GridView>
                </td>
                <th>
                    OTHER DOCUMETNS REQUIREMENTS
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtOtherDocuments" ClientIDMode="Static" Text=""
                        Width="340px" Height="70px" TextMode="MultiLine" onKeyUp="javascript:CheckMaxLength(this, 999);"
                        onChange="javascript:CheckMaxLength(this, 999);" CssClass="form-control"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    PAYMENT TERMS
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtPayTerms" ClientIDMode="Static" TextMode="MultiLine"
                        Width="450px" Height="120px" Text="" Enabled="false" CssClass="form-control"></asp:TextBox>
                </td>
                <th>
                    DISCOUNT
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtInstructions" ClientIDMode="Static" TextMode="MultiLine"
                        Width="450px" Height="120px" Text="" Enabled="false" CssClass="form-control"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    SPECIAL INSTRUCTION
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtSplIns" ClientIDMode="Static" TextMode="MultiLine"
                        Height="80px" CssClass="form-control" onKeyUp="javascript:CheckMaxLength(this, 3999);"
                        onChange="javascript:CheckMaxLength(this, 3999);"></asp:TextBox>
                </td>
                <th>
                    SPECIAL NOTES
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtSplReq" ClientIDMode="Static" TextMode="MultiLine"
                        Height="80px" CssClass="form-control" onKeyUp="javascript:CheckMaxLength(this, 3999);"
                        onChange="javascript:CheckMaxLength(this, 3999);"></asp:TextBox>
                </td>
            </tr>
            <tr style="text-align: center;">
                <td colspan="3">
                    <asp:Label runat="server" ID="lblErrMsg" Text="" ClientIDMode="Static" ForeColor="Red"
                        Font-Bold="true"></asp:Label>
                </td>
                <td>
                    <asp:Button runat="server" ID="btnSendOrder" ClientIDMode="Static" Text="SEND ORDER"
                        CssClass="btn btn-success" OnClientClick="javascript:return CtrlExpSendOrder()"
                        OnClick="btnSendOrder_Click" />
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnBillID" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnShipID" ClientIDMode="Static" Value="" />
    <script src="scripts/datemonthyear.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#txtDesiredDate").datepicker({ minDate: "+1D", maxDate: "+360D" }).keydown(function (e) { e.keyWhich = 0; e.keyCode = 0; e.preventDefault(); });

            $("input:radio[id*=rdbPackingMethod_]").click(function () {
                PackingMehtod();
            });
        });
        function PackingMehtod() {
            $('.divMethodOthers').css({ "display": "none" });
            $('#txtPackingOthers').val('');
            if ($("input:radio[id*=rdbPackingMethod_]:checked").val() == 'OTHERS')
                $('.divMethodOthers').css({ "display": "block" });
        }

        function CtrlExpSendOrder() {
            var errmsg = ''; $('#lblErrMsg').html('');
            if ($('#txtDesiredDate').val().length == 0)
                errmsg += 'Enter desired shipping date</br>';
            if ($('#hdnBillID').val().length == 0)
                errmsg += 'Choose billing address<br/>';
            if ($('#hdnShipID').val().length == 0)
                errmsg += 'Choose dispatch address<br/>';
            if ($("input:radio[id*='rdbPackingMethod_']:checked").length == 0)
                errmsg += 'Choose pacikng menthod</br>';
            else if ($("input:radio[id*='rdbPackingMethod_']:checked").val() == "OTHERS") {
                if ($('#txtPackingOthers').val().length == 0)
                    errmsg += 'Enter Other packing method</br>';
            }
            if ($("input:radio[id*='rdbDeliveryTerms_']:checked").length == 0)
                errmsg += 'Choose delivery menthod</br>';
            if ($("input:radio[id*='rdbDeliveryMethod_']:checked").length == 0)
                errmsg += 'Choose delivery type</br>';
            if ($("#txtDeliveryAddress").val().length == 0)
                errmsg += 'Enter freight address details</br>';
            if ($("#txtCoutryDestination").val().length == 0)
                errmsg += 'Enter country of final destination</br>';
            if ($("#txtFinalDestination").val().length == 0)
                errmsg += 'Enter final destination</br>';
            if (errmsg.length > 0) {
                $('#lblErrMsg').html(errmsg);
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
