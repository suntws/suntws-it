<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RequirementTicketResponse.aspx.cs"
    MasterPageFile="~/master.Master" Inherits="TTS.RequirementTicketResponse" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/datemonthyear.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="displaycontent" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: center;" class="pageTitleHead">
        TICKET RESPONSE
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="12px" ForeColor="Red"></asp:Label>
    </div>
    <div>
        <div>
            <table align="center">
                <tr>
                    <td id="rdb_selection">
                        <div align="center" style="font-weight: bold;">
                            <asp:RadioButtonList runat="server" ID="rdbResponseType" RepeatDirection="Horizontal"
                                ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="rdbResponseType_SelectedIndexChanged">
                                <asp:ListItem Value="ALL" Text="ALL REQUEST" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="NEW" Text="NEW REQUEST"></asp:ListItem>
                                <asp:ListItem Value="MODIFY" Text="EXISTING REQUEST"> </asp:ListItem>
                                <asp:ListItem Value="FILE" Text="FILE REQUEST"> </asp:ListItem>
                                <asp:ListItem Value="STATUS" Text="STATUS CHANGE"> </asp:ListItem>
                                <asp:ListItem Value="VIEW" Text="VIEW DETAILS"> </asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <hr />
    </div>
    <div>
        <div id="div_MainTable">
            <!--MainView-->
            <table>
                <tr>
                    <td>
                        <asp:GridView runat="server" ID="gvTicketDetails" AutoGenerateColumns="false" Width="1072px"
                            HeaderStyle-BackColor="#FFCC00" AlternatingRowStyle-BackColor="#f5f5f5" RowStyle-Height="24px"
                            AutoPostBack="false">
                            <Columns>
                                <asp:BoundField DataField="Ticket_No" HeaderText="TICKET NO" ItemStyle-Width="70px">
                                </asp:BoundField>
                                <asp:BoundField DataField="Ticket_Raised_User" HeaderText="TICKET RAISED USER" ItemStyle-Width="70px">
                                </asp:BoundField>
                                <asp:BoundField DataField="Ticket_Raised_Date" HeaderText="TICKET RAISED DATE" ItemStyle-Width="70px">
                                </asp:BoundField>
                                <asp:BoundField DataField="Updation_Type" HeaderText="UPDATION TYPE" ItemStyle-Width="70px">
                                </asp:BoundField>
                                <asp:BoundField DataField="Priority" HeaderText="PRIORITY" ItemStyle-Width="70px">
                                </asp:BoundField>
                                <asp:BoundField DataField="Subject" HeaderText="SUBJECT" ItemStyle-Width="70px">
                                </asp:BoundField>
                                <asp:BoundField DataField="Expected_Date" HeaderText="EXPECTED DATE" ItemStyle-Width="70px">
                                </asp:BoundField>
                                <asp:BoundField DataField="Requested_Comments" HeaderText="REQUESTED COMMENTS" ItemStyle-Width="160px">
                                </asp:BoundField>
                                <asp:BoundField DataField="Responsed_Comments" HeaderText="RESPONSED COMMENTS" ItemStyle-Width="160px">
                                </asp:BoundField>
                                <asp:TemplateField ItemStyle-Width="80px" HeaderText="REQUESTED DATA">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkrequestedData" runat="server" Text='<%# Eval("Requested_Data") %>' ClientIDMode="Static" OnClick="lnkrequestedData_ViewDetails_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Possible_Days" HeaderText="POSSIBLE DAYS" ItemStyle-Width="70px">
                                </asp:BoundField>
                                <asp:BoundField DataField="Starts_From" HeaderText="STARTS FROM" ItemStyle-Width="70px">
                                </asp:BoundField>
                                <asp:BoundField DataField="End_To" HeaderText="END TO" ItemStyle-Width="70px"></asp:BoundField>
                                <asp:BoundField DataField="Ticket_Status" HeaderText="TICKET STATUS" ItemStyle-Width="70px">
                                </asp:BoundField>
                                <asp:TemplateField ItemStyle-Width="80px" HeaderText="ACTION">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkTciketresponse" runat="server" Text="View" ClientIDMode="Static"
                                            OnClick="lnkTciketresponse_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </div>
        <div id="div_SubTable" style="display:none;">
            <!--SingleView-->
            <table>
                <tr>
                    <td id="singleveiw">
                        <table>
                            <tr>
                                <td>
                                    <div id="divheading" style="width: 1063px; float: left; background-color: Highlight;">
                                        <div style="width: 530px; float: left; text-align: left;">
                                            <asp:Label runat="server" ID="lblTicketNumber" ClientIDMode="Static" Text="" Font-Size="Medium"
                                                Font-Bold="true"></asp:Label>
                                        </div>
                                        <div style="width: 530px; float: left; text-align: right;">
                                            <asp:Label runat="server" ID="lblPriority" ClientIDMode="Static" Text="" Font-Size="Medium"
                                                Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div style="width: 1063px; float: left;">
                                        <asp:GridView runat="server" ID="gv_SingleView" AutoGenerateColumns="false"
                                            Width="1065px" AlternatingRowStyle-BackColor="#f5f5f5">
                                            <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" Height="22px" />
                                            <Columns>
                                                <asp:BoundField DataField="Ticket_Raised_User" HeaderText="TICKET RAISED USER" ItemStyle-Width="70px">
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Ticket_Raised_Date" HeaderText="TICKET RAISED DATE" ItemStyle-Width="70px">
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Updation_Type" HeaderText="UPDATION TYPE" ItemStyle-Width="70px">
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Subject" HeaderText="SUBJECT" ItemStyle-Width="70px">
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Expected_Date" HeaderText="EXPECTED DATE" ItemStyle-Width="70px">
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Ticket_Status" HeaderText="TICKET STATUS" ItemStyle-Width="70px">
                                                </asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table align="center">
                                        <tr>
                                            <td colspan="2">
                                                <div>
                                                    <span style="color: Black; font-size: 14px;">Requested Commands</span>
                                                    <br />
                                                    <asp:TextBox runat="server" ID="txtrequestedcommands" ClientIDMode="Static" Text=""
                                                        TextMode="MultiLine" Width="530px" Height="120px" Enabled="false"></asp:TextBox>
                                                </div>
                                            </td>
                                            <td colspan="2">
                                                <div>
                                                    <span style="color: Black; font-size: 14px;">Response Commands</span>
                                                    <br />
                                                    <asp:TextBox runat="server" ID="txtresponsedcommands" ClientIDMode="Static" Text=""
                                                        TextMode="MultiLine" Width="530px" Height="120px" Enabled="true"></asp:TextBox>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label runat="server" ID="lblpossibledays" ClientIDMode="Static" ForeColor="Black"
                                                    Text="Possible Days" Font-Size="14px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtpossibledays" Text="" ClientIDMode="Static" Width="200px"
                                                    Height="25px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblstartsfrom" ClientIDMode="Static" ForeColor="Black"
                                                    Text="Starts From" Font-Size="14px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtstartsfrom" ClientIDMode="Static" Width="200px"
                                                    Height="25px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label runat="server" ID="lblendto" ClientIDMode="Static" ForeColor="Black" Text="End To"
                                                    Font-Size="14px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtendto" ClientIDMode="Static" Width="200px" Height="25px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <div id="div_lblfileupload" style="display:none;">
                                                    <asp:Label runat="server" ID="lblfileupload" ClientIDMode="Static" ForeColor="Black"
                                                        Text="File Upload" Font-Size="14px"></asp:Label>
                                                </div>
                                            </td>
                                            <td>
                                                <div id="div_upfileupload" style="display:none;">
                                                    <asp:FileUpload runat="server" ID="uprequestedfile" ClientIDMode="Static" Width="200px"
                                                        Height="25px" />
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" align="center">
                                                <asp:Button runat="server" ID="btn_ResonseSend" ClientIDMode="Static" Text="RESPONSE SEND"
                                                    BackColor="DeepSkyBlue" Height="30px" CssClass="btnsave" OnClientClick="javascript:return Ctrlbtnticketresponse()"
                                                    OnClick="btn_ResonseSend_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <script src="Scripts/datemonthyear.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#txtstartsfrom").datepicker({
                minDate: "+0D", maxDate: "+90D"
            }).keydown(function (e) {
                e.keyWhich = 0; e.keyCode = 0; e.preventDefault();
            });
            $("#txtendto").datepicker({
                minDate: "+0D", maxDate: "+90D"
            }).keydown(function (e) {
                e.keyWhich = 0; e.keyCode = 0; e.preventDefault();
            });
            blinkOrderHead();
        });
        function Ctrlbtnticketresponse() {

            var errmsg = "";
            $('#lblErrMsgcontent').html('');
            if ($('#txtresponsedcommands').val().trim().length == 0)
                errmsg += "Please Fill Response Commands<br/>";
            if ($('#txtpossibledays').val().trim().length == 0)
                errmsg += "Please Fill Possible Days<br/>";
            if ($('#txtstartsfrom').val().trim().length == 0)
                errmsg += "Please Fill 'Starts From' Date<br/>";
            if ($('#txtendto').val().trim().length == 0)
                errmsg += "Please Fill 'End To' Date<br/>";
            if (errmsg.length > 0) {
                $('#lblErrMsgcontent').html(errmsg);
                return false;
            }
            else
                return true;
        }
        function openSubTable() {
            $('#div_SubTable').css({'display': 'block'});
        }
        function openMainTable() {
            $('#div_MainTable').css({ 'display': 'block' });
        }
        function openSubTableForStatusChange() {
            $('#div_SubTable').css({ 'display': 'block' });
            $('#txtresponsedcommands').prop('disabled', 'false');
            $('#txtpossibledays').prop('disabled', 'false');
            $('#txtstartsfrom').prop('disabled', 'false');
            $('#txtendto').prop('disabled', 'false');
        }
    </script>
</asp:Content>
