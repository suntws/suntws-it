<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="cotsRimStockReport.aspx.cs" Inherits="TTS.cotsRimReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        table
        {
            background-color: #E4F7CF !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        <asp:Label runat="server" ID="lblPageTitle" ClientIDMode="Static" Text=" DOMESTIC DEBTORS RECEIPTS "></asp:Label>
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="background-color: #ffffff; width: 100%;
            border-color: White; border-collapse: separate;" id="tbDebtorsPage">
            <tr>
                <th>
                    <asp:Label ID="lbl_Rimsize" runat="server" ClientIDMode="Static" Text="RIM SIZE"
                        Font-Bold="true"></asp:Label>
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="cmb_RimSize" ClientIDMode="Static" OnSelectedIndexChanged="cmb_RimSize_SelectedIndexChanged"
                        AutoPostBack="true" CssClass="form-control" Width="120px">
                    </asp:DropDownList>
                </td>
                <th>
                    <asp:Label ID="lbl_EDCNumber" runat="server" ClientIDMode="Static" Text="EDC NO"
                        Font-Bold="true"></asp:Label>
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="cmb_EdcNumber" ClientIDMode="Static" OnSelectedIndexChanged="cmb_EdcNumber_SelectedIndexChanged"
                        AutoPostBack="true" CssClass="form-control" Width="120px">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <div>
            <asp:GridView runat="server" ID="dg_StockDetails" AutoGenerateColumns="false" Width="100%"
                CssClass="gridcss" AllowPaging="true" PageSize="50" PagerStyle-Height="30px"
                PagerStyle-Font-Bold="true" PagerStyle-Font-Size="15px" PagerStyle-HorizontalAlign="Center"
                PagerStyle-VerticalAlign="Middle" RowStyle-Height="35px">
                <Columns>
                <asp:BoundField HeaderText="SUPPLIER" DataField="SupplierName" />
                    <asp:BoundField HeaderText="EDC NO" DataField="EDCNo" />
                    <asp:BoundField HeaderText="PO NO/QTY/DATE" DataField="PO" />
                    <asp:BoundField HeaderText="INVOICE NO/DATE" DataField="Invoice" />
                    <asp:BoundField HeaderText="RECEIVED QTY/DATE/BY" DataField="Received" />
                    <asp:BoundField HeaderText="STOCK QTY" DataField="StockQty" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
