<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Exp_OrderModifyPlantQty.aspx.cs"
    MasterPageFile="~/master.Master" Inherits="TTS.Exp_OrderModifyPlantQty" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/cotsScript.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        EXPORT ORDER -> REVISE PLANT QTY
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="background-color: #dcecfb; width: 100%;
            border-color: White; border-collapse: separate;">
            <tr>
                <td>
                    <asp:GridView runat="server" ID="gvorderlist" AutoGenerateColumns="false" Width="100%"
                        AlternatingRowStyle-BackColor="#d6eafb" RowStyle-Height="30px" 
                        onselectedindexchanged="gvorderlist_SelectedIndexChanged">
                        <HeaderStyle BackColor="#74bbf9" ForeColor="White" Font-Bold="true" Height="25px"
                            HorizontalAlign="Center" />
                        <Columns>
                            <asp:TemplateField HeaderText="CUSTOMER NAME" ItemStyle-Width="300px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatusCustName" Text='<%# Eval("custfullname") %>'></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdnStatusCustCode" Value='<%# Eval("CustCode") %>' />
                                    <asp:HiddenField runat="server" ID="hdnOrderID" Value='<%# Eval("ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ORDER REF NO." ItemStyle-Width="250px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOrderRefNo" Text='<%#Eval("OrderRefNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ORDER DATE" ItemStyle-Width="70px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOrderDate" Text='<%#Eval("CompletedDate") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="REVISED DATE" DataField="RevisedDate" ItemStyle-Width="70px" />
                            <asp:BoundField DataField="ShipmentType" HeaderText="SHIPMENT TYPE" />
                            <asp:BoundField HeaderText="QTY" DataField="itemqty" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Right" />
                            <asp:TemplateField HeaderText="ACTION" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkPlantAssign" runat="server" Text="REASSIGN QTY" CssClass="btn btn-success"
                                        OnClick="lnkPlantAssign_Click"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <hr />
                </td>
            </tr>
            <tr>
                <td>
                    <div id="div_Subgv" style="display: none;">
                        <div class="exporderhead">
                            <div style="float: left;">
                                <asp:Label runat="server" ID="lblCustName" ClientIDMode="Static" Text=""></asp:Label>
                            </div>
                            <div style="float: right;">
                                <asp:Label runat="server" ID="lblStausOrderRefNo" ClientIDMode="Static" Text=""></asp:Label>
                            </div>
                        </div>
                        <div>
                            <asp:GridView runat="server" ID="gvPlantAssignList" AutoGenerateColumns="false" Width="100%"
                                RowStyle-Height="22px" CssClass="gridcss">
                                <Columns>
                                    <asp:TemplateField HeaderText="SLNO." ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSRNO" runat="server" Text='<%#Container.DataItemIndex+1 %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="CATEGORY" ItemStyle-Width="90px">
                                        <ItemTemplate>
                                            <%# Eval("category") %>
                                            <asp:Label runat="server" ID="lblAssyStatus" Text='<%# Eval("AssyRimstatus").ToString() == "True" ? " (ASSY)" : ""%>'></asp:Label>
                                            <asp:Label runat="server" ID="lblProcessid" Text='<%#Eval("processid") %>' CssClass="headerNone"></asp:Label>
                                            <asp:Label runat="server" ID="lblEdcNo" Text='<%#Eval("EdcNo") %>' CssClass="headerNone"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="PLATFORM" DataField="config" ItemStyle-Width="90px" />
                                    <asp:BoundField HeaderText="TYRE SIZE" DataField="tyresize" ItemStyle-Width="140px" />
                                    <asp:BoundField HeaderText="RIM" DataField="rimsize" ItemStyle-Width="40px" />
                                    <asp:BoundField HeaderText="TYPE" DataField="tyretype" ItemStyle-Width="50px" />
                                    <asp:BoundField HeaderText="BRAND" DataField="brand" ItemStyle-Width="100px" />
                                    <asp:BoundField HeaderText="SIDEWALL" DataField="sidewall" ItemStyle-Width="100px" />
                                    <asp:TemplateField HeaderText="QTY" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="40px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblItemQty" Text='<%# Eval("itemqty") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="MMN QTY" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="40px">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtqty_mmn" CssClass="form-control" Width="40px"
                                                Text='<%#Eval("MMN_TYRE") %>'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SLTL QTY" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="40px">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtqty_sltl" CssClass="form-control" Width="40px"
                                                Text='<%#Eval("SLTL_TYRE") %>'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SITL QTY" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="40px">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtqty_sitl" CssClass="form-control" Width="40px"
                                                Text='<%#Eval("SITL_TYRE") %>'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="PDK QTY" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="40px">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtqty_pdk" CssClass="form-control" Width="40px"
                                                Text='<%#Eval("PDK_TYRE") %>'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="RIM QTY" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="40px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblRimItemqty" Text='<%# Eval("Rimitemqty") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="MMN RIM QTY" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="40px">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtrimqty_mmn" CssClass="form-control" Width="40px"
                                                Visible='<%# Eval("Rimitemqty").ToString() == "0" ? false : true%>' Text='<%#Eval("MMN_RIM") %>'
                                                Enabled="false"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SLTL RIM QTY" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="40px">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtrimqty_sltl" CssClass="form-control" Width="40px"
                                                Visible='<%# Eval("Rimitemqty").ToString() == "0" ? false : true%>' Text='<%#Eval("SLTL_RIM") %>'
                                                Enabled="false"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SITL RIM QTY" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="40px">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtrimqty_sitl" CssClass="form-control" Width="40px"
                                                Visible='<%# Eval("Rimitemqty").ToString() == "0" ? false : true%>' Text='<%#Eval("SITL_RIM") %>'
                                                Enabled="false"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="PDK RIM QTY" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="40px">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtrimqty_pdk" CssClass="form-control" Width="40px"
                                                Visible='<%# Eval("Rimitemqty").ToString() == "0" ? false : true%>' Text='<%#Eval("PDK_RIM") %>'
                                                Enabled="false"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div>
                            <asp:Label ID="lblErrmsg" runat="server" ForeColor="Red" ClientIDMode="Static"></asp:Label>
                        </div>
                        <div style="text-align: center;">
                            <asp:Button ID="btnPlantQtyReAssign" runat="server" Text="SAVE PLANT QTY" ClientIDMode="Static"
                                CssClass="btn btn-success" Font-Bold="true" OnClientClick="javascript:return chkPlantAssignItem();"
                                OnClick="btnPlantQtyReAssign_Click" />
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnCustCode" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnOID" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        $(function () {
            $('input:text').blur(function (e) {
                var id1 = this.id;
                if ($(this).val() == '')
                    $(this).val('0');
                $('#' + id1.replace('_txtqty_', '_txtrimqty_')).val($(this).val());
            });
        });
        function chkPlantAssignItem() {
            var errmsg = '';
            $('#lblErrmsg').html('');
            $("#MainContent_gvPlantAssignList tr").each(function (e) {
                var itemqty = $("#MainContent_gvPlantAssignList_lblItemQty_" + e).text();
                var MmnQty = $("#MainContent_gvPlantAssignList_txtqty_mmn_" + e).val() != '' ? $("#MainContent_gvPlantAssignList_txtqty_mmn_" + e).val() : '0';
                var SltlQty = $("#MainContent_gvPlantAssignList_txtqty_sltl_" + e).val() != '' ? $("#MainContent_gvPlantAssignList_txtqty_sltl_" + e).val() : '0';
                var SitlQty = $("#MainContent_gvPlantAssignList_txtqty_sitl_" + e).val() != '' ? $("#MainContent_gvPlantAssignList_txtqty_sitl_" + e).val() : '0';
                var PdkQty = $("#MainContent_gvPlantAssignList_txtqty_pdk_" + e).val() != '' ? $("#MainContent_gvPlantAssignList_txtqty_pdk_" + e).val() : '0';
                if (itemqty != "" && itemqty != (parseInt(MmnQty) + parseInt(SltlQty) + parseInt(SitlQty) + parseInt(PdkQty)))
                    errmsg += 'Enter proper tyre qty in row ' + $("#MainContent_gvPlantAssignList_lblSRNO_" + e).text() + '<br/>';
            });
            if (errmsg.length > 0) {
                $('#lblErrmsg').html(errmsg);
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
