<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="expscaninspectionreport.aspx.cs" Inherits="TTS.expscaninspectionreport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="styles/datemonthyear.css" rel="stylesheet" type="text/css" />
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/cotsScript.js" type="text/javascript"></script>
    <style type="text/css">
        .btn-xs
        {
            padding: 1px 5px;
            font-size: 12px;
            border-radius: 3px;
        }
        #tbl_Inspection
        {
            border-collapse: collapse;
            border-color: Black;
        }
        #tbl_Inspection th
        {
            background-color: #79bbff;
            color: White;
            font-family: Times New Roman;
            font-size: 14px;
            text-align: center;
            font-weight: bold;
        }
        #tbl_Inspection td
        {
            font-family: Times New Roman;
            text-align: center;
            font-weight: bold;
            font-size: 12px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: center;" class="pageTitleHead">
        <asp:Label runat="server" ID="lblPageTitle" ClientIDMode="Static" Text=""></asp:Label>
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="border: solid 1px #525252; border-collapse: collapse;
            width: 100%;">
            <tr>
                <td>
                    <asp:GridView ID="gv_InspectionOrders" runat="server" AutoGenerateColumns="false"
                        CssClass="gridcss">
                        <Columns>
                            <asp:BoundField HeaderText="CUSTOMER" DataField="custfullname" />
                            <asp:BoundField HeaderText="ORDER REF NO" DataField="orderrefno" />
                            <asp:BoundField HeaderText="WORKORDER" DataField="workorderno" />
                            <asp:BoundField HeaderText="QTY" DataField="orderqty" />
                            <asp:BoundField HeaderText="INSPECTION ON" DataField="inspectdate" />
                            <asp:BoundField HeaderText="INSPECTED By" DataField="inspectedby" />
                            <asp:BoundField HeaderText="APPROVED BY" DataField="approvedby" />
                            <asp:TemplateField HeaderText="ACTION" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdn_CustCode" runat="server" Value='<%# Eval("custcode") %>' />
                                    <asp:HiddenField ID="hdn_PID" runat="server" Value='<%# Eval("ID") %>' />
                                    <asp:LinkButton ID="lnk_ViewJathagam" runat="server" Text="Process Order" CssClass="btn btn-info btn-xs"
                                        OnClick="lnk_ViewJathagam_Click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="div_Jathagam" style="width: 100%; display: none;">
                        <div style="text-align: center; padding-left: 10px; padding-right: 10px; background-color: #3c763d;
                            overflow: hidden; height: 25px; font-size: 15px; color: #ffffff;">
                            <div style="float: left;">
                                <asp:Label ID="lbl_CustomerName" ClientIDMode="Static" runat="server" Text="" Font-Bold="true"></asp:Label>
                            </div>
                            <div style="float: right;">
                                <asp:Label ID="lbl_WorkOrderNo" ClientIDMode="Static" runat="server" Text="" Font-Bold="true"></asp:Label>
                            </div>
                        </div>
                        <div style="width: 100%;">
                            <asp:GridView ID="gv_Jathagam" runat="server" AutoGenerateColumns="false" CssClass="gridcss">
                                <Columns>
                                    <asp:BoundField HeaderText="PLATFORM" DataField="config" />
                                    <asp:BoundField HeaderText="BRAND" DataField="brand" />
                                    <asp:BoundField HeaderText="SIDEWALL" DataField="sidewall" />
                                    <asp:BoundField HeaderText="TYPE" DataField="tyretype" />
                                    <asp:BoundField HeaderText="TYRE SIZE" DataField="tyresize" />
                                    <asp:BoundField HeaderText="RIM" DataField="rimsize" />
                                    <asp:TemplateField HeaderText="PROCESS ID" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_ProcessID" runat="server" Text='<%# Eval("processid") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="QTY" DataField="Qty" />
                                    <asp:TemplateField HeaderText="ACTION" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnk_ViewBarcode" runat="server" Text="Process Item" CssClass="btn btn-info btn-xs"
                                                OnClick="lnk_ViewBarcode_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div style="width: 100%;">
                        <asp:LinkButton runat="server" ID="lnkFinlaInpsect" ClientIDMode="Static" OnClick="lnkPDIFileDownload_Click"></asp:LinkButton>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="div_Barcode" style="width: 100%; display: none;">
                        <table cellspacing="0" rules="all" border="1" style="border: solid 1px #525fff; border-collapse: collapse;
                            width: 100%;">
                            <tr>
                                <th>
                                    <span class="spanCss">Platform :</span>
                                </th>
                                <td>
                                    <asp:Label ID="lbl_sel_Platform" runat="server" Font-Bold="true" Text=""></asp:Label>
                                </td>
                                <th>
                                    <span class="spanCss">Brand :</span>
                                </th>
                                <td>
                                    <asp:Label ID="lbl_sel_Brand" runat="server" Font-Bold="true" Text=""></asp:Label>
                                </td>
                                <th>
                                    <span class="spanCss">Sidewall :</span>
                                </th>
                                <td>
                                    <asp:Label ID="lbl_sel_SideWall" runat="server" Font-Bold="true" Text=""></asp:Label>
                                </td>
                                <th>
                                    <span class="spanCss">Type :</span>
                                </th>
                                <td>
                                    <asp:Label ID="lbl_sel_Type" runat="server" Font-Bold="true" Text=""></asp:Label>
                                </td>
                                <th>
                                    <span class="spanCss">Size :</span>
                                </th>
                                <td>
                                    <asp:Label ID="lbl_sel_Size" runat="server" Font-Bold="true" Text=""></asp:Label>
                                </td>
                                <th>
                                    <span class="spanCss">Rim :</span>
                                </th>
                                <td>
                                    <asp:Label ID="lbl_sel_Rim" runat="server" Font-Bold="true" Text=""></asp:Label>
                                </td>
                                <th>
                                    <span class="spanCss">ProcessID :</span>
                                </th>
                                <td>
                                    <asp:Label ID="lbl_sel_ProcessID" runat="server" Font-Bold="true" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="10">
                                    <asp:CheckBoxList runat="server" ID="chk_BarcodeSel" ClientIDMode="Static" RepeatDirection="Horizontal"
                                        RepeatColumns="8" RepeatLayout="Table">
                                    </asp:CheckBoxList>
                                </td>
                                <td colspan="2">
                                    <asp:Button ID="btn_InspectBarcode" runat="server" CssClass="btn btn-success" Text="Inpsect Barcode"
                                        ClientIDMode="Static" Style="font-family: Times New Roman;" OnClick="btn_InspectBarcode_Click" />
                                </td>
                                <td colspan="2">
                                    <asp:Button ID="btn_Reset" runat="server" CssClass="btn btn-warning" Text="Reset Selection"
                                        ClientIDMode="Static" Style="font-family: Times New Roman;" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="14">
                                    <asp:Label ID="lbl_ErrMsg" runat="server" ForeColor="Red" ClientIDMode="Static"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="div_BarcodeInspection" style="width: 100%; display: none;">
                        <table cellspacing="0" rules="all" border="1" style="border: solid 1px #525fff; border-collapse: collapse;
                            width: 100%;">
                            <tr>
                                <td colspan="2">
                                    <div id="div_Dynamictbl" runat="server" style="width: 100%;">
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lbl_ErrMsg1" runat="server" ForeColor="Red" ClientIDMode="Static"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btn_SaveRecords" runat="server" CssClass="btn btn-success" Text="Save Records"
                                        ClientIDMode="Static" Style="font-family: Times New Roman;" OnClick="btn_SaveRecords_Click" />
                                </td>
                                <td>
                                    <asp:Button ID="btn_Reload" runat="server" CssClass="btn btn-warning" Text="Reload Page"
                                        ClientIDMode="Static" Style="font-family: Times New Roman;" />
                                </td>
                            </tr>
                        </table>
                        <div id="div_GenerateRpt" style="width: 100%; display: none;">
                            <table id="tbl_GenerateRpt" class="gridcss" border="1" cellpadding="3" style="width: 100%;">
                                <tr>
                                    <td colspan="4">
                                        <asp:Label ID="lbl_ErrMsg2" runat="server" ForeColor="Red" ClientIDMode="Static"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="spanCss">Order Date</span><br />
                                        <asp:TextBox ID="txt_OrderDate" runat="server" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                    </td>
                                    <td>
                                        <span class="spanCss">Invoive No</span><br />
                                        <asp:TextBox ID="txt_InvoiceNo" runat="server" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                    </td>
                                    <td>
                                        <span class="spanCss">Invoice Date</span><br />
                                        <asp:TextBox ID="txt_InvoiceDate" runat="server" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnGenerateRpt" runat="server" CssClass="btn btn-success" Text="Generate Report"
                                            ClientIDMode="Static" Style="font-family: Times New Roman;" OnClick="btnGenerateRpt_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField ID="hdn_CustCode" runat="server" />
    <asp:HiddenField ID="hdn_PID" runat="server" />
    <asp:HiddenField ID="hdn_OrderQty" runat="server" />
    <asp:HiddenField ID="hdn_orderrefno" runat="server" />
    <script src="scripts/datemonthyear.js" type="text/javascript"></script>
    <script type="text/javascript">
        $("#txt_OrderDate,#txt_InvoiceDate").datepicker({ minDate: "+1D", maxDate: "+90D" }).keydown(function (e) { e.keyWhich = 0; e.keyCode = 0; e.preventDefault(); });
        $('input:checkbox').on('change', function () {
            Chk_EnableDisable();
        });
        $('input[type=text]').keypress(function (evt) {
            var id = $(this).attr('id');
            if (id.indexOf("_Spec") == -1 && id.indexOf("_Judge") == -1) {
                if (id.indexOf("RimWidth") > 0 || id.indexOf("OuterDiameter") > 0 || id.indexOf("TyreWidth") > 0 || id.indexOf("InnerDiameter") > 0 || id.indexOf("Hardness") > 0) {
                    var charCode = (evt.which) ? evt.which : event.keyCode;
                    if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57))
                        return false;
                    else
                        return true;
                }
            }
        });
        $('#btn_InspectBarcode').click(function () {
            $('#lbl_ErrMsg').html();
            var checkedCount = $('input:checkbox[id*="chk_BarcodeSel"]:checked').length;
            if (checkedCount > 0) {
                Chk_EnableDisable();
                return true;
            }
            else {
                $('#lbl_ErrMsg').html('Choose Atleast one Stencil No');
                return false;
            }
        });
        $('#btn_Reset').click(function () {
            $('input:checkbox[id*="chk_BarcodeSel"]:checked').prop('checked', false);
            $('input:checkbox[id*="chk_BarcodeSel"]:not(:checked)').prop('disabled', false);
            return false;
        });
        $('#btn_SaveRecords').click(function () {
            $('#lbl_ErrMsg1').html();
            var ErrMsg = "";
            $('#tbl_Inspection tr').find('input[type=text]').each(function () {
                if ($(this).val() == "")
                    ErrMsg += "Enter " + $(this).attr('id').substring(4, $(this).attr('id').length) + "<br/>";
            });
            if (ErrMsg.length > 0) {
                $('#lbl_ErrMsg1').html(ErrMsg);
                return false;
            }
            else
                return true;
        });
        $('#btnGenerateRpt').click(function () {
            $('#lbl_ErrMsg2').html();
            var ErrMsg = "";
            $('#tbl_GenerateRpt tr').find('input[type=text]').each(function () {
                if ($(this).val() == "") {
                    var name = $(this).attr('id').substring(4, $(this).attr('id').length);
                    ErrMsg += "Enter " + name + "<br/>";
                }
            });
            if (ErrMsg.length > 0) {
                $('#lbl_ErrMsg2').html(ErrMsg);
                return false;
            }
            else
                return true;
        });
        function Chk_EnableDisable() {
            var checkedCount = $('input:checkbox[id*="chk_BarcodeSel"]:checked').length;
            if (checkedCount >= 5)
                $('input:checkbox[id*="chk_BarcodeSel"]:not(:checked)').prop('disabled', true);
            else
                $('input:checkbox[id*="chk_BarcodeSel"]:not(:checked)').prop('disabled', false);
        }
    </script>
</asp:Content>
