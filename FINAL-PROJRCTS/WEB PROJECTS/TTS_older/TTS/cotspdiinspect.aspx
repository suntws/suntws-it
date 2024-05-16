<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="cotspdiinspect.aspx.cs" Inherits="TTS.cotspdiinspect" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        <asp:Label runat="server" ID="lblPageTitle" ClientIDMode="Static" Text=""></asp:Label>
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="background-color: #dff3ce; border: solid 1px #525252;
            border-collapse: collapse; width: 100%;">
            <tr>
                <td colspan="4">
                    <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:GridView runat="server" ID="gvDomPdiList" AutoGenerateColumns="false" Width="100%"
                        HeaderStyle-BackColor="#FFCC00" RowStyle-Height="22px">
                        <Columns>
                            <asp:TemplateField HeaderText="CUSTOMER NAME" ItemStyle-Width="200px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatusCustName" Text='<%# Eval("custfullname") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="WORK ORDER" DataField="workorderno" ItemStyle-Width="60px" />
                            <asp:TemplateField HeaderText="ORDER REF NO." ItemStyle-Width="200px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOrderRefNo" Text='<%#Eval("OrderRefNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="ORDERED DATE" DataField="CompletedDate" ItemStyle-Width="60px" />
                            <asp:BoundField HeaderText="LAST REVISED DATE" DataField="RevisedDate" ItemStyle-Width="60px" />
                            <asp:BoundField HeaderText="CUSTOMER DESIRED SHIP DATE" DataField="DesiredShipDate"
                                ItemStyle-Width="60px" />
                            <asp:BoundField HeaderText="QTY" DataField="itemqty" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField HeaderText="PLANT" DataField="Plant" ItemStyle-Width="40px" />
                            <asp:TemplateField HeaderText="ACTION" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hdnOrderID" Value='<%# Eval("ID") %>' />
                                    <asp:HiddenField runat="server" ID="hdnCustCode" Value='<%# Eval("CustCode") %>' />
                                    <asp:HiddenField runat="server" ID="hdnOrderStatus" Value='<%# Eval("OrderStatus") %>' />
                                    <asp:LinkButton runat="server" ID="lnkPdiInspect" OnClick="lnkPdiInspect_Click" Text="MAKE INSPECTION"
                                        Visible='<%# Eval("CustHoldStatus").ToString() == "True" ? false : true%>'></asp:LinkButton>
                                    <span style="color: #ff0000; font-style: italic;">
                                        <%# Eval("CustHoldStatus").ToString() == "True" ? "CREDIT HOLD" : ""%></span>
                                    <span style="color: #ff0000; font-style: italic;">
                                        <%# Eval("CreditNote").ToString() == "True" ? "Probation Order" : ""%></span>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label runat="server" ID="lblSplitRims" ClientIDMode="Static" Text="" Font-Bold="true"
                        Font-Size="14px"></asp:Label>
                    <asp:GridView runat="server" ID="gvRimsPDI" AutoGenerateColumns="false" Width="100%"
                        HeaderStyle-BackColor="#FFCC00" RowStyle-Height="22px">
                        <Columns>
                            <asp:BoundField HeaderText="CUSTOMER" DataField="custfullname" />
                            <asp:BoundField HeaderText="WORK ORDER" DataField="workorderno" />
                            <asp:BoundField HeaderText="ORDER REF NO" DataField="OrderRefNo" />
                            <asp:BoundField HeaderText="ORDER DATE" DataField="CompletedDate" />
                            <asp:BoundField HeaderText="LAST REVISED DATE" DataField="RevisedDate" />
                            <asp:BoundField HeaderText="CUSTOMER DESIRED SHIP DATE" DataField="DesiredShipDate" />
                            <asp:BoundField HeaderText="QTY" DataField="itemqty" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField HeaderText="PLANT" DataField="Plant" />
                            <asp:TemplateField HeaderText="ACTION">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hdnOrderID" Value='<%# Eval("ID") %>' />
                                    <asp:HiddenField runat="server" ID="hdnCustCode" Value='<%# Eval("CustCode") %>' />
                                    <asp:LinkButton runat="server" ID="lnkRimPdiInspect" OnClick="lnkRimPdiInspect_Click"
                                        Text="RIM ORDER MOVED TO NEXT PROCESS" Visible='<%# Eval("CustHoldStatus").ToString() == "True" ? false : true%>'></asp:LinkButton>
                                    <span style="color: #ff0000; font-style: italic;">
                                        <%# Eval("CustHoldStatus").ToString() == "True" ? "CREDIT HOLD" : ""%></span>
                                    <span style="color: #ff0000; font-style: italic;">
                                        <%# Eval("CreditNote").ToString() == "True" ? "<br/>Probation Order" : ""%></span>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    <asp:Label runat="server" ID="lblStatusMsg" ClientIDMode="Static" Font-Bold="true"
                        ForeColor="Blue"></asp:Label>
                    <asp:Button runat="server" ID="btnRefresh" ClientIDMode="Static" OnClick="btnRefresh_Click"
                        CssClass="" />
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnOID" ClientIDMode="Static" Value="" />
</asp:Content>
