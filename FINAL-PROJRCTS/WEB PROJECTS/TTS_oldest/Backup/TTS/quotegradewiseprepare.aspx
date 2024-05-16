<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="quotegradewiseprepare.aspx.cs" Inherits="TTS.quotegradewiseprepare" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .billAddress
        {
            border-collapse: collapse;
            border-color: #000000;
            width: 530px;
        }
        .billAddress th
        {
            text-align: right;
            background-color: #f3e2e2;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div align="center" class="pageTitleHead">
        <asp:Label runat="server" ID="lblQuoteHead" ClientIDMode="Static" Text=""></asp:Label></div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #000;
            width: 100%;">
            <tr>
                <td colspan="2">
                    <div style="text-align: center;">
                        <asp:Label runat="server" ID="lblQuoteRefHead" ClientIDMode="Static" Text="" Font-Bold="true"
                            Font-Size="14px" ForeColor="DarkGreen"></asp:Label>
                    </div>
                    <div style="display: none;">
                        <asp:Label runat="server" ID="lblQuoteAcYear" ClientIDMode="Static" Text="" Font-Bold="true"
                            Font-Size="14px"></asp:Label>
                        <asp:Label runat="server" ID="lblQuoteRefNo" ClientIDMode="Static" Text="" Font-Bold="true"
                            Font-Size="14px"></asp:Label>
                        <asp:Label runat="server" ID="lblQuoteReviseCount" ClientIDMode="Static" Text=""
                            Font-Bold="true" Font-Size="14px"></asp:Label>
                    </div>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:RadioButtonList runat="server" ID="rdbTYPECustomer" ClientIDMode="Static" RepeatColumns="5"
                        Width="300px" AutoPostBack="true" OnSelectedIndexChanged="rbdTYPECustomer_SelectedIndexChanged">
                        <asp:ListItem Text="NEW CUSTOMER"></asp:ListItem>
                        <asp:ListItem Text="EXISTING CUSTOMER"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td align="left">
                    <span class="headCss" style="width: 100px; float: left; line-height: 25px;">GRADE
                    </span>
                    <asp:RadioButtonList runat="server" ID="rdbQuoteGrade" ClientIDMode="Static" RepeatColumns="5"
                        Width="230px">
                        <asp:ListItem Text="A" Value="A"></asp:ListItem>
                        <asp:ListItem Text="B" Value="B"></asp:ListItem>
                        <asp:ListItem Text="C" Value="C"></asp:ListItem>
                        <asp:ListItem Text="D" Value="D"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td class="headCss">
                    <span class="headCss" style="width: 120px; float: left; line-height: 25px;">CUSTOMER
                        NAME </span>
                    <asp:TextBox runat="server" ID="txtQuoteCustomer" ClientIDMode="Static" Text="" Width="300px"
                        MaxLength="100" CssClass="txtCss"></asp:TextBox>
                    <asp:DropDownList ID="ddlQuotecustomer" runat="server" Visible="false" AutoPostBack="true"
                        Width="300px" OnSelectedIndexChanged="ddlQuotecustomer_SelectedIndexChanged"
                        CssClass="ddlCss">
                    </asp:DropDownList>
                </td>
                <td rowspan="2" style="line-height: 22px;">
                    <span class="headCss" style="width: 70px; float: left; line-height: 25px;">EMAIL-ID
                    </span>
                    <div>
                        TO:<asp:TextBox runat="server" ID="txtQuoteEmail" ClientIDMode="Static" Text="" Width="300px"
                            MaxLength="100" CssClass="txtCss"></asp:TextBox>
                    </div>
                    <div>
                        CC:<asp:TextBox runat="server" ID="txtQuoteCC" ClientIDMode="Static" Text="" Width="300px"
                            MaxLength="100" CssClass="txtCss"></asp:TextBox>
                    </div>
                    <div>
                        <span style="font-size: 11px; color: #2809C9;">Note: Put comma (,) for more mail-id</span></div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblUsername" runat="server" Text="USER ID" CssClass="headCss" Width="120px"
                        Visible="false"></asp:Label>
                    <asp:DropDownList ID="ddlUsername" runat="server" AutoPostBack="true" Width="300px"
                        Visible="false" OnSelectedIndexChanged="ddlUsername_SelectedIndexChanged" CssClass="ddlCss">
                    </asp:DropDownList>
                    <asp:Label ID="lblDUserName" runat="server" Visible="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span class="headCss">BILLING ADDRESS </span>
                        <asp:DropDownList runat="server" ID="ddlBillingAddress" ClientIDMode="Static" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlBillingAddress_IndexChange" Width="380" Style="margin-left: 10px;"
                            Visible="false">
                        </asp:DropDownList>
                    </div>
                    <table cellspacing="0" rules="all" border="1" class="billAddress">
                        <tr>
                            <th>
                                NAME: M/S.
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtBillCompanyName" ClientIDMode="Static" Text=""
                                    Width="350px" CssClass="txtCss"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                ADDRESS
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtBillAddress" ClientIDMode="Static" Text="" Width="350px"
                                    CssClass="txtCss" Height="50px" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                CITY
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtBillCity" ClientIDMode="Static" Text="" Width="300px"
                                    MaxLength="50" CssClass="txtCss"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                ZIP CODE
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtBillZipCode" ClientIDMode="Static" Text="" Width="300px"
                                    MaxLength="50" CssClass="txtCss"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                STATE / CODE
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtBillState" ClientIDMode="Static" Text="" Width="300px"
                                    MaxLength="50" CssClass="txtCss"></asp:TextBox>&nbsp;/&nbsp;<asp:TextBox runat="server"
                                        ID="txtBillStateCode" ClientIDMode="Static" Text="" Width="80px" MaxLength="10"
                                        CssClass="txtCss"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                GSTIN No.
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtBillGSTNo" ClientIDMode="Static" Text="" Width="300px"
                                    MaxLength="50" CssClass="txtCss"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                CONTACT PERSON
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtBillContactName" ClientIDMode="Static" Text=""
                                    Width="300px" MaxLength="50" CssClass="txtCss"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                CONTACT NO.
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtBillContactNo" ClientIDMode="Static" Text="" Width="300px"
                                    MaxLength="20" CssClass="txtCss"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <div>
                        <span class="headCss">CONSIGNEE ADDRESS </span>
                        <asp:DropDownList runat="server" ID="ddlShippingAddress" ClientIDMode="Static" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlShippingAddress_IndexChange" Width="380" Style="margin-left: 10px;"
                            Visible="false">
                        </asp:DropDownList>
                    </div>
                    <table cellspacing="0" rules="all" border="1" class="billAddress">
                        <tr>
                            <th>
                                NAME: M/S.
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtConsCompanyName" ClientIDMode="Static" Text=""
                                    Width="350px" CssClass="txtCss"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                ADDRESS
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtConsAddress" ClientIDMode="Static" Text="" Width="350px"
                                    CssClass="txtCss" Height="50px" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                CITY
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtConsCity" ClientIDMode="Static" Text="" Width="300px"
                                    MaxLength="50" CssClass="txtCss"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                ZIP CODE
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtConsZipCode" ClientIDMode="Static" Text="" Width="300px"
                                    MaxLength="50" CssClass="txtCss"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                STATE / CODE
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtConsState" ClientIDMode="Static" Text="" Width="300px"
                                    MaxLength="50" CssClass="txtCss"></asp:TextBox>&nbsp;/&nbsp;
                                <asp:TextBox runat="server" ID="txtConsStateCode" ClientIDMode="Static" Text="" Width="80px"
                                    MaxLength="10" CssClass="txtCss"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                GSTIN No
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtConsGSTNo" ClientIDMode="Static" Text="" Width="300px"
                                    MaxLength="50" CssClass="txtCss"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                CONTACT PERSON
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtConsContactName" ClientIDMode="Static" Text=""
                                    Width="300px" MaxLength="50" CssClass="txtCss"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                CONTACT NO.
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtConsContactNo" ClientIDMode="Static" Text="" Width="300px"
                                    MaxLength="20" CssClass="txtCss"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <span class="headCss" style="line-height: 30px; width: 200px; float: left;">CUSTOMER
                        TYPE</span>
                    <asp:RadioButtonList runat="server" ID="rdbQuoteType" ClientIDMode="Static" RepeatColumns="7"
                        Width="800px" OnSelectedIndexChanged="rdbQuoteType_IndexChange" AutoPostBack="true">
                        <asp:ListItem Text="OEM" Value="OEM"></asp:ListItem>
                        <asp:ListItem Text="END USER" Value="END USER"></asp:ListItem>
                        <asp:ListItem Text="DEALER" Value="DEALER"></asp:ListItem>
                        <asp:ListItem Text="PSU" Value="PSU"></asp:ListItem>
                        <asp:ListItem Text="RAILWAY" Value="RAILWAY"></asp:ListItem>
                        <asp:ListItem Text="RENTAL FLEET" Value="RENTAL FLEET"></asp:ListItem>
                        <asp:ListItem Text="ME" Value="ME"></asp:ListItem>
                        <asp:ListItem Text="ST" Value="ST"></asp:ListItem>
                        <asp:ListItem Text="ARC" Value="ARC"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #CE8686;
                                width: 100%;">
                                <tr align="center" class="headCss" style="background-color: #EBEEED;">
                                    <td class="tdhide">
                                        PLATFORM
                                    </td>
                                    <td class="tdhide">
                                        BRAND
                                    </td>
                                    <td class="tdhide">
                                        SIDEWALL
                                    </td>
                                    <td class="tdhide">
                                        TYPE
                                    </td>
                                    <td>
                                        SIZE
                                    </td>
                                    <td>
                                        RIM
                                    </td>
                                    <td>
                                        DISCOUNT
                                    </td>
                                    <td>
                                        SHEET PRICE
                                    </td>
                                    <td>
                                        BASIC PRICE
                                    </td>
                                    <td>
                                        QTY
                                    </td>
                                    <td>
                                        WT
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlPlatform" ClientIDMode="Static" Width="100px"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlPlatform_IndexChange" CssClass="ddlCss">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlBrand" ClientIDMode="Static" Width="100px"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlBrand_IndexChange" CssClass="ddlCss">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlSidewall" ClientIDMode="Static" Width="100px"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlSidewall_IndexChange" CssClass="ddlCss">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlType" ClientIDMode="Static" Width="70px"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlType_IndexChange" CssClass="ddlCss">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlSize" ClientIDMode="Static" Width="140px"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlSize_IndexChange" CssClass="ddlCss">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlRim" ClientIDMode="Static" Width="55px" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlRim_IndexChange" CssClass="ddlCss">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtQuoteDiscount" ClientIDMode="Static" Width="60px"
                                            MaxLength="5" onkeypress="return isNumberKey(event)" Text="0" onblur="CalcFromDiscount()"
                                            CssClass="txtCss"></asp:TextBox>
                                    </td>
                                    <td style="width: 55px">
                                        <asp:TextBox runat="server" ID="txtCustomPriceSheet" ClientIDMode="Static" Width="55px"
                                            onkeypress="return isNumberKey(event)" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td style="width: 55px">
                                        <asp:TextBox runat="server" ID="txtBasicPrice" ClientIDMode="Static" Width="55px"
                                            onkeypress="return isNumberKey(event)" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtQuoteQty" ClientIDMode="Static" Text="" Width="60px"
                                            MaxLength="4" onkeypress="return isNumberKey(event)" CssClass="txtCss"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtFinishedWt" ClientIDMode="Static" Text="" Width="50px"
                                            onkeypress="return isNumberKey(event)" MaxLength="8" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td style="text-align: left; font-weight: bold; font-size: 12px; color: #f00;">
                    <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text=""></asp:Label>
                    <asp:LinkButton runat="server" ID="lnkQuoteFile" ClientIDMode="Static" Text="" OnClick="lnkQuoteFile_click"
                        OnClientClick="aspnetForm.target ='_blank';"></asp:LinkButton>
                </td>
                <td align="center" style="vertical-align: top;">
                    <asp:Button runat="server" ID="btnQuoteMoreItem" ClientIDMode="Static" Text="ADD MORE"
                        CssClass="btnactive" OnClientClick="javascript:return CtrlQuoteItemAdd();" OnClick="btnQuoteMoreItem_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #E26BE2;
                        width: 100%;">
                        <tr>
                            <td>
                                <asp:GridView runat="server" ID="gvQuoteItem" AutoGenerateColumns="false" Width="100%"
                                    OnRowDeleting="gvQuoteItem_RowDeleting" OnRowEditing="gvQuoteItem_RowEditing"
                                    OnRowUpdating="gvQuoteItem_RowUpdating" OnRowCancelingEdit="gvQuoteItem_RowCanceling">
                                    <HeaderStyle BackColor="#CACA55" Font-Bold="true" Height="22px" />
                                    <Columns>
                                        <asp:TemplateField ItemStyle-CssClass="headerNone" HeaderStyle-CssClass="headerNone">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblProcessid" Text='<%#Eval("ProcessID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PLATFORM">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblConfig" Text='<%#Eval("Config")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="BRAND">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblBrand" Text='<%#Eval("Brand")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SIDEWALL">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblSidewall" Text='<%#Eval("Sidewall")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="TYPE">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblTyreType" Text='<%#Eval("TyreType") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SIZE">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblSize" Text='<%#Eval("TyreSize")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="RIM">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblRim" Text='<%#Eval("RimSize")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="DISC %">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblDisc" Text='<%#Eval("Discount")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox runat="server" ID="txtEditQuoteDisc" Width="60px" MaxLength="5" onkeypress="return isNumberKey(event)"
                                                    Text='<%#Eval("Discount")%>' onblur="CalcEditDisc(this)" CssClass="txtCss"></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="LIST PRICE" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblListPrice" Text='<%#Eval("ListPrice") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="BASIC PRICE" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblBasicPrice" Text='<%#Eval("BasicPrice") %>' Font-Bold="true"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="FWT" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblFinishedWt" Text='<%#Eval("FinishedWt") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="QTY" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblQty" Text='<%#Eval("Qty") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox runat="server" ID="txtItemQty" Text='<%#Eval("Qty") %>' Width="40px"></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="TOTAL PRICE" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblTotalPrice" Text='<%#Eval("TotalPrice") %>' Font-Bold="true"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ACTION">
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="btnEdit" Text="EDIT" CommandName="Edit"></asp:LinkButton>
                                                <asp:LinkButton runat="server" ID="btnDelete" Text="DELETE" CommandName="Delete"></asp:LinkButton>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:LinkButton runat="server" ID="btnUpdate" Text="UPDATE" CommandName="Update"
                                                    OnClientClick="javascript:return CtrlQUoteItemUpdate(this);"></asp:LinkButton>
                                                <asp:LinkButton runat="server" ID="btnCancel" Text="CANCEL" CommandName="Cancel"></asp:LinkButton>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr align="center">
                            <td style="background-color: #e6e2e2; font-weight: bold; float: right; text-align: right;">
                                <asp:Label runat="server" ID="lblWtTotal" ClientIDMode="Static" Text="" Width="200px"></asp:Label>
                                <asp:Label runat="server" ID="lblQtyTotal" ClientIDMode="Static" Text="" Width="100px"></asp:Label>
                                <asp:Label runat="server" ID="lblPriceTotal" ClientIDMode="Static" Text="" Width="160px"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="width: 530px; vertical-align: top;">
                    <span class="headCss">TERMS & CONDITION</span>
                    <asp:TextBox runat="server" ID="txtQuoteTerms" ClientIDMode="Static" Text="" TextMode="MultiLine"
                        Width="520px" Height="150px" onKeyUp="javascript:CheckMaxLength(this, 1999);"
                        onChange="javascript:CheckMaxLength(this, 1999);" CssClass="txtCss"></asp:TextBox>
                </td>
                <td align="right" style="width: 530px;">
                    <div>
                        <asp:GridView runat="server" ID="gvAmountSub" AutoGenerateColumns="false" Width="450px">
                            <Columns>
                                <asp:BoundField DataField="slno" Visible="false" />
                                <asp:TemplateField ItemStyle-Width="350px" HeaderText="OTHER CHARGES DESCRIPTION"
                                    HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtAddDesc" Text="" Width="300px" MaxLength="100"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="35px" HeaderText="+/-">
                                    <ItemTemplate>
                                        <asp:DropDownList runat="server" ID="ddlCalcType" Width="35px">
                                            <asp:ListItem Text="+" Value="ADD"></asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="70px" HeaderText="AMOUNT">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtCAddAmt" onkeypress="return isNumberKey(event)"
                                            Text="" Width="70px" MaxLength="8" OnTextChanged="gvAmountSub_Amount_TextChanged"
                                            AutoPostBack="true"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div>
                        <table cellspacing="0" rules="all" border="1" style="width: 450px; border-collapse: collapse;">
                            <tr>
                                <td colspan="3" style="text-align: left; font-weight: bold;">
                                    <span>CLAIM ADJUSTMENT</span>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 350px;">
                                    <asp:TextBox runat="server" ID="txtClaimAdjustment" Text="" Width="300px" MaxLength="100"
                                        ToolTip="CLAIM ADJUSTMENT"></asp:TextBox>
                                </td>
                                <td style="width: 35px; text-align: center">
                                    <asp:Label ID="lblLESSclaimAdjus" runat="server" Text="-" ToolTip="LESS"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtLESSAMT" onkeypress="return isNumberKey(event)"
                                        Text="" Width="70px" MaxLength="8" OnTextChanged="gvAmountSub_Amount_TextChanged"
                                        AutoPostBack="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="text-align: left; font-weight: bold;">
                                    OTHER DISCOUNT
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 350px;">
                                    <asp:TextBox runat="server" ID="txtotherdiscount" Text="" Width="300px" MaxLength="100"
                                        ToolTip="OTHER DISCOUNT"></asp:TextBox>
                                </td>
                                <td style="width: 37px; text-align: center">
                                    <asp:Label ID="lblLessdiscount" runat="server" Text="-" ToolTip="LESS"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtOtherDisAmt" onkeypress="return isNumberKey(event)"
                                        Text="" Width="70px" MaxLength="8" OnTextChanged="gvAmountSub_Amount_TextChanged"
                                        AutoPostBack="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="headCss" style="height: 22px;">
                                    SUB TOTAL
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblsubTotalVal" ClientIDMode="Static" Text="" Font-Bold="true"
                                        ForeColor="DarkBlue"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="border: 1px solid #868282; line-height: 15px; width: 242px; text-align: left"
                        id="divGST">
                        <table>
                            <tr>
                                <th style="text-align: center; width: 237px;">
                                    GST VALUE
                                </th>
                            </tr>
                            <tr>
                                <td style="background-color: #F1CED5;">
                                    <div style="float: left;">
                                        <div style="float: left; background-color: #ccc; width: 75px;">
                                            <asp:CheckBox runat="server" ID="chkCGST" ClientIDMode="Static" Text="CGST %" OnCheckedChanged="GSTValue_Changed"
                                                AutoPostBack="true" /></div>
                                        <div id="divCGST" style="display: none; float: left; width: 160px;">
                                            <div style="float: left;">
                                                <asp:TextBox runat="server" ID="txtCGST" ClientIDMode="Static" Text="" Width="60px"
                                                    onkeypress="return isNumberKey(event)" MaxLength="5" CssClass="txtCss" Style="margin-left: 10px;
                                                    float: left;" OnTextChanged="GSTValue_Changed" AutoPostBack="true"></asp:TextBox>
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
                                            <asp:CheckBox runat="server" ID="chkSGST" ClientIDMode="Static" Text="SGST %" OnCheckedChanged="GSTValue_Changed"
                                                AutoPostBack="true" /></div>
                                        <div id="divSGST" style="display: none; float: left; width: 160px;">
                                            <div style="float: left;">
                                                <asp:TextBox runat="server" ID="txtSGST" ClientIDMode="Static" Text="" Width="60px"
                                                    onkeypress="return isNumberKey(event)" MaxLength="5" CssClass="txtCss" Style="margin-left: 10px;
                                                    float: left;" OnTextChanged="GSTValue_Changed" AutoPostBack="true"></asp:TextBox>
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
                                            <asp:CheckBox runat="server" ID="chkIGST" ClientIDMode="Static" Text="IGST %" OnCheckedChanged="GSTValue_Changed"
                                                AutoPostBack="true" /></div>
                                        <div id="divIGST" style="display: none; float: left; width: 160px;">
                                            <asp:TextBox runat="server" ID="txtIGST" ClientIDMode="Static" Text="" Width="60px"
                                                onkeypress="return isNumberKey(event)" MaxLength="5" CssClass="txtCss" Style="margin-left: 10px;
                                                float: left;" OnTextChanged="GSTValue_Changed" AutoPostBack="true"></asp:TextBox>
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
                <td align="center" colspan="2">
                    <div style="width: 433px; float: left; text-align: center; padding-right: 50px; padding-left: 120px;">
                        <asp:Button runat="server" ID="btnQuotePrepareSend" ClientIDMode="Static" Text="PREPARE QUOTE (*.PDF) AND SEND TO CUSTOMER"
                            CssClass="btnshow" OnClick="btnQuotePrepareSend_Click" OnClientClick="javascript:return CtrlPrepareQuote();" />
                    </div>
                    <div style="width: 130px; float: left; text-align: center;">
                        <asp:Button runat="server" ID="btnQuoteSave" ClientIDMode="Static" Text="SAVE QUOTE"
                            CssClass="btnauthorize" OnClick="btnQuoteSave_Click" OnClientClick="javascript:return CtrlPrepareQuote();" />
                    </div>
                    <div style="width: 150px; float: left; text-align: center; padding-left: 50px;">
                        <asp:Button runat="server" ID="btnQuoteDelete" ClientIDMode="Static" Text="DELETE QUOTE"
                            CssClass="btnclear" OnClick="btnQuoteDelete_Click" OnClientClick="javascript:return CtrlPrepareQuote();" />
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div style="display: none;">
        <asp:Label runat="server" ID="hdnListPrice" ClientIDMode="Static" Text=""></asp:Label>
        <asp:Label runat="server" ID="hdnProcessID" ClientIDMode="Static" Text=""></asp:Label>
        <asp:Label runat="server" ID="hdnFinishedWt" ClientIDMode="Static" Text=""></asp:Label>
        <asp:Label runat="server" ID="hdnLoadingQty" ClientIDMode="Static" Text=""></asp:Label>
        <asp:Label runat="server" ID="hdnSizePosition" ClientIDMode="Static" Text=""></asp:Label>
        <asp:Label runat="server" ID="hdnTypePosition" ClientIDMode="Static" Text=""></asp:Label>
        <asp:Label runat="server" ID="hdncategory" ClientIDMode="Static" Text=""></asp:Label>
        <asp:Label runat="server" ID="hdnTypeDesc" ClientIDMode="Static" Text=""></asp:Label>
        <asp:HiddenField runat="server" ID="hdnTotalBasicPrice" ClientIDMode="Static" Value="" />
        <asp:HiddenField runat="server" ID="hdnQuoteTotalDuty" ClientIDMode="Static" Value="" />
        <asp:HiddenField runat="server" ID="hdnQuoteGrandTotal" ClientIDMode="Static" Value="" />
        <asp:HiddenField runat="server" ID="hdnMaxQuoteDiscount" ClientIDMode="Static" Value="" />
        <asp:HiddenField runat="server" ID="hdnFullName" ClientIDMode="Static" Value="" />
        <asp:HiddenField runat="server" ID="hdnUsername" ClientIDMode="Static" Value="" />
        <asp:HiddenField runat="server" ID="hdnCGSTVal" ClientIDMode="Static" Value="" />
        <asp:HiddenField runat="server" ID="hdnSGSTVal" ClientIDMode="Static" Value="" />
        <asp:HiddenField runat="server" ID="hdnIGSTVal" ClientIDMode="Static" Value="" />
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $(':text').bind('keydown', function (e) { if (e.keyCode == 13) { e.preventDefault(); return false; } });

            $("#MainContent_ddlQuotecustomer").change(function () { $('#hdnFullName').val($('#MainContent_ddlQuotecustomer option:selected').text()); });
            $("#MainContent_ddlUsername").change(function () { $('#hdnUsername').val($('#MainContent_ddlUsername option:selected').val()); });

            $("input:radio[id*=rdbQuoteType_]").click(function () {
                $('#lblQuotePrice').html(''); $('#txtQuoteQty').val('');
                $('#ddlRim option:selected').removeAttr('selected');
                $('#ddlRim option:eq(0)').attr('selected', 0);
            });

            $("input:radio[id*=rdbQuoteGrade_]").click(function () {
                $('#MainContent_ddlQuotecustomer option:selected').removeAttr('selected');
                $('#MainContent_ddlQuotecustomer option:eq(0)').attr('selected', 'choose');
                $('#MainContent_ddlUsername option:selected').removeAttr('selected');
                $('#MainContent_ddlUsername option:eq(0)').attr('selected', 'choose');
                $('#ddlRim option:selected').removeAttr('selected');
                $('#ddlRim option:eq(0)').attr('selected', 'selected');
                $('#MainContent_txtQuotecustomer').val('');
                $('#txtQuoteEmail').val('');
                $('#txtQuoteCC').val('');
                $('#txtQuoteContactName').val('');
                $('#txtQuoteContactNo').val('');
                $('#txtExcisePer').val('0'); $('#txtEducationPer').val('0'); $('#txtHigherPer').val('0'); $('#txtTaxPer').val('0');
                $('#chkQuoteAgainstForm').attr('checked', false); $("input:radio[id*=rdbTaxType_]").attr('checked', false);
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
                var taxPer = parseFloat($('#' + txtID).val());
                var totAmnt = parseFloat($("#lblsubTotalVal").text());
                $('#' + lblID).text(Math.round(totAmnt * taxPer / 100).toFixed(2));
            }
            else
                $('#' + divID).css({ 'display': 'none' });
        }

        function CtrlQuoteItemAdd() {
            var errmsg = ''; $('#lblErrMsg').html('');
            if ($('input:radio[id*=rdbQuoteType_]:checked').length == 0)
                errmsg += 'Choose customer type <br />';
            var emailpattern = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
            if ($('input:radio[id*=rdbTYPECustomer_]:checked').val() == "NEW CUSTOMER") {
                if ($('#txtQuoteCustomer').val().length == 0)
                    errmsg += 'Enter customer name<br/>';
                if ($('#txtBillCompanyName').val().trim().length == 0)
                    errmsg += 'Enter Billing Name M/S. <br/>';
                if ($('#txtBillAddress').val().trim().length == 0)
                    errmsg += 'Enter Billing Address <br/>';
                if ($('#txtBillCity').val().trim().length == 0)
                    errmsg += 'Enter Bill City <br/>';
                if ($('#txtBillZipCode').val().trim().length == 0)
                    errmsg += 'Enter Bill zipcode <br/>';
                if ($('#txtBillState').val().trim().length == 0)
                    errmsg += 'Enter Bill state <br/>';
                if ($('#txtBillStateCode').val().trim().length == 0)
                    errmsg += 'Enter Bill State Code <br/>';
                if ($('#txtBillGSTNo').val().trim().length == 0)
                    errmsg += 'Enter Bill GST No<br/>';
                if ($('#txtBillContactName').val().trim().length == 0)
                    errmsg += 'Enter Bill Contact Name <br/>';
                if ($('#txtBillContactNo').val().trim().length == 0)
                    errmsg += 'Enter Bill Contact No <br/>';
            }
            else if ($('input:radio[id*=rdbTYPECustomer_]:checked').val() == "EXISTING CUSTOMER") {
                if ($('#MainContent_ddlQuotecustomer option:selected').text() == "Choose")
                    errmsg += 'Choose existing customer name<br/>';
                if ($('#MainContent_ddlUsername option:selected').text() == "Choose")
                    errmsg += 'Choose user id<br/>';
                if ($('#ddlBillingAddress option:selected').text() == "Choose")
                    errmsg += 'Choose billing address<br/>';
                if ($('#ddlShippingAddress option:selected').text() == "Choose")
                    errmsg += 'Choose consignee address<br/>';
            }
            if ($('#txtQuoteEmail').val().length == 0)
                errmsg += 'Enter customer email<br/>';
            else if (emailpattern.test($('#txtQuoteEmail').val()) == false)
                errmsg += 'Enter vaild email <br />';
            if ($('input:radio[id*=rdbQuoteGrade_]:checked').length == 0)
                errmsg += 'Choose grade <br />';
            if ($('#ddlPlatform option:selected').text() == 'Choose' || $('#ddlPlatform option:selected').text() == "")
                errmsg += 'Choose platform<br/>';
            if ($('#ddlBrand option:selected').text() == 'Choose' || $('#ddlBrand option:selected').text() == "")
                errmsg += 'Choose brand<br/>';
            if ($('#ddlSidewall option:selected').text() == 'Choose' || $('#ddlSidewall option:selected').text() == "")
                errmsg += 'Choose Sidewall<br/>';
            if ($('#ddlType option:selected').text() == 'Choose' || $('#ddlType option:selected').text() == "")
                errmsg += 'Choose Type<br/>';
            if ($('#txtQuoteDiscount').val().length == 0)
                errmsg += 'Enter discount %<br/>';
            else if (parseFloat($('#txtQuoteDiscount').val()) > parseFloat($('#hdnMaxQuoteDiscount').val()))
                errmsg += 'Maximum Discount ' + $('#hdnMaxQuoteDiscount').val();
            if ($('#ddlSize option:selected').text() == 'Choose' || $('#ddlSize option:selected').text() == "")
                errmsg += 'Choose Size<br/>';
            if ($('#ddlRim option:selected').text() == 'Choose' || $('#ddlRim option:selected').text() == "")
                errmsg += 'Choose rim<br/>';
            if ($('#txtCustomPriceSheet').val().trim().length == 0)
                errmsg += 'Enter Sheet price <br/>';
            else if (parseFloat($('#txtCustomPriceSheet').val()) == 0)
                errmsg += 'sheet price must not 0<br/>';
            if ($('#txtBasicPrice').val().trim().length == 0)
                errmsg += 'Enter Basic price <br/>';
            else if (parseFloat($('#txtBasicPrice').val()) == 0)
                errmsg += 'Basic price must not 0 <br/>';
            if ($('#txtQuoteQty').val().trim().length == 0)
                errmsg += 'Enter qty<br/>';
            else if (parseFloat($('#txtQuoteQty').val()) <= 0)
                errmsg += 'quantity must not 0 <br/>';
            if (errmsg.length > 0) {
                $('#lblErrMsg').html(errmsg);
                return false;
            }
            else
                return true;
        }

        function bind_errmsg(strErr) { $('#lblErrMsg').html(strErr); }

        function CtrlPrepareQuote() {
            var errmsg = "";
            if ($('#MainContent_gvQuoteItem tr').length == 0)
                errmsg += 'Please add any one item<br/>';
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
            if (errmsg.length > 0) {
                $('#lblErrMsg').html(errmsg);
                return false;
            }
            else
                return true;
        }

        function CalcFromDiscount() {
            if ($("#txtQuoteDiscount").val() == 'NaN') $("#txtQuoteDiscount").val('0');
            if ($("#txtBasicPrice").val() == 'NaN') $("#txtBasicPrice").val('0');
            if ($("#txtCustomPriceSheet").val() == 'NaN') $("#txtCustomPriceSheet").val('0');

            if (parseFloat($('#txtQuoteDiscount').val()) > parseFloat($("#hdnMaxQuoteDiscount").val())) {
                alert('Your maximum discount limit is ' + $("#hdnMaxQuoteDiscount").val());
                $('#txtQuoteDiscount').val($("#hdnMaxQuoteDiscount").val());
            }
            var custSheetPrice = parseFloat($('#txtCustomPriceSheet').val() != '' ? $('#txtCustomPriceSheet').val() : 0);
            var custBasicPrice = parseFloat($('#txtBasicPrice').val != '' ? $('#txtBasicPrice').val() : 0);
            var custDiscPer = parseFloat($('#txtQuoteDiscount').val != '' ? $('#txtQuoteDiscount').val() : 0);
            if (custDiscPer != 0 && custSheetPrice != 0)
                $('#txtBasicPrice').val(Math.round(parseFloat((custSheetPrice - (custSheetPrice * (custDiscPer / 100))))).toFixed(2));
            else
                $('#txtBasicPrice').val(Math.round(parseFloat((custSheetPrice))).toFixed(2));
        }

        function CalcEditDisc(eve) {
            var ctrlDiscID = eve.id;
            var ctrlBasicPrice = ctrlDiscID.replace('txtEditQuoteDisc', 'lblBasicPrice');
            var ctrlListPrice = ctrlDiscID.replace('txtEditQuoteDisc', 'lblListPrice');

            if ($("#" + ctrlDiscID).val() == 'NaN') $("#" + ctrlDiscID).val('0');
            if (parseFloat($('#' + ctrlDiscID).val()) > parseFloat($("#hdnMaxQuoteDiscount").val())) {
                alert('Your maximum discount limit is ' + $("#hdnMaxQuoteDiscount").val());
                $('#' + ctrlDiscID).val($("#hdnMaxQuoteDiscount").val());
            }
            if ($("#" + ctrlDiscID).val() != 0 && $("#" + ctrlListPrice).html() != 0)
                $('#' + ctrlBasicPrice).html(Math.round(parseFloat((($("#" + ctrlListPrice).html()) - (($("#" + ctrlListPrice).html()) * (($("#" + ctrlDiscID).val()) / 100))))).toFixed(2));
            else
                $('#' + ctrlBasicPrice).html(Math.round(parseFloat((($("#" + ctrlListPrice).html())))).toFixed(2));
        }

        function CtrlQUoteItemUpdate(eve) {
            var ctrlDiscID = eve.id;
            var ctrlUpdDisc = ctrlDiscID.replace('btnUpdate', 'txtEditQuoteDisc');
            if ($("#" + ctrlUpdDisc).val() == 'NaN' || $("#" + ctrlUpdDisc).val() == '') $("#" + ctrlUpdDisc).val('0');
            if ($('#' + ctrlUpdDisc).val().length == 0) {
                alert('Enter discount value');
                return false;
            }
            else
                return true;
        }

        function gotoQuoteDiv(ctrlID) {
            $('#' + ctrlID).css({ 'display': 'block' });
            $("html, body").stop().animate({
                scrollLeft: $('#' + ctrlID).offset().left,
                scrollTop: $('#' + ctrlID).offset().top
            }, 1200);
        }
    </script>
</asp:Content>
