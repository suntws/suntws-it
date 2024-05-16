<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="stocklocation.aspx.cs" MasterPageFile="~/master.Master"
    Inherits="TTS.stocklocation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .tableCss
        {
            background-color: #dcecfb;
            width: 100%;
            border-color: White;
            border-collapse: separate;
        }
        .tableCss th
        {
            width: 120px;
            color: #008000;
            font-weight: normal;
            text-align: right;
        }
        .tableCss td
        {
            font-weight: 500;
            text-align: left;
        }
        .spanCss
        {
            font-size: 14px;
            font-style: italic;
            color: #008000;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: center;" class="pageTitleHead">
        <asp:Label runat="server" ID="lblPageTitle" ClientIDMode="Static" Text="STOCK LOCATION UPDATE"></asp:Label>
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <asp:ScriptManager runat="server" ID="ScriptManager1">
    </asp:ScriptManager>
    <div id="displaycontent" class="contPage">
        <asp:UpdatePanel ID="updatepanel1" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div style="width: 100%; padding-top: 5px;">
                    <table class="tableCss" cellspacing="0" rules="all" border="1">
                        <tr>
                            <td>
                                <span class="spanCss">PlatForm</span><br />
                                <asp:DropDownList ID="ddl_Config" Width="150" runat="server" ClientIDMode="Static"
                                    CssClass="form-control ddl" AutoPostBack="true" OnSelectedIndexChanged="ddl_Config_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <span class="spanCss">TyreSize</span><br />
                                <asp:DropDownList ID="ddl_TyreSize" Width="150" runat="server" ClientIDMode="Static"
                                    CssClass="form-control ddl" AutoPostBack="true" OnSelectedIndexChanged="ddl_TyreSize_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <span class="spanCss">RimSize</span><br />
                                <asp:DropDownList ID="ddl_RimSize" Width="150" runat="server" ClientIDMode="Static"
                                    CssClass="form-control ddl" AutoPostBack="true" OnSelectedIndexChanged="ddl_RimSize_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <span class="spanCss">TyreType</span><br />
                                <asp:DropDownList ID="ddl_TyreType" Width="150" runat="server" ClientIDMode="Static"
                                    CssClass="form-control ddl" AutoPostBack="true" OnSelectedIndexChanged="ddl_TyreType_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <span class="spanCss">Brand</span><br />
                                <asp:DropDownList ID="ddl_Brand" Width="150" runat="server" ClientIDMode="Static"
                                    CssClass="form-control ddl" AutoPostBack="true" OnSelectedIndexChanged="ddl_Brand_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <span class="spanCss">Sidewall</span><br />
                                <asp:DropDownList ID="ddl_Sidewall" Width="150" runat="server" ClientIDMode="Static"
                                    CssClass="form-control ddl" AutoPostBack="true" OnSelectedIndexChanged="ddl_Sidewall_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </div>
                <hr />
                <div id="div_gif" style="display: none; text-align: center; vertical-align: middle;">
                    <img src="images/Arrow_gif.gif" alt="Loading..." />
                </div>
                <div id="div_gv" style="width: 100%; height: 390px; overflow: auto;">
                    <div style="text-align: center;">
                        <asp:Label ID="lblTotalRecords" runat="server" Font-Bold="true" ForeColor="#137d06"
                            Font-Size="15px"></asp:Label>
                    </div>
                    <div>
                        <asp:GridView runat="server" ID="gvStockDetails" AutoGenerateColumns="false" AlternatingRowStyle-BackColor="#d6eafb"
                            BackColor="White" Width="100%" HeaderStyle-Font-Size="13px" ClientIDMode="Static"
                            RowStyle-Height="22px">
                            <HeaderStyle BackColor="#74bbf9" ForeColor="White" Font-Bold="true" Height="25px"
                                HorizontalAlign="Center" />
                            <Columns>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Choose Item" ItemStyle-Width="10px">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk_item" runat="server" ClientIDMode="Static" Checked='<%# Convert.ToBoolean(Eval("EarmarkStatus")) %>'
                                            Enabled='<%# Convert.ToBoolean(Eval("CtrlStatus")) %>' />
                                        <asp:HiddenField ID="hdn_Barcode" ClientIDMode="Static" runat="server" Value='<%# Eval("barcode") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Platform" DataField="config" ItemStyle-Width="60px" />
                                <asp:BoundField HeaderText="Brand" DataField="brand" ItemStyle-Width="60px" />
                                <asp:BoundField HeaderText="Sidewall" DataField="sidewall" ItemStyle-Width="60px" />
                                <asp:BoundField HeaderText="Type" DataField="tyretype" ItemStyle-Width="80px" />
                                <asp:BoundField HeaderText="Tyre Size" DataField="tyresize" ItemStyle-Width="100px" />
                                <asp:BoundField HeaderText="Rim" DataField="rimsize" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField HeaderText="Process ID" DataField="ProcessId" ItemStyle-Width="80px" />
                                <asp:BoundField HeaderText="Stencil No" DataField="stencilno" ItemStyle-Width="100px" />
                                <asp:BoundField HeaderText="Grade" DataField="grade" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField HeaderText="FWt" DataField="Fwt" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField HeaderText="WareHouse Location" DataField="warehouse_location" ItemStyle-Width="100px" />
                                <asp:BoundField HeaderText="Location" DataField="location" ItemStyle-Width="100px" />
                            </Columns>
                        </asp:GridView>
                        <asp:GridView runat="server" ID="gvLocationUpdate" AutoGenerateColumns="true" Width="100%">
                        </asp:GridView>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div style="width: 100%;">
            <table class="tableCss" cellspacing="0" rules="all" border="1" id="div_Warehouse_Location">
                <tr>
                    <th class="spanCss">
                        Barcode File
                    </th>
                    <td>
                        <asp:FileUpload ID="fupScanLocation" runat="server" />
                        <asp:Button runat="server" ID="btnScanLocation" ClientIDMode="Static" Text="VERIFY"
                            OnClick="btnScanLocation_Click" CssClass="btn btn-info" />
                    </td>
                    <th class="spanCss">
                        Warehouse Location
                    </th>
                    <td>
                        <asp:TextBox runat="server" ID="txtWarehouse_Location" ClientIDMode="Static" Text=""
                            CssClass="form-control"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table class="tableCss" cellspacing="0" rules="all" border="1">
                <tr>
                    <td style="text-align: center;">
                        <asp:Button ID="btnSaveRecords" runat="server" Text="SAVE STOCk LOCATION" ClientIDMode="Static"
                            CssClass="btn btn-success" OnClientClick="javascript:return cntrlSave();" OnClick="btnSaveRecords_Click" />
                    </td>
                    <td style="text-align: center;">
                        <asp:Button ID="btnClearRecords" runat="server" Text="Clear" ClientIDMode="Static"
                            CssClass="btn btn-info" OnClientClick="javascript:return ctrlClear();" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <asp:HiddenField ID="hdnSelectedBarcode" runat="server" ClientIDMode="Static" />
    <script type="text/javascript">
        $(function () {
            $('#div_Warehouse_Location').css({ 'display': 'block' });
            $('.ddl').change(function () {
                $('#div_gif').fadeIn();
                $('#div_gif').fadeOut(10000);
                $('#div_gv').fadeIn(10000);
            });
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_endRequest(function () {
                $('.ddl').change(function () {
                    $('#div_gv').hide();
                    $('#div_gif').fadeIn();
                    $('#div_gif').fadeOut(10000);
                    $('#div_gv').fadeIn(10000);
                });
            });
        });
        function cntrlSave() {
            var ErrMsg = "", barcode = "";
            if ($('#gvStockDetails tr').length != 0) {
                $('#gvStockDetails tr').filter(':has(:checkbox:checked)').each(function () {
                    barcode += "'" + $(this).find('#hdn_Barcode').val() + "',";
                });
                $('#hdnSelectedBarcode').val(barcode);
            }
            if ($('#hdnSelectedBarcode').val() == '')
                ErrMsg += "Choose atleast one item <br/>";
            if ($('#txtWarehouse_Location').val() == "")
                ErrMsg += "Enter warehouse location<br/>";
            if (ErrMsg.length > 0) {
                alert(ErrMsg);
                return false;
            }
            else
                return true;
        }
        function ctrlClear() { window.location.href = window.location.href; return false; }
    </script>
</asp:Content>
