<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="StockReturnAF.aspx.cs" Inherits="TTS.StockReturnAF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
    <link href="Styles/datemonthyear.css" rel="stylesheet" type="text/css" />
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
        <table cellspacing="0" rules="all" border="1" style="background-color: #E2F5E1; width: 100%;
            border-color: #E2F5E1; border-collapse: collapse;">
            <tr>
                <td>
                    <asp:GridView ID="gv_Returnitems" runat="server" AutoGenerateColumns="false" Width="100%"
                        ClientIDMode="Static">
                        <HeaderStyle BackColor="#E2BA4E" Font-Bold="true" Height="22px" HorizontalAlign="Center" />
                        <Columns>
                            <asp:BoundField DataField="custfullname" HeaderText="CUSTOMER" />
                            <asp:BoundField DataField="invoiceno" HeaderText="INVOICE" />
                            <asp:BoundField DataField="OrderRefNo" HeaderText="ORDER REF NO" />
                            <asp:BoundField DataField="ReturnQty" HeaderText="RETURN QTY" />
                            <asp:BoundField DataField="DcNo" HeaderText="DC NO" />
                            <asp:BoundField DataField="DateOfReceipt" HeaderText="DATE OF RECEIPT" />
                            <asp:TemplateField HeaderText="CREDIT NOTE">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCreditNote" Text='<%# Eval("CreditNote") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ACTION" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Button ID="btnCredit" runat="server" Text="SHOW" ClientIDMode="Static" CssClass="btn btn-info"
                                        OnClick="btnCredit_Click" />
                                    <asp:HiddenField runat="server" ID="hdnReturnID" Value='<%# Eval("ReturnID") %>' />
                                    <asp:HiddenField runat="server" ID="hdncustcode" Value='<%# Eval("custcode") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr id="displaytable" style="width: 100%; display: none;">
                <td>
                    <table cellspacing="0" rules="all" border="1" style="background-color: #E2F5E1; width: 100%;
                        border-color: #E2F5E1; border-collapse: collapse;">
                        <tr style="background-color: #fdbee9;">
                            <td colspan="2">
                                <asp:Label runat="server" ID="lblCustName" ClientIDMode="Static" Text="" Font-Bold="true"
                                    ForeColor="Black" Font-Size="17px"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:Label runat="server" ID="lblStausOrderRefNo" ClientIDMode="Static" Text="" Font-Bold="true"
                                    Font-Size="17px" ForeColor="Black"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblRtnReason" ClientIDMode="Static" Text="" Font-Bold="true"
                                    ForeColor="Black"></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblRtnRemarks" ClientIDMode="Static" Text="" Font-Bold="true"
                                    ForeColor="Black"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:GridView ID="GV_QC" runat="server" AutoGenerateColumns="false" Width="100%"
                                    RowStyle-Height="22px">
                                    <HeaderStyle BackColor="#875200" ForeColor="White" Font-Bold="true" Height="25px"
                                        HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:BoundField DataField="ProcessID" HeaderText="PROCESSID" />
                                        <asp:BoundField DataField="Stencilno" HeaderText="STENCILNO" />
                                        <asp:BoundField DataField="Grade" HeaderText="SALES GRADE" />
                                        <asp:BoundField DataField="QCGrade" HeaderText="INSPECTION GRADE" />
                                        <asp:BoundField DataField="QCReason" HeaderText="INSPECTION OPINION" />
                                        <asp:BoundField DataField="QCConditionOfTheTyre" HeaderText="CONDITION OF THE TYRE" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                CREDIT NOTE NO
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtCreditnote" ClientIDMode="Static" Width="285px"
                                    CssClass="form-control" Height="20px" MaxLength="100">
                                </asp:TextBox>
                            </td>
                            <th>
                                CREDIT DATE
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtCreditdate" ClientIDMode="Static" Width="285px"
                                    CssClass="form-control" Height="20px" MaxLength="100">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                AMOUNT
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtAmount" ClientIDMode="Static" Width="285px" CssClass="form-control"
                                    Height="20px" MaxLength="10" onkeypress="javascript:return isNumber(event, this);">
                                </asp:TextBox>
                            </td>
                            <td>
                                <asp:Button ID="btnAFSave" runat="server" Text="SAVE" ClientIDMode="Static" CssClass="btn btn-success"
                                    OnClientClick="javascript:return cntrlSave();" OnClick="btnAFSave_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnAFCancel" runat="server" Text="CANCEL" ClientIDMode="Static" CssClass="btn btn-warning"
                                    OnClick="btnAFCancel_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="displayLabel" style="width: 100%; display: none;">
                <td>
                    <table cellspacing="0" rules="all" border="1" style="background-color: #E2F5E1; width: 100%;
                        border-color: #E2F5E1; border-collapse: collapse;">
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblCustName1" ClientIDMode="Static" Text="" Font-Bold="true"
                                    ForeColor="Black" Font-Size="17px" Style="text-align: left;"></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblStausOrderRefNo1" ClientIDMode="Static" Text=""
                                    Font-Bold="true" Font-Size="17px" ForeColor="Black" Style="text-align: right;"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:TextBox runat="server" ID="txtcomments" ClientIDMode="Static" Text="" Enabled="true"
                                    TextMode="MultiLine" Width="900px" Height="60px" onKeyUp="javascript:CheckMaxLength(this, 999);"
                                    onChange="javascript:CheckMaxLength(this, 999);"></asp:TextBox>
                                <asp:LinkButton runat="server" ID="lnkPdfLink" ClientIDMode="Static" Text="PDF" OnClick="lnkPdfLink_click"
                                    OnClientClick="aspnetForm.target ='_blank';"></asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnClose" runat="server" Text="CLOSE" ClientIDMode="Static" CssClass="btn btn-success"
                                    OnClick="btnClose_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnFGClear" runat="server" Text="CANCEL" ClientIDMode="Static" CssClass="btn btn-warning"
                                    OnClick="btnAFCancel_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnReturnID" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdn_custcode" ClientIDMode="Static" Value="" />
    <script src="Scripts/datemonthyear.js" type="text/javascript"></script>
    <script type="text/javascript">
        $("#txtCreditdate").datepicker({ minDate: "-90D", maxDate: "+0D" }).keydown(function (e) { e.keyWhich = 0; e.keyCode = 0; e.preventDefault(); });
        function isNumber(evt, element) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if ((charCode != 45 || $(element).val().indexOf('-') != -1) && (charCode != 46 || $(element).val().indexOf('.') != -1) && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
        function showdiv(divID) {
            $('#' + divID).css({ 'display': 'block' });
        }

        function cntrlSave() {
            var ErrMsg = "";
            if ($('#txtCreditnote').val() == "")
                ErrMsg += "Enter Credit Note \n";
            if ($('#txtCreditdate').val() == "")
                ErrMsg += "Select Credit Date \n";
            if ($('#txtAmount').val() == "")
                ErrMsg += "Enter Amount \n";
            if (ErrMsg.length > 0) {
                alert(ErrMsg);
                return false;
            } else
                return true;
        }
                     
    </script>
</asp:Content>
