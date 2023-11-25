<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="cotsftinvoice.aspx.cs" Inherits="TTS.cotsftinvoice" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/datemonthyear.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .head1
        {
            width: 145px;
            float: left;
            color: #102320;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <script src="Scripts/cotsScript.js" type="text/javascript"></script>
    <div style="text-align: center;" class="pageTitleHead">
        <asp:Label runat="server" ID="lblHeadPlant" Text=""></asp:Label>
        FITMENT ORDER INVOICE PREPARATION</div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table>
            <tr>
                <td>
                    <asp:GridView runat="server" ID="gvFtList" AutoGenerateColumns="false" Width="1074px"
                        RowStyle-Height="20px">
                        <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" Height="22px" />
                        <Columns>
                            <asp:TemplateField HeaderText="CUSTOMER NAME" ItemStyle-Width="250px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatusCustName" ClientIDMode="Static" Text='<%#Eval("custfullname")%>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="FITMENT ORDER NO" ItemStyle-Width="70px">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdnTaxNo" runat="server" ClientIDMode="Static" Value=' <%#Eval("ServiceTaxno")%>' />
                                    <asp:HiddenField ID="hdnPanno" runat="server" ClientIDMode="Static" Value=' <%#Eval("PanNo")%>' />
                                    <asp:HiddenField ID="hdnCustCode" runat="server" ClientIDMode="Static" Value='<%#Eval("CustCode")%>' />
                                    <asp:HiddenField ID="hdnBillId" runat="server" ClientIDMode="Static" Value=' <%#Eval("BillAddId")%>' />
                                    <asp:Label runat="server" ID="lblFtNo" ClientIDMode="Static" Text='<%#Eval("FtNo")%>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ORDER RECEIVED DATE" ItemStyle-Width="80px">
                                <ItemTemplate>
                                    <%#Eval("CreateDate")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CHALLAN / GATE PASS NO" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <%#Eval("ChallanRefno")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CHALLAN / GATE PASS DATE" ItemStyle-Width="80px">
                                <ItemTemplate>
                                    <%#Eval("ChallanDate")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="RECEIVED QTY" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <%#Eval("ReceivedQty")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DISPATCHED QTY" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <%# Convert.ToInt32(Eval("ReceivedQty")) - Convert.ToInt32(Eval("avaQty"))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="BALANCE QTY" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <%#Eval("avaQty")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ACTION" ItemStyle-Width="80px">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkInvoiceBtn" runat="server" Text="Prepare Invoice" OnClick="lnkInvoiceBtn_Click" /></span>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="width: 1074px; float: left; border: 1px solid #000; display: none; margin-top: 10px;
                        background-color: #F0E2F5; padding-top: 5px;" id="divStatusChange">
                        <div id="divOrderHead" style="width: 1063px; float: left;">
                            <div style="width: 530px; float: left; text-align: left;">
                                <asp:Label runat="server" ID="lblCustName" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                            </div>
                            <div style="width: 530px; float: left; text-align: right;">
                                <asp:Label runat="server" ID="lblStausOrderRefNo" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label></div>
                        </div>
                        <div style="width: 1063px; float: left;">
                            <div style="width: 530px; float: left; line-height: 20px;">
                                <span class="headCss">BILL TO: </span>
                                <asp:Label ID="lblbillTo" runat="server" ClientIDMode="Static" Text=""></asp:Label>
                            </div>
                            <div style="width: 530px; float: left; line-height: 20px;">
                                <asp:Label ID="lbltaxno" runat="server" ClientIDMode="Static" Text=""></asp:Label>
                                <br />
                                <asp:Label ID="lblPanno" runat="server" ClientIDMode="Static" Text=""></asp:Label>
                            </div>
                        </div>
                        <asp:GridView runat="server" ID="gvOtherFtlist" AutoGenerateColumns="false" Width="1062px"
                            RowStyle-Height="20px">
                            <HeaderStyle BackColor="#FCEDC7" Font-Bold="true" Height="22px" />
                            <Columns>
                                <asp:TemplateField HeaderText="" ItemStyle-Width="10px">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkftno" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="FITMENT ORDER NO" ItemStyle-Width="70px">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblFtNo" ClientIDMode="Static" Text='<%#Eval("FtNo")%>'></asp:Label></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="CHALLAN / GATE PASS NO" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <%#Eval("ChallanRefno")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="RECEIVED QTY" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <%#Eval("ReceivedQty")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="DISPATCHED QTY" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <%# Convert.ToInt32(Eval("ReceivedQty")) - Convert.ToInt32(Eval("avaQty"))%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="BALANCE QTY" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <%#Eval("avaQty")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="COMMENTS / INSTRUCTION" ItemStyle-Width="400px">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblcomments" ClientIDMode="Static" Text='<%#Eval("Comments").ToString().Replace("~","<br/>")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <div style="width: 400px; float: right">
                            <asp:Button ID="btnpricelist" runat="server" ClientIDMode="Static" Text="SHOW ITEM LIST"
                                OnClick="btnpricelist_Click" CssClass="btnedit" />
                        </div>
                    </div>
                    <div style="width: 1060px; float: left; padding: 2px; background-color: #DDCFCF;
                        margin-right: 5px; display: none;" id="divInvoiceDetails">
                        <div>
                            <asp:GridView runat="server" ID="gvItemlist" AutoGenerateColumns="false" Width="1060px"
                                RowStyle-Height="20px">
                                <HeaderStyle BackColor="#D7F785" Font-Bold="true" Height="22px" />
                                <Columns>
                                    <asp:TemplateField HeaderText="FITMENT ORDER NO" ItemStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblFtno" ClientIDMode="Static" Text='<%#Eval("Ftno")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="TYRE SIZE" ItemStyle-Width="200px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lbltyresize" ClientIDMode="Static" Text='<%#Eval("tyresize")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="RECEIVED QTY" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblRecQty" ClientIDMode="Static" Text='<%#Eval("ReceivedQty")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="DISPATCHED QTY" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <%# Convert.ToInt32(Eval("ReceivedQty")) - Convert.ToInt32(Eval("avaQty"))%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="AVAILABLE QTY" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblAvaQty" Text='<%#Eval("AvaQty")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="FITMENT CHARGE" ItemStyle-Width="80px">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtRate" runat="server" Text='<%#Eval("Rate")%>' Width="80px" onkeyup="FtQty(this)"
                                                onkeypress="return isNumberKey(event)"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="CURRENT DISPATCH QTY" ItemStyle-Width="60px">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtcurrentDispatch" runat="server" Text="" Width="60px" onkeyup="FtQty(this)"
                                                onkeypress="return isNumberKey(event)"></asp:TextBox>
                                            <div style="width: 100px; float: left;">
                                                <asp:Label runat="server" ID="ctrllblQtyErrMsg" Text=""></asp:Label></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="TOTAL FITMENT CHARGE" ItemStyle-Width="80px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblTotalFtCharge" Text=""></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="BALANCE QTY" ItemStyle-Width="60px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblBalQty" Text=""></asp:Label>
                                            <asp:HiddenField ID="hdnbalQty" runat="server" Value='' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #000;
                            width: 1060px; line-height: 20px; margin-top: 5px;">
                            <tr>
                                <td colspan="2">
                                    <div class="head1">
                                        DELIVERY METHOD:
                                    </div>
                                    <div style="width: 350px; float: left;">
                                        <asp:RadioButtonList ID="rdbDeliveryMethod" runat="server" ClientIDMode="Static"
                                            RepeatDirection="Horizontal">
                                            <asp:ListItem Text="COLLECTED BY CUSTOMER" Value="COLLECTED BY"></asp:ListItem>
                                            <asp:ListItem Text="SENT TO CUSTOMER" Value="SENT TO"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    <div id="divDeliveryMethod" runat="server" clientidmode="Static" style="display: none;
                                        width: 300px; float: left; line-height: 10px; background-color: #FCEDC7; padding-left: 5px;">
                                        <div style="width: 300px; float: left;">
                                            <span class="headCss" style="width: 90px; float: left; line-height: 25px;">LOCATION
                                                :</span>
                                            <asp:RadioButtonList ID="rdbLocation" runat="server" ClientIDMode="Static" RepeatDirection="Horizontal"
                                                Width="150px">
                                                <asp:ListItem Text="LOCAL" Value="LOCAL"></asp:ListItem>
                                                <asp:ListItem Text="OTHERS" Value="OTHERS"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                        <div style="width: 300px; float: left;">
                                            <span class="headCss" style="width: 90px; float: left; line-height: 25px;">DELIVERY
                                                TO :</span>
                                            <asp:RadioButtonList ID="rdbDeliveryTo" runat="server" ClientIDMode="Static" RepeatDirection="Horizontal"
                                                Width="150px">
                                                <asp:ListItem Text="DOOR" Value="DOOR"></asp:ListItem>
                                                <asp:ListItem Text="GODOWN" Value="GODOWN"></asp:ListItem>
                                            </asp:RadioButtonList>
                                            <div id="divgodownname" runat="server" clientidmode="Static" style="width: 300px;
                                                float: left; display: none;">
                                                <span class="headCss">GODOWN NAME:</span>
                                                <asp:TextBox ID="txtGodownname" runat="server" ClientIDMode="Static" Text="" Width="295px"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </td>
                                <td rowspan="3" style="vertical-align: top; text-align: center; width: 250px;">
                                    <div id="divGST" style="line-height: 15px; width: 242px; text-align: left">
                                        <table>
                                            <tr>
                                                <td style="height: 22px;">
                                                    <span class="headCss">TOTAL :</span>
                                                    <asp:Label runat="server" ID="lblSumTotFC" ClientIDMode="Static" Text="" Font-Bold="true"
                                                        ForeColor="DarkBlue"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th style="text-align: left; width: 237px;">
                                                    GST VALUE
                                                </th>
                                            </tr>
                                            <tr>
                                                <td style="background-color: #F1CED5;">
                                                    <div style="float: left;">
                                                        <div style="float: left; background-color: #ccc; width: 75px;">
                                                            <asp:CheckBox runat="server" ID="chkCGST" ClientIDMode="Static" Text="CGST %" /></div>
                                                        <div id="divCGST" style="display: none; float: left; width: 160px;">
                                                            <div style="float: left;">
                                                                <asp:TextBox runat="server" ID="txtCGST" ClientIDMode="Static" Text="" Width="60px"
                                                                    onkeypress="return isNumberKey(event)" MaxLength="5" CssClass="txtCss" Style="margin-left: 10px;
                                                                    float: left;"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lblCGST" ClientIDMode="Static" Width="74px" Text=""
                                                                    BorderWidth="1" BackColor="White" Height="17px" Style="text-align: right; margin-left: 10px;
                                                                    display: block; float: left;">
                                                                </asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="background-color: #BCE0B8;">
                                                    <div style="float: left;">
                                                        <div style="float: left; background-color: #ccc; width: 75px;">
                                                            <asp:CheckBox runat="server" ID="chkSGST" ClientIDMode="Static" Text="SGST %" /></div>
                                                        <div id="divSGST" style="display: none; float: left; width: 160px;">
                                                            <div style="float: left;">
                                                                <asp:TextBox runat="server" ID="txtSGST" ClientIDMode="Static" Text="" Width="60px"
                                                                    onkeypress="return isNumberKey(event)" MaxLength="5" CssClass="txtCss" Style="margin-left: 10px;
                                                                    float: left;"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lblSGST" ClientIDMode="Static" Width="74px" Text=""
                                                                    BorderWidth="1" BackColor="White" Height="17px" Style="text-align: right; margin-left: 10px;
                                                                    display: inline-block; float: left;">
                                                                </asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="background-color: #D2C9E8;">
                                                    <div style="float: left;">
                                                        <div style="float: left; background-color: #ccc; width: 75px;">
                                                            <asp:CheckBox runat="server" ID="chkIGST" ClientIDMode="Static" Text="IGST %" /></div>
                                                        <div id="divIGST" style="display: none; float: left; width: 160px;">
                                                            <asp:TextBox runat="server" ID="txtIGST" ClientIDMode="Static" Text="" Width="60px"
                                                                onkeypress="return isNumberKey(event)" MaxLength="5" CssClass="txtCss" Style="margin-left: 10px;
                                                                float: left;"></asp:TextBox>
                                                            <asp:Label runat="server" ID="lblIGST" ClientIDMode="Static" Width="74px" Text=""
                                                                BorderWidth="1" BackColor="White" Height="17px" Style="text-align: right; margin-left: 10px;
                                                                display: inline-block; float: left;">
                                                            </asp:Label>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 22px;">
                                                    <span class="headCss">TOTAL TAX VALUE :</span>
                                                    <asp:Label runat="server" ID="lblTotTaxValue" ClientIDMode="Static" Text="" Font-Bold="true"
                                                        ForeColor="DarkBlue"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 22px;">
                                                    <span class="headCss">GRAND TOTAL VALUE :</span>
                                                    <asp:Label runat="server" ID="lblGrandTotal" ClientIDMode="Static" Text="" Font-Bold="true"
                                                        ForeColor="DarkBlue"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top;">
                                    <div style="line-height: 25px;">
                                        <div style="display: none;" id="divVehicle">
                                            <span class="head1">VEHICLE REGN NO:</span>
                                            <asp:TextBox runat="server" ID="txtVechicleNo" ClientIDMode="Static" Text="" Width="230px"></asp:TextBox>
                                        </div>
                                        <div>
                                            <span class="head1" id="personhead">PERSON NAME:</span>
                                            <asp:TextBox runat="server" ID="txtContactPerson" ClientIDMode="Static" Text="" Width="230px"
                                                MaxLength="50"></asp:TextBox>
                                        </div>
                                        <div>
                                            <span class="head1">MOBILE NO:</span>
                                            <asp:TextBox runat="server" ID="txtContactNo" ClientIDMode="Static" Text="" Width="230px"
                                                MaxLength="20"></asp:TextBox>
                                        </div>
                                    </div>
                                </td>
                                <td style="vertical-align: top;">
                                    <div style="line-height: 25px; display: none;" id="divLr">
                                        <div>
                                            <span class="head1">TRANSPORTER NAME:</span>
                                            <asp:TextBox runat="server" ID="txtTrasporterName" ClientIDMode="Static" Text=""
                                                Width="230px"></asp:TextBox>
                                        </div>
                                        <div>
                                            <span class="head1">LR NO:</span>
                                            <asp:TextBox runat="server" ID="txtLrNo" ClientIDMode="Static" Text="" Width="230px"
                                                MaxLength="50"></asp:TextBox>
                                        </div>
                                        <div>
                                            <span class="head1">LR DATE:</span>
                                            <asp:TextBox runat="server" ID="txtLrDate" ClientIDMode="Static" Text="" Width="100px"
                                                MaxLength="20"></asp:TextBox>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="vertical-align: top;">
                                    <div>
                                        <div class="head1">
                                            COMMENTS:</div>
                                        <asp:TextBox runat="server" ID="txtComments" ClientIDMode="Static" Text="" TextMode="MultiLine"
                                            onKeyUp="javascript:CheckMaxLength(this, 999);" onChange="javascript:CheckMaxLength(this, 999);"
                                            Width="645px" Height="80px"></asp:TextBox>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="text-align: center">
                                    <div id="divErrMsg" style="width: 250px; color: #f00; line-height: 15px; text-align: left;
                                        margin-left: 10px;">
                                    </div>
                                    <br />
                                    <asp:Button ID="btnInvoiceprepare" runat="server" ClientIDMode="Static" Text="PREPARE FITMENT INVOICE"
                                        OnClick="btnInvoiceprepare_Click" OnClientClick="javascript:return CtrlValidate();"
                                        CssClass="btnsave" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="width: 1063px; float: left; margin-top: 5px; color: #238d08; font-weight: bold;
                        font-size: 16px; text-align: center; background-color: #65ebeb;">
                        <asp:Label runat="server" ID="lblAlreadyPreapre" Text=""></asp:Label>
                    </div>
                    <div style="width: 1072px; float: left; padding-top: 5px;">
                        <asp:GridView runat="server" ID="gvFtInvoiceDownload" AutoGenerateColumns="false"
                            Width="1072px" RowStyle-Height="20px">
                            <HeaderStyle BackColor="#AAD8A6" Font-Bold="true" Height="22px" />
                            <Columns>
                                <asp:TemplateField HeaderText="INVOICE NO" ItemStyle-Width="20px">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblDownloadInvoiceNo" Text='<%# Eval("Invoiceno")%>'></asp:Label>
                                        <asp:HiddenField runat="server" ID="hdnDownloadUInvoiceYear" Value='<%# Eval("InvoiceYear") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="INVOICE DETAILS" ItemStyle-Width="570px" ItemStyle-VerticalAlign="Top">
                                    <ItemTemplate>
                                        <div style="width: 570px; float: left; line-height: 20px;">
                                            <div style="width: 400px; float: left;">
                                                <%# Eval("ContactPerson").ToString() == "" ? "" : "<span class='headCss' style='width: 130px; float: left;'>CONTACT PERSON :</span>" + Eval("ContactPerson").ToString() + " <br />"%>
                                                <%# Eval("Contactno").ToString() == "" ? "" : "<span class='headCss' style='width: 130px; float: left;'>MOBILE NO :</span>" + Eval("Contactno").ToString() + " <br />"%>
                                                <%# Eval("DispatchMethod").ToString() == "" ? "" : "<span class='headCss' style='width: 130px; float: left;'>DELIVERY METHOD :</span>" + Eval("DispatchMethod").ToString() + " CUSTOMER <br />"%>
                                                <%# Eval("VehicleNo").ToString() == "" ? "" : "<span class='headCss' style='width: 130px; float: left;'>VEHICLE REGN NO :</span>" + Eval("VehicleNo").ToString() + " <br />"%>
                                                <%# Eval("DeliveryTo").ToString() == "" ? "" : "<span class='headCss' style='width: 130px; float: left;'>DELIVERY TO :</span>" + Eval("DeliveryTo").ToString() + (Eval("GoDown").ToString()!=""?" ("+ Eval("GoDown").ToString()+")":"")+" <br />"%>
                                                <%# Eval("Transpoter").ToString() == "" ? "" : "<span class='headCss' style='width: 130px; float: left;'>TRANSPORTER :</span>" + Eval("Transpoter").ToString() + " <br />"%>
                                                <%# Eval("LrNo").ToString() == "" ? "" : "<span class='headCss' style='width: 130px; float: left;'>LR NO & DATE :</span>" + Eval("LrNo").ToString() + " & " + Eval("LrDate").ToString() + "<br />"%>
                                            </div>
                                            <div style="width: 170px; float: left;">
                                                <%# Eval("TotalQty").ToString() == "" ? "" : "<span class='headCss' style='width: 100px; float: left;'>TOTAL QTY:</span>" + Eval("TotalQty").ToString() + " <br />"%>
                                                <%# Eval("Total").ToString() == "" ? "" : "<span class='headCss' style='width: 100px; float: left;'>TOTAL AMT :</span>" + Eval("Total").ToString() + " <br />"%>
                                                <%--<%# Eval("servicetax").ToString() == "" ? "" : "<span class='headCss' style='width: 100px; float: left;'>SERVICE TAX @" + Eval("ServicetaxPercent").ToString() + "% :</span>" + Eval("servicetax").ToString() + " <br />"%>--%>
                                                <%# Eval("CGST").ToString() == "" ? "0.00" : "<span class='headCss' style='width: 100px; float: left;'>CGST @" + Eval("CGST").ToString() + "% :</span>" + Eval("CGSTVal").ToString() + " <br />"%>
                                                <%# Eval("SGST").ToString() == "0.00" ? "" : "<span class='headCss' style='width: 100px; float: left;'>SGST @" + Eval("SGST").ToString() + "% :</span>" + Eval("SGSTVal").ToString() + " <br />"%>
                                                <%# Eval("IGST").ToString() == "0.00" ? "" : "<span class='headCss' style='width: 100px; float: left;'>IGST @" + Eval("IGST").ToString() + "% :</span>" + Eval("IGST").ToString() + " <br />"%>
                                                <%# Eval("GrandTotal").ToString() == "" ? "" : "<span class='headCss' style='width: 100px; float: left;'>GRAND TOTAL :</span>" + Eval("GrandTotal").ToString() + " <br />"%>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="COMMENTS" ItemStyle-Width="300px" ItemStyle-VerticalAlign="Top">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblcomments" ClientIDMode="Static" Text='<%#Eval("Comments").ToString().Replace("~","<br/>")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="DOWNLOAD" ItemStyle-Width="120px" ItemStyle-VerticalAlign="Top">
                                    <ItemTemplate>
                                        <div style="line-height: 20px;">
                                            <asp:LinkButton runat="server" ID="Original_Recepient_Invoice" Text="ORIGINAL FOR RECEPIENT"
                                                ClientIDMode="Static" OnClick="lnkInvoiceFile_Click" BackColor="#fcd7d7"></asp:LinkButton>
                                            <br />
                                            <asp:LinkButton runat="server" ID="Duplicate_Transporter_Invoice" Text="DUPLICATE FOR TRANSPORTER"
                                                ClientIDMode="Static" OnClick="lnkInvoiceFile_Click" BackColor="#93f5f5"></asp:LinkButton>
                                            <br />
                                            <asp:LinkButton runat="server" ID="Triplicate_Supplier_Invoice" Text="TRIPLICATE FOR SUPPLIER"
                                                ClientIDMode="Static" OnClick="lnkInvoiceFile_Click" BackColor="#a3fd9b"></asp:LinkButton>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div style="width: 1072px; float: left; padding-top: 5px;">
                        <div style="width: 530px; float: left;">
                            <asp:GridView runat="server" ID="gvdispatchitem" AutoGenerateColumns="false" Width="520px"
                                RowStyle-Height="20px">
                                <HeaderStyle BackColor="#76BF9C" Font-Bold="true" Height="22px" />
                                <Columns>
                                    <asp:TemplateField HeaderText="FITMENT ORDER NO" ItemStyle-Width="80px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblFtno" ClientIDMode="Static" Text='<%#Eval("FtNo")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="TYRE SIZE" ItemStyle-Width="200px">
                                        <ItemTemplate>
                                            <%#Eval("TyreSize")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="FITMENT CHARGE" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <%#Eval("Rate")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="DISPATCHED QTY" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <%#Eval("DispatchedQty")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="INVOICE NO" ItemStyle-Width="60px">
                                        <ItemTemplate>
                                            <%#Eval("Invoiceno")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hdnSelectIndex" runat="server" ClientIDMode="Static" Value='' />
        <asp:HiddenField ID="hdnCotsCustID" runat="server" ClientIDMode="Static" Value='' />
        <asp:HiddenField ID="hdnInvoice" runat="server" ClientIDMode="Static" Value='' />
        <asp:HiddenField ID="hdnBillID" runat="server" ClientIDMode="Static" Value='' />
        <asp:HiddenField ID="hdnTotalDuty" runat="server" ClientIDMode="Static" Value='' />
        <asp:HiddenField ID="hdnGrandTotal" runat="server" ClientIDMode="Static" Value='' />
        <asp:HiddenField ID="hdntotalqty" runat="server" ClientIDMode="Static" Value='' />
        <asp:HiddenField ID="hdntotal" runat="server" ClientIDMode="Static" Value='' />
        <asp:HiddenField ID="hdnyear" runat="server" ClientIDMode="Static" Value='' />
        <asp:HiddenField ID="hdnCGSTPer" runat="server" ClientIDMode="Static" Value='' />
        <asp:HiddenField ID="hdnSGSTPer" runat="server" ClientIDMode="Static" Value='' />
        <asp:HiddenField ID="hdnIGSTPer" runat="server" ClientIDMode="Static" Value='' />
        <asp:HiddenField ID="hdnCGSTVal" runat="server" ClientIDMode="Static" Value='' />
        <asp:HiddenField ID="hdnSGSTVal" runat="server" ClientIDMode="Static" Value='' />
        <asp:HiddenField ID="hdnIGSTVal" runat="server" ClientIDMode="Static" Value='' />
        <asp:HiddenField ID="hdnGSTINNo" runat="server" ClientIDMode="Static" Value='' />
    </div>
    <script src="Scripts/datemonthyear.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            blinkOrderHead();
            $('input:radio[id*=rdbDeliveryMethod_]').click(function () {
                $('input:radio[id*=rdbLocation_]').attr('checked', false);
                $('input:radio[id*=rdbDeliveryTo_]').attr('checked', false);
                $('#divgodownname').css({ 'display': 'none' }); $('#txtGodownname').val('');
                $('#txtVechicleNo').val(''); $('#txtContactPerson').val(''); $('#txtContactNo').val('');
                $('#txtTrasporterName').val(''); $('#txtLrNo').val(''); $('#txtLrDate').val('');
                if (this.id == 'rdbDeliveryMethod_1') {
                    $('#personhead').html('CONTACT PERSON:');
                    $('#divDeliveryMethod').css({ 'display': 'block' });
                    $('#divVehicle').css({ 'display': 'none' });
                    $('#divLr').css({ 'display': 'block' });
                }
                else if (this.id == 'rdbDeliveryMethod_0') {
                    $('#personhead').html('COLLECTED PERSON:');
                    $('#divDeliveryMethod').css({ 'display': 'none' });
                    $('#divVehicle').css({ 'display': 'block' });
                    $('#divLr').css({ 'display': 'none' });
                }
            });
            $('input:radio[id*=rdbDeliveryTo_]').click(function () {
                $('#divgodownname').css({ 'display': 'none' }); $('#txtGodownname').val('');
                if (this.id == 'rdbDeliveryTo_1')
                    $('#divgodownname').css({ 'display': 'block' });
            });
            $("#txtLrDate").datepicker({ minDate: "-30D", maxDate: "+0D" }).keydown(function (e) { e.keyWhich = 0; e.keyCode = 0; e.preventDefault(); });
            $("input:checkbox[id*=MainContent_gvOtherFtlist_chkftno_]").change(function () {
                $('#divInvoiceDetails').css({ 'display': 'block' });
                if ($("input:checkbox[id*=MainContent_gvOtherFtlist_chkftno_]:checked").length > 0)
                    $('#divInvoiceDetails').css({ 'display': 'none' });
            });

            $("input:checkbox[id*=chk]").click(function (e) {
                var ctrlID = e.target.id;
                if (ctrlID == "chkCGST")
                    chktxtEnableDisable('chkCGST', 'divCGST', 'txtCGST', 'lblCGST', 'hdnCGSTVal');
                if (ctrlID == "chkSGST")
                    chktxtEnableDisable('chkSGST', 'divSGST', 'txtSGST', 'lblSGST', 'hdnSGSTVal');
                if (ctrlID == "chkIGST")
                    chktxtEnableDisable('chkIGST', 'divIGST', 'txtIGST', 'lblIGST', 'hdnIGSTVal');
            });

            $("[id^=txt][id$=GST]").change(function (e) {
                var ctrlID = e.target.id;
                if (ctrlID == "txtCGST") {
                    $("#hdnCGSTPer").val($("#txtCGST").val());
                    calcTaxVal("txtCGST", "lblCGST", "hdnCGSTVal");
                }
                if (ctrlID == "txtSGST") {
                    $("#hdnSGSTPer").val($("#txtSGST").val());
                    calcTaxVal("txtSGST", "lblSGST", "hdnSGSTVal");
                }
                if (ctrlID == "txtIGST") {
                    $("#hdnIGSTPer").val($("#txtIGST").val());
                    calcTaxVal("txtIGST", "lblIGST", "hdnIGSTVal");
                }
            });

            $("input:checkbox[id*=chk]").each(function (e, ele) {
                var ctrlID = ele.id;
                if (ctrlID == "chkCGST")
                    chktxtEnableDisable('chkCGST', 'divCGST', 'txtCGST', 'lblCGST', 'hdnCGSTVal');
                if (ctrlID == "chkSGST")
                    chktxtEnableDisable('chkSGST', 'divSGST', 'txtSGST', 'lblSGST', 'hdnSGSTVal');
                if (ctrlID == "chkIGST")
                    chktxtEnableDisable('chkIGST', 'divIGST', 'txtIGST', 'lblIGST', 'hdnIGSTVal');
            });
        });

        function chktxtEnableDisable(chkID, divID, txtID, lblID, hdnID) {
            if ($('#' + chkID).attr('checked') == "checked") {
                $('#' + divID).css({ 'display': 'block' });
                $('#' + txtID).focus();
                calcTaxVal(txtID, lblID, hdnID);
            }
            else {
                $('#' + divID).css({ 'display': 'none' });
                $('#' + hdnID).val("0");
                getGrandTotal();
            }
        }

        function calcTaxVal(txtID, lblID, hdnID) {
            var taxPer = parseFloat($('#' + txtID).val());
            var totAmnt = getTotal();
            if (totAmnt > 0) {
                $('#' + lblID).text(Math.round(totAmnt * taxPer / 100).toFixed(2));
                $('#' + hdnID).val(Math.round(totAmnt * taxPer / 100).toFixed(2));
                getGrandTotal();
            }
            else {
                $('#' + lblID).text("0.00");
                $('#' + hdnID).val("0.00");
            }
        }

        function getGrandTotal() {
            var CGST = $("#chkCGST:checked").length == 1 ? parseFloat($("#lblCGST").text()) : 0;
            var SGST = $("#chkSGST:checked").length == 1 ? parseFloat($("#lblSGST").text()) : 0;
            var IGST = $("#chkIGST:checked").length == 1 ? parseFloat($("#lblIGST").text()) : 0;
            var totTax = CGST + SGST + IGST;
            $("#lblTotTaxValue").text(totTax.toFixed(2));
            var totAmount = parseFloat($("#lblSumTotFC").text() == "" ? "0" : $("#lblSumTotFC").text());
            $("#lblGrandTotal").text(totTax + totAmount);
        }

        function getTotal() {
            var tot = 0;
            $("span[id*=MainContent_gvItemlist_lblTotalFtCharge_]").each(function () {
                tot += parseFloat($(this).text());
            });
            return tot;
        }

        function FtQty(lnk) {
            var clickrow = lnk.parentNode.parentNode;
            var clickIndex = clickrow.rowIndex - 1;
            var currentdispacth = $('#MainContent_gvItemlist_txtcurrentDispatch_' + clickIndex).val();
            var balanceQty = $('#MainContent_gvItemlist_lblAvaQty_' + clickIndex).html();
            var FtRate = $('#MainContent_gvItemlist_txtRate_' + clickIndex).val();
            if (parseInt(balanceQty) >= parseInt(currentdispacth)) {
                $('#MainContent_gvItemlist_ctrllblQtyErrMsg_' + clickIndex).html('');
                var total = parseInt(balanceQty) - parseInt(currentdispacth);
                $('#MainContent_gvItemlist_lblBalQty_' + clickIndex).html(total);
                $('#MainContent_gvItemlist_hdnbalQty_' + clickIndex).val(total);
                var totalAmt = parseFloat(FtRate) * parseInt(currentdispacth);
                $('#MainContent_gvItemlist_lblTotalFtCharge_' + clickIndex).html(totalAmt.toFixed(2));
                setTotalValue();
            }
            else
                $('#MainContent_gvItemlist_ctrllblQtyErrMsg_' + clickIndex).html('Enter proper qty').css({ 'color': '#f00' });
        }

        function setTotalValue() {
            var canGetVal = true;
            $("input:[id^=MainContent_gvItemlist_txtcurrentDispatch_]").each(function () {
                if ($(this).val() == "") canGetVal = false;
            });

            if (canGetVal == true) {
                $("#lblSumTotFC").text(getTotal());
                $("input:checkbox[id*=chk]").each(function (e, ele) {
                    var ctrlID = ele.id;
                    if (ctrlID == "chkCGST")
                        chktxtEnableDisable('chkCGST', 'divCGST', 'txtCGST', 'lblCGST', 'hdnCGSTVal');
                    if (ctrlID == "chkSGST")
                        chktxtEnableDisable('chkSGST', 'divSGST', 'txtSGST', 'lblSGST', 'hdnSGSTVal');
                    if (ctrlID == "chkIGST")
                        chktxtEnableDisable('chkIGST', 'divIGST', 'txtIGST', 'lblIGST', 'hdnIGSTVal');
                });

            }
        }

        function CtrlValidate() {
            var errmsg = ''; $('#divErrMsg').html('');
            var count = 0;
            $('span[id*=MainContent_gvItemlist_lblBalQty_]').each(function (e) {
                if ($('#MainContent_gvItemlist_lblBalQty_' + e).html().length == 0 || $('#MainContent_gvItemlist_txtcurrentDispatch_' + e).val().length == 0 || $('#MainContent_gvItemlist_txtRate_' + e).val().length == 0)
                    count++;
            });
            if (count != 0)
                errmsg += 'Check fitment charge & current dispatch qty<br/>';
            if ($('input:radio[id*=rdbDeliveryMethod_]:checked').length == 0)
                errmsg += 'Choose delivery method<br/>';
            else {
                if ($('#rdbDeliveryMethod_1').attr('checked') == 'checked') {
                    if ($('input:radio[id*=rdbLocation_]:checked').length == 0)
                        errmsg += 'Choose location<br/>';
                    if ($('input:radio[id*=rdbDeliveryTo_]:checked').length == 0)
                        errmsg += 'Choose delivery to<br/>';
                    if ($('#rdbDeliveryTo_1').attr('checked') == 'checked' && $('#txtGodownname').val().length == 0)
                        errmsg += 'Enter godown name<br/>';
                    if ($('#txtTrasporterName').val().length == 0)
                        errmsg += 'Enter transporter name<br/>';
                    if ($('#txtLrNo').val().length == 0)
                        errmsg += 'Enter lr no.<br/>';
                    if ($('#txtLrDate').val().length == 0)
                        errmsg += 'Enter lr date<br/>';
                }
                else if ($('#rdbDeliveryMethod_0').attr('checked') == 'checked') {
                    if ($('#txtVechicleNo').val().length == 0)
                        errmsg += 'Enter vechicle no.<br/>';
                }
            }
            if ($('#txtContactPerson').val().length == 0)
                errmsg += 'Enter ' + $('#personhead').html() + '<br/>';
            if ($('#txtContactNo').val().length == 0)
                errmsg += 'Enter contact no<br/>';
            if ($('#txtComments').val().length == 0)
                errmsg += 'Enter Comments<br/>';
            if ($("input:checkbox[id^=chk][id$=GST]:checked").length == 0)
                errmsg += "check atleast one GST Value";
            if ($('#chkCGST').attr('checked') == 'checked' && $('#txtCGST').val().length == 0)
                errmsg += "Enter CGST % ";
            else if ($('#chkCGST').attr('checked') == 'checked' && parseFloat($('#txtCGST').val()) == 0)
                errmsg += "CGST Value must greater than 0 ";
            if ($('#chkSGST').attr('checked') == 'checked' && $('#txtSGST').val().trim().length == 0)
                errmsg += "Enter SGST % ";
            else if ($('#chkSGST').attr('checked') == 'checked' && parseFloat($('#txtSGST').val()) == 0)
                errmsg += "SGST Value must greater than 0 ";
            if ($('#chkIGST').attr('checked') == 'checked' && $('#txtIGST').val().length == 0)
                errmsg += "Enter IGST % ";
            else if ($('#chkIGST').attr('checked') == 'checked' && parseFloat($('#txtIGST').val()) == 0)
                errmsg += "IGST Value must greater than 0 ";
            $('span[id*=MainContent_gvItemlist_ctrllblQtyErrMsg_').each(function () {
                if ($(this).html() != "")
                    errmsg += 'Enter proper dispatch qty';
            });

            if (errmsg.length > 0) {
                $('#divErrMsg').html(errmsg);
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
