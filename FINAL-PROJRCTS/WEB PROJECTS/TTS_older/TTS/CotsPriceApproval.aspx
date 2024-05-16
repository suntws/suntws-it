<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CotsPriceApproval.aspx.cs"
    MasterPageFile="~/master.Master" Inherits="TTS.CotsPriceApproval" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        th
        {
            line-height: 15px;
            text-align: left;
            font-weight: normal;
        }
        td
        {
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div align="center" class="pageTitleHead">
        LIST PRICE APPROVAL FOR SCOTS ORDER ENTRY
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Size="16px"
            ForeColor="Red"></asp:Label>
    </div>
    <div class="contPage">
        <table cellspacing="0" rules="all" border="1" style="background-color: #f5ffff; width: 100%;
            border-color: #d6d6d6; border-collapse: separate;">
            <tr>
                <th>
                    CUSTOMER
                </th>
                <td colspan="5">
                    <asp:DropDownList ID="ddl_Customer" ClientIDMode="Static" CssClass="form-control"
                        AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddl_Customer_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    PRICE SHEET REF NO
                </th>
                <td>
                    <asp:DropDownList ID="ddl_PriceSheetRefNo" ClientIDMode="Static" CssClass="form-control"
                        AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddl_PriceSheetRefNo_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <th>
                    RATES ID
                </th>
                <td>
                    <asp:TextBox ID="txtRatesId" runat="server" Text="" Enabled="false" CssClass="form-control"></asp:TextBox>
                </td>
                <th>
                    END DATE
                </th>
                <td>
                    <asp:TextBox ID="txtEndDate" runat="server" Text="" Enabled="false" CssClass="form-control"
                        Width="110px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="6">
                    <asp:GridView ID="gv_PriceDetails" Width="100%" runat="server" AutoGenerateColumns="false"
                        HeaderStyle-BackColor="#79bbff">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox runat="server" ID="checkAllChk" ClientIDMode="Static" />
                                    ALL
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CATEGORY">
                                <ItemTemplate>
                                    <asp:Label ID="lblCategory" runat="server" Text='<%# Eval("Category") %>'></asp:Label>
                                    <asp:HiddenField ID="hdnProcessId" runat="server" Value='<%# Eval("Process_ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PLATFORM">
                                <ItemTemplate>
                                    <asp:Label ID="lblPlatform" runat="server" Text='<%# Eval("Config") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TYRE SIZE">
                                <ItemTemplate>
                                    <asp:Label ID="lblTyreSize" runat="server" Text='<%# Eval("TyreSize") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="RIM">
                                <ItemTemplate>
                                    <asp:Label ID="lblRimSize" runat="server" Text='<%# Eval("TyreRim") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TYPE">
                                <ItemTemplate>
                                    <asp:Label ID="lblTyretype" runat="server" Text='<%# Eval("TyreType") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="BRAND">
                                <ItemTemplate>
                                    <asp:Label ID="lblBrand" runat="server" Text='<%# Eval("Brand") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SIDEWALL">
                                <ItemTemplate>
                                    <asp:Label ID="lblSidewall" runat="server" Text='<%# Eval("Sidewall") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="FWT">
                                <ItemTemplate>
                                    <asp:Label ID="FinishedWt" runat="server" Text='<%# Eval("Finished_Wt") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="LIST PRICE" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:Label ID="UnitPrice" runat="server" Text='<%# Eval("Unit_Price") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr style="text-align: center;">
                <td colspan="4">
                    <asp:Label ID="lblErrMsg" ClientIDMode="Static" Text="" runat="server" ForeColor="Red"></asp:Label>
                </td>
                <td>
                    <asp:Button ID="btnSavePriceSheet" ClientIDMode="Static" runat="server" Text="APPROVED TO SCOTS"
                        CssClass="btn btn-success" OnClientClick="javascript:return CtrlEntryValidate();"
                        OnClick="btnSavePriceSheet_Click" />
                </td>
                <td>
                    <span id="btnClearPriceSheet" class="btn btn-info" style="background-color: #f14138;">
                        CLEAR</span>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        $(function () {
            $('#btnClearPriceSheet').click(function () { window.location.href = window.location.href; });
            $('#checkAllChk').click(function () {
                if ($("[id*=MainContent_gv_PriceDetails_chkSelect_]").length > 0) {
                    if ($(this).attr('checked') == "checked")
                        $("[id*=MainContent_gv_PriceDetails_chkSelect_]").attr('checked', true)
                    else
                        $("[id*=MainContent_gv_PriceDetails_chkSelect_]").attr('checked', false)
                }
                else {
                    alert('No records');
                    $(this).attr('checked', false);
                }
            });
            $("[id*=MainContent_gv_PriceDetails_chkSelect_]").click(function () { $('#checkAllChk').attr('checked', false); });
        });

        function CtrlEntryValidate() {
            var ErrMsg = ''; $('#lblErrMsg').html('');
            if ($('#ddl_Customer option:selected').val() == "CHOOSE")
                ErrMsg += "Choose customer<br/>";
            if ($('#ddl_PriceSheetRefNo option:selected').val() == "CHOOSE")
                ErrMsg += "Choose price sheet ref no.<br/>";
            if ($("input:checkbox[id*=MainContent_gv_PriceDetails_chkSelect_]:checked").length == 0)
                ErrMsg += "Choose approval list price<br/>";
            if (ErrMsg.length > 0) {
                $('#lblErrMsg').html(ErrMsg);
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
