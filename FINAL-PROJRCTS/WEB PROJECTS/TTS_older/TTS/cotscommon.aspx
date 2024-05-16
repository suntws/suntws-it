<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="cotscommon.aspx.cs" Inherits="TTS.cotscommon" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        <asp:Label runat="server" ID="lblScotsType" ClientIDMode="Static" Text=""></asp:Label>
        ORDER ENTRY GENERAL ANNOUNCEMENT</div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <div style="padding-left: 10px; padding-top: 20px; padding-right: 10px;">
            <table style="border: 1px solid #000;">
                <tr>
                    <td>
                        <span class="headCss">CUSTOMER CATEGORY: </span>
                        <asp:DropDownList runat="server" ID="ddlCustCategory" ClientIDMode="Static" Width="100px"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlCustCategory_IndexChange">
                            <asp:ListItem Text="Choose" Value="Choose"></asp:ListItem>
                            <asp:ListItem Text="Dealer" Value="Dealer"></asp:ListItem>
                            <asp:ListItem Text="End user" Value="End user"></asp:ListItem>
                            <asp:ListItem Text="OEM" Value="OEM"></asp:ListItem>
                            <asp:ListItem Text="ME" Value="ME"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox runat="server" ID="txtCommonText" ClientIDMode="Static" Text="" TextMode="MultiLine"
                            Width="1050px" Height="400px" onKeyUp="javascript:CheckMaxLength(this, 3999);"
                            onChange="javascript:CheckMaxLength(this, 3999);"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" Font-Bold="true"
                            ForeColor="Red"></asp:Label>
                        <asp:Label runat="server" ID="lblSuccessMsg" ClientIDMode="Static" Text="" Font-Bold="true"
                            ForeColor="Green"></asp:Label>
                        <asp:Button runat="server" ID="btnCommonSave" Text="SAVE" CssClass="btnsave" OnClick="btnCommonSave_Click"
                            OnClientClick="javascript:return CtrlCommonBox();" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <script type="text/javascript">
        function CtrlCommonBox() {
            $('#lblErrMsg').html(''); var errmsg = '';
            if ($("#ddlCustCategory option:selected").text() == 'Choose')
                errmsg += 'Choose customer category <br/>';
            if ($("#txtCommonText").val().length == 0)
                errmsg += 'Enter General announcement<br/>';
            if (errmsg.length > 0) {
                $('#lblErrMsg').html(errmsg);
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
