<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="claimQcAnalysis.aspx.cs" Inherits="TTS.claimQcAnalysis" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/datemonthyear.css" rel="stylesheet" type="text/css" />
    <link href="Styles/GridviewScroll.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .claimImgSize
        {
            height: 300px;
            width: 300px;
        }
        .claimSelectImgSize
        {
            height: 150px;
            width: 150px;
        }
        .claimCatBg
        {
            background-color: #EDCDF1;
        }
        .claimEnterBg
        {
            background-color: #C3B6EC;
        }
        .claimAlyBg
        {
            background-color: #D6F3F8;
        }
        .claimMidBg
        {
            text-align: center;
            background-color: #EDFCBF;
        }
        .claimProcBg
        {
            background-color: #F8F0D6;
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
        <table style="width: 100%;">
            <tr style="text-align: center; background-color: #cec0ea; font-weight: bold;">
                <td>
                    PLANT :
                    <asp:DropDownList runat="server" ID="ddlQcPlant" ClientIDMode="Static" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlQcPlant_IndexChanged">
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
                            <asp:BoundField DataField="Qty" HeaderText="QTY" ItemStyle-Width="30px" />
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
                    <div id="divgvclaim" style="width: 100%; float: left;">
                        <div style="width: 100%; float: left; border: 1px solid #000; background-color: #056442;
                            font-weight: bold; color: #fff; font-size: 15px;">
                            <div style="width: 48%; float: left; text-align: left; padding-right: 5px;">
                                <asp:Label runat="server" ID="lblClaimCustName" ClientIDMode="Static" Text=""></asp:Label>
                            </div>
                            <div style="width: 3%; float: left;">
                                <asp:Label runat="server" ID="lblClaim" ClientIDMode="Static" Text=""></asp:Label>
                            </div>
                            <div style="width: 48%; float: left; text-align: right; padding-left: 5px;">
                                <asp:Label runat="server" ID="lblClaimNo" ClientIDMode="Static" Text=""></asp:Label>
                            </div>
                        </div>
                        <div style="width: 100%; float: left;">
                            <asp:GridView runat="server" ID="gvClaimItems" AutoGenerateColumns="false" Width="100%"
                                RowStyle-Height="20px">
                                <HeaderStyle BackColor="#CACA55" Font-Bold="true" Height="22px" />
                                <Columns>
                                    <asp:BoundField DataField="brand" HeaderText="BRAND" ItemStyle-Width="80px" />
                                    <asp:BoundField DataField="tyresize" HeaderText="SIZE" ItemStyle-Width="120px" />
                                    <asp:BoundField DataField="stencilno" HeaderText="STENCIL NO." ItemStyle-Width="80px" />
                                    <asp:TemplateField HeaderText="COMPLAINT" ItemStyle-Width="180px">
                                        <ItemTemplate>
                                            <%#((string)Eval("appstyle")).Replace("~", "<br/>")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="OPERATING CONDITION" ItemStyle-Width="180px">
                                        <ItemTemplate>
                                            <%#((string)Eval("runninghours")).Replace("~", "<br/>")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="AnalysisStatus" HeaderText="ANALYSED STATUS" ItemStyle-Width="70px" />
                                    <asp:TemplateField HeaderText="REQUEST COMMENTS" ItemStyle-Width="180px">
                                        <ItemTemplate>
                                            <%# Eval("QcAdditionalReq").ToString() != "" ? "<span style='font-weight:bold;'>QC:</span>" : ""%>
                                            <%#((string)Eval("QcAdditionalReq")).Replace("~", "<br/>")%>
                                            <%# Eval("QcAdditionalUpdateComments").ToString() != "" ? "<br /><span style='font-weight:bold;'>CRM:</span>" : ""%>
                                            <%#((string)Eval("QcAdditionalUpdateComments")).Replace("~", "<br/>")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ACTION" ItemStyle-Width="120px">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkClaimItem" runat="server" Text='<%# Eval("QcAdditionalStatus").ToString()=="QC REQUEST"?"Request Cancel":"Prepare/Edit Analysis" %>'
                                                OnClick="lnkClaimItem_Click"></asp:LinkButton>
                                            <asp:HiddenField runat="server" ID="hdnConfig" ClientIDMode="Static" Value='<%# Eval("config") %>' />
                                            <asp:HiddenField runat="server" ID="hdnTyreType" ClientIDMode="Static" Value='<%# Eval("tyretype") %>' />
                                            <asp:HiddenField runat="server" ID="hdnCustgvnType" ClientIDMode="Static" Value='<%# Eval("CustgvnType") %>' />
                                            <asp:HiddenField runat="server" ID="hdnComplaintDesc" ClientIDMode="Static" Value='<%# Eval("ClaimDescription") %>' />
                                            <asp:HiddenField runat="server" ID="hdnReanalysisComment" ClientIDMode="Static" Value='<%# Eval("ReanalysisComment") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div style="width: 1068px; float: left; text-align: left; margin-top: 5px;">
                            <asp:Label runat="server" ID="lblComplaintComments" ClientIDMode="Static" Text=""></asp:Label>
                        </div>
                    </div>
                </td>
            </tr>
            <tr id="divParticularItem" style="display: none;">
                <td align="center">
                    <div style="width: 900px; float: left; padding-left: 80px; padding-right: 80px;">
                        <asp:Label runat="server" ID="lblReanalysisComment" ClientIDMode="Static" Text=""
                            Width="900px" BackColor="#FCD7D7"></asp:Label>
                        <div style="width: 530px; float: left;">
                            <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #000;
                                width: 500px; float: left; padding-left: 10px; line-height: 27px; font-weight: bold;">
                                <tr>
                                    <td class="claimCatBg">
                                        Complaint Date
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblComplaintDate" ClientIDMode="Static" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="claimCatBg">
                                        Complaint Number
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblComplaintNo" ClientIDMode="Static" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="claimCatBg">
                                        Customer Name
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblCustName" ClientIDMode="Static" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="claimCatBg">
                                        Brand
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblClaimBrand" ClientIDMode="Static" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="claimCatBg">
                                        Tyre Size
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblClaimSize" ClientIDMode="Static" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="claimCatBg">
                                        Complaint Stencil Number
                                    </td>
                                    <td style="background-color: #ccc;" id="divBlinkStencil">
                                        <asp:Label runat="server" ID="lblClaimStencils" ClientIDMode="Static" Text="" Font-Size="15px"
                                            Font-Bold="true"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="claimCatBg">
                                        Platform
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlConfig" ClientIDMode="Static" Width="200px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="claimCatBg">
                                        Tyre Type
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlTyretype" ClientIDMode="Static" Width="120px">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblCustgvnType" runat="server" ClientIDMode="Static" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="claimCatBg">
                                        Complaint Desc
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlComplaintDesc" runat="server" ClientIDMode="Static" Width="250px">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtComplaintDesc" runat="server" ClientIDMode="Static" Text="" Width="250px">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="claimEnterBg">
                                        Production Date / Shift
                                    </td>
                                    <td>
                                        <div>
                                            <asp:CheckBox ID="checkdate" runat="server" ClientIDMode="Static" Text="If production date is not available please click here."
                                                TextAlign="Left" ForeColor="Red" Font-Size="9px" /></div>
                                        <asp:TextBox runat="server" ID="txtProDate" ClientIDMode="Static" Text="" Width="100px"></asp:TextBox>
                                        <span style="font-size: 18px; font-weight: bold;">&nbsp;/&nbsp;</span>
                                        <asp:DropDownList runat="server" ID="ddlShift" ClientIDMode="Static" Width="80px">
                                            <asp:ListItem Text="Choose" Value="Choose"></asp:ListItem>
                                            <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div style="width: 330px; float: left;">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <asp:GridView runat="server" ID="gvClaimImages" AutoGenerateColumns="false" AllowPaging="true"
                                        PageSize="1" OnPageIndexChanging="gvClaimImages_PageIndex" PagerStyle-HorizontalAlign="Center"
                                        PagerStyle-VerticalAlign="Middle">
                                        <HeaderStyle CssClass="headerNone" />
                                        <Columns>
                                            <asp:TemplateField ItemStyle-CssClass="claimImgSize">
                                                <ItemTemplate>
                                                    <a href='<%# Eval("ClaimImage") %>' class="claimImgSize" target="_blank">
                                                        <img alt="Claim Images" border="0" src='<%# Eval("ClaimImage") %>' class="claimImgSize">
                                                    </a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div style="width: 900px; float: left; padding-left: 80px; padding-right: 80px;">
                        <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #000;
                            width: 900px; float: left; line-height: 27px; font-weight: bold;">
                            <tr>
                                <td style="width: 213px;" class="claimEnterBg">
                                    Press Details
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtPressDetails" ClientIDMode="Static" Text="" Width="500px"
                                        MaxLength="100"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="claimEnterBg">
                                    Mould Details
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtMouldDetails" ClientIDMode="Static" Text="" Width="500px"
                                        MaxLength="100"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="display: none;">
                                <td class="claimEnterBg">
                                    Obdervation on Mould history
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtMouldHistory" ClientIDMode="Static" Text="" Width="500px"
                                        MaxLength="100"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="width: 900px; float: left; padding-left: 80px; padding-right: 80px;">
                        <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #000;
                            width: 900px; float: left; line-height: 27px; font-weight: bold;">
                            <tr>
                                <td rowspan="2" style="width: 213px; background-color: #BFFCCE">
                                    Compound Details
                                </td>
                                <td style="width: 230px;" class="claimMidBg">
                                    Tread
                                </td>
                                <td style="width: 213px;" class="claimMidBg">
                                    Center
                                </td>
                                <td class="claimMidBg">
                                    Base
                                </td>
                            </tr>
                            <tr style="text-align: center;">
                                <td>
                                    <div>
                                        <asp:DropDownList runat="server" ID="ddlTread" ClientIDMode="Static" Width="160px">
                                        </asp:DropDownList>
                                    </div>
                                    <div style="display: none;" id="divTread">
                                        <asp:TextBox runat="server" ID="txtTreadComp" ClientIDMode="Static" Text="" MaxLength="20"></asp:TextBox>
                                    </div>
                                </td>
                                <td>
                                    <div>
                                        <asp:DropDownList runat="server" ID="ddlCenter" ClientIDMode="Static" Width="160px">
                                        </asp:DropDownList>
                                    </div>
                                    <div style="display: none;" id="divCenter">
                                        <asp:TextBox runat="server" ID="txtCenterComp" ClientIDMode="Static" Text="" MaxLength="20"></asp:TextBox>
                                    </div>
                                </td>
                                <td>
                                    <div>
                                        <asp:DropDownList runat="server" ID="ddlBase" ClientIDMode="Static" Width="160px">
                                        </asp:DropDownList>
                                    </div>
                                    <div style="display: none;" id="divBase">
                                        <asp:TextBox runat="server" ID="txtBaseComp" ClientIDMode="Static" Text="" MaxLength="20"></asp:TextBox>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #000;
                            width: 900px; float: left; line-height: 27px; font-weight: bold;">
                            <tr>
                                <td style="width: 213px;" class="claimProcBg">
                                    Process Analysis :
                                </td>
                                <td class="claimMidBg">
                                    Required
                                </td>
                                <td class="claimMidBg">
                                    Actual
                                </td>
                                <td style="width: 213px;" class="claimProcBg">
                                    Process Analysis :
                                </td>
                                <td class="claimMidBg">
                                    Start time
                                </td>
                                <td class="claimMidBg">
                                    End time
                                </td>
                            </tr>
                            <tr>
                                <td class="claimAlyBg">
                                    Tyre Weight
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtWtReq" ClientIDMode="Static" Text="" Width="80px"
                                        MaxLength="20"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtWtAct" ClientIDMode="Static" Text="" Width="80px"
                                        MaxLength="20"></asp:TextBox>
                                </td>
                                <td class="claimAlyBg">
                                    Tyre Building time
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtBuildStart" ClientIDMode="Static" Text="" Width="80px"
                                        MaxLength="20"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtBuildEnd" ClientIDMode="Static" Text="" Width="80px"
                                        MaxLength="20"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="claimAlyBg">
                                    Steam pressure
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtSteamReq" ClientIDMode="Static" Text="" Width="80px"
                                        MaxLength="20"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtSteamAct" ClientIDMode="Static" Text="" Width="80px"
                                        MaxLength="20"></asp:TextBox>
                                </td>
                                <td class="claimAlyBg">
                                    UL / L time
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtUlStart" ClientIDMode="Static" Text="" Width="80px"
                                        MaxLength="20"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtUlEnd" ClientIDMode="Static" Text="" Width="80px"
                                        MaxLength="20"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="claimAlyBg">
                                    Hydraulic pressure
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtHyReq" ClientIDMode="Static" Text="" Width="80px"
                                        MaxLength="20"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtHyAct" ClientIDMode="Static" Text="" Width="80px"
                                        MaxLength="20"></asp:TextBox>
                                </td>
                                <td class="claimAlyBg">
                                    Curing time
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtCureStart" ClientIDMode="Static" Text="" Width="80px"
                                        MaxLength="20"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtCureEnd" ClientIDMode="Static" Text="" Width="80px"
                                        MaxLength="20"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="claimAlyBg">
                                    Mould Temperature (Before Loading)
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtTempReq" ClientIDMode="Static" Text="" Width="80px"
                                        MaxLength="20"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtTempAct" ClientIDMode="Static" Text="" Width="80px"
                                        MaxLength="20"></asp:TextBox>
                                </td>
                                <td rowspan="5" colspan="3" style="text-align: center; padding-top: 5px;">
                                    <div style="width: 450px;">
                                        <span class="headCss" style="vertical-align: top; text-align: left; width: 440px;
                                            float: left;">Preparer Root Cause / Comments:</span>
                                        <asp:TextBox runat="server" ID="txtQcComments" ClientIDMode="Static" Text="" Width="440px"
                                            Height="100px" TextMode="MultiLine" onKeyUp="javascript:CheckMaxLength(this, 999);"
                                            onChange="javascript:CheckMaxLength(this, 999);"></asp:TextBox>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="line-height: 15px;" class="claimAlyBg">
                                    Mould Temperature - Check 1<br />
                                    (After 30 mins)
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtChk1Req" ClientIDMode="Static" Text="" Width="80px"
                                        MaxLength="20"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtChk1Act" ClientIDMode="Static" Text="" Width="80px"
                                        MaxLength="20"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="claimAlyBg">
                                    Mould Temperature - Check 2
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtChk2Req" ClientIDMode="Static" Text="" Width="80px"
                                        MaxLength="20"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtChk2Act" ClientIDMode="Static" Text="" Width="80px"
                                        MaxLength="20"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="claimAlyBg">
                                    Hardness
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtHardReq" ClientIDMode="Static" Text="" Width="80px"
                                        MaxLength="20"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtHardAct" ClientIDMode="Static" Text="" Width="80px"
                                        MaxLength="20"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="claimAlyBg">
                                    Curing
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtCuringReq" ClientIDMode="Static" Text="" Width="80px"
                                        MaxLength="20"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtCuringAct" ClientIDMode="Static" Text="" Width="80px"
                                        MaxLength="20"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <asp:DataList runat="server" ID="dlImageList" RepeatColumns="5" RepeatDirection="Horizontal"
                                        RepeatLayout="Table" AlternatingItemStyle-BackColor="#A1C9D3" ItemStyle-BackColor="#C9DAC9"
                                        ItemStyle-VerticalAlign="Top">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chk1" />
                                            <asp:HiddenField runat="server" ID="hdn1" Value='<%# Eval("ClaimImage") %>' />
                                            <asp:Image runat="server" ID="img1" ImageUrl='<%# Eval("ClaimImage") %>' CssClass="claimSelectImgSize" />
                                        </ItemTemplate>
                                    </asp:DataList>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr id="divSaveItem" style="display: none;">
                <td>
                    <div style="width: 900px; float: left; padding-left: 80px; padding-right: 80px; text-align: center;">
                        <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label>
                        <div style="width: 450px; float: left; padding-top: 5px; text-align: left;">
                            <span class="headCss">Additional details ask from CRM:</span>
                            <asp:TextBox runat="server" ID="txtQcAdditional" ClientIDMode="Static" Text="" TextMode="MultiLine"
                                Width="442px" Height="45px" onKeyUp="javascript:CheckMaxLength(this, 499);" onChange="javascript:CheckMaxLength(this, 499);"></asp:TextBox>
                        </div>
                        <div style="width: 450px; float: right; padding-top: 30px; text-align: center;">
                            <asp:Button runat="server" ID="btnSaveClaimAnalysis" ClientIDMode="Static" Text="SAVE ANALYSIS DETAILS"
                                CssClass="btnshow" OnClick="btnSaveClaimAnalysis_Click" OnClientClick="javascript:return CtrlClaimAnalysis();" />
                            <asp:Label runat="server" ID="lblSaveMsg" ClientIDMode="Static" Text="" ForeColor="Green"
                                Font-Bold="true" Font-Size="15px"></asp:Label>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="width: 100%; float: left; border: 1px solid #000; background-color: #efefef;">
                        <asp:Label runat="server" ID="lblErrMsg1" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label>
                        <div style="width: 50%; float: left; padding-top: 5px; display: none;" id="divComments">
                            <span class="headCss">Any Comments:</span>
                            <asp:TextBox runat="server" ID="txtComments" ClientIDMode="Static" Text="" TextMode="MultiLine"
                                Width="525px" Height="80px" onKeyUp="javascript:CheckMaxLength(this, 3999);"
                                onChange="javascript:CheckMaxLength(this, 3999);" CssClass="txtCss"></asp:TextBox>
                        </div>
                        <div style="width: 50%; float: left; padding-top: 40px; text-align: center; display: none;"
                            id="tdAnalysisDataqc">
                            <asp:Button runat="server" ID="btnComplaintMoved" ClientIDMode="Static" CssClass="btnauthorize"
                                Text="MOVE TO EDC OPINION" OnClientClick="javascript:return CtrlRootCauseConclusion();"
                                OnClick="btnComplaintMoved_Click" /></div>
                        <div style="width: 50%; float: right; padding-top: 40px; text-align: center; display: none;"
                            id="tdQcAskAdditionalDetails">
                            <asp:Label runat="server" ID="lblMsgForQcAddtionalDetails" ClientIDMode="Static"
                                Text="" ForeColor="Red"></asp:Label>
                            <asp:Button runat="server" ID="btnMoveToCrmRequest" ClientIDMode="Static" CssClass="btnedit"
                                Text="MOVE TO CRM FOR ADDTIONAL DETAILS" OnClientClick="javascript:return CtrlRootCauseConclusion();"
                                OnClick="btnMoveToCrmRequest_Click" />
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnCustCode" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnProDate" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnClaimNoClick" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnClaimStencilClick" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnImagesUrl" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnPlatform" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnType" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnselectedrow" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnStencilPlant" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnSaveType" ClientIDMode="Static" Value="" />
    <script src="Scripts/datemonthyear.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () { $("#txtProDate").datepicker({ changeMonth: true, changeYear: true, maxDate: 0 }).keydown(function (e) { e.keyWhich = 0; e.keyCode = 0; e.preventDefault(); }); });
        $(document).ready(function () {
            $('#txtComplaintDesc').css({ 'display': 'none' });
            $('#ddlComplaintDesc').change(function () {
                if ($('#ddlComplaintDesc option:selected').text() == 'ADD NEW ENTRY')
                    $('#txtComplaintDesc').css({ 'display': 'block' });
                else
                    $('#txtComplaintDesc').css({ 'display': 'none' });
            });
            blinkStencil(); $('#checkdate').removeAttr('checked');
            $('#ddlConfig').change(function () { $('#ddlTyretype').html(''); $('#hdnPlatform').val(''); var strPlatform = $("#ddlConfig option:selected").text(); if (strPlatform != "CHOOSE") { $.ajax({ type: "POST", url: "BindRecords.aspx?type=getTypeConfigWise&config=" + strPlatform + "", context: document.body, success: function (data) { if (data != '') { $('#ddlTyretype').html(data); $('#hdnPlatform').val(strPlatform); } } }); } });
            $('#checkdate').change(function () { $("#txtProDate").val(''); if ($('#checkdate').attr('checked') == 'checked') $('#txtProDate').val('NA'); else $('#txtProDate').val(''); });
            $("#txtProDate").change(function () { if ($("#txtProDate").val() != "NA" && $("#txtProDate").val() != "") $('#checkdate').removeAttr('checked'); });
            $('#ddlTyretype').change(function () { $('#hdnType').val(''); if ($("#ddlTyretype option:selected").text() != "CHOOSE") $('#hdnType').val($("#ddlTyretype option:selected").text()); });
            $('#ddlTread').change(function () { $('#txtTreadComp').val(''); $('#divTread').css({ 'display': 'none' }); var strTread = $("#ddlTread option:selected").text(); if (strTread != "CHOOSE" && strTread != "OTHERS") $('#txtTreadComp').val(strTread); else if (strTread == "OTHERS") $('#divTread').css({ 'display': 'block' }); });
            $('#ddlCenter').change(function () { $('#txtCenterComp').val(''); $('#divCenter').css({ 'display': 'none' }); var strCenter = $("#ddlCenter option:selected").text(); if (strCenter != "CHOOSE" && strCenter != "OTHERS") $('#txtCenterComp').val(strCenter); else if (strCenter == "OTHERS") $('#divCenter').css({ 'display': 'block' }); });
            $('#ddlBase').change(function () { $('#txtBaseComp').val(''); $('#divBase').css({ 'display': 'none' }); var strBase = $("#ddlBase option:selected").text(); if (strBase != "CHOOSE" && strBase != "OTHERS") $('#txtBaseComp').val(strBase); else if (strBase == "OTHERS") $('#divBase').css({ 'display': 'block' }); });

            $("input:checkbox[id*=MainContent_dlImageList_chk1_]").click(function () {
                $('#hdnImagesUrl').val(''); var buildImagStr = '';
                $("input:checkbox[id*=MainContent_dlImageList_chk1_]").attr("disabled", false);
                if ($("input:checkbox[id*=MainContent_dlImageList_chk1_]:checked").length > 3) { $("input:checkbox[id*=MainContent_dlImageList_chk1_]").attr("disabled", true); $("input:checkbox[id*=MainContent_dlImageList_chk1_]:checked").attr("disabled", false); }
                if ($("input:checkbox[id*=MainContent_dlImageList_chk1_]:checked").length > 0) {
                    $("input:checkbox[id*=MainContent_dlImageList_chk1_]:checked").each(function () { var id1 = this.id; if (buildImagStr.length == 0) buildImagStr = $('#' + id1.replace('chk1_', 'hdn1_')).val(); else if (buildImagStr.length > 0) buildImagStr += '~' + $('#' + id1.replace('chk1_', 'hdn1_')).val(); });
                    $('#hdnImagesUrl').val(buildImagStr);
                }
            });
            $('#txtQcAdditional').blur(function () {
                $('#btnSaveClaimAnalysis').val('SAVE ANALYSIS DETAILS');
                if ($('#txtQcAdditional').val().length > 0)
                    $('#btnSaveClaimAnalysis').val('SAVE ADDTIONAL DETAILS REQUEST');
            }).focus(function () {
                $('#btnSaveClaimAnalysis').val('SAVE ANALYSIS DETAILS');
                if ($('#txtQcAdditional').val().length > 0)
                    $('#btnSaveClaimAnalysis').val('SAVE ADDTIONAL DETAILS REQUEST');
            }).keyup(function () {
                $('#btnSaveClaimAnalysis').val('SAVE ANALYSIS DETAILS');
                if ($('#txtQcAdditional').val().length > 0)
                    $('#btnSaveClaimAnalysis').val('SAVE ADDTIONAL DETAILS REQUEST');
            });
        });
        function CtrlRootCauseConclusion() {
            $('#lblErrMsg1').html(''); var ErrMsg = ''; if ($('#txtComments').val().length == 0) ErrMsg += 'Enter any comments<br/>';
            if (ErrMsg.length > 0) { $('#lblErrMsg1').html(ErrMsg); gotoClaimDiv('lblErrMsg1'); return false; } else return true;
        }
        function showParticularItem(Ctrl, ValType) { $('#' + Ctrl).css({ 'display': ValType }); gotoClaimDiv('divParticularItem'); }
        function showAnalysisData(ctrlID) { $('#' + ctrlID).css({ 'display': 'block' }); $('#divComments').css({ 'display': 'block' }); $('#btnSaveClaimAnalysis').css({ 'display': 'none' }); gotoClaimDiv(ctrlID); }
        function CtrlClaimAnalysis() {
            var errmsg = ''; $('#lblErrMsg').html(''); $('#lblSaveMsg').html('');
            if ($('#btnSaveClaimAnalysis').val() == 'SAVE ANALYSIS DETAILS') {
                if ($("#txtProDate").val() == 'NA')
                    $('#hdnProDate').val('15/08/1947');
                else
                    $('#hdnProDate').val($("#txtProDate").val());
                if ($('#hdnPlatform').val().length == 0)
                    errmsg += 'Choose platform<br/>';
                if ($('#hdnType').val().length == 0)
                    errmsg += 'Choose tyretype<br/>';
                if ($('#hdnProDate').val().length == 0)
                    errmsg += 'Enter production date<br/>';
                if ($('#ddlComplaintDesc option:selected').text() == 'CHOOSE')
                    errmsg += 'Choose complaint desc<br/>';
                else if ($('#ddlComplaintDesc option:selected').text() == 'ADD NEW ENTRY' && $('#txtComplaintDesc').val().length == 0)
                    errmsg += 'Enter complaint desc<br/>';
                if ($("input:checkbox[id*=MainContent_dlImageList_chk1_]:checked").length > 0) {
                    if ($('#hdnImagesUrl').val().length == 0)
                        errmsg += 'Once again choose failure images<br/>';
                }
                if ($('#txtQcComments').val().length == 0)
                    errmsg += 'Enter root cause<br/>';
            }
            else if ($('#btnSaveClaimAnalysis').val() == 'SAVE ADDTIONAL DETAILS REQUEST') {
                if ($('#txtQcAdditional').val().length == 0)
                    errmsg += 'Enter your addtional details request<br/>';
            }
            else if ($('#btnSaveClaimAnalysis').val().length == 0)
                errmsg += 'Please reload the page';
            if (errmsg.length > 0) {
                $('#lblErrMsg').html(errmsg);
                gotoClaimDiv('lblErrMsg');
                return false;
            }
            else {
                $('#hdnSaveType').val($('#btnSaveClaimAnalysis').val());
                return true;
            }
        }
    </script>
</asp:Content>
