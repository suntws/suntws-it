<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="cotscustwisetrack.aspx.cs" Inherits="TTS.cotscustwisetrack" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .tbLnk
        {
            border-collapse: collapse;
            border-color: #861616;
            width: 100%;
        }
        .tbLnk th
        {
            text-align: left;
            width: 100px;
            font-size: 12px;
            font-weight: bold;
        }
        .tbLnk td
        {
            background-color: #e4dbdb;
            text-decoration: none;
            font-size: 12px;
        }
        .tbMaster
        {
            border-collapse: collapse;
            width: 100%;
        }
        .tbMaster th
        {
            text-align: left;
            width: 115px;
            font-size: 12px;
        }
        .tbMaster td
        {
            font-weight: bold;
            font-size: 12px;
        }
    </style>
    <script src="Scripts/cotsScript.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        <asp:Label runat="server" ID="lblPageHead" ClientIDMode="Static" Text=""></asp:Label>
        CUSTOMER WISE ORDER LIST</div>
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
                <td colspan="3">
                    <asp:DropDownList runat="server" ID="ddlCustomerName" ClientIDMode="Static" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlCustomerName_IndexChange" CssClass="form-control"
                        Width="400px">
                    </asp:DropDownList>
                </td>
                <th>
                    USER ID
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="ddlUserID" ClientIDMode="Static" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlUserID_IndexChange" CssClass="form-control" Width="200px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    PLANT
                </th>
                <td>
                    <asp:DropDownList ID="ddlplant" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="ddlplant_SelectedIndexChanged"
                        AutoPostBack="true" CssClass="form-control" Width="180px">
                    </asp:DropDownList>
                </td>
                <th>
                    YEAR
                </th>
                <td>
                    <asp:DropDownList ID="ddlYear" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged"
                        AutoPostBack="true" CssClass="form-control" Width="180px">
                    </asp:DropDownList>
                </td>
                <th>
                    MONTH
                </th>
                <td>
                    <asp:DropDownList ID="ddlMonth" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged"
                        AutoPostBack="true" CssClass="form-control" Width="200px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="6">
                    <asp:GridView runat="server" ID="gv_TrackOrderList" AutoGenerateColumns="false" Width="100%"
                        HeaderStyle-CssClass="gvHeadBg" AlternatingRowStyle-BackColor="#F0E8E8" Font-Bold="true"
                        AllowPaging="true" OnPageIndexChanging="gv_TrackOrderList_PageIndex" PageSize="50"
                        PagerStyle-Height="30px" PagerStyle-Font-Bold="true" PagerStyle-Font-Size="15px"
                        PagerStyle-HorizontalAlign="Center" PagerStyle-VerticalAlign="Middle">
                        <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" Height="22px" ForeColor="Black" />
                        <Columns>
                            <asp:BoundField HeaderText="ORDER NO" DataField="OrderRefNo" />
                            <asp:BoundField HeaderText="ORDERED DATE" DataField="CompletedDate" />
                            <asp:BoundField HeaderText="LAST REVISED DATE" DataField="RevisedDate" />
                            <asp:BoundField HeaderText="EXPECT SHIPPING DATE" DataField="ExpectedShipDate" />
                            <asp:BoundField HeaderText="DISPATCHED DATE" DataField="DispatchedDate" />
                            <asp:BoundField HeaderText="QTY" DataField="itemqty" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField HeaderText="INVOICE DATE" DataField="invoicedate" />
                            <asp:BoundField HeaderText="INVOICE NO" DataField="invoiceno" />
                            <asp:BoundField HeaderText="PLANT" DataField="Plant" />
                            <asp:BoundField HeaderText="STATUS" DataField="StatusText" />
                            <asp:TemplateField HeaderText="ACTION">
                                <ItemTemplate>
                                    <asp:Button runat="server" ID="btnShowDetails" Text="SHOW" OnClick="btnShowDetails_Click"
                                        CommandArgument='<%#Eval("ID").ToString() %>' CssClass="btn btn-info" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerSettings Mode="Numeric" Position="Bottom" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="6">
                    <asp:Label runat="server" ID="lblQuoteHead1" ClientIDMode="Static" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="6">
                    <div id="divItemDetails" style="display: none; width: 100%;">
                        <table cellspacing="0" rules="all" border="1" style="line-height: 20px; border-collapse: collapse;
                            border-color: #868282; width: 100%; background-color: #d4f3fd;">
                            <tr>
                                <td>
                                    ORDER REF NO.: &nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="lblOrderNo" runat="server" Font-Bold="True" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DataList runat="server" ID="dlOrderMaster" RepeatColumns="1" RepeatDirection="Horizontal"
                                        RepeatLayout="Table" AlternatingItemStyle-BackColor="#A1C9D3" ItemStyle-BackColor="#C9DAC9"
                                        ItemStyle-VerticalAlign="Top" Width="100%">
                                        <ItemTemplate>
                                            <table cellspacing="0" rules="all" border="1" class="tbMaster">
                                                <tr>
                                                    <th>
                                                        STATUS
                                                    </th>
                                                    <td style="font-weight: bold; font-size: 20px; float: left;">
                                                        <%# Eval("StatusText")%>
                                                    </td>
                                                    <td rowspan="4">
                                                        <span class="headCss">Special Instruction :</span>
                                                        <%# Eval("SplIns")%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        Orderd Date
                                                    </th>
                                                    <td>
                                                        <%# Eval("CompletedDate")%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        Packing Method
                                                    </th>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        Freight Charges
                                                    </th>
                                                    <td>
                                                        <%# Eval("PackingMethod") %>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        Delivery Method
                                                    </th>
                                                    <td>
                                                        <%# Eval("DeliveryMethod")%>
                                                    </td>
                                                    <td rowspan="4">
                                                        <span class="headCss">Special Requset :</span>
                                                        <%# Eval("SpecialRequset") %>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        Transport Details
                                                    </th>
                                                    <td>
                                                        <%# Eval("TransportDetails")%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        Desired Ship Date
                                                    </th>
                                                    <td>
                                                        <%# Eval("DesiredShipDate")%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        Expected Ship Date
                                                    </th>
                                                    <td>
                                                        <%# Eval("ExpectedShipDate")%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <div style="width: 530px; float: left; line-height: 16px;">
                                                            <span class="headCss">Bill To: </span>
                                                            <%# Bind_BillingAddress(Eval("BillID").ToString())%>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="width: 530px; float: left; line-height: 16px;">
                                                            <span class="headCss">Ship To: </span>
                                                            <%# Bind_BillingAddress(Eval("ShipID").ToString())%>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </asp:DataList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView runat="server" ID="gvOrderItemList" AutoGenerateColumns="false" Width="100%"
                                        AlternatingRowStyle-BackColor="#f5f5f5" ShowFooter="true">
                                        <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" Height="22px" />
                                        <Columns>
                                            <asp:BoundField HeaderText="CATEGORY" DataField="category" />
                                            <asp:BoundField HeaderText="PLATFORM" DataField="Config" />
                                            <asp:BoundField HeaderText="SIZE" DataField="tyresize" />
                                            <asp:BoundField HeaderText="RIM" DataField="rimsize" />
                                            <asp:BoundField HeaderText="TYPE" DataField="tyretype" />
                                            <asp:BoundField HeaderText="BRAND" DataField="brand" />
                                            <asp:BoundField HeaderText="SIDEWALL" DataField="sidewall" />
                                            <asp:BoundField HeaderText="PRICE" DataField="listprice" ItemStyle-CssClass="numericright" />
                                            <asp:BoundField HeaderText="QTY" DataField="itemqty" ItemStyle-CssClass="numericright" />
                                            <asp:BoundField HeaderText="TOTAL PRICE" DataField="unitprice" ItemStyle-CssClass="numericright" />
                                            <asp:BoundField HeaderText="TOTAL WT(Kgs)" DataField="finishedwt" ItemStyle-CssClass="numericright" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table cellspacing="0" rules="all" border="1" class="tbLnk">
                                        <tr>
                                            <td colspan="4">
                                                <asp:Label runat="server" ID="lblAttachment" ClientIDMode="Static" Text="" Font-Bold="true"
                                                    Font-Size="20px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                <asp:Label runat="server" ID="lblProformaTxt" ClientIDMode="Static" Text=""></asp:Label>
                                            </th>
                                            <td>
                                                <asp:LinkButton runat="server" ID="lnkproformafiles" Text="" ClientIDMode="Static"
                                                    OnClick="lnkAttachFile_Click"></asp:LinkButton>
                                            </td>
                                            <th>
                                                <asp:Label runat="server" ID="lblInvoiceTxt" ClientIDMode="Static" Text=""></asp:Label>
                                            </th>
                                            <td>
                                                <asp:LinkButton runat="server" ID="lnkinvoicefiles" Text="" ClientIDMode="Static"
                                                    OnClick="lnkAttachFile_Click"></asp:LinkButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                <asp:Label runat="server" ID="lblLRTxt" ClientIDMode="Static" Text=""></asp:Label>
                                            </th>
                                            <td>
                                                <asp:LinkButton runat="server" ID="lnklrcopyfiles" Text="" ClientIDMode="Static"
                                                    OnClick="lnkAttachFile_Click"></asp:LinkButton>
                                            </td>
                                            <th>
                                                <asp:Label runat="server" ID="lblPdiList" ClientIDMode="Static" Text=""></asp:Label>
                                            </th>
                                            <td>
                                                <asp:LinkButton runat="server" ID="lnkPdiList" Text="" ClientIDMode="Static" OnClick="lnkAttachFile_Click"></asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnOID" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnCustomerName" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        $(function () {
            $('#ddlCustomerName').change(function () {
                $('#hdnCustomerName').val($('#ddlCustomerName option:selected').text());
            });
        });
    </script>
</asp:Content>
