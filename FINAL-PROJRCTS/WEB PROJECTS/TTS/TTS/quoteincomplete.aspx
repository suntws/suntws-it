<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="quoteincomplete.aspx.cs" Inherits="TTS.quoteincomplete" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .tableQuote
        {
            border-collapse: collapse;
            border-color: #868282;
            width: 100%;
            line-height: 20px;
        }
        .tableQuote th
        {
            background-color: #FFEEEC;
            text-align: left;
            padding-left: 10px;
            width: 130px;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div align="center" class="pageTitleHead">
        <asp:Label runat="server" ID="lblQuoteHead" ClientIDMode="Static" Text=""></asp:Label>
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #000;
            width: 100%;">
            <tr>
                <td align="center">
                    <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" Font-Bold="true"
                        ForeColor="Red" Font-Size="18px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <table class="tableQuote" cellspacing="0" rules="all" border="1">
                        <tr>
                            <th>
                                TYPE
                            </th>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlQCustType" ClientIDMode="Static" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlQCustType_IndexChange">
                                </asp:DropDownList>
                            </td>
                            <th>
                                CUSTOMER
                            </th>
                            <td colspan="4">
                                <asp:DropDownList runat="server" ID="ddlQCustName" ClientIDMode="Static" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlQCustName_IndexChange">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                QUOTE BY
                            </th>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlQUser" ClientIDMode="Static" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlQUser_IndexChange">
                                </asp:DropDownList>
                            </td>
                            <th>
                                YEAR
                            </th>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlQYear" ClientIDMode="Static" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlQYear_IndexChange">
                                </asp:DropDownList>
                            </td>
                            <th>
                                MONTH
                            </th>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlQMonth" ClientIDMode="Static" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlQMonth_IndexChange">
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: center;">
                                <span class="btnclear" onclick="btnReset_Click()">RESET</span>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView runat="server" ID="gvIncompleteQuoteItem" AutoGenerateColumns="false"
                        Width="100%" RowStyle-Height="24px">
                        <HeaderStyle BackColor="#CACA55" Font-Bold="true" Height="22px" />
                        <Columns>
                            <asp:BoundField DataField="QCustType" HeaderText="CUSTOMER" ItemStyle-Width="80px" />
                            <asp:BoundField DataField="QCustomer" HeaderText="NAME" ItemStyle-Width="200px" />
                            <asp:TemplateField HeaderText="QUOTE REF NO." ItemStyle-Width="120px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblQAcYear" ClientIDMode="Static" Text='<%#Eval("QAcYear") %>'></asp:Label>
                                    /
                                    <asp:Label runat="server" ID="lblQRefNo" ClientIDMode="Static" Text='<%#Eval("QRefNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="createddate" HeaderText="QUOTE DATE" ItemStyle-Width="80px" />
                            <asp:BoundField DataField="UserName" HeaderText="QUOTE BY" ItemStyle-Width="120px" />
                            <asp:TemplateField HeaderText="REVISED COUNT" ItemStyle-Width="60px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblRevisedCount" ClientIDMode="Static" Text='<%# Eval("QRevisedCount") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="reviseddate" HeaderText="REVISED DATE" ItemStyle-Width="80px" />
                            <asp:BoundField DataField="QRevisedBy" HeaderText="REVISED BY" ItemStyle-Width="120px" />
                            <asp:BoundField DataField="totalQty" HeaderText="QTY" ItemStyle-Width="50px" />
                            <asp:TemplateField HeaderText="ACTION" ItemStyle-Width="140px">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkPrepareQuotePage" runat="server" Text="SHOW QUOTE DETAILS"
                                        OnClick="lnkPrepareQuotePage_Click" Font-Size="10px"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PDF" ItemStyle-Width="120px">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkQuotePdf" runat="server" Text="DOWNLOAD QUOTE" OnClick="lnkQuotePdf_Click"
                                        Font-Size="10px"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        function btnReset_Click() {
            window.location.href = window.location.href;
        }
    </script>
</asp:Content>
