<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="ExpReviewMeeting.aspx.cs" Inherits="TTS.ExpReviewMeeting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
    <link href="Styles/datemonthyear.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/cotsScript.js" type="text/javascript"></script>
    <style type="text/css">
        .cssHead
        {
            text-align: right;
            color: #181cb1;
            font-weight: bold;
            font-size: 14px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="pageTitleHead">
        <asp:Label runat="server" ID="lblPageHead" ClientIDMode="Static" Text=""></asp:Label>
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="width: 100%; border-color: #dcecfb;
            border-collapse: separate;">
            <tr>
                <td>
                    <table cellspacing="0" rules="all" border="1" style="width: 100%; border-color: #000000;
                        border-collapse: separate;">
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblReviewPlant" ClientIDMode="Static" Text="" Font-Bold="true"
                                    Font-Size="14px"></asp:Label>
                            </td>
                            <td>
                                WT KGS
                            </td>
                            <td>
                            </td>
                            <td>
                                EXP ACT PLAN
                            </td>
                            <td>
                            </td>
                            <td>
                                PLAN
                            </td>
                            <td>
                                ACT DISP
                            </td>
                            <td>
                                BALANCE
                            </td>
                            <td colspan="2">
                                BALANCE PROD
                            </td>
                        </tr>
                        <tr>
                            <td>
                                TOT BACKLOG
                                <!-- CURRENT MONTH + NEXT MONTH + DOMESTIC + CARRY FORWARD + FUTURE SHIPMENTS  -->
                            </td>
                            <td class="cssHead">
                                <asp:Label runat="server" ID="lblTotBacklog" ClientIDMode="Static" Text=""></asp:Label>
                            </td>
                            <td>
                                CARRY FORWARD
                            </td>
                            <td class="cssHead">
                                <asp:Label runat="server" ID="lblCarryForward" ClientIDMode="Static" Text=""></asp:Label>
                            </td>
                            <td>
                                EXP
                            </td>
                            <td class="cssHead">
                                <asp:Label runat="server" ID="lblExpPlan" ClientIDMode="Static" Text=""></asp:Label>
                            </td>
                            <td class="cssHead">
                                <asp:Label runat="server" ID="lblExpDisp" ClientIDMode="Static" Text=""></asp:Label>
                            </td>
                            <td class="cssHead">
                                <asp:Label runat="server" ID="lblExpBal" ClientIDMode="Static" Text=""></asp:Label>
                            </td>
                            <td class="cssHead">
                                <asp:Label runat="server" ID="lblCurrMonthDisp" ClientIDMode="Static" Text=""></asp:Label>
                            </td>
                            <td class="cssHead">
                                <asp:Label runat="server" ID="lblTotBacklogBalance" ClientIDMode="Static" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                SHIPABLE BACKLOG
                                <!-- CURRENT MONTH + NEXT MONTH + DOMESTIC -->
                            </td>
                            <td class="cssHead">
                                <asp:Label runat="server" ID="lblShipableBacklog" ClientIDMode="Static" Text=""></asp:Label>
                            </td>
                            <td>
                                CURRENT MONTH SHIPMENTS
                            </td>
                            <td class="cssHead">
                                <asp:Label runat="server" ID="lblCurrMonthShip" ClientIDMode="Static" Text=""></asp:Label>
                            </td>
                            <td>
                                DOM
                            </td>
                            <td class="cssHead">
                                <asp:Label runat="server" ID="lblDomPlan" ClientIDMode="Static" Text=""></asp:Label>
                            </td>
                            <td class="cssHead">
                                <asp:Label runat="server" ID="lblDomDisp" ClientIDMode="Static" Text=""></asp:Label>
                            </td>
                            <td class="cssHead">
                                <asp:Label runat="server" ID="lblDomBalance" ClientIDMode="Static" Text=""></asp:Label>
                            </td>
                            <td class="cssHead">
                                <asp:Label runat="server" ID="lblCurrBalance" ClientIDMode="Static" Text=""></asp:Label>
                            </td>
                            <td class="cssHead">
                                <asp:Label runat="server" ID="lblTotBalance" ClientIDMode="Static" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                                TOTAL
                            </td>
                            <td class="cssHead">
                                <asp:Label runat="server" ID="lblExpPlanTot" ClientIDMode="Static" Text=""></asp:Label>
                            </td>
                            <td>
                                TOTAL SHIPMENT
                            </td>
                            <td class="cssHead">
                                <asp:Label runat="server" ID="lblPlanTot" ClientIDMode="Static" Text=""></asp:Label>
                            </td>
                            <td class="cssHead">
                                <asp:Label runat="server" ID="lblPlanDisp" ClientIDMode="Static" Text=""></asp:Label>
                            </td>
                            <td class="cssHead">
                                <asp:Label runat="server" ID="lblPlanBalance" ClientIDMode="Static" Text=""></asp:Label>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView runat="server" ID="gvTrackOrderList" AutoGenerateColumns="false" Width="100%"
                        CssClass="gridcss" RowStyle-Height="25px" OnRowDataBound="gvTrackOrderList_RowDataBound">
                        <Columns>
                            <asp:BoundField HeaderText="CUSTOMER" DataField="custfullname" />
                            <asp:BoundField HeaderText="WORK ORDER" DataField="workorderno" />
                            <asp:BoundField HeaderText="ORDER ON" DataField="CompletedDate" ItemStyle-Width="70px" />
                            <asp:TemplateField HeaderText="WO REVISION" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="80px">
                                <ItemTemplate>
                                    <%# Eval("wo_revision") + " / " +Eval("wo_date") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="CUSTOEMR DESIRED ON" DataField="DesiredShipDate" ItemStyle-Width="70px" />
                            <asp:BoundField HeaderText="PPC RFD" DataField="RFD_Date" ItemStyle-Width="70px" />
                            <asp:BoundField HeaderText="QTY" DataField="itemqty" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField HeaderText="WT" DataField="fwt" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField HeaderText="STOCK" DataField="StockQty" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField HeaderText="SHIPMENT TYPE" DataField="ShipmentType" />
                            <asp:BoundField HeaderText="PLANT" DataField="Plant" />
                            <asp:TemplateField HeaderText="ACTION" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hdnCustCode" Value='<%# Eval("CustCode") %>' />
                                    <asp:HiddenField runat="server" ID="hdnOID" Value='<%# Eval("ID") %>' />
                                    <asp:LinkButton ID="lnkTrackBtn" runat="server" Text="VIEW" OnClick="lnkTrackBtn_click" /></span>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="div_SingleView" style="display: none;">
                        <table cellspacing="0" rules="all" border="1" style="background-color: #dcecfb; width: 100%;
                            border-color: White; border-collapse: separate;">
                            <tr style="width: 100%; background-color: #056442; font-weight: bold; color: #fff;
                                font-size: 15px; text-align: center;">
                                <td>
                                    <asp:Label runat="server" ID="lblCustName" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblWorkorder" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView runat="server" ID="gv_OrderSumValue" AutoGenerateColumns="true" Width="100%"
                                        HeaderStyle-BackColor="#166502" HeaderStyle-ForeColor="#ffffff" ShowFooter="true"
                                        FooterStyle-BackColor="#dfe0f3" RowStyle-HorizontalAlign="Right" RowStyle-VerticalAlign="Middle"
                                        FooterStyle-HorizontalAlign="Right">
                                    </asp:GridView>
                                </td>
                                <td>
                                    <asp:ImageButton ID="imgdownload" ImageUrl="images/Download.png" ClientIDMode="Static"
                                        runat="server" CssClass="imageCss" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label runat="server" ID="lblStatusHistory" CssClass="spanCss" ClientIDMode="Static"
                                        Text="" Font-Bold="true"></asp:Label>
                                    <asp:GridView runat="server" ID="gvStatusHistory" AutoGenerateColumns="false" Width="100%"
                                        CssClass="gridcss" RowStyle-Height="25px">
                                        <Columns>
                                            <asp:BoundField DataField="StatusText" HeaderText="STATUS" ItemStyle-Width="100px" />
                                            <asp:BoundField DataField="feedback" HeaderText="COMMENTS" ItemStyle-Width="220px" />
                                            <asp:BoundField DataField="statusdate" HeaderText="DATE" ItemStyle-Width="80px" />
                                            <asp:BoundField DataField="username" HeaderText="CHANGED BY" ItemStyle-Width="100px" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label runat="server" ID="lblReviewStatus" CssClass="spanCss" ClientIDMode="Static"
                                        Text="" Font-Bold="true"></asp:Label>
                                    <asp:GridView runat="server" ID="gvReviewHistory" AutoGenerateColumns="false" Width="100%"
                                        CssClass="gridcss" RowStyle-Height="25px">
                                        <Columns>
                                            <asp:BoundField DataField="ProcessType" HeaderText="FEEDBACK" />
                                            <asp:BoundField DataField="Comments" HeaderText="COMMENTS" />
                                            <asp:BoundField DataField="Createdate" HeaderText="REVIEW ON" />
                                            <asp:BoundField DataField="Username" HeaderText="REVIEW BY" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <%--<tr>
                                <td>
                                    <span class="spanCss">WORKORDER STATUS</span>
                                    <br />
                                    <asp:RadioButtonList runat="server" ID="rdbstatus" RepeatDirection="Vertical" RepeatColumns="2"
                                        RepeatLayout="Table">
                                        <asp:ListItem Text="CUSTOMER'S REVIEW" Value="CUSTOMER"></asp:ListItem>
                                        <asp:ListItem Text="CRM ALLOCATION" Value="CRM"></asp:ListItem>
                                        <asp:ListItem Text="PPC HOLD" Value="PPC"></asp:ListItem>
                                        <asp:ListItem Text="PDI HOLD" Value="PDI"></asp:ListItem>
                                        <asp:ListItem Text="FINANCE HOLD" Value="FINANCE"></asp:ListItem>
                                        <asp:ListItem Text="OTHERS" Value="OTHERS"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td>
                                    <span class="spanCss">COMMENTS</span>
                                    <br />
                                    <asp:TextBox runat="server" ID="txtCommt" Text="" TextMode="MultiLine" Width="800px"
                                        Height="60px" Enabled="true" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    <asp:Button ID="btn_ClearRecords" runat="server" Text="CLEAR" CssClass="btn btn-info"
                                        ClientIDMode="Static" />
                                </td>
                                <td>
                                    <asp:Label ID="lblErrMsg1" runat="server" ClientIDMode="Static" ForeColor="Red" Font-Bold="true"></asp:Label>
                                    <asp:Button runat="server" ID="btnSave" ClientIDMode="Static" Text="SAVE" CssClass="btn btn-success"
                                        OnClientClick="javascript:return CntrlSave();" OnClick="btn_Save_click" />
                                </td>
                            </tr>--%>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
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
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnO_ID" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdn_Custcode" ClientIDMode="Static" Value="" />
    <script src="Scripts/datemonthyear.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#imgdownload').click(function () {
                $("#dialog").dialog({ title: "Download PDF Files", modal: true, width: '600px', top: '1000px', left: '500px',
                    buttons: [{ id: "Close", text: "Close", click: function () { $(this).dialog('close'); } }]
                });
                $("#dialog").dialog("open");
                return false;
            });
            $('#btn_ClearRecords').click(function () {
                window.location.href = window.location.href;
                return false;
            });
        });
        function CntrlSave() {
            if (!$.trim($('#MainContent_txtCommt').val())) {
                $('#lblErrMsg1').html('PLEASE ENTER YOUR COMMENT TO SAVE');
                $('#MainContent_txtCommt').focus();
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
