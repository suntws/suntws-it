<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="TypeCostForAF.aspx.cs" Inherits="TTS.TypeCostForAF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div align="center" class="pageTitleHead">
        TYPE COST MASTER
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="background-color: #ffffff; width: 100%;
            border-color: #cccbfb; border-collapse: separate; line-height: 25px;">
            <tr style="text-align: center;">
                <td>
                    ACCOUNTING PLANT
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlFinancePlant" OnSelectedIndexChanged="ddlFinancePlant_IndexChange"
                        CssClass="form-control" AutoPostBack="true" Width="100px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:GridView runat="server" ID="gv_TypeCose" AutoGenerateColumns="false" AlternatingRowStyle-BackColor="#f5f5f5">
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="150px" ItemStyle-Height="22px" HeaderText="TYPE">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCostTyreType" Text='<%# Eval("TyreType") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="70px" HeaderText="COST">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txtTypeCostValue" Text='<%# Eval("TypeCost") %>'
                                        onkeypress="return isNumberKey(event)" MaxLength="7" CssClass="form-control"
                                        Width="100px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CostMethod" HeaderText="CURRENCY" />
                            <asp:BoundField DataField="CreatedOn" HeaderText="CREATED ON" />
                            <asp:BoundField DataField="CreatedBy" HeaderText="CREATED BY" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr style="text-align: center;">
                <td>
                    <asp:Button runat="server" ID="btnSaveTypeCost" Text="SAVE TYPE COST" CssClass="btn btn-success"
                        OnClick="btnSaveTypeCost_Click" OnClientClick="javascript:return CtrlTypeCost();" />
                </td>
                <td>
                    <asp:Button runat="server" ID="btnClear" Text="CLEAR" CssClass="btn btn-danger" OnClick="btnClear_Click" />
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        function CtrlTypeCost() {
            var CostMsg = '';
            var txtTypeCostValue = $("input[id*='MainContent_gv_TypeCose_txtTypeCostValue']");
            for (var k = 0; k < txtTypeCostValue.length; k++) {
                if ($('#MainContent_gv_TypeCose_txtTypeCostValue_' + k).val().length == 0) {
                    CostMsg += "," + parseInt(k + 1);
                    $('#MainContent_gv_TypeCose_txtTypeCostValue_' + k).css({ 'background-color': '#fba1a9' });
                }
            }
            if (CostMsg.length > 0) {
                alert("Enter decimal values to rows -" + CostMsg);
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
