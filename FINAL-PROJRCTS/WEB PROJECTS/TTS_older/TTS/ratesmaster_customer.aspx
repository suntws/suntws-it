<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="ratesmaster_customer.aspx.cs" Inherits="TTS.ratesmaster_customer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/datemonthyear.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .form-control
        {
            width: 300px;
            height: 22px;
            font-size: 14px;
            color: #000;
            background-color: #fff;
            border: 1px solid #000;
            border-radius: 4px;
            font-weight: bold;
        }
        .form-control:hover, .form-control:focus
        {
            background-color: #555;
            color: #fff;
        }
        .btn
        {
            text-decoration: none;
            padding: 4px 10px;
            font-size: 14px;
            font-weight: bold;
            text-align: center;
            vertical-align: middle;
            cursor: pointer;
            border: 1px solid transparent;
            border-radius: 4px;
        }
        .btn-success
        {
            color: #fff;
            background-color: #5cb85c;
            border-color: #4cae4c;
        }
        .btn-success:hover
        {
            color: #fff;
            background-color: #449d44;
            border-color: #398439;
        }
        .btn-warning
        {
            color: #fff;
            background-color: #f0ad4e;
            border-color: #eea236;
        }
        .btn-warning:hover
        {
            color: #fff;
            background-color: #ec971f;
            border-color: #d58512;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: center" class="pageTitleHead">
        <asp:Label runat="server" ID="lblPageHead" ClientIDMode="Static" Text="PRICE SHEET PREPARE"></asp:Label>
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div class="contPage">
        <asp:ScriptManager runat="server" ID="scriptManager1">
        </asp:ScriptManager>
        <table cellspacing="0" rules="all" border="1" style="border: solid 1px #525252; border-collapse: collapse;
            width: 100%;">
            <tr>
                <td colspan="2" style="vertical-align: top;">
                    <table cellspacing="0" rules="all" border="1" style="border: solid 1px #525252; border-collapse: collapse;
                        width: 100%; line-height: 23px;">
                        <tr>
                            <td>
                                CUSTOMER
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlCustomer" AutoPostBack="true" OnSelectedIndexChanged="ddlCustomer_IndexChanged"
                                    CssClass="form-control" Width="400px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                CATEGORY
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlCategory" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_IndexChanged"
                                    CssClass="form-control" Width="400px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                SUB-CATEGORY
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlSubCategory" CssClass="form-control" Width="400px">
                                    <asp:ListItem Text="CHOOSE" Value="CHOOSE" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="TYRE ONLY" Value="TYRE ONLY"></asp:ListItem>
                                    <asp:ListItem Text="ASSEMBLY" Value="ASSEMBLY"></asp:ListItem>
                                    <asp:ListItem Text="RIM ONLY" Value="RIM ONLY"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                PLATFORM
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlPlatform" AutoPostBack="true" OnSelectedIndexChanged="ddlPlatform_IndexChanged"
                                    CssClass="form-control" Width="400px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                BASE TYPE
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlBaseType" AutoPostBack="true" OnSelectedIndexChanged="ddlBaseType_IndexChanged"
                                    CssClass="form-control" Width="200px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                BASE RMCB
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtBaseRmcb" ClientIDMode="Static" Text="0" CssClass="form-control"
                                    MaxLength="7" onkeypress="return isNumberKey(event)" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                RATES-ID
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlRatesID" AutoPostBack="true" OnSelectedIndexChanged="ddlRatesID_IndexChanged"
                                    CssClass="form-control" Width="400px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                PRICE SHEET REF NO.
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlPriceSheetNo" AutoPostBack="true" OnSelectedIndexChanged="ddlPriceSheetNo_IndexChanged"
                                    CssClass="form-control" Width="400px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                NEW PRICE SHEET REF NO.
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtNewPriceSheetNo" ClientIDMode="Static" Text=""
                                    CssClass="form-control" MaxLength="50" Width="400px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblBaseCost" ClientIDMode="Static" Text="TYPE COST"></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblBaseCostVal" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td colspan="2" style="vertical-align: top;">
                    <table cellspacing="0" rules="all" border="1" style="border: solid 1px #525252; border-collapse: collapse;
                        width: 100%; line-height: 21px;">
                        <tr>
                            <td style="font-weight: bold;">
                                1 &nbsp;
                                <asp:Label runat="server" ID="lblCurrency" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>&nbsp;=
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtConvCur" ClientIDMode="Static" Text="" CssClass="form-control"
                                    MaxLength="6" onkeypress="return isNumberKey(event)" Width="100px" onblur="calcFreightKg();"></asp:TextBox>&nbsp;<asp:Label
                                        runat="server" ID="lblRatesID_Cur" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                INCOTERM
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblIncoterm" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                DESTINATION PORT
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtDestinationPort" ClientIDMode="Static" Text=""
                                    MaxLength="50" CssClass="form-control" Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                CONTAINER TYPE
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlFreightType" CssClass="form-control" Width="200px">
                                    <asp:ListItem Text="CHOOSE" Value="CHOOSE" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="20'" Value="20'"></asp:ListItem>
                                    <asp:ListItem Text="40'" Value="40'"></asp:ListItem>
                                    <asp:ListItem Text="LCL" Value="LCL"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                CONTAINER WT IN KGS
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtFreightWt" ClientIDMode="Static" Text="0" CssClass="form-control"
                                    MaxLength="10" onkeypress="return isNumberKey(event)" onblur="calcFreightKg();"
                                    Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table cellspacing="0" rules="all" border="1" style="border: solid 1px #525252; border-collapse: collapse;
                                    width: 100%; line-height: 20px;">
                                    <tr>
                                        <td rowspan="2" style="vertical-align: middle; width: 144px;">
                                            COST
                                        </td>
                                        <td>
                                            ORIGIN CLEARANCE
                                        </td>
                                        <td>
                                            OCEAN FREIGHT
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtClearanceCost" ClientIDMode="Static" Text="0"
                                                CssClass="form-control" MaxLength="10" onkeypress="return isNumberKey(event)"
                                                onblur="calcFreightKg();" Width="100px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtFreightCost" ClientIDMode="Static" Text="0" CssClass="form-control"
                                                MaxLength="10" onkeypress="return isNumberKey(event)" onblur="calcFreightKg();"
                                                Width="100px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            FREIGHT INRS/KG
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="lblClearanceInrKg" ClientIDMode="Static" Text="0" Font-Bold="true"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="lblFreightInrKg" ClientIDMode="Static" Text="0" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                APPLICABLE PERIOD
                            </td>
                            <td>
                                FROM:
                                <asp:TextBox runat="server" ID="txtFromDate" ClientIDMode="Static" Text="" CssClass="form-control"
                                    Width="80px"></asp:TextBox>
                                &nbsp;&nbsp;TO:
                                <asp:TextBox runat="server" ID="txtToDate" ClientIDMode="Static" Text="" CssClass="form-control"
                                    Width="80px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="vertical-align: top;">
                    <asp:DataList runat="server" ID="dlPremiumTypeList" RepeatColumns="1" RepeatDirection="Vertical"
                        RepeatLayout="Table">
                        <ItemTemplate>
                            <table cellspacing="0" rules="all" border="1" style="border: solid 1px #525252; border-collapse: collapse;
                                width: 100%;">
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="lblPremiumType" Text='<%# Eval("PremiumType") %>' Width="100px"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtPremiumVal" Text='<%# Eval("PremiumValue") %>'
                                            CssClass="form-control" Width="50px" onkeypress="return isNumberAndMinusKey(event)"
                                            MaxLength="7"></asp:TextBox><b style="font-size: 15px;">%</b>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:DataList><br />
                    <asp:Button runat="server" ID="btnCalculate" ClientIDMode="Static" Text="CALCULATE THE PRICE"
                        CssClass="btn btn-success" OnClientClick="javascript:return CtrlMasterPriceList();"
                        OnClick="btnCalculate_Click" />
                </td>
                <td colspan="3" style="vertical-align: top;">
                    <asp:GridView runat="server" ID="gvTypeBrand" AutoGenerateColumns="false" Width="100%"
                        RowStyle-Height="28px">
                        <Columns>
                            <asp:BoundField DataField="Config" HeaderText="PLATFORM" />
                            <asp:BoundField DataField="Tyretype" HeaderText="TYPE" />
                            <asp:BoundField DataField="brand" HeaderText="BRAND" />
                            <asp:BoundField DataField="Sidewall" HeaderText="SIDEWALL" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chk_SelectRows" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="4" style="text-align: center;">
                    <table cellspacing="0" rules="all" border="1" style="border: solid 1px #525252; border-collapse: collapse;
                        width: 100%; display: none;" runat="server" id="trBufferCtrl" clientidmode="Static">
                        <tr>
                            <td>
                                GENERAL INCREASE OF PRICE %
                                <asp:TextBox runat="server" ID="txtIncrementVal" Text='<%# Eval("PremiumVal") %>'
                                    CssClass="form-control" Width="70px" onkeypress="return isNumberAndMinusKey(event)"
                                    MaxLength="3"></asp:TextBox>
                            </td>
                            <td>
                                CALCULATE TO
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnIncreaseStdPrice" ClientIDMode="Static" Text="STD PRICE"
                                    OnClientClick="javascript:return CtrlMasterPriceList();" OnClick="btnIncreasePrice_Click"
                                    CssClass="btn btn-success" />
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnIncreaseNewPrice" ClientIDMode="Static" Text="NEW PRICE"
                                    OnClientClick="javascript:return CtrlMasterPriceList();" OnClick="btnIncreasePrice_Click"
                                    CssClass="btn btn-success" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                EDIT SIZE WISE
                            </td>
                            <td colspan="3">
                                <asp:RadioButtonList runat="server" ID="rdbEditSizeWise" RepeatColumns="2" RepeatDirection="Horizontal"
                                    AutoPostBack="true" OnSelectedIndexChanged="rdbEditSizeWise_IndexChanged">
                                    <asp:ListItem Text="UNIT PRICE" Value="UNIT PRICE"></asp:ListItem>
                                    <asp:ListItem Text="RMCB" Value="RMCB"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:RadioButtonList runat="server" ID="rdbPriceType" ClientIDMode="Static" RepeatDirection="Horizontal">
                    </asp:RadioButtonList>
                    <br />
                    <asp:GridView runat="server" ID="gvPriceMatrix" AutoGenerateColumns="true" Width="100%"
                        RowStyle-Height="25px">
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button runat="server" ID="btnXlsDownload" ClientIDMode="Static" Text="DOWNLOAD TO EXCEL"
                        OnClientClick="javascript:return CtrlMasterPriceList();" OnClick="btnXlsDownload_Click"
                        CssClass="btn btn-success" />
                </td>
                <td>
                    <asp:Button runat="server" ID="btnSave_PriceMaster" ClientIDMode="Static" Text="SAVE PRICE MASTER VALUES"
                        OnClientClick="javascript:return CtrlMasterPriceList();" OnClick="btnSave_PriceMaster_Click"
                        CssClass="btn btn-success" />
                </td>
                <td>
                    <asp:Button runat="server" ID="btnCalcSizeWiseEdit" ClientIDMode="Static" Text="CALCULATE SIZE WISE EDIT PRICE"
                        OnClientClick="javascript:return CtrlMasterPriceList();" OnClick="btnCalcSizeWiseEdit_Click"
                        CssClass="btn btn-success" />
                </td>
                <td>
                    <asp:Button runat="server" ID="btnClear" ClientIDMode="Static" Text="CLEAR" OnClick="btnClear_Click"
                        CssClass="btn btn-warning" />
                </td>
            </tr>
            <tr>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnGvColCount" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnEditVal" ClientIDMode="Static" Value="" />
    <script src="Scripts/datemonthyear.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            Bind_DatePicker($('#txtFromDate'), $('#txtToDate'));

            $('#txtBaseRmcb').blur(function () {
                if ($(this).val().length > 0 && parseFloat($(this).val()) > 0)
                    $("#MainContent_gvTypeBrand td:contains('" + $('#MainContent_ddlBaseType option:selected').text() + "')").closest('tr').find(':input').attr('checked', true)
                else
                    $("#MainContent_gvTypeBrand td:contains('" + $('#MainContent_ddlBaseType option:selected').text() + "')").closest('tr').find(':input').attr('checked', false)
                $("#rdbPriceType").remove();
                $('#MainContent_gvPriceMatrix tr').remove();
                $('#btnSave_PriceMaster').css({ 'display': 'none' });
            });

            $("input:text[id*=MainContent_dlPremiumTypeList_txtPremiumVal_]").blur(function (e) {
                var _preType = $('#' + ($(this).attr('id').replace('_txtPremiumVal_', '_lblPremiumType_'))).html();
                if ($(this).val().length > 0)
                    $("#MainContent_gvTypeBrand td:contains('" + _preType + "')").closest('tr').find(':input').attr('checked', true)
                else
                    $("#MainContent_gvTypeBrand td:contains('" + _preType + "')").closest('tr').find(':input').attr('checked', false)
                $("#rdbPriceType").remove();
                $('#MainContent_gvPriceMatrix tr').remove();
                $('#btnSave_PriceMaster').css({ 'display': 'none' });
            });

            $("[id*=gvTypeBrand_chk_SelectRows_]").click(function () {
                var _appType = $(this).closest('tr').find('td').eq(1).html();
                if ($(this).attr('checked') == "checked") {
                    if ($('#MainContent_ddlBaseType option:selected').text() == _appType) {
                        if ($('#txtBaseRmcb').val().length == 0 || parseFloat($('#txtBaseRmcb').val()) <= 0) {
                            alert('Enter base RMCB value to ' + _appType);
                            $(this).attr('checked', false);
                        }
                    }
                    else if ($("#MainContent_dlPremiumTypeList td:contains('" + _appType + "')").closest('tr').find(':input').val().length == 0) {
                        alert('Enter premium value to ' + _appType);
                        $(this).attr('checked', false);
                    }
                }
                else {
                    if ($('#MainContent_ddlBaseType option:selected').text() == _appType) {
                        alert('cannot remove the base type');
                        $(this).attr('checked', true);
                    }
                    else if ($("#MainContent_dlPremiumTypeList td:contains('" + _appType + "')").closest('tr').find(':input').val().length > 0) {
                        alert('Premium value cleared to ' + _appType);
                        $("#MainContent_dlPremiumTypeList td:contains('" + _appType + "')").closest('tr').find(':input').val('');
                    }
                }
                $("#rdbPriceType").remove();
                $('#MainContent_gvPriceMatrix tr').remove();
                $('#btnSave_PriceMaster').css({ 'display': 'none' });
            });

            $('#MainContent_gvPriceMatrix,#MainContent_gvTypeBrand').find('tr').hover(function () {
                $(this).css({ 'background-color': '#81f99b' });
            }).mouseleave(function () {
                $(this).css({ 'background-color': '#ffffff' });
            });

            $("input:radio[id*=rdbPriceType_]").click(function () {
                var gvCol = 1;
                $("#MainContent_gvPriceMatrix th").each(function (gv) {
                    if ($('#hdnGvColCount').val() < gvCol) {
                        $("#MainContent_gvPriceMatrix tr").each(function () {
                            $(this).find("td").eq(gv).css("display", "none");
                            $(this).find("th").eq(gv).css("display", "none");
                        });
                    }
                    gvCol++;
                });

                var _gvHead = $("#MainContent_gvPriceMatrix th:contains('" + $("input:radio[id*=rdbPriceType_]:checked").val() + "')");
                _gvHead.css("display", "");
                $("#MainContent_gvPriceMatrix tr").each(function () {
                    $(this).find("td").eq(_gvHead.index()).css("display", "");
                    $(this).find("td").eq(_gvHead.index() - 1).css("display", "");
                    if (!$('#lblIncoterm').html().toLowerCase().includes('fob', 0))
                        $(this).find("td").eq(_gvHead.index() - 2).css("display", "");

                    if ($('#lblPageHead').html().toLowerCase().includes('buffer', 0) && !$('#lblIncoterm').html().toLowerCase().includes('fob', 0)) {
                        $(this).find("td").eq(_gvHead.index() - 3).css("display", "");
                        $(this).find("td").eq(_gvHead.index() - 4).css("display", "");
                        $(this).find("td").eq(_gvHead.index() - 5).css("display", "");
                    }
                    else if ($('#lblPageHead').html().toLowerCase().includes('buffer', 0) && $('#lblIncoterm').html().toLowerCase().includes('fob', 0)) {
                        $(this).find("td").eq(_gvHead.index() - 2).css("display", "");
                        $(this).find("td").eq(_gvHead.index() - 3).css("display", "");
                    }
                });
            });
        });

        function Bind_DatePicker($txtFromDate, $txtToDate) {
            $txtFromDate.datepicker({ minDate: "+0D", maxDate: "+30D", onSelect:
             function (selectedDate) {
                 var date2 = $txtFromDate.datepicker('getDate');
                 date2.setDate(date2.getDate() + 365);
                 $txtToDate.datepicker('option', 'maxDate', date2);
                 $txtToDate.datepicker('option', "minDate", selectedDate);
             }
            }).keydown(function (e) { e.keyWhich = 0; e.keyCode = 0; e.preventDefault(); });
            $txtToDate.datepicker({}).keydown(function (e) { e.keyWhich = 0; e.keyCode = 0; e.preventDefault(); });
        }

        function CtrlMasterPriceList() {
            $('#lblErrMsg').html('');
            var errmsg = '';

            if ($('#MainContent_ddlCustomer option:selected').text() == "" || $('#MainContent_ddlCustomer option:selected').text() == "CHOOSE")
                errmsg += "Select Customer<br/>"
            if ($('#MainContent_ddlCategory option:selected').text() == "" || $('#MainContent_ddlCategory option:selected').text() == "CHOOSE")
                errmsg += "Select Category<br/>"
            if ($('#MainContent_ddlSubCategory option:selected').text() == "" || $('#MainContent_ddlSubCategory option:selected').text() == "CHOOSE")
                errmsg += "Select Sub-Category<br/>"
            if ($('#MainContent_ddlPlatform option:selected').text() == "" || $('#MainContent_ddlPlatform option:selected').text() == "CHOOSE")
                errmsg += "Select Platform<br/>"
            if ($('#MainContent_ddlBaseType option:selected').text() == "" || $('#MainContent_ddlBaseType option:selected').text() == "CHOOSE")
                errmsg += "Select Base Type<br/>"
            if ($('#txtBaseRmcb').val().length == 0 || parseFloat($('#txtBaseRmcb').val()) == 0)
                errmsg += "Enter Base RMCB<br/>";
            if ($('#MainContent_ddlRatesID option:selected').text() == "" || $('#MainContent_ddlRatesID option:selected').text() == "CHOOSE")
                errmsg += "Select Rates ID<br/>";
            if ($('#txtNewPriceSheetNo').val().length == 0)
                errmsg += "Enter Price Sheet Ref No.<br/>";
            if ($('#txtConvCur').val().length == 0)
                errmsg += "Enter Currency Conversion Value<br/>";
            if ($('#txtDestinationPort').val().length == 0)
                errmsg += "Enter Destination Port<br/>";
            if ($('#MainContent_ddlFreightType option:selected').text() == "" || $('#MainContent_ddlFreightType option:selected').text() == "CHOOSE")
                errmsg += "Select Freight Type<br/>";
            if ($('#txtFreightWt').val().length == 0 || parseFloat($('#txtFreightWt').val()) <= 0)
                errmsg += "Enter Freight Wt.<br/>";
            if ($('#txtClearanceCost').val().length == 0 || parseFloat($('#txtClearanceCost').val()) <= 0)
                errmsg += "Enter clearance cost<br/>";
            if (!$('#lblIncoterm').html().toLowerCase().includes('fob', 0)) {
                if ($('#txtFreightCost').val().length == 0 || parseFloat($('#txtFreightCost').val()) <= 0)
                    errmsg += "Enter Freight Cost<br/>";
            }
            if ($('#txtFromDate').val().length == 0 || $('#txtToDate').val().length == 0)
                errmsg += "Enter sheet valid period<br/>";

            if ($('#MainContent_dlPremiumTypeList').find('input:text').length > 0) {
                var boolPremium = false;
                $('#MainContent_dlPremiumTypeList').find('input:text').each(function () {
                    if ($(this).val() != "")
                        boolPremium = true;
                });
                if (!boolPremium)
                    errmsg += "Enter anyone premium value<br/>";
            }

            if ($("[id*=MainContent_gvTypeBrand_chk_SelectRows_]").length > 0 && $("[id*=MainContent_gvTypeBrand_chk_SelectRows_]:checked").length == 0)
                errmsg += "Choose anyone approved list<br/>";

            if ($('#trBufferCtrl').css('display') == 'block') {
                if ($('#txtNewPriceSheetNo').val() == $('#MainContent_ddlPriceSheetNo option:selected').text())
                    errmsg += 'Enter new price sheet ref no.<br/>';
            }

            $('#hdnEditVal').val('');
            if ($("input:radio[id*=MainContent_rdbEditSizeWise_]:checked").length > 0) {
                jsonObj2 = [];
                $('#MainContent_gvPriceMatrix').find('input:text').each(function () {
                    item = {}
                    item["TyreSize"] = ($(this).closest('tr').find('td').eq(0).html());
                    item["RimSize"] = ($(this).closest('tr').find('td').eq(1).html());
                    item["Fwt"] = ($(this).closest('tr').find('td').eq(2).html());
                    item["PriceVal"] = $(this).val();
                    jsonObj2.push(item);
                });
                jsonString = JSON.stringify(jsonObj2);
                $('#hdnEditVal').val(jsonString);
            }

            if (errmsg.length > 0) {
                $('#lblErrMsg').html(errmsg);
                gotoPreviewDiv('lblErrMsg');
                return false;
            }
            return true;
        }

        function calcFreightKg() {
            $('#lblFreightInrKg').html('0'); $('#lblClearanceInrKg').html('0');
            if (parseFloat($('#txtFreightCost').val()) > 0 && parseFloat($('#txtFreightWt').val()) > 0 && parseFloat($('#txtConvCur').val()) > 0) {
                var freightInrKg = ((parseFloat($('#txtFreightCost').val()) * parseFloat($('#txtConvCur').val())) / parseFloat($('#txtFreightWt').val()));
                $('#lblFreightInrKg').html(freightInrKg.toFixed(2));
            }
            if (parseFloat($('#txtClearanceCost').val()) > 0 && parseFloat($('#txtFreightWt').val()) > 0 && parseFloat($('#txtConvCur').val()) > 0) {
                var freightInrKg = ((parseFloat($('#txtClearanceCost').val()) * parseFloat($('#txtConvCur').val())) / parseFloat($('#txtFreightWt').val()));
                $('#lblClearanceInrKg').html(freightInrKg.toFixed(2));
            }
        }

        function BuildGridViewHeader() {
            var gvCol = 1;
            $('#MainContent_gvPriceMatrix tr>th').each(function (gv) {
                $(this).html($(this).html().replace('|', ' ').replace('|', ' ').replace('|', ' '));
                if ($('#hdnGvColCount').val() < gvCol) {
                    $("#MainContent_gvPriceMatrix tr").each(function () {
                        $(this).find("td").eq(gv).css("display", "none");
                        $(this).find("th").eq(gv).css("display", "none");
                    });
                }
                gvCol++;
            });
            //$('#MainContent_gvPriceMatrix th').css({ 'max-width': '120px' });
        }

        function calcEditSizePrice(eve) {
            var _ctrlARV = "", _ctrlIncoterm = "", _ctrlFWT = $(eve).closest('tr').find('td').eq(2).html();
            if ($("input:radio[id*=MainContent_rdbEditSizeWise_]:checked").val() == "UNIT PRICE") {
                _ctrlARV = ((parseFloat($(eve).val()) * parseFloat($('#txtConvCur').val())) / _ctrlFWT);
                var _ctrlRMCB = _ctrlARV - parseFloat($('#lblBaseCostVal').html()) - parseFloat($('#lblClearanceInrKg').html());
                if (!$('#lblIncoterm').html().toLowerCase().includes('fob', 0)) {
                    _ctrlIncoterm = ((_ctrlARV + parseFloat($('#lblFreightInrKg').html())) * _ctrlFWT) /
                    parseFloat($('#txtConvCur').val());
                    $(eve).closest('tr').find('td').eq($(eve).closest('tr').find('td').length - 1).html(_ctrlIncoterm.toFixed(2));
                    $(eve).closest('tr').find('td').eq($(eve).closest('tr').find('td').length - 2).html(_ctrlARV.toFixed(2));
                    $(eve).closest('tr').find('td').eq($(eve).closest('tr').find('td').length - 3).html(_ctrlRMCB.toFixed(2));
                }
                else {
                    $(eve).closest('tr').find('td').eq($(eve).closest('tr').find('td').length - 1).html(_ctrlARV.toFixed(2));
                    $(eve).closest('tr').find('td').eq($(eve).closest('tr').find('td').length - 2).html(_ctrlRMCB.toFixed(2));
                }
            }
            else if ($("input:radio[id*=MainContent_rdbEditSizeWise_]:checked").val() == "RMCB") {
                _ctrlARV = (parseFloat($(eve).val()) + parseFloat($('#lblBaseCostVal').html()) + parseFloat($('#lblClearanceInrKg').html()));
                var _ctrlPRICE = ((_ctrlARV) * _ctrlFWT) / parseFloat($('#txtConvCur').val());
                if (!$('#lblIncoterm').html().toLowerCase().includes('fob', 0)) {
                    _ctrlIncoterm = ((_ctrlARV + parseFloat($('#lblFreightInrKg').html())) * _ctrlFWT) /
                    parseFloat($('#txtConvCur').val());
                    $(eve).closest('tr').find('td').eq($(eve).closest('tr').find('td').length - 1).html(_ctrlIncoterm.toFixed(2));
                    $(eve).closest('tr').find('td').eq($(eve).closest('tr').find('td').length - 2).html(_ctrlARV.toFixed(2));
                    $(eve).closest('tr').find('td').eq($(eve).closest('tr').find('td').length - 3).html(_ctrlPRICE.toFixed(2));
                }
                else {
                    $(eve).closest('tr').find('td').eq($(eve).closest('tr').find('td').length - 1).html(_ctrlARV.toFixed(2));
                    $(eve).closest('tr').find('td').eq($(eve).closest('tr').find('td').length - 2).html(_ctrlPRICE.toFixed(2));
                }
            }
        }

        function gotoPreviewDiv(ctrlID) {
            $("html, body").stop().animate({ scrollLeft: $('#' + ctrlID).offset().left, scrollTop: $('#' + ctrlID).offset().top }, 1200);
        }
    </script>
</asp:Content>
