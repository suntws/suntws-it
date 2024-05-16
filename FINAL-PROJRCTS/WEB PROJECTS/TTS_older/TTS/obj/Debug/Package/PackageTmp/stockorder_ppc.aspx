<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="stockorder_ppc.aspx.cs"
    MasterPageFile="~/master.Master" Inherits="TTS.stockorder_ppc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/cotsScript.js" type="text/javascript"></script>
    <style type="text/css">
        .cssLblMsg
        {
            background-color: #9e2905;
            color: #ffffff;
            font-size: 15px;
            font-weight: bold;
            width: 100%;
            float: left;
            line-height: 25px;
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        <asp:Label runat="server" ID="lblPageHead" ClientIDMode="Static" Text=""></asp:Label>
        Stock Order PPC Analysis
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
                    <asp:GridView ID="gv_Orders" Width="100%" runat="server" AutoGenerateColumns="false"
                        RowStyle-Height="30px">
                        <HeaderStyle BackColor="#74bbf9" ForeColor="White" Font-Bold="true" Height="25px"
                            HorizontalAlign="Center" />
                        <Columns>
                            <asp:TemplateField HeaderText="CUSTOMER NAME" ItemStyle-Width="250px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatusCustName" Text='<%# Eval("custfullname") %>'></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdnStatusCustCode" Value='<%# Eval("CustStdcode") %>' />
                                    <asp:HiddenField runat="server" ID="hdnStockOrderID" Value='<%# Eval("StockOrderid") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="WORK ORDER" DataField="workorderno" ItemStyle-Width="60px" />
                            <asp:TemplateField HeaderText="ORDER REF NO" ItemStyle-Width="220px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOrderRefNo" Text='<%#Eval("RefNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ORDER DATE" ItemStyle-Width="70px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOrderDate" Text='<%#Eval("createdon") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="DESIRED END DATE" DataField="orderendon" ItemStyle-Width="70px" />
                            <asp:BoundField HeaderText="ORDER QTY" DataField="itemqty" ItemStyle-Width="40px"
                                ItemStyle-HorizontalAlign="Right" />
                            <asp:TemplateField ItemStyle-Width="75px" HeaderText="ACTION">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkOrderSelection" CssClass="btn btn-success" runat="server"
                                        OnClick="lnkOrderSelection_click" Text="Process">
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
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:GridView ID="gv_OrderItems" Width="100%" runat="server" AutoGenerateColumns="false"
                                        RowStyle-Height="22px" ShowFooter="true" FooterStyle-Font-Bold="true" HeaderStyle-BackColor="#0688f9"
                                        FooterStyle-HorizontalAlign="Right" FooterStyle-BackColor="#dfe0f3">
                                        <HeaderStyle BackColor="#74bbf9" ForeColor="White" Font-Bold="true" Height="25px"
                                            HorizontalAlign="Center" />
                                        <Columns>
                                            <asp:BoundField HeaderText="CATEGORY" DataField="category" />
                                            <asp:BoundField HeaderText="PLATFORM" DataField="config" />
                                            <asp:BoundField HeaderText="TYRE SIZE" DataField="tyresize" />
                                            <asp:BoundField HeaderText="RIM" DataField="rimsize" />
                                            <asp:BoundField HeaderText="TYPE" DataField="tyretype" />
                                            <asp:BoundField HeaderText="BRAND" DataField="brand" />
                                            <asp:BoundField HeaderText="SIDEWALL" DataField="sidewall" />
                                            <asp:BoundField HeaderText="PROCESS-ID" DataField="processid" />
                                            <asp:TemplateField HeaderText="QTY" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblQty" Text='<%#Eval("itemqty") %>'></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="AVAL QTY" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_AvalQty" runat="server" Height="20px" Width="60px" Text='<%# Eval("AvalQty") %>'
                                                        onkeyup="splitOrderQty(this)" onkeypress="return isNumberWithoutDecimal(event)"
                                                        CssClass="form-control"></asp:TextBox>
                                                    <div>
                                                        <asp:Label runat="server" ID="lblQtyErrMsg" Text=""></asp:Label>
                                                        <asp:HiddenField runat="server" ID="hdnItemID" ClientIDMode="Static" Value='<%# Eval("ItemID") %>' />
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="NEW PROD" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblnewprod" Text='<%# Eval("O_Newproduction").ToString() == "" ? "0" : Eval("O_Newproduction").ToString()%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="FWT" DataField="totfwt" ItemStyle-HorizontalAlign="Right" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    WORK ORDER :
                                    <asp:LinkButton runat="server" ID="lnkStockWorkOrder" ClientIDMode="Static" Text=""
                                        Font-Bold="true" OnClick="lnkStockWorkOrder_Click"></asp:LinkButton>
                                </td>
                                <td>
                                    <asp:Button ID="btn_SaveRecords" ClientIDMode="Static" runat="server" Text="Save Record"
                                        CssClass="btn btn-success" OnClick="btn_SaveRecords_Click" />
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btn_ClearRecords" runat="server" Text="Clear Selection" CssClass="btn btn-info" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblErrMsg" runat="server" ClientIDMode="Static" Font-Bold="true" ForeColor="Red"></asp:Label>
                                </td>
                                <td>
                                    <div id="div_StatusChange" style="text-align: center; display: none;">
                                        <span class="spanCss" style="text-align: left;">Enter Comments</span>
                                        <asp:TextBox ID="txt_StatusChangeComments" runat="server" ClientIDMode="Static" TextMode="MultiLine"
                                            Height="80px" Width="80%" CssClass="form-control"></asp:TextBox><br />
                                        <asp:Button ID="btn_StatusChange" ClientIDMode="Static" runat="server" Text="MOVE TO PRODUCTION"
                                            CssClass="btn btn-success" OnClick="btn_StatusChange_Click" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnCustCode" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnSelectIndex" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnSID" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        function splitOrderQty(e) {
            debugger;
            var ctrltxtSplitQty = e.id;
            var ctrllblSplitQty = ctrltxtSplitQty.replace('txt_AvalQty', 'lblnewprod');
            var ctrlOrderQty = ctrltxtSplitQty.replace('txt_AvalQty', 'lblQty');
            var ctrllblQtyErrMsg = ctrltxtSplitQty.replace('txt_AvalQty', 'lblQtyErrMsg');
            if (parseInt($('#' + ctrltxtSplitQty).val()) <= parseInt($('#' + ctrlOrderQty).html())) {
                $('#' + ctrllblSplitQty).html('');
                $('#' + ctrllblQtyErrMsg).html('');
                if ($('#' + ctrltxtSplitQty).val().length == 0)
                    $('#' + ctrllblSplitQty).html(parseInt($('#' + ctrlOrderQty).html()));
                else
                    $('#' + ctrllblSplitQty).html(parseInt($('#' + ctrlOrderQty).html()) - parseInt($('#' + ctrltxtSplitQty).val()));
            }
            else
                $('#' + ctrllblQtyErrMsg).html('Enter proper qty').css({ 'color': '#f00' });
        }
    </script>
</asp:Content>
