<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="PriceSheetCompare.aspx.cs" Inherits="TTS.PriceSheetCompare" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/popupBox.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div align="center" class="pageTitleHead">
        COMPARE PRICE SHEET</div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <div style="padding-left: 10px;">
            <table>
                <tr>
                    <td style="width: 100%; padding-top: 10px;">
                        <div style="width: 935px; float: left; color: Red;" id="bindErrmsg">
                        </div>
                        <div style="width: 455px; float: left; line-height: 30px;">
                            <div style="width: 455px; float: left;">
                                <div style="width: 85px; float: left;">
                                    <span class="headCss">Customer 1</span>
                                </div>
                                <div style="width: 370px; float: left; line-height: 28px;">
                                    <div class="dropDivCss" style="width: 370px;">
                                        <asp:TextBox runat="server" ID="txtCustName1" CssClass="dropDownID" ClientIDMode="Static"
                                            Width="340px"></asp:TextBox>
                                        <span class="dropdwonCustCss" id="btnCustDropDown1"></span>
                                        <div id="popup_box_cust1">
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div style="width: 455px; float: left;">
                                <div style="width: 85px; float: left;">
                                    <span class="headCss">Category</span>
                                </div>
                                <div style="width: 370px; float: left;" id="categoryDiv">
                                    <div style="width: 100px; float: left;" id="rdb0">
                                        <input type="radio" name="category" value="Solid" />SOLID</div>
                                    <div style="width: 100px; float: left;" id="rdb1">
                                        <input type="radio" name="category" value="Pob" />POB</div>
                                    <div style="width: 120px; float: left; display: none;" id="rdb2">
                                        <input type="radio" name="category" value="Pneumatic" />PNEUMATIC</div>
                                </div>
                            </div>
                            <div style="width: 455px; float: left;">
                                <div style="width: 85px; float: left;">
                                    <span class="headCss">Price Ref No.</span>
                                </div>
                                <div style="width: 370px; float: left;">
                                    <div class="dropDivCss" style="width: 370px; line-height: 28px;">
                                        <asp:TextBox runat="server" ID="txtPriceRefNo1" CssClass="dropDownID" ClientIDMode="Static"
                                            Width="340px"></asp:TextBox>
                                        <span class="dropdwonCustCss" id="btnPriceDropDown1"></span>
                                        <div id="popup_box_price1">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div style="width: 455px; float: left; line-height: 30px; margin-left: 12px;">
                            <div style="width: 455px; float: left;">
                                <div style="width: 85px; float: left;">
                                    <span class="headCss">Customer 2</span>
                                </div>
                                <div style="width: 370px; float: left; line-height: 28px;">
                                    <div class="dropDivCss" style="width: 370px;">
                                        <asp:TextBox runat="server" ID="txtCustName2" CssClass="dropDownID" ClientIDMode="Static"
                                            Width="340px"></asp:TextBox>
                                        <span class="dropdwonCustCss" id="btnCustDropDown2"></span>
                                        <div id="popup_box_cust2">
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div style="width: 455px; float: left;">
                                <div style="width: 85px; float: left;">
                                    <span class="headCss">Price Ref No.</span>
                                </div>
                                <div style="width: 370px; float: left; line-height: 28px;">
                                    <div class="dropDivCss" style="width: 370px;">
                                        <asp:TextBox runat="server" ID="txtPriceRefNo2" CssClass="dropDownID" ClientIDMode="Static"
                                            Width="340px"></asp:TextBox>
                                        <span class="dropdwonCustCss" id="btnPriceDropDown2"></span>
                                        <div id="popup_box_price2">
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div style="width: 455px; float: left;">
                                <div style="width: 85px; float: left;">
                                    <span class="headCss">Rates-ID</span>
                                </div>
                                <div style="width: 370px; float: left; line-height: 28px;">
                                    <div class="dropDivCss" style="width: 370px;">
                                        <asp:TextBox runat="server" ID="txtRatesID" CssClass="dropDownID" ClientIDMode="Static"
                                            Width="340px"></asp:TextBox>
                                        <span class="dropdwonCustCss" id="btnRatesDropDown"></span>
                                        <div id="popup_box_rates">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%; line-height: 30px; padding-top: 5px;">
                        <div style="float: left; width: 625px;">
                            <div style="width: 155px; float: left;">
                                <div style="width: 85px; float: left;">
                                    <span class="headCss">Currency</span>
                                </div>
                                <div style="width: 68px; float: left;">
                                    <asp:TextBox runat="server" ID="txtCurrency" CssClass="txtID" ClientIDMode="Static"
                                        Width="67px" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div style="width: 305px; float: left; margin-left: 10px;">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <asp:Button ID="btnSheetCompare" runat="server" Text="Compare" ClientIDMode="Static"
                                        CssClass="btnshow" OnClientClick="javascript:return CompareMainCtrlValidation();"
                                        OnClick="btnSheetCompare_Click" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div id="divErrmsg" style="float: left; color: #ff0000;" runat="server" clientidmode="Static">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%; padding-top: 10px;">
                        <div style="width: 940px; float: left; overflow: scroll; height: 500px;">
                            <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <asp:GridView runat="server" ID="gv_PriceCompareGrid" AutoGenerateColumns="false"
                                        AlternatingRowStyle-BackColor="#f5f5f5" RowStyle-Height="24px" RowStyle-Width="935px">
                                        <HeaderStyle CssClass="priceGvCss" />
                                        <Columns>
                                            <asp:BoundField DataField="Config" HeaderText="PLATFORM" ItemStyle-Width="100px" />
                                            <asp:BoundField DataField="Size" HeaderText="SIZE" ItemStyle-Width="130px" />
                                            <asp:BoundField DataField="Rim" HeaderText="RIM" ItemStyle-Width="50px" />
                                            <asp:BoundField DataField="Type" HeaderText="TYPE" ItemStyle-Width="80px" />
                                            <asp:BoundField DataField="Price1" HeaderText="PRICE1" ItemStyle-CssClass="txtOldPrice" />
                                            <asp:BoundField DataField="Price2" HeaderText="PRICE2" ItemStyle-CssClass="txtOldPrice" />
                                            <asp:BoundField DataField="Diff1" HeaderText="DIFF PRICE" ItemStyle-CssClass="txtOldPrice" />
                                            <asp:BoundField DataField="RMCB1" HeaderText="RMCB1" ItemStyle-CssClass="txtNewPrice" />
                                            <asp:BoundField DataField="RMCB2" HeaderText="RMCB2" ItemStyle-CssClass="txtNewPrice" />
                                            <asp:BoundField DataField="Diff2" HeaderText="DIFF RMCB" ItemStyle-CssClass="txtNewPrice" />
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdnCustCode1" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnCustCode2" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnCategory" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        $(document).ready(function () {
            //$('#txtCustName1').focus();

            liPossition = 0;
            //Customer 1 Textbox code
            $('#txtCustName1').keyup(function (e) {
                $('#bindErrmsg').html('');
                if ((e.keyCode == 40 || e.which == 40) || (e.keyCode == 38 || e.which == 38)) { // down arrow key code 
                    upanddownKey(e, 'popup_box_cust1');
                } else if (e.keyCode == 13 || e.which == 13) { // enter key code 
                    popupEnterKey('popup_box_cust1', 'txtCustName1');
                    if ($('#txtCustName1').val().length > 0) {
                        getCustDetails1($('#txtCustName1').val());
                        $('#txtPriceRefNo1').focus();
                    }
                }
                else {
                    var cname = $(this).val();
                    if (cname.length > 0) {
                        $.ajax({ type: "POST", url: "UserValidation.aspx?type=getcust&cname=" + cname + "", context: document.body,
                            success: function (data) {
                                if (data != '') {
                                    $('#popup_box_cust1').html(data);
                                    $("div[id*='popup_box_cust1'] ul li").first().addClass('current');
                                    loadPopupBox('popup_box_cust1', 'txtCustName1');
                                }
                                else {
                                    $('#popup_box_cust1').html('');
                                    $('#popup_box_cust1').hide();
                                }
                            }
                        });
                    }
                    else {
                        $('#popup_box_cust1').html('');
                        $('#popup_box_cust1').hide();
                    }
                    liPossition = 0;
                }
            });

            $('#popup_box_cust1').hover(function () {
                popupHover('popup_box_cust1', 'txtCustName1');
                if ($('#txtCustName1').val().length > 0) {
                    getCustDetails1($('#txtCustName1').val());
                    $('#txtPriceRefNo1').focus();
                }
            });

            //PriceSheet RefNo 1 Textbox Code
            $('#txtPriceRefNo1').keyup(function (e) {
                $('#bindErrmsg').html('');
                if ((e.keyCode == 40 || e.which == 40) || (e.keyCode == 38 || e.which == 38)) { // down arrow key code 
                    upanddownKey(e, 'popup_box_price1');
                } else if (e.keyCode == 13 || e.which == 13) { // enter key code 
                    popupEnterKey('popup_box_price1', 'txtPriceRefNo1');
                    $('#txtCustName2').focus();
                }
                else {
                    var ccode = $('#txtCustName1').val();
                    var priceref = $('#txtPriceRefNo1').val();
                    if (ccode.length > 0) {
                        $.ajax({ type: "POST", url: "BindRecords.aspx?type=getPriceRefCustWise&cCname=" + ccode + "&priceref=" + priceref + "&category=" + $('#hdnCategory').val() + "", context: document.body,
                            success: function (data) {
                                if (data != '') {
                                    $('#popup_box_price1').html(data);
                                    $("div[id*='popup_box_price1'] ul li").first().addClass('current');
                                    loadPopupBox('popup_box_price1', 'txtPriceRefNo1');
                                }
                                else {
                                    $('#popup_box_price1').html('');
                                    $('#popup_box_price1').hide();
                                }
                            }
                        });
                    }
                    else {
                        $('#popup_box_price1').html('');
                        $('#popup_box_price1').hide();
                    }
                    liPossition = 0;
                }
            });

            $('#popup_box_price1').hover(function () {
                popupHover('popup_box_price1', 'txtPriceRefNo1');
                $('#txtCustName2').focus();
            });

            //Rates-ID Textbox code
            $('#txtRatesID').keyup(function (e) {
                $('#bindErrmsg').html('');
                if ((e.keyCode == 40 || e.which == 40) || (e.keyCode == 38 || e.which == 38)) { // down arrow key code 
                    upanddownKey(e, 'popup_box_rates');
                } else if (e.keyCode == 13 || e.which == 13) { // enter key code 
                    popupEnterKey('popup_box_rates', 'txtRatesID');
                }
                else {
                    var ccode = $('#hdnCustCode1').val();
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
            });

            //Customer 2 Textbox code
            $('#txtCustName2').keyup(function (e) {
                $('#bindErrmsg').html('');
                if ((e.keyCode == 40 || e.which == 40) || (e.keyCode == 38 || e.which == 38)) { // down arrow key code 
                    upanddownKey(e, 'popup_box_cust2');
                } else if (e.keyCode == 13 || e.which == 13) { // enter key code 
                    popupEnterKey('popup_box_cust2', 'txtCustName2');
                    if ($('#txtCustName2').val().length > 0) {
                        getCustDetails2($('#txtCustName2').val());
                        $('#txtPriceRefNo2').focus();
                    }
                }
                else {
                    var cname = $(this).val();
                    if (cname.length > 0) {
                        $.ajax({ type: "POST", url: "UserValidation.aspx?type=getCompareCust&cname=" + $('#txtCustName1').val() + "&curr=" + $('#txtCurrency').val() + "&likeCust=" + cname + "", context: document.body,
                            success: function (data) {
                                if (data != '') {
                                    $('#popup_box_cust2').html(data);
                                    $("div[id*='popup_box_cust2'] ul li").first().addClass('current');
                                    loadPopupBox('popup_box_cust2', 'txtCustName2');
                                }
                                else {
                                    $('#popup_box_cust2').html('');
                                    $('#popup_box_cust2').hide();
                                }
                            }
                        });
                    }
                    else {
                        $('#popup_box_cust2').html('');
                        $('#popup_box_cust2').hide();
                    }
                    liPossition = 0;
                }
            });

            $('#popup_box_cust2').hover(function () {
                popupHover('popup_box_cust2', 'txtCustName2');
                if ($('#txtCustName2').val().length > 0) {
                    getCustDetails2($('#txtCustName2').val());
                    $('#txtPriceRefNo2').focus();
                }
            });

            //PriceSheet RefNo 2 Textbox Code
            $('#txtPriceRefNo2').keyup(function (e) {
                $('#bindErrmsg').html('');
                if ((e.keyCode == 40 || e.which == 40) || (e.keyCode == 38 || e.which == 38)) { // down arrow key code 
                    upanddownKey(e, 'popup_box_price2');
                } else if (e.keyCode == 13 || e.which == 13) { // enter key code 
                    popupEnterKey('popup_box_price2', 'txtPriceRefNo2');
                    $('#txtRatesID').focus();
                }
                else {
                    var ccode = $('#txtCustName2').val();
                    var priceref = $('#txtPriceRefNo2').val();
                    if (ccode.length > 0) {
                        $.ajax({ type: "POST", url: "BindRecords.aspx?type=getPriceRefCustWise&cCname=" + ccode + "&priceref=" + priceref + "&category=" + $('#hdnCategory').val() + "", context: document.body,
                            success: function (data) {
                                if (data != '') {
                                    $('#popup_box_price2').html(data);
                                    $("div[id*='popup_box_price2'] ul li").first().addClass('current');
                                    loadPopupBox('popup_box_price2', 'txtPriceRefNo2');
                                }
                                else {
                                    $('#popup_box_price2').html('');
                                    $('#popup_box_price2').hide();
                                }
                            }
                        });
                    }
                    else {
                        $('#popup_box_price2').html('');
                        $('#popup_box_price2').hide();
                    }
                    liPossition = 0;
                }
            });

            $('#popup_box_price2').hover(function () {
                popupHover('popup_box_price2', 'txtPriceRefNo2');
                $('#txtRatesID').focus();
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

            $("input:radio[name=category]").click(function () {
                $('#hdnCategory').val($(this).val());
                $('#txtPriceRefNo1').val('');
                $('#txtPriceRefNo2').val('');
                $('#txtCustName2').val('');
                $('#txtRatesID').val('');
            });

            $('#btnCustDropDown1').click(function () {
                $('#bindErrmsg').html('');
                loadCustName('popup_box_cust1');
                loadPopupBox('popup_box_cust1', 'txtCustName1');
            });

            $('#btnCustDropDown2').click(function () {
                $('#bindErrmsg').html('');
                var errRatemsg = '';
                if ($('#txtCustName1').val().length == 0) {
                    errRatemsg += "Enter Customer Name 1 <br />";
                }
                if ($('#txtPriceRefNo1').val().length == 0) {
                    errRatemsg += "Enter Price Sheet Ref No. 1";
                }
                if (errRatemsg.length > 0) {
                    $('#bindErrmsg').html(errRatemsg);
                }
                if ($('#bindErrmsg').html().length == 0) {
                    LoadCompareCustList('popup_box_cust2', $('#txtCustName1').val(), $('#txtCurrency').val());
                    loadPopupBox('popup_box_cust2', 'txtCustName2');
                }
            });

            $('#btnPriceDropDown1').click(function () {
                $('#bindErrmsg').html('');
                var errRatemsg = '';
                if ($('#txtCustName1').val().length == 0) {
                    errRatemsg += "Enter Customer Name 1<br />";
                }
                if ($('#hdnCategory').val().length == 0) {
                    errRatemsg += "Select Category<br />";
                }
                if (errRatemsg.length > 0) {
                    $('#bindErrmsg').html(errRatemsg);
                }
                if ($('#bindErrmsg').html().length == 0) {
                    loadPriceSheetNameWise('popup_box_price1', $('#txtCustName1').val(), $('#hdnCategory').val());
                    loadPopupBox('popup_box_price1', 'txtPriceRefNo1');
                }
            });

            $('#btnPriceDropDown2').click(function () {
                $('#bindErrmsg').html('');
                var errRatemsg = '';
                if ($('#txtCustName1').val().length == 0) {
                    errRatemsg += "Enter Customer Name 1<br />";
                }
                if ($('#txtPriceRefNo1').val().length == 0) {
                    errRatemsg += "Enter Price Sheet Ref No. 1<br />";
                }
                if ($('#txtCustName2').val().length == 0) {
                    errRatemsg += "Enter Customer Name 2<br />";
                }
                if (errRatemsg.length > 0) {
                    $('#bindErrmsg').html(errRatemsg);
                }
                if ($('#bindErrmsg').html().length == 0) {
                    loadPriceSheetNameWise('popup_box_price2', $('#txtCustName2').val(), $('#hdnCategory').val());
                    loadPopupBox('popup_box_price2', 'txtPriceRefNo2');
                }
            });

            $('#btnRatesDropDown').click(function () {
                $('#bindErrmsg').html('');
                var errRatemsg = '';
                if ($('#txtCustName1').val().length == 0) {
                    errRatemsg += "Enter Customer Name 1<br />";
                }
                if ($('#txtPriceRefNo1').val().length == 0) {
                    errRatemsg += "Enter Price Sheet Ref No. 1<br />";
                }
                if ($('#txtCustName2').val().length == 0) {
                    errRatemsg += "Enter Customer Name 2<br />";
                }
                if ($('#txtPriceRefNo2').val().length == 0) {
                    errRatemsg += "Enter Price Sheet Ref No. 2<br />";
                }
                if (errRatemsg.length > 0) {
                    $('#bindErrmsg').html(errRatemsg);
                }
                if ($('#bindErrmsg').html().length == 0) {
                    loadRatesID('popup_box_rates');
                    loadPopupBox('popup_box_rates', 'txtRatesID');
                }
            });

        });

        $('body').click(function (e) {
            if ($('#popup_box_cust1').is(':visible') == true && e.target.id != "btnCustDropDown1")
                unloadPopupBox('popup_box_cust1');
            if ($('#popup_box_cust2').is(':visible') == true && e.target.id != "btnCustDropDown2")
                unloadPopupBox('popup_box_cust2');
            if ($('#popup_box_price1').is(':visible') == true && e.target.id != "btnPriceDropDown1")
                unloadPopupBox('popup_box_price1');
            if ($('#popup_box_price2').is(':visible') == true && e.target.id != "btnPriceDropDown2")
                unloadPopupBox('popup_box_price2');
            if ($('#popup_box_rates').is(':visible') == true && e.target.id != "btnRatesDropDown")
                unloadPopupBox('popup_box_rates');
        });

        function getCustDetails1(strCustName) {
            $.ajax({ type: "POST", url: "PriceSheetCompare.aspx/get_CustDetails_WebMethod", data: '{strCustName:"' + strCustName + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccessCust1,
                failure: function (response) {
                    alert(response.d);
                },
                error: function (response) {
                    alert(response.d);
                }
            });
        }

        function OnSuccessCust1(response) {
            var xmlDoc = $.parseXML(response.d);
            var xml = $(xmlDoc);
            var listVals = xml.find("T1");
            if ($(listVals[0]).find("PriceUnit").text() != "") {
                $('#hdnCustCode1').val($(listVals[0]).find("Custcode").text());
                $('#txtCurrency').val($(listVals[0]).find("PriceUnit").text().substr(0, 3).toUpperCase());

                $('#rdb0').css({ "display": "none" });
                $('#rdb1').css({ "display": "none" });
                $('#rdb2').css({ "display": "none" });
                $("input:radio[name=category]").attr('checked', false);
                for (var j = 0; j < listVals.length; j++) {
                    if ($(listVals[j]).find("SizeCategory").text().toLocaleLowerCase() == "solid")
                        $('#rdb0').css({ "display": "block" });
                    else if ($(listVals[j]).find("SizeCategory").text().toLocaleLowerCase() == "pob")
                        $('#rdb1').css({ "display": "block" });
                    else if ($(listVals[j]).find("SizeCategory").text().toLocaleLowerCase() == "pneumatic")
                        $('#rdb2').css({ "display": "block" });
                }

                $('#txtPriceRefNo1').val('');
                $('#txtPriceRefNo2').val('');
                $('#txtCustName2').val('');
                $('#txtRatesID').val('');
            }
        }

        function getCustDetails2(strCustName) {
            $.ajax({ type: "POST", url: "PriceSheetCompare.aspx/get_CustDetails_WebMethod", data: '{strCustName:"' + strCustName + '"}', //                 contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccessCust2,
                failure: function (response) {
                    alert(response.d);
                },
                error: function (response) {
                    alert(response.d);
                }
            });
        }

        function OnSuccessCust2(response) {
            var xmlDoc = $.parseXML(response.d);
            var xml = $(xmlDoc);
            var listVals = xml.find("T1");
            if ($(listVals[0]).find("PriceUnit").text() != "") {
                $('#hdnCustCode2').val($(listVals[0]).find("Custcode").text());
            }
        }

        function CompareMainCtrlValidation() {
            showProgress();
            $('#bindErrmsg').html('');
            $('#divErrmsg').html('');
            var errMsg = '';
            if ($('#txtCustName1').val().length == 0)
                errMsg += "Enter customer 1 <br />";
            if ($('#hdnCategory').val().length == 0)
                errmsg += "Select Category <br />";
            if ($('#txtPriceRefNo1').val().length == 0)
                errMsg += "Enter price sheet refno 1<br />";
            if ($('#txtCustName2').val().length == 0)
                errMsg += "Enter customer 2<br />";
            if ($('#txtPriceRefNo2').val().length == 0)
                errMsg += "Enter price sheet refno 2<br />";
            if ($('#txtRatesID').val().length == 0)
                errMsg += "Enter Rates-ID";

            if (errMsg.length > 0) {
                $('#bindErrmsg').html(errMsg);
                hideProgress();
                return false;
            }
            else {
                return true;
            }
        }
    </script>
</asp:Content>
