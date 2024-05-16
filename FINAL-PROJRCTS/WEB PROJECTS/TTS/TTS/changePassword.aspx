<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="changePassword.aspx.cs" Inherits="TTS.changePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div align="center" class="pageTitleHead">
        Password Change
    </div>
    <div class="contPage">
        <div align="center">
            <table cellspacing="0" rules="all" border="1" style="background-color: #ffffff; width: 50%;
                border-color: #efefef; border-collapse: separate; line-height: 45px; margin: 30px;">
                <tr>
                    <td>
                        CURRENT PASSWORD
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtOldPassword" ClientIDMode="Static" Text="" CssClass="txtID"
                            TextMode="Password"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        NEW PASSWORD
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtNewPassword" ClientIDMode="Static" Text="" CssClass="txtID"
                            TextMode="Password"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        CONFIRM PASSWORD
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtConfirmPassword" ClientIDMode="Static" Text=""
                            CssClass="txtID" TextMode="Password"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center; line-height: 20px;">
                        <a href="/" class="btnclear" style="text-decoration: none;">CLEAR</a>
                    </td>
                    <td style="text-align: center;">
                        <asp:Button runat="server" ID="btnChangePass" ClientIDMode="Static" Text="CHANGE PASSWORD"
                            CssClass="btnsave" OnClientClick="javascript:return chkPassmatch();" OnClick="btnChangePass_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="line-height: 20px;">
                        <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" ForeColor="Red" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <script type="text/javascript">
        function chkPassmatch() {
            var errMsg = '';
            if ($('#txtOldPassword').val().length == 0)
                errMsg += 'Enter current password <br />';
            if ($('#txtNewPassword').val().length == 0)
                errMsg += 'Enter new password <br />';
            if ($('#txtConfirmPassword').val().length == 0)
                errMsg += 'Enter confirm password <br />';

            if ($('#txtNewPassword').val().length > 0 && $('#txtConfirmPassword').val().length > 0) {
                if ($('#txtNewPassword').val().toString() != $('#txtConfirmPassword').val().toString())
                    errMsg += 'New and confirm password mismatch <br />';
            }

            if (errMsg.length > 0) {
                $('#lblErrMsg').html(errMsg);
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
