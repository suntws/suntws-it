<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="prospectdelete.aspx.cs" Inherits="TTS.prospectdelete" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/prospectStyle.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/prospectScript.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div align="center" class="pageTitleHead">
        DELETE CUSTOMER</div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <div style="padding-left: 10px;">
            <table>
                <tr>
                    <td>
                        <asp:GridView runat="server" ID="gv_ProspectDeleteList" AutoGenerateColumns="false"
                            HeaderStyle-BackColor="#F3E756" AlternatingRowStyle-BackColor="#f5f5f5" OnRowDeleting="gv_ProspectDeleteList_RowDeleting"
                            AllowSorting="true" OnSorting="gv_LeadCustList_sorting" Width="1060px" AllowPaging="true"
                            OnPageIndexChanging="gvProspectDeleteList_PageIndex" PageSize="50" PagerStyle-Height="30px"
                            PagerStyle-Font-Bold="true" PagerStyle-Font-Size="15px" PagerStyle-HorizontalAlign="Center"
                            PagerStyle-VerticalAlign="Middle">
                            <Columns>
                                <asp:BoundField DataField="Custcode" HeaderText="CODE" ItemStyle-Width="70px" />
                                <asp:BoundField DataField="Custname" HeaderText="NAME" SortExpression="Custname"
                                    ItemStyle-Width="250px" />
                                <asp:BoundField DataField="Country" HeaderText="COUNTRY" SortExpression="Country"
                                    ItemStyle-Width="220px" />
                                <asp:BoundField DataField="port" HeaderText="SOURCE" ItemStyle-Width="200px" SortExpression="port" />
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        HISTORY</HeaderTemplate>
                                    <ItemTemplate>
                                        <div>
                                            <%# Bind_SupplierDetails(Eval("Custcode").ToString())%>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="focus" HeaderText="FOCUS" SortExpression="focus" ItemStyle-Width="50px" />
                                <asp:TemplateField HeaderText="FLAG" SortExpression="flag" ItemStyle-Width="50px">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblListFlag" Text='<%# Eval("flag") %>'></asp:Label></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="100px">
                                    <HeaderTemplate>
                                        DELETE</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Button runat="server" ID="btnProsCustDelete" Text="Delete" CommandName="Delete"
                                            CssClass="btncustdel" /></ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
