<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="ClaimFreeReplacement.aspx.cs" Inherits="TTS.ClaimFreeReplacement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .tableReq
        {
            border-collapse: collapse;
            border-color: #000;
            width: 1064px;
            margin-top: 5px;
        }
        .tableReq th
        {
            background-color: #E2F2FF;
            text-align: left;
            padding-left: 10px;
            width: 135px;
            font-weight: bold;
        }
        .tableReq input[type="text"]
        {
            background-color: #fff;
            color: #000;
            border: 1px solid #000;
            margin-left: 10px;
        }
        .tableReq input[type="text"]:hover, .tableReq input[type="text"]:focus
        {
            background-color: #000;
            color: #fff;
            border: 1px solid #000;
            margin-left: 10px;
        }
    </style>
    <link href="Styles/datemonthyear.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div align="center" class="pageTitleHead">
        CLAIM FREE REPLACEMENT
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <div style="padding-left: 5px;">
            <table style="width: 1070px;">
                <tr>
                    <td align="center">
                        <asp:GridView runat="server" ID="gvClaimTrackList" AutoGenerateColumns="false" Width="1065px"
                            RowStyle-Height="22px">
                            <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" Height="22px" />
                            <Columns>
                                <asp:BoundField DataField="custname" HeaderText="CUSTOMER NAME" ItemStyle-Width="200px" />
                                <asp:BoundField DataField="complaintno" HeaderText="COMPLAINT NO." ItemStyle-Width="80px" />
                                <asp:BoundField DataField="complaintdate" HeaderText="COMPLAINT DATE" ItemStyle-Width="80px" />
                                <asp:BoundField DataField="plant" HeaderText="PLANT" ItemStyle-Width="80px" />
                                <asp:BoundField DataField="Qty" HeaderText="QTY" ItemStyle-Width="60px" />
                                <asp:BoundField DataField="claimstatus" HeaderText="STATUS" ItemStyle-Width="200px" />
                                <asp:TemplateField HeaderText="ACTION" ItemStyle-Width="50px">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkClaimNo" runat="server" Text="Show List" OnClick="lnkClaimNo_Click" /></span>
                                        <asp:HiddenField runat="server" ID="hdnClaimCustCode" Value='<%# Eval("custcode") %>' />
                                        <asp:HiddenField runat="server" ID="hdncreditnote" Value='<%# Eval("CreditNoteNo") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div id="divcancel" style="display: none; width: 1065px; float: left;">
                            <table cellspacing='0' rules='all' border='1' class="tableReq">
                                <tr>
                                    <td colspan="2">
                                        <asp:Label runat="server" ID="lblClaimCustName" ClientIDMode="Static" Text=""></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label runat="server" ID="lblClaimNo" ClientIDMode="Static" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:GridView runat="server" ID="gvClaimApproveItems" AutoGenerateColumns="false"
                                            Width="1064px" RowStyle-Height="22px">
                                            <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" Height="22px" HorizontalAlign="Center" />
                                            <Columns>
                                                <asp:BoundField DataField="config" HeaderText="PLATFORM" ItemStyle-Width="100px" />
                                                <asp:BoundField DataField="CrmType" HeaderText="TYPE" ItemStyle-Width="80px" />
                                                <asp:BoundField DataField="brand" HeaderText="BRAND" ItemStyle-Width="100px" />
                                                <asp:BoundField DataField="tyresize" HeaderText="TYRE SIZE" ItemStyle-Width="150px" />
                                                <asp:BoundField DataField="stencilno" HeaderText="STENCIL" ItemStyle-Width="80px" />
                                                <asp:BoundField DataField="assigntoqc" HeaderText="PLANT" ItemStyle-Width="80px" />
                                                <asp:TemplateField HeaderText="STATUS" ItemStyle-Width="60px">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblStencilCrmStatus" Text='<%# Eval("StencilCrmStatus") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ClaimDescription" HeaderText="COMPLAINT DESC" ItemStyle-Width="60px" />
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        DC NO.
                                    </th>
                                    <td>
                                        <asp:TextBox ID="txtDcNo" ClientIDMode="Static" runat="server" Text=""></asp:TextBox>
                                    </td>
                                    <th>
                                        QTY
                                    </th>
                                    <td>
                                        <asp:TextBox ID="txtQty" ClientIDMode="Static" runat="server" Text="" onkeypress="return isNumberWithoutDecimal(event)"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        FORM JJ NO.
                                    </th>
                                    <td>
                                        <asp:TextBox ID="txtJJNo" ClientIDMode="Static" runat="server" Text=""></asp:TextBox>
                                    </td>
                                    <th>
                                        LR NO.
                                    </th>
                                    <td>
                                        <asp:TextBox ID="txtLrno" ClientIDMode="Static" runat="server" Text=""></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        NAME OF TRANSPORT
                                    </th>
                                    <td>
                                        <asp:TextBox ID="txttransport" ClientIDMode="Static" runat="server" Text="" Width="300px"></asp:TextBox>
                                    </td>
                                    <th>
                                        LR DATE
                                    </th>
                                    <td>
                                        <asp:TextBox ID="txtLrDate" ClientIDMode="Static" runat="server" Text=""></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:GridView runat="server" ID="gvReplaceStencil" AutoGenerateColumns="false" Width="100%">
                                            <Columns>
                                                <asp:BoundField DataField="config" HeaderText="PLATFORM" ItemStyle-Width="" />
                                                <asp:BoundField DataField="tyresize" HeaderText="TYRE SIZE" ItemStyle-Width="" />
                                                <asp:BoundField DataField="rimsize" HeaderText="RIM" ItemStyle-Width="" />
                                                <asp:BoundField DataField="tyretype" HeaderText="TYPE" ItemStyle-Width="" />
                                                <asp:BoundField DataField="brand" HeaderText="BRAND" ItemStyle-Width="" />
                                                <asp:BoundField DataField="sidewall" HeaderText="SIDEWALL" ItemStyle-Width="" />
                                                <asp:BoundField DataField="stencilno" HeaderText="STENCIL" ItemStyle-Width="" />
                                                <asp:BoundField DataField="barcode" HeaderText="BARCODE" ItemStyle-Width="" />
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label>
                                    </td>
                                    <td colspan="2" style="text-align: center;">
                                        <asp:Button ID="btnSave" ClientIDMode="Static" runat="server" Text="SAVE" CssClass="btncalc"
                                            OnClick="btnSave_Click" OnClientClick="javascript:return CtrlSave();" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <asp:HiddenField runat="server" ID="hdnCustCode" ClientIDMode="Static" Value="" />
        <asp:HiddenField runat="server" ID="hdnplant" ClientIDMode="Static" Value="" />
        <asp:HiddenField runat="server" ID="hdnCreditNote" ClientIDMode="Static" Value="" />
    </div>
    <script src="Scripts/datemonthyear.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#txtLrDate").datepicker({ minDate: "-15D", maxDate: "+0D" }).keydown(function (e) { e.keyWhich = 0; e.keyCode = 0; e.preventDefault(); });
        });
        function CtrlSave() {
            var errMsg = ''; $('#lblErrMsg').html('');
            var count = 0;
            $('#MainContent_gvClaimApproveItems  tr').each(function (e) {
                var status = $('#MainContent_gvClaimApproveItems_lblStencilCrmStatus_' + e).html();
                if (status == "ACCEPT")
                    count++;
            });
            if ($('#txtDcNo').val().length == 0 && $('#txtJJNo').val().length == 0)
                errMsg += 'Enter DC No./ FORM JJ No.<br/>';
            if ($('#txttransport').val().length == 0)
                errMsg += 'Enter name of transport<br/>';
            if ($('#txtQty').val().length == 0)
                errMsg += 'Enter Qty.<br/>';
            else if ($('#txtQty').val() != count)
                errMsg += 'Enter the correct qty<br/>';
            if ($('#txtLrno').val().length == 0)
                errMsg += 'Enter LR no.<br/>';
            if ($('#txtLrDate').val().length == 0)
                errMsg += 'Enter LR date<br/>';
            if (errMsg.length > 0) {
                $('#lblErrMsg').html(errMsg);
                gotoClaimDiv('lblErrMsg');
                return false;
            }
            else
                return true;
        }
        function displayblock(ctrlID) { $('#' + ctrlID).css({ 'display': 'block' }); gotoClaimDiv('lblErrMsg'); }
    </script>
</asp:Content>
