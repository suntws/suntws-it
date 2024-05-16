<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="exportcotsproduction.aspx.cs" Inherits="TTS.exportcotsproduction" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/datemonthyear.css" rel="stylesheet" type="text/css" />
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/cotsScript.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        EXPORT WORK ORDER PREPARE
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="background-color: #dcecfb; width: 100%;
            border-color: White; border-collapse: separate;">
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" Font-Bold="true"
                        Font-Size="20px" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView runat="server" ID="gvProductionOrderList" AutoGenerateColumns="false"
                        Width="100%" HeaderStyle-BackColor="#FFCC00" RowStyle-Height="22px" HeaderStyle-Height="25px">
                        <Columns>
                            <asp:TemplateField HeaderText="CUSTOMER NAME" ItemStyle-Width="250px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatusCustName" Text='<%# Eval("custfullname") %>'></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdnStatusCustCode" Value='<%# Eval("CustCode") %>' />
                                    <asp:HiddenField runat="server" ID="hdnUserCurrency" Value='<%# Eval("usercurrency") %>' />
                                    <asp:HiddenField runat="server" ID="hdnCreditNote" Value='<%# Eval("CreditNote") %>' />
                                    <asp:HiddenField runat="server" ID="hdnRequestStatus" Value='<%# Eval("RequestStatus") %>' />
                                    <asp:HiddenField runat="server" ID="hdnOrderID" Value='<%# Eval("ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ORDER REF NO." ItemStyle-Width="220px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOrderRefNo" Text='<%#Eval("OrderRefNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ORDERED DATE" ItemStyle-Width="70px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOrderDate" Text='<%#Eval("CompletedDate") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="LAST REVISED DATE" DataField="RevisedDate" ItemStyle-Width="70px" />
                            <asp:BoundField HeaderText="CUSTOMER DESIRED SHIP DATE" DataField="DesiredShipDate"
                                ItemStyle-Width="70px" />
                            <asp:BoundField HeaderText="ORDER QTY" DataField="itemqty" ItemStyle-Width="40px"
                                ItemStyle-HorizontalAlign="Right" />
                            <asp:TemplateField HeaderText="SHIPMENT TYPE" ItemStyle-Width="70px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblShipmenType" Text='<%#Eval("ShipmentType") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="STATUS" ItemStyle-Width="180px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatusText" Text='<%# Eval("StatusText") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="60px" HeaderText="ACTION" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkProductionBtn" runat="server" Text="Process" OnClick="lnkProductionBtn_Click"
                                        Font-Bold="true" /></span>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="width: 100%; float: left; display: none;" id="divStatusChange">
                        <table cellspacing="0" rules="all" border="1" style="background-color: #ecf6ff; border-color: White;
                            border-collapse: separate; width: 100%;">
                            <tr id="divOrderHead" style="text-align: center; line-height: 30px; font-size: 14px;">
                                <td>
                                    <asp:Label runat="server" ID="lblCustName" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblStausOrderRefNo" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:DataList runat="server" ID="dlOrderMaster" RepeatColumns="1" RepeatDirection="Horizontal"
                                        RepeatLayout="Table" AlternatingItemStyle-BackColor="#A1C9D3" ItemStyle-BackColor="#C9DAC9"
                                        ItemStyle-VerticalAlign="Top" Width="1100px">
                                        <ItemTemplate>
                                            <table cellspacing="0" rules="all" border="1" style="background-color: #ecf6ff; border-color: White;
                                                border-collapse: separate; width: 100%;">
                                                <tr>
                                                    <th>
                                                        Orderd Date
                                                    </th>
                                                    <td>
                                                        <%# Eval("CompletedDate")%>
                                                    </td>
                                                    <th>
                                                        Delivery Method
                                                    </th>
                                                    <td>
                                                        <%# Eval("DeliveryMethod")%>
                                                        -
                                                        <%# Eval("GodownName") %>
                                                        -
                                                        <%#Eval("TransportDetails")%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        Desired Ship Date
                                                    </th>
                                                    <td>
                                                        <%# Eval("DesiredShipDate")%>
                                                    </td>
                                                    <th>
                                                        Packing Method
                                                    </th>
                                                    <td>
                                                        <%# Eval("PackingMethod") %>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        SHIPMENT TYPE
                                                    </th>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblShipmentType" Text='<%# Eval("ShipmentType") %>'></asp:Label>
                                                    </td>
                                                    <th>
                                                        CONTAINER FINAL LOADING
                                                    </th>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblContainerLoadFrom" Text='<%# Eval("ContainerLoadFrom")%>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th colspan="2" style="text-align: left;">
                                                        Special Instruction:&nbsp;
                                                        <%# Eval("SplIns").ToString().Replace("~", "\r\n") %>
                                                    </th>
                                                    <th colspan="2" style="text-align: left;">
                                                        Special Notes:&nbsp;
                                                        <%# Eval("SpecialRequset").ToString().Replace("~", "\r\n") %>
                                                    </th>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </asp:DataList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:GridView runat="server" ID="gvOrderItemList" AutoGenerateColumns="false" Width="1100px"
                                        ShowFooter="true" FooterStyle-Font-Bold="true" FooterStyle-HorizontalAlign="Right"
                                        FooterStyle-BackColor="#FAAC58" CssClass="gridcss">
                                        <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" Height="22px" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="CATEGORY" ItemStyle-Width="100px">
                                                <ItemTemplate>
                                                    <%# Eval("category") %>
                                                    <%# Eval("AssyRimstatus").ToString() == "True" ? " (ASSY)" : ""%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="PLATFORM" DataField="config" ItemStyle-Width="100px" />
                                            <asp:BoundField HeaderText="TYRE SIZE" DataField="tyresize" ItemStyle-Width="150px" />
                                            <asp:BoundField HeaderText="RIM" DataField="rimsize" ItemStyle-Width="40px" />
                                            <asp:BoundField HeaderText="TYPE" DataField="tyretype" ItemStyle-Width="60px" />
                                            <asp:BoundField HeaderText="BRAND" DataField="brand" ItemStyle-Width="100px" />
                                            <asp:BoundField HeaderText="SIDEWALL" DataField="sidewall" ItemStyle-Width="100px" />
                                            <asp:BoundField HeaderText="BASIC PRICE" DataField="listprice" ItemStyle-HorizontalAlign="Right"
                                                ItemStyle-Width="100px" />
                                            <asp:BoundField HeaderText="QTY" DataField="itemqty" ItemStyle-HorizontalAlign="Right"
                                                ItemStyle-Width="40px" />
                                            <asp:BoundField HeaderText="TOTAL PRICE" DataField="unitpricepdf" ItemStyle-HorizontalAlign="Right"
                                                ItemStyle-Width="120px" />
                                            <asp:BoundField HeaderText="FWT" DataField="totalfwt" ItemStyle-HorizontalAlign="Right"
                                                ItemStyle-Width="80px" />
                                            <asp:TemplateField HeaderText="RIM PRICE" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px">
                                                <ItemTemplate>
                                                    <%# Eval("Rimunitprice").ToString() == "0.00" ? "" : Eval("Rimunitprice").ToString()%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="RIM QTY" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="40px">
                                                <ItemTemplate>
                                                    <%# Eval("Rimitemqty").ToString() == "0" ? "" : Eval("Rimitemqty").ToString()%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="TOT RIM PRICE" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <%# Eval("Rimpricepdf").ToString() == "0.00" ? "" : Eval("Rimpricepdf").ToString()%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="RIM FWT" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="80px">
                                                <ItemTemplate>
                                                    <%# Eval("Rimfinishedwt").ToString() == "0.00" ? "" : Eval("Rimfinishedwt").ToString()%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="AdditionalInfo" HeaderText="ADDITIONAL INFO" />
                                            <asp:BoundField HeaderText="PLANT" DataField="ItemPlant" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:GridView runat="server" ID="gv_OrderSumValue" AutoGenerateColumns="true" Width="100%"
                                        HeaderStyle-BackColor="#166502" HeaderStyle-ForeColor="#ffffff" ShowFooter="true"
                                        FooterStyle-BackColor="#dfe0f3" RowStyle-HorizontalAlign="Right" RowStyle-VerticalAlign="Middle"
                                        FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label runat="server" ID="lblCurrStatus" ClientIDMode="Static" Text="" Font-Bold="true"
                                        Font-Size="Large"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table cellspacing="0" rules="all" border="1" style="background-color: #dcecfb; width: 100%;
                                        border-color: White; border-collapse: separate;">
                                        <tr>
                                            <th>
                                                Proforma Ref No.
                                            </th>
                                            <td>
                                                <asp:Label runat="server" ID="lblProformaRefNo" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                                            </td>
                                            <th rowspan="6">
                                                Pre Dispatch
                                            </th>
                                            <td rowspan="6" style="line-height: 30px;">
                                                <asp:CheckBox runat="server" ID="chk1" ClientIDMode="Static" Text="FUMIGATION" Value="1" />
                                                <br />
                                                <asp:CheckBox runat="server" ID="chk2" ClientIDMode="Static" Text="SPIRAL WRAP" Value="2" />
                                                <br />
                                                <asp:CheckBox runat="server" ID="chk3" ClientIDMode="Static" Text="SHRINK WRAP" Value="3" />
                                                <br />
                                                <asp:CheckBox runat="server" ID="chk4" ClientIDMode="Static" Text="PALLETISATION / CRATE / BOX"
                                                    Value="4" />
                                                <br />
                                                <asp:CheckBox runat="server" ID="chk5" ClientIDMode="Static" Text="MADE IN INDIA MARKING"
                                                    Value="5" />
                                                <br />
                                                <asp:CheckBox runat="server" ID="chk6" ClientIDMode="Static" Text="20-FOOT CONTAINER"
                                                    Value="6" />
                                                <br />
                                                <asp:CheckBox runat="server" ID="chk7" ClientIDMode="Static" Text="THIRD PARTY INSPECTION / CERTIFICATE"
                                                    Value="7" />
                                                <br />
                                                <asp:CheckBox runat="server" ID="chk8" ClientIDMode="Static" Text="BOUGHT OUT ITEM"
                                                    Value="8" />
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                Plant
                                            </th>
                                            <td>
                                                <asp:RadioButtonList runat="server" ID="rdb_Plant" RepeatColumns="4" RepeatDirection="Horizontal"
                                                    AutoPostBack="true" OnSelectedIndexChanged="rdb_Plant_IndexChanged">
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                Work Order No.
                                            </th>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtWorkOrderNo" ClientIDMode="Static" Text="" CssClass="form-control"></asp:TextBox>
                                                Date:
                                                <asp:Label runat="server" ID="lblWoDate" ClientIDMode="Static" Text="" Width="80px"></asp:Label>
                                                Revision:
                                                <asp:Label runat="server" ID="lblReviseCount" ClientIDMode="Static" Text="" Font-Bold="true"
                                                    Width="50px"></asp:Label>
                                                <br />
                                                <asp:Label runat="server" ID="lblWoExistsMsg" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                Expected Ex-Factory Ship Date
                                            </th>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtDeliveryDate" ClientIDMode="Static" Text="" Width="100px"
                                                    CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                Delivery Priority
                                            </th>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlPriority" CssClass="form-control">
                                                    <asp:ListItem Text="AS PER CUSTOMER SCHEDULE" Value="AS PER CUSTOMER SCHEDULE"></asp:ListItem>
                                                    <asp:ListItem Text="WITH IN 24 HOURS" Value="WITH IN 24 HOURS"></asp:ListItem>
                                                    <asp:ListItem Text="WITH IN 48 HOURS" Value="WITH IN 48 HOURS"></asp:ListItem>
                                                    <asp:ListItem Text="WITH IN 1 WEEK" Value="WITH IN 1 WEEK"></asp:ListItem>
                                                    <asp:ListItem Text="WITH IN 3-4 WEEK" Value="WITH IN 3-4 WEEK"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                Special Remarks
                                            </th>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtSplRemarks" ClientIDMode="Static" Text="" TextMode="MultiLine"
                                                    onKeyUp="javascript:CheckMaxLength(this, 499);" onChange="javascript:CheckMaxLength(this, 499);"
                                                    Width="375px" Height="80px" CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="text-align: right;">
                                                <asp:CheckBox runat="server" ID="chkAssyWt" ClientIDMode="Static" Text="Do you want show the tyre & rim weight should be separate"
                                                    TextAlign="Left" Font-Bold="true" Font-Size="14px" />
                                            </td>
                                            <td colspan="2">
                                                <div id="divworkorderErr" style="width: 380px; float: right; font-weight: bold; color: #f00;">
                                                </div>
                                                <asp:Button runat="server" ID="btnPrepareWorkOrder" ClientIDMode="Static" Text="PREPARE WORK ORDER"
                                                    CssClass="btn btn-success" OnClientClick="javascript:return CtrlPrepareWorkOrder();"
                                                    OnClick="btnPrepareWorkOrder_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table cellspacing="0" rules="all" border="1" style="background-color: #dcecfb; width: 100%;
                                        border-color: White; border-collapse: separate;">
                                        <tr style="background-color: #bef9d8;">
                                            <td>
                                                <asp:GridView runat="server" ID="gv_DownloadFiles" ClientIDMode="Static" AutoGenerateColumns="false">
                                                    <HeaderStyle CssClass="headerNone" />
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFileType" runat="server" Text='<%# Eval("FileType")%>' ForeColor="#a52a2a">
                                                                </asp:Label></ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkPdfFileName" runat="server" Text='<%# Eval("AttachFileName")%>'
                                                                    OnClick="lnkPdfLink_click" OnClientClick="aspnetForm.target ='_blank';">
                                                                </asp:LinkButton></ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="statusChangeDiv" style="display: none;">
                        <table cellspacing="0" rules="all" border="1" style="background-color: #dcecfb; width: 100%;
                            border-color: White; border-collapse: separate;">
                            <tr>
                                <td>
                                    Enter your Comments
                                    <asp:TextBox runat="server" ID="txtOrderChangeComments" ClientIDMode="Static" Text=""
                                        TextMode="MultiLine" onKeyUp="javascript:CheckMaxLength(this, 999);" onChange="javascript:CheckMaxLength(this, 999);"
                                        Width="645px" Height="80px" CssClass="form-control"></asp:TextBox>
                                </td>
                                <td style="vertical-align: middle; text-align: center;">
                                    <asp:Button runat="server" ID="btnSaveChangeStatus" ClientIDMode="Static" Text="CHANGE STATUS TO ASSIGN STENCIL"
                                        CssClass="btn btn-info" OnClick="btnSaveChangeStatus_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnCustCode" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnSelectIndex" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnOrderDate" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnShipType" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnRequestStatusTo" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnOID" ClientIDMode="Static" Value="" />
    <script src="Scripts/datemonthyear.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            blinkOrderHead();

            $("#txtDeliveryDate").datepicker({ minDate: "+0D", maxDate: "+90D" }).keydown(function (e) { e.keyWhich = 0; e.keyCode = 0; e.preventDefault(); });

            $('#txtWorkOrderNo').blur(function (e) {
                $('#lblWoExistsMsg').html('');
                if ($("label[for='" + $("input:radio[id*=MainContent_rdb_Plant_]:checked").attr('id') + "']").text() == "") {
                    $('#lblWoExistsMsg').html('Choose Plant');
                    $('#txtWorkOrderNo').val('');
                }
                else if ($('#txtWorkOrderNo').val() == "") {
                    $('#lblWoExistsMsg').html('Enter work order no');
                    $('#txtWorkOrderNo').val('');
                }
                else if ($('#txtWorkOrderNo').val().length > 0) {
                    var plant = $("label[for='" + $("input:radio[id*=MainContent_rdb_Plant_]:checked").attr('id') + "']").text();
                    var Workorderno = $('#txtWorkOrderNo').val();
                    $.ajax({ type: "POST", url: "BindRecords.aspx?type=chkexpwo&plant=" + plant + "&workorderno=" + Workorderno, context: document.body,
                        success: function (data) { if (data != '') { $('#lblWoExistsMsg').html(data); $('#txtWorkOrderNo').focus(); } }
                    });
                }
            });
        });

        function CtrlPrepareWorkOrder() {
            $('#divworkorderErr').html(''); var errmsg = '';
            if ($("input:radio[id*=MainContent_rdb_Plant_]:checked").length == 0)
                errmsg += 'Choose plant<br>';
            if ($('#txtWorkOrderNo').val().length == 0)
                errmsg += 'Enter Work Order No.<br/>';
            if ($('#txtDeliveryDate').val().length == 0)
                errmsg += 'Choose expected shipping date<br/>';
            if ($("input:checkbox[id*=chk]:checked").length == 0)
                errmsg += 'Choose atleast one PreDispatch <br>';
            if ($('#lblWoExistsMsg').html().length > 0)
                errmsg += $('#lblWoExistsMsg').html();
            if (errmsg.length > 0) {
                $('#divworkorderErr').html(errmsg);
                return false;
            }
            else
                return true;
        }

        function showStatusChangeBtn() {
            $('#divStatusChange').find("input,button,textarea,radio,checkbox,select").attr("disabled", true).css({ 'cursor': 'not-allowed' });
            gotoPreviewDiv('statusChangeDiv');
        }
    </script>
</asp:Content>
