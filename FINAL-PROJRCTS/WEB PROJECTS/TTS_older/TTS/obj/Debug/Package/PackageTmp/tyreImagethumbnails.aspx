<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="tyreImagethumbnails.aspx.cs" Inherits="TTS.tyreImagethumbnails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/TinyStyle.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/TinyboxScript.js" type="text/javascript"></script>
    <style type="text/css">
        .thumbText
        {
            width: 100px;
            float: left;
            font-size: 8px;
            color: #021EDF;
        }
        .thumbText:hover
        {
            font-size: 12px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        ALL IMAGES LIBRARY</div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #CE8686;
            width: 1070px;">
            <tr style="text-align: center; background-color: #8BCDDD; font-weight: bold;">
                <td>
                    CATEGORY
                </td>
                <td>
                    PLATFORM
                </td>
                <td>
                    BRAND
                </td>
                <td>
                    SIDEWALL
                </td>
                <td>
                    TYPE
                </td>
                <td>
                    SIZE
                </td>
                <td>
                    RIM
                </td>
            </tr>
            <tr>
                <td>
                    <asp:DropDownList runat="server" ID="ddlimgCategory" ClientIDMode="Static" Width="100px">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlPlatform" ClientIDMode="Static" Width="135px">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlBrand" ClientIDMode="Static" Width="120px">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlSidewall" ClientIDMode="Static" Width="120px">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlType" ClientIDMode="Static" Width="100px">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlSize" ClientIDMode="Static" Width="200px">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlRim" ClientIDMode="Static" Width="80px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="7">
                    <div style="text-align: center; margin-top: 5px;">
                        <asp:Button ID="btnShow" runat="server" Text="SHOW IMAGE THUMBNAILS" CssClass="btnshow"
                            ClientIDMode="Static" OnClientClick="javascript:return CtrlImgThumbnails();"
                            OnClick="btnShow_Click" /></div>
                </td>
            </tr>
            <tr>
                <td colspan="7">
                    <div id="divImageLibraryView" style="width: 1078px; float: left;">
                        <div>
                            <asp:DataList runat="server" ID="dlimglist" RepeatColumns="8" RepeatDirection="Horizontal"
                                RepeatLayout="Table">
                                <ItemTemplate>
                                    <div id="temp" style="width: 100px; height: 115px; margin: 5px 15px 5px 5px;">
                                        <div style="width: 100px; height: 100px; border: 1px solid #000; background-color: #F5F5F5;">
                                            <a id="btnModalPopup" class="pop" style="cursor: pointer;" name='<%# Eval("ImgURL") %>'
                                                title='<%# Eval("ImgName") %>' charset='<%# Eval("tbImgName") %>' onclick="modalpopup(this.name,this.title,this.charset);">
                                                <div style="width: 100px; height: 100px;">
                                                    <img id="imgthub" class="lazy" data-original='<%# Eval("ThumbnailImgurl") %>' height='<%# Eval("ImgHeight") %>'
                                                        width='<%# Eval("ImgWidth") %>' />
                                                </div>
                                            </a>
                                        </div>
                                        <div class="thumbText">
                                            <%#((string)Eval("tbImgName")).Replace("NA-", "").Replace("-NA", "")%>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:DataList>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
        <asp:HiddenField runat="server" ID="hdnImgCategory" ClientIDMode="Static" Value="" />
        <asp:HiddenField runat="server" ID="hdnimgname" ClientIDMode="Static" Value="" />
    </div>
    <script src="Scripts/jquery.lazyload.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            jQuery("img.lazy").lazyload();
        });
        function modalpopup(imgurl, name, imgname) {
            var strcatagory = $('#hdnImgCategory').val() + "~" + imgname + "~" + name;
            TINY.box.show({ iframe: 'TyreImgPopup.aspx?imgurl=' + imgurl + '&strcatagory=' + strcatagory, boxid: 'frameless', width: 1000, height: 750, fixed: false, maskid: 'bluemask', maskopacity: 40, closejs: function () { } })
            return false;
        }

        function CtrlImgThumbnails() {
            if ($('#ddlimgCategory option:selected').text() == 'Choose') {
                alert('Choose category');
                return false;
            }
            else {
                return true;
            }
        }
    </script>
</asp:Content>
