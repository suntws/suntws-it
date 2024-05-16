<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SessionExp.aspx.cs" Inherits="TTS.SessionExp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <span style="font-size: 30px; font-weight: bold; color: Red; font-family: arial;">Your
            Cookies/Session Expired</span>
        <br />
        <asp:LinkButton runat="server" ID="lnkLoginAgain" ClientIDMode="Static" Text="Login Again"
            OnClick="lnkLoginAgain_Click"></asp:LinkButton>
    </div>
    </form>
</body>
</html>
