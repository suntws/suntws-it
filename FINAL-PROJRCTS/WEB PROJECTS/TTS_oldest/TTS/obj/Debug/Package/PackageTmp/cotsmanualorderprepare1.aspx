<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="cotsmanualorderprepare1.aspx.cs" Inherits="TTS.cotsmanualorderprepare1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .Clrbutton
        {
            background-color: #4CAF50;
            border: none;
            color: white;
            padding: 10px 25px;
            text-align: center;
            text-decoration: none;
            display: inline-block;
            font-size: 16px;
            margin: 4px 2px;
            cursor: pointer;
        }
        .deletebutton
        {
            background-color: #E83D19;
            border: none;
            color: white;
            text-align: center;
            cursor: pointer;
            height: 25px;
            font-weight: bold;
            font-family: Times New Roman;
        }
        .tableCss
        {
            background-color: #dcecfb;
            width: 1100px;
            border-color: White;
        }
        .tableCss th
        {
            width: 120px;
            color: #008000;
            font-weight: normal;
            text-align: right;
        }
        .tableCss td
        {
            font-weight: bold;
            text-align: left;
        }
        .tableItems
        {
            background-color: #dcecfb;
            width: 1100px;
            border-color: White;
        }
        .tableItems th
        {
            width: 120px;
            color: #008000;
            font-weight: normal;
            text-align: center;
        }
        .spanCss
        {
            font-size: 12px;
            font-style: italic;
            width: 101px;
            float: left;
            text-align: right;
        }
        .lblCss
        {
            font-size: 12px;
            font-family: Times New Roman;
            font-weight: bold;
        }
        .itemheadercss
        {
            font-size: 12px;
            color: #614126;
            font-style: italic;
        }
        input[type=text]:disabled
        {
            background: #ffffff;
            cursor: no-drop;
        }
        select:disabled, textarea:disabled
        {
            background: #ffffff;
            cursor: no-drop;
        }
        input[type=checkbox]:disabled
        {
            cursor: no-drop;
        }
        input[type=radio]:disabled
        {
            cursor: no-drop;
        }
        input[type=button]:disabled
        {
            cursor: no-drop;
        }
        #tdRimData th
        {
            text-align: right;
            font-weight: normal;
            padding-right: 10px;
            background-color: #f1f1f1;
        }
        #tdRimData td
        {
            text-align: left;
            font-weight: bold;
            padding-left: 10px;
            background-color: #b7ff9a;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        DOMESTIC ORDER ENTRY
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" class="tableItems" style="background-color: #b6fb78;">
            <tr>
                <td colspan="3">
                    <asp:FormView ID="frmOrderMasterDetails" runat="server" Width="100%">
                        <ItemTemplate>
                            <table cellspacing="0" rules="all" border="1" class="tableCss">
                                <tr>
                                    <th>
                                        Customer
                                    </th>
                                    <td colspan="3">
                                        <asp:Label runat="server" ID="lblCustomerName" ClientIDMode="Static" Text='<%# Eval("custfullname")%>'></asp:Label>
                                    </td>
                                    <th>
                                        Order Ref No.
                                    </th>
                                    <td colspan="3">
                                        <%# Eval("OrderRefNo")%>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        Order Created
                                    </th>
                                    <td>
                                        <asp:Label runat="server" ID="lblOrderDate" ClientIDMode="Static" Text='<%# Eval("CreatedDate")%>'></asp:Label>
                                    </td>
                                    <th>
                                        Desired ShipDate
                                    </th>
                                    <td>
                                        <%# Eval("DesiredShipDate")%>
                                    </td>
                                    <th>
                                        Delivery Method
                                    </th>
                                    <td colspan="3">
                                        <%# Eval("DeliveryMethod")%>
                                        <%# Eval("GodownName").ToString() != "" ? (" - "+Eval("GodownName").ToString()):"" %>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        Expected Ship Date
                                    </th>
                                    <td>
                                        <%# Eval("ExpectedShipDate")%>
                                    </td>
                                    <th>
                                        Transport
                                    </th>
                                    <td>
                                        <%# Eval("TransportDetails")%>
                                    </td>
                                    <th>
                                        Freight Charges
                                    </th>
                                    <td>
                                        <%# Eval("FreightCharges")%>
                                    </td>
                                    <th>
                                        Packing Method
                                    </th>
                                    <td>
                                        <%# Eval("PackingMethod")%>
                                        <%# Eval("PackingOthers").ToString() !="" ? (" - " +Eval("PackingOthers").ToString()):"" %>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        Billing To
                                    </th>
                                    <td colspan="3">
                                        <%# Bind_BillingAddress(Eval("BillID").ToString())%>
                                    </td>
                                    <th>
                                        Ship To
                                    </th>
                                    <td colspan="3">
                                        <%# Bind_BillingAddress(Eval("ShipID").ToString())%>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        Special Instruction
                                    </th>
                                    <td colspan="3">
                                        <%# Eval("SplIns")%>
                                    </td>
                                    <th>
                                        Special Notes
                                    </th>
                                    <td colspan="3">
                                        <%# Eval("SpecialRequset")%>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:FormView>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Label ID="lblErrMsg1" runat="server" ClientIDMode="Static" ForeColor="Red" CssClass="lblCss"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:UpdatePanel ID="upt_Orderitems" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <table cellspacing="0" rules="all" border="1" style="width: 1100px; border-color: White;">
                                <tr>
                                    <td colspan="6" style="background-color: #fff; font-size: 15px; font-style: italic;
                                        font-weight: bold; color: #614126;">
                                        ADD ITEMS TO ORDER
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        <span class="itemheadercss">ORDER GRADE</span>
                                    </th>
                                    <td colspan="2">
                                        <asp:RadioButtonList runat="server" ID="rdo_GradeSelection" ClientIDMode="Static"
                                            Width="250px" RepeatColumns="5" RepeatDirection="Horizontal" CellSpacing="5"
                                            ToolTip="Select Grade">
                                            <asp:ListItem Text="A" Value="A" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="B" Value="B"></asp:ListItem>
                                            <asp:ListItem Text="C" Value="C"></asp:ListItem>
                                            <asp:ListItem Text="D" Value="D"></asp:ListItem>
                                            <asp:ListItem Text="E" Value="E"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <th>
                                        <span class="itemheadercss">PROCESS ID</span>
                                    </th>
                                    <td colspan="2" style="text-align: center; font-size: 14px;">
                                        <asp:Label ID="lbl_ProcessID" ClientIDMode="Static" runat="server" CssClass="lblCss"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="itemheadercss">Category</span><br />
                                        <asp:DropDownList ID="ddl_Category" ClientIDMode="Static" AutoPostBack="true" runat="server"
                                            Width="140px" Height="25px" OnSelectedIndexChanged="ddl_Category_SelectedIndexChanged">
                                            <asp:ListItem Text="CHOOSE" Value="CHOOSE"></asp:ListItem>
                                            <asp:ListItem Text="SOLID" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="POB" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="PNEUMATIC" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="SPLIT RIMS" Value="4"></asp:ListItem>
                                            <asp:ListItem Text="POB WHEEL" Value="5"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <span class="itemheadercss">Platform</span><br />
                                        <asp:DropDownList ID="ddl_Platform" ClientIDMode="Static" AutoPostBack="true" runat="server"
                                            Width="140px" Height="25px" OnSelectedIndexChanged="ddl_Platform_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <span class="itemheadercss">Brand</span><br />
                                        <asp:DropDownList ID="ddl_Brand" ClientIDMode="Static" AutoPostBack="true" runat="server"
                                            Width="140px" Height="25px" OnSelectedIndexChanged="ddl_Brand_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <span class="itemheadercss">Sidewall</span><br />
                                        <asp:DropDownList ID="ddl_Sidwall" ClientIDMode="Static" AutoPostBack="true" runat="server"
                                            Width="140px" Height="25px" OnSelectedIndexChanged="ddl_Sidwall_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <span class="itemheadercss">Type</span><br />
                                        <asp:DropDownList ID="ddl_Type" ClientIDMode="Static" AutoPostBack="true" runat="server"
                                            Width="140px" Height="25px" OnSelectedIndexChanged="ddl_Type_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <span class="itemheadercss">Tyre Size</span><br />
                                        <asp:DropDownList ID="ddl_Size" ClientIDMode="Static" AutoPostBack="true" runat="server"
                                            Width="140px" Height="25px" OnSelectedIndexChanged="ddl_Size_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="itemheadercss">Rim Width</span><br />
                                        <asp:DropDownList ID="ddl_RimWidth" ClientIDMode="Static" AutoPostBack="true" runat="server"
                                            Width="140px" Height="25px" OnSelectedIndexChanged="ddl_RimWidth_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <span class="itemheadercss">Disc %</span><br />
                                        <asp:TextBox ID="txt_Discount" ClientIDMode="Static" runat="server" Width="140px"
                                            MaxLength="5" Height="20px" onkeypress="return isNumberKey(event)" onblur="CalcForDiscount_Sheet_Basic('Discount')"></asp:TextBox>
                                        <asp:HiddenField ID="hdnDiscount" Value="" runat="server" ClientIDMode="Static" />
                                    </td>
                                    <td>
                                        <span class="itemheadercss">List Price</span><br />
                                        <asp:TextBox ID="txt_SheetPrice" ClientIDMode="Static" runat="server" Width="140px"
                                            MaxLength="8" Height="20px" onkeypress="return isNumberKey(event)" onblur="CalcForDiscount_Sheet_Basic('ListPrice')"></asp:TextBox>
                                    </td>
                                    <td>
                                        <span class="itemheadercss">Basic Price</span><br />
                                        <asp:TextBox ID="txt_BasicPrice" ClientIDMode="Static" runat="server" Width="140px"
                                            MaxLength="8" Height="20px" onkeypress="return isNumberKey(event)" onblur="CalcForDiscount_Sheet_Basic('BasicPrice')"></asp:TextBox>
                                    </td>
                                    <td>
                                        <span class="itemheadercss">Qty</span><br />
                                        <asp:TextBox ID="txt_Quantity" ClientIDMode="Static" runat="server" Width="140px"
                                            Height="20px" onkeypress="return isNumberWithoutDecimal(event)" MaxLength="4"></asp:TextBox>
                                    </td>
                                    <td>
                                        <span class="itemheadercss">Weight</span><br />
                                        <asp:TextBox ID="txt_Weight" ClientIDMode="Static" runat="server" Width="140px" Height="20px"
                                            onkeypress="return isNumberKey(event)" MaxLength="8"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chk_AddRimAssembly" runat="server" Text="Rim Assembly" ForeColor="Green"
                                            ClientIDMode="Static" AutoPostBack="true" OnCheckedChanged="chk_AddRimAssembly_Changed" />
                                    </td>
                                    <td colspan="5">
                                        <table cellspacing="0" rules="all" border="1" id="tr_rimAssembly" style="display: none;
                                            width: 100%;">
                                            <tr>
                                                <td>
                                                    <span class="itemheadercss">EDC No</span><br />
                                                    <asp:DropDownList runat="server" ID="ddl_EdcNo" Width="140px" Height="25px" ClientIDMode="Static"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddl_EdcNo_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <span class="itemheadercss">Rim Price</span><br />
                                                    <asp:TextBox ID="txt_RimPrice" runat="server" Width="140px" ClientIDMode="Static"
                                                        Height="20px" onkeypress="return isNumberKey(event)" MaxLength="8"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <span class="itemheadercss">Rim Weight</span><br />
                                                    <asp:TextBox ID="txt_RimWeight" runat="server" Width="140px" ClientIDMode="Static"
                                                        Height="20px" onkeypress="return isNumberKey(event)" MaxLength="8"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <span class="itemheadercss">Description</span><br />
                                                    <asp:TextBox ID="txt_DrawingNo" runat="server" Width="340px" ClientIDMode="Static"
                                                        Height="20px" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:FormView runat="server" ID="frmRimProcessID_Details" Width="100%">
                                                        <ItemTemplate>
                                                            <table cellspacing="0" rules="all" border="1" style="background-color: #ecf6ff; width: 100%;
                                                                border-color: White; line-height: 15px;" id="tdRimData">
                                                                <tr>
                                                                    <th>
                                                                        RIM SIZE
                                                                    </th>
                                                                    <td>
                                                                        <%# Eval("Rimsize") %>
                                                                    </td>
                                                                    <th>
                                                                        MOUNTING HOLES DIA
                                                                    </th>
                                                                    <td>
                                                                        <%# Eval("MHdia")%>
                                                                    </td>
                                                                    <th>
                                                                        FIXING HOLES DIA
                                                                    </th>
                                                                    <td>
                                                                        <%# Eval("FHdia")%>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <th>
                                                                        RIM TYPE
                                                                    </th>
                                                                    <td>
                                                                        <%# Eval("RimType") %>
                                                                        <%# Eval("NoofPiece").ToString() !="" ? " - ":"" %>
                                                                        <%# Eval("NoofPiece") %>
                                                                    </td>
                                                                    <th>
                                                                        MOUNTING HOLES TYPE
                                                                    </th>
                                                                    <td>
                                                                        <%# Eval("MHtype")%>
                                                                        <%# Eval("MHtype").ToString() == "COUNTERSINK SPHERICAL" && Eval("radius").ToString() != "" ? (" - RADIUS " + Eval("radius").ToString()) : ""%>
                                                                        <%# Eval("MHtype").ToString() == "COUNTERSINK CONICAL" && Eval("angle").ToString() != "" ? (" - ANGLE " + Eval("angle").ToString()) : ""%>
                                                                    </td>
                                                                    <th>
                                                                        FIXING HOLES TYPE
                                                                    </th>
                                                                    <td>
                                                                        <%# Eval("FHtype")%>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <th>
                                                                        TYRE CATEGORY
                                                                    </th>
                                                                    <td>
                                                                        <asp:Label runat="server" ID="lblTyreCategory" ClientIDMode="Static" Text='<%# Eval("TyreCategory") %>'></asp:Label>
                                                                    </td>
                                                                    <th>
                                                                        DISC OFFSET
                                                                    </th>
                                                                    <td>
                                                                        <%# Eval("DiscOffSet") %>
                                                                    </td>
                                                                    <th>
                                                                        BORE DIA
                                                                    </th>
                                                                    <td>
                                                                        <%# Eval("Boredia")%>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <th>
                                                                        PILOTED
                                                                    </th>
                                                                    <td>
                                                                        <%# Eval("Piloted") %>
                                                                    </td>
                                                                    <th>
                                                                        DISC THICKNESS
                                                                    </th>
                                                                    <td>
                                                                        <%# Eval("DiscThickness")%>
                                                                    </td>
                                                                    <th>
                                                                        PAINTING COLOR
                                                                    </th>
                                                                    <td>
                                                                        <%# Eval("PaintingColor")%>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <th>
                                                                        NO. OF MOUNTING HOLES
                                                                    </th>
                                                                    <td>
                                                                        <%# Eval("NoOfMH") %>
                                                                    </td>
                                                                    <th>
                                                                        NO. OF FIXING HOLES
                                                                    </th>
                                                                    <td>
                                                                        <%# Eval("NoofFH") %>
                                                                    </td>
                                                                    <th>
                                                                        WALL THICKNESS
                                                                    </th>
                                                                    <td>
                                                                        <%# Eval("WallThickness")%>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <th>
                                                                        MOUNTING HOLES PCD
                                                                    </th>
                                                                    <td>
                                                                        <%# Eval("MHpcd")%>
                                                                    </td>
                                                                    <th>
                                                                        FIXING HOLES PCD
                                                                    </th>
                                                                    <td>
                                                                        <%# Eval("FHpcd") %>
                                                                    </td>
                                                                    <th>
                                                                        EDC-NO/ PROCESS-ID
                                                                    </th>
                                                                    <td>
                                                                        <%# Eval("EDCNO") %>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                    </asp:FormView>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblErrMsg2" runat="server" ClientIDMode="Static" ForeColor="Red" CssClass="lblCss"></asp:Label>
                </td>
                <td style="text-align: center;">
                    <asp:Button ID="btnAddItem" runat="server" Text="ADD ITEM" ClientIDMode="Static"
                        CssClass="deletebutton" BackColor="#c012cc" OnClientClick="javascript:return cntrlAddItems();"
                        OnClick="btnAddItem_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:GridView ID="gv_Addeditems" runat="server" AutoGenerateColumns="false" Width="1100px"
                        OnRowDeleting="gv_Addeditems_RowDeleting" ClientIDMode="Static" ShowFooter="true"
                        FooterStyle-Font-Bold="true">
                        <HeaderStyle BackColor="#a1ccf3" Font-Bold="true" Height="22px" HorizontalAlign="Center" />
                        <Columns>
                            <asp:TemplateField HeaderText="Category" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <%# Eval("category") %>
                                    <%# Eval("AssyRimstatus").ToString() == "True" ? " (ASSY)" : ""%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Config" HeaderText="Platform" />
                            <asp:BoundField DataField="brand" HeaderText="Brand" />
                            <asp:BoundField DataField="sidewall" HeaderText="Sidewall" />
                            <asp:BoundField DataField="tyretype" HeaderText="Type" />
                            <asp:BoundField DataField="tyresize" HeaderText="Tyre Size" />
                            <asp:BoundField DataField="rimsize" HeaderText="Rim" />
                            <asp:BoundField DataField="SheetPrice" HeaderText="List Price" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="Discount" HeaderText="Disc" />
                            <asp:BoundField DataField="listprice" HeaderText="Basic Price" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="itemqty" HeaderText="Qty" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="unitpricepdf" HeaderText="Tot Tyre Price" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="totalfwt" HeaderText="Fwt" ItemStyle-HorizontalAlign="Right" />
                            <asp:TemplateField HeaderText="Rim Price" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <%# Eval("Rimunitprice").ToString() == "0.00" ? "" : Eval("Rimunitprice").ToString()%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rim Qty" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <%# Eval("Rimitemqty").ToString() == "0" ? "" : Eval("Rimitemqty").ToString()%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tot Rim Price" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <%# Eval("Rimpricepdf").ToString() == "0.00" ? "" : Eval("Rimpricepdf").ToString()%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tot Rim Wt" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <%# Eval("totalRimWt").ToString() == "0.00" ? "" : Eval("totalRimWt").ToString()%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="EDC No">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblEdcNo" Text='<%# Eval("EdcNo") %>'></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdnAssyStatus" Value='<%# Eval("AssyRimstatus") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hdn_ProcessID" Value='<%# Eval("processid") %>' />
                                    <asp:Button ID="btnDeleteItem" runat="server" Text="Delete" ClientIDMode="Static"
                                        CssClass="deletebutton" CommandName="Delete" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle BackColor="#f7b26b" HorizontalAlign="Right" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td style="text-align: center;">
                    <asp:Button ID="btnSaveOrder" runat="server" Text="SAVE IN DRAFT" ClientIDMode="Static"
                        CssClass="deletebutton" BackColor="#09c6e4" OnClientClick="javascript:return cntrlSaveMoveOrder();"
                        OnClick="btnSaveOrder_Click" />
                </td>
                <td>
                    <asp:Label ID="lblErrMsg3" runat="server" ClientIDMode="Static" ForeColor="Red" CssClass="lblCss"></asp:Label>
                </td>
                <td style="text-align: center;">
                    <asp:Button ID="btnMoveOrder" runat="server" Text="ORDER SEND TO PLANT ASSIGN" ClientIDMode="Static"
                        CssClass="deletebutton" BackColor="#1fb90c" OnClientClick="javascript:return cntrlSaveMoveOrder();"
                        OnClick="btnMoveOrder_Click" />
                </td>
            </tr>
        </table>
        <asp:HiddenField runat="server" ID="hdnCustCode" ClientIDMode="Static" Value="" />
        <asp:HiddenField runat="server" ID="hdnOrderRefNo" ClientIDMode="Static" Value="" />
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#chk_AddRimAssembly').click(function () {
                if ($('#chk_AddRimAssembly').is(':checked'))
                    $('#tr_rimAssembly').fadeIn('slow');
                else
                    $('#tr_rimAssembly').fadeOut('slow');
            });
        });

        function DisableJathagam_Category() {
            DisableCntrls('#ddl_Platform');
            DisableCntrls('#ddl_Brand');
            DisableCntrls('#ddl_Sidwall');
            DisableCntrls('#ddl_Type');
            DisableCntrls('#ddl_Size');
            DisableCntrls('#ddl_RimWidth');
            DisableCntrls('#txt_Discount');
            DisableCntrls('#txt_SheetPrice');
            DisableCntrls('#txt_BasicPrice');
            DisableCntrls('#txt_Weight');
            $('#tr_rimAssembly').fadeIn('slow');
        }

        //function to disable cntrl
        function DisableCntrls(id) {
            $(id).prop('disabled', true);
            $(id).css({ 'cursor': 'no-drop', 'display': 'none' });
        }
        //function to calc for Discount,SheetPrice,Basic
        function CalcForDiscount_Sheet_Basic(category) {
            if (parseFloat($('#txt_Discount').val()) > parseFloat($("#hdnDiscount").val())) {
                alert('Your maximum discount limit is ' + $("#hdnDiscount").val());
                $('#txt_Discount').val($("#hdnDiscount").val());
            }
            calcListBasicDisc(category);
        }
        //function to calc for discount,sheetprice,basic textboxes
        function calcListBasicDisc(ctrlID) {
            if ($("#txt_Discount").val() == 'NaN') $("#txt_Discount").val('0');
            if ($("#txt_BasicPrice").val() == 'NaN') $("#txt_BasicPrice").val('0');
            if ($("#txt_SheetPrice").val() == 'NaN') $("#txt_SheetPrice").val('0');
            var custSheetPrice = parseFloat($('#txt_SheetPrice').val() != '' ? $('#txt_SheetPrice').val() : 0);
            var custBasicPrice = parseFloat($('#txt_BasicPrice').val != '' ? $('#txt_BasicPrice').val() : 0);
            var custDiscPer = parseFloat($('#txt_Discount').val != '' ? $('#txt_Discount').val() : 0);
            if (ctrlID == 'Discount' || ctrlID == 'ListPrice') {
                if (custDiscPer != 0 && custSheetPrice != 0)
                    $('#txt_BasicPrice').val(parseFloat((custSheetPrice - (custSheetPrice * (custDiscPer / 100)))).toFixed());
                else
                    $('#txt_BasicPrice').val(parseFloat((custSheetPrice)).toFixed());
            }
            else if (ctrlID == 'BasicPrice') {
                if (custDiscPer != 0 && custBasicPrice != 0)
                    $('#txt_SheetPrice').val(parseFloat((custBasicPrice / ((100 - custDiscPer) / 100))).toFixed());
                else
                    $('#txt_SheetPrice').val(parseFloat((custBasicPrice)).toFixed());
            }
        }
        //function to check for add items
        function cntrlAddItems() {
            var ErrMsg = '';
            $('#lblErrMsg2').html(''); $('#lblErrMsg3').html('');
            //check for null values in jathagam
            if ($('input:radio[id*=rdo_GradeSelection ]:checked').length == 0)
                ErrMsg += "Select Order Grade <br/>";
            if ($('#ddl_Category option:selected').val() == null || $('#ddl_Category option:selected').val() == "CHOOSE")
                ErrMsg += "Select Category <br/>";
            else if ($('#ddl_Category option:selected').val() == '1' || $('#ddl_Category option:selected').val() == '2' || $('#ddl_Category option:selected').val() == '3') {
                if ($('#ddl_Platform option:selected').val() == null || $('#ddl_Platform option:selected').val() == "CHOOSE")
                    ErrMsg += "Select Platform <br/>";
                if ($('#ddl_Brand option:selected').val() == null || $('#ddl_Brand option:selected').val() == "CHOOSE")
                    ErrMsg += "Select Brand <br/>";
                if ($('#ddl_Sidwall option:selected').val() == null || $('#ddl_Sidwall option:selected').val() == "CHOOSE")
                    ErrMsg += "Select Sidewall <br/>";
                if ($('#ddl_Type option:selected').val() == null || $('#ddl_Type option:selected').val() == "CHOOSE")
                    ErrMsg += "Select Type <br/>";
                if ($('#ddl_Size option:selected').val() == null || $('#ddl_Size option:selected').val() == "CHOOSE")
                    ErrMsg += "Select Tyre Size <br/>";
                if ($('#ddl_RimWidth option:selected').val() == null || $('#ddl_RimWidth option:selected').val() == "CHOOSE")
                    ErrMsg += "Select Rim Width <br/>";
                if ($('#txt_SheetPrice').val().length == 0 || parseFloat($('#txt_SheetPrice').val()) == 0)
                    ErrMsg += "Enter List Price <br/>";
                if ($('#txt_BasicPrice').val().length == 0 || parseFloat($('#txt_BasicPrice').val()) == 0)
                    ErrMsg += "Enter Basci Price <br/>";
                if ($('#txt_Weight').val().length == 0 || parseFloat($('#txt_Weight').val()) == 0)
                    ErrMsg += "Enter finished wt.<br/>";
                if ($('#lbl_ProcessID').html().length == 0)
                    ErrMsg += "Process-ID not available<br/>";
            }
            if ($('#txt_Quantity').val().length == 0 || parseInt($('#txt_Quantity').val()) <= 0)
                ErrMsg += "Enter item Quantity <br/>";
            if ($('#chk_AddRimAssembly').is(':checked') || $('#ddl_Category option:selected').val() == '4' || $('#ddl_Category option:selected').val() == '5') {
                $('#tr_rimAssembly').fadeIn('slow');
                if ($('#ddl_EdcNo option:selected').val() == null || $('#ddl_EdcNo option:selected').val() == "CHOOSE")
                    ErrMsg += "Select Edc No<br/>";
                if ($('#txt_RimPrice').val().length == 0 || parseFloat($('#txt_RimPrice').val()) == 0)
                    ErrMsg += "Enter Rim Price <br/>";
                if ($('#txt_RimWeight').val().length == 0 || parseFloat($('#txt_RimWeight').val()) == 0)
                    ErrMsg += "Enter Rim Weight <br/>";
                if ($('#txt_DrawingNo').val().length == 0)
                    ErrMsg += "Enter Rim Description<br/>";
            }
            if (ErrMsg.length > 0) {
                $('#lblErrMsg2').html(ErrMsg);
                return false;
            }
            else
                return true;
        }
        //function to save final
        function cntrlSaveMoveOrder() {
            var ErrMsg = '';
            $('#lblErrMsg3').html('');
            if ($('#gv_Addeditems tr').length == 0)
                ErrMsg += " Add atleast one items to continue!!!.... <br/>";
            if (ErrMsg.length > 0) {
                $('#lblErrMsg3').html(ErrMsg);
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
