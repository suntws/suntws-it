<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderPositionDetailsViewAll.aspx.cs"
    MasterPageFile="~/master.Master" Inherits="TTS.OrderPositionDetailsViewAll" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/orderposition.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div align="center" class="pageTitleHead">
        ORDER POSITION REPORT
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table>
            <tr>
                <td>
                    <div>
                        <!---For MonthReport--->
                        <asp:Repeater runat="server" ID="rptMonthRecord" ClientIDMode="Static">
                            <ItemTemplate>
                                <table cellspacing="0" rules="all" border="1" class="tbMainMonth">
                                    <tr style="background-color: #8ce054; color: #ffffff; font-size: 18px;">
                                        <th style="background-color: #26bd93;">
                                            <%# Eval("AsMonth")%>
                                            -
                                            <%# Eval("AsYear")%>
                                        </th>
                                        <th>
                                            INFLOW
                                        </th>
                                        <th>
                                            DESPATCH
                                        </th>
                                        <th>
                                            BACKLOG
                                        </th>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top;">
                                            <table cellspacing="0" rules="all" border="1" class="tbHeadMonth">
                                                <tr>
                                                    <th style="line-height: 25px;">
                                                        TYPE
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        SLTL (EXPORT)
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        SITL (EXPORT)
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        MMN (EXPORT)
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        PDK (EXPORT)
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        MMN (DOM+C&F)
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        PDK (DOM+C&F)
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th style="background-color: #631009; color: #ffffff; font-weight: bold; font-size: 14px;">
                                                        GRAND TOTAL
                                                    </th>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="vertical-align: top;">
                                            <table cellspacing="0" rules="all" border="1" class="tbInflowMonth">
                                                <tr>
                                                    <th>
                                                        SOLID TYRE
                                                    </th>
                                                    <th>
                                                        RIM
                                                    </th>
                                                    <th>
                                                        PNEUMATICS
                                                    </th>
                                                    <th>
                                                        TOTAL
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%# Eval("LankaExp_TyreInflow")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("LankaExp_RimInflow")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("LankaExp_PneumaticsInflow")%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("LankaExp_TyreInflow").ToString()) + Convert.ToDecimal(Eval("LankaExp_RimInflow").ToString()) + Convert.ToDecimal(Eval("LankaExp_PneumaticsInflow").ToString())%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%# Eval("StarcoExp_TyreInflow")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("StarcoExp_RimInflow")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("StarcoExp_PneumaticsInflow")%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("StarcoExp_TyreInflow").ToString()) + Convert.ToDecimal(Eval("StarcoExp_RimInflow").ToString()) + Convert.ToDecimal(Eval("StarcoExp_PneumaticsInflow").ToString())%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%# Eval("mmnExp_TyreInflow")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("mmnExp_RimInflow")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("mmnExp_PneumaticsInflow")%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("mmnExp_TyreInflow").ToString()) + Convert.ToDecimal(Eval("mmnExp_RimInflow").ToString()) + Convert.ToDecimal(Eval("mmnExp_PneumaticsInflow").ToString())%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%# Eval("pdkExp_TyreInflow")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("pdkExp_RimInflow")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("pdkExp_PneumaticsInflow")%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("pdkExp_TyreInflow").ToString()) + Convert.ToDecimal(Eval("pdkExp_RimInflow").ToString()) + Convert.ToDecimal(Eval("pdkExp_PneumaticsInflow").ToString())%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("mmnDom_TyreInflow").ToString()) + Convert.ToDecimal(Eval("mmnCF_TyreInflow").ToString())%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("mmnDom_RimInflow").ToString()) + Convert.ToDecimal(Eval("mmnCF_RimInflow").ToString())%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("mmnDom_PneumaticsInflow").ToString()) + Convert.ToDecimal(Eval("mmnCF_PneumaticsInflow").ToString())%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("mmnDom_TyreInflow").ToString()) + Convert.ToDecimal(Eval("mmnDom_RimInflow").ToString()) + Convert.ToDecimal(Eval("mmnDom_PneumaticsInflow").ToString())
                                            + Convert.ToDecimal(Eval("mmnCF_TyreInflow").ToString()) + Convert.ToDecimal(Eval("mmnCF_RimInflow").ToString()) + Convert.ToDecimal(Eval("mmnCF_PneumaticsInflow").ToString())%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("pdkDom_TyreInflow").ToString()) + Convert.ToDecimal(Eval("pdkCF_TyreInflow").ToString())%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("pdkDom_RimInflow").ToString()) + Convert.ToDecimal(Eval("pdkCF_RimInflow").ToString())%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("pdkDom_PneumaticsInflow").ToString()) + Convert.ToDecimal(Eval("pdkCF_PneumaticsInflow").ToString())%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("pdkDom_TyreInflow").ToString()) + Convert.ToDecimal(Eval("pdkDom_RimInflow").ToString()) + Convert.ToDecimal(Eval("pdkDom_PneumaticsInflow").ToString())
                                            + Convert.ToDecimal(Eval("pdkCF_TyreInflow").ToString()) + Convert.ToDecimal(Eval("pdkCF_RimInflow").ToString()) + Convert.ToDecimal(Eval("pdkCF_PneumaticsInflow").ToString())%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="background-color: #e0d6ff; font-size: 16px;">
                                                        <%# Eval("InflowTyreTotal")%>
                                                    </td>
                                                    <td style="background-color: #e0d6ff; font-size: 16px;">
                                                        <%# Eval("InflowRimTotal")%>
                                                    </td>
                                                    <td style="background-color: #e0d6ff; font-size: 16px;">
                                                        <%# Eval("InflowPneuTotal")%>
                                                    </td>
                                                    <td style="background-color: #fbb2de; font-size: 20px;">
                                                        <%# Convert.ToDecimal(Eval("InflowTyreTotal").ToString()) + Convert.ToDecimal(Eval("InflowRimTotal").ToString()) + Convert.ToDecimal(Eval("InflowPneuTotal").ToString())%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="vertical-align: top;">
                                            <table cellspacing="0" rules="all" border="1" class="tbDespatchMonth">
                                                <tr>
                                                    <th>
                                                        SOLID TYRE
                                                    </th>
                                                    <th>
                                                        RIM
                                                    </th>
                                                    <th>
                                                        PNEUMATICS
                                                    </th>
                                                    <th>
                                                        TOTAL
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%# Eval("LankaExp_TyreDespatch")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("LankaExp_RimDespatch")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("LankaExp_PneumaticsDespatch")%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("LankaExp_TyreDespatch").ToString()) + Convert.ToDecimal(Eval("LankaExp_RimDespatch").ToString()) + Convert.ToDecimal(Eval("LankaExp_PneumaticsDespatch").ToString())%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%# Eval("StarcoExp_TyreDespatch")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("StarcoExp_RimDespatch")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("StarcoExp_PneumaticsDespatch")%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("StarcoExp_TyreDespatch").ToString()) + Convert.ToDecimal(Eval("StarcoExp_RimDespatch").ToString()) + Convert.ToDecimal(Eval("StarcoExp_PneumaticsDespatch").ToString())%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%# Eval("mmnExp_TyreDespatch")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("mmnExp_RimDespatch")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("mmnExp_PneumaticsDespatch")%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("mmnExp_TyreDespatch").ToString()) + Convert.ToDecimal(Eval("mmnExp_RimDespatch").ToString()) + Convert.ToDecimal(Eval("mmnExp_PneumaticsDespatch").ToString())%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%# Eval("pdkExp_TyreDespatch")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("pdkExp_RimDespatch")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("pdkExp_PneumaticsDespatch")%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("pdkExp_TyreDespatch").ToString()) + Convert.ToDecimal(Eval("pdkExp_RimDespatch").ToString()) + Convert.ToDecimal(Eval("pdkExp_PneumaticsDespatch").ToString())%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("mmnDom_TyreDespatch").ToString()) + Convert.ToDecimal(Eval("mmnCF_TyreDespatch").ToString())%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("mmnDom_RimDespatch").ToString()) + Convert.ToDecimal(Eval("mmnCF_RimDespatch").ToString())%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("mmnDom_PneumaticsDespatch").ToString()) + Convert.ToDecimal(Eval("mmnCF_PneumaticsDespatch").ToString())%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("mmnDom_TyreDespatch").ToString()) + Convert.ToDecimal(Eval("mmnDom_RimDespatch").ToString()) + Convert.ToDecimal(Eval("mmnDom_PneumaticsDespatch").ToString())
                                            + Convert.ToDecimal(Eval("mmnCF_TyreDespatch").ToString()) + Convert.ToDecimal(Eval("mmnCF_RimDespatch").ToString()) + Convert.ToDecimal(Eval("mmnCF_PneumaticsDespatch").ToString())%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("pdkDom_TyreDespatch").ToString()) + Convert.ToDecimal(Eval("pdkCF_TyreDespatch").ToString())%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("pdkDom_RimDespatch").ToString()) + Convert.ToDecimal(Eval("pdkCF_RimDespatch").ToString())%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("pdkDom_PneumaticsDespatch").ToString()) + Convert.ToDecimal(Eval("pdkCF_PneumaticsDespatch").ToString())%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("pdkDom_TyreDespatch").ToString()) + Convert.ToDecimal(Eval("pdkDom_RimDespatch").ToString()) + Convert.ToDecimal(Eval("pdkDom_PneumaticsDespatch").ToString())
                                            + Convert.ToDecimal(Eval("pdkCF_TyreDespatch").ToString()) + Convert.ToDecimal(Eval("pdkCF_RimDespatch").ToString()) + Convert.ToDecimal(Eval("pdkCF_PneumaticsDespatch").ToString())%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="background-color: #fcedc7; font-size: 16px;">
                                                        <%# Eval("DespatchTyreTotal")%>
                                                    </td>
                                                    <td style="background-color: #fcedc7; font-size: 16px;">
                                                        <%# Eval("DespatchRimTotal")%>
                                                    </td>
                                                    <td style="background-color: #fcedc7; font-size: 16px;">
                                                        <%# Eval("DespatchPneuTotal")%>
                                                    </td>
                                                    <td style="background-color: #b8d3fd; font-size: 20px;">
                                                        <%# Convert.ToDecimal(Eval("DespatchTyreTotal").ToString()) + Convert.ToDecimal(Eval("DespatchRimTotal").ToString()) + Convert.ToDecimal(Eval("DespatchPneuTotal").ToString())%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="vertical-align: top;">
                                            <table cellspacing="0" rules="all" border="1" class="tbBacklogMonth">
                                                <tr>
                                                    <th>
                                                        SOLID TYRE
                                                    </th>
                                                    <th>
                                                        RIM
                                                    </th>
                                                    <th>
                                                        PNEUMATICS
                                                    </th>
                                                    <th>
                                                        TOTAL
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%# Eval("LankaExp_TyreBacklog")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("LankaExp_RimBacklog")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("LankaExp_PneumaticsBacklog")%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("LankaExp_TyreBacklog").ToString()) + Convert.ToDecimal(Eval("LankaExp_RimBacklog").ToString()) + Convert.ToDecimal(Eval("LankaExp_PneumaticsBacklog").ToString())%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%# Eval("StarcoExp_TyreBacklog")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("StarcoExp_RimBacklog")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("StarcoExp_PneumaticsBacklog")%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("StarcoExp_TyreBacklog").ToString()) + Convert.ToDecimal(Eval("StarcoExp_RimBacklog").ToString()) + Convert.ToDecimal(Eval("StarcoExp_PneumaticsBacklog").ToString())%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%# Eval("mmnExp_TyreBacklog")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("mmnExp_RimBacklog")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("mmnExp_PneumaticsBacklog")%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("mmnExp_TyreBacklog").ToString()) + Convert.ToDecimal(Eval("mmnExp_RimBacklog").ToString()) + Convert.ToDecimal(Eval("mmnExp_PneumaticsBacklog").ToString())%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%# Eval("pdkExp_TyreBacklog")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("pdkExp_RimBacklog")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("pdkExp_PneumaticsBacklog")%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("pdkExp_TyreBacklog").ToString()) + Convert.ToDecimal(Eval("pdkExp_RimBacklog").ToString()) + Convert.ToDecimal(Eval("pdkExp_PneumaticsBacklog").ToString())%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("mmnDom_TyreBacklog").ToString()) + Convert.ToDecimal(Eval("mmnCF_TyreBacklog").ToString())%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("mmnDom_RimBacklog").ToString()) + Convert.ToDecimal(Eval("mmnCF_RimBacklog").ToString())%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("mmnDom_PneumaticsBacklog").ToString()) + Convert.ToDecimal(Eval("mmnCF_PneumaticsBacklog").ToString())%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("mmnDom_TyreBacklog").ToString()) + Convert.ToDecimal(Eval("mmnDom_RimBacklog").ToString()) + Convert.ToDecimal(Eval("mmnDom_PneumaticsBacklog").ToString())
                                            + Convert.ToDecimal(Eval("mmnCF_TyreBacklog").ToString()) + Convert.ToDecimal(Eval("mmnCF_RimBacklog").ToString()) + Convert.ToDecimal(Eval("mmnCF_PneumaticsBacklog").ToString())%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("pdkDom_TyreBacklog").ToString()) + Convert.ToDecimal(Eval("pdkCF_TyreBacklog").ToString())%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("pdkDom_RimBacklog").ToString()) + Convert.ToDecimal(Eval("pdkCF_RimBacklog").ToString())%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("pdkDom_PneumaticsBacklog").ToString()) + Convert.ToDecimal(Eval("pdkCF_PneumaticsBacklog").ToString())%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("pdkDom_TyreBacklog").ToString()) + Convert.ToDecimal(Eval("pdkDom_RimBacklog").ToString()) + Convert.ToDecimal(Eval("pdkDom_PneumaticsBacklog").ToString())
                                            + Convert.ToDecimal(Eval("pdkCF_TyreBacklog").ToString()) + Convert.ToDecimal(Eval("pdkCF_RimBacklog").ToString()) + Convert.ToDecimal(Eval("pdkCF_PneumaticsBacklog").ToString())%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="background-color: #ffdbe0; font-size: 16px;">
                                                        <%# Eval("BacklogTyreTotal")%>
                                                    </td>
                                                    <td style="background-color: #ffdbe0; font-size: 16px;">
                                                        <%# Eval("BacklogRimTotal")%>
                                                    </td>
                                                    <td style="background-color: #ffdbe0; font-size: 16px;">
                                                        <%# Eval("BacklogPneuTotal")%>
                                                    </td>
                                                    <td style="background-color: #b1fdc5; font-size: 20px;">
                                                        <%# Convert.ToDecimal(Eval("BacklogTyreTotal").ToString()) + Convert.ToDecimal(Eval("BacklogRimTotal").ToString()) + Convert.ToDecimal(Eval("BacklogPneuTotal").ToString())%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: center;">
                    <hr style="width: 1060px; float: left; border-width: 7px; border-color: #aeb7ae;" />
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <!---For LastUpdatedDayReport--->
                        <asp:Repeater runat="server" ID="rptLastUpdatedRecord" ClientIDMode="Static">
                            <ItemTemplate>
                                <table cellspacing="0" rules="all" border="1" class="tbMain">
                                    <tr style="background-color: #496077; color: #ffffff; font-size: 18px;">
                                        <th style="line-height: 13px;">
                                            <span style="font-size: 9px;">LAST UPDATED ON</span>
                                            <%# Eval("AsonDate")%>
                                        </th>
                                        <th>
                                            INFLOW
                                        </th>
                                        <th>
                                            DESPATCH
                                        </th>
                                        <th>
                                            BACKLOG
                                        </th>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top;">
                                            <table cellspacing="0" rules="all" border="1" class="tbHead">
                                                <tr>
                                                    <th style="line-height: 25px;">
                                                        TYPE
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        SLTL (EXPORT)
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        SITL (EXPORT)
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        MMN (EXPORT)
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        PDK (EXPORT)
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        MMN (DOM+C&F)
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        PDK (DOM+C&F)
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th style="background-color: #631009; color: #ffffff; font-weight: bold; font-size: 14px;">
                                                        GRAND TOTAL
                                                    </th>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="vertical-align: top;">
                                            <table cellspacing="0" rules="all" border="1" class="tbInflow">
                                                <tr>
                                                    <th>
                                                        SOLID TYRE
                                                    </th>
                                                    <th>
                                                        RIM
                                                    </th>
                                                    <th>
                                                        PNEUMATICS
                                                    </th>
                                                    <th>
                                                        TOTAL
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%# Eval("LankaExp_TyreInflow")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("LankaExp_RimInflow")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("LankaExp_PneumaticsInflow")%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("LankaExp_TyreInflow").ToString()) + Convert.ToDecimal(Eval("LankaExp_RimInflow").ToString()) + Convert.ToDecimal(Eval("LankaExp_PneumaticsInflow").ToString())%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%# Eval("StarcoExp_TyreInflow")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("StarcoExp_RimInflow")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("StarcoExp_PneumaticsInflow")%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("StarcoExp_TyreInflow").ToString()) + Convert.ToDecimal(Eval("StarcoExp_RimInflow").ToString()) + Convert.ToDecimal(Eval("StarcoExp_PneumaticsInflow").ToString())%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%# Eval("mmnExp_TyreInflow")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("mmnExp_RimInflow")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("mmnExp_PneumaticsInflow")%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("mmnExp_TyreInflow").ToString()) + Convert.ToDecimal(Eval("mmnExp_RimInflow").ToString()) + Convert.ToDecimal(Eval("mmnExp_PneumaticsInflow").ToString())%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%# Eval("pdkExp_TyreInflow")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("pdkExp_RimInflow")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("pdkExp_PneumaticsInflow")%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("pdkExp_TyreInflow").ToString()) + Convert.ToDecimal(Eval("pdkExp_RimInflow").ToString()) + Convert.ToDecimal(Eval("pdkExp_PneumaticsInflow").ToString())%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("mmnDom_TyreInflow").ToString()) + Convert.ToDecimal(Eval("mmnCF_TyreInflow").ToString())%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("mmnDom_RimInflow").ToString()) + Convert.ToDecimal(Eval("mmnCF_RimInflow").ToString())%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("mmnDom_PneumaticsInflow").ToString()) + Convert.ToDecimal(Eval("mmnCF_PneumaticsInflow").ToString())%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("mmnDom_TyreInflow").ToString()) + Convert.ToDecimal(Eval("mmnDom_RimInflow").ToString()) + Convert.ToDecimal(Eval("mmnDom_PneumaticsInflow").ToString())
                                            + Convert.ToDecimal(Eval("mmnCF_TyreInflow").ToString()) + Convert.ToDecimal(Eval("mmnCF_RimInflow").ToString()) + Convert.ToDecimal(Eval("mmnCF_PneumaticsInflow").ToString())%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("pdkDom_TyreInflow").ToString()) + Convert.ToDecimal(Eval("pdkCF_TyreInflow").ToString())%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("pdkDom_RimInflow").ToString()) + Convert.ToDecimal(Eval("pdkCF_RimInflow").ToString())%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("pdkDom_PneumaticsInflow").ToString()) + Convert.ToDecimal(Eval("pdkCF_PneumaticsInflow").ToString())%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("pdkDom_TyreInflow").ToString()) + Convert.ToDecimal(Eval("pdkDom_RimInflow").ToString()) + Convert.ToDecimal(Eval("pdkDom_PneumaticsInflow").ToString())
                                            + Convert.ToDecimal(Eval("pdkCF_TyreInflow").ToString()) + Convert.ToDecimal(Eval("pdkCF_RimInflow").ToString()) + Convert.ToDecimal(Eval("pdkCF_PneumaticsInflow").ToString())%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="background-color: #c7baf1;">
                                                        <%# Eval("InflowTyreTotal")%>
                                                    </td>
                                                    <td style="background-color: #c7baf1;">
                                                        <%# Eval("InflowRimTotal")%>
                                                    </td>
                                                    <td style="background-color: #c7baf1;">
                                                        <%# Eval("InflowPneuTotal")%>
                                                    </td>
                                                    <td style="background-color: #c7baf1;">
                                                        <%# Convert.ToDecimal(Eval("InflowTyreTotal").ToString()) + Convert.ToDecimal(Eval("InflowRimTotal").ToString()) + Convert.ToDecimal(Eval("InflowPneuTotal").ToString())%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="vertical-align: top;">
                                            <table cellspacing="0" rules="all" border="1" class="tbDespatch">
                                                <tr>
                                                    <th>
                                                        SOLID TYRE
                                                    </th>
                                                    <th>
                                                        RIM
                                                    </th>
                                                    <th>
                                                        PNEUMATICS
                                                    </th>
                                                    <th>
                                                        TOTAL
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%# Eval("LankaExp_TyreDespatch")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("LankaExp_RimDespatch")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("LankaExp_PneumaticsDespatch")%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("LankaExp_TyreDespatch").ToString()) + Convert.ToDecimal(Eval("LankaExp_RimDespatch").ToString()) + Convert.ToDecimal(Eval("LankaExp_PneumaticsDespatch").ToString())%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%# Eval("StarcoExp_TyreDespatch")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("StarcoExp_RimDespatch")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("StarcoExp_PneumaticsDespatch")%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("StarcoExp_TyreDespatch").ToString()) + Convert.ToDecimal(Eval("StarcoExp_RimDespatch").ToString()) + Convert.ToDecimal(Eval("StarcoExp_PneumaticsDespatch").ToString())%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%# Eval("mmnExp_TyreDespatch")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("mmnExp_RimDespatch")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("mmnExp_PneumaticsDespatch")%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("mmnExp_TyreDespatch").ToString()) + Convert.ToDecimal(Eval("mmnExp_RimDespatch").ToString()) + Convert.ToDecimal(Eval("mmnExp_PneumaticsDespatch").ToString())%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%# Eval("pdkExp_TyreDespatch")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("pdkExp_RimDespatch")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("pdkExp_PneumaticsDespatch")%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("pdkExp_TyreDespatch").ToString()) + Convert.ToDecimal(Eval("pdkExp_RimDespatch").ToString()) + Convert.ToDecimal(Eval("pdkExp_PneumaticsDespatch").ToString())%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("mmnDom_TyreDespatch").ToString()) + Convert.ToDecimal(Eval("mmnCF_TyreDespatch").ToString())%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("mmnDom_RimDespatch").ToString()) + Convert.ToDecimal(Eval("mmnCF_RimDespatch").ToString())%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("mmnDom_PneumaticsDespatch").ToString()) + Convert.ToDecimal(Eval("mmnCF_PneumaticsDespatch").ToString())%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("mmnDom_TyreDespatch").ToString()) + Convert.ToDecimal(Eval("mmnDom_RimDespatch").ToString()) + Convert.ToDecimal(Eval("mmnDom_PneumaticsDespatch").ToString())
                                            + Convert.ToDecimal(Eval("mmnCF_TyreDespatch").ToString()) + Convert.ToDecimal(Eval("mmnCF_RimDespatch").ToString()) + Convert.ToDecimal(Eval("mmnCF_PneumaticsDespatch").ToString())%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("pdkDom_TyreDespatch").ToString()) + Convert.ToDecimal(Eval("pdkCF_TyreDespatch").ToString())%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("pdkDom_RimDespatch").ToString()) + Convert.ToDecimal(Eval("pdkCF_RimDespatch").ToString())%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("pdkDom_PneumaticsDespatch").ToString()) + Convert.ToDecimal(Eval("pdkCF_PneumaticsDespatch").ToString())%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("pdkDom_TyreDespatch").ToString()) + Convert.ToDecimal(Eval("pdkDom_RimDespatch").ToString()) + Convert.ToDecimal(Eval("pdkDom_PneumaticsDespatch").ToString())
                                            + Convert.ToDecimal(Eval("pdkCF_TyreDespatch").ToString()) + Convert.ToDecimal(Eval("pdkCF_RimDespatch").ToString()) + Convert.ToDecimal(Eval("pdkCF_PneumaticsDespatch").ToString())%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="background-color: #fcedc7;">
                                                        <%# Eval("DespatchTyreTotal")%>
                                                    </td>
                                                    <td style="background-color: #fcedc7;">
                                                        <%# Eval("DespatchRimTotal")%>
                                                    </td>
                                                    <td style="background-color: #fcedc7;">
                                                        <%# Eval("DespatchPneuTotal")%>
                                                    </td>
                                                    <td style="background-color: #fcedc7;">
                                                        <%# Convert.ToDecimal(Eval("DespatchTyreTotal").ToString()) + Convert.ToDecimal(Eval("DespatchRimTotal").ToString()) + Convert.ToDecimal(Eval("DespatchPneuTotal").ToString())%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="vertical-align: top;">
                                            <table cellspacing="0" rules="all" border="1" class="tbBacklog">
                                                <tr>
                                                    <th>
                                                        SOLID TYRE
                                                    </th>
                                                    <th>
                                                        RIM
                                                    </th>
                                                    <th>
                                                        PNEUMATICS
                                                    </th>
                                                    <th>
                                                        TOTAL
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%# Eval("LankaExp_TyreBacklog")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("LankaExp_RimBacklog")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("LankaExp_PneumaticsBacklog")%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("LankaExp_TyreBacklog").ToString()) + Convert.ToDecimal(Eval("LankaExp_RimBacklog").ToString()) + Convert.ToDecimal(Eval("LankaExp_PneumaticsBacklog").ToString())%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%# Eval("StarcoExp_TyreBacklog")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("StarcoExp_RimBacklog")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("StarcoExp_PneumaticsBacklog")%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("StarcoExp_TyreBacklog").ToString()) + Convert.ToDecimal(Eval("StarcoExp_RimBacklog").ToString()) + Convert.ToDecimal(Eval("StarcoExp_PneumaticsBacklog").ToString())%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%# Eval("mmnExp_TyreBacklog")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("mmnExp_RimBacklog")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("mmnExp_PneumaticsBacklog")%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("mmnExp_TyreBacklog").ToString()) + Convert.ToDecimal(Eval("mmnExp_RimBacklog").ToString()) + Convert.ToDecimal(Eval("mmnExp_PneumaticsBacklog").ToString())%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%# Eval("pdkExp_TyreBacklog")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("pdkExp_RimBacklog")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("pdkExp_PneumaticsBacklog")%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("pdkExp_TyreBacklog").ToString()) + Convert.ToDecimal(Eval("pdkExp_RimBacklog").ToString()) + Convert.ToDecimal(Eval("pdkExp_PneumaticsBacklog").ToString())%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("mmnDom_TyreBacklog").ToString()) + Convert.ToDecimal(Eval("mmnCF_TyreBacklog").ToString())%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("mmnDom_RimBacklog").ToString()) + Convert.ToDecimal(Eval("mmnCF_RimBacklog").ToString())%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("mmnDom_PneumaticsBacklog").ToString()) + Convert.ToDecimal(Eval("mmnCF_PneumaticsBacklog").ToString())%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("mmnDom_TyreBacklog").ToString()) + Convert.ToDecimal(Eval("mmnDom_RimBacklog").ToString()) + Convert.ToDecimal(Eval("mmnDom_PneumaticsBacklog").ToString())
                                            + Convert.ToDecimal(Eval("mmnCF_TyreBacklog").ToString()) + Convert.ToDecimal(Eval("mmnCF_RimBacklog").ToString()) + Convert.ToDecimal(Eval("mmnCF_PneumaticsBacklog").ToString())%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("pdkDom_TyreBacklog").ToString()) + Convert.ToDecimal(Eval("pdkCF_TyreBacklog").ToString())%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("pdkDom_RimBacklog").ToString()) + Convert.ToDecimal(Eval("pdkCF_RimBacklog").ToString())%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("pdkDom_PneumaticsBacklog").ToString()) + Convert.ToDecimal(Eval("pdkCF_PneumaticsBacklog").ToString())%>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToDecimal(Eval("pdkDom_TyreBacklog").ToString()) + Convert.ToDecimal(Eval("pdkDom_RimBacklog").ToString()) + Convert.ToDecimal(Eval("pdkDom_PneumaticsBacklog").ToString())
                                            + Convert.ToDecimal(Eval("pdkCF_TyreBacklog").ToString()) + Convert.ToDecimal(Eval("pdkCF_RimBacklog").ToString()) + Convert.ToDecimal(Eval("pdkCF_PneumaticsBacklog").ToString())%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="background-color: #f9b3bd;">
                                                        <%# Eval("BacklogTyreTotal")%>
                                                    </td>
                                                    <td style="background-color: #f9b3bd;">
                                                        <%# Eval("BacklogRimTotal")%>
                                                    </td>
                                                    <td style="background-color: #f9b3bd;">
                                                        <%# Eval("BacklogPneuTotal")%>
                                                    </td>
                                                    <td style="background-color: #f9b3bd;">
                                                        <%# Convert.ToDecimal(Eval("BacklogTyreTotal").ToString()) + Convert.ToDecimal(Eval("BacklogRimTotal").ToString()) + Convert.ToDecimal(Eval("BacklogPneuTotal").ToString())%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
