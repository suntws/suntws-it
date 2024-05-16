<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="orderpositionentry.aspx.cs" Inherits="TTS.orderpositionentry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .txtRightAlign
        {
            text-align: right;
        }
        .monthTable
        {
            border-color: #07A714;
            line-height: 25px;
            width: 530px;
            background-color: #9ce159;
        }
        .monthTable td:nth-child(1)
        {
            font-weight: bold;
            width: 200px;
            background-color: #B3F6C9;
        }
        .monthTable td:nth-child(2)
        {
            width: 90px;
            float: left;
            text-align: right;
            padding-right: 12px;
        }
        .monthTable td:nth-child(3)
        {
            width: 90px;
            float: left;
            text-align: right;
            padding-right: 12px;
        }
        .monthTable td:nth-child(4)
        {
            width: 90px;
            float: left;
            text-align: right;
            padding-right: 12px;
        }
        .monthTableTotal
        {
            width: 92px;
            float: left;
            padding-right: 10px;
            text-align: right;
            font-size: 16px;
            background-color: #B3F6C9;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div align="center" class="pageTitleHead">
        ORDER POSITION ENTRY</div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table>
            <tr>
                <td style="width: 500px; float: left; padding: 10px;">
                    <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #07A714;
                        line-height: 28px; width: 500px;" id="tableentry">
                        <tr style="text-align: center; font-weight: bold; font-size: 15px; background-color: #E7FCA7;">
                            <td>
                                <asp:Label runat="server" ID="lblCurDate" ClientIDMode="Static" Text=""></asp:Label>
                            </td>
                            <td>
                                INFLOW
                            </td>
                            <td>
                                DISPATCH
                            </td>
                            <td>
                                BACKLOG
                            </td>
                        </tr>
                        <tr>
                            <td>
                                SLTL (EXPORT)
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtLankaExpInflow" ClientIDMode="Static" Text=""
                                    Width="70px" CssClass="txtRightAlign" MaxLength="8" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtLankaExpDispatch" ClientIDMode="Static" Text=""
                                    Width="70px" CssClass="txtRightAlign" MaxLength="8" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtLankaExpBacklog" ClientIDMode="Static" Text=""
                                    Width="70px" CssClass="txtRightAlign" MaxLength="8" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                SITL DIRECT (EXPORT)
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtStarcoExpInflow" ClientIDMode="Static" Text=""
                                    Width="70px" CssClass="txtRightAlign" MaxLength="8" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtStarcoExpDispatch" ClientIDMode="Static" Text=""
                                    Width="70px" CssClass="txtRightAlign" MaxLength="8" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtStarcoExpBacklog" ClientIDMode="Static" Text=""
                                    Width="70px" CssClass="txtRightAlign" MaxLength="8" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                SITL JOB WORK (EXPORT)
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtStarcoJobWorkInflow" ClientIDMode="Static" Text=""
                                    Width="70px" CssClass="txtRightAlign" MaxLength="8" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtStarcoJobWorkDispatch" ClientIDMode="Static" Text=""
                                    Width="70px" CssClass="txtRightAlign" MaxLength="8" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtStarcoJobWorkBacklog" ClientIDMode="Static" Text=""
                                    Width="70px" CssClass="txtRightAlign" MaxLength="8" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                MMN (EXPORT)
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtMMNExpInflow" ClientIDMode="Static" Text="" Width="70px"
                                    CssClass="txtRightAlign" MaxLength="8" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtMMNExpDispatch" ClientIDMode="Static" Text=""
                                    Width="70px" CssClass="txtRightAlign" MaxLength="8" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtMMNExpBacklog" ClientIDMode="Static" Text="" Width="70px"
                                    CssClass="txtRightAlign" MaxLength="8" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                PDK (EXPORT)
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtPdkExpInflow" ClientIDMode="Static" Text="" Width="70px"
                                    CssClass="txtRightAlign" MaxLength="8" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtPDKExpDispatch" ClientIDMode="Static" Text=""
                                    Width="70px" CssClass="txtRightAlign" MaxLength="8" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtPDKExpBacklog" ClientIDMode="Static" Text="" Width="70px"
                                    CssClass="txtRightAlign" MaxLength="8" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                PNEUMATIC (EXPORT)
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtPneuExpInflow" ClientIDMode="Static" Text="" Width="70px"
                                    CssClass="txtRightAlign" MaxLength="8" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtPneuExpDispatch" ClientIDMode="Static" Text=""
                                    Width="70px" CssClass="txtRightAlign" MaxLength="8" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtPneuExpBacklog" ClientIDMode="Static" Text=""
                                    Width="70px" CssClass="txtRightAlign" MaxLength="8" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                RIMS (EXPORT)
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtRimsExpInflow" ClientIDMode="Static" Text="" Width="70px"
                                    CssClass="txtRightAlign" MaxLength="8" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtRimsExpDispatch" ClientIDMode="Static" Text=""
                                    Width="70px" CssClass="txtRightAlign" MaxLength="8" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtRimsExpBacklog" ClientIDMode="Static" Text=""
                                    Width="70px" CssClass="txtRightAlign" MaxLength="8" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                MMN (DOMESTIC)
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtMMNDomInflow" ClientIDMode="Static" Text="" Width="70px"
                                    CssClass="txtRightAlign" MaxLength="8" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtMMNDomDispatch" ClientIDMode="Static" Text=""
                                    Width="70px" CssClass="txtRightAlign" MaxLength="8" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtMMNDomBacklog" ClientIDMode="Static" Text="" Width="70px"
                                    CssClass="txtRightAlign" MaxLength="8" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                PDK (DOMESTIC)
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtPDKDomInflow" ClientIDMode="Static" Text="" Width="70px"
                                    CssClass="txtRightAlign" MaxLength="8" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtPDKDomDispatch" ClientIDMode="Static" Text=""
                                    Width="70px" CssClass="txtRightAlign" MaxLength="8" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtPDKDomBacklog" ClientIDMode="Static" Text="" Width="70px"
                                    CssClass="txtRightAlign" MaxLength="8" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="display: none;">
                            <td>
                                C&F (DOMESTIC)
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtCFDomInflow" ClientIDMode="Static" Text="" Width="70px"
                                    CssClass="txtRightAlign" MaxLength="8" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtCFDomDispatch" ClientIDMode="Static" Text="" Width="70px"
                                    CssClass="txtRightAlign" MaxLength="8" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtCFDomBacklog" ClientIDMode="Static" Text="" Width="70px"
                                    CssClass="txtRightAlign" MaxLength="8" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                MMN (ME)
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtMmnMeInflow" ClientIDMode="Static" Text="" Width="70px"
                                    CssClass="txtRightAlign" MaxLength="8" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtMmnMeDispatch" ClientIDMode="Static" Text="" Width="70px"
                                    CssClass="txtRightAlign" MaxLength="8" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtMmnMeBacklog" ClientIDMode="Static" Text="" Width="70px"
                                    CssClass="txtRightAlign" MaxLength="8" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                PDK (ME)
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtPdkMeInflow" ClientIDMode="Static" Text="" Width="70px"
                                    CssClass="txtRightAlign" MaxLength="8" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtPdkMeDispatch" ClientIDMode="Static" Text="" Width="70px"
                                    CssClass="txtRightAlign" MaxLength="8" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtPdkMeBacklog" ClientIDMode="Static" Text="" Width="70px"
                                    CssClass="txtRightAlign" MaxLength="8" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="background-color: #FADADA;">
                            <td style="font-weight: bold; font-size: 15px;">
                                TOTAL
                            </td>
                            <td style="text-align: center; padding-right: 15px;">
                                <asp:TextBox runat="server" ID="txtTotInflow" ClientIDMode="Static" Text="" Width="85px"
                                    ReadOnly="true" Font-Bold="true" CssClass="txtRightAlign"></asp:TextBox>
                            </td>
                            <td style="text-align: center; padding-right: 15px;">
                                <asp:TextBox runat="server" ID="txtTotDispatch" ClientIDMode="Static" Text="" Width="85px"
                                    ReadOnly="true" Font-Bold="true" CssClass="txtRightAlign"></asp:TextBox>
                            </td>
                            <td style="text-align: center; padding-right: 15px;">
                                <asp:TextBox runat="server" ID="txtTotBacklog" ClientIDMode="Static" Text="" Width="85px"
                                    ReadOnly="true" Font-Bold="true" CssClass="txtRightAlign"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="text-align: center; line-height: 20px; vertical-align: middle;
                                padding-top: 5px;">
                                <div style="width: 190px; float: left;">
                                    <asp:Button runat="server" ID="btnPositionSave" ClientIDMode="Static" Text="SAVE"
                                        CssClass="btnsave" OnClientClick="javascript:return ctrlValidate();" OnClick="btnPositionSave_Click" />
                                    <asp:Button runat="server" ID="btnClear" ClientIDMode="Static" Text="CLEAR" CssClass="btnclear"
                                        OnClick="btnClear_Click" />
                                </div>
                                <div style="width: 25px; float: left; line-height: 30px;">
                                    <asp:Label runat="server" ID="lblOR" ClientIDMode="Static" Text="" Font-Size="15px"></asp:Label>
                                </div>
                                <div style="width: 270px; float: left; text-align: left; padding-left: 10px;">
                                    <asp:Label runat="server" ID="lblError" ClientIDMode="Static" Text=""></asp:Label>
                                    <span id="spanPreviousBtn" style="display: none;">
                                        <asp:Button runat="server" ID="btnPreviousMonthSave" ClientIDMode="Static" Text=""
                                            CssClass="btnauthorize" OnClientClick="javascript:return ctrlValidate();" OnClick="btnPreviousMonthSave_Click" /></span>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; line-height: 28px;
                        border: 1px solid #000; margin-top: 20px; background-color: #C5A8A8; display: none;
                        width: 500px;" id="divOpenEntry">
                        <tr>
                            <td colspan="4" style="text-align: center; font-weight: bold; font-size: 14px;">
                                ENTER OPENING STOCK
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 150px;">
                                SLTL (EXPORT)
                            </td>
                            <td style="width: 100px;">
                                <asp:TextBox runat="server" ID="txtOpenStockLanka" ClientIDMode="Static" Text=""
                                    Width="85px" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                            <td>
                                RIMS (EXPORT)
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtOpenStockRimsExport" ClientIDMode="Static" Text=""
                                    Width="85px" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                SITL DIRECT (EXPORT)
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtOpenStockStarco" ClientIDMode="Static" Text=""
                                    Width="85px" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                            <td style="width: 150px;">
                                MMN (DOMESTIC)
                            </td>
                            <td style="width: 100px;">
                                <asp:TextBox runat="server" ID="txtOpenStockMMNDomestic" ClientIDMode="Static" Text=""
                                    Width="85px" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="line-height: 15px;">
                                SITL JOB WORK (EXPORT)
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtOpenStockStarcoJobWork" ClientIDMode="Static"
                                    Text="" Width="85px" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                            <td>
                                PDK (DOMESTIC)
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtOpenStockPDKDomestic" ClientIDMode="Static" Text=""
                                    Width="85px" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                MMN (EXPORT)
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtOpenStockMMN" ClientIDMode="Static" Text="" Width="85px"
                                    onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                            <td>
                                MMN (ME)
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtOpenStockMmnMe" ClientIDMode="Static" Text=""
                                    Width="85px" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                PDK (EXPORT)
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtOpenStockPDKExport" ClientIDMode="Static" Text=""
                                    Width="85px" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                            <td>
                                PDK (ME)
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtOpenStockPdkMe" ClientIDMode="Static" Text=""
                                    Width="85px" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                PNEUMATIC (EXPORT)
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtOpenStockPneuExport" ClientIDMode="Static" Text=""
                                    Width="85px" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                            <td colspan="2" style="text-align: center;">
                                <asp:Button runat="server" ID="btnSaveOpenStock" ClientIDMode="Static" Text="SAVE OPEN STOCK"
                                    OnClick="btnSaveOpenStock_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td style="display: none;" colspan="2">
                                C&F (DOMESTIC)
                            </td>
                            <td style="display: none;" colspan="2">
                                <asp:TextBox runat="server" ID="txtOpenStockCFDom" ClientIDMode="Static" Text=""
                                    Width="85px" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <div style="width: 530px; float: left; padding: 10px;">
                        <asp:Repeater runat="server" ID="rptOrderPositionMonth">
                            <ItemTemplate>
                                <table cellspacing="0" rules="all" border="1" class="monthTable" style="border-collapse: collapse;">
                                    <tr style="text-align: center; font-weight: bold; font-size: 13px; background-color: #B3F6C9;">
                                        <td style="background-color: #9ce159; width: 210px;">
                                            <%# Eval("AsMonth")%>
                                            &nbsp;
                                            <%# Eval("AsYear")%>
                                        </td>
                                        <td style="text-align: center; padding-right: 0px; width: 102px;">
                                            INFLOW
                                        </td>
                                        <td style="text-align: center; padding-right: 0px; width: 102px;">
                                            DISPATCH
                                        </td>
                                        <td style="text-align: center; padding-right: 0px; width: 102px;">
                                            BACKLOG
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            SLTL (EXPORT)
                                        </td>
                                        <td>
                                            <%# Eval("LankaExpInflow")%>
                                        </td>
                                        <td>
                                            <%# Eval("LankaExpDispatch")%>
                                        </td>
                                        <td>
                                            <%# Eval("LankaExpBacklog")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            SITL DIRECT (EXPORT)
                                        </td>
                                        <td>
                                            <%# Eval("StarcoExpInflow")%>
                                        </td>
                                        <td>
                                            <%# Eval("StarcoExpDispatch")%>
                                        </td>
                                        <td>
                                            <%# Eval("StarcoExpBacklog")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            SITL JOB WORK (EXPORT)
                                        </td>
                                        <td>
                                            <%# Eval("StarcoJobWorkInflow")%>
                                        </td>
                                        <td>
                                            <%# Eval("StarcoJobWorkDispatch")%>
                                        </td>
                                        <td>
                                            <%# Eval("StarcoJobWorkBacklog")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            MMN (EXPORT)
                                        </td>
                                        <td>
                                            <%# Eval("MmnExpInflow")%>
                                        </td>
                                        <td>
                                            <%# Eval("MmnExpDispatch")%>
                                        </td>
                                        <td>
                                            <%# Eval("MmnExpBacklog")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            PDK (EXPORT)
                                        </td>
                                        <td>
                                            <%# Eval("PdkExpInflow")%>
                                        </td>
                                        <td>
                                            <%# Eval("PdkExpDispatch")%>
                                        </td>
                                        <td>
                                            <%# Eval("PdkExpBacklog")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            PNEUMATIC (EXPORT)
                                        </td>
                                        <td>
                                            <%# Eval("PneuExpInflow")%>
                                        </td>
                                        <td>
                                            <%# Eval("PneuExpDispatch")%>
                                        </td>
                                        <td>
                                            <%# Eval("PneuExpBacklog")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            RIMS (EXPORT)
                                        </td>
                                        <td>
                                            <%# Eval("RimsExpInflow")%>
                                        </td>
                                        <td>
                                            <%# Eval("RimsExpDispatch")%>
                                        </td>
                                        <td>
                                            <%# Eval("RimsExpBacklog")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            MMN (DOMESTIC)
                                        </td>
                                        <td>
                                            <%# Eval("MmnDomInflow")%>
                                        </td>
                                        <td>
                                            <%# Eval("MmnDomDispatch")%>
                                        </td>
                                        <td>
                                            <%# Eval("MmnDomBacklog")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            PDK (DOMESTIC)
                                        </td>
                                        <td>
                                            <%# Eval("PdkDomInflow")%>
                                        </td>
                                        <td>
                                            <%# Eval("PdkDomDispatch")%>
                                        </td>
                                        <td>
                                            <%# Eval("PdkDomBacklog")%>
                                        </td>
                                    </tr>
                                    <tr style="display: none;">
                                        <td>
                                            C&F (DOMESTIC)
                                        </td>
                                        <td>
                                            <%# Eval("CfDomInflow")%>
                                        </td>
                                        <td>
                                            <%# Eval("CfDomDispatch")%>
                                        </td>
                                        <td>
                                            <%# Eval("CfDomBacklog")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            MMN (ME)
                                        </td>
                                        <td>
                                            <%# Eval("MmnMeInflow")%>
                                        </td>
                                        <td>
                                            <%# Eval("MmnMeDispatch")%>
                                        </td>
                                        <td>
                                            <%# Eval("MmnMeBacklog")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            PDK (ME)
                                        </td>
                                        <td>
                                            <%# Eval("PdkMeInflow")%>
                                        </td>
                                        <td>
                                            <%# Eval("PdkMeDispatch")%>
                                        </td>
                                        <td>
                                            <%# Eval("PdkMeBacklog")%>
                                        </td>
                                    </tr>
                                    <tr style="font-weight: bold;">
                                        <td style="font-size: 14px; background-color: #9ce159; text-align: right;">
                                            TOTAL MONTH
                                        </td>
                                        <td>
                                            <div class="monthTableTotal">
                                                <%# Eval("InflowTot")%>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="monthTableTotal">
                                                <%# Eval("DispatchTot")%>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="monthTableTotal">
                                                <%# Eval("BacklogTot")%>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                    <div style="width: 530px; float: left; padding: 10px;">
                        <asp:Repeater runat="server" ID="rptOrderPosition">
                            <ItemTemplate>
                                <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #07A714;
                                    line-height: 28px; width: 530px;">
                                    <tr>
                                        <td colspan="3" style="text-align: center; font-weight: bold; font-size: 15px; background-color: #E7FCA7;">
                                            <span>Last updated on : </span>
                                            <%# Eval("AsOnDate")%>
                                            <span style="padding-left: 20px;">By : </span>
                                            <%# Eval("UserName")%>
                                        </td>
                                    </tr>
                                    <tr style="text-align: center; font-weight: bold; font-size: 15px;">
                                        <td style="width: 200px;">
                                        </td>
                                        <td>
                                            INFLOW
                                        </td>
                                        <td>
                                            DISPATCH
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            SLTL (EXPORT)
                                        </td>
                                        <td>
                                            <%# Eval("LankaExpInflow")%>
                                        </td>
                                        <td>
                                            <%# Eval("LankaExpDispatch")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            SITL DIRECT (EXPORT)
                                        </td>
                                        <td>
                                            <%# Eval("StarcoExpInflow")%>
                                        </td>
                                        <td>
                                            <%# Eval("StarcoExpDispatch")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            SITL JOB WORK (EXPORT)
                                        </td>
                                        <td>
                                            <%# Eval("StarcoJobWorkInflow")%>
                                        </td>
                                        <td>
                                            <%# Eval("StarcoJobWorkDispatch")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            MMN (EXPORT)
                                        </td>
                                        <td>
                                            <%# Eval("MmnExpInflow")%>
                                        </td>
                                        <td>
                                            <%# Eval("MmnExpDispatch")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            PDK (EXPORT)
                                        </td>
                                        <td>
                                            <%# Eval("PdkExpInflow")%>
                                        </td>
                                        <td>
                                            <%# Eval("PdkExpDispatch")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            PNEUMATIC (EXPORT)
                                        </td>
                                        <td>
                                            <%# Eval("PneuExpInflow")%>
                                        </td>
                                        <td>
                                            <%# Eval("PneuExpDispatch")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            RIMS (EXPORT)
                                        </td>
                                        <td>
                                            <%# Eval("RimsExpInflow")%>
                                        </td>
                                        <td>
                                            <%# Eval("RimsExpDispatch")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            MMN (DOMESTIC)
                                        </td>
                                        <td>
                                            <%# Eval("MmnDomInflow")%>
                                        </td>
                                        <td>
                                            <%# Eval("MmnDomDispatch")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            PDK (DOMESTIC)
                                        </td>
                                        <td>
                                            <%# Eval("PdkDomInflow")%>
                                        </td>
                                        <td>
                                            <%# Eval("PdkDomDispatch")%>
                                        </td>
                                    </tr>
                                    <tr style="display: none;">
                                        <td>
                                            C&F (DOMESTIC)
                                        </td>
                                        <td>
                                            <%# Eval("CfDomInflow")%>
                                        </td>
                                        <td>
                                            <%# Eval("CfDomDispatch")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            MMN (ME)
                                        </td>
                                        <td>
                                            <%# Eval("MmnMeInflow")%>
                                        </td>
                                        <td>
                                            <%# Eval("MmnMeDispatch")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            PDK (ME)
                                        </td>
                                        <td>
                                            <%# Eval("PdkMeInflow")%>
                                        </td>
                                        <td>
                                            <%# Eval("PdkMeDispatch")%>
                                        </td>
                                    </tr>
                                    <tr style="font-weight: bold;">
                                        <td style="font-weight: bold; font-size: 16px;">
                                            TOTAL
                                        </td>
                                        <td>
                                            <div style="width: 91px; float: left; padding-right: 30px; text-align: right; font-size: 16px;">
                                                <%# Eval("InflowTot")%>
                                            </div>
                                        </td>
                                        <td>
                                            <div style="width: 91px; float: left; padding-right: 30px; text-align: right; font-size: 16px;">
                                                <%# Eval("DispatchTot")%>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnInflowTot" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnBacklogTot" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnDispatchTot" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnLankaExpBacklog" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnStarcoExpBacklog" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnStarcoJobWorkBacklog" ClientIDMode="Static"
        Value="" />
    <asp:HiddenField runat="server" ID="hdnMMNExpBacklog" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnPDKExpBacklog" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnPneuExpBacklog" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnRimsExpBacklog" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnMMNDomBacklog" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnPDKDomBacklog" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnCFDomBacklog" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnMmnMeBacklog" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnPdkMeBacklog" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        $(document).ready(function () {
            $(':text').bind('keydown', function (e) { if (e.keyCode == 13) { e.preventDefault(); return false; } });
            if ($('#btnPreviousMonthSave').val() == '')
                $('#tableentry').find('tr :nth-child(4)').css({ 'display': 'none' });
            $('.txtRightAlign').blur(function () {
                $('#txtLankaExpBacklog').val(Number($('#hdnLankaExpBacklog').val()) + Number($('#txtLankaExpInflow').val()) - Number($('#txtLankaExpDispatch').val()));
                $('#txtStarcoExpBacklog').val(Number($('#hdnStarcoExpBacklog').val()) + Number($('#txtStarcoExpInflow').val()) - Number($('#txtStarcoExpDispatch').val()));
                $('#txtStarcoJobWorkBacklog').val(Number($('#hdnStarcoJobWorkBacklog').val()) + Number($('#txtStarcoJobWorkInflow').val()) - Number($('#txtStarcoJobWorkDispatch').val()));
                $('#txtMMNExpBacklog').val(Number($('#hdnMMNExpBacklog').val()) + Number($('#txtMMNExpInflow').val()) - Number($('#txtMMNExpDispatch').val()));
                $('#txtPDKExpBacklog').val(Number($('#hdnPDKExpBacklog').val()) + Number($('#txtPdkExpInflow').val()) - Number($('#txtPDKExpDispatch').val()));
                $('#txtPneuExpBacklog').val(Number($('#hdnPneuExpBacklog').val()) + Number($('#txtPneuExpInflow').val()) - Number($('#txtPneuExpDispatch').val()));
                $('#txtRimsExpBacklog').val(Number($('#hdnRimsExpBacklog').val()) + Number($('#txtRimsExpInflow').val()) - Number($('#txtRimsExpDispatch').val()));
                $('#txtMMNDomBacklog').val(Number($('#hdnMMNDomBacklog').val()) + Number($('#txtMMNDomInflow').val()) - Number($('#txtMMNDomDispatch').val()));
                $('#txtPDKDomBacklog').val(Number($('#hdnPDKDomBacklog').val()) + Number($('#txtPDKDomInflow').val()) - Number($('#txtPDKDomDispatch').val()));
                $('#txtCFDomBacklog').val(Number($('#hdnCFDomBacklog').val()) + Number($('#txtCFDomInflow').val()) - Number($('#txtCFDomDispatch').val()));
                $('#txtMmnMeBacklog').val(Number($('#hdnMmnMeBacklog').val()) + Number($('#txtMmnMeInflow').val()) - Number($('#txtMmnMeDispatch').val()));
                $('#txtPdkMeBacklog').val(Number($('#hdnPdkMeBacklog').val()) + Number($('#txtPdkMeInflow').val()) - Number($('#txtPdkMeDispatch').val()));

                var decInflowVal = Number($('#txtLankaExpInflow').val()) + Number($('#txtStarcoExpInflow').val()) + Number($('#txtStarcoJobWorkInflow').val()) +
                Number($('#txtMMNExpInflow').val()) + Number($('#txtPdkExpInflow').val()) + Number($('#txtPneuExpInflow').val()) + Number($('#txtRimsExpInflow').val()) +
                Number($('#txtMMNDomInflow').val()) + Number($('#txtPDKDomInflow').val()) + Number($('#txtCFDomInflow').val()) + Number($('#txtMmnMeInflow').val()) +
                Number($('#txtPdkMeInflow').val());

                var decBacklogVal = Number($('#txtLankaExpBacklog').val()) + Number($('#txtStarcoExpBacklog').val()) + Number($('#txtStarcoJobWorkBacklog').val()) +
                Number($('#txtMMNExpBacklog').val()) + Number($('#txtPDKExpBacklog').val()) + Number($('#txtPneuExpBacklog').val()) + Number($('#txtRimsExpBacklog').val()) +
                Number($('#txtMMNDomBacklog').val()) + Number($('#txtPDKDomBacklog').val()) + Number($('#txtCFDomBacklog').val()) + Number($('#txtMmnMeBacklog').val()) +
                Number($('#txtPdkMeBacklog').val());

                var decDispatchVal = Number($('#txtLankaExpDispatch').val()) + Number($('#txtStarcoExpDispatch').val()) + Number($('#txtStarcoJobWorkDispatch').val()) +
                Number($('#txtMMNExpDispatch').val()) + Number($('#txtPDKExpDispatch').val()) + Number($('#txtPneuExpDispatch').val()) + Number($('#txtRimsExpDispatch').val()) +
                Number($('#txtMMNDomDispatch').val()) + Number($('#txtPDKDomDispatch').val()) + Number($('#txtCFDomDispatch').val()) + Number($('#txtMmnMeDispatch').val()) +
                Number($('#txtPdkMeDispatch').val());

                $('#txtTotInflow').val(decInflowVal.toFixed(2)); $('#txtTotBacklog').val(decBacklogVal.toFixed(2)); $('#txtTotDispatch').val(decDispatchVal.toFixed(2));
                $('#hdnInflowTot').val($('#txtTotInflow').val()); $('#hdnBacklogTot').val($('#txtTotBacklog').val()); $('#hdnDispatchTot').val($('#txtTotDispatch').val());
            });
        });

        function ctrlValidate() {
            if ($('#txtTotInflow').val().length == 0 && $('#txtTotBacklog').val().length == 0 && $('#txtTotDispatch').val().length == 0) {
                $('#lblError').html('Enter any one data').css({ 'color': '#ff0000' });
                return false;
            }
            if (parseFloat($('#txtTotInflow').val()) == 0 && parseFloat($('#txtTotBacklog').val()) == 0 && parseFloat($('#txtTotDispatch').val()) == 0) {
                $('#lblError').html('Enter any one data').css({ 'color': '#ff0000' });
                return false;
            }
            else
                return true;
        }

        function ctrlBacklogShow() {
            $('#tableentry').find('tr :nth-child(4)').css({ 'display': 'block' });
            $('#spanPreviousBtn').css({ 'display': 'block' }); $('#divOpenEntry').css({ 'display': 'block' });
        }
    </script>
</asp:Content>
