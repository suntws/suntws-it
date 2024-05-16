<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="editmasterdetails.ascx.cs"
    Inherits="TTS.cargomanagement.editprocessiddetails" %>
<style type="text/css">
    #ddlPlatform, #ddlTyreSize, #ddlRimsize, #ddlTyreType, #ddlBrand, #ddlSidewall
    {
        width: 160px;
    }
    #divContent
    {
        padding-left: 10px;
        padding-top: 10px;
    }
    #gvTyredimension
    {
        width: 99%;
        margin-top: 20px;
        margin-bottom: 20px;
    }
</style>
<div style="width: 1080px;">
    <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label>
    <div id="divContent">
        <div style="column-count: 6">
            <div>
                <label>
                    PLATFORM
                </label>
                <br />
                <asp:DropDownList ID="ddlPlatform" ClientIDMode="Static" runat="server" OnSelectedIndexChanged="ddlPlatform_SelectedIndexChanged"
                    AutoPostBack="true">
                </asp:DropDownList>
            </div>
            <div>
                <label>
                    TYRE SIZE
                </label>
                <br />
                <asp:DropDownList ID="ddlTyreSize" ClientIDMode="Static" runat="server" OnSelectedIndexChanged="ddlTyreSize_SelectedIndexChanged"
                    AutoPostBack="true">
                </asp:DropDownList>
            </div>
            <div>
                <label>
                    RIMSIZE
                </label>
                <br />
                <asp:DropDownList ID="ddlRimsize" ClientIDMode="Static" runat="server" OnSelectedIndexChanged="ddlRimsize_SelectedIndexChanged"
                    AutoPostBack="true">
                </asp:DropDownList>
            </div>
            <div>
                <label>
                    TYRE TYPE
                </label>
                <br />
                <asp:DropDownList ID="ddlTyreType" ClientIDMode="Static" runat="server" OnSelectedIndexChanged="ddlTyreType_SelectedIndexChanged"
                    AutoPostBack="true">
                </asp:DropDownList>
            </div>
            <div>
                <label>
                    BRAND
                </label>
                <br />
                <asp:DropDownList ID="ddlBrand" ClientIDMode="Static" runat="server" OnSelectedIndexChanged="ddlBrand_SelectedIndexChanged"
                    AutoPostBack="true">
                </asp:DropDownList>
            </div>
            <div>
                <label>
                    SIDEWALL
                </label>
                <br />
                <asp:DropDownList ID="ddlSidewall" ClientIDMode="Static" runat="server" OnSelectedIndexChanged="ddlSidewall_SelectedIndexChanged"
                    AutoPostBack="true">
                </asp:DropDownList>
            </div>
        </div>
        <div>
            <div>
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
            </div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="gvTyredimension" runat="server" AutoGenerateColumns="false" Font-Names="Arial"
                        Font-Size="12px" AlternatingRowStyle-BackColor="#C2D69B" HeaderStyle-BackColor="green"
                        HeaderStyle-Height="22px" HeaderStyle-ForeColor="white" OnRowEditing="EditDimensions"
                        OnRowUpdating="UpdateDimensions" OnRowCancelingEdit="CancelEdit" ClientIDMode="Static"
                        OnPageIndexChanging="OnPaging" PageSize="25" AllowPaging="true">
                        <Columns>
                            <asp:TemplateField HeaderText="CONFIG">
                                <ItemTemplate>
                                    <asp:Label ID="lblConfig" runat="server" Text='<%# Eval("Config")%>'></asp:Label>
                                    <asp:HiddenField ID="hdnProcessId" runat="server" Value='<%# Eval("ProcessID")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TYRE SIZE">
                                <ItemTemplate>
                                    <asp:Label ID="lblTyreSize" runat="server" Text='<%# Eval("TyreSize")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TYRE RIM">
                                <ItemTemplate>
                                    <asp:Label ID="lblTyreRim" runat="server" Text='<%# Eval("TyreRim")%>'></asp:Label>
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
                            <asp:TemplateField HeaderText="WEIGHT">
                                <ItemTemplate>
                                    <asp:Label ID="lblWeight" runat="server" Text='<%# Eval("Weight")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtWeight" Text='<%# Eval("Weight")%>' onkeypress="return isNumberKey(event)"
                                        runat="server"></asp:TextBox>
                                </EditItemTemplate>
                                <ControlStyle Width="60" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="WIDTH">
                                <ItemTemplate>
                                    <asp:Label ID="lblWidth" runat="server" Text='<%# Eval("Width")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtWidth" Text='<%# Eval("Width")%>' onkeypress="return isNumberKey(event)"
                                        runat="server"></asp:TextBox>
                                </EditItemTemplate>
                                <ControlStyle Width="60" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="INNER DIA">
                                <ItemTemplate>
                                    <asp:Label ID="lblInnerDiameter" runat="server" Text='<%# Eval("InnerDiameter")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtInnerDiameter" Text='<%# Eval("InnerDiameter")%>' onkeypress="return isNumberKey(event)"
                                        runat="server"></asp:TextBox>
                                </EditItemTemplate>
                                <ControlStyle Width="60" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="OUTER DIA">
                                <ItemTemplate>
                                    <asp:Label ID="lblOuterDiameter" runat="server" Text='<%# Eval("OuterDiameter")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtOuterDiameter" Text='<%# Eval("OuterDiameter")%>' onkeypress="return isNumberKey(event)"
                                        runat="server"></asp:TextBox>
                                </EditItemTemplate>
                                <ControlStyle Width="60" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="VOLUME">
                                <ItemTemplate>
                                    <asp:Label ID="lblVolume" runat="server" Text='<%# Eval("Volume")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtVolume" Text='<%# Eval("Volume")%>' onkeypress="return isNumberKey(event)"
                                        runat="server"></asp:TextBox>
                                </EditItemTemplate>
                                <ControlStyle Width="60" />
                            </asp:TemplateField>
                            <asp:CommandField ButtonType="Link" ShowEditButton="True" HeaderText="ACTION" />
                        </Columns>
                        <AlternatingRowStyle BackColor="#C2D69B" />
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="gvTyredimension" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</div>
<script src="/Scripts/ttsJS.js" type="text/javascript"></script>
<script type="text/javascript">
    $("#lblPageHead").text("Edit Master details");
</script>
