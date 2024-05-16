<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="StockFilteredReport_V1.aspx.cs" Inherits="TTS.StockFilteredReport_V1" %>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
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
            color: #ffffff;
            font-weight: normal;
            text-align: center;
            border: 1px solid #a0a21c;
            width: 6%;
        }
        .tableCss td
        {
            text-align: left;
            width: 90%;
        }
        .tableCss1
        {
            background-color: #bcd3d6;
            width: 100%;
            border-color: White;
            border-collapse: separate;
        }
        .tableCss1 td
        {
            text-align: center;
            width: 12.5%;
            border-radius: 20px;
            height: 22px;
        }
        .tableCss1 td:hover
        {
            background-color: #f7abf5 !important;
        }
        
        /* label,span css -- for heading */
        .spanCss
        {
            font-size: 13px;
            font-style: italic;
            color: #006600;
            font-weight: bold;
        }
        .spanCss-filter
        {
            font-size: 12px;
            color: Black;
            font-weight: bold;
            width: 100%;
            padding: 5px 30px 5px 30px;
        }
        .spanCss-filter:hover
        {
            cursor: pointer;
            font-weight: bold;
        }
        .divContent
        {
            overflow: auto;
            max-height: 550px;
            position: relative;
            left: 1px;
            top: 5px;
            width: 100%;
        }
        #gvStockDetails .paging a
        {
            text-decoration: none;
        }
        #gvStockDetails .paging span
        {
            background-color: #000;
            color: #ffffff;
        }
        #gvStockDetails .paging:hover
        {
            background-color: #E4F7CF !important;
        }
        #gvStockDetails .paging td
        {
            border: 1px solid #000;
            width: 20px;
            border-radius: 2px;
            text-align: center;
        }
        
        .recCss
        {
            float: left;
            background-color: #075865;
            color: #fff;
            font-size: 14px;
            font-weight: bold;
            text-align: center;
        }
        .badge-Platform
        {
            background-color: #b3f5c2;
        }
        .badge-Brand
        {
            background-color: #cbd8e4;
        }
        .badge-Sidewall
        {
            background-color: #a3edf5;
        }
        .badge-Tyretype
        {
            background-color: #fbfbdf;
        }
        .badge-Tyresize
        {
            background-color: #cbe6df;
        }
        .badge-Rimsize
        {
            background-color: #fbe1e1;
        }
        .badge-Grade
        {
            background-color: #dcf7b9;
        }
        .badge-Year
        {
            background-color: #f2d0f3;
        }
        span.badge
        {
            font-size: 12px;
            padding: 6px 3px 3px 6px;
        }
        .filter-content
        {
            max-height: 200px;
            overflow: auto;
        }
        .spnDel
        {
            vertical-align: super;
            color: Red;
            text-align: right;
            height: 10px;
            width: 10px;
            cursor: pointer;
            padding-left: 4px;
            font-weight: bold;
        }
        #lnkbtnApplyFilters
        {
            background-color: #ffffff;
            border: 1px solid #1b9c4d;
            color: #1b9c4d;
            padding: 5px;
            font-weight: bold;
            text-align: center;
            width: 100px;
            display: none;
            border-radius: 10px;
            text-decoration: none;
        }
        #lnkbtnApplyFilters:hover
        {
            background-color: #1b9c4d;
            color: #ffffff;
        }
        #lnkbtnClearFilter
        {
            background-color: #ffffff;
            border: 1px solid #b14c11;
            color: #b14c11;
            padding: 5px;
            text-align: center;
            font-weight: bold;
            width: 100px;
            display: none;
            border-radius: 10px;
            text-decoration: none;
        }
        #lnkbtnClearFilter:hover
        {
            background-color: #b14c11;
            color: #ffffff;
        }
        #lnkbtnDownload
        {
            background-color: #ffffff;
            border: 1px solid #b70a84;
            color: #b70a84;
            padding: 5px;
            text-align: center;
            font-weight: bold;
            width: 100px;
            display: none;
            border-radius: 10px;
            text-decoration: none;
        }
        #lnkbtnDownload:hover
        {
            background-color: #b70a84;
            color: #ffffff;
        }
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager runat="server" ID="scriptManager1">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        <asp:Label runat="server" ID="lblPageTitle" ClientIDMode="Static"></asp:Label>
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="background-color: #dcecfb; width: 100%;
            border-color: White; border-collapse: separate;">
            <tr>
                <td style="background-color: #4293da; color: #ffffff; text-align: center; font-weight: bold;
                    font-size: 14px;">
                    Filters
                </td>
                <td>
                    <span style="font-size: 13px; font-style: italic; color: #006600; width: 250px; float: left;
                        line-height: 25px; padding-left: 220px;">Stock Report With Stencil No.</span>
                    <span>
                        <asp:RadioButtonList ID="rdblStencil" runat="server" ClientIDMode="Static" Width="100px"
                            RepeatColumns="2">
                            <asp:ListItem Text="Yes" Value="1" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                        </asp:RadioButtonList>
                    </span>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div>
                        <table class="tableCss1" border="1">
                            <tr>
                                <td id="tdPlatform">
                                    <span id="lblPlatform" class="spanCss-filter" onclick="filterClick(this)">Platform</span>
                                </td>
                                <td id="tdBrand">
                                    <span id="lblBrand" class="spanCss-filter" onclick="filterClick(this)">Brand</span>
                                </td>
                                <td id="tdSidewall">
                                    <span id="lblSidewall" class="spanCss-filter" onclick="filterClick(this)">Sidewall</span>
                                </td>
                                <td id="tdTyretype">
                                    <span id="lblTyretype" class="spanCss-filter" onclick="filterClick(this)">Tyre Type</span>
                                </td>
                                <td id="tdTyresize">
                                    <span id="lblTyresize" class="spanCss-filter" onclick="filterClick(this)">Tyre Size</span>
                                </td>
                                <td id="tdRimsize">
                                    <span id="lblRimsize" class="spanCss-filter" onclick="filterClick(this)">Rim Width</span>
                                </td>
                                <td id="tdGrade">
                                    <span id="lblGrade" class="spanCss-filter" onclick="filterClick(this)">Grade</span>
                                </td>
                                <td id="tdYear">
                                    <span id="lblYear" class="spanCss-filter" onclick="filterClick(this)">Year</span>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div id="divPlatform" class="filter-content" style="display: none;">
                        <asp:CheckBoxList runat="server" ID="chkPlatform" ClientIDMode="Static" RepeatColumns="6"
                            RepeatDirection="Horizontal" Width="100%" onclick="bindFilterCateg(this)">
                        </asp:CheckBoxList>
                    </div>
                    <div id="divBrand" class="filter-content" style="display: none;">
                        <asp:CheckBoxList runat="server" ID="chkBrand" ClientIDMode="Static" RepeatColumns="6"
                            RepeatDirection="Horizontal" Width="100%" onclick="bindFilterCateg(this)">
                        </asp:CheckBoxList>
                    </div>
                    <div id="divSidewall" class="filter-content" style="display: none;">
                        <asp:CheckBoxList runat="server" ID="chkSidewall" ClientIDMode="Static" RepeatColumns="6"
                            RepeatDirection="Horizontal" Width="100%" onclick="bindFilterCateg(this)">
                        </asp:CheckBoxList>
                    </div>
                    <div id="divTyretype" class="filter-content" style="display: none;">
                        <asp:CheckBoxList runat="server" ID="chkTyretype" ClientIDMode="Static" RepeatColumns="6"
                            RepeatDirection="Horizontal" Width="100%" onclick="bindFilterCateg(this)">
                        </asp:CheckBoxList>
                    </div>
                    <div id="divTyresize" class="filter-content" style="display: none;">
                        <asp:CheckBoxList runat="server" ID="chkTyresize" ClientIDMode="Static" RepeatColumns="6"
                            RepeatDirection="Horizontal" Width="100%" onclick="bindFilterCateg(this)">
                        </asp:CheckBoxList>
                    </div>
                    <div id="divRimsize" class="filter-content" style="display: none;">
                        <asp:CheckBoxList runat="server" ID="chkRimsize" ClientIDMode="Static" RepeatColumns="6"
                            RepeatDirection="Horizontal" Width="100%" onclick="bindFilterCateg(this)">
                        </asp:CheckBoxList>
                    </div>
                    <div id="divGrade" class="filter-content" style="display: none;">
                        <asp:CheckBoxList runat="server" ID="chkGrade" ClientIDMode="Static" RepeatColumns="6"
                            RepeatDirection="Horizontal" Width="100%" onclick="bindFilterCateg(this)">
                        </asp:CheckBoxList>
                    </div>
                    <div id="divYear" class="filter-content" style="display: none;">
                        <asp:CheckBoxList runat="server" ID="chkYear" ClientIDMode="Static" RepeatColumns="6"
                            RepeatDirection="Horizontal" Width="100%" onclick="bindFilterCateg(this)">
                        </asp:CheckBoxList>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div class="divContent">
                        <div style="width: 100%; font-size: 12px;">
                            <div id="tr_Select_Platform" style="display: none;">
                                <table class="tableCss" border="1px" cellpadding="3">
                                    <tr>
                                        <th>
                                            <span class="spanCss">Platform</span>
                                        </th>
                                        <td>
                                            <div id="div_Platform" runat="server">
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="tr_Select_Brand" style="display: none;">
                                <table class="tableCss" border="1px" cellpadding="3">
                                    <tr>
                                        <th>
                                            <span class="spanCss">Brand</span>
                                        </th>
                                        <td>
                                            <div id="div_Brand" runat="server">
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="tr_Select_Sidewall" style="display: none;">
                                <table class="tableCss" border="1px" cellpadding="3">
                                    <tr>
                                        <th>
                                            <span class="spanCss">Sidewall</span>
                                        </th>
                                        <td>
                                            <div id="div_Sidewall" runat="server">
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="tr_Select_Tyretype" style="display: none;">
                                <table class="tableCss" border="1px" cellpadding="3">
                                    <tr>
                                        <th>
                                            <span class="spanCss">Type</span>
                                        </th>
                                        <td>
                                            <div id="div_Tyretype" runat="server">
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="tr_Select_Tyresize" style="display: none;">
                                <table class="tableCss" border="1px" cellpadding="3">
                                    <tr>
                                        <th>
                                            <span class="spanCss">Size</span>
                                        </th>
                                        <td>
                                            <div id="div_Tyresize" runat="server">
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="tr_Select_Rimsize" style="display: none;">
                                <table class="tableCss" border="1px" cellpadding="3">
                                    <tr>
                                        <th>
                                            <span class="spanCss">Rim</span>
                                        </th>
                                        <td>
                                            <div id="div_Rimsize" runat="server">
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="tr_Select_Grade" style="display: none;">
                                <table class="tableCss" border="1px" cellpadding="3">
                                    <tr>
                                        <th>
                                            <span class="spanCss">Grade</span>
                                        </th>
                                        <td>
                                            <div id="div_Grade" runat="server">
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="tr_Select_Year" style="display: none;">
                                <table class="tableCss" border="1px" cellpadding="3">
                                    <tr>
                                        <th>
                                            <span class="spanCss">Year</span>
                                        </th>
                                        <td>
                                            <div id="div_Year" runat="server">
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: center;">
                    <table style="width: 100%; background-color: #dcecfb; border-collapse: collapse;
                        border-color: #dcecfb;" border="1px">
                        <tr>
                            <td style="width: 30%;">
                                <asp:LinkButton ID="lnkbtnDownload" runat="server" ClientIDMode="Static" Text="Download"
                                    OnClick="btnDownload_Click"></asp:LinkButton>
                            </td>
                            <td style="width: 40%;">
                                <asp:LinkButton ID="lnkbtnApplyFilters" runat="server" ClientIDMode="Static" Text="Search"
                                    OnClick="btnApplyFilters_Click"></asp:LinkButton>
                            </td>
                            <td style="width: 30%;">
                                <asp:LinkButton ID="lnkbtnClearFilter" runat="server" ClientIDMode="Static" Text="Clear"
                                    OnClick="btnClearFilter_Click"></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div class="divContent">
                        <div id="divGrid" style="width: 100%; float: left;">
                            <asp:Label ID="lblNoOfRecords" runat="server" ClientIDMode="Static" Width="100%"
                                CssClass="recCss"></asp:Label>
                            <asp:GridView runat="server" AllowPaging="true" ID="gvStockDetails" PageSize="100"
                                Width="100%" CssClass="gridcss" ClientIDMode="Static" PagerStyle-BackColor="#e4f7cf"
                                PagerSettings-LastPageText="Last" PagerSettings-Mode="NumericFirstLast" PagerStyle-HorizontalAlign="Center"
                                PagerStyle-Font-Size="14px" PagerStyle-ForeColor="#000" PagerStyle-CssClass="paging"
                                ViewStateMode="Enabled" OnPageIndexChanging="gvStockList_PageIndex">
                            </asp:GridView>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
        <hr />
    </div>
    <asp:HiddenField ID="hdnPlatform" runat="server" ClientIDMode="Static" EnableViewState="true" />
    <asp:HiddenField ID="hdnBrand" runat="server" ClientIDMode="Static" EnableViewState="true" />
    <asp:HiddenField ID="hdnSidewall" runat="server" ClientIDMode="Static" EnableViewState="true" />
    <asp:HiddenField ID="hdnRimsize" runat="server" ClientIDMode="Static" EnableViewState="true" />
    <asp:HiddenField ID="hdnTyretype" runat="server" ClientIDMode="Static" EnableViewState="true" />
    <asp:HiddenField ID="hdnGrade" runat="server" ClientIDMode="Static" EnableViewState="true" />
    <asp:HiddenField ID="hdnYear" runat="server" ClientIDMode="Static" EnableViewState="true" />
    <asp:HiddenField ID="hdnstocktype" runat="server" ClientIDMode="Static" EnableViewState="true" />
    <asp:HiddenField ID="hdnTyresize" runat="server" ClientIDMode="Static" EnableViewState="true" />
    <script type="text/javascript">
        $(document).ready(function () {
            $('#rdblStencil').change(function () {
                btnFiltShow();
            });
        });
        function bindFilterCateg(e) {
            var hdnitems = "";
            var items = [];
            var filtItem = $(e).attr("id");
            filtItem = filtItem.replace("chk", "");
            $('#chk' + filtItem + ' input[type=checkbox]:checked').each(function () {
                items.push($(this).val());
                hdnitems += "'" + $(this).val() + "',";
            });
            $('#hdn' + filtItem).val(hdnitems);
            bindCateg('#tr_Select_' + filtItem, 'badge-' + filtItem, items, '#MainContent_div_' + filtItem);
            btnFiltShow();
        }

        function bindCateg(row, cssClass, itemArray, divBind) {
            if (itemArray.length > 0) {
                $(row).css('display', 'block');
                var str = "";
                for (i = 0; i < itemArray.length; i++) {
                    if (itemArray[i].length > 0) {
                        str += "<span class='badge " + cssClass + "' style='border-radius: 20px;'>" + itemArray[i] + "<span class='spnDel' onclick='delFilter(this)'>x</span> </span> &nbsp;";
                    }
                }
                $(divBind).html(str);
            }
            else {
                $(row).css('display', 'none');
            }
        }
        function delFilter(e) {
            var itemText = $(e).parent().text().slice(0, -2);
            $('input[type=checkbox]:checked').each(function () {
                if ($(this).val() == itemText) {
                    $('#' + $(this).attr("id")).prop("checked", false);
                    var chkselect = $(this).attr("id").split("_");
                    bindFilterCateg('#' + chkselect[0]);
                }

            });
        }
        function filterClick(e) {
            var divFilt = $(e).attr("id");
            divFilt = divFilt.replace("lbl", "");
            if ($('#div' + divFilt).css('display') == 'none') {
                $('td[id^="td"]:not(#td' + divFilt + ')').css('background-color', 'rgb(138 189 195 / 40%)');
                $('#td' + divFilt).css('background-color', 'antiquewhite');
                $('.filter-content:not(#div' + divFilt + ')').css({ 'display': 'none' });
                $('#div' + divFilt).slideToggle(1);
            }
            else {
                $('#div' + divFilt).slideToggle(1);
                $('#td' + divFilt).css('background-color', 'rgb(138 189 195 / 40%)');
            }
        }
        function btnFiltShow() {
            btnClearShow();
            $('#divGrid').css('display', 'none');
            $('#lnkbtnDownload').css('display', 'none');
        }
        function btnClearShow() {
            var i = 0;
            $("input[type=checkbox]:checked'").each(function () {
                i++;
            });
            $('#lnkbtnClearFilter').css('display', (i > 0 ? 'block' : 'none'));
            $('#lnkbtnApplyFilters').css('display', (i > 0 ? 'block' : 'none'));
        }
        function btnHide() {
            $('#lnkbtnApplyFilters').css('display', 'none');
            $('#divGrid').css('display', 'block');
            $('#lnkbtnDownload').css('display', 'block');
        }
    </script>
</asp:Content>
