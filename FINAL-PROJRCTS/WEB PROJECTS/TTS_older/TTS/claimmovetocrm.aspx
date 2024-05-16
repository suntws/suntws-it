<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="claimmovetocrm.aspx.cs" Inherits="TTS.claimmovetocrm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .analysisdata td:first-child
        {
            background-color: #f3e0ad;
            width: 200px;
        }
        .tbEdcCss
        {
            border-collapse: collapse;
            border-color: #000;
            width: 100%;
            float: left;
        }
        .tbEdcCss th
        {
            background-color: #ccc;
            font-weight: bold;
            text-align: left;
            line-height: 20px;
        }
    </style>
    <link href="Styles/lightbox.css" rel="stylesheet" type="text/css" />
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
        <table style="width: 100%;">
            <tr style="text-align: center; background-color: #cec0ea; font-weight: bold;">
                <td>
                    PLANT :
                    <asp:DropDownList runat="server" ID="ddlEdcPlant" ClientIDMode="Static" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlEdcPlant_IndexChanged">
                    </asp:DropDownList>
                    <asp:Label runat="server" ID="lblErr" ClientIDMode="Static" Text="" Font-Bold="true"
                        ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:GridView runat="server" ID="gvClaimNoList" AutoGenerateColumns="false" Width="100%"
                        RowStyle-Height="22px">
                        <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" Height="22px" />
                        <Columns>
                            <asp:BoundField DataField="custname" HeaderText="CUSTOMER NAME" ItemStyle-Width="300px" />
                            <asp:BoundField DataField="complaintno" HeaderText="COMPLAINT NO." ItemStyle-Width="80px" />
                            <asp:BoundField DataField="complaintdate" HeaderText="COMPLAINT DATE" ItemStyle-Width="80px" />
                            <asp:BoundField DataField="Qty" HeaderText="QTY" ItemStyle-Width="40px" />
                            <asp:BoundField DataField="claimstatus" HeaderText="STATUS" ItemStyle-Width="150px" />
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
                <td align="center">
                    <div style="width: 100%; float: left;" id="divclaimitems">
                        <div style="width: 100%; float: left; border: 1px solid #000; background-color: #056442;
                            font-weight: bold; color: #fff; font-size: 15px;">
                            <div style="width: 48%; float: left; text-align: left; padding-right: 5px;">
                                <asp:Label runat="server" ID="lblClaimCustName" ClientIDMode="Static" Text=""></asp:Label>
                            </div>
                            <div style="width: 3%; float: left;">
                                <asp:Label runat="server" ID="lblClaim" ClientIDMode="Static" Text=""></asp:Label>
                            </div>
                            <div style="width: 48%; float: right; text-align: right; padding-left: 5px;">
                                <asp:Label runat="server" ID="lblClaimNo" ClientIDMode="Static" Text=""></asp:Label>
                            </div>
                        </div>
                        <div style="width: 100%; float: left;">
                            <asp:GridView runat="server" ID="gvClaimItems" AutoGenerateColumns="false" Width="1068px"
                                RowStyle-Height="20px">
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
                                    <asp:TemplateField HeaderText="QC STATUS" ItemStyle-Width="60px">
                                        <ItemTemplate>
                                            <%#Eval("Crm_EdcReOpnionStatus").ToString() != "" ? Eval("Crm_EdcReOpnionStatus").ToString() : Eval("AnalysisStatus").ToString()%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ConclusionStatus" HeaderText="EDC CONCLUSION" ItemStyle-Width="100px" />
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div style="width: 100%; float: left; text-align: left; margin-top: 5px;">
                            <asp:Label runat="server" ID="lblComplaintComments" ClientIDMode="Static" Text=""></asp:Label>
                            <asp:Label runat="server" ID="lblQcComments" ClientIDMode="Static" Text=""></asp:Label>
                            <asp:Label runat="server" ID="lblQcMovedUser" ClientIDMode="Static" Text=""></asp:Label>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView runat="server" ID="dgList" PageSize="1" AllowPaging="true" AutoGenerateColumns="false"
                        OnPageIndexChanging="dgList_PageIndex" Width="100%" PagerStyle-HorizontalAlign="Center"
                        PagerStyle-Font-Bold="true" PagerStyle-Font-Size="15px">
                        <HeaderStyle CssClass="headerNone" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #AF99F5;
                                        width: 100%;">
                                        <tr>
                                            <td style="width: 50%; vertical-align: top;">
                                                <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #EA99F5;
                                                    width: 100%;" class="analysisdata">
                                                    <tr>
                                                        <td>
                                                            STENCIL NO.
                                                        </td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblAnalysedSetncil" Text='<%# Eval("stencilno")%>'
                                                                Font-Bold="true" Font-Size="12px"></asp:Label>
                                                            <asp:HiddenField ID="hdnimageUrl" runat="server" Value='<%# Eval("complaintmoveimg")%>' />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            PLATFORM
                                                        </td>
                                                        <td>
                                                            <%# Eval("config")%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            TYPE
                                                        </td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lbltyretype" Text='<%# Eval("tyretype")%>' Font-Bold="true"
                                                                Font-Size="12px"></asp:Label>
                                                            <asp:HiddenField ID="hdnEdctype" runat="server" ClientIDMode="Static" Value='<%# Eval("EdcType")%>' />
                                                            <asp:HiddenField ID="hdnCustType" runat="server" ClientIDMode="Static" Value='<%# Eval("CustgvnType")%>' />
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
                                                            BRAND
                                                        </td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblbrand" Text='<%# Eval("brand")%>' Font-Bold="true"
                                                                Font-Size="12px"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            SIZE
                                                        </td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lbltyresize" Text='<%# Eval("tyresize")%>' Font-Bold="true"
                                                                Font-Size="12px"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #EA99F5;
                                                    width: 100%;" class="analysisdata">
                                                    <tr>
                                                        <td>
                                                            PRODUCTION :
                                                        </td>
                                                        <td>
                                                            DATE:
                                                            <%# Eval("prodate")%>
                                                            &nbsp; | &nbsp; SHIFT:
                                                            <%# Eval("shift") %>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            PRESS
                                                        </td>
                                                        <td>
                                                            <%# Eval("pressdetails")%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            MOULD
                                                        </td>
                                                        <td>
                                                            <%# Eval("moulddetails")%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            TREAD
                                                        </td>
                                                        <td>
                                                            <%# Eval("treadcomp")%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            CENTER
                                                        </td>
                                                        <td>
                                                            <%# Eval("centercomp")%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            BASE
                                                        </td>
                                                        <td>
                                                            <%# Eval("basecomp")%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" style="text-align: center;">
                                                            USER
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            PREPARED BY
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <%# Eval("username")%></b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            VERIFIED BY
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <%# Eval("updateuser")%></b>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td style="width: 50%; vertical-align: top;">
                                                <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #EA99F5;
                                                    width: 100%;" class="analysisdata">
                                                    <tr style="text-align: center;">
                                                        <td style="width: 200px;">
                                                            PROCESS ANALYSIS
                                                        </td>
                                                        <td style="background-color: #f3e0ad;">
                                                            REQUIRED
                                                        </td>
                                                        <td style="background-color: #f3e0ad;">
                                                            ACTUAL
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            TYRE WEIGHT
                                                        </td>
                                                        <td>
                                                            <%# Eval("wtReq")%>
                                                        </td>
                                                        <td>
                                                            <%# Eval("wtAct")%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            STEAM PRESSURE
                                                        </td>
                                                        <td>
                                                            <%# Eval("steamReq")%>
                                                        </td>
                                                        <td>
                                                            <%# Eval("steamAct")%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            HYDRAULIC PRESSURE
                                                        </td>
                                                        <td>
                                                            <%# Eval("hydReq")%>
                                                        </td>
                                                        <td>
                                                            <%# Eval("hydAct")%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            MOULD TEMP (BEFORE LOADING)
                                                        </td>
                                                        <td>
                                                            <%# Eval("tempReq")%>
                                                        </td>
                                                        <td>
                                                            <%# Eval("tempAct")%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            MOULD TEMP (CHECK1)
                                                        </td>
                                                        <td>
                                                            <%# Eval("chk1Req")%>
                                                        </td>
                                                        <td>
                                                            <%# Eval("chk1Act")%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            MOULD TEMP (CHECK2)
                                                        </td>
                                                        <td>
                                                            <%# Eval("chk2Req")%>
                                                        </td>
                                                        <td>
                                                            <%# Eval("chk2Act")%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            HARDNESS
                                                        </td>
                                                        <td>
                                                            <%# Eval("hardReq")%>
                                                        </td>
                                                        <td>
                                                            <%# Eval("hardAct")%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            CURING
                                                        </td>
                                                        <td>
                                                            <%# Eval("cureReq")%>
                                                        </td>
                                                        <td>
                                                            <%# Eval("cureAct")%>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #BE99F5;
                                                    width: 100%;" class="analysisdata">
                                                    <tr style="text-align: center;">
                                                        <td style="width: 200px;">
                                                            PROCESS ANALYSIS
                                                        </td>
                                                        <td style="background-color: #f3e0ad;">
                                                            START TIME
                                                        </td>
                                                        <td style="background-color: #f3e0ad;">
                                                            END TIME
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            BUILDING TIME
                                                        </td>
                                                        <td>
                                                            <%# Eval("buildStart")%>
                                                        </td>
                                                        <td>
                                                            <%# Eval("buildEnd")%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            UL / L TIME
                                                        </td>
                                                        <td>
                                                            <%# Eval("ulStart")%>
                                                        </td>
                                                        <td>
                                                            <%# Eval("ulEnd")%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            CURING TIME
                                                        </td>
                                                        <td>
                                                            <%# Eval("cureStart")%>
                                                        </td>
                                                        <td>
                                                            <%# Eval("cureEnd")%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <span class="headCss">QC ROOT CAUSE / COMMENTS:</span><br />
                                                            <asp:Label runat="server" ID="lblAnalyserRootCause" Text='<%#Eval("QcComments").ToString()!=""? (((string)Eval("QcComments")).Replace("~", "<br/>")):""%>'></asp:Label>
                                                            <asp:HiddenField runat="server" ID="hdnReanalysisComment" ClientIDMode="Static" Value='<%# Eval("ReanalysisComment") %>' />
                                                            <asp:HiddenField runat="server" ID="hdnAnalysisStatus" ClientIDMode="Static" Value='<%# Eval("AnalysisStatus") %>' />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <asp:HiddenField runat="server" ID="hdnEdcReopinionComment" ClientIDMode="Static"
                                                                Value='<%# Eval("Crm_EdcReOpinion") %>' />
                                                            <asp:HiddenField runat="server" ID="hdnEdcReopinionUpdateComments" ClientIDMode="Static"
                                                                Value='<%# Eval("CrmEdcReOpinionUpdateComments") %>' />
                                                            <!--
                                                            <b>CRM :</b><asp:Label runat="server" ID="lblCrm_edcReopinionComment" Text='<%#Eval("Crm_EdcReOpinion").ToString()!="" ? "" + (((string)Eval("Crm_EdcReOpinion")).Replace("~", "<br/>")):""%>'></asp:Label>
                                                            <br />
                                                            <b>EDC :</b><asp:Label runat="server" ID="lbledcReopinion" ClientIDMode="Static"
                                                                Text='<%#Eval("CrmEdcReOpinionUpdateComments").ToString()!="" ?  "" + (((string)Eval("CrmEdcReOpinionUpdateComments")).Replace("~", "<br/>")):""%>'></asp:Label>
                                                            -->
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3" style="background-color: #fff;">
                                                            <%#Eval("QcAdditionalReq").ToString() != "" ? "<span style='font-weight:bold;'>QC:</span> " + (((string)Eval("QcAdditionalReq")).Replace("~", "<br/>")) : ""%>
                                                            <%#Eval("QcAdditionalUpdateComments").ToString() != "" ? "<br/><span style='font-weight:bold;'>CRM:</span> " + (((string)Eval("QcAdditionalUpdateComments")).Replace("~", "<br/>")) : ""%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3" style="background-color: #fff;">
                                                            <%# Eval("EdcAdditionalReq").ToString() != "" ? "<span style='font-weight:bold;'>EDC:</span> " : ""%>
                                                            <asp:Label runat="server" ID="lblEdcAdditionalReq" ClientIDMode="Static" Text='<%# Eval("EdcAdditionalReq") %>'></asp:Label>
                                                            <%# Eval("EdcAdditionalUpdateComments").ToString() != "" ? "<br /><span style='font-weight:bold;'>CRM:</span> " : ""%>
                                                            <asp:Label runat="server" ID="lblEdcAdditionalUpdates" ClientIDMode="Static" Text='<%# Eval("EdcAdditionalUpdateComments") %>'></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr style="background-color: #E6FCBF;">
                <td>
                    <table cellspacing="0" rules="all" border="1" class="tbEdcCss" id="divEDCFinalUpdate"
                        style="display: none;">
                        <tr>
                            <th>
                                STENCIL NO.
                            </th>
                            <td>
                                <div style="width: 200px; float: left; text-align: center;" id="divBlinkStencil">
                                    <asp:Label runat="server" ID="lblEdcSetncil" ClientIDMode="Static" Text="" Font-Bold="true"
                                        Font-Size="20px"></asp:Label>
                                </div>
                            </td>
                            <td rowspan="2">
                                <div style="width: 615px; float: left;">
                                    <span>VERIFIER ROOT CAUSE</span>
                                    <asp:TextBox runat="server" ID="txtEdcRootCause" ClientIDMode="Static" Text="" TextMode="MultiLine"
                                        Width="610px" Height="80px" onKeyUp="javascript:CheckMaxLength(this, 3999);"
                                        onChange="javascript:CheckMaxLength(this, 3999);"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                TYRE TYPE MENTIONED BY
                            </th>
                            <td>
                                <div style="width: 260px; float: left; margin-top: 5px; line-height: 20px; border: 1px solid #000;
                                    background-color: #fcd7d7;">
                                    <asp:Label runat="server" ID="lblCustType" Text=""></asp:Label>
                                    <br />
                                    <asp:Label runat="server" ID="lblQcType" Text=""></asp:Label>
                                    <br />
                                    <span class="headCss">EDC:</span>
                                    <asp:DropDownList ID="ddltype" ClientIDMode="Static" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                COMPLAINT DESCRIPTION
                            </th>
                            <td>
                                <asp:DropDownList ID="ddlComplaintDesc" runat="server" ClientIDMode="Static" Width="250px">
                                </asp:DropDownList>
                                <asp:TextBox ID="txtComplaintDesc" runat="server" ClientIDMode="Static" Text="" Width="250px">
                                </asp:TextBox>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblStencilShowImages" ClientIDMode="Static" Text=""
                                    Font-Bold="true" Font-Size="12px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Label runat="server" ID="lblclaimimages" ClientIDMode="Static" Text="" ForeColor="#D34639"
                                    Font-Bold="true"></asp:Label>
                                <asp:DataList runat="server" ID="gvClaimImages" RepeatColumns="6" RepeatDirection="Horizontal"
                                    RepeatLayout="Table" AlternatingItemStyle-BackColor="#ffffcd" ItemStyle-BackColor="#fcd7d7"
                                    ItemStyle-VerticalAlign="Top" Width="1070px" ItemStyle-Width="175px">
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chk1" />
                                        <asp:HiddenField runat="server" ID="hdnClaimImage" Value='<%# Eval("ClaimImage") %>' />
                                        <a id="imageLink" name="nullDownload" href='<%# Eval("ClaimImage") %>' rel="lightbox[Brussels]"
                                            runat="server">
                                            <asp:Image runat="server" ID="img1" ImageUrl='<%# Eval("ClaimImage") %>' Height="150px"
                                                Width="150px" rel="lightbox[Brussels]" /></a>
                                    </ItemTemplate>
                                </asp:DataList>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                CONCLUSION
                            </th>
                            <td colspan="2">
                                <div style="width: 450px; float: left;">
                                    <asp:RadioButtonList runat="server" ID="rdbEdcConclusion" ClientIDMode="Static" RepeatColumns="4"
                                        RepeatDirection="Horizontal">
                                        <asp:ListItem Text="Manufacturing Defect" Value="Manufacturing Defect"></asp:ListItem>
                                        <asp:ListItem Text="Customer Abuse" Value="Customer Abuse"></asp:ListItem>
                                        <asp:ListItem Text="Wrong Application" Value="Wrong Application"></asp:ListItem>
                                        <asp:ListItem Text="Others" Value="Others"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                                <div id="divOtherConclusion" style="display: none; width: 450px; float: left;">
                                    <asp:TextBox runat="server" ID="txtOtherEdcConclusion" ClientIDMode="Static" Text=""
                                        Width="440px" MaxLength="100"></asp:TextBox>
                                </div>
                                <div id="divClaimStatus" style="display: none; width: 470px; float: left; background-color: #2e2d2d;
                                    color: #fff;">
                                    <div style="width: 100px; float: left; line-height: 25px;">
                                        CLAIM STATUS</div>
                                    <asp:RadioButtonList runat="server" ID="rdbClaimNoStatus" ClientIDMode="Static" RepeatColumns="2"
                                        RepeatDirection="Horizontal" Width="160px">
                                        <asp:ListItem Text="ACCEPT" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="REJECT" Value="0"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div id="divAdditionalDetails" style="width: 450px; float: left;">
                                    <span class="headCss" style="width: 570px; float: left;">ADDITIONAL DETAILS ASK FROM
                                        CRM</span>
                                    <asp:TextBox runat="server" ID="txtEdcAdditional" ClientIDMode="Static" Text="" TextMode="MultiLine"
                                        Width="442px" Height="45px" onKeyUp="javascript:CheckMaxLength(this, 499);" onChange="javascript:CheckMaxLength(this, 499);"></asp:TextBox>
                                </div>
                            </td>
                            <td>
                                <div style="width: 570px; float: left; border: 1px solid #000; padding: 5px; background-color: #f3e0ad;
                                    margin-left: 10px;" id="divreanalysis" runat="server" clientidmode="Static">
                                    <span class="headCss" style="width: 570px; float: left;">ENTER YOUR REANALYSIS COMMENTS
                                        / INSTRUCTION:</span>
                                    <asp:TextBox runat="server" ID="txtReanalysis" ClientIDMode="Static" Text="" TextMode="MultiLine"
                                        Width="565px" Height="35px" onKeyUp="javascript:CheckMaxLength(this, 499);" onChange="javascript:CheckMaxLength(this, 499);">
                                    </asp:TextBox>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <div id="divEdcReopinion" style="width: 452px; border-right: 1px solid black; float: left;
                                    display: none">
                                    <span id="spnReopinionTitle" class="headCss" style="float: left;">CRM REOPINION COMMENTS
                                        :</span>
                                    <br />
                                    <asp:Label runat="server" ID="lblCrm_EdcReopinion" ClientIDMode="Static" Text=""></asp:Label>
                                    <br />
                                    <span class="headCss" style="float: left;">RE-OPINION COMMENTS TO CRM</span>
                                    <asp:TextBox runat="server" ID="txtEdc_crmReopinion" ClientIDMode="Static" Text=""
                                        TextMode="MultiLine" Width="442px" Height="45px" onKeyUp="javascript:CheckMaxLength(this, 499);"
                                        onChange="javascript:CheckMaxLength(this, 499);"></asp:TextBox>
                                </div>
                                <div style="width: 1064px;">
                                    <div style="width: 525px; float: left;">
                                        <asp:Button runat="server" ID="btnEdcFinalStatement" ClientIDMode="Static" Text="SAVE ROOT CAUSE / CONCLUSION"
                                            CssClass="btnsave" OnClick="btnEdcFinalStatement_Click" OnClientClick="javascript:return CtrlUpdateEdcFinalStatus();" />
                                    </div>
                                    <div style="width: 525px; float: left; padding-top: 5px;">
                                        <asp:LinkButton runat="server" ClientIDMode="Static" ID="lnkCancelRequest" Text=""
                                            Style="text-decoration: none;" OnClick="lnkCancelRequest_Click"></asp:LinkButton>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" Font-Bold="true"
                        Font-Size="12px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="width: 100%; float: left; border: 1px solid #000; background-color: #efefef;">
                        <div style="width: 532px; float: left; font-size: 14px; padding-top: 5px; display: none;"
                            id="tdAnalysisData">
                            <span class="headCss">ANY COMMENTS TO CRM:</span>
                            <asp:TextBox runat="server" ID="txtComments" ClientIDMode="Static" Text="" TextMode="MultiLine"
                                Width="525px" Height="80px" onKeyUp="javascript:CheckMaxLength(this, 3999);"
                                onChange="javascript:CheckMaxLength(this, 3999);"></asp:TextBox>
                            <asp:Button runat="server" ID="btnComplaintMoved" ClientIDMode="Static" CssClass="btnauthorize"
                                Text="" OnClientClick="javascript:return CtrlRootCauseConclusion();" OnClick="btnComplaintMoved_Click" />
                        </div>
                        <div style="width: 532px; float: right; padding-top: 40px; text-align: center; display: none;"
                            id="tdEdcAdditional">
                            <asp:Label runat="server" ID="lblMsgForEdcAddtionalDetails" ClientIDMode="Static"
                                Text="" ForeColor="Red"></asp:Label>
                            <asp:Button runat="server" ID="btnMoveToCrmRequest" ClientIDMode="Static" CssClass="btnedit"
                                Text="MOVE TO CRM FOR ADDTIONAL DETAILS" OnClick="btnMoveToCrmRequest_Click" />
                        </div>
                        <div style="width: 532px; float: right; padding-top: 40px; text-align: center; display: none;"
                            id="tdQcRecheck">
                            <asp:Label runat="server" ID="lblRechkMsg" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label>
                            <asp:Button runat="server" ID="btnmovetoqc" Text="MOVE TO QC FOR REANALYSIS" CssClass="btnedit"
                                OnClick="btnmovetoqc_Click" />
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnCustCode" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnClaimNoClick" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnselectedrow" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnStencilPlant" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnStatus" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnID" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnCount" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnstencilurl" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnSaveType" ClientIDMode="Static" Value="" />
    <script src="Scripts/lightbox.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            blinkStencil();
            $("input:radio[id*=rdbEdcConclusion_]").click(function () {
                ShowConclusionWise();
            });
            $('#txtComplaintDesc').css({ 'display': 'none' });
            $('#ddlComplaintDesc').change(function () {
                if ($('#ddlComplaintDesc option:selected').text() == 'ADD NEW ENTRY')
                    $('#txtComplaintDesc').css({ 'display': 'block' });
                else
                    $('#txtComplaintDesc').css({ 'display': 'none' });
            });
            $("input:checkbox[id*=MainContent_gvClaimImages_chk1_]").click(function () {
                $('#hdnstencilurl').val(''); var buildImagStr = '';
                if ($("input:checkbox[id*=MainContent_gvClaimImages_chk1_]:checked").length > 0) {
                    $("input:checkbox[id*=MainContent_gvClaimImages_chk1_]:checked").each(function () { var id1 = this.id; if (buildImagStr.length == 0) buildImagStr = $('#' + id1.replace('chk1_', 'hdnClaimImage_')).val(); else if (buildImagStr.length > 0) buildImagStr += '~' + $('#' + id1.replace('chk1_', 'hdnClaimImage_')).val(); });
                    $('#hdnstencilurl').val(buildImagStr);
                }
            });
            $('#txtEdcAdditional,#txtReanalysis').blur(function () {
                $('#btnEdcFinalStatement').val('SAVE ROOT CAUSE / CONCLUSION');
                if ($('#txtEdcAdditional').val().length > 0 && $('#txtEdcAdditional').attr("disabled") != "disabled")
                    $('#btnEdcFinalStatement').val('SAVE ADDTIONAL DETAILS REQUEST');
                else if ($('#txtReanalysis').val().length > 0 && $('#txtReanalysis').attr("disabled") != "disabled")
                    $('#btnEdcFinalStatement').val('SAVE REANALYSIS COMMENTS / INSTRUCTION');
            }).focus(function () {
                $('#btnEdcFinalStatement').val('SAVE ROOT CAUSE / CONCLUSION');
                if ($('#txtEdcAdditional').val().length > 0 && $('#txtEdcAdditional').attr("disabled") != "disabled")
                    $('#btnEdcFinalStatement').val('SAVE ADDTIONAL DETAILS REQUEST');
                else if ($('#txtReanalysis').val().length > 0 && $('#txtReanalysis').attr("disabled") != "disabled")
                    $('#btnEdcFinalStatement').val('SAVE REANALYSIS COMMENTS / INSTRUCTION');
            }).keyup(function () {
                $('#btnEdcFinalStatement').val('SAVE ROOT CAUSE / CONCLUSION');
                if ($('#txtEdcAdditional').val().length > 0 && $('#txtEdcAdditional').attr("disabled") != "disabled")
                    $('#btnEdcFinalStatement').val('SAVE ADDTIONAL DETAILS REQUEST');
                else if ($('#txtReanalysis').val().length > 0 && $('#txtReanalysis').attr("disabled") != "disabled")
                    $('#btnEdcFinalStatement').val('SAVE REANALYSIS COMMENTS / INSTRUCTION');
            });

            //            var lblReopinion = $("#lblCrm_EdcReopinion");
            //            if (lblReopinion != null) {
            //                if ($("#lblCrm_EdcReopinion").val() == "") {
            //                    $("#spnReopinionTitle").css("display", "none");
            //                }
            //                else {
            //                    $("#spnReopinionTitle").css("display", "block");
            //                }
            //            }
        });

        function ShowConclusionWise() {
            $('#divOtherConclusion').hide(); $('#divClaimStatus').hide();
            if ($("input:radio[id*=rdbEdcConclusion_]:checked").val() == "Manufacturing Defect")
                $('#divClaimStatus').show();
            else if ($("input:radio[id*=rdbEdcConclusion_]:checked").val() == "Others")
                $('#divOtherConclusion').show();
        }


        function showAnalysisData(ctrlID) {
            $('#' + ctrlID).css({ 'display': 'block' });
            gotoClaimDiv(ctrlID);
        }

        function CtrlRootCauseConclusion() {
            var ErrmsgAnalyse = ''; $('#lblErrMsg').html(''); var rowNo = '';
            $('#MainContent_gvClaimItems tr').find('td:eq(6)').each(function (e) {
                if ($(this).html() == '&nbsp;') {
                    rowNo = parseInt(e.toString()) + 1;
                    if (ErrmsgAnalyse.length == 0) ErrmsgAnalyse = rowNo.toString();
                    else ErrmsgAnalyse += ',' + rowNo.toString();
                }
            });
            rowNo = ''; var ErrmsgConc = '';
            $('#MainContent_gvClaimItems tr').find('td:eq(7)').each(function (e) {
                if ($(this).html() == '&nbsp;' || $(this).html() == '') {
                    rowNo = parseInt(e.toString()) + 1;
                    if (ErrmsgConc.length == 0) ErrmsgConc = rowNo.toString();
                    else ErrmsgConc += ',' + rowNo.toString();
                }
            });
            if (ErrmsgAnalyse.length > 0)
                ErrMsg += 'Claim record ' + ErrmsgAnalyse + ' analysis is pending';
            if (ErrmsgConc.length > 0)
                ErrMsg += 'Claim record ' + ErrmsgConc + ' conclusion is pending';
            var ErrMsg = '';
            if ($('#txtComments').val().length == 0)
                ErrMsg += 'Enter any comments<br/>';
            if (ErrMsg.length > 0) {
                $('#lblErrMsg').html(ErrMsg).css({ 'color': '#f00' });
                gotoClaimDiv('lblErrMsg');
                return false;
            }
            else
                return true;
        }
        function CtrlUpdateEdcFinalStatus() {
            var errmsg = ''; $('#lblErrMsg').html('');
            if ($('#btnEdcFinalStatement').val() == 'SAVE ROOT CAUSE / CONCLUSION') {
                if ($('#ddltype option:selected').text() == 'Choose')
                    errmsg += 'Choose tyre type<br/>';
                if ($('#ddlComplaintDesc option:selected').text() == 'CHOOSE')
                    errmsg += 'Choose complaint desc<br/>';
                else if ($('#ddlComplaintDesc option:selected').text() == 'ADD NEW ENTRY' && $('#txtComplaintDesc').val().length == 0)
                    errmsg += 'Enter complaint description<br/>';
                if ($('input:radio[id*=rdbEdcConclusion_]:checked').length == 0)
                    errmsg += 'Choose conclusion<br />';
                else {
                    if ($('input:radio[id*=rdbEdcConclusion_]:checked').val() == 'Others') {
                        if ($('#txtOtherEdcConclusion').val().length == 0)
                            errmsg += 'Enter other conclusion<br/>';
                    }
                    if ($('input:radio[id*=rdbEdcConclusion_]:checked').val() == 'Manufacturing Defect') {
                        if ($('input:radio[id*=rdbClaimNoStatus_]:checked').length == 0)
                            errmsg += 'Choose claim accept / reject <br/>';
                    }
                }
                if ($('#txtEdcRootCause').val().length == 0)
                    errmsg += 'Enter root cause<br/>';
            }
            if (errmsg.length > 0) {
                $('#lblErrMsg').html(errmsg).css({ 'color': '#f00' });
                gotoClaimDiv('lblErrMsg');
                return false;
            }
            else {
                $('#hdnSaveType').val($('#btnEdcFinalStatement').val());
                return true;
            }
        }

        function showDivEdcReopinion(val) {
            if ($("#txtReanalysis").val() == "") $("#divreanalysis").css("display", "none");
            else $("#divreanalysis").css("disabled", "disabled");
            if ($("#txtEdcAdditional").val() == "") $("#divAdditionalDetails").css("display", "none");
            else $("#divAdditionalDetails").css("disabled", "disabled");
            if (val == 1) {
                $("#divEdcReopinion").css("display", "block");
            }
            else {
                if ($("#txtEdc_crmReopinion").val() != "") $("#divEdcReopinion").css("display", "block");
            }
        }

        $('#txtEdc_crmReopinion').keyup(function () {
            if ($('#txtEdc_crmReopinion').val().length > 0)
                $('#btnEdcFinalStatement').val('SAVE REOPINION COMMENTS');
            else
                $('#btnEdcFinalStatement').val('SAVE ROOT CAUSE / CONCLUSION');
        });

    </script>
</asp:Content>
