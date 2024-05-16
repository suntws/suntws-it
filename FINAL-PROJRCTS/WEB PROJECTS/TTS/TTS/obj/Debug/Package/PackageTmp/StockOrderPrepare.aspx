<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="StockOrderPrepare.aspx.cs" Inherits="TTS.StockOrderPrepare" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/datemonthyear.css" rel="stylesheet" type="text/css" />
    <link href="Styles/popupBox.css" rel="stylesheet" type="text/css" />
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        <asp:Label runat="server" ID="lblPageHead" ClientIDMode="Static" Text=""></asp:Label>
        STOCK ORDER PREPARE
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <div id="div_master" style="background-color: #f5fdcb;">
            <table runat="server" cellspacing="0" rules="all" border="0" style="width: 100%;">
                <tr>
                    <th>
                        CUSTOMER
                    </th>
                    <td colspan="3">
                        <asp:DropDownList ID="ddl_StockCustomerName" ClientIDMode="Static" runat="server"
                            Width="500px" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddl_StockCustomerName_IndexChange">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th>
                        STOCK ORDER REF NO
                    </th>
                    <td>
                        <asp:TextBox ID="txt_StockOrderRefNo" ClientIDMode="Static" runat="server" Width="215px"
                            ToolTip="Enter Order Reference No" MaxLength="100" CssClass="form-control"></asp:TextBox>
                        <asp:Label ID="lbl_duplicaterefno" ClientIDMode="Static" runat="server" ForeColor="Red"
                            Font-Size="12px" CssClass="lblCss" Text=""></asp:Label>
                    </td>
                    <th>
                        PLANT
                    </th>
                    <td>
                        <asp:DropDownList ID="ddl_plant" ClientIDMode="Static" runat="server" Width="100px"
                            CssClass="form-control" ToolTip="Select Customer name">
                            <asp:ListItem>--SELECT--</asp:ListItem>
                            <asp:ListItem>MMN</asp:ListItem>
                            <asp:ListItem>PDK</asp:ListItem>
                            <asp:ListItem>SLTL</asp:ListItem>
                            <asp:ListItem>SITL</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th>
                        START DATE
                    </th>
                    <td>
                        <asp:TextBox ID="txt_StockStartDate" ClientIDMode="Static" runat="server" Width="150px"
                            CssClass="form-control" ToolTip="Select Start Date"></asp:TextBox>
                    </td>
                    <th>
                        END DATE
                    </th>
                    <td>
                        <asp:TextBox ID="txt_StockEndDate" ClientIDMode="Static" runat="server" Width="150px"
                            CssClass="form-control" ToolTip="Select End Date"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;" colspan="4">
                        <asp:Button ID="btnSaveCustDetails" runat="server" Text="SAVE & ADD ITEMS" ClientIDMode="Static"
                            OnClientClick="javascript:return ctrlValidation();" OnClick="btnSaveCustDetails_Click"
                            CssClass="btn btn-success" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="div_custorder" style="display: none; background-color: #defbf0;">
            <table style="width: 100%;">
                <tr>
                    <td colspan="7" style="background-color: #fff; font-size: 15px; font-style: italic;
                        font-weight: bold; color: #614126;">
                        ADD ITEMS TO ORDER!!
                    </td>
                </tr>
                <tr>
                    <td>
                        CATEGORY
                    </td>
                    <td>
                        PLATFORM
                    </td>
                    <td>
                        BRAND
                    </td>
                    <td>
                        SIDEWALL
                    </td>
                    <td>
                        TYPE
                    </td>
                    <td>
                        TYRE SIZE
                    </td>
                    <td>
                        RIM
                    </td>
                </tr>
                <tr style="background-color: #C2E79A;">
                    <td>
                        <asp:DropDownList ID="ddl_Category" ClientIDMode="Static" AutoPostBack="true" runat="server"
                            Width="150px" OnSelectedIndexChanged="ddl_Category_SelectedIndexChanged" CssClass="form-control">
                            <asp:ListItem Text="CHOOSE" Value="CHOOSE"></asp:ListItem>
                            <asp:ListItem Text="SOLID" Value="1"></asp:ListItem>
                            <asp:ListItem Text="POB" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlPlatform" ClientIDMode="Static" Width="150px"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlPlatform_IndexChange" CssClass="form-control">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlBrand" ClientIDMode="Static" Width="150px"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlBrand_IndexChange" CssClass="form-control">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlSidewall" ClientIDMode="Static" Width="140px"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlSidewall_IndexChange" CssClass="form-control">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlType" ClientIDMode="Static" Width="140px"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlType_IndexChange" CssClass="form-control">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlSize" ClientIDMode="Static" Width="140px"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlSize_IndexChange" CssClass="form-control">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlRim" ClientIDMode="Static" Width="120px"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlRim_IndexChange" CssClass="form-control">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr style="background-color: #C2E79A;">
                    <td colspan="2">
                        <span class="itemheadercss">PROCESS ID</span><br />
                        <asp:TextBox ID="txt_processid" runat="server" Width="140px" ClientIDMode="Static"
                            Enabled="false" CssClass="form-control"></asp:TextBox>
                    </td>
                    <td colspan="2">
                        <span class="itemheadercss">FINISHED WEIGHT</span><br />
                        <asp:TextBox ID="txt_finishWght" runat="server" Width="140px" ClientIDMode="Static"
                            Enabled="false" CssClass="form-control"></asp:TextBox>
                    </td>
                    <td colspan="3">
                        <span class="itemheadercss">QUANTITY</span><br />
                        <asp:TextBox ID="txt_Itemqty" runat="server" Width="140px" ClientIDMode="Static"
                            onkeypress="return isNumberWithoutDecimal(event)" CssClass="form-control"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                    </td>
                    <td style="text-align: center;" colspan="3">
                        <asp:Button ID="btnAddItem" runat="server" Text="ADD ITEM" ClientIDMode="Static"
                            OnClientClick="javascript:return cntrlAddItems();" OnClick="btnAddItem_Click"
                            CssClass="btn btn-success" />
                    </td>
                </tr>
                <tr>
                    <td colspan="7">
                        <asp:GridView ID="gv_Addeditems" runat="server" AutoGenerateColumns="false" Width="1100px"
                            ClientIDMode="Static" ShowFooter="true" FooterStyle-Font-Bold="true" OnRowDeleting="gv_Addeditems_RowDeleting"
                            OnRowEditing="gv_Addeditems_RowEditing" OnRowCancelingEdit="gv_Addeditems_RowCancelingEdit"
                            OnRowUpdating="gv_Addeditems_RowUpdating">
                            <HeaderStyle BackColor="#D1E67D" Font-Bold="true" Height="22px" HorizontalAlign="Center" />
                            <Columns>
                                <asp:TemplateField ItemStyle-CssClass="headerNone" HeaderStyle-CssClass="headerNone">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblSid" Text='<%#Eval("ItemID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="CATEGORY">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblCategory" Text='<%#Eval("Category") %>'></asp:Label></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="PLATFORM">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblPlatform" Text='<%#Eval("Config") %>'></asp:Label></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="BRAND">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblBrand" Text='<%#Eval("brand") %>'></asp:Label></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="SIDEWALL">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblSidewall" Text='<%#Eval("sidewall") %>'></asp:Label></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="TYRE TYPE">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblTyretype" Text='<%#Eval("tyretype") %>'></asp:Label></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="TYRE SIZE">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lbltyresize" Text='<%#Eval("tyresize") %>'></asp:Label></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="RIM">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblrimsize" Text='<%#Eval("rimsize") %>'></asp:Label></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="FWT">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblfinishedwt" Text='<%#Eval("finishedwt") %>'></asp:Label></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="QTY" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblChangeQty" Text='<%#Eval("itemqty") %>' Visible='<%# Eval("itemqty").ToString() == "" ? false : true%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox runat="server" ID="txtChangeQty" Width="70px" MaxLength="4" onkeypress="return isNumberWithoutDecimal(event)"
                                            CssClass="form-control" Text='<%# Eval("itemqty") %>'></asp:TextBox>
                                        <asp:HiddenField runat="server" ID="hdnitemqty" Value='<%# Eval("itemqty") %>' />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ACTION" ItemStyle-Width="220px" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Button runat="server" ID="btnEdit" Text="EDIT" CommandName="Edit" CssClass="btn btn-info" />
                                        <asp:Button ID="btnDeleteItem" runat="server" Text="DELETE" ClientIDMode="Static"
                                            CommandName="Delete" CssClass="btn btn-warning" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Button runat="server" ID="btnUpdate" Text="UPDATE" CommandName="Update" CssClass="btn btn-success" />
                                        <asp:Button runat="server" ID="btnCancel" Text="CANCEL" CommandName="Cancel" CssClass="btn btn-danger" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <FooterStyle BackColor="#E0E9C4" HorizontalAlign="Right" />
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </div>
        <div id="div_process">
            <table runat="server" cellspacing="0" rules="all" border="0" style="width: 100%;">
                <tr>
                    <th>
                        <asp:Label runat="server" ID="lblworkorderno" ClientIDMode="Static" Text="WORK ORDER NO:"
                            Font-Bold="true" Visible="false" Font-Size="12px"></asp:Label>
                    </th>
                    <td>
                        <asp:TextBox ID="txt_workorderNo" ClientIDMode="Static" runat="server" Width="150px"
                            Visible="false" CssClass="form-control"></asp:TextBox>
                    </td>
                    <th>
                        <asp:Label runat="server" ID="lblreviseno" ClientIDMode="Static" Text=" REVISE NO:"
                            Font-Bold="true" Visible="false" Font-Size="12px"></asp:Label>
                    </th>
                    <td>
                        <asp:TextBox ID="txt_stockreviseno" ClientIDMode="Static" runat="server" Width="115px"
                            MaxLength="50" Visible="false" Enabled="false" CssClass="form-control"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblErrMsg" runat="server" ClientIDMode="Static" ForeColor="Red"></asp:Label>
                        <asp:Button runat="server" ID="btnDraft" ClientIDMode="Static" Text="SAVE DRAFT"
                            OnClick="btnDraft_Click" CssClass="btn btn-info" Visible="false" />
                    </td>
                    <td>
                        <asp:Button ID="btnSaveOrder" runat="server" Text="" ClientIDMode="Static" OnClick="btnSaveOrder_Click"
                            CssClass="btn btn-success" Visible="false" />
                    </td>
                    <td>
                        <asp:LinkButton runat="server" ID="lnkStockWorkOrder" ClientIDMode="Static" Text=""
                            Font-Bold="true" OnClick="lnkStockWorkOrder_Click"></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdnCustCode" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnStockorderid" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnPlant" ClientIDMode="Static" Value="" />
    <script src="Scripts/datemonthyear.js" type="text/javascript"></script>
    <script src="Scripts/cotsScript.js" type="text/javascript"></script>
    <script type="text/javascript">
        $('#txt_StockStartDate').change(function () {
            $("#txt_StockEndDate").datepicker("destroy");
            $('#txt_StockEndDate').val("");
            var date2 = $('#txt_StockStartDate').datepicker('getDate');
            date2.setDate(date2.getDate() + 1);
            var date3 = $('#txt_StockStartDate').datepicker('getDate');
            date3.setDate(date3.getDate() + 120);
            $("#txt_StockEndDate").datepicker({ minDate: date2, maxDate: date3 });
            $("#txt_StockEndDate").datepicker("widget");
        });
        $('#ddl_plant').click(function () {
            $('#hdnPlant').val($('#ddl_plant option:selected').text());
        });
        $(function () { $("#txt_StockStartDate").datepicker({ minDate: "+0D", maxDate: "+10D" }).keydown(function (e) { e.keyWhich = 0; e.keyCode = 0; e.preventDefault(); }); });
        function gotoPreviewDiv(ctrlID) {
            $('#' + ctrlID).css({ 'display': 'block' });
            $("html, body").stop().animate({ scrollLeft: $('#' + ctrlID).offset().left, scrollTop: $('#' + ctrlID).offset().top }, 1200);
        }
        function div_master() { $('#div_master *').prop('disabled', true).css('cursor', 'no-drop'); };
        function div_custorder() { $('#div_custorder *').prop('disabled', true).css('cursor', 'no-drop'); };

        function ctrlValidation() {
            var errmsg = '';
            if ($('#ddl_StockCustomerName option:selected').text() == '--SELECT--')
                errmsg += 'Choose StockCustomer Name </br>';
            if ($('#txt_StockOrderRefNo').val().length == 0)
                errmsg += 'Enter Reference No.</br>';
            if ($('#ddl_plant option:selected').text() == '--SELECT--')
                errmsg += 'Choose Plant </br>';
            if ($('#txt_StockStartDate').val().length == 0)
                errmsg += 'Enter Start date</br>';
            if ($('#txt_StockEndDate').val().length == 0)
                errmsg += 'Enter End date</br>';
            $('#lblErrMsg').html('');
            if (errmsg.length > 0) {
                $('#lblErrMsg').html(errmsg);
                return false;
            }
            else
                return true;
        }
        //function to check for add items
        function cntrlAddItems() {
            var ErrMsg = '';
            if ($('#ddl_Category option:selected').val() == null || $('#ddl_Category option:selected').val() == "CHOOSE")
                ErrMsg += "Select Category <br/>";
            if ($('#ddlPlatform option:selected').val() == null || $('#ddlPlatform option:selected').val() == "CHOOSE")
                ErrMsg += "Select Platform <br/>";
            if ($('#ddlBrand option:selected').val() == null || $('#ddlBrand option:selected').val() == "CHOOSE")
                ErrMsg += "Select Brand <br/>";
            if ($('#ddlSidewall option:selected').val() == null || $('#ddlSidewall option:selected').val() == "CHOOSE")
                ErrMsg += "Select Sidewall <br/>";
            if ($('#ddlType option:selected').val() == null || $('#ddlType option:selected').val() == "CHOOSE")
                ErrMsg += "Select Type <br/>";
            if ($('#ddlSize option:selected').val() == null || $('#ddlSize option:selected').val() == "CHOOSE")
                ErrMsg += "Select Size <br/>";
            if ($('#ddlRim option:selected').val() == null || $('#ddlRim option:selected').val() == "CHOOSE")
                ErrMsg += "Select RimSize <br/>";
            if ($('#txt_Itemqty').val().length == 0 || parseFloat($('#txt_Itemqty').val()) == 0)
                ErrMsg += "Enter item Quantity <br/>";
            $('#lblErrMsg').html('');
            if (ErrMsg.length > 0) {
                $('#lblErrMsg').html(ErrMsg);
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
