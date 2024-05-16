<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="frmitemlist.aspx.cs" Inherits="COTS.frmitemlist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div align="center" class="pageTitleHead">
        ORDER ITEM LIST
    </div>
    <div class="contPage">
        <div>
            <table>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"
                            Font-Bold="true" Font-Size="X-Large"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:DataList runat="server" ID="dlOrderMaster" RepeatColumns="1" RepeatDirection="Horizontal"
                            RepeatLayout="Table" AlternatingItemStyle-BackColor="#A1C9D3" ItemStyle-BackColor="#C9DAC9"
                            ItemStyle-VerticalAlign="Top">
                            <ItemTemplate>
                                <div style="width: 590px; float: left; line-height: 20px;">
                                    <div style="width: 295px; float: left;">
                                        <span class="headCss">Orderd Date: </span>
                                        <%# Eval("CompletedDate")%>
                                    </div>
                                    <div style="width: 295px; float: left;">
                                        <span class="headCss">Packing Method: </span>
                                        <%# Eval("PackingMethod") %>
                                    </div>
                                    <div style="width: 295px; float: left;">
                                        <span class="headCss">Freight Charges: </span>
                                        <%# Eval("FreightCharges") %>
                                    </div>
                                    <div style="width: 295px; float: left;">
                                        <span class="headCss">Delivery Method: </span>
                                        <%# Eval("DeliveryMethod")%>
                                    </div>
                                    <div style="width: 295px; float: left;">
                                        <span class="headCss">Transport Details: </span> 
                                        <%# Eval("TransportDetails")%>
                                    </div>
                                    <div style="width: 590px; float: left; line-height: 16px;">
                                        <span class="headCss">Bill To: </span>
                                        <%# Bind_BillingAddress(Eval("BillID").ToString())%>
                                    </div>
                                </div>
                                <div style="width: 590px; float: left; line-height: 20px;">
                                    <div style="width: 295px; float: left;">
                                        <span class="headCss">Desired Ship Date:</span>
                                        <%# Eval("DesiredShipDate")%>
                                    </div>
                                    <div style="width: 295px; float: left;">
                                        <span class="headCss">Expected Ship Date:</span>
                                        <%# Eval("ExpectedShipDate")%>
                                    </div>
                                    <div>
                                        <span class="headCss">Special Instruction :</span>
                                        <%# Eval("SplIns")%>
                                    </div>
                                    <div>
                                        <span class="headCss">Special Requset :</span>
                                        <%# Eval("SpecialRequset") %>
                                    </div>
                                    <div style="width: 590px; float: left; line-height: 16px;">
                                        <span class="headCss">Ship To: </span>
                                        <%# Bind_BillingAddress(Eval("ShipID").ToString())%>
                                    </div>
                                </div>
                                <div style="font-weight: bold; font-size: 25px; float: left; width: 1180px; text-align: center;">
                                    <%# Eval("StatusText").ToString() == "ORDER RECEIVED" ? "ORDER SENT" : Eval("StatusText").ToString()%></div>
                            </ItemTemplate>
                        </asp:DataList>
                    </td>
                </tr>
                <tr>
                    <td style="line-height: 20px;">
                        <asp:GridView runat="server" ID="gvOrderItemList" AutoGenerateColumns="false" Width="1170px"
                            AlternatingRowStyle-BackColor="#f5f5f5">
                            <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" Height="22px" />
                            <Columns>
                                <asp:BoundField HeaderText="CATEGORY" DataField="category" ItemStyle-CssClass="headerNone"
                                    HeaderStyle-CssClass="headerNone" />
                                <asp:BoundField HeaderText="PLATFORM" DataField="Config" ItemStyle-CssClass="headerNone"
                                    HeaderStyle-CssClass="headerNone" />
                                <asp:BoundField HeaderText="SIZE" DataField="tyresize" />
                                <asp:BoundField HeaderText="RIM" DataField="rimsize" />
                                <asp:BoundField HeaderText="TYPE" DataField="tyretype" />
                                <asp:BoundField HeaderText="BRAND" DataField="brand" />
                                <asp:BoundField HeaderText="SIDEWALL" DataField="sidewall" />
                                <asp:BoundField HeaderText="PRICE" DataField="listprice" ItemStyle-CssClass="numericright" />
                                <asp:BoundField HeaderText="QTY" DataField="itemqty" ItemStyle-CssClass="numericright" />
                                <asp:BoundField HeaderText="TOTAL PRICE" DataField="unitprice" ItemStyle-CssClass="numericright" />
                                <asp:BoundField HeaderText="TOTAL WT(Kgs)" DataField="finishedwt" ItemStyle-CssClass="numericright" />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <span class="headCss">Total Volume :</span>
                        <asp:TextBox runat="server" ID="txtTotVolume" ClientIDMode="Static" Enabled="false"
                            Text="" Width="100px"></asp:TextBox><span class="headCss">Total Weight :</span>
                        <asp:TextBox runat="server" ID="txtTotWeight" ClientIDMode="Static" Enabled="false"
                            Text="" Width="100px"></asp:TextBox>
                        <span class="headCss">Total Qty :</span>
                        <asp:TextBox runat="server" ID="txtTotQty" ClientIDMode="Static" Enabled="false"
                            Text="" Width="50px"></asp:TextBox>
                        <span class="headCss">Total Cost :</span>
                        <asp:TextBox runat="server" ID="txtTotCost" ClientIDMode="Static" Enabled="false"
                            Text="" Width="100px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div style="width: 652px; float: left;">
                            <asp:Label runat="server" ID="lblProformaTxt" ClientIDMode="Static" Text="" ForeColor="Green"
                                Font-Bold="true" Font-Size="20px"></asp:Label>
                            <asp:LinkButton runat="server" ID="lnkProformaFile" Text="" ClientIDMode="Static"
                                OnClick="lnkProformaFile_Click"></asp:LinkButton>
                        </div>
                        <div style="width: 652px; float: left;">
                            <asp:Label runat="server" ID="lblInvoiceTxt" ClientIDMode="Static" Text="" ForeColor="Green"
                                Font-Bold="true" Font-Size="20"></asp:Label>
                            <asp:LinkButton runat="server" ID="lnkInvoiceFile" Text="" ClientIDMode="Static"
                                OnClick="lnkInvoiceFile_Click"></asp:LinkButton>
                        </div>
                        <div style="width: 652px; float: left;">
                            <asp:Label runat="server" ID="lblLRTxt" ClientIDMode="Static" Text="" ForeColor="Green"
                                Font-Bold="true" Font-Size="20"></asp:Label>
                            <asp:LinkButton runat="server" ID="lnkLRCopyFile" Text="" ClientIDMode="Static" OnClick="lnkLRCopyFile_Click"></asp:LinkButton>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="line-height: 30px; padding-top: 10px;">
                        <div style="width: 1000px; float: left;">
                            <div id="divStatusButton" style="display: none; width: 1000px; float: left;">
                                <div class="headCss" style="width: 651px; float: left; line-height: 20px;">
                                    COMMENTS
                                    <asp:TextBox runat="server" ID="txtOrderChangeComments" ClientIDMode="Static" Text=""
                                        TextMode="MultiLine" onKeyUp="javascript:CheckMaxLength(this, 3999);" onChange="javascript:CheckMaxLength(this, 3999);"
                                        Width="645px" Height="80px"></asp:TextBox></div>
                                <div style="width: 180px; float: left; text-align: center; padding-top: 45px;">
                                    <asp:Button runat="server" ID="btnStatusChange" ClientIDMode="Static" Text="CONFIRM ORDER"
                                        OnClick="btnStatusChange_Click" CssClass="btnAuthorize" /></div>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdnOrderNo" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        function display_divStatusButton() {
            $('#divStatusButton').css({ 'display': 'block' });
        }
    </script>
</asp:Content>
