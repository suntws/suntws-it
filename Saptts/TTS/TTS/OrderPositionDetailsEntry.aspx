<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderPositionDetailsEntry.aspx.cs"
    MasterPageFile="~/master.Master" Inherits="TTS.OrderPositionDetailsEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/orderposition.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div align="center" class="pageTitleHead">
        ORDER POSITION ENTRY
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table>
            <tr>
                <td>
                    <table cellspacing="0" rules="all" border="1" class="tbMain">
                        <tr style="color: #ffffff; font-size: 18px;">
                            <th>
                                <asp:Label ID="lblcurrentDate" runat="server" ClientIDMode="Static"></asp:Label>
                            </th>
                            <th>
                                INFLOW
                            </th>
                            <th>
                                DESPATCH
                            </th>
                            <th>
                                BACKLOG
                            </th>
                        </tr>
                        <tr>
                            <td style="vertical-align: top;">
                                <table cellspacing="0" rules="all" border="1" class="tbHead">
                                    <tr>
                                        <th style="line-height: 25px;">
                                            TYPE
                                        </th>
                                    </tr>
                                    <tr>
                                        <th>
                                            SLTL (EXPORT)
                                        </th>
                                    </tr>
                                    <tr>
                                        <th>
                                            SITL (EXPORT)
                                        </th>
                                    </tr>
                                    <tr>
                                        <th>
                                            MMN (EXPORT)
                                        </th>
                                    </tr>
                                    <tr>
                                        <th>
                                            PDK (EXPORT)
                                        </th>
                                    </tr>
                                    <tr>
                                        <th>
                                            MMN (DOMESTIC)
                                        </th>
                                    </tr>
                                    <tr>
                                        <th>
                                            PDK (DOMESTIC)
                                        </th>
                                    </tr>
                                    <tr>
                                        <th>
                                            MMN (C&F)
                                        </th>
                                    </tr>
                                    <tr>
                                        <th>
                                            PDK (C&F)
                                        </th>
                                    </tr>
                                    <tr>
                                        <th style="background-color: #631009; color: #ffffff; font-weight: bold; font-size: 14px;">
                                            GRAND TOTAL
                                        </th>
                                    </tr>
                                </table>
                            </td>
                            <td style="vertical-align: top;">
                                <table cellspacing="0" rules="all" border="1" class="tbInflow">
                                    <tr>
                                        <th>
                                            SOLID TYRE
                                        </th>
                                        <th>
                                            RIM
                                        </th>
                                        <th>
                                            PNEUMATICS
                                        </th>
                                        <th>
                                            TOTAL
                                        </th>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtLankaExp_TyreInflow" runat="server" ClientIDMode="Static" CssClass="txtRightAlign"
                                                MaxLength="6" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLankaExp_RimInflow" runat="server" ClientIDMode="Static" CssClass="txtRightAlign"
                                                MaxLength="6" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLankaExp_PneumaticsInflow" runat="server" ClientIDMode="Static"
                                                CssClass="txtRightAlign" MaxLength="6" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLankaExp_TotalInflow" runat="server" ClientIDMode="Static" CssClass="txtTotal"
                                                ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtStarcoExp_TyreInflow" runat="server" ClientIDMode="Static" CssClass="txtRightAlign"
                                                MaxLength="6" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtStarcoExp_RimInflow" runat="server" ClientIDMode="Static" CssClass="txtRightAlign"
                                                MaxLength="6" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtStarcoExp_PneumaticsInflow" runat="server" ClientIDMode="Static"
                                                CssClass="txtRightAlign" MaxLength="6" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtStarcoExp_TotalInflow" runat="server" ClientIDMode="Static" CssClass="txtTotal"
                                                ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtmmnExp_TyreInflow" runat="server" ClientIDMode="Static" CssClass="txtRightAlign"
                                                MaxLength="6" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtmmnExp_RimInflow" runat="server" ClientIDMode="Static" CssClass="txtRightAlign"
                                                MaxLength="6" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtmmnExp_PneumaticsInflow" runat="server" ClientIDMode="Static"
                                                CssClass="txtRightAlign" MaxLength="6" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtmmnExp_TotalInflow" runat="server" ClientIDMode="Static" CssClass="txtTotal"
                                                ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtpdkExp_TyreInflow" runat="server" ClientIDMode="Static" CssClass="txtRightAlign"
                                                MaxLength="6" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtpdkExp_RimInflow" runat="server" ClientIDMode="Static" CssClass="txtRightAlign"
                                                MaxLength="6" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtpdkExp_PneumaticsInflow" runat="server" ClientIDMode="Static"
                                                CssClass="txtRightAlign" MaxLength="6" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtpdkExp_TotalInflow" runat="server" ClientIDMode="Static" CssClass="txtTotal"
                                                ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtmmnDom_TyreInflow" runat="server" ClientIDMode="Static" CssClass="txtRightAlign"
                                                MaxLength="6" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtmmnDom_RimInflow" runat="server" ClientIDMode="Static" CssClass="txtRightAlign"
                                                MaxLength="6" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtmmnDom_PneumaticsInflow" runat="server" ClientIDMode="Static"
                                                CssClass="txtRightAlign" MaxLength="6" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtmmnDom_TotalInflow" runat="server" ClientIDMode="Static" CssClass="txtTotal"
                                                ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtpdkDom_TyreInflow" runat="server" ClientIDMode="Static" CssClass="txtRightAlign"
                                                MaxLength="6" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtpdkDom_RimInflow" runat="server" ClientIDMode="Static" CssClass="txtRightAlign"
                                                MaxLength="6" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtpdkDom_PneumaticsInflow" runat="server" ClientIDMode="Static"
                                                CssClass="txtRightAlign" MaxLength="6" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtpdkDom_TotalInflow" runat="server" ClientIDMode="Static" CssClass="txtTotal"
                                                ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtmmnCF_TyreInflow" runat="server" ClientIDMode="Static" CssClass="txtRightAlign"
                                                MaxLength="6" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtmmnCF_RimInflow" runat="server" ClientIDMode="Static" CssClass="txtRightAlign"
                                                MaxLength="6" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtmmnCF_PneumaticsInflow" runat="server" ClientIDMode="Static"
                                                CssClass="txtRightAlign" MaxLength="6" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtmmnCF_TotalInflow" runat="server" ClientIDMode="Static" CssClass="txtTotal"
                                                ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtpdkCF_TyreInflow" runat="server" ClientIDMode="Static" CssClass="txtRightAlign"
                                                MaxLength="6" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtpdkCF_RimInflow" runat="server" ClientIDMode="Static" CssClass="txtRightAlign"
                                                MaxLength="6" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtpdkCF_PneumaticsInflow" runat="server" ClientIDMode="Static"
                                                CssClass="txtRightAlign" MaxLength="6" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtpdkCF_TotalInflow" runat="server" ClientIDMode="Static" CssClass="txtTotal"
                                                ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="background-color: #d7f785;">
                                            <asp:TextBox ID="txtGrandTotal_TyreInflow" runat="server" ClientIDMode="Static" CssClass="txtTotal"
                                                ReadOnly="true"></asp:TextBox>
                                        </td>
                                        <td style="background-color: #d7f785;">
                                            <asp:TextBox ID="txtGrandTotal_RimInflow" runat="server" ClientIDMode="Static" CssClass="txtTotal"
                                                ReadOnly="true"></asp:TextBox>
                                        </td>
                                        <td style="background-color: #d7f785;">
                                            <asp:TextBox ID="txtGrandTotal_PneumaticsInflow" runat="server" ClientIDMode="Static"
                                                CssClass="txtTotal" ReadOnly="true"></asp:TextBox>
                                        </td>
                                        <td style="background-color: #d7f785;">
                                            <asp:TextBox ID="txtGrandTotal_TotalInflow" runat="server" ClientIDMode="Static"
                                                CssClass="txtTotal" ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="vertical-align: top;">
                                <table cellspacing="0" rules="all" border="1" class="tbDespatch">
                                    <tr>
                                        <th>
                                            SOLID TYRE
                                        </th>
                                        <th>
                                            RIM
                                        </th>
                                        <th>
                                            PNEUMATICS
                                        </th>
                                        <th>
                                            TOTAL
                                        </th>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtLankaExp_TyreDespatch" runat="server" ClientIDMode="Static" CssClass="txtRightAlign"
                                                MaxLength="6" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLankaExp_RimDespatch" runat="server" ClientIDMode="Static" CssClass="txtRightAlign"
                                                MaxLength="6" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLankaExp_PneumaticsDespatch" runat="server" ClientIDMode="Static"
                                                CssClass="txtRightAlign" MaxLength="6" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLankaExp_TotalDespatch" runat="server" ClientIDMode="Static"
                                                CssClass="txtTotal" ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtStarcoExp_TyreDespatch" runat="server" ClientIDMode="Static"
                                                CssClass="txtRightAlign" MaxLength="6" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtStarcoExp_RimDespatch" runat="server" ClientIDMode="Static" CssClass="txtRightAlign"
                                                MaxLength="6" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtStarcoExp_PneumaticsDespatch" runat="server" ClientIDMode="Static"
                                                CssClass="txtRightAlign" MaxLength="6" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtStarcoExp_TotalDespatch" runat="server" ClientIDMode="Static"
                                                CssClass="txtTotal" ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtmmnExp_TyreDespatch" runat="server" ClientIDMode="Static" CssClass="txtRightAlign"
                                                MaxLength="6" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtmmnExp_RimDespatch" runat="server" ClientIDMode="Static" CssClass="txtRightAlign"
                                                MaxLength="6" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtmmnExp_PneumaticsDespatch" runat="server" ClientIDMode="Static"
                                                CssClass="txtRightAlign" MaxLength="6" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtmmnExp_TotalDespatch" runat="server" ClientIDMode="Static" CssClass="txtTotal"
                                                ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtpdkExp_TyreDespatch" runat="server" ClientIDMode="Static" CssClass="txtRightAlign"
                                                MaxLength="6" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtpdkExp_RimDespatch" runat="server" ClientIDMode="Static" CssClass="txtRightAlign"
                                                MaxLength="6" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtpdkExp_PneumaticsDespatch" runat="server" ClientIDMode="Static"
                                                CssClass="txtRightAlign" MaxLength="6" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtpdkExp_TotalDespatch" runat="server" ClientIDMode="Static" CssClass="txtTotal"
                                                ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtmmnDom_TyreDespatch" runat="server" ClientIDMode="Static" CssClass="txtRightAlign"
                                                MaxLength="6" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtmmnDom_RimDespatch" runat="server" ClientIDMode="Static" CssClass="txtRightAlign"
                                                MaxLength="6" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtmmnDom_PneumaticsDespatch" runat="server" ClientIDMode="Static"
                                                CssClass="txtRightAlign" MaxLength="6" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtmmnDom_TotalDespatch" runat="server" ClientIDMode="Static" CssClass="txtTotal"
                                                ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtpdkDom_TyreDespatch" runat="server" ClientIDMode="Static" CssClass="txtRightAlign"
                                                MaxLength="6" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtpdkDom_RimDespatch" runat="server" ClientIDMode="Static" CssClass="txtRightAlign"
                                                MaxLength="6" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtpdkDom_PneumaticsDespatch" runat="server" ClientIDMode="Static"
                                                CssClass="txtRightAlign" MaxLength="6" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtpdkDom_TotalDespatch" runat="server" ClientIDMode="Static" CssClass="txtTotal"
                                                ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtmmnCF_TyreDespatch" runat="server" ClientIDMode="Static" CssClass="txtRightAlign"
                                                MaxLength="6" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtmmnCF_RimDespatch" runat="server" ClientIDMode="Static" CssClass="txtRightAlign"
                                                MaxLength="6" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtmmnCF_PneumaticsDespatch" runat="server" ClientIDMode="Static"
                                                CssClass="txtRightAlign" MaxLength="6" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtmmnCF_TotalDespatch" runat="server" ClientIDMode="Static" CssClass="txtTotal"
                                                ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtpdkCF_TyreDespatch" runat="server" ClientIDMode="Static" CssClass="txtRightAlign"
                                                MaxLength="6" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtpdkCF_RimDespatch" runat="server" ClientIDMode="Static" CssClass="txtRightAlign"
                                                MaxLength="6" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtpdkCF_PneumaticsDespatch" runat="server" ClientIDMode="Static"
                                                CssClass="txtRightAlign" MaxLength="6" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtpdkCF_TotalDespatch" runat="server" ClientIDMode="Static" CssClass="txtTotal"
                                                ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="background-color: #fcedc7;">
                                            <asp:TextBox ID="txtGrandTotal_TyreDespatch" runat="server" ClientIDMode="Static"
                                                CssClass="txtTotal" ReadOnly="true"></asp:TextBox>
                                        </td>
                                        <td style="background-color: #fcedc7;">
                                            <asp:TextBox ID="txtGrandTotal_RimDespatch" runat="server" ClientIDMode="Static"
                                                CssClass="txtTotal" ReadOnly="true"></asp:TextBox>
                                        </td>
                                        <td style="background-color: #fcedc7;">
                                            <asp:TextBox ID="txtGrandTotal_PneumaticsDespatch" runat="server" ClientIDMode="Static"
                                                CssClass="txtTotal" ReadOnly="true"></asp:TextBox>
                                        </td>
                                        <td style="background-color: #fcedc7;">
                                            <asp:TextBox ID="txtGrandTotal_TotalDespatch" runat="server" ClientIDMode="Static"
                                                CssClass="txtTotal" ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="vertical-align: top;">
                                <table cellspacing="0" rules="all" border="1" class="tbBacklog">
                                    <tr>
                                        <th>
                                            SOLID TYRE
                                        </th>
                                        <th>
                                            RIM
                                        </th>
                                        <th>
                                            PNEUMATICS
                                        </th>
                                        <th>
                                            TOTAL
                                        </th>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtLankaExp_TyreBacklog" runat="server" ClientIDMode="Static" CssClass="txtTotal"></asp:TextBox>
                                            <asp:HiddenField runat="server" ID="hdnLankaExpTyre" ClientIDMode="Static" Value="" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLankaExp_RimBacklog" runat="server" ClientIDMode="Static" CssClass="txtTotal"></asp:TextBox>
                                            <asp:HiddenField runat="server" ID="hdnLankaExpRim" ClientIDMode="Static" Value="" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLankaExp_PneumaticsBacklog" runat="server" ClientIDMode="Static"
                                                CssClass="txtTotal"></asp:TextBox>
                                            <asp:HiddenField runat="server" ID="hdnLankaExpPneu" ClientIDMode="Static" Value="" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLankaExp_TotalBacklog" runat="server" ClientIDMode="Static" CssClass="txtTotal"
                                                ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtStarcoExp_TyreBacklog" runat="server" ClientIDMode="Static" CssClass="txtTotal"></asp:TextBox>
                                            <asp:HiddenField runat="server" ID="hdnStarcoExpTyre" ClientIDMode="Static" Value="" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtStarcoExp_RimBacklog" runat="server" ClientIDMode="Static" CssClass="txtTotal"></asp:TextBox>
                                            <asp:HiddenField runat="server" ID="hdnStarcoExpRim" ClientIDMode="Static" Value="" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtStarcoExp_PneumaticsBacklog" runat="server" ClientIDMode="Static"
                                                CssClass="txtTotal"></asp:TextBox>
                                            <asp:HiddenField runat="server" ID="hdnStarcoExpPneu" ClientIDMode="Static" Value="" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtStarcoExp_TotalBacklog" runat="server" ClientIDMode="Static"
                                                CssClass="txtTotal" ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtmmnExp_TyreBacklog" runat="server" ClientIDMode="Static" CssClass="txtTotal"></asp:TextBox>
                                            <asp:HiddenField runat="server" ID="hdnmmnExpTyre" ClientIDMode="Static" Value="" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtmmnExp_RimBacklog" runat="server" ClientIDMode="Static" CssClass="txtTotal"></asp:TextBox>
                                            <asp:HiddenField runat="server" ID="hdnmmnExpRim" ClientIDMode="Static" Value="" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtmmnExp_PneumaticsBacklog" runat="server" ClientIDMode="Static"
                                                CssClass="txtTotal"></asp:TextBox>
                                            <asp:HiddenField runat="server" ID="hdnmmnExpPneu" ClientIDMode="Static" Value="" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtmmnExp_TotalBacklog" runat="server" ClientIDMode="Static" CssClass="txtTotal"
                                                ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtpdkExp_TyreBacklog" runat="server" ClientIDMode="Static" CssClass="txtTotal"></asp:TextBox>
                                            <asp:HiddenField runat="server" ID="hdnpdkExpTyre" ClientIDMode="Static" Value="" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtpdkExp_RimBacklog" runat="server" ClientIDMode="Static" CssClass="txtTotal"></asp:TextBox>
                                            <asp:HiddenField runat="server" ID="hdnpdkExpRim" ClientIDMode="Static" Value="" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtpdkExp_PneumaticsBacklog" runat="server" ClientIDMode="Static"
                                                CssClass="txtTotal"></asp:TextBox>
                                            <asp:HiddenField runat="server" ID="hdnpdkExpPneu" ClientIDMode="Static" Value="" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtpdkExp_TotalBacklog" runat="server" ClientIDMode="Static" CssClass="txtTotal"
                                                ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtmmnDom_TyreBacklog" runat="server" ClientIDMode="Static" CssClass="txtTotal"></asp:TextBox>
                                            <asp:HiddenField runat="server" ID="hdnmmnDomTyre" ClientIDMode="Static" Value="" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtmmnDom_RimBacklog" runat="server" ClientIDMode="Static" CssClass="txtTotal"></asp:TextBox>
                                            <asp:HiddenField runat="server" ID="hdnmmnDomRim" ClientIDMode="Static" Value="" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtmmnDom_PneumaticsBacklog" runat="server" ClientIDMode="Static"
                                                CssClass="txtTotal"></asp:TextBox>
                                            <asp:HiddenField runat="server" ID="hdnmmnDomPneu" ClientIDMode="Static" Value="" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtmmnDom_TotalBacklog" runat="server" ClientIDMode="Static" CssClass="txtTotal"
                                                ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtpdkDom_TyreBacklog" runat="server" ClientIDMode="Static" CssClass="txtTotal"></asp:TextBox>
                                            <asp:HiddenField runat="server" ID="hdnpdkDomTyre" ClientIDMode="Static" Value="" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtpdkDom_RimBacklog" runat="server" ClientIDMode="Static" CssClass="txtTotal"></asp:TextBox>
                                            <asp:HiddenField runat="server" ID="hdnpdkDomRim" ClientIDMode="Static" Value="" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtpdkDom_PneumaticsBacklog" runat="server" ClientIDMode="Static"
                                                CssClass="txtTotal"></asp:TextBox>
                                            <asp:HiddenField runat="server" ID="hdnpdkDomPneu" ClientIDMode="Static" Value="" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtpdkDom_TotalBacklog" runat="server" ClientIDMode="Static" CssClass="txtTotal"
                                                ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtmmnCF_TyreBacklog" runat="server" ClientIDMode="Static" CssClass="txtTotal"></asp:TextBox>
                                            <asp:HiddenField runat="server" ID="hdnmmnCFTyre" ClientIDMode="Static" Value="" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtmmnCF_RimBacklog" runat="server" ClientIDMode="Static" CssClass="txtTotal"></asp:TextBox>
                                            <asp:HiddenField runat="server" ID="hdnmmnCFRim" ClientIDMode="Static" Value="" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtmmnCF_PneumaticsBacklog" runat="server" ClientIDMode="Static"
                                                CssClass="txtTotal"></asp:TextBox>
                                            <asp:HiddenField runat="server" ID="hdnmmnCFPneu" ClientIDMode="Static" Value="" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtmmnCF_TotalBacklog" runat="server" ClientIDMode="Static" CssClass="txtTotal"
                                                ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtpdkCF_TyreBacklog" runat="server" ClientIDMode="Static" CssClass="txtTotal"></asp:TextBox>
                                            <asp:HiddenField runat="server" ID="hdnpdkCFTyre" ClientIDMode="Static" Value="" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtpdkCF_RimBacklog" runat="server" ClientIDMode="Static" CssClass="txtTotal"></asp:TextBox>
                                            <asp:HiddenField runat="server" ID="hdnpdkCFRim" ClientIDMode="Static" Value="" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtpdkCF_PneumaticsBacklog" runat="server" ClientIDMode="Static"
                                                CssClass="txtTotal"></asp:TextBox>
                                            <asp:HiddenField runat="server" ID="hdnpdkCFPneu" ClientIDMode="Static" Value="" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtpdkCF_TotalBacklog" runat="server" ClientIDMode="Static" CssClass="txtTotal"
                                                ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="background-color: #fcd7d7;">
                                            <asp:TextBox ID="txtGrandTotal_TyreBacklog" runat="server" ClientIDMode="Static"
                                                CssClass="txtTotal" ReadOnly="true"></asp:TextBox>
                                        </td>
                                        <td style="background-color: #fcd7d7;">
                                            <asp:TextBox ID="txtGrandTotal_RimBacklog" runat="server" ClientIDMode="Static" CssClass="txtTotal"
                                                ReadOnly="true"></asp:TextBox>
                                        </td>
                                        <td style="background-color: #fcd7d7;">
                                            <asp:TextBox ID="txtGrandTotal_PneumaticsBacklog" runat="server" ClientIDMode="Static"
                                                CssClass="txtTotal" ReadOnly="true"></asp:TextBox>
                                        </td>
                                        <td style="background-color: #fcd7d7;">
                                            <asp:TextBox ID="txtGrandTotal_TotalBacklog" runat="server" ClientIDMode="Static"
                                                CssClass="txtTotal" ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr style="text-align: center;">
                            <td>
                                <asp:Button ID="btnclear" Text="CLEAR" runat="server" ForeColor="#ffffff" BackColor="#d42222"
                                    ClientIDMode="Static" Font-Bold="true" Font-Size="16px" OnClick="btnclear_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnSave" Text="SAVE" runat="server" ForeColor="#ffffff" BackColor="#155267"
                                    ClientIDMode="Static" Font-Bold="true" Font-Size="16px" OnClientClick="javascript:return btnsavecheck();"
                                    OnClick="btnSave_Click" />
                            </td>
                            <td style="text-align: center;">
                                <asp:Label runat="server" ID="lblPreviousEntry" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                                <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" Font-Bold="true"
                                    ForeColor="Red"></asp:Label>
                                <asp:Button ID="btnSavepreviosMonth" runat="server" ForeColor="#000000" Text="" BackColor="#378de5"
                                    ClientIDMode="Static" Font-Size="16px" Font-Bold="true" OnClientClick="javascript:return btnsavecheck();"
                                    OnClick="btnSavepreviosMonth_Click" />
                            </td>
                            <td style="text-align: center;">
                                <asp:Label runat="server" ID="lblBacklogEntry" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                                <asp:Button ID="btnSaveBacklog" runat="server" ForeColor="#000000" Text="" BackColor="#ffab23"
                                    ClientIDMode="Static" Font-Size="16px" Font-Bold="true" OnClientClick="javascript:return btnsavebacklogcheck();"
                                    OnClick="btnSaveBacklog_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Repeater runat="server" ID="rptMonthRecord" ClientIDMode="Static">
                        <ItemTemplate>
                            <table cellspacing="0" rules="all" border="1" class="tbMainMonth">
                                <tr style="color: #ffffff; font-size: 18px;">
                                    <th>
                                        <%# Eval("AsMonth")%>
                                        -
                                        <%# Eval("AsYear")%>
                                    </th>
                                    <th>
                                        INFLOW
                                    </th>
                                    <th>
                                        DESPATCH
                                    </th>
                                    <th>
                                        BACKLOG
                                    </th>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top;">
                                        <table cellspacing="0" rules="all" border="1" class="tbHeadMonth">
                                            <tr>
                                                <th style="line-height: 25px;">
                                                    TYPE
                                                </th>
                                            </tr>
                                            <tr>
                                                <th>
                                                    SLTL (EXPORT)
                                                </th>
                                            </tr>
                                            <tr>
                                                <th>
                                                    SITL (EXPORT)
                                                </th>
                                            </tr>
                                            <tr>
                                                <th>
                                                    MMN (EXPORT)
                                                </th>
                                            </tr>
                                            <tr>
                                                <th>
                                                    PDK (EXPORT)
                                                </th>
                                            </tr>
                                            <tr>
                                                <th>
                                                    MMN (DOMESTIC)
                                                </th>
                                            </tr>
                                            <tr>
                                                <th>
                                                    PDK (DOMESTIC)
                                                </th>
                                            </tr>
                                            <tr>
                                                <th>
                                                    MMN (C&F)
                                                </th>
                                            </tr>
                                            <tr>
                                                <th>
                                                    PDK (C&F)
                                                </th>
                                            </tr>
                                            <tr>
                                                <th style="background-color: #631009; color: #ffffff; font-weight: bold; font-size: 14px;">
                                                    GRAND TOTAL
                                                </th>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="vertical-align: top;">
                                        <table cellspacing="0" rules="all" border="1" class="tbInflowMonth">
                                            <tr>
                                                <th>
                                                    SOLID TYRE
                                                </th>
                                                <th>
                                                    RIM
                                                </th>
                                                <th>
                                                    PNEUMATICS
                                                </th>
                                                <th>
                                                    TOTAL
                                                </th>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <%# Eval("LankaExp_TyreInflow")%>
                                                </td>
                                                <td>
                                                    <%# Eval("LankaExp_RimInflow")%>
                                                </td>
                                                <td>
                                                    <%# Eval("LankaExp_PneumaticsInflow")%>
                                                </td>
                                                <td>
                                                    <%# Convert.ToDecimal(Eval("LankaExp_TyreInflow").ToString()) + Convert.ToDecimal(Eval("LankaExp_RimInflow").ToString()) + Convert.ToDecimal(Eval("LankaExp_PneumaticsInflow").ToString())%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <%# Eval("StarcoExp_TyreInflow")%>
                                                </td>
                                                <td>
                                                    <%# Eval("StarcoExp_RimInflow")%>
                                                </td>
                                                <td>
                                                    <%# Eval("StarcoExp_PneumaticsInflow")%>
                                                </td>
                                                <td>
                                                    <%# Convert.ToDecimal(Eval("StarcoExp_TyreInflow").ToString()) + Convert.ToDecimal(Eval("StarcoExp_RimInflow").ToString()) + Convert.ToDecimal(Eval("StarcoExp_PneumaticsInflow").ToString())%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <%# Eval("mmnExp_TyreInflow")%>
                                                </td>
                                                <td>
                                                    <%# Eval("mmnExp_RimInflow")%>
                                                </td>
                                                <td>
                                                    <%# Eval("mmnExp_PneumaticsInflow")%>
                                                </td>
                                                <td>
                                                    <%# Convert.ToDecimal(Eval("mmnExp_TyreInflow").ToString()) + Convert.ToDecimal(Eval("mmnExp_RimInflow").ToString()) + Convert.ToDecimal(Eval("mmnExp_PneumaticsInflow").ToString())%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <%# Eval("pdkExp_TyreInflow")%>
                                                </td>
                                                <td>
                                                    <%# Eval("pdkExp_RimInflow")%>
                                                </td>
                                                <td>
                                                    <%# Eval("pdkExp_PneumaticsInflow")%>
                                                </td>
                                                <td>
                                                    <%# Convert.ToDecimal(Eval("pdkExp_TyreInflow").ToString()) + Convert.ToDecimal(Eval("pdkExp_RimInflow").ToString()) + Convert.ToDecimal(Eval("pdkExp_PneumaticsInflow").ToString())%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <%# Eval("mmnDom_TyreInflow")%>
                                                </td>
                                                <td>
                                                    <%# Eval("mmnDom_RimInflow")%>
                                                </td>
                                                <td>
                                                    <%# Eval("mmnDom_PneumaticsInflow")%>
                                                </td>
                                                <td>
                                                    <%# Convert.ToDecimal(Eval("mmnDom_TyreInflow").ToString()) + Convert.ToDecimal(Eval("mmnDom_RimInflow").ToString()) + Convert.ToDecimal(Eval("mmnDom_PneumaticsInflow").ToString())%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <%# Eval("pdkDom_TyreInflow")%>
                                                </td>
                                                <td>
                                                    <%# Eval("pdkDom_RimInflow")%>
                                                </td>
                                                <td>
                                                    <%# Eval("pdkDom_PneumaticsInflow")%>
                                                </td>
                                                <td>
                                                    <%# Convert.ToDecimal(Eval("pdkDom_TyreInflow").ToString()) + Convert.ToDecimal(Eval("pdkDom_RimInflow").ToString()) + Convert.ToDecimal(Eval("pdkDom_PneumaticsInflow").ToString())%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <%# Eval("mmnCF_TyreInflow")%>
                                                </td>
                                                <td>
                                                    <%# Eval("mmnCF_RimInflow")%>
                                                </td>
                                                <td>
                                                    <%# Eval("mmnCF_PneumaticsInflow")%>
                                                </td>
                                                <td>
                                                    <%# Convert.ToDecimal(Eval("mmnCF_TyreInflow").ToString()) + Convert.ToDecimal(Eval("mmnCF_RimInflow").ToString()) + Convert.ToDecimal(Eval("mmnCF_PneumaticsInflow").ToString())%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <%# Eval("pdkCF_TyreInflow")%>
                                                </td>
                                                <td>
                                                    <%# Eval("pdkCF_RimInflow")%>
                                                </td>
                                                <td>
                                                    <%# Eval("pdkCF_PneumaticsInflow")%>
                                                </td>
                                                <td>
                                                    <%# Convert.ToDecimal(Eval("pdkCF_TyreInflow").ToString()) + Convert.ToDecimal(Eval("pdkCF_RimInflow").ToString()) + Convert.ToDecimal(Eval("pdkCF_PneumaticsInflow").ToString())%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="background-color: #e0d6ff; font-size: 16px;">
                                                    <%# Eval("InflowTyreTotal")%>
                                                </td>
                                                <td style="background-color: #e0d6ff; font-size: 16px;">
                                                    <%# Eval("InflowRimTotal")%>
                                                </td>
                                                <td style="background-color: #e0d6ff; font-size: 16px;">
                                                    <%# Eval("InflowPneuTotal")%>
                                                </td>
                                                <td style="background-color: #fbb2de; font-size: 20px;">
                                                    <%# Convert.ToDecimal(Eval("InflowTyreTotal").ToString()) + Convert.ToDecimal(Eval("InflowRimTotal").ToString()) + Convert.ToDecimal(Eval("InflowPneuTotal").ToString())%>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="vertical-align: top;">
                                        <table cellspacing="0" rules="all" border="1" class="tbDespatchMonth">
                                            <tr>
                                                <th>
                                                    SOLID TYRE
                                                </th>
                                                <th>
                                                    RIM
                                                </th>
                                                <th>
                                                    PNEUMATICS
                                                </th>
                                                <th>
                                                    TOTAL
                                                </th>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <%# Eval("LankaExp_TyreDespatch")%>
                                                </td>
                                                <td>
                                                    <%# Eval("LankaExp_RimDespatch")%>
                                                </td>
                                                <td>
                                                    <%# Eval("LankaExp_PneumaticsDespatch")%>
                                                </td>
                                                <td>
                                                    <%# Convert.ToDecimal(Eval("LankaExp_TyreDespatch").ToString()) + Convert.ToDecimal(Eval("LankaExp_RimDespatch").ToString()) + Convert.ToDecimal(Eval("LankaExp_PneumaticsDespatch").ToString())%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <%# Eval("StarcoExp_TyreDespatch")%>
                                                </td>
                                                <td>
                                                    <%# Eval("StarcoExp_RimDespatch")%>
                                                </td>
                                                <td>
                                                    <%# Eval("StarcoExp_PneumaticsDespatch")%>
                                                </td>
                                                <td>
                                                    <%# Convert.ToDecimal(Eval("StarcoExp_TyreDespatch").ToString()) + Convert.ToDecimal(Eval("StarcoExp_RimDespatch").ToString()) + Convert.ToDecimal(Eval("StarcoExp_PneumaticsDespatch").ToString())%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <%# Eval("mmnExp_TyreDespatch")%>
                                                </td>
                                                <td>
                                                    <%# Eval("mmnExp_RimDespatch")%>
                                                </td>
                                                <td>
                                                    <%# Eval("mmnExp_PneumaticsDespatch")%>
                                                </td>
                                                <td>
                                                    <%# Convert.ToDecimal(Eval("mmnExp_TyreDespatch").ToString()) + Convert.ToDecimal(Eval("mmnExp_RimDespatch").ToString()) + Convert.ToDecimal(Eval("mmnExp_PneumaticsDespatch").ToString())%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <%# Eval("pdkExp_TyreDespatch")%>
                                                </td>
                                                <td>
                                                    <%# Eval("pdkExp_RimDespatch")%>
                                                </td>
                                                <td>
                                                    <%# Eval("pdkExp_PneumaticsDespatch")%>
                                                </td>
                                                <td>
                                                    <%# Convert.ToDecimal(Eval("pdkExp_TyreDespatch").ToString()) + Convert.ToDecimal(Eval("pdkExp_RimDespatch").ToString()) + Convert.ToDecimal(Eval("pdkExp_PneumaticsDespatch").ToString())%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <%# Eval("mmnDom_TyreDespatch")%>
                                                </td>
                                                <td>
                                                    <%# Eval("mmnDom_RimDespatch")%>
                                                </td>
                                                <td>
                                                    <%# Eval("mmnDom_PneumaticsDespatch")%>
                                                </td>
                                                <td>
                                                    <%# Convert.ToDecimal(Eval("mmnDom_TyreDespatch").ToString()) + Convert.ToDecimal(Eval("mmnDom_RimDespatch").ToString()) + Convert.ToDecimal(Eval("mmnDom_PneumaticsDespatch").ToString())%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <%# Eval("pdkDom_TyreDespatch")%>
                                                </td>
                                                <td>
                                                    <%# Eval("pdkDom_RimDespatch")%>
                                                </td>
                                                <td>
                                                    <%# Eval("pdkDom_PneumaticsDespatch")%>
                                                </td>
                                                <td>
                                                    <%# Convert.ToDecimal(Eval("pdkDom_TyreDespatch").ToString()) + Convert.ToDecimal(Eval("pdkDom_RimDespatch").ToString()) + Convert.ToDecimal(Eval("pdkDom_PneumaticsDespatch").ToString())%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <%# Eval("mmnCF_TyreDespatch")%>
                                                </td>
                                                <td>
                                                    <%# Eval("mmnCF_RimDespatch")%>
                                                </td>
                                                <td>
                                                    <%# Eval("mmnCF_PneumaticsDespatch")%>
                                                </td>
                                                <td>
                                                    <%# Convert.ToDecimal(Eval("mmnCF_TyreDespatch").ToString()) + Convert.ToDecimal(Eval("mmnCF_RimDespatch").ToString()) + Convert.ToDecimal(Eval("mmnCF_PneumaticsDespatch").ToString())%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <%# Eval("pdkCF_TyreDespatch")%>
                                                </td>
                                                <td>
                                                    <%# Eval("pdkCF_RimDespatch")%>
                                                </td>
                                                <td>
                                                    <%# Eval("pdkCF_PneumaticsDespatch")%>
                                                </td>
                                                <td>
                                                    <%# Convert.ToDecimal(Eval("pdkCF_TyreDespatch").ToString()) + Convert.ToDecimal(Eval("pdkCF_RimDespatch").ToString()) + Convert.ToDecimal(Eval("pdkCF_PneumaticsDespatch").ToString())%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="background-color: #fcedc7; font-size: 16px;">
                                                    <%# Eval("DespatchTyreTotal")%>
                                                </td>
                                                <td style="background-color: #fcedc7; font-size: 16px;">
                                                    <%# Eval("DespatchRimTotal")%>
                                                </td>
                                                <td style="background-color: #fcedc7; font-size: 16px;">
                                                    <%# Eval("DespatchPneuTotal")%>
                                                </td>
                                                <td style="background-color: #b8d3fd; font-size: 20px;">
                                                    <%# Convert.ToDecimal(Eval("DespatchTyreTotal").ToString()) + Convert.ToDecimal(Eval("DespatchRimTotal").ToString()) + Convert.ToDecimal(Eval("DespatchPneuTotal").ToString())%>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="vertical-align: top;">
                                        <table cellspacing="0" rules="all" border="1" class="tbBacklogMonth">
                                            <tr>
                                                <th>
                                                    SOLID TYRE
                                                </th>
                                                <th>
                                                    RIM
                                                </th>
                                                <th>
                                                    PNEUMATICS
                                                </th>
                                                <th>
                                                    TOTAL
                                                </th>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <%# Eval("LankaExp_TyreBacklog")%>
                                                </td>
                                                <td>
                                                    <%# Eval("LankaExp_RimBacklog")%>
                                                </td>
                                                <td>
                                                    <%# Eval("LankaExp_PneumaticsBacklog")%>
                                                </td>
                                                <td>
                                                    <%# Convert.ToDecimal(Eval("LankaExp_TyreBacklog").ToString()) + Convert.ToDecimal(Eval("LankaExp_RimBacklog").ToString()) + Convert.ToDecimal(Eval("LankaExp_PneumaticsBacklog").ToString())%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <%# Eval("StarcoExp_TyreBacklog")%>
                                                </td>
                                                <td>
                                                    <%# Eval("StarcoExp_RimBacklog")%>
                                                </td>
                                                <td>
                                                    <%# Eval("StarcoExp_PneumaticsBacklog")%>
                                                </td>
                                                <td>
                                                    <%# Convert.ToDecimal(Eval("StarcoExp_TyreBacklog").ToString()) + Convert.ToDecimal(Eval("StarcoExp_RimBacklog").ToString()) + Convert.ToDecimal(Eval("StarcoExp_PneumaticsBacklog").ToString())%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <%# Eval("mmnExp_TyreBacklog")%>
                                                </td>
                                                <td>
                                                    <%# Eval("mmnExp_RimBacklog")%>
                                                </td>
                                                <td>
                                                    <%# Eval("mmnExp_PneumaticsBacklog")%>
                                                </td>
                                                <td>
                                                    <%# Convert.ToDecimal(Eval("mmnExp_TyreBacklog").ToString()) + Convert.ToDecimal(Eval("mmnExp_RimBacklog").ToString()) + Convert.ToDecimal(Eval("mmnExp_PneumaticsBacklog").ToString())%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <%# Eval("pdkExp_TyreBacklog")%>
                                                </td>
                                                <td>
                                                    <%# Eval("pdkExp_RimBacklog")%>
                                                </td>
                                                <td>
                                                    <%# Eval("pdkExp_PneumaticsBacklog")%>
                                                </td>
                                                <td>
                                                    <%# Convert.ToDecimal(Eval("pdkExp_TyreBacklog").ToString()) + Convert.ToDecimal(Eval("pdkExp_RimBacklog").ToString()) + Convert.ToDecimal(Eval("pdkExp_PneumaticsBacklog").ToString())%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <%# Eval("mmnDom_TyreBacklog")%>
                                                </td>
                                                <td>
                                                    <%# Eval("mmnDom_RimBacklog")%>
                                                </td>
                                                <td>
                                                    <%# Eval("mmnDom_PneumaticsBacklog")%>
                                                </td>
                                                <td>
                                                    <%# Convert.ToDecimal(Eval("mmnDom_TyreBacklog").ToString()) + Convert.ToDecimal(Eval("mmnDom_RimBacklog").ToString()) + Convert.ToDecimal(Eval("mmnDom_PneumaticsBacklog").ToString())%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <%# Eval("pdkDom_TyreBacklog")%>
                                                </td>
                                                <td>
                                                    <%# Eval("pdkDom_RimBacklog")%>
                                                </td>
                                                <td>
                                                    <%# Eval("pdkDom_PneumaticsBacklog")%>
                                                </td>
                                                <td>
                                                    <%# Convert.ToDecimal(Eval("pdkDom_TyreBacklog").ToString()) + Convert.ToDecimal(Eval("pdkDom_RimBacklog").ToString()) + Convert.ToDecimal(Eval("pdkDom_PneumaticsBacklog").ToString())%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <%# Eval("mmnCF_TyreBacklog")%>
                                                </td>
                                                <td>
                                                    <%# Eval("mmnCF_RimBacklog")%>
                                                </td>
                                                <td>
                                                    <%# Eval("mmnCF_PneumaticsBacklog")%>
                                                </td>
                                                <td>
                                                    <%# Convert.ToDecimal(Eval("mmnCF_TyreBacklog").ToString()) + Convert.ToDecimal(Eval("mmnCF_RimBacklog").ToString()) + Convert.ToDecimal(Eval("mmnCF_PneumaticsBacklog").ToString())%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <%# Eval("pdkCF_TyreBacklog")%>
                                                </td>
                                                <td>
                                                    <%# Eval("pdkCF_RimBacklog")%>
                                                </td>
                                                <td>
                                                    <%# Eval("pdkCF_PneumaticsBacklog")%>
                                                </td>
                                                <td>
                                                    <%# Convert.ToDecimal(Eval("pdkCF_TyreBacklog").ToString()) + Convert.ToDecimal(Eval("pdkCF_RimBacklog").ToString()) + Convert.ToDecimal(Eval("pdkCF_PneumaticsBacklog").ToString())%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="background-color: #ffdbe0; font-size: 16px;">
                                                    <%# Eval("BacklogTyreTotal")%>
                                                </td>
                                                <td style="background-color: #ffdbe0; font-size: 16px;">
                                                    <%# Eval("BacklogRimTotal")%>
                                                </td>
                                                <td style="background-color: #ffdbe0; font-size: 16px;">
                                                    <%# Eval("BacklogPneuTotal")%>
                                                </td>
                                                <td style="background-color: #b1fdc5; font-size: 20px;">
                                                    <%# Convert.ToDecimal(Eval("BacklogTyreTotal").ToString()) + Convert.ToDecimal(Eval("BacklogRimTotal").ToString()) + Convert.ToDecimal(Eval("BacklogPneuTotal").ToString())%>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
            </tr>
        </table>
    </div>
    <!--Backlog_DB-->
    <script type="text/javascript">
        $(document).ready(function () {
            $(':text').bind('keydown', function (e) { if (e.keyCode == 13) { e.preventDefault(); return false; } });
            for (var z = 1; z <= 8; z++) {
                for (var y = 0; y < 3; y++) {
                    $('.tbBacklog tr:eq(' + z + ')').find('td:eq(' + y + ') input[type="text"]').attr('readonly', true).css('border', '2px inset initial');
                }
            }
            $('.txtRightAlign').blur(function () {
                BacklogCalc();
                SubTotal('tbInflow');
                GrandTotal('tbInflow', 'txtGrandTotal_TotalInflow');
                SubTotal('tbDespatch');
                GrandTotal('tbDespatch', 'txtGrandTotal_TotalDespatch');
                SubTotal('tbBacklog');
                GrandTotal('tbBacklog', 'txtGrandTotal_TotalBacklog');
            });
            $('.txtTotal').blur(function () {
                SubTotal('tbBacklog');
                GrandTotal('tbBacklog', 'txtGrandTotal_TotalBacklog');
            });
        });

        function backlogEntryEnable() {
            for (var z = 1; z <= 8; z++) {
                for (var y = 0; y < 3; y++) {
                    $('.tbBacklog tr:eq(' + z + ')').find('td:eq(' + y + ') input[type="text"]').attr('readonly', false).css('border', '1px solid #000');
                }
            }
            $('.tbInflow').find('input[type="text"]').attr('readonly', true).css('border', '0px solid #fff');
            $('.tbDespatch').find('input[type="text"]').attr('readonly', true).css('border', '0px solid #fff');
        }

        function BacklogCalc() {
            var inflowID = ''; var despatchID = ''; var backlogID = ''; var backlogHiddenID = '';
            for (var z = 1; z <= 8; z++) {
                for (var y = 0; y < 3; y++) {
                    var inflowVal = 0; var despatchVal = 0; var backlogVal = 0;
                    inflowID = $('.tbInflow tr:eq(' + z + ')').find('td:eq(' + y + ') input[type="text"]').attr('id');
                    despatchID = $('.tbDespatch tr:eq(' + z + ')').find('td:eq(' + y + ') input[type="text"]').attr('id');
                    backlogID = $('.tbBacklog tr:eq(' + z + ')').find('td:eq(' + y + ') input[type="text"]').attr('id');
                    backlogHiddenID = $('.tbBacklog tr:eq(' + z + ')').find('td:eq(' + y + ') input[type="hidden"]').attr('id');
                    inflowVal = $('#' + inflowID).val() != '' ? Number($('#' + inflowID).val()) : 0;
                    despatchVal = $('#' + despatchID).val() != '' ? Number($('#' + despatchID).val()) : 0;
                    backlogVal = $('#' + backlogHiddenID).val() != '' ? Number($('#' + backlogHiddenID).val()) : 0;
                    $('#' + backlogID).val((backlogVal + inflowVal - despatchVal).toFixed(2));
                }
            }
        }

        function GrandTotal(cssname, txtid) {
            var grandTot = 0; var txtVal = 0; var readonlyTxt = ''; var totalVal = 0;
            for (var z = 0; z <= 3; z++) {
                grandTot = 0; txtVal = 0; readonlyTxt = '';
                $('.' + cssname + ' tr').find('td:eq(' + z + ') input[type="text"]').each(function (eve) {
                    if ($('#' + this.id).attr('readonly') != 'readonly') {
                        txtVal = $('#' + this.id).val() != "" ? $('#' + this.id).val() : 0;
                        grandTot += Number(txtVal);
                    }
                    else
                        readonlyTxt = this.id;
                });
                $('#' + readonlyTxt).val(grandTot.toFixed(2));
                totalVal += grandTot;
            }
            $('#' + txtid).val(totalVal.toFixed(2));
        }

        function SubTotal(tbCssName) {
            var subTot = 0; var txtVal = 0; var readonlyTxt = '';
            for (var z = 1; z <= 8; z++) {
                subTot = 0; txtVal = 0; readonlyTxt = '';
                $('.' + tbCssName + ' tr:eq(' + z + ')').find('input[type="text"]').each(function (eve) {
                    if ($('#' + this.id).attr('readonly') != 'readonly') {
                        txtVal = $('#' + this.id).val() != "" ? $('#' + this.id).val() : 0;
                        subTot += Number(txtVal);
                    }
                    else
                        readonlyTxt = this.id;
                });
                $('#' + readonlyTxt).val(subTot.toFixed(2));
            }
        }

        function btnsavecheck() {
            $('#lblErrMsg').html(''); var errmsg = ""; var inflowVal = false; var despatchVal = false;
            $('.tbInflow').find('input[type="text"]').each(function () { if ($('#' + this.id).attr('readonly') != 'readonly' && $('#' + this.id).val() != '') { inflowVal = true; } });
            $('.tbDespatch').find('input[type="text"]').each(function () { if ($('#' + this.id).attr('readonly') != 'readonly' && $('#' + this.id).val() != '') { despatchVal = true; } });
            if (inflowVal == false && despatchVal == false) { $('#lblErrMsg').html("Fill atleast one value"); return false; } else return true;
        }
        function btnsavebacklogcheck() {
            $('#lblErrMsg').html(''); var errmsg = ""; var backlogVal = true;
            $('.tbBacklog').find('input[type="text"]').each(function () { if ($('#' + this.id).attr('readonly') != 'readonly' && $('#' + this.id).val() == '') { backlogVal = false; } });
            if (backlogVal == false) { $('#lblErrMsg').html("Fill all backlog values"); return false; } else return true;
        }
        function openpreviousmonth() {
            $('#btnSavepreviosMonth').css({ 'display': 'block' });
            $('#lblBacklogEntry').html('');
            $('#lblPreviousEntry').html('');
        }
        function openbacklog() {
            backlogEntryEnable();
            $('#btnSaveBacklog').css({ 'display': 'block' });
            $('#lblBacklogEntry').html('');
            $('#lblPreviousEntry').html('');
        }
    </script>
</asp:Content>
