<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="claimreview.aspx.cs" Inherits="TTS.claimreview" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .tableclaimreview
        {
            border-collapse: collapse;
            border-color: #000;
            width: 1070px;
            float: left;
        }
        .tableclaimreview th
        {
            font-weight: bold;
            background-color: #65ebeb;
            text-align: left;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="pageTitleHead" align="center">
        CLAIM REVIEW</div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <div style="padding-left: 5px;">
            <table>
                <tr>
                    <td>
                        <div style="width: 1070px; float: left; padding-top: 5px;">
                            <table cellspacing="0" rules="all" border="1" class="tableclaimreview">
                                <tr>
                                    <th>
                                        REVIEW TYPE
                                    </th>
                                    <td>
                                        <asp:RadioButtonList runat="server" ID="rdbReviewType" ClientIDMode="Static" RepeatColumns="3"
                                            RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rdbReviewType_IndexChange">
                                            <asp:ListItem Text="MONTHLY" Value="MONTHLY"></asp:ListItem>
                                            <asp:ListItem Text="QUARTERLY" Value="QUARTERLY"></asp:ListItem>
                                            <asp:ListItem Text="YEARLY" Value="YEARLY"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <th>
                                        <asp:Label runat="server" ID="lblReviewCustomer" ClientIDMode="Static" Text=""></asp:Label>
                                    </th>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlClaimCustomer" AutoPostBack="true" OnSelectedIndexChanged="ddlClaimCustomer_IndexChange">
                                        </asp:DropDownList>
                                        <asp:DropDownList runat="server" ID="ddlClaimCustUser" AutoPostBack="true" OnSelectedIndexChanged="ddlClaimCustUser_IndexChange">
                                        </asp:DropDownList>
                                        <asp:DropDownList runat="server" ID="ddlClaimPlant" AutoPostBack="true" OnSelectedIndexChanged="ddlClaimPlant_IndexChange">
                                            <asp:ListItem Text="CHOOSE" Value="CHOOSE"></asp:ListItem>
                                            <asp:ListItem Text="ALL" Value="ALL"></asp:ListItem>
                                            <asp:ListItem Text="MMN" Value="MMN"></asp:ListItem>
                                            <asp:ListItem Text="SLTL" Value="SLTL"></asp:ListItem>
                                            <asp:ListItem Text="SITL" Value="SITL"></asp:ListItem>
                                            <asp:ListItem Text="PDK" Value="PDK"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        <asp:Label runat="server" ID="lblReviewYear" ClientIDMode="Static" Text="YEAR"></asp:Label>
                                    </th>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlClaimYear" AutoPostBack="true" OnSelectedIndexChanged="ddlClaimYear_IndexChange"
                                            Width="80px">
                                        </asp:DropDownList>
                                    </td>
                                    <th>
                                        <asp:Label runat="server" ID="lblReviewOption" ClientIDMode="Static" Text="PERIOD"></asp:Label>
                                    </th>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlReviewPeriod" AutoPostBack="true" OnSelectedIndexChanged="ddlReviewPeriod_IndexChange"
                                            Width="150px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div style="width: 1070px; float: left; padding-top: 5px;">
                            <div style="width: 600px; float: left; padding-left: 10px; display: none;">
                                <span class="headCss">Chart Method :</span>
                                <asp:DropDownList runat="server" ID="ddlChartType" ClientIDMode="Static" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlChartType_IndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div style="width: 1070px; float: left; padding-top: 5px;">
                                <asp:Chart runat="server" ID="chartTyreSize" Width="1070px" Height="400px" ViewStateContent="All"
                                    BackColor="DarkCyan" BorderlineColor="Cyan" BorderlineDashStyle="Solid" BorderlineWidth="1">
                                    <Titles>
                                        <asp:Title Name="TYRESIZE" Alignment="TopCenter" Text="TYRE SIZE WISE" Font="Times New Roman, 15pt, style=Bold, Italic"
                                            ForeColor="White">
                                        </asp:Title>
                                    </Titles>
                                    <Series>
                                        <asp:Series Name="tyresize" ToolTip="#VALY">
                                        </asp:Series>
                                    </Series>
                                    <ChartAreas>
                                        <asp:ChartArea Name="ChartSize" BackColor="NavajoWhite" BackGradientStyle="LeftRight"
                                            ShadowOffset="5">
                                            <AxisX Title="SIZE" IsLabelAutoFit="True" TitleForeColor="White" TitleFont="Times New Roman, 12pt,style=Bold">
                                                <LabelStyle Angle="-90" Interval="1" />
                                            </AxisX>
                                            <AxisY Title="QTY" IsLabelAutoFit="true" TitleForeColor="White" TitleFont="Times New Roman, 12pt, style=Bold">
                                                <LabelStyle Angle="90" />
                                            </AxisY>
                                        </asp:ChartArea>
                                    </ChartAreas>
                                </asp:Chart>
                            </div>
                            <div style="width: 1070px; float: left; padding-top: 5px;">
                                <asp:Chart runat="server" ID="chartTyreType" Width="1070px" Height="400px" ViewStateContent="All"
                                    BackColor="DarkSalmon" BorderlineColor="Salmon" BorderlineDashStyle="Solid" BorderlineWidth="1">
                                    <Titles>
                                        <asp:Title Name="TYRETYPE" Alignment="TopCenter" Text="TYRE TYPE WISE" Font="Times New Roman, 15pt, style=Bold, Italic"
                                            ForeColor="White">
                                        </asp:Title>
                                    </Titles>
                                    <Series>
                                        <asp:Series Name="tyretype" ToolTip="#VALY">
                                        </asp:Series>
                                    </Series>
                                    <ChartAreas>
                                        <asp:ChartArea Name="ChartType" BackColor="NavajoWhite" BackGradientStyle="LeftRight"
                                            ShadowOffset="5">
                                            <AxisX Title="TYPE" IsLabelAutoFit="True" TitleForeColor="White" TitleFont="Times New Roman, 12pt, style=Bold">
                                                <LabelStyle Angle="-90" Interval="1" />
                                            </AxisX>
                                            <AxisY Title="QTY" IsLabelAutoFit="true" TitleForeColor="White" TitleFont="Times New Roman, 12pt, style=Bold">
                                                <LabelStyle Angle="90" />
                                            </AxisY>
                                        </asp:ChartArea>
                                    </ChartAreas>
                                </asp:Chart>
                            </div>
                            <div style="width: 1070px; float: left; padding-top: 5px;">
                                <asp:Chart runat="server" ID="chartDefect" Width="1070px" Height="400px" ViewStateContent="All"
                                    BackColor="DarkViolet" BorderlineColor="Violet" BorderlineDashStyle="Solid" BorderlineWidth="1">
                                    <Titles>
                                        <asp:Title Name="TYREDEFECT" Alignment="TopCenter" Text="TYRE DEFECT WISE" Font="Times New Roman, 15pt, style=Bold, Italic"
                                            ForeColor="White">
                                        </asp:Title>
                                    </Titles>
                                    <Series>
                                        <asp:Series Name="ClaimDescription" ToolTip="#VALY" Font="Times New Roman, 12pt, style=Bold">
                                        </asp:Series>
                                    </Series>
                                    <ChartAreas>
                                        <asp:ChartArea Name="ChartDefect" BackColor="NavajoWhite" BackGradientStyle="LeftRight"
                                            ShadowOffset="5">
                                            <AxisX Title="TYRE DEFECT" IsLabelAutoFit="True" TitleForeColor="White" TitleFont="Times New Roman, 12pt, style=Bold">
                                                <LabelStyle Angle="-90" Interval="1" />
                                            </AxisX>
                                            <AxisY Title="QTY" IsLabelAutoFit="true" TitleForeColor="White" TitleFont="Times New Roman, 12pt, style=Bold">
                                                <LabelStyle Angle="90" />
                                            </AxisY>
                                        </asp:ChartArea>
                                    </ChartAreas>
                                </asp:Chart>
                            </div>
                            <div style="width: 1070px; float: left; padding-top: 5px;">
                                <asp:Chart runat="server" ID="chartMonthQty" Width="1070px" Height="400px" ViewStateContent="All"
                                    BackColor="DarkMagenta" BorderlineColor="Magenta" BorderlineDashStyle="Solid"
                                    BorderlineWidth="1">
                                    <Titles>
                                        <asp:Title Name="MONTHWISE" Alignment="TopCenter" Text="MONTHLY QTY WISE" Font="Times New Roman, 15pt, style=Bold, Italic"
                                            ForeColor="White">
                                        </asp:Title>
                                    </Titles>
                                    <Series>
                                        <asp:Series Name="ClaimMonth" ToolTip="#VALY" Font="Times New Roman, 12pt, style=Bold">
                                        </asp:Series>
                                    </Series>
                                    <ChartAreas>
                                        <asp:ChartArea Name="ChartMonthQty" BackColor="NavajoWhite" BackGradientStyle="LeftRight"
                                            ShadowOffset="5">
                                            <AxisX Title="MONTH" IsLabelAutoFit="True" TitleForeColor="White" TitleFont="Times New Roman, 12pt, style=Bold">
                                                <LabelStyle Angle="-90" Interval="1" />
                                            </AxisX>
                                            <AxisY Title="QTY" IsLabelAutoFit="true" TitleForeColor="White" TitleFont="Times New Roman, 12pt, style=Bold">
                                                <LabelStyle Angle="90" />
                                            </AxisY>
                                        </asp:ChartArea>
                                    </ChartAreas>
                                </asp:Chart>
                            </div>
                            <div style="width: 1070px; float: left; padding-top: 5px;">
                                <asp:Chart runat="server" ID="chartClassification" Width="1070px" Height="400px"
                                    ViewStateContent="All" BackColor="DarkOliveGreen" BorderlineColor="Olive" BorderlineDashStyle="Solid"
                                    BorderlineWidth="1">
                                    <Titles>
                                        <asp:Title Name="CLASSIFICATIONWISE" Alignment="TopCenter" Text="CLASSIFICATION WISE"
                                            Font="Times New Roman, 15pt, style=Bold, Italic" ForeColor="White">
                                        </asp:Title>
                                    </Titles>
                                    <Series>
                                        <asp:Series Name="Classification" ToolTip="#VALY" Font="Times New Roman, 12pt, style=Bold"
                                            ChartType="Pie">
                                        </asp:Series>
                                    </Series>
                                    <ChartAreas>
                                        <asp:ChartArea Name="ChartClassify" BackColor="NavajoWhite" BackGradientStyle="LeftRight"
                                            ShadowOffset="5">
                                            <AxisX Title="MONTH" IsLabelAutoFit="True" TitleForeColor="White" TitleFont="Times New Roman, 12pt, style=Bold">
                                                <LabelStyle Angle="-90" Interval="1" />
                                            </AxisX>
                                            <AxisY Title="QTY" IsLabelAutoFit="true" TitleForeColor="White" TitleFont="Times New Roman, 12pt, style=Bold">
                                                <LabelStyle Angle="90" />
                                            </AxisY>
                                        </asp:ChartArea>
                                    </ChartAreas>
                                    <Legends>
                                        <asp:Legend Name="Classification" BackColor="White" Docking="Right">
                                        </asp:Legend>
                                    </Legends>
                                </asp:Chart>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdnClaimReviewType" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        $(document).ready(function () {
            $('#MainContent_ddlClaimCustomer').change(function () {
                if ($("input:radio[id*='rdbReviewType_']:checked").length == 0) {
                    alert('Choose review type');
                    return false;
                }
            });
            $('#MainContent_ddlClaimCustUser').change(function () {
                if ($("input:radio[id*='rdbReviewType_']:checked").length == 0) {
                    alert('Choose review type');
                    return false;
                }
            });
            $('#MainContent_ddlClaimPlant').change(function () {
                if ($("input:radio[id*='rdbReviewType_']:checked").length == 0) {
                    alert('Choose review type');
                    return false;
                }
            });
            $('#MainContent_ddlClaimYear').change(function () {
                if ($("input:radio[id*='rdbReviewType_']:checked").length == 0) {
                    alert('Choose review type');
                    return false;
                }
            });
            $('#MainContent_ddlReviewPeriod').change(function () {
                if ($("input:radio[id*='rdbReviewType_']:checked").length == 0) {
                    alert('Choose review type');
                    return false;
                }
            });
        });
    </script>
</asp:Content>
