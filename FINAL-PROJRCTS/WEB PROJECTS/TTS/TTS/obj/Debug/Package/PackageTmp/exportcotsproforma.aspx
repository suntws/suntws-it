<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="exportcotsproforma.aspx.cs" Inherits="TTS.exportcotsproforma" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/cotsScript.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        Export Order Proforma Generated / Confirmed</div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="background-color: #ecf6ff; border-color: White;
            border-collapse: separate; width: 100%;">
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" Font-Bold="true"
                        Font-Size="20px" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView runat="server" ID="gvReceivedOrderList" AutoGenerateColumns="false"
                        Width="100%" HeaderStyle-BackColor="#FFCC00" AlternatingRowStyle-BackColor="#f5f5f5"
                        RowStyle-Height="25px">
                        <Columns>
                            <asp:TemplateField HeaderText="CUSTOMER NAME">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatusCustName" Text='<%# Eval("custfullname") %>'></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdnStatusCustCode" Value='<%# Eval("CustCode") %>' />
                                    <asp:HiddenField runat="server" ID="hdnUserCurrency" Value='<%# Eval("usercurrency") %>' />
                                    <asp:HiddenField runat="server" ID="hdnRequestStatus" Value='<%# Eval("RequestStatus") %>' />
                                    <asp:HiddenField runat="server" ID="hdnOrderID" Value='<%# Eval("ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CUSTOMER ORDER NO">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOrderRefNo" Text='<%#Eval("OrderRefNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ORDER DATE">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOrderDate" Text='<%#Eval("CompletedDate") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="LAST REVISED DATE" DataField="RevisedDate" />
                            <asp:BoundField HeaderText="CUSTOMER DESIRED SHIP DATE" DataField="DesiredShipDate" />
                            <asp:BoundField HeaderText="ORDER QTY" DataField="itemqty" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField HeaderText="SHIPMENT TYPE" DataField="ShipmentType" />
                            <asp:TemplateField HeaderText="STATUS">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatusText" Text='<%# Eval("StatusText") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ACTION" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkProformaBtn" runat="server" Text="Process" OnClick="lnkProformaBtn_Click"
                                        Font-Bold="true"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="width: 100%; float: left; display: none;" id="divStatusChange">
                        <table cellspacing="0" rules="all" border="1" style="background-color: #ecf6ff; border-color: White;
                            border-collapse: separate; width: 100%;">
                            <tr id="divOrderHead" style="text-align: center; line-height: 30px; font-size: 14px;">
                                <td>
                                    <asp:Label runat="server" ID="lblCustName" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblStausOrderRefNo" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:DataList runat="server" ID="dlOrderMaster" RepeatColumns="1" RepeatDirection="Horizontal"
                                        RepeatLayout="Table" AlternatingItemStyle-BackColor="#A1C9D3" ItemStyle-BackColor="#C9DAC9"
                                        ItemStyle-VerticalAlign="Top" Width="100%">
                                        <ItemTemplate>
                                            <table cellspacing="0" rules="all" border="1" style="background-color: #ecf6ff; border-color: White;
                                                border-collapse: separate; width: 100%;">
                                                <tr>
                                                    <th>
                                                        Orderd Date
                                                    </th>
                                                    <td>
                                                        <%# Eval("CompletedDate")%>
                                                    </td>
                                                    <th>
                                                        Delivery Method
                                                    </th>
                                                    <td>
                                                        <%# Eval("DeliveryMethod")%>
                                                        -
                                                        <%# Eval("GodownName") %>
                                                        -
                                                        <%# Eval("TransportDetails")%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        Desired Ship Date
                                                    </th>
                                                    <td>
                                                        <%# Eval("DesiredShipDate")%>
                                                    </td>
                                                    <th>
                                                        Packing Method
                                                    </th>
                                                    <td>
                                                        <%# Eval("PackingMethod") %>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        Bill To
                                                    </th>
                                                    <td>
                                                        <%# Bind_BillingAddress(Eval("BillID").ToString())%>
                                                    </td>
                                                    <th>
                                                        Ship To
                                                    </th>
                                                    <td>
                                                        <%# Bind_BillingAddress(Eval("ShipID").ToString())%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th colspan="2" style="text-align: left;">
                                                        Special Instruction:&nbsp;
                                                        <%# Eval("SplIns").ToString().Replace("~", "\r\n") %>
                                                    </th>
                                                    <th colspan="2" style="text-align: left;">
                                                        Special Notes:&nbsp;
                                                        <%# Eval("SpecialRequset").ToString().Replace("~", "\r\n") %>
                                                    </th>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </asp:DataList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:GridView runat="server" ID="gvOrderItemList" AutoGenerateColumns="false" Width="100%"
                                        ShowFooter="true" FooterStyle-Font-Bold="true" FooterStyle-HorizontalAlign="Right"
                                        FooterStyle-BackColor="#FAAC58" CssClass="gridcss">
                                        <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" Height="22px" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="CATEGORY">
                                                <ItemTemplate>
                                                    <%# Eval("category") %>
                                                    <%# Eval("AssyRimstatus").ToString() == "True" ? " (ASSY)" : ""%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="PLATFORM" DataField="config" />
                                            <asp:BoundField HeaderText="TYRE SIZE" DataField="tyresize" />
                                            <asp:BoundField HeaderText="RIM" DataField="rimsize" />
                                            <asp:BoundField HeaderText="TYPE" DataField="tyretype" />
                                            <asp:BoundField HeaderText="BRAND" DataField="brand" />
                                            <asp:BoundField HeaderText="SIDEWALL" DataField="sidewall" />
                                            <asp:BoundField HeaderText="BASIC PRICE" DataField="listprice" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField HeaderText="QTY" DataField="itemqty" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField HeaderText="TOTAL PRICE" DataField="unitpricepdf" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField HeaderText="FWT" DataField="totalfwt" ItemStyle-HorizontalAlign="Right" />
                                            <asp:TemplateField HeaderText="RIM PRICE" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <%# Eval("Rimunitprice").ToString() == "0.00" ? "" : Eval("Rimunitprice").ToString()%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="RIM QTY" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <%# Eval("Rimitemqty").ToString() == "0" ? "" : Eval("Rimitemqty").ToString()%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="TOT RIM PRICE" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <%# Eval("Rimpricepdf").ToString() == "0.00" ? "" : Eval("Rimpricepdf").ToString()%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="RIM FWT" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <%# Eval("Rimfinishedwt").ToString() == "0.00" ? "" : Eval("Rimfinishedwt").ToString()%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="AdditionalInfo" HeaderText="ADDITIONAL INFO" />
                                            <asp:BoundField HeaderText="PLANT" DataField="ItemPlant" />
                                            <asp:TemplateField HeaderText="CODE">
                                                <ItemTemplate>
                                                    <asp:HiddenField runat="server" ID="hdnProcessID" Value='<%# Eval("processid") %>' />
                                                    <asp:TextBox runat="server" ID="txtCode" Text='<%# Eval("ItemCode") %>' Width="80px"
                                                        Enabled="true" MaxLength="20"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:GridView runat="server" ID="gv_OrderSumValue" AutoGenerateColumns="true" Width="100%"
                                        HeaderStyle-BackColor="#166502" HeaderStyle-ForeColor="#ffffff" ShowFooter="true"
                                        FooterStyle-BackColor="#A9F5A9" RowStyle-HorizontalAlign="Right" RowStyle-VerticalAlign="Middle"
                                        FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label runat="server" ID="lblCurrStatus" ClientIDMode="Static" Text="" Font-Bold="true"
                                        Font-Size="Large"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table cellspacing="0" rules="all" border="1" style="background-color: #ecf6ff; border-color: White;
                                        border-collapse: separate; width: 100%;">
                                        <tr>
                                            <th>
                                                Exporter Authorized Address
                                            </th>
                                            <td colspan="3">
                                                <asp:DropDownList ID="ddlExpAuthorizedAddress" ClientIDMode="Static" runat="server"
                                                    Width="950px" CssClass="form-control">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                Proforma Ref No
                                            </th>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtProformaRefNo" ClientIDMode="Static" Text="" Width="300px"
                                                    MaxLength="100" CssClass="form-control"></asp:TextBox>
                                                Date:
                                                <asp:Label runat="server" ID="lblProformaDate" ClientIDMode="Static" Text="" Width="80px"></asp:Label>
                                                Revision:
                                                <asp:Label runat="server" ID="lblReviseCount" ClientIDMode="Static" Text="" Font-Bold="true"
                                                    Width="50px"></asp:Label>
                                            </td>
                                            <th>
                                                Container Type
                                            </th>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlContainerType" ClientIDMode="Static" Width="70px"
                                                    CssClass="form-control">
                                                    <asp:ListItem Text="20'" Value="20'" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="40'" Value="40'"></asp:ListItem>
                                                    <asp:ListItem Text="LCL" Value="LCL"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                Pre-Carriage by
                                            </th>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlPreCarriageBy" ClientIDMode="Static" Width="265px"
                                                    CssClass="form-control">
                                                    <asp:ListItem Text="SEA" Value="SEA"></asp:ListItem>
                                                    <asp:ListItem Text="AIR" Value="AIR"></asp:ListItem>
                                                    <asp:ListItem Text="ROAD" Value="ROAD"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <th>
                                                Incoterm
                                            </th>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlPriceBasis" ClientIDMode="Static" Width="70px"
                                                    CssClass="form-control">
                                                    <asp:ListItem Text="FOB" Value="FOB"></asp:ListItem>
                                                    <asp:ListItem Text="CIF" Value="CIF"></asp:ListItem>
                                                    <asp:ListItem Text="DDU" Value="DDU"></asp:ListItem>
                                                    <asp:ListItem Text="DDP" Value="DDP"></asp:ListItem>
                                                    <asp:ListItem Text="C&F" Value="C&F"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                Country of origin of goods
                                            </th>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlCountryOrigin" ClientIDMode="Static" Width="265px"
                                                    CssClass="form-control">
                                                    <asp:ListItem Text="INDIA" Value="INDIA"></asp:ListItem>
                                                    <asp:ListItem Text="SRILANKA" Value="SRILANKA"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <th>
                                                Incoterm Port
                                            </th>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtPriceBasisContent" ClientIDMode="Static" Text=""
                                                    Width="200px" MaxLength="20" CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                Port of loading
                                            </th>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlPortLoading" ClientIDMode="Static" Width="265px"
                                                    CssClass="form-control">
                                                    <asp:ListItem Text="CHENNAI, INDIA" Value="INDIA"></asp:ListItem>
                                                    <asp:ListItem Text="COLOMBO, SRILANKA" Value="SRILANKA"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <th>
                                                Place of receipt by pre-carrier
                                            </th>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlPlaceofReceipt" ClientIDMode="Static" Width="265px"
                                                    CssClass="form-control">
                                                    <asp:ListItem Text="CHENNAI" Value="CHENNAI"></asp:ListItem>
                                                    <asp:ListItem Text="LANKA" Value="LANKA"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                Payment Terms
                                            </th>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtPayterms" ClientIDMode="Static" Text="" TextMode="MultiLine"
                                                    Width="450px" Height="188px" onKeyUp="javascript:CheckMaxLength(this, 1999);"
                                                    onChange="javascript:CheckMaxLength(this, 1999);" CssClass="form-control"></asp:TextBox>
                                            </td>
                                            <td colspan="2">
                                                <div>
                                                    <asp:GridView runat="server" ID="gvAmountSub" AutoGenerateColumns="false" Width="500px">
                                                        <Columns>
                                                            <asp:BoundField DataField="slno" Visible="false" />
                                                            <asp:TemplateField ItemStyle-Width="350px" HeaderText="DESCRIPTION OF OTHER CHARGES IF ANY"
                                                                HeaderStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:TextBox runat="server" ID="txtAddDesc" Text="" Width="350px" MaxLength="100"
                                                                        CssClass="form-control"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ItemStyle-Width="35px" HeaderText="+" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCalcTypeAdd" runat="server" Text="+" ToolTip="ADD" Font-Bold="true"
                                                                        Font-Size="20px"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ItemStyle-Width="70px" HeaderText="AMOUNT">
                                                                <ItemTemplate>
                                                                    <asp:TextBox runat="server" ID="txtCAddAmt" onkeypress="return isNumberKey(event)"
                                                                        Text="" Width="70px" MaxLength="8" CssClass="form-control"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                                <div>
                                                    <table cellspacing="0" rules="all" border="1" style="width: 500px; border-collapse: collapse;">
                                                        <tr>
                                                            <td colspan="3" style="text-align: left; font-weight: bold;">
                                                                <span>CLAIM ADJUSTMENT</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 370px;">
                                                                <asp:TextBox runat="server" ID="txtClaimAdjustment" Text="" Width="350px" MaxLength="100"
                                                                    ToolTip="CLAIM ADJUSTMENT" CssClass="form-control"></asp:TextBox>
                                                            </td>
                                                            <td style="width: 35px; text-align: center">
                                                                <asp:Label ID="lblLESSclaimAdjus" runat="server" Text="-" ToolTip="LESS" Font-Bold="true"
                                                                    Font-Size="20px"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtLESSAMT" onkeypress="return isNumberKey(event)"
                                                                    Text="" Width="70px" MaxLength="8" CssClass="form-control"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3" style="text-align: left; font-weight: bold;">
                                                                OTHER DISCOUNT
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 374px;">
                                                                <asp:TextBox runat="server" ID="txtotherdiscount" Text="" Width="350px" MaxLength="100"
                                                                    ToolTip="OTHER DISCOUNT" CssClass="form-control"></asp:TextBox>
                                                            </td>
                                                            <td style="width: 37px; text-align: center">
                                                                <asp:Label ID="lblLessdiscount" runat="server" Text="-" ToolTip="LESS" Font-Bold="true"
                                                                    Font-Size="20px"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtOtherDisAmt" onkeypress="return isNumberKey(event)"
                                                                    Text="" Width="70px" MaxLength="8" CssClass="form-control"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="text-align: right;">
                                                <asp:CheckBox runat="server" ID="chkAssyPrice" ClientIDMode="Static" Text="Do you want show the tyre & rim Price should be separate"
                                                    TextAlign="Left" Font-Bold="true" Font-Size="14px" />
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblErrMsg1" ClientIDMode="Static" Text="" ForeColor="Red"
                                                    Font-Bold="true"></asp:Label>
                                                <asp:Button runat="server" ID="btnPrepareProforma" ClientIDMode="Static" Text="PREPARE PROFORMA"
                                                    CssClass="btn btn-success" OnClick="btnPrepareProforma_Click" OnClientClick="javascript:return CtrlProformChk();" />
                                            </td>
                                            <td style="text-align: right; padding-right: 50px;">
                                                <asp:Label runat="server" ID="lblCustCurrency" ClientIDMode="Static" Text="" Font-Bold="true"
                                                    Font-Size="20px"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="statusChangeDiv" style="display: none;">
                        <table cellspacing="0" rules="all" border="1" style="background-color: #ecf6ff; border-color: White;
                            border-collapse: separate; width: 100%;">
                            <tr>
                                <td rowspan="2">
                                    Enter Your Commets
                                    <asp:TextBox runat="server" ID="txtOrderChangeComments" ClientIDMode="Static" Text=""
                                        TextMode="MultiLine" onKeyUp="javascript:CheckMaxLength(this, 999);" onChange="javascript:CheckMaxLength(this, 999);"
                                        Width="645px" Height="80px" CssClass="form-control"></asp:TextBox>
                                </td>
                                <td>
                                    Proforma:
                                    <asp:LinkButton runat="server" ID="lnkPdfLink" ClientIDMode="Static" Text="" OnClick="lnkPdfLink_click"
                                        OnClientClick="aspnetForm.target ='_blank';"></asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button runat="server" ID="btnSaveChangeStatus" ClientIDMode="static" Text=""
                                        CssClass="btn btn-success" OnClick="btnSaveChangeStatus_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnCustCode" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnStatusOrderDate" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnSelectIndex" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnShipmentType" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnRequestStatusID" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnPartNo" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnAuthorized" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnOID" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        $(document).ready(function () {
            blinkOrderHead();
            $('#txtProformaRefNo').blur(function () {
                if ($('#txtProformaRefNo').val().length > 0) {
                    $('#lblErrMsg1').html('');
                    $.ajax({ type: "POST", url: "BindRecords.aspx?type=ChkProformaNo&ProfromaId=" + $('#txtProformaRefNo').val() + "", context: document.body,
                        success: function (data) {
                            if (data != '') { $('#lblErrMsg1').html(data); }
                        }
                    });
                }
            });
        });

        function CtrlProformChk() {
            var errmsg = ''; $('#lblErrMsg1').html('');
            if ($('#hdnPartNo').val() != "") {
                $('input:text[id^=MainContent_gvOrderItemList_txtCode_]').each(function () { if ($(this).val().length == 0) { errmsg += 'Please enter barcode text<br/>'; } });
            }
            $("input:text[id*=MainContent_gvAmountSub_txtAddDesc_]").each(function () {
                var id1 = this.id; var amtId = id1.replace('txtAddDesc_', 'txtCAddAmt_');
                if ($('#' + id1).val() != '' && $('#' + amtId).val() == '')
                    errmsg += 'Enter extra charges amount<br/>';
                if ($('#' + id1).val() == '' && $('#' + amtId).val() != '')
                    errmsg += 'Enter extra charges description<br/>';
            });
            if ($('#MainContent_txtClaimAdjustment').val() != '' && $('#MainContent_txtLESSAMT').val() == '')
                errmsg += 'Enter Claim Adjustment amount<br/>';
            else if ($('#MainContent_txtClaimAdjustment').val() == '' && $('#MainContent_txtLESSAMT').val() != '')
                errmsg += 'Enter Claim Adjustment description<br/>';
            if ($('#MainContent_txtotherdiscount').val() != '' && $('#MainContent_txtOtherDisAmt').val() == '')
                errmsg += 'Enter other discount amount<br/>';
            else if ($('#MainContent_txtotherdiscount').val() == '' && $('#MainContent_txtOtherDisAmt').val() != '')
                errmsg += 'Enter other discount description<br/>';
            if ($('#txtProformaRefNo').val().length == 0) {
                errmsg += 'Enter Proforma Ref No.<br/>';
                $('#txtProformaRefNo').focus();
            }
            if ($('#ddlExpAuthorizedAddress option:selected').text() == 'CHOOSE')
                errmsg += 'Choose exporter authorized address<br/>';
            if (errmsg.length > 0) {
                $('#lblErrMsg1').html(errmsg);
                return false;
            }
            else
                return true;
        }

        function showStatusChangeBtn() {
            $('#divStatusChange').find("input,button,textarea,radio,checkbox,select").attr("disabled", true).css({ 'cursor': 'not-allowed' });
            gotoPreviewDiv('statusChangeDiv');
        }
    </script>
</asp:Content>
