<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="frmexporder.aspx.cs" Inherits="COTS.frmexporder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div align="center" class="pageTitleHead">
        Order Item Details
    </div>
    <div class="contPage">
        <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #07A714;
            width: 100%;">
            <tr>
                <th>
                    ORDER NO
                </th>
                <td colspan="5">
                    <asp:Label runat="server" ID="lblOrderNo" ClientIDMode="Static" Text="" Font-Bold="true"
                        Font-Size="15px" ForeColor="#000CCC"></asp:Label>
                </td>
            </tr>
            <tr>
                <th>
                    PROCESS CODE
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="ddlProcessID" ClientIDMode="Static" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlProcessID_IndexChange" CssClass="form-control" Width="100px">
                    </asp:DropDownList>
                </td>
                <th>
                    QTY
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtPrepareQty" ClientIDMode="Static" Text="" onkeypress="return isNumberWithoutDecimal(event)"
                        MaxLength="4" CssClass="form-control" Width="40px"></asp:TextBox>
                </td>
                <td>
                    <asp:CheckBox runat="server" ID="chkRim" ClientIDMode="Static" Text="TYRE & WHEEL"
                        TextAlign="Left" AutoPostBack="true" OnCheckedChanged="chkRim_CheckedChange" />
                </td>
                <td style="vertical-align: top;">
                    <table runat="server" cellspacing="0" rules="rows" border="0" style="display: none;
                        width: 300px;" id="tbEdcDetails">
                        <tr>
                            <th>
                                EDC
                            </th>
                            <td>
                                <asp:DropDownList runat="server" ID="ddl_EdcNo" ClientIDMode="Static" CssClass="form-control"
                                    Width="100px" AutoPostBack="true" OnSelectedIndexChanged="ddlEdcNo_IndexChange">
                                </asp:DropDownList>
                            </td>
                            <th>
                                WT
                            </th>
                            <td>
                                <asp:Label runat="server" ID="lblRimWt" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                            </td>
                            <th>
                                PRICE
                            </th>
                            <td>
                                <asp:Label runat="server" ID="lblRimPrice" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                DESC
                            </th>
                            <td colspan="5">
                                <asp:TextBox runat="server" ID="txt_RimDesc" CssClass="form-control" Width="300px"
                                    Text="" MaxLength="50"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="3" style="vertical-align: top;">
                    <asp:FormView runat="server" ID="frmProcessIDDetails" ClientIDMode="Static" Font-Bold="true"
                        Width="100%">
                        <ItemTemplate>
                            <%# Eval("TyreSize")%>&nbsp<%# Eval("RimSize")%>&nbsp<%# Eval("TyreType")%><br />
                            <%# Eval("Brand")%>&nbsp<%# Eval("Sidewall")%>&nbsp<%# Eval("TypeDesc")%><br />
                            WT:&nbsp;<%# Eval("FinishedWt")%>&nbsp;PRICE:&nbsp;<%# Eval("UnitPrice")%>
                        </ItemTemplate>
                    </asp:FormView>
                </td>
                <td colspan="3" style="vertical-align: top;">
                    <asp:Label runat="server" ID="lblErrmsg" ClientIDMode="Static" Text="" ForeColor="Red"
                        Font-Bold="true"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="6" style="text-align: center;">
                    <asp:Button runat="server" ID="btnAddItem" ClientIDMode="Static" Text="ADD ITEM"
                        OnClick="btnAddItem_Click" OnClientClick="javascript:return CtrlAddItem();" CssClass="btn btn-info" />
                </td>
            </tr>
            <tr>
                <td colspan="6">
                    <asp:GridView runat="server" ID="gvPrepareItems" AutoGenerateColumns="false" Width="100%"
                        ShowFooter="true" FooterStyle-Font-Bold="true" FooterStyle-HorizontalAlign="Right"
                        RowStyle-Font-Bold="true" FooterStyle-BackColor="#e0fff4" OnRowDeleting="gvPrepareItems_RowDeleting">
                        <HeaderStyle BackColor="#FEFE8B" Font-Bold="false" Height="22px" />
                        <Columns>
                            <asp:BoundField DataField="tyresize" HeaderText="TYRE SIZE" />
                            <asp:BoundField DataField="rimsize" HeaderText="RIM" />
                            <asp:BoundField DataField="tyretype" HeaderText="TYPE" />
                            <asp:BoundField DataField="brand" HeaderText="BRAND" />
                            <asp:BoundField DataField="sidewall" HeaderText="SIDEWALL" />
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
                            <asp:BoundField DataField="itemqty" HeaderText="QTY" ItemStyle-HorizontalAlign="Right" />
                            <asp:TemplateField HeaderText="ADDITIONAL INFO">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblRimStatus" Text='<%# Eval("AssyRimstatus").ToString() == "True" ? "TYRE & WHEEL" : "" %>'></asp:Label>
                                    <%#  Eval("RimDwg")%></ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="unitprice" HeaderText="UNIT PRICE" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="unitwt" HeaderText="UNIT WT" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="totprice" HeaderText="TOTAL PRICE" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="totwt" HeaderText="TOTAL WT(Kgs)" ItemStyle-HorizontalAlign="Right" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hdnOItem" ClientIDMode="Static" Value='<%# Eval("O_ItemID") %>' />
                                    <asp:Button runat="server" ID="btnDelete" Text="X" CommandName="Delete" CssClass="btn btn-warning" /></ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="5">
                </td>
                <td style="text-align: center;">
                    <asp:Button runat="server" ID="btnCompleted" ClientIDMode="Static" Text="MOVE TO ORDER COMPLETE"
                        OnClick="btnCompleted_Click" CssClass="btn btn-success" />
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        function CtrlAddItem() {
            var errmsg = ''; $('#lblErrmsg').html();
            if ($('#ddlProcessID option:selected').val() == 'CHOOSE')
                errmsg += 'Choose Process Code<br/>';
            if ($('#txtPrepareQty').val() == '' || parseInt($('#txtPrepareQty').val()) == 0)
                errmsg += 'Enter qty<br/>';
            if ($('#chkRim').is(':checked') == true) {
                if ($('#ddl_EdcNo option:selected').val() == 'CHOOSE')
                    errmsg += 'Choose Edc No<br/>';
                if ($('#lblRimWt').html() == "" || $('#lblRimPrice').html() == "")
                    errmsg += 'Price sheet not available to edc no';
            }
            if (errmsg.length > 0) {
                $('#lblErrmsg').html(errmsg);
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
