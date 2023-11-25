<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="exportcotstrackstatus.aspx.cs" Inherits="TTS.exportcotstrackstatus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="Scripts/cotsScript.js" type="text/javascript"></script>
    <link href="Styles/datemonthyear.css" rel="stylesheet" type="text/css" />
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        TRACK EXPORT ORDER CURRENT STATUS</div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="background-color: #ecf6ff; border-color: White;
            border-collapse: separate; width: 100%;">
            <tr>
                <th>
                    PLANT
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="ddl_Plant" ClientIDMode="Static" CssClass="form-control"
                        AutoPostBack="true" OnSelectedIndexChanged="ddl_Plant_IndexChange" Width="80px">
                    </asp:DropDownList>
                </td>
                <th>
                    CUSTOMER
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="ddl_Customer" ClientIDMode="Static" CssClass="form-control"
                        AutoPostBack="true" OnSelectedIndexChanged="ddl_Customer_IndexChange" Width="450px">
                    </asp:DropDownList>
                </td>
                <th>
                    WORK ORDER
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="ddl_WorkOrder" ClientIDMode="Static" CssClass="form-control"
                        AutoPostBack="true" OnSelectedIndexChanged="ddl_WorkOrder_IndexChange">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="6">
                    <asp:GridView runat="server" ID="gvTrackOrderList" AutoGenerateColumns="false" Width="100%"
                        HeaderStyle-BackColor="#FFCC00" RowStyle-Height="24px" 
                        onselectedindexchanged="gvTrackOrderList_SelectedIndexChanged">
                        <Columns>
                            <asp:TemplateField HeaderText="CUSTOMER NAME" ItemStyle-Width="250px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatusCustName" Text='<%# Eval("custfullname") %>'></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdnStatusCustCode" Value='<%# Eval("CustCode") %>' />
                                    <asp:HiddenField runat="server" ID="hdnUserCurrency" Value='<%# Eval("usercurrency") %>' />
                                    <asp:HiddenField runat="server" ID="hdnOrderID" Value='<%# Eval("ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ORDER REFERENCE NO" ItemStyle-Width="200px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOrderRefNo" Text='<%#Eval("OrderRefNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="workorderno" HeaderText="WO NO" />
                            <asp:TemplateField HeaderText="ORDER DATE" ItemStyle-Width="65px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOrderDate" Text='<%#Eval("CompletedDate") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="CUSTOMER DESIRED SHIP DATE" ItemStyle-Width="65px" DataField="DesiredShipDate" />
                            <asp:BoundField HeaderText="PPC RFD DATE" ItemStyle-Width="70px" DataField="RFD_Date" />
                            <asp:BoundField HeaderText="QTY" ItemStyle-Width="40px" DataField="itemqty" />
                            <asp:BoundField DataField="ShipmentType" HeaderText="SHIPMENT TYPE" />
                            <asp:TemplateField HeaderText="PLANT">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblPlant" Text='<%# Eval("Plant") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="STATUS" ItemStyle-Width="220px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatusText" Text='<%# Eval("StatusText") %>'></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdnStatusID" Value='<%# Eval("OrderStatus") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="80px" HeaderText="ACTION" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkTrackBtn" runat="server" Text="Show Details" OnClick="lnkTrackBtn_Click" /></span>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="6">
                    <div style="width: 100%; float: left; border: 1px solid #000; display: none; line-height: 20px;
                        margin-top: 10px; background-color: #F0E2F5; padding-top: 5px;" id="divStatusChange">
                        <div id="divOrderHead" style="width: 1100px; float: left;">
                            <div style="width: 530px; float: left; text-align: left;">
                                <asp:Label runat="server" ID="lblCustName" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                            </div>
                            <div style="width: 530px; float: left; text-align: right;">
                                <asp:Label runat="server" ID="lblStausOrderRefNo" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                            </div>
                        </div>
                        <div style="width: 1100px; float: left;">
                            <asp:Label runat="server" ID="lblCurrStatus" ClientIDMode="Static" Text="" Font-Bold="true"
                                Font-Size="15px"></asp:Label>
                            <asp:DataList runat="server" ID="dlOrderMaster" RepeatColumns="1" RepeatDirection="Horizontal"
                                RepeatLayout="Table" AlternatingItemStyle-BackColor="#A1C9D3" ItemStyle-BackColor="#C9DAC9"
                                ItemStyle-VerticalAlign="Top" Width="100%">
                                <ItemTemplate>
                                    <table cellspacing="0" rules="all" border="1" style="background-color: #ecf6ff; border-color: White;
                                        border-collapse: separate; width: 100%;">
                                        <tr>
                                            <th>
                                                Orderd Date
                                            </th>
                                            <td>
                                                <%# Eval("CompletedDate")%>
                                            </td>
                                            <th>
                                                Delivery Method
                                            </th>
                                            <td>
                                                <%# Eval("DeliveryMethod")%>
                                                -
                                                <%# Eval("GodownName") %>
                                                -
                                                <%#((string)Eval("TransportDetails")).Replace("~", "<br/>")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                Desired Ship Date
                                            </th>
                                            <td>
                                                <%# Eval("DesiredShipDate")%>
                                            </td>
                                            <th>
                                                Packing Method
                                            </th>
                                            <td>
                                                <%# Eval("PackingMethod") %>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                Bill To
                                            </th>
                                            <td>
                                                <%# Bind_BillingAddress(Eval("BillID").ToString())%>
                                            </td>
                                            <th>
                                                Ship To
                                            </th>
                                            <td>
                                                <%# Bind_BillingAddress(Eval("ShipID").ToString())%>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:DataList>
                        </div>
                        <div style="width: 100%; float: left;">
                            <asp:GridView runat="server" ID="gvOrderItemList" AutoGenerateColumns="false" Width="100%"
                                AlternatingRowStyle-BackColor="#f5f5f5" ShowFooter="true">
                                <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" Height="22px" />
                                <Columns>
                                    <asp:TemplateField HeaderText="CATEGORY" ItemStyle-Width="100px">
                                        <ItemTemplate>
                                            <%# Eval("category") %>
                                            <%# Eval("AssyRimstatus").ToString() == "True" ? " (ASSY)" : ""%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="PLATFORM" DataField="config" ItemStyle-Width="100px" />
                                    <asp:BoundField HeaderText="TYRE SIZE" DataField="tyresize" ItemStyle-Width="150px" />
                                    <asp:BoundField HeaderText="RIM" DataField="rimsize" ItemStyle-Width="40px" />
                                    <asp:BoundField HeaderText="TYPE" DataField="tyretype" ItemStyle-Width="60px" />
                                    <asp:BoundField HeaderText="BRAND" DataField="brand" ItemStyle-Width="100px" />
                                    <asp:BoundField HeaderText="SIDEWALL" DataField="sidewall" ItemStyle-Width="100px" />
                                    <asp:BoundField HeaderText="QTY" DataField="itemqty" ItemStyle-HorizontalAlign="Right"
                                        ItemStyle-Width="40px" />
                                    <asp:BoundField HeaderText="BASIC PRICE" DataField="listprice" ItemStyle-HorizontalAlign="Right"
                                        ItemStyle-Width="100px" />
                                    <asp:BoundField HeaderText="FWT" DataField="tyrewt" ItemStyle-HorizontalAlign="Right"
                                        ItemStyle-Width="80px" />
                                    <asp:TemplateField HeaderText="RIM QTY" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="40px">
                                        <ItemTemplate>
                                            <%# Eval("Rimitemqty").ToString() == "0" ? "" : Eval("Rimitemqty").ToString()%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="RIM BASIC PRICE" ItemStyle-HorizontalAlign="Right"
                                        ItemStyle-Width="100px">
                                        <ItemTemplate>
                                            <%# Eval("Rimunitprice").ToString() == "0.00" ? "" : Eval("Rimunitprice").ToString()%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="RIM FWT" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="80px">
                                        <ItemTemplate>
                                            <%# Eval("Rimfinishedwt").ToString() == "0.00" ? "" : Eval("Rimfinishedwt").ToString()%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="TOTAL PRICE" DataField="unitprice" ItemStyle-HorizontalAlign="Right"
                                        ItemStyle-Width="120px" />
                                    <asp:BoundField HeaderText="TOTAL FWT" DataField="finishedwt" ItemStyle-HorizontalAlign="Right"
                                        ItemStyle-Width="100px" />
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div id="div_DownloadFiles" style="width: 100%; text-align: right;">
                            <table align="right">
                                <tr>
                                    <td class="spanCss">
                                        Download PDF Files
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="imgdownload" ImageUrl="images/Download.png" ClientIDMode="Static"
                                            runat="server" CssClass="imageCss" />
                                    </td>
                                    <td class="spanCss">
                                        Download Excel Files
                                    </td>
                                    <td>
                                        <asp:LinkButton runat="server" ID="lnkExpExcelFiles" ClientIDMode="Static" Text=""
                                            OnClick="lnkExpExcelFiles_click"></asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div style="width: 1063px; float: left;">
                            <div style="width: 530px; float: left;">
                                <span class="headCss">Special Instruction</span>
                                <asp:TextBox runat="server" ID="txtOrderSplIns" Text="" TextMode="MultiLine" Width="520px"
                                    Height="100px" Enabled="false"></asp:TextBox>
                            </div>
                            <div style="width: 530px; float: right;">
                                <span class="headCss">Special Notes</span>
                                <asp:TextBox runat="server" ID="txtOrdersplReq" Text="" TextMode="MultiLine" Width="520px"
                                    Height="100px" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                        <div style="width: 1063px; float: left; line-height: 15px;">
                            <div style="width: 530px; float: left;">
                                <asp:Label runat="server" ID="lblReviseHistory" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                                <asp:GridView runat="server" ID="gvRevisedHistory" AutoGenerateColumns="false" Width="520px"
                                    HeaderStyle-BackColor="#FFCC00" AlternatingRowStyle-BackColor="#f5f5f5" RowStyle-Height="25px">
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
                                <asp:Label runat="server" ID="lblStatusHistory" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                                <asp:GridView runat="server" ID="gvStatusHistory" AutoGenerateColumns="false" Width="520px"
                                    HeaderStyle-BackColor="#FFCC00" AlternatingRowStyle-BackColor="#f5f5f5" RowStyle-Height="25px">
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
    <asp:HiddenField runat="server" ID="hdnStatusid" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnSelectIndex" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnCustCode" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnCurrency" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnPlant" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnOID" ClientIDMode="Static" Value="" />
    <script src="Scripts/datemonthyear.js" type="text/javascript"></script>
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
