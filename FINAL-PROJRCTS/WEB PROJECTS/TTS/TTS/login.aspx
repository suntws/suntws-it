<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="TTS.login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>LOGIN</title>
    <style type="text/css">
        body
        {
            background-image: url(images/hide.jpg);
            background-position: left top;
            background-repeat: repeat-x;
        }
        .divlogin
        {
            background-image: url(images/loginbg.jpg);
            background-position: left top;
            background-repeat: no-repeat;
            width: 400px;
            height: 300px;
            margin: 15% 0px 0px 35%;
        }
        .divTxtBox
        {
            width: 285px;
            float: left;
            margin-left: 120px;
        }
        .divBtn
        {
            width: 330px;
            float: left;
            margin-left: 70px;
            margin-top: 20px;
        }
        .divErrMsg
        {
            width: 335px;
            float: left;
            margin-left: 65px;
            margin-top: 5px;
            color: #f00;
        }
        .txtStyle
        {
            height: 25px;
            width: 195px;
            border: none;
            background-color: #DBE0E4 !important;
            font-size: 15px;
            font-weight: bold;
        }
        .txtStyle:focus
        {
            outline: 0;
        }
        .btnStyle
        {
            height: 35px;
            width: 260px;
        }
        input:-webkit-autofill
        {
            background-color: #DBE0E4 !important;
            -webkit-box-shadow: 0 0 0px 1000px #DBE0E4 inset;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <script type="text/javascript">
        function login_click() {
            var name = document.getElementById('txtUserName').value; var pwd = document.getElementById('txtPassword').value;
            if (name.length > 0 && pwd.length > 0) { return true; }
            else if (name.length == 0) { document.getElementById("errmsg").innerHTML = "Enter your name"; document.getElementById("txtUserName").focus(); return false; }
            else if (name.length > 0 && pwd.length == 0) { document.getElementById("errmsg").innerHTML = "Enter your password"; document.getElementById("txtPassword").focus(); return false; }
            else { document.getElementById("errmsg").innerHTML = "Enter Correct Values"; return false; }
        }
        window.onload = function () { document.getElementById("txtUserName").focus(); };
    </script>
    <div class="divlogin">
        <asp:Panel runat="server" ID="LoginPanel" DefaultButton="lnkLogin">
            <div style="margin-top: 110px;" class="divTxtBox">
                <asp:TextBox runat="server" ID="txtUserName" ClientIDMode="Static" Text="" CssClass="txtStyle"></asp:TextBox>
            </div>
            <div style="margin-top: 25px;" class="divTxtBox">
                <asp:TextBox runat="server" ID="txtPassword" TextMode="Password" ClientIDMode="Static"
                    Text="" CssClass="txtStyle" BackColor="#DBE0E4"></asp:TextBox>
            </div>
            <div class="divBtn">
                <asp:LinkButton runat="server" ID="lnkLogin" ClientIDMode="Static" Text="" OnClientClick="javascript:return login_click();"
                    OnClick="lnkLogin_click"><div class="btnStyle"></div></asp:LinkButton>
            </div>
            <div id="errmsg" runat="server" class="divErrMsg">
            </div>
        </asp:Panel>
    </div>
    <asp:HiddenField runat="server" ID="hdnReturnUrl" Value="" ClientIDMode="Static" />
    </form>
</body>
</html>
