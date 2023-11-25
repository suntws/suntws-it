<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="cotsdomdebtors1.aspx.cs" Inherits="TTS.cotsdomdebtors1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/datemonthyear.css" rel="stylesheet" type="text/css" />
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .tableCss
        {
            width: 100%;
            background-color: #dcecfb;
            border-color: White;
            border-collapse: separate;
        }
        .tableCss th
        {
            font-weight: bold;
            text-align: center;
            color: #ffffff;
            background-color: #2abac1;
        }
        .tableCss td
        {
            font-weight: 500;
            text-align: left;
            padding-left: 5px;
        }
        #btnSave
        {
            padding: 5px 10px;
            font-size: 12px;
            line-height: 1.5;
            border-radius: 3px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        <asp:Label runat="server" ID="lblPageTitle" ClientIDMode="Static" Text=""></asp:Label>
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="background-color: #ffffff; width: 100%;
            border-color: White; border-collapse: separate;" id="tbDebtorsPage">
            <tr>
                <td>
                    <div id="divtable1">
                        <table style="border: 1px solid #000; width: 100%; border-collapse: collapse;" cellspacing="0"
                            rules="all" border="1" class="tableCss">
                            <tr>
                                <th>
                                    Customer
                                </th>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlCustName" ClientIDMode="Static" CssClass="form-control"
                                        Width="120px" AutoPostBack="true" OnSelectedIndexChanged="ddlCustName_indexchanged">
                                        <asp:ListItem>--SELECT--</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <th>
                                    User Id
                                </th>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlUserId" ClientIDMode="Static" CssClass="form-control"
                                        Width="120px" AutoPostBack="true" OnSelectedIndexChanged="ddlUserId_indexchanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    CRM
                                </th>
                                <td>
                                    <asp:Label runat="server" ID="lblCrm" ClientIDMode="Static" Text=""></asp:Label>
                                </td>
                                <th>
                                    Today Received Amt
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="txtTodayRecAmt" ClientIDMode="Static" Text=""></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    Category
                                </th>
                                <td>
                                    <asp:Label runat="server" ID="lblCateg" ClientIDMode="Static" Text=""></asp:Label>
                                </td>
                                <th>
                                    Ref Details
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="txtRefDet" ClientIDMode="Static" Text=""></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    Region
                                </th>
                                <td>
                                    <asp:Label runat="server" ID="lblRegion" ClientIDMode="Static" Text=""></asp:Label>
                                </td>
                                <th>
                                    Comments
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="txtComment" ClientIDMode="Static" Text=""></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    Credit Days
                                </th>
                                <td>
                                    <asp:Label runat="server" ID="lblCreDays" ClientIDMode="Static" Text=""></asp:Label>
                                </td>
                                <th>
                                    Received On
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="txtDatepick" ClientIDMode="Static" Text=""></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    Credit Limit
                                </th>
                                <td colspan="3">
                                    <asp:Label runat="server" ID="lblCreLimit" ClientIDMode="Static" Text=""></asp:Label>
                                </td>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="divGridview">
                        <asp:GridView runat="server" ID="gvInvDebtors" AutoGenerateColumns="false" Width="100%"
                            CssClass="gridcss">
                            <Columns>
                                <asp:TemplateField HeaderText="CHOOSE" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chkSelect" onclick="chkFindIndex(this)"></asp:CheckBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="INVOICE NO" DataField="Invoice_No" />
                                <asp:BoundField HeaderText="INVOICE DATE" DataField="Invoice_Date" />
                                <asp:BoundField HeaderText="AMOUNT" DataField="Amount" />
                                <asp:BoundField HeaderText="PENDING" DataField="Pending" />
                                <asp:BoundField HeaderText="DUE ON" DataField="Due_On" />
                                <asp:BoundField HeaderText="CREDIT DAYS" DataField="Credit_Days" />
                                <asp:BoundField HeaderText="DUE DAYS" DataField="Due_Days" />
                                <asp:BoundField HeaderText="OVER DUE" DataField="Over_Due" />
                                <asp:BoundField HeaderText="DUE AMOUNT" DataField="Due_Amt" />
                                <asp:TemplateField HeaderText="RECEIVED AMOUNT" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:HiddenField runat="server" ID="hdnPending" Value='<%# Eval("Pending")%>' />
                                        <asp:TextBox runat="server" ID="txtRecAmt" disabled="disabled" Text="0"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="PENDING AMOUNT" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblPendAmt" Text="0"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="text-align: center;">
                        <asp:Button runat="server" ID="btnSave" ClientIDMode="Static" Text="Save" OnClick="btnSave_click"
                            OnClientClick="javascript:return CtrlFilter();" Visible="false" BackColor="#CC6699" />
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnTodayRecAmt" ClientIDMode="Static" Value="" />
    <script src="Scripts/datemonthyear.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CtrlFilter() {
            var ErrMsg = '';
            $('#lblErrMsgcontent').html('');
            if ($("#txtTodayRecAmt").val() == '')
                ErrMsg += 'Today Received Amount should not be left Empty <br/>';
            if ($("#txtRefDet").val() == '')
                ErrMsg += 'Reference Details should not be left Empty <br/>';
            if ($("#txtComment").val() == '')
                ErrMsg += 'Give some Comments about the payment <br/>';
            if ($("#txtDatepick").val() == '')
                ErrMsg += 'Select Received Date <br/>';
            if ($("input:checkbox[id*=MainContent_gvInvDebtors_chkSelect_]:checked").length == 0)
                ErrMsg += 'Choose atleast one Invoice <br/>';
            if (ErrMsg.length > 0) {
                $('#lblErrMsgcontent').html(ErrMsg)
                return false;
            }
            else
                return true;
        }
        $(function () {
            $("#txtDatepick").datepicker({ minDate: "-5D", maxDate: "0D" }).keydown(function (e) { e.keyWhich = 0; e.keyCode = 0; e.preventDefault(); });
            $("#txtTodayRecAmt").blur(function () {
                $("#hdnTodayRecAmt").val('');
                $("#hdnTodayRecAmt").val($("#txtTodayRecAmt").val());
                chkDisableEnable();
            });
            $("#txtTodayRecAmt").click(function () { $('#lblErrMsgcontent').html(''); });
            $("#txtRefDet").click(function () { $('#lblErrMsgcontent').html(''); });
            $("#txtComment").click(function () { $('#lblErrMsgcontent').html(''); });
            $("#txtDatepick").click(function () { $('#lblErrMsgcontent').html(''); });
        });
        function chkFindIndex(i) {
            var arr = ($(i).attr('ID')).split("_");
            var ind = arr[arr.length - 1];

            if ($("#MainContent_gvInvDebtors_chkSelect_" + ind).attr('checked') == 'checked') {
                $('#lblErrMsgcontent').html('');
                $("#MainContent_gvInvDebtors_txtRecAmt_" + ind).removeAttr('disabled');
                if ($("#MainContent_gvInvDebtors_hdnPending_" + ind).val() >= $("#hdnTodayRecAmt").val()) {
                    $("#MainContent_gvInvDebtors_txtRecAmt_" + ind).val($("#hdnTodayRecAmt").val());
                }
                if ($("#MainContent_gvInvDebtors_hdnPending_" + ind).val() < $("#hdnTodayRecAmt").val()) {
                    $("#MainContent_gvInvDebtors_txtRecAmt_" + ind).val($("#MainContent_gvInvDebtors_hdnPending_" + ind).val())
                }
                $("#MainContent_gvInvDebtors_txtRecAmt_" + ind).focus();
                txtchanged(ind);
            }
            if ($("#MainContent_gvInvDebtors_chkSelect_" + ind).attr('checked') != 'checked') {
                $("#hdnTodayRecAmt").val(parseInt($("#hdnTodayRecAmt").val()) + parseInt($("#MainContent_gvInvDebtors_txtRecAmt_" + ind).val()));
                $("#MainContent_gvInvDebtors_hdnPending_" + ind).val(parseInt($("#MainContent_gvInvDebtors_hdnPending_" + ind).val()) + parseInt($("#MainContent_gvInvDebtors_lblPendAmt_" + ind).text()));
                $("#MainContent_gvInvDebtors_txtRecAmt_" + ind).attr("disabled", 'disabled');
                $("#MainContent_gvInvDebtors_txtRecAmt_" + ind).val('0');
                $("#MainContent_gvInvDebtors_lblPendAmt_" + ind).text('0');
            }
        }
        function txtchanged(index) {
            $("#MainContent_gvInvDebtors_txtRecAmt_" + index).blur(function () {
                $("#MainContent_gvInvDebtors_lblPendAmt_" + index).text(parseInt($("#MainContent_gvInvDebtors_hdnPending_" + index).val()) - parseInt($("#MainContent_gvInvDebtors_txtRecAmt_" + index).val()));
                $("#MainContent_gvInvDebtors_hdnPending_" + index).val($("#MainContent_gvInvDebtors_lblPendAmt_" + index).text());
                $("#hdnTodayRecAmt").val($("#hdnTodayRecAmt").val() - $("#MainContent_gvInvDebtors_txtRecAmt_" + index).val());

                if ($("#hdnTodayRecAmt").val() == '' || $("#hdnTodayRecAmt").val() == 0) {
                    if ($("input:checkbox[id*=MainContent_gvInvDebtors_chkSelect_]:checked").length == 0)
                        $("input:checkbox[id*=MainContent_gvInvDebtors_chkSelect_]").attr("disabled", 'disabled');
                } else {
                    $("input:checkbox[id*=MainContent_gvInvDebtors_chkSelect_]").removeAttr("disabled");
                }
            });
        }
        function chkDisableEnable() {
            if ($("#txtTodayRecAmt").val() == '' || $("#txtTodayRecAmt").val() == '0') {
                $('#lblErrMsgcontent').html('Enter Valid Amount <br/>');
                $("input:checkbox[id*=MainContent_gvInvDebtors_chkSelect_]").attr("disabled", true);
            }
            if ($("#txtTodayRecAmt").val() != '' || $("#txtTodayRecAmt").val() > '0') {
                $("input:checkbox[id*=MainContent_gvInvDebtors_chkSelect_]").removeAttr("disabled");
            }
        }
    
    </script>
</asp:Content>
