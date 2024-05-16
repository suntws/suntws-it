<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExpPPC_Verification1.aspx.cs"
    MasterPageFile="~/master.Master" Inherits="TTS.ExpPPC_Verification1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/cotsScript.js" type="text/javascript"></script>
    <link href="Styles/datemonthyear.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .cssLblMsg
        {
            background-color: #9e2905;
            color: #ffffff;
            font-size: 15px;
            font-weight: bold;
            width: 100%;
            float: left;
            line-height: 25px;
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        <asp:Label runat="server" ID="lblPageHead" ClientIDMode="Static" Text=""></asp:Label>
        Order PPC Verification
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="background-color: #dcecfb; width: 100%;
            border-color: White; border-collapse: separate;">
            <tr>
                <td>
                    <asp:GridView ID="gv_Orders" Width="100%" runat="server" AutoGenerateColumns="false"
                        RowStyle-Height="30px" 
                        onselectedindexchanged="gv_Orders_SelectedIndexChanged">
                        <HeaderStyle BackColor="#74bbf9" ForeColor="White" Font-Bold="true" Height="25px"
                            HorizontalAlign="Center" />
                        <Columns>
                            <asp:TemplateField HeaderText="CUSTOMER NAME" ItemStyle-Width="250px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatusCustName" Text='<%# Eval("custfullname") %>'></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdnStatusCustCode" Value='<%# Eval("CustCode") %>' />
                                    <asp:HiddenField runat="server" ID="hdnContainerLoadFrom" Value='<%# Eval("ContainerLoadFrom") %>' />
                                    <asp:HiddenField runat="server" ID="hdnOrderStatus" Value='<%# Eval("OrderStatus") %>' />
                                    <asp:HiddenField runat="server" ID="hdnOrderID" Value='<%# Eval("ID") %>' />
                                    <asp:HiddenField runat="server" ID="hdnExpectedShipDate" Value='<%# Eval("ExpectedShipDate") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="WORK ORDER" ItemStyle-Width="60px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblWorkorderNo" Text='<%#Eval("workorderno") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ORDER REF NO" ItemStyle-Width="220px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOrderRefNo" Text='<%#Eval("OrderRefNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ORDER DATE" ItemStyle-Width="70px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOrderDate" Text='<%#Eval("CompletedDate") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="CUSTOMER DESIRED SHIP DATE" DataField="DesiredShipDate"
                                ItemStyle-Width="70px" />
                            <asp:BoundField HeaderText="ORDER QTY" DataField="itemqty" ItemStyle-Width="40px"
                                ItemStyle-HorizontalAlign="Right" />
                            <asp:TemplateField HeaderText="SHIPMENT TYPE" ItemStyle-Width="45px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblShipmentType" Text='<%# Eval("ShipmentType") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="STATUS" ItemStyle-Width="130px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatusText" Text='<%# Eval("StatusText") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="75px" HeaderText="ACTION" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkOrderSelection" CssClass="btn btn-success" runat="server"
                                        OnClick="lnkOrderSelection_click" Text="Review">
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="div_Sub_OrderItems" style="display: none;">
                        <table cellspacing="0" rules="all" border="1" style="background-color: #dcecfb; width: 100%;
                            border-color: White; border-collapse: separate;">
                            <tr style="text-align: center; background-color: #3c763d; font-size: 15px; color: #ffffff;">
                                <td>
                                    <asp:Label ID="lblSelectedCustomerName" runat="server" Text="" Font-Bold="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblSelectedOrderRefNo" ClientIDMode="Static" runat="server" Text=""
                                        Font-Bold="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblSelectedWorkOrderNo" ClientIDMode="Static" runat="server" Text=""
                                        Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:GridView ID="gv_OrderItems" Width="100%" runat="server" AutoGenerateColumns="false"
                                        RowStyle-Height="22px" ShowFooter="true" FooterStyle-Font-Bold="true" HeaderStyle-BackColor="#022442"
                                        FooterStyle-HorizontalAlign="Right" FooterStyle-BackColor="#dfe0f3">
                                        <HeaderStyle ForeColor="White" Font-Bold="true" Height="25px" HorizontalAlign="Center" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="CATEGORY" ItemStyle-Width="100px">
                                                <ItemTemplate>
                                                    <%# Eval("category") %>
                                                    <asp:Label runat="server" ID="lblAssyStatus" Text='<%# Eval("AssyRimstatus").ToString() == "True" ? " (ASSY)" : ""%>'></asp:Label>
                                                    <asp:HiddenField ID="hdnProcessID" runat="server" Value='<%# Eval("processid") %>' />
                                                    <asp:HiddenField ID="hdnEdcNo" runat="server" Value='<%# Eval("EdcNo") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="PLATFORM" DataField="config" ItemStyle-Width="100px" />
                                            <asp:BoundField HeaderText="TYRE SIZE" DataField="tyresize" ItemStyle-Width="150px" />
                                            <asp:BoundField HeaderText="RIM" DataField="rimsize" ItemStyle-Width="40px" />
                                            <asp:BoundField HeaderText="TYPE" DataField="tyretype" ItemStyle-Width="60px" />
                                            <asp:BoundField HeaderText="BRAND" DataField="brand" ItemStyle-Width="100px" />
                                            <asp:BoundField HeaderText="SIDEWALL" DataField="sidewall" ItemStyle-Width="100px" />
                                            <asp:BoundField HeaderText="FWT" DataField="tyrewt" ItemStyle-HorizontalAlign="Right"
                                                ItemStyle-Width="60px" />
                                            <asp:TemplateField HeaderText="ORDER QTY" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Right"
                                                ItemStyle-Font-Bold="true">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblOrderQty" Text='<%#Eval("itemqty") %>'></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ASSIGNED PART A/B/C/D/E/F" ItemStyle-HorizontalAlign="Right"
                                                ItemStyle-Font-Bold="true" ItemStyle-Width="40px">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblearmark" Text='<%# Eval("earmarkqty") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ASSIGNED FOR NEW PRODUCTION" ItemStyle-Width="50px"
                                                ItemStyle-HorizontalAlign="Right" ItemStyle-Font-Bold="true">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblnewprod" Text='<%# Eval("newProduction") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="SO FOR PRODUCED" ItemStyle-HorizontalAlign="Right"
                                                ItemStyle-Font-Bold="true" ItemStyle-Width="40px">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblProduced" Text='<%# Eval("PartG") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="BALANCE TO BE PRODUCED" ItemStyle-HorizontalAlign="Right"
                                                ItemStyle-Font-Bold="true" ItemStyle-Width="40px" ItemStyle-BackColor="#edfb7f">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblBalProduce" Text='<%# Eval("balancetoproduce") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PART A&D AVAILABLE IN STOCK" ItemStyle-HorizontalAlign="Right"
                                                ItemStyle-ForeColor="#ff1800" ItemStyle-Font-Bold="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_ProdApprove" runat="server" Text='<%# Eval("ProdApprove") %>'
                                                        Visible="false"></asp:Label>
                                                    <asp:Label ID="lbl_ProcessID" runat="server" Text='<%# Eval("processid") %>' Visible="false"></asp:Label>
                                                    <asp:Label runat="server" ID="lbl_OItemID" Text='<%# Eval("O_ItemID") %>' Visible="false"></asp:Label>
                                                    <asp:LinkButton runat="server" ID="lnkStockUnAssign" Text="ASSIGN" OnClick="lnkStockUnAssign_Click"
                                                        Visible='<%# Convert.ToInt32(Eval("balancetoproduce").ToString()) > 0 && Convert.ToInt32(Eval("Stock").ToString()) > 0 && 
                                                        Convert.ToInt32(Eval("ProdApprove").ToString()) == 1 ? true : false %>'></asp:LinkButton>
                                                    <%# Convert.ToInt32(Eval("balancetoproduce").ToString()) > 0 && Convert.ToInt32(Eval("Stock").ToString()) > 0 && 
                                                    Convert.ToInt32(Eval("ProdApprove").ToString()) == 1 ? Eval("Stock").ToString() : ""%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="RIM FWT" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="60px">
                                                <ItemTemplate>
                                                    <%# Eval("Rimfinishedwt").ToString() == "0.00" ? "" : Eval("Rimfinishedwt").ToString()%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="RIM QTY" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="40px">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblRimqty" Text='<%# Eval("Rimitemqty").ToString() == "0" ? "" : Eval("Rimitemqty").ToString()%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="TOTAL FWT" DataField="finishedwt" ItemStyle-HorizontalAlign="Right"
                                                ItemStyle-Width="100px" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    <asp:Label runat="server" ID="lblCombiOrder" ClientIDMode="Static" Text="" Font-Bold="true"
                                        Font-Size="14px"></asp:Label>
                                </td>
                                <td style="color: #0000f1; text-align: center; font-weight: bold;">
                                    <span id="spandisplay" runat="server" clientidmode="Static" onclick="show_other_plant();"
                                        style="cursor: pointer;"></span>
                                </td>
                                <td>
                                    <asp:LinkButton runat="server" ID="lnk_MoveStatusToStencilAssign" ClientIDMode="Static"
                                        Text="" OnClick="lnk_MoveStatusToStencilAssign_Click" Font-Bold="true"></asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <div id="div_OtherPlantList" style="display: none;">
                                        <asp:GridView ID="gvCombiOrderItem" Width="100%" runat="server" AutoGenerateColumns="false"
                                            RowStyle-Height="22px" ShowFooter="true" FooterStyle-Font-Bold="true" HeaderStyle-BackColor="#548606"
                                            FooterStyle-HorizontalAlign="Right" FooterStyle-BackColor="#b8fdfd">
                                            <HeaderStyle BackColor="#548606" ForeColor="White" Font-Bold="true" Height="25px"
                                                HorizontalAlign="Center" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="CATEGORY" ItemStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <%# Eval("category") %>
                                                        <asp:Label runat="server" ID="lblAssyStatus" Text='<%# Eval("AssyRimstatus").ToString() == "True" ? " (ASSY)" : ""%>'></asp:Label>
                                                        <asp:HiddenField ID="hdnProcessID" runat="server" Value='<%# Eval("processid") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="PLATFORM" DataField="config" ItemStyle-Width="100px" />
                                                <asp:BoundField HeaderText="TYRE SIZE" DataField="tyresize" ItemStyle-Width="150px" />
                                                <asp:BoundField HeaderText="RIM" DataField="rimsize" ItemStyle-Width="40px" />
                                                <asp:BoundField HeaderText="TYPE" DataField="tyretype" ItemStyle-Width="60px" />
                                                <asp:BoundField HeaderText="BRAND" DataField="brand" ItemStyle-Width="100px" />
                                                <asp:BoundField HeaderText="SIDEWALL" DataField="sidewall" ItemStyle-Width="100px" />
                                                <asp:BoundField HeaderText="FWT" DataField="tyrewt" ItemStyle-HorizontalAlign="Right"
                                                    ItemStyle-Width="60px" />
                                                <asp:BoundField HeaderText="QTY" DataField="itemqty" ItemStyle-HorizontalAlign="Right"
                                                    ItemStyle-Width="40px" />
                                                <asp:BoundField DataField="AvalQty" HeaderText="AVAL QTY" ItemStyle-HorizontalAlign="Right"
                                                    ItemStyle-Width="60px" />
                                                <asp:TemplateField HeaderText="RIM FWT" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="60px">
                                                    <ItemTemplate>
                                                        <%# Eval("Rimfinishedwt").ToString() == "0.00" ? "" : Eval("Rimfinishedwt").ToString()%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="RIM QTY" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="40px">
                                                    <ItemTemplate>
                                                        <%# Eval("Rimitemqty").ToString() == "0" ? "" : Eval("Rimitemqty").ToString()%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="TOTAL FWT" DataField="finishedwt" ItemStyle-HorizontalAlign="Right"
                                                    ItemStyle-Width="100px" />
                                                <asp:BoundField DataField="ItemPlant" HeaderText="PLANT" ItemStyle-Width="" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <table id="tbl_TechHelp" cellspacing="0" rules="all" border="1" style="background-color: #ecf6ff;
                                        border-color: White; border-collapse: separate; width: 100%;">
                                        <tr>
                                            <td colspan="3">
                                                <asp:Label runat="server" ID="lblOrderType" ClientIDMode="Static" Text="" CssClass="cssLblMsg"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th class="spanCss" style="width: 150px;">
                                                Expected Ex-Factory Ship Date
                                            </th>
                                            <td>
                                                <asp:Label runat="server" ID="lblExpectedShipDate" ClientIDMode="Static" Text=""
                                                    Font-Bold="true"></asp:Label>
                                            </td>
                                            <td rowspan="2">
                                                <div style="width: 650px; float: left;">
                                                    <span style="font-size: 13px;">WORK ORDER DOWNLOAD:</span>
                                                    <br />
                                                    <asp:GridView runat="server" ID="gv_DownloadFiles" ClientIDMode="Static" AutoGenerateColumns="false">
                                                        <HeaderStyle CssClass="headerNone" />
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkPdfFileName" runat="server" Text='<%# Eval("AttachFileName")%>'
                                                                        OnClick="lnkPdfLink_click" OnClientClick="aspnetForm.target ='_blank';">
                                                                    </asp:LinkButton></ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th class="spanCss" style="width: 150px;">
                                                RFD
                                            </th>
                                            <td>
                                                <asp:TextBox ID="txt_RFD" ClientIDMode="Static" runat="server" CssClass="form-control"
                                                    Width="100px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th class="spanCss">
                                                Production feasibility
                                            </th>
                                            <td>
                                                <asp:RadioButtonList ID="rdo_ProdFeasibility" ClientIDMode="Static" runat="server"
                                                    RepeatDirection="Horizontal" Width="130px">
                                                    <asp:ListItem Text="OK" Value="OK"></asp:ListItem>
                                                    <asp:ListItem Text="NOT OK" Value="NOT OK"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td>
                                                <div id="div_ProdFeasablity" style="display: none; width: 650px; float: left; line-height: 50px;">
                                                    <span class="spanCss" style="width: 640px; float: left; line-height: 20px;">Enter Production
                                                        Feasibility Comments</span>
                                                    <asp:TextBox ID="txt_ProdFeasiblityComments" runat="server" ClientIDMode="Static"
                                                        TextMode="MultiLine" Height="40px" Width="640px" CssClass="form-control" onKeyUp="javascript:CheckMaxLength(this, 999);"
                                                        onChange="javascript:CheckMaxLength(this, 999);"></asp:TextBox>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th class="spanCss">
                                                Loading feasibility
                                            </th>
                                            <td>
                                                <asp:RadioButtonList ID="rdo_LoadFeasibility" ClientIDMode="Static" runat="server"
                                                    RepeatDirection="Horizontal" Width="130px">
                                                    <asp:ListItem Text="OK" Value="OK"></asp:ListItem>
                                                    <asp:ListItem Text="NOT OK" Value="NOT OK"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td>
                                                <div id="div_LoadFeasibility" style="display: none; width: 650px; float: left; line-height: 50px;">
                                                    <span class="spanCss" style="width: 640px; float: left; line-height: 20px;">Enter Loading
                                                        Feasibility Comments</span>
                                                    <asp:TextBox ID="txt_LoadFeasibilityComments" runat="server" ClientIDMode="Static"
                                                        TextMode="MultiLine" Height="40px" Width="640px" CssClass="form-control" onKeyUp="javascript:CheckMaxLength(this, 999);"
                                                        onChange="javascript:CheckMaxLength(this, 999);"></asp:TextBox>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th class="spanCss">
                                                Equipments requirement
                                            </th>
                                            <td>
                                                <asp:RadioButtonList ID="rdo_EquipmentReq" ClientIDMode="Static" runat="server" RepeatDirection="Horizontal"
                                                    Width="130px">
                                                    <asp:ListItem Text="NO" Value="NO"></asp:ListItem>
                                                    <asp:ListItem Text="YES" Value="YES"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td>
                                                <div id="div_EquipmentReq" style="display: none; width: 650px; float: left; line-height: 50px;">
                                                    <span class="spanCss" style="width: 640px; float: left; line-height: 20px;">Enter Equipments
                                                        requirement Comments</span>
                                                    <asp:TextBox ID="txt_EquipmentReqComments" runat="server" ClientIDMode="Static" TextMode="MultiLine"
                                                        Height="40px" Width="640px" CssClass="form-control"></asp:TextBox>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th class="spanCss">
                                                EDC/QC requirement
                                            </th>
                                            <td>
                                                <asp:RadioButtonList ID="rdo_TechReq" ClientIDMode="Static" runat="server" RepeatDirection="Horizontal"
                                                    Width="130px">
                                                    <asp:ListItem Text="NO" Value="NO"></asp:ListItem>
                                                    <asp:ListItem Text="YES" Value="YES"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td>
                                                <div id="div_TechReq" style="display: none; width: 650px; float: left; line-height: 50px;">
                                                    <span class="spanCss" style="width: 640px; float: left; line-height: 20px;">Enter EDC/QC
                                                        requirement Comments</span>
                                                    <asp:TextBox ID="txt_TechReqComments" runat="server" ClientIDMode="Static" TextMode="MultiLine"
                                                        Height="40px" Width="640px" CssClass="form-control"></asp:TextBox>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th class="spanCss">
                                                Purchase requirement
                                            </th>
                                            <td>
                                                <asp:RadioButtonList ID="rdo_PurchaseReq" ClientIDMode="Static" runat="server" RepeatDirection="Horizontal"
                                                    Width="130px">
                                                    <asp:ListItem Text="NO" Value="NO"></asp:ListItem>
                                                    <asp:ListItem Text="YES" Value="YES"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td>
                                                <div id="div_PurchaseReq" style="display: none; width: 650px; float: left; line-height: 50px;">
                                                    <span class="spanCss" style="width: 640px; float: left; line-height: 20px;">Enter Purchase
                                                        requirement Comments</span>
                                                    <asp:TextBox ID="txt_PurchaseReqComments" runat="server" ClientIDMode="Static" TextMode="MultiLine"
                                                        Height="40px" Width="640px" CssClass="form-control"></asp:TextBox>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label ID="lblErrMsg" runat="server" ClientIDMode="Static" Font-Bold="true" ForeColor="Red"></asp:Label>
                                            </td>
                                            <td>
                                                <div id="div_Cntrls" style="text-align: center; display: none;">
                                                    <asp:Button ID="btn_SaveRecords" ClientIDMode="Static" runat="server" Text="Save Record"
                                                        CssClass="btn btn-success" OnClientClick="javascript:return chk_CntrlSave();"
                                                        OnClick="btn_SaveRecords_Click" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Button ID="btn_ClearRecords" runat="server" Text="Clear Selection" CssClass="btn btn-info" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <div id="div_StatusChange" style="text-align: center; display: none;">
                                        <span class="spanCss" style="text-align: left;">Enter Comments</span>
                                        <asp:TextBox ID="txt_StatusChangeComments" runat="server" ClientIDMode="Static" TextMode="MultiLine"
                                            Height="80px" Width="80%" CssClass="form-control"></asp:TextBox><br />
                                        <asp:Button ID="btn_StatusChange" ClientIDMode="Static" runat="server" Text="MOVE TO PRODUCTION/ PDI"
                                            CssClass="btn btn-success" OnClick="btn_StatusChange_Click" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <span class="btn btn-info" onclick="EnableAllCtrls();">CLICK HERE FOR RFD REVISE</span>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField ID="hdnCustCode" runat="server" Value="" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hdnSelectIndex" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnSelectStatus" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnCompleteStatus" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnOID" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnOrderMovedTo" ClientIDMode="Static" Value="" />
    <script src="Scripts/datemonthyear.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $('#rdo_ProdFeasibility').parent('td').parent('tr').hide();
            $('#rdo_LoadFeasibility').parent('td').parent('tr').hide();
            $('#rdo_EquipmentReq').parent('td').parent('tr').hide();
            $('#rdo_TechReq').parent('td').parent('tr').hide();
            $('#rdo_PurchaseReq').parent('td').parent('tr').hide();

            $('#rdo_ProdFeasibility').change(function () {
                $('#div_ProdFeasablity').hide();
                if ($('#rdo_ProdFeasibility :checked').val() == "NOT OK")
                    $('#div_ProdFeasablity').show();
            });
            $('#rdo_LoadFeasibility').change(function () {
                $('#div_LoadFeasibility').hide();
                if ($('#rdo_LoadFeasibility :checked').val() == "NOT OK")
                    $('#div_LoadFeasibility').show();
            });
            $('#rdo_EquipmentReq').change(function () {
                $('#div_EquipmentReq').hide();
                if ($('#rdo_EquipmentReq :checked').val() == "YES")
                    $('#div_EquipmentReq').show();
            });
            $('#rdo_TechReq').change(function () {
                $('#div_TechReq').hide();
                if ($('#rdo_TechReq :checked').val() == "YES")
                    $('#div_TechReq').show();
            });
            $('#rdo_PurchaseReq').change(function () {
                $('#div_PurchaseReq').hide();
                if ($('#rdo_PurchaseReq :checked').val() == "YES")
                    $('#div_PurchaseReq').show();
            });
            $('#txt_RFD').datepicker({ minDate: "+0D", maxDate: "+90D" }).keydown(function (e) { e.keyWhich = 0; e.keyCode = 0; e.preventDefault(); });
            $('input:text').blur(function () { $(this).css({ 'border': '1px solid #000' }); });
        });

        function chk_CntrlSave() {
            var errMsg = "";
            $('#lblErrMsg').html('');

            if ($('#txt_RFD').val() == "")
                errMsg += "Enter RFD <br/>";
            if ($('#rdo_ProdFeasibility :checked').length == 0)
                errMsg += "Choose Production Feasiblity <br/>";
            else if ($('#rdo_ProdFeasibility :checked').val() == "NOT OK" && $('#txt_ProdFeasiblityComments').val() == "")
                errMsg += "Enter Production Feasiblity Comments <br/>";
            if ($('#rdo_LoadFeasibility :checked').length == 0)
                errMsg += "Choose Loading Feasiblity <br/>";
            else if ($('#rdo_LoadFeasibility :checked').val() == "NOT OK" && $('#txt_LoadFeasibilityComments').val() == "")
                errMsg += "Enter Loading Feasiblity Comments <br/>";
            if ($('#rdo_EquipmentReq :checked').length == 0)
                errMsg += "Choose Equipments Requirement <br/>";
            else if ($('#rdo_EquipmentReq :checked').val() == "YES" && $('#txt_EquipmentReqComments').val() == "")
                errMsg += "Enter Equipments Requirement Comments <br/>";
            if ($('#rdo_TechReq :checked').length == 0)
                errMsg += "Choose EDC/QC Technical Requirement <br/>";
            else if ($('#rdo_TechReq :checked').val() == "YES" && $('#txt_TechReqComments').val() == "")
                errMsg += "Enter EDC/QC Technical requirement Comments <br/>";
            if ($('#rdo_PurchaseReq :checked').length == 0)
                errMsg += "Choose Purchase Requirement <br/>";
            else if ($('#rdo_PurchaseReq :checked').val() == "YES" && $('#txt_PurchaseReqComments').val() == "")
                errMsg += "Enter Purchase requirement Comments <br/>";
            if (errMsg.length > 0) {
                $('#lblErrMsg').html(errMsg);
                return false;
            }
            else
                return true;
        }
        function DisableAllCtrls() {
            $('#tbl_TechHelp').find("input,button,textarea,radio").attr("disabled", true);
            $('#MainContent_gv_OrderItems').find("input").attr("disabled", true);
            $('#div_StatusChange').css({ 'display': 'block' });
            $('#div_Cntrls').css({ 'display': 'none' });
        }
        function EnableAllCtrls() {
            $('#tbl_TechHelp').find("input,button,textarea,radio").attr("disabled", false);
            $('#MainContent_gv_OrderItems').find("input").attr("disabled", false);
            $('#div_StatusChange').css({ 'display': 'none' });
            $('#div_Cntrls').css({ 'display': 'block' });
        }

        function show_other_plant() {
            $('#div_OtherPlantList').hide();
            if ($('#spandisplay').html() == 'SHOW') {
                $('#div_OtherPlantList').show();
                $('#spandisplay').html('HIDE');
            }
            else
                $('#spandisplay').html('SHOW');
        }
        function gotoPreviewDiv(ctrlID) {
            $('#' + ctrlID).css({ 'display': 'block' });
            $("html, body").stop().animate({ scrollLeft: $('#' + ctrlID).offset().left, scrollTop: $('#' + ctrlID).offset().top }, 1200);
        }
        function ctrlRfdDivShow(cont) {
            $('#tbl_TechHelp').css({ 'display': '' + cont + '' });
        }
    </script>
</asp:Content>
