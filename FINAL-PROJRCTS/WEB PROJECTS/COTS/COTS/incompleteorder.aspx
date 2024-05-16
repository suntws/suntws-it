<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="incompleteorder.aspx.cs" Inherits="COTS.incompleteorder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="pageTitleHead">
        ORDER INCOMPLETE LIST
    </div>
    <div class="contPage">
        <table cellspacing="0" rules="all" border="1" style="background-color: #ffffff; width: 100%;
            border-color: #cccbfb; border-collapse: separate; line-height: 30px;">
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" Font-Bold="true"
                        Font-Size="X-Large" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView runat="server" ID="gv_Incomplete" AutoGenerateColumns="false" Width="100%"
                        HeaderStyle-CssClass="gvHeadBg" AlternatingRowStyle-BackColor="#F0E8E8" Font-Bold="true">
                        <Columns>
                            <asp:TemplateField HeaderText="ORDER NO">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblIncompleteOrderNo" Text='<%# Eval("OrderRefNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="ORDER PREPARE ON" DataField="createddate" />
                            <asp:BoundField HeaderText="DESIRED SHIPPING ON" DataField="DesiredShipDate" />
                            <asp:BoundField HeaderText="QTY" DataField="orderqty" ItemStyle-HorizontalAlign="Right" />
                            <asp:TemplateField HeaderText="DISPATCH TO">
                                <ItemTemplate>
                                    <div style="line-height: 20px;">
                                        <div>
                                            <%# Eval("CompanyName")%></div>
                                        <div>
                                            <%# Eval("city")%></div>
                                        <div>
                                            <%# Eval("statename")%></div>
                                        <div>
                                            <%# Eval("country")%></div>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ACTION" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <span>
                                        <asp:Button runat="server" ID="btnGotoOrderEntry" Text="Prepare Order" OnClick="btnGotoOrderEntry_Click"
                                            CssClass="btn  btn-success" />
                                    </span><span>
                                        <asp:Button ID="btnDeleteOrder" runat="server" Text="Delete Order" OnClick="btnDeleteOrder_Click"
                                            CssClass="btn btn-warning" />
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
