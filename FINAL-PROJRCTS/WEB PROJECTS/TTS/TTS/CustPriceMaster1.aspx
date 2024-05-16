<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="CustPriceMaster1.aspx.cs" Inherits="TTS.CustPriceMaster1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/datemonthyear.css" rel="stylesheet" type="text/css" />
    <link href="Styles/popupBox.css" rel="stylesheet" type="text/css" />
    <link href="styles/GridviewScroll.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .ddl
        {
            height: 22px;
            width: 360px;
        }
        .rdoAlign label
        {
            vertical-align: 2px;
        }
        #tblUI tr
        {
            padding: 5px 5px 5px 5px;
            height: 30px;
        }
        #tblUI, #tblUI tr, #tblUI tr td
        {
            border: 1px solid black;
        }
        #tblUI
        {
            border-collapse: collapse;
            width: 100%;
        }
        #txtCustPriceSpl
        {
            width: 415px;
            height: 100px;
            overflow-y: auto;
            resize: none;
        }
        
        .highlight
        {
            background-color:#f9fdbf;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div align="center" class="pageTitleHead">
        CUSTOMER PRICE MASTER</div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <div style="padding-left: 10px;">
            <table style="width: 100%">
                <tr>
                    <td style="width: 680px; border-right: 1px solid black;">
                        <table id="tblUI">
                            <tr>
                                <td>
                                    <span class="headCss">Customer Name</span>
                                </td>
                                <td colspan="5">
                                    <asp:DropDownList runat="server" ID="ddlCustName" CssClass="ddl" OnSelectedIndexChanged="ddlCustName_SelectedIndexChanged"
                                        AutoPostBack="true" ClientIDMode="Static">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="border-right: none;">
                                </td>
                                <td colspan="5">
                                    <asp:RadioButton ID="rdoExistingPrice" runat="server" GroupName="radioPriceCategory"
                                        ClientIDMode="Static" Text="Existing Price Ref" AutoPostBack="true" OnCheckedChanged="rdoCategory_CheckedChanged"
                                        Enabled="false" CssClass="rdoAlign" />
                                    <asp:RadioButton ID="rdoNewPrice" runat="server" GroupName="radioPriceCategory" ClientIDMode="Static"
                                        Text="New Price Ref" AutoPostBack="true" OnCheckedChanged="rdoCategory_CheckedChanged"
                                        Enabled="false" CssClass="rdoAlign" Style="padding-left: 50px;" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="headCss">Price Ref No.</span>
                                </td>
                                <td colspan="5">
                                    <asp:DropDownList runat="server" ID="ddlPriceRef" CssClass="ddl" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlPriceRef_SelectedIndexChanged" ClientIDMode="Static">
                                    </asp:DropDownList>
                                    <asp:TextBox runat="server" ID="txtPriceRefNo" ClientIDMode="Static" Width="355px"
                                        Visible="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="headCss">Rates-ID</span>
                                </td>
                                <td colspan="5">
                                    <asp:DropDownList runat="server" ID="ddlRatesId" CssClass="ddl" ClientIDMode="Static"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlRatesId_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="headCss">Category</span>
                                </td>
                                <td colspan="5">
                                    <asp:RadioButton runat="server" ID="rdoSolid" GroupName="SizeCategory" ClientIDMode="Static"
                                        AutoPostBack="true" Text="SOLID" OnCheckedChanged="SizeCategory_CheckedChanged"
                                        CssClass="rdoAlign" />
                                    <asp:RadioButton runat="server" ID="rdoPob" GroupName="SizeCategory" ClientIDMode="Static"
                                        AutoPostBack="true" Text="POB" OnCheckedChanged="SizeCategory_CheckedChanged"
                                        CssClass="rdoAlign" />
                                    <asp:RadioButton runat="server" ID="rdoPneumatic" GroupName="SizeCategory" ClientIDMode="Static"
                                        AutoPostBack="true" Text="PNEUMATIC" Visible="false" OnCheckedChanged="SizeCategory_CheckedChanged"
                                        CssClass="rdoAlign" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="headCss">Platform</span>
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="ddlPlatform" Width="180px" Height="22px" runat="server" ClientIDMode="Static"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlPlatform_SelectedIndexChange">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <span class="headCss">Base Type</span>
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="ddlType" Width="145px" runat="server" ClientIDMode="Static"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="headCss">Base RMCB</span>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtBaseRmcb" CssClass="txtID" ClientIDMode="Static"
                                        Width="135px" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                </td>
                                <td>
                                    <span class="headCss">Negotiate %</span>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtNegotiate" CssClass="txtID" ClientIDMode="Static"
                                        Width="135px" onkeypress="return isNumberAndMinusKey(event)" Text="0" MaxLength="4"></asp:TextBox>
                                </td>
                                <td>
                                    <span class="headCss">Currency</span>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtCurType" CssClass="txtID" ClientIDMode="Static"
                                        Width="85px" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <div style="width: 280px; border: 1px solid #ccc; padding: 5px; background-color: #BD1607;
                                        margin: 0px auto;">
                                        <span style="width: inherit; text-align: center; color: #fff !important; position: absolute">
                                            Applicable Period</span>
                                        <br />
                                        <div style="display: inline-block; width: 47%; padding: 5px 0px 2px 5px;">
                                            <span style="color: #fff !important;">From</span>
                                            <br />
                                            <asp:TextBox runat="server" ID="txtCustPriceAppFrom" ClientIDMode="Static" CssClass="txtID"
                                                Width="100px" Text=""></asp:TextBox>
                                        </div>
                                        <div style="display: inline-block; width: 48%; padding: 5px 0px 2px 5px;">
                                            <span style="color: #fff !important;">Till</span>
                                            <br />
                                            <asp:TextBox runat="server" ID="txtCustPriceAppTill" ClientIDMode="Static" CssClass="txtID"
                                                Width="100px" Text=""></asp:TextBox>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="headCss">Instruction</span>
                                </td>
                                <td colspan="5">
                                    <asp:TextBox runat="server" ID="txtCustPriceSpl" ClientIDMode="Static" TextMode="MultiLine"
                                        ReadOnly="true" onKeyUp="javascript:CheckMaxLength(this, 3999);" onChange="javascript:CheckMaxLength(this, 3999);"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="width: 320px; position: absolute; margin: 0px;">
                        <div style="padding: 20px;">
                            <div style="width: 210px; float: left;">
                                <span style="width: 104px; float: left; border: 1px solid #000; background-color: #fefe8b;">
                                    TYPE</span><span style="width: 82px; float: left; border: 1px solid #000; background-color: #fefe8b;">PREMIUM</span></div>
                            <div style="width: 210px; float: left; margin-right: 10px; overflow-y: scroll; height: Auto;
                                padding: 20px; max-height:380px;">
                                <asp:GridView runat="server" ID="gv_CustType" AutoGenerateColumns="false" AlternatingRowStyle-BackColor="#f5f5f5" >
                                    <HeaderStyle CssClass="gvSizeVisible"  />
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="120px" ItemStyle-Height="22px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblPreType" Text=""></asp:Label></ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="80px">
                                            <ItemTemplate>
                                                <asp:TextBox runat="server" ID="txtPreValue" Text="" onkeypress="return isNumberAndMinusKey(event)"
                                                    MaxLength="8" CssClass="txtCustPriceNum"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div style="width: 655px; padding: 5px 5px 5px 20px; border: 1px solid #F0C9A5; background-color: #F3F1D7;">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <div style="width: 80px; display: inline-block">
                                        <a id="btnCustPriceClear" class="btnclear" style="line-height: 18px;">Clear</a>
                                    </div>
                                    <div style="width: 80px; display: inline-block">
                                        <asp:Button runat="server" ID="btnCustPriceSave" ClientIDMode="Static" Text="Save"
                                            CssClass="btnsave" OnClientClick="javascript:return ctrlCustPriceMaster(event);" OnClick="btnCustPriceSave_Click" />
                                    </div>
                                    <div style="width: 70px; display: inline-block">
                                        <asp:Button runat="server" ID="btnCustPriceEdit" ClientIDMode="Static" Text="Edit"
                                            CssClass="btnedit" OnClientClick="javascript:return ctrlCustPriceMaster(event);" OnClick="btnCustPriceEdit_Click" />
                                    </div>
                                    <div style="width: 140px; display: inline-block">
                                        <asp:Button runat="server" ID="btnCustPriceCalc" ClientIDMode="Static" Text="Show / Calculate"
                                            CssClass="btnshow" OnClientClick="javascript:return ctrlCalPriceMaster(event);" OnClick="btnCustPriceCal_Click" />
                                    </div>
                                    <div style="width: 155px; display: inline-block; margin-left: 20px">
                                        <asp:Button runat="server" ID="btnCheckNullValues" ClientIDMode="Static" Text="Check Null Values"
                                            CssClass="btnnull" OnClientClick="javascript:return ctrlChkNullValues();" OnClick="btnCheckNullValues_Click" />
                                    </div>
                                    <div style="width: 640px; color: #ff0000; line-height: 15px; display: inline-block">
                                        <span id="errMsg" runat="server" clientidmode="Static"></span>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 20px;" colspan="2">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <div id="divGvMasterPrice">
                                    <asp:GridView runat="server" ID="gv_CustMasterPrice" RowStyle-Height="24px" OnRowDataBound="gv_CustMasterPrice_RowDataBound" 
                                    ViewStateMode="Enabled" OnInit="gv_CustMasterPrice_PreRender" ClientIDMode="Static">
                                    </asp:GridView>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divNullValuePopup">
            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <div style="font-weight: bold; font-size: 15px; text-decoration: underline; width: 820px;
                        float: left;">
                        Empty / Null value list(s)</div>
                    <a style="float: right; width: 18px; font-weight: bold; font-size: 20px; border: 1px solid #000000;
                        margin: 2px; padding-left: 4px;" onclick="NullValueCloseFunc();">X</a>
                    <asp:Literal runat="server" ID="litNullValueCustPrice" ClientIDMode="Static" Text=""></asp:Literal></ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdnCustCode" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnPlatform" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnType" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnPremiumValue" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnCategory" ClientIDMode="Static" Value="" />
    <script src="Scripts/datemonthyear.js" type="text/javascript"></script>
     
    <script src="scripts/jqueryui191.js" type="text/javascript"></script>
    <script src="scripts/gridviewScroll.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            liPossition = 0;

            $("#txtCustPriceAppFrom").datepicker({
                minDate: "+0D", maxDate: "+30D"
            }).keydown(function (e) {
                e.keyWhich = 0; e.keyCode = 0; e.preventDefault();
            });

            $("#txtCustPriceAppTill").datepicker({
                minDate: "+1D", maxDate: "+120D"
            }).keydown(function (e) {
                e.keyWhich = 0; e.keyCode = 0; e.preventDefault();
            });

            $(':text').bind('keydown', function (e) {
                //on keydown for all textboxes
                if (e.target.className == "txtID" || e.target.className == "dropDownID") {
                    if (e.keyCode == 13) { //if this is enter key
                        e.preventDefault();
                        return false;
                    }
                }
            });

            $('#btnCustPriceClear').click(function () {
                $('#errMsg').html(''); $('#txtCustName').val(''); $('#txtPriceRefNo').val(''); $('#txtRatesID').val('');
                $('#ddlPlatform').html(''); $('#ddlType').html(''); $('#txtBaseRmcb').val(''); $('#txtNegotiate').val('0');
                $('#txtCurType').val(''); $('#txtCustPriceSpl').val(''); $('#MainContent_gv_CustType tr:gt(1)').remove();
                $('#lblPreType_0').html(''); $('#txtPreValue_0').val(''); $('#MainContent_gv_CustMasterPrice tr:gt(0)').remove();
                $("input:radio[name=category]").attr('checked', false);
            });

            $("input:radio[name=ctl00$MainContent$SizeCategory]").click(function () {
                $('#ddlPlatform').html(''); $('#ddlType').html(''); $('#txtBaseRmcb').val(''); $('#txtNegotiate').val('0');
                $('#txtCustPriceSpl').val(''); $('#MainContent_gv_CustType tr:gt(1)').remove(); $('#lblPreType_0').html('');
                $('#txtPreValue_0').val(''); $('#MainContent_gv_CustMasterPrice tr:gt(0)').remove();
                $('#txtCustPriceAppTill').val(''); $('#txtCustPriceAppFrom').val('');
                $('#hdnCategory').val($(this).val().substr(3, $(this).val().length));
            });

            $('.dropdwonCustCss').click(function () {
                loadCustName('popup_box_cust');
                loadPopupBox('popup_box_cust', 'txtCustName');
            });

            $('.dropdwonPriceCss').click(function () {
                $('#errMsg').html('');
                if ($('#txtCustName').val().length > 0) {
                    loadPriceSheetRefNo('popup_box_price', $('#hdnCustCode').val());
                    loadPopupBox('popup_box_price', 'txtPriceRefNo');
                }
                else {
                    $('#errMsg').html('Enter Customer Name');
                }
            });

            $('.dropdwonRatesCss').click(function () {
                $('#errMsg').html('');
                var errRatemsg = '';
                if ($('#txtCustName').val().length == 0) {
                    errRatemsg += "Enter Customer Name <br />";
                }
                if ($('#txtPriceRefNo').val().length == 0) {
                    errRatemsg += "Enter Price Sheet Ref No.";
                }
                if (errRatemsg.length > 0) {
                    $('#errMsg').html(errRatemsg);
                }
                if ($('#errMsg').html().length == 0) {
                    loadRatesID('popup_box_rates');
                    loadPopupBox('popup_box_rates', 'txtRatesID');
                }
            });

        });

        $('body').click(function (e) {
            if ($('#popup_box_cust').is(':visible') == true && e.target.className != "dropdwonCustCss")
                unloadPopupBox('popup_box_cust');
            if ($('#popup_box_price').is(':visible') == true && e.target.className != "dropdwonPriceCss")
                unloadPopupBox('popup_box_price');
            if ($('#popup_box_rates').is(':visible') == true && e.target.className != "dropdwonRatesCss")
                unloadPopupBox('popup_box_rates');
        });

        function bindGvCustType(data) {
            if (data != '') {
                var strSplit = data.split(',');
                var row = $("[id*=MainContent_gv_CustType] tr:last-child").clone(true);
                $("[id*=MainContent_gv_CustType] tr").not($("[id*=MainContent_gv_CustType] tr:first-child")).remove();
                for (var j = 0; j < strSplit.length - 1; j++) {
                    $("td", row).eq(0).html("<span id='lblPreType_" + j + "'>" + strSplit[j].toString() + "</span>");
                    $("td", row).eq(1).html("<input type='text' value='' maxlength='8'  id='txtPreValue_" + j + "' class='txtCustPriceNum' onkeypress='return isNumberAndMinusKey(event)' autocomplete='off'>");
                    $("[id*=MainContent_gv_CustType]").append(row);
                    row = $("[id*=MainContent_gv_CustType] tr:last-child").clone(true);
                }
            }
        }

        function bindPremium(data) {
            if (data != '') {
                var strSplit = data.split('~');
                for (var j = 0; j < strSplit.length - 1; j++) {
                    var valSplit = strSplit[j].toString().split(':');
                    var gv_PreValue = $("input[id*='txtPreValue']");
                    for (var k = 0; k < gv_PreValue.length; k++) {
                        if ($('#lblPreType_' + k).html() == valSplit[0].toString()) {
                            $('#txtPreValue_' + k).val(valSplit[1].toString());
                            break;
                        }
                    }
                }
            }
        }

        function ctrlCalPriceMaster(eve) {
            if ($('#txtBaseRmcb').val().length == 0 || $('#txtBaseRmcb').val() == "0") {
                $('#errMsg').html("Enter base rmcb");
                eve.preventDefault();
                return false;
            }
            else {
                var concatVal = ''; var gv_PreValue = $("input[id*='txtPreValue']");
                for (j = 0; j < gv_PreValue.length; j++) {
                    if (concatVal.length > 0)
                        concatVal += "~" + $('#lblPreType_' + j).html() + "," + $('#txtPreValue_' + j).val();
                    else
                        concatVal = $('#lblPreType_' + j).html() + "," + $('#txtPreValue_' + j).val();
                }
                $('#hdnPremiumValue').val(concatVal);
                return true;
            }
        }

        function ctrlCustPriceMaster(eve) {
            showProgress(); $('#errMsg').html(''); $('#bindErrmsg').html('');
            var ctrlErrMsg = '';

            if ($('#txtCustPriceAppFrom').val().length == 0)
                ctrlErrMsg += 'Choose from date, ';
            if ($('#txtCustPriceAppTill').val().length == 0)
                ctrlErrMsg += 'Choose till date, ';
            var validDec = /^[-+]?(?:\d+\.?\d*|\.\d+)$/;
            if ($("#ddlCustName option:selected").text().length == 0)
                ctrlErrMsg += 'Enter customer name, ';
            if ($("#ddlPriceRef option:selected").text().length == 0 && $("#txtPriceRefNo").val().length == 0)
                ctrlErrMsg += 'Enter price ref no., ';
            if ($("#ddlRatesId option:selected").text().length == 0)
                ctrlErrMsg += 'Enter rates id, ';
            if ($("#ddlPlatform option:selected").text() == "" || $("#ddlPlatform option:selected").text() == "--SELECT--")
                ctrlErrMsg += 'Select platform, ';
            if ($("#ddlType option:selected").text() == "" || $("#ddlType option:selected").text() == "--SELECT--")
                ctrlErrMsg += 'Select base type, ';
            if ($('#txtBaseRmcb').val().length == 0 || $('#txtBaseRmcb').val() == "0")
                ctrlErrMsg += 'Enter base rmcb, ';
            else if (!validDec.test($('#txtBaseRmcb').val()))
                ctrlErrMsg += 'Enter proper rmcb value, ';

            var j;

            var finalErr = '';
            if (ctrlErrMsg.length > 0)
                finalErr += ctrlErrMsg;

            if (finalErr.length > 0) {
                $('#errMsg').html(finalErr).css({ "line-height": "15px" });
                hideProgress();
                eve.preventDefault();
                return false;
            }
            else {
                var concatVal = ''; var gv_PreValue = $("input[id*='txtPreValue']");
                for (j = 0; j < gv_PreValue.length; j++) {
                    if (concatVal.length > 0)
                        concatVal += "~" + $('#lblPreType_' + j).html() + "," + $('#txtPreValue_' + j).val();
                    else
                        concatVal = $('#lblPreType_' + j).html() + "," + $('#txtPreValue_' + j).val();
                }
                $('#hdnPremiumValue').val(concatVal);
                return true;
            }
        }

        function ctrlChkNullValues() {
            showProgress();
            $('#errMsg').html('');
            $('#bindErrmsg').html('');
            var ctrlErrMsg = '';
            var validDec = /^[-+]?(?:\d+\.?\d*|\.\d+)$/;
            if ($('#txtPriceRefNo').val().length == 0)
                ctrlErrMsg += 'Enter price ref no., ';
            if ($('#txtRatesID').val().length == 0)
                ctrlErrMsg += 'Enter rates id, ';
            if ($("#ddlPlatform option:selected").text() == "" || $("#ddlPlatform option:selected").text() == "CHOOSE")
                ctrlErrMsg += 'Select platform, ';
            if ($("#ddlType option:selected").text() == "" || $("#ddlType option:selected").text() == "CHOOSE")
                ctrlErrMsg += 'Select base type, ';

            if (ctrlErrMsg.length > 0) {
                $('#errMsg').html(ctrlErrMsg).css({ "line-height": "15px" });
                hideProgress();

                return false;
            }
            else {
                return true;
            }
        }

        function gvModel() {
            $('#gv_CustMasterPrice').gridviewScroll({ width: 1064, height: 500, freezesize: 2, headerrowcount: 2 });
        }

    </script>
</asp:Content>
