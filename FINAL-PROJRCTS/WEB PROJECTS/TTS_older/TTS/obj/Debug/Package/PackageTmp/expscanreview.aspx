<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="expscanreview.aspx.cs" Inherits="TTS.expscanreview" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
    <link href="Styles/pdistyle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .tableCss
        {
            width: 100%;
            background-color: #dcecfb;
            border-color: White;
            border-collapse: separate;
        }
        .tableCss th
        {
            font-weight: normal;
            text-align: center;
        }
        .tableCss td
        {
            font-weight: bold;
            text-align: left;
        }
    </style>
    <script src="Scripts/cotsScript.js" type="text/javascript"></script>
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
        <table cellspacing="0" rules="all" border="1" class="tableCss" style="width: 100%;">
            <tr>
                <th>
                    Plant
                </th>
                <td>
                    <asp:DropDownList ID="ddlplant" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="ddlplant_SelectedIndexChanged"
                        AutoPostBack="true" CssClass="form-control" Width="120px">
                    </asp:DropDownList>
                </td>
                <th>
                    Year
                </th>
                <td>
                    <asp:DropDownList ID="ddlYear" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged"
                        AutoPostBack="true" CssClass="form-control" Width="120px">
                    </asp:DropDownList>
                </td>
                <th>
                    Month
                </th>
                <td>
                    <asp:DropDownList ID="ddlMonth" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged"
                        AutoPostBack="true" CssClass="form-control" Width="120px">
                    </asp:DropDownList>
                </td>
                <th>
                    Total Dispatched List
                </th>
                <td>
                    <asp:Label runat="server" ID="lblOrderCount" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="8">
                    <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" Font-Bold="true"
                        ForeColor="Red"></asp:Label>
                    <asp:GridView runat="server" ID="gvDispatchedPdiList" AutoGenerateColumns="false"
                        Width="100%" CssClass="gridcss" AllowPaging="true" OnPageIndexChanging="Pdiscantracklist_PageIndex"
                        PageSize="50" PagerStyle-Height="30px" PagerStyle-Font-Bold="true" PagerStyle-Font-Size="15px"
                        PagerStyle-HorizontalAlign="Center" PagerStyle-VerticalAlign="Middle" RowStyle-Height="35px">
                        <Columns>
                            <asp:TemplateField HeaderText="CUSTOMER NAME" ItemStyle-Width="250px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatusCustName" Text='<%# Eval("custfullname") %>'></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdnStatusCustCode" Value='<%# Eval("CustCode") %>' />
                                    <asp:HiddenField runat="server" ID="hdnSNo" Value='<%#Eval("ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ORDER REF NO." ItemStyle-Width="200px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOrderRefNo" Text='<%#Eval("OrderRefNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="WORK ORDER" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblWorkorderno" Text='<%# Eval("workorderno") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="INSPECTED DATE" DataField="createddate" ItemStyle-Width="70px" />
                            <asp:BoundField HeaderText="APPROVED DATE" DataField="completedon" ItemStyle-Width="70px" />
                            <asp:BoundField HeaderText="LOADED DATE" DataField="loadedon" ItemStyle-Width="80px" />
                            <asp:BoundField HeaderText="QTY" DataField="orderqty" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField HeaderText="PLANT" DataField="pdiplant" ItemStyle-Width="40px" />
                            <asp:TemplateField HeaderText="ACTION" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="110px">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkShowDetails" runat="server" Text="Show Details" CssClass="btn btn-info"
                                        OnClick="lnkShowDetails_Click" /></span>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="8">
                    <div style="width: 100%; display: none; padding-top: 5px;" id="divStatusChange">
                        <table cellspacing="0" rules="all" style="width: 100%; border-color: White; border-collapse: separate;"
                            border="1">
                            <tr style="width: 100%; background-color: #cecece; font-size: 18px;">
                                <td>
                                    <asp:Label runat="server" ID="lblCustName" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblStausOrderRefNo" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblWorkorderno" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:DataList runat="server" ID="dlOrderMaster" RepeatColumns="1" RepeatDirection="Horizontal"
                                        RepeatLayout="Table" AlternatingItemStyle-BackColor="#A1C9D3" ItemStyle-BackColor="#c6dcef"
                                        ItemStyle-VerticalAlign="Top" Width="100%">
                                        <ItemTemplate>
                                            <table cellspacing="0" rules="all" style="width: 100%; border-color: White; border-collapse: separate;"
                                                border="1">
                                                <tr>
                                                    <th>
                                                        ORDER QUANTITY
                                                    </th>
                                                    <td colspan="7">
                                                        <asp:Label runat="server" ID="lblOrderQty" Text='<%# Eval("orderqty")%>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        INSPECTED BY
                                                    </th>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblInspectedBy" Text='<%# Eval("inspectedby")%>'></asp:Label>
                                                    </td>
                                                    <th>
                                                        INSPECTED DATE
                                                    </th>
                                                    <td>
                                                        <%# Eval("inspectdate")%>
                                                    </td>
                                                    <th>
                                                        APPROVED DATE
                                                    </th>
                                                    <td>
                                                        <%# Eval("approvedon")%>
                                                    </td>
                                                    <th>
                                                        APPROVED BY
                                                    </th>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblApprovedBy" Text='<%# Eval("approvedby")%>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        LOADED DATE
                                                    </th>
                                                    <td>
                                                        <%# Eval("loadedon")%>
                                                    </td>
                                                    <th>
                                                        LOADED BY
                                                    </th>
                                                    <td>
                                                        <%# Eval("loadedby")%>
                                                    </td>
                                                    <th>
                                                        CONTAINER NO
                                                    </th>
                                                    <td>
                                                        <%# Eval("containerno")%>
                                                    </td>
                                                    <th>
                                                        VEHICLE NO
                                                    </th>
                                                    <td>
                                                        <%# Eval("vehicleno")%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        REMARKS
                                                    </th>
                                                    <td colspan="7">
                                                        <%# Eval("loadedremarks").ToString().Replace("~","<br/>") %>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </asp:DataList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:LinkButton runat="server" ID="btnExportXls" ClientIDMode="Static" CssClass=""
                                        Text="Export To Excel File" OnClick="btnExportXls_Click"></asp:LinkButton>
                                    <div style="display: none;">
                                        <asp:Button runat="server" ID="btnPdiPdfGenerate" ClientIDMode="Static" Text="PDF GENERATE"
                                            OnClick="btnPdiPdfGenerate_Click" />
                                    </div>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblDownload" ClientIDMode="Static" Text="DOWNLOAD PDI REPORT"
                                        Font-Bold="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:LinkButton runat="server" ID="lnkPDIFileDownload" ClientIDMode="Static" OnClick="lnkPDIFileDownload_Click"></asp:LinkButton>
                                    <br />
                                    <asp:LinkButton runat="server" ID="lnkPDIFile" ClientIDMode="Static" OnClick="lnkPDIFileDownload_Click"></asp:LinkButton>
                                    <br />
                                    <asp:LinkButton runat="server" ID="lnkFinlaInpsect" ClientIDMode="Static" OnClick="lnkPDIFileDownload_Click"></asp:LinkButton>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <div style="width: 100%;">
                                        <asp:Button Text="BARCODE WISE" BorderStyle="None" ID="Button2" CssClass="Initial"
                                            runat="server" OnClick="Tab_Click" />
                                        <asp:Button Text="ITEM QTY WISE" BorderStyle="None" ID="Button1" CssClass="Initial"
                                            runat="server" OnClick="Tab_Click" />
                                        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                                            <asp:View ID="Tab1" runat="server">
                                                <asp:GridView runat="server" ID="gvScannedItemWise" Width="100%" ClientIDMode="Static"
                                                    ViewStateMode="Enabled" CssClass="gridcss">
                                                </asp:GridView>
                                            </asp:View>
                                            <asp:View ID="Tab2" runat="server">
                                                <table cellspacing="0" id="div_ddlSelect" rules="all" border="1" style="border-collapse: collapse;
                                                    border-color: #CE8686; width: 100%;">
                                                    <tr align="center" class="headCss" style="background-color: #EBEEED;">
                                                        <td class="tdhide">
                                                            PLATFORM
                                                        </td>
                                                        <td class="tdhide">
                                                            TYRE SIZE
                                                        </td>
                                                        <td class="tdhide">
                                                            RIM SIZE
                                                        </td>
                                                        <td>
                                                            TYRE TYPE
                                                        </td>
                                                        <td>
                                                            BRAND
                                                        </td>
                                                        <td>
                                                            SIDEWALL
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 200px">
                                                            <asp:DropDownList ID="ddlPlatform" runat="server" ClientIDMode="Static" AutoPostBack="true"
                                                                CssClass="form-control" Width="200px" OnSelectedIndexChanged="ddlPlatform_IndexChange">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="width: 170px">
                                                            <asp:DropDownList ID="ddlTyreSize" runat="server" ClientIDMode="Static" AutoPostBack="true"
                                                                CssClass="form-control" Width="170px" OnSelectedIndexChanged="ddlTyreSize_IndexChange">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="width: 210px">
                                                            <asp:DropDownList ID="ddlRimSize" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="ddlRimSize_IndexChange"
                                                                AutoPostBack="true" CssClass="form-control" Width="210px">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="width: 150px">
                                                            <asp:DropDownList ID="ddlTyretype" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="ddlTyretype_IndexChange"
                                                                AutoPostBack="true" CssClass="form-control" Width="150px">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="width: 150px">
                                                            <asp:DropDownList ID="ddlBrand" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="ddlBrand_IndexChange"
                                                                AutoPostBack="true" CssClass="form-control" Width="150px">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="width: 170px">
                                                            <asp:DropDownList ID="ddl_Sidewall" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="ddl_Sidewall_IndexChange"
                                                                AutoPostBack="true" CssClass="form-control" Width="170px">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <hr />
                                                <asp:GridView runat="server" ID="gvScanPdiLsit" ClientIDMode="Static" AutoGenerateColumns="true"
                                                    Width="100%" CssClass="gridcss">
                                                </asp:GridView>
                                            </asp:View>
                                        </asp:MultiView>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnCustCode" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnID" ClientIDMode="Static" Value="" />
</asp:Content>
