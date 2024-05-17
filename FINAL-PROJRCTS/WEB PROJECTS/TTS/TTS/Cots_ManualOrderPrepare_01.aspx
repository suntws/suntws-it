<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Cots_ManualOrderPrepare_01.aspx.cs"
    MasterPageFile="~/master.Master" Inherits="TTS.Cots_ManualOrderPrepare_01" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/cotsScript.js" type="text/javascript"></script>
    <style type="text/css">
        .btn-sm
        {
            padding: 5px 10px;
            font-size: 12px;
            line-height: 1.5;
            border-radius: 3px;
        }
        .tableCss
        {
            width: 100%;
            background-color: #dcecfb;
            border-color: White;
            border-collapse: separate;
        }
        .tableCss th
        {
            font-weight: bold;
            text-align: center;
            color: #ffffff;
            background-color: #2abac1;
        }
        .tableCss td
        {
            font-weight: 500;
            text-align: left;
        }
        .white_content
        {
            display: block;
            position: fixed;
            width: 7%;
            height: 6%;
            padding: 5px;
            border: 3px solid #3ad1d8;
            background-color: #000000;
            right: 5px;
            top: 5px;
            color: #ffffff;
            font-size: 18px;
            font-weight: bold;
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        ORDER PREPARE -> CHOOSE PREVIOUS ITEMS</div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div class="white_content">
        Timeout<br />
        <asp:Label ID="lblSessionMinutes" runat="server"></asp:Label>
        :
        <asp:Label ID="lblSessionSecond" runat="server"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table class="tableCss" style="width: 100%;" border="1">
            <tr>
                <td>
                    <asp:FormView ID="frmOrderMasterDetails_Dom" runat="server" Width="100%">
                        <ItemTemplate>
                            <table cellspacing="0" rules="all" border="1" class="tableCss">
                                <tr>
                                    <th>
                                        Customer
                                    </th>
                                    <td colspan="3">
                                        <asp:Label runat="server" ID="lblCustomerName" ClientIDMode="Static" Text='<%# Eval("custfullname")%>'></asp:Label>
                                        <asp:HiddenField runat="server" ID="hdncustcodeStd" ClientIDMode="Static" Value='<%# Eval("custcodeStd") %>' />
                                    </td>
                                    <th>
                                        Order Ref No.
                                    </th>
                                    <td colspan="3">
                                        <%# Eval("OrderRefNo")%>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        Order Created
                                    </th>
                                    <td>
                                        <asp:Label runat="server" ID="lblOrderDate" ClientIDMode="Static" Text='<%# Eval("CreatedDate")%>'></asp:Label>
                                    </td>
                                    <th>
                                        Desired ShipDate
                                    </th>
                                    <td>
                                        <%# Eval("DesiredShipDate")%>
                                    </td>
                                    <th>
                                        Delivery Method
                                    </th>
                                    <td colspan="3">
                                        <%# Eval("DeliveryMethod")%>
                                        <%# Eval("GodownName").ToString() != "" ? (" - "+Eval("GodownName").ToString()):"" %>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        Expected Ship Date
                                    </th>
                                    <td>
                                        <%# Eval("ExpectedShipDate")%>
                                    </td>
                                    <th>
                                        Transport
                                    </th>
                                    <td>
                                        <%# Eval("TransportDetails")%>
                                    </td>
                                    <th>
                                        Freight Charges
                                    </th>
                                    <td>
                                        <%# Eval("FreightCharges")%>
                                    </td>
                                    <th>
                                        Packing Method
                                    </th>
                                    <td>
                                        <%# Eval("PackingMethod")%>
                                        <%# Eval("PackingOthers").ToString() !="" ? (" - " +Eval("PackingOthers").ToString()):"" %>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        Bill To
                                    </th>
                                    <td colspan="3">
                                        <%# Bind_BillingAddress(Eval("BillID").ToString(), false)%>
                                    </td>
                                    <th>
                                        Ship To
                                    </th>
                                    <td colspan="3">
                                        <%# Bind_BillingAddress(Eval("ShipID").ToString(), true)%>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        Special Instruction
                                    </th>
                                    <td colspan="3">
                                        <%# Eval("SplIns")%>
                                    </td>
                                    <th>
                                        Special Notes
                                    </th>
                                    <td colspan="3">
                                        <%# Eval("SpecialRequset")%>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:FormView>
                    <asp:FormView ID="frmOrderMasterDetails_Exp" runat="server" Width="100%">
                        <ItemTemplate>
                            <table cellspacing="0" rules="all" border="1" style="background-color: #dcecfb; width: 100%;
                                border-color: White; border-collapse: separate;">
                                <tr>
                                    <th class="spanCss">
                                        Customer
                                    </th>
                                    <td>
                                        <asp:Label runat="server" ID="lblCustomerName" ClientIDMode="Static" Text='<%# Eval("custfullname")%>'></asp:Label>
                                        <asp:HiddenField runat="server" ID="hdncustcodeStd" ClientIDMode="Static" Value='<%# Eval("custcodeStd") %>' />
                                    </td>
                                    <th class="spanCss">
                                        Order Ref No.
                                    </th>
                                    <td>
                                        <%# Eval("OrderRefNo")%>
                                    </td>
                                </tr>
                                <tr>
                                    <th class="spanCss">
                                        Bill To
                                    </th>
                                    <td>
                                        <%# Bind_BillingAddress(Eval("BillID").ToString(), false)%>
                                    </td>
                                    <th class="spanCss">
                                        Ship To
                                    </th>
                                    <td>
                                        <%# Bind_BillingAddress(Eval("ShipID").ToString(), true)%>
                                    </td>
                                </tr>
                                <tr>
                                    <th class="spanCss">
                                        Special Instruction
                                    </th>
                                    <td>
                                        <%# Eval("SplIns")%>
                                    </td>
                                    <th class="spanCss">
                                        Special Notes
                                    </th>
                                    <td>
                                        <%# Eval("SpecialRequset")%>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:FormView>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="div_Jathagam">
                        <asp:Label runat="server" ID="lblPreText" ClientIDMode="Static" Text="" CssClass="spanCss"
                            Font-Size="18px"></asp:Label>
                        <asp:GridView ID="gv_Jathagam" runat="server" AutoGenerateColumns="false" Width="100%"
                            CssClass="gridcss">
                            <Columns>
                                <asp:TemplateField HeaderText="CHOOSE" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk_jathagam" runat="server" />
                                        <asp:HiddenField ID="hdn_ProcessId" runat="server" Value='<%# Eval("ProcessID") %>' />
                                        <asp:HiddenField ID="hdn_FinishedWt" runat="server" Value='<%# Eval("FinishedWt") %>' />
                                        <asp:HiddenField ID="hdn_Discount" runat="server" Value='<%# Eval("MaxDiscount") %>' />
                                        <asp:HiddenField ID="hdn_PartNo" runat="server" Value='<%# Eval("ItemCode") %>' />
                                        <asp:HiddenField ID="hdn_Unitprice" runat="server" Value='<%# Eval("Unitprice") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="CATEGORY" DataField="category" />
                                <asp:BoundField HeaderText="PLATFORM" DataField="Config" />
                                <asp:BoundField HeaderText="BRAND" DataField="brand" />
                                <asp:BoundField HeaderText="SIDEWALL" DataField="sidewall" />
                                <asp:BoundField HeaderText="TYPE" DataField="tyretype" />
                                <asp:BoundField HeaderText="TYRE SIZE" DataField="tyresize" />
                                <asp:BoundField HeaderText="RIM" DataField="rimsize" />
                                <asp:BoundField HeaderText="PROCESS-ID" DataField="ProcessID" />
                                <asp:TemplateField HeaderText="ITEM CODE">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblPartNo" Text='<%# Eval("ItemCode") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lbl_ErrMsg" runat="server" ForeColor="Red" ClientIDMode="Static"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <table id="tbButtons" style="width: 100%; border-color: White; border-collapse: separate;">
                        <tr>
                            <td style="text-align: center;">
                                <asp:Button ID="btn_SelectItem" Text="Move Choosed Items" CssClass="btn btn-success btn-sm"
                                    runat="server" OnClick="btn_SelectItem_Click" ClientIDMode="Static" />
                            </td>
                            <td style="text-align: center;">
                                <asp:Button ID="btn_clearSelection" Text="Clear Selection" CssClass="btn btn-warning btn-sm"
                                    runat="server" ClientIDMode="Static" />
                            </td>
                            <td style="text-align: center;">
                                <asp:Button ID="btn_SkipItems" Text="Skip & Choose Other Items" CssClass="btn btn-info btn-sm"
                                    runat="server" ClientIDMode="Static" OnClick="btn_SkipItems_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lbl_ErrMsg1" runat="server" ForeColor="Red" ClientIDMode="Static"></asp:Label>
                    <div id="div_SelectedItems" style="width: 100%; display: none;">
                        <div style="text-align: right; padding-right: 10px;">
                            <asp:Button ID="btn_AddItem" runat="server" CssClass="btn btn-success btn-sm" Text="Add More Item"
                                ClientIDMode="Static" />
                        </div>
                        <asp:GridView ID="gv_SelectedItems" runat="server" AutoGenerateColumns="false" Width="100%"
                            CssClass="gridcss" OnRowDeleting="gv_SelectedItems_RowDeleting" OnDataBound="gvSelectedItems_DataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="CATEGORY">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lbl_Category" ClientIDMode="Static" Text='<%# Eval("category") %>'></asp:Label>
                                        <%# Eval("AssyRimstatus").ToString() == "True" ? " (ASSY)" : ""%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="PLATFORM" DataField="Config" />
                                <asp:BoundField HeaderText="BRAND" DataField="Brand" />
                                <asp:BoundField HeaderText="SIDEWALL" DataField="Sidewall" />
                                <asp:BoundField HeaderText="TYPE" DataField="TyreType" />
                                <asp:BoundField HeaderText="TYRE SIZE" DataField="TyreSize" />
                                <asp:BoundField HeaderText="RIM" DataField="RimSize" />
                                <asp:BoundField HeaderText="WT" DataField="FinishedWt" />
                                <asp:TemplateField HeaderText="QTY" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_ItemQty" runat="server" CssClass="form-control" Width="40px"
                                            onkeypress="return isNumberWithoutDecimal(event)" Text='<%# Eval("itemqty") %>'
                                            MaxLength="4"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="DISC %" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_Discount" runat="server" CssClass="form-control" Width="40px"
                                            MaxLength="5" Text='<%# Eval("Discount1") %>' onkeypress="return isNumberKey(event)"
                                            onblur="CalcForDiscount_Sheet_Basic(this)"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="LIST PRICE" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_SheetPrice" runat="server" CssClass="form-control" Width="60px"
                                            MaxLength="8" ReadOnly="true" Text='<%# Eval("SheetPrice") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="BASIC PRICE" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_BasicPrice" runat="server" CssClass="form-control" Width="60px"
                                            MaxLength="8" ReadOnly="true" Text='<%# Eval("unitprice") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="BASIC PRICE" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_ExpBasicPrice" runat="server" CssClass="form-control" Width="60px"
                                            MaxLength="8" onkeypress="return isNumberKey(event)" Text='<%# Eval("unitprice") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ASSY" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk_RimAssembly" runat="server" Checked='<%# Convert.ToBoolean(Eval("AssyRimstatus")) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="RIM WT" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_Rim_FinishedWt" runat="server" CssClass="form-control" Width="40px"
                                            MaxLength="8" onkeypress="return isNumberKey(event)" Style="display: none;" Text='<%# Eval("Rimfinishedwt") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="RIM PRICE" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_Rim_BasicPrice" runat="server" CssClass="form-control" Width="60px"
                                            MaxLength="8" onkeypress="return isNumberKey(event)" Style="display: none;" Text='<%# Eval("Rimunitprice") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="EDC NO / DESC" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:HiddenField runat="server" ID="hdnEdcNo" Value='<%# Eval("EdcNo") %>' />
                                        <asp:DropDownList runat="server" ID="ddl_Rim_EdcNo" CssClass="form-control" Width="140px"
                                            Style="display: none;">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txt_Rim_DrawingNo" runat="server" CssClass="form-control" Width="140px"
                                            MaxLength="50" Style="display: none;" Text='<%# Eval("RimDwg") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="INFO" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtAddInfo" runat="server" CssClass="form-control" Width="140px"
                                            MaxLength="50" Text='<%# Eval("AdditionalInfo") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ACTION" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Button ID="btn_DeleteItem" runat="server" CssClass="btn btn-danger btn-sm" Text="Delete"
                                            CommandName="Delete" OnClientClick="DeleteItem(this);" />
                                        <asp:HiddenField ID="hdn_ProcessId" runat="server" Value='<%# Eval("ProcessID") %>' />
                                        <asp:HiddenField ID="hdn_Discount" runat="server" Value='<%# Eval("Discount") %>' />
                                        <asp:HiddenField ID="hdn_PartNo" runat="server" Value='<%# Eval("ItemCode") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <hr />
                        <div id="div_SeletedCntrls" style="width: 100%; text-align: center;">
                            <asp:Button ID="btn_SaveItems" Text="Save Choosed Items" CssClass="btn btn-success btn-sm"
                                runat="server" ClientIDMode="Static" OnClick="btn_SaveItems_Click" />
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField ID="hdn_CustCode" runat="server" Value="" ClientIDMode="Static" />
    <script type="text/javascript">
        $(function () {
            if ($('#<%=gv_SelectedItems.ClientID  %> tr').length > 1) {
                $('#div_Jathagam *').prop('disabled', true).css({ 'cursor': 'no-drop' });
                $('#btn_SkipItems').prop('disabled', false).css({ 'cursor': 'pointer' });
            }
            $('#<%=gv_SelectedItems.ClientID  %> tr').find("input[id*='chk_RimAssembly']").each(function () {
                var row = $(this).parent().parent();
                if ($(this).is(':checked')) {
                    row.find("input[id*='txt_Rim_']").show();
                    row.find("select[id*='ddl_Rim_']").show();
                    $(this).css({ 'cursor': 'no-drop' });
                }
                else {
                    row.find("input[id*='txt_Rim_']").hide();
                    row.find("select[id*='ddl_Rim_']").hide();
                }
            });
            $("select[id*='MainContent_gv_SelectedItems_ddl_Rim_EdcNo_']").change(function () {
                var id1 = this.id;
                var txt1 = id1.replace('ddl_Rim_EdcNo', 'txt_Rim_FinishedWt');
                $('#' + txt1).val($('#' + id1).val());
            });
        });
        $('#btn_SelectItem').click(function () {
            $('#lbl_ErrMsg').html('');
            if ($("input[id*='chk_jathagam']:checked").length == 0) {
                $('#lbl_ErrMsg').html('Choose atleast one Item');
                return false;
            }
        });
        $('#btn_clearSelection').click(function () {
            $("input[id*='chk_jathagam']:enabled").prop('checked', false);
            $('#lbl_ErrMsg').html('');
            return false;
        });
        $('#<%=gv_SelectedItems.ClientID  %> tr').find("input[id*='chk_RimAssembly']").change(function () {
            var row = $(this).parent().parent();
            if ($(this).is(':checked')) {
                row.find("input[id*='txt_Rim_']").show();
                row.find("select[id*='ddl_Rim_']").show();
            }
            else {
                row.find("input[id*='txt_Rim_']").hide();
                row.find("select[id*='ddl_Rim_']").hide();
            }
        });
        $('#btn_AddItem').click(function () {
            $('#tbButtons').css({ 'display': 'block' });
            $('#div_Jathagam *').prop('disabled', false).css({ 'cursor': 'pointer' });
            $("input[id*='chk_jathagam']:checked").prop('disabled', true);
            return false;
        });
        $('#btn_SaveItems').click(function () {
            $('#lbl_ErrMsg1').html('');
            var count = 0;
            $('#<%=gv_SelectedItems.ClientID  %> tr').find("input[id*='txt_']").each(function () {
                var id = $(this).attr('id');
                if (id.indexOf("txt_Rim_") == -1) {
                    if (id.indexOf("Discount") == -1) {
                        if ($('#' + id).val().length == 0 || parseFloat($('#' + id).val()) == 0) {
                            $('#' + id).css({ 'background-color': '#e6a1a1' });
                            count++;
                        }
                        else
                            $('#' + id).addClass('form-control').css({ 'background-color': '' });
                    }
                }
            });
            $('#<%=gv_SelectedItems.ClientID  %> tr').find("input[id*='chk_RimAssembly']").each(function () {
                if ($(this).is(':checked')) {
                    var row = $(this).parent().parent();
                    $(row).find("input[id*='txt_Rim_']").each(function () {
                        var id = $(this).attr('id');
                        if (id.indexOf("DrawingNo") == -1) {
                            if ($('#' + id).val().length == 0 || parseFloat($('#' + id).val()) == 0) {
                                $('#' + id).css({ 'background-color': '#e6a1a1' });
                                count++;
                            }
                            else
                                $('#' + id).addClass('form-control').css({ 'background-color': '' });
                        }
                        else if (id.indexOf("DrawingNo") > 0) {
                            if ($('#' + id).val().length == 0) {
                                $('#' + id).css({ 'background-color': '#e6a1a1' });
                                count++;
                            }
                            else
                                $('#' + id).addClass('form-control').css({ 'background-color': '' });
                        }
                    });
                }
            });
            if (count > 0) {
                $('#lbl_ErrMsg1').html("Fill red colored textboxes with greater than 0");
                return false;
            }
            else
                return true;
        });
        function DeleteItem(event) {
            $('#div_Jathagam *').prop('disabled', false);
            return true;
        }
        //function to calc for Discount,SheetPrice,Basic
        function CalcForDiscount_Sheet_Basic(event) {
            var row = $(event).parent().parent();
            var txt_Discount = $(row).find("input[id*='txt_Discount']").attr('id');
            var hdn_Discount = $(row).find("input[id*='hdn_Discount']").attr('id');
            var txt_BasicPrice = $(row).find("input[id*='txt_BasicPrice']").attr('id');
            var txt_SheetPrice = $(row).find("input[id*='txt_SheetPrice']").attr('id');
            if (parseFloat($('#' + txt_Discount).val()) > parseFloat($('#' + hdn_Discount).val())) {
                alert('Your maximum discount limit is ' + $('#' + hdn_Discount).val());
                $('#' + txt_Discount).val($('#' + hdn_Discount).val());
            }

            if ($('#' + txt_Discount).val() == 'NaN') $('#' + txt_Discount).val('0');
            if ($('#' + txt_BasicPrice).val() == 'NaN') $('#' + txt_BasicPrice).val('0');
            if ($('#' + txt_SheetPrice).val() == 'NaN') $('#' + txt_SheetPrice).val('0');
            var custSheetPrice = parseFloat($('#' + txt_SheetPrice).val() != '' ? $('#' + txt_SheetPrice).val() : 0);
            var custBasicPrice = parseFloat($('#' + txt_BasicPrice).val != '' ? $('#' + txt_BasicPrice).val() : 0);
            var custDiscPer = parseFloat($('#' + txt_Discount).val != '' ? $('#' + txt_Discount).val() : 0);

            if (custDiscPer != 0 && custSheetPrice != 0)
                $('#' + txt_BasicPrice).val(Math.round(parseFloat((custSheetPrice - (custSheetPrice * (custDiscPer / 100))))).toFixed(2));
            else
                $('#' + txt_BasicPrice).val(Math.round(parseFloat((custSheetPrice))).toFixed(2));
        }

        function SessionExpireAlert(timeout) {
            var seconds = 59;
            var minutes = (timeout / 60000) - 1;
            document.getElementById("<%= lblSessionMinutes.ClientID %>").innerText = minutes;
            document.getElementById("<%= lblSessionSecond.ClientID %>").innerText = seconds
            setseconds = setInterval(function () {
                seconds--;
                if (seconds == -1)
                    seconds = 59;
                document.getElementById("<%= lblSessionSecond.ClientID %>").innerText = (seconds < 10 ? '0' + seconds.toString() : seconds.toString());
            }, 1000);
            setminutes = setInterval(function () {
                minutes--;
                document.getElementById("<%= lblSessionMinutes.ClientID %>").innerText = (minutes < 10 ? '0' + minutes.toString() : minutes.toString());
            }, 60000);
            setTimeout(function () { alert("session Expired!"); }, timeout - 0 * 1000);
        }
        function gotoPreviewDiv(ctrlID) {
            $('#' + ctrlID).css({ 'display': 'block' });
            $("html, body").stop().animate({
                scrollLeft: $('#' + ctrlID).offset().left,
                scrollTop: $('#' + ctrlID).offset().top
            }, 1200);
            BindItem_PriceCalc();
        }
        function BindItem_PriceCalc() {
            $("input:text[id*='MainContent_gv_SelectedItems_txt_SheetPrice_']").each(function (e) {
                $(this).focus();
            });
            $('#btn_SaveItems').focus();
        }
    </script>
</asp:Content>
