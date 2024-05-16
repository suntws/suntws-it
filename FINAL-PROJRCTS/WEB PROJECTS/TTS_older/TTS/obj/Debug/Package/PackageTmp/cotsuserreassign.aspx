<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="cotsuserreassign.aspx.cs" Inherits="TTS.cotsuserreassign" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .DdlTableCss
        {
            background-color: #088F9B;
            font-weight: bold;
            text-align: center;
            color: #fff;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        ASSOCIATE LEAD RE-ASSIGN</div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table style="border: 1px solid #000; width: 1070px;">
            <tr>
                <td colspan="3">
                    <div id="divLeadValue" style="display: none;">
                        <table style="border: 1px solid #000; width: 1068px; border-collapse: collapse;"
                            cellspacing="0" rules="all" border="1">
                            <tr>
                                <td colspan="3">
                                    <asp:GridView runat="server" ID="gvCotsUserAssign" AutoGenerateColumns="false" HeaderStyle-BackColor="#FFCC00"
                                        Width="1070px" AlternatingRowStyle-BackColor="#f5f5f5" OnPageIndexChanging="gvCotsUserAssign_PageIndex"
                                        AllowPaging="true" PageSize="25" PagerStyle-Height="30px" PagerStyle-Font-Bold="true"
                                        PagerStyle-Font-Size="15px" PagerStyle-HorizontalAlign="Center" PagerStyle-VerticalAlign="Middle">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox runat="server" ID="ChkCotsCustList" onclick='SelectChk(this);' />
                                                    <asp:HiddenField runat="server" ID="hdnCotsCustID" Value='<%# Eval("ID") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="custfullname" HeaderText="CUSTOMER NAME" ItemStyle-Width="" />
                                            <asp:BoundField DataField="CustCategory" HeaderText="CATEGORY" ItemStyle-Width="" />
                                            <asp:BoundField DataField="region" HeaderText="REGION" ItemStyle-Width="" />
                                            <asp:BoundField DataField="city" HeaderText="CITY" ItemStyle-Width="" />
                                            <asp:BoundField DataField="statename" HeaderText="STATE" ItemStyle-Width="" />
                                            <asp:BoundField DataField="Lead" HeaderText="LEAD" ItemStyle-Width="" />
                                            <asp:BoundField DataField="Supervisor" HeaderText="SUPERVISOR" ItemStyle-Width="" />
                                            <asp:BoundField DataField="Manager" HeaderText="MANAGER" ItemStyle-Width="" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td class="DdlTableCss">
                                    LEAD
                                </td>
                                <td class="DdlTableCss">
                                    SUPERVISOR
                                </td>
                                <td class="DdlTableCss">
                                    MANAGER
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlCustLead" ClientIDMode="Static" Width="200px"
                                        CssClass="ddlCss">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlCustSupervisor" ClientIDMode="Static" Width="200px"
                                        CssClass="ddlCss">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlCustManger" ClientIDMode="Static" Width="200px"
                                        CssClass="ddlCss">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divCustType" style="display: none;">
                        <table style="border: 1px solid #000; width: 1068px; border-collapse: collapse;"
                            cellspacing="0" rules="all" border="1">
                            <tr>
                                <td colspan="2">
                                    <asp:GridView runat="server" ID="gvCustTypeAssign" AutoGenerateColumns="false" HeaderStyle-BackColor="#FFCC00"
                                        Width="1070px" AlternatingRowStyle-BackColor="#f5f5f5" OnPageIndexChanging="gvCustTypeAssign_PageIndex"
                                        AllowPaging="true" PageSize="25" PagerStyle-Height="30px" PagerStyle-Font-Bold="true"
                                        PagerStyle-Font-Size="15px" PagerStyle-HorizontalAlign="Center" PagerStyle-VerticalAlign="Middle">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox runat="server" ID="ChkCotsCustList" onclick='SelectChk(this);' />
                                                    <asp:HiddenField runat="server" ID="hdnCotsCustID" Value='<%# Eval("ID") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="custfullname" HeaderText="CUSTOMER NAME" ItemStyle-Width="" />
                                            <asp:BoundField DataField="CustCategory" HeaderText="CATEGORY" ItemStyle-Width="" />
                                            <asp:BoundField DataField="region" HeaderText="REGION" ItemStyle-Width="" />
                                            <asp:BoundField DataField="Lead" HeaderText="LEAD" ItemStyle-Width="" />
                                            <asp:BoundField DataField="Supervisor" HeaderText="SUPERVISOR" ItemStyle-Width="" />
                                            <asp:BoundField DataField="Manager" HeaderText="MANAGER" ItemStyle-Width="" />
                                            <asp:BoundField DataField="CustType" HeaderText="CUSTOMER TYPE" ItemStyle-Width="" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; font-weight: bold;">
                                    CUSTOMER TYPE
                                </td>
                                <td>
                                    <asp:RadioButtonList runat="server" ID="rdbCustType" ClientIDMode="Static" RepeatColumns="4"
                                        RepeatDirection="Horizontal" Width="300px">
                                        <asp:ListItem Text="A" Value="A"></asp:ListItem>
                                        <asp:ListItem Text="B1" Value="B1"></asp:ListItem>
                                        <asp:ListItem Text="B2" Value="B2"></asp:ListItem>
                                        <asp:ListItem Text="C" Value="C"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label>
                </td>
                <td style="text-align: center;">
                    <asp:Button runat="server" ID="btnAssign" ClientIDMode="Static" Text="ASSIGN" CssClass="btnshow"
                        OnClick="btnAssign_Click" OnClientClick="javascript:return CtrlBtnAssign();" />
                </td>
                <td style="text-align: center;">
                    <span class="btnclear" onclick="page_reload();">CLEAR</span>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnQType" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        function SelectChk(chk) {
            var strID = $(chk).attr('id');
            if ($(chk).is(":checked")) { $(chk).attr('checked', true); $('#' + strID).parent("td").parent("tr").css({ "background-color": "#32E247" }) }
            else { $(chk).attr('checked', false); $('#' + strID).parent("td").parent("tr").css({ "background-color": "#ffffff" }) }
        }

        function CtrlBtnAssign() {
            $('#lblErrMsg').html(''); var errMsg = '';
            if ($("input:checkbox[id*=MainContent_gv]:checked").length == 0)
                errMsg += 'Choose customer list<br/>';
            if ($('#hdnQType').val() == 'lead') {
                if ($("#ddlCustLead option:selected").text() == 'Choose' && $("#ddlCustSupervisor option:selected").text() == 'Choose' && $("#ddlCustManger option:selected").text() == 'Choose')
                    errMsg += 'Choose Lead/ Supervisor/ Manager <br/>';
            }
            else if ($('#hdnQType').val() == 'type') {
                if ($('input:radio[id*=rdbCustType_]:checked').length == 0)
                    errMsg += 'Choose customer type<br />';
            }
            if (errMsg.length > 0) {
                $('#lblErrMsg').html(errMsg); return false;
            }
            else
                return true;
        }

        function page_reload() {
            window.location.href = window.location.href;
        }

        function showDiv(ctrlID) {
            $('#' + ctrlID).css({ 'display': 'block' });
        }
    </script>
</asp:Content>
