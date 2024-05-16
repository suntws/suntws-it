<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="cotsdomesticmaillist.aspx.cs" Inherits="TTS.cotsdomesticmaillist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        <asp:Label runat="server" ID="lblPageHead" ClientIDMode="Static" Text=""></asp:Label>
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <div style="padding-left: 5px;">
            <table>
                <tr>
                    <td>
                        <asp:GridView runat="server" ID="gvCotsDomesticMailAlert" AutoGenerateColumns="false"
                            HeaderStyle-Height="24px" Width="1065px" HeaderStyle-BackColor="#6DF3BE" AlternatingRowStyle-BackColor="#f5f5f5"
                            RowStyle-Height="24px" OnRowEditing="gvCotsDomesticMailAlert_RowEditing" OnRowUpdating="gvCotsDomesticMailAlert_RowUpdating"
                            OnRowCancelingEdit="gvCotsDomesticMailAlert_RowCanceling">
                            <Columns>
                                <asp:TemplateField HeaderText="SL NO." ItemStyle-Width="40px">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblSlNo" Text='<%# Eval("ID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="MAIL TYPE" ItemStyle-Width="250px">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblMailType" Text='<%#Eval("MailType") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="RECEIPENT" ItemStyle-Width="300px">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblReceipent" Text='<%#Eval("MailReceipent") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox runat="server" ID="txtReceipent" Text='<%#Eval("MailReceipent") %>'
                                            TextMode="MultiLine" Width="290px" Height="70px" onKeyUp="javascript:CheckMaxLength(this, 999);"
                                            onChange="javascript:CheckMaxLength(this, 999);" Enabled='<%# DataBinder.Eval(Container.DataItem,"MailReceipent").ToString()=="CUSTOMER"?false:true %>'></asp:TextBox>
                                        <span style="font-size: 11px; color: #2809C9;">Note: Put comma (,) for more mail-id</span>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="CC" ItemStyle-Width="300px">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblMailCC" Text='<%# Eval("MailCC") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox runat="server" ID="txtMailCC" Text='<%#Eval("MailCC") %>' TextMode="MultiLine"
                                            Width="290px" Height="70px" onKeyUp="javascript:CheckMaxLength(this, 999);" onChange="javascript:CheckMaxLength(this, 999);"
                                            Enabled='<%# DataBinder.Eval(Container.DataItem,"MailCC").ToString()=="CUSTOMER"?false:true %>'></asp:TextBox>
                                        <span style="font-size: 11px; color: #2809C9;">Note: Put comma (,) for more mail-id</span>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ACTION">
                                    <ItemTemplate>
                                        <asp:Button runat="server" ID="btnEdit" Text="Edit" CommandName="Edit" CssClass="btnedit" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Button runat="server" ID="btnUpdate" Text="Update" CommandName="Update" />
                                        <asp:Button runat="server" ID="btnCancel" Text="Cancel" CommandName="Cancel" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
