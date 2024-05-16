<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="CotsReviseCancel1.aspx.cs" Inherits="TTS.CotsReviseCancel1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/cotsScript.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: center;" class="pageTitleHead">
        <asp:Label runat="server" ID="lblHeadPage" ClientIDMode="Static" Text=""></asp:Label>
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="width: 100%; border-color: White;
            border-collapse: separate;">
            <tr>
                <td>
                    <asp:GridView runat="server" ID="gvReviseOrderList" AutoGenerateColumns="false" Width="100%"
                        HeaderStyle-BackColor="#FFCC00" AlternatingRowStyle-BackColor="#f5f5f5">
                        <Columns>
                            <asp:TemplateField HeaderText="CUSTOMER NAME">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatusCustName" Text='<%# Eval("custfullname") %>'></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdnOrderCustCode" Value='<%# Eval("CustCode") %>' />
                                    <asp:HiddenField runat="server" ID="hdnRequestStatus" Value='<%# Eval("RequestStatus") %>' />
                                    <asp:HiddenField runat="server" ID="hdnOrderID" Value='<%# Eval("ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ORDER REFERENCE NO.">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOrderRefNo" Text='<%# Eval("OrderRefNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="ORDERD DATE" DataField="CompletedDate" />
                            <asp:BoundField HeaderText="LAST REVISED DATE" DataField="RevisedDate" />
                            <asp:BoundField HeaderText="CUSTOMER DESIRED SHIP DATE" DataField="DesiredShipDate" />
                            <asp:BoundField HeaderText="QTY" DataField="itemqty" ItemStyle-HorizontalAlign="Right" />
                            <asp:TemplateField HeaderText="PLANT">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblPlant" Text='<%# Eval("Plant") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="STATUS">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatusText" Text='<%# Eval("StatusText") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ACTION" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkReviseBtn" runat="server" Text="Revise" OnClick="lnkReviseBtn_Click"
                                        Visible='<%# Eval("CustHoldStatus").ToString() == "True" ? false : true%>' /><span
                                            style="color: #ff0000; font-style: italic;">
                                            <%# Eval("CustHoldStatus").ToString() == "True" ? "CREDIT HOLD" : ""%></span>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ACTION" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkCancelBtn" runat="server" Text="Cancel" OnClick="lnkCancelBtn_Click"
                                        Visible='<%# Eval("CustHoldStatus").ToString() == "True" ? false : true%>' /><span
                                            style="color: #ff0000; font-style: italic;">
                                            <%# Eval("CustHoldStatus").ToString() == "True" ? "CREDIT HOLD" : ""%></span></ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <div runat="server" clientidmode="Static" style="width: 100%; float: left; padding-top: 10px;"
                        id="divCancelBtn">
                        <table cellspacing="0" rules="all" border="1" style="width: 100%; border-color: White;
                            border-collapse: separate;">
                            <tr id="divOrderHead">
                                <td>
                                    <asp:Label runat="server" ID="lblCustName" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblStausOrderRefNo" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:GridView runat="server" ID="gvOrderItemList" AutoGenerateColumns="false" Width="100%"
                                        AlternatingRowStyle-BackColor="#ecf6ff" ShowFooter="true">
                                        <HeaderStyle BackColor="#a1ccf3" Font-Bold="true" Height="22px" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="CATEGORY">
                                                <ItemTemplate>
                                                    <%# Eval("category") %>
                                                    <%# Eval("AssyRimstatus").ToString() == "True" ? " (ASSY)" : ""%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="PLATFORM" DataField="config" />
                                            <asp:BoundField HeaderText="TYRE SIZE" DataField="tyresize" />
                                            <asp:BoundField HeaderText="RIM" DataField="rimsize" />
                                            <asp:BoundField HeaderText="TYPE" DataField="tyretype" />
                                            <asp:BoundField HeaderText="BRAND" DataField="brand" />
                                            <asp:BoundField HeaderText="SIDEWALL" DataField="sidewall" />
                                            <asp:BoundField HeaderText="BASIC PRICE" DataField="listprice" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField HeaderText="QTY" DataField="itemqty" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField HeaderText="FWT" DataField="tyrewt" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField HeaderText="RIM QTY" DataField="Rimitemqty" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField HeaderText="RIM BASIC PRICE" DataField="Rimunitprice" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField HeaderText="RIM FWT" DataField="Rimfinishedwt" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField HeaderText="DWG NO." DataField="RimDwg" />
                                        </Columns>
                                        <FooterStyle BackColor="#fbe6d0" HorizontalAlign="Right" Font-Bold="true" />
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <span class="headCss">Reason for cancel this order </span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:TextBox runat="server" ID="txtCotsOrderCancelFeedBack" ClientIDMode="Static"
                                        onKeyUp="javascript:CheckMaxLength(this, 3999);" onChange="javascript:CheckMaxLength(this, 3999);"
                                        TextMode="MultiLine" Width="1050px" Height="100px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Button runat="server" ID="btnCotsOrderCancel" ClientIDMode="Static" CssClass="btnclear"
                                        Text="ORDER CANCEL SAVE" OnClick="btnCotsOrderCancel_Click" OnClientClick="javascript:return CtrlCancelTxtVal();" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView runat="server" ID="gv_eInvoiceCancel" AutoGenerateColumns="false" Width="100%"
                        HeaderStyle-BackColor="#FFCC00" AlternatingRowStyle-BackColor="#f5f5f5" CssClass="gridcss">
                        <Columns>
                            <asp:BoundField HeaderText="CUSTOMER" DataField="custfullname" />
                            <asp:BoundField HeaderText="ORDER REF NO" DataField="OrderRefNo" />
                            <asp:BoundField HeaderText="ORDER DATE" DataField="CompletedDate" />
                            <asp:BoundField HeaderText="INVOICE NO" DataField="invoiceno" />
                            <asp:BoundField HeaderText="INVOICE DATE" DataField="invoicedate" />
                            <asp:BoundField HeaderText="PLANT" DataField="Plant" />
                            <asp:BoundField HeaderText="QTY" DataField="itemqty" ItemStyle-HorizontalAlign="Right" />
                            <asp:TemplateField HeaderText="REMARKS" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txt_eInvoiceCancelRemarks" Text="" TextMode="MultiLine"
                                        CssClass="form-control" Width="200px" Height="80px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ACTION" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="lnk_eInvoice" Text="CANCEL" OnClientClick="javascript:return Ctrl_lnk_eInvoice(this);"
                                        OnClick="lnk_eInvoice_click"></asp:LinkButton>
                                    <asp:HiddenField runat="server" ID="hdnCustCode" Value='<%# Eval("CustCode") %>' />
                                    <asp:HiddenField runat="server" ID="hdn_OID" Value='<%# Eval("ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnCotsCustCode" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnPlant" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnOID" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        blinkOrderHead();
        function CtrlCancelTxtVal() {
            if ($('#txtCotsOrderCancelFeedBack').val().length == 0) {
                alert('Enter the reason for cancel this order');
                return false;
            }
            else
                return true;
        }

        function Ctrl_lnk_eInvoice(e) {
            var strID = $(e).attr('id');
            var strTxtID = strID.replace('lnk_eInvoice', 'txt_eInvoiceCancelRemarks');
            if ($('#' + strTxtID).val().length == 0) {
                alert('Enter e-Invoice Cancel Remarks');
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
