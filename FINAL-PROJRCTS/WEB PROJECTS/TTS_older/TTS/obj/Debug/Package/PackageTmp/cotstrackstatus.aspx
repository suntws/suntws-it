<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="cotstrackstatus.aspx.cs" Inherits="TTS.cotstrackstatus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="Scripts/cotsScript.js" type="text/javascript"></script>
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
    <link href="Styles/datemonthyear.css" rel="stylesheet" type="text/css" />
    <link href="Styles/scotsdomestic.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .btn-sm
        {
            padding: 5px 10px;
            font-size: 12px;
            line-height: 1.5;
            border-radius: 3px;
        }
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
            background-color: #4293da;
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
        #gvTrackOrderList th
        {
            color: White;
        }
        .filelnk
        {
            color: #095B80 !important;
            text-decoration: underline;
        }
        .imageCss
        {
            width: 40px;
            height: 40px;
            cursor: pointer;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        TRACK DOMESTIC ORDER CURRENT STATUS</div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table class="tableCss" style="width: 100%;" border="1">
            <tr>
                <th>
                    <span class="spanCss">Plant</span>
                </th>
                <td>
                    <asp:DropDownList ID="ddlplant" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="ddlplant_SelectedIndexChanged"
                        AutoPostBack="true" CssClass="form-control">
                        <asp:ListItem Text="ALL" Value="ALL"></asp:ListItem>
                        <asp:ListItem Text="MMN" Value="MMN"></asp:ListItem>
                        <asp:ListItem Text="PDK" Value="PDK"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <th>
                    <span class="spanCss">Total Orders</span>
                </th>
                <td>
                    <asp:Label runat="server" ID="lblOrderCount" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                </td>
                <th>
                    <span class="spanCss">Total Wt</span>
                </th>
                <td>
                    <asp:Label runat="server" ID="lblOrderTotWt" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                </td>
            </tr>
            <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" Font-Bold="true"
                ForeColor="Red" Font-Size="16px"></asp:Label>
        </table>
        <table style="width: 100%; background-color: #dcecfb; border-color: White; border-collapse: separate;"
            border="1">
            <tr>
                <td>
                    <asp:GridView runat="server" ID="gvTrackOrderList" AutoGenerateColumns="false" Width="100%"
                        AllowPaging="true" OnPageIndexChanging="gvTrackOrderList_PageIndex" PageSize="200"
                        PagerStyle-Height="30px" PagerStyle-Font-Bold="true" PagerStyle-Font-Size="15px"
                        PagerStyle-HorizontalAlign="Center" PagerStyle-VerticalAlign="Middle" CssClass="gridcss">
                        <Columns>
                            <asp:TemplateField HeaderText="CUSTOMER NAME">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatusCustName" Text='<%# Eval("custfullname") %>'></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdnStatusCustCode" Value='<%# Eval("CustCode") %>' />
                                    <asp:HiddenField runat="server" ID="hdnUserCurrency" Value='<%# Eval("usercurrency") %>' />
                                    <asp:HiddenField runat="server" ID="hdnOrderID" Value='<%# Eval("ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ORDER REFERENCE NO">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOrderRefNo" Text='<%#Eval("OrderRefNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ORDER DATE" ItemStyle-Width="70px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOrderDate" Text='<%#Eval("CompletedDate") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="DESIRED SHIP DATE" DataField="DesiredShipDate" ItemStyle-Width="65px" />
                            <asp:BoundField HeaderText="PPC RFD DATE" DataField="RFD_Date" ItemStyle-Width="70px" />
                            <asp:BoundField HeaderText="QTY" DataField="itemqty" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField HeaderText="FWT" DataField="FWT" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField HeaderText="PLANT" DataField="Plant" />
                            <asp:TemplateField HeaderText="STATUS">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatusText" Text='<%# Eval("StatusText") %>'></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdnStatusID" Value='<%# Eval("OrderStatus") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="LEAD" DataField="Lead" />
                            <asp:BoundField HeaderText="MANAGER" DataField="Manager" />
                            <asp:TemplateField HeaderText="ACTION" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkTrackBtn" runat="server" Text="Show" OnClick="lnkTrackBtn_Click" />
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
                            <asp:Label runat="server" ID="lblCurrStatus" ClientIDMode="Static" Text="" Font-Bold="true"
                                Font-Size="Large"></asp:Label>
                            <asp:DataList runat="server" ID="dlOrderMaster" RepeatColumns="1" RepeatDirection="Horizontal"
                                RepeatLayout="Table" AlternatingItemStyle-BackColor="#A1C9D3" ItemStyle-BackColor="#c6dcef"
                                ItemStyle-VerticalAlign="Top" Width="100%">
                                <ItemTemplate>
                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="width: 50%;">
                                                <div style="width: 265px; float: left;">
                                                    <span class="spanCss">Orderd Date: </span>
                                                    <%# Eval("CompletedDate")%>
                                                </div>
                                                <div style="width: 265px; float: left;">
                                                    <span class="spanCss">Packing Method: </span>
                                                    <%# Eval("PackingMethod") %>
                                                </div>
                                                <div style="width: 265px; float: left;">
                                                    <span class="spanCss">Freight Charges: </span>
                                                    <%# Eval("FreightCharges") %>
                                                </div>
                                                <div style="width: 525px; float: left; line-height: 16px;">
                                                    <span class="spanCss">Bill To: </span>
                                                    <%# Bind_BillingAddress(Eval("BillID").ToString())%>
                                                </div>
                                            </td>
                                            <td style="width: 50%;">
                                                <div style="width: 265px; float: left;">
                                                    <span class="spanCss">Delivery Method: </span>
                                                    <%# Eval("DeliveryMethod")%>
                                                </div>
                                                <div style="width: 265px; float: left;">
                                                    <span class="spanCss">Transport Details: </span>
                                                    <%# Eval("TransportDetails")%>
                                                </div>
                                                <div style="width: 265px; float: left;">
                                                    <span class="spanCss">Desired Ship Date:</span>
                                                    <%# Eval("DesiredShipDate")%>
                                                </div>
                                                <div style="width: 265px; float: left;">
                                                    <span class="spanCss">Expected Ship Date:</span>
                                                    <%# Eval("ExpectedShipDate")%>
                                                </div>
                                                <div style="width: 525px; float: left; line-height: 16px;">
                                                    <span class="spanCss">Ship To: </span>
                                                    <%# Bind_BillingAddress(Eval("ShipID").ToString())%>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:DataList>
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
                        <div id="div_DownloadFiles" style="width: 100%; text-align: right;">
                            <table align="right">
                                <tr>
                                    <td>
                                        <span class="spanCss">Download Files</span>&nbsp;&nbsp;
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="imgdownload" ImageUrl="images/Download.png" ClientIDMode="Static"
                                            runat="server" CssClass="imageCss" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div style="width: 1063px; float: left;">
                            <div style="width: 530px; float: left;">
                                <span class="spanCss">Special Instruction</span>
                                <asp:TextBox runat="server" ID="txtOrderSplIns" Text="" TextMode="MultiLine" Width="520px"
                                    Height="100px" Enabled="false"></asp:TextBox>
                            </div>
                            <div style="width: 530px; float: right;">
                                <span class="spanCss">Special Notes</span>
                                <asp:TextBox runat="server" ID="txtOrdersplReq" Text="" TextMode="MultiLine" Width="520px"
                                    Height="100px" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                        <div style="width: 1063px; float: left; line-height: 15px;">
                            <div style="width: 530px; float: left;">
                                <asp:Label runat="server" ID="lblReviseHistory" CssClass="spanCss" ClientIDMode="Static"
                                    Text="" Font-Bold="true"></asp:Label>
                                <asp:GridView runat="server" ID="gvRevisedHistory" AutoGenerateColumns="false" Width="520px"
                                    RowStyle-Height="25px" CssClass="gridcss">
                                    <Columns>
                                        <asp:BoundField DataField="ReviseType" HeaderText="REVISE TYPE" ItemStyle-Width="120px" />
                                        <asp:BoundField DataField="ProcessID" ItemStyle-CssClass="headerNone" HeaderStyle-CssClass="headerNone" />
                                        <asp:BoundField DataField="Preview" HeaderText="PREVIOUS" ItemStyle-Width="100px" />
                                        <asp:BoundField DataField="Revise" HeaderText="REVISE" ItemStyle-Width="100px" />
                                        <asp:BoundField DataField="CreatedDate" HeaderText="DATE" ItemStyle-Width="80px" />
                                        <asp:BoundField DataField="UserName" HeaderText="BY" ItemStyle-Width="120px" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div style="width: 530px; float: left;">
                                <asp:Label runat="server" ID="lblStatusHistory" CssClass="spanCss" ClientIDMode="Static"
                                    Text="" Font-Bold="true"></asp:Label>
                                <asp:GridView runat="server" ID="gvStatusHistory" AutoGenerateColumns="false" Width="520px"
                                    CssClass="gridcss" RowStyle-Height="25px">
                                    <Columns>
                                        <asp:BoundField DataField="StatusText" HeaderText="STATUS" ItemStyle-Width="100px" />
                                        <asp:BoundField DataField="feedback" HeaderText="COMMENTS" ItemStyle-Width="220px" />
                                        <asp:BoundField DataField="statusdate" HeaderText="DATE" ItemStyle-Width="80px" />
                                        <asp:BoundField DataField="username" HeaderText="CHANGED BY" ItemStyle-Width="100px" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <!-- Popup Window -->
    <div id="dialog" style="display: none;">
        <asp:GridView runat="server" ID="gv_DownloadFiles" ClientIDMode="Static" AutoGenerateColumns="false"
            CssClass="gridcss">
            <HeaderStyle />
            <Columns>
                <asp:TemplateField HeaderText="File Type">
                    <ItemTemplate>
                        <asp:Label ID="lblFileType" runat="server" Text='<%# Eval("FileType")%>' CssClass="spanCss">
                        </asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Click link to Download File" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkPdfFileName" runat="server" Text='<%# Eval("AttachFileName")%>'
                            ForeColor="#ec5252" OnClick="ddl_DownloadFiles_ItemCommand">
                        </asp:LinkButton></ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <asp:HiddenField runat="server" ID="hdnSelectIndex" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnCustCode" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnOID" ClientIDMode="Static" Value="" />
    <script src="Scripts/datemonthyear.js" type="text/javascript"></script>
    <script src="Scripts/scotsdomestic.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            blinkOrderHead();
        });
        $('#imgdownload').click(function () {
            $("#dialog").dialog({ title: "Download PDF Files", modal: true, width: '600px', top: '1000px', left: '500px',
                buttons: [{ id: "Close", text: "Close", click: function () { $(this).dialog('close'); } }]
            });
            $("#dialog").dialog("open");
            return false;
        });
    </script>
</asp:Content>
