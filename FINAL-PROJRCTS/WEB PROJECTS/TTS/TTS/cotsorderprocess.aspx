<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cotsorderprocess.aspx.cs"
    MasterPageFile="~/master.Master" Inherits="TTS.cotsorderprocess" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .spanCss
        {
            color: Green;
            font-weight: bold;
            font-size: 14px;
            font-family: Times New Roman;
        }
        .tableCss
        {
            background-color: #ecf6ff;
            width: 100%;
        }
        .buttonProcess
        {
            background-color: #4CAF50;
            border: none;
            color: white;
            padding: 4px 10px;
            text-align: center;
            text-decoration: none;
            font-size: 12px;
            margin: 2px 1px;
            cursor: pointer;
            font-weight: bold;
            font-family: Times New Roman;
        }
        .buttonHold
        {
            background-color: #242f7d;
            border: none;
            color: white;
            padding: 4px 10px;
            text-align: center;
            text-decoration: none;
            font-size: 12px;
            margin: 2px 1px;
            cursor: pointer;
            font-weight: bold;
            font-family: Times New Roman;
        }
        .CreditSpan
        {
            font-size: 11px;
            font-weight: bold;
            color: #ff0000;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        <asp:Label runat="server" ID="lblpageHeading" Text=""></asp:Label>
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" class="tableCss">
            <tr>
                <td colspan="4">
                    <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <th>
                    <span class="spanCss">PLANT</span>
                </th>
                <td>
                    <asp:DropDownList ID="ddlplant" runat="server" Width="150px" Height="30px" ClientIDMode="Static"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlplant_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <th>
                    <span class="spanCss">NO OF ORDERS</span>
                </th>
                <td>
                    <asp:TextBox ID="txtOrderCount" runat="server" Enabled="false" Width="150px" Height="20px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:GridView runat="server" ID="gvReceivedOrderList" ClientIDMode="Static" AutoGenerateColumns="false"
                        Width="100%" AlternatingRowStyle-BackColor="#ecf6ff" 
                        RowStyle-Height="20px" 
                        onselectedindexchanged="gvReceivedOrderList_SelectedIndexChanged">
                        <HeaderStyle BackColor="#a1ccf3" Font-Bold="true" Height="22px" />
                        <Columns>
                            <asp:BoundField DataField="custfullname" HeaderText="CUSTOMER" />
                            <asp:BoundField DataField="OrderRefNo" HeaderText="ORDER REF NO" />
                            <asp:BoundField DataField="OrderDate" HeaderText="ORDER DATE" ItemStyle-Width="65px" />
                            <asp:BoundField HeaderText="LAST REVISED DATE" DataField="RevisedDate" />
                            <asp:BoundField HeaderText="DESIRED SHIP DATE" DataField="DesiredShipDate" />
                            <asp:BoundField HeaderText="PLANT" DataField="Plant" ItemStyle-HorizontalAlign="left" />
                            <asp:BoundField HeaderText="QTY" DataField="itemqty" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="StatusText" HeaderText="STATUS" ItemStyle-HorizontalAlign="Center" />
                            <asp:TemplateField HeaderText="ACTION" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hdnOrderID" Value='<%# Eval("id") %>' />
                                    <asp:Button ID="btnProcessingOrder" ClientIDMode="Static" runat="server" OnClick="lnkProcessOrders_Click"
                                        CssClass='<%# Eval("CustHoldStatus").ToString() == "True" ? "buttonHold" : "buttonProcess"%>'
                                        Text='<%# Eval("CustHoldStatus").ToString() == "True" ? "View Order" : "Process Order"%>' />
                                    <span class="CreditSpan">
                                        <%# Eval("CustHoldStatus").ToString() == "True" ? "CREDIT HOLD" : ""%></span>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
