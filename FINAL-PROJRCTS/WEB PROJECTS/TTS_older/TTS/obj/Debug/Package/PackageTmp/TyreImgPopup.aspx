<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TyreImgPopup.aspx.cs" Inherits="TTS.TyreImgPopup" %>

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
    </style>
</head>
<body>
    <script src="Scripts/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function popup(imgurl, name, imgname) {
            getImageSize(imgurl);
            jQuery("#imgpop").attr('src', imgurl);
            return false;
        }

        function getImageSize(imgSrc) {
            $("<img/>").attr("src", imgSrc).load(function () {
                s = { w: this.width, h: this.height };
                var imageHeight = s.h;
                var imageWidth = s.w;
                if (s.w > 760 || s.h > 760) {
                    var decmultitimes = 0;
                    if (s.h > s.w)
                        decmultitimes = s.h / 760;
                    else if (s.h < s.w)
                        decmultitimes = s.w / 760;
                    var imgWidth = s.w / (decmultitimes + 1);
                    var imgHeight = s.h / (decmultitimes + 1);
                    var decDivEqual = 0;
                    if (imgHeight > imgWidth)
                        decDivEqual = 755 - imgHeight;
                    else if (imgWidth > imgHeight)
                        decDivEqual = 755 - imgWidth;
                    if (imageWidth > 760)
                        imageWidth = imgWidth + decDivEqual;
                    if (imageHeight > 760)
                        imageHeight = imgHeight + decDivEqual;
                } else {
                    imageHeight = s.h;
                    imageWidth = s.w;
                }
                jQuery("#imgpop").attr('height', imageHeight + "px");
                jQuery("#imgpop").attr('width', imageWidth + "px");

            });
        }
        function closePopup() {
            window.parent.TINY.box.hide();
            window.parent.location.href = 'tyreimagesdownload.aspx';
        }
        function openedit() {
            window.parent.TINY.box.hide();
            var img = document.getElementById('imgpop');
            var strupath = img.getAttribute('src');
            var strcatagory = $('#hdntgname').val();
            window.parent.location.href = $('#hdnVirtualPath').val() + 'tyreimageedit.aspx?strupath=' + strupath + '&filename1=' + strcatagory;
        }
        function closePopupOnly() {
            window.parent.TINY.box.hide();
        }

        $(window).load(function () { $("#spinner1").fadeOut("slow"); }) 
    </script>
    <form id="form1" runat="server">
    <div id="spinner1">
    </div>
    <div id="contentText">
        <table style="width: 970px;">
            <tr>
                <td colspan="2" style="text-align: right;">
                    <img src="images/cancel.png" alt="CLOSE" onclick="closePopupOnly();" style="width: 15px;
                        cursor: pointer;" />
                </td>
            </tr>
            <tr>
                <td style="text-align: center;">
                    <img id="imgpop" alt="pop img" />
                </td>
                <td>
                    <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #CE8686;
                        line-height: 17px; font-size: 12px;">
                        <tr>
                            <td style="width: 50px;">
                                CATEGORY
                            </td>
                            <td>
                                <asp:Label ID="lblCategory" runat="server" Text="" ClientIDMode="Static"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                PLATFORM
                            </td>
                            <td>
                                <asp:Label ID="lblPlatform" runat="server" Text="" ClientIDMode="Static"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                BRAND
                            </td>
                            <td>
                                <asp:Label ID="lblBrand" runat="server" Text="" ClientIDMode="Static"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                SIDEWALL
                            </td>
                            <td>
                                <asp:Label ID="lblSideWall" runat="server" Text="" ClientIDMode="Static"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                TYPE
                            </td>
                            <td>
                                <asp:Label ID="lblType" runat="server" Text="" ClientIDMode="Static"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                SIZE
                            </td>
                            <td>
                                <asp:Label ID="lblSize" runat="server" Text="" ClientIDMode="Static"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                RIM
                            </td>
                            <td>
                                <asp:Label ID="lblRim" runat="server" Text="" ClientIDMode="Static"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: center;">
                                <asp:LinkButton runat="server" ID="lnkImgDownload" ClientIDMode="Static" Text="DOWNLOAD"
                                    OnClick="lnkImgDownload_Click"></asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center;">
                                <asp:LinkButton ID="btnEdit" Text="" Style="color: #082DEA;" runat="server" OnClientClick="javascript:return openedit();"></asp:LinkButton>
                            </td>
                            <td style="text-align: center;">
                                <asp:LinkButton ID="lnkImgDelete" Text="" Style="color: #082DEA;" runat="server"
                                    OnClick="btnImgDelete_Click"></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hdnimgurl" runat="server" ClientIDMode="Static" Value="" />
        <asp:HiddenField ID="hdnname" runat="server" ClientIDMode="Static" Value="" />
        <asp:HiddenField ID="hdntgname" runat="server" ClientIDMode="Static" Value="" />
        <asp:HiddenField runat="server" ID="hdnVirtualPath" ClientIDMode="Static" Value="" />
    </div>
    </form>
</body>
</html>
