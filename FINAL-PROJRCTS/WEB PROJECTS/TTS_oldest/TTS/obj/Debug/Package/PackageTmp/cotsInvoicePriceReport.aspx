<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="cotsInvoicePriceReport.aspx.cs" Inherits="TTS.cotsInvoicePriceReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager runat="server" ClientIDMode="static" ID="scriptmanager1">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        DOMESTIC ORDER PRICE ANALYZE
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="line-height: 20px; border-collapse: collapse;
            border-color: #868282; width: 100%; background-color: #d4f3fd;">
            <tr>
                <th>
                    CUSTOMER
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="ddlCotsCustName" ClientIDMode="Static" Width="500px"
                        OnSelectedIndexChanged="ddlCotsCustName_IndexChange" AutoPostBack="true" Font-Bold="true"
                        CssClass="form-control">
                    </asp:DropDownList>
                </td>
                <th>
                    USER ID
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="ddlLoginUserName" ClientIDMode="Static" Width="150px"
                        OnSelectedIndexChanged="ddlLoginUserName_IndexChange" AutoPostBack="true" CssClass="form-control">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #CE8686;
                                width: 100%;" id="tblPriceSheet">
                                <tr align="center" class="headCss" style="background-color: #EBEEED;">
                                    <td style="width: 95px">
                                        CATEGORY
                                    </td>
                                    <td style="width: 120px">
                                        PLATFORM
                                    </td>
                                    <td style="width: 120px">
                                        BRAND
                                    </td>
                                    <td style="width: 120px">
                                        SIDEWALL
                                    </td>
                                    <td style="width: 80px">
                                        TYPE
                                    </td>
                                    <td style="width: 200px">
                                        SIZE
                                    </td>
                                    <td style="width: 80px">
                                        RIM
                                    </td>
                                    <td style="width: 120px">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlCategory" ClientIDMode="Static" Width="95px"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_IndexChange" CssClass="form-control">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlPlatform" ClientIDMode="Static" Width="118px"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlPlatform_IndexChange" CssClass="form-control">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlBrand" ClientIDMode="Static" Width="118px"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlBrand_IndexChange" CssClass="form-control">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlSidewall" ClientIDMode="Static" Width="118px"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlSidewall_IndexChange" CssClass="form-control">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlType" ClientIDMode="Static" Width="78px"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlType_IndexChange" CssClass="form-control">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlSize" ClientIDMode="Static" Width="198px"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlSize_IndexChange" CssClass="form-control">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlRim" ClientIDMode="Static" Width="78px" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlRim_IndexChange" CssClass="form-control">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:HiddenField runat="server" ID="hdnProcessId" ClientIDMode="Static" />
                                        <asp:Button runat="server" ClientIDMode="Static" ID="btnGetReport" OnClick="btnGetReport_Click"
                                            Visible="false" Text="GET REPORT" CssClass="btn btn-success" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="8">
                                        <asp:GridView runat="server" ClientIDMode="Static" ID="gvItemPriceList" AutoGenerateColumns="false"
                                            Width="100%" HeaderStyle-BackColor="#FFCC00">
                                            <Columns>
                                                <asp:BoundField DataField="OrderRefNo" HeaderText="ORDER REF NO" />
                                                <asp:BoundField DataField="invoiceno" HeaderText="INVOICE NO" />
                                                <asp:BoundField DataField="createdate" HeaderText="INVOICE DATE" />
                                                <asp:BoundField DataField="Config" HeaderText="PLATFORM" />
                                                <asp:BoundField DataField="tyresize" HeaderText="SIZE" />
                                                <asp:BoundField DataField="rimsize" HeaderText="RIM" />
                                                <asp:BoundField DataField="tyretype" HeaderText="TYPE" />
                                                <asp:BoundField DataField="brand" HeaderText="BRAND" />
                                                <asp:BoundField DataField="sidewall" HeaderText="SIDEWALL" />
                                                <asp:BoundField DataField="itemqty" HeaderText="QTY" ItemStyle-HorizontalAlign="Right" />
                                                <asp:BoundField DataField="unitprice" HeaderText="PRICE" ItemStyle-HorizontalAlign="Right" />
                                                <asp:BoundField DataField="Rimunitprice" HeaderText="RIM PRICE" ItemStyle-HorizontalAlign="Right" />
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
