<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="Cotsfttordertrrack.aspx.cs" Inherits="TTS.Cotsfttordertrrack" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="Scripts/cotsScript.js" type="text/javascript"></script>
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        <asp:Label runat="server" ID="lblHeadPlant" Text=""></asp:Label>
        FITMENT ORDER
        <asp:Label runat="server" ID="lblheadSufix" Text="" /></div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" class="tableCss" style="background-color: #FEF5E7;
            width: 100%;">
            <tr style="background-color: #ABEBC6;">
                <th>
                    YEAR
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="ddlYear" ClientIDMode="Static" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlYear_IndexChange" Width="180px" CssClass="form-control">
                    </asp:DropDownList>
                </td>
                <th>
                    MONTH
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="ddlMonth" ClientIDMode="Static" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlMonth_IndexChange" Width="180px" CssClass="form-control">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:GridView runat="server" class="gridcss" ID="gvdispatchlist" AutoGenerateColumns="false"
                        Width="100%" RowStyle-Height="20px">
                        <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" Height="28px" />
                        <Columns>
                            <asp:TemplateField HeaderText="CUSTOMER NAME">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatusCustName" ClientIDMode="Static" Text='<%#Eval("custfullname")%>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="FITMENT ORDER NO">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblFtNo" ClientIDMode="Static" Text='<%#Eval("FtNo").ToString().Replace("~"," / ")%>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="INVOICE NO">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdnCustCode" runat="server" ClientIDMode="Static" Value='<%#Eval("CustCode")%>' />
                                    <asp:HiddenField ID="hdngvyear" runat="server" ClientIDMode="Static" Value='<%#Eval("InvoiceYear")%>' />
                                    <asp:Label runat="server" ID="lblinvoice" ClientIDMode="Static" Text='<%#Eval("Invoiceno")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DISPATCHED QTY" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <%#Eval("qty")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DISPATCHED DATE">
                                <ItemTemplate>
                                    <%#Eval("createddate")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ACTION" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkShow" runat="server" Text="View Details" OnClick="lnkShow_Click" /></span>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <table cellspacing="0" rules="all" class="tableCss" style="background-color: #FEF5E7;
                        width: 100%; display: none;" id="divFtDetails">
                        <tr id="divOrderHead" style="line-height: 30px; font-size: 14px;">
                            <td>
                                <asp:Label runat="server" ID="lblCustName" ClientIDMode="Static" Style="text-align: left;"
                                    Text="" Font-Bold="true"></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblStausInvoiceNo" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:GridView runat="server" ID="gvFtInvoiceDownload" AutoGenerateColumns="false"
                                    Width="1105px" RowStyle-Height="20px">
                                    <HeaderStyle BackColor="#AAD8A6" Font-Bold="true" Height="22px" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="INVOICE NO" ItemStyle-Width="20px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblDownloadInvoiceNo" Text='<%# Eval("Invoiceno")%>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdnDownloadUInvoiceYear" Value='<%# Eval("InvoiceYear") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="INVOICE DETAILS" ItemStyle-Width="600px" ItemStyle-VerticalAlign="Top">
                                            <ItemTemplate>
                                                <div style="width: 600px; background-color: #E0F8E0; float: left; line-height: 20px;">
                                                    <div style="width: 400px; float: left;">
                                                        <%# Eval("DispatchMethod").ToString() == "" ? "" : "<span class='headCss' style='width: 130px; float: left;'>DELIVERY METHOD :</span>" + Eval("DispatchMethod").ToString() + " CUSTOMER <br />"%>
                                                        <%# Eval("ContactPerson").ToString() == "" ? "" : "<span class='headCss' style='width: 130px; float: left;'>CONTACT PERSON :</span>" + Eval("ContactPerson").ToString() + " <br />"%>
                                                        <%# Eval("Contactno").ToString() == "" ? "" : "<span class='headCss' style='width: 130px; float: left;'>CONTACT NO :</span>" + Eval("Contactno").ToString() + " <br />"%>
                                                        <%# Eval("VehicleNo").ToString() == "" ? "" : "<span class='headCss' style='width: 130px; float: left;'>VEHICLE REGN NO :</span>" + Eval("VehicleNo").ToString() + " <br />"%>
                                                        <%# Eval("DeliveryTo").ToString() == "" ? "" : "<span class='headCss' style='width: 130px; float: left;'>DELIVERY TO :</span>" + Eval("DeliveryTo").ToString() + (Eval("GoDown").ToString()!=""?" ("+ Eval("GoDown").ToString()+")":"")+" <br />"%>
                                                        <%# Eval("Transpoter").ToString() == "" ? "" : "<span class='headCss' style='width: 130px; float: left;'>TRANSPORTER :</span>" + Eval("Transpoter").ToString() + " <br />"%>
                                                        <%# Eval("LrNo").ToString() == "" ? "" : "<span class='headCss' style='width: 130px; float: left;'>LR NO & DATE :</span>" + Eval("LrNo").ToString() + " & " + Eval("LrDate").ToString() + "<br />"%>
                                                    </div>
                                                    <div style="width: 200px; background-color: #E0F8E0; float: left;">
                                                        <%# Eval("TotalQty").ToString() == "" ? "" : "<span class='headCss' style='width: 130px; float: left;'>TOTAL QTY:</span>" + Eval("TotalQty").ToString() + " <br />"%>
                                                        <%# Eval("Total").ToString() == "" ? "" : "<span class='headCss' style='width: 130px; float: left;'>TOTAL AMT :</span>" + Eval("Total").ToString() + " <br />"%>
                                                        <%# Eval("CGST").ToString() == "" ? "" : "<span class='headCss' style='width: 130px; float: left;'>CGST @ " + Eval("CGST").ToString() + "% :</span>" + Eval("CGSTVal").ToString() + " <br />"%>
                                                        <%# Eval("SGST").ToString() == "" ? "" : "<span class='headCss' style='width: 130px; float: left;'>SGST @ " + Eval("SGST").ToString() + "% :</span>" + Eval("SGSTVal").ToString() + " <br />"%>
                                                        <%# Eval("IGST").ToString() == "" ? "" : "<span class='headCss' style='width: 130px; float: left;'>IGST @ " + Eval("IGST").ToString() + "% :</span>" + Eval("IGSTVal").ToString() + " <br />"%>
                                                        <%# Eval("GrandTotal").ToString() == "" ? "" : "<span class='headCss' style='width: 130px; float: left;'>GRAND TOTAL :</span>" + Eval("GrandTotal").ToString() + " <br />"%>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="COMMENTS" ItemStyle-Width="300px" ItemStyle-VerticalAlign="Top">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblcomments" ClientIDMode="Static" Text='<%#Eval("Comments").ToString().Replace("~","<br/>")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="DOWNLOAD" ItemStyle-Width="30px" ItemStyle-VerticalAlign="Top">
                                            <ItemTemplate>
                                                <div style="line-height: 20px;">
                                                    <asp:LinkButton runat="server" ID="Original_Recepient_Invoice" Text="ORIGINAL_FOR_RECEPIENT"
                                                        ClientIDMode="Static" OnClick="lnkInvoiceFile_Click"></asp:LinkButton>
                                                    <br />
                                                    <asp:LinkButton runat="server" ID="Duplicate_Transporter_Invoice" Text="DUPLICATE_FOR_TRANSPORTER"
                                                        ClientIDMode="Static" OnClick="lnkInvoiceFile_Click"></asp:LinkButton>
                                                    <br />
                                                    <asp:LinkButton runat="server" ID="Triplicate_Supplier_Invoice" Text="TRIPLICATE_FOR_SUPPLIER"
                                                        ClientIDMode="Static" OnClick="lnkInvoiceFile_Click"></asp:LinkButton>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:GridView runat="server" ID="gvdispatchitem" AutoGenerateColumns="false" Width="520px"
                                    RowStyle-Height="20px">
                                    <HeaderStyle BackColor="#AAD8A6" Font-Bold="true" Height="22px" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="FITMENT ORDER NO" ItemStyle-Width="80px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblFtno" ClientIDMode="Static" Text='<%#Eval("FtNo")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="TYRE SIZE" ItemStyle-Width="200px">
                                            <ItemTemplate>
                                                <%#Eval("TyreSize")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="FITMENT CHARGE" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <%#Eval("Rate")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="DISPATCHED QTY" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <%#Eval("DispatchedQty")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="INVOICE NO" ItemStyle-Width="60px">
                                            <ItemTemplate>
                                                <%#Eval("Invoiceno")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hdnCotsCustID" runat="server" ClientIDMode="Static" Value='' />
        <asp:HiddenField ID="hdnyear" runat="server" ClientIDMode="Static" Value='' />
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            blinkOrderHead();
        });
        function Hidedispatch() {
            $('#divFtDetails').show();
        }
    </script>
</asp:Content>
