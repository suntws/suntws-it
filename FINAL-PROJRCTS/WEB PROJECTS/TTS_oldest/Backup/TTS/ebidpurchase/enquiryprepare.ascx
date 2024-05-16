<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="enquiryprepare.ascx.cs"
    Inherits="TTS.ebidpurchase.enquiryprepare" %>
<link href="../Styles/datemonthyear.css" rel="stylesheet" type="text/css" />
<style type="text/css">
    #overlay
    {
        visibility: hidden;
        position: absolute;
        left: 0px;
        top: 0px;
        width: 100%;
        height: 100%;
        z-index: 1000;
        background-image: url('images/background-trans.png');
    }
    
    #overlay .row
    {
        margin: auto auto;
        background-color: #fff;
        border: 1px solid #000;
        padding: 15px;
        text-align: left;
        position: absolute;
    }
    
    #close
    {
        float: right;
        display: inline-block;
        top: -20px;
        right: -20px;
        position: relative;
    }
    
    #close :hover
    {
        cursor: pointer;
    }
    .hide
    {
        display: none;
    }
    
    .show
    {
        display: block;
    }
    
    #btnSendToCustomer
    {
        margin-left: 50%;
        margin-top: 10px;
    }
</style>
<script type="text/javascript">
</script>
<div style="width: 1080px;">
    <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label>
    <div id="divEnquiryItem">
        <table cellspacing="0" rules="all" border="1" class="enquirypage">
            <tr style="background-color: #d7f785;">
                <th>
                    PRODUCT CATEGORY
                </th>
                <th>
                    DESCRIPTION
                </th>
                <th>
                    MEASUREMENT
                </th>
                <th>
                    QTY
                </th>
                <th>
                </th>
            </tr>
            <tr id="trGroupControls">
                <td>
                    <asp:DropDownList runat="server" ID="ddlProductCategory" ClientIDMode="Static" CssClass="ddlCss"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlProductCategory_IndexChange">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlProductDesc" ClientIDMode="Static" CssClass="ddlCss"
                        Width="400px" AutoPostBack="true" OnSelectedIndexChanged="ddlProductDesc_IndexChange">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlMeasurement" ClientIDMode="Static" CssClass="ddlCss"
                        Width="100px" AutoPostBack="true" OnSelectedIndexChanged="ddlMeasurement_IndexChange">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtProductQty" ClientIDMode="Static" Text="" CssClass="txtCss"
                        Width="50px"></asp:TextBox>
                </td>
                <td>
                    <asp:HiddenField runat="server" ID="hdnEnqProductID" ClientIDMode="Static" Value="" />
                    <asp:Button runat="server" ID="btnPurchaseItemAdd" ClientIDMode="Static" Text="ADD ITEM"
                        OnClick="btnPurchaseItemAdd_Click" OnClientClick="javascript:return CtrlbtnPurchaseItemAdd();" />
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <asp:GridView runat="server" ID="gvPurchaseEnqItem" AutoGenerateColumns="false" Width="1076px"
                        OnRowEditing="gvPurchaseEnqItem_RowEditing" OnRowUpdating="gvPurchaseEnqItem_RowUpdating"
                        OnRowCancelingEdit="gvPurchaseEnqItem_RowCanceling" OnRowDeleting="gvPurchaseEnqItem_RowDeleting">
                        <HeaderStyle CssClass="tbEnquiryList" />
                        <Columns>
                            <asp:TemplateField HeaderText="CATEGORY">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCategory" Text='<%# Eval("ProdCategory") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PRODUCT DESCRIPTION">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblDesc" Text='<%# Eval("ProdDesc") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="MEASUREMENT" ItemStyle-Width="100px" HeaderStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblMeasurement" Text='<%# Eval("ProdMeasurement") %>'></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdnEnqID" Value='<%# Eval("EnqPordID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="QTY" ItemStyle-Width="60px" HeaderStyle-Width="60px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblPurchaseQTy" Text='<%# Eval("EnqItemQty") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox runat="server" ID="txtPurchaseQty" Text='<%# Eval("EnqItemQty") %>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ACTION" ItemStyle-Width="150px" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Button runat="server" ID="btnGvEdit" Text="EDIT" CommandName="EDIT" />
                                    <asp:Button runat="server" ID="btnGvDelete" Text="DELETE" CommandName="DELETE" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Button runat="server" ID="btnGvUpdate" Text="UPDATE" CommandName="UPDATE" />
                                    <asp:Button runat="server" ID="btnGvCancel" Text="CANCEL" CommandName="CANCEL" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="5" style="text-align: center;">
                    <asp:Button runat="server" ID="btnGotoEnqNext" ClientIDMode="Static" Text="SHOW MATCHING SUPPLIER"
                        Visible="false" OnClick="btnGotoEnqNext_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="5" id="divEnquirySupplier" style="display: none;">
                    <table cellspacing="0" rules="all" border="1" class="enquirypage">
                        <tr>
                            <td colspan="2">
                                <asp:GridView runat="server" ID="gvPurchaseEnqSupplier" AutoGenerateColumns="false"
                                    AllowPaging="True" EnableSortingAndPagingCallbacks="False" Width="1070px">
                                    <HeaderStyle BackColor="#fcedc7" />
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="50px" HeaderText="CHOOSE">
                                            <ItemTemplate>
                                                <asp:CheckBox runat="server" ID="chkSupplierAssign" AutoPostBack="false" />
                                                <asp:HiddenField runat="server" ID="hdnSupID" Value='<%# Eval("ID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="SupplierName" HeaderText="SUPPLIER" />
                                        <asp:BoundField DataField="SuppCountry" HeaderText="COUNTRY" />
                                        <asp:BoundField DataField="SuppCity" HeaderText="CITY" />
                                        <asp:BoundField DataField="SuppContactPerson" HeaderText="CONTACT PERSON" />
                                        <asp:BoundField DataField="SuppContactNo" HeaderText="CONTACT NO." />
                                        <asp:BoundField DataField="RecCount" HeaderText="AVAILABLE ENQUIRY ITEM" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr style="background-color: #cccccc;">
                            <th>
                                ENQUIRY COMMENTS
                            </th>
                            <th>
                                ENQUIRY NO. &amp; EXPIRE DATE
                            </th>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox runat="server" ID="txtEnqComments" Text="" TextMode="MultiLine" Width="500px"
                                    Height="80px"></asp:TextBox>
                            </td>
                            <td>
                                <div style="float: left; width: 50%; padding-left: 20px;">
                                    <label style="font-weight: bold;">
                                        EXPIRE DATE
                                    </label>
                                    <asp:TextBox runat="server" ID="txtEnqExpiredDate" ClientIDMode="Static" Text=""></asp:TextBox>
                                </div>
                                <div style="width: 40%; float: left;">
                                    <asp:Button runat="server" ID="btnEnqPrepareSave" ClientIDMode="Static" Text="ENQUIRY SAVE"
                                        OnClick="btnEnqPrepareSave_Click" OnClientClick="javascript:return CtrlSaveEnquiry();" />
                                    <input type="button" id="btnCancel" value="CANCEL" class="hide" onclick="refresh();" />
                                </div>
                                <br />
                                <div style="padding-left: 20px;">
                                    <asp:Button runat="server" ID="btnSendToCustomer" CssClass="hide" ClientIDMode="Static"
                                        Text="SEND TO CUSTOMER" OnClick="btnSendToCustomer_Click" OnClientClick="javascript:return CrtlSendToCustomer();" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <div id="overlay">
        <div class="row">
            <span id='close' onclick='showOverlay()'>
                <img src="../images/close.png" alt="close" style="width: 18px; height: 18px;" /></span>
            <div id="divEnqList">
                <asp:Literal ID="ltrlEnqList" runat="server"></asp:Literal>
            </div>
        </div>
    </div>
</div>
<script src="../Scripts/datemonthyear.js" type="text/javascript"></script>
<script type="text/javascript">
    function CtrlbtnPurchaseItemAdd() {
        var errMsg = '';
        $('#lblErrMsg').html('');
        if ($('#ddlProductCategory option:selected').val() == "CHOOSE")
            errMsg += 'Choose product category<br/>';
        if ($('#ddlProductDesc option:selected').val() == "CHOOSE")
            errMsg += 'Choose product description<br/>';
        if ($('#ddlMeasurement option:selected').val() == "CHOOSE")
            errMsg += 'Choose product measurement<br/>';
        if ($('#txtProductQty').val().length == 0)
            errMsg += 'Enter enquiry required qty<br/>';
        if (errMsg.length > 0) {
            $('#lblErrMsg').html(errMsg);
            return false
        }
        else
            return true;
    }

    function ShowEnqAssignSupplier() {
        $("[name = 'ctl00$MainContent$ctl00$btnGotoEnqNext']").attr('disabled', 'disabled')
        $('#divEnquirySupplier').show();
    }
    $(document).ready(function () {
        $("#txtEnqExpiredDate").datepicker({
            minDate: "+0D", maxDate: "+30D"
        }).keydown(function (e) {
            e.keyWhich = 0; e.keyCode = 0; e.preventDefault();
        });

        $("#lblPageHead").text("Purchase enquiry Prepare");
    });

    function showOverlay(e) {
        el = document.getElementById("overlay");
        el.style.visibility = (el.style.visibility == "visible") ? "hidden" : "visible";
        $($(el).children()).css("width", "auto");
        $($(el).children()).css("height", "auto");
        var overlaywidth = $($($(el).children())).width();
        var totwidth = $(document).width();
        var remwidth = totwidth - overlaywidth;
        var overlayheight = $($($(el).children())).height();
        var totheight = $(document).height();
        var remheight = totheight - overlayheight;
        $($(el).children()).css("margin-left", (remwidth / 2) + "px");
        $($(el).children()).css("margin-top", (remheight / 2) + "px");
    }

    function toggleSendButton() {
        if ($("#btnSendToCustomer").hasClass("show")) { $("#btnSendToCustomer").addClass("hide"); $("#btnSendToCustomer").removeClass("show"); }
        else if ($("#btnSendToCustomer").hasClass("hide")) { $("#btnSendToCustomer").addClass("show"); $("#btnSendToCustomer").removeClass("hide"); }
    }

    function toggleEditMode(mode) {
        if (mode == 0) {
            $("#btnEnqPrepareSave").val("EDIT");
            $("#MainContent_ctl00_gvPurchaseEnqSupplier, #MainContent_ctl00_gvPurchaseEnqItem").find("input,button,textarea,select").attr("disabled", "disabled");
            $("#<%=txtEnqComments.ClientID%>").attr("disabled", "disabled");
            $("#<%=txtEnqExpiredDate.ClientID%>").attr("disabled", "disabled");
            $("#trGroupControls").find("input,button,textarea,select").attr("disabled", "disabled");
            $("#btnSendToCustomer").removeClass("hide");
            $("#btnCancel").addClass("hide");
        }
        else if (mode == 1) {
            $("#btnEnqPrepareSave").val("ENQUIRY SAVE");
            $("#MainContent_ctl00_gvPurchaseEnqSupplier, #MainContent_ctl00_gvPurchaseEnqItem").find("input,button,textarea,select").removeAttr("disabled");
            $("#<%=txtEnqComments.ClientID%>").removeAttr("disabled");
            $("#<%=txtEnqExpiredDate.ClientID%>").removeAttr("disabled");
            $("#trGroupControls").find("input,button,textarea,select").removeAttr("disabled");
            $("#btnSendToCustomer").addClass("hide");
            $("#btnCancel").removeClass("hide");
        }
    }


    $(document).submit(function () {
        toggleEditMode(1);
    });

    function refresh() {
        window.location.reload();
    }

    function CtrlSaveEnquiry() {
        
        if ($("#btnEnqPrepareSave").val() == "EDIT") {
            toggleEditMode(1);
            return false;
        }
        $('#lblErrMsg').html('');
        if (CrtlSendToCustomer() == true)  return true; 
        else return false;
    }

    function CrtlSendToCustomer() {
        var ErrMsg = ''; $('#lblErrMsg').html('');
        if ($('input:checkbox[id*=MainContent_ctl00_gvPurchaseEnqSupplier_chkSupplierAssign_]:checked').length == 0)
            ErrMsg += 'Choose any one supplier<br/>';
        if ($('#txtEnqExpiredDate').val().length == 0)
            ErrMsg += 'choose expire date<br/>';
        if ($('#MainContent_ctl00_txtEnqComments').val().length == 0)
            ErrMsg += 'Enter enquiry comments';
        if (ErrMsg.length > 0) {
            $('#lblErrMsg').html(ErrMsg);
            return false;
        }
        else
            return true;
    }
</script>
<style type="text/css">
    #divEnqList ul
    {
        list-style: none;
        padding-right: 20px;
        text-align: left;
    }
    
    #divEnqList ul li
    {
        height: 30px;
    }
    
    #divEnqList ul li:hover
    {
        cursor: pointer;
        text-decoration: underline;
    }
    
    #divEnqList ul li a
    {
        color: black;
        text-decoration: none;
    }
</style>
