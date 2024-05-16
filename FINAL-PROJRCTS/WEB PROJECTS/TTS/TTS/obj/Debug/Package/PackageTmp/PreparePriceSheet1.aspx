<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="PreparePriceSheet1.aspx.cs" Inherits="TTS.PreparePriceSheet1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/datemonthyear.css" rel="stylesheet" type="text/css" />
    <link href="Styles/popupBox.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .ddl
        {
            height: 22px;
            width: 360px;
        }
        .tooltip
        {
            color:Red;
            padding-left:20px;
            display:none;
        }
        
        #lblPrevtRatesID:hover + .tooltip
        {
            display:block;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="1200">
    </asp:ScriptManager>
    <div align="center" class="pageTitleHead">
        PREPARE PRICE SHEET</div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <div style="padding-left: 10px;">
            <table>
                <tr>
                    <td style="width: 100%;">
                        <div style="width: 935px; float: left; color: Red;" id="bindErrmsg">
                        </div>
                        <div style="width: 510px; float: left; line-height: 35px; padding-top: 10px;">
                            <div style="float: left; width: 500px;">
                                <div style="width: 105px; float: left;">
                                    <span class="headCss">Customer Name</span>
                                </div>
                                <div style="width: 385px; float: left; line-height: 28px;">
                                    <asp:DropDownList runat="server" ID="ddlCustName" CssClass="ddl" OnSelectedIndexChanged="ddlCustName_SelectedIndexChanged"
                                        AutoPostBack="true" ClientIDMode="Static">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div style="float: left; width: 500px;">
                                <div style="width: 105px; float: left;">
                                    <span class="headCss">Price Ref No.</span>
                                </div>
                                <div style="width: 385px; float: left; line-height: 28px;">
                                    <asp:DropDownList runat="server" ID="ddlPriceRef" CssClass="ddl" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlPriceRef_SelectedIndexChanged" ClientIDMode="Static">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div style="float: left; width: 500px;">
                                <div style="width: 105px; float: left;">
                                    <span class="headCss">Rates-ID</span>
                                </div>
                                <div style="width: 385px; float: left; line-height: 28px;">
                                    <asp:DropDownList runat="server" ID="ddlRatesId" CssClass="ddl" ClientIDMode="Static"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlRatesId_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:Label runat="server" ID="lblPrevtRatesID" ClientIDMode="static" Width="360px"
                                        Height="22px" style="border:1px solid rgba(0,0,0,0.30);" tool-tip="Previous Rates Id" Visible="false" ></asp:Label> <br />
                                        <span class="tooltip"> Previous Rates Id</span>
                                </div>
                            </div>
                            <div style="width: 105px; float: left;">
                                <span class="headCss">Category</span>
                            </div>
                            <div style="width: 385px; float: left;" id="categoryDiv">
                                <div style="width: 100px; float: left;" id="rdb0">
                                    <input type="radio" name="category" value="Solid" />SOLID</div>
                                <div style="width: 100px; float: left;" id="rdb1">
                                    <input type="radio" name="category" value="Pob" />POB</div>
                                <div style="width: 120px; float: left; display: none;" id="rdb2">
                                    <input type="radio" name="category" value="Pneumatic" />PNEUMATIC</div>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                                    <ContentTemplate>
                                        <asp:Button ID="btnPriceShow" runat="server" Text="SHOW" ClientIDMode="Static" CssClass="btnshow"
                                            OnClientClick="javascript:return PriceMainCtrlValidation();" OnClick="btnPriceShow_Click" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div style="width: 425px; float: left; line-height: 20px; padding-top: 5px; padding-left: 5px;">
                            <span class="headCss">Special Instruction:</span>
                            <asp:TextBox runat="server" ID="txtCustPriceSpl" ClientIDMode="Static" TextMode="MultiLine"
                                Width="415px" Height="120px"></asp:TextBox>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%; display: none;" id="tdPremium">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <div style="border: 1px solid #ccc; width: 500px; float: left; margin-right: 10px;
                                    height: 260px;">
                                    <div style="width: 500px; float: left; text-align: center; font-weight: bold; background-color: #FEFE8B;">
                                        <span style="width: 91px; float: left; border: 1px solid #000;">PLATFORM</span>
                                        <span style="width: 92px; float: left; border: 1px solid #000;">PREMIUM</span> <span
                                            style="width: 97px; float: left; border: 1px solid #000;">VALUE</span> <span style="width: 94px;
                                                float: left; border: 1px solid #000;">BASE</span> <span style="width: 96px; float: left;
                                                    border: 1px solid #000;">VALUE</span>
                                    </div>
                                    <div style="width: 500px; float: left; overflow: scroll; height: 200px;">
                                        <asp:GridView runat="server" ID="gv_PremiumValue" AutoGenerateColumns="false" AlternatingRowStyle-BackColor="#f5f5f5">
                                            <HeaderStyle CssClass="gvSizeVisible" />
                                            <Columns>
                                                <asp:TemplateField ItemStyle-Width="97px" ItemStyle-Height="22px">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblPlatform" Text='<%#Eval("Config") %>'></asp:Label></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="97px">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblPremium" Text='<%#Eval("PremiumType") %>'></asp:Label></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="97px">
                                                    <ItemTemplate>
                                                        <asp:TextBox runat="server" ID="txtPreValue" Text='<%#Eval("PremiumValue") %>' onkeypress="return isNumberAndMinusKey(event)"
                                                            MaxLength="8" CssClass="txtPreparePriceNum"></asp:TextBox></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="97px">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblBase" Text='<%#Eval("BaseType") %>'></asp:Label></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="97px">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblBaseValue" Text='<%#Eval("BaseRmcb") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div style="width: 520px; float: right; text-align: center; padding-top: 5px;">
                                        <asp:Button runat="server" ID="btnCalc" ClientIDMode="Static" Text="CALCULATE FOR PREMIUM VALUE CHANGES"
                                            CssClass="btncalc" OnClientClick="javascript:return PriceMainCtrlValidation();"
                                            OnClick="btnCalc_Click" /></div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div style="width: 425px; float: left; line-height: 30px;">
                            <div style="width: 85px; text-align: center; float: left; padding-right: 10px; border-bottom: 1px solid #ccc;
                                border-left: 1px solid #ccc; border-top: 1px solid #ccc; padding-bottom: 8px;
                                padding-left: 4px;">
                                <div style="width: 85px; float: left;">
                                    <span class="headCss">Currency</span>
                                </div>
                                <div style="width: 85px; float: left;">
                                    <asp:TextBox runat="server" ID="txtCurType" CssClass="txtID" ClientIDMode="Static"
                                        Width="85px" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Always">
                                <ContentTemplate>
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
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <div style="width: 420px; float: left; margin-top: 15px; border: 1px solid #ccc;
                                        text-align: center;">
                                        <div style="font-size: 18px; font-weight: bold;">
                                            <span class="headCss" style="line-height: 20px; padding-right: 10px;">General Increase
                                                %</span>
                                            <asp:TextBox runat="server" ID="txtGenIncr" ClientIDMode="Static" CssClass="txtID"
                                                Width="70px" onkeypress="return isNumberAndMinusKey(event)" MaxLength="8" Font-Bold="true"></asp:TextBox>
                                        </div>
                                        <div style="color: #5b14d8; font-weight: bold;">
                                            <asp:Button runat="server" ID="btnSTDPrice" ClientIDMode="Static" OnClick="rdbUnitPrice_Click"
                                                Text="CHANGE STD PRICE" OnClientClick="javascript:return PriceIncrement(this);"
                                                CssClass="stdpricebtn" Font-Bold="true" />
                                            <asp:Button runat="server" ID="btnNEWPrice" ClientIDMode="Static" OnClick="rdbUnitPrice_Click"
                                                Text="CHANGE NEW PRICE" OnClientClick="javascript:return PriceIncrement(this);"
                                                CssClass="newpricebtn" Font-Bold="true" />
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <div style="float: left; width: 420px; line-height: 30px; margin-top: 10px; border: 1px solid #ccc;
                                text-align: center;">
                                <div style="width: 420px; float: left; padding-top: 5px;">
                                    <a class="btnactive" style="line-height: 20px; width: 300px; text-align: center;"
                                        id="btnEdit">EDIT EACH SIZE UNITPRICE / RMCB</a>
                                </div>
                                <div id="divOptEdit" style="float: left; width: 290px; display: none; padding-left: 130px;">
                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                            <asp:RadioButtonList runat="server" ID="rdbEditPrice" OnSelectedIndexChanged="rdbEditPrice_IndexChanged"
                                                RepeatDirection="Horizontal" AutoPostBack="true">
                                                <asp:ListItem Value="UNITPRICE" Text="UNITPRICE"></asp:ListItem>
                                                <asp:ListItem Value="RMCB" Text="RMCB"> </asp:ListItem>
                                            </asp:RadioButtonList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <div style="width: 420px; float: left; line-height: 30px; margin-top: 10px; border: 1px solid #ccc;">
                                        <div style="width: 420px; float: left; text-align: center;">
                                            <div style="width: 420px; float: left;">
                                                <asp:Button runat="server" ID="btnCalcRmcb" ClientIDMode="Static" Text="CALCULATE FOR UNITPRICE/RMCB CHANGES"
                                                    CssClass="btncalc" OnClientClick="javascript:return PriceMainCtrlValidation();"
                                                    OnClick="btnCalcRmcb_Click" /></div>
                                        </div>
                                        <div style="width: 420px; float: left; display: none;">
                                            <span class="headCss" style="width: 74px; float: left;">OLD RMCB</span>
                                            <div style="width: 54px; float: left;">
                                                <asp:TextBox runat="server" ID="txtOldRmcb" ClientIDMode="Static" CssClass="txtID"
                                                    Width="50px"></asp:TextBox></div>
                                            <span class="headCss" style="width: 74px; float: left;">NEW RMCB</span>
                                            <asp:TextBox runat="server" ID="txtNewRmcb" ClientIDMode="Static" CssClass="txtID"
                                                Width="50px"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div style="width: 420px; float: left; margin-top: 10px; color: #ff0000; font-weight: bold;"
                                        id="divErrmsg" runat="server" clientidmode="Static">
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%; padding-top: 10px; display: none;" id="tdPrice">
                        <div style="width: 940px; float: left;">
                            <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse;">
                                <tbody>
                                </tbody>
                            </table>
                        </div>
                        <div style="width: 940px; float: left; overflow: scroll; height: 500px;" id="divPriceGrid">
                            <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <asp:GridView runat="server" ID="gv_PriceGrid" AutoGenerateColumns="false" RowStyle-Height="24px"
                                        RowStyle-Width="935px">
                                        <HeaderStyle CssClass="priceGvCss" />
                                        <Columns>
                                            <asp:BoundField DataField="Config" HeaderText="PLATFORM" ItemStyle-Width="100px" />
                                            <asp:BoundField DataField="Size" HeaderText="SIZE" ItemStyle-Width="130px" />
                                            <asp:BoundField DataField="Rim" HeaderText="RIM" ItemStyle-Width="50px" />
                                            <asp:BoundField DataField="Type" HeaderText="TYPE" ItemStyle-Width="80px" />
                                            <asp:BoundField DataField="Brand" HeaderText="BRAND" ItemStyle-Width="90px" />
                                            <asp:BoundField DataField="Sidewall" HeaderText="SIDEWALL" ItemStyle-Width="90px" />
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    QTY</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtQty" Text="0" onkeypress="return isNumberKey(event)"
                                                        MaxLength="5" Width="45px" CssClass="txtPreparePriceNum"></asp:TextBox></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Price1" HeaderText="OLD PRICE" ItemStyle-CssClass="txtOldPrice" />
                                            <asp:BoundField DataField="Arv1" HeaderText="ARV" ItemStyle-CssClass="txtOldPrice" />
                                            <asp:BoundField DataField="Rmc1" HeaderText="RMC" ItemStyle-CssClass="txtOldPrice" />
                                            <asp:BoundField DataField="Rmcb1" HeaderText="OLD RMCB" ItemStyle-CssClass="txtOldPrice" />
                                            <asp:BoundField DataField="Price2" HeaderText="NEW PRICE" ItemStyle-CssClass="txtNewPrice" />
                                            <asp:BoundField DataField="Arv2" HeaderText="ARV" ItemStyle-CssClass="txtNewPrice" />
                                            <asp:BoundField DataField="Rmc2" HeaderText="RMC" ItemStyle-CssClass="txtNewPrice" />
                                            <asp:BoundField DataField="Rmcb2" HeaderText="NEW RMCB" ItemStyle-CssClass="txtNewPrice" />
                                            <asp:BoundField DataField="CalcValue" ItemStyle-CssClass="calcField" HeaderStyle-CssClass="calcField" />
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    EDIT PRICE</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtEditUnitPrice" Text="" onkeypress="return isNumberAndMinusKey(event)"
                                                        MaxLength="10" Width="75px" CssClass="txtPreparePriceNum" Visible="false" onkeyup="changeValUnitPrice(this)"
                                                        onfocus="changeRowBg(this);" onblur="reverseRowBg(this)"></asp:TextBox></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    EDIT RMCB</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtEditRMCB" Text="" onkeypress="return isNumberAndMinusKey(event)"
                                                        MaxLength="10" Width="75px" CssClass="txtPreparePriceNum" Visible="false" onkeyup="changeValRMCB(this)"
                                                        onfocus="changeRowBg(this);" onblur="reverseRowBg(this)"></asp:TextBox></ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%; display: none;" id="tdFinal">
                        <div style="float: left; margin-top: 10px; width: 345px; border: 1px solid #ccc;
                            padding: 5px; margin-right: 20px; background-color: #a0cda0;">
                            <div style="width: 110px; float: left; padding-left: 3px;">
                                    <a id="btnSave" class="btnsave" style="width: 60px; text-align: center;">SAVE</a>
                            </div>
                            <div style="width: 130px; float: left;">
                                <a id="btnAuthorize" class="btnauthorize" style="text-align: center;">AUTHORIZE</a>
                            </div>
                            <div style="width: 100px; float: left;">
                                <a class="btnclear" style="line-height: 20px; width: 75px; text-align: center;" id="btnPricePrepareClear">
                                    CLEAR</a></div>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <div style="float: right; width: 125px; display: none;">
                                        <asp:Button runat="server" ID="btnEditCalc" ClientIDMode="Static" OnClick="btnEditCalc_Click"
                                            Text="" /></div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div style="float: left; width: 550px; line-height: 30px; margin-top: 10px; border: 1px solid #ccc;
                            padding: 5px; background-color: #E6D7DE; display: none;" id="divSaveTxt">
                            <span class="headCss" style="float: left; width: 110px; line-height: 15px;">Price RefNo.</span>
                            <div style="float: left; width: 390px;">
                                <asp:TextBox runat="server" ID="txtSaveRefNo" ClientIDMode="Static" CssClass="txtID"
                                    Width="365px"></asp:TextBox></div>
                            <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <div style="width: 550px; float: left; color: #ff0000; font-weight: bold; line-height: 15px;"
                                        id="divSaveErrMsg" runat="server" clientidmode="Static">
                                    </div>
                                    <asp:Button runat="server" ID="btnOK" ClientIDMode="Static" CssClass="btnedit" Text="OK"
                                        OnClientClick="javascript:return PriceSaveCtrlValidation();" OnClick="btnRecordsSave_Click" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdnCustCode" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnPreparePreValue" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnCategory" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnIncrType" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnPriceEditType" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnPremiumValue" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnBaseValue" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnRecordSaveType" ClientIDMode="Static" Value="" />
    <script src="Scripts/datemonthyear.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //$('#txtCustName').focus();

            $('#txtCustPriceSpl').blur(function () {
                if ($(this).val().trim().length == 0) {
                    $(this).val('Enter Special Instructions').css({ "color": '#2E2D2D', "border-color": '#CCCCCC' });
                }
            }).focus(function () {
                if ($(this).val().trim().toLowerCase() == "enter special instructions") {
                    $(this).val('').css('color', '#333333');
                }
            })
            $('#txtCustPriceSpl').trigger("blur");

            liPossition = 0;
            //Customer Textbox code
//            $('#txtCustName').keyup(function (e) {
//                $('#bindErrmsg').html('');
//                if ((e.keyCode == 40 || e.which == 40) || (e.keyCode == 38 || e.which == 38)) { // down arrow key code 
//                    upanddownKey(e, 'popup_box_cust');
//                } else if (e.keyCode == 13 || e.which == 13) { // enter key code 
//                    popupEnterKey('popup_box_cust', 'txtCustName');
//                    if ($('#txtCustName').val().length > 0) {
//                        getCustDetails($('#txtCustName').val());
//                        $('#txtPriceRefNo').focus();
//                    }
//                }
//                else {
//                    var cname = $(this).val();
//                    if (cname.length > 0) {
//                        //                        $.ajax({ type: "POST", url: "UserValidation.aspx?type=getcust&cname=" + cname + "", context: document.body,
//                        //                            success: function (data) {
//                        //                                if (data != '') {
//                        //                                    $('#popup_box_cust').html(data); $("div[id*='popup_box_cust'] ul li").first().addClass('current'); loadPopupBox('popup_box_cust', 'txtCustName');
//                        //                                }
//                        //                                else {
//                        //                                    $('#popup_box_cust').html(''); $('#popup_box_cust').hide();
//                        //                                }
//                        //                            }
//                        //                        });
//                    }
//                    else {
//                        $('#popup_box_cust').html(''); $('#popup_box_cust').hide();
//                    }
//                    liPossition = 0;
//                }
//            });

//            $('#popup_box_cust').hover(function () {
//                popupHover('popup_box_cust', 'txtCustName');
//                if ($('#txtCustName').val().length > 0) {
//                    getCustDetails($('#txtCustName').val());
//                    $('#txtPriceRefNo').focus();
//                }
//            });

            //PriceSheet RefNo Textbox Code
//            $('#txtPriceRefNo').keyup(function (e) {
//                $('#bindErrmsg').html('');
//                if ((e.keyCode == 40 || e.which == 40) || (e.keyCode == 38 || e.which == 38)) { // down arrow key code 
//                    upanddownKey(e, 'popup_box_price');
//                } else if (e.keyCode == 13 || e.which == 13) { // enter key code 
//                    popupEnterKey('popup_box_price', 'txtPriceRefNo');
//                    $('#txtRatesID').focus();
//                }
//                else {
//                    var ccode = $('#hdnCustCode').val();
//                    var priceref = $('#txtPriceRefNo').val();
//                    if (ccode.length > 0) {
//                        //                        $.ajax({ type: "POST", url: "BindRecords.aspx?type=getPriceRef&cCode=" + ccode + "&priceref=" + priceref + "", context: document.body,
//                        //                            success: function (data) {
//                        //                                if (data != '') {
//                        //                                    $('#popup_box_price').html(data); $("div[id*='popup_box_price'] ul li").first().addClass('current'); loadPopupBox('popup_box_price', 'txtPriceRefNo');
//                        //                                }
//                        //                                else {
//                        //                                    $('#popup_box_price').html(''); $('#popup_box_price').hide();
//                        //                                }
//                        //                            }
//                        //                        });
//                    }
//                    else {
//                        $('#popup_box_price').html(''); $('#popup_box_price').hide();
//                    }
//                    liPossition = 0;
//                }
//            });

//            $('#popup_box_price').hover(function () {
//                popupHover('popup_box_price', 'txtPriceRefNo');
//                $('#txtRatesID').focus();
//            });

            //Rates-ID Textbox code
//            $('#txtRatesID').keyup(function (e) {
//                $('#bindErrmsg').html('');
//                if ((e.keyCode == 40 || e.which == 40) || (e.keyCode == 38 || e.which == 38)) { // down arrow key code 
//                    upanddownKey(e, 'popup_box_rates');
//                } else if (e.keyCode == 13 || e.which == 13) { // enter key code 
//                    popupEnterKey('popup_box_rates', 'txtRatesID');
//                    $('#ddlPlatform').focus();
//                }
//                else {
//                    var ccode = $('#hdnCustCode').val();
//                    var ratesid = $(this).val();
//                    if (ratesid.length > 0 && ccode.length > 0) {
//                        //                        $.ajax({ type: "POST", url: "BindRecords.aspx?type=getRatesID&rid=" + ratesid + "", context: document.body,
//                        //                            success: function (data) {
//                        //                                if (data != '') {
//                        //                                    $('#popup_box_rates').html(data); $("div[id*='popup_box_rates'] ul li").first().addClass('current'); loadPopupBox('popup_box_rates', 'txtRatesID');
//                        //                                }
//                        //                                else {
//                        //                                    $('#popup_box_rates').html(''); $('#popup_box_rates').hide();
//                        //                                }
//                        //                            }
//                        //                        });
//                    }
//                    else {
//                        $('#popup_box_rates').html(''); $('#popup_box_rates').hide();
//                    }
//                    liPossition = 0;
//                }
//            });

//            $('#popup_box_rates').hover(function () {
//                popupHover('popup_box_rates', 'txtRatesID');
//                $('#ddlPlatform').focus();
//            });

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
                if (e.target.className == "txtID" || e.target.className == "dropDownID" || e.target.className == "txtPreparePriceNum") {
                    if (e.keyCode == 13) { //if this is enter key
                        e.preventDefault();
                        return false;
                    }
                }
            });

            $("input:radio[name=category]").click(function () {
                $('#hdnCategory').val($(this).val());
                $('#tdPremium').css({ "display": "none" }); $('#tdPrice').css({ "display": "none" }); $('#tdFinal').css({ "display": "none" });
            });

            $('#btnPricePrepareClear').click(function () {
                $('#txtCustName').val(''); $('#txtPriceRefNo').val(''); $('#txtRatesID').val(''); $('#txtCustPriceSpl').val('');
                $('#txtCurType').val(''); $('#txtGenIncr').val(''); $('#txtOldRmcb').val(''); $('#txtNewRmcb').val('');
                $("input:radio[name=category]").attr('checked', false); $("input:radio[id*=rdbUnitPrice_]").attr('checked', false);
                $('#tdPremium').css({ "display": "none" }); $('#tdPrice').css({ "display": "none" }); $('#tdFinal').css({ "display": "none" });
                $('#divSaveTxt').css({ "display": "none" });
            });

            $('#btnEdit').click(function () {
                PriceMainCtrlValidation();
                if ($('#bindErrmsg').html().length == 0) {
                    $("input:radio[id*=MainContent_rdbEditPrice_]").attr('checked', false); $('#divOptEdit').show(); priceGvEditColumnHide(); hideProgress();
                }
            });

            $('#btnSave').click(function () {
                $('#divSaveTxt').fadeIn(100); $('#txtSaveRefNo').val($('#txtPriceRefNo').val()); $('#hdnRecordSaveType').val('SAVE');
                $('#divSaveTxt').find('.headCss').html('Save Price Sheet Ref No.')
            });

            $('#btnAuthorize').click(function () {
                $('#divSaveTxt').fadeIn(100); $('#txtSaveRefNo').val($('#txtPriceRefNo').val()); $('#hdnRecordSaveType').val('AUTHORIZE');
                $('#divSaveTxt').find('.headCss').html('Authorize Price Sheet Ref No.')
            });

            $('.dropdwonCustCss').click(function () {
                loadCustName('popup_box_cust');
                loadPopupBox('popup_box_cust', 'txtCustName');
            });

            $('.dropdwonPriceCss').click(function () {
                $('#bindErrmsg').html('');
                if ($('#txtCustName').val().length > 0) {
                    loadPriceSheetRefNo('popup_box_price', $('#hdnCustCode').val());
                    loadPopupBox('popup_box_price', 'txtPriceRefNo');
                }
                else {
                    $('#bindErrmsg').html('Enter Customer Name');
                }
            });

            $('.dropdwonRatesCss').click(function () {
                $('#bindErrmsg').html('');
                var errRatemsg = '';
                if ($('#txtCustName').val().length == 0) {
                    errRatemsg += "Enter Customer Name <br />";
                }
                if ($('#txtPriceRefNo').val().length == 0) {
                    errRatemsg += "Enter Price Sheet Ref No.";
                }
                if (errRatemsg.length > 0) {
                    $('#bindErrmsg').html(errRatemsg);
                }
                if ($('#bindErrmsg').html().length == 0) {
                    loadRatesID('popup_box_rates');
                    loadPopupBox('popup_box_rates', 'txtRatesID');
                }
            });

//            $('#divPriceGrid').scroll(function () {
//                if ($(this).scrollTop() > 0) {
//                    if ($('#headVal').html().trim().length == 0) {
//                        PriceGridHeadFixed();
//                    }
//                }
//                else if ($(this).scrollTop() == 0) {
//                    $('#headVal').html('');
//                }
//            });

        });

        $('body').click(function (e) {
            if ($('#popup_box_cust').is(':visible') == true && e.target.className != "dropdwonCustCss")
                unloadPopupBox('popup_box_cust');
            if ($('#popup_box_price').is(':visible') == true && e.target.className != "dropdwonPriceCss")
                unloadPopupBox('popup_box_price');
            if ($('#popup_box_rates').is(':visible') == true && e.target.className != "dropdwonRatesCss")
                unloadPopupBox('popup_box_rates');
        });

        function changeRowBg(e) {
            var ctrltxtID = e.id;
            $('#' + ctrltxtID).parent("td").parent("tr").css({ 'background-color': '#FAC641' });
            $('#' + ctrltxtID).parent("td").parent("tr").find("td").eq(1).css({ 'font-weight': 'bold' });
            $('#' + ctrltxtID).parent("td").parent("tr").find("td").eq(2).css({ 'font-weight': 'bold' });
        }

        function reverseRowBg(e) {
            var ctrltxtID = e.id;
            $('#' + ctrltxtID).parent("td").parent("tr").css({ 'background-color': '#fff' });
            $('#' + ctrltxtID).parent("td").parent("tr").find("td").eq(1).css({ 'font-weight': 'normal' });
            $('#' + ctrltxtID).parent("td").parent("tr").find("td").eq(2).css({ 'font-weight': 'normal' });
        }

        function getCustDetails(strCustName) {
//            $.ajax({ type: "POST", url: "PreparePriceSheet.aspx/get_CustDetails_WebMethod", data: '{strCustName:"' + strCustName + '"}', contentType: "application/json; charset=utf-8",
//                dataType: "json", success: OnSuccessCust,
//                failure: function (response) {
//                    alert(response.d);
//                },
//                error: function (response) {
//                    alert(response.d);
//                }
//            });
        }

        function OnSuccessCust(response) {
            $('#txtPriceRefNo').val(''); $('#txtRatesID').val('');
            var xmlDoc = $.parseXML(response.d); var xml = $(xmlDoc); var listVals = xml.find("T1");
            if ($(listVals[0]).find("Custcode").text() != "") {
                $('#hdnCustCode').val($(listVals[0]).find("Custcode").text());
                $('#txtCurType').val($(listVals[0]).find("PriceUnit").text().substr(0, 3).toUpperCase());
                $('#txtCustPriceSpl').val($(listVals[0]).find("specialinstruction").text());

                $('#rdb0').css({ "display": "none" }); $('#rdb1').css({ "display": "none" }); $('#rdb2').css({ "display": "none" });
                $("input:radio[name=category]").attr('checked', false);
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

        function getPreparePriceDateDetails() {
            $.ajax({ type: "POST", url: "PreparePriceSheet.aspx/get_PriceDateDetails_WebMethod", data: '{strCustCode:"' + $('#hdnCustCode').val() + '",strPriceNo:"' + $('#txtPriceRefNo').val() + '",strCategory:"' + $('#hdnCategory').val() + '"}', contentType: "application/json; charset=utf-8",
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

        function PriceMainCtrlValidation() {
            showProgress();
            $('#txtGenIncr').val(''); $('#txtOldRmcb').val(''); $('#txtNewRmcb').val(); $('#divErrmsg').html(''); $('#bindErrmsg').html('');
            var errmsg = '';
//            if ($('#txtCustName').val().length == 0)
//                errmsg += "Enter Customer Name. <br />";
//            if ($('#hdnCustCode').val().length == 0)
//                errmsg += "Customer Name Invalid. <br />";
//            if ($('#txtPriceRefNo').val().length == 0)
//                errmsg += "Enter Price Sheet Ref No. <br />";
//            if ($('#txtRatesID').val().length == 0)
//                errmsg += "Enter Rates-ID. <br />";
            if ($('#hdnCategory').val().length == 0)
                errmsg += "Select Category. <br />";

            if (errmsg.length > 0) {
                $('#bindErrmsg').html(errmsg); hideProgress(); return false;
            }
            else {
                $('#tdPremium').css({ "display": "block" }); $('#tdPrice').css({ "display": "block" }); $('#tdFinal').css({ "display": "block" }); return true;
            }
        }

        function ctrlPreparePriceMaster() {
            var concatVal = '';
            var gv_PreValue = $("input[id*='txtPreValue']");
            for (j = 0; j < gv_PreValue.length; j++) {
                if (concatVal.length > 0)
                    concatVal += "~" + $('#lblPlatform_' + j).html() + "," + $('#lblPremium_' + j).html() + "," + $('#txtPreValue_' + j).val() + "," + $('#lblBase_' + j).html() + "," + $('#lblBaseValue_' + j).html();
                else
                    concatVal = $('#lblPlatform_' + j).html() + "," + $('#lblPremium_' + j).html() + "," + $('#txtPreValue_' + j).val() + "," + $('#lblBase_' + j).html() + "," + $('#lblBaseValue_' + j).html();
            }
            $('#hdnPreparePreValue').val(concatVal);
        }

        function PriceIncrement(e) {
            showProgress();
            $('#divErrmsg').html('');
            $('#hdnIncrType').val(e.value);
            if ($('#txtGenIncr').val().length == 0) {
                $('#divErrmsg').html('Enter proper increment value.'); hideProgress();
                return false;
            }
            else {
                return true;
            }
        }

        function hideDivOptEdit() {
            $('#divOptEdit').hide();
        }

        function changeValRMCB(e) {
            var ctrlRMCBID = e.id;
            if ($('#' + ctrlRMCBID).val().length > 0) {
                var calcVal = $('#' + ctrlRMCBID).parent("td").parent("tr").find("td").eq(15).html();
                if (calcVal.length > 0) {
                    var splitVal = calcVal.split('~');
                    if (splitVal.length == 6) {
                        var strUPrice = ''; var strArv = ''; var strRmc = '';
                        $('#' + ctrlRMCBID).parent("td").parent("tr").find("td").eq(14).html($('#' + ctrlRMCBID).val());
                        if (splitVal[0].toString().toLowerCase() == "true") {
                            //unitprice
                            strUPrice = ((parseFloat($('#' + ctrlRMCBID).val()) + (parseFloat(splitVal[1].toString()) + (parseFloat(splitVal[3].toString()) * parseFloat(splitVal[2].toString())) / parseFloat(splitVal[4].toString()))) * parseFloat(splitVal[4].toString())) / parseFloat(splitVal[5].toString());
                            $('#' + ctrlRMCBID).parent("td").parent("tr").find("td").eq(11).html(strUPrice.toFixed(2));
                            //Arv value
                            strArv = (parseFloat(strUPrice.toFixed(2)) * parseFloat(splitVal[5].toString())) / parseFloat(splitVal[4].toString());
                            $('#' + ctrlRMCBID).parent("td").parent("tr").find("td").eq(12).html(strArv.toFixed(2));
                            //Rmc Value
                            strRmc = parseFloat(splitVal[1].toString()) + (parseFloat(splitVal[3].toString()) * parseFloat(splitVal[2].toString())) / parseFloat(splitVal[4].toString());
                            $('#' + ctrlRMCBID).parent("td").parent("tr").find("td").eq(13).html(strRmc.toFixed(2));
                        }
                        else if (splitVal[0].toString().toLowerCase() == "false") {
                            //unitprice
                            strUPrice = ((parseFloat($('#' + ctrlRMCBID).val()) + (parseFloat(splitVal[1].toString()) + (0 * parseFloat(splitVal[2].toString())) / parseFloat(splitVal[4].toString()))) * parseFloat(splitVal[4].toString())) / parseFloat(splitVal[5].toString());
                            $('#' + ctrlRMCBID).parent("td").parent("tr").find("td").eq(11).html(strUPrice.toFixed(2));
                            //Arv value
                            strArv = (parseFloat(strUPrice.toFixed(2)) * parseFloat(splitVal[5].toString())) / parseFloat(splitVal[4].toString());
                            $('#' + ctrlRMCBID).parent("td").parent("tr").find("td").eq(12).html(strArv.toFixed(2));
                            //Rmc Value
                            strRmc = parseFloat(splitVal[1].toString());
                            $('#' + ctrlRMCBID).parent("td").parent("tr").find("td").eq(13).html(strRmc.toFixed(2));
                        }
                    }
                }
            }
            else {
                $('#' + ctrlRMCBID).parent("td").parent("tr").find("td").eq(11).html("0.00");
                $('#' + ctrlRMCBID).parent("td").parent("tr").find("td").eq(12).html("0.00");
                $('#' + ctrlRMCBID).parent("td").parent("tr").find("td").eq(13).html("0.00");
                $('#' + ctrlRMCBID).parent("td").parent("tr").find("td").eq(14).html("0.00");
            }
        }

        function changeValUnitPrice(e) {
            var ctrlUNITPRICEID = e.id;
            if ($('#' + ctrlUNITPRICEID).val().length > 0) {
                var calcVal = $('#' + ctrlUNITPRICEID).parent("td").parent("tr").find("td").eq(15).html()
                if (calcVal.length > 0) {
                    var splitVal = calcVal.split('~');
                    if (splitVal.length == 6) {
                        var strRMCB = ''; var strArv = ''; var strRmc = '';
                        $('#' + ctrlUNITPRICEID).parent("td").parent("tr").find("td").eq(11).html($('#' + ctrlUNITPRICEID).val());
                        if (splitVal[0].toString().toLowerCase() == "true") {
                            //RMCB
                            strRMCB = (parseFloat($('#' + ctrlUNITPRICEID).val()) * parseFloat(splitVal[5].toString())) / parseFloat(splitVal[4].toString()) - (parseFloat(splitVal[1].toString()) + (parseFloat(splitVal[3].toString()) * parseFloat(splitVal[2].toString())) / parseFloat(splitVal[4].toString()));
                            $('#' + ctrlUNITPRICEID).parent("td").parent("tr").find("td").eq(14).html(strRMCB.toFixed(2));
                            //Arv value
                            strArv = (parseFloat($('#' + ctrlUNITPRICEID).val()) * parseFloat(splitVal[5].toString())) / parseFloat(splitVal[4].toString());
                            $('#' + ctrlUNITPRICEID).parent("td").parent("tr").find("td").eq(12).html(strArv.toFixed(2));
                            //Rmc Value
                            strRmc = parseFloat(splitVal[1].toString()) + (parseFloat(splitVal[3].toString()) * parseFloat(splitVal[2].toString())) / parseFloat(splitVal[4].toString());
                            $('#' + ctrlUNITPRICEID).parent("td").parent("tr").find("td").eq(13).html(strRmc.toFixed(2));
                        }
                        else if (splitVal[0].toString().toLowerCase() == "false") {
                            //RMCB
                            strRMCB = (parseFloat($('#' + ctrlUNITPRICEID).val()) * parseFloat(splitVal[5].toString())) / parseFloat(splitVal[4].toString()) - (parseFloat(splitVal[1].toString()) + (0 * parseFloat(splitVal[2].toString())) / parseFloat(splitVal[4].toString()));
                            $('#' + ctrlUNITPRICEID).parent("td").parent("tr").find("td").eq(14).html(strRMCB.toFixed(2));
                            //Arv value
                            strArv = (parseFloat($('#' + ctrlUNITPRICEID).val()) * parseFloat(splitVal[5].toString())) / parseFloat(splitVal[4].toString());
                            $('#' + ctrlUNITPRICEID).parent("td").parent("tr").find("td").eq(12).html(strArv.toFixed(2));
                            //Rmc Value
                            strRmc = parseFloat(splitVal[1].toString());
                            $('#' + ctrlUNITPRICEID).parent("td").parent("tr").find("td").eq(13).html(strRmc.toFixed(2));
                        }
                    }
                }
            }
            else {
                $('#' + ctrlUNITPRICEID).parent("td").parent("tr").find("td").eq(11).html("0.00");
                $('#' + ctrlUNITPRICEID).parent("td").parent("tr").find("td").eq(12).html("0.00");
                $('#' + ctrlUNITPRICEID).parent("td").parent("tr").find("td").eq(13).html("0.00");
                $('#' + ctrlUNITPRICEID).parent("td").parent("tr").find("td").eq(14).html("0.00");
            }
        }

        function PriceSaveCtrlValidation() {
            showProgress(); $('#divSaveErrMsg').html(''); var ctrlErrMsg = '';
            if ($('#txtCustPriceAppFrom').val().length == 0)
                ctrlErrMsg += 'Choose from date, ';
            if ($('#txtCustPriceAppTill').val().length == 0)
                ctrlErrMsg += 'Choose till date, ';
            if (ctrlErrMsg.length > 0) {
                $('#divSaveErrMsg').html(ctrlErrMsg);
                hideProgress();
                return false;
            }
            else {
                var concatVal = '';
                var gv_PreValue = $("input[id*='txtPreValue']");
                for (j = 0; j < gv_PreValue.length; j++) {
                    if (concatVal.length > 0)
                        concatVal += "~" + $('#MainContent_gv_PremiumValue_lblPremium_' + j).html() + "," + $('#MainContent_gv_PremiumValue_txtPreValue_' + j).val();
                    else
                        concatVal = $('#MainContent_gv_PremiumValue_lblPremium_' + j).html() + "," + $('#MainContent_gv_PremiumValue_txtPreValue_' + j).val();
                }
                $('#hdnPremiumValue').val(concatVal);
                $('#hdnBaseValue').val($('#MainContent_gv_PremiumValue_lblBase_0').html() + "," + $('#MainContent_gv_PremiumValue_lblBaseValue_0').html());
                return true;
            }
        }

        $("#lblPrevtRatesID").hover(function () {
            $(".tooltip").css("display", "block");
        });

        $("#lblPrevtRatesID").mouseout(function () {
            $(".tooltip").css("display", "none");
        });
    </script>
</asp:Content>
