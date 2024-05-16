<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="frmproforma.aspx.cs" Inherits="COTS.frmproforma" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div align="center" class="pageTitleHead">
        ORDER ITEM LIST
    </div>
    <div class="contPage">
        <table cellspacing="0" rules="all" border="1" style="background-color: #ffffff; width: 100%;
            border-color: #cccbfb; border-collapse: separate; line-height: 25px;">
            <tr>
                <th style="text-align: left; width: 250px;">
                    ORDER NO
                </th>
                <td colspan="3">
                    <asp:Label runat="server" ID="lblOrderNo" ClientIDMode="Static" Text="" Font-Bold="true"
                        Font-Size="15px" ForeColor="#000CCC"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:GridView runat="server" ID="gvItemList" AutoGenerateColumns="false" Width="100%"
                        AlternatingRowStyle-BackColor="#f5f5f5" OnRowEditing="gvItemList_RowEditing"
                        OnRowUpdating="gvItemList_RowUpdating" OnRowCancelingEdit="gvItemList_RowCanceling"
                        OnRowDeleting="gvItemList_RowDeleting" ShowFooter="true" FooterStyle-Font-Bold="true"
                        FooterStyle-HorizontalAlign="Right" RowStyle-Font-Bold="true">
                        <HeaderStyle BackColor="#FEFE8B" Font-Bold="false" Height="22px" />
                        <Columns>
                            <asp:TemplateField HeaderText="TYRE SIZE">
                                <ItemTemplate>
                                    <%# Eval("tyresize") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label runat="server" ID="lblTyreSize" Text='<%# Eval("tyresize") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="RIM">
                                <ItemTemplate>
                                    <%# Eval("rimsize") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label runat="server" ID="lblRimSize" Text='<%# Eval("rimsize") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TYPE">
                                <ItemTemplate>
                                    <%# Eval("tyretype") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="BRAND">
                                <ItemTemplate>
                                    <%# Eval("brand") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SIDEWALL">
                                <ItemTemplate>
                                    <%# Eval("sidewall") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PROCESS CODE">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblProcessID" Text='<%#Eval("processid") %>'></asp:Label>
                                    <%# Eval("AssyRimstatus").ToString() == "True" && Eval("EdcNo").ToString() != "" ? (" EDC " + Eval("EdcNo")) : ""%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label runat="server" ID="lblProcessID" Text='<%#Eval("processid") %>'></asp:Label>
                                    <asp:Label runat="server" ID="lblEdcNo" Text='<%# Eval("AssyRimstatus").ToString() == "True" && Eval("EdcNo").ToString() != "" ? (" EDC " +Eval("EdcNo")):"" %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="QTY" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblQty" Text='<%#Eval("itemqty") %>'></asp:Label></ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox runat="server" ID="txtQty" onkeypress="return isNumberWithoutDecimal(event)"
                                        Width="40px" MaxLength="4" Text='<%# Eval("itemqty") %>' CssClass="form-control"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ADDITIONAL INFO" ItemStyle-Width="175px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblRimStatus" Text='<%# Eval("AssyRimstatus").ToString() == "True" ? "TYRE & WHEEL" : "" %>'></asp:Label>
                                    <%#  Eval("RimDwg")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox runat="server" ID="chkRimAssy" Text="ASSY" Font-Bold="true" TextAlign="Left" />
                                    <br />
                                    <table runat="server" cellspacing="0" rules="rows" border="1" style="background-color: #b1ff80;
                                        width: 175px; display: none;" id="tbAssyDetails">
                                        <tr>
                                            <th style="transform: rotate(310deg);">
                                                EDC
                                            </th>
                                            <td colspan="3">
                                                <asp:DropDownList runat="server" ID="ddl_EdcNo" CssClass="form-control" Width="135px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th style="transform: rotate(310deg);">
                                                WT
                                            </th>
                                            <td>
                                                <asp:TextBox runat="server" ID="txt_RimWt" CssClass="form-control" Width="40px" Text="0"
                                                    MaxLength="6" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                            </td>
                                            <th style="transform: rotate(310deg);">
                                                PRICE
                                            </th>
                                            <td>
                                                <asp:TextBox runat="server" ID="txt_RimPrice" CssClass="form-control" Width="50px"
                                                    Text="0" MaxLength="6" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th style="transform: rotate(310deg);">
                                                DESC
                                            </th>
                                            <td colspan="3">
                                                <asp:TextBox runat="server" ID="txt_RimDesc" CssClass="form-control" Width="135px"
                                                    Text="" MaxLength="50"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="UNIT PRICE">
                                <ItemTemplate>
                                    <%# Eval("unitprice")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="UNIT WT">
                                <ItemTemplate>
                                    <%# Eval("unitwt") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TOTAL PRICE" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <%# Eval("totprice")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TOTAL WT(Kgs)" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <%# Eval("totwt")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ACTION" ItemStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Button runat="server" ID="btnEdit" Text="Edit" CommandName="Edit" CssClass="btn btn-info" />
                                    <asp:HiddenField runat="server" ID="hdnO_ItemID" ClientIDMode="Static" Value='<%# Eval("O_ItemID") %>' />
                                    <asp:Button runat="server" ID="btnDelete" Text="Delete" CommandName="Delete" CssClass="btn btn-warning" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Button runat="server" ID="btnUpdate" Text="Update" CommandName="Update" CssClass="btn btn-success"
                                        OnClientClick="javascript:return CtrlSaveChk(this);" />
                                    <asp:Button runat="server" ID="btnCancel" Text="Cancel" CommandName="Cancel" CssClass="btn btn-warning" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"
                        Font-Bold="true" Font-Size="X-Large"></asp:Label>
                </td>
            </tr>
            <tr style="text-align: center;">
                <td>
                    <asp:Button runat="server" ID="btnPrepareMaster" ClientIDMode="Static" Text="MOVE TO ORDER COMPLETE"
                        CssClass="btn btn-success" OnClick="btnPrepareMaster_Click" />
                </td>
                <td>
                    <asp:Button runat="server" ID="btnBackToEntry" ClientIDMode="Static" Text="AMEND ORDER"
                        CssClass="btn btn-info" OnClick="btnBackToEntry_Click" />
                </td>
                <td>
                    <asp:Button runat="server" ID="btnSendToTvs" ClientIDMode="Static" Text="SEND ORDER"
                        CssClass="btn btn-success" OnClick="btnSendToTvs_Click" />
                </td>
                <td>
                    <span class="btn btn-info" onclick="saveandclosethemsg();">SAVE & EXIT</span>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%=gvItemList.ClientID  %> tr').find("input[id*='ContentPlaceHolder1_gvItemList_chkRimAssy_']").change(function () {
                if ($(this).is(':checked'))
                    $("#" + this.id.replace('chkRimAssy', 'tbAssyDetails')).css({ 'display': 'block' });
                else
                    $("#" + this.id.replace('chkRimAssy', 'tbAssyDetails')).css({ 'display': 'none' });
            });
            $('#<%=gvItemList.ClientID  %> tr').find("input[id*='ContentPlaceHolder1_gvItemList_chkRimAssy_']").each(function () {
                if ($(this).is(':checked'))
                    $("#" + this.id.replace('chkRimAssy', 'tbAssyDetails')).css({ 'display': 'block' });
                else
                    $("#" + this.id.replace('chkRimAssy', 'tbAssyDetails')).css({ 'display': 'none' });
            });
        });
        function saveandclosethemsg() {
            var result = confirm("Attention : Your order has not been sent. Please remember to access the incomplete order tab to finalize and send. Thank you.");
            if (result == true) {
                var pathname = window.location.href.toLowerCase();
                var splitval = pathname.split('/frmproforma.aspx');
                window.location.href = splitval[0].toString() + '/default.aspx';
            }
        }
        function CtrlSaveChk(e) {
            var errmsg = '';
            if ($("#" + e.id.replace('btnUpdate', 'txtQty')).val().length == 0 || parseInt($("#" + e.id.replace('btnUpdate', 'txtQty')).val()) == 0)
                errmsg = 'Enter Qty\n';
            if ($("#" + e.id.replace('btnUpdate', 'chkRimAssy')).is(':checked')) {
                if ($("#" + e.id.replace('btnUpdate', 'ddl_EdcNo') + ' option:selected').text() == 'CHOOSE')
                    errmsg += 'Choose EDC No.\n';
                if ($("#" + e.id.replace('btnUpdate', 'txt_RimWt')).val().length == '' || parseFloat($("#" + e.id.replace('btnUpdate', 'txt_RimWt')).val()) == 0)
                    errmsg += 'Enter rim wt\n';
                if ($("#" + e.id.replace('btnUpdate', 'txt_RimPrice')).val().length == '' || parseFloat($("#" + e.id.replace('btnUpdate', 'txt_RimPrice')).val()) == 0)
                    errmsg += 'Enter rim price\n';
            }
            if (errmsg.length > 0) {
                alert(errmsg);
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
