<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="quotediscount.aspx.cs" Inherits="TTS.quotediscount" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div align="center" class="pageTitleHead">
        QUOTE DISCOUNT STRUCTURE</div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <div style="padding-left: 5px;">
            <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #000;
                width: 1070px;">
                <tr>
                    <td style="text-align: left;">
                        <span class="headCss" style="width: 100px; float: left; line-height: 30px;">GRADE</span>
                        <asp:RadioButtonList runat="server" ID="rdbDiscGrade" ClientIDMode="Static" Width="300px"
                            RepeatColumns="4" OnSelectedIndexChanged="rdbDiscGrade_IndexChange" AutoPostBack="true">
                            <asp:ListItem Text="A" Value="A"></asp:ListItem>
                            <asp:ListItem Text="B" Value="B"></asp:ListItem>
                            <asp:ListItem Text="C" Value="C"></asp:ListItem>
                            <asp:ListItem Text="D" Value="D"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView runat="server" ID="gvCustTypeDisc" AutoGenerateColumns="false" Width="1060px"
                            HeaderStyle-BackColor="#F3E756" AlternatingRowStyle-BackColor="#f5f5f5" RowStyle-Height="25px"
                            OnRowEditing="gv_RowEditing" OnRowUpdating="gv_RowUpdating" OnRowCancelingEdit="gv_RowCanceling">
                            <Columns>
                                <asp:TemplateField HeaderText="GROUP" ItemStyle-Width="80px">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblQDSno" ClientIDMode="Static" Text='<%# Eval("Sno") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="CUSTOMER TYPE" ItemStyle-Width="300px">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblQDCustType" ClientIDMode="Static" Text='<%# Eval("CustType") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="LEAD<br/>MAX %" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblQDLeadPer" ClientIDMode="Static" Text='<%# Eval("LeadPer") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox runat="server" ID="txtQLeadPer" ClientIDMode="Static" Text='<%# Eval("LeadPer") %>'
                                            Width="70px" onkeypress="return isNumberKey(event)" MaxLength="5"></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="SUPERVISOR<br/> MAX %" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblQDSupervisorPer" ClientIDMode="Static" Text='<%# Eval("SupervisorPer") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox runat="server" ID="txtQDSupervisorPer" ClientIDMode="Static" Text='<%# Eval("SupervisorPer") %>'
                                            Width="70px" onkeypress="return isNumberKey(event)" MaxLength="5"></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="MANAGER<br/> MAX %" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblQDManagerPer" ClientIDMode="Static" Text='<%# Eval("ManagerPer") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox runat="server" ID="txtQDManagerPer" ClientIDMode="Static" Text='<%# Eval("ManagerPer") %>'
                                            Width="70px" onkeypress="return isNumberKey(event)" MaxLength="5"></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ACTION" ItemStyle-Width="200px">
                                    <ItemTemplate>
                                        <asp:Button runat="server" ID="btnEdit" Text="EDIT" CommandName="Edit" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Button runat="server" ID="btnUpdate" Text="SAVE" CommandName="Update" />
                                        <asp:Button runat="server" ID="btnCancel" Text="CANCEL" CommandName="Cancel" />
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
