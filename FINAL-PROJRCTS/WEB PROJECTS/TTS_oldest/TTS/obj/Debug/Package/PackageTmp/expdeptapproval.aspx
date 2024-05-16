<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="expdeptapproval.aspx.cs" Inherits="TTS.expdeptapproval" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        <asp:Label runat="server" ID="lblPageHead" ClientIDMode="Static" Text=""></asp:Label>
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="background-color: #dcecfb; width: 100%;
            border-color: White; border-collapse: separate;" id="tbWaitingOrder">
            <tr>
                <td style="text-align: center;">
                    <span>PLANT : </span>
                    <asp:DropDownList ID="ddlplant" runat="server" ClientIDMode="Static" AutoPostBack="true"
                        Width="120px" OnSelectedIndexChanged="ddlplant_SelectedIndexChanged">
                        <asp:ListItem Text="MMN" Value="MMN" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="PDK" Value="PDK"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="gv_WaitingOrders" Width="100%" runat="server" AutoGenerateColumns="false"
                        RowStyle-Height="30px">
                        <HeaderStyle BackColor="#74bbf9" ForeColor="White" Font-Bold="true" Height="25px"
                            HorizontalAlign="Center" />
                        <Columns>
                            <asp:TemplateField HeaderText="CUSTOMER NAME" ItemStyle-Width="250px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatusCustName" Text='<%# Eval("custfullname") %>'></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdnStatusCustCode" Value='<%# Eval("CustCode") %>' />
                                    <asp:HiddenField runat="server" ID="hdnContainerLoadFrom" Value='<%# Eval("ContainerLoadFrom") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ORDER REFERENCE NO" ItemStyle-Width="220px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOrderRefNo" Text='<%#Eval("OrderRefNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ORDER DATE" ItemStyle-Width="70px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOrderDate" Text='<%#Eval("CompletedDate") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="CUSTOMER DESIRED SHIP DATE" DataField="DesiredShipDate"
                                ItemStyle-Width="70px" />
                            <asp:BoundField HeaderText="ORDER QTY" DataField="itemqty" ItemStyle-Width="40px" />
                            <asp:TemplateField HeaderText="SHIPMENT TYPE" ItemStyle-Width="45px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblShipmentType" Text='<%# Eval("ShipmentType") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="STATUS" ItemStyle-Width="130px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatusText" Text='<%# Eval("StatusText") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="140px" HeaderText="ACTION">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkOrderSelection" CssClass="btn btn-success" runat="server"
                                        OnClick="lnkOrderSelection_click" Text="Process Order">
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="div_OrderItems" style="display: none;">
                        <table cellspacing="0" rules="all" border="1" style="background-color: #dcecfb; width: 100%;
                            border-color: White; border-collapse: separate;">
                            <tr style="text-align: center; background-color: #3c763d; overflow: hidden; font-size: 15px;
                                color: #ffffff; width: 100%;">
                                <td>
                                    <asp:Label ID="lblSelectedCustomerName" runat="server" Text="" Font-Bold="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblSelectedOrderRefNo" ClientIDMode="Static" runat="server" Text=""
                                        Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:GridView ID="gv_OrderItems" Width="100%" runat="server" AutoGenerateColumns="false"
                                        RowStyle-Height="22px" ShowFooter="true" FooterStyle-Font-Bold="true" HeaderStyle-BackColor="#0688f9"
                                        FooterStyle-HorizontalAlign="Right" FooterStyle-BackColor="#dfe0f3">
                                        <HeaderStyle BackColor="#74bbf9" ForeColor="White" Font-Bold="true" Height="25px"
                                            HorizontalAlign="Center" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="CATEGORY" ItemStyle-Width="100px">
                                                <ItemTemplate>
                                                    <%# Eval("category") %>
                                                    <asp:Label runat="server" ID="lblAssyStatus" Text='<%# Eval("AssyRimstatus").ToString() == "True" ? " (ASSY)" : ""%>'></asp:Label>
                                                    <asp:HiddenField ID="hdnProcessID" runat="server" Value='<%# Eval("processid") %>' />
                                                    <asp:HiddenField ID="hdnAssyRimstatus" runat="server" Value='<%# Eval("AssyRimstatus") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="PLATFORM" DataField="config" ItemStyle-Width="100px" />
                                            <asp:BoundField HeaderText="TYRE SIZE" DataField="tyresize" ItemStyle-Width="150px" />
                                            <asp:BoundField HeaderText="RIM" DataField="rimsize" ItemStyle-Width="40px" />
                                            <asp:BoundField HeaderText="TYPE" DataField="tyretype" ItemStyle-Width="60px" />
                                            <asp:BoundField HeaderText="BRAND" DataField="brand" ItemStyle-Width="100px" />
                                            <asp:BoundField HeaderText="SIDEWALL" DataField="sidewall" ItemStyle-Width="100px" />
                                            <asp:BoundField HeaderText="QTY" DataField="itemqty" ItemStyle-HorizontalAlign="Right"
                                                ItemStyle-Width="40px" />
                                            <asp:BoundField HeaderText="FinishedWeight" DataField="finishedwt" ItemStyle-Width="100px" />
                                            <asp:BoundField HeaderText="TotalWeight" DataField="TotalfinishedWT" ItemStyle-HorizontalAlign="Right"
                                                ItemStyle-Width="40px" />
                                            <asp:TemplateField HeaderText="RIM QTY" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="40px">
                                                <ItemTemplate>
                                                    <%# Eval("Rimitemqty").ToString() == "0" ? "" : Eval("Rimitemqty").ToString()%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"
                        Font-Bold="true"></asp:Label>
                </td>
            </tr>
        </table>
        <table runat="server" visible="false" cellspacing="0" rules="all" border="1" style="background-color: #dcecfb;
            width: 100%; border-color: White; border-collapse: separate;" id="tbSelection">
            <tr>
                <td style="text-align: left;">
                    <asp:Label runat="server" ID="lblDepartment" ClientIDMode="Static" Text="Department"></asp:Label>
                </td>
                <td style="text-align: left;">
                    <asp:RadioButtonList runat="server" ID="rdbSelection" ClientIDMode="Static" RepeatColumns="5"
                        Width="230px">
                        <asp:ListItem Text="REJECT" Value="0"></asp:ListItem>
                        <asp:ListItem Text="ACCEPT" Value="1"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblComments" ClientIDMode="Static" Text="Comments"></asp:Label>
                    <asp:TextBox runat="server" ID="txtComments" ClientIDMode="Static" Width="350px"
                        MaxLength="50" CssClass="txtCss"></asp:TextBox>
                </td>
                <td style="text-align: right; padding-right: 20px;">
                    <asp:Button ID="btnSaveDeptApproval" runat="server" Text="SAVE & ADD ITEMS" ClientIDMode="Static"
                        OnClick="btnSaveDeptApproval_Click" CssClass="Clrbutton" />
                </td>
            </tr>
        </table>
        <table runat="server" visible="false" cellspacing="0" rules="all" border="1" style="background-color: #dcecfb;
            width: 100%; border-color: White; border-collapse: separate;" id="tbApproval">
            <tr>
                <td colspan="4">
                    <span class="headCss" style="width: 400px; float: left; line-height: 25px;">DEPARTMENT
                    </span>
                </td>
            </tr>
            <tr>
                <td style="text-align: left;">
                    <asp:Label runat="server" ID="lblCrm" ClientIDMode="Static" Text="CRM"></asp:Label>
                </td>
                <td style="text-align: left;">
                    <asp:RadioButtonList runat="server" ID="rdbCrm" ClientIDMode="Static" RepeatColumns="5"
                        Width="230px" Enabled="false">
                        <asp:ListItem Text="REJECT" Value="0"></asp:ListItem>
                        <asp:ListItem Text="ACCEPT" Value="1"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtCrmComments" ClientIDMode="Static" Width="350px"
                        Enabled="false" MaxLength="50" CssClass="txtCss"></asp:TextBox>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblCrmApprovalBy" ClientIDMode="Static" Text="ApprovalBy"></asp:Label>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblCrmApprovalOn" ClientIDMode="Static" Text="ApprovedON"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: left;">
                    <asp:Label runat="server" ID="lblPpl" ClientIDMode="Static" Text="PPL"></asp:Label>
                </td>
                <td style="text-align: left;">
                    <asp:RadioButtonList runat="server" ID="rdbPpl" ClientIDMode="Static" RepeatColumns="5"
                        Width="230px" Enabled="false">
                        <asp:ListItem Text="REJECT" Value="0"></asp:ListItem>
                        <asp:ListItem Text="ACCEPT" Value="1"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtPplComments" ClientIDMode="Static" Width="350px"
                        Enabled="false" MaxLength="50" CssClass="txtCss"></asp:TextBox>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblPplApprovalBy" ClientIDMode="Static" Text="ApprovalBy"></asp:Label>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblPplApprovalOn" ClientIDMode="Static" Text="ApprovedON"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: left;">
                    <asp:Label runat="server" ID="lblPdi" ClientIDMode="Static" Text="PDI"></asp:Label>
                </td>
                <td>
                    <asp:RadioButtonList runat="server" ID="rdbPdi" ClientIDMode="Static" RepeatColumns="5"
                        Width="230px" Enabled="false">
                        <asp:ListItem Text="REJECT" Value="0"></asp:ListItem>
                        <asp:ListItem Text="ACCEPT" Value="1"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtPdiComments" ClientIDMode="Static" Width="350px"
                        Enabled="false" MaxLength="50" CssClass="txtCss"></asp:TextBox>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblPdiApprovalBy" ClientIDMode="Static" Text="ApprovalBy"></asp:Label>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblPdiApprovalOn" ClientIDMode="Static" Text="ApprovedON"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: left;">
                    <asp:Label runat="server" ID="lblLog" ClientIDMode="Static" Text="LOGISTIC"></asp:Label>
                </td>
                <td style="text-align: left;">
                    <asp:RadioButtonList runat="server" ID="rdbLog" ClientIDMode="Static" RepeatColumns="5"
                        Width="230px" Enabled="false">
                        <asp:ListItem Text="REJECT" Value="0"></asp:ListItem>
                        <asp:ListItem Text="ACCEPT" Value="1"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtLogComments" ClientIDMode="Static" Width="350px"
                        Enabled="false" MaxLength="50" CssClass="txtCss"></asp:TextBox>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblLogApprovalBy" ClientIDMode="Static" Text="ApprovalBy"></asp:Label>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblLogApprovalOn" ClientIDMode="Static" Text="ApprovedON"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: left;">
                    <asp:Label runat="server" ID="lblAF" ClientIDMode="Static" Text="A&F"></asp:Label>
                </td>
                <td style="text-align: left;">
                    <asp:RadioButtonList runat="server" ID="rdbAF" ClientIDMode="Static" RepeatColumns="5"
                        Width="230px" Enabled="false">
                        <asp:ListItem Text="REJECT" Value="0"></asp:ListItem>
                        <asp:ListItem Text="ACCEPT" Value="1"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtAFComments" ClientIDMode="Static" Width="350px"
                        Enabled="false" MaxLength="50" CssClass="txtCss"></asp:TextBox>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblAFApprovalBy" ClientIDMode="Static" Text="ApprovalBy"></asp:Label>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblAFApprovalOn" ClientIDMode="Static" Text="ApprovedON"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: left;">
                    <asp:Label runat="server" ID="lblPurchase" ClientIDMode="Static" Text="Purchase"></asp:Label>
                </td>
                <td style="text-align: left;">
                    <asp:RadioButtonList runat="server" ID="rdbPurchase" ClientIDMode="Static" RepeatColumns="5"
                        Width="230px" Enabled="false">
                        <asp:ListItem Text="REJECT" Value="0"></asp:ListItem>
                        <asp:ListItem Text="ACCEPT" Value="1"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtPurchaseComments" ClientIDMode="Static" Width="350px"
                        Enabled="false" MaxLength="50" CssClass="txtCss"></asp:TextBox>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblPurchaseApprovalBy" ClientIDMode="Static" Text="ApprovalBy"></asp:Label>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblPurchaseApprovalOn" ClientIDMode="Static" Text="ApprovedON"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnCustCode" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnPartType" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnPartMethod" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnAssyStatus" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        function gotoPreviewDiv(ctrlID) {
            $('#' + ctrlID).css({ 'display': 'block' });
            $("html, body").stop().animate({ scrollLeft: $('#' + ctrlID).offset().left, scrollTop: $('#' + ctrlID).offset().top }, 1200);
        }

        function CtrlFilter() {
            var ErrMsg = ''; $('#lblErrMsg').html('');
            if ($("input:checkbox[id*=chkManufactureYear_]:checked").length == 0)
                ErrMsg += 'Choose atleast one manufacture year<br/>';
            if ($("input:checkbox[id*=chkManufactureGrade_]:checked").length == 0)
                ErrMsg += 'Choose atleast one qulaity grade<br/>';
            if (ErrMsg.length > 0) {
                $('#lblErrMsg').html(ErrMsg)
                return false;
            }
            else
                return true;
        }

        $(function () {
            blinkText();
            $("[id*=gvStockSelection_chk_selectQty_]").on('change', function () {
                if ($("[id*=gvStockSelection_chk_selectQty_]:checked").length < $('#lblRequiredQty').html())
                    $("[id*=gvStockSelection_chk_selectQty_]:not(:checked)").prop('disabled', false);
                else
                    $("[id*=gvStockSelection_chk_selectQty_]:not(:checked)").prop('disabled', true);
            });
            $('#btnClear').click(function () { $('input:checkbox').prop('checked', false); return false; });
            $('#checkAllChk').click(function () {
                if ($("[id*=gvStockSelection_chk_selectQty_]").length > 0) {
                    if ($(this).attr('checked') == "checked")
                        $("[id*=gvStockSelection_chk_selectQty_]").attr('checked', true)
                    else
                        $("[id*=gvStockSelection_chk_selectQty_]").attr('checked', false)
                }
                else {
                    alert('No records');
                    $(this).attr('checked', false);
                }
            });
            $("[id*=gvStockSelection_chk_selectQty_]").click(function () { $('#checkAllChk').attr('checked', false); });
        });

        $(function () {
            $('#btnSaveDeptApproval').click(function (e) {
                debugger;
                var isValid = true;
                $('input[type="text"]').each(function () {
                    if (($.trim($(this).val()) == '') && ($(this).prop("disabled") == false)) {
                        isValid = false;
                        $(this).css({
                            "border": "1px solid red",
                            "background": "#FFCECE"
                        });
                    }
                    else {
                        $(this).css({
                            "border": "",
                            "background": ""
                        });
                    }
                });
                if (isValid == false)
                    alert('Type Comment');
                else
                    return true;
            });
        });
        function ctrlBtn() {
            if ($("[id*=gvStockSelection_chk_selectQty_]:checked").length != 0)
                return true;
            else {
                alert("Choose atleast one quantity");
                return false;
            }
        }
        function setblinkText() { $('#lblYearMsg').css({ 'background-color': '#660053', 'color': '#fff' }); setTimeout("blinkText()", 3000) }
        function blinkText() { $('#lblYearMsg').css({ 'background-color': '#dadada', 'color': '#000' }); setTimeout("setblinkText()", 3000) }

        function ShowUpgardeLevel() {
            $('#divUpgradePopup').css({ 'display': 'block' });
            $('#tbEarmarkPage').css({ 'opacity': '0.1' });
        }
        function CloseUpgrade() {
            $('#divUpgradePopup').css({ 'display': 'none' });
            $('#tbEarmarkPage').css({ 'opacity': '1' });
        }
        function ChkBox_hide() {
            $("[id*=chkUpgradeList_]").css({ 'display': 'none' });
            $("[id*=chkUpgradeList_]").attr({ 'disabled': true });
        }
    </script>
</asp:Content>
