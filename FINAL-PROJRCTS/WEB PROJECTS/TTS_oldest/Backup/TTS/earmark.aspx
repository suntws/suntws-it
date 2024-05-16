<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="earmark.aspx.cs" Inherits="TTS.earmark" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #frmEarmarkPrepareItem th
        {
            font-weight: normal;
            text-align: left;
        }
        #frmEarmarkPrepareItem td
        {
            font-weight: bold;
            background-color: #b2f7f4;
        }
        .white_content
        {
            display: none;
            position: absolute;
            top: 25%;
            left: 25%;
            width: 60%;
            height: auto;
            padding: 16px;
            border: 16px solid orange;
            background-color: white;
            z-index: 1002;
        }
        .lnkCss
        {
            color: #0000ef;
            cursor: pointer;
            text-decoration: underline;
            font-weight: bold;
        }
        .tableCss
        {
            background-color: #dcecfb;
            width: 100%;
            line-height: 25px;
        }
        .tableCss th
        {
            font-size: 12px;
            color: #025a02;
            font-style: italic;
            text-align: center;
            font-weight: normal;
            line-height: 12px;
        }
        .tableCss td
        {
            line-height: 18px;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        <asp:Label runat="server" ID="lblPageHead" ClientIDMode="Static" Text=""></asp:Label>
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="background-color: #ffffff; width: 100%;
            border-color: White; border-collapse: separate;" id="tbEarmarkPage">
            <tr>
                <td>
                    <div id="div_EarmarkOrders" style="display: none;">
                        <asp:GridView ID="gv_EarmarkOrders" Width="100%" runat="server" AutoGenerateColumns="false"
                            RowStyle-Height="30px" CssClass="gridcss">
                            <HeaderStyle BackColor="#74bbf9" ForeColor="White" Font-Bold="true" Height="25px"
                                HorizontalAlign="Center" />
                            <Columns>
                                <asp:TemplateField HeaderText="CUSTOMER">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblStatusCustName" Text='<%# Eval("custfullname") %>'></asp:Label>
                                        <asp:HiddenField runat="server" ID="hdnStatusCustCode" Value='<%# Eval("CustCode") %>' />
                                        <asp:HiddenField runat="server" ID="hdnOrderID" Value='<%# Eval("ID") %>' />
                                        <asp:HiddenField runat="server" ID="hdnOrderStatus" Value='<%# Eval("OrderStatus") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ORDER REF NO">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblOrderRefNo" Text='<%#Eval("OrderRefNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="WORK ORDER" DataField="workorderno" />
                                <asp:BoundField HeaderText="ORDER QTY" DataField="itemqty" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="STATUS" DataField="StatusText" />
                                <asp:TemplateField HeaderText="ACTION" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEarmarkOrderSelection" runat="server" OnClick="lnkEarmarkOrderSelection_click"
                                            Text="PROCESS">
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="div_earkmark_OrderItems" style="display: none;">
                        <div style="width: 50%; float: left; text-align: center; background-color: #bffbc7;
                            line-height: 30px;">
                            <span class="lnkCss" onclick="showorderlist()">SHOW LIST OF ALL ORDERS FOR STENCIL ASSIGN</span>
                        </div>
                        <div style="width: 50%; float: left; text-align: center; background-color: #f2fbbf;
                            line-height: 30px;">
                            <asp:LinkButton runat="server" ID="lnkSkipEarmark" ClientIDMode="Static" Text=""
                                OnClick="lnkSkipEarmark_Click" Font-Bold="true"></asp:LinkButton>
                        </div>
                        <table cellspacing="0" rules="all" border="1" style="background-color: #ffffff; width: 100%;
                            border-color: White; border-collapse: separate;">
                            <tr style="text-align: center; background-color: #3c763d; overflow: hidden; font-size: 15px;
                                color: #ffffff; width: 100%;">
                                <td>
                                    <asp:Label ID="lblSelectedCustomerName" runat="server" Text="" Font-Bold="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblSelectedOrderRefNo" ClientIDMode="Static" runat="server" Text=""
                                        Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:GridView ID="gv_OrderItems" Width="100%" runat="server" AutoGenerateColumns="false"
                                        ShowFooter="true" FooterStyle-Font-Bold="true" FooterStyle-HorizontalAlign="Right"
                                        FooterStyle-BackColor="#dfe0f3" RowStyle-Font-Bold="true" CssClass="gridcss">
                                        <HeaderStyle BackColor="#3f06af" ForeColor="White" Font-Bold="false" Height="25px"
                                            HorizontalAlign="Center" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="CATEGORY" ItemStyle-Width="80px">
                                                <ItemTemplate>
                                                    <%# Eval("category") %>
                                                    <asp:Label runat="server" ID="lblAssyStatus" Text='<%# Eval("AssyRimstatus").ToString() == "True" ? " (ASSY)" : ""%>'></asp:Label>
                                                    <asp:HiddenField ID="hdnProcessID" runat="server" Value='<%# Eval("processid") %>' />
                                                    <asp:HiddenField ID="hdnAssyRimstatus" runat="server" Value='<%# Eval("AssyRimstatus") %>' />
                                                    <asp:HiddenField ID="hdnOrder_ItemID" runat="server" Value='<%# Eval("O_ItemID") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="PLATFORM" DataField="config" ItemStyle-Width="100px" />
                                            <asp:BoundField HeaderText="TYRE SIZE" DataField="tyresize" ItemStyle-Width="150px" />
                                            <asp:BoundField HeaderText="RIM" DataField="rimsize" ItemStyle-Width="40px" />
                                            <asp:BoundField HeaderText="TYPE" DataField="tyretype" ItemStyle-Width="60px" />
                                            <asp:BoundField HeaderText="BRAND" DataField="brand" ItemStyle-Width="100px" />
                                            <asp:BoundField HeaderText="SIDEWALL" DataField="sidewall" ItemStyle-Width="100px" />
                                            <asp:BoundField HeaderText="QTY" DataField="itemqty" ItemStyle-HorizontalAlign="Right"
                                                ItemStyle-Width="30px" />
                                            <asp:TemplateField HeaderText="RIM QTY" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="40px">
                                                <ItemTemplate>
                                                    <%# Eval("Rimitemqty").ToString() == "0" ? "" : Eval("Rimitemqty").ToString()%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="PART-A (GSA EXACT MATCH)" DataField="PartA" ItemStyle-HorizontalAlign="Right"
                                                ItemStyle-Width="60px" />
                                            <asp:BoundField HeaderText="PART-B (GSA MATCH WITH REBRAND)" DataField="PartB" ItemStyle-HorizontalAlign="Right"
                                                ItemStyle-Width="60px" />
                                            <asp:BoundField HeaderText="PART-C (GSA UPGRADE WITH REBRAND)" DataField="PartC"
                                                ItemStyle-HorizontalAlign="Right" ItemStyle-Width="60px" />
                                            <asp:BoundField HeaderText="PART-D (CURRENT STOCK EXACT MATCH)" DataField="PartD"
                                                ItemStyle-HorizontalAlign="Right" ItemStyle-Width="60px" />
                                            <asp:BoundField HeaderText="PART-E (CURRENT STOCK MATCH WITH REBRAND)" DataField="PartE"
                                                ItemStyle-HorizontalAlign="Right" ItemStyle-Width="60px" />
                                            <asp:BoundField HeaderText="PART-F (CURRENT STOCK UPGRADE WITH REBRAND)" DataField="PartF"
                                                ItemStyle-HorizontalAlign="Right" ItemStyle-Width="60px" />
                                            <asp:BoundField HeaderText="PART-G (PRODUCED FOR THIS WORK ORDER)" DataField="PartG"
                                                ItemStyle-HorizontalAlign="Right" ItemStyle-Width="60px" />
                                            <asp:TemplateField ItemStyle-Width="60px" HeaderText="TO EARMARK" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkEarmarkItem" runat="server" Text="Assign" ClientIDMode="Static"
                                                        OnClick="lnkEarmarkItem_Click" Font-Bold="true" Visible='<%# Eval("ReqQty").ToString() == "0" || Eval("LnkStatus").ToString() == "False" ? false : true %>' />
                                                    <span style="color: #2cb543;">
                                                        <%# Eval("ReqQty").ToString() == "0" ? "Completed" : "" %></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div id="div_itemwisedesc" style="display: none;">
                                        <table cellspacing="0" rules="all" border="1" style="background-color: #dcecfb; width: 100%;
                                            border-color: White; border-collapse: separate; line-height: 22px;" id="frmEarmarkPrepareItem">
                                            <tr>
                                                <th colspan="2" style="text-align: center; background-color: #68e266; font-weight: bold;
                                                    font-size: 14px;">
                                                    ITEM DESCRIPTION
                                                </th>
                                                <th colspan="2" style="text-align: center; background-color: #b8e266; font-weight: bold;
                                                    font-size: 14px;">
                                                    PART WISE ASSIGNED QTY
                                                </th>
                                                <th style="text-align: center; background-color: #ec89a9; font-weight: bold; font-size: 14px;">
                                                    STOCK AVAILABILITY
                                                </th>
                                            </tr>
                                            <tr>
                                                <th style="width: 100px;">
                                                    PLATFORM
                                                </th>
                                                <td style="width: 200px;">
                                                    <asp:Label runat="server" ID="lblPlatform" Text=""></asp:Label>
                                                </td>
                                                <th style="width: 320px;">
                                                    PART-A (GSA EXACT MATCH)
                                                </th>
                                                <td style="width: 50px;">
                                                    <asp:Label runat="server" ID="lblPartA" Text=""></asp:Label>
                                                </td>
                                                <td style="width: 320px;">
                                                    <asp:LinkButton runat="server" ID="lnkPartA" ClientIDMode="Static" Text="" OnClick="lnkAvailablePart_Click"
                                                        Font-Bold="false" Font-Size="12px"></asp:LinkButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>
                                                    TYRE SIZE
                                                </th>
                                                <td>
                                                    <asp:Label runat="server" ID="lblTyresize" Text=""></asp:Label>
                                                </td>
                                                <th>
                                                    PART-B (GSA MATCH WITH REBRAND)
                                                </th>
                                                <td>
                                                    <asp:Label runat="server" ID="lblPartB" Text=""></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:LinkButton runat="server" ID="lnkPartB" ClientIDMode="Static" Text="" OnClick="lnkAvailablePart_Click"
                                                        Font-Bold="false" Font-Size="12px"></asp:LinkButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>
                                                    RIM
                                                </th>
                                                <td>
                                                    <asp:Label runat="server" ID="lblRim" Text=""></asp:Label>
                                                </td>
                                                <th>
                                                    PART-C (GSA UPGRADE WITH REBRAND)
                                                </th>
                                                <td>
                                                    <asp:Label runat="server" ID="lblPartC" Text=""></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:LinkButton runat="server" ID="lnkPartC" ClientIDMode="Static" Text="" OnClick="lnkAvailablePart_Click"
                                                        Font-Bold="false" Font-Size="12px"></asp:LinkButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>
                                                    TYPE
                                                </th>
                                                <td>
                                                    <asp:Label runat="server" ID="lblType" Text=""></asp:Label>
                                                </td>
                                                <th>
                                                    PART-D (CURRENT STOCK EXACT MATCH)
                                                </th>
                                                <td>
                                                    <asp:Label runat="server" ID="lblPartD" Text=""></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:LinkButton runat="server" ID="lnkPartD" ClientIDMode="Static" Text="" OnClick="lnkAvailablePart_Click"
                                                        Font-Bold="false" Font-Size="12px"></asp:LinkButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>
                                                    BRAND
                                                </th>
                                                <td>
                                                    <asp:Label runat="server" ID="lblBrand" Text=""></asp:Label>
                                                </td>
                                                <th>
                                                    PART-E (CURRENT STOCK MATCH WITH REBRAND)
                                                </th>
                                                <td>
                                                    <asp:Label runat="server" ID="lblPartE" Text=""></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:LinkButton runat="server" ID="lnkPartE" ClientIDMode="Static" Text="" OnClick="lnkAvailablePart_Click"
                                                        Font-Bold="false" Font-Size="12px"></asp:LinkButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>
                                                    SIDEWALL
                                                </th>
                                                <td>
                                                    <asp:Label runat="server" ID="lblSidewall" Text=""></asp:Label>
                                                </td>
                                                <th>
                                                    PART-F (CURRENT STOCK UPGRADE WITH REBRAND)
                                                </th>
                                                <td>
                                                    <asp:Label runat="server" ID="lblPartF" Text=""></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:LinkButton runat="server" ID="lnkPartF" ClientIDMode="Static" Text="" OnClick="lnkAvailablePart_Click"
                                                        Font-Bold="false" Font-Size="12px"></asp:LinkButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>
                                                    PROCESS-ID
                                                </th>
                                                <td>
                                                    <asp:Label runat="server" ID="lblProcessID" Text=""></asp:Label>
                                                    <asp:HiddenField runat="server" ID="hdnO_ItemID" ClientIDMode="Static" Value="" />
                                                </td>
                                                <th>
                                                    TOTAL EARMARK QTY
                                                </th>
                                                <td>
                                                    <asp:Label runat="server" ID="lblEarmarkQty" Text="" Font-Bold="true" Font-Size="16px"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblProducedQty" Text="" Font-Bold="true" Font-Size="12px"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>
                                                    ORDER QTY
                                                </th>
                                                <td>
                                                    <asp:Label runat="server" ID="lblOrderQty" Text="" Font-Bold="true" Font-Size="16px"></asp:Label>
                                                </td>
                                                <th>
                                                    REQUIRED QTY
                                                </th>
                                                <td>
                                                    <asp:Label runat="server" ID="lblRequiredQty" ClientIDMode="Static" Text="" ForeColor="Red"
                                                        Font-Size="16px"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblStockMsg" ClientIDMode="Static" Text="" Width="300px"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div id="div_manufactureyear" style="display: none;">
                                        <table cellspacing="0" rules="all" border="1" style="background-color: #dcecfb; width: 100%;
                                            border-color: White; border-collapse: separate;">
                                            <tr>
                                                <td colspan="3">
                                                    <asp:Label runat="server" ID="lblYearMsg" ClientIDMode="Static" Text="" ForeColor="Green"
                                                        Font-Bold="true" Font-Size="15px"></asp:Label>
                                                    <asp:Label runat="server" ID="lblManuYearErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"
                                                        Font-Bold="true" Font-Size="15px"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" ID="lbl_MANUFACTUREYEAR" ClientIDMode="Static" Text=""></asp:Label>
                                                </td>
                                                <td colspan="2">
                                                    <asp:CheckBoxList runat="server" ID="chkManufactureYear" ClientIDMode="Static" RepeatColumns="15"
                                                        RepeatDirection="Horizontal">
                                                    </asp:CheckBoxList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" ID="lbl_QUALITYGRADE" ClientIDMode="Static" Text=""></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:CheckBoxList runat="server" ID="chkManufactureGrade" ClientIDMode="Static" RepeatColumns="10"
                                                        RepeatDirection="Horizontal" Font-Bold="true">
                                                    </asp:CheckBoxList>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label>
                                                    <div style="width: 260px; float: left;">
                                                        <asp:Button runat="server" ID="btnFindStencil" ClientIDMode="Static" Text="" CssClass="btn btn-info"
                                                            OnClick="btnFindStencil_Click" OnClientClick="Javascript:return CtrlFilter();" />
                                                    </div>
                                                    <div style="width: 300px; float: right; background-color: #fbd5fa; text-align: center;
                                                        line-height: 25px;">
                                                        <span class="lnkCss" onclick="ShowUpgardeLevel('divUpgradePopup')" runat="server"
                                                            clientidmode="Static" id="spanTag"></span>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label runat="server" ID="lbl_StockErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"
                                        Font-Bold="true" Font-Size="15px"></asp:Label>
                                    <div id="div_availablestencil" style="display: none;">
                                        <table cellspacing="0" rules="all" border="1" style="background-color: #dcecfb; width: 100%;
                                            border-color: White; border-collapse: separate;">
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Label runat="server" ID="lbl_StockQty" ClientIDMode="Static" Text="" Font-Bold="true"
                                                        Font-Size="15px" ForeColor="DarkGreen"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:GridView runat="server" ID="gvStockSelection" AutoGenerateColumns="false" Width="100%"
                                                        CssClass="gridcss">
                                                        <Columns>
                                                            <asp:BoundField HeaderText="PLATFORM" DataField="config" />
                                                            <asp:BoundField HeaderText="TYRE SIZE" DataField="tyresize" />
                                                            <asp:BoundField HeaderText="RIM" DataField="rimsize" />
                                                            <asp:BoundField HeaderText="TYPE" DataField="tyretype" />
                                                            <asp:BoundField HeaderText="BRAND" DataField="brand" />
                                                            <asp:BoundField HeaderText="SIDEWALL" DataField="sidewall" />
                                                            <asp:BoundField HeaderText="PLANT" DataField="Plant" />
                                                            <asp:BoundField HeaderText="GRADE" DataField="grade" />
                                                            <asp:BoundField HeaderText="STENCIL NO" DataField="stencilno" />
                                                            <asp:BoundField HeaderText="YOM" DataField="yearofmanufacture" />
                                                            <asp:BoundField HeaderText="LOCATION" DataField="location" />
                                                            <asp:BoundField HeaderText="REMARKS" DataField="remarks" />
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox runat="server" ID="checkAllChk" ClientIDMode="Static" />
                                                                    ALL
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chk_selectQty" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;">
                                                    <asp:Button ID="btnClear" ClientIDMode="Static" runat="server" Text="CLEAR CHECKED"
                                                        CssClass="btn btn-warning" />
                                                </td>
                                                <td style="text-align: center;">
                                                    <asp:Button ID="btnGSA_Assign" ClientIDMode="Static" runat="server" Text="" CssClass="btn btn-success"
                                                        OnClientClick="javascript:return ctrlBtn();" OnClick="btnGSA_Assign_Click" />
                                                    <asp:Label runat="server" ID="lblNextMsg" ClientIDMode="Static" Text="" Font-Bold="true"
                                                        Font-Size="15px"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div id="div_skipearmark" style="display: none; line-height: 30px;">
                                        <asp:Label runat="server" ID="lblAvlStock" ClientIDMode="Static" Text="" Font-Bold="true"
                                            Font-Size="15px"></asp:Label>
                                        <span style="color: #e85252;">ENTER YOUR COMMENTS FOR SKIP THE STENCIL ASSIGN:</span>
                                        <asp:TextBox ID="txtSkipEarmarkComments" runat="server" ClientIDMode="Static" TextMode="MultiLine"
                                            Height="40px" Width="640px" CssClass="form-control" onKeyUp="javascript:CheckMaxLength(this, 999);"
                                            onChange="javascript:CheckMaxLength(this, 999);"></asp:TextBox>
                                        <asp:Button ID="btn_SkipEarmark" ClientIDMode="Static" runat="server" Text="SKIP STENCIL ASSIGN PROCESS"
                                            CssClass="btn btn-success" OnClientClick="javascript:return chk_CntrlSave();"
                                            OnClick="btn_SkipEarmark_Click" />
                                        <br />
                                        <asp:Label runat="server" ID="lblSkipStencil" ClientIDMode="Static" Text="" ForeColor="#e85252"
                                            Font-Underline="true"></asp:Label>
                                        <asp:GridView runat="server" ID="gv_SkipStencil" ClientIDMode="Static" AutoGenerateColumns="true"
                                            Width="100%" HeaderStyle-BackColor="#cccccc" Font-Bold="true">
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
        <div id="divUpgradePopup" class="white_content">
            <div style="width: 100%; float: left; line-height: 30px;">
                <asp:Label runat="server" ID="lblUpgradePopMsg" ClientIDMode="Static" ForeColor="#21a709"
                    Font-Bold="true" Font-Size="14px"></asp:Label>
                <span style="width: 10%; float: right;"><span class="lnkCss" onclick="CloseUpgrade('divUpgradePopup')">
                    CLOSE</span></span>
            </div>
            <div style="overflow: auto; width: 100%; height: 80%;">
                <asp:CheckBoxList runat="server" ID="chkUpgradeList" ClientIDMode="Static" RepeatColumns="6"
                    RepeatDirection="Vertical" Width="100%">
                </asp:CheckBoxList>
            </div>
            <div style="width: 100%; height: 20%;">
                <asp:Label runat="server" ID="lblPartFLevel" ClientIDMode="Static" Text="" Font-Bold="true"
                    Font-Size="14px" ForeColor="#21a709"></asp:Label><br />
                <asp:CheckBoxList runat="server" ID="chkUpgrade_PartF" ClientIDMode="Static" RepeatColumns="6"
                    RepeatDirection="Vertical" Width="100%">
                </asp:CheckBoxList>
            </div>
        </div>
        <div id="div_FifoUnAssignList" class="white_content">
            <div style="width: 100%; float: left; line-height: 30px;">
                <asp:Label runat="server" ID="Label1" ClientIDMode="Static" Text="FIFO METHOD NOT FOLLOWED AND BELOW STENCIL NOT ASSINGED TO THIS ORDER ITEM: "
                    Font-Bold="true" Font-Size="12px" ForeColor="#21a709"></asp:Label>
                <span style="width: 10%; float: right;"><span class="lnkCss" onclick="CloseUpgrade('div_FifoUnAssignList')">
                    CLOSE</span></span>
            </div>
            <asp:DataList runat="server" ID="dlFifoItemMaster" RepeatColumns="1" CellSpacing="3"
                RepeatLayout="Table">
                <ItemTemplate>
                    <table cellspacing="0" rules="all" border="1" class="tableCss">
                        <tr>
                            <td colspan="7">
                                <%# Eval("custfullname")%>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <%# Eval("OrderRefNo")%>
                            </td>
                            <td colspan="3">
                                <%# Eval("workorderno")%>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                PLATFORM
                            </th>
                            <th>
                                TYRE SIZE
                            </th>
                            <th>
                                RIM SIZE
                            </th>
                            <th>
                                TYRE TYPE
                            </th>
                            <th>
                                BRAND
                            </th>
                            <th>
                                SIDEWALL
                            </th>
                            <th>
                                ORDER QTY
                            </th>
                        </tr>
                        <tr>
                            <td>
                                <%# Eval("Config")%>
                            </td>
                            <td>
                                <%# Eval("tyresize")%>
                            </td>
                            <td>
                                <%# Eval("rimsize")%>
                            </td>
                            <td>
                                <%# Eval("tyretype")%>
                            </td>
                            <td>
                                <%# Eval("brand")%>
                            </td>
                            <td>
                                <%# Eval("sidewall")%>
                            </td>
                            <td>
                                <%# Eval("itemqty")%>
                            </td>
                        </tr>
                        <tr>
                            <th colspan="7">
                                PART WISE ASSIGNED QTY
                            </th>
                        </tr>
                        <tr>
                            <th>
                                A
                            </th>
                            <th>
                                B
                            </th>
                            <th>
                                C
                            </th>
                            <th>
                                D
                            </th>
                            <th>
                                E
                            </th>
                            <th>
                                F
                            </th>
                            <th>
                                G
                            </th>
                        </tr>
                        <tr>
                            <td>
                                <%# Eval("PartA")%>
                            </td>
                            <td>
                                <%# Eval("PartB")%>
                            </td>
                            <td>
                                <%# Eval("PartC")%>
                            </td>
                            <td>
                                <%# Eval("PartD")%>
                            </td>
                            <td>
                                <%# Eval("PartE")%>
                            </td>
                            <td>
                                <%# Eval("PartF")%>
                            </td>
                            <td>
                                <%# Eval("PartG")%>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:DataList>
            <asp:GridView runat="server" ID="gvFifoUnassign" AutoGenerateColumns="false" Width="100%">
                <Columns>
                    <asp:BoundField HeaderText="PLATFORM" DataField="config" />
                    <asp:BoundField HeaderText="TYRE SIZE" DataField="tyresize" />
                    <asp:BoundField HeaderText="RIM" DataField="rimsize" />
                    <asp:BoundField HeaderText="TYRE TYPE" DataField="tyretype" />
                    <asp:BoundField HeaderText="BRAND" DataField="brand" />
                    <asp:BoundField HeaderText="SIDEWALL" DataField="sidewall" />
                    <asp:BoundField HeaderText="STENCIL" DataField="stencil" />
                    <asp:BoundField HeaderText="GRADE" DataField="grade" />
                    <asp:BoundField HeaderText="SKIP PART TYPE" DataField="AvoidPartType" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdnCustCode" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnPartType" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnPartMethod" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnAssyStatus" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnSelectItem" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnOID" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnStatusID" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        function gotoPreviewDiv(ctrlID) {
            $('#' + ctrlID).css({ 'display': 'block' });
            $("html, body").stop().animate({ scrollLeft: $('#' + ctrlID).offset().left, scrollTop: $('#' + ctrlID).offset().top }, 1200);
        }

        function CtrlFilter() {
            var ErrMsg = ''; $('#lblErrMsg').html('');
            if ($("input:checkbox[id*=chkManufactureYear_]:checked").length == 0)
                ErrMsg += 'Choose atleast one manufacture year<br/>';
            if ($("input:checkbox[id*=chkManufactureGrade_]:checked").length == 0)
                ErrMsg += 'Choose atleast one qulaity grade<br/>';
            if (ErrMsg.length > 0) {
                $('#lblErrMsg').html(ErrMsg)
                return false;
            }
            else
                return true;
        }

        $(function () {
            blinkText();
            $("[id*=gvStockSelection_chk_selectQty_]").on('change', function () {
                if ($("[id*=gvStockSelection_chk_selectQty_]:checked").length < $('#lblRequiredQty').html())
                    $("[id*=gvStockSelection_chk_selectQty_]:not(:checked)").prop('disabled', false);
                else
                    $("[id*=gvStockSelection_chk_selectQty_]:not(:checked)").prop('disabled', true);
            });
            $('#btnClear').click(function () { $('input:checkbox').prop('checked', false); return false; });
            $('#checkAllChk').click(function () {
                if ($("[id*=gvStockSelection_chk_selectQty_]").length > 0) {
                    if ($(this).attr('checked') == "checked")
                        $("[id*=gvStockSelection_chk_selectQty_]").attr('checked', true)
                    else
                        $("[id*=gvStockSelection_chk_selectQty_]").attr('checked', false)
                }
                else {
                    alert('No records');
                    $(this).attr('checked', false);
                }
            });
            $("[id*=gvStockSelection_chk_selectQty_]").click(function () { $('#checkAllChk').attr('checked', false); });
            $("input:checkbox[id*=chkManufactureYear_]").click(function () { $('#div_availablestencil').css({ 'display': 'none' }); $('#btnFindStencil').css({ 'display': 'block' }); });
            $("input:checkbox[id*=chkManufactureGrade_]").click(function () { $('#div_availablestencil').css({ 'display': 'none' }); $('#btnFindStencil').css({ 'display': 'block' }); });
        });
        function ctrlBtn() {
            if ($("[id*=gvStockSelection_chk_selectQty_]:checked").length != 0)
                return true;
            else {
                alert("Choose atleast one quantity");
                return false;
            }
        }
        function setblinkText() { $('#lblYearMsg').css({ 'background-color': '#660053', 'color': '#fff' }); setTimeout("blinkText()", 3000) }
        function blinkText() { $('#lblYearMsg').css({ 'background-color': '#dadada', 'color': '#000' }); setTimeout("setblinkText()", 3000) }

        function ShowUpgardeLevel(CtrlID) {
            $('#' + CtrlID).css({ 'display': 'block', 'top': $('#spanTag').offset().top - 400 });
            $('#tbEarmarkPage').css({ 'opacity': '0.1' });
        }
        function CloseUpgrade(CtrlID) {
            $('#' + CtrlID).css({ 'display': 'none' });
            $('#tbEarmarkPage').css({ 'opacity': '1' });
        }
        function ChkBox_hide() {
            $("[id*=chkUpgradeList_]").css({ 'display': 'none' });
            $("[id*=chkUpgradeList_]").attr({ 'disabled': true });
            $("[id*=chkUpgrade_PartF_]").css({ 'display': 'none' });
            $("[id*=chkUpgrade_PartF_]").attr({ 'disabled': true });
        }
        function showorderlist() {
            $('#div_EarmarkOrders').css({ 'display': 'block' });
            $("#div_earkmark_OrderItems").css({ 'display': 'none' });
        }
        function chk_CntrlSave() {
            if ($('#txtSkipEarmarkComments').val().length == 0) {
                alert('Enter Your Comments');
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
