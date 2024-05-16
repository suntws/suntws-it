<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="exp_pricecreate.aspx.cs"
    MasterPageFile="~/master.Master" Inherits="TTS.exp_pricecreate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
    <link href="Styles/GridviewScroll.css" rel="stylesheet" type="text/css" />
    <link href="Styles/datemonthyear.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        th
        {
            line-height: 15px;
            text-align: left;
            font-weight: normal;
        }
        td
        {
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div align="center" class="pageTitleHead">
        EXPORT - LIST PRICE ENTRY</div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Size="16px"
            ForeColor="Red"></asp:Label>
    </div>
    <div class="contPage">
        <table cellspacing="0" rules="all" border="1" style="background-color: #f5ffff; width: 100%;
            border-color: #d6d6d6; border-collapse: separate;">
            <tr>
                <th>
                    CUSTOMER
                </th>
                <td colspan="7">
                    <asp:DropDownList runat="server" ID="ddlCustomer" ClientIDMode="Static" CssClass="form-control"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlCustomer_IndexChange" Width="400px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    CATEGORY
                </th>
                <td>
                    <asp:DropDownList ID="ddl_Category" ClientIDMode="Static" CssClass="form-control"
                        AutoPostBack="true" runat="server" Width="120px" OnSelectedIndexChanged="ddl_Category_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <th>
                    PLATFORM
                </th>
                <td>
                    <asp:DropDownList ID="ddl_Platform" ClientIDMode="Static" CssClass="form-control"
                        AutoPostBack="true" runat="server" Width="120px" OnSelectedIndexChanged="ddl_Platform_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <th>
                    BRAND
                </th>
                <td>
                    <asp:DropDownList ID="ddl_Brand" ClientIDMode="Static" CssClass="form-control" AutoPostBack="true"
                        runat="server" Width="120px" OnSelectedIndexChanged="ddl_Brand_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <th>
                    SIDEWALL
                </th>
                <td>
                    <asp:DropDownList ID="ddl_Sidewall" ClientIDMode="Static" CssClass="form-control"
                        AutoPostBack="true" runat="server" Width="120px" OnSelectedIndexChanged="ddl_Sidewall_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr id="tr_typeSelection">
                <th style="vertical-align: top;">
                    TYRE TYPE
                </th>
                <td colspan="7">
                    <asp:CheckBoxList ID="chk_TypeSelection" ClientIDMode="Static" runat="server" RepeatColumns="6"
                        RepeatDirection="Horizontal" RepeatLayout="Table" Width="750px">
                    </asp:CheckBoxList>
                </td>
            </tr>
            <tr>
                <th>
                    PRICE SHEET REF NO
                </th>
                <td colspan="3">
                <asp:DropDownList ID="ddlPriceSheetSelection" ClientIDMode="Static" CssClass="form-control" AutoPostBack="true"
                            Width="400px" runat="server" 
                        onselectedindexchanged="ddlPriceSheetSelection_SelectedIndexChanged1" >
                            
                        </asp:DropDownList>

                   <%-- <asp:TextBox runat="server" ID="txtPriceSheetRefNo" ClientIDMode="Static" CssClass="form-control"
                        Width="400px" MaxLength="50"></asp:TextBox>--%>
                </td>
                <th>
                    RATES ID
                </th>
                <td>
                    <asp:TextBox ID="txtRatesId" runat="server" ClientIDMode="Static" Text="" Width="200px"
                        Height="25px" CssClass="form-control" MaxLength="50"></asp:TextBox>
                </td>
                <th>
                    END DATE
                </th>
                <td>
                    <asp:TextBox ID="txtEndDate" runat="server" ClientIDMode="Static" Text="" Width="200px"
                        Height="25px" CssClass="form-control"></asp:TextBox>
                </td>
            </tr>
            <tr style="text-align: center;">
                <td colspan="4">
                    <span id="btnReset" class="btn btn-warning" style="background-color: #f14138;">Reset</span>
                </td>
                <td colspan="4">
                    <asp:Button ID="btnView" runat="server" Text="View" CssClass="btn btn-success" BackColor="#2196f3"
                        OnClick="btnView_Click" OnClientClick="javascript:return ChkEmptyJathagam();" />
                </td>
            </tr>
            <tr>
                <td colspan="8">
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
                <td colspan="6" style="text-align: center;">
                    <asp:Label ID="lblErrMsg" Text="" ForeColor="Red" ClientIDMode="Static" runat="server"></asp:Label>
                </td>
                <td colspan="2">
                    <asp:Button ID="btnSavePriceSheet" ClientIDMode="Static" runat="server" Text="Save PriceSheet"
                        CssClass="btn btn-info" OnClientClick="javascript:return CtrlSave();" />
                </td>
            </tr>
        </table>
    </div>
    <script src="Scripts/datemonthyear.js" type="text/javascript"></script>
    <script src="Scripts/gridviewScroll1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#txtEndDate").datepicker({ minDate: "+1D", maxDate: "+120D" }).keydown(function (e) { e.keyWhich = 0; e.keyCode = 0; e.preventDefault(); });
            $('#<%=gvPriceDetails.ClientID %>').gridviewScroll({ width: 1065, height: 400, freezesize: 1, headerrowcount: 1 });
            $('#btnReset').click(function () { window.location.href = window.location.href; });
            $('#<%=gvPriceDetails.ClientID %>').find("input[id*='txt_']").focus(function (e) {
                $(this).css({ 'background-color': '#000000', 'color': '#ffffff' });
            }).blur(function (e) {
                $(this).css({ 'background-color': '#ffffff', 'color': '#000000' });
            }); ;
        });

        var strEntryCount = 0;
        function SaveRecords() {
            //To Save Master Data
            var custcode = $('#ddlCustomer option:selected').val();
            var category = $('#ddl_Category option:selected').val();
            var priceref = $('#txtPriceSheetRefNo').val();
            var retesref = $('#txtRatesId').val();
            var enddate = $('#txtEndDate').val();
            $.ajax({ url: "exp_pricecreate.aspx/SaveMasterData",
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

                                    $.ajax({ url: "exp_pricecreate.aspx/SaveItemDetails", data: "{PriceSheetRefNo:\"" + priceref + "\",Jathagam:\"" + jathagam + "\"}",
                                        type: "POST", contentType: "application/json; charset=utf-8", dataType: "JSON",
                                        success: function (result) {
                                            if (result.d == "Success")
                                                $(location).attr("href", "exp_pricecreate.aspx");
                                        },
                                        failure: function (result) {
                                            var str = result;
                                        }
                                    });
                                }
                            }
                        });
                    }
                    else if (result.d == "Fail")
                        alert('Save process failure');
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
            if ($('#ddl_Category option:selected').val() == "CHOOSE")
                ErrMsg += "Choose Category<br/>";
            if ($('#ddl_Platform option:selected').val() == "CHOOSE")
                ErrMsg += "Choose Platform<br/>";
            if ($('#ddl_Brand option:selected').val() == "CHOOSE")
                ErrMsg += "Choose Brand<br/>";
            if ($('#ddl_Sidewall option:selected').val() == "CHOOSE")
                ErrMsg += "Choose Sidewall<br/>";
            if ($('#chk_TypeSelection input:checked').length == 0)
                ErrMsg += "Choose Tyre Type<br/>";
//            if ($('#txtPriceSheetRefNo').val().length == 0)
//                ErrMsg += "Enter Price Sheet Ref No<br/>";
            if ($('#txtRatesId').val() == "")
                ErrMsg += "Enter Rates-ID<br/>";
            if ($('#txtEndDate').val() == "")
                ErrMsg += "Enter Price Sheet End Date<br/>";
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
            if ($('#txtPriceSheetRefNo').val() == "")
                ErrMsg += "Enter PriceSheet RefNo<br/>";
            if ($('#txtRatesId').val() == "")
                ErrMsg += "Enter Rates-ID<br/>";
            if ($('#txtEndDate').val() == "")
                ErrMsg += "Enter Price Sheet End Date<br/>";
            else {
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
