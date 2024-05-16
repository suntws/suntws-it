<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StockMatrixReview.aspx.cs"
    Inherits="TTS.StockMatrixReview" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
        #lnkExport
        {
            float: right;
            font-size: 14px;
            width: 80px;
            height: 42px;
        }
        .xlsLink
        {
            color: #030BD1;
            font-weight: bold;
            cursor: pointer;
            text-decoration: none;
            background: url(images/imagexls.png) left center no-repeat;
            padding-left: 20px;
        }
    </style>
    <script src="scripts/jquerymin182.js" type="text/javascript"></script>
    <script src="scripts/jqueryui191.js" type="text/javascript"></script>
    <script src="scripts/gridviewScroll.min.js" type="text/javascript"></script>
</head>
<body>
    <form runat="server">
    <div style="width: auto; font-size: 12px;">
        <div class="pageTitleHead">
            MATRIX FORMAT STOCK COUNT
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
            <div>
                <asp:LinkButton runat="server" ID="lnkExport" ClientIDMode="Static" CssClass="xlsLink"
                    Text="" OnClick="btnStockXls_Click"></asp:LinkButton>
                <asp:GridView ID="gvStockDetails" ClientIDMode="Static" runat="server" OnRowDataBound="gvStockDetails_RowDataBound"
                    OnInit="gvStockDetails_PreRender">
                </asp:GridView>
            </div>
        </div>
        <asp:HiddenField runat="server" ID="hdnConfig" ClientIDMode="Static" Value="" />
        <asp:HiddenField runat="server" ID="hdnPlant" ClientIDMode="Static" Value="" />
        <asp:HiddenField runat="server" ID="hdnBrand" ClientIDMode="Static" Value="" />
        <asp:HiddenField runat="server" ID="hdnSidewall" ClientIDMode="Static" Value="" />
        <asp:HiddenField runat="server" ID="hdnGrade" ClientIDMode="Static" Value="" />
    </div>
    <script type="text/javascript">
        function gvModel() {
            $("#gvStockDetails").find("tr").each(function (i) {
                if (i <= 4) {
                    $("#gvStockDetails").find("tr").eq(i).find("td").eq(1).addClass("gvHeading");
                    $("#gvStockDetails").find("tr").eq(i).find("td").eq(0).addClass("gvHeading");
                }
            });
        }

        $(document).ready(function () {
            $('#gvStockDetails').gridviewScroll({ width: window.innerWidth - 20, height: window.innerHeight - 40, freezesize: 2, headerrowcount: 4 });
        });
    </script>
    </form>
</body>
</html>
