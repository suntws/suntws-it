<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="enquirycancel.ascx.cs"
    Inherits="TTS.ebidpurchase.enquirycancel" %>
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
        margin: 2px 8px 2px 2px;
        padding: 10px 10px 10px 10px;
    }
    .Action
    {
        text-align: center;
        margin: 10px 10px 10px 10px;
        width: 98%;
        line-height: 20px;
    }
    #divPODetails span, #divInvoiceDetails span
    {
        font-weight: bold;
    }
    #arePOComments
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
    .details
    {
        display: none;
    }
    .comments:hover
    {
        cursor: pointer;
    }
</style>
<style>
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
    .PopUpSupplier
    {
        text-decoration: underline;
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
            HeaderStyle-BackColor="#fcd7d7">
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
                        <a href="ebid_purchase.aspx?vid=9&enqid=<%#Eval("ID")%>">View </a>
                        <asp:HiddenField ID="hdnEnqStatusId" runat="server" Value='<%#Eval("StatusID") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <div class="caption details">
            <asp:Label runat="server" ID="lblEnquiryNo" ClientIDMode="Static"></asp:Label>
        </div>
        <div class="details">
            <asp:GridView runat="server" ID="gvEnquiryDetails" AutoGenerateColumns="false" ClientIDMode="Static"
                HeaderStyle-BackColor="#fcedc7">
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
                    <asp:BoundField DataField="ProductAmnt" HeaderText="AMOUNT"   ItemStyle-HorizontalAlign="Right"/>
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
                    <asp:BoundField DataField="TotAmount" HeaderText="TOTAL"  ItemStyle-HorizontalAlign="Right" />
                </Columns>
            </asp:GridView>
        </div>
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
                            <div style="background-color: #eeeeec; width: 400px; height: 80px; margin-top: 10px;
                                border: 1px solid black; margin-left: 40px; padding-left: 5px; padding-top: 5px;"
                                id="divComments">
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div id="divPODetails" class="details">
            <h3 style="margin: 0px;">
                PURCHASE ORDER DETAILS
            </h3>
            <div style="line-height: 20px;margin-top:10px;">
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
                        Exp Arrival Date :
                    </label>
                    <span id="spnExpArrivalDate"></span>
                    <asp:HiddenField ID="hdnExpArrivalDate" Value='<%#Eval("ExpArrivalDate")%>' runat="server" />
                </div>
                <div style="width: 25%; float: left">
                    <label>
                        POUsername :
                    </label>
                    <span id="spnPoUsername"></span>
                    <asp:HiddenField ID="hdnPoUsername" Value='<%#Eval("PoUsername")%>' runat="server" />
                </div>
                <div style="clear: both">
                </div>
            </div>
            <div style="line-height: 30px;">
                <div style="width:50%;float:left;">
                    <label>
                        POComments :
                    </label>
                    <textarea id="arePOComments" readonly="readonly"></textarea>
                    <asp:HiddenField ID="hdnPoComments" Value='<%#Eval("PoComments")%>' runat="server" />
                </div>
                <div style="width:50%; float:left;">
                    <div style="width:50%; float:left;">
                        <label>
                            Bill Address :
                        </label>
                        <span id="spnBillAddr"></span>
                        <asp:HiddenField ID="hdnBillAddr" Value='<%#Eval("BillAddr")%>' runat="server" />
                    </div>
                    <div style="width:50%; float:left;">
                        <label>
                            Ship Address :
                        </label>
                        <span id="spnShipAddr"></span>
                        <asp:HiddenField ID="hdnShipAddr" Value='<%#Eval("ShipAddr")%>' runat="server" />
                    </div>
                    <div style="clear:both"></div>
                </div>
                <div style="clear:both"></div>
            </div>
        </div>
        <div class="Action details">
            <input type="button" onclick="showOverlay()" value="CANCEL ENQUIRY" />
        </div>
    </div>
</div>
<div id="Overlay">
    <div id="innerOverlay">
        <span id='close' onclick='showOverlay()'>
            <img src="images/close.png" alt="close" style="width: 22px; height: 22px;" /></span>
        <span style="font-weight: bold;">CANCEL COMMENTS </span>
        <br />
        <textarea id="areaCancelComments"> </textarea>
        <asp:HiddenField ID="hdnCancelComment" runat="server" />
        <div class="Action">
            <asp:Button ID="btnConfirmCancel" runat="server" Text="SUBMIT" OnClick="btnConfirmCancel_Click" />
        </div>
    </div>
</div>
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

    $("#areaCancelComments").change(function () {
        $("#<%=hdnCancelComment.ClientID %>").val($("#areaCancelComments").val());
    });

    $(document).submit(function () {
        $("span[name='spnError']").remove();
        if ($("#areaCancelComments").val().trim() == "") {
            $("#areaCancelComments").after("<span name='spnError' style='color:red; line-height:30px;'><br/> Enter Comments for cancel </span>")
            return false;
        }
    });

    $(document).ready(function () {
        $("#lblPageHead").text("cancel enquiry");
    });

    function showDetailsDiv(val) {
        if (val == 1) { $(".details").css("display", "block"); }
        else if (val == 0) { $(".details").css("display", "none"); }
    }

</script>
<script type="text/javascript">
    var rowTop = "", rowCount = 1;
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
                $(this).find("td").eq(0).css("display", "none");
                $(this).find("td").eq(5).css("display", "none");
                $(this).find("td").eq(6).css("display", "none");
                $(this).find("td").eq(7).css("display", "none");
                $(this).find("td").eq(8).css("display", "none");
            }
            else {
                rowCount = 1;
                rowTop = this;
            }
        }
    });

    $(document).ready(function () {
        $("#spnPoNo").text($("#<%=hdnPoNo.ClientID%>").val());
        $("#spnPoDate").text($("#<%=hdnPoDate.ClientID%>").val());
        $("#spnExpArrivalDate").text($("#<%=hdnExpArrivalDate.ClientID%>").val());
        $("#spnPoUsername").text($("#<%=hdnPoUsername.ClientID%>").val());
        $("#arePOComments").text($("#<%=hdnPoComments.ClientID%>").val());
        $("#spnBillAddr").text($("#<%=hdnBillAddr.ClientID%>").val());
        $("#spnShipAddr").text($("#<%=hdnShipAddr.ClientID%>").val());
    });

    if ($("#<%=hdnPoNo.ClientID%>").val() == "") {
        $("#divPODetails").removeClass("details");
        $("#divPODetails").css("display", "none");
    }

    function showPriceColumns(val) {
        if (val == 1) {
            for (i = 4; i <= 9; i++) {
                $('#gvEnquiryDetails th:nth-child(' + i + '),#gvEnquiryDetails td:nth-child(' + i + ')').hide();
            }
        }
    }

    $(".comments").click(function (e, ele) {
        alert($(this).find(".comment").text());
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
</script>
