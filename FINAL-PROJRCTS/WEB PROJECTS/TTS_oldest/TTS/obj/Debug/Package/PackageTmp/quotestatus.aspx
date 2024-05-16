<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="quotestatus.aspx.cs" Inherits="TTS.quotestatus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    &nbsp;&nbsp;
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div align="center" class="pageTitleHead">
        CONFORMED QUOTATION
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <div style="padding-left: 5px;">
            <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #000;
                width: 1070px;">
                <tr>
                    <td align="center">
                        <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" Font-Bold="true"
                            ForeColor="Red" Font-Size="18px"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView runat="server" ID="gvConfirmedQuote" AutoGenerateColumns="false" Width="1063px"
                            RowStyle-Height="24px">
                            <HeaderStyle BackColor="#CACA55" Font-Bold="true" Height="22px" />
                            <Columns>
                                <asp:BoundField DataField="QCustType" HeaderText="CUSTOMER" ItemStyle-Width="100px" />
                                <asp:TemplateField HeaderText="TYPE OF CUSTOMER" ItemStyle-Width="120px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustomerType" runat="server" ClientIDMode="Static" Text='<%# Eval("customer") %>' /></span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="NAME" ItemStyle-Width="200px">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblQCustomer" ClientIDMode="Static" Text='<%# Eval("QCustomer") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="QUOTE REF NO." ItemStyle-Width="150px">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblQAcYear" ClientIDMode="Static" Text='<%#Eval("QAcYear") %>'></asp:Label>
                                        <asp:Label runat="server" ID="lblQRefNo" ClientIDMode="Static" Text='<%#Eval("QRefNo") %>'></asp:Label>
                                        <asp:HiddenField ID="hdnUSERID" runat="server" ClientIDMode="Static" Value='<%#Eval("USERID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="createddate" HeaderText="SENT DATE" ItemStyle-Width="80px" />
                                <asp:BoundField DataField="UserName" HeaderText="SENT BY" ItemStyle-Width="120px" />
                                <asp:BoundField DataField="confirmeddate" HeaderText="CONFIRMED DATE" ItemStyle-Width="120px" />
                                <asp:BoundField DataField="totalQty" HeaderText="QTY" ItemStyle-Width="60px" />
                                <asp:TemplateField HeaderText="ACTION" ItemStyle-Width="120px" ItemStyle-CssClass="headerNone">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkMoveToOriginal" runat="server" Text="MOVE TO S-COTS" OnClick="lnkMoveToOriginal_Click" /></span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr style="display: none;" id="divQuoteConfirm">
                    <td>
                        <div style="width: 1063px; float: left;" id="quoteHeadDiv">
                            <div style="width: 530px; float: left; text-align: left;">
                                <asp:Label runat="server" ID="lblQuoteCustName" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                            </div>
                            <div style="width: 530px; float: left; text-align: right;">
                                <asp:Label runat="server" ID="lblQuoteAcYear" ClientIDMode="Static" Font-Bold="true"></asp:Label>
                                <asp:Label runat="server" ID="lblQuoteRefNo" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label></div>
                        </div>
                        <asp:DataList runat="server" ID="dlQuoteCustDetails" RepeatColumns="1" RepeatDirection="Horizontal"
                            RepeatLayout="Table" AlternatingItemStyle-BackColor="#A1C9D3" ItemStyle-BackColor="#C9DAC9"
                            ItemStyle-VerticalAlign="Top" Width="1063px">
                            <ItemTemplate>
                                <div style="width: 530px; float: left;">
                                    <div>
                                        <%# Eval("CustFullName")%></div>
                                    <div>
                                        TIN NO.: &nbsp;
                                        <%# Eval("TinNo")%></div>
                                    <div>
                                        CST NO.: &nbsp;
                                        <%# Eval("CstNo")%></div>
                                    <div>
                                        ECC NO.: &nbsp;
                                        <%# Eval("EccNo")%></div>
                                    <div>
                                        ECC DETAILS.: &nbsp;
                                        <%# Eval("EccDetails")%></div>
                                    <div>
                                        COMMENTS: &nbsp;
                                        <%# Eval("EccDetails")%></div>
                                </div>
                                <div style="width: 530px; float: left;">
                                    <div>
                                        <div style="font-weight: bold; font-size: 14px; color: #0EA739; text-decoration: underline;
                                            padding-bottom: 5px;">
                                            BILLING ADDRESS:
                                        </div>
                                        <div>
                                            <%# Eval("BillAddress")%></div>
                                        <div>
                                            <%# Eval("BillCity")%></div>
                                        <div>
                                            <%# Eval("BillStateName")%></div>
                                        <div>
                                            <%# Eval("BillZipCode")%></div>
                                    </div>
                                    <div>
                                        <div style="font-weight: bold; font-size: 14px; color: #0E45A7; text-decoration: underline;
                                            padding-bottom: 5px; padding-top: 10px;">
                                            SHIPPING ADDRESS:
                                        </div>
                                        <div>
                                            <%# Eval("ShipAddress")%></div>
                                        <div>
                                            <%# Eval("ShipCity")%></div>
                                        <div>
                                            <%# Eval("ShipStateName")%></div>
                                        <div>
                                            <%# Eval("ShipZipCode")%></div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:DataList>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdnSelectIndex" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        $(document).ready(function () {
            blinkQuoteHead();
        });

        function gotoConfirmQuoteDiv(ctrlID) {
            $('#' + ctrlID).css({ 'display': 'block' });
            $("html, body").stop().animate({
                scrollLeft: $('#' + ctrlID).offset().left,
                scrollTop: $('#' + ctrlID).offset().top
            }, 1200);
        }

        function blinkQuoteHead() {
            $('#quoteHeadDiv').css({ 'background-color': '#AF9AFB', 'color': '#000' });
            setTimeout("setblinkQuoteHead()", 1000)
        }

        function setblinkQuoteHead() {
            $('#quoteHeadDiv').css({ 'background-color': '#660053', 'color': '#fff' });
            setTimeout("blinkQuoteHead()", 1000)
        }
    </script>
</asp:Content>
