<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="claimopinion.aspx.cs" Inherits="TTS.claimopinion" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/lightbox.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .tbCrmCss
        {
            border-collapse: collapse;
            border-color: #000;
            width: 500px;
            float: left;
        }
        .tbCrmCss th
        {
            background-color: #ccc;
            font-weight: bold;
            text-align: left;
            line-height: 20px;
        }
    </style>
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
            <tr style="text-align: center; background-color: #cec0ea; font-weight: bold;">
                <td>
                    PLANT :
                    <asp:DropDownList runat="server" ID="ddlCrmPlant" ClientIDMode="Static" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlCrmPlant_IndexChanged">
                    </asp:DropDownList>
                    <asp:Label runat="server" ID="lblErr" ClientIDMode="Static" Text="" Font-Bold="true"
                        ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:GridView runat="server" ID="gvClaimCrmList" AutoGenerateColumns="false" Width="1060px"
                        RowStyle-Height="22px">
                        <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" Height="22px" />
                        <Columns>
                            <asp:BoundField DataField="custname" HeaderText="CUSTOMER NAME" ItemStyle-Width="300px" />
                            <asp:BoundField DataField="complaintno" HeaderText="COMPLAINT NO." ItemStyle-Width="80px" />
                            <asp:BoundField DataField="complaintdate" HeaderText="COMPLAINT DATE" ItemStyle-Width="80px" />
                            <asp:BoundField DataField="Qty" HeaderText="QTY" ItemStyle-Width="40px" />
                            <asp:BoundField DataField="claimstatus" HeaderText="STATUS" ItemStyle-Width="200px" />
                            <asp:TemplateField HeaderText="ACTION" ItemStyle-Width="60px">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkClaimNo" runat="server" Text="Show List" OnClick="lnkClaimNo_Click" /></span>
                                    <asp:HiddenField runat="server" ID="hdnClaimCustCode" Value='<%# Eval("custcode") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="width: 1060px; float: left;" id="divclaimitems">
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
                        <div style="width: 1060px; float: left;">
                            <asp:GridView runat="server" ID="gvClaimItems" AutoGenerateColumns="false" Width="1060px">
                                <HeaderStyle BackColor="#CACA55" Font-Bold="true" Height="22px" />
                                <Columns>
                                    <asp:BoundField DataField="brand" HeaderText="BRAND" ItemStyle-Width="100px" />
                                    <asp:BoundField DataField="tyresize" HeaderText="SIZE" ItemStyle-Width="150px" />
                                    <asp:BoundField DataField="stencilno" HeaderText="STENCIL NO." ItemStyle-Width="70px" />
                                    <asp:TemplateField HeaderText="COMPLAINT" ItemStyle-Width="200px">
                                        <ItemTemplate>
                                            <%#((string)Eval("appstyle")).Replace("~", "<br/>")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="OPERATING CONDITION" ItemStyle-Width="150px">
                                        <ItemTemplate>
                                            <%#((string)Eval("runninghours")).Replace("~", "<br/>")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ConclusionStatus" HeaderText="EDC CONCLUSION" ItemStyle-Width="100px" />
                                    <asp:BoundField DataField="CrmStatus" HeaderText="CRM APPROVED STATUS" ItemStyle-Width="80px" />
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div style="width: 1060px; float: left; text-align: left; margin-top: 5px;">
                            <asp:Label runat="server" ID="lblComplaintComments" ClientIDMode="Static" Text=""></asp:Label>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView runat="server" ID="dgAnalysisList" PageSize="1" AllowPaging="true"
                        AutoGenerateColumns="false" OnPageIndexChanging="dgAnalysisData_PageIndex" Width="1060px"
                        PagerStyle-HorizontalAlign="Center" PagerStyle-Font-Bold="true" PagerStyle-Font-Size="15px"
                        PagerStyle-BackColor="#F5F4F4">
                        <HeaderStyle CssClass="headerNone" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #C0F8F6;
                                        width: 1058px;">
                                        <tr>
                                            <td>
                                                STENCIL NO.
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblCRMOpinionSetncil" Text='<%# Eval("stencilno")%>'
                                                    Font-Bold="true" Font-Size="12px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                PLATFORM
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblconfig" Text='<%# Eval("config")%>' Font-Bold="true"
                                                    Font-Size="12px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                TYRE TYPE
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblType" Text='<%# Eval("tyretype") %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdnEDCType" Value='<%# Eval("EdcType")%>' />
                                                <asp:HiddenField runat="server" ID="hdnCustType" Value='<%# Eval("CustgvnType")%>' />
                                                <asp:HiddenField runat="server" ID="hdnCrmType" Value='<%# Eval("CrmType")%>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                COMPLAINT DESC
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblComplaintDesc" Text='<%# Eval("ClaimDescription")%>'
                                                    Font-Bold="true" Font-Size="12px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                EDC CONCLUSION
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblStencilConclusion" ClientIDMode="Static" Text='<%# Eval("QcConclusion")%>'
                                                    Font-Bold="true" Font-Size="12px"></asp:Label>
                                                <br />
                                                <%# Eval("conclusion").ToString() == "" ? "" : ((string)Eval("conclusion")).Replace("~", "<br/>")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                PREPARED BY:<b>
                                                    <%# Eval("PreparedBy")%></b>
                                            </td>
                                            <td>
                                                <div class="headCss">
                                                    PREPARER ROOT CAUSE / COMMENTS:</div>
                                                <%#Eval("QcComments").ToString()!=""? (((string)Eval("QcComments")).Replace("~", "<br/>")):""%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                VERIFIED BY: <b>
                                                    <%# Eval("VerifiedBy")%></b>
                                            </td>
                                            <td>
                                                <div class="headCss">
                                                    VERIFIER ROOT CAUSE / COMMENTS:</div>
                                                <%#Eval("rootcause").ToString() != "" ? (((string)Eval("rootcause")).Replace("~", "<br/>")) : ""%>
                                                <asp:HiddenField runat="server" ID="hdnEdcReOpinion" Value='<%# Eval("Crm_EdcReOpinion") %>' />
                                                <asp:HiddenField runat="server" ID="hdnEdcReopinionUpdate" Value='<%# Eval("CrmEdcReOpinionUpdateComments") %>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div style='width: 50%; float: left;'>
                                                    <div class="headCss">
                                                        <%#Eval("QcAdditionalReq").ToString() != "" ? "Additional Request:" : "" %>
                                                    </div>
                                                    <%#Eval("QcAdditionalReq").ToString() != "" ?  "&emsp; <b>QC :</b>" + Eval("QcAdditionalReq") + "<br/>" : "" %>
                                                    <%#Eval("QcAdditionalUpdateComments").ToString() != "" ?  "&emsp;<b>CRM  : </b>" + Eval("QcAdditionalReq") : "" %>
                                                </div>
                                                <div style='width: 50%; float: left;'>
                                                    <div class="headCss">
                                                        <%#Eval("EdcAdditionalReq").ToString() != "" ? "Additional Request:" : "" %>
                                                    </div>
                                                    <%#Eval("EdcAdditionalReq").ToString() != "" ?  "&emsp; <b>EDC : </b>" + Eval("EdcAdditionalReq") + "<br/>" : "" %>
                                                    <%#Eval("EdcAdditionalUpdateComments").ToString() != "" ?  "&emsp; <b>CRM : </b>" + Eval("EdcAdditionalUpdateComments") : "" %>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <div style="width: 50%; float: left; text-align: left; margin-top: 5px;">
                        <asp:Label runat="server" ID="lblEdcMovedUser" ClientIDMode="Static" Text=""></asp:Label>
                        <br />
                        <asp:Label runat="server" ID="lblEdcMovedComments" ClientIDMode="Static" Text=""></asp:Label>
                    </div>
                </td>
            </tr>
            <tr id="divCrmdecision" style="display: none;">
                <td style="width: 1060px; float: left; border: 1px solid #000;">
                    <div style="width: 500px; float: left;">
                        <table cellspacing="0" rules="all" border="1" class="tbCrmCss">
                            <tr>
                                <th>
                                    STENCIL NO
                                </th>
                                <td>
                                    <div style="width: 175px; float: left;" id="divBlinkStencil">
                                        <asp:Label runat="server" ID="lblCRMSetncil" ClientIDMode="Static" Text="" Font-Bold="true"
                                            Font-Size="20px"></asp:Label>
                                    </div>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblStencilShowImages" ClientIDMode="Static" Text=""
                                        ForeColor="Red" Font-Bold="true"></asp:Label>
                                    <asp:DataList runat="server" ID="gvClaimImages" RepeatColumns="1" RepeatDirection="Horizontal">
                                        <ItemTemplate>
                                            <a id="imageLink" href='<%# Eval("ClaimImage") %>' rel="lightbox[Brussels]" runat="server">
                                                Show Complaint Images</a>
                                        </ItemTemplate>
                                    </asp:DataList>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    STATUS
                                </th>
                                <td>
                                    <asp:RadioButtonList runat="server" ID="rdbClaimNoStatus" ClientIDMode="Static" RepeatColumns="2"
                                        RepeatDirection="Horizontal" Width="175px">
                                        <asp:ListItem Text="ACCEPT" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="REJECT" Value="0"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td>
                                    <span class="headCss">PLATFORM: </span>
                                    <asp:DropDownList ID="ddlplatform" ClientIDMode="Static" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <th style="line-height: 15px;">
                                    TYRE TYPE MENTIONED BY
                                </th>
                                <td>
                                    <asp:Label runat="server" ID="lblCustType" Text=""></asp:Label>
                                    <asp:Label runat="server" ID="lblQcType" Text=""></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblEdcType" Text=""></asp:Label>
                                    <br />
                                    <span class="headCss">CRM: </span>
                                    <asp:DropDownList ID="ddlType" ClientIDMode="Static" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <span class="headCss">DECISION COMMENTS</span>
                                    <asp:TextBox runat="server" ID="txtCommercialDisc" ClientIDMode="Static" Text=""
                                        TextMode="MultiLine" Width="490px" Height="55px" onKeyUp="javascript:CheckMaxLength(this, 999);"
                                        onChange="javascript:CheckMaxLength(this, 999);"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Label runat="server" ID="lblRecheckData" ForeColor="#9C27B0" Font-Bold="true"
                                        Text="If you want re-opinion from EDC?"></asp:Label>
                                    <br />
                                    ENTER YOUR COMMENTS / INSTRUCTION:
                                    <asp:TextBox runat="server" ID="txtEdcReOpinion" ClientIDMode="Static" Text="" TextMode="MultiLine"
                                        Width="490px" Height="35px" onKeyUp="javascript:CheckMaxLength(this, 499);" onChange="javascript:CheckMaxLength(this, 499);"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="width: 560px; float: left;">
                        <span class="headCss">CLAIM CLASSIFICATION IN ACCOUNTING</span>
                        <asp:GridView runat="server" ID="gvClassification" AutoGenerateColumns="false" Width="560px"
                            AlternatingRowStyle-BackColor="#F7F5F5">
                            <HeaderStyle CssClass="headerNone" />
                            <Columns>
                                <asp:TemplateField ItemStyle-Width="150px">
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chkClassification" Text='<%# Eval("text") %>' Font-Bold="true" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtClassification" Text="" Width="400px" MaxLength="400"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <div style="width: 560px; float: left; text-align: center;">
                            <asp:Button runat="server" ID="btnReOpinion" ClientIDMode="Static" Text="SAVE RE-OPINION COMMENTS / INSTRUCTION"
                                CssClass="btncalc" OnClick="btnReOpinion_Click" />
                            <asp:Button runat="server" ID="btnCrmStautsChange" ClientIDMode="Static" Text="SAVE CRM DECISION & STATUS CURRENT STENCIL NO."
                                OnClientClick="javascript:return CtrlSettlementOpinion();" CssClass="btnshow"
                                OnClick="btnCrmStautsChange_Click" />
                        </div>
                    </div>
                </td>
            </tr>
            <tr id="divSettleOpinion" style="display: none;">
                <td style="width: 1064px; float: left;">
                    <div style="width: 1062px; float: left; border: 1px solid #000; margin-bottom: 5px;
                        text-align: left;">
                        <div style="margin-bottom: 5px; text-align: left;">
                            <span class="headCss" style="width: 150px; float: left; line-height: 28px;">SETTLEMENT
                                OPINION</span>
                            <asp:RadioButtonList runat="server" ID="rdbSettlementOpinion" ClientIDMode="Static"
                                RepeatColumns="3" RepeatDirection="Horizontal" Width="700px" Font-Bold="true">
                                <asp:ListItem Text="Free replacement in next shipment" Value="Free replacement in next shipment"></asp:ListItem>
                                <asp:ListItem Text="Adjustable in the subsequent invoice" Value="Adjustable in the subsequent invoice"></asp:ListItem>
                                <asp:ListItem Text="Claim amount to be refunded" Value="Claim amount to be refunded"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                        <div>
                            <asp:GridView runat="server" ID="gvOpinionItems" AutoGenerateColumns="false" Width="1058px">
                                <HeaderStyle BackColor="#C7C705" Font-Bold="true" Height="22px" HorizontalAlign="Center"
                                    VerticalAlign="Middle" />
                                <Columns>
                                    <asp:BoundField DataField="config" HeaderText="PLATFORM" ItemStyle-Width="120px" />
                                    <asp:BoundField DataField="tyretype" HeaderText="TYPE" ItemStyle-Width="100px" />
                                    <asp:BoundField DataField="brand" HeaderText="BRAND" ItemStyle-Width="120px" />
                                    <asp:BoundField DataField="tyresize" HeaderText="SIZE" ItemStyle-Width="150px" />
                                    <asp:BoundField DataField="stencilno" HeaderText="STENCIL" ItemStyle-Width="80px" />
                                    <asp:TemplateField HeaderText="CLASSIFICATION" ItemStyle-Width="350px">
                                        <ItemTemplate>
                                            <%# Bind_ClaimClassification(Eval("stencilno").ToString()) %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="StencilCrmStatus" HeaderText="STATUS" ItemStyle-Width="60px" />
                                    <asp:TemplateField HeaderText="ACTUAL PRICE" ItemStyle-Width="90px">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtTyrePrice" Text='<%# Eval("Tyreprice").ToString()==""?"0":Eval("Tyreprice").ToString() %>'
                                                onkeypress="return isNumberKey(event)" onkeyup="valCopyToNextTxt(event,this)"
                                                MaxLength="12" Width="80px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="CLAIM PRICE" ItemStyle-Width="90px">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtClaimPrice" Text='<%# Eval("unitprice").ToString()==""?"0":Eval("unitprice").ToString() %>'
                                                onkeypress="return isRangeValid(event,this)" MaxLength="12" Width="80px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div style="width: 1058px; float: left; margin-top: 10px;">
                            <span class="headCss" style="vertical-align: top;">ANY COMMENTS TO ACCOUNTS:</span>
                            <asp:TextBox runat="server" ID="txtMoveToAccount" ClientIDMode="Static" Text="" TextMode="MultiLine"
                                Width="1050px" Height="100px" onKeyUp="javascript:CheckMaxLength(this, 999);"
                                onChange="javascript:CheckMaxLength(this, 999);"></asp:TextBox>
                        </div>
                    </div>
                    <div style="border: 1px solid #000; margin-bottom: 5px; text-align: center;">
                        <asp:Button runat="server" ID="btnMoveToAccount" ClientIDMode="Static" Text="" CssClass="btncalc"
                            OnClick="btnMoveToAccount_Click" OnClientClick="javascript:return CtrlMoveToAccount();" />
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="width: 532px; float: right; padding-top: 5px; display: none;" id="tdEdcReOpinion">
                        <div style="width: 350px; float: left; text-align: right;">
                            <asp:Label runat="server" ID="lblRechkMsg" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label>
                        </div>
                        <div style="width: 275px; float: right;">
                            <asp:Button runat="server" ID="btnmovetoEdcReopinion" Text="MOVE TO EDC FOR RE-OPINION"
                                CssClass="btnedit" OnClick="btnmovetoEdcReopinion_Click" />
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnCustCode" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnSelectIndex" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnselectedrow" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnconculsion" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnStencilPlant" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdncondinalstatus" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnSettletype" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnPlatform" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnType" ClientIDMode="Static" Value="" />
    <script src="Scripts/lightbox.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            blinkStencil();
            visible_dt();
            $('#btnReOpinion').css({ 'display': 'none' });
            $('#ddlplatform').change(function () {
                $('#ddlTyretype').html(''); $('#hdnPlatform').val('');
                var strPlatform = $("#ddlplatform option:selected").text();
                if (strPlatform != "CHOOSE") {
                    $.ajax({ type: "POST", url: "BindRecords.aspx?type=getTypeConfigWise&config=" + strPlatform + "", context: document.body, success: function (data) {
                        if (data != '') { $('#ddlType').html(data); $('#hdnPlatform').val(strPlatform); }
                    }
                    });
                }
            });
            $('#ddlType').change(function () {
                $('#hdnType').val('');
                if ($("#ddlType option:selected").text() != "CHOOSE")
                    $('#hdnType').val($("#ddlType option:selected").text());
            });
            if ($('input:radio[id*=rdbSettlementOpinion_]:checked').length == 0) {
                $('input:text[id*=MainContent_gvOpinionItems_txtClaimPrice_]').attr('disabled', true);
                $('input:text[id*=MainContent_gvOpinionItems_txtTyrePrice_]').attr('disabled', true);
            }
            else {
                settlement();
            }
            $('input:radio[id*=rdbSettlementOpinion_]').click(function () {
                settlement();
            });
            $('#MainContent_dgAnalysisList tr').find($('#lblStencilConclusion')).each(function (e) {
                if ($(this).text() != 'Manufacturing Defect') {
                    $('#MainContent_gvClassification_chkClassification_0').attr('disabled', true);
                    $('#MainContent_gvClassification_txtClassification_0').attr('disabled', true);
                }
                else {
                    $('#MainContent_gvClassification_chkClassification_0').attr('disabled', false);
                    $('#MainContent_gvClassification_txtClassification_0').attr('disabled', false);
                }
            });
            $('#txtEdcReOpinion').keyup(function () {
                if ($('#txtEdcReOpinion').val().length > 0) {
                    $('#btnReOpinion').css({ 'display': 'block' });
                    $('#btnCrmStautsChange').css({ 'display': 'none' });
                }
                else {
                    $('#btnCrmStautsChange').css({ 'display': 'block' });
                    $('#btnReOpinion').css({ 'display': 'none' });
                }
            });
        });
        function settlement() {
            $('input:text[id*=MainContent_gvOpinionItems_txtClaimPrice_]').attr('disabled', false);
            $('input:text[id*=MainContent_gvOpinionItems_txtTyrePrice_]').attr('disabled', false);
            $('#MainContent_gvOpinionItems tr').find('td:eq(6)').each(function (e) {
                $('#MainContent_gvOpinionItems_txtClaimPrice_' + e).val("0");
                $('#MainContent_gvOpinionItems_txtClaimPrice_' + e).show();
                $('#MainContent_gvOpinionItems_txtTyrePrice_' + e).val("0");
                $('#MainContent_gvOpinionItems_txtTyrePrice_' + e).show();
                if ($(this).html() == 'REJECT') {
                    $('#MainContent_gvOpinionItems_txtClaimPrice_' + e).val("0");
                    $('#MainContent_gvOpinionItems_txtClaimPrice_' + e).hide();
                    $('#MainContent_gvOpinionItems_txtTyrePrice_' + e).val("0");
                    $('#MainContent_gvOpinionItems_txtTyrePrice_' + e).hide();
                }
            });
        }
        function visible_dt() {
            var count = 1;
            $('#MainContent_gvClaimImages').find('tr').each(function () {
                if (count != 1)
                    $(this).hide();
                count++;
            });
        }
        function showCrmOpinion(CtrlID) { $('#' + CtrlID).fadeIn(1000); gotoClaimDiv(CtrlID); }

        function hideCrmOpinion() { $('#divCrmdecision').fadeOut(); $('#divSettleOpinion').fadeOut(); gotoClaimDiv('lblClaimCustName'); }

        function showMoveToAccountBtn() {
            var conclMsg = '';
            $('#MainContent_gvClaimItems tr').find('td:eq(6)').each(function (e) {
                if ($(this).html() == '&nbsp;') {
                    $(this).css({ 'background-color': '#f00', 'color': '#fff' });
                    conclMsg += 'Yes';
                }
            });
            if (conclMsg.length == 0) { showCrmOpinion('divSettleOpinion'); }
        }

        function btnEdcReanalysis_Click() {
            if ($("#txtEDCReanalysis").val().length == 0) {
                var msg = "<span style='color:red'> <br/> Enter value for the Edc opinion Re-analysis Comment <br/></span>"
                $("#txtEDCReanalysis").after(msg);
                return false;
            }
        }

        function CtrlSettlementOpinion() {
            $('#lblErrMsg').html(''); var errMsg = '';
            if ($('#hdnPlatform').val().length == 0)
                errMsg += 'Choose platform<br/>';
            if ($("#ddlType option:selected").text() == "CHOOSE")
                errMsg += 'Choose tyretype<br/>';
            if ($('input:radio[id*=rdbClaimNoStatus_]:checked').length == 0) errMsg += 'Choose Accept/Reject status<br/>';
            if ($("input:checkbox[id*=MainContent_gvClassification_chkClassification_]:checked").length == 0) errMsg += 'Choose claim classification<br/>';
            if (errMsg.length > 0) { $('#lblErrMsg').html(errMsg); gotoClaimDiv('lblErrMsg'); return false; }
            else return true;
        }

        function CtrlMoveToAccount() {
            $('#lblErrMsg').html(''); var errMsg = '';
            if ($('input:radio[id*=rdbSettlementOpinion_]:checked').length == 0)
                errMsg += 'Choose settlement opinion<br />';
            if ($('#txtMoveToAccount').val().length == 0)
                errMsg += 'Enter your Comments to accounts<br/>';
            if (errMsg.length > 0) {
                $('#lblErrMsg').html(errMsg);
                gotoClaimDiv('lblErrMsg');
                return false;
            }
            else
                return true;
        }
        function isRangeValid(evt, source) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            else {
                // check if the claim price is greater than the unit price. allow if less or equal.
                return parseFloat($(source).val() + evt.key) > parseFloat($(source).closest("td").prev().find("input").val()) ? false : true;
            }
        }

        function valCopyToNextTxt(evt, source) {
            $(source).closest("td").next().find("input").val($(source).val());
        }
    </script>
</asp:Content>
