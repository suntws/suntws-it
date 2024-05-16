<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="stockinward.aspx.cs" Inherits="TTS.stockinward" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/cotsScript.js" type="text/javascript"></script>
    <style type="text/css">
        #lblMsg
        {
            line-height: 30px;
            background-color: #8DDEA6;
            font-weight: bold;
            width: 100%;
            float: left;
        }
        .Initial
        {
            display: block;
            padding: 4px 18px 4px 18px;
            float: left;
            color: #000;
            font-weight: bold;
            margin-right: 10px;
            border: 1px solid transparent;
            border-radius: 4px;
        }
        .Initial:hover
        {
            color: #5f94f7;
            border: 1px solid #000000;
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
            background-color: #074845;
            border: 1px solid transparent;
            border-radius: 4px;
        }
        input[type="file"]
        {
            text-align-last: end;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="360000">
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
                    <asp:FileUpload ID="fupStockInward" runat="server" ClientIDMode="Static" accept=".xls,.xlsx,.csv,.txt"
                        CssClass="btn btn-warning" Width="95%" onchange="fupStockInward_change()" />
                </td>
                <td>
                    <asp:Button runat="server" ID="btnBarcodeVerify" ClientIDMode="Static" Text="VERIFY"
                        OnClick="btnBarcodeVerify_Click" CssClass="btn btn-info" />
                </td>
                <td>
                    <asp:Button runat="server" ID="btnInwardUploadCancel" Text="CLEAR" OnClick="btnInwardUploadCancel_Click"
                        CssClass="btn btn-danger" />
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Label runat="server" ID="lblMsg" ClientIDMode="Static" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Button runat="server" ID="btnInwardFileSave" Text="SAVE INWARD BARCODE DATA"
                        OnClick="btnInwardFileSave_Click" CssClass="btn btn-success" />
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <table id="tb_stock_inward" runat="server" cellspacing="0" rules="all" border="1"
                        style="background-color: #dcecfb; width: 100%; border-color: White; border-collapse: separate;
                        display: none;">
                        <tr>
                            <td style="width: 100%;">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:Button Text="BARCODE ITEM QTY" BorderStyle="None" ID="Button1" CssClass="Initial"
                                            runat="server" OnClick="Tab_Click" />
                                        <asp:Button Text="BARCODE DATA" BorderStyle="None" ID="Button2" CssClass="Initial"
                                            runat="server" OnClick="Tab_Click" />
                                        <asp:Button Text="INVALID BARCODE" BorderStyle="None" ID="Button3" CssClass="Initial"
                                            runat="server" OnClick="Tab_Click" />
                                        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                                            <asp:View ID="Tab1" runat="server">
                                                <asp:GridView runat="server" ID="gvItemCount" AutoGenerateColumns="false" Width="100%"
                                                    AlternatingRowStyle-BackColor="#f5f5f5" CssClass="gridcss">
                                                    <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" Height="22px" />
                                                    <Columns>
                                                        <asp:BoundField HeaderText="PROCESS-ID" DataField="PROCESSID" />
                                                        <asp:BoundField HeaderText="PLATFORM" DataField="PLATFORM" />
                                                        <asp:BoundField HeaderText="TYRE SIZE" DataField="TYRESIZE" />
                                                        <asp:BoundField HeaderText="RIM" DataField="RIMWIDTH" />
                                                        <asp:BoundField HeaderText="TYPE" DataField="TYRETYPE" />
                                                        <asp:BoundField HeaderText="BRAND" DataField="BRAND" />
                                                        <asp:BoundField HeaderText="SIDEWALL" DataField="SIDEWALL" />
                                                        <asp:BoundField HeaderText="INWARD QTY" DataField="INWARDQTY" ItemStyle-HorizontalAlign="Right" />
                                                        <asp:BoundField HeaderText="FWT" DataField="FWT" ItemStyle-HorizontalAlign="Right" />
                                                        <asp:TemplateField HeaderText="TOTAL FWT" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <%# (Convert.ToInt32(Eval("INWARDQTY").ToString())* Convert.ToDecimal(Eval("FWT").ToString())) %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </asp:View>
                                            <asp:View ID="Tab2" runat="server">
                                                <asp:GridView runat="server" ID="gvItemList" AutoGenerateColumns="false" Width="100%"
                                                    AlternatingRowStyle-BackColor="#f5f5f5" CssClass="gridcss">
                                                    <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" Height="22px" />
                                                    <Columns>
                                                        <asp:BoundField HeaderText="BARCODE" DataField="BARCODE" />
                                                        <asp:BoundField HeaderText="PLATFORM" DataField="PLATFORM" />
                                                        <asp:BoundField HeaderText="TYRE SIZE" DataField="TYRESIZE" />
                                                        <asp:BoundField HeaderText="RIM" DataField="RIMWIDTH" />
                                                        <asp:BoundField HeaderText="TYPE" DataField="TYRETYPE" />
                                                        <asp:BoundField HeaderText="BRAND" DataField="BRAND" />
                                                        <asp:BoundField HeaderText="SIDEWALL" DataField="SIDEWALL" />
                                                        <asp:BoundField HeaderText="FWT" DataField="FWT" ItemStyle-HorizontalAlign="Right" />
                                                    </Columns>
                                                </asp:GridView>
                                            </asp:View>
                                            <asp:View ID="Tab3" runat="server">
                                                <asp:GridView runat="server" ID="gvNotMatchedList" AutoGenerateColumns="false" Width="100%"
                                                    CssClass="gridcss">
                                                    <HeaderStyle BackColor="#f3e0ad" Font-Bold="true" Height="22px" />
                                                    <Columns>
                                                        <asp:BoundField HeaderText="INVALID BARCODE" DataField="NonProcessIDBarcode" />
                                                    </Columns>
                                                </asp:GridView>
                                            </asp:View>
                                        </asp:MultiView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <asp:HiddenField runat="server" ID="hdnPlant" ClientIDMode="Static" Value="" />
    </div>
    <script type="text/javascript">
        function fupStockInward_change() {
            $('#MainContent_tb_stock_inward').css('display', 'none');
            $('#btnBarcodeVerify').click();
        }
    </script>
</asp:Content>
