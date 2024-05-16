<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="liquidationreport.aspx.cs" Inherits="TTS.liquidationreport" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
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
        <table cellspacing="0" rules="all" border="1" style="background-color: #dcecfb; width: 100%;
            border-color: White; border-collapse: separate;">
            <tr>
                <td>
                    PLANT
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlLiqPlant" ClientIDMode="Static" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlLiqPlant_IndexChange" CssClass="form-control" Width="80px">
                    </asp:DropDownList>
                </td>
                <td>
                    YEAR
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlLiqYear" ClientIDMode="Static" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlLiqYear_IndexChange" CssClass="form-control" Width="80px">
                    </asp:DropDownList>
                </td>
                <td>
                    FROM MONTH
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlLiqFromMonth" ClientIDMode="Static" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlLiqFromMonth_IndexChange" CssClass="form-control"
                        Width="100px">
                    </asp:DropDownList>
                </td>
                <td>
                    TO MONTH
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlLiqToMonth" ClientIDMode="Static" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlLiqToMonth_IndexChange" CssClass="form-control" Width="100px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="8">
                    <asp:GridView runat="server" ID="gvLiqTotalReport" ClientIDMode="Static" AutoGenerateColumns="false"
                        CssClass="gridcss" Width="100%" ShowFooter="true" FooterStyle-HorizontalAlign="Right"
                        FooterStyle-Font-Bold="true" Font-Size="16px" FooterStyle-Font-Size="17px">
                        <Columns>
                            <asp:BoundField HeaderText="YEAR" DataField="YEAR" />
                            <asp:BoundField HeaderText="MONTH" DataField="MONTH" />
                            <asp:BoundField HeaderText="PART-A" DataField="A_FWT" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField HeaderText="PART-B" DataField="B_FWT" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField HeaderText="PART-C" DataField="C_FWT" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField HeaderText="PART-D" DataField="D_FWT" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField HeaderText="PART-E" DataField="E_FWT" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField HeaderText="PART-F" DataField="F_FWT" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField HeaderText="PART-G" DataField="G_FWT" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField HeaderText="TOTAL" DataField="TOT_FWT" ItemStyle-HorizontalAlign="Right" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="6">
                    <asp:Label runat="server" ID="lblLiqDescription" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                </td>
                <td>
                    <span id="spnLink" runat="server" clientidmode="Static" onclick="Ctrl_LiqChartShow()"
                        style="font-weight: bold; cursor: pointer; padding: 6px; border-radius: 10px;
                        width: 100px; text-align: center;">SHOW CHART</span>
                </td>
                <td>
                    <asp:Button runat="server" ID="btnLiqDownload" ClientIDMode="Static" Text="DOWNLOAD"
                        CssClass="btn btn-success" OnClick="btnLiqDownload_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="8">
                    <table cellspacing="0" rules="all" border="1" style="background-color: #dcecfb; width: 100%;
                        border-color: White; border-collapse: separate; display: none;" id="tbLiqChart">
                        <tr>
                            <td>
                                <asp:Chart runat="server" ID="chartLiqPie" Width="1090px" Height="400px" ViewStateContent="All"
                                    BackColor="White" BorderlineColor="Olive" BorderlineDashStyle="Solid" BorderlineWidth="1">
                                    <Titles>
                                        <asp:Title Name="STOCK LIQUIDATION" Alignment="TopCenter" Text="STOCK LIQUIDATION"
                                            Font="Times New Roman, 15pt, style=Bold, Italic" ForeColor="Black">
                                        </asp:Title>
                                    </Titles>
                                    <Series>
                                        <asp:Series Name="PART" ToolTip="#VALY" Font="Times New Roman, 12pt, style=Bold"
                                            ChartType="Pie">
                                        </asp:Series>
                                    </Series>
                                    <ChartAreas>
                                        <asp:ChartArea Name="ChartPart" BackColor="NavajoWhite" BackGradientStyle="LeftRight">
                                            <AxisX Title="PART" IsLabelAutoFit="True" TitleForeColor="White" TitleFont="Times New Roman, 12pt, style=Bold">
                                                <LabelStyle Angle="-90" Interval="1" />
                                            </AxisX>
                                            <AxisY Title="TOTAL" IsLabelAutoFit="true" TitleForeColor="White" TitleFont="Times New Roman, 12pt, style=Bold">
                                                <LabelStyle Angle="90" />
                                            </AxisY>
                                        </asp:ChartArea>
                                    </ChartAreas>
                                    <Legends>
                                        <asp:Legend Name="PART" BackColor="White" Docking="Right">
                                        </asp:Legend>
                                    </Legends>
                                </asp:Chart>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Chart runat="server" ID="chartLiqColumn" Height="400px" Width="1090px" ViewStateContent="All">
                                    <Series>
                                        <asp:Series Name="A" ChartType="Column">
                                        </asp:Series>
                                        <asp:Series Name="B" ChartType="Column">
                                        </asp:Series>
                                        <asp:Series Name="C" ChartType="Column">
                                        </asp:Series>
                                        <asp:Series Name="D" ChartType="Column">
                                        </asp:Series>
                                        <asp:Series Name="E" ChartType="Column">
                                        </asp:Series>
                                        <asp:Series Name="F" ChartType="Column">
                                        </asp:Series>
                                    </Series>
                                    <ChartAreas>
                                        <asp:ChartArea Name="studentChartArea" BackColor="NavajoWhite" BackGradientStyle="LeftRight"
                                            ShadowOffset="10">
                                            <AxisX Title="PART" IsLabelAutoFit="true" TitleForeColor="White" TitleFont="Times New Roman, 12pt,style=Bold">
                                                <LabelStyle Angle="-90" Interval="1" />
                                            </AxisX>
                                            <AxisY Title="FWT" IsLabelAutoFit="true" TitleForeColor="White" TitleFont="Times New Roman, 12pt, style=Bold">
                                                <LabelStyle Angle="90" />
                                            </AxisY>
                                        </asp:ChartArea>
                                    </ChartAreas>
                                    <Legends>
                                        <asp:Legend Name="A" BackColor="White" Docking="Right">
                                        </asp:Legend>
                                    </Legends>
                                </asp:Chart>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="8">
                    <asp:GridView runat="server" ID="gvLiqCustReport" ClientIDMode="Static" AutoGenerateColumns="false"
                        CssClass="gridcss" Width="100%">
                        <Columns>
                            <asp:BoundField HeaderText="CUSTOMER" DataField="CUSTOMER" />
                            <asp:BoundField HeaderText="WORK ORDER" DataField="WORK ORDER" />
                            <asp:BoundField HeaderText="YEAR" DataField="YEAR" />
                            <asp:BoundField HeaderText="MONTH" DataField="MONTH" />
                            <asp:BoundField HeaderText="PART-A" DataField="A_FWT" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField HeaderText="PART-B" DataField="B_FWT" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField HeaderText="PART-C" DataField="C_FWT" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField HeaderText="PART-D" DataField="D_FWT" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField HeaderText="PART-E" DataField="E_FWT" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField HeaderText="PART-F" DataField="F_FWT" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField HeaderText="PART-G" DataField="G_FWT" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField HeaderText="TOTAL" DataField="TOT_FWT" ItemStyle-HorizontalAlign="Right" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        $(function () {
            blinkText();
        });

        function setblinkText() { $('#spnLink').css({ 'background-color': '#000', 'color': '#fff' }); setTimeout("blinkText()", 1000) }
        function blinkText() { $('#spnLink').css({ 'background-color': '#fff', 'color': '#000' }); setTimeout("setblinkText()", 1000) }

        function Ctrl_LiqChartShow() {
            if ($('#spnLink').html() == 'SHOW CHART') {
                $('#tbLiqChart').css({ 'display': 'block' });
                $('#spnLink').html('HIDE CHART');
                gotoChart('spnLink');
            }
            else if ($('#spnLink').html() == 'HIDE CHART') {
                $('#tbLiqChart').css({ 'display': 'none' });
                $('#spnLink').html('SHOW CHART');
                gotoChart('gvLiqTotalReport');
            }
        }

        function gotoChart(ctrlID) {
            $("html, body").stop().animate({
                scrollLeft: $('#' + ctrlID).offset().left,
                scrollTop: $('#' + ctrlID).offset().top
            }, 1200);
        }
    </script>
</asp:Content>
