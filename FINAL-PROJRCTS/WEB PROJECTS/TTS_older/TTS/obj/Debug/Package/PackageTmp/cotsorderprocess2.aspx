<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cotsorderprocess2.aspx.cs"
    MasterPageFile="~/master.Master" Inherits="TTS.cotsorderprocess2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/datemonthyear.css" rel="stylesheet" type="text/css" />
    <link href="Styles/popupBox.css" rel="stylesheet" type="text/css" />
    <link href="Styles/scotsdomestic.css" rel="stylesheet" type="text/css" />
    <link href="Styles/TinyStyle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .lblCss
        {
            font-size: 14px;
            font-family: Times New Roman;
        }
        .tableCss
        {
            background-color: #dcecfb;
            width: 100%;
            line-height: 25px;
        }
        .tableCss1
        {
            background-color: #dcecfb;
            width: 100%;
        }
        .tableCss th
        {
            font-size: 12px;
            color: #025a02;
            font-style: italic;
            text-align: center;
            font-weight: normal;
            line-height: 12px;
        }
        .tableCss1 th
        {
            width: 90px;
        }
        .tableCss td
        {
            line-height: 18px;
            font-weight: bold;
        }
        .headerCss
        {
            font-family: Times New Roman;
            font-style: italic;
            font-weight: bold;
            color: #614126;
            font-size: 14px;
        }
        .button
        {
            background-color: #4CAF50;
            border: none;
            color: white;
            padding: 10px 25px;
            text-align: center;
            text-decoration: none;
            font-size: 16px;
            margin: 4px 2px;
            cursor: pointer;
        }
        .imageCss
        {
            width: 55px;
            height: 55px;
            cursor: pointer;
        }
        .lblHeader
        {
            font-size: 18px;
            font-style: italic;
            font-weight: bold;
            color: #614126;
            width: 100%;
        }
        .tableInvoice
        {
            background-color: #dcecfb;
            width: 100%;
        }
        .tableInvoice th
        {
            font-size: 12px;
            color: #025a02;
            font-style: italic;
            text-align: center;
            font-weight: normal;
            line-height: 12px;
            width: 200px;
        }
        .tableInvoice td
        {
            font-weight: bold;
            width: 400px;
        }
        .filelnk
        {
            color: #095B80 !important;
            line-height: 15px;
            text-decoration: underline;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        <asp:Label runat="server" ID="lblpageHeading" Text=""></asp:Label>
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="width: 100%;">
            <tr>
                <td>
                    <div id="div_CustDetails" style="text-align: center;">
                        <div>
                            <!-- Order Master Details -->
                            <asp:FormView ID="frmOrderMasterDetails" runat="server" Width="100%">
                                <ItemTemplate>
                                    <table cellspacing="0" rules="all" border="1" class="tableCss">
                                        <tr>
                                            <th>
                                                Customer
                                            </th>
                                            <td colspan="3">
                                                <asp:Label ID="lblCustomerName" runat="server" CssClass="lblCss" Text='<%# Eval("custfullname")%>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdnCreditNote" ClientIDMode="Static" Value='<%# Eval("CreditNote") %>' />
                                                <asp:HiddenField runat="server" ID="hdnPaymentdays" ClientIDMode="Static" Value='<%# Eval("Paymentdays") %>' />
                                                <asp:HiddenField runat="server" ID="hdnCustHoldStatus" ClientIDMode="Static" Value='<%# Eval("CustHoldStatus") %>' />
                                            </td>
                                            <th>
                                                Order Ref No.
                                            </th>
                                            <td colspan="3">
                                                <%# Eval("OrderRefNo")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                Order Date
                                            </th>
                                            <td>
                                                <asp:Label runat="server" ID="lblOrderDate" ClientIDMode="Static" Text='<%# Eval("CompletedDate")%>'></asp:Label>
                                            </td>
                                            <th>
                                                Desired ShipDate
                                            </th>
                                            <td>
                                                <%# Eval("DesiredShipDate")%>
                                            </td>
                                            <th>
                                                Delivery Method
                                            </th>
                                            <td colspan="3">
                                                <asp:Label runat="server" ID="lblDeliveryMethod" ClientIDMode="Static" Text='<%# Eval("DeliveryMethod")%>'></asp:Label>
                                                <br />
                                                <asp:Label runat="server" ID="lblGodownName" ClientIDMode="Static" Text='<%# Eval("GodownName") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                Expected Ship Date
                                            </th>
                                            <td>
                                                <asp:Label ID="lblExpectedDate" Text='<%# Eval("ExpectedShipDate")%>' CssClass="lblCss"
                                                    runat="server"></asp:Label>
                                            </td>
                                            <th>
                                                Transport
                                            </th>
                                            <td>
                                                <asp:Label runat="server" ID="lblTransportDetails" ClientIDMode="Static" Text='<%# Eval("TransportDetails")%>'></asp:Label>
                                            </td>
                                            <th>
                                                Freight Charges
                                            </th>
                                            <td>
                                                <asp:Label runat="server" ID="lblFreightCharges" ClientIDMode="Static" Text='<%# Eval("FreightCharges")%>'></asp:Label>
                                            </td>
                                            <th>
                                                Packing Method
                                            </th>
                                            <td>
                                                <%# Eval("PackingMethod")%>
                                                <%# Eval("PackingOthers").ToString() !="" ? (" - " +Eval("PackingOthers").ToString()):"" %>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                Bill To
                                            </th>
                                            <td colspan="3">
                                                <%# Bind_BillingAddress(Eval("BillID").ToString(), false)%>
                                            </td>
                                            <th>
                                                Ship To
                                            </th>
                                            <td colspan="3">
                                                <%# Bind_BillingAddress(Eval("ShipID").ToString(), true)%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                Special Instruction
                                            </th>
                                            <td colspan="3">
                                                <%# Eval("SplIns")%>
                                            </td>
                                            <th>
                                                Special Notes
                                            </th>
                                            <td colspan="3">
                                                <%# Eval("SpecialRequset")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                Current Status
                                            </th>
                                            <td colspan="3">
                                                <asp:Label ID="lblCurrentStatus" runat="server" Style="font-size: 16px; font-weight: bold;"
                                                    CssClass="lblCss" Text='<%# Eval("StatusText") %>' ClientIDMode="Static"></asp:Label>
                                            </td>
                                            <th>
                                                Plant
                                            </th>
                                            <td>
                                                <%# Eval("Plant")%>
                                            </td>
                                            <th>
                                                RFD
                                            </th>
                                            <td>
                                                <%# Eval("RFD_Date") %>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:FormView>
                        </div>
                        <div class="lblHeader">
                            ORDER ITEM DETAILS
                        </div>
                        <asp:GridView runat="server" ID="gvOrderItemList" AutoGenerateColumns="false" Width="100%"
                            AlternatingRowStyle-BackColor="#ecf6ff" ShowFooter="true">
                            <HeaderStyle BackColor="#a1ccf3" Font-Bold="true" Height="22px" />
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
                                <asp:BoundField HeaderText="FWT" DataField="tyrewt" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="RIM QTY" DataField="Rimitemqty" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="RIM BASIC PRICE" DataField="Rimunitprice" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="RIM FWT" DataField="Rimfinishedwt" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="DWG NO." DataField="RimDwg" />
                                <asp:TemplateField HeaderText="PART NO">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtPartNo" Text='<%# Eval("ItemCode") %>' Width="100px"
                                            Enabled="true" MaxLength="20"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <FooterStyle BackColor="#fbe6d0" HorizontalAlign="Right" Font-Bold="true" />
                        </asp:GridView>
                        <!-- Total calculation -->
                        <hr style="width: 100%; float: left;" />
                        <div>
                            <table cellspacing="0" rules="all" border="1" class="tableCss1" id="OrderProcessingTable">
                                <!-- Proforma Content 1 -->
                                <tr>
                                    <td colspan="4" style="text-align: right; padding-right: 30px;">
                                        <asp:CheckBox runat="server" ID="chkSpecialOffer" ClientIDMode="Static" Text="" TextAlign="Left"
                                            AutoPostBack="true" OnCheckedChanged="chkSpecialOffer_IndexChanged" />
                                    </td>
                                </tr>
                                <tr id="tr_Proforma1_RefNoModeofTrans" style="display: none;">
                                    <th>
                                        Ref No:
                                    </th>
                                    <td style="text-align: left;">
                                        <span style="font-size: 11px; color: #f00; width: 400px; float: left;">Somecase ref
                                            no may vary in time of proforma preparation</span>
                                        <asp:TextBox runat="server" ID="txtProformaRefNo" ClientIDMode="Static" Text="" Width="200px"
                                            Height="25px" Enabled="false"></asp:TextBox>
                                        REVISE:
                                        <asp:TextBox runat="server" ID="txtProformaReviseNo" ClientIDMode="Static" Text=""
                                            Width="100px" Height="25px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <th>
                                        Mode Of Transport
                                    </th>
                                    <td style="text-align: left;">
                                        <asp:RadioButtonList runat="server" ID="rdbModeOfTransport" ClientIDMode="Static"
                                            RepeatColumns="3" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="BY ROAD" Value="BY ROAD" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="BY TRAIN" Value="BY TRAIN"></asp:ListItem>
                                            <asp:ListItem Text="BY AIR" Value="BY AIR"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <!-- Proforma Content 2 -->
                                <tr id="tr_Proforma2_PaymentOtherCharges" style="display: none;">
                                    <th>
                                        Payment Terms
                                    </th>
                                    <td style="text-align: left;">
                                        <asp:TextBox runat="server" ID="txtPayterms" ClientIDMode="Static" Text="" Enabled="true"
                                            TextMode="MultiLine" Width="525px" Height="200px" onKeyUp="javascript:CheckMaxLength(this, 1999);"
                                            onChange="javascript:CheckMaxLength(this, 1999);"></asp:TextBox>
                                    </td>
                                    <th>
                                        Other Charges
                                    </th>
                                    <td style="text-align: left;">
                                        <table style="width: 420px; text-align: center; border-collapse: collapse;" border="0.5px"
                                            cellpadding="2" id="tr_OtherCharges">
                                            <!--Other Heading -->
                                            <tr>
                                                <td style="width: 70%;">
                                                    Charges Description
                                                </td>
                                                <td style="width: 5%;">
                                                    +/-
                                                </td>
                                                <td style="width: 25%;">
                                                    Amount
                                                </td>
                                            </tr>
                                            <!--Other Charges Heading -->
                                            <tr>
                                                <td colspan="3">
                                                    <div style="text-align: center;">
                                                        <asp:GridView runat="server" ID="gvAmountSub" AutoGenerateColumns="false" Width="420px"
                                                            GridLines="None" HeaderStyle-CssClass="headerNone">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Charges" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="headerCss"
                                                                    ItemStyle-Width="71%">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox runat="server" ID="txtAddDesc" Text="" Width="100%" Height="20px" MaxLength="100"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-Width="5%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblplus" runat="server" Text="+" Width="100%" ToolTip="ADD"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-Width="24%">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox runat="server" ID="txtCAddAmt" Text="" Width="98%" Height="20px" MaxLength="100"
                                                                            onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </td>
                                            </tr>
                                            <!--Claim Adjustment 
                                            <tr>
                                                <td colspan="3" style="text-align: left;">
                                                    <span class="headerCss">Claim Adjustment</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 75%;">
                                                    <asp:TextBox runat="server" ID="txtClaimAdjustment" Text="" Width="100%" Height="20px"
                                                        MaxLength="100"></asp:TextBox>
                                                </td>
                                                <td style="width: 5%;">
                                                    <asp:Label ID="lblLESSclaimAdjus" runat="server" Text="-" Width="100%" ToolTip="LESS"></asp:Label>
                                                </td>
                                                <td style="width: 25%;">
                                                    <asp:TextBox runat="server" ID="txtLESSAMT" onkeypress="return isNumberKey(event)"
                                                        Text="" Width="98%" Height="20px" MaxLength="8"></asp:TextBox>
                                                </td>
                                            </tr>-->
                                            <!--Other Discount 
                                            <tr>
                                                <td colspan="3" style="text-align: left;">
                                                    <span class="headerCss">Other Discount</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 75%;">
                                                    <asp:TextBox runat="server" ID="txtotherdiscount" Text="" Width="100%" Height="20px"
                                                        MaxLength="100"></asp:TextBox>
                                                </td>
                                                <td style="width: 5%;">
                                                    <asp:Label ID="lblLessdiscount" runat="server" Text="-" Width="100%" ToolTip="LESS"></asp:Label>
                                                </td>
                                                <td style="width: 25%;">
                                                    <asp:TextBox runat="server" ID="txtOtherDisAmt" onkeypress="return isNumberKey(event)"
                                                        Text="" Width="98%" Height="20px" MaxLength="8"></asp:TextBox>
                                                </td>
                                            </tr>-->
                                            <!--GST Assign -->
                                            <tr>
                                                <th colspan="3" style="text-align: left;">
                                                    <span class="headerCss">GST Value</span>
                                                </th>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <table style="width: 420px;">
                                                        <tr>
                                                            <td style="background-color: #F1CED5;">
                                                                <div style="float: left;">
                                                                    <div style="float: left; background-color: #ccc; width: 75px;">
                                                                        <asp:CheckBox runat="server" ID="chkCGST" ClientIDMode="Static" Checked="false" Text="CGST %" /></div>
                                                                    <div id="divCGST" style="display: none; float: left;">
                                                                        <div style="float: left;">
                                                                            <asp:TextBox runat="server" ID="txtCGST" ClientIDMode="Static" Text="" Width="60px"
                                                                                onkeypress="return isNumberKey(event)" MaxLength="5" CssClass="txtCss"></asp:TextBox></div>
                                                                    </div>
                                                                </div>
                                                            </td>
                                                            <td style="background-color: #BCE0B8;">
                                                                <div style="float: left;">
                                                                    <div style="float: left; background-color: #ccc; width: 75px;">
                                                                        <asp:CheckBox runat="server" ID="chkSGST" ClientIDMode="Static" Checked="false" Text="SGST %" /></div>
                                                                    <div id="divSGST" style="display: none; float: left;">
                                                                        <div style="float: left;">
                                                                            <asp:TextBox runat="server" ID="txtSGST" ClientIDMode="Static" Text="" Width="60px"
                                                                                onkeypress="return isNumberKey(event)" MaxLength="5" CssClass="txtCss"></asp:TextBox></div>
                                                                    </div>
                                                                </div>
                                                            </td>
                                                            <td style="background-color: #D2C9E8;">
                                                                <div style="float: left;">
                                                                    <div style="float: left; background-color: #ccc; width: 75px;">
                                                                        <asp:CheckBox runat="server" ID="chkIGST" ClientIDMode="Static" Checked="false" Text="IGST %" /></div>
                                                                    <div id="divIGST" style="display: none; width: 100px; float: left;">
                                                                        <asp:TextBox runat="server" ID="txtIGST" ClientIDMode="Static" Text="" Width="60px"
                                                                            onkeypress="return isNumberKey(event)" MaxLength="5" CssClass="txtCss"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <!-- Invoice Content -->
                                <tr id="tr_Invoice_InvoiceDetails" style="display: none;">
                                    <td colspan="4">
                                        <table cellspacing="0" rules="all" border="1" class="tableInvoice">
                                            <tr>
                                                <th>
                                                    Invoice No / Date
                                                </th>
                                                <td>
                                                    <span style="font-size: 11px; color: #f00; width: 400px; float: left;">Somecase invoice
                                                        no may vary in time of preparation</span>
                                                    <asp:Label runat="server" ID="lblInvoiceNo" ClientIDMode="Static" Text=""></asp:Label>
                                                    /
                                                    <asp:Label runat="server" ID="lblInvoiceDate" ClientIDMode="Static" Text=""></asp:Label>
                                                </td>
                                                <td colspan="2" style="text-align: right;">
                                                    <div id="div_addtionalCharge" runat="server">
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>
                                                    Mode of Transport
                                                </th>
                                                <td>
                                                    <asp:Label runat="server" ID="lblModeOfTransport" ClientIDMode="Static" Text=""></asp:Label>
                                                </td>
                                                <th>
                                                    Transporter Name
                                                </th>
                                                <td>
                                                    <asp:Label runat="server" ID="lblTransportName" ClientIDMode="Static" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>
                                                    Contact Person
                                                </th>
                                                <td>
                                                    <asp:Label runat="server" ID="lblContactPerson" ClientIDMode="Static" Text=""></asp:Label>
                                                </td>
                                                <th>
                                                    LR No
                                                </th>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtLRno" ClientIDMode="Static" Text="" Width="300px"
                                                        Height="20px" MaxLength="100" ToolTip="LR No"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>
                                                    Contact No
                                                </th>
                                                <td>
                                                    <asp:Label runat="server" ID="lblContactNo" ClientIDMode="Static" Text=""></asp:Label>
                                                </td>
                                                <th>
                                                    Vehicle No
                                                </th>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtvehicleNo" ClientIDMode="Static" Text="" Width="300px"
                                                        Height="20px" MaxLength="100" ToolTip="Vehicle No"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>
                                                    Irn
                                                </th>
                                                <td colspan="3">
                                                    <asp:Label runat="server" ID="lblIrn" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                                                    <asp:Label runat="server" ID="lblIrnErrMsg" ClientIDMode="Static" Text="" Font-Bold="true"
                                                        ForeColor="Red"></asp:Label>
                                                    <asp:HiddenField runat="server" ID="hdnIrnQrCode" ClientIDMode="Static" Value="" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>
                                                    Ack. No
                                                </th>
                                                <td>
                                                    <asp:Label runat="server" ID="lblAckNo" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                                                </td>
                                                <th>
                                                    Date
                                                </th>
                                                <td>
                                                    <asp:Label runat="server" ID="lblAckDate" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <!-- WorkOrder Content -->
                                <tr id="tr_WorkOrder_WorkOrderDetails" style="display: none;">
                                    <td colspan="4">
                                        <table cellspacing="0" rules="all" border="1" style="line-height: 20px;">
                                            <tr>
                                                <th>
                                                    WorkOrder No
                                                </th>
                                                <td style="text-align: left;">
                                                    <span style="font-size: 11px; color: #f00; width: 400px; float: left;">Somecase ref
                                                        no may vary in time of workorder preparation</span>
                                                    <asp:TextBox runat="server" ID="txtWorkOrderNo" ClientIDMode="Static" Text="" Width="170px"
                                                        Height="20px" ToolTip="WorkOrderNo" Enabled="false"></asp:TextBox>
                                                    REVISE:
                                                    <asp:TextBox runat="server" ID="txtWorkorderReviseNo" ClientIDMode="Static" Text=""
                                                        Width="70px" Height="20px" Enabled="false"></asp:TextBox>
                                                </td>
                                                <th rowspan="3">
                                                    Pre Dispatch
                                                </th>
                                                <td rowspan="3" style="text-align: left;">
                                                    <asp:CheckBoxList ID="chk_preDispatch" RepeatColumns="2" ClientIDMode="Static" RepeatDirection="Horizontal"
                                                        runat="server" ToolTip="pre Dispatch" Width="400px">
                                                        <asp:ListItem Value="1" Text="FUMIGATION"></asp:ListItem>
                                                        <asp:ListItem Value="2" Text="SPIRAL WRAP"></asp:ListItem>
                                                        <asp:ListItem Value="3" Text="SHRINK WRAP"></asp:ListItem>
                                                        <asp:ListItem Value="4" Text="PALLETISATION/CRATE/BOX"></asp:ListItem>
                                                        <asp:ListItem Value="5" Text="MADE IN INDIA MARKING"></asp:ListItem>
                                                        <asp:ListItem Value="6" Text="20-FOOT CONTAINER"></asp:ListItem>
                                                    </asp:CheckBoxList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>
                                                    Expected Shipping Date
                                                </th>
                                                <td style="text-align: left;">
                                                    <asp:TextBox runat="server" ID="txtExpectedShippingDate" ClientIDMode="Static" Text=""
                                                        Width="300px" Height="20px" ToolTip="Expected Date"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>
                                                    Delivery Priority
                                                </th>
                                                <td style="text-align: left;">
                                                    <asp:DropDownList ID="ddl_deliverypriority" ClientIDMode="Static" runat="server"
                                                        Width="300px" Height="25px" ToolTip="Priority">
                                                        <asp:ListItem Text="AS PER CUSTOMER SCHEDULE" Value="AS PER CUSTOMER SCHEDULE"></asp:ListItem>
                                                        <asp:ListItem Text="WITH IN 24 HOURS" Value="WITH IN 24 HOURS"></asp:ListItem>
                                                        <asp:ListItem Text="WITH IN 48 HOURS" Value="WITH IN 48 HOURS"></asp:ListItem>
                                                        <asp:ListItem Text="WITH IN 1 WEEK" Value="WITH IN 1 WEEK"></asp:ListItem>
                                                        <asp:ListItem Text="WITH IN 3-4 WEEK" Value="WITH IN 3-4 WEEK"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <!-- Tcs Content -->
                                <tr id="tr_Tcs_Details" style="display: none;">
                                    <td colspan="4">
                                        <table cellspacing="0" rules="all" border="1" class="tableInvoice">
                                            <tr>
                                                <th style="width: 300px;">
                                                    Whether the amount received after 01-oct-2020 exceeds Rs.50 Lakhs.
                                                </th>
                                                <td>
                                                    <asp:RadioButtonList runat="server" ID="rdbTcsAmt" RepeatColumns="2" RepeatDirection="Horizontal"
                                                        Width="100px">
                                                        <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th style="width: 300px;">
                                                    Whether Having Valid PAN No.
                                                </th>
                                                <td>
                                                    <asp:RadioButtonList runat="server" ID="rdbTcsPan" RepeatColumns="2" RepeatDirection="Horizontal"
                                                        Width="100px">
                                                        <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <!-- Comments -->
                                <tr id="tr_comments">
                                    <th>
                                        Comments
                                    </th>
                                    <td colspan="3" style="text-align: left;">
                                        <asp:TextBox runat="server" ID="txtcomments" ClientIDMode="Static" Text="" Enabled="true"
                                            TextMode="MultiLine" Width="900px" Height="60px" onKeyUp="javascript:CheckMaxLength(this, 499);"
                                            onChange="javascript:CheckMaxLength(this, 499);"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <hr style="width: 100%; float: left;" />
                        <div>
                            <table cellspacing="0" rules="all" border="0" style="border-collapse: collapse; border-color: #fff;
                                width: 100%; text-align: center;">
                                <tr>
                                    <td colspan="4">
                                        <asp:Label ID="lblErrMsg" ForeColor="Red" ClientIDMode="Static" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblPrepareMessage" ClientIDMode="Static" CssClass="spanCss" ForeColor="#614126"
                                            runat="server" Text=""></asp:Label><br />
                                        <a id="lnkPrepare" runat="server">
                                            <asp:ImageButton ID="imgPrepare" ImageUrl="~/images/Prepare3.png" Width="50px" Height="50px"
                                                runat="server" ClientIDMode="Static" CssClass="imageCss" OnClick="imgPrepare_Click"
                                                OnClientClick="javascript:return CntrlPreparePDF();" />
                                        </a>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblEmailsendMessage" ClientIDMode="Static" CssClass="spanCss" ForeColor="#614126"
                                            runat="server" Text="No need to mail sent"></asp:Label><br />
                                        <a id="lnkEmail" runat="server">
                                            <asp:ImageButton ID="imgEmail" ImageUrl="~/images/Email4.png" runat="server" ClientIDMode="Static"
                                                CssClass="imageCss" Height="50px" Width="50px" OnClick="imgEmail_Click" OnClientClick="javascript:return cntrlEmailSend();" />
                                        </a>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblUploadMessage" ClientIDMode="Static" CssClass="spanCss" ForeColor="#614126"
                                            runat="server" Text="No need to upload"></asp:Label><br />
                                        <a id="lnkUpload" runat="server">
                                            <asp:ImageButton ID="imgUpload" ImageUrl="~/images/Upload.png" runat="server" ClientIDMode="Static"
                                                CssClass="imageCss" />
                                        </a>
                                        <div id="div_FileUpload" style="display: none;">
                                            <asp:FileUpload ID="FileUploadControl" runat="server" />
                                            <asp:Button runat="server" ID="btnUpload" ClientIDMode="Static" Text="Upload" OnClick="btnUpload_Click" />
                                        </div>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_downloadMessage" ClientIDMode="Static" CssClass="spanCss" ForeColor="#614126"
                                            runat="server" Text="No files to download"></asp:Label><br />
                                        <a id="linkdowload" runat="server">
                                            <asp:ImageButton ID="imgdownload" ImageUrl="images/Download.png" ClientIDMode="Static"
                                                runat="server" CssClass="imageCss" />
                                        </a>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnBacktoHome" runat="server" Text="Move Home" CssClass="button"
                                            BackColor="#4e7e9a" OnClick="btnBacktoHome_Click" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnBack" runat="server" Text="Move Back" CssClass="button" BackColor="#4cafaa"
                                            OnClick="btnBack_Click" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnMoveOrder" runat="server" Text="" ClientIDMode="Static" CssClass="button"
                                            OnClick="btnMoveOrder_Click" OnClientClick="javascript:return CntrlMoveOrder();" />
                                    </td>
                                    <td align="center">
                                        <asp:Button ID="btnClearOrder" runat="server" Text="Clear Order" CssClass="button"
                                            BackColor="#ec971f" OnClick="btnClearOrder_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <!-- Popup Window -->
    <div id="dialog" style="display: none;">
        <asp:GridView runat="server" ID="gv_DownloadFiles" ClientIDMode="Static" AutoGenerateColumns="false">
            <HeaderStyle CssClass="headerNone" />
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Label ID="lblFileType" runat="server" Text='<%# Eval("FileType")%>' ForeColor="#a52a2a">
                        </asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkPdfFileName" runat="server" Text='<%# Eval("AttachFileName")%>'
                            CssClass="filelnk" OnClick="ddl_DownloadFiles_ItemCommand">
                        </asp:LinkButton></ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <asp:HiddenField ID="hdnCustCode" ClientIDMode="Static" runat="server" Value="" />
    <asp:HiddenField ID="hdnOrderRefNo" ClientIDMode="Static" runat="server" Value="" />
    <asp:HiddenField ID="hdnCustCategory" ClientIDMode="Static" runat="server" Value="" />
    <asp:HiddenField ID="hdnorderPlant" ClientIDMode="Static" runat="server" Value="" />
    <asp:HiddenField ID="hdnPartNo" ClientIDMode="Static" runat="server" Value="" />
    <asp:HiddenField ID="hdnShipID" ClientIDMode="Static" runat="server" Value="" />
    <asp:HiddenField ID="hdnQstring" ClientIDMode="Static" runat="server" Value="" />
    <asp:HiddenField runat="server" ID="hdnStatusID" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnFreightChanges" ClientIDMode="Static" Value="" />
    <script src="Scripts/datemonthyear.js" type="text/javascript"></script>
    <script src="Scripts/TinyboxScript.js" type="text/javascript"></script>
    <script src="Scripts/scotsdomestic.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            blink('#lblCurrentStatus');
            $(':text').bind('keydown', function (e) { if (e.keyCode == 13) { e.preventDefault(); return false; } });
            $("#txtExpectedShippingDate").datepicker({ minDate: "+0D", maxDate: "+90D" }).keydown(function (e) { e.keyWhich = 0; e.keyCode = 0; e.preventDefault(); });

            //To open Dialog for download
            $("#imgdownload").click(function (e) {
                var x = $(this).position();
                $("#dialog").dialog({ title: "Download PDF Files", modal: true, width: '600px', top: '1000px', left: '500px',
                    buttons: [{ id: "Close", text: "Close", click: function () { $(this).dialog('close'); } }]
                });
                return false;
            });

            $("input:checkbox[id*=chk]").click(function (e) { fnCheckbox_Click(e.target.id); }).each(function () { fnCheckbox_Click(this.id); });

            //cntrl to Upload Button
            $('#imgUpload').click(function () {
                if ($('#hdnQstring').val() == "invoice")
                    $('input[type=file]').trigger('click');
                return false;
            });

            $('input[type=file]').change(function () {
                $('#btnUpload').trigger('click');
            });
        });

        function blink(element) { $(element).fadeToggle('slow', function () { blink(this); }); }

        function fnCheckbox_Click(ctrlID) {
            if (ctrlID == "chkCGST")
                chktxtEnableDisable('chkCGST', 'divCGST', 'txtCGST');
            if (ctrlID == "chkSGST")
                chktxtEnableDisable('chkSGST', 'divSGST', 'txtSGST');
            if (ctrlID == "chkIGST")
                chktxtEnableDisable('chkIGST', 'divIGST', 'txtIGST');
        }
        function chktxtEnableDisable(chkID, divID, txtID) {
            if ($('#' + chkID).attr('checked') == "checked") {
                $('#' + divID).css({ 'display': 'block' });
                $('#' + txtID).focus();
            }
            else
                $('#' + divID).css({ 'display': 'none' });
        }

        function CntrlMoveOrder() {
            var Qstring = $('#hdnQstring').val();
            var ErrMsg = "";
            $('#lblErrMsg').html("");
            if (!$('#imgPrepare').prop('disabled')) {
                if (Qstring == "proforma")
                    ErrMsg += "Yet to create Proforma.. <br/>"
                else if (Qstring == "invoice")
                    ErrMsg += "Yet to create Invoice.. <br/>"
                else if (Qstring == "workorder")
                    ErrMsg += "Yet to create WorkOrder.. <br/>"
            }
            if (!$('#imgEmail').prop('disabled')) {
                if (Qstring == "proforma")
                    ErrMsg += "Yet to send Customer conformation Mail.. <br/>"
                else if (Qstring == "workorder")
                    ErrMsg += "Yet to send conformation Mail.. <br/>"
            }
            if (Qstring == "production") {
                if (!$('#imgUpload').prop('disabled'))
                    ErrMsg += "Yet to upload stencilNo <br/>";
            }
            if (Qstring == "payment") {
                if ($('#txtcomments').val().trim().length == 0)
                    ErrMsg += 'Enter comments';
            }
            if (Qstring == "tcs") {
                if ($("input:radio[id*=rdbTcsAmt]:checked").length == 0)
                    ErrMsg += 'Confirm Tcs Applicable <br/>';
                else if ($("input:radio[id*=rdbTcsAmt]:checked").val() == "1") {
                    if ($("input:radio[id*=rdbTcsPan]:checked").length == 0)
                        ErrMsg += 'Confirm Pan Validation';
                }
                if ($('#txtcomments').val().trim().length == 0)
                    ErrMsg += 'Enter comments';
            }
            if (ErrMsg.length > 0) { $('#lblErrMsg').html(ErrMsg); return false; } else { return true; }
        }

        function CntrlPreparePDF() {
            var ErrMsg = ""; $('#lblErrMsg').html("");
            if ($('#hdnQstring').val() == "proforma") {
                if ($('input:text[id^=MainContent_gvOrderItemList_txtPartNo_]').length > 0) {
                    $('input:text[id^=MainContent_gvOrderItemList_txtPartNo_]').each(function () { if ($(this).val().length == 0) { ErrMsg += 'Please enter Part No.. <br/>'; } });
                }
                if ($("input:radio[id*=rdbModeOfTransport]:checked").length == 0)
                    ErrMsg += 'Choose mode of transport <br>';
                else if ($("input:radio[id*=rdbModeOfTransport]:checked").val() == "BY TRAIN" && $('#hdnFreightChanges').val() == "PAID") {
                    var str2 = "freight";
                    if (($('#MainContent_gvAmountSub_txtAddDesc_0').val().toLowerCase()).indexOf(str2) == -1 &&
                     ($('#MainContent_gvAmountSub_txtAddDesc_0').val().toLowerCase()).indexOf(str2) == -1 &&
                     ($('#MainContent_gvAmountSub_txtAddDesc_0').val().toLowerCase()).indexOf(str2) == -1) {
                        ErrMsg += 'Enter proper freight charges amount <br>';
                        $('#MainContent_gvAmountSub_txtAddDesc_0').focus();
                    }
                }
                $('#MainContent_gvAmountSub tr').each(function () {
                    var amount = $(this).find('td:eq(2)').find('input:text').val();
                    if (parseFloat(amount) == 0)
                        ErrMsg += 'Other Charges amount should be greater than 0 <br>';
                });
                if ($('#hdnShipID').val() != 189 && $('#hdnShipID').val() != 960 && $('#hdnShipID').val() != 1071 && $('#hdnShipID').val() != 1076 &&
                $('#hdnShipID').val() != 1168 && $('#hdnShipID').val() != 2340 && $('#hdnShipID').val() != 4594 && $('#hdnShipID').val() != 4616 &&
                $('#hdnShipID').val() != 4824 && $('#hdnShipID').val() != 8016 && $('#hdnShipID').val() != 8033 && $('#hdnShipID').val() != 8044 &&
                $('#hdnShipID').val() != 11193 && $('#hdnShipID').val() != 11262 && $('#hdnShipID').val() != 11429 && $('#hdnShipID').val() != 11432 && $('#hdnShipID').val() != 12620 && $('#hdnShipID').val() != 12728) {
                    if ($("input:checkbox[id*=chk]:checked").length == 0)
                        ErrMsg += "check atleast one GST Value<br/>";
                    else {
                        if ($('#chkCGST').attr('checked') == 'checked' && $('#txtCGST').val().trim().length == 0)
                            ErrMsg += "Enter CGST % <br/>";
                        else if ($('#chkCGST').attr('checked') == 'checked' && parseFloat($('#txtCGST').val()) == 0)
                            ErrMsg += "CGST Value must greater than 0 <br/>";
                        if ($('#chkSGST').attr('checked') == 'checked' && $('#txtSGST').val().trim().length == 0)
                            ErrMsg += "Enter SGST % <br/>";
                        else if ($('#chkSGST').attr('checked') == 'checked' && parseFloat($('#txtSGST').val()) == 0)
                            ErrMsg += "SGST Value must greater than 0 <br/>";
                        if ($('#chkIGST').attr('checked') == 'checked' && $('#txtIGST').val().trim().length == 0)
                            ErrMsg += "Enter IGST % <br/>";
                        else if ($('#chkIGST').attr('checked') == 'checked' && parseFloat($('#txtIGST').val()) == 0)
                            ErrMsg += "IGST Value must greater than 0 <br/>";
                    }
                }
            }
            else if ($('#hdnQstring').val() == "workorder") {
                if ($('#txtExpectedShippingDate').val().length == 0)
                    ErrMsg += "Please Choose Expected Shipping Date <br/>";
                if ($("input:checkbox[id*=chk_preDispatch_]:checked").length == 0)
                    ErrMsg += 'Choose atleast one PreDispatch <br>';
                if ($("#ddl_deliverypriority option:selected").val() == "Choose")
                    ErrMsg += 'Choose Priority <br>';
            }
            else if ($('#hdnQstring').val() == "invoice") {
                if ($('#txtLRno').val().length == 0)
                    ErrMsg += "Please enter LR No <br/>";
                if ($('#txtvehicleNo').val().length == 0)
                    ErrMsg += "Please enter VehicleNo <br/>";
            }
            if (ErrMsg.length > 0) { $('#lblErrMsg').html(ErrMsg); return false; } else { return true; }
        }

        function cntrlEmailSend() {
            var ErrMsg = ""; $('#lblErrMsg').html("");
            if (!$('#imgPrepare').prop('disabled')) {
                if ($('#hdnQstring').val() == "proforma")
                    ErrMsg += "Yet to create Proforma.. <br/>"
                if ($('#hdnQstring').val() == "workorder")
                    ErrMsg += "Yet to create WorkOrder.. <br/>"
            }
            if (ErrMsg.length > 0) { $('#lblErrMsg').html(ErrMsg); return false; } else { return true; }
        }

        function writeErrMsg(errStr) {
            $('#tr_Invoice_InvoiceDetails').show();
            $('#lblIrnErrMsg').html(errStr);
        }
    </script>
</asp:Content>
