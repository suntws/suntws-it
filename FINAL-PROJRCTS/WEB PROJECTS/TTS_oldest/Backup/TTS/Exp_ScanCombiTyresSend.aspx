<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="Exp_ScanCombiTyresSend.aspx.cs" Inherits="TTS.Exp_ScanCombiTyresSend" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/pdistyle.css" rel="stylesheet" type="text/css" />
    <link href="Styles/TinyStyle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .form-control1
        {
            display: block;
            width: 250px;
            height: 30px;
            font-size: 14px;
            color: #000;
            background-color: #fff;
            border: 1px solid #000;
            border-radius: 4px;
        }
        .form-control1:hover, .form-control1:focus
        {
            background-color: #555;
            color: #fff;
        }
        #txtLoadTotQty:hover, #txtLoadTotQty:focus
        {
            background-color: #555;
            color: #fff;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: center;" class="pageTitleHead">
        <asp:Label runat="server" ID="lblPageTitle" ClientIDMode="Static"></asp:Label>
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div class="contPage">
        <asp:ScriptManager runat="server" ID="scriptManager1">
        </asp:ScriptManager>
        <table cellspacing="0" rules="all" border="1" style="background-color: #dcecfb; width: 100%;
            border-color: White; border-collapse: separate;">
            <tr>
                <td colspan="2">
                    <asp:GridView runat="server" ID="gvLoadCheckOrder" AutoGenerateColumns="false" Width="100%"
                        HeaderStyle-BackColor="#FFCC00" RowStyle-Height="22px">
                        <Columns>
                            <asp:BoundField HeaderText="CUSTOMER" DataField="custfullname" />
                            <asp:BoundField HeaderText="WORK ORDER" DataField="workorderno" />
                            <asp:BoundField HeaderText="COMBI ORDER QTY" DataField="orderqty" />
                            <asp:BoundField HeaderText="INSPECTED QTY" DataField="inspectedqty" />
                            <asp:BoundField HeaderText="SENT QTY" DataField="sentqty" />
                            <asp:BoundField HeaderText="BALANCE QTY TO SEND" DataField="remainqty" />
                            <asp:TemplateField HeaderText="ACTION">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hdnCustname" Value='<%# Eval("custfullname") %>' />
                                    <asp:HiddenField runat="server" ID="hdnWo" Value='<%# Eval("workorderno") %>' />
                                    <asp:HiddenField runat="server" ID="hdnOrderPID" Value='<%# Eval("ID") %>' />
                                    <asp:HiddenField runat="server" ID="hdnSenQty" Value='<%# Eval("sentqty") %>' />
                                    <asp:HiddenField runat="server" ID="hdnInspQty" Value='<%# Eval("inspectedqty") %>' />
                                    <asp:HiddenField runat="server" ID="hdnRemain" Value='<%# Eval("remainqty") %>' />
                                    <asp:LinkButton runat="server" ID="lnkPdiLoad" OnClick="lnkPdiLoad_Click" Text="MAKE LOADING"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr style="text-align: center; line-height: 30px; font-size: 18px; background-color: #077305;
                color: #ffffff;">
                <td>
                    <asp:Label runat="server" ID="lblCustName" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblWorkorderNo" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div id="div_LoadOrder" style="display: none;">
                        <table cellspacing="0" rules="all" border="1" class="tbMas" style="line-height: 25px;">
                            <tr>
                                <td colspan="5">
                                    <asp:GridView runat="server" ID="grdDcDetails" AutoGenerateColumns="false" Width="100%"
                                        HeaderStyle-BackColor="#FFCC00" RowStyle-Height="22px">
                                        <Columns>
                                            <asp:BoundField HeaderText="DC No" DataField="dcno" />
                                            <asp:BoundField HeaderText="SENT BY" DataField="createdby" />
                                            <asp:BoundField HeaderText="SENT DATE" DataField="createdon" />
                                            <asp:BoundField HeaderText="SENT QTY" DataField="qty" />
                                            <asp:BoundField HeaderText="VEHICLE No" DataField="vehicleno" />
                                            <asp:BoundField HeaderText="RECEIVED BY" DataField="receivedby" />
                                            <asp:BoundField HeaderText="RECEIVED DATE" DataField="receivedon" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <table id="tblBarcodeScan" cellspacing="0" rules="all" border="1" style="width: 100%;">
                                        <tr>
                                            <th>
                                                LOADED SCAN QTY
                                            </th>
                                            <td style="padding-top: 10px;">
                                                <asp:Label runat="server" ID="lblLoadScanQty" ClientIDMode="Static" Text="0" Font-Bold="true"
                                                    Font-Size="30px"></asp:Label>
                                                &nbsp;<asp:Label runat="server" ID="Label1" ClientIDMode="Static" Text="/" Font-Bold="true"
                                                    Font-Size="30px"></asp:Label>
                                                &nbsp;<asp:TextBox runat="server" ID="txtLoadTotQty" ClientIDMode="Static" Text=""
                                                    Font-Bold="true" Font-Size="30px" Width="80px" oninput="this.value = this.value.replace(/[^0-9.]/g, '').replace(/(\..*?)\..*/g, '$1');"></asp:TextBox>
                                            </td>
                                            <th>
                                                BARCODE
                                            </th>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtBarcode" ClientIDMode="Static" Text="" MaxLength="19"
                                                    Width="180px" TabIndex="0" CssClass="form-control"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Button runat="server" ID="btnBarcodeCheck" Text="CHECK & SAVE" OnClick="btnBarcodeCheck_Click"
                                                    OnClientClick="javascript:return btnBarCheck();" BackColor="#c5c013" />
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblBarcode" ClientIDMode="Static" Text="" Width="300px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6">
                                                <asp:TextBox runat="server" ID="txtLoadScanStatus" ClientIDMode="Static" Text=""
                                                    CssClass="statusTxt"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6">
                                                <asp:GridView runat="server" ID="gvBarcodelist" AutoGenerateColumns="false" Width="100%"
                                                    HeaderStyle-BackColor="#FFCC00" RowStyle-Height="22px">
                                                    <Columns>
                                                        <asp:BoundField HeaderText="PLATFORM" DataField="cplatform" />
                                                        <asp:BoundField HeaderText="TYRE SIZE" DataField="tyresize" />
                                                        <asp:BoundField HeaderText="RIM" DataField="rim" />
                                                        <asp:BoundField HeaderText="TYPE" DataField="tyretype" />
                                                        <asp:BoundField HeaderText="BRAND" DataField="brand" />
                                                        <asp:BoundField HeaderText="SIDEWALL" DataField="sidewall" />
                                                        <asp:BoundField HeaderText="PROCESSID" DataField="processid" />
                                                        <asp:BoundField HeaderText="STENCIL NO" DataField="stencilno" />
                                                        <asp:BoundField HeaderText="GRADE" DataField="grade" />
                                                        <asp:TemplateField HeaderText="ACTION">
                                                            <ItemTemplate>
                                                                <asp:HiddenField runat="server" ID="hdnDelBarcode" Value='<%# Eval("barcode") %>' />
                                                                <asp:LinkButton runat="server" ID="lnkbtnDel" OnClick="lnkbtnDel_Click" Text="DELETE"></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <table cellspacing="0" rules="all" border="1" class="tbMas" id="divLoadDetails" runat="server"
                        clientidmode="Static" style="display: none;">
                        <tr>
                            <th>
                                DC NO.
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtDcno" ClientIDMode="Static" Text="" CssClass="form-control1"></asp:TextBox>
                            </td>
                            <th>
                                VEHICLE NO.
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtVehicleNo" ClientIDMode="Static" Text="" CssClass="form-control1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                REMARKS<br />
                                <asp:TextBox runat="server" ID="txtRemarks" ClientIDMode="Static" TextMode="MultiLine"
                                    Text="" Height="70px" Width="500px" onKeyUp="javascript:CheckMaxLength(this, 499);"
                                    onChange="javascript:CheckMaxLength(this, 499);" CssClass="form-control"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnSaveLoadStatus" ClientIDMode="Static" Text="PREPARE DC"
                                    OnClick="btnSaveLoadStatus_Click" OnClientClick="javascript:return CtrlbtnSaveLoadStatus();"
                                    CssClass="btn btn-success" BackColor="#387509" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnPID" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnSentQty" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnSelectIndex" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnInspectQty" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnBalToSend" ClientIDMode="Static" Value="" />
    <script src="Scripts/TinyboxScript.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //$(':text').bind('keydown', function (e) { if (e.keyCode == 13) { e.preventDefault(); return false; } });
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

            $('#txtLoadTotQty').blur(function (e) {
                if ($('#txtLoadTotQty').val() != "" && (parseInt($('#txtLoadTotQty').val()) > parseInt($('#hdnBalToSend').val()) || parseInt($('#txtLoadTotQty').val()) == 0)) {
                    $('#txtLoadTotQty').val('');
                    alert('ENTER LESS THAN OR EQUAL TO BALANCE QTY');
                    disableElements();
                }
                else if (parseInt($('#txtLoadTotQty').val()) <= parseInt($('#hdnBalToSend').val()) && $('#txtLoadTotQty').val() != "" && parseInt($('#txtLoadTotQty').val()) != 0) {
                    if (parseInt($('#txtLoadTotQty').val()) == parseInt($('#lblLoadScanQty').html())) {
                        endLoad();
                    }
                    if (parseInt($('#txtLoadTotQty').val()) > parseInt($('#lblLoadScanQty').html())) {
                        $('#divLoadDetails').css({ 'display': 'none' });
                        $('#MainContent_btnBarcodeCheck').css({ 'display': 'block' });
                        $('#txtBarcode').removeAttr('disabled');
                        $('#txtBarcode').focus();
                    }
                    if (parseInt($('#txtLoadTotQty').val()) < parseInt($('#lblLoadScanQty').html())) {
                        alert('ENTER GREATER THAN OR EQUAL TO SCANNED QTY');
                        $('#txtLoadTotQty').val($('#lblLoadScanQty').html());
                        $('#divLoadDetails').css({ 'display': 'none' });
                        $('#txtLoadTotQty').focus();
                    }
                }
            });
            $('#txtDcno').blur(function (e) {
                if ($('#txtDcno').val().length > 0) {
                    $.ajax({ type: "POST", url: "BindRecords.aspx?type=checkDCno&dcno=" + $('#txtDcno').val(), context: document.body,
                        success: function (data) {
                            if (data != '') {
                                alert(data);
                                $('#txtDcno').val('');
                                $('#txtDcno').focus();
                            }
                        }
                    });
                }
            });
            $('#txtLoadScanStatus').keydown(function (e) {
                e.keyWhich = 0; e.keyCode = 0; e.preventDefault();
            });
        });

        function btnBarCheck() {
            if ($('#txtBarcode').val().length == 0) {
                alert('ENTER BARCODE TO CHECK');
                $('#txtBarcode').focus();
                return false;
            }
            else
                return true;
        }

        function CtrlbtnSaveLoadStatus() {
            var errMsg = '';
            if ($('#txtDcno').val().length == 0) {
                errMsg += 'Enter DC No.\n';
                $('#txtDcno').focus();
            }
            if ($('#txtVehicleNo').val().length == 0) {
                errMsg += 'Enter Vehicle No.\n';
                $('#txtVehicleNo').focus();
            }
            if (errMsg.length > 0) {
                alert(errMsg);
                return false;
            }
            else
                return true;
        }

        function gotoPreviewDiv(ctrlID) {
            $('#' + ctrlID).css({ 'display': 'block' });
            $("html, body").stop().animate({ scrollLeft: $('#' + ctrlID).offset().left, scrollTop: $('#' + ctrlID).offset().top }, 1200);
        }

        function disableElements() {
            $('#divLoadDetails').css({ 'display': 'none' });
            $('#MainContent_btnBarcodeCheck').css({ 'display': 'none' });
            $('#txtBarcode').attr('disabled', 'disabled');
            $('#lblBarcode').html('');
            $('#txtLoadScanStatus').val('');
            $('#txtLoadTotQty').focus();
        }
        function endLoad() {
            $('#MainContent_btnBarcodeCheck').css({ 'display': 'none' });
            $('#txtBarcode').attr('disabled', 'disabled');
            $('#divLoadDetails').css({ 'display': 'block' });
        }
    </script>
</asp:Content>
