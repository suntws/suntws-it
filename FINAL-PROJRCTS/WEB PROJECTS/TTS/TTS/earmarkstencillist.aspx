<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="earmarkstencillist.aspx.cs" Inherits="TTS.earmarkstencillist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .gridcss
        {
            width: 100%;
            background-color: #fff;
            margin: 5px 0 10px 0;
            border: solid 1px #525252;
            border-collapse: collapse;
        }
        .gridcss td
        {
            padding: 2px;
            border: solid 1px #c1c1c1;
            height: 25px;
        }
        .gridcss th
        {
            padding: 4px 2px;
            color: #fff;
            background: #4293da;
            border-left: solid 1px #74bbf9;
            font-size: 0.9em;
        }
        .gridcss tr:hover
        {
            background-color: #4293da45;
            font-weight: bold;
        }
        .tableCss
        {
            width: 100%;
            background-color: #dcecfb;
            border-color: White;
            border-collapse: separate;
        }
        .tableCss th
        {
            font-weight: normal;
            text-align: center;
        }
        .tableCss td
        {
            font-weight: 500;
            text-align: left;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        <asp:Label runat="server" ID="lblPageHead" ClientIDMode="Static" Text=""></asp:Label>
        STENCIL ASSIGNED LIST</div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table border="1" class="tableCss" style="width: 100%;">
            <tr>
                <th>
                    <span class="spanCss">Plant</span>
                </th>
                <td>
                    <asp:DropDownList ID="ddlplant" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="ddlplant_SelectedIndexChanged"
                        AutoPostBack="true" CssClass="form-control" Width="120px">
                    </asp:DropDownList>
                </td>
                <th>
                    <span class="spanCss">Year</span>
                </th>
                <td>
                    <asp:DropDownList ID="ddlYear" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged"
                        AutoPostBack="true" CssClass="form-control" Width="120px">
                    </asp:DropDownList>
                </td>
                <th>
                    <span class="spanCss">Month</span>
                </th>
                <td>
                    <asp:DropDownList ID="ddlMonth" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged"
                        AutoPostBack="true" CssClass="form-control" Width="120px">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <table style="width: 100%; background-color: #dcecfb; border-color: White; border-collapse: separate;"
            border="1">
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" Font-Bold="true"
                        ForeColor="Red"></asp:Label>
                    <asp:GridView runat="server" ID="gvEarmarkedorderlist" AutoGenerateColumns="false"
                        Width="100%" CssClass="gridcss" 
                        onselectedindexchanged="gvEarmarkedorderlist_SelectedIndexChanged">
                        <Columns>
                            <asp:TemplateField HeaderText="CUSTOMER NAME">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatusCustName" Text='<%# Eval("custfullname") %>'></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdnStatusCustCode" Value='<%# Eval("CustCode") %>' />
                                    <asp:HiddenField runat="server" ID="hdnOrderID" Value='<%# Eval("ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ORDER REF NO.">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOrderRefNo" Text='<%#Eval("OrderRefNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="WORK ORDER" DataField="workorderno" />
                            <asp:BoundField HeaderText="ORDERED DATE" DataField="CompletedDate" ItemStyle-Width="70px" />
                            <asp:BoundField HeaderText="ORDER QTY" DataField="orderqty" ItemStyle-Width="40px"
                                ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField HeaderText="EARMARK QTY" DataField="earmarkedqty" ItemStyle-Width="40px"
                                ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField HeaderText="PLANT" DataField="Plant" ItemStyle-Width="40px" />
                            <asp:TemplateField HeaderText="ACTION" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px">
                                <ItemTemplate>
                                    <%--<asp:LinkButton ID="lnkEarmarkBtn" runat="server" Text="Show" OnClick="lnkEarmarkBtn_Click" />--%>
                                    <asp:LinkButton runat="server" ID="lnkEarmarkExcel" ClientIDMode="Static" Text="DOWNLOAD EXCEL"
                                        OnClick="lnkEarmarkExcel_Click" Visible='<%# Convert.ToBoolean(Eval("earmarkfilestatus")) %>'></asp:LinkButton>
                                    <asp:HiddenField runat="server" ID="hdnEarmarkfile" ClientIDMode="Static" Value='<%# Eval("earmarkfilename") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="width: 100%; display: none; padding-top: 5px;" id="divStatusChange">
                        <div style="width: 100%; background-color: #cecece; height: 25px; font-size: 18px;">
                            <div style="width: 50%; float: left; text-align: left; background-color: #cecece;
                                height: 25px;">
                                <asp:Label runat="server" ID="lblCustName" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                            </div>
                            <div style="width: 50%; float: left; text-align: right; background-color: #cecece;
                                height: 25px;">
                                <asp:Label runat="server" ID="lblStausOrderRefNo" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label></div>
                        </div>
                        <div style="width: 100%;">
                            <asp:GridView runat="server" ID="gvOrderItemList" AutoGenerateColumns="false" Width="100%"
                                ShowFooter="true" CssClass="gridcss">
                                <Columns>
                                    <asp:TemplateField HeaderText="CATEGORY">
                                        <ItemTemplate>
                                            <%# Eval("category") %>
                                            <%# Eval("AssyRimstatus").ToString() == "True" ? " (ASSY)" : ""%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="PLATFORM" DataField="config" />
                                    <asp:BoundField HeaderText="TYRE SIZE" DataField="tyresize" />
                                    <asp:BoundField HeaderText="RIM" DataField="rimsize" />
                                    <asp:BoundField HeaderText="TYPE" DataField="tyretype" />
                                    <asp:BoundField HeaderText="BRAND" DataField="brand" />
                                    <asp:BoundField HeaderText="SIDEWALL" DataField="sidewall" />
                                    <asp:BoundField HeaderText="BASIC PRICE" DataField="listprice" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField HeaderText="QTY" DataField="itemqty" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField HeaderText="FWT" DataField="tyrewt" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField HeaderText="RIM QTY" DataField="Rimitemqty" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField HeaderText="RIM BASIC PRICE" DataField="Rimunitprice" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField HeaderText="RIM FWT" DataField="Rimfinishedwt" ItemStyle-HorizontalAlign="Right" />
                                </Columns>
                                <FooterStyle BackColor="#cecece" HorizontalAlign="Right" />
                            </asp:GridView>
                        </div>
                    </div>
                    <div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnCustCode" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnOID" ClientIDMode="Static" Value="" />
</asp:Content>
