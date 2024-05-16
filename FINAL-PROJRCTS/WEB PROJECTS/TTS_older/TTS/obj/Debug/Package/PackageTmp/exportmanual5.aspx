<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="exportmanual5.aspx.cs" Inherits="TTS.exportmanual5" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
    <link href="styles/datemonthyear.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: center;" class="pageTitleHead">
        EXPORT ORDER CONFIRMATION
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <div class="exporderhead">
            <div style="float: left;">
                <asp:Label runat="server" ID="lblCustomer" ClientIDMode="Static" Text=""></asp:Label>
            </div>
            <div style="float: right;">
                <asp:Label runat="server" ID="lblOrderNo" ClientIDMode="Static" Text=""></asp:Label>
            </div>
        </div>
        <hr />
        <span style="font-size: 14px; font-weight: bold; color: #614126; font-style: italic;">
            Confirmed Item List</span>
        <div style="width: 100%;">
            <asp:GridView runat="server" ID="gvOrderItemList" AutoGenerateColumns="false" Width="100%"
                AlternatingRowStyle-BackColor="#f5f5f5" ShowFooter="true" FooterStyle-Font-Bold="true"
                RowStyle-Height="22px">
                <HeaderStyle BackColor="#74bbf9" ForeColor="White" Font-Bold="true" Height="25px"
                    HorizontalAlign="Center" />
                <Columns>
                    <asp:TemplateField HeaderText="Category" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <%# Eval("category") %>
                            <%# Eval("AssyRimstatus").ToString() == "True" ? " (ASSY)" : ""%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Config" HeaderText="Platform" />
                    <asp:BoundField DataField="brand" HeaderText="Brand" />
                    <asp:BoundField DataField="sidewall" HeaderText="Sidewall" />
                    <asp:BoundField DataField="tyretype" HeaderText="Type" />
                    <asp:BoundField DataField="tyresize" HeaderText="Tyre Size" />
                    <asp:BoundField DataField="rimsize" HeaderText="Rim" />
                    <asp:BoundField DataField="listprice" HeaderText="Unit Price" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="itemqty" HeaderText="Qty" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="unitpricepdf" HeaderText="Tot Tyre Price" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="totalfwt" HeaderText="Fwt" ItemStyle-HorizontalAlign="Right" />
                    <asp:TemplateField HeaderText="Rim Price" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <%# Eval("Rimunitprice").ToString() == "0.00" ? "" : Eval("Rimunitprice").ToString()%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Rim Qty" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <%# Eval("Rimitemqty").ToString() == "0" ? "" : Eval("Rimitemqty").ToString()%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Tot Rim Price" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <%# Eval("Rimpricepdf").ToString() == "0.00" ? "" : Eval("Rimpricepdf").ToString()%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Rim Wt" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <%# Eval("totalRimWt").ToString() == "0.00" ? "" : Eval("totalRimWt").ToString()%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:HiddenField runat="server" ID="hdn_ProcessID" Value='<%# Eval("processid") %>' />
                            <asp:Button ID="btnDeleteItem" runat="server" Text="Delete Item" ClientIDMode="Static"
                                CssClass="btn btn-danger" CommandName="Delete" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <FooterStyle BackColor="#b0ceb0" HorizontalAlign="Right" />
            </asp:GridView>
        </div>
        <hr />
        <span style="font-size: 14px; font-weight: bold; color: #614126; font-style: italic;">
            Order Master Details</span>
        <table cellspacing="0" rules="all" border="1" style="background-color: #ecf6ff; border-color: White;
            border-collapse: separate; width: 100%;">
            <tr>
                <th class="spanCss">
                    CURRENCY
                </th>
                <td>
                    <asp:Label runat="server" ID="lblUserCurrency" ClientIDMode="Static" Text=""></asp:Label>
                </td>
                <th class="spanCss">
                    PAYMENT
                </th>
                <td>
                    <asp:Label runat="server" ID="lblPayMentDetails" ClientIDMode="Static" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <th class="spanCss">
                    BILLING
                </th>
                <td colspan="3">
                    <asp:DropDownList ID="ddlBillingAddress" ClientIDMode="Static" AutoPostBack="true"
                        runat="server" CssClass="form-control" Width="700px" ToolTip="Select Billing Address"
                        OnSelectedIndexChanged="ddlBillingAddress_IndexChange">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th class="spanCss">
                    SHIPPING
                </th>
                <td colspan="3">
                    <asp:DropDownList runat="server" ID="ddlShippingAddress" ClientIDMode="Static" CssClass="form-control"
                        AutoPostBack="true" Width="700px" OnSelectedIndexChanged="ddlShippingAddress_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th class="spanCss">
                    BILL TO
                </th>
                <td>
                    <asp:Label ID="lblBillAddress" Text="" Style="width: 100px;" ClientIDMode="Static"
                        runat="server"></asp:Label>
                </td>
                <th class="spanCss">
                    SHIP TO
                </th>
                <td>
                    <asp:Label ID="lblShipDetails" Text="" Style="width: 100px;" ClientIDMode="Static"
                        runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <th class="spanCss">
                    COUNTRY OF FINAL DESTINATION
                </th>
                <td>
                    <asp:TextBox ID="txtCoutryDestination" ClientIDMode="Static" runat="server" CssClass="form-control"
                        ToolTip="Select Desired Shipping Date" MaxLength="50"></asp:TextBox>
                </td>
                <th class="spanCss">
                    PORT OF FINAL DESTINATION
                </th>
                <td>
                    <asp:TextBox ID="txtFinalDestination" ClientIDMode="Static" runat="server" CssClass="form-control"
                        MaxLength="50" ToolTip="Select Desired Shipping Date"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th class="spanCss">
                    DELIVERY METHOD
                </th>
                <td>
                    <div>
                        <asp:RadioButtonList runat="server" ID="rdbDeliveryTerms" ClientIDMode="Static" RepeatColumns="3"
                            RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rdbDeliveryTerms_IndexChange"
                            Width="380px">
                            <asp:ListItem Text="OCEAN FREIGHT" Value="SEA"></asp:ListItem>
                            <asp:ListItem Text="AIR FREIGHT" Value="AIR"></asp:ListItem>
                            <asp:ListItem Text="ROAD WAY FREIGHT" Value="ROAD"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </td>
                <th class="spanCss">
                    PACKING METHOD
                </th>
                <td>
                    <div>
                        <asp:RadioButtonList runat="server" ID="rdbPackingMethod" ClientIDMode="Static" RepeatColumns="3"
                            RepeatDirection="Horizontal" ToolTip="Select Packing Method">
                        </asp:RadioButtonList>
                    </div>
                    <div id="divMethodOthers" style="display: none;">
                        <span style="color: #c1162e;">ENTER PACKING METHOD</span>
                        <asp:TextBox ID="txtPackingOthers" runat="server" ClientIDMode="Static" CssClass="form-control"
                            ToolTip="Enter Packing Method"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <th class="spanCss">
                    DELIVERY TYPE
                </th>
                <td>
                    <div>
                        <asp:RadioButtonList runat="server" ID="rdbDeliveryMethod" ClientIDMode="Static"
                            RepeatColumns="5" RepeatDirection="Horizontal">
                        </asp:RadioButtonList>
                    </div>
                </td>
                <th class="spanCss">
                    DESIRED SHIPPING DATE
                </th>
                <td>
                    <asp:TextBox ID="txtDesiredDate" ClientIDMode="Static" runat="server" CssClass="form-control"
                        ToolTip="Select Desired Shipping Date"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <%--<th class="spanCss">
                    FREIGHT ADDRESS
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtDeliveryAddress" ClientIDMode="Static" Text=""
                        Width="350px" Height="80px" CssClass="form-control" TextMode="MultiLine" onKeyUp="javascript:CheckMaxLength(this, 999);"
                        onChange="javascript:CheckMaxLength(this, 999);"></asp:TextBox>
                </td>--%>
                <th class="spanCss">
                    SHIPPING DOCUMENTS REQ
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
                <th class="spanCss">
                    OTHER DOCS
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtOtherDocuments" ClientIDMode="Static" Text=""
                        PlaceHolder="Any other Documents" Width="350px" Height="80px" CssClass="form-control"
                        TextMode="MultiLine" onKeyUp="javascript:CheckMaxLength(this, 999);" onChange="javascript:CheckMaxLength(this, 999);"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th class="spanCss">
                    SPECIAL INSTRUCTION
                </th>
                <td>
                    <asp:TextBox ID="txtSplIns" ClientIDMode="Static" runat="server" CssClass="form-control"
                        Width="350px" Height="80px" ToolTip="Enter Special Instructions" TextMode="MultiLine"
                        onKeyUp="javascript:CheckMaxLength(this, 3999);" onChange="javascript:CheckMaxLength(this, 3999);"></asp:TextBox>
                </td>
                <th class="spanCss">
                    SPECIAL NOTES
                </th>
                <td>
                    <asp:TextBox ID="txtSplReq" runat="server" CssClass="form-control" ClientIDMode="Static"
                        Width="350px" Height="80px" ToolTip="Enter Special Notes" TextMode="MultiLine"
                        onKeyUp="javascript:CheckMaxLength(this, 3999);" onChange="javascript:CheckMaxLength(this, 3999);"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblErrMsg" runat="server" ClientIDMode="Static" ForeColor="Red" CssClass="lblCss"></asp:Label>
                </td>
                <td>
                    <asp:Button ID="btnSendOrder" runat="server" Text="ORDER SEND TO PLANT ASSIGN" ClientIDMode="Static"
                        CssClass="btn btn-success" Font-Bold="true" OnClientClick="javascript:return CtrlExpSendOrder();"
                        OnClick="btnSendOrder_Click" />
                </td>
                <td>
                    <asp:Button ID="btnClearOrder" runat="server" Text="CLEAR SELECTION" ClientIDMode="Static"
                        CssClass="btn btn-info" Font-Bold="true" OnClientClick="javascript:return fnPageReload();" />
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnCustCode" ClientIDMode="Static" Value="" />
    <script src="scripts/datemonthyear.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#txtDesiredDate").datepicker({ minDate: "+1D", maxDate: "+360D" }).keydown(function (e) { e.keyWhich = 0; e.keyCode = 0; e.preventDefault(); });
            $('#ddlShippingAddress').change(function () {
                $('#lblShipDetails').html('');
                if ($('#ddlShippingAddress option:selected').text() != 'Choose')
                    GetAddressDetails('lblShipDetails', $('#ddlShippingAddress option:selected').val());
            });
            $('#ddlBillingAddress').change(function () {
                $('#lblBillAddress').html('');
                if ($('#ddlBillingAddress option:selected').text() != 'Choose')
                    GetAddressDetails('lblBillAddress', $('#ddlBillingAddress option:selected').val());
            });
            $("input:radio[id*=rdbPackingMethod_]").click(function () {
                $('#divMethodOthers').css({ "display": "none" }); $('#txtPackingOthers').val('');
                if ($(this).val() == 'Others')
                    $('#divMethodOthers').css({ "display": "block" });
            });
        });
        function GetAddressDetails(lblCtrl, addID) {
            $('#' + lblCtrl).html(''); var custcode = $('#hdnCustCode').val();
            $.ajax({ type: "POST", url: "BindRecords.aspx?type=getAddress&addid=" + addID + "&custcode=" + custcode + "", context: document.body,
                success: function (data) { if (data != '') { $('#' + lblCtrl).html(data); } }
            });
        }

        function CtrlExpSendOrder() {
            var errmsg = ''; $('#lblErrMsg').html('');
            if ($("#txtDesiredDate").val().length == 0)
                errmsg += 'Enter desired shipping date</br>';
            if ($('#ddlBillingAddress option:selected').text() == 'Choose')
                errmsg += 'Choose billing address<br/>';
            if ($('#ddlShippingAddress option:selected').text() == 'Choose')
                errmsg += 'Choose shipping address<br/>';
            if ($("#txtCoutryDestination").val().length == 0)
                errmsg += 'Enter country of final destination</br>';
            if ($("#txtFinalDestination").val().length == 0)
                errmsg += 'Enter port of final destination</br>';
            if ($("input:radio[id*='rdbPackingMethod_']:checked").length == 0)
                errmsg += 'Choose packing menthod</br>';
            else if ($("input:radio[id*='rdbPackingMethod_']:checked").val() == "Others") {
                if ($('#txtPackingOthers').val().length == 0)
                    errmsg += 'Enter Other packing method</br>';
            }
            if ($("input:radio[id*='rdbDeliveryTerms_']:checked").length == 0)
                errmsg += 'Choose delivery menthod</br>';
            if ($("input:radio[id*='rdbDeliveryMethod_']:checked").length == 0)
                errmsg += 'Choose delivery type</br>';
            //            if ($("#txtDeliveryAddress").val().length == 0)
            //                errmsg += 'Enter freight address details</br>';
            if (errmsg.length > 0) {
                $('#lblErrMsg').html(errmsg);
                return false;
            }
            else
                return true;
        }
        function fnPageReload() { window.location.href = window.location.href; }
    </script>
</asp:Content>
