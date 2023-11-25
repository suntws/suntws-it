<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="Stockorderrevise.aspx.cs" Inherits="TTS.Stockorderrevise" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        <asp:Label runat="server" ID="lblQuoteHead" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <asp:GridView ID="gv_StockOrderList" runat="server" AutoGenerateColumns="false" Width="100%"
            ClientIDMode="Static" CssClass="gridcss">
            <HeaderStyle BackColor="#D3CF44" Font-Bold="true" Height="22px" HorizontalAlign="Center" />
            <Columns>
                <asp:BoundField DataField="CustomerName" HeaderText="CUSTOMER" />
                <asp:TemplateField HeaderText="ORDER REF NO">
                    <ItemTemplate>
                        <asp:HiddenField runat="server" ID="hdnStockOrderid" Value='<%# Eval("StockOrderid") %>' />
                        <asp:Label runat="server" ID="lbl_OrderRefNo" Text='<%# Eval("refno") %>'></asp:Label>
                        <asp:HiddenField runat="server" ID="hdnCustStdcode" Value='<%# Eval("CustStdcode") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="StartDate" HeaderText="START ON" />
                <asp:BoundField DataField="EndDate" HeaderText="END ON" />
                <asp:BoundField DataField="Plant" HeaderText="PLANT" />
                <asp:BoundField DataField="orderQty" HeaderText="QTY" />
                <asp:TemplateField HeaderText="ACTION" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Button ID="btnIncompleteOrder" runat="server" Text="PREPARE" ClientIDMode="Static"
                            CssClass="btnsave" OnClick="btnIncompleteOrder_Click" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ACTION" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Button ID="btnReviseOrder" runat="server" Text="REVISE" ClientIDMode="Static"
                            CssClass="btnsave" OnClick="btnReviseOrder_Click" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ACTION" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Button ID="btnTrackOrder" runat="server" Text="TRACK" ClientIDMode="Static"
                            CssClass="btnsave" OnClick="btnTrackOrder_Click" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <div id="div_custorder" style="display: none; background-color: #defbf0;">
            <asp:Label runat="server" ID="lblCustomer" ClientIDMode="Static" Text="" Font-Bold="true"
                Font-Size="14px"></asp:Label>
            <asp:GridView ID="gv_Addeditems" runat="server" AutoGenerateColumns="false" Width="1100px"
                ClientIDMode="Static" ShowFooter="true" FooterStyle-Font-Bold="true">
                <HeaderStyle BackColor="#D1E67D" Font-Bold="true" Height="22px" HorizontalAlign="Center" />
                <Columns>
                    <asp:BoundField DataField="Category" HeaderText="CATEGORY" />
                    <asp:BoundField DataField="Config" HeaderText="PLATFORM" />
                    <asp:BoundField DataField="brand" HeaderText="BRAND" />
                    <asp:BoundField DataField="sidewall" HeaderText="SIDEWALL" />
                    <asp:BoundField DataField="tyretype" HeaderText="TYRE TYPE" />
                    <asp:BoundField DataField="tyresize" HeaderText="TYRE SIZE" />
                    <asp:BoundField DataField="rimsize" HeaderText="RIM" />
                    <asp:BoundField DataField="finishedwt" HeaderText="FWT" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="totfwt" HeaderText="TOT FWT" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="itemqty" HeaderText="QTY" ItemStyle-HorizontalAlign="Right" />
                </Columns>
                <FooterStyle BackColor="#E0E9C4" HorizontalAlign="Right" />
            </asp:GridView>
            <asp:LinkButton runat="server" ID="lnkStockWorkOrder" ClientIDMode="Static" Text=""
                Font-Bold="true" OnClick="lnkStockWorkOrder_Click"></asp:LinkButton>
            <asp:HiddenField runat="server" ID="hdn_CustCode" ClientIDMode="Static" Value="" />
        </div>
    </div>
    <script src="Scripts/cotsScript.js" type="text/javascript"></script>
</asp:Content>
