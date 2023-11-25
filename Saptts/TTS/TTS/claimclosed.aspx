<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="claimclosed.aspx.cs" Inherits="TTS.claimclosed" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .tableCust
        {
            border-collapse: collapse;
            border-color: #868282;
            width: 1070px;
            line-height: 20px;
        }
        .tableCust input[type="text"], textarea, select
        {
            border: 1px solid #000;
            margin-left: 10px;
        }
        .tableCust th
        {
            background-color: #FFEEEC;
            text-align: left;
            padding-left: 10px;
            width: 105px;
            font-weight: bold;
        }
        .divShow
        {
            width: 1060px;
            float: left;
            display: block;
        }
        .divHide
        {
            width: 1060px;
            float: left;
            display: none;
        }
        .hide
        {
            cursor: pointer;
            text-align: left;
            width: 15px;
            height: 15px;
            -webkit-transform: rotate(90deg);
        }
        .FadeIn
        {
            -webkit-transform: rotate(360deg);
        }
        .StatusMainDiv
        {
            width: 1060px;
            float: left;
            margin-bottom: 5px;
        }
        .StatusTitleDiv
        {
            float: left;
            text-align: left;
            width: 1035px;
            color: #000;
            font-size: 15px;
            font-weight: bold;
            line-height: 22px;
        }
        .StatusToggle
        {
            float: left;
            width: 20px;
            padding: 2px;
        }
        .headStyle
        {
            font-weight: bold;
            color: #27b9dd;
            float: left;
            width: 200px;
        }
        .valueStyle
        {
            width: 860px;
            float: left;
        }
        .commentDiv
        {
            width: 1060px;
            float: left;
            display: none;
        }
    </style>
    <link href="Styles/lightbox.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/lightbox.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <script src="Scripts/lightbox.js" type="text/javascript"></script>
    <div align="center" class="pageTitleHead">
        <asp:Label runat="server" ID="lblClosedHead" ClientIDMode="Static" Text=""></asp:Label>
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table style="width: 1070px;">
            <tr>
                <td align="center">
                    <table cellspacing="0" rules="all" border="1" class="tableCust">
                        <tr>
                            <th>
                                PLANT :
                            </th>
                            <td>
                                <asp:DropDownList ID="ddlplant" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="ddlplant_SelectedIndexChanged"
                                    AutoPostBack="true" Width="100px">
                                </asp:DropDownList>
                            </td>
                            <th>
                                YEAR :
                            </th>
                            <td>
                                <asp:DropDownList ID="ddlYear" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged"
                                    AutoPostBack="true" Width="100px">
                                </asp:DropDownList>
                            </td>
                            <th>
                                MONTH :
                            </th>
                            <td>
                                <asp:DropDownList ID="ddlMonth" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged"
                                    AutoPostBack="true" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <th>
                                RECORD COUNT:
                            </th>
                            <td style="text-align: center;">
                                <asp:Label runat="server" ID="lblRecordCount" ClientIDMode="Static" Text="0" Font-Bold="true"
                                    Width="100px" Font-Size="15px" BackColor="#5D0B03" ForeColor="#FFFFFF"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:GridView runat="server" ID="gvClaimClosedList" AutoGenerateColumns="false" Width="1068px"
                        RowStyle-Height="22px" OnDataBound="gvClaimTrackList_OnDataBound">
                        <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" Height="22px" />
                        <Columns>
                            <asp:BoundField DataField="custname" HeaderText="CUSTOMER NAME" ItemStyle-Width="200px" />
                            <asp:BoundField DataField="complaintno" HeaderText="COMPLAINT NO." ItemStyle-Width="80px" />
                            <asp:BoundField DataField="complaintdate" HeaderText="COMPLAINT DATE" ItemStyle-Width="80px" />
                            <asp:BoundField DataField="plant" HeaderText="PLANT" ItemStyle-Width="80px" />
                            <asp:BoundField DataField="Qty" HeaderText="QTY" ItemStyle-Width="40px" />
                            <asp:BoundField DataField="CreditNoteNo" HeaderText="CREDIT NOTE" ItemStyle-Width="80px" />
                            <asp:BoundField DataField="CreditNoteDate" HeaderText="CREDIT NOTE DATE" ItemStyle-Width="80px" />
                            <asp:BoundField DataField="InvoiceNo" HeaderText="INVOICE" ItemStyle-Width="80px" />
                            <asp:BoundField DataField="closeddate" HeaderText="CLOSED DATE" ItemStyle-Width="80px" />
                            <asp:BoundField DataField="totalamount" HeaderText="TOTAL AMOUNT" ItemStyle-Width="50px" />
                            <asp:TemplateField HeaderText="ACTION" ItemStyle-Width="50px">
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
                    <div style="width: 1065px; float: left; border: 1px solid #000; display: none;" id="divshow">
                        <div style="width: 1065px; float: left;">
                            <div style="width: 1064px; float: left; border: 1px solid #000; background-color: #056442;
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
                        <div style="border: 1px solid #000; margin-bottom: 5px; text-align: left;" id="divgvclaimapproved">
                            <asp:GridView runat="server" ID="gvClaimApproveItems" AutoGenerateColumns="false"
                                Width="1064px" RowStyle-Height="22px">
                                <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" Height="22px" HorizontalAlign="Center" />
                                <Columns>
                                    <asp:BoundField DataField="config" HeaderText="PLATFORM" ItemStyle-Width="95px" />
                                    <asp:TemplateField HeaderText="TYPE MENTIONED BY" ItemStyle-Width="90px">
                                        <ItemTemplate>
                                            <%# Eval("tyretype").ToString() == Eval("EdcType").ToString() ? (Eval("tyretype").ToString() == Eval("CustgvnType").ToString() ? Eval("tyretype").ToString() : (Eval("CustgvnType").ToString() != "" ? "<span class='headCss'>CUSTOMER : </span>" + Eval("CustgvnType").ToString() + "<br/>" : "") + "<span class='headCss'>QC : </span>" + Eval("tyretype").ToString() + "<br/>") : ((Eval("CustgvnType").ToString() != "" ? "<span class='headCss'>CUSTOMER : </span>" + Eval("CustgvnType").ToString() + "<br/>" : "") + "<span class='headCss'>QC : </span>" + Eval("tyretype").ToString() + "<br/>")%>
                                            <%#Eval("EdcType").ToString() != "" ? "<span class='headCss'>EDC : </span>" + Eval("EdcType").ToString() + "<br/>" : ""%>
                                            <%# (Eval("CrmType").ToString() == Eval("EdcType").ToString() && Eval("tyretype").ToString() == Eval("EdcType").ToString()) ? "" : (Eval("CrmType").ToString() != "" ? "<span class='headCss'>CRM : </span>" + Eval("CrmType").ToString() + "<br/>" : "")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="brand" HeaderText="BRAND" ItemStyle-Width="95px" />
                                    <asp:BoundField DataField="tyresize" HeaderText="TYRE SIZE" ItemStyle-Width="110px" />
                                    <asp:BoundField DataField="stencilno" HeaderText="STENCIL" ItemStyle-Width="70px" />
                                    <asp:TemplateField HeaderText="CONCLUSION" ItemStyle-Width="50px">
                                        <ItemTemplate>
                                            <%# Eval("ConclusionStatus")%><%# Eval("ConclusionStatus").ToString() == "Others" ? " - " + Eval("otherconclusion").ToString() : ""%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="CrmStatus" HeaderText="COMPLAINT" ItemStyle-Width="50px" />
                                    <asp:BoundField DataField="ClaimDescription" HeaderText="COMPLAINT DESC" ItemStyle-Width="50px" />
                                    <asp:TemplateField HeaderText="IMAGES" ItemStyle-Width="35px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%# Build_ClaimImages1(Eval("stencilno").ToString())%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
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
                        <asp:GridView runat="server" ID="gvClaimDetails" AutoGenerateColumns="false" Width="1064px"
                            RowStyle-Height="22px">
                            <HeaderStyle CssClass="headerNone" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <div class='<%# Convert.ToInt32(Eval("statusid")) < 28 && Eval("CondCrmAppDate").ToString()==""? "divHide":"divShow" %>'>
                                            <div class="StatusMainDiv">
                                                <div class="StatusToggle" style="background-color: #DCDADA;">
                                                    <asp:Image ID="CNA" runat="server" src="images/collapse.gif" ClientIDMode="Static"
                                                        CssClass="hide FadeIn" />
                                                </div>
                                                <div class="StatusTitleDiv" style="background-color: #DCDADA;">
                                                    <%#  (Convert.ToInt32(Eval("statusid")) > 25 && Convert.ToInt32(Eval("statusid"))!=27) ? "CREDIT NOTE APPROVE" : "CREDIT NOTE CONDITIONALLY APPROVED"%>
                                                </div>
                                                <div id="divCNA" class="commentDiv">
                                                    <div>
                                                        <span class="headStyle">COMMENTS :</span> <span class="valueStyle">
                                                            <%# Eval("CreditApproveComments") == DBNull.Value ? (Eval("CondCrmAppComments") != DBNull.Value ? Eval("CondCrmAppComments").ToString().Replace("~", "<br/>") : "") : Eval("CreditApproveComments").ToString().Replace("~", "<br/>")%>
                                                        </span>
                                                    </div>
                                                    <div>
                                                        <span class="headStyle">BY :</span>
                                                        <%# Eval("CreditApprovedUser").ToString()==""? Eval("CondCrmAppBy").ToString():Eval("CreditApprovedUser").ToString()%>
                                                        <%# Eval("CreditApprovedDate").ToString()==""? Eval("CondCrmAppDate").ToString():Eval("CreditApprovedDate").ToString()%>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class='<%#Convert.ToInt32(Eval("statusid")) <26  && Eval("CondAccDate").ToString()=="" ? "divHide":"divShow" %>'>
                                            <div class="StatusMainDiv">
                                                <div class="StatusToggle" style="background-color: #DCDADA;">
                                                    <asp:Image ID="CNPRE" runat="server" src="images/collapse.gif" ClientIDMode="Static"
                                                        CssClass="hide FadeIn" />
                                                </div>
                                                <div class="StatusTitleDiv" style="background-color: #DCDADA;">
                                                    <%#  Convert.ToInt32(Eval("statusid")) > 20 ?  (Convert.ToInt32(Eval("statusid"))!=27?"CREDIT NOTE PREPARED":"CREDIT NOTE REPREPARE" ): "CREDIT NOTE CONDITIONALLY PREPARED"%>
                                                </div>
                                                <div id="divCNPRE" class="commentDiv">
                                                    <div>
                                                        <div style="width: 1063px; float: left;">
                                                            <span class="headStyle">CREDIT NOTE NO :</span>
                                                            <%#CreaditNote(Eval("CreditNoteNo").ToString(), Eval("CreditNoteYear").ToString())%></div>
                                                        <div style="width: 1063px; float: left;">
                                                            <div style="float: left;">
                                                                <span class="headStyle">FILE :</span>
                                                                <asp:LinkButton ID="lnkcreditno" runat="server" Text='<%# Eval("CreditNoteFile")%>'
                                                                    OnClick="lnkcreditno_Click"></asp:LinkButton>
                                                            </div>
                                                            <div style="padding-left: 40px; float: left;">
                                                                <asp:LinkButton ID="lnkcreditExcel" runat="server" Text='<%# Excel_Export(Eval("CreditNoteFile").ToString().Replace(".pdf", ".xls"))%>'
                                                                    OnClick="lnkcreditExcel_Click"></asp:LinkButton></div>
                                                        </div>
                                                    </div>
                                                    <div style="width: 1063px; float: left;">
                                                        <span class="headStyle">COMMENTS :</span> <span class="valueStyle">
                                                            <%#Eval("CreditNoteComments") == DBNull.Value ? (Eval("CondAccComments") != DBNull.Value ? ((string)Eval("CondAccComments")).Replace("~", "<br/>") : "") : ((string)Eval("CreditNoteComments")).Replace("~", "<br/>")%>
                                                        </span>
                                                    </div>
                                                    <div style="width: 1063px; float: left;">
                                                        <span class="headStyle">BY :</span>
                                                        <%# Eval("CreditMoveUser").ToString()==""? Eval("CondAccBy").ToString():Eval("CreditMoveUser").ToString()%>
                                                        <%# Eval("CreditMoveDate").ToString()==""? Eval("CondAccDate").ToString():Eval("CreditMoveDate").ToString() %>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class='<%#Convert.ToInt32(Eval("statusid")) <25 && Eval("CondCrmDate").ToString()==""? "divHide":"divShow" %>'>
                                            <div class="StatusMainDiv">
                                                <div class="StatusToggle" style="background-color: #DCDADA;">
                                                    <asp:Image ID="CCRMOPINION" runat="server" src="images/collapse.gif" ClientIDMode="Static"
                                                        CssClass="hide FadeIn" /></div>
                                                <div class="StatusTitleDiv" style="background-color: #DCDADA;">
                                                    <%#  Convert.ToInt32(Eval("statusid")) > 12 ? "CRM" : "CRM CONDITIONALLY APPORVED"%>
                                                </div>
                                                <div id="divCRMOPINION" class="commentDiv">
                                                    <div>
                                                        <span class="headStyle">SETTLEMENT TYPE :</span>
                                                        <%# Eval("CrmSettleType")%>
                                                    </div>
                                                    <div>
                                                        <span class="headStyle">COMMENTS :</span> <span class="valueStyle">
                                                            <%# Eval("crmcomments") == DBNull.Value ? (Eval("CondCrmComments") != DBNull.Value ? ((string)Eval("CondCrmComments")).Replace("~", "<br/>") : "") : ((string)Eval("crmcomments")).Replace("~", "<br/>")%>
                                                        </span>
                                                    </div>
                                                    <div>
                                                        <span class="headStyle">BY :</span>
                                                        <%# Eval("crmuser").ToString()==""? Eval("CondCrmBy").ToString():Eval("crmuser").ToString()%>
                                                        <%# Eval("crmstatusdate").ToString()==""? Eval("CondCrmDate").ToString():Eval("crmstatusdate").ToString()%>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class='<%#(Convert.ToInt32(Eval("statusid")) < 15 && Eval("CondEdcDate").ToString()=="") ? "divHide":"divShow" %>'>
                                            <div class="StatusMainDiv">
                                                <div class="StatusToggle" style="background-color: #DCDADA;">
                                                    <asp:Image ID="CEDC" runat="server" src="images/collapse.gif" ClientIDMode="Static"
                                                        CssClass="hide FadeIn" />
                                                </div>
                                                <div class="StatusTitleDiv" style="background-color: #DCDADA;">
                                                    <%#  Convert.ToInt32(Eval("statusid")) >10?"EDC":"EDC CONDITIONALLY APPORVED"%>
                                                </div>
                                                <div id="divCEDC" class="commentDiv">
                                                    <div>
                                                        <span class="headStyle">COMMENTS :</span> <span class="valueStyle">
                                                            <%# Eval("EdcMovedcomments") == DBNull.Value ? (Eval("CondEdcComments") != DBNull.Value ? ((string)Eval("CondEdcComments")).Replace("~", "<br/>") : "") : ((string)Eval("EdcMovedcomments")).Replace("~", "<br/>")%>
                                                        </span>
                                                    </div>
                                                    <div>
                                                        <span class="headStyle">BY :</span>
                                                        <%# Eval("EdcUser").ToString()==""? Eval("CondEdcBy").ToString():Eval("EdcUser").ToString()%>
                                                        <%# Eval("EdcMovedDate").ToString() == "" ? Eval("CondEdcDate").ToString() : Eval("EdcMovedDate").ToString()%>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class='<%#Convert.ToInt32(Eval("statusid"))< 12? "divHide":"divShow" %>'>
                                            <div class="StatusMainDiv">
                                                <div class="StatusToggle" style="background-color: #DCDADA;">
                                                    <asp:Image ID="CQC" runat="server" src="images/collapse.gif" ClientIDMode="Static"
                                                        CssClass="hide FadeIn" />
                                                </div>
                                                <div class="StatusTitleDiv" style="background-color: #DCDADA;">
                                                    QC
                                                </div>
                                                <div id="divCQC" class="commentDiv">
                                                    <div>
                                                        <span class="headStyle">COMMENTS :</span> <span class="valueStyle">
                                                            <%#Eval("QcAnalysiscomments") != DBNull.Value ? ((string)Eval("QcAnalysiscomments")).Replace("~", "<br/>") : ""%>
                                                        </span>
                                                    </div>
                                                    <div>
                                                        <span class="headStyle">BY :</span>
                                                        <%# Eval("QcAnalysisuser")%>
                                                        <%# Eval("QcAnalysisDate")%>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class='<%#Eval("Commets").ToString()=="" ? "divHide":"divShow" %>'>
                                            <div class="StatusMainDiv">
                                                <div class="StatusToggle" style="background-color: #DCDADA;">
                                                    <asp:Image ID="CUser" runat="server" src="images/collapse.gif" ClientIDMode="Static"
                                                        CssClass="hide FadeIn" />
                                                </div>
                                                <div class="StatusTitleDiv" style="background-color: #DCDADA;">
                                                    CUSTOMER
                                                </div>
                                                <div id="divCUSER" class="commentDiv">
                                                    <div class="StatusMainDiv">
                                                        <span class="headStyle">COMMENTS :</span> <span class="valueStyle">
                                                            <%#Eval("Commets") != DBNull.Value ? ((string)Eval("Commets")).Replace("~", "<br/>") : ""%>
                                                        </span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnCustCode" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnacc" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        function gotoClaimDiv(ctrlID) {
            $("html, body").stop().animate({
                scrollLeft: $('#' + ctrlID).offset().left,
                scrollTop: $('#' + ctrlID).offset().top
            }, 1200);
        }
        $(document).ready(function () {
            $(".divShow:first").find('.commentDiv').fadeToggle('slow');
            $(".divShow:first").find('.hide').removeClass('FadeIn');
            $('#CUser').click(function () {
                $('#divCUSER').fadeToggle('slow');
                $(this).toggleClass('FadeIn');
                gotoClaimDiv('CUser')
            });
            $('#CQC').click(function () {
                $('#divCQC').fadeToggle('slow');
                $(this).toggleClass('FadeIn');
                gotoClaimDiv('CQC')
            });
            $('#CEDC').click(function () {
                $('#divCEDC').fadeToggle('slow');
                $(this).toggleClass('FadeIn');
                gotoClaimDiv('CEDC')
            });
            $('#CCRMOPINION').click(function () {
                $('#divCRMOPINION').fadeToggle('slow');
                $(this).toggleClass('FadeIn');
                gotoClaimDiv('CCRMOPINION')
            });
            $('#CNPRE').click(function () {
                $('#divCNPRE').fadeToggle('slow');
                $(this).toggleClass('FadeIn');
                gotoClaimDiv('CNPRE')
            });
            $('#CNA').click(function () {
                $('#divCNA').fadeToggle('slow');
                $(this).toggleClass('FadeIn');
                gotoClaimDiv('CNA')
            });
        });
        function showdiv(ctrl) {
            $('#' + ctrl).css("display", "block");
            gotoClaimDiv(ctrl);
        }
    </script>
</asp:Content>
