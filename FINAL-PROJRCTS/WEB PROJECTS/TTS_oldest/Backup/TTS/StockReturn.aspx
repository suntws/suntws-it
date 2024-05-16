<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StockReturn.aspx.cs" MasterPageFile="~/master.Master"
    Inherits="TTS.StockReturn" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
    <link href="Styles/datemonthyear.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: center;" class="pageTitleHead">
        <asp:Label runat="server" ID="lblPageHead" ClientIDMode="Static" Text=""></asp:Label>
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <asp:ScriptManager runat="server" ID="ScriptManager1">
    </asp:ScriptManager>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="background-color: #E2F5E1; width: 100%;
            border-color: #E2F5E1; border-collapse: collapse;">
            <tr>
                <th>
                    YEAR
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="ddlDispatchYear" ClientIDMode="Static" CssClass="form-control"
                        Width="180px" AutoPostBack="true" OnSelectedIndexChanged="ddlDispatchYear_IndexChanged">
                    </asp:DropDownList>
                </td>
                <th>
                    MONTH
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="ddlDispatchMonth" ClientIDMode="Static" CssClass="form-control"
                        Width="180px" AutoPostBack="true" OnSelectedIndexChanged="ddlDispatchMonth_IndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    CUSTOMER
                </th>
                <td>
                    <asp:DropDownList ID="ddl_CustomerName" Width="450" runat="server" ClientIDMode="Static"
                        CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddl_CustomerName_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <th>
                    USER ID
                </th>
                <td colspan="3">
                    <asp:DropDownList ID="ddl_UserID" Width="180px" runat="server" ClientIDMode="Static"
                        CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddl_UserID_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    ORDER REF NO
                </th>
                <td>
                    <asp:DropDownList ID="ddl_OrderRefNo" runat="server" Width="240px" ClientIDMode="Static"
                        CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddl_OrderRefNo_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <th>
                    INVOICE NO
                </th>
                <td>
                    <asp:DropDownList ID="ddl_InvoiceNo" runat="server" Width="180px" ClientIDMode="Static"
                        CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddl_InvoiceNo_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    RETURN QTY
                </th>
                <td>
                    <asp:TextBox ID="txt_ReturnQty" runat="server" Width="180px" ClientIDMode="Static"
                        CssClass="form-control" onkeypress="return isNumberWithoutDecimal(event)" MaxLength="4"></asp:TextBox>
                </td>
                <th>
                    CUSTOMER DC NO
                </th>
                <td>
                    <asp:TextBox ID="txt_DcNO" runat="server" Width="180px" ClientIDMode="Static" PlaceHolder="Enter DCNO"
                        CssClass="form-control"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    DATE OF DC NO
                </th>
                <td>
                    <asp:TextBox ID="txt_DcnoDate" ClientIDMode="Static" runat="server" Width="180px"
                        CssClass="form-control" Height="30px" ToolTip="Select Desired Shipping Date"></asp:TextBox>
                </td>
                <th>
                    DATE OF RECEIVED
                </th>
                <td>
                    <asp:TextBox ID="txt_DateofReceipt" ClientIDMode="Static" runat="server" Width="180px"
                        CssClass="form-control" Height="30px" ToolTip="DATE OF DCNO"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    REMARKS
                </th>
                <td>
                    <asp:TextBox ID="txt_Remarks" runat="server" TextMode="MultiLine" Height="60px" Width="380px"
                        PlaceHolder="ENTER REMARKS" CssClass="form-control" MaxLength="100" ClientIDMode="Static"></asp:TextBox>
                </td>
                <th>
                    RETURN FOR REASON
                </th>
                <td>
                    <asp:TextBox ID="txt_ReturnforReason" runat="server" TextMode="MultiLine" Height="60px"
                        Width="350px" PlaceHolder="ENTER REASON" CssClass="form-control" MaxLength="200"
                        ClientIDMode="Static"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:GridView runat="server" ID="gvStockDetails" AutoGenerateColumns="false" CssClass="gridcss"
                        Width="100%" ClientIDMode="Static">
                        <RowStyle HorizontalAlign="Center" />
                        <HeaderStyle ForeColor="Black" Font-Bold="true" Height="22px" HorizontalAlign="Center" />
                        <Columns>
                            <asp:BoundField HeaderText="PLATFORM" DataField="config" ItemStyle-Width="60px" />
                            <asp:BoundField HeaderText="TYRE SIZE" DataField="tyresize" ItemStyle-Width="100px" />
                            <asp:BoundField HeaderText="RIM" DataField="rimsize" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField HeaderText="TYPE" DataField="tyretype" ItemStyle-Width="80px" />
                            <asp:BoundField HeaderText="BRAND" DataField="brand" ItemStyle-Width="60px" />
                            <asp:BoundField HeaderText="SIDEWALL" DataField="sidewall" ItemStyle-Width="60px" />
                            <asp:BoundField HeaderText="PROCESS-ID" DataField="ProcessId" ItemStyle-Width="80px" />
                            <asp:BoundField HeaderText="STENCIL NO" DataField="stencilno" ItemStyle-Width="100px" />
                            <asp:BoundField HeaderText="GRADE" DataField="grade" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField HeaderText="BARCODE" DataField="barcode" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center" />
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chk_All" runat="server" ClientIDMode="Static" Text="All" onclick="SelectAll(this)" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chk_item" runat="server" />
                                    <asp:HiddenField ID="hdn_Barcode" ClientIDMode="Static" runat="server" Value='<%# Eval("barcode") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <th>
                    STOCK INVENTORY TO
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="ddlInventoryPlant" ClientIDMode="Static" CssClass="form-control"
                        Width="100px">
                        <asp:ListItem Text="MMN" Value="MMN"></asp:ListItem>
                        <asp:ListItem Text="PDK" Value="PDK"></asp:ListItem>
                        <asp:ListItem Text="SLTL" Value="SLTL"></asp:ListItem>
                        <asp:ListItem Text="SITL" Value="SITL"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="btnSaveRecords" runat="server" Text="Return Records" ClientIDMode="Static"
                        Style="float: right;" CssClass="btn btn-success" OnClientClick="javascript:return cntrlSave();"
                        OnClick="btnSaveRecords_Click" />
                </td>
                <td>
                    <asp:Button ID="btnClearRecords" runat="server" Text="Clear Selection" ClientIDMode="Static"
                        CssClass="btn btn-info" OnClientClick="javascript:return ctrlClear();" />
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField ID="hdnSelectedBarcode" runat="server" ClientIDMode="Static" />
    <script src="Scripts/datemonthyear.js" type="text/javascript"></script>
    <script type="text/javascript">
        function SelectAll(evnt) {
            if ($('#txt_ReturnQty').val() == 0 || $('#txt_ReturnQty').val().length == 0) {
                alert('Enter distpch return qty');
                $(evnt).prop('checked', false)
                $('#txt_ReturnQty').focus();
                return false;
            }
            else {
                if (evnt.checked) {
                    $("input:checkbox[id='chk_item']").prop('checked', true).css({ 'background-color': '#4293da' });
                    $("#txt_ReturnQty").val($('#gvStockDetails tr').filter(':has(:checkbox:checked)').length - 1);
                }
                else {
                    $("input:checkbox[id='chk_item']").prop('checked', false).css({ 'background-color': '#ffffff' });
                    $("#txt_ReturnQty").val('0');
                }
            }
        }

        function cntrlSave() {
            var ErrMsg = "";
            if ($('#ddlDispatchYear').val() == "CHOOSE")
                ErrMsg += "Select Dispatch Year \n";
            if ($('#ddlDispatchMonth').val() == "CHOOSE")
                ErrMsg += "Select Dispatch Month \n";
            if ($('#ddl_CustomerName').val() == "CHOOSE")
                ErrMsg += "Enter CustomerName \n";
            if ($('#ddl_UserID').val() == "CHOOSE")
                ErrMsg += "Enter UserID \n";
            if ($('#ddl_OrderRefNo').val() == "CHOOSE")
                ErrMsg += "Enter Order Ref No \n";
            if ($('#ddl_InvoiceNo').val() == "CHOOSE")
                ErrMsg += "Enter Invoice No \n";
            if ($('#txt_DcNO').val() == "")
                ErrMsg += "Enter Dcno \n";
            if ($('#txt_DcnoDate').val() == "")
                ErrMsg += "Enter Dcno Date \n";
            if ($('#txt_DateofReceipt').val() == "")
                ErrMsg += "Enter Received Date \n";
            if ($('#txt_ReturnQty').val() == "")
                ErrMsg += "Enter Return Qty \n";
            else if ($("input:checkbox[id='chk_item']:checked").length == 0)
                ErrMsg += "Choose atleast one item \n";
            else if ($("input:checkbox[id='chk_item']:checked").length != parseInt($('#txt_ReturnQty').val()))
                ErrMsg += "Choosen Qty and Return Qty must be equal \n";
            if ($('#txt_Remarks').val() == "")
                ErrMsg += "Enter Remarks \n";
            if ($('#txt_ReturnforReason').val() == "")
                ErrMsg += "Enter Return for Reason \n";
            if (ErrMsg.length > 0) {
                alert(ErrMsg);
                return false;
            }
            else {
                var barcode = "";
                $('#gvStockDetails tr').filter(':has(:checkbox:checked)').each(function () {
                    barcode += "'" + $(this).find('#hdn_Barcode').val() + "',";
                });
                $('#hdnSelectedBarcode').val(barcode);
                return true;
            }
        }

        function ctrlClear() { window.location.href = window.location.href; return false; }

        $(function () {
            $("#txt_DateofReceipt").datepicker({ minDate: "-90D", maxDate: "+0D" }).keydown(function (e) { e.keyWhich = 0; e.keyCode = 0; e.preventDefault(); });
            $("#txt_DcnoDate").datepicker({ minDate: "-90D", maxDate: "+0D" }).keydown(function (e) { e.keyWhich = 0; e.keyCode = 0; e.preventDefault(); });
        });
    </script>
</asp:Content>
