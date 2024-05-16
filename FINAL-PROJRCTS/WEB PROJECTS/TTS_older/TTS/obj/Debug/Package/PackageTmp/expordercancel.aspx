<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="expordercancel.aspx.cs" Inherits="TTS.expordercancel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: center;" class="pageTitleHead">
        Export Orders -> Item Revise
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <asp:ScriptManager runat="server" ID="ScriptManager1">
    </asp:ScriptManager>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="background-color: #dcecfb; width: 100%;
            border-color: White; border-collapse: separate;">
            <tr>
                <td>
                    <asp:GridView runat="server" ID="gvCancelOrderList" AutoGenerateColumns="false" Width="100%"
                        HeaderStyle-BackColor="#FFCC00" AlternatingRowStyle-BackColor="#f5f5f5">
                        <Columns>
                            <asp:TemplateField HeaderText="CUSTOMER NAME" ItemStyle-Width="200px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatusCustName" Text='<%# Eval("custfullname") %>'></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdnOrderCustCode" Value='<%# Eval("CustCode") %>' />
                                    <asp:HiddenField runat="server" ID="hdnRequestStatus" Value='<%# Eval("RequestStatus") %>' />
                                    <asp:HiddenField runat="server" ID="hdnOrderID" Value='<%# Eval("ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ORDER REF NO." ItemStyle-Width="220px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOrderRefNo" Text='<%# Eval("OrderRefNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="ORDERD DATE" DataField="CompletedDate" ItemStyle-Width="60px" />
                            <asp:BoundField HeaderText="LAST REVISED DATE" DataField="RevisedDate" ItemStyle-Width="60px" />
                            <asp:BoundField HeaderText="CUSTOMER DESIRED SHIP DATE" DataField="DesiredShipDate"
                                ItemStyle-Width="60px" />
                            <asp:BoundField HeaderText="QTY" DataField="itemqty" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Right" />
                            <asp:TemplateField HeaderText="STATUS" ItemStyle-Width="160px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatusText" Text='<%# Eval("StatusText") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ACTION" ItemStyle-Width="80px">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkOrderCancel" runat="server" Text="Cancel" OnClick="lnkOrderCancel_Click"
                                        Visible='<%# Eval("CustHoldStatus").ToString() == "True" ? false : true%>' /><span
                                            style="color: #ff0000; font-style: italic;">
                                            <%# Eval("CustHoldStatus").ToString() == "True" ? "CREDIT HOLD" : ""%></span></ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <div runat="server" clientidmode="Static" style="width: 100%; float: left; padding-top: 10px;"
                        id="divCancelBtn">
                        <div id="divOrderHead" style="width: 100%; height: 20px; float: left;">
                            <div style="width: 50%; float: left; text-align: left;">
                                <asp:Label runat="server" ID="lblCustName" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                            </div>
                            <div style="width: 50%; float: left; text-align: right;">
                                <asp:Label runat="server" ID="lblStausOrderRefNo" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label></div>
                        </div>
                        <span class="headCss">Reason for cancel this order :</span>
                        <asp:TextBox runat="server" ID="txtCotsOrderCancelFeedBack" ClientIDMode="Static"
                            MaxLength="1000" TextMode="MultiLine" Width="1050px" Height="100px"></asp:TextBox>
                        <asp:Button runat="server" ID="btnSaveOrderCancel" ClientIDMode="Static" CssClass="btnclear"
                            Text="SAVE ORDER CANCEL DETAILS" OnClick="btnSaveOrderCancel_Click" OnClientClick="javascript:return CtrlCancelTxtVal();" />
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnCustCode" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnOID" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        function CtrlCancelTxtVal() {
            if ($('#txtCotsOrderCancelFeedBack').val().length == 0) {
                alert('Enter the reason for cancel this order');
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
