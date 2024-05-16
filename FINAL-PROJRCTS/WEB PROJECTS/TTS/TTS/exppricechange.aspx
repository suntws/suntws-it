<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="exppricechange.aspx.cs" Inherits="TTS.exppricechange" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/popupBox.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/cotsScript.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        ORDER ITEMS PRICE CHANGE</div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="background-color: #dcecfb; width: 100%;
            border-color: White; border-collapse: separate;">
            <tr>
                <td colspan="2">
                    <asp:GridView runat="server" ID="gvPriceChangeOrderList" AutoGenerateColumns="false"
                        Width="100%" HeaderStyle-BackColor="#FFCC00" AlternatingRowStyle-BackColor="#f5f5f5"
                        OnRowDeleting="gvPriceChangeOrderList_RowChoose">
                        <Columns>
                            <asp:TemplateField HeaderText="CUSTOMER NAME">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatusCustName" Text='<%# Eval("custfullname") %>'></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdnCotsCustCode" Value='<%# Eval("CustCode") %>' />
                                    <asp:HiddenField runat="server" ID="hdnOrderID" Value='<%# Eval("ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ORDER REFERENCE NO">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOrderRefNo" Text='<%#Eval("OrderRefNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="ORDERD DATE" DataField="CompletedDate" />
                            <asp:BoundField HeaderText="LAST REVISED DATE" DataField="RevisedDate" />
                            <asp:BoundField HeaderText="QTY" DataField="itemqty" ItemStyle-HorizontalAlign="Right" />
                            <asp:TemplateField HeaderText="ACTION">
                                <ItemTemplate>
                                    <asp:Button runat="server" ID="btnPriceChange" Text="Show Item List" CommandName="Delete"
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
                <td colspan="2">
                    <div style="width: 100%; float: left; border: 1px solid #000; display: none; line-height: 20px;
                        margin-top: 10px; background-color: #F0E2F5; padding-top: 5px;" id="divStatusChange">
                        <div id="divOrderHead" style="width: 100%; float: left;">
                            <div style="width: 50%; float: left; text-align: left;">
                                <asp:Label runat="server" ID="lblCustName" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                            </div>
                            <div style="width: 50%; float: left; text-align: right;">
                                <asp:Label runat="server" ID="lblStausOrderRefNo" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                            </div>
                        </div>
                        <div style="width: 100%; float: left;">
                            <div style="width: 50%; float: left;">
                                <span class="headCss">Special Instruction</span>
                                <asp:TextBox runat="server" ID="txtOrderSplIns" Text="" TextMode="MultiLine" Width="520px"
                                    Height="100px" Enabled="false"></asp:TextBox>
                            </div>
                            <div style="width: 50%; float: right;">
                                <span class="headCss">Special Notes</span>
                                <asp:TextBox runat="server" ID="txtOrdersplReq" Text="" TextMode="MultiLine" Width="520px"
                                    Height="100px" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:GridView runat="server" ID="gvRevisePriceList" AutoGenerateColumns="false" Width="100%"
                        AlternatingRowStyle-BackColor="#f5f5f5" OnRowEditing="gvRevisePriceList_RowEditing"
                        OnRowUpdating="gvRevisePriceList_RowUpdating" OnRowCancelingEdit="gvRevisePriceList_RowCanceling">
                        <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" Height="22px" />
                        <Columns>
                            <asp:TemplateField ItemStyle-CssClass="headerNone" HeaderStyle-CssClass="headerNone">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblProcessid" Text='<%#Eval("processid") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CATEGORY" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <%#Eval("category")%>
                                    <%# Eval("AssyRimstatus").ToString() == "True" ? " (ASSY)" : ""%>
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
                            <asp:TemplateField HeaderText="BRAND" ItemStyle-Width="80px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblBrand" Text='<%#Eval("brand") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SIDEWALL" ItemStyle-Width="80px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblSidewall" Text='<%#Eval("sidewall") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PRICE" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblChangePrice" Text='<%#Eval("unitprice") %>'></asp:Label></ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox runat="server" ID="txtChangePrice" onkeypress="return isNumberAndMinusKey(event)"
                                        Width="70px" MaxLength="12" BackColor="#f9c232" Text='<%# Eval("unitprice") %>'
                                        title="price"></asp:TextBox>
                                    <asp:HiddenField runat="server" ID="hdnCurrentPrice" Value='<%# Eval("unitprice") %>' />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="QTY" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblItemQty" Text='<%#Eval("itemqty") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="RIM PRICE" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblRimChangePrice" Text='<%#Eval("Rimunitprice") %>'
                                        Visible='<%# Eval("Rimunitprice").ToString() == "0.00" ? false : true%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox runat="server" ID="txtRimChangePrice" onkeypress="return isNumberAndMinusKey(event)"
                                        Width="70px" MaxLength="12" BackColor="#f9c232" Text='<%# Eval("Rimunitprice") %>'
                                        Visible='<%# Eval("Rimunitprice").ToString() == "0.00" ? false : true%>'></asp:TextBox>
                                    <asp:HiddenField runat="server" ID="hdnRimCurrentPrice" Value='<%# Eval("Rimunitprice") %>' />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="RIM QTY" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblRimItemQty" Text='<%# Eval("Rimitemqty").ToString() == "0" ? "" : Eval("Rimitemqty").ToString()%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="EDC NO">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblEdcNo" ClientIDMode="Static" Text='<%# Eval("EdcNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ACTION" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:Button runat="server" ID="btnEdit" Text="Change Price" CommandName="Edit" CssClass="btnedit" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Button runat="server" ID="btnUpdate" Text="Update" CommandName="Update" />
                                    <asp:Button runat="server" ID="btnCancel" Text="Cancel" CommandName="Cancel" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr id="divPriceChange" style="display: none; text-align: left; vertical-align: middle;">
                <td>
                    If price changes done, Click here for next process.
                </td>
                <td>
                    <asp:Button runat="server" ID="btnPriceChangeCompleted" Text="Revision Completed"
                        CssClass="btnedit" OnClick="btnPriceChangeCompleted_Click" />
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnCustCode" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnOID" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnPriceChangeStatus" ClientIDMode="Static" Value="0" />
    <script type="text/javascript">
        $(document).ready(function () {
            blinkOrderHead();
        });
    </script>
</asp:Content>
