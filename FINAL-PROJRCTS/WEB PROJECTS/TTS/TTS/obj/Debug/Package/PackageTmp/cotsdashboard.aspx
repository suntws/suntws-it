<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cotsdashboard.aspx.cs"
    MasterPageFile="~/master.Master" Inherits="TTS.cotsdashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        tr:last-child
        {
            background: yellow;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: center;" class="pageTitleHead">
        SCOTS DOMESTIC SALES REPORT
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table style="font-weight: bold;">
            <tr style="vertical-align: top;">
                <td>
                    <asp:GridView ID="gvSEGMENT" runat="server" Width="500px" AutoGenerateColumns="false"
                        OnDataBound="gvtemp_DataBound">
                        <HeaderStyle HorizontalAlign="Center" Font-Bold="true" BackColor="#93f981" Height="20px" />
                        <Columns>
                            <asp:BoundField DataField="segment" HeaderText="SEGMENT" ItemStyle-Height="18px" />
                            <asp:BoundField DataField="ItemQty_ft15To16" HeaderText="NOS" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Height="18px" />
                            <asp:BoundField DataField="Tonnage_ft15To16" HeaderText="TON" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Height="18px" />
                            <asp:BoundField DataField="ItemQty_ft16To17" HeaderText="NOS" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Height="18px" />
                            <asp:BoundField DataField="Tonnage_ft16To17" HeaderText="TON" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Height="18px" />
                            <asp:BoundField DataField="ItemQty_ft17To18" HeaderText="NOS" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Height="18px" />
                            <asp:BoundField DataField="Tonnage_ft17To18" HeaderText="TON" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Height="18px" />
                            <asp:BoundField DataField="ItemQty_ft18To19" HeaderText="NOS" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Height="18px" />
                            <asp:BoundField DataField="Tonnage_ft18To19" HeaderText="TON" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Height="18px" />
                        </Columns>
                    </asp:GridView>
                </td>
                <td style="vertical-align: middle;">
                    <hr style="vertical-align: middle; height: 150px; width: 5px; background-color: #000;
                        margin: 5px;" />
                </td>
                <td>
                    <asp:GridView ID="gvREGION" runat="server" Width="500px" AutoGenerateColumns="false"
                        OnDataBound="gvtemp_DataBound">
                        <HeaderStyle HorizontalAlign="Center" Font-Bold="true" BackColor="#e181f9" Height="20px" />
                        <Columns>
                            <asp:BoundField DataField="region" HeaderText="REGION" ItemStyle-CssClass="" />
                            <%--<asp:BoundField DataField="ItemQty_ft14To15" HeaderText="NOS" ItemStyle-HorizontalAlign="Right" ItemStyle-Height="18px" />
                            <asp:BoundField DataField="Tonnage_ft14To15" HeaderText="TON" ItemStyle-HorizontalAlign="Right" ItemStyle-Height="18px" />--%>
                            <asp:BoundField DataField="ItemQty_ft15To16" HeaderText="NOS" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Height="18px" />
                            <asp:BoundField DataField="Tonnage_ft15To16" HeaderText="TON" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Height="18px" />
                            <asp:BoundField DataField="ItemQty_ft16To17" HeaderText="NOS" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Height="18px" />
                            <asp:BoundField DataField="Tonnage_ft16To17" HeaderText="TON" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Height="18px" />
                            <asp:BoundField DataField="ItemQty_ft17To18" HeaderText="NOS" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Height="18px" />
                            <asp:BoundField DataField="Tonnage_ft17To18" HeaderText="TON" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Height="18px" />
                            <asp:BoundField DataField="ItemQty_ft18To19" HeaderText="NOS" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Height="18px" />
                            <asp:BoundField DataField="Tonnage_ft18To19" HeaderText="TON" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Height="18px" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <hr style="vertical-align: middle; height: 5px; width: 1050px; background-color: #000;" />
                </td>
            </tr>
            <tr style="vertical-align: top;">
                <td>
                    <asp:GridView ID="gvLEAD" runat="server" Width="500px" AutoGenerateColumns="false"
                        OnDataBound="gvtemp_DataBound">
                        <HeaderStyle HorizontalAlign="Center" Font-Bold="true" BackColor="#a2e8ef" Height="20px" />
                        <Columns>
                            <asp:BoundField DataField="lead" HeaderText="LEAD" ItemStyle-CssClass="" />
                            <%--<asp:BoundField DataField="ItemQty_ft14To15" HeaderText="NOS" ItemStyle-HorizontalAlign="Right" ItemStyle-Height="18px" />
                            <asp:BoundField DataField="Tonnage_ft14To15" HeaderText="TON" ItemStyle-HorizontalAlign="Right" ItemStyle-Height="18px" />--%>
                            <asp:BoundField DataField="ItemQty_ft15To16" HeaderText="NOS" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Height="18px" />
                            <asp:BoundField DataField="Tonnage_ft15To16" HeaderText="TON" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Height="18px" />
                            <asp:BoundField DataField="ItemQty_ft16To17" HeaderText="NOS" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Height="18px" />
                            <asp:BoundField DataField="Tonnage_ft16To17" HeaderText="TON" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Height="18px" />
                            <asp:BoundField DataField="ItemQty_ft17To18" HeaderText="NOS" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Height="18px" />
                            <asp:BoundField DataField="Tonnage_ft17To18" HeaderText="TON" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Height="18px" />
                            <asp:BoundField DataField="ItemQty_ft18To19" HeaderText="NOS" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Height="18px" />
                            <asp:BoundField DataField="Tonnage_ft18To19" HeaderText="TON" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Height="18px" />
                        </Columns>
                    </asp:GridView>
                </td>
                <td style="vertical-align: middle;">
                    <hr style="vertical-align: middle; height: 150px; width: 5px; background-color: #000;
                        margin: 5px;" />
                </td>
                <td>
                    <asp:GridView ID="gvMONTH" runat="server" Width="500px" AutoGenerateColumns="false"
                        OnDataBound="gvtemp_DataBound">
                        <HeaderStyle HorizontalAlign="Center" Font-Bold="true" BackColor="#f9e581" Height="20px" />
                        <Columns>
                            <asp:BoundField DataField="MONTH" HeaderText="MONTH" ItemStyle-CssClass="" />
                            <%--<asp:BoundField DataField="ItemQty_ft14To15" HeaderText="NOS" ItemStyle-HorizontalAlign="Right" ItemStyle-Height="18px" />
                            <asp:BoundField DataField="Tonnage_ft14To15" HeaderText="TON" ItemStyle-HorizontalAlign="Right" ItemStyle-Height="18px" />--%>
                            <asp:BoundField DataField="ItemQty_ft15To16" HeaderText="NOS" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Height="18px" />
                            <asp:BoundField DataField="Tonnage_ft15To16" HeaderText="TON" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Height="18px" />
                            <asp:BoundField DataField="ItemQty_ft16To17" HeaderText="NOS" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Height="18px" />
                            <asp:BoundField DataField="Tonnage_ft16To17" HeaderText="TON" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Height="18px" />
                            <asp:BoundField DataField="ItemQty_ft17To18" HeaderText="NOS" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Height="18px" />
                            <asp:BoundField DataField="Tonnage_ft17To18" HeaderText="TON" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Height="18px" />
                            <asp:BoundField DataField="ItemQty_ft18To19" HeaderText="NOS" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Height="18px" />
                            <asp:BoundField DataField="Tonnage_ft18To19" HeaderText="TON" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Height="18px" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <hr style="vertical-align: middle; height: 5px; width: 1050px; background-color: #000;" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="gvStock" runat="server" Width="500px" AutoGenerateColumns="false"
                        OnDataBound="gvtemp_DataBound">
                        <HeaderStyle HorizontalAlign="Center" Font-Bold="true" BackColor="#f1f1f1" Height="20px" />
                        <Columns>
                            <asp:BoundField DataField="STOCK" HeaderText="STOCK TRANSFER" ItemStyle-CssClass="" />
                            <%--<asp:BoundField DataField="ItemQty_ft14To15" HeaderText="NOS" ItemStyle-HorizontalAlign="Right" ItemStyle-Height="18px" />
                            <asp:BoundField DataField="Tonnage_ft14To15" HeaderText="TON" ItemStyle-HorizontalAlign="Right" ItemStyle-Height="18px" />--%>
                            <asp:BoundField DataField="ItemQty_ft15To16" HeaderText="NOS" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Height="18px" />
                            <asp:BoundField DataField="Tonnage_ft15To16" HeaderText="TON" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Height="18px" />
                            <asp:BoundField DataField="ItemQty_ft16To17" HeaderText="NOS" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Height="18px" />
                            <asp:BoundField DataField="Tonnage_ft16To17" HeaderText="TON" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Height="18px" />
                            <asp:BoundField DataField="ItemQty_ft17To18" HeaderText="NOS" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Height="18px" />
                            <asp:BoundField DataField="Tonnage_ft17To18" HeaderText="TON" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Height="18px" />
                            <asp:BoundField DataField="ItemQty_ft18To19" HeaderText="NOS" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Height="18px" />
                            <asp:BoundField DataField="Tonnage_ft18To19" HeaderText="TON" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Height="18px" />
                        </Columns>
                    </asp:GridView>
                </td>
                <td style="vertical-align: middle;">
                    <hr style="vertical-align: middle; height: 150px; width: 5px; background-color: #000;
                        margin: 5px;" />
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <hr style="vertical-align: middle; height: 5px; width: 1050px; background-color: #000;" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
