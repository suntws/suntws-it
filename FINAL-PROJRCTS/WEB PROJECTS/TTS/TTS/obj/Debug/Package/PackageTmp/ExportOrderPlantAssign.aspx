<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="ExportOrderPlantAssign.aspx.cs" Inherits="TTS.ExportOrderPlantAssign" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/cotsScript.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        PLANT ASSIGN FOR EXPORT ORDER
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <asp:GridView runat="server" ID="gvorderlist" AutoGenerateColumns="false" Width="1106px"
            AlternatingRowStyle-BackColor="#d6eafb" RowStyle-Height="30px">
            <HeaderStyle BackColor="#74bbf9" ForeColor="White" Font-Bold="true" Height="25px"
                HorizontalAlign="Center" />
            <Columns>
                <asp:TemplateField HeaderText="CUSTOMER">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblStatusCustName" Text='<%# Eval("custfullname") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ORDER REF NO.">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblOrderRefNo" Text='<%#Eval("OrderRefNo") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ORDER DATE">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblOrderDate" Text='<%#Eval("CompletedDate") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="REVISED DATE" DataField="RevisedDate" />
                <asp:BoundField HeaderText="CUSTOMER DESIRED SHIP DATE" DataField="DesiredShipDate" />
                <asp:BoundField HeaderText="QTY" DataField="itemqty" ItemStyle-HorizontalAlign="Right" />
                <asp:TemplateField HeaderText="ACTION" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:HiddenField runat="server" ID="hdnStatusCustCode" Value='<%# Eval("CustCode") %>' />
                        <asp:HiddenField runat="server" ID="hdnOID" Value='<%# Eval("OID") %>' />
                        <asp:LinkButton ID="lnkPlantAssign" runat="server" Text="Assign Plant" OnClick="lnkPlantAssign_Click"
                            CssClass="btn btn-success"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <hr />
        <div id="div_Subgv" style="display: none;">
            <div class="exporderhead">
                <div style="float: left;">
                    <asp:Label runat="server" ID="lblCustName" ClientIDMode="Static" Text=""></asp:Label>
                </div>
                <div style="float: right;">
                    <asp:Label runat="server" ID="lblStausOrderRefNo" ClientIDMode="Static" Text=""></asp:Label>
                </div>
            </div>
            <table cellspacing="0" rules="all" border="1" style="background-color: #ecf6ff; border-color: White;
                border-collapse: separate; width: 100%;">
                <tr>
                    <td>
                        <div style="float: left; width: 100px; line-height: 30px; color: #825d0c;">
                            SHIPMENT TYPE
                        </div>
                        <asp:RadioButtonList runat="server" ID="rdbShipmentType" ClientIDMode="Static" RepeatColumns="2"
                            RepeatDirection="Horizontal" Width="140px" AutoPostBack="true" OnSelectedIndexChanged="rdbShipmentType_IndexChange">
                            <asp:ListItem Text="COMBI" Value="COMBI"></asp:ListItem>
                            <asp:ListItem Text="DIRECT" Value="DIRECT"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td style="font-weight: bold; font-size: 14px; line-height: 30px; text-align: center;
                        color: #ffffff; background-color: #020e48;">
                        STOCK COLUMN FORMAT: EXACT/ REBRAND/ UPGRADE
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:GridView runat="server" ID="gvPlantAssignList" AutoGenerateColumns="false" Width="100%"
                            AlternatingRowStyle-BackColor="#d6eafb" RowStyle-Height="22px">
                            <HeaderStyle BackColor="#047302" ForeColor="White" Font-Bold="true" Height="25px"
                                HorizontalAlign="Center" />
                            <Columns>
                                <asp:TemplateField HeaderText="SLNO." ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSRNO" runat="server" Text='<%#Container.DataItemIndex+1 %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="CATEGORY">
                                    <ItemTemplate>
                                        <%# Eval("category") %>
                                        <asp:Label runat="server" ID="lblAssyStatus" Text='<%# Eval("AssyRimstatus").ToString() == "True" ? " (ASSY)" : ""%>'></asp:Label>
                                        <asp:Label runat="server" ID="lblProcessid" Text='<%#Eval("processid") %>' CssClass="headerNone"></asp:Label>
                                        <asp:Label runat="server" ID="lblEdcNo" Text='<%# Eval("EdcNo") %>' CssClass="headerNone"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="PLATFORM" DataField="config" />
                                <asp:BoundField HeaderText="TYRE SIZE" DataField="tyresize" ItemStyle-Width="150px" />
                                <asp:BoundField HeaderText="RIM" DataField="rimsize" />
                                <asp:BoundField HeaderText="TYPE" DataField="tyretype" />
                                <asp:BoundField HeaderText="BRAND" DataField="brand" />
                                <asp:BoundField HeaderText="SIDEWALL" DataField="sidewall" />
                                <asp:TemplateField HeaderText="QTY" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblItemQty" Text='<%# Eval("itemqty") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="MMN" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtqty_mmn" CssClass="form-control" Width="40px"
                                            Text="0"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="MMN STOCK" ItemStyle-HorizontalAlign="Right" ItemStyle-Font-Bold="true">
                                    <ItemTemplate>
                                        <%# Eval("MMN_Stock").ToString()!=""?Eval("MMN_Stock").ToString():"0" %>/
                                        <%# Eval("MMN_RebrandStock").ToString() != "" ? Eval("MMN_RebrandStock").ToString() : "0"%>/
                                        <%# Eval("MMN_UpGradeStock").ToString() != "" ? Eval("MMN_UpGradeStock").ToString() : "0"%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="PDK" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtqty_pdk" CssClass="form-control" Width="40px"
                                            Text="0"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="PDK STOCK" ItemStyle-HorizontalAlign="Right" ItemStyle-Font-Bold="true">
                                    <ItemTemplate>
                                        <%# Eval("PDK_Stock").ToString()!=""?Eval("PDK_Stock").ToString():"0" %>/
                                        <%# Eval("PDK_RebrandStock").ToString() != "" ? Eval("PDK_RebrandStock").ToString() : "0"%>/
                                        <%# Eval("PDK_UpGradeStock").ToString() != "" ? Eval("PDK_UpGradeStock").ToString() : "0"%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="SLTL" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtqty_sltl" CssClass="form-control" Width="40px"
                                            Text="0"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="SLTL STOCK" ItemStyle-HorizontalAlign="Right" ItemStyle-Font-Bold="true">
                                    <ItemTemplate>
                                        <%# Eval("SLTL_Stock").ToString()!=""?Eval("SLTL_Stock").ToString():"0" %>/
                                        <%# Eval("SLTL_RebrandStock").ToString() != "" ? Eval("SLTL_RebrandStock").ToString() : "0"%>/
                                        <%# Eval("SLTL_UpGradeStock").ToString() != "" ? Eval("SLTL_UpGradeStock").ToString() : "0"%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="SITL" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtqty_sitl" CssClass="form-control" Width="40px"
                                            Text="0"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="SITL STOCK" ItemStyle-HorizontalAlign="Right" ItemStyle-Font-Bold="true">
                                    <ItemTemplate>
                                        <%# Eval("SITL_Stock").ToString()!=""?Eval("SITL_Stock").ToString():"0" %>/
                                        <%# Eval("SITL_RebrandStock").ToString() != "" ? Eval("SITL_RebrandStock").ToString() : "0"%>/
                                        <%# Eval("SITL_UpGradeStock").ToString() != "" ? Eval("SITL_UpGradeStock").ToString() : "0"%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="RIM QTY" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblRimItemqty" Text='<%# Eval("Rimitemqty").ToString() == "0" ? "" : Eval("Rimitemqty").ToString()%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="MMN RIM QTY" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtrimqty_mmn" CssClass="form-control" Width="40px"
                                            Visible='<%# Eval("Rimitemqty").ToString() == "0" ? false : true%>' Text="0"
                                            Enabled="false"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="PDK RIM QTY" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtrimqty_pdk" CssClass="form-control" Width="40px"
                                            Visible='<%# Eval("Rimitemqty").ToString() == "0" ? false : true%>' Text="0"
                                            Enabled="false"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="SLTL RIM QTY" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtrimqty_sltl" CssClass="form-control" Width="40px"
                                            Visible='<%# Eval("Rimitemqty").ToString() == "0" ? false : true%>' Text="0"
                                            Enabled="false"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="SITL RIM QTY" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtrimqty_sitl" CssClass="form-control" Width="40px"
                                            Visible='<%# Eval("Rimitemqty").ToString() == "0" ? false : true%>' Text="0"
                                            Enabled="false"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr style="display: none;" id="tr_Plantselctn">
                    <td>
                        <asp:Label runat="server" ID="lblText" ClientIDMode="Static" Text="" Font-Bold="true"
                            ForeColor="#614126" Font-Italic="true"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlPlantList" CssClass="form-control" ClientIDMode="Static"
                            Width="250px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnOrderPlantAssign" runat="server" Text="SAVE ORDER PLANT" ClientIDMode="Static"
                            CssClass="btn btn-success" Font-Bold="true" OnClientClick="javascript:return chkPlantAssignItem();"
                            OnClick="btnOrderPlantAssign_Click" />
                    </td>
                    <td>
                        <asp:Label ID="lblErrmsg" runat="server" ForeColor="Red" ClientIDMode="Static"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdnCustCode" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnStatus" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnOrderID" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        $(function () {
            $('input:text').blur(function (e) {
                var id1 = this.id;
                if ($(this).val() == '')
                    $(this).val('0');
                if ($('#hdnStatus').val() == 'ASSY')
                    $('#' + id1.replace('_txtqty_', '_txtrimqty_')).val($(this).val());
            });
        });
        function chkPlantAssignItem() {
            var errmsg = '';
            $('#lblErrmsg').html('');
            if ($("input:radio[id*=rdbShipmentType_]:checked").length == 0)
                errmsg += 'Choose shipment type<br/>';
            else if ($("input:radio[id*=rdbShipmentType_]:checked").val() == 'DIRECT') {
                if ($("#ddlPlantList option:selected").text() == "CHOOSE")
                    errmsg += 'Choose order plant<br/>';
            }
            else if ($("input:radio[id*=rdbShipmentType_]:checked").val() == 'COMBI') {
                var mmnTot = 0, pdkTot = 0, sltlTot = 0, sitlTot = 0;
                $("#MainContent_gvPlantAssignList tr").each(function (e) {
                    var itemqty = $("#MainContent_gvPlantAssignList_lblItemQty_" + e).text();
                    var MmnQty = $("#MainContent_gvPlantAssignList_txtqty_mmn_" + e).val() != '' ? $("#MainContent_gvPlantAssignList_txtqty_mmn_" + e).val() : '0';
                    var SltlQty = $("#MainContent_gvPlantAssignList_txtqty_sltl_" + e).val() != '' ? $("#MainContent_gvPlantAssignList_txtqty_sltl_" + e).val() : '0';
                    var SitlQty = $("#MainContent_gvPlantAssignList_txtqty_sitl_" + e).val() != '' ? $("#MainContent_gvPlantAssignList_txtqty_sitl_" + e).val() : '0';
                    var PdkQty = $("#MainContent_gvPlantAssignList_txtqty_pdk_" + e).val() != '' ? $("#MainContent_gvPlantAssignList_txtqty_pdk_" + e).val() : '0';
                    if (itemqty != "" && itemqty != (parseInt(MmnQty) + parseInt(SltlQty) + parseInt(SitlQty) + parseInt(PdkQty)))
                        errmsg += 'Enter proper tyre qty in row ' + $("#MainContent_gvPlantAssignList_lblSRNO_" + e).text() + '<br/>';
                    else if (parseInt(MmnQty) >= 0 && parseInt(SltlQty) >= 0 && parseInt(SitlQty) >= 0 && parseInt(PdkQty) >= 0) {
                        mmnTot += parseInt(MmnQty);
                        pdkTot += parseInt(PdkQty);
                        sltlTot += parseInt(SltlQty);
                        sitlTot += parseInt(SitlQty);
                    }
                });
                if ($("#ddlPlantList option:selected").text() == "CHOOSE")
                    errmsg += 'Choose final container loading<br/>';
                else if ($("#ddlPlantList option:selected").text() != "CHOOSE") {
                    if (($("#ddlPlantList option:selected").text() == "MMN" && mmnTot == 0) || ($("#ddlPlantList option:selected").text() == "PDK" && pdkTot == 0) ||
                    ($("#ddlPlantList option:selected").text() == "SLTL" && sltlTot == 0) || ($("#ddlPlantList option:selected").text() == "SITL" && sitlTot == 0))
                        errmsg += 'Choose the final container loading from production assigned plant';
                }
            }
            if (errmsg.length > 0) {
                $('#lblErrmsg').html(errmsg);
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
