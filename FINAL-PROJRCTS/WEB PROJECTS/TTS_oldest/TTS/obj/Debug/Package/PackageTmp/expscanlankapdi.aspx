<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="expscanlankapdi.aspx.cs" Inherits="TTS.expscanlankapdi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
    <link href="Styles/pdistyle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #lblMsg
        {
            line-height: 30px;
            background-color: #8DDEA6;
            font-weight: bold;
            width: 100%;
            float: left;
        }
        input[type="file"]
        {
            text-align-last: end;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: center" class="pageTitleHead">
        <asp:Label runat="server" ID="lblPageTitle" ClientIDMode="Static" Text=""></asp:Label>
        UPLOAD PDI INSPECTION BARCODE LIST
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div class="contPage">
        <asp:ScriptManager runat="server" ID="scriptManager1">
        </asp:ScriptManager>
        <table cellspacing="0" rules="all" border="1" style="border: solid 1px #525252; border-collapse: collapse;
            width: 100%;">
            <tr>
                <td colspan="2">
                    <table cellspacing="0" rules="all" border="1" style="border: solid 1px #525252; border-collapse: collapse;
                        width: 100%;" class="tbMas">
                        <tr>
                            <th>
                                CUSTOMER
                            </th>
                            <td colspan="3">
                                <asp:Label runat="server" ID="lblCustomer" ClientIDMode="Static" Text=""></asp:Label>
                                <asp:HiddenField runat="server" ID="hdnCustCode" ClientIDMode="Static" Value="" />
                            </td>
                        </tr>
                        <tr>
                            <th>
                                ORDER REF NO
                            </th>
                            <td colspan="3">
                                <asp:Label runat="server" ID="lbl_OrderNo" ClientIDMode="Static" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                DOWNLOAD
                            </th>
                            <td colspan="3">
                                <asp:LinkButton runat="server" ID="lnkWorkOrder" ClientIDMode="Static" Text="" OnClick="lnkWorkOrder_CLick"></asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                W/O NO
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txt_OrderRefNo" ClientIDMode="Static" Text="" MaxLength="100"
                                    Width="150px" CssClass="form-control"></asp:TextBox>
                            </td>
                            <th>
                                ORDER QTY
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txt_OrderQty" ClientIDMode="Static" Text="" MaxLength="4"
                                    Enabled="false" Width="60px" CssClass="form-control" Font-Size="25px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <table id="tb_pdiuploadCtrls" runat="server" cellspacing="0" rules="all" border="1"
                        style="border: solid 1px #525252; border-collapse: collapse; width: 100%; display: none;">
                        <tr>
                            <td style="width: 788px;">
                                <asp:FileUpload ID="fupStockInward" runat="server" ClientIDMode="Static" accept=".xls,.xlsx,.csv,.txt"
                                    CssClass="btn btn-warning" Width="95%" onchange="fupPdiUpload_change()" />
                            </td>
                            <td style="width: 150px; text-align: center;">
                                <asp:Button runat="server" ID="btnBarcodeVerify" ClientIDMode="Static" Text="VERIFY"
                                    OnClick="btnBarcodeVerify_Click" CssClass="btn btn-info" />
                            </td>
                            <td style="width: 150px; text-align: center;">
                                <asp:Button runat="server" ID="btnPdiUploadCancel" Text="CLEAR" OnClick="btnPdiUploadCancel_Click"
                                    CssClass="btn btn-danger" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" style="width: 1100px;">
                                <asp:Label runat="server" ID="lblMsg" ClientIDMode="Static" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <table id="tb_pdi_uploadMsg" runat="server" cellspacing="0" rules="all" border="1"
                        style="border: solid 1px #525252; border-collapse: collapse; width: 100%; display: none;">
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblError" ClientIDMode="Static" Text="" Font-Bold="true"
                                    ForeColor="Red"></asp:Label>
                            </td>
                            <td style="width: 300px; text-align: center;">
                                <asp:Button runat="server" ID="btnPdiUploadFileSave" Text="SAVE PDI BARCODE DATA"
                                    OnClick="btnPdiUploadFileSave_Click" CssClass="btn btn-success" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:GridView runat="server" ID="gvLankaPdiList" AutoGenerateColumns="true" Width="100%"
                                    CssClass="gridcss">
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table id="tb_Shipmethod" runat="server" clientidmode="Static" cellspacing="0" rules="all"
                        border="1" style="width: 400px; display: none;">
                        <tr>
                            <th>
                                SHIPMENT METHOD
                            </th>
                            <td>
                                <asp:Label runat="server" ID="lblShipType" ClientIDMode="Static" Text=""></asp:Label>
                            </td>
                            <td colspan="2" rowspan="3" style="vertical-align: top; line-height: 30px;">
                                <asp:Label runat="server" ID="lblQtyDetails" ClientIDMode="Static" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <th style="width: 150px;">
                                FINAL LOADING FROM
                            </th>
                            <td>
                                <asp:Label runat="server" ID="lblFinalLoading" ClientIDMode="Static" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label runat="server" ID="lblErr" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table cellspacing="0" rules="all" border="1" id="tb_LoadDetails" runat="server"
                        clientidmode="Static" style="display: none;">
                        <tr>
                            <th>
                                CONTAINER NO.
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtContainerNo" ClientIDMode="Static" Text="" CssClass="form-control"
                                    Width="170px"></asp:TextBox>
                            </td>
                            <th>
                                VEHICLE NO.
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtVehicleNo" ClientIDMode="Static" Text="" CssClass="form-control"
                                    Width="170px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                REMARKS<br />
                                <asp:TextBox runat="server" ID="txtRemarks" ClientIDMode="Static" TextMode="MultiLine"
                                    Text="" Height="70px" Width="488px" onKeyUp="javascript:CheckMaxLength(this, 499);"
                                    onChange="javascript:CheckMaxLength(this, 499);" CssClass="form-control"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnSavePdiLoadStatus" ClientIDMode="Static" Text="DISPATCH TO CUSTOMER"
                                    OnClick="btnSavePdiLoadStatus_Click" OnClientClick="javascript: return CtrlbtnSavePdiLoadStatus();"
                                    CssClass="btn btn-success" BackColor="#387509" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnPID" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnPlant" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnPdiFor" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnTotOrderQty" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        function fupPdiUpload_change() {
            $('#MainContent_tb_pdi_upload').css('display', 'none');
            $('#btnBarcodeVerify').click();
        }

        function CtrlbtnSavePdiLoadStatus() {
            var errMsg = '';
            if ($('#txtContainerNo').val().length == 0)
                errMsg += 'Enter Container No.\n';
            if ($('#txtVehicleNo').val().length == 0)
                errMsg += 'Enter Vehicle No.\n';
            if (errMsg.length > 0) {
                alert(errMsg);
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
