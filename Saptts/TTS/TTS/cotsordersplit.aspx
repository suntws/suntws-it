<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="cotsordersplit.aspx.cs" Inherits="TTS.cotsordersplit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="Scripts/cotsScript.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        <asp:Label runat="server" ID="lblTitlepage" ClientIDMode="Static" Text=""></asp:Label>
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
                    <asp:GridView runat="server" ID="gvorderlist" AutoGenerateColumns="false" Width="100%"
                        HeaderStyle-BackColor="#FFCC00" AlternatingRowStyle-BackColor="#f5f5f5" RowStyle-Height="25px">
                        <Columns>
                            <asp:TemplateField HeaderText="CUSTOMER NAME" ItemStyle-Width="200px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatusCustName" Text='<%# Eval("custfullname") %>'></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdnStatusCustCode" Value='<%# Eval("CustCode") %>' />
                                    <asp:HiddenField runat="server" ID="hdnOrderID" Value='<%# Eval("ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ORDER REF NO." ItemStyle-Width="220px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOrderRefNo" Text='<%#Eval("OrderRefNo") %>'></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdnUserCurrency" Value='<%# Eval("usercurrency") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ORDER DATE" ItemStyle-Width="60px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOrderDate" Text='<%#Eval("CompletedDate") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="REVISED DATE" DataField="RevisedDate" ItemStyle-Width="60px" />
                            <asp:BoundField HeaderText="CUSTOMER DESIRED SHIP DATE" DataField="DesiredShipDate"
                                ItemStyle-Width="60px" />
                            <asp:TemplateField HeaderText="EXPECT SHIPPING DATE" ItemStyle-Width="60px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblExpectShipDate" Text='<%# Eval("ExpectedShipDate") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="QTY" DataField="itemqty" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Right" />
                            <asp:TemplateField HeaderText="PLANT" ItemStyle-Width="40px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblPlant" Text='<%# Eval("Plant") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="STATUS" ItemStyle-Width="170px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatusText" Text='<%# Eval("StatusText") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="50px" HeaderText="ACTION">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkSplitBtn" runat="server" Text="SPLIT" OnClick="lnkSplitBtn_Click"
                                        Visible='<%# Eval("CustHoldStatus").ToString() == "True" ? false : true%>' />
                                    <span style="color: #ff0000; font-style: italic;">
                                        <%# Eval("CustHoldStatus").ToString() == "True" ? "CREDIT HOLD" : ""%></span>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="width: 100%; float: left; border: 1px solid #000; display: none; line-height: 20px;
                        margin-top: 10px; background-color: #F0E2F5; padding-top: 5px;" id="divStatusChange">
                        <div style="width: 100%; float: left; font-weight: bold; font-size: 18px; color: #fff;
                            background-color: #2E2B2B;">
                            <span style="width: 50%; float: left;">
                                <asp:Label runat="server" ID="lblCustName" ClientIDMode="Static" Text=""></asp:Label></span>
                            <span style="width: 50%; float: right;">
                                <asp:Label runat="server" ID="lblStausOrderRefNo" ClientIDMode="Static" Text=""></asp:Label></span>
                        </div>
                        <div>
                            <asp:GridView runat="server" ID="gvSplitOrderQty" AutoGenerateColumns="false" Width="100%"
                                AlternatingRowStyle-BackColor="#f5f5f5">
                                <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" Height="22px" />
                                <Columns>
                                    <asp:TemplateField ItemStyle-CssClass="headerNone" HeaderStyle-CssClass="headerNone">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblProcessid" Text='<%#Eval("processid") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="CATEGORY" ItemStyle-Width="100px">
                                        <ItemTemplate>
                                            <%# Eval("category") %>
                                            <%# Eval("AssyRimstatus").ToString() == "True" ? " (ASSY)" : ""%>
                                            <asp:HiddenField runat="server" ID="hdnItemID" ClientIDMode="Static" Value='<%# Eval("O_ItemID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="PLATFORM" ItemStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblCofig" Text='<%#Eval("config") %>'></asp:Label></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="TYRE SIZE" ItemStyle-Width="150px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblSize" Text='<%#Eval("tyresize") %>'></asp:Label></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="RIM" ItemStyle-Width="40px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblRim" Text='<%#Eval("rimsize") %>'></asp:Label></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="TYPE" ItemStyle-Width="60px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblType" Text='<%#Eval("tyretype") %>'></asp:Label></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="BRAND" ItemStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblBrand" Text='<%#Eval("brand") %>'></asp:Label></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SIDEWALL" ItemStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblSidewall" Text='<%#Eval("sidewall") %>'></asp:Label></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="TYER ORDER QTY" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblQty" Text='<%#Eval("itemqty") %>'></asp:Label></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="PART-A QTY" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtPart1Qty" onkeypress="return isNumberKey(event)"
                                                Width="50px" MaxLength="4" BackColor="#f9c232" Text='<%# Eval("itemqty") %>'
                                                onkeyup="splitOrderQty(this)"></asp:TextBox>
                                            <div>
                                                <asp:Label runat="server" ID="lblQtyErrMsg" Text=""></asp:Label></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="PART-B QTY" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblPart2Qty" Text="0"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="RIM ORDER QTY" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblRimQty" Text='<%# Eval("Rimitemqty").ToString() == "0" ? "" : Eval("Rimitemqty").ToString()%>'></asp:Label></ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <div style="width: 100%; float: left;">
                                <div style="width: 800px; float: left; text-align: right; color: #f00; font-weight: bold;
                                    padding-right: 10px; line-height: 30px;" id="divErrmsg">
                                    &nbsp;
                                </div>
                                <div style="width: 200px; float: right; text-align: center;">
                                    <asp:Button runat="server" ID="btnSplitOrder" Text="SAVE SPLIT ORDER" CssClass="btnsave"
                                        OnClientClick="javascript:return chkSplitQty();" OnClick="btnSplitOrder_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnCustCode" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnCurrency" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnStatus" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnStatusOrderDate" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnOrderPlant" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnOID" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        function showTotalValue() {
            $('#divStatusChange').css({ 'display': 'block' });
        }

        function splitOrderQty(e) {
            var ctrltxtSplitQty = e.id;
            var ctrllblSplitQty = ctrltxtSplitQty.replace('txtPart1Qty', 'lblPart2Qty');
            var ctrlOrderQty = ctrltxtSplitQty.replace('txtPart1Qty', 'lblQty');
            var ctrllblQtyErrMsg = ctrltxtSplitQty.replace('txtPart1Qty', 'lblQtyErrMsg');
            if (parseInt($('#' + ctrltxtSplitQty).val()) <= parseInt($('#' + ctrlOrderQty).html())) {
                $('#' + ctrllblQtyErrMsg).html('');
                if ($('#' + ctrltxtSplitQty).val().length == 0)
                    $('#' + ctrllblSplitQty).html(parseInt($('#' + ctrlOrderQty).html()));
                else
                    $('#' + ctrllblSplitQty).html(parseInt($('#' + ctrlOrderQty).html()) - parseInt($('#' + ctrltxtSplitQty).val()));
            }
            else
                $('#' + ctrllblQtyErrMsg).html('Enter proper qty').css({ 'color': '#f00' });
        }

        function chkSplitQty() {
            var errmsg = 'No change(s) in the order qty';
            $('#divErrmsg').html('');
            var txtPart1Qty = $("input[id*='MainContent_gvSplitOrderQty_txtPart1Qty_']");
            for (var k = 0; k < txtPart1Qty.length; k++) {
                if ($('#MainContent_gvSplitOrderQty_txtPart1Qty_' + k).val() != $('#MainContent_gvSplitOrderQty_lblQty_' + k).html()) {
                    errmsg = ''; break;
                }
            }
            if (errmsg.length > 0) {
                $('#divErrmsg').html(errmsg);
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
