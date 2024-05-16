<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="tyreimageedit.aspx.cs" Inherits="TTS.tyreimageedit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="Scripts/cotsScript.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: center;" class="pageTitleHead">
        TYRE IMAGE EDIT</div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #CE8686;
            width: 1078px; line-height: 20px;">
            <tr>
                <td colspan="2">
                    <asp:GridView runat="server" ID="gvImageLibraryList" AutoGenerateColumns="false"
                        Width="1076px" AlternatingRowStyle-BackColor="#f5f5f5" RowStyle-Height="15px"
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
                        <div style="width: 1078; float: left;">
                            <asp:DataList runat="server" ID="dtimglist" RepeatColumns="8" RepeatDirection="Horizontal"
                                RepeatLayout="Table">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hdndturl" ClientIDMode="Static" Value='<%# Eval("ImgUrl") %>' />
                                    <div id="temp" style="width: 100px; height: 115px; border: 1px solid #000; background-color: #F5F5F5;
                                        margin: 5px;">
                                        <div style="width: 100px; height: 100px;">
                                            <asp:Image runat="server" ClientIDMode="Static" ID="thumbimg" ImageUrl='<%# Eval("ThumbnailImgurl") %>' />
                                        </div>
                                        <div style="width: 100px; float: left; line-height: 15px; text-align: center;">
                                            <asp:LinkButton ID="lnkEdit" runat="server" ClientIDMode="Static" OnClick="lnkEdit_Click"
                                                Text="EDIT"></asp:LinkButton>
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
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="imgedit" style="display: none;">
                    </div>
                </td>
                <td>
                    <div id="imgdata" style="display: none;">
                        <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #CE8686;
                            width: 300px; line-height: 25px;">
                            <tr>
                                <td>
                                    CATEGORY
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlimgCategory" ClientIDMode="Static" Width="100px"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddlimgCategory_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtimgCategory" runat="server" ClientIDMode="Static" Visible="false"></asp:TextBox>
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
                            <tr>
                                <td colspan="2" style="line-height: 15px;">
                                    <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div style="text-align: center; margin-top: 5px;">
                                        <asp:Button ID="btnSave" runat="server" Text="SAVE" CssClass="btnsave" ClientIDMode="Static"
                                            OnClientClick="javascript:return Ctrlsave();" OnClick="btnSave_Click" /></div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnURL" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hdnSelectIndex" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hdnimgurl" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnimgname" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnimgCategory" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnDelcount" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnwidth" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnheigth" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        $(document).ready(function () {
            blinkOrderHead();
        });
        function imageupload(imgWidth, imgheight) {
            $('#imgedit').css({ 'display': 'block' });
            var url = $('#hdnimgurl').val();
            var img = '<img src="' + url + '" height="' + imgheight + 'px" width="' + imgWidth + 'px">';
            $('#imgedit').append(img);
            $('#imgdata').css({ 'display': 'block' });
            gotoPreviewDiv('imgedit');
        }
        function displayDiv(ctrlID) {
            $('#' + ctrlID).css({ 'display': 'none' });
        }
        function Ctrlsave() {
            var errMsg = '';
            if ($('#ddlimgCategory option:selected').text() == 'ADD NEW CATEGORY' && $('#txtimgCategory').val().length == 0)
                errMsg += 'Enter image category<br/>';
            if ($('#ddlPlatform option:selected').text() == 'Choose' || $('#ddlPlatform option:selected').text() == '')
                errMsg += 'Choose platform<br/>';
            if ($('#ddlBrand option:selected').text() == 'Choose' || $('#ddlBrand option:selected').text() == '')
                errMsg += 'Choose brand<br/>';
            if ($('#ddlSidewall option:selected').text() == 'Choose' || $('#ddlSidewall option:selected').text() == '')
                errMsg += 'Choose sidewall<br/>';
            if ($('#ddlType option:selected').text() == 'Choose' || $('#ddlType option:selected').text() == '')
                errMsg += 'Choose type<br/>';
            if ($('#ddlSize option:selected').text() == 'Choose' || $('#ddlSize option:selected').text() == '')
                errMsg += 'Choose size<br/>';
            if ($('#ddlRim option:selected').text() == 'Choose' || $('#ddlRim option:selected').text() == '')
                errMsg += 'Choose rim<br/>';
            if (errMsg.length > 0) {
                $('#lblErrMsg').html(errMsg);
                return false;
            }
            else
                return true;
        }
       
    </script>
</asp:Content>
