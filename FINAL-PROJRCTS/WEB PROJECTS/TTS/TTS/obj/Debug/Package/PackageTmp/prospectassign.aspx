<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="prospectassign.aspx.cs" Inherits="TTS.prospectassign" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/prospectStyle.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/prospectScript.js" type="text/javascript"></script>
    <style type="text/css">
        .tableCust
        {
            border-collapse: collapse;
            border-color: #868282;
            width: 1075px;
            line-height: 15px;
            padding-left: 5px;
        }
        .ui-widget-content td
        {
            border: 1px solid #A9F5E1;
        }
        .tableCust input[type="text"], textarea, select
        {
            border: 1px solid #000;
        }
        .tableCust th
        {
            background-color: #BBE4BB;
            text-align: center;
            width: 130px;
            font-weight: bold;
        }
        .ui-jqgrid-btable tr:nth-child(even)
        {
            word-break: break-word;
            background: #FFFFFF;
        }
        .ui-jqgrid-btable tr:nth-child(odd)
        {
            word-break: break-word;
            background: #E0F8E0;
        }
        
        #myToolTip
        {
            display: none;
            position: absolute;
        }
        .ui-jqgrid-labels th
        {
            text-align: center;
            border: 1px solid #A9F5E1;
            background-color: #BBBB4E;
        }
        .ui-jqgrid-labels th:hover
        {
            border: 1px solid #BBBB4E;
            background-color: #A9F5E1;
        }
        #jqGridPager_left
        {
            border: 1px solid #A9F5E1;
            background-color: #BBBB4E;
            font-weight: normal;
            color: #0A0A0A;
        }
        #jqGridPager_center
        {
            border: 1px solid #A9F5E1;
            background-color: #BBBB4E;
            font-weight: normal;
            color: #0A0A0A;
        }
        #jqGridPager_right
        {
            border: 1px solid #A9F5E1;
            background-color: #BBBB4E;
            font-weight: normal;
            color: #0A0A0A;
        }
        .ui-state-hover
        {
            border: 1px solid #A9F5E1;
            color: black;
            background-color: #D0F5A9 !important;
        }
        .ui-state-highlight1
        {
            border: 1px solid #fcd113;
            background: #A0EF88 !important;
            color: #000;
        }
    </style>
    <link href="Styles/ui.jqgrid-bootstrap.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery-1.11.0.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div align="center" class="pageTitleHead">
        ASSIGN TO</div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label></div>
    <div id="displaycontent" class="contPage">
        <table style="width: 1075px;">
            <tr>
                <td>
                    <table cellspacing="0" rules="all" border="1" class="tableCust">
                        <tr>
                            <th>
                                CUSTOMER NAME
                            </th>
                            <th>
                                COUNTRY
                            </th>
                            <th>
                                CITY
                            </th>
                            <th>
                                FOCUS
                            </th>
                            <th>
                                FLAG
                            </th>
                            <th>
                                PORT
                            </th>
                        </tr>
                        <tr>
                            <td>
                                <asp:DropDownList ID="ddlCustName" runat="server" Width="300px" CssClass="ddlchange"
                                    ClientIDMode="Static">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCountry" runat="server" Width="200px" ClientIDMode="Static">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCity" runat="server" Width="200px" CssClass="ddlchange"
                                    ClientIDMode="Static">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlFocus" runat="server" Width="50px" CssClass="ddlchange"
                                    ClientIDMode="Static">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlFlag" runat="server" CssClass="ddlchange" ClientIDMode="Static">
                                    <asp:ListItem Text="ALL" Value="ALL"></asp:ListItem>
                                    <asp:ListItem Text="Off" Value="Off"></asp:ListItem>
                                    <asp:ListItem Text="On" Value="On"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlPort" runat="server" CssClass="ddlchange" Width="200px"
                                    ClientIDMode="Static">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <th>
                                LEAD SOURCE
                            </th>
                            <td colspan="2">
                                <asp:DropDownList runat="server" ID="ddlLeadSource" CssClass="ddlchange" ClientIDMode="Static"
                                    Width="200px">
                                </asp:DropDownList>
                            </td>
                            <th>
                                SUPPLIER
                            </th>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlSupplier" CssClass="ddlchange" ClientIDMode="Static"
                                    Width="170px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="width: 1075px; float: left;">
                        <table>
                            <tr>
                                <td>
                                    <div id="divcheck">
                                        <table id="jqGrid" style="width: 1075px;">
                                        </table>
                                        <div id="jqGridPager" style="width: 1075px;">
                                        </div>
                                        <div id="myToolTip" class="cssTip" style="background: #ccc;">
                                            <table>
                                                <tr>
                                                    <td colspan='3' style='background-color: Orange; font-weight: bold;'>
                                                        <div class='cssTip' style='text-align: center; width: 280px; float: left;'>
                                                            <div class='cssTip' style="width: 280px; float: left; text-align: left;" id='code'>
                                                            </div>
                                                        </div>
                                                        <div class='divclose' style='width: 15px; background-color: red; text-align: center;
                                                            float: right; cursor: pointer;'>
                                                            X</div>
                                                    </td>
                                                </tr>
                                            </table>
                                            <div id="myToolTip1" class='cssTip'>
                                            </div>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-top: 5px;">
                                    <div style="width: 1075px; float: left; background-color: #ccc; display: none;" id="divassignuser">
                                        <table cellspacing="0" rules="all" border="1" class="tableCust">
                                            <tr>
                                                <td style="float: left; margin-right: 20px;">
                                                    <span style="font-weight: bold; color: #f00;">Note: </span><span>If you choose any one
                                                        customer in the list for assign, please assign to lead then choose next page.</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div style="width: 200px; float: left;">
                                                        <span class="headCss">Assign To :</span>
                                                        <asp:DropDownList runat="server" ID="ddlChannel" ClientIDMode="Static" Width="70px">
                                                            <asp:ListItem Text="CHOOSE" Value="CHOOSE"></asp:ListItem>
                                                            <asp:ListItem Text="DIRECT" Value="DIRECT"></asp:ListItem>
                                                            <asp:ListItem Text="AGENT" Value="AGENT"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div style="width: 270px; float: left;">
                                                        <span class="headCss">Lead :</span>
                                                        <asp:DropDownList runat="server" ID="ddlLeadName" ClientIDMode="Static" Width="180px">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div style="width: 270px; float: left;">
                                                        <span class="headCss">Supervisor :</span>
                                                        <asp:DropDownList runat="server" ID="ddlSupervisor" ClientIDMode="Static" Width="180px">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div style="width: 270px; float: left;">
                                                        <span class="headCss">Manager :</span>
                                                        <asp:DropDownList runat="server" ID="ddlManager" ClientIDMode="Static" Width="180px">
                                                        </asp:DropDownList>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: center; padding-top: 3px;">
                                                    <asp:Button runat="server" ID="btnAssign" Text="ASSIGN" OnClientClick="javascript:return getSelectedRows();"
                                                        CssClass="btnsave" OnClick="btnAssign_Click" />
                                                    <span onclick="ctrlClear();" class="btnclear">Clear</span>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="divErrmsg" style="color: #ff0000;">
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnpositionid" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdncustcode" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnSupplier" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnLeadName" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnSupevisorName" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnManagerName" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnCity" ClientIDMode="Static" Value="" />
    <script src="Scripts/grid.locale-en.js" type="text/javascript"></script>
    <script src="Scripts/jquery.jqGrid.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var supplierhistory; var left;
            $('.divclose').click(function () { $('#myToolTip').hide(); });
            $(window).scroll(function (e) {
                var id = $("#hdnpositionid").val();
                if (id != "") {
                    var top1 = $('#' + id).position().top;
                    $('#myToolTip').css({ "top": top1 + 213, "left": left });
                }
            });
            $('body').click(function (e) {
                if (e.target.className != "cssTip" && e.target.className != 'supplier' && e.target.id != "myToolTip1") $('#myToolTip').hide();
            });

            $('.ddlchange').change(function () { GetCustomers($("#ddlCustName option:selected").text(), $("#ddlCountry option:selected").text(), $("#ddlCity option:selected").text(), $("#ddlFlag option:selected").text(), $("#ddlPort option:selected").text(), $("#ddlFocus option:selected").text(), $("#ddlSupplier option:selected").text(), $("#ddlLeadSource option:selected").text()); });

            $('#ddlCountry').change(function () {
                if ($("#ddlCountry option:selected").text() == 'ALL')
                    $('#ddlCity').html($("#hdnCity").val());
                else {
                    $.ajax({ type: "POST", url: "prospectassign.aspx/get_City", data: '{Country:"' + $("#ddlCountry option:selected").text() + '"}', contentType: "application/json; charset=utf-8", dataType: "json",
                        success: OnSuccessCountry,
                        failure: function (response) { alert(response.d); },
                        error: function (response) { alert(response.d); }
                    });
                } GetCustomers($("#ddlCustName option:selected").text(), $("#ddlCountry option:selected").text(), 'ALL', $("#ddlFlag option:selected").text(), $("#ddlPort option:selected").text(), $("#ddlFocus option:selected").text(), $("#ddlSupplier option:selected").text(), $("#ddlLeadSource option:selected").text());
            });

            $('#ddlChannel').change(function () {
                var strChannel = $("#ddlChannel option:selected").text();
                if (strChannel != 'CHOOSE') { getLeadNameList(strChannel); } else $('#ddlLeadName').html('');
            });
            $('#ddlLeadName').change(function () {
                var strLead = $("#ddlLeadName option:selected").text();
                $('#hdnLeadName').val($("#ddlLeadName option:selected").text());
                var strChannel = $("#ddlChannel option:selected").text();
                if (strLead != 'CHOOSE') { getSupervisorNameList(strChannel, strLead); } else { $('#ddlSupervisor').html(''); $('#ddlManager').html(''); }
            });
            $('#ddlSupervisor').change(function () {
                var strSuperviosr = $("#ddlSupervisor option:selected").text();
                $('#hdnSupevisorName').val($("#ddlSupervisor option:selected").text());
                if (strSuperviosr != 'CHOOSE') { getManagerNameList(strSuperviosr); } else { $('#ddlManager').html(''); }
            });
            $('#ddlManager').change(function () { $('#hdnManagerName').val($("#ddlManager option:selected").text()); });
        });

        function showLead() {
            var chkLength = $("[id*=chk_]:checked").length;
            if (chkLength > 0) {
                $('#divassignuser').css({ 'display': 'block' });
                $('#ddlChannel').find("option[value='CHOOSE']").attr('selected', true)
                $('#ddlLeadName').html('');
            } else $('#divassignuser').css({ 'display': 'none' });
            var chkLength = $("[id*=chk_]").length;
            var chkLength1 = $("[id*=chk_]:checked").length;
            if (chkLength == chkLength1) $("#chkall").attr('checked', true);
            else $("#chkall").attr('checked', false);
        }
        function GetCustomers(CustName, country, City, flag, port, focus, Supplier, LeadSource) {
            supplierhistory = ''; var focus1 = encodeURIComponent(focus);
            var country1 = encodeURIComponent(country);
            var City1 = encodeURIComponent(City);
            supplierhistory = $("#hdnSupplier").val();
            if (supplierhistory != '') {
                $.ajax({
                    type: "POST",
                    url: 'BindRecords.aspx?type=GetCustomerspage&CustName=' + CustName + '&country=' + country1 + '&City=' + City1 + '&flag=' + flag + '&port=' + port + '&focus=' + focus1 + '&Supplier=' + Supplier + '&LeadSource=' + LeadSource + '',
                    data: '{}', contentType: "application/json; charset=utf-8", dataType: "json",
                    success: function OnSuccess(response) {
                        $('#jqGrid').jqGrid('GridUnload');
                        $('#cb_jqGrid').change(function () { showLead(); });
                        var mydata = response;
                        $('#divErrmsg').html('');
                        $("#jqGrid").jqGrid({
                            mtype: "GET", datatype: 'local', data: mydata, styleUI: 'Bootstrap',
                            colNames: ['<input type="checkbox" name="chck" id="chkall" class="cbox" onclick="SelectSingle1(this);" />', 'CODE', 'CUSTOMER NAME', 'CITY', 'COUNTRY', 'PORT', 'LEAD SOURCE', 'FOCUS', 'FLAG', 'SUPPLIER'],
                            colModel: [{ name: 'select', label: 'select', align: 'center', width: 20, formatter: Checkbtn, sortable: false, resizable: false },
                            { name: 'Custcode', key: true, width: 70, resizable: false }, { name: 'Custname', width: 345, resizable: false },
                            { name: 'City', width: 150, resizable: false }, { name: 'Country', width: 150, resizable: false },
                            { name: 'port', width: 100, resizable: false }, { name: 'LeadSource', width: 100, resizable: false }, { name: 'focus', width: 60, resizable: false },
                            { name: 'flag', width: 60, resizable: false }, { name: 'action', index: 'action', sortable: false, formatter: SupplierData, width: 60}],
                            viewrecords: true, Width: 'auto', height: 'auto', width: 1060, rowNum: 20, rowList: [20, 30, 50, 100],
                            pager: "#jqGridPager",
                            gridComplete: function (e) {
                                $('#divcheck').find($('.cbox')).click(function () { showLead(); }); $('.cbox').parent().removeClass('ui-jqgrid-sortable');
                                ShowToolTip(); showLead();
                            }
                        });
                    },
                    failure: function (response) { alert(response.d); },
                    error: function (response) { $('#jqGrid').jqGrid('GridUnload'); $('#divErrmsg').html('No Record'); }
                });
            }
        }
        function SelectSingle1(e) {
            var chkLength = $("[id*=chk_]").length;
            if (chkLength > 0) {
                if ($('#chkall').attr('checked') == "checked") {
                    $("[id*=chk_]").attr('checked', true);
                    $("[id*=chk_]").parent().parent().attr('aria-selected', 'true'); $("[id*=chk_]").parent().parent().addClass("ui-state-highlight1");
                }
                else {
                    $("[id*=chk_]").attr('checked', false);
                    $("[id*=chk_]").parent().parent().removeAttr('aria-selected', 'true'); $("[id*=chk_]").parent().parent().removeClass("ui-state-highlight1");
                }
            }
            showLead();
        }
        function SelectSingleReview(rdBtnID) {
            if ($('#chk_' + rdBtnID).attr('checked') != "checked") {
                $('#' + rdBtnID).removeAttr('aria-selected'); $('#' + rdBtnID).removeClass("ui-state-highlight1");
            } else {
                $('#' + rdBtnID).attr('aria-selected', 'true'); $('#' + rdBtnID).addClass("ui-state-highlight1");
            } showLead();
        }
        function Checkbtn(cellvalue, options, rowObject) {
            var id = options.rowId; var btn = '<input type="checkbox" name="chk" value="' + id + '" id="chk_' + id + '" onclick=SelectSingleReview(this.value);   />'
            return btn;
        }
        function getLeadNameList(strChannelVal) {
            $.ajax({ type: "POST", url: "prospectassign.aspx/get_LeadName", data: '{strChannel:"' + strChannelVal + '"}', contentType: "application/json; charset=utf-8", dataType: "json",
                success: OnSuccessLeadList,
                failure: function (response) { alert(response.d); },
                error: function (response) { alert(response.d); }
            });
        }
        function OnSuccessLeadList(response) {
            var xmlDoc = $.parseXML(response.d); var xml = $(xmlDoc); var PLeadVals = xml.find("T1"); var concatStr = '';
            if (PLeadVals.length > 0) {
                for (var j = 0; j < PLeadVals.length; j++)
                    concatStr += "<option value='" + $(PLeadVals[j]).find("PUserName").text() + "'>" + $(PLeadVals[j]).find("PUserName").text() + "</option>";
            } concatStr = "<option value='CHOOSE'>CHOOSE</option>" + concatStr;
            $('#ddlLeadName').html(concatStr);
        }
        function getSupervisorNameList(strChannelVal, strLeadVal) {
            $.ajax({ type: "POST", url: "prospectassign.aspx/get_SupervisorName", data: '{strChannel:"' + strChannelVal + '",strLead1:"' + strLeadVal + '"}', contentType: "application/json; charset=utf-8", dataType: "json",
                success: OnSuccessSupervisorList,
                failure: function (response) { alert(response.d); },
                error: function (response) { alert(response.d); }
            });
        }
        function OnSuccessSupervisorList(response) {
            var xmlDoc = $.parseXML(response.d); var xml = $(xmlDoc); var PLeadVals = xml.find("T1"); var concatStr = '';
            if (PLeadVals.length > 0) {
                for (var j = 0; j < PLeadVals.length; j++)
                    concatStr += "<option value='" + $(PLeadVals[j]).find("PUserName").text() + "'>" + $(PLeadVals[j]).find("PUserName").text() + "</option>";
            } concatStr = "<option value='CHOOSE'>CHOOSE</option>" + concatStr;
            $('#ddlSupervisor').html(concatStr);
            $('#ddlManager').html(concatStr);
        }
        function getManagerNameList(strSupervisorVal) {
            $.ajax({ type: "POST", url: "prospectassign.aspx/get_ManagerName", data: '{strChannel:"' + $("#ddlChannel option:selected").text() + '",strLead1:"' + $("#ddlLeadName option:selected").text() + '",strLead2:"' + strSupervisorVal + '"}', contentType: "application/json; charset=utf-8", dataType: "json",
                success: OnSuccessManagerList,
                failure: function (response) { alert(response.d); },
                error: function (response) { alert(response.d); }
            });
        }
        function OnSuccessManagerList(response) {
            var xmlDoc = $.parseXML(response.d); var xml = $(xmlDoc); var PLeadVals = xml.find("T1"); var concatStr = '';
            if (PLeadVals.length > 0) {
                for (var j = 0; j < PLeadVals.length; j++)
                    concatStr += "<option value='" + $(PLeadVals[j]).find("PUserName").text() + "'>" + $(PLeadVals[j]).find("PUserName").text() + "</option>";
            } concatStr = "<option value='CHOOSE'>CHOOSE</option>" + concatStr;
            $('#ddlManager').html(concatStr);
        }
        function ctrlClear() {
            var path = window.location.href; var pathname = path.replace('#', '');
            window.location.href = pathname;
        }

        function gridbind() { GetCustomers($("#ddlCustName option:selected").text(), $("#ddlCountry option:selected").text(), $("#ddlCity option:selected").text(), $("#ddlFlag option:selected").text(), $("#ddlPort option:selected").text(), $("#ddlFocus option:selected").text(), $("#ddlSupplier option:selected").text(), $("#ddlLeadSource option:selected").text()); }

        function getSelectedRows() {
            var errmsg = ''; $('#divErrmsg').html('');
            var grid = $("#jqGrid"); var errmsg = '';
            var selectedIDs = $("[id*=chk_]:checked").length;
            var result = ""; var i = 1;
            $('#hdnCity').val('');
            $("[id*=chk_]:checked").each(function (e) {
                var id = $(this).val();
                var rowData = grid.getRowData(id);
                if (selectedIDs === i) result += rowData['Custcode'] + "~" + rowData['focus'];
                else result += rowData['Custcode'] + "~" + rowData['focus'] + ",";
                i++;
            });
            $('#hdncustcode').val(result);
            if (selectedIDs == 0)
                errmsg = "Choose any one customer<br />";
            if ($("#ddlChannel option:selected").text() == "CHOOSE")
                errmsg += 'Please select channel<br />';
            if ($('#ddlLeadName option:selected').text() == "CHOOSE" || $('#ddlLeadName option:selected').text() == "")
                errmsg += 'Please select leader<br />';
            if ($("#ddlChannel option:selected").text() == "DIRECT") {
                if ($("#ddlSupervisor option:selected").text() == "CHOOSE" && $("#ddlManager option:selected").text() == "CHOOSE")
                    errmsg += 'Please choose supervisor or manager<br/>';
            }
            if (errmsg.length > 0) {
                $('#divErrmsg').html(errmsg);
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
