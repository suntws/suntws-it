<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="cotsDomDebtorsReceipts.aspx.cs" Inherits="TTS.cotsDomDebtorsReceipts" %>

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
                    Year
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlYear" ClientIDMode="Static" CssClass="form-control"
                        Width="120px" AutoPostBack="true" OnSelectedIndexChanged="ddlYear_indexchanged">
                    </asp:DropDownList>
                </td>
                <td>
                    Month
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlMonth" ClientIDMode="Static" CssClass="form-control"
                        Width="120px" AutoPostBack="true" OnSelectedIndexChanged="ddlMonth_indexchanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Day
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlDay" ClientIDMode="Static" CssClass="form-control"
                        Width="120px" AutoPostBack="true" OnSelectedIndexChanged="ddlDay_indexchanged">
                    </asp:DropDownList>
                </td>
                <td colspan="2">
                    <asp:Button runat="server" ID="btnDownload" ClientIDMode="Static" CssClass="btn btn-success"
                        Text="DOWNLOAD" OnClick="btnDownload_Click" OnClientClick="javascript:return ctrlValidate();" />
                </td>
                <td colspan="2">
                    <asp:Button runat="server" ID="btnClear" ClientIDMode="Static" CssClass="btn btn-warning"
                        Text="CLEAR" OnClick="btnClear_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="6">
                    <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"
                        Font-Size="15px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="6">
                    <asp:GridView runat="server" ID="gvDebtReceipts" AutoGenerateColumns="false" Width="100%"
                        CssClass="gridcss">
                        <Columns>
                            <asp:BoundField HeaderText="DATE" DataField="CUSTOMER" />
                            <asp:BoundField HeaderText="NAME OF PARTY" DataField="PLANT" />
                            <asp:BoundField HeaderText="AMOUNT" DataField="FROM" />
                            <asp:BoundField HeaderText="TOTAL AMOUNT" DataField="TO" />
                        </Columns>
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
            if ($('#ddlYear option:selected').val() == "" || $('#ddlYear option:selected').val() == "--SELECT--")
                errmsg += "Select Year <br/>"
            if ($('#ddlMonth option:selected').val() == "" || $('#ddlMonth option:selected').val() == "--SELECT--")
                errmsg += "Select Month <br/>"
            if ($('#ddlDay option:selected').val() == "" || $('#ddlDay option:selected').val() == "--SELECT--")
                errmsg += "Select Day <br/>"
            if (errmsg.length > 0) {
                $('#lblErrMsg').html(errmsg);
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
