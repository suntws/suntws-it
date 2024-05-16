<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExpPPC_Earmark_Revoke.aspx.cs"
    MasterPageFile="~/master.Master" Inherits="TTS.ExpPPC_Earmark_Revoke" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
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
                    <asp:GridView ID="gv_EarmarkedOrders" Width="100%" runat="server" AutoGenerateColumns="false"
                        RowStyle-Height="30px">
                        <HeaderStyle BackColor="#74bbf9" ForeColor="White" Font-Bold="true" Height="25px"
                            HorizontalAlign="Center" />
                        <Columns>
                            <asp:TemplateField HeaderText="CUSTOMER NAME">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatusCustName" Text='<%# Eval("custfullname") %>'></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdnStatusCustCode" Value='<%# Eval("CustCode") %>' />
                                    <asp:HiddenField runat="server" ID="hdnContainerLoadFrom" Value='<%# Eval("ContainerLoadFrom") %>' />
                                    <asp:HiddenField runat="server" ID="hdnOrderID" Value='<%# Eval("ID") %>' />
                                    <asp:HiddenField runat="server" ID="hdnOrderStatus" Value='<%# Eval("OrderStatus") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ORDER REF NO">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOrderRefNo" Text='<%#Eval("OrderRefNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="WORK ORDER" DataField="workorderno" />
                            <asp:BoundField HeaderText="ORDER QTY" DataField="itemqty" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField HeaderText="ASSIGNED QTY" DataField="earmarkqty" ItemStyle-HorizontalAlign="Right" />
                            <asp:TemplateField HeaderText="ACTION" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEarmarkedOrderSelection" CssClass="btn btn-success" runat="server"
                                        OnClick="lnkEarmarkedOrderSelection_click" Text="Approve/ Revoke">
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="div_Sub_OrderItems" style="display: none;">
                        <div style="text-align: center; padding-left: 10px; padding-right: 10px; background-color: #3c763d;
                            overflow: hidden; height: 25px; font-size: 15px; color: #ffffff;">
                            <div style="float: left;">
                                <asp:Label ID="lblSelectedCustomerName" runat="server" Text="" Font-Bold="true"></asp:Label>
                            </div>
                            <div style="float: right;">
                                <asp:Label ID="lblSelectedOrderRefNo" ClientIDMode="Static" runat="server" Text=""
                                    Font-Bold="true"></asp:Label>
                            </div>
                        </div>
                        <div>
                            <asp:GridView ID="gv_EarmarkRevokeItems" Width="100%" runat="server" AutoGenerateColumns="false"
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
                                    <asp:BoundField HeaderText="PART-A (GSA EXACT MATCH)" DataField="PartA" ItemStyle-HorizontalAlign="Right"
                                        ItemStyle-Width="60px" />
                                    <asp:BoundField HeaderText="PART-B (GSA MATCH WITH REBRAND)" DataField="PartB" ItemStyle-HorizontalAlign="Right"
                                        ItemStyle-Width="60px" />
                                    <asp:BoundField HeaderText="PART-C (GSA UPGRADE WITH REBRAND)" DataField="PartC"
                                        ItemStyle-HorizontalAlign="Right" ItemStyle-Width="60px" />
                                    <asp:BoundField HeaderText="PART-D (CURRENT STOCK EXACT MATCH)" DataField="PartD"
                                        ItemStyle-HorizontalAlign="Right" ItemStyle-Width="60px" />
                                    <asp:BoundField HeaderText="PART-E (CURRENT STOCK MATCH WITH REBRAND)" DataField="PartE"
                                        ItemStyle-HorizontalAlign="Right" ItemStyle-Width="60px" />
                                    <asp:BoundField HeaderText="PART-F (CURRENT STOCK UPGRADE MATCH REBRAND)" DataField="PartF"
                                        ItemStyle-HorizontalAlign="Right" ItemStyle-Width="60px" />
                                    <asp:BoundField HeaderText="PART-G (PRODUCED FOR THIS WORK ORDER)" DataField="PartG"
                                        ItemStyle-HorizontalAlign="Right" ItemStyle-Width="60px" />
                                    <asp:TemplateField ItemStyle-Width="115px" HeaderText="REVOKE" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEarmarkRevokeItem" runat="server" Text="SHOW STENCIL" ClientIDMode="Static"
                                                OnClick="lnkEarmarkRevokeItem_Click" Font-Bold="true" Visible='<%# Eval("totEarmarkqty").ToString() == "0" ? false : true %>' /></span>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div id="div_StockCtrls" style="display: none; border: 1px solid #000; width: 100%;
                            float: left; margin-top: 5px; background-color: #e5ffc9; border-radius: 5px;">
                            <div style="width: 100%; float: left;">
                                <asp:GridView runat="server" ID="gv_EarmarkedStock" AutoGenerateColumns="false" Width="100%"
                                    CssClass="gridcss">
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
                                        <asp:BoundField HeaderText="ASSIGNED BY" DataField="EarmarkBy" />
                                        <asp:BoundField HeaderText="ASSIGNED ON" DataField="EarmarkOn" />
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox runat="server" ID="checkAllChk" ClientIDMode="Static" />
                                                ALL
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chk_selectQty" runat="server" Visible='<%# Convert.ToBoolean(Eval("ChkEnable")) %>' />
                                                <asp:HiddenField runat="server" ID="hdnO_ItemID" ClientIDMode="Static" Value='<%# Eval("O_I_ID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div style="width: 100%; float: left; text-align: left; margin-top: 5px; margin-bottom: 5px;">
                                <div style="width: 65%; float: left;">
                                    <span style="width: 140px; float: left; font-weight: bold; font-size: 13px; padding-top: 10px;">
                                        Comments for revoke:</span><asp:TextBox runat="server" ID="txtRevokeCommets" ClientIDMode="Static"
                                            Text="" MaxLength="50" CssClass="form-control" Width="565px"></asp:TextBox>
                                </div>
                                <div style="width: 35%; float: left; text-align: center;">
                                    <asp:Button ID="btnEarmarkRevoke" runat="server" Text="ASSIGNED STENCIL REVOKE" ClientIDMode="Static"
                                        CssClass="btn btn-success" OnClientClick="javascript:return cntrlSave();" OnClick="btnEarmarkRevoke_Click" />
                                    <asp:Button ID="btnClear" ClientIDMode="Static" runat="server" Text="CLEAR CHECKED"
                                        CssClass="btn btn-warning" />
                                </div>
                            </div>
                        </div>
                        <div id="div_earmarkcompleted" style="display: none; border: 1px solid #000; width: 99%;
                            float: left; margin-top: 5px; padding-bottom: 5px; background-color: #e8e7e7;
                            border-radius: 5px; padding-left: 10px; line-height: 35px;">
                            <span style="color: #e21f3a; font-weight: bold; font-size: 15px; margin-top: 5px;
                                width: 100%; float: left;">If stencil assign/revoke process completed for above
                                order, Kindly move for next process</span>
                            <asp:TextBox runat="server" ID="txtEarmarkCommetns" ClientIDMode="Static" Text=""
                                Enabled="true" TextMode="MultiLine" Width="90%" Height="80px" onKeyUp="javascript:CheckMaxLength(this, 999);"
                                onChange="javascript:CheckMaxLength(this, 999);" CssClass="form-control"></asp:TextBox>
                            <asp:Button runat="server" ID="btnMoveStatus" ClientIDMode="Static" OnClick="btnMoveStatus_Click"
                                Text="STENCIL ASSIGN COMPLETED. ORDER MOVE FOR NEXT PROCESS" CssClass="btn btn-success" />
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnCustCode" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnOID" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnStatusID" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        $(function () {
            $('#btnClear').click(function () { $('input:checkbox').prop('checked', false); return false; });
            $('#checkAllChk').click(function () {
                if ($("[id*=gv_EarmarkedStock_chk_selectQty_]:enabled").length > 0) {
                    if ($(this).attr('checked') == "checked")
                        $("[id*=gv_EarmarkedStock_chk_selectQty_]:enabled").attr('checked', true)
                    else
                        $("[id*=gv_EarmarkedStock_chk_selectQty_]:enabled").attr('checked', false)
                }
                else {
                    alert('No records');
                    $(this).attr('checked', false);
                }
            });
            $("[id*=gv_EarmarkedStock_chk_selectQty_]").click(function () { $('#checkAllChk').attr('checked', false); });
        });
        function gotoPreviewDiv(ctrlID) {
            $('#' + ctrlID).css({ 'display': 'block' });
            $("html, body").stop().animate({ scrollLeft: $('#' + ctrlID).offset().left, scrollTop: $('#' + ctrlID).offset().top }, 1200);
        }
        function cntrlSave() {
            if ($("[id*=gv_EarmarkedStock_chk_selectQty_]:checked").length != 0 && $('#txtRevokeCommets').val().length > 0)
                return true;
            else {
                if ($("[id*=gv_EarmarkedStock_chk_selectQty_]:checked").length == 0)
                    alert("Choose atleast one quantity");
                else if ($('#txtRevokeCommets').val().length == 0)
                    alert("Enter revoke comments");
                return false;
            }
        }
    </script>
</asp:Content>
