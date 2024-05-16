<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="cotsorderrevise.aspx.cs" Inherits="TTS.cotsorderrevise" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .tbMain
        {
            border-collapse: collapse;
            border-color: #000;
            width: 100%;
            line-height: 25px;
        }
        .tbMain th
        {
            background-color: #eafbf1;
            text-align: left;
        }
        .tblAddress
        {
            border-collapse: collapse;
            border-color: #000;
            width: 380px;
            line-height: 20px;
        }
        .tblAddress th
        {
            background-color: #eaebff;
            text-align: left;
        }
        .divDeliveryGodown
        {
            width: 350px;
        }
        .divMethodOthers
        {
            width: 350px;
        }
        #tdRimData th
        {
            text-align: right;
            font-weight: normal;
            padding-right: 10px;
            background-color: #f1f1f1;
        }
        #tdRimData td
        {
            text-align: left;
            font-weight: bold;
            padding-left: 10px;
            background-color: #b7ff9a;
        }
    </style>
    <link href="Styles/datemonthyear.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        ORDER REVISE</div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #000;
            width: 100%;">
            <tr>
                <td colspan="3">
                    <div style="width: 100%; float: left; font-weight: bold; font-size: 18px; color: #fff;
                        background-color: #2E2B2B;">
                        <span style="width: 450px; float: left;">
                            <asp:Label runat="server" ID="lblCustName" ClientIDMode="Static" Text=""></asp:Label></span>
                        <span style="width: 100px; float: left;">&nbsp;</span> <span style="width: 450px;
                            float: left;">
                            <asp:Label runat="server" ID="lblStausOrderRefNo" ClientIDMode="Static" Text=""></asp:Label></span>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="background-color: #bbeff5;">
                    TYPE OF REVISE
                </td>
                <td>
                    <asp:RadioButtonList ID="rdbEditType" runat="server" ClientIDMode="Static" AutoPostBack="false"
                        RepeatColumns="3" RepeatDirection="Horizontal">
                    </asp:RadioButtonList>
                </td>
                <td style="text-align: center;">
                    <asp:LinkButton runat="server" ID="lnkBack" ClientIDMode="Static" Text="GO TO BACK"
                        OnClick="lnkBack_Click"></asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <div runat="server" id="divErrmsg" clientidmode="Static" style="clear: both; width: 100%;
                        color: #f00">
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <div id="divMasterDetails" style="display: none">
                        <asp:GridView runat="server" ID="gvMasterDetail" ClientIDMode="Static" AutoGenerateColumns="false"
                            OnRowEditing="gvMasterDetail_RowEdit" OnRowUpdating="gvMasterDetail_RowUpdating"
                            OnRowCancelingEdit="gvMasterDetail_CancelEdit" Width="100%">
                            <Columns>
                                <asp:TemplateField HeaderStyle-CssClass="headerNone">
                                    <ItemTemplate>
                                        <table cellspacing="0" rules="all" border="1" class="tbMain">
                                            <tr>
                                                <th>
                                                    GRADE
                                                </th>
                                                <td>
                                                    <asp:Label runat="server" ClientIDMode="Static" ID="lblGrade" Text='<%#Eval("grade") %>'></asp:Label>
                                                </td>
                                                <th>
                                                    PLANT
                                                </th>
                                                <td>
                                                    <asp:Label runat="server" ID="lblOrderPlant" ClientIDMode="Static" Text='<%# Eval("Plant") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>
                                                    CUSTOMER
                                                </th>
                                                <td>
                                                    <asp:Label runat="server" ID="lblCustName" ClientIDMode="Static" Width="380px" Text='<%#Eval("custfullname") %>'></asp:Label>
                                                </td>
                                                <th>
                                                    ORDER REFERENCE NO
                                                </th>
                                                <td>
                                                    <asp:Label runat="server" ID="lblRefNo" ClientIDMode="Static" Width="250px" Text='<%#Eval("OrderRefNo") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>
                                                    BILLING ADDRESS
                                                </th>
                                                <td style="vertical-align: top;">
                                                    <table class="tblAddress">
                                                        <%#bindAddress(Eval("BillID").ToString())%>
                                                    </table>
                                                </td>
                                                <th>
                                                    SHIPPING ADDRESS
                                                </th>
                                                <td style="vertical-align: top;">
                                                    <table class="tblAddress">
                                                        <%#bindAddress(Eval("ShipID").ToString())%>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>
                                                    DESIRED SHIPPING DATE
                                                </th>
                                                <td>
                                                    <asp:Label runat="server" ID="lblDesiredShipDate" ClientIDMode="Static" Width="250px"
                                                        Text='<%#Convert.ToDateTime(Eval("DesiredShipDate").ToString()).ToString("dd/MM/yyyy") %>'></asp:Label>
                                                </td>
                                                <th>
                                                    PACKING METHOD
                                                </th>
                                                <td>
                                                    <asp:Label runat="server" ID="lblPackingMethod" ClientIDMode="Static" Width="250px"
                                                        Text='<%#Eval("PackingOthers").ToString() != "" ? Eval("PackingMethod") +  " - "+ Eval("PackingOthers").ToString() : Eval("PackingMethod") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>
                                                    FREIGHT CHARGES
                                                </th>
                                                <td>
                                                    <asp:Label runat="server" ID="lblFreighCharge" Text='<%#Eval("FreightCharges") %>'></asp:Label>
                                                </td>
                                                <th>
                                                    <span style="line-height: 15px;">DELIVERY METHOD
                                                        <br />
                                                        <span style="font-size: 10px;">C/C ATTACHED </span></span>
                                                </th>
                                                <td>
                                                    <%# Eval("DeliveryMethod")%>
                                                    <%# Eval("GodownName").ToString() == "" ? "" : " ( "+ Eval("GodownName").ToString() + " )"  %>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>
                                                    PREFERED TRANSPORTER
                                                </th>
                                                <td>
                                                    <asp:Label runat="server" ID="lblPreferedTransport" Text='<%#Eval("TransportDetails") %>'> </asp:Label>
                                                </td>
                                                <th>
                                                </th>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>
                                                    SPECIAL INSTRUCTION
                                                </th>
                                                <td>
                                                    <%#Eval("SplIns").ToString().Replace("~", "<br/>")%>
                                                </td>
                                                <th>
                                                    SPECIAL NOTES
                                                </th>
                                                <td>
                                                    <%#Eval("SpecialRequset").ToString().Replace("~", "<br/>")%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4" style="text-align: center">
                                                    <asp:Button CommandName="Edit" ID="btnEdit" Text="EDIT MASTER DETAILS" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                        runat="server" CssClass="btn btn-info" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <table cellspacing="0" rules="all" border="1" class="tbMain">
                                            <tr>
                                                <th>
                                                    GRADE
                                                </th>
                                                <td>
                                                    <asp:RadioButtonList runat="server" ID="rdbOrderGrade" ClientIDMode="Static" Width="300px"
                                                        RepeatColumns="5" RepeatDirection="Horizontal">
                                                        <asp:ListItem Text="A" Value="A"></asp:ListItem>
                                                        <asp:ListItem Text="B" Value="B"></asp:ListItem>
                                                        <asp:ListItem Text="C" Value="C"></asp:ListItem>
                                                        <asp:ListItem Text="D" Value="D"></asp:ListItem>
                                                        <asp:ListItem Text="E" Value="E"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                                <th>
                                                    PLANT
                                                </th>
                                                <td>
                                                    <asp:Label runat="server" ID="lblEditOrderPlant" ClientIDMode="Static" Text='<%# Eval("Plant") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>
                                                    CUSTOMER
                                                </th>
                                                <td>
                                                    <asp:Label runat="server" ID="lblCustName" ClientIDMode="Static" Width="380px" Text='<%#Eval("custfullname") %>'></asp:Label>
                                                </td>
                                                <th>
                                                    ORDER REFERENCE NO
                                                </th>
                                                <td>
                                                    <asp:Label runat="server" ID="lblRefNo" ClientIDMode="Static" Width="250px" Text='<%#Eval("OrderRefNo") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>
                                                    BILLING ADDRESS
                                                </th>
                                                <td colspan="3">
                                                    <asp:DropDownList runat="server" ID="ddlBillingAddress" ClientIDMode="Static" Width="925px"
                                                        AutoPostBack="true" CssClass="form-control">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>
                                                    SHIPPING ADDRESS
                                                </th>
                                                <td colspan="3">
                                                    <asp:DropDownList runat="server" ID="ddlShippingAddress" ClientIDMode="Static" Width="925px"
                                                        AutoPostBack="true" CssClass="form-control">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="vertical-align: top;">
                                                    <table class="tblAddress">
                                                        <asp:Literal runat="server" ID="ltrlBillAddress"></asp:Literal>
                                                    </table>
                                                </td>
                                                <td colspan="2" style="vertical-align: top;">
                                                    <table class="tblAddress">
                                                        <asp:Literal runat="server" ID="ltrlShipAddress"></asp:Literal>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>
                                                    DESIRED SHIPPING DATE
                                                </th>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtDesiredDate" ClientIDMode="Static" Width="80px"
                                                        CssClass="form-control"></asp:TextBox>
                                                </td>
                                                <th>
                                                    PACKING METHOD
                                                </th>
                                                <td>
                                                    <asp:RadioButtonList runat="server" ID="rdbPackingMethod" ClientIDMode="Static" RepeatColumns="4"
                                                        RepeatDirection="Horizontal">
                                                    </asp:RadioButtonList>
                                                    <span <%#Eval("PackingOthers").ToString() == "" ? "style='display: none;'" : "" %>
                                                        class="divMethodOthers"><span style="color: #f00;">Other Packing Method:</span>
                                                        <asp:TextBox runat="server" ID="txtPackingOthers" ClientIDMode="Static" Text='<%#Eval("PackingOthers").ToString()%>'
                                                            Width="350px" MaxLength="50" CssClass="form-control"></asp:TextBox>
                                                    </span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>
                                                    FREIGHT CHARGES
                                                </th>
                                                <td>
                                                    <asp:RadioButtonList runat="server" ID="rdbFreightCharges" ClientIDMode="Static"
                                                        RepeatColumns="4" RepeatDirection="Horizontal">
                                                    </asp:RadioButtonList>
                                                </td>
                                                <th>
                                                    <span style="line-height: 15px;">Delivery Method <span style="font-size: 10px;">C/C
                                                        Attached </span></span>
                                                </th>
                                                <td>
                                                    <asp:RadioButtonList runat="server" ID="rdbDelivery" ClientIDMode="Static" RepeatColumns="4"
                                                        RepeatDirection="Horizontal">
                                                    </asp:RadioButtonList>
                                                    <span <%#Eval("GodownName").ToString() == "" ? "style='display: none;'": "" %> class="divDeliveryGodown">
                                                        <span style="color: #f00;">Enter Godown Name:</span>
                                                        <asp:TextBox runat="server" ID="txtGodownName" ClientIDMode="Static" Text='<%#Eval("GodownName").ToString()%>'
                                                            Width="350px" MaxLength="50" CssClass="form-control"></asp:TextBox>
                                                    </span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>
                                                    PREFERED TRANSPORTER
                                                </th>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtTransportDetails" ClientIDMode="Static" Text='<%#Eval("TransportDetails").ToString()%>'
                                                        Width="350px" CssClass="form-control" MaxLength="50"></asp:TextBox>
                                                </td>
                                                <th>
                                                </th>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>
                                                    SPECIAL INSTRUCTION
                                                </th>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtSplIns" ClientIDMode="Static" TextMode="MultiLine"
                                                        Width="380px" Height="100px" onKeyUp="javascript:CheckMaxLength(this, 3999);"
                                                        onChange="javascript:CheckMaxLength(this, 3999);" Text='<%#Eval("SplIns").ToString().Replace("~","\r\n")%>'
                                                        CssClass="form-control"></asp:TextBox>
                                                </td>
                                                <th>
                                                    SPECIAL NOTES
                                                </th>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtSplReq" ClientIDMode="Static" TextMode="MultiLine"
                                                        Width="380px" Height="100px" onKeyUp="javascript:CheckMaxLength(this, 3999);"
                                                        onChange="javascript:CheckMaxLength(this, 3999);" Text='<%#Eval("SpecialRequset").ToString().Replace("~","\r\n")%>'
                                                        CssClass="form-control"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="text-align: right; padding-right: 10px;">
                                                    <asp:Button runat="server" ID="btnOrderSaveSend" ClientIDMode="Static" CssClass="btn btn-success"
                                                        Text="UPDATE" OnClientClick="javascript:return CtrlSaveOrder();" CommandName="Update" />
                                                </td>
                                                <td colspan="2" style="text-align: left; padding-left: 10px;">
                                                    <asp:Button runat="server" ID="btnCancelUpdate" ClientIDMode="Static" CssClass="btn btn-warning"
                                                        Text="CANCEL" CommandName="Cancel" />
                                                </td>
                                            </tr>
                                        </table>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div style="display: none; width: 100%;" id="divAddItem">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #CE8686;
                                    width: 100%;" id="tblPriceSheet">
                                    <tr>
                                        <td>
                                            CATEGORY<br />
                                            <asp:DropDownList runat="server" ID="ddlCategory" ClientIDMode="Static" Width="95px"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_IndexChange" CssClass="form-control">
                                                <asp:ListItem Text="Choose" Value="Choose"></asp:ListItem>
                                                <asp:ListItem Text="SOLID" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="POB" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="PNEUMATIC" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="SPLIT RIMS" Value="4"></asp:ListItem>
                                                <asp:ListItem Text="POB WHEEL" Value="5"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            PLATFORM<br />
                                            <asp:DropDownList runat="server" ID="ddlPlatform" ClientIDMode="Static" Width="100px"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlPlatform_IndexChange" CssClass="form-control">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            BRAND<br />
                                            <asp:DropDownList runat="server" ID="ddlBrand" ClientIDMode="Static" Width="100px"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlBrand_IndexChange" CssClass="form-control">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            SIDEWALL<br />
                                            <asp:DropDownList runat="server" ID="ddlSidewall" ClientIDMode="Static" Width="100px"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlSidewall_IndexChange" CssClass="form-control">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            TYPE<br />
                                            <asp:DropDownList runat="server" ID="ddlType" ClientIDMode="Static" Width="80px"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlType_IndexChange" CssClass="form-control">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            TYRE SIZE<br />
                                            <asp:DropDownList runat="server" ID="ddlSize" ClientIDMode="Static" Width="160px"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlSize_IndexChange" CssClass="form-control">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            RIM WIDTH<br />
                                            <asp:DropDownList runat="server" ID="ddlRim" ClientIDMode="Static" Width="60px" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlRim_IndexChange" CssClass="form-control">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            DISC %<br />
                                            <asp:TextBox runat="server" ID="txtDiscount" ClientIDMode="Static" Text="" Width="50px"
                                                MaxLength="50" onkeypress="return isNumberKey(event)" onblur="CalcFromDiscount()"
                                                CssClass="form-control"></asp:TextBox>
                                            <asp:HiddenField runat="server" ID="hdnDiscount" ClientIDMode="Static" />
                                        </td>
                                        <td>
                                            LIST PRICE<br />
                                            <asp:TextBox runat="server" ID="txtCustomPriceSheet" ClientIDMode="Static" Width="60px"
                                                onkeypress="return isNumberKey(event)" MaxLength="10" onblur="CalcFromSheetPrice()"
                                                CssClass="form-control"></asp:TextBox>
                                        </td>
                                        <td>
                                            BASIC PRICE<br />
                                            <asp:TextBox runat="server" ID="txtBasicPrice" ClientIDMode="Static" Width="60px"
                                                onkeypress="return isNumberKey(event)" MaxLength="10" onblur="CalcFromBasicPrice()"
                                                CssClass="form-control"></asp:TextBox>
                                        </td>
                                        <td>
                                            QTY<br />
                                            <asp:TextBox runat="server" ID="txtQty" ClientIDMode="Static" Text="" Width="50px"
                                                MaxLength="4" onkeypress="return isNumberWithoutDecimal(event)" CssClass="form-control"></asp:TextBox>
                                        </td>
                                        <td>
                                            WT<br />
                                            <asp:TextBox runat="server" ID="txtFinishedWt" ClientIDMode="Static" Text="" Width="60px"
                                                onkeypress="return isNumberKey(event)" MaxLength="8" CssClass="form-control"></asp:TextBox>
                                            <asp:Label runat="server" ID="lblProcessID" ClientIDMode="Static" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkrimassmbly" runat="server" Text="RIM  ASSEMBLY" ClientIDMode="Static"
                                                ForeColor="Green" AutoPostBack="true" OnCheckedChanged="chkrimassmbly_CheckedChanged" />
                                        </td>
                                        <td colspan="5">
                                            <table cellspacing="0" rules="all" border="1" id="tr_rimAssembly" style="display: none;
                                                width: 100%;">
                                                <tr>
                                                    <td>
                                                        EDC NO<br />
                                                        <asp:DropDownList runat="server" ID="ddl_EdcNo" Width="140px" ClientIDMode="Static"
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddl_EdcNo_SelectedIndexChanged" CssClass="form-control">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        RIM PRICE<br />
                                                        <asp:TextBox runat="server" ID="txtRimPrice" ClientIDMode="Static" Text="" Font-Bold="true"
                                                            onkeypress="return isNumberKey(event)" MaxLength="10" Width="80px" CssClass="form-control"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        RIM WEIGHT<br />
                                                        <asp:TextBox runat="server" ID="txtRimWt" ClientIDMode="Static" Text="" Width="50px"
                                                            onkeypress="return isNumberKey(event)" MaxLength="8" CssClass="form-control"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        DESCRIPTION<br />
                                                        <asp:TextBox runat="server" ID="txtRimDwg" ClientIDMode="Static" Text="" Width="400px"
                                                            MaxLength="50" CssClass="form-control"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4">
                                                        <asp:FormView runat="server" ID="frmRimProcessID_Details" Width="100%">
                                                            <ItemTemplate>
                                                                <table cellspacing="0" rules="all" border="1" style="background-color: #ecf6ff; width: 100%;
                                                                    border-color: White; line-height: 15px;" id="tdRimData">
                                                                    <tr>
                                                                        <th>
                                                                            RIM SIZE
                                                                        </th>
                                                                        <td>
                                                                            <%# Eval("Rimsize") %>
                                                                        </td>
                                                                        <th>
                                                                            MOUNTING HOLES DIA
                                                                        </th>
                                                                        <td>
                                                                            <%# Eval("MHdia")%>
                                                                        </td>
                                                                        <th>
                                                                            FIXING HOLES DIA
                                                                        </th>
                                                                        <td>
                                                                            <%# Eval("FHdia")%>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <th>
                                                                            RIM TYPE
                                                                        </th>
                                                                        <td>
                                                                            <%# Eval("RimType") %>
                                                                            <%# Eval("NoofPiece").ToString() !="" ? " - ":"" %>
                                                                            <%# Eval("NoofPiece") %>
                                                                        </td>
                                                                        <th>
                                                                            MOUNTING HOLES TYPE
                                                                        </th>
                                                                        <td>
                                                                            <%# Eval("MHtype")%>
                                                                            <%# Eval("MHtype").ToString() == "COUNTERSINK SPHERICAL" && Eval("radius").ToString() != "" ? (" - RADIUS " + Eval("radius").ToString()) : ""%>
                                                                            <%# Eval("MHtype").ToString() == "COUNTERSINK CONICAL" && Eval("angle").ToString() != "" ? (" - ANGLE " + Eval("angle").ToString()) : ""%>
                                                                        </td>
                                                                        <th>
                                                                            FIXING HOLES TYPE
                                                                        </th>
                                                                        <td>
                                                                            <%# Eval("FHtype")%>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <th>
                                                                            TYRE CATEGORY
                                                                        </th>
                                                                        <td>
                                                                            <asp:Label runat="server" ID="lblTyreCategory" ClientIDMode="Static" Text='<%# Eval("TyreCategory") %>'></asp:Label>
                                                                        </td>
                                                                        <th>
                                                                            DISC OFFSET
                                                                        </th>
                                                                        <td>
                                                                            <%# Eval("DiscOffSet") %>
                                                                        </td>
                                                                        <th>
                                                                            BORE DIA
                                                                        </th>
                                                                        <td>
                                                                            <%# Eval("Boredia")%>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <th>
                                                                            PILOTED
                                                                        </th>
                                                                        <td>
                                                                            <%# Eval("Piloted") %>
                                                                        </td>
                                                                        <th>
                                                                            DISC THICKNESS
                                                                        </th>
                                                                        <td>
                                                                            <%# Eval("DiscThickness")%>
                                                                        </td>
                                                                        <th>
                                                                            PAINTING COLOR
                                                                        </th>
                                                                        <td>
                                                                            <%# Eval("PaintingColor")%>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <th>
                                                                            NO. OF MOUNTING HOLES
                                                                        </th>
                                                                        <td>
                                                                            <%# Eval("NoOfMH") %>
                                                                        </td>
                                                                        <th>
                                                                            NO. OF FIXING HOLES
                                                                        </th>
                                                                        <td>
                                                                            <%# Eval("NoofFH") %>
                                                                        </td>
                                                                        <th>
                                                                            WALL THICKNESS
                                                                        </th>
                                                                        <td>
                                                                            <%# Eval("WallThickness")%>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <th>
                                                                            MOUNTING HOLES PCD
                                                                        </th>
                                                                        <td>
                                                                            <%# Eval("MHpcd")%>
                                                                        </td>
                                                                        <th>
                                                                            FIXING HOLES PCD
                                                                        </th>
                                                                        <td>
                                                                            <%# Eval("FHpcd") %>
                                                                        </td>
                                                                        <th>
                                                                            EDC-NO/ PROCESS-ID
                                                                        </th>
                                                                        <td>
                                                                            <%# Eval("EDCNO") %>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:FormView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div style="clear: both; width: 100%; text-align: center;">
                            <asp:Button runat="server" ID="btnAddNextItem" ClientIDMode="Static" Text="ADD NEXT ITEM"
                                CssClass="btn btn-success" OnClientClick="javascript:return CtrlAddNextItem();"
                                OnClick="btnAddNextItem_Click" />
                            <input type="button" id="btnCancelItem" value="CANCEL" class="btn btn-danger" />
                        </div>
                    </div>
                    <div id="divManualItemList" style="display: none;">
                        <asp:GridView runat="server" ID="gv_ManualOrderList" AutoGenerateColumns="false"
                            Width="100%" RowStyle-Height="22px" OnRowDeleting="gv_ManualOrderList_RowDeleting"
                            OnRowCancelingEdit="gv_ManualOrderList_RowCanceling" OnRowEditing="gv_ManualOrderList_RowEditing"
                            ShowFooter="true" FooterStyle-Font-Bold="true" FooterStyle-HorizontalAlign="Right">
                            <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" Height="22px" HorizontalAlign="Center"
                                VerticalAlign="Middle" />
                            <Columns>
                                <asp:TemplateField HeaderText="CATEGORY" ItemStyle-Width="80px">
                                    <ItemTemplate>
                                        <%#Eval("category")%>
                                        <%# Eval("AssyRimstatus").ToString() == "True" ? " (ASSY)" : ""%>
                                        <asp:HiddenField ID="hdnprocessid" runat="server" Value='<%# Eval("processid") %>' />
                                        <asp:HiddenField runat="server" ID="hdnAssyStatus" Value='<%# Eval("AssyRimstatus") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="PLATFORM" ItemStyle-Width="80px">
                                    <ItemTemplate>
                                        <%#Eval("Config")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="BRAND" ItemStyle-Width="80px">
                                    <ItemTemplate>
                                        <%#Eval("brand")%></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="SIDEWALL" ItemStyle-Width="80px">
                                    <ItemTemplate>
                                        <%#Eval("sidewall")%></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="TYPE" ItemStyle-Width="50px">
                                    <ItemTemplate>
                                        <%#Eval("tyretype") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="TYRE SIZE" ItemStyle-Width="150px">
                                    <ItemTemplate>
                                        <%#Eval("tyresize")%></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="RIM" ItemStyle-Width="40px">
                                    <ItemTemplate>
                                        <%#Eval("rimsize")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="LIST PRICE" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <%#Eval("SheetPrice")%></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="DISC%" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <%#Eval("Discount")%></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="BASIC PRICE" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <%#Eval("listprice")%></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="QTY" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <%#Eval("itemqty")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="FWT" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <%# Eval("tyrewt")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="RIM PRICE" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <%# Eval("Rimunitprice").ToString() == "0.00" ? "" : Eval("Rimunitprice").ToString()%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="RIM FWT" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <%# Eval("Rimfinishedwt").ToString() == "0.00" ? "" : Eval("Rimfinishedwt").ToString()%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="EDC NO">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblEdcNo" Text='<%# Eval("EdcNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ACTION" ItemStyle-Width="60px">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="lnkEdit" Text="EDIT" CommandName="Edit" Font-Size="10px"></asp:LinkButton>
                                        <asp:LinkButton runat="server" ID="lnkDelete" Text="DELETE" CommandName="Delete"
                                            Font-Size="10px"></asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Button runat="server" ID="btnCancel" Text="Cancel" CommandName="Cancel" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <input type="button" id="btnAddNewItem" value="ADD NEW ITEM" class="btn btn-info" />
                    </div>
                    <div id="profPreapreDiv" style="background-color: rgba(166, 172, 171, 0.2); clear: both;
                        display: none">
                        <div style="width: 100%; text-align: left; line-height: 18px;">
                            <div style="float: right;">
                                <span class="headCss" style="width: 150px; float: left; line-height: 28px;">MODE OF
                                    TRANSPORT:</span>
                                <asp:RadioButtonList CssClass="profEditMode radiobutton" Style="float: left;" runat="server"
                                    ID="rdbModeOfTransport" ClientIDMode="Static" Width="230px" RepeatColumns="3"
                                    RepeatDirection="Horizontal">
                                    <asp:ListItem Text="BY ROAD" Value="BY ROAD"></asp:ListItem>
                                    <asp:ListItem Text="BY TRAIN" Value="BY TRAIN"></asp:ListItem>
                                    <asp:ListItem Text="BY AIR" Value="BY AIR"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            <asp:GridView runat="server" ID="gvAmountSub" AutoGenerateColumns="false" Style="float: right;
                                clear: both;" Width="500px">
                                <Columns>
                                    <asp:BoundField DataField="slno" Visible="false" />
                                    <asp:TemplateField ItemStyle-Width="350px" HeaderText="OTHER CHARGES DESCRIPTION"
                                        HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtAddDesc" Text="" Width="350px" MaxLength="100"
                                                CssClass="form-control"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="35px" HeaderText="+/-" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAmountAdd" runat="server" Text="+" ToolTip="ADD" Font-Bold="true"
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
                            <div style="float: right; clear: both">
                                <table cellspacing="0" rules="all" border="1" style="width: 500px; border-collapse: collapse;
                                    display: none;">
                                    <tr>
                                        <td colspan="3" style="text-align: left; font-weight: bold;">
                                            <span>CLAIM ADJUSTMENT</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 370px;">
                                            <asp:TextBox runat="server" ID="txtClaimAdjustment" ClientIDMode="Static" CssClass="profEditMode text form-control"
                                                Text="" Width="350px" MaxLength="100" ToolTip="CLAIM ADJUSTMENT"></asp:TextBox>
                                        </td>
                                        <td style="width: 35px; text-align: center">
                                            <asp:Label ID="lblLESSclaimAdjus" runat="server" Text="-" ToolTip="LESS" Font-Bold="true"
                                                Font-Size="20px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtLESSAMT" ClientIDMode="Static" CssClass="profEditMode text form-control"
                                                onkeypress="return isNumberKey(event)" Text="" Width="70px" MaxLength="8"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="text-align: left; font-weight: bold;">
                                            OTHER DISCOUNT
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 374px;">
                                            <asp:TextBox runat="server" ID="txtotherdiscount" ClientIDMode="Static" CssClass="profEditMode text form-control"
                                                Text="" Width="350px" MaxLength="100" ToolTip="OTHER DISCOUNT"></asp:TextBox>
                                        </td>
                                        <td style="width: 37px; text-align: center">
                                            <asp:Label ID="lblLessdiscount" runat="server" Text="-" ToolTip="LESS" Font-Bold="true"
                                                Font-Size="20px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtOtherDisAmt" ClientIDMode="Static" CssClass="profEditMode text form-control"
                                                onkeypress="return isNumberKey(event)" Text="" Width="70px" MaxLength="8"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div style="float: right; clear: both; border: 1px solid #868282; line-height: 15px;"
                                id="divGST">
                                <table style="">
                                    <tr>
                                        <th colspan="3" style="text-align: center;">
                                            GST VALUE
                                        </th>
                                    </tr>
                                    <tr>
                                        <td style="background-color: #F1CED5;">
                                            <div style="float: left;">
                                                <div style="float: left; background-color: #ccc; width: 75px;">
                                                    <asp:CheckBox runat="server" ID="chkCGST" CssClass="profEditMode check" ClientIDMode="Static"
                                                        Checked="false" Text="CGST %" /></div>
                                                <div id="divCGST" style="display: none; float: left;">
                                                    <div style="float: left;">
                                                        <asp:TextBox runat="server" ID="txtCGST" CssClass="profEditMode text form-control"
                                                            ClientIDMode="Static" Text="" Width="60px" onkeypress="return isNumberKey(event)"
                                                            MaxLength="5"></asp:TextBox></div>
                                                </div>
                                            </div>
                                        </td>
                                        <td style="background-color: #BCE0B8;">
                                            <div style="float: left;">
                                                <div style="float: left; background-color: #ccc; width: 75px;">
                                                    <asp:CheckBox runat="server" ID="chkSGST" CssClass="profEditMode check" ClientIDMode="Static"
                                                        Checked="false" Text="SGST %" /></div>
                                                <div id="divSGST" style="display: none; float: left;">
                                                    <div style="float: left;">
                                                        <asp:TextBox runat="server" ID="txtSGST" CssClass="profEditMode text form-control"
                                                            ClientIDMode="Static" Text="" Width="60px" onkeypress="return isNumberKey(event)"
                                                            MaxLength="5"></asp:TextBox></div>
                                                </div>
                                            </div>
                                        </td>
                                        <td style="background-color: #D2C9E8;">
                                            <div style="float: left;">
                                                <div style="float: left; background-color: #ccc; width: 75px;">
                                                    <asp:CheckBox runat="server" ID="chkIGST" CssClass="profEditMode check" ClientIDMode="Static"
                                                        Checked="false" Text="IGST %" /></div>
                                                <div id="divIGST" style="display: none; width: 100px; float: left;">
                                                    <asp:TextBox runat="server" ID="txtIGST" CssClass="profEditMode text form-control"
                                                        ClientIDMode="Static" Text="" Width="60px" onkeypress="return isNumberKey(event)"
                                                        MaxLength="5"></asp:TextBox>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div style="clear: both; text-align: center; margin-top: 5px;">
                                <input type="button" value="EDIT PROFORMA DETAILS" id="btnProformaEdit" class="btn btn-info" />
                                <asp:Button runat="server" ID="btnPrepareProforma" ClientIDMode="Static" Text="UPDATE"
                                    CssClass="btn btn-success profEditMode button" OnClientClick="javascript:return CtrlProformChk();"
                                    OnClick="btnProformaUpdate_Click" />
                                <asp:Button runat="server" ID="btnProformaCancel" Style="display: none" ClientIDMode="Static"
                                    Text="CANCEL" OnClick="btnProformaCancel_Click" CssClass="btn btn-danger" />
                            </div>
                        </div>
                    </div>
                    <div id="divStatusChange" style="display: none;">
                        <asp:TextBox runat="server" ID="txtStatusComments" ClientIDMode="Static" TextMode="MultiLine"
                            Width="1000px" Height="60px" onKeyUp="javascript:CheckMaxLength(this, 999);"
                            onChange="javascript:CheckMaxLength(this, 999);" Text='<%#Eval("SplIns").ToString().Replace("~","\r\n")%>'
                            CssClass="form-control"></asp:TextBox>
                        <br />
                        <asp:Button runat="server" ID="btnStatusChange" ClientIDMode="Static" Text="REVISE COMPLETED"
                            OnClick="btnStatusChange_Click" CssClass="btnactive" />
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnSizePosition" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnTypePosition" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnTypeDesc" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnEditType" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hdnItemEditMode" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hdnProformaEditMode" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hdnEditProcessId" ClientIDMode="Static" />
    <script src="Scripts/datemonthyear.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(':text').bind('keydown', function (e) { if (e.keyCode == 13) { e.preventDefault(); return false; } }); // prevent form submit on enter key press.
            $("#txtDesiredDate").datepicker({ minDate: "+0D", maxDate: "+360D" }).keydown(function (e) { e.keyWhich = 0; e.keyCode = 0; e.preventDefault(); });

            $(".profEditMode.radiobutton").find("input").attr("disabled", "disabled").css('cursor', 'no-drop');
            $("#MainContent_gvAmountSub").find("input,select").attr("readonly", "readonly").css('cursor', 'no-drop');
            $(".profEditMode.text").attr("readonly", "readonly").css('cursor', 'no-drop');
            $(".profEditMode.check").find("input").attr("disabled", "disabled").css('cursor', 'no-drop');
            $(".profEditMode.button").css("display", "none");

            $("#btnProformaEdit").click(function () {
                $(".profEditMode.radiobutton").find("input").removeAttr("disabled").css('cursor', 'auto');
                $("#MainContent_gvAmountSub").find("input,select").removeAttr("readonly").css('cursor', 'auto');
                $(".profEditMode.text").removeAttr("readonly").css('cursor', 'auto');
                $(".profEditMode.check").find("input").removeAttr("disabled", "disabled").css('cursor', 'auto');
                $(".profEditMode.button").css("display", "inline-block");
                $("#btnProformaEdit").css("display", "none");
                $("#hdnProformaEditMode").val("1");
                $("#rdbEditType input").attr("disabled", "disabled");
                $("#btnProformaCancel").css("display", "inline-block");
            });

            $("#btnAddNewItem").click(function () {
                $("#divAddItem").css("display", "inline-block");
                $("#btnAddNewItem").css("display", "none");
                $("#rdbEditType input").attr("disabled", "disabled");
                $("#divErrmsg").html("");
            });

            $("#btnCancelItem").click(function () {
                $("#btnAddNewItem").css("display", "inline-block");
                $("#divAddItem").css("display", "none");
                $("#rdbEditType input").removeAttr("disabled");
            });

            $("#divMasterDetails").css("display", "none");
            $("#divManualItemList").css("display", "none");
            $("#profPreapreDiv").css("display", "none");

            $("input:radio[id*=rdbPackingMethod_]").click(function () {
                $("input:radio[id*=rdbPackingMethod_]").each(function (e) {
                    $("input:radio[id*=rdbPackingMethod_]").eq(e).removeAttr("checked");
                });
                $(this).attr("checked", "checked");
                $('.divMethodOthers').css({ "display": "none" });
                $('#txtPackingOthers').val('');
                if ($(this).val() == 'OTHERS')
                    $('.divMethodOthers').css({ "display": "block" });
            });

            $("input:radio[id*=rdbDelivery_]").click(function () {
                $("input:radio[id*=rdbDelivery_]").each(function (e) { $("input:radio[id*=rdbDelivery_]").eq(e).removeAttr("checked"); });
                $(this).attr("checked", "checked");
                $('.divDeliveryGodown').css({ "display": "none" });
                $('#txtGodownName').val('');
                if ($(this).val() == 'TRANSPORTERS GODOWN')
                    $('.divDeliveryGodown').css({ "display": "block" });
            });

            $("#rdbEditType input").click(function () {
                $("#hdnEditType").val(this.id);
                $("#divMasterDetails").css("display", "none");
                $("#divManualItemList").css("display", "none");
                $("#profPreapreDiv").css("display", "none");
                if ($(this).val() == 0)
                    $("#divMasterDetails").css("display", "block");
                else if ($(this).val() == 1)
                    $("#divManualItemList").css("display", "block");
                else if ($(this).val() == 2)
                    $("#profPreapreDiv").css("display", "block");
            });

            $("#" + $("#hdnEditType").val()).trigger("click");
            if ($("#hdnItemEditMode").val() == "1") {
                $("#btnAddNewItem").trigger("click");
                $("#btnCancelItem").css("display", "none");
            }

            $("input:checkbox[id*=chk]").click(function (e) {
                var ctrlID = e.target.id;
                if (ctrlID == "chkCGST")
                    chktxtEnableDisable('chkCGST', 'divCGST', 'txtCGST');
                if (ctrlID == "chkSGST")
                    chktxtEnableDisable('chkSGST', 'divSGST', 'txtSGST');
                if (ctrlID == "chkIGST")
                    chktxtEnableDisable('chkIGST', 'divIGST', 'txtIGST');
            });

            $("input:checkbox[id*=chk]").each(function (e, ele) {
                var ctrlID = ele.id;
                if (ctrlID == "chkCGST")
                    chktxtEnableDisable('chkCGST', 'divCGST', 'txtCGST');
                if (ctrlID == "chkSGST")
                    chktxtEnableDisable('chkSGST', 'divSGST', 'txtSGST');
                if (ctrlID == "chkIGST")
                    chktxtEnableDisable('chkIGST', 'divIGST', 'txtIGST');
            });
        });

        function DisableJathagam_Category() {
            DisableCntrls('#ddlPlatform');
            DisableCntrls('#ddlBrand');
            DisableCntrls('#ddlSidewall');
            DisableCntrls('#ddlType');
            DisableCntrls('#ddlSize');
            DisableCntrls('#ddlRim');
            DisableCntrls('#txtDiscount');
            DisableCntrls('#txtCustomPriceSheet');
            DisableCntrls('#txtBasicPrice');
            DisableCntrls('#txtFinishedWt');
            $('#tr_rimAssembly').fadeIn('slow');
        }

        function DisableCntrls(id) {
            $(id).prop('disabled', true);
            $(id).css({ 'cursor': 'no-drop', 'display': 'none' });
        }

        function chktxtEnableDisable(chkID, divID, txtID) {
            if ($('#' + chkID).attr('checked') == "checked") {
                $('#' + divID).css({ 'display': 'block' });
                $('#' + txtID).focus();
            }
            else
                $('#' + divID).css({ 'display': 'none' });
        }

        function CtrlSaveOrder() {
            $('#divErrmsg').html('');
            var errmsg = '';
            if ($("#ddlBillingAddress option:selected").text() == "choose")
                errmsg += 'Choose billing address </br>';
            if ($("#ddlShippingAddress option:selected").text() == "choose")
                errmsg += 'Choose shipping address </br>';
            if ($('#txtDesiredDate').val().length == 0)
                errmsg += 'Enter desired shipping date</br>';
            if ($("#rdbPackingMethod input:checked").val() == "OTHERS") {
                if ($('#txtPackingOthers').val().length == 0)
                    errmsg += 'Enter Other packing method</br>';
            }
            if ($("#rdbDelivery input:checked").val() == "TRANSPORTERS GODOWN") {
                if ($('#txtGodownName').val().length == 0)
                    errmsg += 'Enter godown name</br>';
            }
            if (errmsg.length > 0 || $('#lblErrMsg').html().length > 0) {
                $('#divErrmsg').html(errmsg);
                return false;
            }
            else
                return true;
        }

        function CalcFromDiscount() {
            if (parseFloat($('#txtDiscount').val()) > parseFloat($("#hdnDiscount").val())) {
                alert('Your maximum discount limit is ' + $("#hdnDiscount").val());
                $('#txtDiscount').val($("#hdnDiscount").val());
            }
            calcListBasicDisc('Discount');
        }
        function CalcFromSheetPrice() {
            if (parseFloat($('#txtDiscount').val()) > parseFloat($("#hdnDiscount").val())) {
                alert('Your maximum discount limit is ' + $("#hdnDiscount").val());
                $('#txtDiscount').val($("#hdnDiscount").val());
            }
            calcListBasicDisc('ListPrice');
        }
        function CalcFromBasicPrice() {
            if (parseFloat($('#txtDiscount').val()) > parseFloat($("#hdnDiscount").val())) {
                alert('Your maximum discount limit is ' + $("#hdnDiscount").val());
                $('#txtDiscount').val($("#hdnDiscount").val());
            }
            calcListBasicDisc('BasicPrice');
        }
        function calcListBasicDisc(ctrlID) {
            if ($("#txtDiscount").val() == 'NaN') $("#txtDiscount").val('0');
            if ($("#txtBasicPrice").val() == 'NaN') $("#txtBasicPrice").val('0');
            if ($("#txtCustomPriceSheet").val() == 'NaN') $("#txtCustomPriceSheet").val('0');
            var custSheetPrice = parseFloat($('#txtCustomPriceSheet').val() != '' ? $('#txtCustomPriceSheet').val() : 0);
            var custBasicPrice = parseFloat($('#txtBasicPrice').val != '' ? $('#txtBasicPrice').val() : 0);
            var custDiscPer = parseFloat($('#txtDiscount').val != '' ? $('#txtDiscount').val() : 0);
            if (ctrlID == 'Discount' || ctrlID == 'ListPrice') {
                if (custDiscPer != 0 && custSheetPrice != 0)
                    $('#txtBasicPrice').val(parseFloat((custSheetPrice - (custSheetPrice * (custDiscPer / 100)))).toFixed());
                else
                    $('#txtBasicPrice').val(parseFloat((custSheetPrice)).toFixed());
            }
            else if (ctrlID == 'BasicPrice') {
                if (custDiscPer != 0 && custBasicPrice != 0)
                    $('#txtCustomPriceSheet').val(parseFloat((custBasicPrice / ((100 - custDiscPer) / 100))).toFixed());
                else
                    $('#txtCustomPriceSheet').val(parseFloat((custBasicPrice)).toFixed());
            }
        }

        function show_ManualOrderCtrl(ctrlID) { $('#' + ctrlID).show(); }

        if ($("#hdnItemEditMode").val() == "1") { $("#btnAddNewItem").trigger("click"); }

        function CtrlAddNextItem() {
            var errMsg = '';
            $('#divErrmsg').html('');
            if ($('#ddlCategory option:selected').text() == 'Choose')
                errMsg += 'Choose category<br/>';
            else if ($('#ddlCategory option:selected').val() == '1' || $('#ddlCategory option:selected').val() == '2' || $('#ddlCategory option:selected').val() == '3') {
                if ($('#ddlPlatform option:selected').text() == 'Choose')
                    errMsg += 'Choose platform<br/>';
                if ($('#ddlBrand option:selected').text() == 'Choose')
                    errMsg += 'Choose brand<br/>';
                if ($('#ddlSidewall option:selected').text() == 'Choose')
                    errMsg += 'Choose sidewall<br/>';
                if ($('#ddlType option:selected').text() == 'Choose')
                    errMsg += 'Choose type<br/>';
                if ($('#ddlSize option:selected').text() == 'Choose')
                    errMsg += 'Choose size<br/>';
                if ($('#ddlRim option:selected').text() == 'Choose')
                    errMsg += 'Choose rim<br/>';
                if ($("#txtDiscount").val() == 'NaN') $("#txtDiscount").val('0');
                if ($("#txtBasicPrice").val() == 'NaN') $("#txtBasicPrice").val('0');
                if ($("#txtCustomPriceSheet").val() == 'NaN') $("#txtCustomPriceSheet").val('0');
                if ($('#txtCustomPriceSheet').val().length == 0 || parseFloat($('#txtCustomPriceSheet').val()) == 0)
                    errMsg += 'Enter list price<br/>';
                if ($('#txtBasicPrice').val().length == 0 || parseFloat($('#txtBasicPrice').val()) == 0)
                    errMsg += 'Enter basic price<br/>';
                if ($('#txtFinishedWt').val().length == 0)
                    errMsg += 'Enter finished Wt.<br/>';
            }
            if ($("#chkrimassmbly").is(":checked") || $('#ddlCategory option:selected').val().toString() == '4' || $('#ddlCategory option:selected').val().toString() == '5') {
                $('#tr_rimAssembly').fadeIn('slow');
                if ($('#ddl_EdcNo option:selected').val() == "Choose")
                    errMsg += "Select Edc No<br/>";
                if ($('#txtRimWt').val().length == 0)
                    errMsg += 'Enter rim weight<br/>';
                if ($('#txtRimPrice').val().length == 0)
                    errMsg += 'Enter rim price<br/>';
                if ($('#txtRimDwg').val().length == 0)
                    errMsg += 'Enter rim description<br/>';
            }
            else {
                $("#ddl_EdcNo option[value=Choose]").attr('selected', 'selected');
                $('#txtRimWt').val("0.00");
                $('#txtRimPrice').val("0.00");
                $('#txtRimDwg').val("");
            }
            if ($('#txtQty').val().length == 0 || parseInt($('#txtQty').val()) <= 0)
                errMsg += 'Enter item qty<br/>';
            if (errMsg.length > 0) {
                $('#divErrmsg').html(errMsg);
                return false;
            }
            else
                return true;
        }

        function CtrlProformChk() {
            var errmsg = "";
            if ($("input:checkbox[id*=chk]:checked").length == 0)
                errmsg += "check atleast one GST Value<br/>";
            if ($('#chkCGST').attr('checked') == 'checked' && $('#txtCGST').val().length == 0)
                errmsg += "Enter CGST % <br/>";
            else if ($('#chkCGST').attr('checked') == 'checked' && parseFloat($('#txtCGST').val()) == 0)
                errmsg += "CGST Value must greater than 0 <br/>";
            if ($('#chkSGST').attr('checked') == 'checked' && $('#txtSGST').val().trim().length == 0)
                errmsg += "Enter SGST % <br/>";
            else if ($('#chkSGST').attr('checked') == 'checked' && parseFloat($('#txtSGST').val()) == 0)
                errmsg += "SGST Value must greater than 0 <br/>";
            if ($('#chkIGST').attr('checked') == 'checked' && $('#txtIGST').val().length == 0)
                errmsg += "Enter IGST % <br/>";
            else if ($('#chkIGST').attr('checked') == 'checked' && parseFloat($('#txtIGST').val()) == 0)
                errmsg += "IGST Value must greater than 0 <br/>";

            $("input:text[id*=MainContent_gvAmountSub_txtAddDesc_]").each(function () {
                var id1 = this.id; var amtId = id1.replace('txtAddDesc_', 'txtCAddAmt_');
                if ($('#' + id1).val() != '' && $('#' + amtId).val() == '')
                    errmsg += 'Enter extra charges amount<br/>';
                if ($('#' + id1).val() == '' && $('#' + amtId).val() != '')
                    errmsg += 'Enter extra charges description<br/>';
            });

            if ($("input:radio[id*=rdbModeOfTransport]:checked").length == 0)
                errmsg += 'Choose mode of transport';
            else if ($("input:radio[id*=rdbModeOfTransport]:checked").val() == "BY TRAIN") {
                var str2 = "freight";
                if (($('#MainContent_gvAmountSub_txtAddDesc_0').val().toLowerCase()).indexOf(str2) == -1 && ($('#MainContent_gvAmountSub_txtAddDesc_0').val().toLowerCase()).indexOf(str2) == -1 && ($('#MainContent_gvAmountSub_txtAddDesc_0').val().toLowerCase()).indexOf(str2) == -1) {
                    errmsg += 'Enter proper freight charges amount';
                    $('#MainContent_gvAmountSub_txtAddDesc_0').focus();
                }
            }
            if (errmsg.length > 0) {
                alert(errmsg);
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
