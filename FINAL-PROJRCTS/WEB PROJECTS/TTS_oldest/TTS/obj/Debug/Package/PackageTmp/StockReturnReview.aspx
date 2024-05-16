<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="StockReturnReview.aspx.cs" Inherits="TTS.StockReturnReview" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: center;" class="pageTitleHead">
        DISPATCH RETURN REVIEW
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <asp:ScriptManager runat="server" ID="ScriptManager1">
    </asp:ScriptManager>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="background-color: #E2F5E1; width: 100%;
            border-color: #E2F5E1; border-collapse: collapse;">
            <tr>
                <th>
                    Plant
                </th>
                <td>
                    <asp:DropDownList ID="ddlplant" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="ddlplant_SelectedIndexChanged"
                        AutoPostBack="true" CssClass="form-control" Width="120px" Font-Size="14px">
                    </asp:DropDownList>
                </td>
                <th>
                    Year
                </th>
                <td>
                    <asp:DropDownList ID="ddlYear" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged"
                        AutoPostBack="true" CssClass="form-control" Width="120px" Font-Size="14px">
                    </asp:DropDownList>
                </td>
                <th>
                    Month
                </th>
                <td>
                    <asp:DropDownList ID="ddlMonth" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged"
                        AutoPostBack="true" CssClass="form-control" Width="120px" Font-Size="14px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="6">
                    <asp:GridView ID="gv_Returnitemslist" runat="server" AutoGenerateColumns="false"
                        Width="100%">
                        <HeaderStyle BackColor="#E2BA4E" Font-Bold="true" Height="22px" HorizontalAlign="Center" />
                        <Columns>
                            <asp:BoundField DataField="custfullname" HeaderText="CUSTOMER" />
                            <asp:BoundField DataField="OrderRefNo" HeaderText="ORDER REF NO" />
                            <asp:BoundField DataField="InvoiceNo" HeaderText="INVOICE NO" />
                            <asp:BoundField DataField="ReturnQty" HeaderText="RETURN QTY" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="DcNo" HeaderText="DC NO" />
                            <asp:BoundField DataField="DateOfReceipt" HeaderText="DATE OF RECEIPT" />
                            <asp:BoundField DataField="CreditNote" HeaderText="CREDIT NOTE NO" />
                            <asp:BoundField DataField="CreditDate" HeaderText="CREDIT ON" />
                            <asp:BoundField DataField="CreditAmount" HeaderText="CREDIT VALUE" />
                            <asp:TemplateField HeaderText="ACTION" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Button runat="server" ID="btnView" ClientIDMode="Static" Text="VIEW" CssClass="btn btn-success"
                                        OnClick="btnView_Click" />
                                    <asp:LinkButton runat="server" ID="lnkPdfLink" ClientIDMode="Static" Text="PDF DOWNLOAD"
                                        OnClick="lnkPdf_Click" OnClientClick="aspnetForm.target ='_blank';" Font-Size="10px"></asp:LinkButton>
                                    <asp:HiddenField runat="server" ID="hdnReturnID" ClientIDMode="Static" Value='<%# Eval("ReturnID") %>' />
                                    <asp:HiddenField runat="server" ID="hdnCustCode" ClientIDMode="Static" Value='<%# Eval("CustCode") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="6">
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
                                <asp:GridView ID="GV_ReturnItem" runat="server" AutoGenerateColumns="false" Width="100%"
                                    HorizontalAlign="Center" ClientIDMode="Static">
                                    <HeaderStyle BackColor="#875200" ForeColor="White" Font-Bold="true" Height="22px"
                                        HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:BoundField DataField="ProcessID" HeaderText="PROCESSID" />
                                        <asp:BoundField DataField="Stencilno" HeaderText="STENCILNO" />
                                        <asp:BoundField DataField="Grade" HeaderText="SALES GRADE" />
                                        <asp:BoundField DataField="QCGrade" HeaderText="INSPECTION GRADE" />
                                        <asp:BoundField DataField="QCReason" HeaderText="RETURN REASON" />
                                        <asp:BoundField DataField="QCConditionOfTheTyre" HeaderText="CONDITION OF THE TYRE" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
