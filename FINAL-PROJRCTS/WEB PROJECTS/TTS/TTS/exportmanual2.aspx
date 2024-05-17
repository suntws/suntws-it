<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="exportmanual2.aspx.cs" Inherits="TTS.exportmanual2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .form-control
        {
            cursor: pointer;
            position: relative;
            width: 151px;
            height: 25px;
        }
        .itemheadercss
        {
            font-size: 12px;
            color: #614126;
            font-weight: 700;
            font-style: italic;
        }
        input[type=text]:disabled
        {
            background: #ffffff;
            cursor: no-drop;
            border: 1px solid #e2e1e1;
        }
        select:disabled, textarea:disabled
        {
            background: #ffffff;
            cursor: no-drop;
            border: 1px solid #e2e1e1;
        }
        input[type=checkbox]:disabled
        {
            cursor: no-drop;
        }
        input[type=button]:disabled
        {
            cursor: no-drop;
        }
        #gv_AddedItems th
        {
            color: White;
            font-family: Times New Roman;
            font-size: 14px;
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
        EXPORT ORDER ENTRY
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <asp:FormView ID="frmOrderMasterDetails" runat="server" Width="100%">
            <ItemTemplate>
                <table cellspacing="0" rules="all" border="1" style="background-color: #dcecfb; width: 100%;
                    border-color: White; border-collapse: separate;">
                    <tr>
                        <th class="spanCss">
                            Customer
                        </th>
                        <td>
                            <asp:Label runat="server" ID="lblCustomerName" ClientIDMode="Static" Text='<%# Eval("custfullname")%>'></asp:Label>
                            <asp:HiddenField runat="server" ID="hdncustcodeStd" ClientIDMode="Static" Value='<%# Eval("custcodeStd") %>' />
                        </td>
                        <th class="spanCss">
                            Order Ref No.
                        </th>
                        <td>
                            <%# Eval("OrderRefNo")%>
                        </td>
                    </tr>
                    <tr>
                        <th class="spanCss">
                            Bill To
                        </th>
                        <td>
                            <%# Bind_BillingAddress(Eval("BillID").ToString(), false)%>
                        </td>
                        <th class="spanCss">
                            Ship To
                        </th>
                        <td>
                            <%# Bind_BillingAddress(Eval("ShipID").ToString(), true)%>
                        </td>
                    </tr>
                    <tr>
                        <th class="spanCss">
                            Special Instruction
                        </th>
                        <td>
                            <%# Eval("SplIns")%>
                        </td>
                        <th class="spanCss">
                            Special Notes
                        </th>
                        <td>
                            <%# Eval("SpecialRequset")%>
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
        </asp:FormView>
        <hr />
        <table cellspacing="0" rules="all" border="1" style="width: 100%; border-color: #fff;
            border-collapse: separate;">
            <tr style="text-align: center;">
                <td>
                    <asp:LinkButton runat="server" ID="lnkApplicableGrade" ClientIDMode="Static" Text=""
                        OnClick="lnkApplicableGrade_Click" Font-Bold="true"></asp:LinkButton>
                </td>
                <td>
                    <asp:LinkButton runat="server" ID="lnkProcessID" ClientIDMode="Static" Text="" OnClick="lnkProcessID_Click"
                        Font-Bold="true"></asp:LinkButton>
                </td>
            </tr>
        </table>
        <hr />
        <div id="div_Details">
            <div style="background-color: #3c763e; height: 20px; font-size: 15px; color: #fff;
                width: 100%; font-style: italic; font-weight: bold; text-align: center; border-radius: 5px;
                padding-top: 5px;">
                ADD ITEMS TO ORDER
            </div>
            <table cellspacing="0" rules="all" border="1" style="background-color: #a9f9a7; width: 100%;
                border-color: #fff; border-collapse: separate;">
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblErrMsg1" runat="server" ClientIDMode="Static" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:UpdatePanel ID="upt_Orderitems" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <table cellspacing="0" rules="all" border="1" style="width: 100%;">
                                    <tr>
                                        <td>
                                            <span class="itemheadercss">Category</span><br />
                                            <asp:DropDownList ID="ddl_Category" ClientIDMode="Static" AutoPostBack="true" runat="server"
                                                CssClass="form-control" OnSelectedIndexChanged="ddl_Category_SelectedIndexChanged">
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
                                                CssClass="form-control" OnSelectedIndexChanged="ddl_Platform_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <span class="itemheadercss">Brand</span><br />
                                            <asp:DropDownList ID="ddl_Brand" ClientIDMode="Static" AutoPostBack="true" runat="server"
                                                CssClass="form-control" OnSelectedIndexChanged="ddl_Brand_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <span class="itemheadercss">Sidewall</span><br />
                                            <asp:DropDownList ID="ddl_Sidwall" ClientIDMode="Static" AutoPostBack="true" runat="server"
                                                CssClass="form-control" OnSelectedIndexChanged="ddl_Sidwall_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <span class="itemheadercss">Type</span><br />
                                            <asp:DropDownList ID="ddl_Type" ClientIDMode="Static" AutoPostBack="true" runat="server"
                                                CssClass="form-control" OnSelectedIndexChanged="ddl_Type_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span class="itemheadercss">Tyre Size</span><br />
                                            <asp:DropDownList ID="ddl_Size" ClientIDMode="Static" AutoPostBack="true" runat="server"
                                                CssClass="form-control" OnSelectedIndexChanged="ddl_Size_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <span class="itemheadercss">Rim Width</span><br />
                                            <asp:DropDownList ID="ddl_RimWidth" ClientIDMode="Static" AutoPostBack="true" runat="server"
                                                CssClass="form-control" OnSelectedIndexChanged="ddl_RimWidth_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <span class="itemheadercss">Basic Price</span><br />
                                            <asp:TextBox ID="txt_BasicPrice" ClientIDMode="Static" runat="server" CssClass="form-control"
                                                onkeypress="return isNumberKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <span class="itemheadercss">Qty</span><br />
                                            <asp:TextBox ID="txt_Quantity" ClientIDMode="Static" runat="server" CssClass="form-control"
                                                onkeypress="return isNumberWithoutDecimal(event)" MaxLength="4"></asp:TextBox>
                                        </td>
                                        <td>
                                            <span class="itemheadercss">Weight</span><br />
                                            <asp:TextBox ID="txt_Weight" ClientIDMode="Static" runat="server" CssClass="form-control"
                                                onkeypress="return isNumberKey(event)" MaxLength="8"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left;">
                                            <asp:Label ID="lbl_ProcessID" ClientIDMode="Static" runat="server" Font-Bold="true"
                                                Font-Size="14px"></asp:Label>
                                            <br />
                                            <asp:CheckBox ID="chk_AddRimAssembly" runat="server" Text="Rim Assembly" ForeColor="Green"
                                                ClientIDMode="Static" AutoPostBack="true" OnCheckedChanged="chk_AddRimAssembly_Changed" />
                                        </td>
                                        <td colspan="3">
                                            <table cellspacing="0" rules="all" border="1" style="width: 100%; display: none;"
                                                id="tr_rimAssembly">
                                                <tr>
                                                    <td>
                                                        <span class="itemheadercss">EDC No</span><br />
                                                        <asp:DropDownList runat="server" ID="ddl_EdcNo" Width="140px" ClientIDMode="Static"
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddl_EdcNo_SelectedIndexChanged" CssClass="form-control">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <span class="itemheadercss">Rim Price</span><br />
                                                        <asp:TextBox ID="txt_RimPrice" runat="server" ClientIDMode="Static" CssClass="form-control"
                                                            onkeypress="return isNumberKey(event)" MaxLength="8"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <span class="itemheadercss">Rim Weight</span><br />
                                                        <asp:TextBox ID="txt_RimWeight" runat="server" ClientIDMode="Static" CssClass="form-control"
                                                            onkeypress="return isNumberKey(event)" MaxLength="8"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <span class="itemheadercss">Description</span><br />
                                                        <asp:TextBox ID="txt_DrawingNo" runat="server" CssClass="form-control" ClientIDMode="Static"
                                                            MaxLength="50"></asp:TextBox>
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
                                        <td>
                                            <span class="itemheadercss">Additional Info</span><br />
                                            <asp:TextBox runat="server" ID="txtAddInfo" ClientIDMode="Static" CssClass="form-control"
                                                MaxLength="50"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            AVAILABLE STOCK QTY
                                        </th>
                                        <th>
                                            <asp:Label runat="server" ID="lbl_StockMMN" ClientIDMode="Static" Text=""></asp:Label>
                                        </th>
                                        <th>
                                            <asp:Label runat="server" ID="lbl_StockPDK" ClientIDMode="Static" Text=""></asp:Label>
                                        </th>
                                        <th>
                                            <asp:Label runat="server" ID="lbl_StockSLTL" ClientIDMode="Static" Text=""></asp:Label>
                                        </th>
                                        <th>
                                            <asp:Label runat="server" ID="lbl_StockSITL" ClientIDMode="Static" Text=""></asp:Label>
                                        </th>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblErrMsg2" runat="server" ClientIDMode="Static" ForeColor="Red"></asp:Label>
                    </td>
                    <td style="text-align: center;">
                        <asp:Button ID="btnAddItem" runat="server" Text="ADD ITEM" ClientIDMode="Static"
                            CssClass="btn btn-warning" Font-Bold="true" OnClientClick="javascript:return cntrlAddItems();"
                            OnClick="btnAddItem_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:GridView ID="gv_AddedItems" runat="server" AutoGenerateColumns="false" OnRowDeleting="gv_Addeditems_RowDeleting"
                            ClientIDMode="Static" ShowFooter="true" RowStyle-Height="22px" Width="100%" RowStyle-BackColor="#ffffff"
                            FooterStyle-Height="25px" FooterStyle-Font-Bold="true">
                            <HeaderStyle BackColor="#74bbf9" ForeColor="White" Font-Bold="true" Height="25px"
                                HorizontalAlign="Center" />
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
                                <asp:BoundField DataField="listprice" HeaderText="Unit Price" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="itemqty" HeaderText="Qty" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="tyrewt" HeaderText="Tyre Wt" ItemStyle-HorizontalAlign="Right" />
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
                                <asp:TemplateField HeaderText="Rim Wt" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <%# Eval("Rimfinishedwt").ToString() == "0.00" ? "" : Eval("Rimfinishedwt").ToString()%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="EDC No">
                                    <ItemTemplate>
                                        <%# Eval("EdcRimSize") %>
                                        <asp:Label runat="server" ID="lblEdcNo" Text='<%# Eval("EdcNo") %>'></asp:Label>
                                        <asp:HiddenField runat="server" ID="hdnAssyStatus" Value='<%# Eval("AssyRimstatus") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="AdditionalInfo" HeaderText="Info" />
                                <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:HiddenField runat="server" ID="hdn_ProcessID" Value='<%# Eval("processid") %>' />
                                        <asp:Button ID="btnDeleteItem" runat="server" Text="Delete Item" ClientIDMode="Static"
                                            CssClass="btn btn-danger" CommandName="Delete" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <FooterStyle BackColor="#b0ceb0" HorizontalAlign="Right" />
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblErrMsg3" runat="server" ClientIDMode="Static" ForeColor="Red"></asp:Label>
                    </td>
                    <td>
                        <div style="text-align: center; width: 100%;">
                            <asp:Button ID="btnSaveOrder" runat="server" Text="SAVE IN DRAFT" ClientIDMode="Static"
                                CssClass="btn btn-info" Font-Bold="true" OnClientClick="javascript:return cntrlSaveMoveOrder();"
                                OnClick="btnSaveOrder_Click" />&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnMoveOrder" runat="server" Text="SAVE AND COMPLETE" ClientIDMode="Static"
                                CssClass="btn btn-success" Font-Bold="true" BackColor="#1fb90c" OnClientClick="javascript:return cntrlSaveMoveOrder();"
                                OnClick="btnMoveOrder_Click" />
                        </div>
                    </td>
                </tr>
            </table>
        </div>
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
            DisableCntrls('#txt_BasicPrice');
            DisableCntrls('#txt_Weight');
            $('#tr_rimAssembly').fadeIn('slow');
        }

        //function to disable cntrl
        function DisableCntrls(id) {
            $(id).prop('disabled', true);
            $(id).css({ 'cursor': 'no-drop', 'display': 'none' });
        }

        //function to check for add items
        function cntrlAddItems() {
            var ErrMsg = '';
            $('#lblErrMsg2').html(''); $('#lblErrMsg3').html('');
            //check for null values in jathagam
            if ($('#ddl_Category option:selected').val() == null || $('#ddl_Category option:selected').val() == "CHOOSE")
                ErrMsg += "Select Category <br/>";
            else if ($('#ddl_Category option:selected').val() == '1' || $('#ddlCategory option:selected').val() == '2' || $('#ddlCategory option:selected').val() == '3') {
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
                    ErrMsg += "Enter Description<br/>";
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
            if ($('#gv_AddedItems tr').length == 0)
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
