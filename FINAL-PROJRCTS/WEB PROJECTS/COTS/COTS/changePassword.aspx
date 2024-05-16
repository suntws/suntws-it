<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="changePassword.aspx.cs" Inherits="COTS.changePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #pswd_info
        {
            -webkit-border-radius: 10px;
            background: #fefefe;
            font-size: 11px;
            border: 1px solid #212dbb;
            display: none;
        }
        .invalid
        {
            line-height: 15px;
            color: #ec3f41;
        }
        .valid
        {
            line-height: 18px;
            color: #3a7d34;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div align="center" class="pageTitleHead">
        Password Change</div>
    <div class="contPage">
        <div align="center">
            <table cellspacing="0" rules="all" border="1" style="background-color: #ffffff; width: 70%;
                border-color: #efefef; border-collapse: separate; line-height: 45px; margin: 30px;">
                <tr>
                    <td colspan="3" class="pageTitleHead">
                        <asp:Label runat="server" ID="lblPassChangedOn" ClientIDMode="Static" Text="" ForeColor="#09987e"
                            BackColor="#ffffff"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        CURRENT PASSWORD
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtOldPassword" ClientIDMode="Static" Text="" CssClass="form-control"
                            TextMode="Password"></asp:TextBox>
                    </td>
                    <td rowspan="3" style="width: 320px;">
                        <div id="pswd_info">
                            <h4 style="padding-left: 10px; line-height: 20px;">
                                Password must meet the following requirements:
                            </h4>
                            <ul>
                                <li id="length" class="invalid">The Password Should be at least 8 characters long</li>
                                <li id="capital" class="invalid">Include at least 1 upper case character</li>
                                <li id="letter" class="invalid">Include at least 1 lower case character</li>
                                <li id="number" class="invalid">Include at least 1 number</li>
                                <li id="Li1" class="invalid">Inlcude at least 1 Special Character</li>
                            </ul>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        NEW PASSWORD
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtNewPassword" ClientIDMode="Static" Text="" CssClass="form-control"
                            TextMode="Password" MaxLength="16"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        CONFIRM PASSWORD
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtConfirmPassword" ClientIDMode="Static" Text=""
                            CssClass="form-control" TextMode="Password"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        <a href="changePassword.aspx" style="text-decoration: none;"><span class="btn btn-warning">
                            CLEAR</span></a>
                    </td>
                    <td style="text-align: center;">
                        <asp:Button runat="server" ID="btnChangePass" ClientIDMode="Static" Text="CHANGE PASSWORD"
                            CssClass="btn btn-success" OnClientClick="javascript:return chkPassmatch();"
                            OnClick="btnChangePass_Click" />
                    </td>
                    <td style="line-height: 20px;">
                        <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" ForeColor="Red" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#txtNewPassword').live('keyup', cntrlSave);
            $("#txtNewPassword").focus(function () {
                $('#pswd_info').show();
            });
            $("#txtNewPassword").blur(function () {
                $('#pswd_info').hide();
            });
        });
        function cntrlSave() {
            $('#pswd_info').show();
            var pswd = $('#txtNewPassword').val();
            if (pswd.length < 8)
                $('#length').removeClass('valid').addClass('invalid');
            else
                $('#length').removeClass('invalid').addClass('valid');

            if (pswd.match(/[a-z]/))
                $('#letter').removeClass('invalid').addClass('valid');
            else
                $('#letter').removeClass('valid').addClass('invalid');

            if (pswd.match(/[A-Z]/))
                $('#capital').removeClass('invalid').addClass('valid');
            else
                $('#capital').removeClass('valid').addClass('invalid');

            if (pswd.match(/\d/))
                $('#number').removeClass('invalid').addClass('valid');
            else
                $('#number').removeClass('valid').addClass('invalid');

            if (pswd.match(/[*!@#$%^&()~{}]+/))
                $('#Li1').removeClass('invalid').addClass('valid');
            else
                $('#Li1').removeClass('valid').addClass('invalid');
        }

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
                    errMsg += 'New and confirm password not matched <br />';
            }
            else if ($('#txtOldPassword').val() == $('#txtNewPassword').val())
                errMsg += 'Current and new password are same<br />';
            if (!($('#txtNewPassword').val()).match(/^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$/)) {
                $('#pswd_info').show();
                return false;
            }
            if (errMsg.length > 0) {
                $('#lblErrMsg').html(errMsg);
                return false;
            }
            else {
                $('#letter', '#length', '#capital', '#number', '#Li1').removeClass('valid').addClass('invalid');
                $('#pswd_info').show();
                return true;
            }
        }
    </script>
</asp:Content>
