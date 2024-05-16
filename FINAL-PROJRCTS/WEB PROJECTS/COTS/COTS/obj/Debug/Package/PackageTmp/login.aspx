<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="COTS.login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>SCOTS</title>
    <style type="text/css">
        body
        {
            background-color: #000;
        }
        .divCotsLogin
        {
            background-image: url(images/bg1.jpg);
            background-position: left top;
            background-repeat: no-repeat;
            width: 585px;
            height: 295px;
            margin: 15% 0px 0px 30%;
        }
        .divLoginTxtBox
        {
            width: 345px;
            float: left;
            margin-left: 240px;
        }
        .divErrMsg
        {
            width: 370px;
            float: left;
            margin-left: 210px;
            color: #f00;
        }
        .txtStyle
        {
            height: 25px;
            width: 265px;
            border: none;
            background-color: #fff !important;
            font-size: 15px;
            font-weight: bold;
        }
        .txtStyle:focus
        {
            outline: 0;
        }
        .divBtn
        {
            width: 70px;
            float: left;
            margin-left: 330px;
            margin-top: 25px;
        }
        .btnStyle
        {
            height: 40px;
            width: 70px;
        }
        input:-webkit-autofill
        {
            background-color: #DBE0E4 !important;
            -webkit-box-shadow: 0 0 0px 1000px #DBE0E4 inset;
        }
        .divSiteUrl
        {
            width: 225px;
            float: left;
            text-align: left;
            margin-top: -35px;
            line-height: 20px;
            font-size: 13px;
            margin-left: 30px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <script type="text/javascript">
        function login_click() {
            document.getElementById("errmsg").innerHTML = ""; var name = document.getElementById('txtUserName').value;
            var pwd = document.getElementById('txtPassword').value; var reURL = document.getElementById('<%=hdnReturnUrl.ClientID%>').value;
            if (name.length > 0 && pwd.length > 0) { return true; }
            else if (name.length == 0) { document.getElementById("errmsg").innerHTML = "Enter your name"; document.getElementById("txtUserName").focus(); return false; }
            else if (name.length > 0 && pwd.length == 0) { document.getElementById("errmsg").innerHTML = "Enter your password"; document.getElementById("txtPassword").focus(); return false; }
            else { document.getElementById("errmsg").innerHTML = "Enter Correct Values"; return false; }
        }
        window.onload = function () { document.getElementById("txtUserName").focus(); };
    </script>
    <div class="divCotsLogin">
        <asp:Panel runat="server" ID="LoginPanel" DefaultButton="lnkLogin">
            <div style="margin-top: 110px;" class="divLoginTxtBox">
                <asp:TextBox runat="server" ID="txtUserName" ClientIDMode="Static" Text="" CssClass="txtStyle"></asp:TextBox>
            </div>
            <div style="margin-top: 30px;" class="divLoginTxtBox">
                <asp:TextBox runat="server" ID="txtPassword" TextMode="Password" ClientIDMode="Static"
                    Text="" CssClass="txtStyle" BackColor="#DBE0E4"></asp:TextBox>
            </div>
            <div class="divBtn">
                <asp:LinkButton runat="server" ID="lnkLogin" ClientIDMode="Static" Text="" OnClientClick="javascript:return login_click();"
                    OnClick="lnkLogin_click"><div class="btnStyle"></div></asp:LinkButton>
            </div>
            <div class="divSiteUrl">
                <div style="width: 225px; float: left;">
                    Email: crm@sun-tws.com</div>
                <div style="width: 225px; float: left;">
                    Website: <a href="http://www.sun-tws.com" target="_blank">www.sun-tws.com</a></div>
            </div>
            <div id="errmsg" runat="server" class="divErrMsg">
            </div>
        </asp:Panel>
    </div>
    <asp:HiddenField runat="server" ID="hdnReturnUrl" Value="" ClientIDMode="Static" />
    </form>
</body>
</html>
