<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="expppc_stencilapproval.aspx.cs" Inherits="TTS.expppc_stencilapproval" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .tableCss
        {
            background-color: #dcecfb;
            width: 100%;
            border-color: White;
            border-collapse: separate;
        }
        .tableCss th
        {
            color: #008000;
            font-weight: normal;
            text-align: center;
        }
        .tableCss td
        {
            font-weight: 500;
            text-align: left;
        }
    </style>
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
        <table cellspacing="0" rules="all" border="1" style="background-color: #dcecfb; width: 100%;
            border-color: White; border-collapse: separate;">
            <tr>
                <td>
                    <asp:GridView runat="server" ID="gv_StencilAssignOrder" AutoGenerateColumns="false"
                        Width="100%">
                        <Columns>
                        </Columns>
                    </asp:GridView>
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
                    <asp:GridView ID="gv_EarmarkedItems" Width="100%" runat="server" AutoGenerateColumns="false"
                        RowStyle-Height="22px" ShowFooter="true" FooterStyle-Font-Bold="true" HeaderStyle-BackColor="#0688f9"
                        FooterStyle-HorizontalAlign="Right" FooterStyle-BackColor="#dfe0f3">
                        <HeaderStyle BackColor="#74bbf9" ForeColor="White" Font-Bold="true" Height="25px"
                            HorizontalAlign="Center" />
                        <Columns>
                            <asp:TemplateField HeaderText="CATEGORY" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <%# Eval("category") %>
                                    <asp:Label runat="server" ID="lblAssyStatus" Text='<%# Eval("AssyRimstatus").ToString() == "True" ? " (ASSY)" : ""%>'></asp:Label>
                                    <asp:HiddenField ID="hdnProcessID" runat="server" Value='<%# Eval("processid") %>' />
                                    <asp:HiddenField ID="hdnAssyRimstatus" runat="server" Value='<%# Eval("AssyRimstatus") %>' />
                                    <asp:HiddenField ID="hdnOrder_ItemID" runat="server" Value='<%# Eval("O_ItemID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="PLATFORM" DataField="config" ItemStyle-Width="100px" />
                            <asp:BoundField HeaderText="TYRE SIZE" DataField="tyresize" ItemStyle-Width="150px" />
                            <asp:BoundField HeaderText="RIM" DataField="rimsize" ItemStyle-Width="40px" />
                            <asp:BoundField HeaderText="TYPE" DataField="tyretype" ItemStyle-Width="60px" />
                            <asp:BoundField HeaderText="BRAND" DataField="brand" ItemStyle-Width="100px" />
                            <asp:BoundField HeaderText="SIDEWALL" DataField="sidewall" ItemStyle-Width="100px" />
                            <asp:BoundField HeaderText="QTY" DataField="itemqty" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Width="40px" />
                            <asp:TemplateField HeaderText="RIM QTY" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="40px">
                                <ItemTemplate>
                                    <%# Eval("Rimitemqty").ToString() == "0" ? "" : Eval("Rimitemqty").ToString()%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="EARMARK QTY" DataField="totEarmarkqty" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Width="40px" />
                            <asp:BoundField HeaderText="QC REJECT QTY" DataField="rejectqty" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Width="40px" />
                            <asp:TemplateField ItemStyle-Width="50px" HeaderText="ACTION" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEarmarkSencil" runat="server" Text="VIEW" ClientIDMode="Static"
                                        OnClick="lnkEarmarkSencil_Click" Font-Bold="true" Visible='<%# Eval("totEarmarkqty").ToString() == "0" ? false : true %>' /></span>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label runat="server" ID="lblText" ClientIDMode="Static" Text="" Font-Bold="true"
                        Font-Size="14px"></asp:Label>
                    <asp:LinkButton runat="server" ID="lnkEarmarkExcel" ClientIDMode="Static" Text=""
                        OnClick="lnkEarmarkExcel_Click" Font-Bold="true" Font-Size="14px"></asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div id="div_StockCtrls" style="display: none;">
                        <table cellspacing="0" rules="all" border="1" style="background-color: #dcecfb; width: 100%;
                            border-color: White; border-collapse: separate;">
                            <tr>
                                <td colspan="2">
                                    <asp:GridView runat="server" ID="gv_EarmarkedStock" AutoGenerateColumns="false" Width="100%"
                                        OnDataBound="gv_EarmarkedStock_OnDataBound">
                                        <Columns>
                                            <asp:BoundField HeaderText="PLATFORM" DataField="config" />
                                            <asp:BoundField HeaderText="TYRE SIZE" DataField="tyresize" />
                                            <asp:BoundField HeaderText="RIM" DataField="rimsize" />
                                            <asp:BoundField HeaderText="TYPE" DataField="tyretype" />
                                            <asp:BoundField HeaderText="BRAND" DataField="brand" />
                                            <asp:BoundField HeaderText="SIDEWALL" DataField="sidewall" />
                                            <asp:BoundField HeaderText="PLANT" DataField="Plant" />
                                            <asp:BoundField HeaderText="PROCESS-ID" DataField="processid" />
                                            <asp:BoundField HeaderText="STENCIL NO" DataField="stencilno" />
                                            <asp:BoundField HeaderText="GRADE" DataField="grade" />
                                            <asp:BoundField HeaderText="YOM" DataField="yearofmanufacture" />
                                            <asp:BoundField HeaderText="LOCATION" DataField="warehouse_location" />
                                            <asp:BoundField HeaderText="PART" DataField="EarmarkPart" />
                                            <asp:TemplateField HeaderText="STATUS">
                                                <ItemTemplate>
                                                    <asp:RadioButtonList ID="rdoSelect" runat="Server" onclick="cknunck(this)">
                                                        <asp:ListItem Text="ACCEPT" Value="ACCEPT">                           
                                                        </asp:ListItem>
                                                        <asp:ListItem Text="REJECT" Value="REJECT"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="COMMENTS">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtCmt" runat="server" Text='<%# Eval("Earmark_PpcStatus") %>' CssClass="form-control"
                                                        Width="200px" MaxLength="50" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnClear" ClientIDMode="Static" runat="server" Text="CLEAR CHECKED"
                                        OnClick="btnClear_Click" CssClass="btn btn-warning" />
                                </td>
                                <td>
                                    <asp:Button ID="btnSave" runat="server" Text="SAVE STENCIL STATUS" ClientIDMode="Static"
                                        CssClass="btn btn-success" OnClientClick="javascript:return CtrlRejectSave();"
                                        OnClick="btnSave_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="line-height: 40px;">
                                    <asp:Label runat="server" ID="lblStatusMsg" ClientIDMode="Static" Text="" Font-Bold="true"
                                        Font-Size="15px" ForeColor="#e21f3a"></asp:Label>
                                    <asp:TextBox runat="server" ID="txtEarmarkVerifyCommetns" ClientIDMode="Static" Text=""
                                        Enabled="true" TextMode="MultiLine" Width="90%" Height="80px" onKeyUp="javascript:CheckMaxLength(this, 999);"
                                        onChange="javascript:CheckMaxLength(this, 999);" CssClass="form-control"></asp:TextBox>
                                    <asp:Button runat="server" ID="btnMoveToRfdStatus" ClientIDMode="Static" OnClick="btnMoveToRfdStatus_Click"
                                        Text="STENCIL ASSIGN COMPLETED. ORDER MOVE TO NEXT PROCESS" CssClass="btn btn-success" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnCustCode" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnSelectProcessID" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnOrderStatus" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnO_ItemID" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        $(function () {
            $('#btnSave').css({ 'display': 'none' });
            $('#btnClear').css({ 'display': 'none' });
        });
        function bind_existStatusVal() {
            $('input[type=radio]:checked').each(function (elem) {
                if ($(this).val() == 'REJECT')
                    $('#MainContent_gv_EarmarkedStock_txtCmt_' + elem).css({ 'display': 'block' });
                else
                    $('#MainContent_gv_EarmarkedStock_txtCmt_' + elem).css({ 'display': 'none' });
            });
        }
        function cknunck(elem) {
            var value = $(elem).find('input[type=radio]:checked').val();
            var $this = $(elem);
            var cellIndex = $this.index();
            if (value == "REJECT") {
                $('#btnSave').css({ 'display': 'block' });
                $('#btnClear').css({ 'display': 'block' });
                $this.closest('td').next().children().eq(cellIndex).css({ 'display': 'block' });
                $this.closest('td').next().children().eq(cellIndex).focus();
            }
            else
                $this.closest('td').next().children().eq(cellIndex).css({ 'display': 'none' }).val('');
        }
        function CtrlRejectSave() {
            var isValid = true;
            $('input[type="text"]').each(function () {
                if ($(this).is(":visible")) {
                    if ($.trim($(this).val()) == '') {
                        isValid = false;
                        $(this).css({ "border": "1px solid red", "background": "#FFCECE" });
                    }
                    else
                        $(this).css({ "border": "", "background": "" });
                }
            });
            if (isValid == false) {
                alert('Enter the comments in red colored box');
                return false;
            }
            else
                return true;
        }
        function gotoPreviewDiv(ctrlID) {
            $('#' + ctrlID).css({ 'display': 'block' });
            $("html, body").stop().animate({ scrollLeft: $('#' + ctrlID).offset().left, scrollTop: $('#' + ctrlID).offset().top }, 1200);
        }
        function cntrlSave() {
            if ($("[id*=gv_EarmarkedStock_chk_selectQty_]:checked").length != 0)
                return true;
            else {
                alert("Choose atleast one quantity");
                return false;
            }
        }
    </script>
</asp:Content>
