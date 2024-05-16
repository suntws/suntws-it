<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CotsCustomerHoldRelease.aspx.cs"
    MasterPageFile="~/master.Master" Inherits="TTS.CotsCustomerHoldRelease" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        CUSTOMER CREDIT HOLD / REVOKE
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage" style="padding-top: 3px;">
        <table align="center" cellspacing="2" cellpadding="2" style="display: none;">
            <tr>
                <td>
                    <div align="center">
                        <asp:Label ID="lblsearchNameWise" runat="server" Text="Search Customer Name" Font-Bold="true"
                            Font-Size="Medium" ForeColor="Green"></asp:Label>
                    </div>
                </td>
                <td>
                    <div align="center">
                        <asp:TextBox ID="txtsearchNamewise" runat="server" Width="200px" Height="20px"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div align="center">
                        <asp:Label ID="lblsearchRegionWise" runat="server" Text="Search Customer Region"
                            Font-Bold="true" Font-Size="Medium" ForeColor="Green"></asp:Label>
                    </div>
                </td>
                <td>
                    <div align="center">
                        <asp:RadioButtonList runat="server" ID="RadioButtonList1" RepeatDirection="Horizontal"
                            ClientIDMode="Static" AutoPostBack="true">
                            <asp:ListItem Value="NORTH" Text="NORTH"></asp:ListItem>
                            <asp:ListItem Value="SOUTH" Text="SOUTH"></asp:ListItem>
                            <asp:ListItem Value="EAST" Text="EAST"> </asp:ListItem>
                            <asp:ListItem Value="WEST" Text="WEST"> </asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </td>
            </tr>
        </table>
        <hr />
        <table align="center" width="100%">
            <tr>
                <td id="td_MainMenu">
                    <asp:GridView runat="server" ID="gv_customerdetails" ClientIDMode="Static" AlternatingRowStyle-BackColor="#f5f5f5"
                        Width="1070px" AutoGenerateColumns="False" HeaderStyle-BackColor="#a9f9d4" HeaderStyle-Font-Bold="true">
                        <Columns>
                            <asp:BoundField DataField="custfullname" HeaderText="CUSTOMER NAME" />
                            <asp:BoundField DataField="CustCategory" HeaderText="CATEGORY" />
                            <asp:BoundField DataField="region" HeaderText="REGION" />
                            <asp:BoundField DataField="city" HeaderText="CITY" />
                            <asp:BoundField DataField="Lead" HeaderText="LEAD" />
                            <asp:BoundField DataField="Supervisor" HeaderText="SUPERVISOR" />
                            <asp:BoundField DataField="Manager" HeaderText="MANAGER" />
                            <asp:TemplateField HeaderText="ACTION">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hdnStatusCustCode" Value='<%# Eval("custcode") %>' />
                                    <asp:LinkButton ID="lnkaction" runat="server" Text="View" ClientIDMode="Static" OnClick="lnkaction_Click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td id="td_SubMenu" style="display: none;">
                    <table width="100%" align="center">
                        <tr>
                            <td>
                                <div id="divheading" style="width: 1070px; float: left; background-color: #8fdae6;
                                    text-align: center;">
                                    <asp:Label runat="server" ID="lblCustomerName" ClientIDMode="Static" Text="" Font-Size="Medium"
                                        Font-Bold="true"></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div>
                                    <asp:Label ID="lblpurposemessage" runat="server" ClientIDMode="Static" Text="Purpose Of Hold"
                                        Font-Bold="true" ForeColor="Green"></asp:Label>
                                    <asp:TextBox runat="server" ID="txtpurposemessage" ClientIDMode="Static" Text=""
                                        TextMode="MultiLine" Width="1064px" Height="60px" onKeyUp="javascript:CheckMaxLength(this, 499);"
                                        onChange="javascript:CheckMaxLength(this, 499);"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center;">
                                <asp:Button runat="server" ID="btnStatusChange" ClientIDMode="Static" Text="HOLD"
                                    BackColor="DeepSkyBlue" Width="120px" Height="30px" CssClass="btnsave" OnClientClick="javascript:return btnsavecheck()"
                                    OnClick="btnStatusChange_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="static" Text="" Font-Bold="true"
                                    ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <asp:HiddenField runat="server" ID="hdncustcode" ClientIDMode="Static" Value="" />
    </div>
    <script type="text/javascript">
        function openSubMenu() {
            $('#td_SubMenu').css({ 'display': 'block' });
        }
        function gotoPreviewDiv(ctrlID) {
            $('#' + ctrlID).css({ 'display': 'block' });
            $("html, body").stop().animate({
                scrollLeft: $('#' + ctrlID).offset().left,
                scrollTop: $('#' + ctrlID).offset().top
            }, 1200);
        }
        function btnsavecheck() {
            var errmsg = "";
            $('#lblErrMsg').html('');
            if ($('#txtpurposemessage').val().trim().length == 0)
                errmsg += "Please Fill " + $('#lblpurposemessage').html() + "<br/>";
            if (errmsg.length > 0) {
                $('#lblErrMsg').html(errmsg);
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
