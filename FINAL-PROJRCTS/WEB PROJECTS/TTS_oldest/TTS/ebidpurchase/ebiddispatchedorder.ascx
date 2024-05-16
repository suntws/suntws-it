<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ebiddispatchedorder.ascx.cs"
    Inherits="TTS.ebidpurchase.ebiddispatchedorder" %>
<style type="text/css">
    #gvEnquiryList, #gvEnquiryDetails
    {
        width: 99%;
        line-height: 18px;
        border-color: tan;
        font-size: 12px;
        margin: 2px 2px 2px 2px;
    }
</style>
<style type="text/css">
    #lblEnquiryNo
    {
        padding-left: 20px;
        color: brown;
        font-size: 22px;
    }
    .caption
    {
        margin-top: 10px;
    }
    .hide
    {
        display: none;
    }
    #divPODetails, #divInvoiceDetails
    {
        border: 1px solid tan;
        margin: 2px 2px 2px 2px;
        padding: 10px 10px 10px 10px;
        font-size: 14px;
    }
    #divPODetails span, #divInvoiceDetails span
    {
        font-weight: bold;
    }
    #areaPOComments, #areaReceiverComments
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
</style>
<style type="text/css">
    /********************* overlay styles ********************/
    #Overlay
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
    
    #Overlay #innerOverlay
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
        background-image: url("Images/close.png");
        position: relative;
    }
    
    #close:hover
    {
        cursor: pointer;
    }
    #areaCancelComments
    {
        background-color: white;
        overflow: none;
        border-color: rgba(12, 12, 12, 0.42);
        border-radius: 5px 5px 5px 5px;
        height: 90px;
        width: 320px;
        text-align: justify;
        vertical-align: top;
        outline-style: none;
        max-width: 320px;
        max-height: 150px;
    }
    
    .Action
    {
        text-align: center;
        margin: 10px 10px 10px 10px;
        width: 98%;
        line-height: 20px;
    }
    .bold
    {
        font-weight: bold;
    }
    .PopUpSupplier
    {
        text-decoration: underline;
    }
    .comments:hover
    {
        cursor: pointer;
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
                <asp:BoundField DataField="EnqNo" HeaderText="ENQUIRY NO." />
                <asp:BoundField DataField="EnqSentDate" HeaderText="SENT ON" />
                <asp:BoundField DataField="EnqUsername" HeaderText="ENQUIRED BY" />
                <asp:TemplateField HeaderText="COMMENTS">
                    <ItemTemplate>
                        <div style="max-width: 200px; height: 20px;" class="comments">
                            <div style="max-width: 180px; height: 20px; float: left; overflow: hidden" class="comment">
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
                        <a href="ebid_purchase.aspx?vid=11&enqid=<%#Eval("ID")%>">View </a>
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
                        <span class="PopUpSupplier" style="text-transform: uppercase; color: Blue;">
                            <%#Eval("SupplierName")%>
                        </span>
                        <asp:HiddenField ID="hdnSuppEmailID" Value='<%#Eval("SuppEmailID")%>' runat="server" />
                        <asp:HiddenField ID="hdnSuppCountry" Value='<%#Eval("SuppCountry")%>' runat="server" />
                        <asp:HiddenField ID="hdnSupplierId" Value='<%#Eval("SuppID")%>' runat="server" />
                        <asp:HiddenField ID="hdnSuppAddress" Value='<%#Eval("SuppAddress")%>' runat="server" />
                        <asp:HiddenField ID="hdnSuppCity" Value='<%#Eval("SuppCity")%>' runat="server" />
                        <asp:HiddenField ID="hdnSupplierComment" Value='<%#Eval("SupplierComment")%>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ProdCategory" HeaderText="PROD CATEGORY" />
                <asp:BoundField DataField="ProductDesc" HeaderText="PROD DESC" />
                <asp:TemplateField HeaderText="PRODUCT QTY">
                    <ItemTemplate>
                        <%#Eval("Product_Qty")%>
                        (
                        <%#Eval("ProdMeasurement")%>
                        )
                        <asp:HiddenField ID="hdnEnqCancelComments" Value='<%#Eval("EnqCancelComments")%>'
                            runat="server" />
                        <asp:HiddenField ID="hdnEnqCancelDate" Value='<%#Eval("EnqCancelDate")%>' runat="server" />
                        <asp:HiddenField ID="hdnSuppID" Value='<%#Eval("SuppID")%>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="PricePerProduct" HeaderText="EACH PRICE" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="ProductAmnt" HeaderText="AMOUNT" ItemStyle-HorizontalAlign="Right" />
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
                <asp:BoundField DataField="TotAmount" HeaderText="TOTAL" ItemStyle-HorizontalAlign="Right" />
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
        <div id="divInvoiceDetails" class="hide">
            <h3 style="margin: 0px;">
                INVOICE DETAILS
            </h3>
            <div style="line-height: 18px; margin-top: 10px;">
                <div style="width: 33%; float: left">
                    <label style="line-height: 30px;">
                        INVOICE NO :
                    </label>
                    <span id="spnInvoiceNo"></span>
                    <asp:HiddenField ID="hdnInvoiceNo" Value='<%#Eval("InvoiceNo")%>' runat="server" />
                </div>
                <div style="width: 33%; float: left">
                    <label style="line-height: 30px;">
                        INVOICE DATE :
                    </label>
                    <span id="spnInvoiceDate"></span>
                    <asp:HiddenField ID="hdnInvoiceDate" Value='<%#Eval("InvoiceDate")%>' runat="server" />
                </div>
                <div style="width: 33%; float: left">
                    <label style="line-height: 30px;">
                        L/R NUMBER :
                    </label>
                    <span id="spnLRNumber"></span>
                    <asp:HiddenField ID="hdnLRNumber" Value='<%#Eval("LRNumber")%>' runat="server" />
                </div>
                <div style="width: 40%; float: left">
                    <label style="width: 120px; float: left;" style="line-height: 30px;">
                        ATTACHMENT :
                    </label>
                    <asp:LinkButton ID="lnkAttachment" runat="server" ClientIDMode="Static" OnClick="lnkAttachment_Click">
                    <span id="spnFilename" style=" text-decoration:underline;white-space:pre-line; width:260px;word-break: break-word;float:left;">
                    </span></asp:LinkButton>
                    <asp:HiddenField ID="hdnFilename" Value='<%#Eval("Filename")%>' runat="server" />
                    <asp:HiddenField ID="hdnSupplierID" ClientIDMode="Static" runat="server" />
                </div>
                <div style="clear: both">
                </div>
            </div>
        </div>
        <div id="divPODetails" class="hide">
            <h3 style="margin: 0px; width: 50%">
                PURCHASE ORDER DETAILS
            </h3>
            <div style="line-height: 20px; margin-top: 10px;">
                <div style="width: 25%; float: left">
                    <label>
                        PO NUMBER :
                    </label>
                    <span id="spnPoNo"></span>
                    <asp:HiddenField ID="hdnPoNo" Value='<%#Eval("PoNo")%>' runat="server" />
                </div>
                <div style="width: 25%; float: left">
                    <label>
                        PO DATE :
                    </label>
                    <span id="spnPoDate"></span>
                    <asp:HiddenField ID="hdnPoDate" Value='<%#Eval("PoDate")%>' runat="server" />
                </div>
                <div style="width: 25%; float: left">
                    <label>
                        DESIRED SHIP DATE :
                    </label>
                    <span id="spnExpArrivalDate"></span>
                    <asp:HiddenField ID="hdnExpArrivalDate" Value='<%#Eval("ExpArrivalDate")%>' runat="server" />
                </div>
                <div style="width: 25%; float: left">
                    <label>
                        PO USERNAME :
                    </label>
                    <span id="spnPoUsername"></span>
                    <asp:HiddenField ID="hdnPoUsername" Value='<%#Eval("PoUsername")%>' runat="server" />
                </div>
                <div style="clear: both;">
                </div>
            </div>
            <div style="line-height: 30px;">
                <div style="width: 50%; float: left">
                    <label>
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
        <div class="Action hide">
            <input type="button" onclick="showOverlay()" value="UPDATE RECEIVED" />
        </div>
    </div>
</div>
<div id="Overlay">
    <div id="innerOverlay">
        <span id='close' onclick='showOverlay()'>
            <img src="images/close.png" alt="close" style="width: 22px; height: 22px;" /></span>
        <span style="font-weight: bold;">COMMENTS </span>
        <br />
        <textarea id="areaReceiverComments"> </textarea>
        <asp:HiddenField ID="hdnReceiverComments" runat="server" />
        <div class="Action hide">
            <asp:Button ID="btnSubmit" runat="server" Text="SUBMIT" OnClick="btnSubmit_Click" />
        </div>
    </div>
</div>
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
                $(rowTop).find("td").eq(6).attr("rowspan", rowCount);
                $(rowTop).find("td").eq(7).attr("rowspan", rowCount);
                $(rowTop).find("td").eq(8).attr("rowspan", rowCount);
                $(rowTop).find("td").eq(9).attr("rowspan", rowCount);
                $(this).find("td").eq(0).css("display", "none");
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

        $("#spnPoNo").text($("#<%=hdnPoNo.ClientID%>").val());
        $("#spnPoDate").text($("#<%=hdnPoDate.ClientID%>").val());
        $("#spnExpArrivalDate").text($("#<%=hdnExpArrivalDate.ClientID%>").val());
        $("#spnPoUsername").text($("#<%=hdnPoUsername.ClientID%>").val());
        $("#areaPOComments").text($("#<%=hdnPoComments.ClientID%>").val());
        $("#spnBillAddr").text($("#<%=hdnBillAddr.ClientID%>").val());
        $("#spnShipAddr").text($("#<%=hdnShipAddr.ClientID%>").val());

        if ($("#<%=hdnPoNo.ClientID%>").val() != "") {
            $("#divPODetails").removeClass("hide");
        }

        if ($("#<%=hdnInvoiceNo.ClientID%>").val() != "") {
            $("#divInvoiceDetails").removeClass("hide");
            $(".Action").removeClass("hide");
        }


    });

</script>
<script type="text/javascript">
    // ------------------------- div overlay portion ----------------------
    function showOverlay() {
        el = document.getElementById("Overlay");
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

    $("#areaReceiverComments").change(function () {
        $("#<%=hdnReceiverComments.ClientID %>").val($("#areaReceiverComments").val());
    });

    $(document).submit(function () {
        $("span[name='spnError']").remove();
        if ($("#areaReceiverComments").val().trim() == "") {
            $("#areaReceiverComments").after("<span name='spnError' style='color:red; line-height:30px;'><br/> Enter value for this </span>")
            return false;
        }
    });

    $(document).ready(function () {
        $("#lblPageHead").text("Dispatched orders");
    });

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

    $(".comments").click(function (e, ele) {
        alert($(this).find(".comment").text());
    });
</script>
