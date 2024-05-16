<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="tyreimagesentry.aspx.cs" Inherits="TTS.tyreimagesentry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        IMAGE UPLOAD</div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent">
        <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #CE8686;
            width: 1078px;">
            <tr>
                <td>
                    <div style="width: 760px; float: left;">
                        IMAGE FOLDER
                        <div>
                            <asp:FileUpload ID="fupImg" ClientIDMode="Static" name="name_fupImg[]" runat="server"
                                Width="660px" Visible="false" />
                            <asp:DropDownList runat="server" ID="ddlFolderNmae" ClientIDMode="Static" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlFolderNmae_IndexChange">
                            </asp:DropDownList>
                        </div>
                        <div id="fileImages" style="width: 760px; float: left;">
                        </div>
                        <asp:DataList runat="server" ID="dlImageList" RepeatColumns="1" RepeatDirection="Horizontal"
                            RepeatLayout="Table" ItemStyle-VerticalAlign="Top" Width="760px">
                            <ItemTemplate>
                                <div style="width: 760px; float: left; border: 1px solid #000000; margin: 2px;">
                                    <asp:Label runat="server" ID="lblImgName" Text='<%# Eval("ImgName") %>'></asp:Label>
                                    <div style="width: 760px; float: left; font-weight: bold; line-height: 20px;">
                                        Size:
                                        <%# Eval("ImgSize")%>
                                    </div>
                                    <%# Bind_ExistingImage(Eval("ImgUrl").ToString(), Eval("ImgWidth").ToString(), Eval("ImgHeight").ToString())%>
                                </div>
                            </ItemTemplate>
                        </asp:DataList>
                    </div>
                </td>
                <td>
                    <div style="width: 300px; float: left; margin-left: 5px; display: none;" id="divImgDdpList"
                        runat="server" clientidmode="Static">
                        <div>
                            <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label>
                        </div>
                        <div>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #CE8686;
                                        width: 300px; line-height: 25px;">
                                        <tr>
                                            <td>
                                                CATEGORY
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlimgCategory" ClientIDMode="Static" Width="100px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                PLATFORM
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlPlatform" ClientIDMode="Static" Width="135px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                BRAND
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlBrand" ClientIDMode="Static" Width="120px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                SIDEWALL
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlSidewall" ClientIDMode="Static" Width="120px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                TYPE
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlType" ClientIDMode="Static" Width="100px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                SIZE
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlSize" ClientIDMode="Static" Width="200px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                RIM
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlRim" ClientIDMode="Static" Width="80px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div style="text-align: center; margin-top: 5px;">
                            <asp:Button ID="btnSave" runat="server" Text="SAVE" CssClass="btnsave" ClientIDMode="Static"
                                OnClientClick="javascript:return CtrlImagesUploadEntry();" OnClick="btnSave_Click" /></div>
                        <div style="text-align: center; margin-top: 20px;">
                            <asp:LinkButton runat="server" ID="lnkSkipImage" ClientIDMode="Static" OnClick="lnkSkipImage_Click"
                                Text="SKIP IMAGE"></asp:LinkButton>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        $('#fupImg').change(function (evt) {
            $('#divImgDdpList').css({ 'display': 'none' });
            $('#lblErrMsg').html('');
            var array = ["bmp", "gif", "png", "jpg", "jpeg", "tif"];
            var xyz = document.getElementById('<%= fupImg.ClientID %>');
            var Extension = xyz.value.substring(xyz.value.lastIndexOf('.') + 1).toLowerCase();
            if (array.indexOf(Extension) <= -1) {
                alert("Please Upload only bmp, gif, png, jpg, tif, jpeg extension flle");
                return false;
            }
            else {
                showimages(evt.target.files);
                gotoPreviewDiv('fileImages');
            }
        });

        function showimages(files) {
            $('#divImgDdpList').css({ 'display': 'block' });
            $('#fileImages').html('');
            for (var i = 0, f; f = files[i]; i++) {
                var r = new FileReader();
                r.onload = (function (f) {
                    return function (e) {
                        var dataUri = e.target.result;
                        var datatext = f.name;
                        var imggg = document.createElement("img");
                        imggg.src = e.target.result;
                        var w = imggg.width;
                        var h = imggg.height;
                        var img = '<div style="width:760px;float:left;border: 1px solid #000000;margin:2px;"><div style="width:760px;float:left;">';
                        img += '<div style="width:760px;float:left; font-weight: bold;">Size: ' + ((f.size) / 1024).toFixed(2) + ' KB</div>';
                        if (w > 760 || h > 760) {
                            var decMultiTimes = 0;
                            if (h > w)
                                decMultiTimes = h / 760;
                            else if (w > h)
                                decMultiTimes = w / 760;
                            var imageWidth = ((w) / (decMultiTimes + 1));
                            var imageHeight = ((h) / (decMultiTimes + 1));
                            var decDivEqual = 0;
                            if (imageHeight > imageWidth)
                                decDivEqual = 755 - imageHeight;
                            else if (w > h)
                                decDivEqual = 755 - imageWidth;
                            if (w > 760)
                                w = imageWidth + decDivEqual;
                            if (h > 760)
                                h = imageHeight + decDivEqual;
                        }
                        img += '<img src="' + dataUri + '" name="' + datatext + '" id="imageupload" width="' + w + 'px" height="' + h + 'px" />';
                        img += '</div></div>';
                        $('#fileImages').append(img);
                    };
                })(f);
                r.readAsDataURL(f);
            }
        }

        function gotoPreviewDiv(ctrlID) {
            $('#' + ctrlID).css({ 'display': 'block' });
            $("html, body").stop().animate({
                scrollLeft: $('#' + ctrlID).offset().left,
                scrollTop: $('#' + ctrlID).offset().top
            }, 1200);
        }

        function CtrlImagesUploadEntry() {
            var errMsg = ''; $('#lblErrMsg').html('');
            var array = ["bmp", "gif", "png", "jpg", "jpeg", "tif"];
            var xyz = document.getElementById('<%= fupImg.ClientID %>');
            var Extension = xyz.value.substring(xyz.value.lastIndexOf('.') + 1).toLowerCase();

            if (array.indexOf(Extension) <= -1)
                errMsg += "Please upload any one bmp, gif, png, tif, jpeg extension flle ";
            if ($('#ddlimgCategory option:selected').text() != 'UNCLASSIFIED' && $('#ddlPlatform option:selected').text() == 'Choose' && $('#ddlBrand option:selected').text() == 'Choose'
            && $('#ddlSidewall option:selected').text() == 'Choose' && $('#ddlType option:selected').text() == 'Choose' && $('#ddlSize option:selected').text() == 'Choose'
            && $('#ddlRim option:selected').text() == 'Choose')
                errMsg += 'Please choose any one dropdownlist';

            if (errMsg.length > 0) {
                $('#lblErrMsg').html(errMsg);
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
