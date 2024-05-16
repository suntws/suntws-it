<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="masterentry.ascx.cs"
    Inherits="TTS.ebidpurchase.masterentry" %>
<div style="width: 1080px;">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"
            Font-Bold="true"></asp:Label>
    </div>
    <div style="display: none;" runat="server" id="divProduct" clientidmode="Static">
        <table cellspacing="0" rules="all" border="1" class="masterpage">
            <tr>
                <th>
                    CATEGORY
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtCategory" ClientIDMode="Static" Text="" CssClass="txtCss"
                        MaxLength="50"></asp:TextBox>
                </td>
                <th>
                    MEASUREMENT
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="ddlMeasurement" ClientIDMode="Static" CssClass="ddlCss"
                        Width="60px">
                        <asp:ListItem Text="Choose" Value="Choose"></asp:ListItem>
                        <asp:ListItem Text="Nos" Value="Nos"></asp:ListItem>
                        <asp:ListItem Text="Set" Value="Set"></asp:ListItem>
                        <asp:ListItem Text="Kgs" Value="Kgs"></asp:ListItem>
                        <asp:ListItem Text="Tons" Value="Tons"></asp:ListItem>
                        <asp:ListItem Text="Pack" Value="Pack"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    PRODUCT DESC
                </th>
                <td colspan="2">
                    <asp:TextBox runat="server" ID="txtProductDesc" ClientIDMode="Static" Text="" CssClass="txtCss"
                        MaxLength="100"></asp:TextBox>
                </td>
                <td>
                    <asp:Button runat="server" ID="btnSaveProduct" ClientIDMode="Static" Text="SAVE PRODUCT"
                        OnClick="btnSaveProduct_Click" OnClientClick="javascript:return CtrlSaveProduct();" />
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:GridView runat="server" ID="gvPurchaseProductList" AutoGenerateColumns="false"
                        Width="1070px">
                        <HeaderStyle CssClass="tbMaster" />
                        <Columns>
                            <asp:BoundField DataField="ProdCategory" HeaderText="CATEGORY" />
                            <asp:BoundField DataField="ProdDesc" HeaderText="PRODUCT DESC" />
                            <asp:BoundField DataField="ProdMeasurement" HeaderText="MEASUREMENT" ItemStyle-Width="100px" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
    <div style="display: none;" runat="server" id="divSupplier" clientidmode="Static">
        <table cellspacing="0" rules="all" border="1" class="masterpage">
            <tr>
                <th>
                    SUPPLIER NAME
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtSupplier" ClientIDMode="Static" Text="" CssClass="txtCss"
                        MaxLength="50"></asp:TextBox>
                </td>
                <th>
                    CONTACT PERSON
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtContactPerson" ClientIDMode="Static" Text="" CssClass="txtCss"
                        MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    COUNTRY
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtCountry" ClientIDMode="Static" Text="" CssClass="txtCss"
                        MaxLength="50"></asp:TextBox>
                </td>
                <th>
                    CONTACT NO <span style="font-size: 14px">/</span> ALTERNATE NO
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtContactNo" ClientIDMode="Static" Text="" CssClass="txtCss"
                        MaxLength="50" Width="170"></asp:TextBox>
                    <span style="">/</span>
                    <asp:TextBox runat="server" ID="txtAlterContactNo" ClientIDMode="Static" Text=""
                        CssClass="txtCss" MaxLength="50" Width="170" Style="margin: 0px;"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    CITY
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtCity" ClientIDMode="Static" Text="" CssClass="txtCss"
                        MaxLength="50"></asp:TextBox>
                </td>
                <th>
                    EMAIL ID <span style="font-size: 14px">/</span> ALTERNATE ID
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtEmailID" ClientIDMode="Static" Text="" CssClass="txtCss"
                        MaxLength="50"></asp:TextBox>
                    <asp:TextBox runat="server" ID="txtAlterEmailID" ClientIDMode="Static" Text="" CssClass="txtCss"
                        MaxLength="50" Style="margin-top: 3px;"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    ADDRESS
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtSupplierAddress" ClientIDMode="Static" TextMode="MultiLine"
                        CssClass="txtCss" Width="340px" Height="65px" Text="" onKeyUp="javascript:CheckMaxLength(this, 999);"
                        onChange="javascript:CheckMaxLength(this, 999);"></asp:TextBox>
                </td>
                <td colspan="2">
                    <table style="border: 1px solid #868282; border-top: none; border-left: none; border-right: none;
                        border-collapse: collapse; border-bottom: none; position: relative; top: -3px;
                        left: -1px; width: 576px;">
                        <tr style="height: 25px; border-bottom: 1px solid #868282;">
                            <th style="border-right: 1px solid #868282">
                                PAYMENT TERMS
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtPaymentTerms" CssClass="txtCss"> </asp:TextBox>
                            </td>
                        </tr>
                        <tr style="border-bottom: none;border-bottom: 1px solid #868282;">
                            <th style="border-right: 1px solid #868282">
                                CATEGORY
                            </th>
                            <td colspan="2">
                                <asp:DropDownList runat="server" ClientIDMode="Static" ID="ddlCategory" Width="348" style="border:1px solid black;">
                                    <asp:ListItem Text="A" Value="A"> </asp:ListItem>
                                    <asp:ListItem Text="B" Value="B"> </asp:ListItem>
                                    <asp:ListItem Text="C" Value="C"> </asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: center">
                                <asp:Button runat="server" ID="btnSaveSupplier" ClientIDMode="Static" Text="SAVE SUPPLIER"
                                    OnClick="btnSaveSupplier_Click" OnClientClick="javascript:return CtrlbtnSaveSupplier();"
                                    Style="position: relative; top: 3px;" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:GridView runat="server" ID="gvPurchaseSupplierList" AutoGenerateColumns="false"
                        Width="1076px">
                        <HeaderStyle CssClass="tbMaster" />
                        <Columns>
                            <asp:BoundField DataField="SupplierName" HeaderText="SUPPLIER" />
                            <asp:BoundField DataField="SuppCountry" HeaderText="COUNTRY" />
                            <asp:BoundField DataField="SuppCity" HeaderText="CITY" />
                            <asp:BoundField DataField="SuppContactPerson" HeaderText="CONTACT PERSON" />
                            <asp:TemplateField HeaderText="CONTACT NO">
                                <ItemTemplate>
                                    <%#Eval("SuppContactNo")%>
                                    <%# Eval("SuppAltContactNo").ToString() != "" ? "/  "+Eval("SuppAltContactNo").ToString(): ""   %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="EMAIL ID">
                                <ItemTemplate>
                                    <%#Eval("SuppEmailID")%>
                                    <%# Eval("SuppAltEmailID").ToString() != "" ? "/  " + Eval("SuppAltEmailID").ToString() : ""%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="PaymentTerms" HeaderText="PAYMENT TERMS" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        function CtrlSaveProduct() {
            var errMsg = '';
            $('#lblErrMsg').html('');
            if ($('#txtCategory').val().length == 0)
                errMsg += 'Enter product category<br/>';
            if ($('#txtProductDesc').val().length == 0)
                errMsg += 'Enter product description<br/>';
            if ($('#ddlMeasurement option:selected').val() == "Choose")
                errMsg += 'Choose measurement<br/>';
            if (errMsg.length > 0) {
                $('#lblErrMsg').html(errMsg);
                return false;
            }
            else
                return true;
        }

        function CtrlbtnSaveSupplier() {
            var errMsg = '';
            $('#lblErrMsg').html('');
            var emailpattern = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
            if ($('#txtSupplier').val().length == 0)
                errMsg += 'Enter supplier name<br/>';
            if ($('#txtCountry').val().length == 0)
                errMsg += 'Enter country<br/>';
            if ($('#txtCity').val().length == 0)
                errMsg += 'Enter city<br/>';
            if ($('#txtContactPerson').val().length == 0)
                errMsg += 'Enter contact person<br/>';
            if ($('#txtContactNo').val().length == 0)
                errMsg += 'Enter contact no<br/>';
            if ($('#txtEmailID').val().length == 0)
                errMsg += 'Enter email-id<br/>';
            else if (emailpattern.test($('#txtEmailID').val()) == false)
                errMsg += 'Enter vaild email <br />';
            if (errMsg.length > 0) {
                $('#lblErrMsg').html(errMsg);
                return false;
            }
            else
                return true;
        };

        function setTitle(title) {
            $("#lblPageHead").text(title);
        }
    </script>
</div>
