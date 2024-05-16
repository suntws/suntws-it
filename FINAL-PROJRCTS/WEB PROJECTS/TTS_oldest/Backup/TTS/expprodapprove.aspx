<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="expprodapprove.aspx.cs" Inherits="TTS.expprodapprove" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        <asp:Label runat="server" ID="lblPageHead" ClientIDMode="Static" Text="APPROVE FOR FRESH PRODUCTION - "></asp:Label>
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="background-color: #dcecfb; width: 100%;
            border-color: White; border-collapse: separate;">
            <tr>
                <td>
                    <asp:GridView runat="server" ID="gv_ApprovePendingOrders" AutoGenerateColumns="false"
                        Width="100%" CssClass="gridcss" HeaderStyle-BackColor="#3c763d" HeaderStyle-ForeColor="#ffffff">
                        <Columns>
                            <asp:BoundField HeaderText="CUSTOMER" DataField="custfullname" />
                            <asp:BoundField HeaderText="ORDER REF" DataField="OrderRefNo" />
                            <asp:BoundField HeaderText="WORK ORDER" DataField="workorderno" />
                            <asp:BoundField HeaderText="ORDER QTY" DataField="OrderQty" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField HeaderText="ORDER ITEMS COUNT" DataField="orderitems" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField HeaderText="REQUEST CREATED ITEMS COUNT" DataField="ReqItems" ItemStyle-HorizontalAlign="Right" />
                            <asp:TemplateField HeaderText="ACTION" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hdnOID" ClientIDMode="Static" Value='<%# Eval("ID") %>' />
                                    <asp:LinkButton runat="server" ID="lnkApproveOrder" ClientIDMode="Static" Text="VIEW ITEMS"
                                        OnClick="lnkApproveOrder_click"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="div_Sub_OrderItems" style="display: none;">
                        <table cellspacing="0" rules="all" border="1" style="background-color: #dcecfb; width: 100%;
                            border-color: White; border-collapse: separate;">
                            <tr style="text-align: center; background-color: #3c763d; font-size: 15px; color: #ffffff;">
                                <td>
                                    <asp:Label ID="lblSelectedCustomerName" runat="server" Text="" Font-Bold="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblSelectedOrderRefNo" ClientIDMode="Static" runat="server" Text=""
                                        Font-Bold="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblSelectedWorkOrderNo" ClientIDMode="Static" runat="server" Text=""
                                        Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:GridView ID="gv_ReqOrderItems" Width="100%" runat="server" AutoGenerateColumns="false"
                                        RowStyle-Height="22px" HeaderStyle-BackColor="#022442" CssClass="gridcss">
                                        <HeaderStyle ForeColor="White" Font-Bold="true" Height="25px" HorizontalAlign="Center" />
                                        <Columns>
                                            <asp:BoundField HeaderText="PLATFORM" DataField="Config" />
                                            <asp:BoundField HeaderText="TYRE SIZE" DataField="tyresize" />
                                            <asp:BoundField HeaderText="RIM" DataField="rimsize" />
                                            <asp:BoundField HeaderText="TYPE" DataField="tyretype" />
                                            <asp:BoundField HeaderText="BRAND" DataField="brand" />
                                            <asp:BoundField HeaderText="SIDEWALL" DataField="sidewall" />
                                            <asp:BoundField HeaderText="ORDER QTY" DataField="itemqty" ItemStyle-Width="40px"
                                                ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField HeaderText="STOCK QTY" DataField="earmarkqty" ItemStyle-Width="40px"
                                                ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField HeaderText="PRODUCED QTY" DataField="PartG" ItemStyle-Width="60px"
                                                ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField HeaderText="BALANCE QTY" DataField="balanceqty" ItemStyle-Width="60px"
                                                ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField HeaderText="STILL AVAILABLE QTY" DataField="Stock" ItemStyle-Width="60px"
                                                ItemStyle-HorizontalAlign="Right" />
                                            <asp:TemplateField HeaderText="REQUEST COMMENTS">
                                                <ItemTemplate>
                                                    <%# Eval("ReqComment") + "<br/> BY " + Eval("ReqBy") + " ON " + Eval("ReqOn")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="APPROVE COMMENTS">
                                                <ItemTemplate>
                                                    <asp:HiddenField runat="server" ID="hdnReqID" Value='<%# Eval("ReqID") %>' />
                                                    <asp:TextBox runat="server" ID="txtApproveComments" Text="" Width="180px" MaxLength="100"
                                                        CssClass="form-control"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:TextBox runat="server" ID="txt_Comments" ClientIDMode="Static" Text="" MaxLength="499"
                                        TextMode="MultiLine" CssClass="form-control" Width="90%" Height="50px"></asp:TextBox>
                                </td>
                                <td style="text-align: center;">
                                    <asp:Button runat="server" ID="btnApproveSave" ClientIDMode="Static" Text="APPROVE AND SAVE"
                                        OnClick="btnApproveSave_click" OnClientClick="javascript:return ctrlApproveComments();"
                                        CssClass="btn btn-success" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnOrderID" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        $(function () { $('input:text').blur(function () { $(this).css({ 'border': '1px solid #000' }); }); });
        function gotoPreviewDiv(ctrlID) {
            $('#' + ctrlID).css({ 'display': 'block' });
            $("html, body").stop().animate({ scrollLeft: $('#' + ctrlID).offset().left, scrollTop: $('#' + ctrlID).offset().top }, 1200);
        }
        function ctrlApproveComments() {
            var errMsg = '';
            $("input:text[id*=MainContent_gv_ReqOrderItems_txtApproveComments_]").each(function (e) {
                if ($('#' + this.id).val() == "") {
                    $('#' + this.id).css({ 'border': '2px solid #cc3210' });
                    errMsg = "Fill the comment box";
                }
            });
            if (errMsg.length > 0) {
                alert(errMsg);
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
