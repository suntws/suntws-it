<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="expscanincomplete.aspx.cs" Inherits="TTS.expscanincomplete" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/pdistyle.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: center" class="pageTitleHead">
        <asp:Label runat="server" ID="lblPageTitle" ClientIDMode="Static" Text=""></asp:Label>
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div class="contPage">
        <table cellspacing="0" rules="all" border="1" style="background-color: #dff3ce; border: solid 1px #525252;
            border-collapse: collapse; width: 100%;">
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" Font-Bold="true"
                        Font-Size="12px" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView runat="server" ID="gv_PdiIncomplete" AutoGenerateColumns="false" Width="100%"
                        HeaderStyle-BackColor="#c1dbf7">
                        <Columns>
                            <asp:TemplateField HeaderText="CUSTOMER" ItemStyle-Width="250px">
                                <ItemTemplate>
                                    <asp:Label ID="lblCustomerName" Text='<%# Eval("custfullname") %>' runat="server"></asp:Label>
                                    <asp:HiddenField ID="hdnCustCode" Value='<%# Eval("custcode") %>' runat="server" />
                                    <asp:HiddenField ID="hdnPID" Value='<%# Eval("ID") %>' runat="server" />
                                    <asp:HiddenField ID="hdnOrderRefno" Value='<%# Eval("orderrefno") %>' runat="server" />
                                    <asp:HiddenField ID="hdnOrderID" Value='<%# Eval("O_ID") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="WORK ORDER NO" ItemStyle-Width="250px">
                                <ItemTemplate>
                                    <asp:Label ID="lblWorkOrderNo" Text='<%# Eval("workorderno") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ORDER QTY" ItemStyle-Width="50px">
                                <ItemTemplate>
                                    <asp:Label ID="lblOrderQty" Text='<%# Eval("orderqty") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SCANNED QTY" ItemStyle-Width="50px">
                                <ItemTemplate>
                                    <asp:Label ID="lblScanedQty" Text='<%# Eval("ScanQty") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ACTION" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="160px">
                                <ItemTemplate>
                                    <asp:Button ID="btnViewOrderForInspect" runat="server" Text="MAKE INSPECTION" CssClass="button"
                                        Width="150px" BackColor="#2196f3" Font-Bold="true" OnClick="btnViewOrderForInspect_click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
