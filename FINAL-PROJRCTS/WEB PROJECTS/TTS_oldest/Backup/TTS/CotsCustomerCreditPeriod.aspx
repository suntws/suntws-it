<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CotsCustomerCreditPeriod.aspx.cs"
    MasterPageFile="~/master.Master" Inherits="TTS.CotsCustomerCreditPeriod" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .button
        {
            background-color: #4CAF50;
            color: white;
            text-align: center;
            display: inline-block;
            font-size: 12px;
            cursor: pointer;
            height: 25px;
            width: 50px;
            border: none;
        }
        input:focus
        {
            background-color: Yellow;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        <asp:Label runat="server" ID="lblPageHead" ClientIDMode="Static" Text=""></asp:Label>
        CUSTOMER CREDIT PERIOD</div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <asp:GridView ID="gvCustPeriodDetails" Width="100%" runat="server" AutoGenerateColumns="false"
            HeaderStyle-BackColor="#7faed6" HeaderStyle-Font-Bold="true" HeaderStyle-Font-Size="12px"
            OnRowEditing="gvCustPeriodDetails_RowEditing" OnRowCancelingEdit="gvCustPeriodDetails_RowCancelingEdit"
            OnRowUpdating="gvCustPeriodDetails_RowUpdating" CssClass="gridcss">
            <Columns>
                <asp:TemplateField HeaderText="CUSTOMER NAME">
                    <ItemTemplate>
                        <asp:Label ID="lblCustomerName" runat="server" Text='<%# Eval("custfullname") %>'></asp:Label>
                        <asp:HiddenField ID="hdnCustCode" runat="server" Value='<%# Eval("ID") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="CATEGORY">
                    <ItemTemplate>
                        <asp:Label ID="lblCustCategory" runat="server" Text='<%# Eval("CustCategory") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="REGION">
                    <ItemTemplate>
                        <asp:Label ID="lblRegion" runat="server" Text='<%# Eval("region") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="PAYMENT DAYS">
                    <ItemTemplate>
                        <%# Eval("CreditNote").ToString() == "True" ? "Probation" : "Establish" %>
                        :
                        <%# Eval("Paymentdays") %>
                        days
                    </ItemTemplate>
                    <EditItemTemplate>
                        <div>
                            <asp:HiddenField runat="server" ID="hdnCrediteNote" Value='<%# Eval("CreditNote")%>' />
                            <asp:RadioButtonList ID="rdoPaymentMode" runat="server" ClientIDMode="Static" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Establish" Value="Establish"></asp:ListItem>
                                <asp:ListItem Text="Probation" Value="Probation"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                        <div id="divPaymentDays" style="display: none;">
                            <span style="color: Green;">No Of Days:</span>&nbsp;
                            <asp:TextBox ID="txtpaymentDays" runat="server" Text='<%# Eval("Paymentdays") %>'
                                MaxLength="3" Width="100px" ClientIDMode="Static" onkeypress="return isNumberWithoutDecimal(event)"></asp:TextBox>
                        </div>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="SUPPLY LIMIT">
                    <ItemTemplate>
                        <%# Eval("SalesLimit")%>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtSalesLimit" runat="server" Text='<%# Eval("SalesLimit") %>' MaxLength="13"
                            Width="100px" ClientIDMode="Static" onkeypress="return isNumberKey(event)"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ACTION" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="button" BackColor="#27868a"
                            Font-Bold="true" CommandName="Edit" />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" CommandName="Update"
                            OnClientClick="javascript:return CtrlValidate();" />
                        &nbsp;&nbsp;
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="button" BackColor="#de820f"
                            CommandName="Cancel" />
                    </EditItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <script type="text/javascript">
        $(function () {
            $('input:radio[id*=rdoPaymentMode]').click(function (e) {
                ShowPayDaysCtrl();
            });
        });
        function ShowPayDaysCtrl() {
            if ($('input:radio[id*=rdoPaymentMode]:checked').val() == "Establish")
                $('#divPaymentDays').css({ 'display': 'block' });
            else if ($('input:radio[id*=rdoPaymentMode]:checked').val() == "Probation")
                $('#divPaymentDays').css({ 'display': 'none' });
        }

        function CtrlValidate() {
            var errmsg = '';
            if ($('input:radio[id*=rdoPaymentMode]:checked').length == 0)
                errmsg += 'Select Establish/Probation <br />';
            else if ($('input:radio[id*=rdoPaymentMode]:checked').val() == "Establish") {
                if ($('#txtpaymentDays').val().length == 0)
                    errmsg += 'Enter days <br/>';
            }
            if ($('#txtSalesLimit').val().length == 0)
                errmsg += 'Enter Sales Limit <br/>';
            if (errmsg.length > 0) {
                alert(errmsg);
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
