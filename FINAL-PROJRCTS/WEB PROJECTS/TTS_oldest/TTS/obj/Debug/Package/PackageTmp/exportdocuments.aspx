<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="exportdocuments.aspx.cs" Inherits="TTS.exportdocuments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        EXPORT CUSTOMER CARRY DOCUMENTS
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="background-color: #ecf6ff; border-color: White;
            border-collapse: separate; width: 100%;">
            <tr>
                <th class="spanCss">
                    Customer Name
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="ddlExpCustName" ClientIDMode="Static" Width="450px"
                        OnSelectedIndexChanged="ddlExpCustName_IndexChange" CssClass="form-control" AutoPostBack="true">
                    </asp:DropDownList>
                </td>
                <th class="spanCss">
                    User Name
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="ddlExpLoginUserName" ClientIDMode="Static" OnSelectedIndexChanged="ddlExpLoginUserName_IndexChange"
                        CssClass="form-control" AutoPostBack="true">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th class="spanCss">
                    Type of Export Documents
                </th>
                <td>
                    <asp:CheckBoxList runat="server" ID="chkExpDocuments" RepeatColumns="1">
                    </asp:CheckBoxList>
                </td>
                <th class="spanCss">
                    Required Documents For Customer
                </th>
                <td style="vertical-align: top;">
                    <asp:GridView runat="server" ID="gv_ExportDocList" AutoGenerateColumns="false" AlternatingRowStyle-BackColor="#f5f5f5"
                        OnRowDeleting="gv_ExportDocList_RowDeleting" RowStyle-Height="22px">
                        <HeaderStyle BackColor="#118e38" ForeColor="White" Font-Bold="true" Height="25px"
                            HorizontalAlign="Center" />
                        <Columns>
                            <asp:BoundField DataField="DocName" HeaderText="DOCUMENT" ItemStyle-Width="450px" />
                            <asp:TemplateField ItemStyle-Width="100px" HeaderText="ACTION">
                                <ItemTemplate>
                                    <asp:Button runat="server" ID="btnDocDelete" Text="Delete" CommandName="Delete" CssClass="btncustdel" /></ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btn_ClearRecords" runat="server" Text="Clear Selection" CssClass="btn btn-info"
                        OnClientClick="javascript:return ctrlClear();" />
                </td>
                <td style="text-align: center;">
                    <asp:Button ID="btnSaveDocuments" ClientIDMode="Static" runat="server" Text="Save Required Documents"
                        CssClass="btn btn-success" OnClientClick="javascript:return CtrlExportDoc();"
                        OnClick="btnSaveDocuments_Click" />
                </td>
                <td colspan="2">
                    <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnCotsCustID" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnLoginName" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnFullName" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        $(document).ready(function () {
            $("#ddlExpCustName").change(function () {
                $('#hdnFullName').val($('#ddlExpCustName option:selected').text());
            });
            $('#ddlExpLoginUserName').change(function () {
                $('#hdnLoginName').val($('#ddlExpLoginUserName option:selected').text());
                $('#hdnCotsCustID').val($('#ddlExpLoginUserName option:selected').val());
            });
        });

        function CtrlExportDoc() {
            $('#lblErrMsg').html(''); var ErrMsg = '';
            if ($('#ddlExpCustName option:selected').text() == 'Choose')
                ErrMsg += 'Choose customer full name<br />';
            if ($('#ddlExpLoginUserName option:selected').text() == 'Choose')
                ErrMsg += 'Choose user name <br />';
            if ($("input:checkbox[id*=MainContent_chkExpDocuments_]:enabled:checked").length == 0)
                ErrMsg += 'Choose any one documents';
            if (ErrMsg.length > 0) {
                $('#lblErrMsg').html(ErrMsg);
                return false;
            }
            else
                return true;
        }
        function ctrlClear() { window.location.href = window.location.href; return false; }
    </script>
</asp:Content>
