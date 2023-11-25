<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="Exp_Documents.aspx.cs" Inherits="TTS.Exp_Documents" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
    <link href="Styles/uploadify.css" rel="stylesheet" type="text/css" />
    <link href="Styles/datemonthyear.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/cotsScript.js" type="text/javascript"></script>
    <style type="text/css">
        .subTable th
        {
            text-align: left;
            font-size: 12px;
            width: 160px;
        }
    </style>
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
                <td colspan="2">
                    <asp:GridView runat="server" ID="gvDocumentVerifyList" AutoGenerateColumns="false"
                        Width="100%" HeaderStyle-BackColor="#3c763d" HeaderStyle-ForeColor="#ffffff"
                        RowStyle-Height="30px" HeaderStyle-Height="30px">
                        <Columns>
                            <asp:TemplateField HeaderText="CUSTOMER NAME">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCustomerName" Text='<%#Eval("custfullname") %>'></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdnStatusCustCode" Value='<%# Eval("CustCode") %>' />
                                    <asp:HiddenField runat="server" ID="hdnUserCurrency" Value='<%# Eval("usercurrency") %>' />
                                    <asp:HiddenField runat="server" ID="hdContainerNo" Value='<%# Eval("containerno") %>' />
                                    <asp:HiddenField runat="server" ID="hdnStatusID" Value='<%# Eval("OrderStatus") %>' />
                                    <asp:HiddenField runat="server" ID="hdnOrderID" Value='<%# Eval("ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ORDER REF NO.">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOrderRefNo" Text='<%#Eval("OrderRefNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ORDER DATE">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOrderDate" Text='<%#Eval("CompletedDate") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="QTY" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblQuantity" Text='<%#Eval("itemqty") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="WT" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblWt" Text='<%# Eval("wt") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="LOADED">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblLoaded" Text='<%# Eval("Loadedon") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SHIPMENT TYPE">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblShipmentType" Text='<%# Eval("ShipmentType") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="FINAL LOADING">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblPlant" Text='<%#Eval("ContainerLoadFrom") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="STATUS" DataField="StatusText" />
                            <asp:TemplateField HeaderText="ACTION" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Button runat="server" ID="btnShowDetails" Text="PROCESS" OnClick="btnShowDetails_Click"
                                        ClientIDMode="Static" CssClass="btn btn-info"></asp:Button>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:GridView runat="server" ID="gv_OrderSumValue" AutoGenerateColumns="true" Width="100%"
                        HeaderStyle-BackColor="#166502" HeaderStyle-ForeColor="#ffffff" ShowFooter="true"
                        FooterStyle-BackColor="#dfe0f3" RowStyle-HorizontalAlign="Right" RowStyle-VerticalAlign="Middle"
                        FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" Font-Bold="true"
                        Font-Size="15px" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr style="text-align: center; background-color: #3c763d; overflow: hidden; font-size: 15px;
                color: #ffffff; width: 100%;">
                <td>
                    <asp:Label ID="lblSelectedCustomerName" runat="server" Text="" Font-Bold="true"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblSelectedOrderRefNo" ClientIDMode="Static" runat="server" Text=""
                        Font-Bold="true"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div id="div_Doc_Upload" style="display: none;">
                        <table cellspacing="0" rules="all" border="1" style="background-color: #FFFED0; width: 100%;
                            border-color: White; line-height: 30px;" class="subTable">
                            <tr style="text-align: center; background-color: #75edf9; font-size: 12px; width: 100%;
                                height: 20px; font-weight: bold; line-height: 0px;">
                                <td colspan="4">
                                    FILE UPLOAD
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    INVOICE
                                </th>
                                <td colspan="3">
                                    <div style="text-align: left;" runat="server" id="div_IN_upload" clientidmode="Static">
                                        <asp:FileUpload ID="FileUploadControl_IN" ClientIDMode="Static" runat="server" />
                                    </div>
                                    <div id="div_IN_Clear" runat="server" style="display: none;">
                                        <asp:LinkButton runat="server" ID="lnkIN" Text="" OnClick="lnlPdfDownload_Click"></asp:LinkButton>
                                        <asp:Button runat="server" ID="btnDeleteIN" ClientIDMode="Static" Text="DELETE" OnClick="btnDelete_Click"
                                            CssClass="btn btn-danger" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    PACKING LIST
                                </th>
                                <td colspan="3">
                                    <div style="text-align: left;" id="div_PL_upload" runat="server" clientidmode="Static">
                                        <asp:FileUpload ID="FileUploadControl_PL" ClientIDMode="Static" runat="server" />
                                    </div>
                                    <div id="div_PL_Clear" runat="server" style="display: none;">
                                        <asp:LinkButton runat="server" ID="lnkPL" Text="" OnClick="lnlPdfDownload_Click"></asp:LinkButton>
                                        <asp:Button runat="server" ID="btnDeletePL" ClientIDMode="Static" Text="DELETE" OnClick="btnDelete_Click"
                                            CssClass="btn btn-danger" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    BL/AWP
                                </th>
                                <td colspan="3">
                                    <div style="text-align: left;" id="div_BL_upload" runat="server" clientidmode="Static">
                                        <asp:FileUpload ID="FileUploadControl_BL" ClientIDMode="Static" runat="server" />
                                    </div>
                                    <div id="div_BL_Clear" runat="server" style="display: none;">
                                        <asp:LinkButton runat="server" ID="lnkBL" Text="" OnClick="lnlPdfDownload_Click"></asp:LinkButton>
                                        <asp:Button runat="server" ID="btnDeleteBL" ClientIDMode="Static" Text="DELETE" OnClick="btnDelete_Click"
                                            CssClass="btn btn-danger" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    CERTIFICATE OF ORIGIN
                                </th>
                                <td colspan="3">
                                    <div style="text-align: left;" id="div_Origin_upload" runat="server" clientidmode="Static">
                                        <asp:FileUpload ID="FileUploadControl_ORIGIN" ClientIDMode="Static" runat="server" />
                                    </div>
                                    <div id="div_Origin_Clear" runat="server" style="display: none;">
                                        <asp:LinkButton runat="server" ID="lnkOrigin" Text="" OnClick="lnlPdfDownload_Click"></asp:LinkButton>
                                        <asp:Button runat="server" ID="btnDeleteOrigin" ClientIDMode="Static" Text="DELETE"
                                            OnClick="btnDelete_Click" CssClass="btn btn-danger" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    INSURANCE
                                </th>
                                <td colspan="3">
                                    <div style="text-align: left;" id="div_Insur_upload" runat="server" clientidmode="Static">
                                        <asp:FileUpload ID="FileUploadControl_Insur" ClientIDMode="Static" runat="server" />
                                    </div>
                                    <div id="div_Ins_Clear" runat="server" style="display: none;" clientidmode="Static">
                                        <asp:LinkButton runat="server" ID="lnkIns" Text="" OnClick="lnlPdfDownload_Click"></asp:LinkButton>
                                        <asp:Button runat="server" ID="btnDeleteInsur" ClientIDMode="Static" Text="DELETE"
                                            OnClick="btnDelete_Click" CssClass="btn btn-danger" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    OTHER DOCUMENT
                                </th>
                                <td colspan="3">
                                    <div style="text-align: left;" runat="server" id="div_Oth_upload" clientidmode="Static">
                                        <asp:FileUpload ID="FileUploadControl_Oth" ClientIDMode="Static" runat="server" />
                                    </div>
                                    <div id="div_Oth_Clear" runat="server" style="display: none;">
                                        <asp:LinkButton runat="server" ID="lnkOth" Text="" OnClick="lnlPdfDownload_Click"></asp:LinkButton>
                                        <asp:Button runat="server" ID="btnDeleteOth" ClientIDMode="Static" Text="DELETE"
                                            OnClick="btnDelete_Click" CssClass="btn btn-danger" />
                                    </div>
                                    <div style="display: none;">
                                        <asp:Button runat="server" ID="btnUpload" ClientIDMode="Static" Text="" OnClick="btnUpload_Click" />
                                    </div>
                                </td>
                            </tr>
                            <tr style="text-align: center; background-color: #e0d65a; font-size: 12px; width: 100%;
                                height: 20px; font-weight: bold; line-height: 0px;">
                                <td colspan="4">
                                    FILE BASED DATA
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    INVOICE NO
                                </th>
                                <td>
                                    <asp:TextBox ID="txt_InvoiceNo" Width="250px" runat="server" ClientIDMode="Static"
                                        CssClass="form-control" MaxLength="20"></asp:TextBox>
                                </td>
                                <th>
                                    INVOICE DATE
                                </th>
                                <td>
                                    <asp:TextBox ID="txt_Date" Width="250px" runat="server" ClientIDMode="Static" CssClass="form-control"
                                        MaxLength="20"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    SHIPPING BILL NO
                                </th>
                                <td>
                                    <asp:TextBox ID="txt_SBillingNO" Width="250px" runat="server" ClientIDMode="Static"
                                        CssClass="form-control" MaxLength="50"></asp:TextBox>
                                </td>
                                <th>
                                    SHIPPING BILL DATE
                                </th>
                                <td>
                                    <asp:TextBox ID="txt_SBillDate" Width="250px" runat="server" ClientIDMode="Static"
                                        CssClass="form-control" MaxLength="20"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    BL NO
                                </th>
                                <td>
                                    <asp:TextBox ID="txt_Blno" Width="250px" runat="server" ClientIDMode="Static" CssClass="form-control"
                                        MaxLength="50"></asp:TextBox>
                                </td>
                                <th>
                                    BL DATE
                                </th>
                                <td>
                                    <asp:TextBox ID="txt_BLDate" Width="250px" runat="server" ClientIDMode="Static" CssClass="form-control"
                                        MaxLength="20"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    CURRENCY
                                </th>
                                <td>
                                    <asp:TextBox ID="txt_Currency" Width="250px" runat="server" ClientIDMode="Static"
                                        CssClass="form-control" Enabled="false" MaxLength="20"></asp:TextBox>
                                </td>
                                <th>
                                    EXCHANGE RATE
                                </th>
                                <td>
                                    <asp:TextBox ID="txt_ExchangeRate" Width="250px" runat="server" ClientIDMode="Static"
                                        CssClass="form-control" MaxLength="6" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    CONTAINER NO
                                </th>
                                <td>
                                    <asp:TextBox ID="txt_ContainerNo" Width="250px" runat="server" ClientIDMode="Static"
                                        CssClass="form-control" MaxLength="50"></asp:TextBox>
                                </td>
                                <th>
                                    PORT
                                </th>
                                <td>
                                    <asp:TextBox ID="txt_Port" Width="250px" runat="server" ClientIDMode="Static" CssClass="form-control"
                                        MaxLength="20"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="text-align: center;">
                                    <asp:Button ID="btn_Clear" runat="server" Text="CLEAR" ClientIDMode="Static" CssClass="btn btn-danger"
                                        OnClick="btn_Clear_Click"></asp:Button><asp:Button runat="server" ID="btnMoveStatus"
                                            ClientIDMode="Static" Text="MOVE TO DOCUMENTS COURIER" OnClick="btnMoveStatus_Click"
                                            CssClass="btn btn-info" />
                                </td>
                                <td colspan="2" style="text-align: center">
                                    <asp:Button ID="btn_Save" runat="server" Text="SAVE" ClientIDMode="Static" CssClass="btn btn-success"
                                        OnClientClick="return Check();" OnClick="btn_Save_Click"></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="div_doc_courier" style="display: none;">
                        <table cellspacing="0" rules="all" border="1" style="background-color: #dcecfb; width: 100%;
                            border-color: White; border-collapse: separate;">
                            <tr>
                                <th>
                                    Tracking Number
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_TarckNo" ClientIDMode="Static" Text="" MaxLength="50"
                                        CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    Courier Through
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_CourierThrough" ClientIDMode="Static" Text=""
                                        CssClass="form-control" MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    Couriered Date
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_CourierDate" ClientIDMode="Static" Text="" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="text-align: center;">
                                    <asp:Button runat="server" ID="btnCourier" ClientIDMode="Static" Text="SAVE COURIER DETAILS"
                                        CssClass="btn btn-success" OnClick="btnCourier_Click" OnClientClick="javascript:return CtrlCourierData();" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="div_payment_received" style="display: none;">
                        PAYMENT RECEIVED DATE:
                        <asp:TextBox runat="server" ID="txt_PaymentReceive" ClientIDMode="Static" Text=""
                            CssClass="form-control"></asp:TextBox>
                        <br />
                        <asp:Button runat="server" ID="btnPaymentSave" ClientIDMode="Static" Text="PAYMENT RECEIVED"
                            OnClick="btnPaymentSave_Click" CssClass="btn btn-success" />
                    </div>
                    <div id="div_arrived" style="display: none;">
                        CONTAINER DELIVERED DATE:
                        <asp:TextBox runat="server" ID="txt_ArrivedOn" ClientIDMode="Static" Text="" CssClass="form-control"></asp:TextBox>
                        <br />
                        <asp:Button runat="server" ID="btnDelivered" ClientIDMode="Static" Text="DELIVERED"
                            OnClick="btnDelivered_Click" CssClass="btn btn-success" />
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnCustCode" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnPlant" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnSelectIndex" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnFileType" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnOID" ClientIDMode="Static" Value="" />
    <script src="Scripts/datemonthyear.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#txt_Date").datepicker({ minDate: "-150D", maxDate: "+0D" }).keydown(function (e) { e.keyWhich = 0; e.keyCode = 0; e.preventDefault(); });
            $("#txt_BLDate").datepicker({ minDate: "-150D", maxDate: "+0D" }).keydown(function (e) { e.keyWhich = 0; e.keyCode = 0; e.preventDefault(); });
            $("#txt_SBillDate").datepicker({ minDate: "-150D", maxDate: "+0D" }).keydown(function (e) { e.keyWhich = 0; e.keyCode = 0; e.preventDefault(); });
            $("#txt_CourierDate").datepicker({ minDate: "-150D", maxDate: "+0D" }).keydown(function (e) { e.keyWhich = 0; e.keyCode = 0; e.preventDefault(); });
            $("#txt_PaymentReceive").datepicker({ minDate: "-150D", maxDate: "+0D" }).keydown(function (e) { e.keyWhich = 0; e.keyCode = 0; e.preventDefault(); });
            $("#txt_ArrivedOn").datepicker({ minDate: "-150D", maxDate: "+0D" }).keydown(function (e) { e.keyWhich = 0; e.keyCode = 0; e.preventDefault(); });
            $('input[type=file]').change(function () {
                $('#hdnFileType').val($(this).attr('id'));
                $('#btnUpload').trigger('click');
            });
            $(':text').bind('keydown', function (e) { if (e.keyCode == 13) { e.preventDefault(); return false; } });
        });
        function Check() {
            var errMsg = '';
            if ($('#MainContent_lnkIN').html().length == 0)
                errMsg += 'Kinldy upload invoice file<br/>';
            if ($('#txt_InvoiceNo').val().length == 0)
                errMsg += 'Enter invoice no<br/>';
            if ($('#txt_Date').val().length == 0)
                errMsg += 'Enter invoice date<br/>';
            if ($('#txt_SBillingNO').val().length > 0 && $('#txt_SBillDate').val().length == 0)
                errMsg += 'Enter shipping bill date<br/>';
            if ($('#txt_Blno').val().length > 0 && $('#txt_BLDate').val().length == 0)
                errMsg += 'Enter BL date<br/>';
            if ($('#txt_ExchangeRate').val().length == 0)
                errMsg += 'Enter Exchange Rate<br/>';
            if (errMsg.length > 0) {
                $('#lblErrMsg').html(errMsg);
                return false;
            }
            else
                return true;
        }

        function CtrlCourierData() {
            var errmsg = '';
            if ($('#txt_TarckNo').val() == "")
                errmsg += 'Enter tracking number\n';
            if ($('#txt_CourierThrough').val() == "")
                errmsg += 'Enter courier through\n';
            if ($('#txt_CourierDate').val() == "")
                errmsg += 'Enter couriered date\n';
            if (errmsg.length > 0) {
                alert(errmsg);
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
