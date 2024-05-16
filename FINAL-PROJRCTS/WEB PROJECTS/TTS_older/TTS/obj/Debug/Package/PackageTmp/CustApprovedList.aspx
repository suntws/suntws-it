<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="CustApprovedList.aspx.cs" Inherits="TTS.CustApprovedList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div align="center" class="pageTitleHead">
        CUSTOMER APPROVED LIST</div>
    <div class="contPage">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div align="center">
                    <table cellspacing="0" rules="all" border="1" style="background-color: #d3f7d3; width: 100%;
                        border-color: #fff; border-collapse: separate; line-height: 25px;">
                        <tr>
                            <td colspan="3">
                                <asp:Label runat="server" ID="lblCustName" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                            </td>
                            <td>
                                <asp:LinkButton runat="server" ID="lnkBack" ClientIDMode="Static" Text="GO TO BACK"
                                    OnClick="lnkBack_Click"></asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                CATEGORY
                            </td>
                            <td>
                                PLATFORM
                            </td>
                            <td>
                                BRAND
                            </td>
                            <td>
                                SIDEWALL
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:RadioButtonList runat="server" ID="rbPlatformType" OnSelectedIndexChanged="rbPlatformType_IndexChanged"
                                    RepeatDirection="Horizontal" AutoPostBack="true" Width="280px" RepeatColumns="3">
                                    <asp:ListItem Value="1" Text="SOLID" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="POB"> </asp:ListItem>
                                    <asp:ListItem Value="3" Text="PNEUMATIC"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlPlatform" AutoPostBack="true" OnSelectedIndexChanged="ddlPlatform_IndexChanged"
                                    Width="180px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlBrand" AutoPostBack="true" OnSelectedIndexChanged="ddlBrand_IndexChanged"
                                    Width="180px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlSidewall" AutoPostBack="true" OnSelectedIndexChanged="ddlSidewall_IndexChanged"
                                    Width="180px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:CheckBoxList runat="server" ID="chkTypeList" ClientIDMode="Static" RepeatColumns="10"
                                    RepeatDirection="Horizontal" RepeatLayout="Table" Width="100%" CellPadding="2"
                                    CellSpacing="2" OnDataBound="chkTypeList_DataBind">
                                </asp:CheckBoxList>
                                <asp:Label runat="server" ID="lblMsg" ClientIDMode="Static" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:LinkButton runat="server" ID="lnkAddType" ClientIDMode="Static" OnClick="lnkAddType_click"
                                    Text="Add/Edit List" CssClass="btnGreen" OnClientClick="javascript:return chkCheck();"></asp:LinkButton>
                            </td>
                            <td colspan="2">
                                <div id="errMsg" style="float: right; color: #ff0000; padding-top: 8px; padding-right: 5px;
                                    font-weight: bold;">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="center">
                                <asp:GridView runat="server" ID="gv_ApprovedList" ClientIDMode="Static" AutoGenerateColumns="false"
                                    AllowPaging="true" OnPageIndexChanging="gv_PageIndex" PageSize="20" AlternatingRowStyle-BackColor="#f5f5f5"
                                    OnRowDeleting="gv_RowDeleting" HeaderStyle-Height="25px" AlternatingRowStyle-Height="22px"
                                    PagerStyle-Height="30px" PagerStyle-Font-Bold="true" PagerStyle-Font-Size="15px"
                                    PagerStyle-HorizontalAlign="Center" PagerStyle-VerticalAlign="Middle">
                                    <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" Height="25px" />
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="140px" ItemStyle-Height="25px">
                                            <HeaderTemplate>
                                                PLATFORM</HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblConfig" Text='<%#Eval("Config") %>'></asp:Label></ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="140px">
                                            <HeaderTemplate>
                                                TYPE</HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblType" Text='<%#Eval("Tyretype") %>'></asp:Label></ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="140px">
                                            <HeaderTemplate>
                                                BRAND</HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblBrand" Text='<%#Eval("brand") %>'></asp:Label></ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="140px">
                                            <HeaderTemplate>
                                                SIDEWALL</HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblSidewall" Text='<%#Eval("Sidewall") %>'></asp:Label></ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="0px">
                                            <HeaderTemplate>
                                                CATEGORY</HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblCategory" Text='<%#Eval("SizeCategory") %>'></asp:Label></ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                ACTION</HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Button runat="server" ID="btnDelete" CssClass="btnGVDelete" Text="X" CommandName="Delete" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerSettings Mode="Numeric" Position="Bottom" />
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:HiddenField runat="server" ID="hdnCustCode" ClientIDMode="Static" Value="" />
    <script type="text/javascript" language="javascript">
        function chkCheck() {
            if ($('#chkTypeList input:checked').length > 0)
                return true;
            else {
                $('#errMsg').html('Please select atleast one type');
                return false;
            }
        }
    </script>
</asp:Content>
