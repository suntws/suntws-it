<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="orderpositiondomestic.aspx.cs" Inherits="TTS.orderpositiondomestic" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div align="center" class="pageTitleHead">
        DASHBOARD DOMESTIC</div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <div style="padding-left: 280px; padding-top: 5px;">
            <asp:Repeater runat="server" ID="rptDomesticToday">
                <ItemTemplate>
                    <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #07A714;
                        margin-top: 10px; line-height: 25px; width: 500px;">
                        <tr style="text-align: center; font-weight: bold; font-size: 15px; background-color: #E7FCA7;">
                            <td colspan="3">
                                <span>Last updated on : </span>
                                <%# Eval("AsOnDate")%>
                                <span style="padding-left: 20px;">By : </span>
                                <%# Eval("UserName")%>
                            </td>
                        </tr>
                        <tr style="text-align: center; font-weight: bold; font-size: 13px; background-color: #F1F0E8;">
                            <td style="background-color: #ffe061;">
                                TODAY
                            </td>
                            <td>
                                INFLOW
                            </td>
                            <td>
                                DISPATCH
                            </td>
                        </tr>
                        <tr style="background-color: #CAD3FD;">
                            <td style="font-weight: bold; width: 121px; background-color: #F1F0E8;">
                                MMN (DOEMSTIC)
                                <br />
                                PDK (DOEMSTIC)
                                <br />
                                C&F (DOEMSTIC)
                            </td>
                            <td>
                                <div style="width: 91px; float: left; padding-right: 30px; text-align: right;">
                                    <%# Eval("MmnDomInflow")%>
                                    <br />
                                    <%# Eval("PdkDomInflow")%>
                                    <br />
                                    <%# Eval("CfDomInflow")%>
                                </div>
                            </td>
                            <td>
                                <div style="width: 91px; float: left; padding-right: 30px; text-align: right;">
                                    <%# Eval("MmnDomDispatch")%>
                                    <br />
                                    <%# Eval("PdkDomDispatch")%>
                                    <br />
                                    <%# Eval("CfDomDispatch")%>
                                </div>
                            </td>
                        </tr>
                        <tr style="background-color: #ffe061;">
                            <td style="font-weight: bold; width: 121px; background-color: #BAB69A;">
                                TOTAL TODAY
                            </td>
                            <td>
                                <div style="width: 91px; float: left; padding-right: 30px; text-align: right;">
                                    <%# Convert.ToDecimal(Eval("MmnDomInflow")) + Convert.ToDecimal(Eval("PdkDomInflow")) + Convert.ToDecimal(Eval("CfDomInflow"))%>
                                </div>
                            </td>
                            <td>
                                <div style="width: 91px; float: left; padding-right: 30px; text-align: right;">
                                    <%# Convert.ToDecimal(Eval("MmnDomDispatch")) + Convert.ToDecimal(Eval("PdkDomDispatch")) + Convert.ToDecimal(Eval("CfDomDispatch")) %>
                                </div>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:Repeater>
        </div>
        <div style="padding-left: 280px; padding-top: 5px;">
            <asp:Repeater runat="server" ID="rptDomesticMonth">
                <ItemTemplate>
                    <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #07A714;
                        margin-top: 20px; line-height: 25px; width: 500px; background-color: #9ce159;">
                        <tr style="text-align: center; font-weight: bold; font-size: 13px; background-color: #B3F6C9;">
                            <td style="background-color: #9ce159;">
                                <%# Eval("AsMonth")%>
                                &nbsp;
                                <%# Eval("AsYear")%>
                            </td>
                            <td>
                                INFLOW
                            </td>
                            <td>
                                DISPATCH
                            </td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold; width: 121px; background-color: #B3F6C9;">
                                MMN (DOMESTIC)
                                <br />
                                PDK (DOMESTIC)
                                <br />
                                C&F (DOMESTIC)
                            </td>
                            <td>
                                <div style="width: 91px; float: left; padding-right: 30px; text-align: right;">
                                    <%# Eval("MmnDomInflow")%>
                                    <br />
                                    <%# Eval("PdkDomInflow")%>
                                    <br />
                                    <%# Eval("CfDomInflow")%>
                                </div>
                            </td>
                            <td>
                                <div style="width: 91px; float: left; padding-right: 30px; text-align: right;">
                                    <%# Eval("MmnDomDispatch")%>
                                    <br />
                                    <%# Eval("PdkDomDispatch")%>
                                    <br />
                                    <%# Eval("CfDomDispatch")%>
                                </div>
                            </td>
                        </tr>
                        <tr style="background-color: #FFF6CF;">
                            <td style="font-weight: bold; width: 121px; background-color: #BAB69A;">
                                TOTAL MONTH
                            </td>
                            <td>
                                <div style="width: 91px; float: left; padding-right: 30px; text-align: right;">
                                    <%# Convert.ToDecimal(Eval("MmnDomInflow")) + Convert.ToDecimal(Eval("PdkDomInflow")) + Convert.ToDecimal(Eval("CfDomInflow")) %>
                                </div>
                            </td>
                            <td>
                                <div style="width: 91px; float: left; padding-right: 30px; text-align: right;">
                                    <%# Convert.ToDecimal(Eval("MmnDomDispatch")) + Convert.ToDecimal(Eval("PdkDomDispatch")) + Convert.ToDecimal(Eval("CfDomDispatch")) %>
                                </div>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:Repeater>
        </div>
        <div style="padding-left: 280px; padding-top: 20px; line-height: 25px;">
            <asp:GridView runat="server" ID="gv_DomesticLast12Months" AutoGenerateColumns="false"
                AlternatingRowStyle-BackColor="#DDD5D5" Width="500px" HeaderStyle-BackColor="#BCDFF3"
                Font-Bold="true" RowStyle-BackColor="#F2F3F3">
                <Columns>
                    <asp:TemplateField HeaderText="HISTORY" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <span>
                                <%# Eval("MonthField")%>
                                &nbsp;
                                <%# Eval("AsYear")%></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="INFLOW MONTH" ItemStyle-Width="100px" DataField="MonthInflow"
                        ItemStyle-CssClass="gvmonthsTD" />
                    <asp:BoundField HeaderText="DISPATCH MONTH" ItemStyle-Width="100px" DataField="MonthDispatch"
                        ItemStyle-CssClass="gvmonthsTD" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
