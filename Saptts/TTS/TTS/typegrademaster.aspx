<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="typegrademaster.aspx.cs" Inherits="TTS.typegrademaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div align="center" class="pageTitleHead">
        Grade Selection Data Entry
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table>
            <tr>
                <td>
                    <div style="width: 1063px; float: left; border-bottom: 1px solid #000;">
                        <asp:GridView runat="server" ID="gvAllCategory" AutoGenerateColumns="true" Width="1063px"
                            HeaderStyle-CssClass="gvHeadCss" RowStyle-CssClass="bottomborder">
                        </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="width: 1060px; float: left; border: 1px solid #000; padding-bottom: 5px;
                        padding-top: 10px; padding-left: 5px;">
                        <div style="width: 210px; float: left; margin: 5px;">
                            <div style="border: 1px solid #000; background-color: #6E0672; color: #fff; font-weight: bold;
                                margin-bottom: 10px;">
                                <asp:RadioButtonList runat="server" ID="rdbMasterStyle" OnSelectedIndexChanged="rdbMasterStyle_IndexChange"
                                    RepeatColumns="1" Width="210px" AutoPostBack="true">
                                    <asp:ListItem Text="Create Category / Sub Category" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Add/Edit Type Details" Value="2"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" Font-Bold="true"
                                ForeColor="Red"></asp:Label>
                        </div>
                        <div style="width: 820px; float: left; line-height: 25px; margin: 3px; padding: 3px;"
                            id="div1" runat="server" clientidmode="Static">
                            <div style="width: 460px; float: left;">
                                <div style="width: 450px; float: left;">
                                    <span class="headCss" style="width: 142px; float: left;">Category :</span>
                                    <asp:DropDownList runat="server" ID="ddlCategory" ClientIDMode="Static" Width="300px"
                                        OnSelectedIndexChanged="ddlCategory_IndexChange" AutoPostBack="true">
                                    </asp:DropDownList>
                                </div>
                                <div style="width: 450px; float: left; display: none;" id="categoryDiv">
                                    <span style="color: #f00; width: 142px; float: left;">Enter Caterogy :</span>
                                    <asp:TextBox runat="server" ID="txtCategory" ClientIDMode="Static" Width="295px"></asp:TextBox>
                                </div>
                                <div style="width: 450px; float: left;">
                                    <span class="headCss" style="width: 142px; float: left;">Enter Sub Caterogy :</span>
                                    <asp:TextBox runat="server" ID="txtSubCategory" ClientIDMode="Static" Width="295px"></asp:TextBox>
                                </div>
                                <div style="width: 450px; float: left; text-align: center;">
                                    <asp:Button runat="server" ID="btnCategorySave" ClientIDMode="Static" Text="SAVE"
                                        CssClass="btnsave" OnClientClick="javascript:return ctrlCategoryChk()" OnClick="btnCategorySave_Click" />
                                </div>
                            </div>
                            <div style="width: 350px; float: left; line-height: 15px;">
                                <asp:DataList runat="server" ID="dlSubCategory" RepeatColumns="1" RepeatDirection="Horizontal"
                                    RepeatLayout="Table" Width="340px">
                                    <ItemTemplate>
                                        <%# Eval("Position")%>
                                        .
                                        <%# Eval("SubCategory")%>
                                    </ItemTemplate>
                                </asp:DataList>
                            </div>
                        </div>
                        <div style="width: 820px; float: left; line-height: 25px; margin: 3px; padding: 3px;"
                            id="div2" runat="server" clientidmode="Static">
                            <div style="width: 220px; float: left;">
                                <div style="width: 200px; float: left;">
                                    <span class="headCss" style="width: 75px; float: left;">Type :</span>
                                    <asp:DropDownList runat="server" ID="ddlTypeList" ClientIDMode="Static" Width="115px"
                                        OnSelectedIndexChanged="ddlTypeList_IndexChange" AutoPostBack="true">
                                    </asp:DropDownList>
                                </div>
                                <div style="width: 200px; float: left; display: none;" id="typeDiv">
                                    <span style="color: #f00; width: 75px; float: left;">Enter Type :</span>
                                    <asp:TextBox runat="server" ID="txtCreateType" ClientIDMode="Static" Text="" Width="100px"
                                        MaxLength="10"></asp:TextBox>
                                </div>
                                <div style="width: 200px; float: left; text-align: center;">
                                    <asp:Button runat="server" ID="btnTypeCreate" ClientIDMode="Static" Text="SAVE" CssClass="btnsave"
                                        OnClientClick="javascript:return ctrlTypeChk()" OnClick="btnTypeCreate_Click" />
                                </div>
                            </div>
                            <div style="width: 600px; float: left;">
                                <asp:GridView runat="server" ID="gvTypeWiseDetails" AutoGenerateColumns="false" HeaderStyle-CssClass="headerNone"
                                    Width="580px">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="200px" ItemStyle-BackColor="#C6AECC" ItemStyle-Font-Bold="true">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblCateogyName" Text='<%# Eval("CategoryList") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="380px">
                                            <ItemTemplate>
                                                <asp:TextBox runat="server" ID="txtSubList" Text='<%# Eval("SubCategoryList") %>'
                                                    Width="360px" onkeypress="return isGrdaeDetailsKey(event)" CssClass="txtGradeList"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="display: none;">
                    <asp:GridView runat="server" ID="gvApplicationDetails" AutoGenerateColumns="true"
                        Width="1063px" HeaderStyle-CssClass="gvHeadCss">
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView runat="server" ID="gvTypeDetails" AutoGenerateColumns="true" Width="1063px"
                        HeaderStyle-CssClass="gvHeadCss">
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#MainContent_gvAllCategory th').css({ 'width': '100px' });
            $('#MainContent_gvTypeDetails th').css({ 'width': '100px' });
            $('#MainContent_gvApplicationDetails th').css({ 'width': '100px' });
            $('#ddlCategory').change(function () {
                $('#categoryDiv').css({ 'display': 'none' });
                if ($('#ddlCategory option:selected').text() != 'Choose') {
                    if ($('#ddlCategory option:selected').text() == 'Add New Category')
                        $('#categoryDiv').css({ 'display': 'block' });
                }
            });

            $('#ddlTypeList').change(function () {
                $('#typeDiv').css({ 'display': 'none' });
                if ($('#ddlTypeList option:selected').text() != 'Choose') {
                    if ($('#ddlTypeList option:selected').text() == 'Add New Type')
                        $('#typeDiv').css({ 'display': 'block' });
                }
            });
        });

        function isGrdaeDetailsKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode != 44 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

        function categoryShow() {
            $('#categoryDiv').css({ 'display': 'block' });
        }

        function typeShow() {
            $('#typeDiv').css({ 'display': 'block' });
        }

        function ctrlCategoryChk() {
            $('#lblErrMsg').html('');
            var errmsg = '';
            if ($('#ddlCategory option:selected').text() == 'Choose')
                errmsg += 'Choose any one category<br/>';
            if ($('#ddlCategory option:selected').text() == 'Add New Category') {
                if ($('#txtCategory').val().length == 0)
                    errmsg += 'Enter category<br/>';
            }
            if ($('#txtSubCategory').val().length == 0)
                errmsg += 'Enter sub category<br/>';

            if (errmsg.length > 0) {
                $('#lblErrMsg').html(errmsg);
                return false;
            }
            else
                return true;
        }

        function ctrlTypeChk() {
            $('#lblErrMsg').html('');
            var errmsg = '';
            if ($('#ddlTypeList option:selected').text() == 'Choose')
                errmsg += 'Choose any one type<br/>';
            if ($('#ddlTypeList option:selected').text() == 'Add New Type') {
                if ($('#txtCreateType').val().length == 0)
                    errmsg += 'Enter type<br/>';
            }

            if (errmsg.length > 0) {
                $('#lblErrMsg').html(errmsg);
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
