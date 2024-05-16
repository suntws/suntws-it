<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="claimstencilassign.aspx.cs" Inherits="TTS.claimstencilassign" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div align="center" class="pageTitleHead">
        CLAIM STENCIL NO. ASSIGN TO QC</div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <div>
            <table style="width: 100%;">
                <tr>
                    <td align="center">
                        <asp:GridView runat="server" ID="gvClaimNoList" AutoGenerateColumns="false" Width="100%"
                            RowStyle-Height="22px">
                            <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" Height="22px" />
                            <Columns>
                                <asp:BoundField DataField="custname" HeaderText="CUSTOMER NAME" ItemStyle-Width="300px" />
                                <asp:BoundField DataField="complaintno" HeaderText="COMPLAINT NO." ItemStyle-Width="80px" />
                                <asp:BoundField DataField="complaintdate" HeaderText="COMPLAINT DATE" ItemStyle-Width="80px" />
                                <asp:BoundField DataField="Qty" HeaderText="QTY" ItemStyle-Width="40px" />
                                <asp:BoundField DataField="claimstatus" HeaderText="STATUS" ItemStyle-Width="150px" />
                                <asp:TemplateField HeaderText="ACTION" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkClaimNo" runat="server" Text="Show List" OnClick="lnkClaimNo_Click" /></span>
                                        <asp:HiddenField runat="server" ID="hdnClaimCustCode" Value='<%# Eval("custcode") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <div style="width: 100%; float: left;">
                            <div style="width: 100%; float: left; border: 1px solid #000; background-color: #056442;
                                font-weight: bold; color: #fff; font-size: 15px;">
                                <div style="width: 48%; float: left; text-align: left; padding-right: 5px;">
                                    <asp:Label runat="server" ID="lblClaimCustName" ClientIDMode="Static" Text=""></asp:Label>
                                </div>
                                <div style="width: 3%; float: left;">
                                    <asp:Label runat="server" ID="lblClaim" ClientIDMode="Static" Text=""></asp:Label>
                                </div>
                                <div style="width: 48%; float: left; text-align: right; padding-left: 5px;">
                                    <asp:Label runat="server" ID="lblClaimNo" ClientIDMode="Static" Text=""></asp:Label>
                                </div>
                            </div>
                            <div id="divclaim" style="width: 100%; float: left;">
                                <asp:GridView runat="server" ID="gvClaimItems" AutoGenerateColumns="false" Width="100%"
                                    OnRowEditing="gv_RowEditing" OnRowUpdating="gv_RowUpdating" OnRowCancelingEdit="gv_RowCanceling">
                                    <HeaderStyle BackColor="#CACA55" Font-Bold="true" Height="22px" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="BRAND" ItemStyle-Width="100px">
                                            <ItemTemplate>
                                                <%# Eval("brand")%></ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SIZE" ItemStyle-Width="150px">
                                            <ItemTemplate>
                                                <%# Eval("tyresize")%></ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="STENCIL NO." ItemStyle-Width="80px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblstencilno" Text='<%# Eval("stencilno") %>'></asp:Label></ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label runat="server" ID="lblAssignStencil" Text='<%# Eval("stencilno")%>'></asp:Label></EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="COMPLAINT" ItemStyle-Width="200px">
                                            <ItemTemplate>
                                                <%#((string)Eval("appstyle")).Replace("~", "<br/>")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="OPERATING CONDITION" ItemStyle-Width="200px">
                                            <ItemTemplate>
                                                <%#((string)Eval("runninghours")).Replace("~", "<br/>")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ASSIGN TO" ItemStyle-Width="80px">
                                            <ItemTemplate>
                                                <asp:DropDownList runat="server" ID="ddlAssignQc" CssClass="cssassignqc">
                                                    <asp:ListItem Text="CHOOSE" Value="CHOOSE"></asp:ListItem>
                                                    <asp:ListItem Text="MMN" Value="MMN"></asp:ListItem>
                                                    <asp:ListItem Text="SLTL" Value="SLTL"></asp:ListItem>
                                                    <asp:ListItem Text="SITL" Value="SITL"></asp:ListItem>
                                                    <asp:ListItem Text="PDK" Value="PDK"></asp:ListItem>
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ASSIGN TO" ItemStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblAssignedQc" Text='<%# Eval("assigntoqc") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label runat="server" ID="lblAssignToQc" Text='<%# Eval("assigntoqc") %>'></asp:Label>
                                                <asp:DropDownList runat="server" ID="ddlAssignToQc">
                                                    <asp:ListItem Text="MMN" Value="MMN"></asp:ListItem>
                                                    <asp:ListItem Text="SLTL" Value="SLTL"></asp:ListItem>
                                                    <asp:ListItem Text="SITL" Value="SITL"></asp:ListItem>
                                                    <asp:ListItem Text="PDK" Value="PDK"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Label runat="server" ID="lblErr" Text=""></asp:Label>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ACTION" ItemStyle-Width="70px">
                                            <ItemTemplate>
                                                <asp:Button runat="server" ID="btnEdit" Text="EDIT" CommandName="Edit" CssClass="btnedit" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Button runat="server" ID="btnUpdate" Text="Update" CommandName="Update" CssClass="btnsave" />
                                                <asp:Button runat="server" ID="btnCancel" Text="Cancel" CommandName="Cancel" CssClass="btnclear" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div style="width: 100%; float: left; text-align: left; margin-top: 5px;">
                                <asp:Label runat="server" ID="lblComplaintComments" ClientIDMode="Static" Text=""></asp:Label>
                            </div>
                            <div style="width: 200px; float: right; text-align: left;">
                                <asp:Button runat="server" ID="btnAssignto" Text="Assign To" CssClass="btnedit" OnClick="btnAssignto_Click"
                                    OnClientClick="javascript:return chkstencilAssign();" />
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label>
                        <asp:Button runat="server" ID="btnMoveToQc" ClientIDMode="Static" Text="MOVE TO QC FOR ANALYSIS"
                            CssClass="btnsave" OnClick="btnMoveToQc_Click" OnClientClick="javascript:return CtrlMoveToQc();" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdnCustCode" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnClaimNoClick" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnClaimStencilClick" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        function Ctrlassigplant() {
            $("span[id*=MainContent_gvClaimItems_lblstencilno_]").each(function (e) {
                var stencilno = $("#MainContent_gvClaimItems_lblstencilno_" + e).text();
                var firstchar = stencilno.charAt(0);
                if (firstchar == 'c' || firstchar == 'C')
                    $('#MainContent_gvClaimItems_ddlAssignQc_' + e + ' option[value="MMN"]').attr("selected", "selected");
                else if (firstchar == 'P' || firstchar == 'p')
                    $("#MainContent_gvClaimItems_ddlAssignQc_" + e + " option[value='PDK']").attr("selected", "selected");
                else if (firstchar == 'l' || firstchar == 'L')
                    $("#MainContent_gvClaimItems_ddlAssignQc_" + e + " option[value='SLTL']").attr("selected", "selected");
                else if (firstchar == 's' || firstchar == 'S')
                    $("#MainContent_gvClaimItems_ddlAssignQc_" + e + " option[value='SITL']").attr("selected", "selected");
                else
                    $("#MainContent_gvClaimItems_ddlAssignQc_" + e + " option[value='CHOOSE']").attr("selected", "selected");
            });
        }
        function chkstencilAssign() {
            var errmsg = '';
            $('#lblErrMsg').html('');
            $("span[id*=MainContent_gvClaimItems_lblstencilno_]").each(function (e) {
                if ($('#MainContent_gvClaimItems_ddlAssignQc_' + e + ' option:selected').text() == "CHOOSE") {
                    errmsg += 'Choose plant in row no. ' + (parseInt(e) + 1).toString() + '<br/>';
                }
            });
            if (errmsg.length > 0) {
                $('#lblErrMsg').html(errmsg);
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
