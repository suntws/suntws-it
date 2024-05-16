<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="cotsRimMoveFromGodown.aspx.cs" Inherits="TTS.cotsRimMoveFromGodown" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        table
        {
            background-color: #E4F7CF !important;
        }
        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        <asp:Label runat="server" ID="lblPageTitle" ClientIDMode="Static" Text=""></asp:Label>
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="background-color: #ffffff; width: 100%;
            border-color: White; border-collapse: separate;" id="tbDebtorsPage">
            <tr>
                <th>
                    CUSTOMER
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="cboCustomer" ClientIDMode="Static" OnSelectedIndexChanged="cboCustomer_SelectedIndexChanged"
                        AutoPostBack="true" CssClass="form-control" Width="250px">
                    </asp:DropDownList>
                </td>
                <th>
                    WORK ORDER
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="cboWorkOrder" ClientIDMode="Static" OnSelectedIndexChanged="cboWorkOrder_SelectedIndexChanged"
                        AutoPostBack="true" CssClass="form-control" Width="250px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    MOVE TO DISPATCH / ORDER QTY
                </th>
                <td style="padding-left: 10px;">
                    <asp:Label runat="server" ID="lblLeftGodownQty" ClientIDMode="Static" Text="0000"
                        Font-Bold="true" Font-Size="Larger" ForeColor="Purple"></asp:Label>
                    <asp:Label runat="server" ID="Label1" ClientIDMode="Static" Text="/" Font-Bold="true"
                        Font-Size="Larger" ForeColor="Black"></asp:Label>
                    <asp:Label runat="server" ID="lblOrderQty" ClientIDMode="Static" Text="0000" Font-Bold="true"
                        Font-Size="Larger" ForeColor="Green"></asp:Label>
                </td>
                <th>
                    BARCODE
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txt_Barcode" ClientIDMode="Static" 
                        Width="240PX" ontextchanged="txt_Barcode_TextChanged"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:GridView runat="server" ID="dg_OrderEdc" AutoGenerateColumns="false" Width="100%" CssClass="gridcss"
                        AllowPaging="true" PageSize="10" PagerStyle-Height="30px" PagerStyle-Font-Bold="true"
                        PagerStyle-Font-Size="15px" PagerStyle-HorizontalAlign="Center" PagerStyle-VerticalAlign="Middle"
                        RowStyle-Height="35px">


                    </asp:GridView>
                </td>
            </tr>
        </table>
        <div>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label runat="server" ID="lblCustcode" ClientIDMode="Static" Text="CUSTCODE"></asp:Label>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label runat="server" ID="lblDispatchID" ClientIDMode="Static" Text="DISPATCHID"></asp:Label>
        </div>
    </div>
</asp:Content>
