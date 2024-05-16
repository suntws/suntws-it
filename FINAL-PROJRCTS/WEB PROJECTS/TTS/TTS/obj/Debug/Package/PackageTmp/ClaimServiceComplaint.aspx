<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="ClaimServiceComplaint.aspx.cs" Inherits="TTS.ClaimServiceComplaint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div align="center" class="pageTitleHead">
        <asp:Label ID="lblheader" runat="server" Text=""></asp:Label></div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <div>
            <table style="width: 1070px;">
                <tr>
                    <td>
                        <div id="divregister" style="display: none; line-height: 25px;">
                            <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #CE8686;
                                width: 1068px;">
                                <tr>
                                    <td class="headCss">
                                        CUSTOMER NAME
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlCustName" runat="server" ClientIDMode="Static" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlCustName_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="headCss">
                                        USER ID
                                    </td>
                                    <td style="width: 200px;">
                                        <asp:DropDownList ID="ddlUserId" runat="server" ClientIDMode="Static" Width="250px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="headCss">
                                        ORDER REF NO / INVOICE NO
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRefNo" runat="server" ClientIDMode="Static" Width="750px" Text="" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="headCss">
                                        COMMENTS
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClaimComments" runat="server" TextMode="MultiLine" ClientIDMode="Static"
                                            Width="850px" Height="80px" onKeyUp="javascript:CheckMaxLength(this, 999);" onChange="javascript:CheckMaxLength(this, 999);"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div style="width: 1070px; float: left;">
                            <asp:GridView runat="server" ID="gvclosed" AutoGenerateColumns="false" Width="1068px"
                                RowStyle-Height="22px">
                                <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" Height="22px" />
                                <Columns>
                                    <asp:BoundField DataField="custfullname" HeaderText="CUSTOMER NAME" ItemStyle-Width="200px" />
                                    <asp:BoundField DataField="ComplaintNo" HeaderText="COMPLAINT NO." ItemStyle-Width="50px" />
                                    <asp:BoundField DataField="CreateDate" HeaderText="COMPLAINT DATE" ItemStyle-Width="50px" />
                                    <asp:TemplateField HeaderText="COMPLAINT COMMENTS" ItemStyle-Width="200px">
                                        <ItemTemplate>
                                            <%#Eval("ClaimCommets") != DBNull.Value ? ((string)Eval("ClaimCommets")).Replace("~", "<br/>") : ""%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="CREDIT NOTE NO." ItemStyle-Width="80px">
                                        <ItemTemplate>
                                            EXP/SCN-<%# Eval("CreditNoteNo") %>/<%# Eval("CreditNoteYear") %><br />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="InvoiceNo" HeaderText="INVOICE NO." ItemStyle-Width="80px" />
                                    <asp:TemplateField HeaderText="CLOSED COMMENTS" ItemStyle-Width="200px">
                                        <ItemTemplate>
                                            <%#Eval("SettlementComments") != DBNull.Value ? ((string)Eval("SettlementComments")).Replace("~", "<br/>") : ""%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="closeddate" HeaderText="CLOSED DATE" ItemStyle-Width="60px" />
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div style="width: 1070px; float: left;">
                            <asp:GridView runat="server" ID="gvClaimServiceList" AutoGenerateColumns="false"
                                Width="1068px" RowStyle-Height="22px">
                                <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" Height="22px" />
                                <Columns>
                                    <asp:BoundField DataField="custfullname" HeaderText="CUSTOMER NAME" ItemStyle-Width="200px" />
                                    <asp:BoundField DataField="ComplaintNo" HeaderText="COMPLAINT NO." ItemStyle-Width="50px" />
                                    <asp:BoundField DataField="CreateDate" HeaderText="COMPLAINT DATE" ItemStyle-Width="50px" />
                                    <asp:TemplateField HeaderText="COMPLAINT COMMENTS" ItemStyle-Width="300px">
                                        <ItemTemplate>
                                            <%#Eval("ClaimCommets") != DBNull.Value ? ((string)Eval("ClaimCommets")).Replace("~", "<br/>") : ""%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="RefNo" HeaderText="REF NO." ItemStyle-Width="120px" />
                                    <asp:BoundField DataField="claimstatus" HeaderText="STATUS" ItemStyle-Width="150px" />
                                    <asp:TemplateField HeaderText="ACTION" ItemStyle-Width="50px">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkshow" runat="server" Text="Show List" OnClick="lnkshow_Click" /></span>
                                            <asp:HiddenField runat="server" ID="hdnClaimCustCode" Value='<%# Eval("CustCode") %>' />
                                            <asp:HiddenField runat="server" ID="hdnCreditNoteYear" ClientIDMode="Static" Value='<%# Eval("CreditNoteYear") %>' />
                                            <asp:HiddenField runat="server" ID="hdnCreditNoteNo" ClientIDMode="Static" Value='<%# Eval("CreditNoteNo") %>' />
                                            <asp:HiddenField runat="server" ID="hdnCreditNoteFile" ClientIDMode="Static" Value='<%# Eval("CreditNoteFile") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div id="divMaindisplay" style="width: 1068px; float: left; margin-top: 5px; display: none;">
                            <div style="width: 1066px; float: left; border: 1px solid #000; background-color: #056442;
                                font-weight: bold; color: #fff; font-size: 15px; margin-bottom: 5px;">
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
                            <div style="width: 1066px; float: left; border: 1px solid #ccc; margin-bottom: 5px;">
                                <asp:Label runat="server" ID="lblHistoryHead" ClientIDMode="Static" Text="" Font-Bold="true"
                                    Font-Size="15px"></asp:Label>
                                <asp:GridView runat="server" ID="gvhistory" AutoGenerateColumns="false" Width="1066px">
                                    <HeaderStyle BackColor="#EFEFED" Font-Bold="true" Height="22px" />
                                    <Columns>
                                        <asp:BoundField DataField="ReviseType" HeaderText="TYPE" ItemStyle-Width="200px" />
                                        <asp:TemplateField HeaderText="COMMENTS" ItemStyle-Width="500px">
                                            <ItemTemplate>
                                                <%#Eval("Comments") != DBNull.Value ? ((string)Eval("Comments")).Replace("~", "<br/>") : ""%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CreateDate" HeaderText="DATE" ItemStyle-Width="60px" />
                                        <asp:BoundField DataField="UserName" HeaderText="CHANGED BY" ItemStyle-Width="100px" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div style="width: 1066px; float: left; border: 1px solid #000;">
                                <div style="width: 1068px; float: left; margin-bottom: 5px;">
                                    <asp:Label runat="server" ClientIDMode="Static" ID="lblcreditNO" Text=""></asp:Label>
                                    <asp:LinkButton runat="server" ID="lnkCrditNoteDownload" ClientIDMode="Static" Text=""
                                        OnClick="lnkCrditNoteDownload_Click"></asp:LinkButton>
                                </div>
                                <div id="divsubcrpre" style="width: 666px; float: left; margin-bottom: 5px; display: none;
                                    padding-left: 200px; padding-right: 200px;">
                                    <asp:GridView runat="server" ID="gvClaimOtherCharges" AutoGenerateColumns="false"
                                        Width="600px">
                                        <HeaderStyle BackColor="#CDECE6" Font-Bold="true" Height="18px" />
                                        <Columns>
                                            <asp:BoundField DataField="slno" Visible="false" />
                                            <asp:TemplateField ItemStyle-Width="350px" HeaderText="SERVICE COMPLAINT DESCRIPTION"
                                                HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtClaimAddDesc" Text='<%# Eval("DescrDiscount") %>'
                                                        Width="350px" MaxLength="100"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Width="35px" HeaderText="+/-">
                                                <ItemTemplate>
                                                    <asp:DropDownList runat="server" ID="ddlClaimCalcType" Width="35px">
                                                        <asp:ListItem Text="+" Value="ADD"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Width="70px" HeaderText="AMOUNT">
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtClaimAddAmt" onkeypress="return isNumberKey(event)"
                                                        Text='<%# Eval("Amount").ToString()=="0"?"": Eval("Amount").ToString() %>' Width="70px"
                                                        MaxLength="8"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <div style="width: 1066px; float: left; margin-bottom: 5px; display: none;" id="divBtnSettlement">
                                    <span class="headCss">INVOICE NO.</span>
                                    <asp:TextBox runat="server" ID="txtSettleInvoiceNo" ClientIDMode="Static" Text=""></asp:TextBox>
                                </div>
                                <div id="subcrmopi" style="width: 1066px; float: left; margin-bottom: 5px; display: none;">
                                    <asp:Label runat="server" ID="lblCommentsHead" ClientIDMode="Static" Text="" CssClass="headCss"></asp:Label>
                                    <asp:TextBox runat="server" ID="txtCrmComments" ClientIDMode="Static" Text="" TextMode="MultiLine"
                                        Width="1056px" Height="80px" onKeyUp="javascript:CheckMaxLength(this, 999);"
                                        onChange="javascript:CheckMaxLength(this, 999);"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div style="width: 1058px; float: left; text-align: center; display: none; border: 1px solid #000;
                            padding: 4px;" id="divbtn">
                            <asp:Label runat="server" ID="lblerrmsg" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label>
                            <div id="divcrmappsub" style="width: 520px; float: left; display: none;">
                                <asp:Button runat="server" ClientIDMode="Static" ID="btnrecheck" Text="RECHECK" CssClass="btnedit"
                                    OnClick="btnrecheck_Click" OnClientClick="javascript:return recheck();" />
                            </div>
                            <div style="width: 530px; float: right;">
                                <asp:Button runat="server" ID="btnSAVE" ClientIDMode="Static" Text="" OnClientClick="javascript:return ClaimRegister();"
                                    CssClass="btnshow " OnClick="btnSAVE_Click" />
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdnCustCode" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnCreditNote" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnCreditCurrency" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnstrcredityear" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnstatus" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnSelectIndex" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnstrStatus" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnFullName" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnLoginName" ClientIDMode="Static" Value="" />
    <script src="Scripts/cotsScript.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#ddlCustName').change(function () {
                $('#hdnFullName').val($('#ddlCustName option:selected').text());
            });
            $('#ddlUserId').change(function () {
                $('#hdnLoginName').val($('#ddlUserId option:selected').text());
            });
        });

        function recheck() {
            $('#lblerrmsg').html(''); var errMsg = '';
            if ($('#txtCrmComments').val().length == 0)
                errMsg += 'Enter  comments<br />';
            if (errMsg.length > 0) {
                $('#lblerrmsg').html(errMsg);
                return false;
            }
            else
                return true;
        }
        function ClaimRegister() {
            $('#lblerrmsg').html(''); var errMsg = '';
            if ($('#hdnstatus').val() == "1") {
                if ($('#ddlCustName option:selected').text() == 'Choose')
                    errMsg += 'Choose customer name<br />';
                if ($('#ddlUserId option:selected').text() == '')
                    errMsg += 'Choose user id<br />';
                if ($('#txtClaimComments').val().length == 0)
                    errMsg += 'Enter  comments<br />';
            }
            else {
                if ($('#hdnstatus').val() == "3") {
                    if ($('#hdnstrStatus').val() != "WAITING FOR CREDIT NOTE MOVE TO CRM") {
                        var count = 0;
                        $("input:text[id*=MainContent_gvClaimOtherCharges_txtClaimAddDesc_]").each(function () {
                            var id1 = this.id; var amtId = id1.replace('txtClaimAddDesc_', 'txtClaimAddAmt_');
                            if ($('#' + id1).val() != '' && $('#' + amtId).val() != '')
                                count++;
                            if ($('#' + id1).val() != '' && $('#' + amtId).val() == '')
                                errMsg += 'Enter extra charges amount<br/>';
                            if ($('#' + id1).val() == '' && $('#' + amtId).val() != '')
                                errMsg += 'Enter extra charges description<br/>';
                        });
                        if (count == 0)
                            errMsg += 'Enter complaint description & amount <br/>';
                    }
                }
                else if ($('#hdnstatus').val() == "5") {
                    if ($('#txtSettleInvoiceNo').val().length == 0)
                        errMsg += 'Enter Settlement Invoice No.<br />';
                }
                if ($('#txtCrmComments').val().length == 0)
                    errMsg += 'Enter  comments<br />';
            }
            if (errMsg.length > 0) {
                $('#lblerrmsg').html(errMsg);
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
