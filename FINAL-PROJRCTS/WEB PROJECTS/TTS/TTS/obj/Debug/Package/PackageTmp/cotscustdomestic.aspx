<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="cotscustdomestic.aspx.cs" Inherits="TTS.cotscustdomestic" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        <asp:Label runat="server" ID="lblPageHead" ClientIDMode="Static" Text=""></asp:Label>
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <div>
            <table cellspacing="0" rules="all" border="1" style="background-color: #ecf6ff; border-color: White;
                border-collapse: separate; width: 100%;">
                <tr>
                    <th>
                        Customer Name:
                    </th>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlCotsCustName" ClientIDMode="Static" Width="300px"
                            OnSelectedIndexChanged="ddlCotsCustName_IndexChange" AutoPostBack="true" Font-Bold="true"
                            CssClass="form-control">
                        </asp:DropDownList>
                    </td>
                    <th>
                        User ID:
                    </th>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlLoginUserName" ClientIDMode="Static" Width="275px"
                            OnSelectedIndexChanged="ddlLoginUserName_IndexChange" AutoPostBack="true" CssClass="form-control">
                        </asp:DropDownList>
                    </td>
                    <th>
                        Grade:
                    </th>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlCotsGrade" ClientIDMode="Static" Width="100px"
                            OnSelectedIndexChanged="ddlCotsGrade_IndexChange" AutoPostBack="true" Font-Bold="true"
                            CssClass="form-control">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="vertical-align: top;">
                        <asp:GridView runat="server" ID="gv_TTSApprovedList" AutoGenerateColumns="false"
                            Width="420px">
                            <HeaderStyle BackColor="#E0E0BD" Font-Bold="true" Height="22px" HorizontalAlign="Center" />
                            <Columns>
                                <asp:BoundField HeaderText="PLATFORM" DataField="Config" />
                                <asp:BoundField HeaderText="TYPE" DataField="tyretype" />
                                <asp:BoundField HeaderText="BRAND" DataField="brand" />
                                <asp:BoundField HeaderText="SIDEWALL" DataField="Sidewall" />
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:CheckBox runat="server" ID="checkAllChk" ClientIDMode="Static" />
                                        ALL
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="listCheckBox" runat="server" Checked="false" /></ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <div style="float: left; display: none; text-align: center;" id="divGradeEntireList">
                            <asp:Button runat="server" ID="btnApprovedUserTypes" ClientIDMode="Static" Text="Selected Grade Assign To Customer"
                                Width="260px" CssClass="btnBread" OnClientClick="javascript:return chkApprovedCheck();"
                                OnClick="btnApprovedUserTypes_Click" />
                            <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text=""></asp:Label>
                        </div>
                    </td>
                    <td colspan="3">
                        <asp:GridView runat="server" ID="gv_TypeWiseDiscount" AutoGenerateColumns="false"
                            Width="650px" OnRowEditing="gv_TypeWiseDiscount_RowEditing" OnRowUpdating="gv_TypeWiseDiscount_RowUpdating"
                            OnRowCancelingEdit="gv_TypeWiseDiscount_RowCanceling" OnRowDeleting="gv_TypeWiseDiscount_RowDeleting">
                            <HeaderStyle CssClass="discHeader" />
                            <RowStyle HorizontalAlign="Left" />
                            <Columns>
                                <asp:TemplateField HeaderText="CONFIG" ItemStyle-Width="90px">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblConfigDiscount" Text='<%# Eval("Config") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="TYPE" ItemStyle-Width="60px">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblTypeDiscount" Text='<%# Eval("tyretype") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="BRAND" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblBrandDiscount" Text='<%# Eval("brand") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="SIDEWALL" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblSidewallDiscount" Text='<%# Eval("Sidewall") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="DISC %" ItemStyle-Width="50px">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblCotsDiscount" Text='<%# Eval("Discount") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox runat="server" ID="txtTypeBrandDiscount" onkeypress="return isNumberAndMinusKey(event)"
                                            Width="50px" MaxLength="6" BackColor="#f9c232" Text='<%# Eval("Discount") %>'
                                            CssClass="form-control"></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="CUSTOMER VIEW" ItemStyle-Width="50px">
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chkGradeView" Enabled="false" Checked='<%# DataBinder.Eval(Container.DataItem,"GradeView").ToString()=="1"?true:false %>' /></ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:CheckBox runat="server" ID="chkGradeView" Enabled="true" Checked='<%# DataBinder.Eval(Container.DataItem,"GradeView").ToString()=="1"?true:false %>' /></EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ACTION" ItemStyle-Width="180px">
                                    <ItemTemplate>
                                        <asp:Button runat="server" ID="btnEdit" Text="Edit" CommandName="Edit" />
                                        <asp:Button runat="server" ID="btnDelete" Text="Delete" CommandName="Delete" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Button runat="server" ID="btnUpdate" Text="Update" CommandName="Update" />
                                        <asp:Button runat="server" ID="btnCancel" Text="Cancel" CommandName="Cancel" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdnCotsCustID" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnLoginName" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnFullName" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnGrade" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        $(document).ready(function () {
            $("#ddlCotsCustName").change(function () {
                $('#hdnFullName').val($('#ddlCotsCustName option:selected').text());
            });
            $('#ddlLoginUserName').change(function () {
                $('#hdnLoginName').val($('#ddlLoginUserName option:selected').text());
                $('#hdnCotsCustID').val($('#ddlLoginUserName option:selected').val());
            });
            $('#ddlCotsGrade').change(function () {
                $('#hdnGrade').val($('#ddlCotsGrade option:selected').text());
            });

            $(':text').bind('keydown', function (e) {
                //on keydown for all textboxes
                if (e.target.className == "txtID") {
                    if (e.keyCode == 13) { //if this is enter key
                        e.preventDefault();
                        return false;
                    }
                }
            });

            $('#checkAllChk').click(function () {
                var chkLength = $("[id*=MainContent_gv_TTSApprovedList_listCheckBox_]").length;
                var j = 0;
                if (chkLength > 0) {
                    if ($(this).attr('checked') == "checked")
                        $("[id*=MainContent_gv_TTSApprovedList_listCheckBox_]").attr('checked', true)
                    else {
                        $("[id*=MainContent_gv_TTSApprovedList_listCheckBox_]").attr('checked', false)
                        $("[id*=MainContent_gv_TTSApprovedList_listCheckBox_]").each(function () {
                            if ($(this).attr('disabled') == 'disabled')
                                $(this).attr('checked', true);
                        });
                    }
                }
                else {
                    alert('No records');
                    $(this).attr('checked', false);
                }
            });
            $("[id*=MainContent_gv_TTSApprovedList_listCheckBox_]").click(function () {
                $('#checkAllChk').attr('checked', false);
            });
        });

        function chkApprovedCheck() {
            var errmsg = '';
            if ($("#ddlCotsCustName option:selected").text() == "Choose")
                errmsg += 'Choose customer name<br/>';
            if ($("#ddlCotsGrade option:selected").text() == "Choose")
                errmsg += 'Choose grade<br/>';
            if ($('#MainContent_gv_TTSApprovedList input:checkbox:checked').length == 0)
                errmsg += 'Choose any one approved type list';
            if (errmsg.length > 0) {
                $('#lblErrMsg').html(errmsg).css({ 'color': '#f00' });
                return false;
            }
            else {
                if ($("input:checkbox[id*=MainContent_gv_TTSApprovedList_listCheckBox_]").length == $("input:checkbox[id*=MainContent_gv_TTSApprovedList_listCheckBox_]:checked").length)
                    $('#checkAllChk').attr('checked', true);
                return true;
            }
        }

        function showgradelist() {
            $('#divGradeEntireList').css({ 'display': 'block' });
        }
    </script>
</asp:Content>
