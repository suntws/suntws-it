<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="assignentry.ascx.cs"
    Inherits="TTS.ebidpurchase.assignentry" %>
<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
<div style="width: 1080px;">
    <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label>
    <div style="display: none;" runat="server" id="divAssignSupplier" clientidmode="Static">
        <div style="text-align: center; font-size: 16px; font-weight: bold; margin-top: 2px;
            color: #525212;">
            PRODUCT LIST</div>
        <table width="1078px;">
            <tr>
                <td>
                    <asp:GridView runat="server" ID="gvPurchaseProductList" AutoGenerateColumns="false"
                        Width="1078px">
                        <HeaderStyle CssClass="tbMaster" />
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="40px" HeaderStyle-Width="40px">
                                <ItemTemplate>
                                    <asp:RadioButton runat="server" ID="rdbProductID" AutoPostBack="true" OnCheckedChanged="rdbProductID_IndexChange"
                                        onclick="RadioCheckProduct(this);" />
                                    <asp:HiddenField runat="server" ID="hdnProductID" Value='<%#Eval("ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ProdCategory" HeaderText="CATEGORY" />
                            <asp:BoundField DataField="ProdDesc" HeaderText="PRODUCT DESC" />
                            <asp:BoundField DataField="ProdMeasurement" HeaderText="MEASUREMENT" ItemStyle-Width="100px" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblProductHead" ClientIDMode="Static" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="text-align: center; font-size: 16px; font-weight: bold; margin-top: 2px;
                        color: #13aee9;">
                        SUPPLIER LIST</div>
                    <asp:GridView runat="server" ID="gvPurchaseAssignSupplierList" AutoGenerateColumns="false"
                        Width="1076px">
                        <HeaderStyle CssClass="tbAssignList" />
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="40px" HeaderStyle-Width="40px">
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="chkSupplierAssign" />
                                    <asp:HiddenField runat="server" ID="hdnSupplierAssign" Value='<%# Eval("ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="SupplierName" HeaderText="SUPPLIER" />
                            <asp:BoundField DataField="SuppCountry" HeaderText="COUNTRY" />
                            <asp:BoundField DataField="SuppCity" HeaderText="CITY" />
                            <asp:BoundField DataField="SuppContactPerson" HeaderText="CONTACT PERSON" />
                            <asp:BoundField DataField="SuppContactNo" HeaderText="CONTACT NO" />
                            <asp:BoundField DataField="SuppEmailID" HeaderText="EMAIL ID" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button runat="server" ID="btnAssignSupplier" ClientIDMode="Static" Text="SAVE PRODUCT'S SUPPLIERS"
                        OnClick="btnAssignSupplier_Click" OnClientClick="javascipt:return CtrlAssignSupplier();"
                        Font-Bold="true" />
                </td>
            </tr>
        </table>
    </div>
    <div style="display: none;" runat="server" id="divAssignProduct" clientidmode="Static">
        <div style="text-align: center; font-size: 16px; font-weight: bold; margin-top: 2px;
            color: #163f4e;">
            SUPPLIER LIST</div>
        <table width="1078px;">
            <tr>
                <td>
                    <asp:GridView runat="server" ID="gvPurchaseSupplierList" AutoGenerateColumns="false"
                        Width="1078px">
                        <HeaderStyle CssClass="tbMaster" />
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="40px" HeaderStyle-Width="40px">
                                <ItemTemplate>
                                    <asp:RadioButton runat="server" ID="rdbSupplierID" AutoPostBack="true" OnCheckedChanged="rdbSupplierID_IndexChange"
                                        onclick="RadioCheckSupplier(this);" />
                                    <asp:HiddenField runat="server" ID="hdnSupplierID" Value='<%#Eval("ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="SupplierName" HeaderText="SUPPLIER" />
                            <asp:BoundField DataField="SuppCountry" HeaderText="COUNTRY" />
                            <asp:BoundField DataField="SuppCity" HeaderText="CITY" />
                            <asp:BoundField DataField="SuppContactPerson" HeaderText="CONTACT PERSON" />
                            <asp:BoundField DataField="SuppContactNo" HeaderText="CONTACT NO" />
                            <asp:BoundField DataField="SuppEmailID" HeaderText="EMAIL ID" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblSupplierHead" ClientIDMode="Static" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="text-align: center; font-size: 16px; font-weight: bold; margin-top: 2px;
                        color: #13aee9;">
                        PRODUCT LIST</div>
                    <asp:GridView runat="server" ID="gvPurchaseAssignProductList" AutoGenerateColumns="false"
                        Width="1076px">
                        <HeaderStyle CssClass="tbAssignList" />
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="40px" HeaderStyle-Width="40px">
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="chkProductAssign" />
                                    <asp:HiddenField runat="server" ID="hdnProductAssign" Value='<%# Eval("ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ProdCategory" HeaderText="CATEGORY" />
                            <asp:BoundField DataField="ProdDesc" HeaderText="PRODUCT DESC" />
                            <asp:BoundField DataField="ProdMeasurement" HeaderText="MEASUREMENT" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button runat="server" ID="btnAssignProduct" ClientIDMode="Static" Text="SAVE SUPPLIER'S PRODUCTS"
                        OnClick="btnAssignProduct_Click" OnClientClick="javascript:return CtrlAssignProduct();"
                        Font-Bold="true" />
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnProductId" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnSupplierId" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        function RadioCheckProduct(rd) {
            var gv = document.getElementById("<%=gvPurchaseProductList.ClientID%>");
            RadioCheck(rd, gv);
        }
        function RadioCheckSupplier(rd) {
            var gv = document.getElementById("<%=gvPurchaseSupplierList.ClientID%>");
            RadioCheck(rd, gv);
        }
        function RadioCheck(rb, gv) {
            var rbs = gv.getElementsByTagName("input"); var row = rb.parentNode.parentNode;
            for (var i = 0; i < rbs.length; i++) { if (rbs[i].type == "radio") { if (rbs[i].checked && rbs[i] != rb) { rbs[i].checked = false; break; } } }
        }

        function CtrlAssignSupplier() {
            var ErrMsg = '';
            $('#lblErrMsg').html('');
            if ($('input:radio[id*=MainContent_ctl00_gvPurchaseProductList_rdbProductID_]:checked').length == 0)
                ErrMsg += 'Choose any one product <br />';
            else {
                var id1 = $('input:radio[id*=MainContent_ctl00_gvPurchaseProductList_rdbProductID_]:checked').attr('id');
                var hdn1 = id1.replace('rdbProductID', 'hdnProductID');
                $('#hdnProductId').val($('#' + hdn1).val());
            }
            if ($('input:checkbox[id*=MainContent_ctl00_gvPurchaseAssignSupplierList_chkSupplierAssign_]:checked').length == 0)
                ErrMsg += 'Choose any one supplier <br />';
            if (ErrMsg.length > 0) {
                $('#lblErrMsg').html(ErrMsg);
                return false;
            }
            else
                return true;
        }

        function CtrlAssignProduct() {
            var ErrMsg = '';
            $('#lblErrMsg').html('');
            if ($('input:radio[id*=MainContent_ctl00_gvPurchaseSupplierList_rdbSupplierID_]:checked').length == 0)
                ErrMsg += 'Choose any one supplier<br />';
            else {
                var id1 = $('input:radio[id*=MainContent_ctl00_gvPurchaseSupplierList_rdbSupplierID_]:checked').attr('id');
                var hdn1 = id1.replace('rdbSupplierID', 'hdnSupplierID');
                $('#hdnSupplierId').val($('#' + hdn1).val());
            }
            if ($('input:checkbox[id*=MainContent_ctl00_gvPurchaseAssignProductList_chkProductAssign_]:checked').length == 0)
                ErrMsg += 'Choose any one product<br />';
            if (ErrMsg.length > 0) {
                $('#lblErrMsg').html(ErrMsg);
                return false;
            }
            else
                return true;
        }

        function setTitle(title) {
            $("#lblPageHead").text(title);
        }
    </script>
</div>
