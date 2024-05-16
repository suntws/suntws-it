<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="exp_revisemasterdata.aspx.cs"
    MasterPageFile="~/master.Master" Inherits="TTS.exp_revisemasterdata" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
    <link href="styles/datemonthyear.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/cotsScript.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: center;" class="pageTitleHead">
        <asp:Label runat="server" ID="lblHeadPage" ClientIDMode="Static" Text="MASTER DATA REVISE"></asp:Label>
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
                            <asp:TemplateField HeaderText="CUSTOMER NAME" ItemStyle-Width="250px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatusCustName" Text='<%# Eval("custfullname") %>'></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdnCustCode" Value='<%# Eval("CustCode") %>' />
                                    <asp:HiddenField runat="server" ID="hdnOrderID" Value='<%# Eval("ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ORDER REFERENCE NO." ItemStyle-Width="220px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOrderRefNo" Text='<%# Eval("OrderRefNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="ORDERED DATE" DataField="CompletedDate" ItemStyle-Width="70px" />
                            <asp:BoundField HeaderText="QTY" DataField="itemqty" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="ShipmentType" HeaderText="SHIPMENT TYPE" ItemStyle-Width="70px" />
                            <asp:BoundField DataField="ContainerLoadFrom" HeaderText="FINAL LOADING" ItemStyle-Width="70px" />
                            <asp:TemplateField HeaderText="REVISE" ItemStyle-Width="170px" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="lnkMasterRevise" ClientIDMode="Static" Text="MASTER DATA"
                                        OnClick="lnkMasterRevise_Click"></asp:LinkButton>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:LinkButton runat="server" ID="lnkFinalLoad" ClientIDMode="Static" Text="FINAL LOAD"
                                        OnClick="lnkFinalLoad_Click"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="display: none;" id="tb_Master">
                        <table cellspacing="0" rules="all" border="1" style="background-color: #dcecfb; width: 100%;
                            border-color: White; border-collapse: separate;">
                            <tr>
                                <th colspan="2">
                                    <asp:Label runat="server" ID="lblSelectedCustomer" ClientIDMode="Static" Text=""></asp:Label>
                                </th>
                                <th colspan="2">
                                    <asp:Label runat="server" ID="lblSelectedOrderNo" ClientIDMode="Static" Text=""></asp:Label>
                                </th>
                            </tr>
                            <tr>
                                <th class="spanCss">
                                    BILLING
                                </th>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddl_BillingAddress" ClientIDMode="Static" AutoPostBack="true"
                                        runat="server" CssClass="form-control" Width="700px" OnSelectedIndexChanged="ddl_BillingAddress_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <th class="spanCss">
                                    SHIPPING
                                </th>
                                <td colspan="3">
                                    <asp:DropDownList runat="server" ID="ddl_ShippingAddress" ClientIDMode="Static" CssClass="form-control"
                                        AutoPostBack="true" Width="700px" OnSelectedIndexChanged="ddl_ShippingAddress_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <th class="spanCss">
                                    BILL TO
                                </th>
                                <td>
                                    <asp:Label ID="lbl_BillingAddress" Text="" Style="width: 100px;" ClientIDMode="Static"
                                        runat="server"></asp:Label>
                                </td>
                                <th class="spanCss">
                                    SHIP TO
                                </th>
                                <td>
                                    <asp:Label ID="lbl_ShipingDetails" Text="" Style="width: 100px;" ClientIDMode="Static"
                                        runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <th class="spanCss">
                                    COUNTRY OF FINAL DESTINATION
                                </th>
                                <td>
                                    <asp:TextBox ID="txt_CountryDestination" ClientIDMode="Static" runat="server" CssClass="form-control"
                                        MaxLength="20"></asp:TextBox>
                                </td>
                                <th class="spanCss">
                                    FINAL DESTINATION
                                </th>
                                <td>
                                    <asp:TextBox ID="txt_FinalDestination" ClientIDMode="Static" runat="server" CssClass="form-control"
                                        MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th class="spanCss">
                                    DELIVERY METHOD
                                </th>
                                <td>
                                    <div>
                                        <asp:RadioButtonList runat="server" ID="rdo_DeliveryTerms" ClientIDMode="Static"
                                            RepeatColumns="2" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rdo_DeliveryTerms_SelectedIndexChanged">
                                            <asp:ListItem Text="OCEAN FREIGHT" Value="SEA"></asp:ListItem>
                                            <asp:ListItem Text="AIR FREIGHT" Value="AIR"></asp:ListItem>
                                            <asp:ListItem Text="ROAD WAY FREIGHT" Value="ROAD"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </td>
                                <th class="spanCss">
                                    PACKING METHOD
                                </th>
                                <td>
                                    <div>
                                        <asp:RadioButtonList runat="server" ID="rdo_PackingMethod" ClientIDMode="Static"
                                            RepeatColumns="3" RepeatDirection="Horizontal">
                                        </asp:RadioButtonList>
                                    </div>
                                    <div id="div_MethodOthers" style="display: none;">
                                        <span style="color: #c1162e;">ENTER PACKING METHOD</span>
                                        <asp:TextBox ID="txt_PackingOthers" runat="server" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <th class="spanCss">
                                    DELIVERY TYPE
                                </th>
                                <td>
                                    <div>
                                        <asp:RadioButtonList runat="server" ID="rdo_DeliveryMethod" ClientIDMode="Static"
                                            RepeatColumns="5" RepeatDirection="Horizontal">
                                        </asp:RadioButtonList>
                                    </div>
                                </td>
                                <th class="spanCss">
                                    DESIRED SHIPPING DATE
                                </th>
                                <td>
                                    <asp:TextBox ID="txt_DesiredDate" ClientIDMode="Static" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th class="spanCss">
                                    STATIONARY REQ
                                </th>
                                <td>
                                    <asp:GridView runat="server" ID="gv_DocList" AutoGenerateColumns="false" AlternatingRowStyle-BackColor="#f5f5f5"
                                        Width="210px">
                                        <HeaderStyle CssClass="headerNone" />
                                        <Columns>
                                            <asp:BoundField DataField="DocName" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                                <th class="spanCss">
                                    OTHER DOCS
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_OtherDocuments" ClientIDMode="Static" Width="350px"
                                        Height="80px" CssClass="form-control" TextMode="MultiLine" onKeyUp="javascript:CheckMaxLength(this, 999);"
                                        onChange="javascript:CheckMaxLength(this, 999);"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th class="spanCss">
                                    SPECIAL INSTRUCTION
                                </th>
                                <td>
                                    <asp:TextBox ID="txt_SplIns" ClientIDMode="Static" runat="server" CssClass="form-control"
                                        Width="350px" Height="80px" ToolTip="Enter Special Instructions" TextMode="MultiLine"
                                        onKeyUp="javascript:CheckMaxLength(this, 3999);" onChange="javascript:CheckMaxLength(this, 3999);"></asp:TextBox>
                                </td>
                                <th class="spanCss">
                                    SPECIAL NOTES
                                </th>
                                <td>
                                    <asp:TextBox ID="txt_SplReq" runat="server" CssClass="form-control" ClientIDMode="Static"
                                        Width="350px" Height="80px" ToolTip="Enter Special Notes" TextMode="MultiLine"
                                        onKeyUp="javascript:CheckMaxLength(this, 3999);" onChange="javascript:CheckMaxLength(this, 3999);"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblErrMsg" runat="server" ClientIDMode="Static" ForeColor="Red" CssClass="lblCss"></asp:Label>
                                </td>
                                <td>
                                    <asp:Button ID="btn_Update_Records" runat="server" Text="Update Record" ClientIDMode="Static"
                                        CssClass="btn btn-success" OnClientClick="javascript:return CtrlExpSendOrder();"
                                        OnClick="btn_Update_Records_Click" />
                                </td>
                                <td>
                                    <span onclick="fnPageReload()" class="btn btn-info">Clear Selection</span>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr id="divMasterChange" style="display: none; text-align: left; vertical-align: middle;">
                <td>
                    If master data changes done, Click here for next process.
                    <asp:Button runat="server" ID="btnMasterChangeCompleted" Text="Revision Completed"
                        CssClass="btnedit" OnClick="btnMasterChangeCompleted_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    <div id="tr_ContainerLoad" style="display: none; text-align: left; vertical-align: middle;">
                        <table cellspacing="0" rules="all" border="1" style="background-color: #dcecfb; width: 100%;
                            border-color: White; border-collapse: separate;">
                            <tr>
                                <td colspan="3">
                                    <asp:GridView runat="server" ID="gv_OrderSumValue" AutoGenerateColumns="true" Width="100%"
                                        HeaderStyle-BackColor="#166502" HeaderStyle-ForeColor="#ffffff" ShowFooter="true"
                                        FooterStyle-BackColor="#A9F5A9" RowStyle-HorizontalAlign="Right" RowStyle-VerticalAlign="Middle"
                                        FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    CONTAINER FINAL LOADING
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlPlantList" CssClass="form-control" ClientIDMode="Static"
                                        Width="100px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button runat="server" ID="btnChangeFinalLoad" Text="SAVE FINAL LOADING PLANT CHANGE"
                                        CssClass="btn btn-success" OnClick="btnChangeFinalLoad_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnCustCode" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnOID" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnMasterChangeStatus" ClientIDMode="Static"
        Value="0" />
    <script src="scripts/datemonthyear.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#txt_DesiredDate").datepicker({ minDate: "+1D", maxDate: "+360D" }).keydown(function (e) { e.keyWhich = 0; e.keyCode = 0; e.preventDefault(); });
            $('#rdo_PackingMethod').change(function () {
                $('#div_MethodOthers').hide();
                if ($('#rdo_PackingMethod :checked').val() == "Others")
                    $('#div_MethodOthers').show();
            });

        });

        function CtrlExpSendOrder() {
            var errmsg = ''; $('#lblErrMsg').html('');
            if ($("#txt_DesiredDate").val().length == 0)
                errmsg += 'Enter desired shipping date</br>';
            if ($('#ddl_BillingAddress option:selected').text() == 'Choose')
                errmsg += 'Choose billing address<br/>';
            if ($('#ddl_ShippingAddress option:selected').text() == 'Choose')
                errmsg += 'Choose shipping address<br/>';
            if ($("#txt_CountryDestination").val().length == 0)
                errmsg += 'Enter country of final destination</br>';
            if ($("#txt_FinalDestination").val().length == 0)
                errmsg += 'Enter final destination</br>';
            if ($("input:radio[id*='rdo_PackingMethod_']:checked").length == 0)
                errmsg += 'Choose pacikng menthod</br>';
            else if ($("input:radio[id*='rdo_PackingMethod_']:checked").val() == "Others") {
                if ($('#txt_PackingOthers').val().length == 0)
                    errmsg += 'Enter Other packing method</br>';
            }
            if ($("input:radio[id*='rdo_DeliveryTerms_']:checked").length == 0)
                errmsg += 'Choose delivery menthod</br>';
            if ($("input:radio[id*='rdo_DeliveryMethod_']:checked").length == 0)
                errmsg += 'Choose delivery type</br>';
            if (errmsg.length > 0) {
                $('#lblErrMsg').html(errmsg);
                return false;
            }
            else
                return true;
        }
        function fnPageReload() { window.location.href = window.location.href; }
    </script>
</asp:Content>
