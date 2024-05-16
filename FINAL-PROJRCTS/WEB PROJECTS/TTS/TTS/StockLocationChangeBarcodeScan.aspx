<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="StockLocationChangeBarcodeScan.aspx.cs" Inherits="TTS.StockLocationChangeBarcodeScan" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
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
        .deletebutton
        {
            background-color: #E83D19;
            border: none;
            color: white;
            text-align: center;
            cursor: pointer;
            height: 25px;
            font-weight: bold;
            font-family: Times New Roman;
        }
        .editbutton
        {
            background-color: #7a8a42;
            border: none;
            color: white;
            text-align: center;
            cursor: pointer;
            height: 25px;
            font-weight: bold;
            font-family: Times New Roman;
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
    <div id="displayOrderPrepare" class="contPage" >
        <table class="tableCss" border="1">
            <tr>
                <th style="width: 17%;">
                    FROM
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="ddlLocatFrom" ClientIDMode="Static" CssClass="form-control"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlLocatFrom_indexchanged" Width="275px">
                    </asp:DropDownList>
                </td>
                <th style="width: 17%;">
                    TO
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="ddlLocatTo" ClientIDMode="Static" 
                        CssClass="form-control"  AutoPostBack="true"
                        Width="275px" onselectedindexchanged="ddlLocatTo_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="4" style="padding-left: 0px;">
                    <div id="divFilters" style="display: none;">
                        <table class="tableCss1" border="1">
                            <tr>
                                <th>
                                    PLATFORM
                                </th>
                                <th>
                                    BRAND
                                </th>
                                <th>
                                    SIDEWALL
                                </th>
                                <th>
                                    TYRE TYPE
                                </th>
                                <th>
                                    TYRE SIZE
                                </th>
                                <th>
                                    RIM SIZE
                                </th>
                                <th>
                                    QUANTITY
                                </th>
                                <td style="text-align: center;">
                                    <asp:Button runat="server" ID="btnClear" ClientIDMode="Static" Text="CLEAR" CssClass="btn-danger"
                                        Width="120px" OnClick="btnClear_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlPLatform" ClientIDMode="Static" CssClass="form-control"
                                        Width="110px" AutoPostBack="true" OnSelectedIndexChanged="ddlPlatform_indexchanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlBrand" ClientIDMode="Static" CssClass="form-control"
                                        Width="110px" AutoPostBack="true" OnSelectedIndexChanged="ddlBrand_indexchanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlSidewall" ClientIDMode="Static" CssClass="form-control"
                                        Width="110px" AutoPostBack="true" OnSelectedIndexChanged="ddlSidewall_indexchanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlType" ClientIDMode="Static" CssClass="form-control"
                                        Width="110px" AutoPostBack="true" OnSelectedIndexChanged="ddlType_indexchanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlSize" ClientIDMode="Static" CssClass="form-control"
                                        Width="110px" AutoPostBack="true" OnSelectedIndexChanged="ddlSize_indexchanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlRim" ClientIDMode="Static" CssClass="form-control"
                                        Width="110px" AutoPostBack="true" OnSelectedIndexChanged="ddlRim_indexchanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtQty" ClientIDMode="Static" Width="150px" Height="20px"
                                        MaxLength="4" oninput="this.value = this.value.replace(/[^0-9]/g, '').replace(/(\..*?)\..*/g, '$1');"></asp:TextBox>
                                </td>
                                <td style="text-align: center;">
                                    <asp:Button runat="server" ID="btnSave" ClientIDMode="Static" Text="SAVE" CssClass="btn-success"
                                        OnClick="btnSave_Click" OnClientClick="javascript:return cntrlCheck();" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="4" style="padding-left: 0px;">
                    <asp:GridView runat="server" ID="grdStockChangeDetails" CssClass="gridcss" AutoGenerateColumns="false"
                        ClientIDMode="Static" ShowFooter="true" FooterStyle-Font-Bold="true" OnRowDeleting="grdStockChangeDetails_RowDeleting"
                        OnSelectedIndexChanged="grdStockChangeDetails_SelectedIndexChanged">
                        <Columns>
                            <asp:BoundField DataField="processId" HeaderText="PROCESS ID" />
                            <asp:BoundField DataField="Config" HeaderText="PLATFORM" />
                            <asp:BoundField DataField="Brand" HeaderText="BRAND" />
                            <asp:BoundField DataField="Sidewall" HeaderText="SIDEWALL" />
                            <asp:BoundField DataField="TyreType" HeaderText="TYRE TYPE" />
                            <asp:BoundField DataField="TyreSize" HeaderText="TYRE SIZE" />
                            <asp:BoundField DataField="TyreRim" HeaderText="RIM" />
                            <asp:BoundField DataField="qty" HeaderText="QTY" ItemStyle-HorizontalAlign="Right" />
                            <asp:TemplateField HeaderText="ACTION">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hdn_ItemID" Value='<%# Eval("item_ID") %>' />
                                    <asp:Button ID="btnEditItem" runat="server" Text="EDIT" ClientIDMode="Static" CssClass="editbutton"
                                        CommandName="Select" />
                                    <asp:Button ID="btnDeleteItem" runat="server" Text="DELETE" ClientIDMode="Static"
                                        CssClass="deletebutton" CommandName="Delete" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle BackColor="#f7b26b" HorizontalAlign="Right" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="4" style="text-align: center;">
                    <asp:Button runat="server" ID="btnComplete" ClientIDMode="Static" Text="COMPLETE & MOVE TO NEXT PROCESS"
                        CssClass="btnactive" Style="display: none;" OnClick="btnComplete_Click" />
                </td>
            </tr>
        </table>
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
                            <asp:BoundField HeaderText="PLANT" DataField="plant" />
                            <asp:BoundField HeaderText="FROM PLACE" DataField="fromPlace" />
                            <asp:BoundField HeaderText="TO PLACE" DataField="toPlace" />
                            <asp:BoundField HeaderText="QTY" DataField="totalqty" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField HeaderText="CREATED USER" DataField="completedBy" />
                            <asp:BoundField HeaderText="CREATED DATE" DataField="completedOn" />
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
                            <asp:BoundField DataField="scannedqty" HeaderText="SCANNED QTY" ItemStyle-HorizontalAlign="Right" />
                            <asp:TemplateField HeaderText="ACTION" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hdnProcess_Id" Value='<%# Eval("processId") %>' />
                                    <asp:LinkButton runat="server" ID="lnkDelOrder" OnClick="lnkDelOrder_Click" Text="GO TO DELETE"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="divDelBarcode" style="text-align:center;display:none;">
                        <asp:GridView runat="server" ID="gv_ScanedList" AutoGenerateColumns="false" Width="100%"
                            HeaderStyle-BackColor="#c1dbf7" BackColor="WhiteSmoke">
                            <Columns>
                                <asp:BoundField DataField="Barcode" HeaderText="BARCODE" />
                                <asp:BoundField DataField="scannedDate" HeaderText="SCANNED DATE" />
                                <asp:BoundField DataField="scannedBy" HeaderText="SCANNED BY" />
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:CheckBox runat="server" ID="checkAllChk" ClientIDMode="Static" />
                                        ALL
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hdnItem_Id" Value='<%# Eval("O_Item_Id") %>' />
                                        <asp:CheckBox ID="chk_selectQty" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:Label runat="server" ID="lblDelSelectCount" ClientIDMode="Static" Text="" Font-Bold="true"
                            Font-Size="14px"></asp:Label>
                        <asp:Button runat="server" ID="btnDelBarcode" ClientIDMode="Static" Text="DELETE"
                            CssClass="btn btn-danger" OnClick="btnDelBarcode_click" OnClientClick="javascript:return ctrlBtnDelete();" />
                    </div>
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
                    <asp:Button runat="server" ID="btnComplOrder" ClientIDMode="Static" Text="COMPLETE ORDER AND MOVE TO NEXT PROCESS"
                        OnClick="btnSaveDc_Click" CssClass="btn btn-success" BackColor="#387509" Style="display: none;"
                        Width="375px" />
                </td>
            </tr>
            <tr>
                <td style="padding-left: 0px;">
                    <table cellspacing="0" rules="all" border="1" class="tableCss" id="tblComplete" runat="server"
                        clientidmode="Static" style="display: none;">
                        <tr>
                            <th>
                                DC NO.
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtDcno" ClientIDMode="Static" MaxLength="50" Text=""
                                    CssClass="form-control1"></asp:TextBox>
                            </td>
                            <th>
                                VEHICLE NO.
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtVehicleNo" ClientIDMode="Static" MaxLength="50"
                                    Text="" CssClass="form-control1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                REMARKS<br />
                                <asp:TextBox runat="server" ID="txtRemarks" ClientIDMode="Static" TextMode="MultiLine"
                                    Text="" Height="70px" MaxLength="100" Width="500px" onKeyUp="javascript:CheckMaxLength(this, 100);"
                                    onChange="javascript:CheckMaxLength(this, 100);" CssClass="form-control"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnSaveDc" ClientIDMode="Static" Text="PREPARE DC"
                                    OnClick="btnSaveDc_Click" OnClientClick="javascript:return CtrlbtnSaveDc();"
                                    CssClass="btn btn-success" BackColor="#387509" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnMasterId" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnItemId" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnprocessId" ClientIDMode="Static" Value="" />
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
            $('#txtDcno').blur(function (e) {
                if ($('#txtDcno').val().length > 0) {
                    $.ajax({ type: "POST", url: "BindRecords.aspx?type=LocateChangecheckDCno&dcno=" + $('#txtDcno').val(), context: document.body,
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
            $('#checkAllChk').click(function () {
                if ($("[id*=MainContent_gv_ScanedList_chk_selectQty_]").length > 0) {
                    if ($(this).attr('checked') == "checked")
                        $("[id*=MainContent_gv_ScanedList_chk_selectQty_]").attr('checked', true)
                    else
                        $("[id*=MainContent_gv_ScanedList_chk_selectQty_]").attr('checked', false)
                }
                else {
                    alert('No records');
                    $(this).attr('checked', false);
                }
                $('#lblDelSelectCount').html($("[id*=MainContent_gv_ScanedList_chk_selectQty_]:checked").length + ' QTY SELECTED FOR ');
            });
            $("[id*=MainContent_gv_ScanedList_chk_selectQty_]").click(function () {
                $('#lblDelSelectCount').html($("[id*=MainContent_gv_ScanedList_chk_selectQty_]:checked").length + ' QTY SELECTED FOR ');
                $('#checkAllChk').attr('checked', false);
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
        function CtrlbtnSaveDc() {
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
        function ctrlBtnDelete() {
            if ($("[id*=gv_ScanedList_chk_selectQty_]:checked").length != 0)
                return true;
            else {
                alert("Choose atleast one barcode");
                return false;
            }
        }
        function gotoPreviewDiv(ctrlID) {
            $('#' + ctrlID).css({ 'display': 'block' });
            $("html, body").stop().animate({ scrollLeft: $('#' + ctrlID).offset().left, scrollTop: $('#' + ctrlID).offset().top }, 1200);
        }
        function isCompleted() {
            $('#MainContent_btnBarcodeCheck').css({ 'display': 'none' });
            $('#txtBarcode').attr('disabled', 'disabled');
            $('#btnComplOrder').css({ 'display': 'inline-block' });
        }
        function cntrlCheck() {
            var msg = '';
            if ($('#ddlPLatform option:selected').text() == "CHOOSE" || $('#ddlPLatform option:selected').text() == "") {
                msg = "CHOOSE PLATFORM";
            } else if ($('#ddlBrand option:selected').text() == "CHOOSE" || $('#ddlBrand option:selected').text() == "") {
                msg = "CHOOSE BRAND";
            } else if ($('#ddlSidewall option:selected').text() == "CHOOSE" || $('#ddlSidewall option:selected').text() == "") {
                msg = "CHOOSE SIDEWALL";
            } else if ($('#ddlType option:selected').text() == "CHOOSE" || $('#ddlType option:selected').text() == "") {
                msg = "CHOOSE TYRE TYPE";
            } else if ($('#ddlSize option:selected').text() == "CHOOSE" || $('#ddlSize option:selected').text() == "") {
                msg = "CHOOSE TYRE SIZE";
            } else if ($('#ddlRim option:selected').text() == "CHOOSE" || $('#ddlRim option:selected').text() == "") {
                msg = "CHOOSE RIM SIZE";
            }
            if ($('#txtQty').val() == '')
                msg += "\nENTER ITEM QUANTITY";
            if (msg != '') {
                alert(msg);
                if (msg == "\nENTER ITEM QUANTITY")
                    $('#txtQty').focus();
                return false;
            }
            else
                return true;
        }
        function cntrlConfirmCheck() {
            var msg = '';
            if ($('#ddlLocatFrom option:selected').text() == "CHOOSE" || $('#ddlLocatFrom option:selected').text() == "") {
                msg = "CHOOSE FROM LOCATION";
            } else if ($('#ddlLocatTo option:selected').text() == "CHOOSE" || $('#ddlLocatTo option:selected').text() == "") {
                msg = "CHOOSE TO LOCATION";
            }
            if (msg != '') {
                alert(msg);
                return false;
            }
            else
                return true;
        }
        function disDivShowHide(qAction) {
            if (qAction == 'prepare') {
                $('#displaycontent').attr('hidden', 'hidden');
                $('#displayOrderPrepare').removeAttr('hidden');
            }
            else {
                $('#displayOrderPrepare').attr('hidden', 'hidden');
                $('#displaycontent').removeAttr('hidden');
            }
        }
    </script>
</asp:Content>
