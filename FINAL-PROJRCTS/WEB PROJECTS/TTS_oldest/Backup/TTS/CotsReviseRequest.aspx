<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CotsReviseRequest.aspx.cs"
    MasterPageFile="~/master.Master" Inherits="TTS.CotsReviseRequest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="Scripts/cotsScript.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        DOMESTIC ORDER REVISE REQUEST</div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="width: 100%; border-color: White;
            border-collapse: separate;">
            <tr>
                <td style="text-align: center;">
                    <span>PLANT: </span>
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
                    <asp:GridView runat="server" ID="gvOrderDetails" AutoGenerateColumns="false" Width="100%"
                        HeaderStyle-BackColor="#FFCC00" AlternatingRowStyle-BackColor="#f5f5f5">
                        <Columns>
                            <asp:TemplateField HeaderText="CUSTOMER NAME" ItemStyle-Width="250px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatusCustName" Text='<%# Eval("custfullname") %>'></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdnStatusCustCode" Value='<%# Eval("CustCode") %>' />
                                    <asp:HiddenField runat="server" ID="hdnUserCurrency" Value='<%# Eval("usercurrency") %>' />
                                    <asp:HiddenField runat="server" ID="hdnStatusID" Value='<%# Eval("OrderStatus") %>' />
                                    <asp:HiddenField runat="server" ID="hdnOrderID" Value='<%# Eval("ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ORDER REF NO." ItemStyle-Width="250px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOrderRefNo" Text='<%#Eval("OrderRefNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ORDER DATE" ItemStyle-Width="60px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOrderDate" Text='<%#Eval("CompletedDate") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="DESIRED SHIP DATE" ItemStyle-Width="60px" DataField="DesiredShipDate" />
                            <asp:BoundField HeaderText="QTY" ItemStyle-Width="30px" DataField="itemqty" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField HeaderText="PLANT" DataField="Plant" ItemStyle-Width="40px" />
                            <asp:TemplateField HeaderText="STATUS" ItemStyle-Width="240px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatusText" Text='<%# Eval("StatusText") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="50px" HeaderText="ACTION" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkTrackBtn" runat="server" Text="REQUEST" OnClick="lnkRequest_Click" /></span>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <table id="div_SingleView" style="display: none; width: 100%;">
                        <tr>
                            <td colspan="2">
                                <div id="divOrderHead" style="width: 100%; height: 20px; float: left;">
                                    <div style="width: 50%; float: left; text-align: left;">
                                        <asp:Label runat="server" ID="lblCustName" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                                    </div>
                                    <div style="width: 50%; float: left; text-align: right;">
                                        <asp:Label runat="server" ID="lblStausOrderRefNo" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label></div>
                                </div>
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
                                        Width="100%" AlternatingRowStyle-BackColor="#f5f5f5" ShowFooter="true" FooterStyle-Font-Bold="true"
                                        FooterStyle-BackColor="#abaede" FooterStyle-HorizontalAlign="Right">
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
                                            <asp:TemplateField HeaderText="RIM BASIC PRICE" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px">
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
                            <td>
                                <span class="headCss">REQUEST PURPOSE: </span>
                                <asp:RadioButtonList runat="server" ID="rdbrequestcrm" ClientIDMode="Static" Width="350px"
                                    Font-Bold="true">
                                </asp:RadioButtonList>
                            </td>
                            <td>
                                <span class="headCss">COMMENTS FOR REVISE: </span>
                                <asp:TextBox runat="server" ID="txtrequestcommands" ClientIDMode="Static" Text=""
                                    TextMode="MultiLine" Width="700px" Height="70px" onKeyUp="javascript:CheckMaxLength(this, 3999);"
                                    onChange="javascript:CheckMaxLength(this, 3999);"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblErrRequestSend" ClientIDMode="Static" Text="" Font-Bold="true"
                                    Font-Size="12px" ForeColor="Red"></asp:Label>
                            </td>
                            <td align="center">
                                <asp:Button runat="server" ID="btnrequestsend" ClientIDMode="Static" Text="REQUEST SEND"
                                    CssClass="btnsave" OnClientClick="javascript:return requestsend();" OnClick="btnrequestsend_Click" />
                                <div style="width: 450px; float: left; font-size: 15px; color: #07635e; font-weight: bold;">
                                    YOU SHOULD GET APPROVAL FROM
                                    <asp:Label runat="server" ID="lblApproveTeam" ClientIDMode="Static" Text=""></asp:Label>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnCustCode" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnApprovalID" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnApprovalTeam" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnPlant" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnOID" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        $(document).ready(function () {
            blinkOrderHead();
        });
        function requestsend() {
            var errmsg = '';
            if ($('input:radio[id*=rdbrequestcrm_]:checked').length == 0)
                errmsg += ' Select request purpose <br/>';
            if ($('#txtrequestcommands').val().length == 0)
                errmsg += 'Enter comments for request <br/>'
            if (errmsg.length > 0) {
                $('#lblErrRequestSend').html(errmsg);
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
