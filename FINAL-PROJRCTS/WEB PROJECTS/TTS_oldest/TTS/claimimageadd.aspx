<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="claimimageadd.aspx.cs" Inherits="TTS.claimimageadd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div align="center" class="pageTitleHead">
        <asp:Label runat="server" ID="lblPageHead" ClientIDMode="Static" Text=""></asp:Label>
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <div style="padding-left: 5px;">
            <table style="width: 1070px;">
                <tr>
                    <td align="center">
                        <asp:GridView runat="server" ID="gvClaimImageAddList" AutoGenerateColumns="false"
                            Width="1065px" RowStyle-Height="22px">
                            <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" Height="22px" />
                            <Columns>
                                <asp:BoundField DataField="custname" HeaderText="CUSTOMER NAME" ItemStyle-Width="200px" />
                                <asp:BoundField DataField="complaintno" HeaderText="COMPLAINT NO." ItemStyle-Width="80px" />
                                <asp:BoundField DataField="complaintdate" HeaderText="COMPLAINT DATE" ItemStyle-Width="80px" />
                                <asp:BoundField DataField="plant" HeaderText="PLANT" ItemStyle-Width="80px" />
                                <asp:BoundField DataField="Qty" HeaderText="QTY" ItemStyle-Width="60px" />
                                <asp:BoundField DataField="claimstatus" HeaderText="STATUS" ItemStyle-Width="200px" />
                                <asp:TemplateField HeaderText="ACTION" ItemStyle-Width="50px">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkClaimNo" runat="server" Text="Show List" OnClick="lnkClaimNo_Click" /></span>
                                        <asp:HiddenField runat="server" ID="hdnClaimCustCode" Value='<%# Eval("custcode") %>' />
                                        <asp:HiddenField runat="server" ID="hdncreditnote" Value='<%# Eval("CreditNoteNo") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdnStencilPlant" ClientIDMode="Static" Value="" />
</asp:Content>
