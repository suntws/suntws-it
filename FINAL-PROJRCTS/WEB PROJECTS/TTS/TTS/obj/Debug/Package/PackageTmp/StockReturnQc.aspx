<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="StockReturnQc.aspx.cs" Inherits="TTS.StockReturnQc" %>

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
        <table cellspacing="0" rules="all" border="1" style="background-color: #E2F5E1; width: 100%;
            border-color: #E2F5E1; border-collapse: collapse;">
            <tr>
                <td>
                    <asp:GridView ID="gv_Returnitems" runat="server" AutoGenerateColumns="false" Width="100%"
                        CssClass="gridcss" ClientIDMode="Static">
                        <RowStyle HorizontalAlign="Center" />
                        <HeaderStyle Font-Bold="true" Height="28px" HorizontalAlign="Center" Font-Size="13px" />
                        <Columns>
                            <asp:BoundField DataField="custfullname" HeaderText="CUSTOMER" />
                            <asp:BoundField DataField="InvoiceNo" HeaderText="INVOICE NO" />
                            <asp:BoundField DataField="OrderRefNo" HeaderText="ORDER REF NO" />
                            <asp:BoundField DataField="ReturnQty" HeaderText="RETURN QTY" />
                            <asp:BoundField DataField="DcNo" HeaderText="DC NO" />
                            <asp:BoundField DataField="DateOfReceipt" HeaderText="DATE OF RECEIPT" />
                            <asp:TemplateField HeaderText="ACTION">
                                <ItemTemplate>
                                    <asp:Button ID="btnQCReport" runat="server" Text="INSPECTION" ClientIDMode="Static"
                                        BorderStyle="None" CssClass="btn btn-info" OnClick="btnQCReport_Click" />
                                    <asp:HiddenField runat="server" ID="hdnReturnID" Value='<%# Eval("ReturnID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr id="displayGrid" style="width: 100%; display: none;">
                <td>
                    <table cellspacing="0" rules="all" border="1" style="background-color: #E2F5E1; width: 100%;
                        border-color: #E2F5E1; border-collapse: collapse;">
                        <tr style="background-color: #fdbee9;">
                            <td>
                                <asp:Label runat="server" ID="lblCustName" ClientIDMode="Static" Text="" Font-Bold="true"
                                    ForeColor="Black" Font-Size="17px"></asp:Label>
                            </td>
                            <td>
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
                            <td colspan="2">
                                <asp:GridView ID="GV_QC" runat="server" AutoGenerateColumns="false" Width="100%"
                                    HorizontalAlign="Center" BorderStyle="None" ClientIDMode="Static">
                                    <HeaderStyle BackColor="#DBE8A9" ForeColor="#A52A2A" Font-Bold="true" Height="22px"
                                        HorizontalAlign="Center" />
                                    <RowStyle BackColor="#F5EBDD" HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:BoundField DataField="ProcessID" HeaderText="PROCESS-ID" />
                                        <asp:BoundField DataField="Stencilno" HeaderText="STENCIL NO" />
                                        <asp:TemplateField HeaderText="GRADE">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddl_Grade" runat="server" CssClass="txtGrade form-control"
                                                    Width="80px">
                                                    <asp:ListItem Text="SELECT"></asp:ListItem>
                                                    <asp:ListItem Text="A"></asp:ListItem>
                                                    <asp:ListItem Text="B"></asp:ListItem>
                                                    <asp:ListItem Text="C"></asp:ListItem>
                                                    <asp:ListItem Text="D"></asp:ListItem>
                                                    <asp:ListItem Text="E"></asp:ListItem>
                                                    <asp:ListItem Text="R"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:HiddenField runat="server" ID="hdnGrade" Value='<%# Eval("Grade") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="QC OPINION">
                                            <ItemTemplate>
                                                <asp:TextBox runat="server" ID="txtReason" CssClass="txtReason form-control" MaxLength="50"
                                                    Style="width: 250px;" Text="ACCEPT" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="CONDITION OF THE TYRE">
                                            <ItemTemplate>
                                                <asp:TextBox runat="server" ID="txtConditionofthetyre" CssClass="txtReason form-control"
                                                    MaxLength="50" Style="width: 450px;" Text="OK" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr style="text-align: center;">
                            <td>
                                <asp:Button ID="btnQCSave" runat="server" Text="SAVE" ClientIDMode="Static" CssClass="btn btn-success"
                                    OnClick="btnQCSave_Click" OnClientClick="javascript:return cntrlSave();" />
                            </td>
                            <td>
                                <asp:Button ID="btnQCCancel" runat="server" Text="CANCEL" ClientIDMode="Static" CssClass="btn btn-warning"
                                    OnClick="btnQCCancel_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnReturnID" ClientIDMode="Static" Value="" />
    <script src="Scripts/datemonthyear.js" type="text/javascript"></script>
    <script type="text/javascript">
        function cntrlSave() {
            var count = 0;
            var count1 = 0;
            $('.txtReason').each(function (index, item) {
                if ($(this).val() == "") {
                    $(this).css("border", "1px solid red");
                    count = 1;
                } else
                    $(this).css("border", "1px solid black");
            }, 0);

            $('.txtGrade').each(function (index, item) {
                if ($(this).val() == "SELECT") {
                    $(this).css("border", "1px solid red");
                    count1 = 1;
                } else
                    $(this).css("border", "1px solid black");
            }, 0);
            if (count > 0 || count1 > 0) {
                alert("SelectGrade,\n Enter Reason, \n Enter Condition of the Tyre");
                return false;
            } else
                return true;
        }
        function showReturnQc() {
            $('#displayGrid').css({ 'display': 'block' });
        }     
    </script>
</asp:Content>
