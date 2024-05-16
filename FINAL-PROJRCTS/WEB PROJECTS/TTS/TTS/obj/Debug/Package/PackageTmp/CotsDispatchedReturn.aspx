<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CotsDispatchedReturn.aspx.cs"
    MasterPageFile="~/master.Master" Inherits="TTS.CotsDispatchedReturn" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .HeadSpan
        {
            color: Green;
            font-size: 12px;
            font-weight: bold;
        }
        .button
        {
            background-color: #4CAF50;
            border: none;
            color: white;
            padding: 10px 25px;
            text-align: center;
            text-decoration: none;
            display: inline-block;
            font-size: 16px;
            margin: 4px 2px;
            cursor: pointer;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        ORDER ITEM REVISE</div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <!-- Selection  -->
        <div>
            <!-- DropDown Selection  -->
            <table cellpadding="10">
                <tr>
                    <td>
                        <span class="HeadSpan">Select CustomerName</span><br />
                        <asp:DropDownList ID="ddl_CustomerName" Height="30px" runat="server" AutoPostBack="true"
                            Width="400px" OnSelectedIndexChanged="ddl_CustomerName_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <span class="HeadSpan">Select UserId</span><br />
                        <asp:DropDownList ID="ddl_UserId" Height="30px" runat="server" Width="200px" AutoPostBack="true"
                            OnSelectedIndexChanged="ddl_UserId_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <span class="HeadSpan">Select OrderRefNo</span><br />
                        <asp:DropDownList ID="ddl_OrderRefNo" Height="30px" runat="server" AutoPostBack="true"
                            Width="350px" OnSelectedIndexChanged="ddl_OrderRefNo_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </div>
        <hr />
        <!-- Textbox Selection  -->
        <div id="div_QtySelection" style="display: none;">
            <div style="float: left; width: 534px; text-align: center;">
                <span class="HeadSpan" style="color: #b55117;">Dispatched Item Quantity:</span>
                &nbsp;
                <asp:Label ID="lblDispatchedQty" ClientIDMode="Static" runat="server" Font-Bold="true"></asp:Label>
            </div>
            <div style="float: right; width: 534px; text-align: center;">
                <span class="HeadSpan" style="color: #b55117;">Quantity to Revise:</span> &nbsp;
                <asp:TextBox ID="txtReviseQty" TextMode="Number" ClientIDMode="Static" runat="server"
                    Font-Bold="true"></asp:TextBox>
            </div>
        </div>
        <br />
        <!-- OrderItem GridViewControl  -->
        <div id="div_GvOrderDetails" style="display: none;">
            <asp:GridView ID="gvCustOrderItem" runat="server" ClientIDMode="Static" AutoGenerateColumns="false"
                AlternatingRowStyle-BackColor="#c9d6c1" BackColor="White" RowStyle-Height="20px"
                Width="1068px" HeaderStyle-Font-Size="14px">
                <HeaderStyle BackColor="#a5e27f" Font-Bold="true" Height="22px" />
                <Columns>
                    <asp:TemplateField ItemStyle-Width="80px" HeaderText="QTY SELECTION">
                        <ItemTemplate>
                            <asp:CheckBox ID="chk_selectQty" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="BARCODE" DataField="barcode" ItemStyle-Width="250px" />
                    <asp:BoundField HeaderText="PROCESS ID" DataField="ProcessId" ItemStyle-Width="80px" />
                    <asp:BoundField HeaderText="CONFIG" DataField="config" ItemStyle-Width="80px" />
                    <asp:BoundField HeaderText="SIZE" DataField="tyresize" ItemStyle-Width="150px" />
                    <asp:BoundField HeaderText="RIM" DataField="rimsize" ItemStyle-Width="80px" />
                    <asp:BoundField HeaderText="TYPE" DataField="tyretype" ItemStyle-Width="80px" />
                    <asp:BoundField HeaderText="BRAND" DataField="brand" ItemStyle-Width="80px" />
                    <asp:BoundField HeaderText="SIDEWALL" DataField="sidewall" ItemStyle-Width="80px" />
                </Columns>
            </asp:GridView>
        </div>
        <!-- Comments MultiTextbox & ButtonControl  -->
        <div id="div_Comments" style="width: 1068px; display: none;">
            <div>
                <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" Font-Bold="true"
                    Font-Size="12px" ForeColor="Red"></asp:Label>
            </div>
            <div>
                <span class="HeadSpan" style="color: #b55117; text-align: left;">Comments:</span><br />
                <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" ClientIDMode="Static"
                    Width="1068px" Height="80px"></asp:TextBox>
            </div>
            <div style="text-align: center;">
                <asp:Button ID="btnSendRevise" CssClass="button" Text="Send Revise" runat="server"
                    OnClientClick="javascript:return CtrlSave()" OnClick="btnSendRevise_Click" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnClear" ClientIDMode="Static" CssClass="button" Text="Clear" runat="server" />
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            var maxlength = $('#lblDispatchedQty').html();
            //restrict textbox using maxlength
            $('#txtReviseQty').on("input", function () {
                var value = $(this).val();
                if (value !== '') {
                    $(this).val(Math.max(Math.min(value, maxlength), 0));
                }
            });
            //show GridView after qty selection
            $('#txtReviseQty').change(function () {
                var length = $('#txtReviseQty').length;
                if (length <= maxlength)
                    $('#div_GvOrderDetails').css({ 'display': 'block' });
                else
                    $('#div_GvOrderDetails').css({ 'display': 'none' });
            });
            //Disable unwanted checkbox
            $('input:checkbox').on('change', function () {
                var checkedCount = $('input:checked[type=checkbox]').length;
                var length = $('#txtReviseQty').length;
                if (checkedCount >= length) {
                    $('input:checkbox').each(function () {
                        if ($(this).is(':checked'))
                            return;
                        else
                            $(this).prop('disabled', true);
                    });
                }
                else {
                    $('input:checkbox').each(function () {
                        $(this).prop('disabled', false);
                    });
                }
            });
            //Show Comments box after check
            $('input:checkbox').change(function () {
                var checkedCount = $('input:checked[type=checkbox]').length;
                if (checkedCount > 0)
                    $('#div_Comments').css({ 'display': 'block' });
                else
                    $('#div_Comments').css({ 'display': 'none' });
            });
            //Reload the page
            $('#btnClear').click(function () {
                location.reload(true);
            });
        });
        //Button check
        function CtrlSave() {
            $('#lblErrMsg').html('');
            if ($('#txtComments').val().length == 0) {
                $('#lblErrMsg').html('Please Fill comments');
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
