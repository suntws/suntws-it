<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="preparepurchaseorder.ascx.cs"
    Inherits="TTS.ebidpurchase.preparepurchaseorder" %>
<link href="Styles/datemonthyear.css" rel="stylesheet" type="text/css" />
<style type="text/css">
    #divEnqList, #divEnqListProd, #divTtlEnqNo, #divPreparePO
    {
        margin: 2px 2px 2px 2px;
    }
    
    #divEnqList th
    {
        background-color: rgba(108, 174, 241, 0.32);
    }
    #divEnqListProd th
    {
        background-color: #064e06;
        color: White;
    }
    #gvPOEnqList, #gvPOEnqListProd
    {
        width: 100%;
        text-align: center;
    }
    #lblEnquiryNo
    {
        margin-left: 20px;
        font-size: 24px;
        font-family: Arial Sans-Serif;
        color: rgba(12,107,6,0.82);
        font-weight: bold;
    }
    .bold
    {
        font-weight: bold;
        font-size: 12px;
    }
    .hide
    {
        display: none;
    }
    .popup-top-left
    {
        border-radius: 120px 20px 90px 0px;
    }
    .popup-bottom-left
    {
        border-radius: 0px 90px 20px 120px;
    }
    .popup-top-right
    {
        border-radius: 20px 120px 0px 90px;
    }
    .popup-bottom-right
    {
        border-radius: 90px 0px 120px 20px;
    }
    #divPopUpSupplier
    {
        background: rgba(243, 243, 242, 0.87);
        width: 480px;
        height: 240px;
        margin: 10px 10px 10px 10px;
        left: 10%;
        border: 1px solid rgba(17, 17, 17, 0.46);
        position: absolute;
        font-family: Sans-Serif Arial;
        font-size: 14px;
        z-index: 100px;
        background-image: url("images/blank.png");
        background-size: 100%;
    }
    .PopUpSupplier
    {
        text-decoration: underline;
        cursor: pointer;
    }
    #divComments
    {
        background-color: white;
        width: 400px;
        height: 80px;
        margin-top: 10px;
        margin-left: 40px;
        padding-left: 5px;
        padding-top: 5px;
        border: 1px solid rgba(78, 79, 76, 0.48);
        border-radius: 10px 10px 10px 10px;
    }
    .comments:hover
    {
        cursor: pointer;
    }
</style>
<style type="text/css">
        /************************* div enq details product *********************************/
        
        .lh30
        {
            line-height:30px;
        }
        
        #<%=lblPONumber.ClientID%>
        {
            color:brown;    
        }
        
        hr
        {
            margin-left:0px;
        }
        
        #areaComments,#areaShipAddress,#areaBillAddress
        {
            background-color: white;
            width: 400px;
            max-width:400px;
            max-height:120px;
            height: 80px;
            margin-top: 10px;
            margin-left: 40px;
            padding-left: 5px;
            padding-top: 5px;
            border: 1px solid rgba(78, 79, 76, 0.48);
            border-radius: 10px 10px 10px 10px;
        }
        
        #areaComments
        {
            height: 80px;
            max-height:80px;
        }
        #btnSendToSupplier, #btnSavePO,#btnCancel
        {
            border: none;
            text-align:center;
            background: #4CAF50;
            color: white;
            padding: 10px 12px;
            text-align: center;
            font-size: 12px;
            cursor: pointer;
            position:relative;
            top:25%;
        }
        #btnSavePO
        {
            background: #69abab;
            
        }
        #btnCancel
        {
            background: #dc377c;
        }
        #tblBillingAddress input[type='text'], #tblShippingAddress input[type='text']
        {
            position:static;
        }
        .dynamicAlign
        {
            padding-top:20px;
            line-height:20px;
        }
        #ddlShippingAddress,#ddlBillingAddress
        {
            width:320px;
            -webkit-appearance: searchfield;
            padding-top: 3px;
            padding-bottom: 3px;
        }
</style>
<div style="width: 1080px">
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="divEnqList">
        <asp:GridView runat="server" ID="gvPOEnqList" AutoGenerateColumns="false" ClientIDMode="Static" RowStyle-HorizontalAlign="Left">
            <Columns>
                <asp:BoundField DataField="EnqNo" HeaderText="ENQUIRY NO" />
                <asp:TemplateField HeaderText="SUPPLIER">
                    <ItemTemplate>
                        <span class="PopUpSupplier" style="text-transform: uppercase">
                            <%#Eval("SupplierName")%>
                        </span>
                        <asp:HiddenField ID="hdnSuppEmailID" Value='<%#Eval("SuppEmailID")%>' runat="server" />
                        <asp:HiddenField ID="hdnSuppCountry" Value='<%#Eval("SuppCountry")%>' runat="server" />
                        <asp:HiddenField ID="hdnSupplierId" Value='<%#Eval("SupplierId")%>' runat="server" />
                        <asp:HiddenField ID="hdnSuppAddress" Value='<%#Eval("SuppAddress")%>' runat="server" />
                        <asp:HiddenField ID="hdnSuppCity" Value='<%#Eval("SuppCity")%>' runat="server" />
                        <asp:HiddenField ID="hdnSupplierComment" Value='<%#Eval("SupplierComment")%>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ProdQuantity" HeaderText="PRODUCT QTY" ItemStyle-HorizontalAlign="Right"  />
                <asp:TemplateField HeaderText="CHARGES DESCP" ItemStyle-Width="200px">
                    <ItemTemplate>
                        <%#Eval("ChargesDescription")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="CHARGES %" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="80px">
                    <ItemTemplate>
                        <%#Eval("ChargesPercent") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="CHARGES RATE" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="80px">
                    <ItemTemplate>
                        <%#Eval("ChargesRate")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="TotAmount" HeaderText="TOTAL AMOUNT" ItemStyle-HorizontalAlign="Right"  />
                <asp:TemplateField HeaderText="ACTION">
                    <ItemTemplate>
                        <input type="button" value="More Details" onclick="getDetails(<%#Eval("SupplierId")%>,<%#Eval("ID")%>, '<%#Eval("EnqNo")%>' );" />
                        <asp:HiddenField ID="hdnEnquiryNo" Value='<%#Eval("EnqNo")%>' runat="server" />
                        <asp:HiddenField ID="hdnEnquiryStatus" Value='<%#Eval("EnqStatus")%>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:HiddenField ID="hdnSuppID" runat="server" ClientIDMode="Static" EnableViewState="true" />
    </div>
    <div id="divPopUpSupplier" class="popup-bottom-left hide">
        <table style="width: 93%; line-height: 18px;">
            <tbody>
                <tr>
                    <td style="float: left;">
                        <label class="bold">
                            Name :
                        </label>
                        <span id="spnName" style="text-transform: uppercase"></span>
                    </td>
                </tr>
                <tr>
                    <td style="float: right;">
                        <label class="bold">
                            Email :
                        </label>
                        <span id="spnEmail"></span>
                    </td>
                </tr>
                <tr>
                    <td style="float: left;">
                        <label class="bold">
                            Country
                        </label>
                        <span id="spnCountry"></span>
                    </td>
                </tr>
                <tr>
                    <td style="float: right;">
                        <label class="bold">
                            City :</label>
                        <span id="spnCity"></span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label class="bold">
                            Comments :</label>
                        <br />
                        <div id="divComments">
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
        <div style="clear: both">
        </div>
    </div>
    <div id="divTtlEnqNo">
        <asp:Label runat="server" ID="lblEnquiryNo" ClientIDMode="Static"></asp:Label>
    </div>
    <div id="divEnqListProd" class="hide">
        <asp:GridView runat="server" ID="gvPOEnqListProd" AutoGenerateColumns="false" ClientIDMode="Static" RowStyle-HorizontalAlign="Left">
            <Columns>
                <asp:BoundField DataField="ProdCategory" HeaderText="PRODUCT CATEGORY" />
                <asp:BoundField DataField="ProductDesc" HeaderText="PRODUCT DESCRIPTION" />
                <asp:TemplateField HeaderText="PRODUCT QTY">
                    <ItemTemplate>
                        <%#Eval("Product_Qty")%>
                        (
                        <%#Eval("Qty_Measurement")%>
                        )
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="PricePerProduct" HeaderText="EACH PRICE" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="ProductAmnt" HeaderText="PRODUCT AMOUNT" ItemStyle-HorizontalAlign="Right" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:HiddenField ID="hdnEnquiryNo" Value='<%#Eval("EnqNo")%>' runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnEnquiredId" Value='<%#Eval("ID")%>' runat="server" ClientIDMode="Static" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <br />
    </div>
    <div id="divPreparePO" class="hide" style="clear: both;">
        <hr />
        <div style="width: 100%">
            <div>
                <div class="bold" style="width: 20%; float: left;">
                    PO NUMBER :
                    <asp:Label runat="server" ID="lblPONumber" ClientIDMode="Static"></asp:Label>
                    <asp:HiddenField runat="server" ID="hdnPONumber" ClientIDMode="Static" />
                </div>
                <div style="width: 36%; float: left;">
                    <label class="bold">
                        EXPECTED SHIP DATE :
                    </label>
                    <asp:TextBox runat="server" ID="txtPOExpiredDate" ClientIDMode="Static"></asp:TextBox>
                </div>
                <div class="bold" style="width: 42%; float: left;">
                    PREFERED TRANSPORTER :
                    <asp:TextBox ID="txtPreferedTransport" runat="server" list="ddlPreferedTransport" />
                    <datalist name="preferedTransport" id="ddlPreferedTransport">
                        <option>SELECT </option>
                        <option>ARS </option>
                        <option>PARVEEN</option>
                        <option>RATHI </option>
                    </datalist>
                </div>
            </div>
            <div style="clear: both;">
            </div>
            <div style="margin-top: 10px; width: 100%">
                <div class="bold" style="width: 50%; float: left;">
                    <label>
                        BILLING ADDRESS
                    </label>
                    <asp:DropDownList ID="ddlBillingAddress" ClientIDMode="Static" runat="server">
                    </asp:DropDownList>
                </div>
                <div class="bold" style="width: 50%; float: left;">
                    <label>
                        SHIPPING ADDRESS
                    </label>
                    <asp:DropDownList ID="ddlShippingAddress" ClientIDMode="Static" runat="server">
                    </asp:DropDownList>
                </div>
                <div style="clear: both;">
                </div>
            </div>
        </div>
        <div style="margin-top: 10px; width: 100%">
            <table style="width: 100%; border: 1px solid black;">
                <tr>
                    <td style="border-right: 1px solid black;">
                        <table style="width: 100%" id="tblBillingAddress">
                            <tr>
                                <th colspan="4">
                                    BILLING ADDRESS
                                </th>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <textarea id="areaBillAddress" readonly="readonly"></textarea>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <table style="width: 100%" id="tblShippingAddress">
                            <tr>
                                <th colspan="4">
                                    SHIPPING ADDRESS
                                </th>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <textarea id="areaShipAddress" readonly="readonly"></textarea>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div style="margin-bottom: 20px; width: 100%;">
            <div style="width: 50%; margin-top: 10px; float: left;">
                <label for="areaComments" class="bold">
                    COMMENTS:</label>
                <textarea id="areaComments" style="vertical-align: text-top; margin-top: 0px; margin-left: 2px;
                    text-align: left;"></textarea>
                <asp:HiddenField ID="hdnAreaComments" ClientIDMode="Static" runat="server" />
            </div>
        </div>
        <div style="margin-top: 10px; width: 50%; height: 100px; float: left;">
            <div style="text-align: center; float: left; width: 100%; position: relative; top: 27%;">
                <asp:Button ID="btnSavePO" Text="SAVE PURCHASE ORDER" runat="server" ClientIDMode="Static"
                    OnClick="btnSavePO_Click" />
                <asp:Button ID="btnSendToSupplier" Text="SEND TO SUPPLIER" runat="server" ClientIDMode="Static"
                    OnClick="btnSendToSupplier_Click" />
                <input type="button" id="btnCancel" value="CANCEL" class="hide" onclick="refresh();" />
            </div>
        </div>
    </div>
    <div style="clear: both;">
    </div>
</div>
<script src="Scripts/datemonthyear.js" type="text/javascript"></script>
<script type="text/javascript">

    $(document).ready(function () {

        $("#gvPOEnqListProd tr").each(function (e) {
            if (e == 0) $("#gvPOEnqListProd tr").find("th:last").css("visibility", "hidden");
            else $($("#gvPOEnqListProd tr").find("td:last")[e - 1]).css("visibility", "hidden");
        });

        $("#areaBillAddress").val($("#ddlBillingAddress :selected").text());
        $("#areaShipAddress").val($("#ddlShippingAddress :selected").text());
        $("#areaComments").val($("#hdnAreaComments").val());
    });

    $("#lblPageHead").text("prepare purchase order");

    $(document).ready(function () {
        $("#txtPOExpiredDate").datepicker({
            minDate: "+0D", maxDate: "+30D"
        }).keydown(function (e) {
            e.keyWhich = 0; e.keyCode = 0; e.preventDefault();
        });
    });

    function getDetails(supid, enqid, enqNo) {
        window.location = "ebid_purchase.aspx?vid=8&enqid=" + enqid + "&supid=" + supid;
    }

    $(".comments").click(function (e, ele) {
        alert($(this).find(".comment").text());
    });

    // ---------------------- popup over suppliername -------------------
    $(".PopUpSupplier").mouseenter(function (eve) {
        var position = $(this).offset();
        var left = (position.left + 40) + "px";
        var top = (position.top + 10) + "px";
        $("#divPopUpSupplier").css("left", left);
        $("#divPopUpSupplier").css("top", top);
        $("#divPopUpSupplier").stop(true, true);
        $("#divPopUpSupplier").delay(50).fadeIn("slow");
        $("#divPopUpSupplier").find("#spnName").html(($(this).html()).trim());
        $("#divPopUpSupplier").find("#spnEmail").html($(this).parent().find("#hdnSuppEmailID").val());
        $("#divPopUpSupplier").find("#spnCountry").html($(this).parent().find("#hdnSuppCountry").val());
        $("#divPopUpSupplier").find("#spnCity").html($(this).parent().find("#hdnSuppCity").val());
        $("#divPopUpSupplier").find("#divComments").html($(this).parent().find("#hdnSupplierComment").val());
    });
    $(".PopUpSupplier").mouseleave(function (eve) {
        $("#divPopUpSupplier").stop(true, true);
        $("#divPopUpSupplier").delay(50).fadeOut("slow"); // $("#divPopUpSupplier").addClass("fadeout");
    });
    $("#divPopUpSupplier").mouseenter(function (eve) { jQuery("#divPopUpSupplier").stop(true, true); $("#divPopUpSupplier").fadeIn("slow") });
    $("#divPopUpSupplier").mouseleave(function (eve) { jQuery("#divPopUpSupplier").stop(true, true); $("#divPopUpSupplier").fadeOut("slow") });


</script>
<script type="text/javascript">
    //---------------------- div products portion -------------------------
    function showDetailsDiv(val) {
        if (val == 0) {
            $("#divEnqListProd, #divPreparePO").addClass("hide");
        }
        else if (val == 1) {
            $("#divEnqListProd, #divPreparePO").removeClass("hide");
            var tot = 0;
            var enqNo = $("#gvPOEnqListProd").find("#hdnEnquiryNo").val();
            generatePoNumber(enqNo);
            $("#lblEnquiryNo").text(enqNo);
        }
    }

    $("#btnCancel").click(function () {
        showOverlay();
    });

    function generatePoNumber(enqNo) {
        var len = enqNo.length;
        var poNum = "PO" + "" + enqNo.substring(3, len);
        $("#hdnPONumber").val(poNum);
        $("#lblPONumber").text(poNum);
    }

    $("#btnSendToSupplier").click(function (event) {
        validate(event);
    });

    function validate(event) {
        var errorMessage = "<span style='color:red; position:relative;' id='spnError'><br/>Enter value of this </span>"
        $("#spnError").remove();
        $(".dynamicAlign").removeClass("dynamicAlign");

        if ($("#txtPOExpiredDate").val() == "") {
            $("#txtPOExpiredDate").after(errorMessage);
            event.preventDefault();
            return false;
        }
        else if ($("#areaComments").val() == "") {
            $("#areaComments").after(errorMessage);
            event.preventDefault();
            return false;
        }
        toggleEditMode(1);
    }

    $("#ddlBillingAddress, #ddlShippingAddress").click(function () {
        if (this.id == "ddlBillingAddress") $("#areaBillAddress").val($("#ddlBillingAddress :selected").text());
        if (this.id == "ddlShippingAddress") $("#areaShipAddress").val($("#ddlShippingAddress :selected").text());
    });

    $("#areaComments").change(function () {
        $("#hdnAreaComments").val($("#areaComments").val());
    });

    function toggleEditMode(mode) {
        if (mode == 0) {
            $("#btnSavePO").val("EDIT");
            $("#areaComments").removeAttr("disabled");
            $("#divPreparePO").find("input[type='text'],button,select,#areaComments").attr("disabled", "disabled");
            $("#btnSendToSupplier").removeClass("hide");
            $("#btnCancel").addClass("hide");

        }
        else if (mode == 1) {
            $("#btnSavePO").val("SAVE PURCHASE ORDER");
            $("#areaComments").removeAttr("disabled");
            $("#divPreparePO").find("input[type='text'],button,select,#areaComments").removeAttr("disabled");
            $("#btnSendToSupplier").addClass("hide");
            $("#btnCancel").removeClass("hide");
        }
    }

    function refresh() {
        window.location.reload();
    }

    $("#btnSavePO").click(function (ele, eve) {
        if ($("#btnSavePO").val() == "EDIT") {
            toggleEditMode(1);
            return false;
        }
        else {
            validate(ele);
        }
    });

</script>
