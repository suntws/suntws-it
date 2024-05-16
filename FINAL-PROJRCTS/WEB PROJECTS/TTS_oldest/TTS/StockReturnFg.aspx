<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="StockReturnFg.aspx.cs" Inherits="TTS.StockReturnFg" %>

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
                    <asp:GridView ID="gv_Returnitems" runat="server" AutoGenerateColumns="false" Width="100%">
                        <HeaderStyle BackColor="#64DFFD" Font-Bold="true" Height="22px" HorizontalAlign="Center" />
                        <Columns>
                            <asp:BoundField DataField="custfullname" HeaderText="CUSTOMER" />
                            <asp:BoundField DataField="InvoiceNo" HeaderText="INVOICENO" />
                            <asp:BoundField DataField="OrderRefNo" HeaderText="ORDER REF NO" />
                            <asp:BoundField DataField="ReturnQty" HeaderText="RETURN QTY" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="DcNo" HeaderText="DC NO" />
                            <asp:BoundField DataField="DateOfReceipt" HeaderText="DATE OF RECEIPT" />
                            <asp:TemplateField HeaderText="ACTION" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Button ID="btnFGReport" runat="server" Text="SHOW" ClientIDMode="Static" CssClass="btn btn-success"
                                        OnClick="btnFGReport_Click" />
                                    <asp:HiddenField runat="server" ID="hdnReturnID" Value='<%# Eval("ReturnID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr id="gvhide" style="display: none;">
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
                                <asp:GridView ID="GV_QC" runat="server" AutoGenerateColumns="false" Width="100%">
                                    <HeaderStyle BackColor="#875200" ForeColor="White" Font-Bold="true" Height="22px"
                                        HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:BoundField DataField="ProcessID" HeaderText="PROCESSID" />
                                        <asp:BoundField DataField="Stencilno" HeaderText="STENCILNO" />
                                        <asp:BoundField DataField="Grade" HeaderText="SALES GRADE" />
                                        <asp:BoundField DataField="QCGrade" HeaderText="INSPECTION GRADE" />
                                        <asp:BoundField DataField="QCReason" HeaderText="INSPECTION OPINION" />
                                        <asp:BoundField DataField="QCConditionOfTheTyre" HeaderText="CONDITION OF THE TYRE" />
                                        <asp:BoundField DataField="Barcode" HeaderText="BARCODE" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lbl_remarks" ClientIDMode="Static" Text="REMARK" Font-Bold="true"
                                    Font-Size="15px"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_Remark" runat="server" TextMode="MultiLine" Height="60px" Width="380px"
                                    MaxLength="999" PlaceHolder="ENTER REMARK" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnFGSave" runat="server" Text="MOVE TO GODOWN" ClientIDMode="Static"
                                    OnClientClick="javascript:return cntrlSave();" CssClass="btn btn-success" OnClick="btnFGSave_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnFGCancel" runat="server" Text="CANCEL" ClientIDMode="Static" CssClass="btn btn-warning"
                                    OnClick="btnFGCancel_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnReturnID" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        function cntrlSave() {
            if ($('#txt_Remark').val() == "") {
                alert("Enter Remark");
                return false;
            }
            else
                return true;
        }

        function showdiv(divID) {
            $('#' + divID).css({ 'display': 'block' });
        }
    </script>
</asp:Content>
