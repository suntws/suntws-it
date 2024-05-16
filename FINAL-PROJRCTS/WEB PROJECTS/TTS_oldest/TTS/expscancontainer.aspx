<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="expscancontainer.aspx.cs" Inherits="TTS.expscancontainer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/pdistyle.css" rel="stylesheet" type="text/css" />
    <link href="Styles/TinyStyle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .tbExp
        {
            text-align: center;
            font-weight: bold;
        }
        .tbExp th
        {
            background-color: #ead1f9;
            text-align: center;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: center" class="pageTitleHead">
        <asp:Label runat="server" ID="lblPageTitle" ClientIDMode="Static" Text=""></asp:Label>
        CONTAINER LOADING BARCODE SCAN VERIFICATION
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
                            <asp:BoundField HeaderText="QTY" DataField="orderqty" />
                            <asp:BoundField HeaderText="INSPECTED BY" DataField="inspectedby" />
                            <asp:BoundField HeaderText="APPROVED BY" DataField="approvedby" />
                            <asp:BoundField HeaderText="APPROVED ON" DataField="approvedon" />
                            <asp:BoundField HeaderText="PRODUCT" DataField="Product" />
                            <asp:TemplateField HeaderText="ACTION">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hdnOrderCustCode" Value='<%# Eval("custcode") %>' />
                                    <asp:HiddenField runat="server" ID="hdnOrderPID" Value='<%# Eval("ID") %>' />
                                    <asp:HiddenField runat="server" ID="hdnOrderRefNo" Value='<%# Eval("orderrefno") %>' />
                                    <asp:HiddenField runat="server" ID="hdnOrderID" Value='<%# Eval("O_ID") %>' />
                                    <asp:HiddenField runat="server" ID="hdnProduct" Value='<%# Eval("Product") %>' />
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
                                <th>
                                    ORDER QTY
                                </th>
                                <td>
                                    <asp:Label runat="server" ID="lblOrderQty" ClientIDMode="Static" Text=""></asp:Label>
                                </td>
                                <th>
                                    INSPECTED BY
                                </th>
                                <td>
                                    <asp:Label runat="server" ID="lblInspectedBy" ClientIDMode="Static" Text=""></asp:Label>
                                </td>
                                <td rowspan="2">
                                    <div id="div_export_data" style="display: none;">
                                        <table cellspacing="0" rules="all" border="1" style="background-color: #dcecfb; width: 100%;
                                            border-color: White; border-collapse: separate;">
                                            <tr>
                                                <th>
                                                    SHIPMENT METHOD
                                                </th>
                                                <td>
                                                    <asp:Label runat="server" ID="lblShipType" ClientIDMode="Static" Text=""></asp:Label>
                                                </td>
                                                <td rowspan="3" style="vertical-align: top;" class="tbExp">
                                                    <asp:GridView runat="server" ID="gvCombiQty" AutoGenerateColumns="false" Width="350px"
                                                        ShowFooter="true" FooterStyle-HorizontalAlign="Right">
                                                        <Columns>
                                                            <asp:BoundField DataField="ItemPlant" HeaderText="PLANT" ItemStyle-Width="50px" HeaderStyle-Width="50px" />
                                                            <asp:BoundField DataField="orderqty" HeaderText="QTY" ItemStyle-HorizontalAlign="Right"
                                                                ItemStyle-Width="40px" HeaderStyle-Width="40px" />
                                                            <asp:BoundField DataField="StatusText" HeaderText="STATUS" ItemStyle-Width="250px"
                                                                HeaderStyle-Width="250px" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th style="width: 150px;">
                                                    PACKING METHOD
                                                </th>
                                                <td>
                                                    <asp:Label runat="server" ID="lblPackingMethod" ClientIDMode="Static" Text=""></asp:Label>
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
                                        </table>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    APPROVED ON
                                </th>
                                <td>
                                    <asp:Label runat="server" ID="lblApprovedDate" ClientIDMode="Static" Text=""></asp:Label>
                                </td>
                                <th>
                                    APPROVED BY
                                </th>
                                <td>
                                    <asp:Label runat="server" ID="lblApprovedBy" ClientIDMode="Static" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <asp:UpdatePanel ID="updatePanel2" runat="server">
                                        <ContentTemplate>
                                            <table cellspacing="0" rules="all" border="1" style="width: 100%;">
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
                                                            CssClass="btn btn-success" BackColor="#c5c013" />
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblBarcode" ClientIDMode="Static" Text="" Width="300px"></asp:Label>
                                                    </td>
                                                    <th>
                                                        LOADED SCAN QTY
                                                    </th>
                                                    <td style="padding-top: 10px;">
                                                        <asp:Label runat="server" ID="lblLoadScanQty" ClientIDMode="Static" Text="" Font-Bold="true"
                                                            Font-Size="40px" Width="280px"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="6">
                                                        <asp:TextBox runat="server" ID="txtLoadScanStatus" ClientIDMode="Static" Text=""
                                                            CssClass="statusTxt"></asp:TextBox><asp:Button ID="btnTriggerLoadScan" runat="server"
                                                                ClientIDMode="static" OnClick="btnTriggerLoadScan_Click" Style="visibility: hidden;" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnTriggerLoadScan" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr style="text-align: center;">
                                <td colspan="3">
                                    <span class="btnShowData" id='All' onclick="showpdilist(this.id)">SHOW LOADED LIST</span>
                                </td>
                                <td colspan="2">
                                    <asp:Button runat="server" ID="btnLoadCheck" ClientIDMode="Static" Text="PALLET LOAD CHECK"
                                        OnClick="btnLoadCheck_Click" CssClass="btn btn-success" />
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
                                CONTAINER NO.
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtContainerNo" ClientIDMode="Static" Text="" CssClass="form-control"></asp:TextBox>
                            </td>
                            <th>
                                VEHICLE NO.
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtVehicleNo" ClientIDMode="Static" Text="" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                REMARKS<br />
                                <asp:TextBox runat="server" ID="txtRemarks" ClientIDMode="Static" TextMode="MultiLine"
                                    Text="" Height="70px" Width="440px" onKeyUp="javascript:CheckMaxLength(this, 499);"
                                    onChange="javascript:CheckMaxLength(this, 499);" CssClass="form-control"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnSaveLoadStatus" ClientIDMode="Static" Text="DISPATCH TO CUSTOMER"
                                    OnClick="btnSaveLoadStatus_Click" OnClientClick="javascript: return CtrlbtnSaveLoadStatus();"
                                    CssClass="btn btn-success" BackColor="#387509" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnCustcode" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnOrderRef" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnPID" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnSelectIndex" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnTotOrderQty" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnOID" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnDispProdcut" ClientIDMode="Static" Value="" />
    <script src="Scripts/TinyboxScript.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#txtBarcode').focus();
            $('#txtBarcode').blur(function (e) {
                if ($('#' + this.id).val().length >= 19 && $('#lblBarcode').html() != $('#' + this.id).val()) {
                    $("#btnTriggerLoadScan").trigger("click");
                    $('#txtBarcode').focus();
                }
            }).keypress(function (e) {
                if (e.keycode == 13 || e.which == 13) {
                    if ($('#' + this.id).val().length >= 19 && $('#lblBarcode').html() != $('#' + this.id).val()) {
                        $("#btnTriggerLoadScan").trigger("click");
                        $('#txtBarcode').focus();
                    }
                }
            });
            $('#txtLoadScanStatus').keydown(function (e) {
                e.keyWhich = 0; e.keyCode = 0; e.preventDefault();
            });
        });

        function CtrlbtnSaveLoadStatus() {
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

        function showpdilist(clicked_id) {
            if ($('#hdnPID').val() != '') {
                TINY.box.show({ iframe: 'expscanpdidatashow.aspx?pid=' + $('#hdnPID').val() + '&plant=' + clicked_id + '&mtype=loaded',
                    boxid: 'frameless', width: 1106, height: 600, fixed: false, maskid: 'bluemask', maskopacity: 40, closejs: function () { }
                })
            }
            else
                alert('NO RECORDS');
        }

        function gotoPreviewDiv(ctrlID) {
            $('#' + ctrlID).css({ 'display': 'block' });
            $("html, body").stop().animate({ scrollLeft: $('#' + ctrlID).offset().left, scrollTop: $('#' + ctrlID).offset().top }, 1200);
        }

        function speak(text, callback) {
            var u = new SpeechSynthesisUtterance();
            u.text = text;
            u.lang = 'en-US';
            u.onend = function () {
                if (callback) {
                    callback();
                }
            };
            u.onerror = function (e) {
                if (callback) {
                    callback(e);
                }
            };
            speechSynthesis.speak(u);
        }
    </script>
</asp:Content>
