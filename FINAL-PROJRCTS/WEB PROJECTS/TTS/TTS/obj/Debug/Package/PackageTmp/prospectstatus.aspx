<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="prospectstatus.aspx.cs" Inherits="TTS.prospectstatus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/prospectStyle.css" rel="stylesheet" type="text/css" />
    <link href="Styles/popupBox.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/prospectScript.js" type="text/javascript"></script>
    <link href="Styles/TinyStyle.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/TinyboxScript.js" type="text/javascript"></script>
    <style type="text/css">
        .tableCust
        {
            border-collapse: collapse;
            border-color: #868282;
            width: 1075px;
            line-height: 20px;
            padding-left: 5px;
        }
        .tableCust input[type="text"], textarea, select
        {
            border: 1px solid #000;
        }
        .tableCust th
        {
            background-color: #E4CEA7;
            text-align: center;
            font-weight: bold;
        }
        .ui-widget-content td
        {
            border: 1px solid #C7C7C7;
        }
        .ui-jqgrid-btable tr:nth-child(even)
        {
            word-break: break-word;
            white-space: normal;
            background: #FFFFFF;
        }
        .ui-jqgrid-btable tr:nth-child(odd)
        {
            word-break: break-word;
            white-space: normal;
            background: #FBF3E5;
        }
        .ui-state-hover
        {
            background: #C1851D !important;
            color: #FFFFFF;
            font-weight: normal;
        }
        #myToolTip
        {
            display: none;
            position: absolute;
        }
        .ui-jqgrid-labels th
        {
            text-align: center;
            border: 1px solid #C7C7C7;
            background-color: #E8BC6F;
        }
        .ui-jqgrid-labels th:hover
        {
            border: 1px solid #C7C7C7;
            background-color: #D09530;
        }
        #jqGridPager_left
        {
            border: 1px solid #C7C7C7;
            background-color: #E8BC6F;
            font-weight: normal;
            color: #0A0A0A;
        }
        #jqGridPager_center
        {
            border: 1px solid #C7C7C7;
            background-color: #E8BC6F;
            font-weight: normal;
            color: #0A0A0A;
        }
        #jqGridPager_right
        {
            border: 1px solid #C7C7C7;
            background-color: #E8BC6F;
            font-weight: normal;
            color: #0A0A0A;
        }
        .ui-state-highlight1
        {
            border: 1px solid #C7C7C7;
            background: #F5B408 !important;
            color: #0A0A0A;
        }
    </style>
    <script src="Scripts/jquery-1.11.0.min.js" type="text/javascript"></script>
    <link href="Styles/ui.jqgrid-bootstrap.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div align="center" class="pageTitleHead">
        LEAD STATUS</div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table>
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
                                <asp:DropDownList ID="ddlCustList" runat="server" Width="300px" CssClass="ddlchange"
                                    ClientIDMode="Static">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCountryList" runat="server" Width="150px" ClientIDMode="Static">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCity" runat="server" Width="150px" CssClass="ddlchange"
                                    ClientIDMode="Static">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlFocusList" runat="server" CssClass="ddlchange" ClientIDMode="Static">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlFlagList" runat="server" CssClass="ddlchange" ClientIDMode="Static">
                                    <asp:ListItem Text="ALL" Value="ALL"></asp:ListItem>
                                    <asp:ListItem Text="Off" Value="Off"></asp:ListItem>
                                    <asp:ListItem Text="On" Value="On"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlPort" runat="server" Width="180px" CssClass="ddlchange"
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
                    <asp:Label runat="server" ID="lblRecordCount" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label>
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
                                <td>
                                    <div id="div1" style="color: #ff0000;">
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnCity" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnSupplier" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnLeadfeedback" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdncust" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnProsCustCode" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnpositionid" ClientIDMode="Static" Value="" />
    <script src="Scripts/grid.locale-en.js" type="text/javascript"></script>
    <script src="Scripts/jquery.jqGrid.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('iframe').unload(function () { gridbind(); });
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
                if (e.target.className != "cssTip" && e.target.className != 'supplier' && e.target.className != 'LeadStatus' && e.target.id != "myToolTip1")
                    $('#myToolTip').hide();
            });

            $('#ddlCountryList').change(function () {
                if ($("#ddlCountryList option:selected").text() == 'ALL')
                    $('#ddlCity').html($("#hdnCity").val());
                else {
                    $.ajax({ type: "POST", url: "prospectreassign.aspx/get_City", data: '{Country:"' + $("#ddlCountryList option:selected").text() + '"}', contentType: "application/json; charset=utf-8", dataType: "json",
                        success: OnSuccessCountry,
                        failure: function (response) { alert(response.d); },
                        error: function (response) { alert(response.d); }
                    });
                } GetCustomers($("#ddlCustList option:selected").text(), $("#ddlCountryList option:selected").text(), 'ALL', $("#ddlFlagList option:selected").text(), $("#ddlPort option:selected").text(), $("#ddlFocusList option:selected").text(), $("#ddlSupplier option:selected").text(), $("#ddlLeadSource option:selected").text());
            });
            $('.ddlchange').change(function () { GetCustomers($("#ddlCustList option:selected").text(), $("#ddlCountryList option:selected").text(), $("#ddlCity option:selected").text(), $("#ddlFlagList option:selected").text(), $("#ddlPort option:selected").text(), $("#ddlFocusList option:selected").text(), $("#ddlSupplier option:selected").text(), $("#ddlLeadSource option:selected").text()); });
        });
        function gridbind() { GetCustomers($("#ddlCustList option:selected").text(), $("#ddlCountryList option:selected").text(), $("#ddlCity option:selected").text(), $("#ddlFlagList option:selected").text(), $("#ddlPort option:selected").text(), $("#ddlFocusList option:selected").text(), $("#ddlSupplier option:selected").text(), $("#ddlLeadSource option:selected").text()); }

        function GetCustomers(CustName, country, City, flag, port, focus, Supplier,LeadSource) {
            supplierhistory = ''; var focus1 = encodeURIComponent(focus);
            var country1 = encodeURIComponent(country); var City1 = encodeURIComponent(City);
            supplierhistory = $('#hdnSupplier').val(); LeadStatus = $('#hdnLeadStatus').val();
            if (supplierhistory != '') {
                $.ajax({
                    type: "POST",
                    url: 'BindRecords.aspx?type=GetSatusLead&CustName=' + CustName + '&country=' + country1 + '&City=' + City1 + '&flag=' + flag + '&port=' + port + '&focus=' + focus1 + '&Supplier=' + Supplier + '&LeadSource=' + LeadSource + '',
                    data: '{}', contentType: "application/json; charset=utf-8", dataType: "json",
                    success: function OnSuccess(response) {
                        $('#jqGrid').jqGrid('GridUnload');
                        $('#cb_jqGrid').change(function () { showLead(); });
                        var mydata = response;
                        $('#lblRecordCount').text('');
                        $("#jqGrid").jqGrid({
                            mtype: "GET", datatype: 'local',
                            data: mydata,
                            styleUI: 'Bootstrap',
                            colNames: ['', 'CODE', 'CUSTOMER NAME', 'CITY', 'COUNTRY', 'PORT','LEAD SOURCE', 'FOCUS', 'FLAG', 'SUPPLIER', 'LAST FEEDBACK FROM LEAD'],
                            colModel: [
                    { name: 'select', label: 'select', align: 'center', width: 20, formatter: radiobtn },
                    { name: 'custcode', key: true, width: 50, resizable: false },
                    { name: 'Custname', width: 220, resizable: false },
                    { name: 'City', width: 100, resizable: false },
                    { name: 'Country', width: 90, resizable: false },
                    { name: 'port', width: 80, resizable: false }, { name: 'LeadSource', width: 80, resizable: false },
                    { name: 'focus', width: 50, resizable: false },
                    { name: 'flag', width: 45, resizable: false },
                    { name: 'Supplieraction', sortable: false, formatter: SupplierData, width: 60, align: 'center' },
                    { name: 'leadfeedback', lable: 'leadfeedback', width: 290, resizable: false, formatter: leadFeedBack }

                ],
                            viewrecords: true, height: 'auto', width: 1060, rowNum: 20, rowList: [20, 30, 50, 100], shrinkToFit: true, pager: "#jqGridPager", autorowheight: true,
                            gridComplete: function (e) {
                                ShowToolTip();
                                var rdBtnID = $('#hdncust').val();
                                if (rdBtnID != "1") {
                                    $('#rdb_' + rdBtnID).attr('checked', 'checked');
                                    SelectSingleReview(rdBtnID);
                                }
                            }
                        });
                    },
                    failure: function (response) { alert(response.d); },
                    error: function (response) { $('#jqGrid').jqGrid('GridUnload'); $('#lblRecordCount').text('No Record'); }
                });

            }
        }
        function SelectSingleReview(rdBtnID) {
            $('.ui-widget-content').removeAttr('aria-selected'); $('.ui-widget-content').removeClass("ui-state-highlight1");
            $('#' + rdBtnID).attr('aria-selected', 'true'); $('#' + rdBtnID).addClass("ui-state-highlight1");
            var rowData = $("#jqGrid").getRowData(rdBtnID);
            var focus = encodeURIComponent(rowData['focus']); var flag = rowData['flag']; var querystr = $('#hdncust').val();
            TINY.box.show({ iframe: 'prospectpopup.aspx?custcode=' + rdBtnID + '&qstr=prospectleadstatus&focus=' + focus + '&flag=' + flag + '&querystr=' + querystr, boxid: 'frameless', width: 985, height: 600, fixed: false, maskid: '#57565A', maskopacity: 40, closejs: function () { } })
        }
   
    </script>
</asp:Content>
