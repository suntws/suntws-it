<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CotsManualOrderIncomplete.aspx.cs"
    MasterPageFile="~/master.Master" Inherits="TTS.CotsManualOrderIncomplete" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        <asp:Label runat="server" ID="lblPageHead" ClientIDMode="Static" Text=""></asp:Label>
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <asp:GridView ID="gv_Addeditems" runat="server" AutoGenerateColumns="false" Width="100%"
            ClientIDMode="Static">
            <HeaderStyle BackColor="#a1ccf3" Font-Bold="true" Height="22px" HorizontalAlign="Center" />
            <Columns>
                <asp:BoundField DataField="customer" HeaderText="CUSTOMER" />
                <asp:BoundField DataField="OrderRefNo" HeaderText="ORDER REF NO" />
                <asp:BoundField DataField="OrderDate" HeaderText="CREATED DATE" />
                <asp:BoundField DataField="DesiredShipDate" HeaderText="DESIRED SHIP DATE" />
                <asp:BoundField DataField="statename" HeaderText="STATE" />
                <asp:BoundField DataField="city" HeaderText="CITY" />
                <asp:BoundField DataField="orderQty" HeaderText="ORDER QTY" />
                <asp:TemplateField HeaderText="ACTION">
                    <ItemTemplate>
                        <asp:HiddenField runat="server" ID="hdnOrderID" Value='<%# Eval("ID") %>' />
                        <asp:Button ID="btnReviseOrder" runat="server" Text="PREPARE" ClientIDMode="Static"
                            CssClass="btnsave" OnClick="btnReviseOrder_Click" BackColor="#139BA9" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
