<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="claimcreditapprove.aspx.cs" Inherits="TTS.claimcreditapprove" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div align="center" class="pageTitleHead">
        <asp:Label runat="server" ID="lblPageHead" ClientIDMode="Static" Text=""></asp:Label>
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table style="width: 1070px;">
            <tr>
                <td align="center">
                    <asp:GridView runat="server" ID="gvComplaintList" AutoGenerateColumns="false" Width="1060px"
                        RowStyle-Height="22px" 
                        onselectedindexchanged="gvComplaintList_SelectedIndexChanged">
                        <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" Height="22px" />
                        <Columns>
                            <asp:BoundField DataField="custname" HeaderText="CUSTOMER NAME" ItemStyle-Width="200px" />
                            <asp:BoundField DataField="complaintno" HeaderText="COMPLAINT NO." ItemStyle-Width="80px" />
                            <asp:BoundField DataField="complaintdate" HeaderText="COMPLAINT DATE" ItemStyle-Width="80px" />
                            <asp:BoundField DataField="Qty" HeaderText="QTY" ItemStyle-Width="40px" />
                            <asp:BoundField DataField="ClaimType" HeaderText="CREDIT NOTE" ItemStyle-Width="120px" />
                            <asp:BoundField DataField="claimstatus" HeaderText="STATUS" ItemStyle-Width="180px" />
                            <asp:TemplateField HeaderText="ACTION" ItemStyle-Width="80px">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hdnCreditFileName" ClientIDMode="Static" Value='<%# Eval("CreditNoteFile") %>' />
                                    <asp:HiddenField runat="server" ID="hdnCreditNoteNo" ClientIDMode="Static" Value='<%# Eval("CreditNoteNo") %>' />
                                    <asp:LinkButton ID="lnkClaimNo" runat="server" Text="Show List" OnClick="lnkClaimNo_Click" /></span>
                                    <asp:HiddenField runat="server" ID="hdnClaimCustCode" Value='<%# Eval("custcode") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="width: 1060px; float: left;">
                        <div style="width: 1058px; float: left; border: 1px solid #000; background-color: #056442;
                            font-weight: bold; color: #fff; font-size: 15px;">
                            <div style="width: 525px; float: left; text-align: left; padding-right: 5px;">
                                <asp:Label runat="server" ID="lblClaimCustName" ClientIDMode="Static" Text=""></asp:Label>
                            </div>
                            <div style="width: 2px; float: left;">
                                <asp:Label runat="server" ID="lblClaim" ClientIDMode="Static" Text=""></asp:Label>
                            </div>
                            <div style="width: 520px; float: left; text-align: right; padding-left: 5px;">
                                <asp:Label runat="server" ID="lblClaimNo" ClientIDMode="Static" Text=""></asp:Label>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="width: 1060px; float: left; display: none;" id="divCrmCreditApprove">
                        <div style="width: 1060px; float: left; text-align: center; font-size: 16px; font-weight: bold;
                            background-color: #ccc; margin-top: 5px;">
                            <span>CREDIT NOTE:</span>
                            <asp:Label runat="server" ID="lblCreditNoteNo" ClientIDMode="Static" Text="" Font-Bold="true"
                                ForeColor="Green"></asp:Label>
                        </div>
                        <div style="text-align: right; color: #EC800C; font-weight: bold; display: none;"
                            id="divChangePrice">
                            DO YOU WANT CHANGE THE UNIT PRICE AND REPREPARATION CREDIT NOTE
                            <input type="checkbox" id="chkPriceChange" onclick="PriceChangeEnable()" />
                        </div>
                        <div style="width: 1060px; float: left; border: 1px solid #000; margin-bottom: 5px;
                            text-align: left;">
                            <asp:GridView runat="server" ID="gvClaimApproveItems" AutoGenerateColumns="false"
                                Width="1060px" RowStyle-Height="22px">
                                <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" Height="20px" HorizontalAlign="Center" />
                                <Columns>
                                    <asp:BoundField DataField="config" HeaderText="PLATFORM" ItemStyle-Width="100px" />
                                    <asp:BoundField DataField="CrmType" HeaderText="TYPE" ItemStyle-Width="80px" />
                                    <asp:BoundField DataField="brand" HeaderText="BRAND" ItemStyle-Width="100px" />
                                    <asp:BoundField DataField="tyresize" HeaderText="TYRE SIZE" ItemStyle-Width="150px" />
                                    <asp:BoundField DataField="stencilno" HeaderText="STENCIL" ItemStyle-Width="80px" />
                                    <asp:BoundField DataField="assigntoqc" HeaderText="PLANT" ItemStyle-Width="80px" />
                                    <asp:BoundField DataField="StencilCrmStatus" HeaderText="STATUS" ItemStyle-Width="60px" />
                                    <asp:BoundField DataField="ClaimDescription" HeaderText="COMPLAINT DESC" ItemStyle-Width="60px" />
                                    <asp:BoundField DataField="PreviewPrice" HeaderText="PREVIOUS CLAIM PRICE" ItemStyle-Width="80px" />
                                    <asp:TemplateField HeaderText="ACTUAL TYRE PRICE" ItemStyle-Width="90px">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtTyrePrice" Text='<%# Eval("Tyreprice").ToString()==""?"0":Eval("Tyreprice").ToString() %>'
                                                MaxLength="12" Width="80px" Enabled="false"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="CURRENT CLAIM PRICE" ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hdnclaimprice" runat="server" Value='<%# Eval("unitprice") %>' />
                                            <asp:TextBox runat="server" ID="txtClaimPrice" Text='<%# Eval("unitprice") %>' Enabled="false"
                                                onkeypress="return isNumberKey(event)" onblur="isCompareTyreValue(event,this)"
                                                MaxLength="12" Width="80px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div style="width: 1060px; float: left; border: 1px solid #000; margin-bottom: 5px;
                            text-align: left;">
                            <asp:Label runat="server" ID="lblCreditMovedComments" ClientIDMode="Static" Text=""></asp:Label>
                            <br />
                            <asp:Label runat="server" ID="lblCreditMovedUser" ClientIDMode="Static" Text=""></asp:Label>
                        </div>
                        <div style="width: 1060px; float: left; border: 1px solid #000; margin-bottom: 5px;">
                            <div style="width: 1060px; float: left;">
                                <span class="headCss">COMMENTS:</span>
                                <asp:TextBox runat="server" ID="txtCreditApproveComments" ClientIDMode="Static" Text=""
                                    TextMode="MultiLine" Width="1050px" Height="80px" onKeyUp="javascript:CheckMaxLength(this, 999);"
                                    onChange="javascript:CheckMaxLength(this, 999);"></asp:TextBox>
                            </div>
                            <div style="width: 530px; float: left;">
                                <span class="headCss">DOWNLOAD: </span>
                                <asp:LinkButton runat="server" ID="lnkCrditNoteDownload" ClientIDMode="Static" Text="CREDIT NOTE FILE"
                                    OnClick="lnkCrditNoteDownload_Click"></asp:LinkButton>
                            </div>
                            <div style="width: 530px; float: left;">
                                <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label></div>
                            <div style="width: 1060px; float: left; text-align: center; display: none;" id="divSavePrice">
                                <asp:Button runat="server" ID="btnSettlePrice" ClientIDMode="Static" Text="SAVE UNIT PRICE & MOVE TO ACCOUNTS FOR CREDIT NOTE REPERPARATION"
                                    OnClientClick="javascript:return CtrlSaveUnirprice();" CssClass="btnshow" OnClick="btnSettlePrice_Click" />
                                <asp:Button runat="server" ID="btnPriceChangeDisable" ClientIDMode="Static" Text="CANCEL"
                                    CssClass="btnclear" OnClick="btnPriceChangeDisable_Click" />
                            </div>
                            <div style="width: 530px; float: left; text-align: center;" id="divapproval">
                                <asp:Button runat="server" ID="btnApproveCreditNote" ClientIDMode="Static" Text="SAVE CREDIT NOTE APPROVAL"
                                    OnClientClick="javascript:return CtrlApproveCreditNote();" CssClass="btnshow"
                                    OnClick="btnApproveCreditNote_Click" />
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnCustCode" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnCreditNote" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnSelectIndex" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnConditionalStatus" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        function showClaimApprove() { $('#divCrmCreditApprove').css({ 'display': 'block' }); gotoClaimDiv('divCrmCreditApprove'); }

        function hideClaimApprove() { $('#divCrmCreditApprove').css({ 'display': 'none' }); }

        function PriceChangeEnable() {
            $('#divSavePrice').show(); $('#divapproval').css({ 'display': 'none' }); $('#chkPriceChange').attr('disabled', true);
            $('input:text[id*=MainContent_gvClaimApproveItems_txtClaimPrice_]').attr('disabled', false);
            $('#MainContent_gvClaimApproveItems tr').find('td:eq(6)').each(function (e) {
                if ($(this).html() == 'REJECT')
                    $('#MainContent_gvClaimApproveItems_txtClaimPrice_' + e).attr('disabled', true);
            });
        }
        function hideClaimAccounts() {
            $('#divClaimAccountEntry').fadeOut(); $('#divClaimCreditEntry').fadeOut();
            $('#divChangePrice').hide(); $('#divSavePrice').hide(); $('#divChangePrice').show();
        }
        function showPriceOption() { $('#divChangePrice').show(); }
        function PriceChangeDisable() { $('#divapproval').css({ 'display': 'block' }); $('#chkPriceChange').attr('disabled', false); }
        function CtrlSaveUnirprice() {
            $('#lblMsg').html(''); var errMsg = ''; var rowCount = ''; var errcount = ''
            $('#MainContent_gvClaimApproveItems tr').find('td:eq(5)').each(function (e) {
                rowCount = parseInt(e) + 1;
                if ($(this).html() != 'REJECT') {
                    if ($('#MainContent_gvClaimApproveItems_txtClaimPrice_' + e).val().length == 0) { if (errcount.length > 0) errcount += "," + rowCount; else errcount += rowCount; }
                }
            });
            if (errcount.length > 0) errMsg += 'Record ' + errcount + ' unit price value is invalid<br />';
            if ($('#txtCreditApproveComments').val().length == 0) errMsg += 'Enter credit note reprepare comments<br />';
            if (errMsg.length > 0) { $('#lblErrMsg').html(errMsg); return false; } else return true;
        }
        function CtrlApproveCreditNote() {
            $('#lblErrMsg').html(''); var errMsg = '';
            if ($('#txtCreditApproveComments').val().length == 0) errMsg += 'Enter approved comments<br />';
            if (errMsg.length > 0) { $('#lblErrMsg').html(errMsg); return false; } else return true;
        }
        function isCompareTyreValue(evt, source) {
            // check if the claim price is greater than the unit price. allow if less or equal.
            if (parseFloat($(source).val()) > parseFloat($(source).closest("td").prev().find("input").val())) {
                $(source).val($(source).closest("td").prev().find("input").val()); $(source).focus();
            }
        }
    </script>
</asp:Content>
