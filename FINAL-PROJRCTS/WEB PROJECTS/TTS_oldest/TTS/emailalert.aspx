<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="emailalert.aspx.cs" Inherits="TTS.emailalert" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/prospectStyle.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div align="center" class="pageTitleHead">
        E-MAIL ALERT SYSTEM</div>
    <div class="contPage">
        <div style="padding-left: 10px; padding-top: 10px;">
            <table>
                <tr>
                    <td>
                        <asp:GridView runat="server" ID="gv_mailalert" AutoGenerateColumns="false" Width="1060px"
                            AlternatingRowStyle-BackColor="#f5f5f5" OnRowEditing="gv_RowEditing" OnRowUpdating="gv_RowUpdating"
                            OnRowCancelingEdit="gv_RowCanceling">
                            <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" Height="22px" />
                            <Columns>
                                <asp:TemplateField HeaderText="FOCUS" ItemStyle-Width="70px" ItemStyle-Height="20px">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblFocus" Text='<%# Eval("focus") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="MAIL TYPE" ItemStyle-Width="150px">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblMailType" Text='<%# Bind_MailType(Eval("focus").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList runat="server" ID="ddlMailType" Width="140px">
                                            <asp:ListItem Text="Day Wise" Value="Day Wise"></asp:ListItem>
                                            <asp:ListItem Text="Month Wise" Value="Month Wise"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:HiddenField runat="server" ID="hdnMailType" Value="Day Wise" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="MAIL COUNT" ItemStyle-Width="150px" ItemStyle-Height="20px">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblMailCount" Text='<%# Bind_DayCount(Eval("focus").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <span id="divDayCount" style="display: block;">
                                            <asp:DropDownList runat="server" ID="ddlMailCount" Width="140px">
                                                <asp:ListItem Text="Choose" Value="Choose"></asp:ListItem>
                                                <asp:ListItem Text="All Days" Value="All Days"></asp:ListItem>
                                                <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                                <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                            </asp:DropDownList>
                                        </span><span id="divMonthCount" style="display: none;">
                                            <asp:DropDownList runat="server" ID="ddlMailMonthCount" Width="140px">
                                                <asp:ListItem Text="Choose" Value="Choose"></asp:ListItem>
                                                <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                            </asp:DropDownList>
                                        </span>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="MAIL DAYS" ItemStyle-Width="400px" ItemStyle-Height="20px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDayList" runat="server" Text='<%# Bind_DayNames(Eval("maildays").ToString(),Eval("focus").ToString())%>'
                                            CssClass="mailDaysCss"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <span id="divDayMail" style="display: block;">
                                            <asp:DropDownList runat="server" ID="ddlMailDays" Width="190px">
                                                <asp:ListItem Text="Choose" Value="Choose"></asp:ListItem>
                                                <asp:ListItem Text="Monday" Value="Monday"></asp:ListItem>
                                                <asp:ListItem Text="Tuesday" Value="Tuesday"></asp:ListItem>
                                                <asp:ListItem Text="Wednesday" Value="Wednesday"></asp:ListItem>
                                                <asp:ListItem Text="Thursday" Value="Thursday"></asp:ListItem>
                                                <asp:ListItem Text="Friday" Value="Friday"></asp:ListItem>
                                                <asp:ListItem Text="Saturday" Value="Saturday"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Label ID="lblDayAddList" runat="server" Text="" CssClass="updatemaildaysCss"></asp:Label>
                                            <asp:HiddenField runat="server" ID="hdnAddDayList" Value="" />
                                        </span><span id="divMonthMail" style="display: none;">
                                            <asp:Label ID="lblMonthAddList" runat="server" Text="" CssClass="updatemaildaysCss"></asp:Label><asp:HiddenField
                                                runat="server" ID="hdnAddMonthList" Value="" />
                                        </span>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ACTION" ItemStyle-Width="200px" ItemStyle-Height="20px"
                                    ControlStyle-Font-Size="13px">
                                    <ItemTemplate>
                                        <asp:Button runat="server" ID="btnEdit" Text="Edit" CommandName="Edit" CssClass="btnedit"
                                            OnClientClick="javascript:return editEmailAlertsBefore(this);" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Button runat="server" ID="btnUpdate" Text="Update" CommandName="Update" CssClass="btnsave"
                                            OnClientClick="javascript:return chkEmailAlerts(this);" />
                                        <asp:Button runat="server" ID="btnCancel" Text="Cancel" CommandName="Cancel" CssClass="btnclear" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="250px" HeaderText="MESSAGE">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblErrMsg" Text="" ForeColor="Red"></asp:Label></ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdnMailCount" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnMailDays" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnMailTypeWise" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        $(document).ready(function () {
            $("select[id*='MainContent_gv_mailalert_ddlMailType_']").change(function () {
                var strmailtype = $(this).find('option:selected').text();
                var hdnMailType = $(this).parent('td').find("[id*='MainContent_gv_mailalert_hdnMailType_']").attr('id');
                if (strmailtype == "Day Wise") {
                    $('#divDayCount').css({ 'display': 'block' }); $('#divMonthCount').css({ 'display': 'none' });
                    $('#divDayMail').css({ 'display': 'block' }); $('#divMonthMail').css({ 'display': 'none' });
                }
                else if (strmailtype == "Month Wise") {
                    $('#divDayCount').css({ 'display': 'none' }); $('#divMonthCount').css({ 'display': 'block' });
                    $('#divDayMail').css({ 'display': 'none' }); $('#divMonthMail').css({ 'display': 'block' });
                }
                $('#' + hdnMailType).val(strmailtype);
            });

            $("select[id*='MainContent_gv_mailalert_ddlMailMonthCount_']").change(function () {
                var strMonthCount = $(this).find('option:selected').text();
                var addMonthlistid = $(this).parent('span').parent('td').next().find("[id*='MainContent_gv_mailalert_lblMonthAddList_']").attr('id');
                var hdnAddMonthList = $(this).parent('span').parent('td').next().find("[id*='MainContent_gv_mailalert_hdnAddMonthList_']").attr('id');
                if (strMonthCount == 'Choose') {
                    alert('Please choose mail count');
                    return false;
                }
                else if (strMonthCount != "Choose") {
                    $('#hdnMailCount').val(strMonthCount);
                    if (strMonthCount == "1")
                        $('#' + addMonthlistid).html('1');
                    else if (strMonthCount == "2")
                        $('#' + addMonthlistid).html('1,15');
                    else if (strMonthCount == "3")
                        $('#' + addMonthlistid).html('1,11,21');
                    $('#' + hdnAddMonthList).val($('#' + addMonthlistid).html());
                }
            });

            $("select[id*='MainContent_gv_mailalert_ddlMailCount_']").change(function () {
                var strdaycount = $(this).find('option:selected').text();
                var adddaylistid = $(this).parent('span').parent('td').next().find("[id*='MainContent_gv_mailalert_lblDayAddList_']").attr('id');
                var hdnAddDayList = $(this).parent('span').parent('td').next().find("[id*='MainContent_gv_mailalert_hdnAddDayList_']").attr('id');
                $(this).parent('span').parent('td').next().find("[id*='MainContent_gv_mailalert_ddlMailDays_']").css({ 'display': 'block' });
                $('#' + adddaylistid).html('');
                if (strdaycount != "All Days" && strdaycount != "Choose") {
                    $('#hdnMailCount').val(strdaycount);
                }
                else if (strdaycount == "All Days") {
                    var ddlDaysID = $(this).parent('span').parent('td').next().find("[id*='MainContent_gv_mailalert_ddlMailDays_'] option");
                    var DaysBuild = '';
                    for (var j = 1; j < ddlDaysID.length; j++) {
                        if (DaysBuild == '')
                            DaysBuild = ddlDaysID.eq(j).html();
                        else
                            DaysBuild += ',' + ddlDaysID.eq(j).html();
                    }
                    if (DaysBuild.length > 0) {
                        $('#' + adddaylistid).html(DaysBuild);
                        $('#' + hdnAddDayList).val(DaysBuild);
                        $(this).parent('span').parent('td').next().find("[id*='MainContent_gv_mailalert_ddlMailDays_']").css({ 'display': 'none' });
                    }
                }
            });

            $("select[id*='MainContent_gv_mailalert_ddlMailDays_']").change(function () {
                var strmailday = $(this).find('option:selected').text();
                var dayDivid = $(this).parent('span').parent('td').find("[id*='MainContent_gv_mailalert_lblDayAddList_']").attr('id');
                var hdnAddDayList = $(this).parent('span').parent('td').find("[id*='MainContent_gv_mailalert_hdnAddDayList_']").attr('id');
                var prevAddDays = $('#' + dayDivid).html();
                var ddlCount = $(this).parent('span').parent('td').prev().find("[id*='MainContent_gv_mailalert_ddlMailCount_']").attr('id');
                if ($('#' + ddlCount).find('option:selected').text() == 'Choose') {
                    alert('Please choose mail count');
                    return false;
                }
                else {
                    if (strmailday != "Choose") {
                        if ($.trim(prevAddDays) != "") {
                            var boolVal = 'false';
                            var splitstr = prevAddDays.split(',');
                            if (splitstr.length > 0) {
                                for (var j = 0; j < splitstr.length; j++) {
                                    if (splitstr[j].toString() == strmailday)
                                        boolVal = 'true';
                                }
                            }
                            if (boolVal == 'false') {
                                $('#' + dayDivid).html(prevAddDays + "," + strmailday);
                                $('#' + hdnAddDayList).val(prevAddDays + "," + strmailday);
                            }
                            else
                                alert('Already this day added');
                        }
                        else if ($.trim(prevAddDays) == "") {
                            $('#' + dayDivid).html(strmailday);
                            $('#' + hdnAddDayList).val(strmailday);
                        }
                        var strDays = $('#' + dayDivid).html();
                        var strDaySplit = strDays.split(',');
                        if (strDaySplit.length == $('#hdnMailCount').val()) {
                            $(this).css({ 'display': 'none' });
                        }
                        $(this)[0].selectedIndex = 0;
                    }
                }
            });
        });

        function editEmailAlertsBefore(e) {
            var mailType = $(e).parent('td').prev().prev().prev().find("[id*='MainContent_gv_mailalert_lblMailType_']").attr('id');
            var mailCount = $(e).parent('td').prev().prev().find("[id*='MainContent_gv_mailalert_lblMailCount_']").attr('id');
            var mailDaylist = $(e).parent('td').prev().find("[id*='MainContent_gv_mailalert_lblDayList_']").attr('id');

            $('#hdnMailTypeWise').val($('#' + mailType).html());
            $('#hdnMailCount').val($('#' + mailCount).html());
            $('#hdnMailDays').val($('#' + mailDaylist).html());
        }

        function editEmailAlertsAfter() {
            var btnupdate = $('#MainContent_gv_mailalert tr').find("[id*='MainContent_gv_mailalert_btnUpdate_']").attr('id');
            var ddlmailType = $('#' + btnupdate).parent('td').prev().prev().prev().find("[id*='MainContent_gv_mailalert_ddlMailType_']").attr('id');
            var hdnmailType = $('#' + btnupdate).parent('td').prev().prev().prev().find("[id*='MainContent_gv_mailalert_hdnMailType_']").attr('id');
            $('#' + ddlmailType).find("option[value='" + $('#hdnMailTypeWise').val() + "']").attr('selected', true)
            $('#' + hdnmailType).val($('#' + ddlmailType + ' option:selected').text());
            if ($('#hdnMailTypeWise').val() == 'Day Wise') {
                var ddlCount = $('#' + btnupdate).parent('td').prev().prev().find("[id*='MainContent_gv_mailalert_ddlMailCount_']").attr('id');
                var lblAddDaylist = $('#' + btnupdate).parent('td').prev().find("[id*='MainContent_gv_mailalert_lblDayAddList_']").attr('id');

                if (parseInt($('#hdnMailCount').val()) < 6)
                    $('#' + ddlCount).find("option[value='" + $('#hdnMailCount').val() + "']").attr('selected', true)
                else if (parseInt($('#hdnMailCount').val()) == 6)
                    $('#' + ddlCount).find("option[value='All Days']").attr('selected', true)
                $('#' + lblAddDaylist).html($('#hdnMailDays').val());

                $('#divDayCount').css({ 'display': 'block' }); $('#divMonthCount').css({ 'display': 'none' });
                $('#divDayMail').css({ 'display': 'block' }); $('#divMonthMail').css({ 'display': 'none' });
            }
            else if ($('#hdnMailTypeWise').val() == 'Month Wise') {
                var ddlMonthCount = $('#' + btnupdate).parent('td').prev().prev().find("[id*='MainContent_gv_mailalert_ddlMailMonthCount_']").attr('id');
                var lblAddMonthlist = $('#' + btnupdate).parent('td').prev().find("[id*='MainContent_gv_mailalert_lblMonthAddList_']").attr('id');
                if (parseInt($('#hdnMailCount').val()) < 6)
                    $('#' + ddlMonthCount).find("option[value='" + $('#hdnMailCount').val() + "']").attr('selected', true)
                $('#' + lblAddMonthlist).html($('#hdnMailDays').val());

                $('#divDayCount').css({ 'display': 'none' }); $('#divMonthCount').css({ 'display': 'block' });
                $('#divDayMail').css({ 'display': 'none' }); $('#divMonthMail').css({ 'display': 'block' });
            }
        }

        function chkEmailAlerts(e) {
            var ddlmailTypeID = $(e).parent('td').prev().prev().prev().find("[id*='MainContent_gv_mailalert_ddlMailType_']").attr('id');
            var errMsg = '';
            var ddlmailType = $('#' + ddlmailTypeID).find('option:selected').text();
            if (ddlmailType == 'Day Wise') {
                var ddlCount = $(e).parent('td').prev().prev().find("[id*='MainContent_gv_mailalert_ddlMailCount_']").attr('id');
                var lblAddDaylist = $(e).parent('td').prev().find("[id*='MainContent_gv_mailalert_lblDayAddList_']").attr('id');
                if ($('#' + ddlCount).find('option:selected').text() == 'Choose')
                    errMsg += 'Choose mail count, ';
                if ($('#' + lblAddDaylist).html().length == 0)
                    errMsg += 'Choose mail days, ';
                if (errMsg.length == 0) {
                    var daycount = $('#' + ddlCount).find('option:selected').text();
                    var daysplit = $('#' + lblAddDaylist).html().split(',');
                    if (daycount != 'All Days') {
                        if (daycount != daysplit.length)
                            errMsg += 'Mail count not equal to mail days';
                    }
                }
            }
            else if (ddlmailType == 'Month Wise') {
                var ddlMonthCount = $(e).parent('td').prev().prev().find("[id*='MainContent_gv_mailalert_ddlMailMonthCount_']").attr('id');
                var lblAddMonthlist = $(e).parent('td').prev().find("[id*='MainContent_gv_mailalert_lblMonthAddList_']").attr('id');
                if ($('#' + ddlMonthCount).find('option:selected').text() == 'Choose')
                    errMsg += 'Choose mail count, ';
                if ($('#' + lblAddMonthlist).html().length == 0)
                    errMsg += 'Choose mail days, ';
            }
            var lblErrMsg = $(e).parent('td').next().find("[id*='MainContent_gv_mailalert_lblErrMsg_']").attr('id');

            if (errMsg.length > 0) {
                $('#' + lblErrMsg).html(errMsg);
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
