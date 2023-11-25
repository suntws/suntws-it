<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="cotsdomdebtorssummary.aspx.cs" Inherits="TTS.cotsdomdebtorssummary" %>
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
        <asp:Label runat="server" ID="lblPageTitle" ClientIDMode="Static" Text=" DOMESTIC DEBTORS RECEIPTS "></asp:Label>
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table style="border: 1px solid #000; width: 100%; border-collapse: collapse;" cellspacing="0"
            rules="all" border="1" class="tableCss">
            <tr>
                <td>
                    Plant
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlPlant" ClientIDMode="Static" CssClass="form-control"
                        Width="120px" AutoPostBack="true" OnSelectedIndexChanged="ddlPlant_indexchanged">
                        <asp:ListItem>--SELECT--</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button runat="server" ID="btnDownload" ClientIDMode="Static" CssClass="btn btn-success"
                        Text="DOWNLOAD" OnClick="btnDownload_Click" OnClientClick="javascript:return ctrlValidate();" />
                </td>
                <td >
                    <asp:Button runat="server" ID="btnClear" ClientIDMode="Static" CssClass="btn btn-warning"
                        Text="CLEAR" OnClick="btnClear_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:GridView runat="server" ID="gvDebtReceipts" Width="100%" CssClass="gridcss">
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        function ctrlValidate() {
            var errmsg = "";
            if ($('#ddlPlant option:selected').val() == "" || $('#ddlPlant option:selected').val() == "--SELECT--")
                errmsg += "Select Plant Name <br/>"
            if (errmsg.length > 0) {
                $('#lblErrMsgcontent').html(errmsg);
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
