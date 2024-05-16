<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="claimpopup.aspx.cs" Inherits="TTS.claimpopup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        #spinner1
        {
            position: fixed;
            left: 0px;
            top: 0px;
            width: 100%;
            height: 100%;
            z-index: 9999;
            background: url(images/progress.gif) 50% 50% no-repeat #ede9df;
        }
        .claimImgSize
        {
            height: 550px;
            width: 900px;
        }
    </style>
    <script src="Scripts/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function closePopupOnly() {
            window.parent.TINY.box.hide();
        }
        $(window).load(function () { $("#spinner1").fadeOut("slow"); }) 
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="spinner1">
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #fff;
            width: 975px;">
            <tr>
                <td style="text-align: center;">
                    <div style="width: 950px; float: left; font-size: 20px; color: #AEAD25; font-weight: bold;">
                        <div style="width: 425px; float: left;">
                            <asp:Label runat="server" ID="lblComplaintNo" ClientIDMode="Static" Text=""></asp:Label></div>
                        <div style="width: 425px; float: left;">
                            <asp:Label runat="server" ID="lblStencilNo" ClientIDMode="Static" Text=""></asp:Label></div>
                    </div>
                    <div style="width: 20px; float: right;">
                        <img src="images/cancel.png" alt="CLOSE" onclick="closePopupOnly();" style="width: 15px;
                            cursor: pointer;" />
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <asp:GridView runat="server" ID="gvClaimImages" AutoGenerateColumns="false" AllowPaging="true"
                                PageSize="1" OnPageIndexChanging="gvClaimImages_PageIndex" PagerStyle-HorizontalAlign="Center"
                                PagerStyle-VerticalAlign="Middle" PagerSettings-Position="Top" PagerStyle-Font-Bold="true"
                                PagerStyle-Font-Size="14px" Width="900px">
                                <HeaderStyle CssClass="headerNone" />
                                <Columns>
                                    <asp:TemplateField ItemStyle-CssClass="claimImgSize">
                                        <ItemTemplate>
                                            <a href='<%# Eval("ClaimImage") %>' target="_blank" class="claimImgSize">
                                                <img alt="Claim Images" border="0" src='<%# Eval("ClaimImage") %>' class="claimImgSize">
                                            </a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnCustCode" ClientIDMode="Static" Value="" />
    </form>
</body>
</html>
