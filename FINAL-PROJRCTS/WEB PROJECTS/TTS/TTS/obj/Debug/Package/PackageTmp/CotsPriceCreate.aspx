<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CotsPriceCreate.aspx.cs"
    MasterPageFile="~/master.Master" Inherits="TTS.CotsPriceCreate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/GridviewScroll.css" rel="stylesheet" type="text/css" />
    <link href="Styles/datemonthyear.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .SpanCss
        {
            color: Green;
            font-size: 14px;
            font-weight: bold;
            font-style: italic;
            font-family: Times New Roman;
            padding-right: 10px;
        }
        .ddlCss
        {
            font-size: 12px;
            height: 25px;
            width: 200px;
        }
        .tableCss
        {
            border-collapse: collapse;
            border-color: #fff;
            background-color: #ecf6ff;
            width: 1080px;
        }
        .button
        {
            background-color: #4CAF50;
            border: none;
            color: white;
            padding: 5px 25px;
            text-align: center;
            text-decoration: none;
            display: inline-block;
            font-size: 18px;
            margin: 4px 2px;
            cursor: pointer;
            font-weight: bold;
            font-family: Times New Roman;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div align="center" class="pageTitleHead">
        LIST PRICE ENTRY</div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Size="16px"
            ForeColor="Red"></asp:Label>
    </div>
    <div class="contPage">
        <div id="div_gv" style="width: 100%;">
            <table id="tblMain" class="tableCss" cellspacing="0" rules="all" border="1">
                <tr>
                    <td>
                        <span class="SpanCss">Category</span><br />
                        <asp:DropDownList ID="ddl_Category" ClientIDMode="Static" CssClass="ddlCss" AutoPostBack="true"
                            runat="server" Width="120px" OnSelectedIndexChanged="ddl_Category_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <span class="SpanCss">Platform</span><br />
                        <asp:DropDownList ID="ddl_Platform" ClientIDMode="Static" CssClass="ddlCss" AutoPostBack="true"
                            runat="server" Width="120px" OnSelectedIndexChanged="ddl_Platform_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <span class="SpanCss">Brand</span><br />
                        <asp:DropDownList ID="ddl_Brand" ClientIDMode="Static" CssClass="ddlCss" AutoPostBack="true"
                            runat="server" Width="120px" OnSelectedIndexChanged="ddl_Brand_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <span class="SpanCss">Sidewall</span><br />
                        <asp:DropDownList ID="ddl_Sidewall" ClientIDMode="Static" CssClass="ddlCss" AutoPostBack="true"
                            runat="server" Width="120px" OnSelectedIndexChanged="ddl_Sidewall_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td style="text-align: center;">
                        <span id="btnReset" class="button" style="background-color: #f14138;">Reset</span>
                    </td>
                </tr>
                <tr id="tr_typeSelection">
                    <td colspan="4">
                        <span class="SpanCss">Select Tyre Types</span><br />
                        <asp:CheckBoxList ID="chk_TypeSelection" ClientIDMode="Static" runat="server" RepeatColumns="6"
                            RepeatDirection="Horizontal" RepeatLayout="Table" Width="750px">
                        </asp:CheckBoxList>
                    </td>
                    <td style="text-align: center; vertical-align: bottom;">
                        <asp:Button ID="btnView" runat="server" Text="View" CssClass="button" BackColor="#2196f3"
                            OnClick="btnView_Click" OnClientClick="javascript:return ChkEmptyJathagam();" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <span class="SpanCss">Select Price Sheet Ref No:</span><br />
                        <asp:DropDownList ID="ddlPriceSheetSelection" ClientIDMode="Static" CssClass="ddlCss"
                            Width="400px" runat="server" 
                            onselectedindexchanged="ddlPriceSheetSelection_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:TextBox runat="server" ID="txtPriceSheet" ClientIDMode="Static" Width="400px"
                            Height="25px" Text=""></asp:TextBox>
                    </td>
                    <td>
                        <span class="SpanCss">Rated Id:</span><br />
                        <asp:TextBox ID="txtRatesId" runat="server" ClientIDMode="Static" Text="" Width="200px"
                            Height="25px" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        <span class="SpanCss">End Date:</span><br />
                        <asp:TextBox ID="txtEndDate" runat="server" ClientIDMode="Static" Text="" Width="200px"
                            Height="25px" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <div style="text-align: center;">
                            <asp:GridView ID="gvPriceDetails" ClientIDMode="Static" runat="server" AutoGenerateColumns="false"
                                Width="1030px">
                                <Columns>
                                </Columns>
                                <HeaderStyle CssClass="GridviewScrollHeader" />
                                <RowStyle CssClass="GridviewScrollItem" />
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="text-align: center;">
                        <asp:Label ID="lblErrMsg" Text="" ForeColor="Red" ClientIDMode="Static" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:Button ID="btnSavePriceSheet" ClientIDMode="Static" runat="server" Text="Save PriceSheet"
                            CssClass="button" OnClientClick="javascript:return CtrlSave();" 
                            onclick="btnSavePriceSheet_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="div_gif" style="display: none; text-align: center; vertical-align: middle;">
            <img src="images/Arrow_gif.gif" alt="Loading..." />
        </div>
    </div>
    <script src="Scripts/datemonthyear.js" type="text/javascript"></script>
    <script src="Scripts/gridviewScroll1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $('#txtPriceSheet').css({ 'display': 'none' });
            $("#txtEndDate").datepicker({ minDate: "0D", maxDate: "+360D" }).keydown(function (e) { e.keyWhich = 0; e.keyCode = 0; e.preventDefault(); });
            $('#<%=gvPriceDetails.ClientID %>').gridviewScroll({ width: 1065, height: 400, freezesize: 1, headerrowcount: 1 });
            $('#btnReset').click(function () { window.location.href = window.location.href; });
            $('#ddlPriceSheetSelection').change(function () {
                if ($('#ddlPriceSheetSelection option:selected').val() != "ADD NEW SHEET") {
                    GetRatesID_FromPriceSheet();
                    $('#txtPriceSheet').css({ 'display': 'none' });
                    $('#txtRatesId,#txtEndDate').attr('disabled', true);
                }
                else if ($('#ddlPriceSheetSelection option:selected').val() == "ADD NEW SHEET") {
                    $('#txtPriceSheet').css({ 'display': 'block' });
                    $('#txtRatesId,#txtEndDate').attr('disabled', false).val('');
                }
            });
        });
        function GetRatesID_FromPriceSheet() {
            if ($('#ddlPriceSheetSelection option:selected').val() != "" && $('#ddlPriceSheetSelection option:selected').val() != "ADD NEW SHEET") {
                $.ajax({ url: "CotsPriceCreate.aspx/GetRatesId",
                    data: "{PriceSheetRefNo:\"" + $('#ddlPriceSheetSelection option:selected').val() + "\"" + "}",
                    type: "POST", contentType: "application/json; charset=utf-8", dataType: "JSON",
                    success: function (result) {
                        if (result.d.length > 0) {
                            var Jstring = result.d;
                            var arr = Jstring.split('||');
                            $('#txtRatesId').val(arr[0]);
                            $('#txtEndDate').val(arr[1]);
                        }
                    }
                });
            }
        }
        var strEntryCount = 0;
        function SaveRecords() {
            strEntryCount = 0;
            //To Save Master Data
            var custcode = 'DE0048';
            var category = $('#ddl_Category option:selected').val();
            var priceref = $('#ddlPriceSheetSelection option').length == 0 || $('#ddlPriceSheetSelection option:selected').val() == "ADD NEW SHEET" ? $('#txtPriceSheet').val() : $('#ddlPriceSheetSelection option:selected').val();
            var retesref = $('#txtRatesId').val();
            var enddate = $('#txtEndDate').val();
            $.ajax({ url: "CotsPriceCreate.aspx/SaveMasterData",
                data: "{PriceSheetRefNo:\"" + priceref + "\",RatesID:\"" + retesref + "\",EndDate:\"" + enddate + "\",Custcode:\"" + custcode + "\",Category:\"" + category + "\"}",
                type: "POST", contentType: "application/json; charset=utf-8", dataType: "JSON",
                success: function (result) {
                    if (result.d == "Success") {
                        //To Save Item Datas
                        $('#gvPriceDetails tr').each(function (e) {
                            var TyreSize = $(this).find('td:eq(0)').find('span').html();
                            var RimSize = $(this).find('td:eq(1)').find('span').html();
                            var count = $(this).find('td').length;
                            for (var i = 2; i < count; i++) {
                                var Isdisabled = $(this).find('td').eq(i).find('input:text').prop('disabled');
                                if (!Isdisabled && $(this).find('td').eq(i).find('input:text').val() != "") {
                                    strEntryCount++;
                                    var TyreType = $(this).find('td').eq(i).find('input:text').attr('id').substring(4);
                                    var UnitPrice = $(this).find('td').eq(i).find('input:text').val();
                                    var jathagam = category + "_" + $('#ddl_Platform option:selected').val() + "_" + $('#ddl_Brand option:selected').val() + "_" +
                                    $('#ddl_Sidewall option:selected').val() + "_" + TyreType + "_" + TyreSize + "_" + RimSize + "_" + UnitPrice + "_" + custcode;

                                    $.ajax({ url: "CotsPriceCreate.aspx/SaveItemDetails",
                                        data: "{PriceSheetRefNo:\"" + priceref + "\",Jathagam:\"" + jathagam + "\"}",
                                        type: "POST", contentType: "application/json; charset=utf-8", dataType: "JSON",
                                        success: function (result) {
                                            if (result.d == "Success")
                                                $(location).attr("href", "cotspricecreate.aspx");
                                        },
                                        failure: function (result) {
                                            var str = result;
                                        }
                                    });
                                }
                            }
                        });
                    }
                },
                failure: function (result) {
                    var str = result;
                }
            });
            return false;
        }
        function ChkEmptyJathagam() {
            var ErrMsg = "";
            $('#lblErrMsg').html("");
            if ($('#ddl_Category option:selected').val() == "Choose")
                ErrMsg += "Please Choose Category<br/>";
            if ($('#ddl_Platform option:selected').val() == "Choose")
                ErrMsg += "Please Choose Platform<br/>";
            if ($('#ddl_Brand option:selected').val() == "Choose")
                ErrMsg += "Please Choose Brand<br/>";
            if ($('#ddl_Sidewall option:selected').val() == "Choose")
                ErrMsg += "Please Choose Sidewall<br/>";
            if ($('#chk_TypeSelection input:checked').length == 0)
                ErrMsg += "Please Choose TyreType<br/>";
            var priceref = $('#ddlPriceSheetSelection option').length == 0 || $('#ddlPriceSheetSelection option:selected').val() == "ADD NEW SHEET" ? $('#txtPriceSheet').val() : $('#ddlPriceSheetSelection option:selected').val();
            if (priceref == "Choose" || priceref == "" || priceref == "undefined")
                ErrMsg += "Choose/Enter PriceSheet RefNo<br/>";
            if (ErrMsg.length > 0) {
                $('#lblErrMsg').html(ErrMsg);
                return false;
            }
            else
                return true;
        }
        function CtrlSave() {
            var ErrMsg = "";
            $('#lblErrMsg').html("");
            var priceref = $('#ddlPriceSheetSelection option').length == 0 || $('#ddlPriceSheetSelection option:selected').val() == "ADD NEW SHEET" ? $('#txtPriceSheet').val() : $('#ddlPriceSheetSelection option:selected').val();
            if (priceref == "Choose" || priceref == "" || priceref == "undefined")
                ErrMsg += "Choose/Enter PriceSheet RefNo<br/>";
            if ($('#txtRatesId').val() == "")
                ErrMsg += "Enter Rates-ID<br/>";
            if ($('#txtEndDate').val() == "")
                ErrMsg += "Enter End date<br/>";
            if (ErrMsg.length == 0) {
                $('#div_gif').fadeIn();
                $('#div_gif').fadeOut(10000);
                $('#div_gv').fadeIn(10000);
                SaveRecords();
                if (strEntryCount == 0)
                    ErrMsg += "Price value not entered<br/>";
            }
            if (ErrMsg.length > 0) {
                $('#lblErrMsg').html(ErrMsg);
                return false;
            }
            else
                return true;
        }
        function Bind_Exists_EnteredPrice(result) {
            var dtVals = JSON.parse(result)
            $('#gvPriceDetails tr').each(function (e) {
                var TyreSize = $(this).find('td:eq(0)').find('span').html();
                var RimSize = $(this).find('td:eq(1)').find('span').html();
                var count = $(this).find('td').length;
                for (var i = 2; i < count; i++) {
                    var Isdisabled = $(this).find('td').eq(i).find('input:text').prop('disabled');
                    var TyreType = $(this).find('td').eq(i).find('input:text').attr('id').substring(4);
                    if (!Isdisabled && TyreType != '') {
                        $(dtVals).each(function (z) {
                            var dtSizeVal = dtVals[z].TyreSize;
                            var dtRimVal = dtVals[z].TyreRim;
                            var dtTypeVal = dtVals[z].TyreType;
                            if (TyreSize == dtSizeVal && dtRimVal == RimSize && dtTypeVal == TyreType) {
                                $('#gvPriceDetails tr:eq(' + e + ')').find('td').eq(i).find('input:text').val(dtVals[z].Unit_Price);
                                return false;
                            }
                        });
                    }
                }
            });
        }
    </script>
</asp:Content>
