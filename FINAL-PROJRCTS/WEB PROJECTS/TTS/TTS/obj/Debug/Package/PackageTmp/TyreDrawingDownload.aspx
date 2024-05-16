<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="TyreDrawingDownload.aspx.cs" Inherits="TTS.TyreDrawingDownload" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/TinyStyle.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/TinyboxScript.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        APPROVED DRAWING DOWNLOAD</div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #CE8686;
            width: 1065px;">
            <tr style="line-height: 22px;">
                <td>
                    FILE CATEGORY
                </td>
                <td>
                    <asp:DropDownList ID="ddlFileCategory" runat="server" ClientIDMode="Static">
                    </asp:DropDownList>
                </td>
                <td>
                    ETRTO REF
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlETRTOREF" ClientIDMode="Static">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    PLATFORM
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlPlatform" ClientIDMode="Static" Width="120px">
                    </asp:DropDownList>
                </td>
                <td>
                    RIM TYPE
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlRIMTYPE" ClientIDMode="Static">
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
                <td>
                    NO OF STUD HOLES
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlNoOfHoles" ClientIDMode="Static">
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
                <td>
                    PCD
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlPCD" ClientIDMode="Static">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    GRADE
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlType" ClientIDMode="Static" Width="100px">
                    </asp:DropDownList>
                </td>
                <td>
                    BOREDIA
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlBOREDIA" ClientIDMode="Static">
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
                <td>
                    STUD HOLES DIA
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlHoleDia" ClientIDMode="Static">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    RIM WIDTH
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlRim" ClientIDMode="Static" Width="60px">
                    </asp:DropDownList>
                </td>
                <td colspan="2">
                    <asp:Label runat="server" ID="lblMsg" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    CUSTOMER SPECIFIC
                </td>
                <td colspan="3">
                    <asp:DropDownList runat="server" ID="ddlCustomerDwg" ClientIDMode="Static">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <div style="text-align: center; margin-top: 5px;">
                        <asp:Button ID="btnShow" runat="server" Text="SHOW DRAWING LIST" CssClass="btnshow"
                            ClientIDMode="Static" OnClick="btnShow_Click" /></div>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:GridView runat="server" ID="gvDwgLibraryList" AutoGenerateColumns="false" Width="1076px"
                        AlternatingRowStyle-BackColor="#f5f5f5" AllowPaging="true" PageSize="25" PagerStyle-Height="30px"
                        PagerStyle-Font-Bold="true" PagerStyle-Font-Size="15px" PagerStyle-HorizontalAlign="Center"
                        OnPageIndexChanging="gvDwgLibraryList_PageIndex" PagerStyle-VerticalAlign="Middle">
                        <HeaderStyle Font-Bold="true" BackColor="#fefe8b" />
                        <Columns>
                            <asp:TemplateField HeaderText="CATEGORY">
                                <ItemTemplate>
                                    <asp:Label ID="lblgrfilecategory" runat="server" Text='<%# Eval("filecategory") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="config" HeaderText="PLATFORM" ItemStyle-Width="" />
                            <asp:BoundField DataField="brand" HeaderText="BRAND" ItemStyle-Width="" />
                            <asp:BoundField DataField="sidewall" HeaderText="SIDEWALL" ItemStyle-Width="" />
                            <asp:BoundField DataField="tyretype" HeaderText="GRADE" ItemStyle-Width="" />
                            <asp:BoundField DataField="tyresize" HeaderText="SIZE" ItemStyle-Width="" />
                            <asp:BoundField DataField="rimwidth" HeaderText="RIM WIDTH" ItemStyle-Width="" />
                            <asp:BoundField DataField="ETRTOREF" HeaderText="ETRTO REF" ItemStyle-Width="" />
                            <asp:BoundField DataField="RIMTYPE" HeaderText="RIM TYPE" ItemStyle-Width="" />
                            <asp:BoundField DataField="NoOfHoles" HeaderText="NO OF STUD HOLES" ItemStyle-Width="" />
                            <asp:BoundField DataField="PCD" HeaderText="PCD" ItemStyle-Width="" />
                            <asp:BoundField DataField="BOREDIA" HeaderText="BOREDIA" ItemStyle-Width="" />
                            <asp:BoundField DataField="HoleDia" HeaderText="STUD HOLES DIA" ItemStyle-Width="" />
                            <asp:TemplateField HeaderText="ACTION" ItemStyle-Width="100px" ItemStyle-Font-Size="10px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblDrwaingNo" Text='<%# Eval("DrwaingNo") %>'></asp:Label>
                                    <br />
                                    <asp:LinkButton runat="server" ID="lblDownload" Text="DOWNLOAD" OnClick="lblDownload_Click"></asp:LinkButton>
                                    <br />
                                    <div style="float:left;">
                                        <%# Bind_DrwaingPdfLink(Eval("PRODUCTDRAWING").ToString(), (Convert.ToInt32(Eval("RIMDRAWING").ToString())).ToString(), Eval("filecategory").ToString(), Eval("DrwaingNo").ToString())%>
                                        </div>
                                    <div style=' float:right;'><a onclick="TINY.box.show({iframe:'TyreDwgPopup.aspx?dwgid=<%# Eval("ID") %>&aid=down&Pid=<%# Eval("Pid") %>',boxid:'frameless',width:1000,height:750,fixed:false,maskid:'bluemask',maskopacity:40,closejs:function(){}})"
                                            style="cursor: pointer; text-decoration: underline; color: #082DEA;">
                                            <%# Eval("lnkText")%></a></div>
                                    <asp:HiddenField ID="hdnsrtname" runat="server" ClientIDMode="Static" Value='<%# Eval("PRODUCTDRAWING") %>' />
                                    <asp:HiddenField ID="hdnstrPdfCount" runat="server" ClientIDMode="Static" Value='<%# Eval("RIMDRAWING") %>' />
                                    <%--<asp:HiddenField ID="hdnDwgApprove" runat="server" ClientIDMode="Static" Value='<%# Eval("DwgApprove") %>' />--%>
                                    <asp:HiddenField ID="hdnfilecategory" runat="server" ClientIDMode="Static" Value='<%# Eval("filecategory") %>' />
                                    <asp:HiddenField ID="hdnid" runat="server" ClientIDMode="Static" Value='<%# Eval("ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hdnDwgCategory" runat="server" ClientIDMode="Static" Value="" />
    </div>
</asp:Content>
