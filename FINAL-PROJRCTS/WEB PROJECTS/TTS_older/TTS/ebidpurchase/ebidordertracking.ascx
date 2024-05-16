<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ebidordertracking.ascx.cs"
    Inherits="TTS.ebidpurchase.ebidordertracking" %>
<style type="text/css">
    #gvEnquiryList, #gvEnquiryDetails
    {
        width: 99%;
        line-height: 18px;
        border-color: tan;
        font-size: 12px;
        margin: 2px 2px 2px 2px;
    }
    #lblEnquiryNo
    {
        padding-left: 20px;
        color: brown;
        font-size: 20px;
    }
    
    .caption
    {
        margin-top: 10px;
    }
    
    .hide
    {
        display: none;
    }
    
    #divPODetails, #divInvoiceDetails, #divOrderCancelDetails
    {
        border: 1px solid tan;
        margin: 2px 2px 2px 2px;
        padding: 10px 10px 10px 10px;
        font-size: 14px;
    }
    
    #divPODetails span, #divInvoiceDetails span, #divOrderCancelDetails span
    {
        font-weight: bold;
    }
    
    #areaPOComments, #areaReceiverComments, #areaSupCancelComments, #areaEnqCancelComments
    {
        background-color: white;
        width: 320px;
        height: 80px;
        margin-top: 10px;
        margin-left: 40px;
        padding-left: 5px;
        padding-top: 5px;
        border: 1px solid rgba(78, 79, 76, 0.48);
        border-radius: 10px 10px 10px 10px;
        vertical-align: top;
    }
    
    .comments:hover
    {
        cursor: pointer;
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
        background-image: url("/images/blank.png");
        background-size: 100%;
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
    
    .spnPopUpSupplier
    {
        text-decoration: underline;
        cursor: pointer;
    }
    .PopUpSupplier
    {
        text-decoration: underline;
    }
</style>
<div style="width: 1080px;">
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div>
        <asp:GridView runat="server" ID="gvEnquiryList" AutoGenerateColumns="false" ClientIDMode="Static"
            HeaderStyle-BackColor="#ffec64">
            <Columns>
                <asp:BoundField DataField="EnqNo" HeaderText="ENQUIRY NO" />
                <asp:BoundField DataField="EnqSentDate" HeaderText="SENT DATE" />
                <asp:BoundField DataField="EnqUsername" HeaderText="ENQUIRED BY" />
                <asp:TemplateField HeaderText="COMMENTS">
                    <ItemTemplate>
                        <div style="max-width: 200px; height: 20px;">
                            <div style="max-width: 180px; height: 20px; float: left; overflow: hidden" class="comments">
                                <%#Eval("EnqComments")%>
                            </div>
                            <literal style="float: left; margin-top: -5px; margin-left: 5px;">... </literal>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ExpiredDate" HeaderText="EXPIRES ON" />
                <asp:BoundField DataField="StateName" HeaderText="ENQUIRY STATE" />
                <asp:TemplateField HeaderText="ACTION">
                    <ItemTemplate>
                        <a href="ebid_purchase.aspx?vid=10&enqid=<%#Eval("ID")%>">View </a>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <div class="caption">
            <asp:Label runat="server" ID="lblEnquiryNo" ClientIDMode="Static"> </asp:Label>
        </div>
        <asp:GridView runat="server" ID="gvEnquiryDetails" AutoGenerateColumns="false" ClientIDMode="Static"
            HeaderStyle-BackColor="#ffffcd">
            <Columns>
                <asp:TemplateField HeaderText="SUPPLIER">
                    <ItemTemplate>
                        <span class="spnPopUpSupplier" style="text-transform: uppercase; color: Blue;">
                            <%#Eval("SupplierName")%>
                        </span>
                        <asp:HiddenField ID="hdnSuppEmailID" Value='<%#Eval("SuppEmailID")%>' runat="server" />
                        <asp:HiddenField ID="hdnSuppCountry" Value='<%#Eval("SuppCountry")%>' runat="server" />
                        <asp:HiddenField ID="hdnSupplierId" Value='<%#Eval("SuppID")%>' runat="server" />
                        <asp:HiddenField ID="hdnSuppAddress" Value='<%#Eval("SuppAddress")%>' runat="server" />
                        <asp:HiddenField ID="hdnSuppCity" Value='<%#Eval("SuppCity")%>' runat="server" />
                        <asp:HiddenField ID="hdnSupplierComment" Value='<%#Eval("SupplierComment")%>' runat="server" />
                        <asp:HiddenField ID="hdnEnqCancelComments" Value='<%#Eval("EnqCancelComments")%>'
                            runat="server" />
                        <asp:HiddenField ID="hdnEnqCancelDate" Value='<%#Eval("EnqCancelDate")%>' runat="server" />
                        <asp:HiddenField ID="hdnSuppID" Value='<%#Eval("SuppID")%>' runat="server" />
                        <asp:HiddenField ID="hdnEnqStatus" Value='<%#Eval("EnqStatus")%>' runat="server" />
                        <asp:HiddenField ID="hdnStatusId" Value='<%#Eval("StatusId")%>' runat="server" />
                        <asp:HiddenField ID="hdnSupCancelComments" Value='<%#Eval("SupCancelComments")%>'
                            runat="server" />
                        <asp:HiddenField ID="hdnSupCancelDate" Value='<%#Eval("SupCancelDate")%>' runat="server" />
                        <asp:HiddenField ID="hdnEnqPrevStatus" Value='<%#Eval("EnqPrevStatus")%>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ProdCategory" HeaderText="PROD CATEGORY" />
                <asp:BoundField DataField="ProductDesc" HeaderText="PROD DESC" />
                <asp:TemplateField HeaderText="PRODUCT">
                    <ItemTemplate>
                        <%#Eval("Product_Qty")%>
                        (
                        <%#Eval("ProdMeasurement")%>
                        )
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ProductAmnt" HeaderText="AMOUNT" />
                <asp:TemplateField HeaderText="CHARGES DESCP" ItemStyle-Width="200px">
                    <ItemTemplate>
                        <%#Eval("ChargesDescription")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="%" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="40px">
                    <ItemTemplate>
                        <%#Eval("ChargesPercent") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="RATE" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="50px">
                    <ItemTemplate>
                        <%#Eval("ChargesRate")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="TotAmount" HeaderText="TOTAL" />
                <asp:BoundField DataField="EnqStatus" HeaderText="STATUS" />
            </Columns>
        </asp:GridView>
        <div id="divPopUpSupplier" class="popup-bottom-left hide">
            <table style="width: 93%; line-height: 18px;">
                <tbody>
                    <tr>
                        <td style="float: left;">
                            <label class="bold">
                                Name :
                            </label>
                            <span id="spnName"></span>
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
                                Country :
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
                            <div style="background-color: #eeeeec; width: 400px; height: 80px; margin-top: 10px;
                                border: 1px solid black; margin-left: 40px; padding-left: 5px; padding-top: 5px;"
                                id="divComments">
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div id="divOrderCancelDetails" class="hide">
            <div style="column-count: 2">
                <div>
                    <h3 style="margin: 0px;">
                        STATUS DETAILS ( <span id="Span1" style="color: Blue; text-transform: uppercase;"
                            class="PopUpSupplier"></span>)</h3>
                </div>
                <div style="float: right; margin-top: 10px; color: #ff8823;">
                    <h3 style="margin: 0px;">
                        <span id="spnEnqStatus"></span>
                    </h3>
                </div>
            </div>
            <br />
            <div style="column-count: 2; line-height: 18px;">
                <div>
                    <label>
                        Requested Date :
                    </label>
                    <span id="spnEnqCancelDate"></span>
                </div>
                <div class="supRespond">
                    <label>
                        Supplier Respond Date
                    </label>
                    <span id="spnSupCancelDate"></span>
                </div>
            </div>
            <div style="column-count: 2; line-height: 45px;">
                <div>
                    <label style="font-weight: bold">
                        Comments:
                    </label>
                    <textarea id="areaEnqCancelComments" readonly="readonly"></textarea>
                </div>
                <div class="supRespond">
                    <label style="font-weight: bold">
                        Supplier comments :
                    </label>
                    <textarea id="areaSupCancelComments" readonly="readonly"></textarea>
                </div>
            </div>
        </div>
        <div id="divInvoiceDetails" class="hide">
            <h3 style="margin: 0px;">
                INVOICE DETAILS
            </h3>
            <div style="line-height: 35px; margin-top: 10px;">
                <div style="width: 33%; float: left">
                    <label>
                        Invoice Number :
                    </label>
                    <span id="spnInvoiceNo"></span>
                    <asp:HiddenField ID="hdnInvoiceNo" Value='<%#Eval("InvoiceNo")%>' runat="server" />
                </div>
                <div style="width: 33%; float: left">
                    <label>
                        Invoice Date :
                    </label>
                    <span id="spnInvoiceDate"></span>
                    <asp:HiddenField ID="hdnInvoiceDate" Value='<%#Eval("InvoiceDate")%>' runat="server" />
                </div>
                <div style="width: 33%; float: left">
                    <label>
                        L/R Number :
                    </label>
                    <span id="spnLRNumber"></span>
                    <asp:HiddenField ID="hdnLRNumber" Value='<%#Eval("LRNumber")%>' runat="server" />
                </div>
                <div style="clear: both">
                </div>
            </div>
            <div>
                <div style="width: 30%; float: left">
                    <label style="width: 100px; float: left;">
                        Attachment :
                    </label>
                    <asp:LinkButton ID="lnkAttachment" runat="server" ClientIDMode="Static" OnClick="lnkAttachment_Click">
                    <span id="spnFilename" style=" text-decoration:underline;white-space:pre-line; width:200px;word-break: break-word;float:left;">
                    </span></asp:LinkButton>
                    <asp:HiddenField ID="hdnFilename" Value='<%#Eval("Filename")%>' runat="server" />
                    <asp:HiddenField ID="hdnSupplierID" ClientIDMode="Static" runat="server" />
                </div>
                <div style="width: 38%; float: left">
                    <label style="font-weight: bold">
                        Receiver Comments
                    </label>
                    <br />
                    <textarea id="areaReceiverComments" readonly="readonly" style="margin-left: 0px;"></textarea>
                    <asp:HiddenField ID="hdnReceiverComments" Value='<%#Eval("ReceiverComments")%>' runat="server" />
                </div>
                <div style="width: 20%; float: left">
                    <label>
                        Received Date :
                    </label>
                    <span id="spnReceivedDate"></span>
                    <asp:HiddenField ID="hdnReceivedDate" Value='<%#Eval("ReceivedDate")%>' runat="server" />
                </div>
                <div style="clear: both;">
                </div>
            </div>
        </div>
        <div id="divPODetails" class="hide">
            <h3 style="margin: 0px; width: 50%">
                PURCHASE ORDER DETAILS ( <span id="spnSuppName" style="color: Blue; text-transform: uppercase;"
                    class="PopUpSupplier"></span>)
            </h3>
            <div style="line-height: 20px; margin-top: 10px;">
                <div style="width: 25%; float: left">
                    <label>
                        PO Number :
                    </label>
                    <span id="spnPoNo"></span>
                    <asp:HiddenField ID="hdnPoNo" Value='<%#Eval("PoNo")%>' runat="server" />
                </div>
                <div style="width: 25%; float: left">
                    <label>
                        PO Date :
                    </label>
                    <span id="spnPoDate"></span>
                    <asp:HiddenField ID="hdnPoDate" Value='<%#Eval("PoDate")%>' runat="server" />
                </div>
                <div style="width: 25%; float: left">
                    <label>
                        Desired Ship Date :
                    </label>
                    <span id="spnExpArrivalDate"></span>
                    <asp:HiddenField ID="hdnExpArrivalDate" Value='<%#Eval("ExpArrivalDate")%>' runat="server" />
                </div>
                <div style="width: 25%; float: left">
                    <label>
                        PO Username :
                    </label>
                    <span id="spnPoUsername"></span>
                    <asp:HiddenField ID="hdnPoUsername" Value='<%#Eval("PoUsername")%>' runat="server" />
                </div>
                <div style="clear: both;">
                </div>
            </div>
            <div style="line-height: 30px;">
                <div style="width: 50%; float: left">
                    <label style="font-weight: bold">
                        PO Comments :
                    </label>
                    <textarea id="areaPOComments" readonly="readonly"></textarea>
                    <asp:HiddenField ID="hdnPoComments" Value='<%#Eval("PoComments")%>' runat="server" />
                </div>
                <div style="width: 50%; float: left">
                    <div style="width: 50%; float: left">
                        <label>
                            Bill Address :
                        </label>
                        <span id="spnBillAddr"></span>
                        <asp:HiddenField ID="hdnBillAddr" Value='<%#Eval("BillAddr")%>' runat="server" />
                    </div>
                    <div style="width: 50%; float: left">
                        <label>
                            Ship Address :
                        </label>
                        <span id="spnShipAddr"></span>
                        <asp:HiddenField ID="hdnShipAddr" Value='<%#Eval("ShipAddr")%>' runat="server" />
                    </div>
                    <div style="clear: both;">
                    </div>
                </div>
                <div style="clear: both;">
                </div>
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">

    // --------------------------- on mouse enter/leave to/from supplier name --------------------------

    $(".PopUpSupplier").mouseenter(function (eve) {
        showPopUp(this);
    });

    function showPopUp(ele) {
        var position = $(ele).offset();
        var left = (position.left + 40) + "px";
        var top = (position.top + 10) + "px";
        $("#divPopUpSupplier").css("left", left);
        $("#divPopUpSupplier").css("top", top);
        $("#divPopUpSupplier").stop(true, true);
        $("#divPopUpSupplier").delay(50).fadeIn("slow");
    }

    $(".PopUpSupplier,.spnPopUpSupplier").mouseleave(function (eve) {
        $("#divPopUpSupplier").stop(true, true);
        $("#divPopUpSupplier").delay(50).fadeOut("slow"); // $("#divPopUpSupplier").addClass("fadeout");
    });
    $("#divPopUpSupplier").mouseenter(function (eve) { jQuery("#divPopUpSupplier").stop(true, true); $("#divPopUpSupplier").fadeIn("slow") });
    $("#divPopUpSupplier").mouseleave(function (eve) { jQuery("#divPopUpSupplier").stop(true, true); $("#divPopUpSupplier").fadeOut("slow") });

    $(".spnPopUpSupplier").mouseenter(function (eve) { // to decide to show popup, if PO not prepared or enquiry closed then show popup else showpopup over another link in PO section or Closed Section
        if (parseInt($(this).parent().find("#hdnStatusId").val()) < 5 || ($("#<%=hdnPoNo.ClientID%>").val() == "" && $(this).parent().find("#hdnStatusId").val() == "5")) {
            $("#divPopUpSupplier").find("#spnName").html(($(this).html()).trim());
            $("#divPopUpSupplier").find("#spnEmail").html($(this).parent().find("#hdnSuppEmailID").val());
            $("#divPopUpSupplier").find("#spnCountry").html($(this).parent().find("#hdnSuppCountry").val());
            $("#divPopUpSupplier").find("#spnCity").html($(this).parent().find("#hdnSuppCity").val());
            $("#divPopUpSupplier").find("#divComments").html($(this).parent().find("#hdnSupplierComment").val());
            showPopUp(this);
        }
    });

    $(".spnPopUpSupplier").click(function () {
        $("#divPopUpSupplier").find("#spnName").html(($(this).html()).trim());
        $("#divPopUpSupplier").find("#spnEmail").html($(this).parent().find("#hdnSuppEmailID").val());
        $("#divPopUpSupplier").find("#spnCountry").html($(this).parent().find("#hdnSuppCountry").val());
        $("#divPopUpSupplier").find("#spnCity").html($(this).parent().find("#hdnSuppCity").val());
        $("#divPopUpSupplier").find("#divComments").html($(this).parent().find("#hdnSupplierComment").val());

        $("#hdnSupplierID").val($(this).closest("tr").find("#hdnSuppID").val())
        $(".PopUpSupplier").html($(this).html().trim());
        $("#spnSupCancelDate").text($(this).closest("tr").find("#hdnSupCancelDate").val());
        $("#areaSupCancelComments").text($(this).closest("tr").find("#hdnSupCancelComments").val());
        $("#spnEnqCancelDate").text($(this).closest("tr").find("#hdnEnqCancelDate").val());
        $("#areaEnqCancelComments").text($(this).closest("tr").find("#hdnEnqCancelComments").val());
        $("#spnEnqStatus").text($(this).closest("tr").find("#hdnEnqStatus").val());

        if ($(this).closest("tr").find("#hdnStatusId").val() == "9" && $(this).closest("tr").find("#hdnEnqPrevStatus").val() != "") {
            $("#divPODetails").addClass("hide");
            $("#divInvoiceDetails").addClass("hide");
            $("#divOrderCancelDetails").removeClass("hide");
            $(".supRespond").addClass("hide");
        }
        else {
            if ($("#<%=hdnPoNo.ClientID%>").val() != "") {
                $("#divPODetails").removeClass("hide");
            }

            if ($("#<%=hdnInvoiceNo.ClientID%>").val() != "") {
                $("#divInvoiceDetails").removeClass("hide");
            }

            if ($(this).closest("tr").find("#hdnStatusId").val() == "11" || $(this).closest("tr").find("#hdnStatusId").val() == "12" || $(this).closest("tr").find("#hdnStatusId").val() == "10") {
                $("#divOrderCancelDetails").removeClass("hide");
                if ($(this).closest("tr").find("#hdnStatusId").val() != "10") $(".supRespond").removeClass("hide");
                else $(".supRespond").addClass("hide");
            }
            else {
                $("#divOrderCancelDetails").addClass("hide");
                $(".supRespond").addClass("hide");
            }
        }
    });

</script>
<script type="text/javascript">

    $(document).ready(function () {
        $("#lblPageHead").text("ebid Enquiry/Quote/Order tracking");
    });

    var rowTop = "";
    var rowCount = 1;
    $("#gvEnquiryDetails tr").each(function (e) {
        if (e == 1) {
            rowTop = this;
        }
        if (e > 1) {
            if ($(rowTop).find("#hdnSuppID").val() == $(this).find("#hdnSuppID").val()) {
                rowCount = rowCount + 1;
                $(rowTop).find("td").eq(0).attr("rowspan", rowCount);
                $(rowTop).find("td").eq(5).attr("rowspan", rowCount);
                $(rowTop).find("td").eq(6).attr("rowspan", rowCount);
                $(rowTop).find("td").eq(7).attr("rowspan", rowCount);
                $(rowTop).find("td").eq(8).attr("rowspan", rowCount);
                $(rowTop).find("td").eq(9).attr("rowspan", rowCount);
                $(this).find("td").eq(0).css("display", "none");
                $(this).find("td").eq(5).css("display", "none");
                $(this).find("td").eq(6).css("display", "none");
                $(this).find("td").eq(7).css("display", "none");
                $(this).find("td").eq(8).css("display", "none");
                $(this).find("td").eq(9).css("display", "none");

            }
            else {
                rowCount = 1;
                rowTop = this;
            }
        }
    });

    $(document).ready(function () {

        $("#spnInvoiceNo").text($("#<%=hdnInvoiceNo.ClientID%>").val());
        $("#spnInvoiceDate").text($("#<%=hdnInvoiceDate.ClientID%>").val());
        $("#spnLRNumber").text($("#<%=hdnLRNumber.ClientID%>").val());
        $("#spnFilename").text($("#<%=hdnFilename.ClientID%>").val());

        $("#spnReceivedDate").text($("#<%=hdnReceivedDate.ClientID%>").val());
        $("#areaReceiverComments").text($("#<%=hdnReceiverComments.ClientID%>").val());

        $("#spnPoNo").text($("#<%=hdnPoNo.ClientID%>").val());
        $("#spnPoDate").text($("#<%=hdnPoDate.ClientID%>").val());
        $("#spnExpArrivalDate").text($("#<%=hdnExpArrivalDate.ClientID%>").val());
        $("#spnPoUsername").text($("#<%=hdnPoUsername.ClientID%>").val());
        $("#areaPOComments").text($("#<%=hdnPoComments.ClientID%>").val());
        $("#spnBillAddr").text($("#<%=hdnBillAddr.ClientID%>").val());
        $("#spnShipAddr").text($("#<%=hdnShipAddr.ClientID%>").val());

        $("#gvEnquiryList td").eq(3).css("width", "200px");

    });

    $(".comments").click(function (e, ele) {
        alert($(this).text());
    });

    function showPriceColumns(val) {
        if (val <= 0) {
            for (i = 4; i < 9; i++) {
                $('#gvEnquiryDetails th:nth-child(' + 4 + '),#gvEnquiryDetails td:nth-child(' + 4 + ')').remove();
            }
        }
    }


</script>
