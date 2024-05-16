<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="expPpcEarmark.aspx.cs" Inherits="TTS.expPpcEarmark" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/cotsScript.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        <asp:Label runat="server" ID="lblPageHead" ClientIDMode="Static" Text="ASSIGN STENCIL TO CURRENT WORK ORDER"></asp:Label>
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="background-color: #dcecfb; width: 100%;
            border-color: White; border-collapse: separate;">
            <tr>
                <td colspan="3">
                    <asp:Label runat="server" ID="lblAssignOrderMsg" ClientIDMode="Static" Text="THE BELOW CUSTOMERS ALSO ORDERED THE SAME ITEMS"
                        Font-Bold="true" Font-Size="16px" BackColor="#039a15" ForeColor="#ffffff" Width="100%"
                        Height="20px"></asp:Label>
                    <asp:GridView runat="server" ID="gv_ItemRelatedOrders" AutoGenerateColumns="false"
                        Width="100%" CssClass="gridcss">
                        <Columns>
                            <asp:BoundField HeaderText="CUSTOMER" DataField="custfullname" />
                            <asp:BoundField HeaderText="WORK ORDER" DataField="workorderno" />
                            <asp:BoundField HeaderText="PLATFORM" DataField="Config" />
                            <asp:BoundField HeaderText="TYRE SIZE" DataField="tyresize" />
                            <asp:BoundField HeaderText="RIM" DataField="rimsize" />
                            <asp:BoundField HeaderText="TYPE" DataField="tyretype" />
                            <asp:BoundField HeaderText="BRAND" DataField="brand" />
                            <asp:BoundField HeaderText="SIDEWALL" DataField="sidewall" />
                            <asp:BoundField HeaderText="ORDER QTY" DataField="itemqty" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Width="60px" />
                            <asp:BoundField HeaderText="ASSIGNED QTY" DataField="EarmarkQty" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Width="60px" />
                            <asp:BoundField HeaderText="PRODUCED QTY" DataField="PartG" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Width="60px" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <%# Convert.ToBoolean(Eval("AssyRimstatus")) ? "ASSY":""%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hdnO_ItemID" Value='<%# Eval("O_ItemID") %>' />
                                    <asp:HiddenField runat="server" ID="hdnAssignOrderOID" Value='<%# Eval("ID") %>' />
                                    <asp:LinkButton ID="lnkAssignOrder" runat="server" OnClick="lnkAssignOrder_click"
                                        Text="ASSIGN"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <div style="width: 100%; float: left;" id="divRequest">
                        <span style="font-weight: bold; cursor: pointer; text-decoration: underline; color: #4348fd;"
                            onclick="showReqDiv()">REQUEST FOR FRESH PRODUCTION</span>
                    </div>
                    <div style="width: 100%; float: left; display: none;" id="divReqRemarks">
                        <div style="width: 75%; float: left;">
                            ENTER YOUR COMMENTS:
                            <asp:TextBox runat="server" ID="txtReqRemarks" ClientIDMode="Static" Text="" Width="98%"
                                CssClass="form-control"></asp:TextBox>
                        </div>
                        <div style="width: 25%; float: left; line-height: 60px; text-align: center;">
                            <asp:Button runat="server" ID="btnReqSave" ClientIDMode="Static" Text="SAVE REQUEST FOR APPROVAL"
                                OnClientClick="javascript:return ctrlSaveRequest();" OnClick="btnReqSave_Click"
                                CssClass="btn btn-info" />
                        </div>
                    </div>
                </td>
            </tr>
            <tr style="font-weight: bold; font-size: 14px;">
                <td>
                    <asp:Label ID="lblSelectedCustomerName" runat="server" Text="" Font-Bold="true"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblSelectedOrderRefNo" ClientIDMode="Static" runat="server" Text=""
                        Font-Bold="true"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblSelectedWorkOrderNo" ClientIDMode="Static" runat="server" Text=""
                        Font-Bold="true"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <table cellspacing="0" rules="all" border="1" style="background-color: #dcecfb; width: 100%;
                        border-color: White; border-collapse: separate; line-height: 22px;" id="frmEarmarkPrepareItem">
                        <tr>
                            <th colspan="2" style="text-align: center; background-color: #68e266; font-weight: bold;
                                font-size: 14px;">
                                ITEM DESCRIPTION
                            </th>
                            <th colspan="2" style="text-align: center; background-color: #b8e266; font-weight: bold;
                                font-size: 14px;">
                                PART WISE ASSIGNED QTY
                            </th>
                            <th style="text-align: center; background-color: #ec89a9; font-weight: bold; font-size: 14px;">
                                STOCK AVAILABILITY
                            </th>
                        </tr>
                        <tr>
                            <th style="width: 100px;">
                                PLATFORM
                            </th>
                            <td style="width: 200px;">
                                <asp:Label runat="server" ID="lblPlatform" Text=""></asp:Label>
                            </td>
                            <th style="width: 320px;">
                                PART-A (GSA EXACT MATCH)
                            </th>
                            <td style="width: 50px;">
                                <asp:Label runat="server" ID="lblPartA" Text=""></asp:Label>
                            </td>
                            <td style="width: 320px;">
                                <asp:LinkButton runat="server" ID="lnkPartA" ClientIDMode="Static" Text="" OnClick="lnkAvailablePart_Click"
                                    Font-Bold="false" Font-Size="12px"></asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                TYRE SIZE
                            </th>
                            <td>
                                <asp:Label runat="server" ID="lblTyresize" Text=""></asp:Label>
                            </td>
                            <th>
                                PART-B (GSA MATCH WITH REBRAND)
                            </th>
                            <td>
                                <asp:Label runat="server" ID="lblPartB" Text=""></asp:Label>
                            </td>
                            <td>
                                <asp:LinkButton runat="server" ID="lnkPartB" ClientIDMode="Static" Text="" OnClick="lnkAvailablePart_Click"
                                    Font-Bold="false" Font-Size="12px"></asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                RIM
                            </th>
                            <td>
                                <asp:Label runat="server" ID="lblRim" Text=""></asp:Label>
                            </td>
                            <th>
                                PART-C (GSA UPGRADE WITH REBRAND)
                            </th>
                            <td>
                                <asp:Label runat="server" ID="lblPartC" Text=""></asp:Label>
                            </td>
                            <td>
                                <asp:LinkButton runat="server" ID="lnkPartC" ClientIDMode="Static" Text="" OnClick="lnkAvailablePart_Click"
                                    Font-Bold="false" Font-Size="12px"></asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                TYPE
                            </th>
                            <td>
                                <asp:Label runat="server" ID="lblType" Text=""></asp:Label>
                            </td>
                            <th>
                                PART-D (CURRENT STOCK EXACT MATCH)
                            </th>
                            <td>
                                <asp:Label runat="server" ID="lblPartD" Text=""></asp:Label>
                            </td>
                            <td>
                                <asp:LinkButton runat="server" ID="lnkPartD" ClientIDMode="Static" Text="" OnClick="lnkAvailablePart_Click"
                                    Font-Bold="false" Font-Size="12px"></asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                BRAND
                            </th>
                            <td>
                                <asp:Label runat="server" ID="lblBrand" Text=""></asp:Label>
                            </td>
                            <th>
                                PART-E (CURRENT STOCK MATCH WITH REBRAND)
                            </th>
                            <td>
                                <asp:Label runat="server" ID="lblPartE" Text=""></asp:Label>
                            </td>
                            <td>
                                <asp:LinkButton runat="server" ID="lnkPartE" ClientIDMode="Static" Text="" OnClick="lnkAvailablePart_Click"
                                    Font-Bold="false" Font-Size="12px"></asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                SIDEWALL
                            </th>
                            <td>
                                <asp:Label runat="server" ID="lblSidewall" Text=""></asp:Label>
                            </td>
                            <th>
                                PART-F (CURRENT STOCK UPGRADE WITH REBRAND)
                            </th>
                            <td>
                                <asp:Label runat="server" ID="lblPartF" Text=""></asp:Label>
                            </td>
                            <td>
                                <asp:LinkButton runat="server" ID="lnkPartF" ClientIDMode="Static" Text="" OnClick="lnkAvailablePart_Click"
                                    Font-Bold="false" Font-Size="12px"></asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                PROCESS-ID
                            </th>
                            <td>
                                <asp:Label runat="server" ID="lblProcessID" Text=""></asp:Label>
                            </td>
                            <th>
                                TOTAL EARMARK QTY
                            </th>
                            <td>
                                <asp:Label runat="server" ID="lblEarmarkQty" Text="" Font-Bold="true" Font-Size="16px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblProducedQty" Text="" Font-Bold="true" Font-Size="12px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                ORDER QTY
                            </th>
                            <td>
                                <asp:Label runat="server" ID="lblOrderQty" Text="" Font-Bold="true" Font-Size="16px"></asp:Label>
                            </td>
                            <th>
                                REQUIRED QTY
                            </th>
                            <td>
                                <asp:Label runat="server" ID="lblRequiredQty" ClientIDMode="Static" Text="" ForeColor="Red"
                                    Font-Size="16px"></asp:Label>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <div id="div_manufactureyear" style="display: none;">
                        <table cellspacing="0" rules="all" border="1" style="background-color: #dcecfb; width: 100%;
                            border-color: White; border-collapse: separate;">
                            <tr>
                                <td colspan="3">
                                    <asp:Label runat="server" ID="lblYearMsg" ClientIDMode="Static" Text="" ForeColor="Green"
                                        Font-Bold="true" Font-Size="15px"></asp:Label>
                                    <asp:Label runat="server" ID="lblManuYearErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"
                                        Font-Bold="true" Font-Size="15px"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lbl_MANUFACTUREYEAR" ClientIDMode="Static" Text=""></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:CheckBoxList runat="server" ID="chkManufactureYear" ClientIDMode="Static" RepeatColumns="15"
                                        RepeatDirection="Horizontal">
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lbl_QUALITYGRADE" ClientIDMode="Static" Text=""></asp:Label>
                                </td>
                                <td>
                                    <asp:CheckBoxList runat="server" ID="chkManufactureGrade" ClientIDMode="Static" RepeatColumns="10"
                                        RepeatDirection="Horizontal" Font-Bold="true">
                                    </asp:CheckBoxList>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label>
                                    <div style="width: 160px; float: left;">
                                        <asp:Button runat="server" ID="btnFindStencil" ClientIDMode="Static" Text="" CssClass="btn btn-info"
                                            OnClick="btnFindStencil_Click" OnClientClick="Javascript:return CtrlFilter();" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Label runat="server" ID="lbl_StockErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"
                        Font-Bold="true" Font-Size="15px"></asp:Label>
                    <div id="div_availablestencil" style="display: none;">
                        <table cellspacing="0" rules="all" border="1" style="background-color: #dcecfb; width: 100%;
                            border-color: White; border-collapse: separate;">
                            <tr>
                                <td colspan="2">
                                    <asp:Label runat="server" ID="lbl_StockQty" ClientIDMode="Static" Text="" Font-Bold="true"
                                        Font-Size="15px" ForeColor="DarkGreen"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:GridView runat="server" ID="gvStockSelection" AutoGenerateColumns="false" Width="100%"
                                        CssClass="gridcss">
                                        <Columns>
                                            <asp:BoundField HeaderText="PLATFORM" DataField="config" />
                                            <asp:BoundField HeaderText="TYRE SIZE" DataField="tyresize" />
                                            <asp:BoundField HeaderText="RIM" DataField="rimsize" />
                                            <asp:BoundField HeaderText="TYPE" DataField="tyretype" />
                                            <asp:BoundField HeaderText="BRAND" DataField="brand" />
                                            <asp:BoundField HeaderText="SIDEWALL" DataField="sidewall" />
                                            <asp:BoundField HeaderText="PLANT" DataField="Plant" />
                                            <asp:BoundField HeaderText="GRADE" DataField="grade" />
                                            <asp:BoundField HeaderText="STENCIL NO" DataField="stencilno" />
                                            <asp:BoundField HeaderText="YOM" DataField="yearofmanufacture" />
                                            <asp:BoundField HeaderText="LOCATION" DataField="location" />
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:CheckBox runat="server" ID="checkAllChk" ClientIDMode="Static" />
                                                    ALL
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chk_selectQty" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    <asp:Button ID="btnClear" ClientIDMode="Static" runat="server" Text="CLEAR CHECKED"
                                        CssClass="btn btn-warning" />
                                </td>
                                <td style="text-align: center;">
                                    <asp:Button ID="btnGSA_Assign" ClientIDMode="Static" runat="server" Text="" CssClass="btn btn-success"
                                        OnClientClick="javascript:return ctrlBtn();" OnClick="btnGSA_Assign_Click" />
                                    <asp:Label runat="server" ID="lblNextMsg" ClientIDMode="Static" Text="" Font-Bold="true"
                                        Font-Size="15px"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnPartType" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnPartMethod" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnAssingOID" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnItemID" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnSelectIndex" ClientIDMode="Static" Value="0" />
    <asp:HiddenField runat="server" ID="hdnMasterOID" ClientIDMode="Static" Value="" />
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
            $("input:checkbox[id*=chkManufactureYear_]").click(function () { $('#div_availablestencil').css({ 'display': 'none' }); $('#btnFindStencil').css({ 'display': 'block' }); });
            $("input:checkbox[id*=chkManufactureGrade_]").click(function () { $('#div_availablestencil').css({ 'display': 'none' }); $('#btnFindStencil').css({ 'display': 'block' }); });
        });
        function ctrlBtn() {
            if ($("[id*=gvStockSelection_chk_selectQty_]:checked").length != 0)
                return true;
            else {
                alert("Choose atleast one quantity");
                return false;
            }
        }
        function ctrlBackColor(eve) {
            $('#MainContent_gv_ItemRelatedOrders tr').css({ 'background-color': '#ffffff' })
            $('#MainContent_gv_ItemRelatedOrders_lnkAssignOrder_' + eve).parent('td').parent('tr').css({ 'background-color': '#ffff00' });
            gotoPreviewDiv('MainContent_gv_ItemRelatedOrders_lnkAssignOrder_' + eve);
        }
        function ctrlSaveRequest() {
            if ($('#txtReqRemarks').val().length == 0) {
                alert('Enter the comments for requesting to fresh production');
                return false;
            }
            else
                return true;
        }
        function showReqDiv() {
            $('#divRequest').css({ 'display': 'none' });
            $('#divReqRemarks').css({ 'display': 'block' });
        }
    </script>
</asp:Content>
