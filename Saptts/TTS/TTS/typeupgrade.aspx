<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="typeupgrade.aspx.cs" Inherits="TTS.typeupgrade" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .Initial
        {
            display: block;
            padding: 4px 18px 4px 18px;
            float: left;
            color: #000;
            font-weight: bold;
            margin-right: 10px;
        }
        .Initial:hover
        {
            color: #5f94f7;
            cursor: pointer;
        }
        .Clicked
        {
            float: left;
            display: block;
            padding: 4px 18px 4px 18px;
            font-weight: bold;
            color: #fff;
            margin-right: 10px;
            background-color: #099690;
        }
        .spanleftright:after
        {
            font-size: 25px;
            content: ' \21C4';
        }
        .spanupdown:after
        {
            font-size: 25px;
            content: ' \21C5';
        }
        .spanright:after
        {
            font-size: 25px;
            content: ' \2192';
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        <asp:Label runat="server" ID="lblPageHead" ClientIDMode="Static" Text=""></asp:Label>
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="background-color: #ffffff; width: 100%;
            border-color: White; border-collapse: separate;">
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" Font-Bold="true"
                        ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button Text="TYPE POSITION" BorderStyle="None" ID="btnPosition" CssClass="Initial"
                        runat="server" OnClick="Tab_Click" />
                    <asp:Button Text="TYPE SUBSTITUTION" BorderStyle="None" ID="btnSubsti" CssClass="Initial"
                        runat="server" OnClick="Tab_Click" />
                    <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                        <asp:View ID="Tab1" runat="server">
                            <table cellspacing="0" rules="all" border="1" style="background-color: #ffffff; width: 100%;
                                border-color: #ffffff; border-collapse: separate;">
                                <tr style="text-align: center; font-weight: bold; background-color: #d5f9c2;">
                                    <td>
                                        CATEGORY
                                    </td>
                                    <td>
                                        PROCESS-ID TYPE
                                    </td>
                                    <td>
                                        <span class="spanright"></span>
                                    </td>
                                    <td>
                                        TYPE SEQUENCE
                                    </td>
                                    <td>
                                        <span class="spanupdown"></span>
                                    </td>
                                    <td>
                                        ACTION
                                    </td>
                                </tr>
                                <tr style="vertical-align: middle; background-color: #f9f8d4;">
                                    <td>
                                        <asp:RadioButtonList runat="server" ID="rdbPositionCategory" ClientIDMode="Static"
                                            Width="80px" OnSelectedIndexChanged="rdbPositionCategory_IndexChange" AutoPostBack="true">
                                            <asp:ListItem Text="SOLID" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="POB" Value="2"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td align="center">
                                        <asp:ListBox ID="lstType" runat="server" ClientIDMode="Static" Width="200px" Height="420px"
                                            SelectionMode="Multiple"></asp:ListBox>
                                    </td>
                                    <td style="text-align: center;">
                                        <input type="button" id="btnMove" clientidmode="Static" value=">" style="width: 50px;
                                            font-size: 18px;" /><br />
                                    </td>
                                    <td align="center">
                                        <asp:ListBox ID="lstTypePosition" runat="server" ClientIDMode="Static" Width="200px"
                                            Height="420px"></asp:ListBox>
                                    </td>
                                    <td style="text-align: center;">
                                        <input type="button" id="btnPositionUp" clientidmode="Static" value="&uarr;" style="width: 50px;
                                            font-weight: bold; font-size: 18px;" /><br />
                                        <br />
                                        <input type="button" id="btnPositionDown" clientidmode="Static" value="&darr;" style="width: 50px;
                                            font-weight: bold; font-size: 18px;" /><br />
                                    </td>
                                    <td style="text-align: center;">
                                        <span class="btn btn-warning" style="width: 77px;" onclick="PageRelaod()">CLEAR</span>
                                        <br />
                                        <br />
                                        <asp:Button runat="server" ID="btnPositionSave" ClientIDMode="Static" Text="SAVE"
                                            CssClass="btn btn-success" OnClientClick="javascript:return CtrlSaveClick('lstTypePosition');"
                                            OnClick="btnPositionSave_Click" Width="72px" />
                                    </td>
                                </tr>
                            </table>
                        </asp:View>
                        <asp:View ID="Tab2" runat="server">
                            <table cellspacing="0" rules="all" border="1" style="background-color: #ffffff; width: 100%;
                                border-color: #ffffff; border-collapse: separate;">
                                <tr style="text-align: center; font-weight: bold; background-color: #ffd6d6;">
                                    <td>
                                        CATEGORY
                                    </td>
                                    <td>
                                        STOCK
                                    </td>
                                    <td>
                                        MASTER TYPE
                                    </td>
                                    <td>
                                        PROCESS-ID TYPE
                                    </td>
                                    <td>
                                        <span class="spanleftright"></span>
                                    </td>
                                    <td>
                                        UPGRADE SEQUENCE
                                    </td>
                                    <td>
                                        <span class="spanupdown"></span>
                                    </td>
                                    <td>
                                        ACTION
                                    </td>
                                </tr>
                                <tr style="vertical-align: middle; background-color: #ececec;">
                                    <td>
                                        <asp:RadioButtonList runat="server" ID="rdbSubstiCategory" ClientIDMode="Static"
                                            Width="80px" OnSelectedIndexChanged="rdbSubstiCategory_IndexChange" AutoPostBack="true">
                                            <asp:ListItem Text="SOLID" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="POB" Value="2"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList runat="server" ID="rdb_StockMethod" ClientIDMode="Static" Width="100px"
                                            AutoPostBack="true" OnSelectedIndexChanged="rdb_StockMethod_IndexChange">
                                            <asp:ListItem Text="CURRENT" Value="CURRENT"></asp:ListItem>
                                            <asp:ListItem Text="GSA" Value="GSA"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td align="center">
                                        <asp:DropDownList runat="server" ID="ddl_MasterType" ClientIDMode="Static" CssClass="form-control"
                                            Width="100px" AutoPostBack="true" OnSelectedIndexChanged="ddl_MasterType_IndexChange">
                                        </asp:DropDownList>
                                    </td>
                                    <td align="center">
                                        <asp:ListBox ID="list1" runat="server" ClientIDMode="Static" Width="200px" Height="420px"
                                            SelectionMode="Multiple"></asp:ListBox>
                                    </td>
                                    <td style="text-align: center;">
                                        <input type="button" id="btnAdd" clientidmode="Static" value=">" style="width: 50px;
                                            font-size: 18px;" /><br />
                                        <br />
                                        <input type="button" id="btnAddAll" clientidmode="Static" value=">>" style="width: 50px;
                                            font-size: 18px;" /><br />
                                        <br />
                                        <input type="button" id="btnRemove" clientidmode="Static" value="<" style="width: 50px;
                                            font-size: 18px;" /><br />
                                        <br />
                                        <input type="button" id="btnRemoveAll" clientidmode="Static" value="<<" style="width: 50px;
                                            font-size: 18px;" />
                                    </td>
                                    <td align="center">
                                        <asp:ListBox ID="list2" runat="server" ClientIDMode="Static" Width="200px" Height="420px">
                                        </asp:ListBox>
                                    </td>
                                    <td style="text-align: center;">
                                        <input type="button" id="btnUp" clientidmode="Static" value="&uarr;" style="width: 50px;
                                            font-weight: bold; font-size: 18px;" /><br />
                                        <br />
                                        <input type="button" id="btnDown" clientidmode="Static" value="&darr;" style="width: 50px;
                                            font-weight: bold; font-size: 18px;" /><br />
                                    </td>
                                    <td style="text-align: center;">
                                        <span class="btn btn-warning" style="width: 77px;" onclick="PageRelaod()">CLEAR</span>
                                        <br />
                                        <br />
                                        <asp:Button runat="server" ID="btnUpgradeSave" ClientIDMode="Static" Text="SAVE"
                                            CssClass="btn btn-success" OnClientClick="javascript:return CtrlSaveClick('list2');"
                                            OnClick="btnUpgradeSave_Click" Width="72px" />
                                    </td>
                                </tr>
                            </table>
                        </asp:View>
                    </asp:MultiView>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnTypePosition" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        $(document).ready(function () {
            $('#btnMove').click(function (e) {
                var selectedOpts = $('#lstType option:selected');
                if (selectedOpts.length == 0) {
                    alert("Nothing to move.");
                    e.preventDefault();
                }
                $('#lstTypePosition').append($(selectedOpts).clone());
                var idx = $('#lstTypePosition option').length - 1;
                $("#lstTypePosition option:eq(" + idx + ")").attr('selected', 'selected');
                $(selectedOpts).remove();
                e.preventDefault();
            });
            $('#btnPositionUp').click(function (e) { ItemsUpDown('Up', 'lstTypePosition'); });
            $('#btnPositionDown').click(function (e) { ItemsUpDown('Down', 'lstTypePosition'); });

            $('#btnAdd').click(function (e) {
                var selectedOpts = $('#list1 option:selected');
                if (selectedOpts.length == 0) {
                    alert("Nothing to move.");
                    e.preventDefault();
                }
                $('#list2').append($(selectedOpts).clone());
                $(selectedOpts).remove();
                e.preventDefault();
            });

            $('#btnAddAll').click(function (e) {
                var selectedOpts = $('#list1 option');
                if (selectedOpts.length == 0) {
                    alert("Nothing to move.");
                    e.preventDefault();
                }
                $('#list2').append($(selectedOpts).clone());
                $(selectedOpts).remove();
                e.preventDefault();
            });

            $('#btnRemove').click(function (e) {
                var selectedOpts = $('#list2 option:selected');
                if (selectedOpts.length == 0) {
                    alert("Nothing to move.");
                    e.preventDefault();
                }
                $('#list1').append($(selectedOpts).clone());
                $(selectedOpts).remove();
                e.preventDefault();
            });

            $('#btnRemoveAll').click(function (e) {
                var selectedOpts = $('#list2 option');
                if (selectedOpts.length == 0) {
                    alert("Nothing to move.");
                    e.preventDefault();
                }
                $('#list1').append($(selectedOpts).clone());
                $(selectedOpts).remove();
                e.preventDefault();
            });

            $('#btnUp').click(function (e) { ItemsUpDown('Up', 'list2'); });
            $('#btnDown').click(function (e) { ItemsUpDown('Down', 'list2'); });
        });

        function ItemsUpDown(eve, ctrl) {
            var selectedOption = $('#' + ctrl + ' option:selected');
            if (selectedOption.length == 1) {
                if (eve == 'Down') {
                    var nextOption = $('#' + ctrl + ' option:selected').next("option");
                    if ($(nextOption).text() != "") {
                        $(selectedOption).remove();
                        $(nextOption).after($(selectedOption));
                    }
                }
                else if (eve == 'Up') {
                    var prevOption = $('#' + ctrl + ' option:selected').prev("option");
                    if ($(prevOption).text() != "") {
                        $(selectedOption).remove();
                        $(prevOption).before($(selectedOption));
                    }
                }
            }
            else
                alert('Nothing to ' + eve);
        }

        function PageRelaod() { window.location.href = window.location.href; }

        function CtrlSaveClick(CtrlLst) {
            var strType = ''; $('#hdnTypePosition').val(''); $('#lblErrMsg').html(''); var errmsg = '';
            for (var k = 0; k < $('#' + CtrlLst + ' option').length; k++) {
                strType = strType == '' ? $('#' + CtrlLst + ' option:eq(' + k + ')').text() : strType + ',' + $('#' + CtrlLst + ' option:eq(' + k + ')').text();
            }
            $('#hdnTypePosition').val(strType);
            if (CtrlLst == 'lstTypePosition') {
                if ($('input:radio[id*=rdbPositionCategory_]:checked').length == 0)
                    errmsg += 'Choose category for type position<br/>';
            }
            else if (CtrlLst == 'list2') {
                if ($('input:radio[id*=rdbSubstiCategory_]:checked').length == 0)
                    errmsg += 'Choose category for type substitution<br/>';
                if ($('input:radio[id*=rdb_StockMethod_]:checked').length == 0)
                    errmsg += 'Choose stock mode<br/>';
                if ($('#ddl_MasterType option:selected').length == 0 || $('#ddl_MasterType option:selected').text() == 'CHOOSE')
                    errmsg += 'Choose master type<br/>';
            }

            if ($('#hdnTypePosition').val().length == 0)
                errmsg += 'No records in the sequence list';

            if (errmsg.length > 0) {
                $('#lblErrMsg').html(errmsg);
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
