<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="RatesMaster.aspx.cs" Inherits="TTS.RatesMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/popupBox.css" rel="stylesheet" type="text/css" />
    <link href="Styles/ratesMasterStyle.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/ratesMaster.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div align="center" class="pageTitleHead">
        RATES MASTER</div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table>
            <tr>
                <td class="ratesMastedTD">
                    <div style="border: 1px solid #000; width: 932px; float: left; padding-top: 5px;
                        background-color: #fcfdf5; line-height: 30px;">
                        <div style="width: 475px; float: left; padding-left: 10px;">
                            <div style="width: 475px; float: left; height: 55px;">
                                <div style="width: 70px; float: left;">
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
                            <div style="width: 70px; float: left;">
                                <span class="headCss">Currency</span></div>
                            <div style="width: 300px; float: left;">
                                <asp:DropDownList runat="server" ID="ddlCurrency" ClientIDMode="Static" CssClass="ddlCur"
                                    Width="390px">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div style="width: 400px; float: left; padding-left: 45px;">
                            <div style="width: 100px; float: left;">
                                <span class="headCss">Value</span></div>
                            <div style="width: 300px; float: left">
                                <asp:TextBox runat="server" ID="txtCurRate" CssClass="txtID" Style="width: 50px;"
                                    ClientIDMode="Static" MaxLength="5" Enabled="false"></asp:TextBox><span>&nbsp;=</span>
                                <asp:TextBox runat="server" ID="txtIndRate" CssClass="txtID" Style="width: 100px;"
                                    ClientIDMode="Static" onkeypress="return isNumberKey(event)" MaxLength="6"></asp:TextBox><span
                                        style="padding-left: 5px;">INR.</span>
                            </div>
                            <div style="width: 100px; float: left; line-height: 15px;">
                                <span class="headCss">Conversion<br />
                                    Cost/Kg</span></div>
                            <div style="width: 100px; float: left;">
                                <asp:TextBox runat="server" ID="txtConvCost" CssClass="txtID" ClientIDMode="Static"
                                    onkeypress="return isNumberKey(event)" MaxLength="5" Width="100px"></asp:TextBox></div>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="ratesMastedTD">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always" ClientIDMode="Static">
                        <ContentTemplate>
                            <div class="ratesCostMainDiv">
                                <div class="ratesMasterCostDiv">
                                    <asp:GridView runat="server" ID="gv_Lip" AutoGenerateColumns="false" AlternatingRowStyle-BackColor="#f5f5f5"
                                        HeaderStyle-BackColor="#fefe8b" RowStyle-BackColor="#f0ffff">
                                        <Columns>
                                            <asp:TemplateField ItemStyle-Width="110px" ItemStyle-Height="22px">
                                                <HeaderTemplate>
                                                    LIP/GUM</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblLip" Text='<%# DataBinder.Eval(Container.DataItem, "lipgum") %>'></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Width="110px">
                                                <HeaderTemplate>
                                                    LIP COST</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtLipCost" Text="0" onkeypress="return isNumberKey(event)"
                                                        MaxLength="7" CssClass="txtCostNum"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <div class="ratesMasterCostDiv">
                                    <asp:GridView runat="server" ID="gv_Base" AutoGenerateColumns="false" AlternatingRowStyle-BackColor="#f5f5f5"
                                        HeaderStyle-BackColor="#fefe8b" RowStyle-BackColor="#f0ffff">
                                        <Columns>
                                            <asp:TemplateField ItemStyle-Width="110px" ItemStyle-Height="22px">
                                                <HeaderTemplate>
                                                    BASE</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblBase" Text='<%# DataBinder.Eval(Container.DataItem, "base") %>'></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Width="110px">
                                                <HeaderTemplate>
                                                    BASE COST</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtBaseCost" Text="0" onkeypress="return isNumberKey(event)"
                                                        MaxLength="7" CssClass="txtCostNum"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <div class="ratesMasterCostDiv">
                                    <asp:GridView runat="server" ID="gv_Center" AutoGenerateColumns="false" AlternatingRowStyle-BackColor="#f5f5f5"
                                        HeaderStyle-BackColor="#fefe8b" RowStyle-BackColor="#f0ffff">
                                        <Columns>
                                            <asp:TemplateField ItemStyle-Width="110px" ItemStyle-Height="22px">
                                                <HeaderTemplate>
                                                    CENTER</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblCenter" Text='<%# DataBinder.Eval(Container.DataItem, "center") %>'></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Width="110px">
                                                <HeaderTemplate>
                                                    CENTER COST</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtCenterCost" Text="0" onkeypress="return isNumberKey(event)"
                                                        MaxLength="7" CssClass="txtCostNum"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <div class="ratesMasterCostDiv">
                                    <asp:GridView runat="server" ID="gv_Tread" AutoGenerateColumns="false" AlternatingRowStyle-BackColor="#f5f5f5"
                                        HeaderStyle-BackColor="#fefe8b" RowStyle-BackColor="#f0ffff">
                                        <Columns>
                                            <asp:TemplateField ItemStyle-Width="110px" ItemStyle-Height="22px">
                                                <HeaderTemplate>
                                                    TREAD</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblTread" Text='<%# DataBinder.Eval(Container.DataItem, "tread") %>'></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Width="110px">
                                                <HeaderTemplate>
                                                    TREAD COST</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtTreadCost" Text="0" onkeypress="return isNumberKey(event)"
                                                        MaxLength="7" CssClass="txtCostNum"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <div style="width: 570px; float: left; border: 1px solid #000; padding-top: 5px;
                                    text-align: center; padding-left: 360px; margin-top: 5px; background-color: #f3e0ad;">
                                    <div style="width: 145px; float: left; padding-left: 8px; padding-bottom: 5px;">
                                        <span class="headCss">Load Factor</span> <span>
                                            <asp:TextBox runat="server" ID="txtLoatFactor" ClientIDMode="Static" Text="" Width="70px"
                                                CssClass="txtID" onkeypress="return isNumberKey(event)" MaxLength="5"></asp:TextBox></span>
                                    </div>
                                    <div style="width: 115px; float: left;">
                                        <span style="padding-left: 20px;">
                                            <asp:Button runat="server" ID="btnCal" ClientIDMode="Static" OnClientClick="javascript:return ctrlRatesMaster();"
                                                OnClick="btnCal_Click" Text="CALCULATE RM COST" CssClass="btnshow" /></span></div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td class="ratesMastedTD">
                    <span id="ErrMsg" style="float: left;"></span>
                </td>
            </tr>
            <tr>
                <td class="ratesMastedTD">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always" ClientIDMode="Static">
                        <ContentTemplate>
                            <div class="sizeHeaderDiv">
                                <div class="sizeChildDiv">
                                    <span class="sizefirstSpan">SOLID TYRE SIZE</span><span class="sizesecondSpan">BEADBAND
                                        (Rs/Kg)</span>
                                </div>
                                <div class="sizeChildDiv">
                                    <span class="sizefirstSpan">POB TYRE SIZE</span><span class="sizesecondSpan">BEADBAND
                                        (Rs/Kg)</span>
                                </div>
                                <div class="sizeChildDiv" style="visibility: hidden;">
                                    <span class="sizefirstSpan">PNEUMATIC TYRE SIZE</span><span class="sizesecondSpan">BEADBAND
                                        (Rs/Kg)</span></div>
                                <div class="sizeChildTypeDiv">
                                    <span class="sizefirstTypeSpan">TYRE TYPE</span><span class="sizesecondTypeSpan"
                                        style="width: 92px;">RM COST (Rs/Kg) </span>
                                </div>
                            </div>
                            <div style="width: 734px; height: 500px; overflow: scroll; float: left;">
                                <div class="sizeChildDiv">
                                    <asp:GridView runat="server" ID="gv_SolidSize" AutoGenerateColumns="false" AlternatingRowStyle-BackColor="#f5f5f5">
                                        <HeaderStyle CssClass="gvSizeVisible" />
                                        <Columns>
                                            <asp:TemplateField ItemStyle-Width="150px" ItemStyle-Height="22px">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblSolidSize" Text='<%# DataBinder.Eval(Container.DataItem, "TyreSize") %>'></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Width="70px">
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtSolidSizeValue" Text="0" onkeypress="return isNumberKey(event)"
                                                        MaxLength="7" CssClass="txtSizeNum"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <div class="sizeChildDiv">
                                    <asp:GridView runat="server" ID="gv_PobSize" AutoGenerateColumns="false" AlternatingRowStyle-BackColor="#f5f5f5">
                                        <HeaderStyle CssClass="gvSizeVisible" />
                                        <Columns>
                                            <asp:TemplateField ItemStyle-Width="150px" ItemStyle-Height="22px">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblPobSize" Text='<%# DataBinder.Eval(Container.DataItem, "TyreSize") %>'></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Width="70px">
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtPobSizeValue" Text="0" onkeypress="return isNumberKey(event)"
                                                        MaxLength="7" CssClass="txtSizeNum"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <div class="sizeChildDiv" style="visibility: hidden;">
                                    <asp:GridView runat="server" ID="gv_PneumaticSize" AutoGenerateColumns="false" AlternatingRowStyle-BackColor="#f5f5f5">
                                        <HeaderStyle CssClass="gvSizeVisible" />
                                        <Columns>
                                            <asp:TemplateField ItemStyle-Width="150px" ItemStyle-Height="22px">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblPneumaticSize" Text='<%# DataBinder.Eval(Container.DataItem, "TyreSize") %>'></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Width="70px">
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtPneumaticSizeValue" Text="0" onkeypress="return isNumberKey(event)"
                                                        MaxLength="7" CssClass="txtSizeNum"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                            <div style="width: 190px; float: left; padding-left: 6px; height: 500px; overflow: scroll;">
                                <asp:GridView runat="server" ID="gv_TypeRates" AutoGenerateColumns="false" AlternatingRowStyle-BackColor="#f5f5f5"
                                    HeaderStyle-BackColor="#fefe8b" RowStyle-BackColor="#f0ffff">
                                    <HeaderStyle CssClass="gvSizeVisible" />
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="70px" ItemStyle-Height="22px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblType" Text='<%# DataBinder.Eval(Container.DataItem, "type") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Middle">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblTypeRatesVal" Text="0"></asp:Label></ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td class="ratesMastedTD">
                    <div style="border: 1px solid #000; width: 920px; float: left; padding: 5px; background-color: #F3F1D7;">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel2" ClientIDMode="Static" UpdateMode="Always">
                            <ContentTemplate>
                                <div style="width: 800px; float: right; margin-right: 15px; padding-left: 30px;">
                                    <asp:Button runat="server" ID="btnAdd" ClientIDMode="Static" Text="SAVE NEW RATES"
                                        CssClass="btnsave" OnClick="btnAdd_Click" OnClientClick="javascript:return ctrlRatesMaster();" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button runat="server"
                                        ID="btnEdit" ClientIDMode="Static" Text="UPDATE EXISTS RATES" CssClass="btnedit"
                                        OnClick="btnEdit_Click" OnClientClick="javascript:return ctrlRatesMaster();" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a id="btnClear" class="btnclear">CLEAR</a></div>
                                <div style="width: 800px; float: left; padding-left: 25px;">
                                    <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" ForeColor="Red"></asp:Label></div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            //$('#txtRatesID').focus();

            liPossition = 0;
            $('#txtRatesID').keyup(function (e) {
                if ((e.keyCode == 40 || e.which == 40) || (e.keyCode == 38 || e.which == 38)) { // down arrow key code 
                    upanddownKey(e, 'popup_box_rates');
                } else if (e.keyCode == 13 || e.which == 13) { // enter key code 
                    popupEnterKey('popup_box_rates', 'txtRatesID');
                    if ($('#txtRatesID').val().length > 0) {
                        get_RatesDetails();
                    }
                    $('#txtIndRate').focus();
                }
                else {
                    var rid = $('#txtRatesID').val();
                    if (rid.length > 0) {
                        $.ajax({ type: "POST", url: "BindRecords.aspx?type=getRatesID&rid=" + rid + "", context: document.body,
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
                    } else {
                        $('#popup_box_rates').html('');
                        $('#popup_box_rates').hide();
                    }
                    liPossition = 0;
                }
            });

            $('#popup_box_rates').hover(function () {
                popupHover('popup_box_rates', 'txtRatesID');
                if ($('#txtRatesID').val().length > 0 && $('#popup_box_rates').css("display") == "none") {
                    get_RatesDetails();
                }
                $('#txtIndRate').focus();
            });

            $('#ddlCurrency').change(function () {
                var strCurType = $("#ddlCurrency option:selected").text();
                $('#txtCurRate').val(strCurType.substr(0, 3).toUpperCase());
                var ratesid = $('#txtRatesID').val();
                if (ratesid.length > 0) {
                    getCurrency_ChangeValue(ratesid, $('#txtCurRate').val());
                }
            });

            $('#btnClear').click(function () {
                $('#txtRatesID').val('');
                $('#txtIndRate').val('');
                $('#txtConvCost').val('');
                $('#txtLoatFactor').val('');
                $('#ddlCurrency').val('INR India, Rupees');
                $('#txtCurRate').val('INR');
                var j;
                var txtLipCost = $("input[id*='MainContent_gv_Lip_txtLipCost']");
                for (j = 0; j < txtLipCost.length; j++) {
                    $('#MainContent_gv_Lip_txtLipCost_' + j).val('0');
                }

                var txtBaseCost = $("input[id*='MainContent_gv_Base_txtBaseCost']");
                for (j = 0; j < txtBaseCost.length; j++) {
                    $('#MainContent_gv_Base_txtBaseCost_' + j).val('0');
                }

                var txtCenterCost = $("input[id*='MainContent_gv_Center_txtCenterCost']");
                for (j = 0; j < txtCenterCost.length; j++) {
                    $('#MainContent_gv_Center_txtCenterCost_' + j).val('0');
                }

                var txtTreadCost = $("input[id*='MainContent_gv_Tread_txtTreadCost']");
                for (j = 0; j < txtTreadCost.length; j++) {
                    $('#MainContent_gv_Tread_txtTreadCost_' + j).val('0');
                }

                var txtTypeValue = $("span[id*='MainContent_gv_TypeRates_lblTypeRatesVal']");
                for (var k = 0; k < txtTypeValue.length; k++) {
                    $('#MainContent_gv_TypeRates_lblTypeRatesVal_' + k).html('0');
                }

                var txtSolidSizeValue = $("input[id*='MainContent_gv_SolidSize_txtSolidSizeValue']");
                for (var k = 0; k < txtSolidSizeValue.length; k++) {
                    $('#MainContent_gv_SolidSize_txtSolidSizeValue_' + k).val('0');
                }

                var txtPobSizeValue = $("input[id*='MainContent_gv_PobSize_txtPobSizeValue']");
                for (var k = 0; k < txtPobSizeValue.length; k++) {
                    $('#MainContent_gv_PobSize_txtPobSizeValue_' + k).val('0');
                }

                var txtPneumaticSizeValue = $("input[id*='MainContent_gv_PneumaticSize_txtPneumaticSizeValue']");
                for (var k = 0; k < txtPneumaticSizeValue.length; k++) {
                    $('#MainContent_gv_PneumaticSize_txtPneumaticSizeValue_' + k).val('0');
                }
            });

            $('.dropdwonRatesCss').click(function () {
                loadRatesID('popup_box_rates');
                loadPopupBox('popup_box_rates', 'txtRatesID');
            });

        });

        $('body').click(function (e) {
            if ($('#popup_box_rates').is(':visible') == true && e.target.className != "dropdwonRatesCss")
                unloadPopupBox('popup_box_rates');
        });

        function get_RatesDetails() {
            showProgress();
            $('#lblErrMsg').html('');
            $('#ErrMsg').html('');
            getLipCost($('#txtRatesID').val(), "lipgum");
            getBaseCost($('#txtRatesID').val(), "base");
            getCenterCost($('#txtRatesID').val(), "center");
            getTreadCost($('#txtRatesID').val(), "tread");
            getSolidSizeList($('#txtRatesID').val(), "Solid");
            getPobSizeList($('#txtRatesID').val(), "Pob");
            getPneumaticSizeList($('#txtRatesID').val(), "Pneumatic");
            getTypeList($('#txtRatesID').val());
            getCurCostList($('#txtRatesID').val());
            unloadPopupBox('popup_box_rates');
            //hideProgress();
            $('#progress').delay(5000).fadeOut("slow");
        }
    </script>
</asp:Content>
