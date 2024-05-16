<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="cotspricechange.aspx.cs" Inherits="TTS.cotspricechange" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/popupBox.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/cotsScript.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        ORDER ITEMS PRICE CHANGE</div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="width: 100%; border-color: White;
            border-collapse: separate;">
            <tr>
                <td style="text-align: left;">
                    CUSTOMER NAME
                </td>
                <td style="text-align: left">
                    <asp:DropDownList runat="server" ID="ddlCotsCustName" ClientIDMode="Static" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlCotsCustName_IndexChange">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:GridView runat="server" ID="gvPriceChangeOrderList" AutoGenerateColumns="false"
                        Width="100%" HeaderStyle-BackColor="#FFCC00" AlternatingRowStyle-BackColor="#f5f5f5"
                        OnRowDeleting="gvPriceChangeOrderList_RowChoose">
                        <Columns>
                            <asp:TemplateField HeaderText="CUSTOMER NAME" ItemStyle-Width="200px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatusCustName" Text='<%# Eval("custfullname") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ORDER REFERENCE NO" ItemStyle-Width="220px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOrderRefNo" Text='<%#Eval("OrderRefNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="ORDERD DATE" DataField="CompletedDate" ItemStyle-Width="60px" />
                            <asp:BoundField HeaderText="LAST REVISED DATE" DataField="RevisedDate" ItemStyle-Width="60px" />
                            <asp:BoundField HeaderText="CUSTOMER DESIRED SHIP DATE" DataField="DesiredShipDate"
                                ItemStyle-Width="60px" />
                            <asp:BoundField HeaderText="QTY" DataField="itemqty" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField HeaderText="PLANT" DataField="Plant" ItemStyle-Width="40px" />
                            <asp:TemplateField HeaderText="STATUS" ItemStyle-Width="180px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatusText" Text='<%# Eval("StatusText") %>'></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdnCotsCustCode" Value='<%# Eval("CustCode") %>' />
                                    <asp:HiddenField runat="server" ID="hdnUserCurrency" Value='<%# Eval("usercurrency") %>' />
                                    <asp:HiddenField runat="server" ID="hdnCustCategory" Value='<%# Eval("CustCategory") %>' />
                                    <asp:HiddenField runat="server" ID="hdnOrderID" Value='<%# Eval("ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ACTION" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Button runat="server" ID="btnPriceChange" Text="Show Item List" CommandName="Delete"
                                        Visible='<%# Eval("CustHoldStatus").ToString() == "True" ? false : true%>' />
                                    <span style="color: #ff0000; font-style: italic;">
                                        <%# Eval("CustHoldStatus").ToString() == "True" ? "CREDIT HOLD" : ""%></span>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div style="width: 100%; float: left; border: 1px solid #000; display: none; line-height: 20px;
                        margin-top: 10px; background-color: #F0E2F5; padding-top: 5px;" id="divStatusChange">
                        <div id="divOrderHead" style="width: 100%; float: left;">
                            <div style="width: 50%; float: left; text-align: left;">
                                <asp:Label runat="server" ID="lblCustName" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                            </div>
                            <div style="width: 50%; float: left; text-align: right;">
                                <asp:Label runat="server" ID="lblStausOrderRefNo" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                            </div>
                        </div>
                        <div style="width: 100%; float: left;">
                            <div style="width: 50%; float: left;">
                                <span class="headCss">Special Instruction</span>
                                <asp:TextBox runat="server" ID="txtOrderSplIns" Text="" TextMode="MultiLine" Width="520px"
                                    Height="100px" Enabled="false"></asp:TextBox>
                            </div>
                            <div style="width: 50%; float: right;">
                                <span class="headCss">Special Notes</span>
                                <asp:TextBox runat="server" ID="txtOrdersplReq" Text="" TextMode="MultiLine" Width="520px"
                                    Height="100px" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                        <div style="width: 100%; float: left; text-align: center;">
                            <asp:Label runat="server" ID="lblCurrentStatus" ClientIDMode="Static" Text="" Font-Bold="true"
                                Font-Size="15px"></asp:Label>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="display: none; padding-top: 10px; padding-bottom: 10px;" id="divRatesValChange">
                    <div style="float: left; width: 500px;">
                        <div style="width: 60px; float: left; line-height: 30px;">
                            <span class="headCss">Rates-ID</span>
                        </div>
                        <div style="width: 385px; float: left; line-height: 28px;">
                            <asp:DropDownList runat="server" ID="ddlPriceChangeRatesID">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div style="width: 565px; float: left; font-weight: bold; font-size: 15px; line-height: 30px;">
                        <span class="headCss">ARV : </span>
                        <asp:Label runat="server" ID="lblArv" ClientIDMode="Static" Text="" Width="100px"></asp:Label>
                        <span class="headCss">RMC : </span>
                        <asp:Label runat="server" ID="lblRmc" ClientIDMode="Static" Text="" Width="100px"></asp:Label>
                        <span class="headCss">RMCB : </span>
                        <asp:Label runat="server" ID="lblTotRmcb" ClientIDMode="Static" Text="" Width="150px"></asp:Label>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:GridView runat="server" ID="gvRevisePriceList" AutoGenerateColumns="false" Width="100%"
                        AlternatingRowStyle-BackColor="#f5f5f5" OnRowEditing="gvRevisePriceList_RowEditing"
                        OnRowUpdating="gvRevisePriceList_RowUpdating" OnRowCancelingEdit="gvRevisePriceList_RowCanceling">
                        <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" Height="22px" />
                        <Columns>
                            <asp:TemplateField ItemStyle-CssClass="headerNone" HeaderStyle-CssClass="headerNone">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblProcessid" Text='<%#Eval("processid") %>'></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdnItemID" Value='<%# Eval("O_ItemID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CATEGORY" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <%#Eval("category")%>
                                    <%# Eval("AssyRimstatus").ToString() == "True" ? " (ASSY)" : ""%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PLATFORM" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCofig" Text='<%#Eval("config") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TYRE SIZE" ItemStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblSize" Text='<%#Eval("tyresize") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="RIM" ItemStyle-Width="40px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblRim" Text='<%#Eval("rimsize") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TYPE" ItemStyle-Width="60px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblType" Text='<%#Eval("tyretype") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="BRAND" ItemStyle-Width="80px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblBrand" Text='<%#Eval("brand") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SIDEWALL" ItemStyle-Width="80px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblSidewall" Text='<%#Eval("sidewall") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="LIST PRICE" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblSheetPrice" Text='<%#Eval("SheetPrice") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DISC %" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblDiscPer" Text='<%#Eval("Discount") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox runat="server" ID="txtDiscPer" Text='<%#Eval("Discount") %>' BackColor="#f9c232"
                                        Width="65px" onkeypress="return isNumberAndMinusKey(event)" onkeyUp="CalcFromDiscount(event,this)"
                                        onchange="calcBasicPrice(event,this)" title="disc"></asp:TextBox>
                                    <asp:HiddenField runat="server" ID="hdnMaxDiscPer" Value='<%#Eval("MaxDiscount") %>'
                                        ClientIDMode="Static" />
                                    <asp:HiddenField runat="server" ID="hdnCurrentDisc" Value='<%#Eval("Discount") %>'
                                        ClientIDMode="Static" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PRICE" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblChangePrice" Text='<%#Eval("listprice") %>'></asp:Label></ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox runat="server" ID="txtChangePrice" onkeypress="return isNumberAndMinusKey(event)"
                                        Width="70px" MaxLength="12" BackColor="#f9c232" Text='<%# Eval("listprice") %>'
                                        title="price" onchange="calcBasicPrice(event,this)" onkeyup="changeUnitPriceWiseRMCB(this)"></asp:TextBox>
                                    <asp:HiddenField runat="server" ID="hdnCurrentPrice" Value='<%# Eval("listprice") %>' />
                                    <asp:HiddenField runat="server" ID="hdnMinPrice" ClientIDMode="Static" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="QTY" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblItemQty" Text='<%#Eval("itemqty") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="RIM PRICE" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblRimChangePrice" Text='<%#Eval("Rimunitprice") %>'
                                        Visible='<%# Eval("Rimunitprice").ToString() == "0.00" ? false : true%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox runat="server" ID="txtRimChangePrice" onkeypress="return isNumberAndMinusKey(event)"
                                        Width="70px" MaxLength="12" BackColor="#f9c232" Text='<%# Eval("Rimunitprice") %>'
                                        onkeyup="changeUnitPriceWiseRMCB(this)" Visible='<%# Eval("Rimunitprice").ToString() == "0.00" ? false : true%>'></asp:TextBox>
                                    <asp:HiddenField runat="server" ID="hdnRimCurrentPrice" Value='<%# Eval("Rimunitprice") %>' />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="RIM QTY" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblRimItemQty" Text='<%# Eval("Rimitemqty").ToString() == "0" ? "" : Eval("Rimitemqty").ToString()%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TOTAL PRICE" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTotPrice" Text='<%#(Convert.ToInt32(Eval("itemqty")) * Convert.ToDouble(Eval("listprice")) + Convert.ToInt32(Eval("Rimitemqty")) * Convert.ToDouble(Eval("Rimunitprice"))).ToString()%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ACTION" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:Button runat="server" ID="btnEdit" Text="Change Price" CommandName="Edit" CssClass="btnedit" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Button runat="server" ID="btnUpdate" Text="Update" CommandName="Update" />
                                    <asp:Button runat="server" ID="btnCancel" Text="Cancel" CommandName="Cancel" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div id="divPriceChange" style="display: none;">
                        <asp:TextBox runat="server" ID="txtPriceChangecomment" ClientIDMode="Static" Text=""
                            TextMode="MultiLine" Width="700px" Height="70px" onKeyUp="javascript:CheckMaxLength(this, 999);"
                            onChange="javascript:CheckMaxLength(this, 999);"></asp:TextBox>
                        <asp:Button runat="server" ID="btnPriceChangeCompleted" Text="Revision Completed"
                            CssClass="btnedit" OnClick="btnPriceChangeCompleted_Click" />
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnCustCode" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnCurVal" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnRMCBCostValue" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnArv" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnRmc" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnRmcb" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnMaxDiscount" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnOrderGrade" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnUserName" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnPlant" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnOID" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        $(document).ready(function () {
            blinkOrderHead();
        });

        function divTotPriceShow() {
            $('#divRatesValChange').css({ 'display': 'block' });
        }

        function changeUnitPriceWiseRMCB(e) {
            var ctrlUNITPRICEID = e.id;
            if ($('#' + ctrlUNITPRICEID).val().length > 0) {
                var calcVal = $('#hdnRMCBCostValue').val()
                if (calcVal.length > 0) {
                    var splitVal = calcVal.split('~');
                    if (splitVal.length == 6) {
                        var strRMCB = ''; var strArv = ''; var strRmc = ''; //  splitVal order - { 0 -BeadBand, 1-  TypeCost, 2- SizeCost, 3- BeadsQty, 4 - FinishedWt, 5- CurVal; ( refer hdnRMCBCostValue in gvRevisePriceList_RowEditing )
                        if (splitVal[0].toString().toLowerCase() == "true") {
                            //RMCB - After discount
                            strRMCB = (parseFloat($('#' + ctrlUNITPRICEID).val()) * parseFloat(splitVal[5].toString())) / parseFloat(splitVal[4].toString()) - (parseFloat(splitVal[1].toString()) + (parseFloat(splitVal[3].toString()) * parseFloat(splitVal[2].toString())) / parseFloat(splitVal[4].toString()));
                            $('#lblTotRmcb').html(strRMCB.toFixed(2));
                            //Arv value
                            strArv = (parseFloat($('#' + ctrlUNITPRICEID).val()) * parseFloat(splitVal[5].toString())) / parseFloat(splitVal[4].toString());
                            $('#lblArv').html(strArv.toFixed(2));
                            //Rmc Value
                            strRmc = parseFloat(splitVal[1].toString()) + (parseFloat(splitVal[3].toString()) * parseFloat(splitVal[2].toString())) / parseFloat(splitVal[4].toString());
                            $('#lblRmc').html(strRmc.toFixed(2));
                        }
                        else if (splitVal[0].toString().toLowerCase() == "false") {
                            //RMCB
                            strRMCB = (parseFloat($('#' + ctrlUNITPRICEID).val()) * parseFloat(splitVal[5].toString())) / parseFloat(splitVal[4].toString()) - (parseFloat(splitVal[1].toString()) + (0 * parseFloat(splitVal[2].toString())) / parseFloat(splitVal[4].toString()));
                            $('#lblTotRmcb').html(strRMCB.toFixed(2));
                            //Arv value
                            strArv = (parseFloat($('#' + ctrlUNITPRICEID).val()) * parseFloat(splitVal[5].toString())) / parseFloat(splitVal[4].toString());
                            $('#lblArv').html(strArv.toFixed(2));
                            //Rmc Value
                            strRmc = parseFloat(splitVal[1].toString());
                            $('#lblRmc').html(strRmc.toFixed(2));
                        }
                    }
                    $('#hdnArv').val($('#lblArv').html());
                    $('#hdnRmc').val($('#lblRmc').html());
                    $('#hdnRmcb').val($('#lblTotRmcb').html());
                }
            }
        }

        function changeDiscCalcRMCB(e) {
            var ctrlUNITPRICEID = e.id;
        }

        function CalcFromDiscount(eve, source) {
            if (parseFloat($(source).val()) > parseFloat($(source).closest("tr").find("#hdnMaxDiscPer").val()) && $('#hdnUserName').val() == '0') {
                alert('Your maximum discount limit is ' + $(source).closest("tr").find("#hdnMaxDiscPer").val());
                $(source).val($(source).closest("tr").find("#hdnMaxDiscPer").val());
            }
            calcBasicPrice(eve, source);
        }

        function calcBasicPrice(eve, source) {
            eve.preventDefault();
            var sheetprice = parseFloat($(source).closest("tr").find("td").eq(8).text().trim());
            var disc = parseFloat($(source).closest("tr").find("td [title='disc']").val());
            var basicprice = parseFloat($(source).closest("tr").find("td [title='price']").val());
            var maxDisc = parseFloat($(source).closest("tr").find("#hdnMaxDiscPer").val());
            if ($(source).attr("title") == "price") {
                var minPrice = $(source).closest("tr").find("#hdnMinPrice");
                if (minPrice.val() == "") {
                    minPrice.val(parseFloat((sheetprice - (sheetprice * (maxDisc / 100)))).toFixed());
                }
                if (parseFloat($(source).val()) < parseFloat(minPrice.val()) && $('#hdnUserName').val() == '0') {
                    alert("Your minimum price limit is " + minPrice.val());
                    $(source).closest("tr").find("td [title='price']").val(minPrice.val());
                    $(source).closest("tr").find("td [title='disc']").val(maxDisc);
                }
                else {
                    var discPer = parseFloat(100 - (100 * (basicprice / sheetprice))).toFixed();

                    if (parseFloat($(source).val()) > sheetprice && $('#hdnUserName').val() == '0') {
                        alert("The Price you entered is exceeding the total price (" + sheetprice + ") without discount");
                        $(source).val(sheetprice);
                        $(source).closest("tr").find("td [title='disc']").val("0");
                    }
                    else {
                        $(source).closest("tr").find("td [title='disc']").val(parseFloat(discPer).toFixed());
                    }
                }
            }
            else if ($(source).attr("title") == "disc") {
                if (disc > 0) {
                    var basePrice = parseFloat((sheetprice - (sheetprice * (disc / 100)))).toFixed();
                }
                else {
                    var basePrice = sheetprice;
                    $(source).closest("tr").find("td [title='disc']").val(0);
                }
                $(source).closest("tr").find("td [title='price']").val(parseFloat(basePrice).toFixed());
            }
        }
    </script>
</asp:Content>
