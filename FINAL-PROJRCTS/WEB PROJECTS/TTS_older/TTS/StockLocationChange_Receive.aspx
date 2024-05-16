<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="StockLocationChange_Receive.aspx.cs" Inherits="TTS.StockLocationChange_Receive" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/pdistyle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .tableCss1
        {
            width: 100%;
            background-color: #dcecfb;
            border-color: White;
            border-collapse: separate;
        }
        .tableCss1 th
        {
            width: 150px;
            font-weight: bold;
            text-align: center;
            background-color: #6ef0ee;
            color: #db4d67;
        }
        .tableCss1 td
        {
            font-size: 12px;
            text-align: left;
            padding-left: 4px;
        }
        .gridcss1
        {
            width: 100%;
            background-color: #fff;
            margin: : 5px 0 5px 0;
            border: solid 1px #525252;
            border-collapse: collapse;
        }
        .gridcss1 td
        {
            padding: 5px;
            border: solid 1px #c1c1c1;
            white-space: nowrap;
        }
        .gridcss1 th
        {
            padding: 4px 2px;
            color: #ffffff;
            background: #1762a5;
            border-left: solid 1px #525252;
            font-size: 0.9em;
        }
        .gridcss1 tr:hover
        {
            background-color: #4293da;
            font-weight: bold;
            color: White;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager runat="server" ID="scriptManager1">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        <asp:Label runat="server" ID="lblPageTitle" ClientIDMode="Static"></asp:Label>
    </div>
    <div style="text-align: center;">
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="background-color: #dcecfb; width: 100%;
            border-color: White; border-collapse: separate;">
            <tr>
                <td style="padding-left: 0px;">
                    <asp:GridView runat="server" ID="gvLocatChangeOrders" AutoGenerateColumns="false"
                        Width="100%" HeaderStyle-BackColor="#FFCC00" RowStyle-Height="22px">
                        <Columns>
                            <asp:BoundField HeaderText="ORDER ID" DataField="ID" />
                            <asp:BoundField HeaderText="DC NUMBER" DataField="dcNo" />
                            <asp:BoundField HeaderText="FROM" DataField="fromPlace" />
                            <asp:BoundField HeaderText="SENT DATE" DataField="DCcreatedOn" />
                            <asp:BoundField HeaderText="SENT BY" DataField="DCcreatedBy" />
                            <asp:BoundField HeaderText="QTY" DataField="totQty" ItemStyle-HorizontalAlign="Right" />
                            <asp:TemplateField HeaderText="ACTION" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hdnMaster_Id" Value='<%# Eval("ID") %>' />
                                    <asp:LinkButton runat="server" ID="lnkOrderView" OnClick="lnkOrderView_Click" Text="VIEW"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td style="padding-left: 0px;">
                    <asp:GridView runat="server" ID="gvOrderItems" AutoGenerateColumns="false" Width="100%"
                        HeaderStyle-BackColor="#FFCC00" RowStyle-Height="22px" CssClass="gridcss1">
                        <Columns>
                            <asp:BoundField DataField="processId" HeaderText="PROCESS ID" />
                            <asp:BoundField DataField="Config" HeaderText="PLATFORM" />
                            <asp:BoundField DataField="Brand" HeaderText="BRAND" />
                            <asp:BoundField DataField="Sidewall" HeaderText="SIDEWALL" />
                            <asp:BoundField DataField="TyreType" HeaderText="TYRE TYPE" />
                            <asp:BoundField DataField="TyreSize" HeaderText="TYRE SIZE" />
                            <asp:BoundField DataField="TyreRim" HeaderText="RIM" />
                            <asp:BoundField DataField="qty" HeaderText="QTY" ItemStyle-HorizontalAlign="Right" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td style="padding-left: 0px;">
                    <div id="divItemDetails" style="display: none;">
                        <table cellspacing="0" rules="all" border="1" class="tableCss1" style="line-height: 25px;">
                            <tr>
                                <th>
                                    BARCODE
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="txtBarcode" ClientIDMode="Static" Text="" MaxLength="19"
                                        Width="180px" TabIndex="0" CssClass="form-control"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Button runat="server" ID="btnBarcodeCheck" Text="CHECK" OnClick="btnBarcodeCheck_Click"
                                        OnClientClick="javascript:return CtrlbtnSaveBarcodeCheck();" CssClass="btn btn-success"
                                        BackColor="#c5c013" />
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblBarcode" ClientIDMode="Static" Text="" Width="180px"></asp:Label>
                                </td>
                                <th>
                                    SCANNED QTY
                                </th>
                                <td style="padding-top: 10px; width: 200px;">
                                    <asp:Label runat="server" ID="lblScannedQty" ClientIDMode="Static" Text="0" Font-Bold="true"
                                        Font-Size="30px"></asp:Label>
                                    &nbsp;<asp:Label runat="server" ID="Label2" ClientIDMode="Static" Text="/" Font-Bold="true"
                                        Font-Size="30px"></asp:Label>
                                    &nbsp;
                                    <asp:Label runat="server" ID="lblTotalQty" ClientIDMode="Static" Text="0" Font-Bold="true"
                                        Font-Size="30px" Width="100px"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <asp:TextBox runat="server" ID="txtLoadScanStatus" ClientIDMode="Static" Text=""
                                        CssClass="statusTxt" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: center;">
                    <asp:Button runat="server" ID="btnComplete" ClientIDMode="Static" Text="COMPLETE ORDER AND INWARD"
                        OnClick="btnComplete_Click" CssClass="btn btn-success" BackColor="#387509" Style="display: none;"
                        Width="375px" />
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnMasterId" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        $(document).ready(function () {
            $('#txtBarcode').blur(function (e) {
                if ($('#' + this.id).val().length >= 19 && $('#lblBarcode').html() != $('#' + this.id).val()) {
                    $('#txtBarcode').focus();
                }
            }).keypress(function (e) {
                if (e.keycode == 13 || e.which == 13) {
                    if ($('#' + this.id).val().length >= 19 && $('#lblBarcode').html() != $('#' + this.id).val()) {
                        $("#btnBarcodeCheck").trigger("click");
                        $('#txtBarcode').focus();
                    }
                }
            });
            $('#txtLoadScanStatus').keydown(function (e) {
                e.keyWhich = 0; e.keyCode = 0; e.preventDefault();
            });
        });
        function CtrlbtnSaveBarcodeCheck() {
            if ($('#txtBarcode').val().length == 0) {
                alert('ENTER BARCODE TO CHECK');
                $('#txtBarcode').focus();
                return false;
            }
            else
                return true;
        }
        function gotoPreviewDiv(ctrlID) {
            $('#' + ctrlID).css({ 'display': 'block' });
            $("html, body").stop().animate({ scrollLeft: $('#' + ctrlID).offset().left, scrollTop: $('#' + ctrlID).offset().top }, 1200);
        }
        function isCompleted() {
            $('#MainContent_btnBarcodeCheck').css({ 'display': 'none' });
            $('#txtBarcode').attr('disabled', 'disabled');
            $('#btnComplete').css({ 'display': 'inline-block' });
        }
    </script>
</asp:Content>
