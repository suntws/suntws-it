<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="claimsettle.aspx.cs" Inherits="TTS.claimsettle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div align="center" class="pageTitleHead">
        <asp:Label runat="server" ID="lblPageHead" ClientIDMode="Static" Text=""></asp:Label>
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table style="width: 1070px;">
            <tr id="trDdlPlant" runat="server" clientidmode="Static" style="text-align: center;
                background-color: #cec0ea; font-weight: bold;">
                <td>
                    PLANT :
                    <asp:DropDownList runat="server" ID="ddlAccPlant" ClientIDMode="Static" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlAccPlant_IndexChanged">
                    </asp:DropDownList>
                    <asp:Label runat="server" ID="lblErr" ClientIDMode="Static" Text="" Font-Bold="true"
                        ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:GridView runat="server" ID="gvAccountSettlement" AutoGenerateColumns="false"
                        Width="1070px" RowStyle-Height="22px" 
                        onselectedindexchanged="gvAccountSettlement_SelectedIndexChanged">
                        <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" Height="22px" />
                        <Columns>
                            <asp:BoundField DataField="custname" HeaderText="CUSTOMER NAME" ItemStyle-Width="250px" />
                            <asp:BoundField DataField="complaintno" HeaderText="COMPLAINT NO." ItemStyle-Width="60px" />
                            <asp:BoundField DataField="complaintdate" HeaderText="COMPLAINT DATE" ItemStyle-Width="60px" />
                            <asp:BoundField DataField="Qty" HeaderText="QTY" ItemStyle-Width="40px" />
                            <asp:BoundField DataField="ClaimType" HeaderText="CREDIT NOTE" ItemStyle-Width="120px" />
                            <asp:BoundField DataField="claimstatus" HeaderText="STATUS" ItemStyle-Width="160px" />
                            <asp:BoundField DataField="totalamount" HeaderText="TOTAL AMOUNT" ItemStyle-Width="50px"
                                ItemStyle-HorizontalAlign="Right" />
                            <asp:TemplateField HeaderText="ACTION" ItemStyle-Width="50px">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hdnCondReprestatus" ClientIDMode="Static" Value='<%# Eval("creditReprepareStatus") %>' />
                                    <asp:HiddenField runat="server" ID="hdnstatus" ClientIDMode="Static" Value='<%# Eval("statusid") %>' />
                                    <asp:HiddenField runat="server" ID="hdnCreditFileName" ClientIDMode="Static" Value='<%# Eval("CreditNoteFile") %>' />
                                    <asp:HiddenField runat="server" ID="hdnCreditNoteNo" ClientIDMode="Static" Value='<%# Eval("CreditNoteNo") %>' />
                                    <asp:LinkButton ID="lnkClaimNo" runat="server" Text="Show List" OnClick="lnkClaimNo_Click" /></span>
                                    <asp:HiddenField runat="server" ID="hdnClaimCustCode" Value='<%# Eval("custcode") %>' />
                                    <asp:HiddenField runat="server" ID="hdnCrmSettleType" Value='<%# Eval("CrmSettleType") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="width: 1060px; float: left;">
                        <div style="width: 1058px; float: left; border: 1px solid #000; background-color: #056442;
                            font-weight: bold; color: #fff; font-size: 15px;">
                            <div style="width: 525px; float: left; text-align: left; padding-right: 5px;">
                                <asp:Label runat="server" ID="lblClaimCustName" ClientIDMode="Static" Text=""></asp:Label>
                            </div>
                            <div style="width: 2px; float: left;">
                                <asp:Label runat="server" ID="lblClaim" ClientIDMode="Static" Text=""></asp:Label>
                            </div>
                            <div style="width: 520px; float: left; text-align: right; padding-left: 5px;">
                                <asp:Label runat="server" ID="lblClaimNo" ClientIDMode="Static" Text=""></asp:Label>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
            <tr id="divClaimCreditEntry" style="display: none;">
                <td style="width: 1064px; float: left;">
                    <div style="border: 1px solid #000; margin-bottom: 5px; text-align: left;">
                        <span class="headCss" style="font-size: 15px;">SETTLEMENT OPINION: </span>
                        <asp:Label runat="server" ID="lblSettleOpinion" ClientIDMode="Static" Text="" Font-Bold="true"
                            Font-Size="15px"></asp:Label><br />
                        <asp:Label runat="server" ID="lblcrmreprepare" ClientIDMode="Static" Text="" Font-Bold="true"
                            Font-Size="15px"></asp:Label>
                        <asp:Label runat="server" ID="lblcrmmovedprepare" ClientIDMode="Static" Text=""></asp:Label>
                        <div style="text-align: right; color: #EC800C; font-weight: bold; display: none;"
                            id="divChangePrice">
                            DO YOU WANT CHANGE THE UNIT PRICE
                            <input type="checkbox" id="chkPriceChange" onclick="PriceChangeEnable()" />
                        </div>
                        <div>
                            <asp:GridView runat="server" ID="gvClaimApproveItems" AutoGenerateColumns="false"
                                Width="1058px" RowStyle-Height="22px" 
                                onselectedindexchanged="gvClaimApproveItems_SelectedIndexChanged">
                                <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" Height="22px" HorizontalAlign="Center" />
                                <Columns>
                                    <asp:BoundField DataField="config" HeaderText="PLATFORM" ItemStyle-Width="100px" />
                                    <asp:BoundField DataField="CrmType" HeaderText="TYPE" ItemStyle-Width="80px" />
                                    <asp:BoundField DataField="brand" HeaderText="BRAND" ItemStyle-Width="100px" />
                                    <asp:BoundField DataField="tyresize" HeaderText="TYRE SIZE" ItemStyle-Width="150px" />
                                    <asp:BoundField DataField="stencilno" HeaderText="STENCIL" ItemStyle-Width="80px" />
                                    <asp:TemplateField HeaderText="CLASSIFICATION" ItemStyle-Width="350px">
                                        <ItemTemplate>
                                            <%# Bind_ClaimClassification(Eval("stencilno").ToString()) %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="StencilCrmStatus" HeaderText="STATUS" ItemStyle-Width="60px" />
                                    <asp:BoundField DataField="ClaimDescription" HeaderText="COMPLAINT DESC" ItemStyle-Width="60px" />
                                    <asp:BoundField DataField="PreviewPrice" HeaderText="PREVIOUS CLAIM PRICE" ItemStyle-Width="90px"
                                        ItemStyle-HorizontalAlign="Right" />
                                    <asp:TemplateField HeaderText="ACTUAL TYRE PRICE" ItemStyle-Width="90px">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtTyrePrice" Text='<%# Eval("Tyreprice").ToString()==""?"0":Eval("Tyreprice").ToString() %>'
                                                MaxLength="12" Width="80px" Enabled="false"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="CURRENT CLAIM PRICE" ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hdnclaimprice" runat="server" Value='<%# Eval("unitprice") %>' />
                                            <asp:TextBox runat="server" ID="txtClaimPrice" Text='<%# Eval("unitprice") %>' Enabled="false"
                                                onkeypress="return isNumberKey(event)" onblur="isCompareTyreValue(event,this)"
                                                MaxLength="12" Width="80px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <div style="border: 1px solid #000; margin-bottom: 5px; text-align: left;">
                                <asp:Label runat="server" ID="lbl1" ClientIDMode="Static" Text="" CssClass="headCss"></asp:Label>
                                <asp:GridView runat="server" ID="gvOTHERPLANT" AutoGenerateColumns="false" Width="1058px"
                                    RowStyle-Height="22px">
                                    <HeaderStyle BackColor="#AAF18D" Font-Bold="true" Height="22px" HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:BoundField DataField="config" HeaderText="PLATFORM" ItemStyle-Width="100px" />
                                        <asp:BoundField DataField="CrmType" HeaderText="TYPE" ItemStyle-Width="80px" />
                                        <asp:BoundField DataField="brand" HeaderText="BRAND" ItemStyle-Width="100px" />
                                        <asp:BoundField DataField="tyresize" HeaderText="TYRE SIZE" ItemStyle-Width="150px" />
                                        <asp:BoundField DataField="stencilno" HeaderText="STENCIL" ItemStyle-Width="80px" />
                                        <asp:TemplateField HeaderText="CLASSIFICATION" ItemStyle-Width="350px">
                                            <ItemTemplate>
                                                <%# Bind_ClaimClassification(Eval("stencilno").ToString()) %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="StencilCrmStatus" HeaderText="STATUS" ItemStyle-Width="60px" />
                                        <asp:BoundField DataField="ClaimDescription" HeaderText="COMPLAINT DESC" ItemStyle-Width="60px" />
                                        <asp:BoundField DataField="PreviewPrice" HeaderText="PREVIOUS CLAIM PRICE" ItemStyle-Width="90px"
                                            ItemStyle-HorizontalAlign="Right" />
                                        <asp:TemplateField HeaderText="ACTUAL TYRE PRICE" ItemStyle-Width="90px">
                                            <ItemTemplate>
                                                <asp:TextBox runat="server" ID="txtTyrePrice" Text='<%# Eval("Tyreprice").ToString()==""?"0":Eval("Tyreprice").ToString() %>'
                                                    MaxLength="12" Width="80px" Enabled="false"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="CURRENT CLAIM PRICE" ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdnclaimprice" runat="server" Value='<%# Eval("unitprice") %>' />
                                                <asp:TextBox runat="server" ID="txtClaimPrice" Text='<%# Eval("unitprice") %>' Enabled="false"
                                                    onkeypress="return isNumberKey(event)" onblur="isCompareTyreValue(event,this)"
                                                    MaxLength="12" Width="80px"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                        <div style="text-align: right; display: none;" id="divSavePrice">
                            <asp:Button runat="server" ID="btnSettlePrice" ClientIDMode="Static" Text="SAVE UNIT PRICE"
                                OnClientClick="javascript:return CtrlSaveUnirprice();" CssClass="btnshow" OnClick="btnSettlePrice_Click" />
                            <asp:Button runat="server" ID="btnPriceChangeDisable" ClientIDMode="Static" Text="CANCEL"
                                CssClass="btnclear" OnClick="btnPriceChangeDisable_Click" />
                        </div>
                    </div>
                    <div id="divClaimforotherplant" style="width: 1060px; float: left; display: none;
                        border: 1px solid #000; margin-bottom: 5px; padding-bottom: 5px;">
                        <div style="width: 1060px; float: left; margin-bottom: 5px; margin-top: 5px;">
                            <span id="divBlinkStencil" style="font-size: 16px; font-weight: bold;">OTHER PLANT STATUS
                                FOR THIS COMPLAINT NO.</span>
                            <asp:GridView runat="server" ID="gvother" AutoGenerateColumns="false" Width="1060px"
                                RowStyle-Height="22px">
                                <HeaderStyle BackColor="#BEEFEB" Font-Bold="true" Height="22px" HorizontalAlign="Center" />
                                <Columns>
                                    <asp:TemplateField ItemStyle-Width="50px" HeaderText="SELECT">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkclaimassign" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="plant" HeaderText="PLANT" ItemStyle-Width="100px" />
                                    <asp:BoundField DataField="qty" HeaderText="QTY" ItemStyle-Width="60px" />
                                    <asp:BoundField DataField="claimstatus" HeaderText="STATUS" ItemStyle-Width="200px" />
                                    <asp:BoundField DataField="CrmSettleType" HeaderText="CRM SETTLEMENT OPINION" ItemStyle-Width="200px" />
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div style="width: 1060px; float: left; line-height: 20px; margin-bottom: 5px; display: none;"
                            id="divAddOtherPlant">
                            <div style="width: 630px; float: left; color: #ff0000;">
                                Do you want merge the other plant stencil no. for same credit note choose above
                                checkbox list then click this button
                            </div>
                            <div style="width: 300px; float: left;">
                                <asp:Button runat="server" ID="btnMergePlant" ClientIDMode="Static" Text="MERGE OTHER PLANT STENCIL NO"
                                    OnClientClick="javascript:return ChkMorePlantForSameComplaint();" OnClick="btnMergePlant_Click"
                                    CssClass="btnauthorize" />
                            </div>
                            <div style="width: 130px; float: left;">
                                <asp:Label runat="server" ID="lblErrMsgSamePlant" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div style="width: 1060px; float: left; border: 1px solid #000; margin-bottom: 5px;">
                        <div style="width: 1060px; float: left; margin-bottom: 5px;">
                            <div style="width: 560px; float: left;">
                                <asp:GridView runat="server" ID="gvCrmComments" AutoGenerateColumns="false" Width="500px"
                                    HeaderStyle-BackColor="#B9D6B9">
                                    <Columns>
                                        <asp:BoundField DataField="plant" HeaderText="PLANT" ItemStyle-Width="90px" />
                                        <asp:TemplateField HeaderText="CRM COMMENTS" ItemStyle-Width="400px">
                                            <ItemTemplate>
                                                <%#Eval("crmcomments") != DBNull.Value ? ((string)Eval("crmcomments")).Replace("~", "<br/>") : ""%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div style="width: 500px; float: left;">
                                <asp:GridView runat="server" ID="gvClaimOtherCharges" AutoGenerateColumns="false"
                                    Width="500px" HeaderStyle-BackColor="#B9D6B9">
                                    <Columns>
                                        <asp:BoundField DataField="slno" Visible="false" />
                                        <asp:TemplateField ItemStyle-Width="350px" HeaderText="OTHER CHARGES DESCRIPTION"
                                            HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:TextBox runat="server" ID="txtClaimAddDesc" Text="" Width="350px" MaxLength="100"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="35px" HeaderText="+/-">
                                            <ItemTemplate>
                                                <asp:DropDownList runat="server" ID="ddlClaimCalcType" Width="35px">
                                                    <asp:ListItem Text="+" Value="ADD"></asp:ListItem>
                                                    <asp:ListItem Text="-" Value="LESS"></asp:ListItem>
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="70px" HeaderText="AMOUNT">
                                            <ItemTemplate>
                                                <asp:TextBox runat="server" ID="txtClaimAddAmt" onkeypress="return isNumberKey(event)"
                                                    Text="" Width="70px" MaxLength="8"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                        <div style="width: 1060px; float: left; text-align: center; display: block;" id="divBtnCreditNote">
                            <asp:Label runat="server" ID="lblErrMsg1" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label>
                            <asp:Button runat="server" ID="btnCreditNote" ClientIDMode="Static" Text="PREPARE CREDIT NOTE PDF FILE"
                                CssClass="btnshow" OnClick="btnCreditNote_Click" OnClientClick="javascript:return otherdescription();" />
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="width: 1060px; float: left; border: 1px solid #000; margin-bottom: 5px;">
                        <asp:Label runat="server" ID="lblMsg" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label>
                    </div>
                </td>
            </tr>
            <tr id="divClaimAccountEntry" style="display: none;">
                <td>
                    <div style="width: 1060px; float: left; border: 1px solid #000; margin-bottom: 5px;">
                        <div style="width: 1060px; float: left; text-align: center; font-size: 16px; font-weight: bold;
                            background-color: #ccc; margin-top: 5px;">
                            <span>CREDIT NOTE:</span>
                            <asp:Label runat="server" ID="lblCreditNoteNo" ClientIDMode="Static" Text="" Font-Bold="true"
                                ForeColor="Green"></asp:Label>
                        </div>
                        <div style="width: 1060px; float: left;">
                            <asp:GridView runat="server" ID="gv_SettleItems" AutoGenerateColumns="false" Width="1060px"
                                RowStyle-Height="22px">
                                <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" Height="22px" HorizontalAlign="Center" />
                                <Columns>
                                    <asp:BoundField DataField="config" HeaderText="PLATFORM" ItemStyle-Width="100px" />
                                    <asp:BoundField DataField="CrmType" HeaderText="TYPE" ItemStyle-Width="80px" />
                                    <asp:BoundField DataField="brand" HeaderText="BRAND" ItemStyle-Width="100px" />
                                    <asp:BoundField DataField="tyresize" HeaderText="TYRE SIZE" ItemStyle-Width="150px" />
                                    <asp:BoundField DataField="stencilno" HeaderText="STENCIL" ItemStyle-Width="80px" />
                                    <asp:BoundField DataField="assigntoqc" HeaderText="PLANT" ItemStyle-Width="80px" />
                                    <asp:BoundField DataField="StencilCrmStatus" HeaderText="STATUS" ItemStyle-Width="60px" />
                                    <asp:BoundField DataField="ClaimDescription" HeaderText="COMPLAINT DESC" ItemStyle-Width="60px" />
                                    <asp:BoundField DataField="TyrePrice" HeaderText="ACTUAL TYRE PRICE" ItemStyle-Width="80px" />
                                    <asp:BoundField DataField="unitprice" HeaderText="CLAIM PRICE" ItemStyle-Width="80px"
                                        ItemStyle-HorizontalAlign="Right" />
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div style="width: 1060px; float: left;">
                            <div style="float: left;">
                                <span class="headCss">DOWNLOAD: </span>
                            </div>
                            <div style="float: left;">
                                <span>PDF FILE :</span>
                                <asp:LinkButton runat="server" ID="lnkCrditNoteDownload" ClientIDMode="Static" Text="CREDIT NOTE"
                                    OnClick="lnkCrditNoteDownload_Click"></asp:LinkButton></div>
                            <div style="float: left; padding-left: 30px;">
                                <asp:Label ID="lblexcel" runat="server" ClientIDMode="Static" Text=""></asp:Label>
                                <asp:LinkButton runat="server" ID="lnkCrditNoteexcel" ClientIDMode="Static" Text="CREDIT NOTE"
                                    OnClick="lnkCrditNoteexcel_Click"></asp:LinkButton></div>
                        </div>
                        <div id="divsettleplant" style="width: 1060px; float: left; display: none; border: 1px solid #000;
                            margin-bottom: 5px; padding-bottom: 5px;">
                            <div style="width: 1060px; float: left; margin-bottom: 5px; margin-top: 5px;">
                                <span id="Span1" style="font-size: 16px; font-weight: bold;">OTHER PLANT STATUS FOR
                                    THIS COMPLAINT NO.</span>
                                <asp:GridView runat="server" ID="gvothersettle" AutoGenerateColumns="false" Width="1060px"
                                    RowStyle-Height="22px">
                                    <HeaderStyle BackColor="#BEEFEB" Font-Bold="true" Height="22px" HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:BoundField DataField="plant" HeaderText="PLANT" ItemStyle-Width="100px" />
                                        <asp:BoundField DataField="claimstatus" HeaderText="STATUS" ItemStyle-Width="200px" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div style="width: 630px; float: left; color: #ff0000;">
                                Above plant are in same credit no, So please wait for settlement
                            </div>
                        </div>
                        <div style="width: 1060px; float: left; border: 1px solid #000; margin-bottom: 5px;
                            display: block;" id="divBtnSettlement">
                            <div style="width: 1060px; float: left; text-align: left; line-height: 20px;">
                                <asp:Label runat="server" ID="lblCreditApprovedComments" ClientIDMode="Static" Text=""></asp:Label>
                                <br />
                                <asp:Label runat="server" ID="lblCreditApprovedUser" ClientIDMode="Static" Text=""></asp:Label>
                                <br />
                                <asp:Label runat="server" ID="lblTotalprice" ClientIDMode="Static" Text=""></asp:Label>
                                <div style="width: 1060px; float: left; text-align: center;">
                                    <asp:Label runat="server" ID="lblmsg1" ClientIDMode="Static" Text="" ForeColor="Red"
                                        Font-Bold="true"></asp:Label>
                                </div>
                                <div id="divdispatch" style="float: left; display: none;">
                                    <div style="width: 520px; float: left; text-align: left; background-color: #FBE4E4;">
                                        <asp:Label runat="server" ID="lblDCno" ClientIDMode="Static" Text=""></asp:Label>
                                        <br />
                                        <asp:Label runat="server" ID="lbljjno" ClientIDMode="Static" Text=""></asp:Label>
                                        <br />
                                        <asp:Label runat="server" ID="lblqty" ClientIDMode="Static" Text=""></asp:Label>
                                    </div>
                                    <div style="width: 530px; float: left; text-align: left; background-color: #EADCC7;">
                                        <asp:Label runat="server" ID="lbltransport" ClientIDMode="Static" Text=""></asp:Label>
                                        <br />
                                        <asp:Label runat="server" ID="lbllrno" ClientIDMode="Static" Text=""></asp:Label>
                                        <br />
                                        <asp:Label runat="server" ID="lbllrdate" ClientIDMode="Static" Text=""></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div style="width: 1060px; float: left; margin-top: 5px;">
                                <div style="width: 525px; float: left;">
                                    <span class="headCss">INVOICE NO.</span>
                                    <asp:TextBox runat="server" ID="txtSettleInvoiceNo" ClientIDMode="Static" Text=""></asp:TextBox>
                                </div>
                            </div>
                            <div>
                                <span class="headCss">SETTLEMENT/CLOSE COMMENTS</span>
                                <asp:TextBox runat="server" ID="txtAccountsComments" ClientIDMode="Static" Text=""
                                    TextMode="MultiLine" Width="1050px" Height="80px" onKeyUp="javascript:CheckMaxLength(this, 999);"
                                    onChange="javascript:CheckMaxLength(this, 999);"></asp:TextBox>
                                <div style="text-align: center;">
                                    <asp:Button runat="server" ID="btnAccountsStatusChange" ClientIDMode="Static" Text="SAVE SETTLEMENT DETAILS & CLOSE CLAIM"
                                        OnClientClick="javascript:return CtrlAccountsSettle();" CssClass="btnshow" OnClick="btnAccountsStatusChange_Click" />
                                </div>
                            </div>
                        </div>
                        <div style="width: 1060px; float: left; margin-bottom: 5px; display: none;" id="divBtnCreditApproval">
                            <div id="divPdfClaim" runat="server" style="width: 988px; float: left;">
                            </div>
                            <div>
                                <span class="headCss">ANY COMMENTS: </span>
                                <asp:TextBox runat="server" ID="txtCreditNoComments" ClientIDMode="Static" Text=""
                                    TextMode="MultiLine" Width="1050px" Height="80px" onKeyUp="javascript:CheckMaxLength(this, 999);"
                                    onChange="javascript:CheckMaxLength(this, 999);"></asp:TextBox>
                            </div>
                            <div style="width: 1060px; float: left; text-align: center;">
                                <asp:Button runat="server" ID="btnCreditNoteApproval" ClientIDMode="Static" Text=""
                                    OnClientClick="javascript:return ctrlCreditNoteApproval();" OnClick="btnCreditNoteApproval_Click"
                                    CssClass="btnauthorize" />
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="display: none;">
                        <asp:GridView ID="gv_Excel" runat="server" AutoGenerateColumns="false" ClientIDMode="Static"
                            EnableEventValidation="false">
                            <HeaderStyle CssClass="headerNone" />
                            <Columns>
                                <asp:TemplateField ItemStyle-Width="35px">
                                    <ItemTemplate>
                                        <%# Eval("Excel")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnStatusid" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnCustCode" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnAcEnterAmt" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnSelectIndex" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnStencilPlant" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnotherplant" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdntotalqty" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdntotalprice" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnCreditCurrency" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnCreditNote" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnCondinalStatus" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnCreditFile" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdncondreparestatus" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnaddress" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdncustaddress" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        $(document).ready(function () {
            blinkStencil();
            $("input:checkbox[id*=MainContent_gvother_chkclaimassign_]").change(function () {
                $('#divBtnCreditNote').css({ 'display': 'block' });
                $('#MainContent_gvOTHERPLANT tr').remove();
                if ($("input:checkbox[id*=MainContent_gvother_chkclaimassign]:checked").length > 0)
                    $('#divBtnCreditNote').css({ 'display': 'none' });
            });
        });
        function otherdescription() {
            var errmsg = ''; $('#lblErrMsg1').html('');
            if ($('#lblSettleOpinion').html() != "Free replacement in next shipment") {
                $('#MainContent_gvClaimApproveItems tr').find('td:eq(6)').each(function (e) {
                    if ($(this).html() != 'REJECT' && $('#MainContent_gvClaimApproveItems_txtClaimPrice_' + e).val() == '0.00')
                        errmsg += 'Enter unit price<br/>';
                });
                $('#MainContent_gvOTHERPLANT tr').find('td:eq(6)').each(function (e) {
                    if ($(this).html() != 'REJECT' && $('#MainContent_gvOTHERPLANT_txtClaimPrice_' + e).val() == '0.00')
                        errmsg += 'Enter unit price<br/>';
                });
            }
            $("input:text[id*=MainContent_gvClaimOtherCharges_txtClaimAddDesc_]").each(function () {
                var id1 = this.id; var amtId = id1.replace('txtClaimAddDesc_', 'txtClaimAddAmt_');
                if ($('#' + id1).val() != '' && $('#' + amtId).val() == '')
                    errmsg += 'Enter extra charges amount<br/>';
                if ($('#' + id1).val() == '' && $('#' + amtId).val() != '')
                    errmsg += 'Enter extra charges description<br/>';
            });
            if (errmsg.length > 0) {
                $('#lblErrMsg1').html(errmsg);
                return false;
            }
            else
                return true;
        }

        function PriceChangeEnable() {
            $('#divBtnCreditNote').css({ 'display': 'none' });
            $('#divSavePrice').show();
            $('#chkPriceChange').attr('disabled', true);
            $('input:text[id*=MainContent_gvClaimApproveItems_txtClaimPrice_]').attr('disabled', false);
            if ($('#MainContent_gvOTHERPLANT tr').length > 0)
                $('input:text[id*=MainContent_gvOTHERPLANT_txtClaimPrice]').attr('disabled', false);
            pricechangehide();
        }
        function pricechangehide() {
            $('#MainContent_gvClaimApproveItems tr').find('td:eq(6)').each(function (e) {
                if ($(this).html() == 'REJECT')
                    $('#MainContent_gvClaimApproveItems_txtClaimPrice_' + e).attr('disabled', true);
            });
            $('#MainContent_gvOTHERPLANT tr').find('td:eq(6)').each(function (e) {
                if ($(this).html() == 'REJECT')
                    $('#MainContent_gvOTHERPLANT_txtClaimPrice_' + e).attr('disabled', true);
            });
        }
        function PriceChangeDisable() {
            $('#divBtnCreditNote').css({ 'display': 'block' });
            $('#chkPriceChange').attr('disabled', false);
        }

        function ChkMorePlantForSameComplaint() {
            //chkgvotherplant();
            $('#lblErrMsgSamePlant').html(''); var errMsg = '';
            $('#MainContent_gvother tr').find('td:eq(3)').each(function (e) {
                if ($("input:checkbox[id*=MainContent_gvother_chkclaimassign_" + e + "]:checked").length == 0 && $(this).html().trim() == 'WAITING FOR CREDIT NOTE PREPARATION')
                    errMsg += 'Please checked any one checkbox<br/>';
                else if ($("input:checkbox[id*=MainContent_gvother_chkclaimassign_" + e + "]:checked").length != 0 && $(this).html().trim() == 'WAITING FOR CREDIT NOTE PREPARATION')
                    return false;
            });
            $('#hdnotherplant').val('');
            $('#MainContent_gvother tr').find('td:eq(1)').each(function (e) {
                var hdnplant = $('#hdnotherplant').val();
                if (hdnplant.length == 0 && $("input:checkbox[id*=MainContent_gvother_chkclaimassign_" + e + "]:checked").length != 0)
                    hdnplant = $(this).html().trim();
                else if (hdnplant.length != 0 && $("input:checkbox[id*=MainContent_gvother_chkclaimassign_" + e + "]:checked").length != 0)
                    hdnplant = hdnplant + '~' + $(this).html().trim();
                $('#hdnotherplant').val(hdnplant);
            });
            if (errMsg.length > 0) {
                $('#lblErrMsgSamePlant').html(errMsg);
                return false;
            }
            else
                return true;
        }

        function chkgvotherplant() {
            $('#MainContent_gvother tr').find('td:eq(3)').each(function (e) {
                $("input:checkbox[id*=MainContent_gvother_chkclaimassign_" + e + "]").attr('disabled', true);
                if ($(this).html().trim() == 'WAITING FOR CREDIT NOTE PREPARATION' && $("#MainContent_gvother tr:eq(" + parseInt(e) + 1 + ")").find('td:eq(4)').html() == $('#lblSettleOpinion').html()) {
                    $("input:checkbox[id*=MainContent_gvother_chkclaimassign_" + e + "]").attr('disabled', false);
                    showOtherPlant('divAddOtherPlant');
                }
                showOtherPlant('divClaimforotherplant');
                showCreditEntry();
            });
        }
        function showCreditEntry() {
            $('#divClaimCreditEntry').fadeIn(1000);
            $('#divClaimAccountEntry').fadeOut();
            gotoClaimDiv('divClaimCreditEntry');
            showPriceTxtBox();
        }

        function showCreditApproval() {
            $('#divClaimCreditEntry').hide();
            $('#divClaimAccountEntry').show();
            $('#divBtnSettlement').hide();
            $('#divBtnCreditApproval').show();
            gotoClaimDiv('divBtnCreditApproval');
        }

        function showInvoiceEntry() {
            $('#divClaimAccountEntry').fadeIn(1000);
            $('#divClaimCreditEntry').fadeOut();
            gotoClaimDiv('divClaimAccountEntry');
        }
        function showOtherPlant(ctrl) {
            $('#' + ctrl).css("display", "block");
            gotoClaimDiv(ctrl);
        }

        function hideClaimAccounts() {
            $('#divClaimAccountEntry').fadeOut();
            $('#divClaimCreditEntry').fadeOut();
            $('#divChangePrice').hide();
            $('#divSavePrice').hide();
        }

        function showPriceOption() {
            $('#divChangePrice').show();
        }

        function gotoClaimDiv(ctrlID) {
            $("html, body").stop().animate({
                scrollLeft: $('#' + ctrlID).offset().left,
                scrollTop: $('#' + ctrlID).offset().top
            }, 1200);
        }

        function showPriceTxtBox() {
            $('#MainContent_gvClaimApproveItems tr').find('td:eq(5)').each(function (e) {
                $('#MainContent_gvClaimApproveItems_txtClaimPrice_' + e).show();
                if ($(this).html() == 'REJECT') {
                    $('#MainContent_gvClaimApproveItems_txtClaimPrice_' + e).hide();
                }
            });
            if ($('#MainContent_gvOTHERPLANT tr').length > 0) {
                $('#MainContent_gvOTHERPLANT tr').find('td:eq(5)').each(function (e) {
                    $('#MainContent_gvOTHERPLANT_txtClaimPrice_' + e).show();
                    if ($(this).html() == 'REJECT') {
                        $('#MainContent_gvOTHERPLANT_txtClaimPrice_' + e).hide();
                    }
                });
            }
        }

        function CtrlSaveUnirprice() {
            $('#lblMsg').html(''); var errMsg = ''; var rowCount = '';
            $('#MainContent_gvClaimApproveItems tr').find('td:eq(5)').each(function (e) {
                rowCount = parseInt(e) + 1;
                if ($(this).html() != 'REJECT') {
                    if ($('#MainContent_gvClaimApproveItems_txtClaimPrice_' + e).val().length == 0) {
                        if (errMsg.length > 0)
                            errMsg += "," + rowCount;
                        else
                            errMsg = rowCount;
                    }
                }
            });
            if (errMsg.length > 0) {
                $('#lblMsg').html('Record ' + errMsg + ' unit price value is invalid');
                return false;
            }
            else
                return true;
        }

        function ctrlCreditNoteApproval() {
            $('#lblMsg').html(''); var errMsg = '';
            if ($('#txtCreditNoComments').val().length == 0)
                errMsg += 'Enter credit note comment';
            if (errMsg.length > 0) {
                $('#lblMsg').html(errMsg);
                return false;
            }
            else
                return true;
        }

        function CtrlAccountsSettle() {
            $('#lblErrMsg').html(''); var errMsg = '';
            if ($('#txtSettleInvoiceNo').val().length == 0)
                errMsg += 'Enter Settlement Invoice No.<br />';
            if ($('#txtAccountsComments').val().length == 0)
                errMsg += 'Enter Settlement/Close comments<br />';
            if (errMsg.length > 0) {
                $('#lblErrMsg').html(errMsg);
                return false;
            }
            else
                return true;
        }
        function isCompareTyreValue(evt, source) {
            // check if the claim price is greater than the unit price. allow if less or equal.
            if (parseFloat($(source).val()) > parseFloat($(source).closest("td").prev().find("input").val())) {
                $(source).val($(source).closest("td").prev().find("input").val()); $(source).focus();
            }
        }
    </script>
</asp:Content>
