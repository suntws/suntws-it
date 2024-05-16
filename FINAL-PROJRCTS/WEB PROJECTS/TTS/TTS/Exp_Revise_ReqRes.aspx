<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Exp_Revise_ReqRes.aspx.cs"
    MasterPageFile="~/master.Master" Inherits="TTS.Exp_Revise_ReqRes" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server" ID="Content1">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/cotsScript.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        <asp:Label ID="lblPageHeading" runat="server"></asp:Label>
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="background-color: #dcecfb; width: 100%;
            border-color: White; border-collapse: separate;">
            <tr>
                <th class="spanCss">
                    Plant
                </th>
                <td>
                    <asp:DropDownList ID="ddl_PlantSelection" runat="server" CssClass="form-control"
                        AutoPostBack="true" OnSelectedIndexChanged="ddl_PlantSelection_SelectedIndexChanged">
                        <asp:ListItem Text="ALL" Value="ALL" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="MMN" Value="MMN"></asp:ListItem>
                        <asp:ListItem Text="SLTL" Value="SLTL"></asp:ListItem>
                        <asp:ListItem Text="SITL" Value="SITL"></asp:ListItem>
                        <asp:ListItem Text="PDK" Value="PDK"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:GridView runat="server" ID="gvOrderDetails" AutoGenerateColumns="false" HeaderStyle-BackColor="#efff9c"
                        RowStyle-Height="28px" Width="100%" 
                        onselectedindexchanged="gvOrderDetails_SelectedIndexChanged">
                        <Columns>
                            <asp:TemplateField HeaderText="CUSTOMER NAME">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatusCustName" Text='<%# Eval("custfullname") %>'></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdnStatusCustCode" Value='<%# Eval("CustCode") %>' />
                                    <asp:HiddenField runat="server" ID="hdnReviseRequestTo" Value='<%# Eval("ReviseRequestTo") %>' />
                                    <asp:HiddenField runat="server" ID="hdnOrderStatus" Value='<%# Eval("OrderStatus") %>' />
                                    <asp:HiddenField runat="server" ID="hdnOrderID" Value='<%# Eval("ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ORDER REF NO.">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOrderRefNo" Text='<%#Eval("OrderRefNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="WORK ORDER NO" DataField="workorderno" />
                            <asp:BoundField HeaderText="ORDER DATE" ItemStyle-Width="65px" DataField="CompletedDate" />
                            <asp:BoundField HeaderText="DESIRED SHIP DATE" ItemStyle-Width="65px" DataField="DesiredShipDate" />
                            <asp:BoundField HeaderText="QTY" DataField="itemqty" ItemStyle-HorizontalAlign="Right" />
                            <asp:TemplateField HeaderText="PLANT">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblPlant" Text='<%#Eval("plant") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="SHIPMENT TYPE" DataField="shipmenttype" />
                            <asp:TemplateField HeaderText="STATUS">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatusText" Text='<%# Eval("StatusText") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ACTION">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkReqResOrder" runat="server" Text="PROCESS" CssClass="btn btn-success btn-xs"
                                        OnClick="lnkReqResOrder_Click" Style="text-decoration: none;" /></span>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr style="background-color: #3c763d; height: 22px; font-size: 18px; color: #ffffff;
                text-align: center;">
                <td>
                    <asp:Label ID="lblSelectedCustomerName" runat="server" Text="" CssClass="lblCss"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblSelectedOrderRefNo" ClientIDMode="Static" runat="server" Text=""
                        CssClass="lblCss"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div id="div_sub" style="display: none;">
                        <div style="width: 100%;">
                            <asp:GridView runat="server" ID="gvOrderItemList" AutoGenerateColumns="false" ShowFooter="true"
                                FooterStyle-Font-Bold="true" Width="100%">
                                <Columns>
                                    <asp:TemplateField HeaderText="CATEGORY" ItemStyle-Width="100px">
                                        <ItemTemplate>
                                            <%# Eval("category") %>
                                            <%# Eval("AssyRimstatus").ToString() == "True" ? " (ASSY)" : ""%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Config" HeaderText="PLATFORM" />
                                    <asp:BoundField DataField="brand" HeaderText="BRAND" />
                                    <asp:BoundField DataField="sidewall" HeaderText="SIDEWALL" />
                                    <asp:BoundField DataField="tyretype" HeaderText="TYPE" />
                                    <asp:BoundField DataField="tyresize" HeaderText="TYRE SIZE" />
                                    <asp:BoundField DataField="rimsize" HeaderText="RIM" />
                                    <asp:BoundField DataField="listprice" HeaderText="TYRE PRICE" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="itemqty" HeaderText="QTY" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="unitpricepdf" HeaderText="TOT TYRE PRICE" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="totalfwt" HeaderText="FWT" ItemStyle-HorizontalAlign="Right" />
                                    <asp:TemplateField HeaderText="RIM PRICE" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <%# Eval("Rimunitprice").ToString() == "0.00" ? "" : Eval("Rimunitprice").ToString()%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="RIM QTY" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <%# Eval("Rimitemqty").ToString() == "0" ? "" : Eval("Rimitemqty").ToString()%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="TOT RIM PRICE" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <%# Eval("Rimpricepdf").ToString() == "0.00" ? "" : Eval("Rimpricepdf").ToString()%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="RIM WT" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <%# Eval("totalRimWt").ToString() == "0.00" ? "" : Eval("totalRimWt").ToString()%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="PLANT" DataField="ItemPlant" ItemStyle-Width="100px" />
                                </Columns>
                                <FooterStyle BackColor="#b0ceb0" HorizontalAlign="Right" />
                            </asp:GridView>
                        </div>
                        <div id="div_Req" style="display: none;">
                            <table style="width: 100%;">
                                <tr>
                                    <th class="spanCss">
                                        Request Purpose
                                    </th>
                                    <td>
                                        <asp:RadioButtonList runat="server" ID="rdo_RequstCrm" ClientIDMode="Static" Font-Bold="true"
                                            Width="100%" RepeatColumns="2" RepeatDirection="Horizontal" AutoPostBack="true"
                                            OnSelectedIndexChanged="rdo_RequstCrm_IndexChange">
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <th class="spanCss">
                                        Request Comments
                                    </th>
                                    <td style="vertical-align: top;">
                                        <asp:TextBox ID="txt_ReqComments_New" runat="server" TextMode="MultiLine" ClientIDMode="Static"
                                            CssClass="form-control" Height="80px" Width="98%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: left; font-size: 16px; font-family: Times New Roman;">
                                        <div class="alert alert-info">
                                            <strong style="color: #fe0c44;">Info! &nbsp;</strong><asp:Label runat="server" ID="lblApproveTeam"></asp:Label>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="div_Res" style="display: none;">
                            <table style="width: 100%;">
                                <tr>
                                    <td style="width: 50%;">
                                        <span class="spanCss">Request Comments: </span>
                                        <asp:TextBox ID="txt_ReqComments_Exist" runat="server" TextMode="MultiLine" Enabled="false"
                                            ClientIDMode="Static" CssClass="form-control" Height="80px" Width="98%"></asp:TextBox>
                                    </td>
                                    <td style="width: 50%;">
                                        <span class="spanCss">Response Comments: </span>
                                        <asp:TextBox ID="txt_ResComments" runat="server" TextMode="MultiLine" ClientIDMode="Static"
                                            CssClass="form-control" Height="80px" Width="98%"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <hr />
                        <div id="div_Cntrls" style="text-align: center;">
                            <asp:Button ID="btn_ReqRes" ClientIDMode="Static" runat="server" Text="" CssClass="btn btn-success"
                                OnClick="btn_ReqRes_Click" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btn_ClearRecords" runat="server" Text="Clear Selection" CssClass="btn btn-info"
                                ClientIDMode="Static" />
                        </div>
                        <hr />
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField ID="hdnType" runat="server" ClientIDMode="Static" Value="" />
    <asp:HiddenField ID="hdnApproveBy_Res" runat="server" Value="" />
    <asp:HiddenField ID="hdnApprovalID" runat="server" Value="" />
    <asp:HiddenField ID="hdnApprovalTeam" runat="server" Value="" />
    <asp:HiddenField ID="hdnCustCode" runat="server" Value="" />
    <asp:HiddenField ID="hdnResponseID" runat="server" Value="" />
    <asp:HiddenField ID="hdnPlant" runat="server" Value="" />
    <asp:HiddenField ID="hdnOrderStatusID" runat="server" Value="" />
    <asp:HiddenField ID="hdnOID" runat="server" Value="" />
    <script type="text/javascript">
        $(function () {
            $('#btn_ReqRes').click(function () {
                var errMsg = "";
                if ($('#hdnType').val() == "request") {
                    if ($('input:radio[id*=rdo_RequstCrm]:checked').length == 0)
                        errMsg += "Choose Request Purpose \n";
                    if ($('#txt_ReqComments_New').val() == "")
                        errMsg += "Enter Request Comments \n";
                }
                else if ($('#hdnType').val() == "response" && $('#txt_ResComments').val() == "")
                    errMsg += "Enter Response Comments \n";
                if (errMsg.length > 0) {
                    alert(errMsg);
                    return false;
                }
                else
                    return true;
            });

            $('#btn_ClearRecords').click(function () {
                window.location.href = window.location.href;
                return false;
            });
        });
        function ShowDiv(type) {
            $('#div_Req').hide();
            $('#div_Res').hide();
            $('#div_sub').hide();
            if (type != "" && type == "request") {
                $('#div_Req').show();
                $('#btn_ReqRes').val('Request Send');
            }
            else if (type != "" && type == "response") {
                $('#div_Res').show();
                $('#btn_ReqRes').val('Response Send');
            }
            gotoPreviewDiv('div_sub');
        }
    </script>
</asp:Content>
