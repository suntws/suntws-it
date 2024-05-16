<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Exp_OrderRevise.aspx.cs"
    MasterPageFile="~/master.Master" Inherits="TTS.Exp_OrderRevise" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: center;" class="pageTitleHead">
        Export Orders -> Item Revise
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <asp:ScriptManager runat="server" ID="ScriptManager1">
    </asp:ScriptManager>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="background-color: #dcecfb; width: 100%;
            border-color: White; border-collapse: separate;">
            <tr>
                <td>
                    <asp:GridView runat="server" ID="gv_ReviseOrders" AutoGenerateColumns="false" Width="100%"
                        CssClass="gridcss" RowStyle-Height="30px">
                        <Columns>
                            <asp:TemplateField HeaderText="CUSTOMER" ItemStyle-Width="250px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatusCustName" Text='<%# Eval("custfullname") %>'></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdnCustCode" Value='<%# Eval("CustCode") %>' />
                                    <asp:HiddenField runat="server" ID="hdnCustCodeStd" Value='<%# Eval("CustCodeStd") %>' />
                                    <asp:HiddenField runat="server" ID="hdnOrderID" Value='<%# Eval("ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ORDER REF NO." ItemStyle-Width="220px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOrderRefNo" Text='<%# Eval("OrderRefNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="ORDERD DATE" DataField="CompletedDate" ItemStyle-Width="70px" />
                            <asp:BoundField HeaderText="QTY" DataField="itemqty" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="right" />
                            <asp:BoundField DataField="ShipmentType" HeaderText="SHIPMENT TYPE" ItemStyle-Width="80px" />
                            <asp:BoundField DataField="Plant" HeaderText="PLANT" ItemStyle-Width="80px" />
                            <asp:TemplateField HeaderText="ACTION" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="lnkItemRevise" ClientIDMode="Static" Text="ITEM REVISE"
                                        OnClick="lnkItemRevise_Click" CssClass="btn btn-success btn-xs"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="div_Item" style="width: 100%; display: none;">
                        <table cellspacing="0" rules="all" border="1" style="background-color: #dcecfb; width: 100%;
                            border-color: White; border-collapse: separate;">
                            <tr style="text-align: center; background-color: #3c763d; font-size: 20px; color: #ffffff;">
                                <td>
                                    <asp:Label ID="lblSelectedCustomerName" ClientIDMode="Static" runat="server" Text=""
                                        CssClass="lblCss"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblSelectedOrderRefNo" ClientIDMode="Static" runat="server" Text=""
                                        CssClass="lblCss"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="upt_Orderitems" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                            <table cellspacing="0" rules="all" border="1" style="background-color: #dcecfb; width: 100%;
                                                border-color: White; border-collapse: separate;" id="tbItemCtrl">
                                                <tr>
                                                    <td style="text-align: center; font-size: 14px;" colspan="5">
                                                        <asp:Label ID="lbl_ProcessID" ClientIDMode="Static" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span class="itemheadercss">Category</span><br />
                                                        <asp:DropDownList ID="ddl_Category" ClientIDMode="Static" AutoPostBack="true" runat="server"
                                                            CssClass="form-control" Width="180px" OnSelectedIndexChanged="ddl_Category_SelectedIndexChanged">
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
                                                            CssClass="form-control" Width="180px" OnSelectedIndexChanged="ddl_Platform_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <span class="itemheadercss">Brand</span><br />
                                                        <asp:DropDownList ID="ddl_Brand" ClientIDMode="Static" AutoPostBack="true" runat="server"
                                                            CssClass="form-control" Width="180px" OnSelectedIndexChanged="ddl_Brand_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <span class="itemheadercss">Sidewall</span><br />
                                                        <asp:DropDownList ID="ddl_Sidwall" ClientIDMode="Static" AutoPostBack="true" runat="server"
                                                            CssClass="form-control" Width="180px" OnSelectedIndexChanged="ddl_Sidwall_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <span class="itemheadercss">Type</span><br />
                                                        <asp:DropDownList ID="ddl_Type" ClientIDMode="Static" AutoPostBack="true" runat="server"
                                                            CssClass="form-control" Width="180px" OnSelectedIndexChanged="ddl_Type_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span class="itemheadercss">Tyre Size</span><br />
                                                        <asp:DropDownList ID="ddl_Size" ClientIDMode="Static" AutoPostBack="true" runat="server"
                                                            CssClass="form-control" Width="180px" OnSelectedIndexChanged="ddl_Size_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <span class="itemheadercss">Rim Width</span><br />
                                                        <asp:DropDownList ID="ddl_RimWidth" ClientIDMode="Static" AutoPostBack="true" runat="server"
                                                            CssClass="form-control" Width="180px" OnSelectedIndexChanged="ddl_RimWidth_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <span class="itemheadercss">Basic Price</span><br />
                                                        <asp:TextBox ID="txt_BasicPrice" ClientIDMode="Static" runat="server" CssClass="form-control"
                                                            onkeypress="return isNumberKey(event)" Width="180px"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <span class="itemheadercss">Qty</span><br />
                                                        <asp:TextBox ID="txt_Quantity" ClientIDMode="Static" runat="server" CssClass="form-control"
                                                            onkeypress="return isNumberWithoutDecimal(event)" Width="180px"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <span class="itemheadercss">Weight</span><br />
                                                        <asp:TextBox ID="txt_Weight" ClientIDMode="Static" runat="server" CssClass="form-control"
                                                            Width="180px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left;">
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
                                                                        onkeypress="return isNumberKey(event)" Width="180px"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <span class="itemheadercss">Rim Weight</span><br />
                                                                    <asp:TextBox ID="txt_RimWeight" runat="server" ClientIDMode="Static" CssClass="form-control"
                                                                        onkeypress="return isNumberKey(event)" Width="180px"></asp:TextBox>
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
                                                            MaxLength="50" Width="180px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    <asp:Label ID="lblErrMsg3" runat="server" ClientIDMode="Static" ForeColor="Red"></asp:Label>
                                    <asp:Button ID="btnAddItem" runat="server" Text="ADD ITEM" ClientIDMode="Static"
                                        CssClass="btn btn-warning" Font-Bold="true" OnClientClick="javascript:return cntrlAddItems();"
                                        OnClick="btnAddItem_Click" />
                                    <span onclick="fnPageReload()" class="btn btn-info">CLEAR</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:GridView ID="gv_AddedItems" runat="server" AutoGenerateColumns="false" ClientIDMode="Static"
                                        ShowFooter="true" Width="100%" FooterStyle-Height="25px" FooterStyle-Font-Bold="true"
                                        CssClass="gridcss" OnRowDeleting="gv_Addeditems_RowDeleting">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Category" ItemStyle-Width="100px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCategory" runat="server" Text='<%# Eval("category") %>'></asp:Label>
                                                    <asp:Label ID="lblAssyRimStatus" runat="server" Text='  <%# Eval("AssyRimstatus").ToString() == "True" ? " (ASSY)" : ""%>'></asp:Label>
                                                    <asp:HiddenField runat="server" ID="hdnAssyStatus" Value='<%# Eval("AssyRimstatus") %>' />
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
                                                    <asp:Label ID="lblRimUnitPrice" runat="Server" Text='<%# Eval("Rimunitprice").ToString() == "0.00" ? "" : Eval("Rimunitprice").ToString()%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Rim Qty" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRimItemqty" runat="Server" Text='<%# Eval("Rimitemqty").ToString() == "0.00" ? "" : Eval("Rimitemqty").ToString()%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Rim Wt" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbltotalRimWt" runat="Server" Text='<%# Eval("Rimfinishedwt").ToString() == "0.00" ? "" : Eval("Rimfinishedwt").ToString()%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="EDC No">
                                                <ItemTemplate>
                                                    <%# Eval("EdcRimSize") %>
                                                    <asp:Label runat="server" ID="lblEdcNo" Text='<%# Eval("EdcNo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Additional Info">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblAdditionalInfo" Text='<%# Eval("AdditionalInfo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:HiddenField runat="server" ID="hdn_ProcessID" Value='<%# Eval("processid") %>' />
                                                    <asp:HiddenField runat="server" ID="hdn_RimDwg" Value='<%# Eval("RimDwg") %>' />
                                                    <asp:Button ID="btnDeleteItem" runat="server" Text="Delete" ClientIDMode="Static"
                                                        CssClass="btn btn-danger btn-xs" CommandName="Delete" />
                                                    <asp:Button ID="btnEditItem" runat="server" Text="Edit " ClientIDMode="Static" CssClass="btn btn-info btn-xs"
                                                        OnClick="btnEditItem_click" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle BackColor="#b0ceb0" HorizontalAlign="Right" />
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr id="divItemChange" style="display: none; text-align: left; vertical-align: middle;">
                                <td colspan="2">
                                    If item revises done, Click here for next process.
                                    <asp:Button runat="server" ID="btnItemChangeCompleted" Text="Revision Completed"
                                        CssClass="btnedit" OnClick="btnItemChangeCompleted_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField ID="hdnCustCode" runat="server" ClientIDMode="Static" Value="" />
    <asp:HiddenField ID="hdnPlant" runat="server" ClientIDMode="Static" Value="" />
    <asp:HiddenField ID="hdnItemChangeStatus" runat="server" ClientIDMode="Static" Value="" />
    <asp:HiddenField ID="hdnOID" runat="server" ClientIDMode="Static" Value="" />
    <asp:HiddenField ID="hdnAssyStatus" runat="server" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdn_Quantity" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        $(function () {
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

        function DisableCntrls(id) {
            $(id).prop('disabled', true);
            $(id).css({ 'cursor': 'no-drop', 'display': 'none' });
        }
        function CrtlEnableDispable(val1) {
            if (val1 == '1') {
                $('#tbItemCtrl').find("input,select").prop('disabled', true).css({ 'cursor': 'no-drop' })
                $('#txt_Quantity').prop('disabled', false).css({ 'cursor': 'default' })
            }
            else if (val1 == '0')
                $('#tbItemCtrl').find("input,select").prop('disabled', false).css({ 'cursor': 'default', 'display': 'block' })
        }
        //function to check for add items
        function cntrlAddItems() {
            var ErrMsg = '';
            $('#lblErrMsg3').html('');
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
                $('#lblErrMsg3').html(ErrMsg);
                return false;
            }
            else
                return true;
        }
        function fnPageReload() { window.location.href = window.location.href; }
        function gotoPreviewDiv(ctrlID) {
            $('#' + ctrlID).css({ 'display': 'block' });
            $("html, body").stop().animate({ scrollLeft: $('#' + ctrlID).offset().left, scrollTop: $('#' + ctrlID).offset().top }, 1200);
        }
    </script>
</asp:Content>
