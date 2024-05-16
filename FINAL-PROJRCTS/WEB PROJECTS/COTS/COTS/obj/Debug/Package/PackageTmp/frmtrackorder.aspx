<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="frmtrackorder.aspx.cs" Inherits="COTS.frmtrackorder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Styles/datemonthyear.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .tbtable
        {
            background-color: #ffffff;
            border-color: #cccbfb;
            border-collapse: separate;
            line-height: 25px;
        }
        .tbtable th
        {
            background-color: #ccf1c7;
            text-align: left;
            font-weight: normal;
        }
        .tbtable td
        {
            background-color: #fffce5;
            font-weight: bold;
        }
        ol.progtrckr li
        {
            display: inline-block;
            text-align: center;
            line-height: 17.2em;
            width: 72.0px;
        }
        ol.progtrckr li.progtrckr-done
        {
            color: black;
            border-bottom: 4px solid yellowgreen;
        }
        ol.progtrckr li.progtrckr-todo
        {
            color: silver;
            border-bottom: 4px solid silver;
        }
        
        ol.progtrckr li:after
        {
            content: "\00a0\00a0";
        }
        ol.progtrckr li:before
        {
            position: relative;
            float: left;
            left: 24px;
            line-height: 1em;
        }
        ol.progtrckr li.progtrckr-done:before
        {
            content: "\2713";
            color: white;
            background-color: #B6DE45;
            height: 2.2em;
            width: 2.2em;
            line-height: 2.2em;
            border-radius: 2.2em;
            bottom: -16.1em;
        }
        ol.progtrckr li.progtrckr-todo:before
        {
            content: "\039F";
            color: silver;
            background-color: white;
            font-size: 2.2em;
            bottom: -7.4em;
        }
        .test
        {
            display: inline-block;
            width: 372px;
            margin-left: -17.6em;
            font-size: 12px;
            padding-left: 120px;
            text-align: left;
            transform: rotate(300deg);
            font-weight: bold;
            line-height: 13px;
            position: absolute;
            bottom: 170px;
        }
        .test1
        {
            display: inline-block;
            width: 370px;
            margin-left: -25.0em;
            font-size: 12px;
            padding-left: 270px;
            text-align: left;
            transform: rotate(300deg);
            position: absolute;
            bottom: 12px;
        }
        .white_content
        {
            display: none;
            position: fixed;
            width: 1174px;
            height: 275px;
            padding: 2px;
            border: 3px solid #3ad1d8;
            background-color: #f7efef;
            left: 62px;
            bottom: 0px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div align="center" class="pageTitleHead">
        TRACK MY ORDER</div>
    <div class="contPage">
        <table cellspacing="0" rules="all" border="1" style="width: 100%; background-color: #ffffff;
            border-color: #cccbfb; border-collapse: separate;">
            <tr>
                <td>
                    <asp:GridView runat="server" ID="gv_TrackOrderList" AutoGenerateColumns="false" Width="100%"
                        Font-Bold="true" AllowPaging="true" OnPageIndexChanging="gv_TrackOrderList_PageIndex"
                        PageSize="50" PagerStyle-Height="30px" PagerStyle-Font-Bold="true" PagerStyle-Font-Size="15px"
                        PagerStyle-HorizontalAlign="Center" PagerStyle-VerticalAlign="Middle">
                        <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" Height="22px" ForeColor="Black"
                            HorizontalAlign="Center" />
                        <Columns>
                            <asp:BoundField HeaderText="ORDER NO" DataField="OrderRefNo" />
                            <asp:BoundField HeaderText="ORDER DATE" DataField="CompletedDate" />
                            <asp:BoundField HeaderText="LAST REVISED DATE" DataField="RevisedDate" />
                            <asp:BoundField HeaderText="DESIRED SHIP DATE" DataField="DesiredShipDate" />
                            <asp:BoundField HeaderText="EXPECT SHIPPING DATE" DataField="ExpectedShipDate" />
                            <asp:BoundField HeaderText="DISPATCHED DATE" DataField="DispatchedDate" />
                            <asp:BoundField HeaderText="QTY" DataField="itemqty" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField HeaderText="INVOICE DATE" DataField="invoicedate" />
                            <asp:BoundField HeaderText="INVOICE NO" DataField="invoiceno" />
                            <asp:BoundField HeaderText="STATUS" DataField="StatusText" />
                            <asp:TemplateField HeaderText="ACTION" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="lnkTrackOrder" ClientIDMode="Static" Text="TRACK"
                                        OnClick="lnkTrackOrder_Click" CommandArgument='<%#Eval("ID").ToString() %>'></asp:LinkButton>
                                    <%# Eval("StatusText").ToString().ToLower() == "proforma generated" ? 
                                    "<br/><span style='line-height: 15px;'>Download proforma and confirm order</span>" : "" %>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerSettings Mode="Numeric" Position="Bottom" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="divItemDetails" style="display: none;">
                        <asp:DataList runat="server" ID="dlOrderMaster" RepeatColumns="1" RepeatDirection="Horizontal"
                            RepeatLayout="Table" AlternatingItemStyle-BackColor="#A1C9D3" ItemStyle-BackColor="#C9DAC9"
                            ItemStyle-VerticalAlign="Top" Width="100%">
                            <ItemTemplate>
                                <table cellspacing="0" rules="all" border="1" style="width: 100%;" class="tbtable">
                                    <tr>
                                        <th>
                                            ORDER NO
                                        </th>
                                        <td colspan="7" style="font-size: 15px;">
                                            <asp:Label runat="server" ID="lblOrderNo" ClientIDMode="Static" Text='<%# Eval("OrderRefNo")%>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            ORDER DATE
                                        </th>
                                        <td>
                                            <asp:Label runat="server" ID="lblOrderDate" ClientIDMode="Static" Text='<%# Eval("CompletedDate")%>'></asp:Label>
                                        </td>
                                        <th>
                                            DESIRED SHIP DATE
                                        </th>
                                        <td>
                                            <%# Eval("DesiredShipDate")%>
                                        </td>
                                        <th>
                                            DELIVERY METHOD
                                        </th>
                                        <td colspan="3">
                                            <asp:Label runat="server" ID="lblDeliveryMethod" ClientIDMode="Static" Text='<%# Eval("DeliveryMethod")%>'></asp:Label>
                                            <br />
                                            <asp:Label runat="server" ID="lblGodownName" ClientIDMode="Static" Text='<%# Eval("GodownName") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            EXPECTED EX-FACTORY SHIP DATE
                                        </th>
                                        <td>
                                            <asp:Label ID="lblExpectedDate" Text='<%# Eval("ExpectedShipDate")%>' CssClass="lblCss"
                                                runat="server"></asp:Label>
                                        </td>
                                        <th>
                                            SHIPPING AGENT
                                        </th>
                                        <td>
                                            <asp:Label runat="server" ID="lblTransportDetails" ClientIDMode="Static" Text='<%# Eval("TransportDetails")%>'></asp:Label>
                                        </td>
                                        <th>
                                            <asp:Label runat="server" ID="lblTypeOf" ClientIDMode="Static" Text="CONTAINER TYPE"></asp:Label>
                                        </th>
                                        <td>
                                            <asp:Label runat="server" ID="lblFreightCharges" ClientIDMode="Static" Text='<%# Eval("FreightCharges")%>'></asp:Label>
                                        </td>
                                        <th>
                                            PACKING METHOD
                                        </th>
                                        <td>
                                            <%# Eval("PackingMethod")%>
                                            <%# Eval("PackingOthers").ToString() !="" ? (" - " +Eval("PackingOthers").ToString()):"" %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            BILL TO
                                        </th>
                                        <td colspan="3" style="vertical-align: top; line-height: 18px;">
                                            <%# Bind_BillingAddress(Eval("BillID").ToString())%>
                                        </td>
                                        <th>
                                            SHIP TO
                                        </th>
                                        <td colspan="3" style="vertical-align: top; line-height: 18px;">
                                            <%# Bind_BillingAddress(Eval("ShipID").ToString())%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            SPECIAL INSTRUCTION
                                        </th>
                                        <td colspan="3">
                                            <%# Eval("SplIns")%>
                                        </td>
                                        <th>
                                            REMARKS
                                        </th>
                                        <td colspan="3">
                                            <%# Eval("SpecialRequset")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            STATUS
                                        </th>
                                        <td colspan="7">
                                            <div style="font-weight: bold; font-size: 20px; float: left; width: 100%;">
                                                <%# Eval("StatusText") %>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:DataList>
                        <asp:GridView runat="server" ID="gvOrderItemList" AutoGenerateColumns="false" Width="100%"
                            AlternatingRowStyle-BackColor="#f5f5f5" ShowFooter="true" FooterStyle-Font-Bold="true"
                            FooterStyle-BackColor="#cccffd" FooterStyle-HorizontalAlign="Right" HeaderStyle-BackColor="#FEFE8B"
                            HeaderStyle-Font-Bold="true" HeaderStyle-Height="22px">
                            <Columns>
                                <asp:BoundField HeaderText="TYRE SIZE" DataField="tyresize" />
                                <asp:BoundField HeaderText="RIM" DataField="rimsize" />
                                <asp:BoundField HeaderText="TYPE" DataField="tyretype" />
                                <asp:BoundField HeaderText="BRAND" DataField="brand" />
                                <asp:BoundField HeaderText="SIDEWALL" DataField="sidewall" />
                                <asp:TemplateField HeaderText="PROCESS CODE">
                                    <ItemTemplate>
                                        <%#Eval("processid") %>
                                        <%# Eval("AssyRimstatus").ToString() == "True" && Eval("EdcNo").ToString() != "" ? (" EDC " + Eval("EdcNo")) : ""%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="UNIT PRICE" DataField="unitprice" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="UNIT WT" DataField="unitwt" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="QTY" DataField="itemqty" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="TOTAL PRICE" DataField="totprice" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="TOTAL WT(Kgs)" DataField="totwt" ItemStyle-HorizontalAlign="Right" />
                            </Columns>
                        </asp:GridView>
                        <div id="divStatusButton" style="display: none; width: 90%; float: left;">
                            <div class="headCss" style="width: 100%; float: left;">
                                COMMENTS
                                <asp:TextBox runat="server" ID="txtOrderChangeComments" ClientIDMode="Static" Text=""
                                    TextMode="MultiLine" MaxLength="3999" Width="100%" Height="80px"></asp:TextBox>
                            </div>
                            <div style="width: 100%; float: left; text-align: center;">
                                <asp:Button runat="server" ID="btnStatusChange" ClientIDMode="Static" Text="CONFIRM ORDER"
                                    OnClick="btnStatusChange_Click" CssClass="btnAuthorize" />
                            </div>
                        </div>
                        <div style="width: 10%; float: right;">
                            <asp:ImageButton ID="imgdownload" ImageUrl="images/Download.png" ClientIDMode="Static"
                                runat="server" CssClass="imageCss" />
                        </div>
                        <div id="orderstatus" style="display: none" class="white_content">
                            <div style="font-weight: bold; width: 100%; float: left;">
                                <asp:Label runat="server" ID="lblReviseCount" ClientIDMode="Static" Text="" ForeColor="#fb8c4b"></asp:Label>
                            </div>
                            <ol class="progtrckr" style="padding-left: 25px; padding-top: 30px; padding: 5px;">
                                <li id="1" class="progtrckr-done">
                                    <asp:Label runat="server" ID="lblOrderReceived" ClientIDMode="Static" Text="ORDER RECEIVED"
                                        CssClass="test"></asp:Label>
                                    <asp:Label runat="server" ID="lblOrderReceiveddt" ClientIDMode="Static" Text="" CssClass="test1"></asp:Label>
                                </li>
                                &nbsp;
                                <li id="2" class="progtrckr-todo">
                                    <asp:Label runat="server" ID="lblProformaPrepared" ClientIDMode="Static" Text="PROFORMA GENERATED"
                                        CssClass="test"></asp:Label>
                                    <asp:Label runat="server" ID="lblProformaPrepareddt" ClientIDMode="Static" Text=""
                                        CssClass="test1"></asp:Label>
                                </li>
                                &nbsp;
                                <li id="3" class="progtrckr-todo">
                                    <asp:Label runat="server" ID="lblOrderConfirmed" ClientIDMode="Static" Text="PROFORMA CONFIRMED"
                                        CssClass="test"></asp:Label>
                                    <asp:Label runat="server" ID="lblOrderConfirmeddt" ClientIDMode="Static" Text=""
                                        CssClass="test1"></asp:Label>
                                </li>
                                &nbsp;
                                <li id="4" class="progtrckr-todo">
                                    <asp:Label runat="server" ID="lblWorkOrder" ClientIDMode="Static" Text="ORDER PROCESSING"
                                        CssClass="test"></asp:Label>
                                    <asp:Label runat="server" ID="lblWorkOrderdt" ClientIDMode="Static" Text="" CssClass="test1"></asp:Label>
                                </li>
                                &nbsp;
                                <li id="7" class="progtrckr-todo">
                                    <asp:Label runat="server" ID="lblRFDByPPC" ClientIDMode="Static" Text="EXPECTED EX-FACTORY<br/>SHIP DATE"
                                        CssClass="test"></asp:Label>
                                    <asp:Label runat="server" ID="lblRFDByPPCdt" ClientIDMode="Static" Text="" CssClass="test1"></asp:Label>
                                </li>
                                &nbsp;
                                <li id="5" class="progtrckr-todo">
                                    <asp:Label runat="server" ID="lblProdComplete" ClientIDMode="Static" Text="PRODUCTION COMPLETED"
                                        CssClass="test"></asp:Label>
                                    <asp:Label runat="server" ID="lblProdCompletedt" ClientIDMode="Static" Text="" CssClass="test1"></asp:Label>
                                </li>
                                &nbsp;
                                <li id="6" class="progtrckr-todo">
                                    <asp:Label runat="server" ID="lblInspect" ClientIDMode="Static" Text="PRE-DELIVERY INSPECTION<br/>& PACKING"
                                        CssClass="test"></asp:Label>
                                    <asp:Label runat="server" ID="lblInspectdt" ClientIDMode="Static" Text="" CssClass="test1"></asp:Label>
                                </li>
                                &nbsp;
                                <li id="8" class="progtrckr-todo">
                                    <asp:Label runat="server" ID="lblDispatch" ClientIDMode="Static" Text="ORDER SHIPPED"
                                        CssClass="test"></asp:Label>
                                    <asp:Label runat="server" ID="lblDispatchdt" ClientIDMode="Static" Text="" CssClass="test1"></asp:Label>
                                </li>
                                &nbsp;
                                <li id="9" class="progtrckr-todo">
                                    <asp:Label runat="server" ID="lblTentVessel" ClientIDMode="Static" Text="TENTATIVE VESSEL SAILING DATE"
                                        CssClass="test"></asp:Label>
                                    <asp:Label runat="server" ID="lblTentVesseldt" ClientIDMode="Static" Text="" CssClass="test1"></asp:Label>
                                </li>
                                &nbsp;
                                <li id="10" class="progtrckr-todo">
                                    <asp:Label runat="server" ID="lblActVessel" ClientIDMode="Static" Text="VESSEL ARRIVAL AT POD"
                                        CssClass="test"></asp:Label>
                                    <asp:Label runat="server" ID="lblActVesseldt" ClientIDMode="Static" Text="" CssClass="test1"></asp:Label>
                                </li>
                                &nbsp;
                                <li id="11" class="progtrckr-todo">
                                    <asp:Label runat="server" ID="lblDocMail" ClientIDMode="Static" Text="SHIPPING DOCUMENTS SENT<br/>TO CUSTOMER BY E-MAIL"
                                        CssClass="test"></asp:Label>
                                    <asp:Label runat="server" ID="lblDocMaildt" ClientIDMode="Static" Text="" CssClass="test1"></asp:Label>
                                </li>
                                &nbsp;
                                <li id="12" class="progtrckr-todo">
                                    <asp:Label runat="server" ID="lblDocCourier" ClientIDMode="Static" Text="ORIGINAL DOCUMENTS<br/>COURIERED"
                                        Font-Bold="true" CssClass="test"></asp:Label>
                                    <asp:Label runat="server" ID="lblDocCourierdt" ClientIDMode="Static" Text="" CssClass="test1"></asp:Label>
                                </li>
                                &nbsp;
                                <li id="13" class="progtrckr-todo">
                                    <asp:Label runat="server" ID="lblPaymentReceived" ClientIDMode="Static" Text="PAYMENT RECEIVED"
                                        CssClass="test"></asp:Label>
                                    <asp:Label runat="server" ID="lblPaymentReceiveddt" ClientIDMode="Static" Text=""
                                        CssClass="test1"></asp:Label>
                                </li>
                                &nbsp;
                                <li id="14" class="progtrckr-todo">
                                    <asp:Label runat="server" ID="lblDelivery" ClientIDMode="Static" Text="CONTAINER DELIVERED<br/>AT DESTINATION"
                                        CssClass="test"></asp:Label>
                                    <asp:Label runat="server" ID="lblDeliverydt" ClientIDMode="Static" Text="" CssClass="test1"></asp:Label>
                                </li>
                            </ol>
                            <div style="font-weight: bold; width: 100%; float: left;">
                                <span onclick="popup_close();" style="cursor: pointer; font-size: 20px; width: 15px;
                                    float: right; color: #213aef;">X</span>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="dialog" style="display: none;">
        <asp:GridView runat="server" ID="gv_DownloadFiles" ClientIDMode="Static" AutoGenerateColumns="false"
            CssClass="gridcss">
            <HeaderStyle />
            <Columns>
                <asp:TemplateField HeaderText="File Type">
                    <ItemTemplate>
                        <asp:Label ID="lblFileType" runat="server" Text='<%# Eval("FileType")%>' CssClass="spanCss">
                        </asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Click link to Download File" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkPdfFileName" runat="server" Text='<%# Eval("AttachFileName")%>'
                            ForeColor="#ec5252" OnClick="ddl_DownloadFiles_ItemCommand">
                        </asp:LinkButton></ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <asp:HiddenField runat="server" ID="hdnOrderNo" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnOID" ClientIDMode="Static" Value="" />
    <script src="Scripts/datemonthyear.js" type="text/javascript"></script>
    <script type="text/javascript">
        function gotoShowDiv(ctrlID) {
            $('#' + ctrlID).css({ 'display': 'block' });
            $("html, body").stop().animate({ scrollLeft: $('#' + ctrlID).offset().left, scrollTop: $('#' + ctrlID).offset().top }, 1200);
        }
        $('#imgdownload').click(function () {
            $("#dialog").dialog({ title: "Download PDF Files", modal: true, width: '600px', top: '1000px', left: '500px',
                buttons: [{ id: "Close", text: "Close", click: function () { $(this).dialog('close'); } }]
            });
            $("#dialog").dialog("open");
            return false;
        });
        $(document).ready(function () {
            $("#<%=gv_TrackOrderList.ClientID%> tr:has(td)").each(function () {
                if ($(this).find("td:eq(9)").html() == "ORDER SHIPPED")
                    $(this).find("td:eq(9)").css({ "background-color": "#e9fd81" });
                else if ($(this).find("td:eq(9)").html() == "DISPATCHED")
                    $(this).find("td:eq(9)").css({ "background-color": "#86f971" });
            });
        });


        function lnkTrackBtn_Click(k) {
            $('#' + k).removeClass('progtrckr-todo').addClass('progtrckr-done');
        }
        function popup_open() {
            $('#orderstatus').show();
        }
        function popup_close() {
            $('#orderstatus').hide();
        }
    </script>
</asp:Content>
