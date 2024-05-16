<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExpPPC_Verification.aspx.cs"
    MasterPageFile="~/master.Master" Inherits="TTS.ExpPPC_Verification" %>

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
                        RowStyle-Height="30px">
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
                            <asp:BoundField HeaderText="WORK ORDER" DataField="workorderno" ItemStyle-Width="60px" />
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
                                        OnClick="lnkOrderSelection_click" Text="Process">
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
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:GridView ID="gv_OrderItems" Width="100%" runat="server" AutoGenerateColumns="false"
                                        RowStyle-Height="22px" ShowFooter="true" FooterStyle-Font-Bold="true" HeaderStyle-BackColor="#0688f9"
                                        FooterStyle-HorizontalAlign="Right" FooterStyle-BackColor="#dfe0f3">
                                        <HeaderStyle BackColor="#74bbf9" ForeColor="White" Font-Bold="true" Height="25px"
                                            HorizontalAlign="Center" />
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
                                            <asp:TemplateField HeaderText="QTY" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblQty" Text='<%#Eval("itemqty") %>'></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="STOCK" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="40px">
                                                <ItemTemplate>
                                                    <%# Eval("Stock").ToString() == "" ? "0" : Eval("Stock").ToString()%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="EARMARK" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="40px">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblearmark" Text='<%# Eval("earmarkqty").ToString() == "" ? "0" : Eval("earmarkqty").ToString()%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="AVAL QTY" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="40px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_AvalQty" runat="server" Height="20px" Width="40px" MaxLength="4"
                                                        Text='<%# Eval("AvalQty") %>' onkeyup="splitOrderQty(this)" onkeypress="return isNumberWithoutDecimal(event)"
                                                        CssClass="form-control" Font-Bold="true"></asp:TextBox>
                                                    <div>
                                                        <asp:Label runat="server" ID="lblQtyErrMsg" Text=""></asp:Label></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="NEW PROD" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Right"
                                                ItemStyle-Font-Bold="true">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblnewprod" Text='<%# Eval("O_Newproduction").ToString() == "" ? "0" : Eval("O_Newproduction").ToString()%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
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
                                            <asp:TemplateField HeaderText="AVAL RIM QTY" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="60px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_AvalRimQty" runat="server" Height="20px" Width="40px" MaxLength="4"
                                                        Text='<%# Eval("AvalRimQty") %>' CssClass="form-control" onkeypress="return isNumberWithoutDecimal(event)"
                                                        Visible='<%# Eval("Rimitemqty").ToString() == "0" ? false : true %>' Font-Bold="true"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="TOTAL FWT" DataField="finishedwt" ItemStyle-HorizontalAlign="Right"
                                                ItemStyle-Width="100px" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblCombiOrder" ClientIDMode="Static" Text="" Font-Bold="true"
                                        Font-Size="14px"></asp:Label>
                                </td>
                                <td style="color: #0000f1; text-align: center; font-weight: bold;">
                                    <span id="spandisplay" runat="server" clientidmode="Static" onclick="show_other_plant();"
                                        style="cursor: pointer;"></span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
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
                                                <asp:TemplateField HeaderText="AVAL RIM QTY" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="60px">
                                                    <ItemTemplate>
                                                        <%# Eval("Rimitemqty").ToString() == "0" ? "" : Eval("AvalRimQty").ToString()%>
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
                                <td colspan="2">
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
                                        <asp:Button ID="btn_StatusChange" ClientIDMode="Static" runat="server" Text="MOVE TO PDI"
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
            $('#MainContent_gv_OrderItems tr').find('td:eq(8)').each(function (e) {
                if ($('#MainContent_gv_OrderItems_txt_AvalQty_' + e).val() == "") {
                    errMsg = "Enter available qty <br/>";
                    $('#MainContent_gv_OrderItems_txt_AvalQty_' + e).css({ 'border': '2px solid #cc3210' });
                }
                else if (parseInt($('#MainContent_gv_OrderItems_txt_AvalQty_' + e).val()) > parseInt($(this).text().trim()) || parseInt($('#MainContent_gv_OrderItems_lblearmark_' + e).val()) > parseInt($(this).text().trim())) {
                    errMsg = "Available qty should be less than or equal to order qty<br/>";
                    $('#MainContent_gv_OrderItems_txt_AvalQty_' + e).css({ 'border': '2px solid #cc3210' });
                }
            });
            $('#MainContent_gv_OrderItems tr').find('td:eq(11)').each(function (e) {
                if ($('#MainContent_gv_OrderItems_txt_AvalQty_' + e).val() == "") {
                    errMsg = "Enter available qty <br/>";
                    $('#MainContent_gv_OrderItems_txt_AvalQty_' + e).css({ 'border': '2px solid #cc3210' });
                }
                else if (parseInt($('#MainContent_gv_OrderItems_txt_AvalQty_' + e).val()) < parseInt($(this).text().trim())) {
                    errMsg = "Available  qty should be less than or equal to Earmark qty<br/>";
                    $('#MainContent_gv_OrderItems_txt_AvalQty_' + e).css({ 'border': '2px solid #cc3210' });
                }
            });
            $('#MainContent_gv_OrderItems tr').find('td:eq(14)').each(function (e) {
                if ($(this).text().trim() != "") {
                    if ($('#MainContent_gv_OrderItems_txt_AvalRimQty_' + e).val() == "") {
                        errMsg += "Enter available rim qty <br/>";
                        $('#MainContent_gv_OrderItems_txt_AvalRimQty_' + e).css({ 'border': '2px solid #cc3210' });
                    }
                    else if (parseInt($('#MainContent_gv_OrderItems_txt_AvalRimQty_' + e).val()) > parseInt($(this).text().trim())) {
                        errMsg += "Available rim qty should be less than or equal to order rim qty<br/>";
                        $('#MainContent_gv_OrderItems_txt_AvalRimQty_' + e).css({ 'border': '2px solid #cc3210' });
                    }

                }
            });

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
        function splitOrderQty(e) {
            var ctrltxtSplitQty = e.id;
            var ctrllblSplitQty = ctrltxtSplitQty.replace('txt_AvalQty', 'lblnewprod');
            var ctrlOrderQty = ctrltxtSplitQty.replace('txt_AvalQty', 'lblQty');
            var ctrllblQtyErrMsg = ctrltxtSplitQty.replace('txt_AvalQty', 'lblQtyErrMsg');
            var ctrllblear = ctrltxtSplitQty.replace('txt_AvalQty', 'lblearmark');
            if (parseInt($('#' + ctrltxtSplitQty).val()) <= parseInt($('#' + ctrlOrderQty).html()) && parseInt($('#' + ctrltxtSplitQty).val()) >= parseInt($('#' + ctrllblear).html())) {
                $('#' + ctrllblSplitQty).html('');
                $('#' + ctrllblQtyErrMsg).html('');
                if ($('#' + ctrltxtSplitQty).val().length == 0)
                    $('#' + ctrllblSplitQty).html(parseInt($('#' + ctrlOrderQty).html()));
                else
                    $('#' + ctrllblSplitQty).html(parseInt($('#' + ctrlOrderQty).html()) - parseInt($('#' + ctrltxtSplitQty).val()));
            }
            else
                $('#' + ctrllblQtyErrMsg).html('Enter proper qty').css({ 'color': '#f00' });

        }
        function gotoPreviewDiv(ctrlID) {
            $('#' + ctrlID).css({ 'display': 'block' });
            $("html, body").stop().animate({
                scrollLeft: $('#' + ctrlID).offset().left,
                scrollTop: $('#' + ctrlID).offset().top
            }, 1200);
            //BindItem_PriceCalc();
        }
        function BindItem_PriceCalc() {
            $("input:text[id*='MainContent_gv_OrderItems_txt_AvalQty_']").each(function (e) {
                var ctrltxtSplitQty = this.id;
                var ctrllblnewprodQty = ctrltxtSplitQty.replace('txt_AvalQty', 'lblnewprod');
                var ctrlOrderQty = ctrltxtSplitQty.replace('txt_AvalQty', 'lblQty');
                $('#' + ctrllblnewprodQty).html(parseInt($('#' + ctrlOrderQty).html()) - parseInt($('#' + ctrltxtSplitQty).val()));
                $(this).attr({ 'disabled': 'disabled' }).css({ 'border': '0px', 'text-align': 'right' }); ;
            });
            $('#btn_SaveRecords').focus();
        }
    </script>
</asp:Content>
