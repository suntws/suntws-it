<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="ReportsPriceSheet.aspx.cs" Inherits="TTS.ReportsPriceSheet" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/popupBox.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div align="center" class="pageTitleHead">
        PRICE SHEET REPORT</div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
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
                        <div style="width: 510px; float: left; line-height: 40px;">
                            <div style="width: 510px; float: left;">
                                <div style="width: 125px; float: left;">
                                    <span class="headCss">Customer Name</span>
                                </div>
                                <div style="width: 385px; float: left; line-height: 28px;">
                                    <div class="dropDivCss">
                                        <asp:TextBox runat="server" ID="txtRptCustName" CssClass="dropDownID" ClientIDMode="Static"
                                            Width="355px"></asp:TextBox>
                                        <span class="dropdwonCustCss"></span>
                                        <div id="popup_box_cust">
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div style="width: 510px; float: left;">
                                <div style="width: 125px; float: left;">
                                    <span class="headCss">Price Sheet No.</span></div>
                                <div style="width: 385px; float: left; line-height: 28px;">
                                    <div class="dropDivCss">
                                        <asp:TextBox runat="server" ID="txtRptPriceSheet" CssClass="dropDownID" ClientIDMode="Static"
                                            Width="355px"></asp:TextBox>
                                        <span class="dropdwonPriceCss"></span>
                                        <div id="popup_box_price">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div style="width: 425px; float: left; padding-left: 5px; padding-top: 5px;">
                            <div style="width: 420px; float: left;">
                                <span class="headCss" style="padding-right: 5px; padding-left: 10px;">Rates-ID</span>
                                <asp:TextBox runat="server" ID="txtRptRatesID" Text="" CssClass="txtID" ClientIDMode="Static"
                                    Enabled="false" Width="335px"></asp:TextBox>
                            </div>
                            <div style="width: 130px; float: left; padding-top: 10px;">
                                <span class="headCss" style="padding-left: 10px;">Currency</span>
                                <asp:TextBox runat="server" ID="txtCurrency" Text="" CssClass="txtID" ClientIDMode="Static"
                                    Enabled="false" Width="50px"></asp:TextBox>
                            </div>
                            <div style="width: 295px; float: left; padding-top: 10px;">
                                <span style="display: none;"><span class="headCss" style="padding-left: 10px;">Valid
                                    From</span>
                                    <asp:TextBox runat="server" ID="txtPriceSheetValidFrom" Text="" CssClass="txtID"
                                        ClientIDMode="Static" Enabled="false" Width="80px"></asp:TextBox>
                                    <span class="headCss" style="padding-left: 10px;">Till</span>
                                    <asp:TextBox runat="server" ID="txtPriceSheetValidTill" Text="" CssClass="txtID"
                                        ClientIDMode="Static" Enabled="false" Width="80px"></asp:TextBox>
                                </span><span id="validDateList"></span>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%; padding-top: 10px;">
                        <div style="float: left; width: 935px;">
                            <div style="float: left; width: 535px;">
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView runat="server" ID="gv_CustRptPriceList" ClientIDMode="Static" AutoGenerateColumns="false"
                                            AlternatingRowStyle-BackColor="#f5f5f5" HeaderStyle-Height="22px" AlternatingRowStyle-Height="22px"
                                            Width="508px">
                                            <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" Height="22px" />
                                            <Columns>
                                                <asp:TemplateField ItemStyle-Width="120px" ItemStyle-Height="22px">
                                                    <HeaderTemplate>
                                                        PLATFORM</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblConfig" Text='<%#Eval("Config") %>'></asp:Label></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="120px">
                                                    <HeaderTemplate>
                                                        TYPE</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblType" Text='<%#Eval("Tyretype") %>'></asp:Label></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="100px">
                                                    <HeaderTemplate>
                                                        <input type="checkbox" id="checkAllChk" />
                                                        SELECT ALL
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div style="width: 400px; float: left;">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView runat="server" ID="gv_Rpt_PremiumValue" AutoGenerateColumns="false"
                                            AlternatingRowStyle-BackColor="#f5f5f5" ClientIDMode="Static">
                                            <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" Height="22px" />
                                            <Columns>
                                                <asp:TemplateField ItemStyle-Width="110px" ItemStyle-Height="22px">
                                                    <HeaderTemplate>
                                                        PLATFORM</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblPlatform" Text='<%#Eval("Config") %>'></asp:Label></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="110px">
                                                    <HeaderTemplate>
                                                        PREMIUM</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblPremium" Text='<%#Eval("PremiumType") %>'></asp:Label></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="80px">
                                                    <HeaderTemplate>
                                                        PREMIUM VALUE</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblPremiumVlaue" Text='<%#Eval("PremiumValue") %>'></asp:Label></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="110px">
                                                    <HeaderTemplate>
                                                        BASE</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblBase" Text='<%#Eval("BaseType") %>'></asp:Label></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="80px">
                                                    <HeaderTemplate>
                                                        BASE VALUE</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblBaseVlaue" Text='<%#Eval("BaseRmcb") %>'></asp:Label></ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <div style="float: left; width: 400px; text-align: center; padding-top: 10px;">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                            <asp:Button runat="server" ID="btnDisplayRecords" ClientIDMode="Static" Text="Display Records"
                                                CssClass="btnshow" OnClientClick="javascript:return ctrlRptPriceSheetValidate();"
                                                OnClick="btnDisplayRecords_Click" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%;">
                        <div style="float: left;">
                            <asp:Label runat="server" ID="lblMsg" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label></div>
                    </td>
                </tr>
                <tr>
                    <td id="divAlreadyTD" style="display: none;">
                        <div class="headCss">
                            Already available few price sheet in order entry module</div>
                        <asp:UpdatePanel ID="UpdatePanel6" runat="server" ClientIDMode="Static" UpdateMode="Always">
                            <ContentTemplate>
                                <asp:GridView runat="server" ID="gv_AlreadyPublishedList" AutoGenerateColumns="false"
                                    HeaderStyle-BackColor="#F3E756" OnRowDeleting="gv_AlreadyPublishedList_RowDeleting"
                                    Width="1060px">
                                    <Columns>
                                        <asp:BoundField HeaderText="PRICE SHEET" DataField="PriceSheetRefNo" />
                                        <asp:BoundField HeaderText="RATES-ID" DataField="RatesID" />
                                        <asp:BoundField HeaderText="CATEGORY" DataField="category" />
                                        <asp:BoundField HeaderText="START DATE" DataField="StartDate" />
                                        <asp:BoundField HeaderText="END DATE" DataField="EndDate" />
                                        <asp:TemplateField ItemStyle-Width="100px">
                                            <HeaderTemplate>
                                                DELETE</HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Button runat="server" ID="btnPublishedSheetDisable" Text="Delete" CommandName="Delete"
                                                    CssClass="btnclear" /></ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <div style="color: #f00;">
                                    If you want remove any old published sheet Please click delete button
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%; padding-top: 15px;">
                        <div style="float: left; width: 1060px; height: 500px; overflow: scroll; border: 1px solid #ccc;">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" ClientIDMode="Static" UpdateMode="Always">
                                <ContentTemplate>
                                    <asp:GridView runat="server" ID="gv_RptFinalPriceList" ClientIDMode="Static" AutoGenerateColumns="true"
                                        AlternatingRowStyle-BackColor="#f5f5f5" Width="1060px" HeaderStyle-Height="25px">
                                        <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" Height="25px" />
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%; padding-top: 15px; padding-bottom: 15px;">
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server" ClientIDMode="Static" UpdateMode="Always">
                            <ContentTemplate>
                                <div style="width: 440px; float: left">
                                    <span class="headCss">*.XLS File Name</span>
                                    <asp:TextBox runat="server" ID="txtXlsFileName" ClientIDMode="Static" Text="" CssClass="txtID"
                                        Width="200px"></asp:TextBox>
                                    <asp:Button runat="server" ID="btnExportExcel" ClientIDMode="Static" OnClick="btnExportExcel_Click"
                                        Text="Export to excel" CssClass="btnauthorize" OnClientClick="javascript:return chkExportList();" />
                                </div>
                                <div style="width: 400px; float: left; padding-left: 80px; display: block;">
                                    <span class="headCss">COUNTRY</span>
                                    <asp:TextBox runat="server" ID="ddlCountry" ClientIDMode="Static" CssClass="txtID"
                                        Width="200px"></asp:TextBox>
                                    <asp:Button runat="server" ID="btnPublish" ClientIDMode="Static" Text="Publish" CssClass="btnauthorize"
                                        OnClientClick="javascript:return ctrlRptPriceSheetValidate();" OnClick="btnPublish_Click" />
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdnCustCode" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnUserName" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnTypeCount" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        $(document).ready(function () {
            $('#checkAllChk').click(function () {
                var chkLength = $("[id*=btnDelete_]").length;
                var j = 0;
                if (chkLength > 0) {
                    if ($(this).attr('checked') == "checked")
                        $("[id*=btnDelete_]").attr('checked', true)
                    else
                        $("[id*=btnDelete_]").attr('checked', false)
                    for (j = 0; j < chkLength; j++) {
                        gv_CustRptPriceList_rowDelete(j);
                    }
                }
                else {
                    alert('No records');
                    $(this).attr('checked', false);
                }
            });
            liPossition = 0;
            //Customer Textbox code
            $('#txtRptCustName').keyup(function (e) {
                $('#bindErrmsg').html('');
                if ((e.keyCode == 40 || e.which == 40) || (e.keyCode == 38 || e.which == 38)) { // down arrow key code 
                    upanddownKey(e, 'popup_box_cust');
                } else if (e.keyCode == 13 || e.which == 13) { // enter key code 
                    popupEnterKey('popup_box_cust', 'txtRptCustName');
                    if ($('#txtRptCustName').val().length > 0) {
                        getCustCurrency($('#txtRptCustName').val());
                        $('#txtRptPriceSheet').focus();
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
                                    loadPopupBox('popup_box_cust', 'txtRptCustName');
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
                popupHover('popup_box_cust', 'txtRptCustName');
                if ($('#txtRptCustName').val().length > 0) {
                    getCustCurrency($('#txtRptCustName').val());
                    $('#txtRptPriceSheet').focus();
                }
            });

            //PriceSheet RefNo Textbox Code
            $('#txtRptPriceSheet').keyup(function (e) {
                $('#bindErrmsg').html('');
                if ((e.keyCode == 40 || e.which == 40) || (e.keyCode == 38 || e.which == 38)) { // down arrow key code 
                    upanddownKey(e, 'popup_box_price');
                } else if (e.keyCode == 13 || e.which == 13) { // enter key code 
                    popupEnterKey('popup_box_price', 'txtRptPriceSheet');
                    if ($('#txtRptPriceSheet').val().length > 0) {
                        getPriceSheetValues();
                        $('#btnDisplayRecords').focus();
                    }
                }
                else {
                    var ccode = $('#hdnCustCode').val();
                    var priceref = $('#txtRptPriceSheet').val();
                    if (ccode.length > 0) {
                        $.ajax({ type: "POST", url: "BindRecords.aspx?type=getPriceRefAutorize&cCode=" + ccode + "&priceref=" + priceref + "", context: document.body,
                            success: function (data) {
                                if (data != '') {
                                    $('#popup_box_price').html(data);
                                    $("div[id*='popup_box_price'] ul li").first().addClass('current');
                                    loadPopupBox('popup_box_price', 'txtRptPriceSheet');
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
                popupHover('popup_box_price', 'txtRptPriceSheet');
                if ($('#txtRptPriceSheet').val().length > 0) {
                    getPriceSheetValues();
                    $('#btnDisplayRecords').focus();
                }
            });

            $('.dropdwonCustCss').click(function () {
                loadCustName('popup_box_cust');
                loadPopupBox('popup_box_cust', 'txtRptCustName');
            });

            $('.dropdwonPriceCss').click(function () {
                $('#bindErrmsg').html('');
                if ($('#txtRptCustName').val().length > 0) {
                    loadPriceSheetRefNo_AuthorizeOnly('popup_box_price', $('#hdnCustCode').val());
                    loadPopupBox('popup_box_price', 'txtRptPriceSheet');
                }
                else {
                    $('#bindErrmsg').html('Enter Customer Name');
                }
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

        });

        $('body').click(function (e) {
            if ($('#popup_box_cust').is(':visible') == true && e.target.className != "dropdwonCustCss")
                unloadPopupBox('popup_box_cust');
            if ($('#popup_box_price').is(':visible') == true && e.target.className != "dropdwonPriceCss")
                unloadPopupBox('popup_box_price');
        });

        function getCustCurrency(strCustName) {
            $.ajax({ type: "POST", url: "ReportsPriceSheet.aspx/get_CustCurrency_WebMethod", data: '{strCustName:"' + strCustName + '"}', contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccessCustCur,
                failure: function (response) {
                    alert(response.d);
                },
                error: function (response) {
                    alert(response.d);
                }
            });
        }

        function OnSuccessCustCur(response) {
            var xmlDoc = $.parseXML(response.d);
            var xml = $(xmlDoc);
            var listVals = xml.find("T1");
            if ($(listVals[0]).find("Custcode").text() != "") {
                $('#hdnCustCode').val($(listVals[0]).find("Custcode").text());
                $('#txtCurrency').val($(listVals[0]).find("PriceUnit").text().substr(0, 3).toUpperCase());
            }
        }

        function getPriceSheetValues() {
            $.ajax({ type: "POST", url: "ReportsPriceSheet.aspx/get_PriceSheetValues_WebMethod",
                data: '{strCustCode:"' + $('#hdnCustCode').val() + '",strPriceSheet:"' + $('#txtRptPriceSheet').val() + '",strUserName:"' + $('#hdnUserName').val() + '"}',
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: OnSuccessPriceSheetValues,
                failure: function (response) {
                    alert(response.d);
                },
                error: function (response) {
                    alert(response.d);
                }
            });
        }

        function OnSuccessPriceSheetValues(response) {
            var xmlDoc = $.parseXML(response.d);
            var xml = $(xmlDoc);
            var listVals = xml.find("T1");
            if ($(listVals[0]).find("RatesID").text() != "") {
                $('#txtRptRatesID').val($(listVals[0]).find("RatesID").text());
                var row = $("[id*=gv_Rpt_PremiumValue] tr:last-child").clone(true);
                $("[id*=gv_Rpt_PremiumValue] tr").not($("[id*=gv_Rpt_PremiumValue] tr:first-child")).remove();
                for (var j = 0; j < listVals.length; j++) {
                    $("td", row).eq(0).html("<span id='lblPlatform_" + j + "'>" + $(listVals[j]).find("Config").text() + "</span>");
                    $("td", row).eq(1).html("<span id='lblPremium_" + j + "'>" + $(listVals[j]).find("PremiumType").text() + "</span>");
                    $("td", row).eq(2).html("<span id='lblPremiumVlaue_" + j + "'>" + $(listVals[j]).find("PremiumValue").text() + "</span>");
                    $("td", row).eq(3).html("<span id='lblBase_" + j + "'>" + $(listVals[j]).find("BaseType").text() + "</span>");
                    $("td", row).eq(4).html("<span id='lblBaseVlaue_" + j + "'>" + $(listVals[j]).find("BaseRmcb").text() + "</span>");
                    $("[id*=gv_Rpt_PremiumValue]").append(row);
                    row = $("[id*=gv_Rpt_PremiumValue] tr:last-child").clone(true);
                }
                getPriceSheetTypeList();
                get_PriceSheetDateList();
            }
            else {
                $('#txtRptRatesID').val('');
                $('#txtPriceSheetValidFrom').val('');
                $('#txtPriceSheetValidTill').val('');
                $('#gv_Rpt_PremiumValue tr:gt(1)').remove();
                $('#lblPlatform_0').html('');
                $('#lblPremium_0').html('');
                $('#lblPremiumVlaue_0').html('');
                $('#lblBase_0').html('');
                $('#lblBaseVlaue_0').html('');
            }

        }

        function getPriceSheetTypeList() {
            $.ajax({ type: "POST", url: "ReportsPriceSheet.aspx/get_PriceSheetTypeList_WebMethod",
                data: '{strCustCode:"' + $('#hdnCustCode').val() + '",strPriceSheet:"' + $('#txtRptPriceSheet').val() + '",strUserName:"' + $('#hdnUserName').val() + '"}',
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: OnSuccessPriceSheetTypeList,
                failure: function (response) {
                    alert(response.d);
                },
                error: function (response) {
                    alert(response.d);
                }
            });
        }

        function OnSuccessPriceSheetTypeList(response) {
            var xmlDoc = $.parseXML(response.d);
            var xml = $(xmlDoc);
            var PSheetVals = xml.find("T1");
            if (PSheetVals.length > 0) {
                var row = $("[id*=gv_CustRptPriceList] tr:last-child").clone(true);
                $("[id*=gv_CustRptPriceList] tr").not($("[id*=gv_CustRptPriceList] tr:first-child")).remove();
                for (var j = 0; j < PSheetVals.length; j++) {
                    $("td", row).eq(0).html("<span id='lblConfig_" + j + "'>" + $(PSheetVals[j]).find("Config").text() + "</span>");
                    $("td", row).eq(1).html("<span id='lblType_" + j + "'>" + $(PSheetVals[j]).find("TyreType").text() + "</span>");
                    $("td", row).eq(2).html("<input id='btnDelete_" + j + "' type='checkbox' onclick='gv_CustRptPriceList_rowDelete(" + j + ");'>");
                    $("[id*=gv_CustRptPriceList]").append(row);
                    row = $("[id*=gv_CustRptPriceList] tr:last-child").clone(true);
                }
            }
            else {
                $('#gv_CustRptPriceList tr:gt(1)').remove();
                $('#lblConfig_0').html('');
                $('#lblType_0').html('');
            }
        }

        function get_PriceSheetDateList() {
            $.ajax({ type: "POST", url: "ReportsPriceSheet.aspx/get_PriceSheetDate_WebMethod",
                data: '{strCustCode:"' + $('#hdnCustCode').val() + '",strPriceSheet:"' + $('#txtRptPriceSheet').val() + '",strUserName:"' + $('#hdnUserName').val() + '"}',
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: OnSuccessPriceSheetDates,
                failure: function (response) {
                    alert(response.d);
                },
                error: function (response) {
                    alert(response.d);
                }
            });
        }

        function OnSuccessPriceSheetDates(response) {
            var xmlDoc = $.parseXML(response.d);
            var xml = $(xmlDoc);
            var PSheetDates = xml.find("T1");
            if (PSheetDates.length > 0) {
                var strDateAppend = '';
                strDateAppend += "<table style='width: 273px;border: 1px solid #000;text-align: center;background-color: #4FFAD8;'><tr style='font-weight: bold;'><td>Category</td><td>From</td><td>To</td></tr>";
                for (var j = 0; j < PSheetDates.length; j++) {
                    strDateAppend += "<tr>";
                    strDateAppend += "<td>" + $(PSheetDates[j]).find("SizeCategory").text() + "</td><td>" + $(PSheetDates[j]).find("StartDate").text() + "</td><td>" + $(PSheetDates[j]).find("EndDate").text() + "</td>";
                    strDateAppend += "</tr>";
                }
                strDateAppend += "</table>";
                $('#validDateList').html(strDateAppend.toString());
            }
        }

        function gv_CustRptPriceList_rowDelete(rowid) {
            var strConfig = $('#lblConfig_' + rowid).html();
            var strType = $('#lblType_' + rowid).html();

            if (strConfig.length > 0 && strType.length > 0) {
                var chk = $('#btnDelete_' + rowid).attr("checked");
                if (chk == "checked") {
                    $.ajax({ type: "POST", url: "BindRecords.aspx?type=removeRptPriceData&cCode=" + $('#hdnCustCode').val() + "&priceref=" + $('#txtRptPriceSheet').val() + "&config=" + strConfig + "&tyretype=" + strType + "&disStatus=true", //&brand=" + strBrand + "&sidewall=" + strSidewall + "
                        context: document.body,
                        success: function (data) {
                            if (parseInt(data) > 0) {
                                $('#gv_RptFinalPriceList tr:gt(0)').remove();
                                $('#gv_RptFinalPriceList th').remove()
                            }
                            else {
                                alert("Please try again");
                            }
                        }
                    });
                }
                else {
                    $.ajax({ type: "POST", url: "BindRecords.aspx?type=removeRptPriceData&cCode=" + $('#hdnCustCode').val() + "&priceref=" + $('#txtRptPriceSheet').val() + "&config=" + strConfig + "&tyretype=" + strType + "&disStatus=false", //&brand=" + strBrand + "&sidewall=" + strSidewall + "
                        context: document.body,
                        success: function (data) {
                            if (parseInt(data) > 0) {
                                $('#gv_RptFinalPriceList tr:gt(0)').remove();
                                $('#gv_RptFinalPriceList th').remove()
                            }
                            else {
                                alert("Please try again");
                            }
                        }
                    });
                }
            }
        }

        function ctrlRptPriceSheetValidate() {
            showProgress();
            $('#lblMsg').html('');
            var errMsg = '';
            if ($('#txtRptCustName').val().length == 0)
                errMsg += "Enter customer name.<br/>";
            if ($('#txtRptPriceSheet').val().length == 0)
                errMsg += "Enter pricesheet ref no.<br/>";
            if ($("table[id*=gv_CustRptPriceList]").find("TR").length - 1 == 0)
                errMsg += "Approved type list is empty.<br/>";
            if ($(':checkbox:checked').length == 0) {
                errMsg += "Please check any one checkbox list.<br/>";
            }

            if (errMsg.length > 0) {
                $('#lblMsg').html(errMsg);
                hideProgress();
                return false;
            }
            else {
                return true;
            }
        }

        function chkExportList() {
            showProgress();
            ctrlRptPriceSheetValidate();

            var errMsg = '';
            if ($('#lblMsg').html().length > 0)
                errMsg += $('#lblMsg').html();
            if ($("table[id*=gv_RptFinalPriceList]").find("TR").length - 1 == -1)
                errMsg += "Report list is empty.<br/>";
            if ($("table[id*=gv_RptFinalPriceList]").find("TR").length - 1 == 0)
                errMsg += "Report list is empty.<br/>";
            if ($('#txtXlsFileName').val().length == 0)
                errMsg += "Enter *.xls file name.";
            if (errMsg.length > 0) {
                $('#lblMsg').html(errMsg);
                hideProgress();
                return false;
            }
            else {
                hideProgress();
                return true;
            }
        }

        function bindReportSheet() {
            $('#gv_RptFinalPriceList  td').each(function (elem) {
                if ($(this).html().trim() == "0.00") {
                    $(this).css({ "background-color": "#ff0000" });
                }
            });
        }

        function alredyPublishedList(strDisplay) {
            $('#divAlreadyTD').css({ 'display': "" + strDisplay + "" });
        }
    </script>
</asp:Content>
