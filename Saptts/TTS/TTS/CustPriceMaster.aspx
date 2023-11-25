<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="CustPriceMaster.aspx.cs" Inherits="TTS.CustPriceMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/datemonthyear.css" rel="stylesheet" type="text/css" />
    <link href="Styles/popupBox.css" rel="stylesheet" type="text/css" />
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
            <table>
                <tr>
                    <td style="width: 100%;">
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
                        <div style="width: 935px; float: left; color: Red;" id="bindErrmsg">
                        </div>
                        <div style="width: 510px; float: left; line-height: 40px; padding-top: 5px;">
                            <div style="width: 500px; float: left;">
                                <div style="width: 105px; float: left;">
                                    <span class="headCss">Customer Name</span>
                                </div>
                                <div style="width: 385px; float: left; line-height: 28px;">
                                    <div class="dropDivCss">
                                        <asp:TextBox runat="server" ID="txtCustName" CssClass="dropDownID" ClientIDMode="Static"
                                            Width="355px"></asp:TextBox>
                                        <span class="dropdwonCustCss"></span>
                                        <div id="popup_box_cust">
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div style="width: 500px; float: left;">
                                <div style="width: 105px; float: left;">
                                    <span class="headCss">Price Ref No.</span>
                                </div>
                                <div style="width: 385px; float: left; line-height: 28px;">
                                    <div class="dropDivCss">
                                        <asp:TextBox runat="server" ID="txtPriceRefNo" CssClass="dropDownID" ClientIDMode="Static"
                                            Width="355px"></asp:TextBox>
                                        <span class="dropdwonPriceCss"></span>
                                        <div id="popup_box_price">
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div style="width: 500px; float: left;">
                                <div style="width: 105px; float: left;">
                                    <span class="headCss">Rates-ID</span>
                                </div>
                                <div style="width: 385px; float: left; line-height: 28px;">
                                    <div class="dropDivCss">
                                        <asp:TextBox runat="server" ID="txtRatesID" CssClass="dropDownID" ClientIDMode="Static"
                                            Width="355px"></asp:TextBox>
                                        <span class="dropdwonRatesCss"></span>
                                        <div id="popup_box_rates">
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div style="width: 500px; float: left;">
                                <div style="width: 105px; float: left;">
                                    <span class="headCss">Category</span>
                                </div>
                                <div style="width: 385px; float: left;" id="categoryDiv">
                                    <div style="width: 100px; float: left;" id="rdb0">
                                        <input type="radio" name="category" value="Solid" />SOLID</div>
                                    <div style="width: 100px; float: left;" id="rdb1">
                                        <input type="radio" name="category" value="Pob" />POB</div>
                                    <div style="width: 130px; float: left; display: none;" id="rdb2">
                                        <input type="radio" name="category" value="Pneumatic" />PNEUMATIC</div>
                                </div>
                            </div>
                        </div>
                        <div style="width: 425px; float: left; line-height: 20px; padding-top: 5px; padding-left: 5px;">
                            <asp:TextBox runat="server" ID="txtCustPriceSpl" ClientIDMode="Static" TextMode="MultiLine"
                                Width="415px" Height="140px" Enabled="false" onKeyUp="javascript:CheckMaxLength(this, 3999);"
                                onChange="javascript:CheckMaxLength(this, 3999);"></asp:TextBox>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%;">
                        <div style="width: 510px; float: left; line-height: 40px;">
                            <div style="width: 250px; float: left;">
                                <div style="width: 105px; float: left;">
                                    <span class="headCss">Platform</span>
                                </div>
                                <div style="width: 145px; float: left;">
                                    <select id="ddlPlatform" class="txtID" style="width: 145px;">
                                    </select>
                                </div>
                                <div style="width: 105px; float: left;">
                                    <span class="headCss">Base Type</span>
                                </div>
                                <div style="width: 145px; float: left;">
                                    <select id="ddlType" class="txtID" style="width: 145px;">
                                    </select>
                                </div>
                            </div>
                            <div style="width: 230px; float: left; padding-left: 20px;">
                                <div style="width: 85px; float: left;">
                                    <span class="headCss">Base RMCB</span>
                                </div>
                                <div style="width: 140px; float: left;">
                                    <asp:TextBox runat="server" ID="txtBaseRmcb" CssClass="txtID" ClientIDMode="Static"
                                        Width="135px" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                </div>
                                <div style="width: 85px; float: left;">
                                    <span class="headCss">Negotiate %</span>
                                </div>
                                <div style="width: 140px; float: left;">
                                    <asp:TextBox runat="server" ID="txtNegotiate" CssClass="txtID" ClientIDMode="Static"
                                        Width="135px" onkeypress="return isNumberAndMinusKey(event)" Text="0" MaxLength="4"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 917px; float: left; padding-left: 20px; padding-top: 3px; border: 1px solid #F0C9A5;
                                margin-top: 10px; background-color: #F3F1D7;">
                                <div style="width: 80px; float: left;">
                                    <a id="btnCustPriceClear" class="btnclear" style="line-height: 18px;">Clear</a>
                                </div>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                                    <ContentTemplate>
                                        <div style="width: 100px; float: left;">
                                            <asp:Button runat="server" ID="btnCustPriceSave" ClientIDMode="Static" Text="Save"
                                                CssClass="btnsave" OnClientClick="javascript:return ctrlCustPriceMaster();" OnClick="btnCustPriceSave_Click" />
                                        </div>
                                        <div style="width: 90px; float: left;">
                                            <asp:Button runat="server" ID="btnCustPriceEdit" ClientIDMode="Static" Text="Edit"
                                                CssClass="btnedit" OnClientClick="javascript:return ctrlCustPriceMaster();" OnClick="btnCustPriceEdit_Click" />
                                        </div>
                                        <div style="width: 140px; float: left; padding-left: 90px;">
                                            <asp:Button runat="server" ID="btnCustPriceCalc" ClientIDMode="Static" Text="Show / Calculate"
                                                CssClass="btncalc" OnClientClick="javascript:return ctrlCustPriceMaster();" OnClick="btnCustPriceCal_Click" />
                                        </div>
                                        <div style="width: 155px; float: right; padding-right: 10px;">
                                            <asp:Button runat="server" ID="btnCheckNullValues" ClientIDMode="Static" Text="Check Null Values"
                                                CssClass="btnnull" OnClientClick="javascript:return ctrlChkNullValues();" OnClick="btnCheckNullValues_Click" />
                                        </div>
                                        <div style="width: 915px; float: left; color: #ff0000; line-height: 15px;">
                                            <span id="errMsg" runat="server" clientidmode="Static"></span>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div style="width: 425px; float: left; line-height: 30px; padding-left: 5px;">
                            <div style="width: 90px; text-align: center; float: left; padding-right: 10px;">
                                <div style="width: 85px; float: left;">
                                    <span class="headCss">Currency</span>
                                </div>
                                <div style="width: 85px; float: left;">
                                    <asp:TextBox runat="server" ID="txtCurType" CssClass="txtID" ClientIDMode="Static"
                                        Width="85px" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 317px; float: left; border: 1px solid #ccc; padding-left: 5px;
                                padding-bottom: 8px; background-color: #BD1607;">
                                <span class="headCss" style="width: 320px; float: left; text-align: center; color: #fff !important;">
                                    Applicable Period</span> <span class="headCss" style="width: 40px; float: left; color: #fff !important;">
                                        From</span><div style="width: 105px; float: left; padding-right: 15px;">
                                            <asp:TextBox runat="server" ID="txtCustPriceAppFrom" ClientIDMode="Static" CssClass="txtID"
                                                Width="100px" Text=""></asp:TextBox></div>
                                <span class="headCss" style="width: 40px; float: left; color: #fff !important;">Till</span><div
                                    style="width: 105px; float: left; padding-right: 10px;">
                                    <asp:TextBox runat="server" ID="txtCustPriceAppTill" ClientIDMode="Static" CssClass="txtID"
                                        Width="100px" Text=""></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%; padding-top: 20px;">
                        <div style="width: 940px; float: left; text-align: center; font-weight: bold; line-height: 25px;">
                            <div style="width: 210px; float: left; margin-right: 10px;">
                                <span style="width: 104px; float: left; border: 1px solid #000; background-color: #fefe8b;">
                                    TYPE</span><span style="width: 82px; float: left; border: 1px solid #000; background-color: #fefe8b;">PREMIUM</span></div>
                            <div style="width: 720px; float: left;">
                                <span style="width: 172px; float: left; border: 1px solid #000; background-color: #fefe8b;">
                                    SIZE</span><span style="width: 59px; float: left; border: 1px solid #000; background-color: #fefe8b;">RIM</span><span
                                        style="width: 97px; float: left; border: 1px solid #000; background-color: #fefe8b;">TYPE</span><span
                                            style="width: 119px; float: left; border: 1px solid #000; background-color: #fefe8b;">BRAND</span><span
                                                style="width: 119px; float: left; border: 1px solid #000; background-color: #fefe8b;">SIDEWALL</span><span
                                                    style="width: 117px; float: left; border: 1px solid #000; background-color: #fefe8b;">UNITPRICE</span></div>
                        </div>
                        <div style="width: 210px; float: left; margin-right: 10px; overflow: scroll; height: 500px">
                            <asp:GridView runat="server" ID="gv_CustType" AutoGenerateColumns="false" AlternatingRowStyle-BackColor="#f5f5f5">
                                <HeaderStyle CssClass="gvSizeVisible" />
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
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <div style="width: 720px; float: left; overflow: scroll; height: 500px">
                                    <asp:GridView runat="server" ID="gv_CustMasterPrice" AutoGenerateColumns="false"
                                        AlternatingRowStyle-BackColor="#f5f5f5" RowStyle-Height="24px">
                                        <HeaderStyle CssClass="gvSizeVisible" />
                                        <Columns>
                                            <asp:BoundField DataField="Size" HeaderText="Size" ItemStyle-Width="175px" />
                                            <asp:BoundField DataField="Rim" HeaderText="Rim" ItemStyle-Width="60px" />
                                            <asp:BoundField DataField="Type" HeaderText="Type" ItemStyle-Width="100px" />
                                            <asp:BoundField DataField="Brand" HeaderText="Brand" ItemStyle-Width="120px" />
                                            <asp:BoundField DataField="Sidewall" HeaderText="Sidewall" ItemStyle-Width="120px" />
                                            <asp:BoundField DataField="UnitPrice" HeaderText="UnitPrice" ItemStyle-Width="120px" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdnCustCode" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnPlatform" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnType" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnPremiumValue" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnCategory" ClientIDMode="Static" Value="" />
    <script src="Scripts/datemonthyear.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //$('#txtCustName').focus();

            liPossition = 0;
            //Customer Textbox code
            $('#txtCustName').keyup(function (e) {
                $('#bindErrmsg').html('');
                if ((e.keyCode == 40 || e.which == 40) || (e.keyCode == 38 || e.which == 38)) { // down arrow key code 
                    upanddownKey(e, 'popup_box_cust');
                } else if (e.keyCode == 13 || e.which == 13) { // enter key code 
                    popupEnterKey('popup_box_cust', 'txtCustName');
                    if ($('#txtCustName').val().length > 0) {
                        getCustDetails($('#txtCustName').val());
                        $('#txtPriceRefNo').focus();
                    }
                }
                else {
                    var cname = $(this).val();
                    if (cname.length > 0) {
                        $.ajax({ type: "POST", url: "UserValidation.aspx?type=getcust&cname=" + cname + "", context: document.body,
                            success: function (data) {
                                if (data != '') {
                                    $('#popup_box_cust').html(data);
                                    $("div[id*='popup_box_cust'] ul li").first().addClass('current');
                                    loadPopupBox('popup_box_cust', 'txtCustName');
                                }
                                else {
                                    $('#popup_box_cust').html('');
                                    $('#popup_box_cust').hide();
                                }
                            }
                        });
                    }
                    else {
                        $('#popup_box_cust').html('');
                        $('#popup_box_cust').hide();
                    }
                    liPossition = 0;
                }
            });

            $('#popup_box_cust').hover(function () {
                popupHover('popup_box_cust', 'txtCustName');
                if ($('#txtCustName').val().length > 0) {
                    getCustDetails($('#txtCustName').val());
                    $('#txtPriceRefNo').focus();
                }
            });

            //PriceSheet RefNo Textbox Code
            $('#txtPriceRefNo').keyup(function (e) {
                $('#bindErrmsg').html('');
                if ((e.keyCode == 40 || e.which == 40) || (e.keyCode == 38 || e.which == 38)) { // down arrow key code 
                    upanddownKey(e, 'popup_box_price');
                } else if (e.keyCode == 13 || e.which == 13) { // enter key code 
                    popupEnterKey('popup_box_price', 'txtPriceRefNo');
                    $('#txtRatesID').focus();
                }
                else {
                    var ccode = $('#hdnCustCode').val();
                    var priceref = $('#txtPriceRefNo').val();
                    if (ccode.length > 0) {
                        $.ajax({ type: "POST", url: "BindRecords.aspx?type=getPriceRefCustMaster&cCode=" + ccode + "&priceref=" + priceref + "", context: document.body,
                            success: function (data) {
                                if (data != '') {
                                    $('#popup_box_price').html(data);
                                    $("div[id*='popup_box_price'] ul li").first().addClass('current');
                                    loadPopupBox('popup_box_price', 'txtPriceRefNo');
                                }
                                else {
                                    $('#popup_box_price').html('');
                                    $('#popup_box_price').hide();
                                }
                            }
                        });
                    }
                    else {
                        $('#popup_box_price').html('');
                        $('#popup_box_price').hide();
                    }
                    liPossition = 0;
                }
            });

            $('#popup_box_price').hover(function () {
                popupHover('popup_box_price', 'txtPriceRefNo');
                $('#txtRatesID').focus();
            });

            //Rates-ID Textbox code
            $('#txtRatesID').keyup(function (e) {
                $('#bindErrmsg').html('');
                if ((e.keyCode == 40 || e.which == 40) || (e.keyCode == 38 || e.which == 38)) { // down arrow key code 
                    upanddownKey(e, 'popup_box_rates');
                } else if (e.keyCode == 13 || e.which == 13) { // enter key code 
                    popupEnterKey('popup_box_rates', 'txtRatesID');
                    $('#ddlPlatform').focus();
                }
                else {
                    var ccode = $('#hdnCustCode').val();
                    var ratesid = $(this).val();
                    if (ratesid.length > 0 && ccode.length > 0) {
                        $.ajax({ type: "POST", url: "BindRecords.aspx?type=getRatesID&rid=" + ratesid + "", context: document.body,
                            success: function (data) {
                                if (data != '') {
                                    $('#popup_box_rates').html(data);
                                    $("div[id*='popup_box_rates'] ul li").first().addClass('current');
                                    loadPopupBox('popup_box_rates', 'txtRatesID');
                                }
                                else {
                                    $('#popup_box_rates').html('');
                                    $('#popup_box_rates').hide();
                                }
                            }
                        });
                    }
                    else {
                        $('#popup_box_rates').html('');
                        $('#popup_box_rates').hide();
                    }
                    liPossition = 0;
                }
            });

            $('#popup_box_rates').hover(function () {
                popupHover('popup_box_rates', 'txtRatesID');
                $('#ddlPlatform').focus();
            });

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

            $('#ddlPlatform').change(function () {
                var ccode = $('#hdnCustCode').val();
                var strPlatform = $("#ddlPlatform option:selected").text();
                var strCategory = $("input:radio[name=category]:checked").val();
                $('#MainContent_gv_CustMasterPrice tr:gt(0)').remove();
                if (strPlatform != "CHOOSE") {
                    $.ajax({ type: "POST", url: "BindRecords.aspx?type=getApprovedTypeConfigWise&config=" + strPlatform + "&cCode=" + ccode + "&category=" + strCategory + "", context: document.body,
                        success: function (data) {
                            if (data != '') {
                                $('#ddlType').html(data);
                                $('#MainContent_gv_CustType tr:gt(1)').remove();
                                $('#lblPreType_0').html('');
                                $('#txtPreValue_0').val('');
                                $('#hdnPlatform').val(strPlatform);
                            }
                        }
                    });
                } else {
                    $('#ddlType').html('');
                    $('#txtBaseRmcb').val('');
                    $('#txtNegotiate').val('');
                    $('#hdnPlatform').val('');
                }
            });

            $('#ddlType').change(function () {
                var ccode = $('#hdnCustCode').val();
                var strPlatform = $("#ddlPlatform option:selected").text();
                var strType = $("#ddlType option:selected").text();
                var strCategory = $("input:radio[name=category]:checked").val();
                $('#hdnType').val(strType);
                $('#MainContent_gv_CustMasterPrice tr:gt(0)').remove();
                if (strPlatform != "CHOOSE" && strType != "CHOOSE") {
                    $.ajax({ type: "POST", url: "BindRecords.aspx?type=getApprovedExceptTypeWise&config=" + strPlatform + "&cCode=" + ccode + "&tyretype=" + strType + "&category=" + strCategory + "",
                        context: document.body,
                        success: function (data) {
                            if (data != '') {
                                var strSplit = data.split('~');
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
                    });
                    var txtPriceRefNo = $('#txtPriceRefNo').val();
                    $.ajax({ type: "POST", url: "BindRecords.aspx?type=getBaseTypeVal&config=" + strPlatform + "&cCode=" + ccode + "&basetype=" + strType + "&PRefNo=" + txtPriceRefNo + "",
                        context: document.body,
                        success: function (data) {
                            if (data != '') {
                                $('#txtBaseRmcb').val(data);
                            }
                        }
                    });

                    var txtRatesID = $('#txtRatesID').val();
                    $('#txtBaseRmcb').val('');
                    $.ajax({ type: "POST", url: "BindRecords.aspx?type=getPremiumTypeVal&config=" + strPlatform + "&cCode=" + ccode + "&tyretype=" + strType + "&category=" + strCategory + "&PRefNo=" + txtPriceRefNo + "&ratesID=" + txtRatesID + "",
                        context: document.body,
                        success: function (data) {
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
                    });
                } else {
                    $('#txtBaseRmcb').val('');
                    $('#txtNegotiate').val('');
                    $('#hdnType').val('');
                }
            });

            $('#btnCustPriceClear').click(function () {
                $('#errMsg').html(''); $('#txtCustName').val(''); $('#txtPriceRefNo').val(''); $('#txtRatesID').val('');
                $('#ddlPlatform').html(''); $('#ddlType').html(''); $('#txtBaseRmcb').val(''); $('#txtNegotiate').val('0');
                $('#txtCurType').val(''); $('#txtCustPriceSpl').val(''); $('#MainContent_gv_CustType tr:gt(1)').remove();
                $('#lblPreType_0').html(''); $('#txtPreValue_0').val(''); $('#MainContent_gv_CustMasterPrice tr:gt(0)').remove();
                $("input:radio[name=category]").attr('checked', false);
            });


            $("input:radio[name=category]").click(function () {
                $('#ddlPlatform').html(''); $('#ddlType').html(''); $('#txtBaseRmcb').val(''); $('#txtNegotiate').val('0');
                $('#txtCustPriceSpl').val(''); $('#MainContent_gv_CustType tr:gt(1)').remove(); $('#lblPreType_0').html('');
                $('#txtPreValue_0').val(''); $('#MainContent_gv_CustMasterPrice tr:gt(0)').remove();
                $('#txtCustPriceAppTill').val(''); $('#txtCustPriceAppFrom').val('');
                var categoryValue = $(this).val(); var ccode = $('#hdnCustCode').val(); $('#hdnCategory').val(categoryValue);
                $.ajax({ type: "POST", url: "BindRecords.aspx?type=getApprovedConfigCategoryWise&category=" + categoryValue + "&cCode=" + ccode + "", context: document.body,
                    success: function (data) {
                        if (data != '') {
                            $('#ddlPlatform').html(data);
                        }
                    }
                });
                getPriceDateDetails($('#hdnCustCode').val(), $('#txtPriceRefNo').val(), $('#hdnCategory').val());
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

        });                                                                //documnet end

        $('body').click(function (e) {
            if ($('#popup_box_cust').is(':visible') == true && e.target.className != "dropdwonCustCss")
                unloadPopupBox('popup_box_cust');
            if ($('#popup_box_price').is(':visible') == true && e.target.className != "dropdwonPriceCss")
                unloadPopupBox('popup_box_price');
            if ($('#popup_box_rates').is(':visible') == true && e.target.className != "dropdwonRatesCss")
                unloadPopupBox('popup_box_rates');
        });

        function getCustDetails(strCustName) {
            $.ajax({ type: "POST", url: "CustPriceMaster.aspx/get_CustDetails_WebMethod", data: '{strCustName:"' + strCustName + '"}', contentType: "application/json; charset=utf-8",
                dataType: "json", success: OnSuccessCust,
                failure: function (response) {
                    alert(response.d);
                },
                error: function (response) {
                    alert(response.d);
                }
            });
        }

        function OnSuccessCust(response) {
            $('#txtPriceRefNo').val(''); $('#txtRatesID').val('');
            var xmlDoc = $.parseXML(response.d); var xml = $(xmlDoc); var listVals = xml.find("T1");
            if ($(listVals[0]).find("Custcode").text() != "") {
                $('#hdnCustCode').val($(listVals[0]).find("Custcode").text());
                $('#txtCurType').val($(listVals[0]).find("PriceUnit").text().substr(0, 3).toUpperCase());
                $('#txtCustPriceSpl').val($(listVals[0]).find("specialinstruction").text());

                $('#rdb0').css({ "display": "none" }); $('#rdb1').css({ "display": "none" }); $('#rdb2').css({ "display": "none" }); $("input:radio[name=category]").attr('checked', false);
                for (var j = 0; j < listVals.length; j++) {
                    if ($(listVals[j]).find("SizeCategory").text().toLocaleLowerCase() == "solid")
                        $('#rdb0').css({ "display": "block" });
                    else if ($(listVals[j]).find("SizeCategory").text().toLocaleLowerCase() == "pob")
                        $('#rdb1').css({ "display": "block" });
                    else if ($(listVals[j]).find("SizeCategory").text().toLocaleLowerCase() == "pneumatic")
                        $('#rdb2').css({ "display": "block" });
                }
            }
        }

        function getPriceDateDetails(strCustCode, strPriceNo, strCategory) {
            $.ajax({ type: "POST", url: "CustPriceMaster.aspx/get_PriceDetails_WebMethod", data: '{strCustCode:"' + strCustCode + '",strPriceNo:"' + strPriceNo + '",strCategory:"' + strCategory + '"}', contentType: "application/json; charset=utf-8",
                dataType: "json", success: OnSuccessPriceDetails,
                failure: function (response) {
                    alert(response.d);
                },
                error: function (response) {
                    alert(response.d);
                }
            });
        }

        function OnSuccessPriceDetails(response) {
            var xmlDoc = $.parseXML(response.d); var xml = $(xmlDoc); var listVals = xml.find("T1");
            if ($(listVals[0]).find("StartDate").text() != "" && $(listVals[0]).find("EndDate").text() != "") {
                $('#txtCustPriceAppFrom').val($(listVals[0]).find("StartDate").text());
                $('#txtCustPriceAppTill').val($(listVals[0]).find("EndDate").text());
            }
        }

        function ctrlCustPriceMaster() {
            showProgress(); $('#errMsg').html(''); $('#bindErrmsg').html('');
            var ctrlErrMsg = '';
            //            var endDate = $('#txtCustPriceAppTill').val();
            //            var startDate = $('#txtCustPriceAppFrom').val();
            //            var fromDateMsg = endDate < startDate;
            //            var d = new Date();
            //            var nowDate = (d.getDate() < 10 ? '0' : '') + d.getDate() + '/' + ((d.getMonth() + 1) < 10 ? '0' : '') + (d.getMonth() + 1) + '/' + d.getFullYear();
            //            var nowdateMsg = endDate < nowDate;
            //            if (fromDateMsg.toString() == "false")
            //                ctrlErrMsg += 'Applicable till date should be > from date, ';
            //            if (nowdateMsg.toString() == "false")
            //                ctrlErrMsg += 'Applicable till date should be > current date, ';
            if ($('#txtCustPriceAppFrom').val().length == 0)
                ctrlErrMsg += 'Choose from date, ';
            if ($('#txtCustPriceAppTill').val().length == 0)
                ctrlErrMsg += 'Choose till date, ';
            var validDec = /^[-+]?(?:\d+\.?\d*|\.\d+)$/;
            if ($('#txtCustName').val().length == 0)
                ctrlErrMsg += 'Enter customer name, ';
            if ($('#txtPriceRefNo').val().length == 0)
                ctrlErrMsg += 'Enter price ref no., ';
            if ($('#txtRatesID').val().length == 0)
                ctrlErrMsg += 'Enter rates id, ';
            if ($("#ddlPlatform option:selected").text() == "" || $("#ddlPlatform option:selected").text() == "CHOOSE")
                ctrlErrMsg += 'Select platform, ';
            if ($("#ddlType option:selected").text() == "" || $("#ddlType option:selected").text() == "CHOOSE")
                ctrlErrMsg += 'Select base type, ';
            if ($('#txtBaseRmcb').val().length == 0 || $('#txtBaseRmcb').val() == "0")
                ctrlErrMsg += 'Enter base rmcb, ';
            else if (!validDec.test($('#txtBaseRmcb').val()))
                ctrlErrMsg += 'Enter proper rmcb value, ';

            var j;
            //            //check premiumtype
            //            var premiumErrmsg = '';
            //            var txtPreValue = $("input[id*='txtPreValue']");
            //            for (j = 0; j < txtPreValue.length; j++) {
            //                var value = $('#txtPreValue_' + j).val();
            //                if (value != undefined) {
            //                    if (value.length > 0) {
            //                        if (!validDec.test(value))
            //                            premiumErrmsg += ", Enter proper values for premium type row-" + parseInt(j + 1);
            //                    }
            //                    else
            //                        premiumErrmsg += ", Enter decimal values for premium type row-" + parseInt(j + 1);
            //                }
            //            }

            var finalErr = '';
            if (ctrlErrMsg.length > 0)
                finalErr += ctrlErrMsg;
            //            if (premiumErrmsg.length > 0)
            //                finalErr += premiumErrmsg;

            if (finalErr.length > 0) {
                $('#errMsg').html(finalErr).css({ "line-height": "15px" });
                hideProgress();
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
            if ($('#txtCustName').val().length == 0)
                ctrlErrMsg += 'Enter customer name, ';
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
    </script>
</asp:Content>
