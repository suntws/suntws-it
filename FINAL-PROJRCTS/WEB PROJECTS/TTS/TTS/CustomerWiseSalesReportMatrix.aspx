<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerWiseSalesReportMatrix.aspx.cs"
    Inherits="TTS.CustomerWiseSalesReportMatrix" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="styles/GridviewScroll.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .pageTitleHead
        {
            color: rgb(53, 86, 129);
            font-size: 20px;
            height: 20px;
            width: 100%;
            text-transform: uppercase;
            font-style: italic;
            background-color: #f3e0ad;
            font-weight: bold;
            text-align: left;
        }
        .gvHeading
        {
            text-transform: uppercase;
            font-weight: bold;
            background-color: #afaeb1;
        }
        .align-right
        {
            text-align: right;
        }
        .hide
        {
            display: none;
        }
        form
        {
            font: 14px verdana;
        }
        #gvStockDetails
        {
            overflow: scroll;
            font-weight: bold;
        }
    </style>
    <script src="scripts/jquerymin182.js" type="text/javascript"></script>
    <script src="scripts/jqueryui191.js" type="text/javascript"></script>
    <script src="scripts/gridviewScroll.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="Form1" runat="server">
    <div style="width: auto; font-size: 12px;">
        <div class="pageTitleHead">
            STOCK COUNT MATRIX REVIEW
        </div>
        <div>
            <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
                Font-Size="20px" ForeColor="Red"></asp:Label>
        </div>
        <div id="displaycontent">
            <h2>
                <asp:Label runat="server" ID="lblErr" ClientIDMode="Static" Text="" Font-Bold="true"
                    ForeColor="Red"></asp:Label>
            </h2>
            <div style="border: 1px solid #000;">
                <asp:LinkButton runat="server" ID="lnkSalesExport" ClientIDMode="Static" Text=""
                    Font-Bold="true" Font-Size="14px" OnClick="lnkSalesExport_Click"></asp:LinkButton>
                <asp:GridView ID="gvSalesExportData" ClientIDMode="Static" runat="server" OnRowDataBound="gvSalesExportData_RowDataBound"
                    OnDataBound="gvSalesExportData_DataBound" OnInit="gvsalesExportData_PreRender">
                </asp:GridView>
            </div>
            <div style="border: 1px solid #000;">
                <asp:LinkButton runat="server" ID="lnkStockExport" ClientIDMode="Static" Text=""
                    Font-Bold="true" Font-Size="14px" OnClick="lnkStockExport_Click"></asp:LinkButton>
                <asp:GridView ID="gvStockExportData" ClientIDMode="Static" runat="server" OnRowDataBound="gvStockExportData_RowDataBound"
                    OnInit="gvStockExportData_PreRender">
                </asp:GridView>
            </div>
        </div>
        <asp:HiddenField runat="server" ID="hdnYear" ClientIDMode="Static" Value="" />
        <asp:HiddenField runat="server" ID="hdngrade" ClientIDMode="Static" Value="" />
        <asp:HiddenField runat="server" ID="hdncustname" ClientIDMode="Static" Value="" />
        <asp:HiddenField runat="server" ID="hdnCategory" ClientIDMode="Static" Value="" />
    </div>
    <script type="text/javascript">
        function gvSalesModel() {
            $("#gvSalesExportData").find("tr").each(function (i) {
                if (i <= 2) {
                    $("#gvSalesExportData").find("tr").eq(i).find("td").eq(1).addClass("gvHeading");
                    $("#gvSalesExportData").find("tr").eq(i).find("td").eq(0).addClass("gvHeading");
                }
            });
        }

        function gvStockModel() {
            $("#gvStockExportData").find("tr").each(function (i) {
                if (i <= 2) {
                    $("#gvStockExportData").find("tr").eq(i).find("td").eq(1).addClass("gvHeading");
                    $("#gvStockExportData").find("tr").eq(i).find("td").eq(0).addClass("gvHeading");
                }
            });
        }

        $(document).ready(function () {
            $('#gvSalesExportData').gridviewScroll({ width: window.innerWidth - 40, height: window.innerHeight - 40, freezesize: 1, headerrowcount: 3 });
            $('#gvStockExportData').gridviewScroll({ width: window.innerWidth - 40, height: window.innerHeight - 40, freezesize: 2, headerrowcount: 3 });
        });
    </script>
    </form>
</body>
</html>
