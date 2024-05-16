<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="cotsorderplantassign.aspx.cs" Inherits="TTS.cotsorderplantassign" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="Scripts/cotsScript.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        ORDER PLANT ASSIGN</div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="background-color: #ecf6ff; border-color: White;
            border-collapse: separate; width: 100%;">
            <tr>
                <td>
                    <asp:GridView runat="server" ID="gvorderlist" AutoGenerateColumns="false" Width="100%"
                        HeaderStyle-BackColor="#FFCC00" AlternatingRowStyle-BackColor="#f5f5f5" RowStyle-Height="25px">
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
                            <asp:BoundField HeaderText="ORDER QTY" DataField="itemqty" ItemStyle-HorizontalAlign="Right" />
                            <asp:TemplateField ItemStyle-Width="80px" HeaderText="ACTION">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hdnOID" Value='<%# Eval("OID") %>' />
                                    <asp:LinkButton ID="lnkPlantAssign" runat="server" Text="Plant Assign" OnClick="lnkPlantAssign_Click"
                                        Visible='<%# Eval("CustHoldStatus").ToString() == "True" ? false : true%>' />
                                    <span style="color: #ff0000; font-style: italic;">
                                        <%# Eval("CustHoldStatus").ToString() == "True" ? "CREDIT HOLD" : ""%></span>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="width: 100%; float: left; display: none;" id="divStatusChange">
                        <table cellspacing="0" rules="all" border="1" style="background-color: #ecf6ff; border-color: White;
                            border-collapse: separate; width: 100%;">
                            <tr style="font-weight: bold; font-size: 18px; color: #fff; background-color: #2E2B2B;
                                text-align: center;">
                                <td>
                                    <asp:Label runat="server" ID="lblCustName" ClientIDMode="Static" Text=""></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblStausOrderRefNo" ClientIDMode="Static" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div style="width: 100%; float: left; font-weight: bold; font-size: 14px; text-align: right;">
                                        STOCK COLUMN FORMAT : EXACT/ REBRAND/ UPGRADE</div>
                                    <asp:GridView runat="server" ID="gvPlantAssignList" AutoGenerateColumns="false" Width="100%"
                                        AlternatingRowStyle-BackColor="#f5f5f5">
                                        <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" Height="22px" />
                                        <Columns>
                                            <asp:TemplateField ItemStyle-Width="65px" ItemStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <asp:CheckBox runat="server" ID="checkAllChk" ClientIDMode="Static" />ALL</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox runat="server" ID="chkPlantAssign" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CATEGORY" ItemStyle-Width="100px">
                                                <ItemTemplate>
                                                    <%# Eval("category") %>
                                                    <%# Eval("AssyRimstatus").ToString() == "True" ? " (ASSY)" : ""%>
                                                    <asp:Label runat="server" ID="lblProcessid" Text='<%#Eval("processid") %>' CssClass="headerNone"></asp:Label>
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
                                            <asp:TemplateField HeaderText="RIM QTY" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="40px">
                                                <ItemTemplate>
                                                    <%# Eval("Rimitemqty").ToString() == "0" ? "" : Eval("Rimitemqty").ToString()%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="MMN STOCK" ItemStyle-HorizontalAlign="Right" ItemStyle-Font-Bold="true">
                                                <ItemTemplate>
                                                    <%# Eval("MMN_Stock").ToString()!=""?Eval("MMN_Stock").ToString():"0" %>/
                                                    <%# Eval("MMN_RebrandStock").ToString() != "" ? Eval("MMN_RebrandStock").ToString() : "0"%>/
                                                    <%# Eval("MMN_UpGradeStock").ToString() != "" ? Eval("MMN_UpGradeStock").ToString() : "0"%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PDK STOCK" ItemStyle-HorizontalAlign="Right" ItemStyle-Font-Bold="true">
                                                <ItemTemplate>
                                                    <%# Eval("PDK_Stock").ToString()!=""?Eval("PDK_Stock").ToString():"0" %>/
                                                    <%# Eval("PDK_RebrandStock").ToString() != "" ? Eval("PDK_RebrandStock").ToString() : "0"%>/
                                                    <%# Eval("PDK_UpGradeStock").ToString() != "" ? Eval("PDK_UpGradeStock").ToString() : "0"%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="BHIWANDI STOCK" ItemStyle-HorizontalAlign="Right"
                                                ItemStyle-Font-Bold="true">
                                                <ItemTemplate>
                                                    <%# Eval("BHIWANDI_Stock").ToString() != "" ? Eval("BHIWANDI_Stock").ToString() : "0"%>/
                                                    <%# Eval("BHIWANDI_RebrandStock").ToString() != "" ? Eval("BHIWANDI_RebrandStock").ToString() : "0"%>/
                                                    <%# Eval("BHIWANDI_UpGradeStock").ToString() != "" ? Eval("BHIWANDI_UpGradeStock").ToString() : "0"%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    PLANT
                                    <asp:DropDownList runat="server" ID="ddlPlantList" ClientIDMode="Static" Width="100px">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align: left;">
                                    <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" Font-Bold="true"
                                        ForeColor="Red"></asp:Label>
                                    <asp:Button runat="server" ID="btnOrderPlantAssign" Text="SAVE ORDER PLANT" CssClass="btnsave"
                                        OnClientClick="javascript:return chkPlantAssignItem();" OnClick="btnOrderPlantAssign_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnOrderID" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        $(document).ready(function () {
            $('#checkAllChk').click(function () {
                var chkLength = $("[id*=MainContent_gvPlantAssignList_chkPlantAssign_]").length;
                if (chkLength > 0) {
                    if ($(this).attr('checked') == "checked")
                        $("[id*=MainContent_gvPlantAssignList_chkPlantAssign_]").attr('checked', true)
                    else
                        $("[id*=MainContent_gvPlantAssignList_chkPlantAssign_]").attr('checked', false)
                }
                else {
                    alert('No records');
                    $(this).attr('checked', false);
                }
            });
            $("[id*=MainContent_gvPlantAssignList_chkPlantAssign_]").click(function () {
                $('#checkAllChk').attr('checked', false);
            });
        });

        function chkPlantAssignItem() {
            var errmsg = '';
            $('#lblErrMsg').html('');
            if ($("input:checkbox[id*=MainContent_gvPlantAssignList_chkPlantAssign_]:checked").length == 0)
                errmsg += 'Choose any one order item<br/>';
            if ($("#ddlPlantList option:selected").text() == "CHOOSE")
                errmsg += 'Choose order plant<br/>';
            if (errmsg.length > 0) {
                $('#lblErrMsg').html(errmsg);
                return false;
            }
            else {
                if ($("input:checkbox[id*=MainContent_gvPlantAssignList_chkPlantAssign_]").length == $("input:checkbox[id*=MainContent_gvPlantAssignList_chkPlantAssign_]:checked").length)
                    $('#checkAllChk').attr('checked', true);
                return true;
            }
        }
    </script>
</asp:Content>
