<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="ratesmaster_v1.aspx.cs" Inherits="TTS.ratesmaster_v1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .form-control
        {
            width: 300px;
            height: 25px;
            font-size: 14px;
            color: #000;
            background-color: #fff;
            border: 1px solid #000;
            border-radius: 4px;
        }
        .form-control:hover, .form-control:focus
        {
            background-color: #555;
            color: #fff;
        }
        .btn
        {
            text-decoration: none;
            padding: 4px 10px;
            font-size: 14px;
            font-weight: bold;
            text-align: center;
            vertical-align: middle;
            cursor: pointer;
            border: 1px solid transparent;
            border-radius: 4px;
        }
        .btn-success
        {
            color: #fff;
            background-color: #5cb85c;
            border-color: #4cae4c;
        }
        .btn-success:hover
        {
            color: #fff;
            background-color: #449d44;
            border-color: #398439;
        }
        .btn-warning
        {
            color: #fff;
            background-color: #f0ad4e;
            border-color: #eea236;
        }
        .btn-warning:hover
        {
            color: #fff;
            background-color: #ec971f;
            border-color: #d58512;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: center" class="pageTitleHead">
        RATES-ID
        <asp:Label runat="server" ID="lblPageHead" ClientIDMode="Static" Text=""></asp:Label>
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div class="contPage">
        <asp:ScriptManager runat="server" ID="scriptManager1">
        </asp:ScriptManager>
        <table cellspacing="0" rules="all" border="1" style="border: solid 1px #525252; border-collapse: collapse;
            width: 100%;">
            <tr>
                <td>
                    RATES-ID
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlRatesID" AutoPostBack="true" OnSelectedIndexChanged="ddlRatesID_IndexChanged"
                        CssClass="form-control" Width="400px">
                    </asp:DropDownList>
                </td>
                <td>
                    NEW RATES-ID
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtRatesID" ClientIDMode="Static" Text="" CssClass="form-control"
                        MaxLength="50" Width="400px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="4" style="display: none;">
                    <asp:GridView runat="server" ID="gvRatesCompound" AutoGenerateColumns="false" HeaderStyle-BackColor="#fefe8b"
                        Width="100%" RowStyle-Font-Bold="true" RowStyle-HorizontalAlign="Center">
                        <Columns>
                            <asp:BoundField DataField="lipgum" HeaderText="LIP/GUM" />
                            <asp:TemplateField HeaderText="LIP COST">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txtLipCost" Text='<%# Eval("LipGumValue") %>' onkeypress="return isNumberKey(event)"
                                        MaxLength="7" CssClass="form-control" Visible='<%# Eval("lipgum").ToString() != "-" ? true : false %>'
                                        Width="100px" Style="text-align: right">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="100px">
                                <ItemTemplate>
                                    &nbsp;
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="base" HeaderText="BASE" />
                            <asp:TemplateField HeaderText="BASE COST">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txtBaseCost" Text='<%# Eval("BaseValue") %>' onkeypress="return isNumberKey(event)"
                                        MaxLength="7" CssClass="form-control" Visible='<%# Eval("base").ToString() != "-" ? true : false %>'
                                        Width="100px" Style="text-align: right">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="100px">
                                <ItemTemplate>
                                    &nbsp;
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="center" HeaderText="CENTER" />
                            <asp:TemplateField HeaderText="CENTER COST">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txtCenterCost" Text='<%# Eval("CenterValue") %>'
                                        onkeypress="return isNumberKey(event)" MaxLength="7" CssClass="form-control"
                                        Visible='<%# Eval("center").ToString() != "-" ? true : false %>' Width="100px"
                                        Style="text-align: right">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="100px">
                                <ItemTemplate>
                                    &nbsp;
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="tread" HeaderText="TREAD" />
                            <asp:TemplateField HeaderText="TREAD COST">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txtTreadCost" Text='<%# Eval("TreadValue") %>' onkeypress="return isNumberKey(event)"
                                        MaxLength="7" CssClass="form-control" Visible='<%# Eval("tread").ToString() != "-" ? true : false %>'
                                        Width="100px" Style="text-align: right">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    CURRENCY
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlCurrency" AutoPostBack="true" OnSelectedIndexChanged="ddlCurrency_IndexChanged"
                        CssClass="form-control" Width="400px">
                    </asp:DropDownList>
                </td>
                <td>
                    LOAD FACTOR
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtLoadFactor" ClientIDMode="Static" Text="" CssClass="form-control"
                        MaxLength="3" onkeypress="return isNumberKey(event)" Width="100px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblCurrencyType" ClientIDMode="Static" Text=""></asp:Label>
                    &nbsp;=
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtInrValue" ClientIDMode="Static" Text="" CssClass="form-control"
                        MaxLength="3" onkeypress="return isNumberKey(event)" Width="100px"></asp:TextBox>&nbsp;INR
                </td>
                <td colspan="2" style="text-align: center; display: none;">
                    <asp:Button runat="server" ID="btnCalcRmCost" ClientIDMode="Static" Text="CALCUALTE RM COST"
                        OnClientClick="javascript:return ctrlRatesMaster_validate();" OnClick="btnCalcRmCost_Click"
                        CssClass="btn btn-success" />
                </td>
            </tr>
            <tr>
                <td colspan="4" style="text-align: center; font-weight: bold;">
                    TYPE COST
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:DataList runat="server" ID="dlTypeList" RepeatColumns="5" RepeatDirection="Vertical"
                        RepeatLayout="Table" Width="100%" GridLines="Both">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblTyreType" Text='<%# Eval("type") %>' Width="100px"></asp:Label>
                            <asp:TextBox runat="server" ID="txtTypeVal" Text='<%# Eval("typeval") %>' Width="70px"
                                Style="text-align: right;" onkeypress="return isNumberKey(event)" MaxLength="7"></asp:TextBox>
                        </ItemTemplate>
                    </asp:DataList>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" ForeColor="Red" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="3" style="text-align: center;">
                    <asp:Button runat="server" ID="btnSave" ClientIDMode="Static" Text="SAVE TYPE COST"
                        OnClientClick="javascript:return ctrlRatesMaster_validate();" OnClick="btnSave_Click"
                        CssClass="btn btn-success" />
                </td>
                <td style="text-align: center;">
                    <asp:Button runat="server" ID="btnClear" ClientIDMode="Static" Text="CLEAR" OnClick="btnClear_Click"
                        CssClass="btn btn-warning" />
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {

            $('#MainContent_dlTypeList').find('td').hover(function () {
                $(this).css({ 'background-color': '#81f99b' });
            }).mouseleave(function () {
                $(this).css({ 'background-color': '#ffffff' });
            });

            $('#MainContent_gvRatesCompound,#MainContent_dlTypeList').find('input:text').blur(function () {
                if ($(this).val().length == 0)
                    $(this).val('0');
                $(this).css({ 'border': '1px solid #000000' })
            });
        });

        function ctrlRatesMaster_validate() {
            $('#lblErrMsg').html('');
            var errMsg = '';
            if ($('#txtRatesID').val().length == 0)
                errMsg += 'Select/Enter Rates-ID<br/>';
            if ($('#lblCurrencyType').html().length == 0)
                errMsg += 'Select Currency<br/>';
            if ($('#txtInrValue').val().length == 0)
                errMsg += 'Enter currency rate of INR<br/>';
            if ($('#txtLoadFactor').val().length == 0)
                errMsg += 'Enter load factor value<br/>';

            var validDec = /^[-+]?(?:\d+\.?\d*|\.\d+)$/;
            var errTypeCost = '', valTypeCost = '';
            $('#MainContent_dlTypeList').find('input:text').each(function (e) {
                if (parseFloat($(this).val()) < 0) {
                    if (valTypeCost.length == 0)
                        valTypeCost = 'Enter type cost in the yellow color boxes<br/>';
                    $(this).css({ 'border': '2px solid #fff500' });
                }
                else if (!validDec.test($(this).val())) {
                    if (errTypeCost.length == 0)
                        errTypeCost = "Enter proper type cost in red color boxes<br/>";
                    $(this).css({ 'border': '2px solid #f00f00' });
                }
            });
            errMsg += valTypeCost;
            errMsg += errTypeCost;

            //            var errCompound = '';
            //            var valCompound = '';
            //            $('#MainContent_gvRatesCompound').find('input:text').each(function (e) {
            //                if (parseFloat($(this).val()) <= 0) {
            //                    if (valCompound.length == 0)
            //                        valCompound = 'Enter compound values in the yellow color boxes<br/>';
            //                    $(this).css({ 'border': '2px solid #fff500' });
            //                }
            //                else if (!validDec.test($(this).val())) {
            //                    if (errCompound.length == 0)
            //                        errCompound = "Enter proper compound values in red color boxes<br/>";
            //                    $(this).css({ 'border': '2px solid #f00f00' });
            //                }
            //            });

            //            errMsg += valCompound;
            //            errMsg += errCompound;

            if ($('#btnSave').val() == 'CLICK THE BUTTON TO DISABLE THE RATES-ID') {
                if ($('#MainContent_ddlRatesID option:selected').val() == 'CHOOSE' || $('#MainContent_ddlRatesID option:selected').val() == '')
                    errMsg += 'Choose rates-id for disable<br/>';
            }
            else if ($('#txtRatesID').val() == $('#MainContent_ddlRatesID option:selected').text())
                errMsg += 'Enter New Rates-ID<br/>';

            if (errMsg.length > 0) {
                $('#lblErrMsg').html(errMsg);
                gotoPreviewDiv('lblErrMsg');
                return false;
            }
            else
                return true;
        }

        function gotoPreviewDiv(ctrlID) {
            $("html, body").stop().animate({ scrollLeft: $('#' + ctrlID).offset().left, scrollTop: $('#' + ctrlID).offset().top }, 1200);
        }
    </script>
</asp:Content>
