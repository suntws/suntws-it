<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="claimLibraryView.aspx.cs" Inherits="TTS.claimLibraryView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .tableReq
        {
            border-collapse: collapse;
            border-color: #000;
            width: 1068px;
            line-height: 20px;
            margin-top: 5px;
        }
        .tableReq th:first-child
        {
            background-color: #FFD8D5;
            text-align: left;
            padding-left: 10px;
            width: 240px;
            font-weight: bold;
        }
        .tableReq tr:nth-child(odd) td
        {
            background-color: #FFFCE0;
        }
        .tableReq tr:nth-child(even) td
        {
            background-color: #BBF3D5;
        }
    </style>
    <link href="Styles/lightbox.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div align="center" class="pageTitleHead">
        CLAIM LIBRARY FOR SOLID TYRES
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table>
            <tr>
                <td style="text-align: center;">
                    <span class="headCss" style="width: 300px;">COMPLAINT TYPE</span>
                    <asp:DropDownList ID="ddlComplainttype" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="ddlComplainttype_SelectedIndexChanged"
                        AutoPostBack="true" Width="600px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView runat="server" ID="gvClaimLibrary" AutoGenerateColumns="false" Width="1070px"
                        AlternatingRowStyle-BackColor="#f5f5f5" AllowPaging="true" PagerStyle-Font-Bold="true"
                        PagerStyle-Font-Size="15px" PagerStyle-HorizontalAlign="Center" PagerStyle-VerticalAlign="Middle"
                        PageSize="1">
                        <HeaderStyle CssClass="headerNone" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <table cellspacing="0" rules="all" border="1" class="tableReq">
                                        <tr>
                                            <th>
                                                COMPLAINT TYPE
                                            </th>
                                            <td>
                                                <%# Eval("Complaintype") %>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                APPEARANCE
                                            </th>
                                            <td>
                                                <%#((string)Eval("Apperance")).Replace("~", "<br/>")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                SUSPECTED MANUFACTURING END
                                            </th>
                                            <td>
                                                <%#((string)Eval("ManufacturingEnd")).Replace("~", "<br/>")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                SUSPECTED CUSTOMER END
                                            </th>
                                            <td>
                                                <%#((string)Eval("CustomerEnd")).Replace("~", "<br/>")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                ACTION
                                            </th>
                                            <td>
                                                <%#((string)Eval("actioncomments")).Replace("~", "<br/>")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                WARRANTY
                                            </th>
                                            <td>
                                                <%#((string)Eval("warranty")).Replace("~", "<br/>")%>
                                                <asp:HiddenField ID="hdnID" runat="server" Value='<%# Eval("ID") %>' />
                                                <asp:HiddenField ID="hdnimgcount" runat="server" Value='<%# Eval("imgcount") %>' />
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:DataList runat="server" ID="dtimglist" RepeatColumns="5" RepeatDirection="Horizontal"
                        RepeatLayout="Table" Width="1078px">
                        <ItemTemplate>
                            <div style="width: 200px; height: 200px; border: 1px solid #000; background-color: #F5F5F5;
                                margin: 5px;">
                                <asp:HiddenField ID="hdnurl" runat="server" ClientIDMode="Static" Value='<%# Eval("ImgUrl") %>' />
                                <a id="imageLink" name="nullDownload" href='<%# Eval("ImgUrl") %>' rel="lightbox[Brussels]" runat="server">
                                    <asp:Image ID="Image1" ImageUrl='<%#  Eval("ImgUrl") %>' runat="server" Width="200px"
                                        Height="200px" /></a>
                            </div>
                        </ItemTemplate>
                    </asp:DataList>
                </td>
            </tr>
        </table>
    </div>
    <script src="Scripts/lightbox.js" type="text/javascript"></script>
</asp:Content>
