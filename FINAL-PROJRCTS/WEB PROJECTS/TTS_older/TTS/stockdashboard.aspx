<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="stockdashboard.aspx.cs" Inherits="TTS.stockdashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        STOCK DASHBOARD
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="width: 100%; border-collapse: collapse;
            text-align: center;">
            <tr>
                <td>
                    <asp:Repeater runat="server" ID="rptCurrentStock">
                        <HeaderTemplate>
                            <table cellspacing="0" rules="all" border="1" style="width: 100%; border-color: #96c2e8;
                                border-collapse: collapse; line-height: 30px; font-size: 14px;">
                                <tr style="font-size: 18px; font-weight: bold; height: 30px;">
                                    <td style="background-color: #f9d481;">
                                        CURRENT STOCK
                                    </td>
                                    <td colspan="6" style="background-color: #c3a1f9;">
                                        QUANTITY (NOS)
                                    </td>
                                    <td colspan="6" style="background-color: #a1e9f9;">
                                        WEIGHT (TONNAGE)
                                    </td>
                                </tr>
                                <tr style="font-size: 16px; font-weight: bold; height: 30px;">
                                    <td>
                                        PLANT
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                        B
                                    </td>
                                    <td>
                                        C
                                    </td>
                                    <td>
                                        D
                                    </td>
                                    <td>
                                        E
                                    </td>
                                    <td>
                                        TOTAL
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                        B
                                    </td>
                                    <td>
                                        C
                                    </td>
                                    <td>
                                        D
                                    </td>
                                    <td>
                                        E
                                    </td>
                                    <td>
                                        TOTAL
                                    </td>
                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr style="font-size: 16px; font-weight: bold; height: 25px;">
                                <td>
                                    <%# Eval("StockType") %>
                                </td>
                                <td>
                                    <%# Eval("AGrade")%>
                                </td>
                                <td>
                                    <%# Eval("BGrade")%>
                                </td>
                                <td>
                                    <%# Eval("CGrade")%>
                                </td>
                                <td>
                                    <%# Eval("DGrade")%>
                                </td>
                                <td>
                                    <%# Eval("EGrade")%>
                                </td>
                                <td>
                                    <%# Eval("PlantTotal")%>
                                </td>
                                <td>
                                    <%# Eval("A_Fwt")%>
                                </td>
                                <td>
                                    <%# Eval("B_Fwt")%>
                                </td>
                                <td>
                                    <%# Eval("C_Fwt")%>
                                </td>
                                <td>
                                    <%# Eval("D_Fwt")%>
                                </td>
                                <td>
                                    <%# Eval("E_Fwt")%>
                                </td>
                                <td>
                                    <%# Eval("PlantTotalFwt")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </td>
            </tr>
            <tr>
                <td>
                    <hr style="height: 10px; width: 80%; background-color: #14b4ce; border-radius: 15px;
                        border-style: double; text-align: center;" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Repeater runat="server" ID="rptPreviousStock">
                        <HeaderTemplate>
                            <table cellspacing="0" rules="all" border="1" style="width: 100%; border-color: #96c2e8;
                                border-collapse: collapse; line-height: 30px; font-size: 14px;">
                                <tr style="font-size: 18px; font-weight: bold; height: 30px;">
                                    <td style="background-color: #81f99b;">
                                        PREVIOUS STOCK
                                    </td>
                                    <td colspan="6" style="background-color: #cdef97;">
                                        QUANTITY (NOS)
                                    </td>
                                    <td colspan="6" style="background-color: #efec97;">
                                        WEIGHT (TONNAGE)
                                    </td>
                                </tr>
                                <tr style="font-size: 16px; font-weight: bold; height: 30px;">
                                    <td>
                                        PLANT
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                        B
                                    </td>
                                    <td>
                                        C
                                    </td>
                                    <td>
                                        D
                                    </td>
                                    <td>
                                        E
                                    </td>
                                    <td>
                                        TOTAL
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                        B
                                    </td>
                                    <td>
                                        C
                                    </td>
                                    <td>
                                        D
                                    </td>
                                    <td>
                                        E
                                    </td>
                                    <td>
                                        TOTAL
                                    </td>
                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr style="font-size: 16px; font-weight: bold; height: 25px;">
                                <td>
                                    <%# Eval("StockType") %>
                                </td>
                                <td>
                                    <%# Eval("AGrade")%>
                                </td>
                                <td>
                                    <%# Eval("BGrade")%>
                                </td>
                                <td>
                                    <%# Eval("CGrade")%>
                                </td>
                                <td>
                                    <%# Eval("DGrade")%>
                                </td>
                                <td>
                                    <%# Eval("EGrade")%>
                                </td>
                                <td>
                                    <%# Eval("PlantTotal")%>
                                </td>
                                <td>
                                    <%# Eval("A_Fwt")%>
                                </td>
                                <td>
                                    <%# Eval("B_Fwt")%>
                                </td>
                                <td>
                                    <%# Eval("C_Fwt")%>
                                </td>
                                <td>
                                    <%# Eval("D_Fwt")%>
                                </td>
                                <td>
                                    <%# Eval("E_Fwt")%>
                                </td>
                                <td>
                                    <%# Eval("PlantTotalFwt")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $('table:eq(2)').find('tr').not(':first').not('tr:eq(0)').hover(function () {
                $(this).find('td:nth-child(n+1)').css({ 'background-color': '#f9d481' });
                $(this).find('td:nth-child(n+2)').css({ 'background-color': '#c3a1f9' });
                $(this).find('td:nth-child(n+8)').css({ 'background-color': '#a1e9f9' });
            }).mouseleave(function (e) {
                $(this).find('td').css({ 'background-color': '#ffffff' });
            });

            $('table:eq(3)').find('tr').not(':first').not('tr:eq(0)').hover(function () {
                $(this).find('td:nth-child(n+1)').css({ 'background-color': '#81f99b' });
                $(this).find('td:nth-child(n+2)').css({ 'background-color': '#cdef97' });
                $(this).find('td:nth-child(n+8)').css({ 'background-color': '#efec97' });
            }).mouseleave(function (e) {
                $(this).find('td').css({ 'background-color': '#ffffff' });
            });
        });
    </script>
</asp:Content>
