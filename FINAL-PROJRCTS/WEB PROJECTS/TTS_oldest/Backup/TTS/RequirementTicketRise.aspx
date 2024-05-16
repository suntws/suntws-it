<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RequirementTicketRise.aspx.cs"
    MasterPageFile="~/master.Master" Inherits="TTS.RequirementTicketRise" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/datemonthyear.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="displaycontent" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: center;" class="pageTitleHead">
        TICKET RAISE
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="displayContent" class="contPage">
        <table id="Content_Table" align="center">
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblErrorRequest" ClientIDMode="Static" Text="" Font-Size="12px"
                        ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <div align="center" style="font-weight: bold;">
                        <asp:RadioButtonList runat="server" ID="rdbrequestType" RepeatDirection="Horizontal"
                            ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="rdbrequestType_SelectedIndexChanged">
                            <asp:ListItem Value="NEW" Text="NEW REQUEST"></asp:ListItem>
                            <asp:ListItem Value="MODIFY" Text="EXISTING REQUEST"> </asp:ListItem>
                            <asp:ListItem Value="FILE" Text="RECORD REQUEST IN EXCEL FILE"> </asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="display: none; text-align: center" id="tdTicketCategory">
                    <table cellspacing="10" border="0.5" align="center">
                        <tr>
                            <td style="float: inherit;">
                                <asp:Label runat="server" ID="lblpriority" ClientIDMode="Static" ForeColor="Black"
                                    Text="Priority" Font-Size="14px"></asp:Label>
                            </td>
                            <td style="float: inherit;">
                                <asp:DropDownList runat="server" ID="ddl_priority" Style="width: 200px; height: 25px;">
                                    <asp:ListItem Enabled="true" Text="Normal"></asp:ListItem>
                                    <asp:ListItem Text="Immediate"></asp:ListItem>
                                    <asp:ListItem Text="Emergency"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lbltickectSubject" ClientIDMode="Static" ForeColor="Black"
                                    Text="Subject" Font-Size="14px"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtticketsubject" Text="" ClientIDMode="Static" Width="200px"
                                    Height="25px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblmoduleSelection" ClientIDMode="Static" ForeColor="Black"
                                    Text="Module" Font-Size="14px"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddl_module_exis" AutoPostBack="true" ClientIDMode="Static"
                                    Style="width: 200px; height: 25px;" OnSelectedIndexChanged="ddl_module_exis_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div style="text-align: center;" align="center" id="div_submodule">
                                    <asp:Label runat="server" ID="lblmoduleselect" ClientIDMode="Static" ForeColor="Black"
                                        Text="" Font-Size="14px"></asp:Label>
                                    <asp:RadioButtonList runat="server" ID="rdosubmodules" RepeatDirection="Horizontal"
                                        RepeatColumns="2" Width="530px" ClientIDMode="Static" BorderWidth="1px" align="center"
                                        display="inline">
                                    </asp:RadioButtonList>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblExpiredDate" ClientIDMode="Static" ForeColor="Black"
                                    Text="Expected Date" Font-Size="14px"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtExpectedDate" ClientIDMode="Static" Width="200px"
                                    Height="25px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div>
                                    <span style="color: Black; font-size: 14px;">Please Give Details about New Entry</span>
                                    <br />
                                    <asp:TextBox runat="server" ID="txtnewmodulecommands" ClientIDMode="Static" Text=""
                                        TextMode="MultiLine" Width="530px" Height="120px" Enabled="true"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button runat="server" ID="btnticketrise" ClientIDMode="Static" Text="RAISE TICKET"
                                    BackColor="DeepSkyBlue" Height="30px" OnClientClick="javascript:return Ctrlbtnticketrise();"
                                    CssClass="btnsave" OnClick="btnticketrise_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table id="Display_Table" align="center" style="display: none;">
            <tr>
                <td>
                    <asp:GridView ID="gv_TicketDetails" AutoGenerateColumns="false" runat="server" AutoPostBack="false"
                        Width="1072px" HeaderStyle-BackColor="#FFCC00" AlternatingRowStyle-BackColor="#f5f5f5"
                        RowStyle-Height="24px" HeaderStyle-Width="100px">
                        <Columns>
                            <asp:BoundField DataField="Ticket_No" HeaderText="TICKET NO" ItemStyle-Width="70px">
                            </asp:BoundField>
                            <asp:BoundField DataField="Ticket_Raised_Date" HeaderText="RAISED DATE" ItemStyle-Width="70px">
                            </asp:BoundField>
                            <asp:BoundField DataField="Priority" HeaderText="PRIORITY" ItemStyle-Width="70px">
                            </asp:BoundField>
                            <asp:BoundField DataField="Subject" HeaderText="SUBJECT" ItemStyle-Width="70px">
                            </asp:BoundField>
                            <asp:BoundField DataField="Expected_Date" HeaderText="EXPECTED DATE" ItemStyle-Width="70px">
                            </asp:BoundField>
                            <asp:BoundField DataField="Module_Name" HeaderText="MODULE NAME" ItemStyle-Width="70px">
                            </asp:BoundField>
                            <asp:BoundField DataField="Function_Name" HeaderText="FUNCTION NAME" ItemStyle-Width="70px">
                            </asp:BoundField>
                            <asp:BoundField DataField="Requested_Comments" HeaderText="REQUESTED COMMENTS" ItemStyle-Width="160px">
                            </asp:BoundField>
                            <asp:BoundField DataField="Responsed_Comments" HeaderText="RESPONSED COMMENTS" ItemStyle-Width="160px">
                            </asp:BoundField>
                            <asp:TemplateField ItemStyle-Width="80px" HeaderText="REQUESTED DATA">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkrequestedData_file" runat="server" Text='<%# Eval("Requested_Data") %>'
                                        OnClick="lnkrequestedData_file_Click" ClientIDMode="Static" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Possible_Days" HeaderText="POSSIBLE DAYS" ItemStyle-Width="70px">
                            </asp:BoundField>
                            <asp:BoundField DataField="Starts_From" HeaderText="STARTS FROM" ItemStyle-Width="70px">
                            </asp:BoundField>
                            <asp:BoundField DataField="End_To" HeaderText="END TO" ItemStyle-Width="70px"></asp:BoundField>
                            <asp:BoundField DataField="Ticket_Status" HeaderText="TICKET STATUS" ItemStyle-Width="70px">
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
    <script src="Scripts/datemonthyear.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#txtExpectedDate").datepicker({
                minDate: "+0D", maxDate: "+90D"
            }).keydown(function (e) {
                e.keyWhich = 0; e.keyCode = 0; e.preventDefault();
            });
            $("input:radio[id*=rdbrequestType_]").click(function (e) {
                requestview();
            });
        });
        function requestview() {
            $('#tdTicketCategory').css({ 'display': 'block' });
            if ($('input:radio[id*=rdbrequestType_]:checked').val() == "NEW" || $('input:radio[id*=rdbrequestType_]:checked').val() == "FILE") {
                $('#lblmoduleSelection').css({ 'display': 'none' });
                $('#ddl_module_exis').css({ 'display': 'none' });
                $('#div_submodule').css({ 'display': 'none' });
            }
            else if ($('input:radio[id*=rdbrequestType_]:checked').val() == "MODIFY") {
                $('#lblmoduleSelection').css({ 'display': 'block' });
                $('#ddl_module_exis').css({ 'display': 'block' });
                $('#div_submodule').css({ 'display': 'block' });
                $('#ddl_module_exis').css('margin-left','100px' );
            }
            $('#Display_Table').css({ 'display': 'block' });
        }

        function Ctrlbtnticketrise() {

            var errmsg = "";
            $('#lblErrorRequest').html('');
            if ($('#txtticketsubject').val().trim().length == 0)
                errmsg += "Please Fill Subject<br/>";
            if ($('#txtExpectedDate').val().trim().length == 0)
                errmsg += "Please Fill ExpectedDate<br/>";
            if ($('#txtnewmodulecommands').val().trim().length == 0)
                errmsg += "Please Fill Comments<br/>";
            if ($('input:radio[id*=rdbrequestType_]:checked').val() == "MODIFY") {
                if ($('#ddl_module_exis').val().trim().length == 0)
                    errmsg += "Please Select Module<br/>";
                //                if ($('#rdosubmodules').val().trim().length == 0)
                //                    errmsg += "Please Select Function<br/>";
            }
            if (errmsg.length > 0) {
                $('#lblErrorRequest').html(errmsg);
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
