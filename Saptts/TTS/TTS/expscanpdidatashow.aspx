<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="expscanpdidatashow.aspx.cs"
    Inherits="TTS.expscanpdidatashow" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .Initial
        {
            display: block;
            padding: 4px 18px 4px 18px;
            float: left;
            color: #000;
            font-weight: bold;
            margin-right: 10px;
        }
        .Initial:hover
        {
            color: #5f94f7;
            cursor: pointer;
        }
        .Clicked
        {
            float: left;
            display: block;
            padding: 4px 18px 4px 18px;
            font-weight: bold;
            color: #fff;
            margin-right: 10px;
            background-color: #099690;
        }
    </style>
    <script src="Scripts/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function closePopup() {
            window.parent.TINY.box.hide();
        }

        function closePopupOnly() {
            window.parent.TINY.box.hide();
        }

        function Bind_AssignList() {
            $('#gvAssignQty tr').find('td:eq(12)').each(function (e) {
                if ($(this).text().trim() == "INSPECTED")
                    $(this).css({ 'color': '#2b67f1' });
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="font-size: 12px;">
        <asp:ScriptManager runat="server" ID="scriptManager1">
        </asp:ScriptManager>
        <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #fff;
            width: 1065px;">
            <tr>
                <td>
                    <div style="width: 1045px; float: left; background-color: #a7dcda; font-size: 15px;">
                        <div style="width: 500px; float: left; text-align: left;">
                            <asp:Label runat="server" ID="lblCustName" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                        </div>
                        <div style="width: 500px; float: left; text-align: right;">
                            <asp:Label runat="server" ID="lblOrderRefNo" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label></div>
                    </div>
                    <div style="width: 15px; float: right; padding-left: 5px;">
                        <img src="images/cancel.png" alt="CLOSE" onclick="closePopupOnly();" style="width: 15px;
                            cursor: pointer;" />
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblNoOfRecords" ClientIDMode="Static" Text="" Font-Bold="true"
                        Font-Italic="true" Font-Size="22px" ForeColor="#543fca"></asp:Label>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:Button Text="BARCODE WISE" BorderStyle="None" ID="Button1" CssClass="Initial"
                                runat="server" OnClick="Tab_Click" />
                            <asp:Button Text="ITEM QTY WISE" BorderStyle="None" ID="Button2" CssClass="Initial"
                                runat="server" OnClick="Tab_Click" />
                            <asp:Button Text="ASSIGN QTY WISE" BorderStyle="None" ID="Button3" CssClass="Initial"
                                runat="server" OnClick="Tab_Click" />
                            <asp:Label runat="server" ID="lbl_plant" ClientIDMode="Static" Text="" Font-Bold="true"
                                Visible="false" Font-Size="14px" ForeColor="#108782" Style="float: right;"></asp:Label>
                            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                                <asp:View ID="Tab1" runat="server">
                                    <table cellspacing="0" id="div_ddlSelect" rules="all" border="1" style="border-collapse: collapse;
                                        border-color: #CE8686; width: 100%;">
                                        <tr align="center" class="headCss" style="background-color: #EBEEED;">
                                            <td>
                                                PLATFORM
                                            </td>
                                            <td>
                                                TYRE SIZE
                                            </td>
                                            <td>
                                                RIM SIZE
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddlPlatform" runat="server" ClientIDMode="Static" AutoPostBack="true"
                                                    CssClass="form-control" OnSelectedIndexChanged="ddlPlatform_IndexChange">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlTyreSize" runat="server" ClientIDMode="Static" AutoPostBack="true"
                                                    CssClass="form-control" OnSelectedIndexChanged="ddlTyreSize_IndexChange">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlRimSize" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="ddlRimSize_IndexChange"
                                                    AutoPostBack="true" CssClass="form-control">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
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
                                            <td>
                                                <asp:DropDownList ID="ddlTyretype" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="ddlTyretype_IndexChange"
                                                    AutoPostBack="true" CssClass="form-control">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlBrand" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="ddlBrand_IndexChange"
                                                    AutoPostBack="true" CssClass="form-control">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_Sidewall" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="ddl_Sidewall_IndexChange"
                                                    AutoPostBack="true" CssClass="form-control">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                    <hr />
                                    <asp:GridView runat="server" ID="gvScanBarcode" Width="1065px" HeaderStyle-Font-Size="12px"
                                        HeaderStyle-BackColor="#eaf1f9" ClientIDMode="Static" ViewStateMode="Enabled"
                                        AllowPaging="true" OnPageIndexChanging="gvScanBarcode_PageIndex" PageSize="100"
                                        PagerStyle-Height="30px" PagerStyle-Font-Bold="true" PagerStyle-Font-Size="15px"
                                        PagerStyle-HorizontalAlign="Center" PagerStyle-VerticalAlign="Middle">
                                        <PagerSettings Mode="Numeric" Position="Bottom" />
                                    </asp:GridView>
                                </asp:View>
                                <asp:View ID="Tab2" runat="server">
                                    <asp:GridView runat="server" ID="gvScannedItemWise" Width="1065px" HeaderStyle-Font-Size="12px"
                                        HeaderStyle-BackColor="#eaf1f9" ClientIDMode="Static" ViewStateMode="Enabled">
                                    </asp:GridView>
                                </asp:View>
                                <asp:View ID="Tab3" runat="server">
                                    <asp:GridView runat="server" ID="gvAssignQty" Width="1065px" HeaderStyle-Font-Size="12px"
                                        HeaderStyle-BackColor="#eaf1f9" ClientIDMode="Static" ViewStateMode="Enabled">
                                    </asp:GridView>
                                </asp:View>
                            </asp:MultiView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
