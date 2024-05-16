<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="tyredimensionmaster.ascx.cs"
    Inherits="TTS.cargomanagement.tyredimensionmaster" %>
<style type="text/css">
    #divContent
    {
        margin: 10px 10px 10px 10px;
        padding: 10px 10px 10px 10px;
        font-family: Arial Sans-Serif;
    }
    #divContent label
    {
        font-size: 14px;
        letter-spacing: 1px;
        text-transform: uppercase;
    }
    #ddlCustomer, #ddlOrder
    {
        min-width: 180px;
        max-width: 430px;
        padding-top: 2px;
    }
    #gvProcessIdDetail
    {
        width: 99%;
    }
    .errorRow
    {
        border: 1px solid red;
    }
    #lblErrMsg
    {
        font-size: 14px;
    }
    .hide
    {
        display: none;
    }
</style>
<asp:ScriptManager ID="scriptManager1" runat="server">
</asp:ScriptManager>
<div style="width: 1080px;">
    <div id="divContent">
        <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #CE8686;
            width: 1065px;">
            <tr>
                <td>
                    Customer
                </td>
                <td>
                    <asp:DropDownList ID="ddlCustomer" ClientIDMode="Static" runat="server" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged"
                        AutoPostBack="true">
                    </asp:DropDownList>
                </td>
                <td>
                    Order Ref No.
                </td>
                <td>
                    <asp:DropDownList ID="ddlOrder" ClientIDMode="Static" runat="server" OnSelectedIndexChanged="ddlOrder_SelectedIndexChanged"
                        AutoPostBack="true">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:TextBox runat="server" ID="txtCargoSplIns" ClientIDMode="Static" TextMode="MultiLine"
                        Text="" Width="650px" Height="50px" Enabled="false"></asp:TextBox>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblUserDetails" ClientIDMode="Static" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:UpdatePanel ID="updatePanel1" runat="server">
                        <ContentTemplate>
                            <div style="margin-bottom: 10px;">
                                <asp:GridView ID="gvProcessIdDetail" runat="server" AutoGenerateColumns="false" Font-Size="12px"
                                    AlternatingRowStyle-BackColor="#C2D69B" HeaderStyle-BackColor="green" HeaderStyle-Height="22px"
                                    HeaderStyle-ForeColor="white" OnPageIndexChanging="OnPaging" PageSize="50" AllowPaging="true"
                                    Width="100%" EnableViewState="true">
                                    <Columns>
                                        <asp:TemplateField HeaderText="CONFIG">
                                            <ItemTemplate>
                                                <asp:Label ID="lblConfig" runat="server" Text='<%# Eval("Config")%>'></asp:Label>
                                                <asp:HiddenField ID="hdnProcessID" runat="server" Value='<%# Eval("ProcessID")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="TYRE SIZE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTyreSize" runat="server" Text='<%# Eval("TyreSize")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="RIM SIZE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTyreRim" runat="server" Text='<%# Eval("rimsize")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="TYRE TYPE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTyreType" runat="server" Text='<%# Eval("TyreType")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="BRAND">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBrand" runat="server" Text='<%# Eval("Brand")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SIDEWALL">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSidewall" runat="server" Text='<%# Eval("Sidewall")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="WEIGHT(mm)" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtWeight" Text='<%# Eval("Weight")%>' onkeypress="return isNumberKey(event)"
                                                    runat="server"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="60" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="WIDTH(mm)" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtWidth" Text='<%# Eval("Width")%>' onkeypress="return isNumberKey(event)"
                                                    runat="server"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="60" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SIDEWALL WIDTH(mm)" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtInnerDiameter" Text='<%# Eval("InnerDiameter")%>' onkeypress="return isNumberKey(event)"
                                                    runat="server"></asp:TextBox>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtInnerDiameter" Text='<%# Eval("InnerDiameter")%>' onkeypress="return isNumberKey(event)"
                                                    runat="server"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ControlStyle Width="60" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="OUTER DIA(mm)" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtOuterDiameter" Text='<%# Eval("OuterDiameter")%>' onkeypress="return isNumberKey(event)"
                                                    runat="server"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="60" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div style="text-align: center; width: 100%">
                                <asp:Button ID="btnSave" runat="server" ClientIDMode="static" Text="SAVE" OnClick="btnSave_Click"
                                    CssClass="hide" />
                                <br />
                                <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvProcessIdDetail" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
</div>
<script type="text/javascript">

    $("#lblPageHead").text("Tyre dimension entry");

    $("#btnSave").click(function (event) {
        $("#<%=gvProcessIdDetail.ClientID%> .errorRow").removeClass("errorRow");
        $("#<%=gvProcessIdDetail.ClientID%> tr").each(function (index) {
            if (index > 0) {
                id = index - 1;
                $(this).find("input[type='text']").each(function () {
                    if ($(this).val() == "" || parseFloat($(this).val()) <= 0) {
                        $(this).closest("tr").addClass("errorRow");
                        $("#lblErrMsg").text("Enter dimension value for all items in the list and value must be greater than 0");
                        event.preventDefault();
                        $(this).focus();
                        return false;
                    }
                });
            }
        });
    });

    function showSaveButton() {
        $("#btnSave").removeClass("hide");
    }
</script>
