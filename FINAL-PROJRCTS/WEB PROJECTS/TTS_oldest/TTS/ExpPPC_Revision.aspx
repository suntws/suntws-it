<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="ExpPPC_Revision.aspx.cs" Inherits="TTS.ExpPPC_Revision" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="Scripts/cotsScript.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: center;" class="pageTitleHead">
        <asp:Label runat="server" ID="lblPageHead" ClientIDMode="Static" Text=""></asp:Label>
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
                    <asp:GridView ID="gv_PPCOrders" Width="100%" runat="server" AutoGenerateColumns="false"
                        RowStyle-Height="30px">
                        <HeaderStyle BackColor="#74bbf9" ForeColor="White" Font-Bold="true" Height="25px"
                            HorizontalAlign="Center" />
                        <Columns>
                            <asp:TemplateField HeaderText="CUSTOMER NAME">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatusCustName" Text='<%# Eval("custfullname") %>'></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdnStatusCustCode" Value='<%# Eval("CustCode") %>' />
                                    <asp:HiddenField runat="server" ID="hdnContainerLoadFrom" Value='<%# Eval("ContainerLoadFrom") %>' />
                                    <asp:HiddenField runat="server" ID="hdnOrderStatus" Value='<%# Eval("OrderStatus") %>' />
                                    <asp:HiddenField runat="server" ID="hdnOrderID" Value='<%# Eval("ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ORDER REFERENCE NO">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOrderRefNo" Text='<%#Eval("OrderRefNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="WORK ORDER" DataField="workorderno" />
                            <asp:BoundField HeaderText="ORDER DATE" DataField="CompletedDate" ItemStyle-Width="65px" />
                            <asp:BoundField HeaderText="CUSTOMER DESIRED SHIP DATE" DataField="DesiredShipDate"
                                ItemStyle-Width="65px" />
                            <asp:BoundField HeaderText="PPC RFD" DataField="DesiredShipDate" ItemStyle-Width="65px" />
                            <asp:BoundField HeaderText="ORDER QTY" DataField="itemqty" ItemStyle-Width="40px" />
                            <asp:TemplateField HeaderText="SHIPMENT TYPE" ItemStyle-Width="45px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblShipmentType" Text='<%# Eval("ShipmentType") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="STATUS">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatusText" Text='<%# Eval("StatusText") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PLANT">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblPlant" Text='<%# Eval("Plant") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ACTION">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkReviseOrder" CssClass="btn btn-success" runat="server" OnClick="lnkReviseOrder_click"
                                        Text="Request/ Response">
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
                        <div style="text-align: center; padding-left: 10px; padding-right: 10px; background-color: #3c763d;
                            overflow: hidden; height: 25px; font-size: 15px; color: #ffffff;">
                            <div style="float: left;">
                                <asp:Label ID="lblSelectedCustomerName" runat="server" Text="" Font-Bold="true"></asp:Label>
                            </div>
                            <div style="float: right;">
                                <asp:Label ID="lblSelectedOrderRefNo" ClientIDMode="Static" runat="server" Text=""
                                    Font-Bold="true"></asp:Label>
                            </div>
                        </div>
                        <div>
                            <asp:Label runat="server" ID="lblCurrStatus" ClientIDMode="Static" Text="" Font-Bold="true"
                                Font-Size="14px"></asp:Label>
                        </div>
                        <div>
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
                                </Columns>
                                <FooterStyle BackColor="#f7b26b" HorizontalAlign="Right" />
                            </asp:GridView>
                        </div>
                    </div>
                    <div id="div_Request" style="display: none;">
                        <span class="headCss" style="width: 100%; float: left; padding-top: 10px;">COMMENTS
                            FOR REVISE: </span>
                        <asp:TextBox runat="server" ID="txtppcrequestcommands" ClientIDMode="Static" Text=""
                            TextMode="MultiLine" Width="700px" Height="70px" onKeyUp="javascript:CheckMaxLength(this, 3999);"
                            onChange="javascript:CheckMaxLength(this, 3999);"></asp:TextBox>
                        <asp:Button runat="server" ID="btnppcrequestsend" ClientIDMode="Static" Text="REQUEST SEND"
                            CssClass="btnsave" OnClientClick="javascript:return requestsend();" OnClick="btnppcrequestsend_Click" />
                    </div>
                    <div id="div_response" style="display: none;">
                        <span class="headCss" style="width: 100%; float: left; padding-top: 10px;">COMMENTS
                            FOR RESPONSE: </span>
                        <asp:TextBox runat="server" ID="txtcrmresponsecomments" ClientIDMode="Static" Text=""
                            TextMode="MultiLine" Width="700px" Height="70px" onKeyUp="javascript:CheckMaxLength(this, 3999);"
                            onChange="javascript:CheckMaxLength(this, 3999);"></asp:TextBox>
                        <asp:Button runat="server" ID="btnPpcApprove" ClientIDMode="Static" Text="APPROVED FOR PPC REQUEST"
                            CssClass="btnsave" OnClientClick="javascript:return responseapprove();" OnClick="btnPpcApprove_Click" />
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField ID="hdnCustCode" runat="server" Value="" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnCurrStatusID" runat="server" Value="" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnPlant" runat="server" Value="" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnOID" runat="server" Value="" ClientIDMode="Static" />
    <script type="text/javascript">
        function requestsend() {
            if ($('#txtppcrequestcommands').val().length == 0) {
                alert('Enter comments for request');
                return false;
            }
            else
                return true;
        }

        function responseapprove() {
            if ($('#txtcrmresponsecomments').val().length == 0) {
                alert('Enter comments for approve');
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
