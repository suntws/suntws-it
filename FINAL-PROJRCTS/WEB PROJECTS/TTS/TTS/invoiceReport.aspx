<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="invoiceReport.aspx.cs" Inherits="TTS.invoiceReport" %>

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
        <asp:Label runat="server" ID="lblPageTitle" ClientIDMode="Static"></asp:Label>
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
                    From Year
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlFromYear" ClientIDMode="Static" CssClass="form-control"
                        Width="120px" AutoPostBack="true" OnSelectedIndexChanged="ddlFromYear_indexchanged">
                    </asp:DropDownList>
                </td>
                <td>
                    From Month
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlFromMonth" ClientIDMode="Static" CssClass="form-control"
                        Width="120px" AutoPostBack="true" OnSelectedIndexChanged="ddlFromMonth_indexchanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    To Year
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlToYear" ClientIDMode="Static" CssClass="form-control"
                        Width="120px" AutoPostBack="true" OnSelectedIndexChanged="ddlToYear_indexchanged">
                    </asp:DropDownList>
                </td>
                <td>
                    To Month
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlToMonth" ClientIDMode="Static" CssClass="form-control"
                        Width="120px" AutoPostBack="true" OnSelectedIndexChanged="ddlToMonth_indexchanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button runat="server" ID="btnSave" ClientIDMode="Static" CssClass="btn btn-success"
                        Text="SAVE/VIEW" OnClick="btnSave_Click" OnClientClick="javascript:return ctrlValidate();" />
                </td>
                <td>
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
                    <asp:GridView runat="server" ID="gvInvoiceNoList" AutoGenerateColumns="false" Width="100%"
                        CssClass="gridcss">
                        <Columns>
                            <asp:BoundField HeaderText="CUSTOMER" DataField="CUSTOMER" />
                            <asp:BoundField HeaderText="PLANT" DataField="PLANT" />
                            <asp:BoundField HeaderText="FROM" DataField="FROM" />
                            <asp:BoundField HeaderText="TO" DataField="TO" />
                            <asp:BoundField HeaderText="REQUESTED BY" DataField="ReqUser" />
                            <asp:BoundField HeaderText="REQUESTED ON" DataField="ReqDate" />
                            <asp:BoundField HeaderText="FILE GENERATED ON" DataField="FileCreateDate" />
                            <asp:TemplateField HeaderText="DOWNLOAD" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="lnkFileName" OnClick="lnkFileName_Click" Text='<%# Eval("FileName") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
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
            if ($('#ddlFromYear option:selected').val() == "" || $('#ddlFromYear option:selected').val() == "--SELECT--")
                errmsg += "Select From Year <br/>"
            if ($('#ddlFromMonth option:selected').val() == "" || $('#ddlFromMonth option:selected').val() == "--SELECT--")
                errmsg += "Select From Month <br/>"
            if ($('#ddlToYear option:selected').val() == "" || $('#ddlToYear option:selected').val() == "--SELECT--")
                errmsg += "Select To Year <br/>"
            if ($('#ddlToMonth option:selected').val() == "" || $('#ddlToMonth option:selected').val() == "--SELECT--")
                errmsg += "Select To Month <br/>"
            if (errmsg.length > 0) {
                $('#lblErrMsg').html(errmsg);
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
