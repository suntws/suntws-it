<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="orderpositionshow.aspx.cs" Inherits="TTS.orderpositionshow" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        th
        {
            line-height: 15px;
        }
        .todayTable
        {
            border-color: #07A714;
            margin-top: 5px;
            line-height: 22px;
            width: 400px;
        }
        .todayTable td:nth-child(1)
        {
            font-weight: bold;
            width: 185px;
            background-color: #ffe061;
        }
        .todayTable td:nth-child(2)
        {
            width: 90px;
            float: left;
            text-align: right;
            padding-right: 12px;
            background-color: #CCCCCC;
        }
        .todayTable td:nth-child(3)
        {
            width: 90px;
            float: left;
            text-align: right;
            padding-right: 12px;
            background-color: #CCCCCC;
        }
        .todayTableTotal
        {
            width: 91px;
            float: left;
            padding-right: 10px;
            text-align: right;
            background-color: #ffe061;
        }
        .monthTable
        {
            border-color: #07A714;
            line-height: 25px;
            width: 530px;
            background-color: #9ce159;
        }
        .monthTable td:nth-child(1)
        {
            font-weight: bold;
            width: 200px;
            background-color: #B3F6C9;
        }
        .monthTable td:nth-child(2)
        {
            width: 90px;
            float: left;
            text-align: right;
            padding-right: 12px;
        }
        .monthTable td:nth-child(3)
        {
            width: 90px;
            float: left;
            text-align: right;
            padding-right: 12px;
        }
        .monthTable td:nth-child(4)
        {
            width: 90px;
            float: left;
            text-align: right;
            padding-right: 12px;
        }
        .monthTableTotal
        {
            width: 92px;
            float: left;
            padding-right: 10px;
            text-align: right;
            font-size: 16px;
            background-color: #B3F6C9;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div align="center" class="pageTitleHead">
        DASHBOARD ALL</div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <div style="padding-left: 5px;">
            <table>
                <tr>
                    <td style="padding: 5px; width: 400px; float: left;">
                        <asp:Repeater runat="server" ID="rptOrderPositionToday">
                            <ItemTemplate>
                                <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse;"
                                    class="todayTable">
                                    <tr style="text-align: center; font-weight: bold; font-size: 15px; background-color: #FFF500;">
                                        <td colspan="3">
                                            <div style="background-color: #2196F3;">
                                                <span>Last updated on : </span>
                                                <%# Eval("AsOnDate")%>
                                                <span style="padding-left: 20px;">By : </span>
                                                <%# Eval("UserName")%>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr style="text-align: center; font-weight: bold; font-size: 13px;">
                                        <td style="background-color: #CCCCCC; width: 150px;">
                                            TODAY
                                        </td>
                                        <td>
                                            <div class="todayTableTotal">
                                                INFLOW
                                            </div>
                                        </td>
                                        <td>
                                            <div class="todayTableTotal">
                                                DISPATCH
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            SLTL (EXPORT)
                                        </td>
                                        <td>
                                            <%# Eval("LankaExpInflow")%>
                                        </td>
                                        <td>
                                            <%# Eval("LankaExpDispatch")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            SITL DIRECT (EXPORT)
                                        </td>
                                        <td>
                                            <%# Eval("StarcoExpInflow")%>
                                        </td>
                                        <td>
                                            <%# Eval("StarcoExpDispatch")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            SITL JOB WORK (EXPORT)
                                        </td>
                                        <td>
                                            <%# Eval("StarcoJobWorkInflow")%>
                                        </td>
                                        <td>
                                            <%# Eval("StarcoJobWorkDispatch")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            MMN (EXPORT)
                                        </td>
                                        <td>
                                            <%# Eval("MmnExpInflow")%>
                                        </td>
                                        <td>
                                            <%# Eval("MmnExpDispatch")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            PDK (EXPORT)
                                        </td>
                                        <td>
                                            <%# Eval("PdkExpInflow")%>
                                        </td>
                                        <td>
                                            <%# Eval("PdkExpDispatch")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            PNEUMATIC (EXPORT)
                                        </td>
                                        <td>
                                            <%# Eval("PneuExpInflow")%>
                                        </td>
                                        <td>
                                            <%# Eval("PneuExpDispatch")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            RIMS (EXPORT)
                                        </td>
                                        <td>
                                            <%# Eval("RimsExpInflow")%>
                                        </td>
                                        <td>
                                            <%# Eval("RimsExpDispatch")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            MMN (DOMESTIC)
                                        </td>
                                        <td>
                                            <%# Eval("MmnDomInflow")%>
                                        </td>
                                        <td>
                                            <%# Eval("MmnDomDispatch")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            PDK (DOMESTIC)
                                        </td>
                                        <td>
                                            <%# Eval("PdkDomInflow")%>
                                        </td>
                                        <td>
                                            <%# Eval("PdkDomDispatch")%>
                                        </td>
                                    </tr>
                                    <tr style="display: none;">
                                        <td>
                                            C&F (DOMESTIC)
                                        </td>
                                        <td>
                                            <%# Eval("CfDomInflow")%>
                                        </td>
                                        <td>
                                            <%# Eval("CfDomDispatch")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            MMN (ME)
                                        </td>
                                        <td>
                                            <%# Eval("MmnMeInflow")%>
                                        </td>
                                        <td>
                                            <%# Eval("MmnMeDispatch")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            PDK (ME)
                                        </td>
                                        <td>
                                            <%# Eval("PdkMeInflow")%>
                                        </td>
                                        <td>
                                            <%# Eval("PdkMeDispatch")%>
                                        </td>
                                    </tr>
                                    <tr style="font-weight: bold; background-color: #F1F0E8;">
                                        <td style="font-weight: bold; font-size: 14px; background-color: #CCCCCC; text-align: right;">
                                            TOTAL TODAY
                                        </td>
                                        <td>
                                            <div class="todayTableTotal">
                                                <%# Eval("InflowTot")%>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="todayTableTotal">
                                                <%# Eval("DispatchTot")%>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:Repeater>
                    </td>
                    <td style="padding: 5px; width: 650px; float: left;">
                        <asp:Repeater runat="server" ID="rptOrderPositionMonth">
                            <ItemTemplate>
                                <table cellspacing="0" rules="all" border="1" class="monthTable" style="border-collapse: collapse;
                                    margin-left: 100px; margin-top: 5px;">
                                    <tr style="text-align: center; font-weight: bold; font-size: 13px; background-color: #B3F6C9;">
                                        <td style="background-color: #9ce159; width: 210px;">
                                            <%# Eval("AsMonth")%>
                                            &nbsp;
                                            <%# Eval("AsYear")%>
                                        </td>
                                        <td style="text-align: center; padding-right: 0px; width: 102px;">
                                            INFLOW
                                        </td>
                                        <td style="text-align: center; padding-right: 0px; width: 102px;">
                                            DISPATCH
                                        </td>
                                        <td style="text-align: center; padding-right: 0px; width: 102px;">
                                            BACKLOG
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            SLTL (EXPORT)
                                        </td>
                                        <td>
                                            <%# Eval("LankaExpInflow")%>
                                        </td>
                                        <td>
                                            <%# Eval("LankaExpDispatch")%>
                                        </td>
                                        <td>
                                            <%# Eval("LankaExpBacklog")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            SITL DIRECT (EXPORT)
                                        </td>
                                        <td>
                                            <%# Eval("StarcoExpInflow")%>
                                        </td>
                                        <td>
                                            <%# Eval("StarcoExpDispatch")%>
                                        </td>
                                        <td>
                                            <%# Eval("StarcoExpBacklog")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            SITL JOB WORK (EXPORT)
                                        </td>
                                        <td>
                                            <%# Eval("StarcoJobWorkInflow")%>
                                        </td>
                                        <td>
                                            <%# Eval("StarcoJobWorkDispatch")%>
                                        </td>
                                        <td>
                                            <%# Eval("StarcoJobWorkBacklog")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            MMN (EXPORT)
                                        </td>
                                        <td>
                                            <%# Eval("MmnExpInflow")%>
                                        </td>
                                        <td>
                                            <%# Eval("MmnExpDispatch")%>
                                        </td>
                                        <td>
                                            <%# Eval("MmnExpBacklog")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            PDK (EXPORT)
                                        </td>
                                        <td>
                                            <%# Eval("PdkExpInflow")%>
                                        </td>
                                        <td>
                                            <%# Eval("PdkExpDispatch")%>
                                        </td>
                                        <td>
                                            <%# Eval("PdkExpBacklog")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            PNEUMATIC (EXPORT)
                                        </td>
                                        <td>
                                            <%# Eval("PneuExpInflow")%>
                                        </td>
                                        <td>
                                            <%# Eval("PneuExpDispatch")%>
                                        </td>
                                        <td>
                                            <%# Eval("PneuExpBacklog")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            RIMS (EXPORT)
                                        </td>
                                        <td>
                                            <%# Eval("RimsExpInflow")%>
                                        </td>
                                        <td>
                                            <%# Eval("RimsExpDispatch")%>
                                        </td>
                                        <td>
                                            <%# Eval("RimsExpBacklog")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            MMN (DOMESTIC)
                                        </td>
                                        <td>
                                            <%# Eval("MmnDomInflow")%>
                                        </td>
                                        <td>
                                            <%# Eval("MmnDomDispatch")%>
                                        </td>
                                        <td>
                                            <%# Eval("MmnDomBacklog")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            PDK (DOMESTIC)
                                        </td>
                                        <td>
                                            <%# Eval("PdkDomInflow")%>
                                        </td>
                                        <td>
                                            <%# Eval("PdkDomDispatch")%>
                                        </td>
                                        <td>
                                            <%# Eval("PdkDomBacklog")%>
                                        </td>
                                    </tr>
                                    <tr style="display: none;">
                                        <td>
                                            C&F (DOMESTIC)
                                        </td>
                                        <td>
                                            <%# Eval("CfDomInflow")%>
                                        </td>
                                        <td>
                                            <%# Eval("CfDomDispatch")%>
                                        </td>
                                        <td>
                                            <%# Eval("CfDomBacklog")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            MMN (ME)
                                        </td>
                                        <td>
                                            <%# Eval("MmnMeInflow")%>
                                        </td>
                                        <td>
                                            <%# Eval("MmnMeDispatch")%>
                                        </td>
                                        <td>
                                            <%# Eval("MmnMeBacklog")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            PDK (ME)
                                        </td>
                                        <td>
                                            <%# Eval("PdkMeInflow")%>
                                        </td>
                                        <td>
                                            <%# Eval("PdkMeDispatch")%>
                                        </td>
                                        <td>
                                            <%# Eval("PdkMeBacklog")%>
                                        </td>
                                    </tr>
                                    <tr style="font-weight: bold;">
                                        <td style="font-size: 14px; background-color: #9ce159; text-align: right;">
                                            TOTAL MONTH
                                        </td>
                                        <td>
                                            <div class="monthTableTotal">
                                                <%# Eval("InflowTot")%>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="monthTableTotal">
                                                <%# Eval("DispatchTot")%>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="monthTableTotal">
                                                <%# Eval("BacklogTot")%>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:Repeater>
                    </td>
                </tr>
                <tr>
                    <td style="padding-left: 5px; padding-top: 10px; line-height: 25px; width: 400px;
                        float: left;">
                        <asp:GridView runat="server" ID="gv_Last12Months" AutoGenerateColumns="false" AlternatingRowStyle-BackColor="#DDD5D5"
                            Width="400px" HeaderStyle-BackColor="#BCDFF3" Font-Bold="true" RowStyle-BackColor="#F2F3F3">
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
                                <asp:BoundField HeaderText="BACKLOG MONTH" ItemStyle-Width="100px" DataField="MonthBacklog"
                                    ItemStyle-CssClass="gvmonthsTD" />
                            </Columns>
                        </asp:GridView>
                    </td>
                    <td style="width: 650px; float: left;">
                        <div style="width: 600px; float: left; padding-left: 50px;">
                            <span class="headCss">Chart Type :</span>
                            <asp:DropDownList runat="server" ID="ddlChartType" ClientIDMode="Static" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlChartType_IndexChanged">
                            </asp:DropDownList>
                        </div>
                        <div style="width: 650px; float: left;">
                            <asp:Chart runat="server" ID="Chart1" Height="425px" Width="650px" ViewStateContent="All">
                                <Series>
                                    <asp:Series Name="Inflow">
                                    </asp:Series>
                                    <asp:Series Name="Dispatch">
                                    </asp:Series>
                                    <asp:Series Name="Closing">
                                    </asp:Series>
                                </Series>
                                <ChartAreas>
                                    <asp:ChartArea Name="studentChartArea" BackColor="NavajoWhite" BackGradientStyle="LeftRight"
                                        ShadowOffset="5">
                                        <AxisX Title="" IsLabelAutoFit="True">
                                            <LabelStyle Angle="-90" Interval="1" />
                                        </AxisX>
                                    </asp:ChartArea>
                                </ChartAreas>
                                <Legends>
                                    <asp:Legend Name="Inflow" BackColor="White" Docking="Bottom">
                                    </asp:Legend>
                                </Legends>
                            </asp:Chart>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
