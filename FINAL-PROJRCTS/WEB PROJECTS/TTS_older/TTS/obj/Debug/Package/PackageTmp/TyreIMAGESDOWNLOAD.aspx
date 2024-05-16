<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="TyreIMAGESDOWNLOAD.aspx.cs" Inherits="TTS.TyreIMAGESDOWNLOAD" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="Scripts/cotsScript.js" type="text/javascript"></script>
    <style type="text/css">
        .scrolled
        {
            position: fixed;
            top: 1px;
        }
    </style>
    <link href="Styles/TinyStyle.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/TinyboxScript.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        TYRE IMAGE LIBRARY</div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #CE8686;
            width: 1070px;">
            <tr>
                <td>
                    <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #CE8686;
                        width: 1078px;">
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
                                    <asp:Button ID="btnShow" runat="server" Text="SHOW IMAGE LIST" CssClass="btnshow"
                                        ClientIDMode="Static" OnClick="btnShow_Click" /></div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <asp:GridView runat="server" ID="gvImageLibraryList" AutoGenerateColumns="false"
                                    Width="1076px" AlternatingRowStyle-BackColor="#f5f5f5" RowStyle-Height="18px"
                                    AllowPaging="true" OnPageIndexChanging="gvImageLibraryList_PageIndex" PageSize="10"
                                    PagerStyle-Height="30px" PagerStyle-Font-Bold="true" PagerStyle-Font-Size="15px"
                                    PagerStyle-HorizontalAlign="Center" PagerStyle-VerticalAlign="Middle">
                                    <HeaderStyle Height="22px" Font-Bold="true" Font-Size="15px" BackColor="#fefe8b" />
                                    <Columns>
                                        <asp:BoundField DataField="imgcategory" HeaderText="CATEGORY" ItemStyle-Width="" />
                                        <asp:BoundField DataField="config" HeaderText="PLATFORM" ItemStyle-Width="" />
                                        <asp:BoundField DataField="brand" HeaderText="BRAND" ItemStyle-Width="" />
                                        <asp:BoundField DataField="sidewall" HeaderText="SIDEWALL" ItemStyle-Width="" />
                                        <asp:BoundField DataField="tyretype" HeaderText="TYPE" ItemStyle-Width="" />
                                        <asp:BoundField DataField="tyresize" HeaderText="SIZE" ItemStyle-Width="" />
                                        <asp:BoundField DataField="rimsize" HeaderText="RIM" ItemStyle-Width="" />
                                        <asp:TemplateField HeaderText="COUNT" ItemStyle-Width="">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblAvailableImgCount" Text='<%#Convert.ToInt32(Eval("imgcount")) - Convert.ToInt32(Eval("Delcount"))%>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdnImgCount" Value='<%# Eval("imgcount") %>' />
                                                <asp:HiddenField runat="server" ID="hdnDelCount" Value='<%# Eval("Delcount") %>' />
                                                <asp:HiddenField runat="server" ID="hdnDelImg" Value='<%# Eval("DelImg") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ACTION">
                                            <ItemTemplate>
                                                <asp:HiddenField runat="server" ID="hdnImgName" Value='<%# Eval("imgname") %>' />
                                                <asp:LinkButton runat="server" ID="lnkShowImg" Text="SHOW IMAGES" OnClick="lnkShowImg_Click"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <div id="divImageLibraryView" style="width: 1078px; float: left; display: none;">
                                    <div id="divOrderHead" style="width: 1078px; float: left; text-align: left; font-weight: bold;">
                                        <asp:Label runat="server" ID="lblListHeading1" ClientIDMode="Static" Text="" Width="350px"></asp:Label>
                                        <asp:Label runat="server" ID="lblListHeading2" ClientIDMode="Static" Text="" Width="350px"></asp:Label>
                                        <asp:Label runat="server" ID="lblListHeading3" ClientIDMode="Static" Text="" Width="350px"></asp:Label>
                                    </div>
                                    <div>
                                        <asp:DataList runat="server" ID="dtimglist" RepeatColumns="8" RepeatDirection="Horizontal"
                                            RepeatLayout="Table">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkall" runat="server" ClientIDMode="Static" />
                                                ALL
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div id="temp" style="width: 100px; height: 115px; border: 1px solid #000; background-color: #F5F5F5;
                                                    margin: 5px;">
                                                    <div style="width: 100px; height: 100px;">
                                                        <a id="btnModalPopup" class="pop" style="cursor: pointer; height: 100px;" name='<%# Eval("ImgUrl") %>'
                                                            title='<%# Eval("ImgName") %>' charset='<%# Eval("tbImgName") %>' onclick="modalpopup(this.name,this.title,this.charset);">
                                                            <div style="width: 100px; height: 100px;">
                                                                <asp:Image runat="server" ClientIDMode="Static" ID="thumbimg" ImageUrl='<%# Eval("ThumbnailImgurl") %>' />
                                                            </div>
                                                        </a>
                                                    </div>
                                                    <div style="width: 100px; float: left; line-height: 15px; text-align: center;">
                                                        <asp:CheckBox ID="chkimage" runat="server" />
                                                        <asp:HiddenField runat="server" ID="hdndturl" ClientIDMode="Static" Value='<%# Eval("ImgUrl") %>' />
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:DataList>
                                        <table>
                                            <tr>
                                                <td style="width: 500px; float: left; text-align: center;">
                                                    <asp:LinkButton ID="lnkPrevious" runat="server" ClientIDMode="Static" OnClick="lnkPrevious_Click">Previous</asp:LinkButton>
                                                </td>
                                                <td style="width: 500px; float: right; text-align: center;">
                                                    <asp:LinkButton ID="lnkNext" runat="server" ClientIDMode="Static" OnClick="lnkNext_Click">Next</asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div style="text-align: center;">
                                        Do you want download the above category images in one zip file checked the image
                                        and
                                        <asp:LinkButton ID="lnkdownall" runat="server" OnClick="btnDownload_Click" OnClientClick="javascript:return CtrlZipDownloadChk();"
                                            Text="CLICK HERE"></asp:LinkButton>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>
        <div>
            <asp:HiddenField runat="server" ID="hdnSelectIndex" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ID="hdnImgCategory" ClientIDMode="Static" Value="" />
            <asp:HiddenField runat="server" ID="hdnimgname" ClientIDMode="Static" Value="" />
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            blinkOrderHead();
            $('#chkall').click(function () {
                var chkLength = $("[id*=MainContent_dtimglist_chkimage_]").length;
                var j = 0;
                if (chkLength > 0) {
                    if ($(this).attr('checked') == "checked")
                        $("[id*=MainContent_dtimglist_chkimage_]").attr('checked', true)
                    else
                        $("[id*=MainContent_dtimglist_chkimage_]").attr('checked', false)
                }
                else {
                    alert('No records');
                    $(this).attr('checked', false);
                }
            });
            $("[id*=MainContent_dtimglist_chkimage_]").click(function () {
                $('#chkall').attr('checked', false);
            });
        });

        function modalpopup(imgurl, name, imgname) {
            var strcatagory = $('#hdnImgCategory').val() + "~" + $('#hdnimgname').val() + "~" + name;
            TINY.box.show({ iframe: 'TyreImgPopup.aspx?imgurl=' + imgurl + '&strcatagory=' + strcatagory, boxid: 'frameless', width: 1000, height: 750, fixed: false, maskid: 'bluemask', maskopacity: 40, closejs: function () { } })
            return false;
        }

        $(window).scroll(function () {
            var scroll = $(window).scrollTop();
            if (scroll >= 470) {
                $('#divOrderHead').addClass("scrolled");
            }
            if (scroll <= 470) {
                $('#divOrderHead').removeClass("scrolled");
            }
        });

        function CtrlZipDownloadChk() {
            if ($('input:checkbox[id*=MainContent_dtimglist_chkimage_]:checked').length == 0) {
                alert('Choose any one image');
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
