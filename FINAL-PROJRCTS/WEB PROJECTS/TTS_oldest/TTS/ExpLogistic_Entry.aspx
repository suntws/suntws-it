<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExpLogistic_Entry.aspx.cs"
    MasterPageFile="~/master.Master" Inherits="TTS.ExpLogistic_Entry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/cotsScript.js" type="text/javascript"></script>
    <link href="Styles/datemonthyear.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #tb_TransitDetails th
        {
            text-align: right;
            font-weight: normal;
            width: 115px;
        }
        #tb_TransitDetails td
        {
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        <asp:Label runat="server" ID="lblPageHead" ClientIDMode="Static" Text=""></asp:Label>
        Logistics Tentative Schedule
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="background-color: #f6fbff; width: 100%;
            border-color: White; border-collapse: separate;">
            <tr>
                <td>
                    <asp:GridView ID="gv_Orders" Width="100%" runat="server" AutoGenerateColumns="false"
                        RowStyle-Height="25px">
                        <HeaderStyle BackColor="#74bbf9" ForeColor="White" Font-Bold="true" Height="30px"
                            HorizontalAlign="Center" />
                        <Columns>
                            <asp:TemplateField HeaderText="CUSTOMER NAME" ItemStyle-Width="250px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatusCustName" Text='<%# Eval("custfullname") %>'></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdnStatusCustCode" Value='<%# Eval("CustCode") %>' />
                                    <asp:HiddenField runat="server" ID="hdnOrderStatus" Value='<%# Eval("OrderStatus") %>' />
                                    <asp:HiddenField runat="server" ID="hdnOrderID" Value='<%# Eval("ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ORDER REFERENCE NO" ItemStyle-Width="220px">
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
                            <asp:BoundField HeaderText="ORDER QTY" DataField="itemqty" ItemStyle-Width="40px" />
                            <asp:BoundField HeaderText="SHIPMENT TYPE" DataField="ShipmentType" ItemStyle-Width="45px" />
                            <asp:BoundField DataField="StatusText" HeaderText="STATUS" ItemStyle-Width="180px" />
                            <asp:TemplateField ItemStyle-Width="120px" HeaderText="ACTION">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkOrderSelection" CssClass="btn btn-success btn-xs" runat="server"
                                        OnClick="lnkOrderSelection_click" Text="Process Order">
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr id="div_Sub_OrderItems" style="display: none;">
                <td>
                    <table cellspacing="0" rules="all" border="1" style="background-color: #dcecfb; width: 100%;
                        border-color: White; border-collapse: separate;">
                        <tr>
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
                                <asp:DataList runat="server" ID="dlOrderMaster" RepeatColumns="1" RepeatDirection="Horizontal"
                                    RepeatLayout="Table" AlternatingItemStyle-BackColor="#A1C9D3" ItemStyle-BackColor="#C9DAC9"
                                    ItemStyle-VerticalAlign="Top" Width="1106px">
                                    <ItemTemplate>
                                        <table cellspacing="0" rules="all" border="1" style="background-color: #ecf6ff; border-color: White;
                                            border-collapse: separate; width: 100%;">
                                            <tr>
                                                <th>
                                                    Delivery Method
                                                </th>
                                                <td>
                                                    <%# Eval("DeliveryMethod")%>
                                                    -
                                                    <%# Eval("GodownName") %>
                                                    -
                                                    <%#((string)Eval("TransportDetails")).Replace("~", "<br/>")%>
                                                </td>
                                                <th>
                                                    Packing Method
                                                </th>
                                                <td>
                                                    <%# Eval("PackingMethod") %>
                                                    <%# Eval("PackingOthers") %>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>
                                                    Country Of Destination
                                                </th>
                                                <td>
                                                    <%# Eval("CountryOfDestination") %>
                                                </td>
                                                <th>
                                                    Final Destination
                                                </th>
                                                <td>
                                                    <%# Eval("FinalDestination")%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>
                                                    Bill To
                                                </th>
                                                <td>
                                                    <%# Bind_BillingAddress(Eval("BillID").ToString())%>
                                                </td>
                                                <th>
                                                    Ship To
                                                </th>
                                                <td>
                                                    <%# Bind_BillingAddress(Eval("ShipID").ToString())%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>
                                                    Special Instruction
                                                </th>
                                                <td>
                                                    <%# Eval("SplIns")%>
                                                </td>
                                                <th>
                                                    Special Notes
                                                </th>
                                                <td>
                                                    <%# Eval("SpecialRequset")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:DataList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:GridView runat="server" ID="gv_OrderSumValue" AutoGenerateColumns="true" Width="100%"
                                    HeaderStyle-BackColor="#166502" HeaderStyle-ForeColor="#ffffff" ShowFooter="true"
                                    FooterStyle-BackColor="#dfe0f3" RowStyle-HorizontalAlign="Right" RowStyle-VerticalAlign="Middle"
                                    FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr style="text-align: center;">
                            <td>
                                <asp:Label ID="lbl_PPC_RFDdate" runat="server" Text="" Font-Bold="true" ClientIDMode="Static"
                                    CssClass="lblCss" Font-Size="20px"></asp:Label>
                            </td>
                            <td>
                                <asp:LinkButton runat="server" ID="lnkSkipTentative" ClientIDMode="Static" Text=""
                                    OnClick="lnkSkipTentative_Click"></asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div id="div_Revise" style="width: 100%; display: none;">
                                    <table id="tbl_entry" cellspacing="0" rules="all" border="1" style="background-color: #ecf6ff;
                                        border-color: White; border-collapse: separate; width: 100%;">
                                        <!--Default Cntrls -->
                                        <tr>
                                            <td style="width: 25%; text-align: center;">
                                                <span class="spanCss" style="color: #8e5e1a;">Category</span>
                                            </td>
                                            <td style="width: 25%; text-align: center;">
                                                <span class="spanCss" style="color: #8e5e1a;">With Promotion(-3 Days)</span>
                                            </td>
                                            <td style="width: 25%; text-align: center;">
                                                <span class="spanCss" style="color: #8e5e1a;">With Delay(+3 Days)</span>
                                            </td>
                                            <td style="width: 25%; text-align: center;">
                                                <span class="spanCss" style="color: #8e5e1a;">With Delay(+7 Days)</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span class="spanCss">RFD</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_RFDdate_Minus3" runat="server" Height="25px" ClientIDMode="Static"
                                                    CssClass="form-control" Width="100px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_RFDdate_Plus3" runat="server" Height="25px" ClientIDMode="Static"
                                                    CssClass="form-control" Width="100px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_RFDdate_Plus7" runat="server" Height="25px" ClientIDMode="Static"
                                                    CssClass="form-control" Width="100px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span class="spanCss">Gate Open</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_GateOpen_Minus3" runat="server" Height="25px" ClientIDMode="Static"
                                                    CssClass="form-control" Width="100px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_GateOpen_Plus3" runat="server" Height="25px" ClientIDMode="Static"
                                                    CssClass="form-control" Width="100px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_GateOpen_Plus7" runat="server" Height="25px" ClientIDMode="Static"
                                                    CssClass="form-control" Width="100px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span class="spanCss">Gate Close </span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_GateClose_Minus3" runat="server" Height="25px" ClientIDMode="Static"
                                                    CssClass="form-control" Width="100px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_GateClose_Plus3" runat="server" Height="25px" ClientIDMode="Static"
                                                    CssClass="form-control" Width="100px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_GateClose_Plus7" runat="server" Height="25px" ClientIDMode="Static"
                                                    CssClass="form-control" Width="100px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <!--Looping Cntrls -->
                                        <!--Looping -1 -->
                                        <tr id="tr_loop1_Etd">
                                            <td>
                                                <span class="spanCss">ETD </span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_ETDdate_Minus3_c1" runat="server" Height="25px" ClientIDMode="Static"
                                                    CssClass="form-control" Width="100px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_ETDdate_Plus3_c1" runat="server" Height="25px" ClientIDMode="Static"
                                                    CssClass="form-control" Width="100px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_ETDdate_Plus7_c1" runat="server" Height="25px" ClientIDMode="Static"
                                                    CssClass="form-control" Width="100px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="tr_loop1_EtdPort">
                                            <td>
                                                <span class="spanCss">ETD Port</span>
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddl_ETDPort_Minus3_c1" Height="25px" CssClass="form-control"
                                                    ClientIDMode="Static">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddl_ETDPort_Plus3_c1" Height="25px" CssClass="form-control"
                                                    ClientIDMode="Static">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddl_ETDPort_Plus7_c1" Height="25px" CssClass="form-control"
                                                    ClientIDMode="Static">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr id="tr_loop1_Eta">
                                            <td>
                                                <span class="spanCss">ETA </span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_ETAdate_Minus3_c1" runat="server" Height="25px" ClientIDMode="Static"
                                                    CssClass="form-control" Width="100px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_ETAdate_Plus3_c1" runat="server" Height="25px" ClientIDMode="Static"
                                                    CssClass="form-control" Width="100px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_ETAdate_Plus7_c1" runat="server" Height="25px" ClientIDMode="Static"
                                                    CssClass="form-control" Width="100px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="tr_loop1_EtaPort">
                                            <td>
                                                <span class="spanCss">ETA Port</span>
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddl_ETAPort_Minus3_c1" Height="25px" CssClass="form-control"
                                                    ClientIDMode="Static">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddl_ETAPort_Plus3_c1" Height="25px" CssClass="form-control"
                                                    ClientIDMode="Static">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddl_ETAPort_Plus7_c1" Height="25px" CssClass="form-control"
                                                    ClientIDMode="Static">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr id="tr_loop1_Vessel">
                                            <td>
                                                <span class="spanCss">Vessel Name & No</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Vessel_Minus3_c1" runat="server" Height="25px" ClientIDMode="Static"
                                                    CssClass="form-control"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Vessel_Plus3_c1" runat="server" Height="25px" ClientIDMode="Static"
                                                    CssClass="form-control"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Vessel_Plus7_c1" runat="server" Height="25px" ClientIDMode="Static"
                                                    CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <!--Looping -2 -->
                                        <tr id="tr_loop2_Etd" style="display: none; background-color: #e0fb8f;">
                                            <td>
                                                <span class="spanCss">ETD </span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_ETDdate_Minus3_c2" runat="server" Height="25px" ClientIDMode="Static"
                                                    CssClass="form-control" Width="100px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_ETDdate_Plus3_c2" runat="server" Height="25px" ClientIDMode="Static"
                                                    CssClass="form-control" Width="100px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_ETDdate_Plus7_c2" runat="server" Height="25px" ClientIDMode="Static"
                                                    CssClass="form-control" Width="100px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="tr_loop2_EtdPort" style="display: none; background-color: #e0fb8f;">
                                            <td>
                                                <span class="spanCss">ETD Port</span>
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddl_ETDPort_Minus3_c2" Height="25px" CssClass="form-control"
                                                    ClientIDMode="Static">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddl_ETDPort_Plus3_c2" Height="25px" CssClass="form-control"
                                                    ClientIDMode="Static">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddl_ETDPort_Plus7_c2" Height="25px" CssClass="form-control"
                                                    ClientIDMode="Static">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr id="tr_loop2_Eta" style="display: none; background-color: #e0fb8f;">
                                            <td>
                                                <span class="spanCss">ETA </span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_ETAdate_Minus3_c2" runat="server" Height="25px" ClientIDMode="Static"
                                                    CssClass="form-control" Width="100px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_ETAdate_Plus3_c2" runat="server" Height="25px" ClientIDMode="Static"
                                                    CssClass="form-control" Width="100px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_ETAdate_Plus7_c2" runat="server" Height="25px" ClientIDMode="Static"
                                                    CssClass="form-control" Width="100px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="tr_loop2_EtaPort" style="display: none; background-color: #e0fb8f;">
                                            <td>
                                                <span class="spanCss">ETA Port</span>
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddl_ETAPort_Minus3_c2" Height="25px" CssClass="form-control"
                                                    ClientIDMode="Static">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddl_ETAPort_Plus3_c2" Height="25px" CssClass="form-control"
                                                    ClientIDMode="Static">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddl_ETAPort_Plus7_c2" Height="25px" CssClass="form-control"
                                                    ClientIDMode="Static">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr id="tr_loop2_Vessel" style="display: none; background-color: #e0fb8f;">
                                            <td>
                                                <span class="spanCss">Vessel Name & No</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Vessel_Minus3_c2" runat="server" Height="25px" ClientIDMode="Static"
                                                    CssClass="form-control"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Vessel_Plus3_c2" runat="server" Height="25px" ClientIDMode="Static"
                                                    CssClass="form-control"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Vessel_Plus7_c2" runat="server" Height="25px" ClientIDMode="Static"
                                                    CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <!--Looping -3 -->
                                        <tr id="tr_loop3_Etd" style="display: none; background-color: #79ffa2;">
                                            <td>
                                                <span class="spanCss">ETD </span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_ETDdate_Minus3_c3" runat="server" Height="25px" ClientIDMode="Static"
                                                    CssClass="form-control" Width="100px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_ETDdate_Plus3_c3" runat="server" Height="25px" ClientIDMode="Static"
                                                    CssClass="form-control" Width="100px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_ETDdate_Plus7_c3" runat="server" Height="25px" ClientIDMode="Static"
                                                    CssClass="form-control" Width="100px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="tr_loop3_EtdPort" style="display: none; background-color: #79ffa2;">
                                            <td>
                                                <span class="spanCss">ETD Port</span>
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddl_ETDPort_Minus3_c3" Height="25px" CssClass="form-control"
                                                    ClientIDMode="Static">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddl_ETDPort_Plus3_c3" Height="25px" CssClass="form-control"
                                                    ClientIDMode="Static">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddl_ETDPort_Plus7_c3" Height="25px" CssClass="form-control"
                                                    ClientIDMode="Static">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr id="tr_loop3_Eta" style="display: none; background-color: #79ffa2;">
                                            <td>
                                                <span class="spanCss">ETA </span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_ETAdate_Minus3_c3" runat="server" Height="25px" ClientIDMode="Static"
                                                    CssClass="form-control" Width="100px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_ETAdate_Plus3_c3" runat="server" Height="25px" ClientIDMode="Static"
                                                    CssClass="form-control" Width="100px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_ETAdate_Plus7_c3" runat="server" Height="25px" ClientIDMode="Static"
                                                    CssClass="form-control" Width="100px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="tr_loop3_EtaPort" style="display: none; background-color: #79ffa2;">
                                            <td>
                                                <span class="spanCss">ETA Port</span>
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddl_ETAPort_Minus3_c3" Height="25px" CssClass="form-control"
                                                    ClientIDMode="Static">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddl_ETAPort_Plus3_c3" Height="25px" CssClass="form-control"
                                                    ClientIDMode="Static">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddl_ETAPort_Plus7_c3" Height="25px" CssClass="form-control"
                                                    ClientIDMode="Static">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr id="tr_loop3_Vessel" style="display: none; background-color: #79ffa2;">
                                            <td>
                                                <span class="spanCss">Vessel Name & No</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Vessel_Minus3_c3" runat="server" Height="25px" ClientIDMode="Static"
                                                    CssClass="form-control"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Vessel_Plus3_c3" runat="server" Height="25px" ClientIDMode="Static"
                                                    CssClass="form-control"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Vessel_Plus7_c3" runat="server" Height="25px" ClientIDMode="Static"
                                                    CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <!--Default Cntrls -->
                                        <tr>
                                            <td colspan="4">
                                                <div style="float: right; padding: 5px;">
                                                    <span class="spanCss">Add / Remove Additional Transit Details</span>&nbsp;&nbsp;
                                                    <asp:Button ID="btnAddLoop" ClientIDMode="Static" runat="server" Text="+ ADD" CssClass="btn btn-success btn-xs" />&nbsp;
                                                    <asp:Button ID="btnRemoveLoop" ClientIDMode="Static" runat="server" Text="- REMOVE"
                                                        CssClass="btn btn-danger btn-xs" />
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span class="spanCss">Transit Days</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_transitDays_Minus3" runat="server" Enabled="false" Height="25px"
                                                    ClientIDMode="Static" CssClass="form-control" Width="100px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_transitDays_Plus3" runat="server" Enabled="false" Height="25px"
                                                    ClientIDMode="Static" CssClass="form-control" Width="100px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_transitDays_Plus7" runat="server" Enabled="false" Height="25px"
                                                    ClientIDMode="Static" CssClass="form-control" Width="100px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span class="spanCss">Destination</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Destination_Minus3" runat="server" Height="25px" ClientIDMode="Static"
                                                    CssClass="form-control"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Destination_Plus3" runat="server" Height="25px" ClientIDMode="Static"
                                                    CssClass="form-control"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Destination_Plus7" runat="server" Height="25px" ClientIDMode="Static"
                                                    CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span class="spanCss">Any Comments</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Comments_Minus3" runat="server" Height="70px" ClientIDMode="Static"
                                                    CssClass="form-control" TextMode="MultiLine" MaxLength="500"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Comments_Plus3" runat="server" Height="70px" ClientIDMode="Static"
                                                    CssClass="form-control" TextMode="MultiLine" MaxLength="500"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Comments_Plus7" runat="server" Height="70px" ClientIDMode="Static"
                                                    CssClass="form-control" TextMode="MultiLine" MaxLength="500"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Label ID="lblErrMsg" runat="server" CssClass="lblCss" ClientIDMode="Static"
                                                    ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" align="center">
                                                <asp:Button ID="btn_SaveRecords" ClientIDMode="Static" runat="server" Text="Save Record"
                                                    CssClass="btn btn-success" OnClick="btn_SaveRecords_Click" OnClientClick="javascript:return cntrlSaveChck()" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <hr />
                                <div id="div_TransitDetails_Gv" style="width: 100%; display: none;">
                                    <asp:DataList runat="server" ID="dlTentativeList" RepeatColumns="2" RepeatDirection="Horizontal"
                                        RepeatLayout="Table" AlternatingItemStyle-BackColor="#A1C9D3" ItemStyle-BackColor="#C9DAC9"
                                        ItemStyle-VerticalAlign="Top" Width="100%">
                                        <ItemTemplate>
                                            <table id="tb_TransitDetails" cellspacing="0" rules="all" border="1" style="background-color: #ecf6ff;
                                                border-color: White; border-collapse: separate; width: 550px;">
                                                <tr>
                                                    <th>
                                                        <%# Eval("Dispatch_Time").ToString() == "Advance -3 days" ? "PROMOTE : " : "DELAY : "%>
                                                        IF RFD
                                                    </th>
                                                    <td>
                                                        <%# Eval("RFD") %>
                                                    </td>
                                                    <th>
                                                        BY
                                                    </th>
                                                    <td>
                                                        <%# Eval("UserName")%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        GATE OPEN
                                                    </th>
                                                    <td>
                                                        <%# Eval("GateOpen") %>
                                                    </td>
                                                    <th>
                                                        CLOSE
                                                    </th>
                                                    <td>
                                                        <%# Eval("GateClose")%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        ETD
                                                    </th>
                                                    <td>
                                                        <%# Eval("ETD_1")%>
                                                    </td>
                                                    <th>
                                                        PORT
                                                    </th>
                                                    <td>
                                                        <%# Eval("ETD_Port_1")%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        ETA
                                                    </th>
                                                    <td>
                                                        <%# Eval("ETA_1")%>
                                                    </td>
                                                    <th>
                                                        PORT
                                                    </th>
                                                    <td>
                                                        <%# Eval("ETA_Port_1")%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        VESSEL
                                                    </th>
                                                    <td colspan="3">
                                                        <%# Eval("Vessel_1") %>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        ADDITIONAL ETD
                                                    </th>
                                                    <td>
                                                        <%# Eval("ETD_2")%>
                                                    </td>
                                                    <th>
                                                        PORT
                                                    </th>
                                                    <td>
                                                        <%# Eval("ETD_Port_2")%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        ETA
                                                    </th>
                                                    <td>
                                                        <%# Eval("ETA_2")%>
                                                    </td>
                                                    <th>
                                                        PORT
                                                    </th>
                                                    <td>
                                                        <%# Eval("ETA_Port_2")%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        VESSEL
                                                    </th>
                                                    <td colspan="3">
                                                        <%# Eval("Vessel_2") %>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        ADDITIONAL ETD
                                                    </th>
                                                    <td>
                                                        <%# Eval("ETD_3")%>
                                                    </td>
                                                    <th>
                                                        PORT
                                                    </th>
                                                    <td>
                                                        <%# Eval("ETD_Port_3")%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        ETA
                                                    </th>
                                                    <td>
                                                        <%# Eval("ETA_3")%>
                                                    </td>
                                                    <th>
                                                        PORT
                                                    </th>
                                                    <td>
                                                        <%# Eval("ETA_Port_3")%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        VESSEL
                                                    </th>
                                                    <td colspan="3">
                                                        <%# Eval("Vessel_3") %>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        TRANSIT DAYS
                                                    </th>
                                                    <td>
                                                        <%# Eval("TransitDays")%>
                                                    </td>
                                                    <th>
                                                        DESTINATION
                                                    </th>
                                                    <td>
                                                        <%# Eval("Destination")%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4">
                                                        <%# Eval("Comments") %>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </asp:DataList>
                                    <div id="div_StatusChange">
                                        <span class="spanCss" style="text-align: left;">Enter Comments</span>
                                        <asp:TextBox ID="txt_StatusChangeComments" runat="server" ClientIDMode="Static" TextMode="MultiLine"
                                            Height="50px" Width="100%" CssClass="form-control"></asp:TextBox><br />
                                        <asp:Button ID="btn_StatusChange" ClientIDMode="Static" runat="server" Text="MOVE TO PDI"
                                            CssClass="btn btn-success" OnClick="btn_StatusChange_Click" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btn_ReviseRecords" runat="server" Text="Revise Transit" ClientIDMode="Static"
                                            OnClick="btn_ReviseRecords_click" CssClass="btn btn-info" />
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField ID="hdnCustCode" runat="server" Value="" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnOID" runat="server" Value="" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hdnRfdDate" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnSelectIndex" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdn_Days_Minus3" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdn_Days_Plus3" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdn_Days_Plus7" ClientIDMode="Static" Value="" />
    <script src="Scripts/datemonthyear.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var arr = ["tr_loop2_Etd", "tr_loop2_EtdPort", "tr_loop2_Eta", "tr_loop2_EtaPort", "tr_loop2_Vessel"];
            var arr1 = ["tr_loop3_Etd", "tr_loop3_EtdPort", "tr_loop3_Eta", "tr_loop3_EtaPort", "tr_loop3_Vessel"];
            //btn click to add Additonal transit details
            $('#btnAddLoop').click(function () {
                var boolView = false;
                if ($('#tr_loop2_Etd').is(":visible") == false) {
                    for (var i = 0; i < arr.length; i++) { $('#' + arr[i]).show(); }
                    boolView = true;
                }
                else if ($('#tr_loop3_Etd').is(":visible") == false) {
                    for (var i = 0; i < arr1.length; i++) { $('#' + arr1[i]).show(); }
                    boolView = true;
                }
                if (boolView == false)
                    alert('Maximum 2 transit details can be add');
                return false;
            });
            //btn click to remove Additonal transit details
            $('#btnRemoveLoop').click(function () {
                var boolView = false;
                if ($('#tr_loop3_Etd').is(":visible") == true) {
                    for (var i = 0; i < arr1.length; i++) { $('#' + arr1[i]).hide(); $('#' + arr1[i]).find("input[type='text']").val(""); }
                    boolView = true;
                }
                else if ($('#tr_loop2_Etd').is(":visible") == true) {
                    for (var i = 0; i < arr.length; i++) { $('#' + arr[i]).hide(); $('#' + arr[i]).find("input[type='text']").val(""); }
                    boolView = true;
                }
                if (boolView == false)
                    alert('Additional Transit Details Not Added');
                //alter transit days for Promotion(-3 days)
                Bind_TransitDays($("#txt_transitDays_Minus3"), $("#txt_ETDdate_Minus3_c1"), $("#txt_ETAdate_Minus3_c3"), $("#txt_ETAdate_Minus3_c2"), $("#txt_ETAdate_Minus3_c1"));
                //alter transit days for Delay(+3 days)
                Bind_TransitDays($("#txt_transitDays_Plus3"), $("#txt_ETDdate_Plus3_c1"), $("#txt_ETAdate_Plus3_c3"), $("#txt_ETAdate_Plus3_c2"), $("#txt_ETAdate_Plus3_c1"));
                //alter transit days for Delay(+7 days)
                Bind_TransitDays($("#txt_transitDays_Plus7"), $("#txt_ETDdate_Plus7_c1"), $("#txt_ETAdate_Plus7_c3"), $("#txt_ETAdate_Plus7_c2"), $("#txt_ETAdate_Plus7_c1"));

                return false;
            });

            var ppc_RfdDate = $('#hdnRfdDate').val(); var d1 = ppc_RfdDate.split('-'); var fullDate = new Date();
            var e = new Date("'" + d1[2] + '-' + d1[1] + '-' + d1[0] + "'"); var day = calcDaysBetween(fullDate, e); var days = day.toString().split('.');
            //function call to bind datepicker for Promotion(-3 days)
            Bind_DatePicker($("#txt_ETDdate_Minus3_c1"), $("#txt_ETAdate_Minus3_c1"), $("#txt_ETDdate_Minus3_c2"), $("#txt_ETAdate_Minus3_c2"), $("#txt_ETDdate_Minus3_c3"), $("#txt_ETAdate_Minus3_c3"),
                             $("#txt_GateOpen_Minus3"), $("#txt_GateClose_Minus3"), $("#txt_transitDays_Minus3"), $("#txt_RFDdate_Minus3"), (parseInt(days[0]) - 3 + "D"), (parseInt(days[0]) + "D"))
            //function call to bind datepicker for Delay(+3 days)
            Bind_DatePicker($("#txt_ETDdate_Plus3_c1"), $("#txt_ETAdate_Plus3_c1"), $("#txt_ETDdate_Plus3_c2"), $("#txt_ETAdate_Plus3_c2"), $("#txt_ETDdate_Plus3_c3"), $("#txt_ETAdate_Plus3_c3"),
                             $("#txt_GateOpen_Plus3"), $("#txt_GateClose_Plus3"), $("#txt_transitDays_Plus3"), $("#txt_RFDdate_Plus3"), (parseInt(days[0]) + 1 + "D"), (parseInt(days[0]) + 4 + "D"))
            //function call to bind datepicker for Delay(+7 days)
            Bind_DatePicker($("#txt_ETDdate_Plus7_c1"), $("#txt_ETAdate_Plus7_c1"), $("#txt_ETDdate_Plus7_c2"), $("#txt_ETAdate_Plus7_c2"), $("#txt_ETDdate_Plus7_c3"), $("#txt_ETAdate_Plus7_c3"),
                             $("#txt_GateOpen_Plus7"), $("#txt_GateClose_Plus7"), $("#txt_transitDays_Plus7"), $("#txt_RFDdate_Plus7"), (parseInt(days[0]) + 4 + "D"), (parseInt(days[0]) + 11 + "D"))
        });
        function Bind_DatePicker($ETD_1, $ETA_1, $ETD_2, $ETA_2, $ETD_3, $ETA_3, $gateopen, $gateclose, $TransitDays, $RFD, RFDMin, RFDMax) {
            // to bind RFD Date Textbox
            $RFD.datepicker({ minDate: RFDMin, maxDate: RFDMax, onSelect:
             function (selectedDate) {
                 var date2 = $RFD.datepicker('getDate');
                 date2.setDate(date2.getDate() + 4);
                 $gateopen.datepicker('option', 'maxDate', date2);
                 $gateopen.datepicker('option', "minDate", selectedDate);
             }
            }).keydown(function (e) {
                if (e.keyCode == 46 || e.keyCode == 8)
                    $(this).val("");
                else
                    e.keyWhich = 0; e.keyCode = 0; e.preventDefault();
            });

            // to bind Gate open Textbox
            $gateopen.datepicker({ minDate: "0D", maxDate: "+100D", onSelect: function (selectedDate) { var date2 = $gateopen.datepicker('getDate'); date2.setDate(date2.getDate() + 7); $gateclose.datepicker('option', 'maxDate', date2); $gateclose.datepicker('option', "minDate", selectedDate); }
            }).keydown(function (e) {
                if (e.keyCode == 46 || e.keyCode == 8)
                    $(this).val("");
                else
                    e.keyWhich = 0; e.keyCode = 0; e.preventDefault();
            });

            // to bind Gate Close Textbox 
            $gateclose.datepicker({ onSelect: function (selectedDate) { var date2 = $gateclose.datepicker('getDate'); date2.setDate(date2.getDate() + 6); $ETD_1.datepicker('option', 'maxDate', date2); $ETD_1.datepicker("option", "minDate", selectedDate); }
            }).keydown(function (e) {
                if (e.keyCode == 46 || e.keyCode == 8)
                    $(this).val("");
                else
                    e.keyWhich = 0; e.keyCode = 0; e.preventDefault();
            });

            // to bind ETD Textbox--loop 1
            $ETD_1.datepicker({ numberOfMonths: 1, minDate: 0, onClose: function (selectedDate) { $gateclose.datepicker('option', "maxDate", selectedDate); $ETA_1.datepicker("option", "minDate", selectedDate); }
            }).keydown(function (e) {
                if (e.keyCode == 46 || e.keyCode == 8)
                    $(this).val("");

                else
                    e.keyWhich = 0; e.keyCode = 0; e.preventDefault();
            });

            // to bind ETA Textbox--loop 1
            $ETA_1.datepicker({ defaultDate: "+1w", numberOfMonths: 1, minDate: 0, onClose: function (selectedDate) {
                var date2 = $ETA_1.datepicker('getDate'); date2.setDate(date2.getDate() + 15); $ETD_2.datepicker('option', 'maxDate', date2);
                $ETD_2.datepicker("option", "minDate", selectedDate);
            }
            }).keydown(function (e) {
                if (e.keyCode == 46 || e.keyCode == 8)
                    $(this).val("");
                else
                    e.keyWhich = 0; e.keyCode = 0; e.preventDefault();
            });

            // to bind ETD Textbox--loop 2
            $ETD_2.datepicker({ numberOfMonths: 1, minDate: 0, onClose: function (selectedDate) { $ETA_2.datepicker("option", "minDate", selectedDate); }
            }).keydown(function (e) { if (e.keyCode == 46 || e.keyCode == 8) $(this).val(""); else e.keyWhich = 0; e.keyCode = 0; e.preventDefault(); });

            // to bind ETA Textbox--loop 2
            $ETA_2.datepicker({ defaultDate: "+1w", numberOfMonths: 1, minDate: 0, onClose: function (selectedDate) {
                var date2 = $ETA_2.datepicker('getDate'); date2.setDate(date2.getDate() + 15); $ETD_3.datepicker('option', 'maxDate', date2);
                $ETD_3.datepicker("option", "minDate", selectedDate);
            }
            }).keydown(function (e) { if (e.keyCode == 46 || e.keyCode == 8) $(this).val(""); else e.keyWhich = 0; e.keyCode = 0; e.preventDefault(); });

            // to bind ETD Textbox--loop 3
            $ETD_3.datepicker({ numberOfMonths: 1, minDate: 0, onClose: function (selectedDate) { $ETA_3.datepicker("option", "minDate", selectedDate); }
            }).keydown(function (e) { if (e.keyCode == 46 || e.keyCode == 8) $(this).val(""); else e.keyWhich = 0; e.keyCode = 0; e.preventDefault(); });
            // to bind ETA Textbox--loop 3
            $ETA_3.datepicker({ defaultDate: "+1w", numberOfMonths: 1, minDate: 0 }).keydown(function (e) {
                if (e.keyCode == 46 || e.keyCode == 8) $(this).val(""); else e.keyWhich = 0; e.keyCode = 0; e.preventDefault();
            });
            // to bind Transit days for ETD-ETA
            $ETA_1.change(function () { Bind_TransitDays($TransitDays, $ETD_1, $ETA_3, $ETA_2, $ETA_1); });
            $ETA_2.change(function () { Bind_TransitDays($TransitDays, $ETD_1, $ETA_3, $ETA_2, $ETA_1); });
            $ETA_3.change(function () { Bind_TransitDays($TransitDays, $ETD_1, $ETA_3, $ETA_2, $ETA_1); });
        }
        function Bind_TransitDays($TransitDays, $ETD_1, $ETA_3, $ETA_2, $ETA_1) {
            var FromDate, ToDate;
            $TransitDays.val("");
            var FromDate = $ETD_1.datepicker('getDate');
            if ($ETA_3.val() != "")
                ToDate = $ETA_3.datepicker('getDate');
            else if ($ETA_2.val() != "")
                ToDate = $ETA_2.datepicker('getDate');
            else
                ToDate = $ETA_1.datepicker('getDate');
            if (FromDate && ToDate) {
                var days = calcDaysBetween(FromDate, ToDate);
                $TransitDays.val(days + 1);
            }
        }
        function calcDaysBetween(startDate, endDate) { var date = (endDate - startDate) / (1000 * 60 * 60 * 24); return date; }

        function cntrlSaveChck() {
            var errMsg = "";
            $('#lblErrMsg').html('');
            if ($('#div_Revise').is(":visible") == true) {
                if ($('#txt_RFDdate_Minus3').val() == "" && $('#txt_RFDdate_Plus3').val() == "" && $('#txt_RFDdate_Plus7').val() == "")
                    errMsg += "Choose RFD <br/>";
                if ($('#txt_GateOpen_Minus3').val() == "" && $('#txt_GateOpen_Plus3').val() == "" && $('#txt_GateOpen_Plus7').val() == "")
                    errMsg += "Choose Gate Open <br/>";
                if ($('#txt_GateClose_Minus3').val() == "" && $('#txt_GateClose_Plus3').val() == "" && $('#txt_GateClose_Plus7').val() == "")
                    errMsg += "Choose Gate Close <br/>";
                if ($('#txt_ETDdate_Minus3_c1').val() == "" && $('#txt_ETDdate_Plus3_c1').val() == "" && $('#txt_ETDdate_Plus7_c1').val() == "")
                    errMsg += "Choose ETD <br/>";
                if ($('#ddl_ETDPort_Minus3_c1 option:selected').text() == "CHOOSE" && $('#ddl_ETDPort_Plus3_c1 option:selected').text() == "CHOOSE"
                    && $('#ddl_ETDPort_Plus7_c1 option:selected').text() == "CHOOSE")
                    errMsg += "Choose ETD Port<br/>";
                if ($('#txt_ETAdate_Minus3_c1').val() == "" && $('#txt_ETAdate_Plus3_c1').val() == "" && $('#txt_ETAdate_Plus7_c1').val() == "")
                    errMsg += "Choose ETA <br/>";
                if ($('#ddl_ETAPort_Minus3_c1 option:selected').text() == "CHOOSE" && $('#ddl_ETAPort_Plus3_c1 option:selected').text() == "CHOOSE"
                   && $('#ddl_ETAPort_Plus7_c1 option:selected').text() == "CHOOSE")
                    errMsg += "Choose ETA Port <br/>";
                if ($('#txt_Vessel_Minus3_c1').val() == "" && $('#txt_Vessel_Plus3_c1').val() == "" && $('#txt_Vessel_Plus7_c1').val() == "")
                    errMsg += "Enter Vessel Name and No <br/>";
                if ($('#tr_loop2_Etd').is(":visible") == true) {
                    if ($('#txt_ETDdate_Minus3_c2').val() == "" && $('#txt_ETDdate_Plus3_c2').val() == "" && $('#txt_ETDdate_Plus7_c2').val() == "")
                        errMsg += "Choose Additional Transit ETD <br/>";
                    if ($('#ddl_ETDPort_Minus3_c2 option:selected').text() == "CHOOSE" && $('#ddl_ETDPort_Plus3_c2 option:selected').text() == "CHOOSE"
                       && $('#ddl_ETDPort_Plus7_c2 option:selected').text() == "CHOOSE")
                        errMsg += "Choose Additional Transit ETD Port<br/>";
                    if ($('#txt_ETAdate_Minus3_c2').val() == "" && $('#txt_ETAdate_Plus3_c2').val() == "" && $('#txt_ETAdate_Plus7_c2').val() == "")
                        errMsg += "Choose Additional Transit ETA <br/>";
                    if ($('#ddl_ETAPort_Minus3_c2 option:selected').text() == "CHOOSE" && $('#ddl_ETAPort_Plus3_c2 option:selected').text() == "CHOOSE"
                       && $('#ddl_ETAPort_Plus7_c2 option:selected').text() == "CHOOSE")
                        errMsg += "Choose Additional Transit ETA Port <br/>";
                    if ($('#txt_Vessel_Minus3_c2').val() == "" && $('#txt_Vessel_Plus3_c2').val() == "" && $('#txt_Vessel_Plus7_c2').val() == "")
                        errMsg += "Enter Additional Transit Vessel Name and No <br/>";
                }
                if ($('#tr_loop3_Etd').is(":visible") == true) {
                    if ($('#txt_ETDdate_Minus3_c3').val() == "" && $('#txt_ETDdate_Plus3_c3').val() == "" && $('#txt_ETDdate_Plus7_c3').val() == "")
                        errMsg += "Choose Another Additional Transit ETD <br/>";
                    if ($('#ddl_ETDPort_Minus3_c3 option:selected').text() == "CHOOSE" && $('#ddl_ETDPort_Plus3_c3 option:selected').text() == "CHOOSE"
                       && $('#ddl_ETDPort_Plus7_c3 option:selected').text() == "CHOOSE")
                        errMsg += "Choose Another Additional Transit ETD Port<br/>";
                    if ($('#txt_ETAdate_Minus3_c3').val() == "" && $('#txt_ETAdate_Plus3_c3').val() == "" && $('#txt_ETAdate_Plus7_c3').val() == "")
                        errMsg += "Choose Another Additional Transit ETA <br/>";
                    if ($('#ddl_ETAPort_Minus3_c3 option:selected').text() == "CHOOSE" && $('#ddl_ETAPort_Plus3_c3 option:selected').text() == "CHOOSE"
                       && $('#ddl_ETAPort_Plus7_c3 option:selected').text() == "CHOOSE")
                        errMsg += "Choose Another Additional Transit ETA Port <br/>";
                    if ($('#txt_Vessel_Minus3_c3').val() == "" && $('#txt_Vessel_Plus3_c3').val() == "" && $('#txt_Vessel_Plus7_c3').val() == "")
                        errMsg += "Enter Another Additional Transit Vessel Name and No <br/>";
                }
                if ($('#txt_Destination_Minus3').val() == "" && $('#txt_Destination_Plus3').val() == "" && $('#txt_Destination_Plus7').val() == "")
                    errMsg += "Enter Destination <br/>";
                var bool_Minus3 = true, bool_Plus3 = true, bool_Plus7 = true;
                $('#tbl_entry tr').find('td:eq(1)').find("input[type='text']").each(function () { if ($(this).val() != "") { bool_Minus3 = false; return false; } });
                $('#tbl_entry tr').find('td:eq(2)').find("input[type='text']").each(function () { if ($(this).val() != "") { bool_Plus3 = false; return false; } });
                $('#tbl_entry tr').find('td:eq(3)').find("input[type='text']").each(function () { if ($(this).val() != "") { bool_Plus7 = false; return false; } });

                if (bool_Minus3 == false) {
                    $('#tbl_entry tr').find('td:eq(1)').find("input[type='text']").each(function () {
                        if ($(this).val() == "" && (this.id).indexOf('_c3') > 0 && $('#tr_loop3_Etd').is(":visible") == true) {
                            errMsg += "Fill or clear all another additional transit details boxes in with advacne(-3 days)</br>"; return false;
                        }
                        else if ($(this).val() == "" && (this.id).indexOf('_c2') > 0 && $('#tr_loop2_Etd').is(":visible") == true) {
                            errMsg += "Fill or clear all additional transit details boxes in with advacne(-3 days)</br>"; return false;
                        }
                        else if ($(this).val() == "" && (this.id).indexOf('_c2') == -1 && (this.id).indexOf('_c3') == -1) {
                            errMsg += "Fill or clear all boxes in with advacne(-3 days)</br>"; return false;
                        }
                    });
                }
                if (bool_Plus3 == false) {
                    $('#tbl_entry tr').find('td:eq(2)').find("input[type='text']").each(function () {
                        if ($(this).val() == "" && (this.id).indexOf('_c3') > 0 && $('#tr_loop3_Etd').is(":visible") == true) {
                            errMsg += "Fill or clear all another additional transit details boxes in with delay(+3 days)</br>"; return false;
                        }
                        else if ($(this).val() == "" && (this.id).indexOf('_c2') > 0 && $('#tr_loop2_Etd').is(":visible") == true) {
                            errMsg += "Fill or clear all additional transit details boxes in with delay(+3 days)</br>"; return false;
                        }
                        else if ($(this).val() == "" && (this.id).indexOf('_c2') == -1 && (this.id).indexOf('_c3') == -1) {
                            errMsg += "Fill or clear all boxes in with delay(+3 days)</br>"; return false;
                        }
                    });
                }
                if (bool_Plus7 == false) {
                    $('#tbl_entry tr').find('td:eq(3)').find("input[type='text']").each(function () {
                        if ($(this).val() == "" && (this.id).indexOf('_c3') > 0 && $('#tr_loop3_Etd').is(":visible") == true) {
                            errMsg += "Fill or clear all another additional transit details boxes in with delay(+7 days)</br>"; return false;
                        }
                        else if ($(this).val() == "" && (this.id).indexOf('_c2') > 0 && $('#tr_loop2_Etd').is(":visible") == true) {
                            errMsg += "Fill or clear all additional transit details boxes in with delay(+7 days)</br>"; return false;
                        }
                        else if ($(this).val() == "" && (this.id).indexOf('_c2') == -1 && (this.id).indexOf('_c3') == -1) {
                            errMsg += "Fill or clear all boxes in with delay(+7 days)</br>"; return false;
                        }
                    });
                }

                if (errMsg.length > 0) {
                    $('#lblErrMsg').html(errMsg);
                    return false;
                }
                else {
                    $('#hdn_Days_Minus3').val($('#txt_transitDays_Minus3').val());
                    $('#hdn_Days_Plus3').val($('#txt_transitDays_Plus3').val());
                    $('#hdn_Days_Plus7').val($('#txt_transitDays_Plus7').val());
                    return true;
                }
            }
        }
        function clrCtrl() {
            $('input[type="text"]').val('');
        }
        function hideTwoCol() {
            $('#tbl_entry tr').find('td:eq(1)').css('display', 'none')
            $('#tbl_entry tr').find('td:eq(3)').css('display', 'none')
        }
    </script>
</asp:Content>
