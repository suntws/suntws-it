<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CotsReviseResponse.aspx.cs"
    MasterPageFile="~/master.Master" Inherits="TTS.CotsReviseResponse" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="Scripts/cotsScript.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        DOMESTIC ORDER REVISE APPROVAL</div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="background-color: #ecf6ff; width: 100%;
            border-color: White;">
            <tr>
                <td style="text-align: center;">
                    <span>PLANT : </span>
                    <asp:DropDownList ID="ddlplant" runat="server" ClientIDMode="Static" AutoPostBack="true"
                        Width="120px" OnSelectedIndexChanged="ddlplant_SelectedIndexChanged">
                        <asp:ListItem Text="ALL" Value="ALL" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="MMN" Value="MMN"></asp:ListItem>
                        <asp:ListItem Text="PDK" Value="PDK"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView runat="server" ID="gvRequestedOrderDetails" AutoGenerateColumns="false"
                        Width="100%" HeaderStyle-BackColor="#FFCC00" AlternatingRowStyle-BackColor="#f5f5f5">
                        <Columns>
                            <asp:TemplateField HeaderText="CUSTOMER NAME" ItemStyle-Width="200px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatusCustName" Text='<%# Eval("custfullname") %>'></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdnStatusCustCode" Value='<%# Eval("CustCode") %>' />
                                    <asp:HiddenField runat="server" ID="hdnReviseRequestTo" Value='<%# Eval("ReviseRequestTo") %>' />
                                    <asp:HiddenField runat="server" ID="hdnOrderStatus" Value='<%# Eval("OrderStatus") %>' />
                                    <asp:HiddenField runat="server" ID="hdnOrderID" Value='<%# Eval("ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ORDER REFERENCE NO" ItemStyle-Width="220px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOrderRefNo" Text='<%#Eval("OrderRefNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ORDER DATE" ItemStyle-Width="60px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOrderDate" Text='<%#Eval("CompletedDate") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="CUSTOMER DESIRED SHIP DATE" ItemStyle-Width="60px" DataField="DesiredShipDate" />
                            <asp:BoundField HeaderText="QTY" ItemStyle-Width="30px" DataField="itemqty" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField HeaderText="PLANT" DataField="Plant" ItemStyle-Width="40px" />
                            <asp:TemplateField HeaderText="STATUS" ItemStyle-Width="270px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatusText" Text='<%# Eval("StatusText") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="50px" HeaderText="ACTION" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkApproveBtn" runat="server" Text="APPROVE" OnClick="lnkApproveBtn_Click"
                                        ClientIDMode="Static" /></span>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <table id="div_SingleView" style="display: none;">
                        <tr id="divOrderHead">
                            <td>
                                <asp:Label runat="server" ID="lblCustName" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblStausOrderRefNo" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="background-color: #efc8a7; width: 100%">
                                <asp:Label runat="server" ID="lblCurrStatus" ClientIDMode="Static" Text="" Font-Bold="true"
                                    Font-Size="14px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div style="width: 100%; float: left;">
                                    <asp:GridView runat="server" ID="gvSingleView_OrderItemList" AutoGenerateColumns="false"
                                        Width="100%" AlternatingRowStyle-BackColor="#f5f5f5" ShowFooter="true" FooterStyle-HorizontalAlign="Right"
                                        FooterStyle-Font-Bold="true">
                                        <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" Height="22px" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="CATEGORY" ItemStyle-Width="80px">
                                                <ItemTemplate>
                                                    <%# Eval("category") %>
                                                    <%# Eval("AssyRimstatus").ToString() == "True" ? " (ASSY)" : ""%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="PLATFORM" DataField="config" ItemStyle-Width="100px" />
                                            <asp:BoundField HeaderText="TYRE SIZE" DataField="tyresize" ItemStyle-Width="150px" />
                                            <asp:BoundField HeaderText="RIM" DataField="rimsize" ItemStyle-Width="40px" />
                                            <asp:BoundField HeaderText="TYPE" DataField="tyretype" ItemStyle-Width="60px" />
                                            <asp:BoundField HeaderText="BRAND" DataField="brand" ItemStyle-Width="100px" />
                                            <asp:BoundField HeaderText="SIDEWALL" DataField="sidewall" ItemStyle-Width="100px" />
                                            <asp:BoundField HeaderText="QTY" DataField="itemqty" ItemStyle-HorizontalAlign="Right"
                                                ItemStyle-Width="40px" />
                                            <asp:BoundField HeaderText="BASIC PRICE" DataField="listprice" ItemStyle-HorizontalAlign="Right"
                                                ItemStyle-Width="100px" />
                                            <asp:BoundField HeaderText="FWT" DataField="tyrewt" ItemStyle-HorizontalAlign="Right"
                                                ItemStyle-Width="80px" />
                                            <asp:TemplateField HeaderText="RIM QTY" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="40px">
                                                <ItemTemplate>
                                                    <%# Eval("Rimitemqty").ToString() == "0" ? "" : Eval("Rimitemqty").ToString()%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="RIM BASIC PRICE" ItemStyle-HorizontalAlign="Right"
                                                ItemStyle-Width="100px">
                                                <ItemTemplate>
                                                    <%# Eval("Rimunitprice").ToString() == "0.00" ? "" : Eval("Rimunitprice").ToString()%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="RIM FWT" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="80px">
                                                <ItemTemplate>
                                                    <%# Eval("Rimfinishedwt").ToString() == "0.00" ? "" : Eval("Rimfinishedwt").ToString()%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="TOTAL PRICE" DataField="unitprice" ItemStyle-HorizontalAlign="Right"
                                                ItemStyle-Width="120px" />
                                            <asp:BoundField HeaderText="TOTAL FWT" DataField="finishedwt" ItemStyle-HorizontalAlign="Right"
                                                ItemStyle-Width="100px" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top;">
                                <span class="headCss">Requested Comments from CRM: </span>
                                <asp:TextBox runat="server" ID="txtRequestedCommands" ClientIDMode="Static" Text=""
                                    TextMode="MultiLine" Width="520px" Height="100px" Enabled="false"></asp:TextBox>
                            </td>
                            <td>
                                <span class="headCss">Approved comments: </span>
                                <asp:TextBox runat="server" ID="txtResponsedCommands" ClientIDMode="Static" Text=""
                                    TextMode="MultiLine" Width="520px" Height="100px" onKeyUp="javascript:CheckMaxLength(this, 3999);"
                                    onChange="javascript:CheckMaxLength(this, 3999);"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblErrRequestSend" ClientIDMode="Static" Text="" Font-Bold="true"
                                    Font-Size="12px" ForeColor="Red"></asp:Label>
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnResponsesend" ClientIDMode="Static" Text="APPROVE"
                                    CssClass="btnsave" OnClientClick="javascript:return requestsend();" OnClick="btnResponsesend_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnSelectIndex" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnCustCode" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnApproveBy" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnResponseID" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnPlant" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnOrderStatusID" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnOID" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        $(document).ready(function () {
            blinkOrderHead();
        });
        function requestsend() {
            if ($('#txtResponsedCommands').val().length == 0) {
                $('#lblErrRequestSend').html('Enter approved comments');
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
