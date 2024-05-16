<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="enquiryquotedlist.ascx.cs"
    Inherits="TTS.ebidpurchase.enquiryquotedlist" %>
<style type="text/css">
    #gvPurEnqList th
    {
        background-color: #65ebeb;
        text-align: center;
    }
    #gvPurEnqList
    {
        width: 100%;
        line-height: 20px;
        border-color: #000;
        font-size: 12px;
        font-family: Sans-Serif;
    }
    #gvEnqDetails, #gvEnquirySuppliersProduct
    {
        width: 100%;
        line-height: 18px;
        border-color: #000;
        font-size: 12px;
    }
    #gvEnqDetails th
    {
        background-color: #fcd7d7;
        text-align: center;
    }
    #gvEnquirySuppliersProduct th
    {
        background-color: #f3e0ad;
        text-align: center;
    }
    .hide
    {
        display: none;
    }
    .PopUpSupplier
    {
        text-decoration: underline;
        cursor: pointer;
    }
    .uppercasesupplier
    {
        text-transform: uppercase;
    }
    #divPopUpSupplier
    {
        background-color: #ffffcd;
        width: 480px;
        height: 240px;
        margin: 10px 10px 10px 10px;
        left: 10%;
        border: 1px solid rgba(17, 17, 17, 0.46);
        position: absolute;
        font-family: Sans-Serif Arial;
        font-size: 14px;
        z-index: 100px;
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
    .bold
    {
        font-weight: bold;
    }
    .show
    {
        display: block;
    }
    #lblEnquiryNo
    {
        padding-left: 20px;
        color: #fff;
        font-size: 18px;
    }
    .comments:hover
    {
        cursor: pointer;
    }
</style>
<style type="text/css">
    /* --------- divDetails portion -------------- */
    #divDetails
    {
        margin-left:4px;
        margin-right:4px;
        border:1px solid tan;
        width:99%;
    }
    #spnSupplierName
    {
        font-size:16px;
        font-family:Arial Sans-Serif;
    }
    #<%=lblSupplierName.ClientID%>
    {
        color:Blue;
        text-transform:uppercase;
        font-size:18px;
        font-family:Arial Sans-Serif;
    }
</style>
<div style="width: 1080px;">
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div>
        <table width="1078px;">
            <tr>
                <td>
                    <asp:GridView runat="server" ID="gvPurEnqList" AutoGenerateColumns="false" ClientIDMode="Static">
                        <Columns>
                            <asp:BoundField DataField="EnqNo" HeaderText="ENQUIRY NO" />
                            <asp:BoundField DataField="EnqSentDate" HeaderText="ENQUIRY DATE" />
                            <asp:BoundField DataField="ExpiredDate" HeaderText="ENQUIRY EXPIRED DATE" ItemStyle-Width="140px" />
                            <asp:TemplateField HeaderText="ENQUIRY COMMENTS" ItemStyle-Width="180px" ItemStyle-CssClass="comments" >
                                <ItemTemplate>
                                    <div style="max-width: 200px; height: 20px;">
                                        <div style="max-width: 180px; height: 20px; float: left; overflow: hidden"  class="comment">
                                            <%#Eval("EnqComments")%>
                                        </div>
                                        <literal style="float: left; margin-top: -5px; margin-left: 5px;">... </literal>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="NumberOfSuppliers" HeaderText="NO OF ENQUIRED SUPP" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Width="100px" />
                            <asp:BoundField DataField="NoOfQuotedSuppliers" HeaderText="NO OF QUOTED SUPP" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Width="100px" />
                            <asp:BoundField DataField="NoOfPendingSuppliers" HeaderText="NO OF PENDING SUPP"
                                ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px" />
                            
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <input type="button" id="btnShowEnqDetails" value="SHOW DETAILS" onclick='ShowDetails(<%#Eval("EnqID")%>)' />
                                    <asp:HiddenField runat="server" ID="hdnEnqId" ClientIDMode="Static" Value='<%#Eval("EnqID")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr style="line-height: 20px; background-color: #011229; color: #fff;">
                <td>
                    <asp:Label runat="server" ID="lblEnquiryNo" ClientIDMode="Static"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView runat="server" ID="gvEnqDetails" ClientIDMode="Static" AutoGenerateColumns="false">
                        <Columns>
                            <asp:TemplateField HeaderText="SUPPLIER">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="lnkSupplierQuote" Text='<%#Eval("SupplierName")%>'
                                        OnClick="lnkSupplierQuote_Click" CssClass="uppercasesupplier"></asp:LinkButton>
                                    <asp:HiddenField ID="hdnSuppEmailID" Value='<%#Eval("SuppEmailID")%>' runat="server" />
                                    <asp:HiddenField ID="hdnSuppCountry" Value='<%#Eval("SuppCountry")%>' runat="server" />
                                    <asp:HiddenField ID="hdnSupplierId" Value='<%#Eval("SupplierId")%>' runat="server" />
                                    <asp:HiddenField ID="hdnSuppAddress" Value='<%#Eval("SuppAddress")%>' runat="server" />
                                    <asp:HiddenField ID="hdnSuppCity" Value='<%#Eval("SuppCity")%>' runat="server" />
                                    <asp:HiddenField ID="hdnSupplierComment" Value='<%#Eval("SupplierComment")%>' runat="server" />
                                    <asp:HiddenField ID="hdnEnquiryNo" Value='<%#Eval("EnquiryNo")%>' runat="server" />
                                    <asp:HiddenField ID="hdnEnquiredId" Value='<%#Eval("EnquiredId")%>' runat="server" />
                                    <asp:HiddenField ID="hdnEnquiryStatus" Value='<%#Eval("EnquiryStatus")%>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="QuoteFinalDate" HeaderText="QUOTATION FINALED DATE" />
                            <asp:BoundField DataField="ProdQuantity" HeaderText="ENQUIRY ITEM" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Right"  />
                            <asp:TemplateField HeaderText="CHARGES DESCP" ItemStyle-Width="200px" >
                                <ItemTemplate>
                                    <%#Eval("ChargesDescription")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CHARGES %" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="80px" >
                                <ItemTemplate>
                                    <%#Eval("ChargesPercent") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CHARGES RATE" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="80px" >
                                <ItemTemplate>
                                    <%#Eval("ChargesRate")%>
                                </ItemTemplate>
                            </asp:TemplateField> 
                            <asp:BoundField DataField="TotAmount" HeaderText="TOTAL QUOTED AMOUNT" ItemStyle-Width="140px" ItemStyle-HorizontalAlign="Right" /> 
                            <asp:BoundField DataField="EnquiryStatusName" HeaderText="ENQUIRY STATUS" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
</div>
<div id="divDetails" class="hide">
    <span id="spnSupplierName" style="font-weight: bold;">SUPPLIER NAME :
        <asp:Label ID="lblSupplierName" runat="server" CssClass="PopUpSupplier"></asp:Label>
    </span>
    <div>
        <asp:GridView runat="server" ID="gvEnquirySuppliersProduct" AutoGenerateColumns="false"
            ClientIDMode="Static">
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
                <asp:BoundField DataField="ProductAmnt" HeaderText="PRODUCT AMOUNT" ItemStyle-HorizontalAlign="Right"  />
                <asp:TemplateField Visible="false">
                    <ItemTemplate>
                        <asp:HiddenField ID="hdnEnquiryNo" Value='<%#Eval("EnquiryNo")%>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <div style="text-align: center; margin-bottom: 5px; margin-top: 5px;">
        <asp:Button ID="btnConfirm" Text="CONFIRM PURCHASE" OnClick="btnConfirm_Click" runat="server"
            ClientIDMode="Static" />
    </div>
</div>
<div id="divPopUpSupplier" class="popup-bottom-left hide">
    <table style="width: 93%; line-height: 18px;">
        <tbody>
            <tr>
                <td style="float: left;">
                    <label class="bold">
                        Name :
                    </label>
                    <span id="spnName" runat="server" clientidmode="Static"></span>
                </td>
            </tr>
            <tr>
                <td style="float: right;">
                    <label class="bold">
                        Email :
                    </label>
                    <span id="spnEmail" runat="server" clientidmode="Static"></span>
                </td>
            </tr>
            <tr>
                <td style="float: left;">
                    <label class="bold">
                        Country
                    </label>
                    <span id="spnCountry" runat="server" clientidmode="Static"></span>
                </td>
            </tr>
            <tr>
                <td style="float: right;">
                    <label class="bold">
                        City :</label>
                    <span id="spnCity" runat="server" clientidmode="Static"></span>
                </td>
            </tr>
            <tr>
                <td>
                    <label class="bold">
                        Comments :</label>
                    <br />
                    <div style="background-color: #e4f7cf; width: 400px; height: 80px; margin-top: 10px;
                        border: 1px solid black; margin-left: 40px; padding-left: 5px; padding-top: 5px;"
                        id="divComments" runat="server" clientidmode="Static">
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
</div>
<asp:HiddenField runat="server" ID="hdnSelectIndex" ClientIDMode="Static" Value="" />
<asp:HiddenField runat="server" ID="hdnsuppID" ClientIDMode="Static" Value="" />
<script type="text/javascript">
    $("#lblPageHead").text("enquiry quoted list");
    function ShowDetails(enqid) {
        window.location = "ebid_purchase.aspx?vid=7&enqid=" + enqid;
    }

    $(".comments").click(function (e, ele) {
        alert($(this).find(".comment").text());
    });

   
    // --------------------------- on mouse enter/leave to/from supplier name --------------------------
    $(".PopUpSupplier").mouseenter(function (eve) {
        var position = $(this).offset();
        var left = (position.left + 40) + "px";
        var top = (position.top + 10) + "px";
        $("#divPopUpSupplier").css("left", left);
        $("#divPopUpSupplier").css("top", top);
        $("#divPopUpSupplier").stop(true, true);
        $("#divPopUpSupplier").delay(50).fadeIn("slow");
    });
    $(".PopUpSupplier").mouseleave(function (eve) {
        $("#divPopUpSupplier").stop(true, true);
        $("#divPopUpSupplier").delay(50).fadeOut("slow"); // $("#divPopUpSupplier").addClass("fadeout");
    });
    $("#divPopUpSupplier").mouseenter(function (eve) { jQuery("#divPopUpSupplier").stop(true, true); $("#divPopUpSupplier").fadeIn("slow") });
    $("#divPopUpSupplier").mouseleave(function (eve) { jQuery("#divPopUpSupplier").stop(true, true); $("#divPopUpSupplier").fadeOut("slow") });
    //---------------------- divDetails portion -------------------------
    function showDetailsDiv(val) {
        if (val == 0) {
            $("#divDetails").addClass("hide");
        }
        else if (val == 1) {
            $("#divDetails").removeClass("hide");
            var rowTop = "";
            var count = 1;
            /*
            $("#gvEnquirySuppliersProduct tr").each(function (e) {
                if (e == 1) {
                    rowTop = this;
                    $(this).find("td").eq(4).attr("rowspan", "2");
                    $(this).find("td").eq(5).attr("rowspan", "2");
                    $(this).find("td").eq(6).attr("rowspan", "2");
                }
                if (e > 1) {
                    $(this).find("td").eq(4).css("display", "none");
                    $(this).find("td").eq(5).css("display", "none");
                    $(this).find("td").eq(6).css("display", "none");
                    count = count + 1;
                }
            });
            $(rowTop).find("td").eq(4).attr("rowspan", count);
            $(rowTop).find("td").eq(5).attr("rowspan", count);
            $(rowTop).find("td").eq(6).attr("rowspan", count);
            */
        }
    }
</script>
