<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="stockdump.aspx.cs" Inherits="TTS.stockdump" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: center;" class="pageTitleHead">
        GLOBAL STOCK DUMP
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <asp:ScriptManager runat="server" ID="ScriptManager1">
    </asp:ScriptManager>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="width: 100%; border-color: White;
            border-collapse: separate;">
            <tr>
                <td>
                    <asp:GridView runat="server" ID="gvStockUpdateDate" AutoGenerateColumns="false" Width="300px"
                        CssClass="gridcss">
                        <Columns>
                            <asp:BoundField DataField="Plant" HeaderText="PLANT" />
                            <asp:BoundField DataField="LastUpdate" HeaderText="LAST UPDATE ON" />
                        </Columns>
                    </asp:GridView>
                </td>
                <td>
                    PLANT
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlStockPlant" ClientIDMode="Static" AutoPostBack="false"
                        OnSelectedIndexChanged="ddlStockPlant_IndexChanged" CssClass="form-control" Width="80px">
                    </asp:DropDownList>
                </td>
                <td>
                    <%--<asp:Button runat="server" ID="btnDownload" ClientIDMode="Static" Text="DOWNLOAD GSD"
                        CssClass="btn btn-success" OnClick="btnDownload_Click" />--%>
                    <asp:Button runat="server" ID="btnGsdFileMail" ClientIDMode="Static" Text="DOWNLOAD GSD"
                        CssClass="btn btn-success" OnClick="btnGsdFileMail_Click" />
                </td>
            </tr>
            <%--<tr>
                <td colspan="3">
                    <asp:GridView runat="server" ID="gvGSD" AutoGenerateColumns="true" Width="100%" CssClass="gridcss">
                    </asp:GridView>
                </td>
            </tr>--%>
        </table>
    </div>
    <script type="text/javascript">
        $(document).ready(function () { });
    </script>
</asp:Content>
