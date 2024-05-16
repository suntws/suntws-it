<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="FilterWiseResponse.aspx.cs" Inherits="TTS.FilterWiseResponse" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .gridcss
        {
            background-color: #fff;
            margin: 5px 0 10px 0;
            border: solid 1px #525252;
            border-collapse: collapse;
        }
        .gridcss td
        {
            padding: 2px;
            border: solid 1px #c1c1c1;
        }
        .gridcss th
        {
            padding: 4px 2px;
            color: #ffffff;
            background: #1762a5;
            border-left: solid 1px #525252;
            font-size: 0.9em;
        }
        .gridcss tr:hover
        {
            background-color: #84f5d9;
            font-weight: bold;
        }
        
        /*table css */
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
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
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
            border-color: White; border-collapse: separate; line-height: 22px;">
            <tr>
                <td colspan="2" style="background-color: #4293da;">
                    <span id="spanFilter" style="text-align: center; height: 20px; font-weight: bold;
                        font-size: 15px; background-color: #4293da; color: #fff;">FILTERS</span>
                </td>
                <td style="text-align: center; background-color: #cccccc;">
                    <asp:LinkButton ID="lnkbtnBack" runat="server" ClientIDMode="Static" Text="BACK"
                        OnClick="lnkbtnBack_Click"></asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <table cellspacing="0" rules="all" border="1" style="background-color: #dcecfb; width: 100%;
                        border-color: White; border-collapse: separate;">
                        <tr style="background-color: #71fd89;">
                            <th>
                                PLANT
                            </th>
                            <th>
                                FROM YEAR
                            </th>
                            <th>
                                FROM MONTH
                            </th>
                            <th>
                                TO YEAR
                            </th>
                            <th>
                                TO MONTH
                            </th>
                            <th>
                                CUSTOMER
                            </th>
                        </tr>
                        <tr style="background-color: #d7f9ab;">
                            <th>
                                <asp:Label runat="server" ID="lblPlant" ClientIDMode="Static" Font-Bold="true" Text=""></asp:Label>
                            </th>
                            <th>
                                <asp:Label runat="server" ID="lblFromYear" ClientIDMode="Static" Font-Bold="true"
                                    Text=""></asp:Label>
                            </th>
                            <th>
                                <asp:Label runat="server" ID="lblFromMonth" ClientIDMode="Static" Font-Bold="true"
                                    Text=""></asp:Label>
                            </th>
                            <th>
                                <asp:Label runat="server" ID="lblToYear" ClientIDMode="Static" Font-Bold="true" Text=""></asp:Label>
                            </th>
                            <th>
                                <asp:Label runat="server" ID="lblToMonth" ClientIDMode="Static" Font-Bold="true"
                                    Text=""></asp:Label>
                            </th>
                            <th>
                                <asp:Label runat="server" ID="lblCustomer" ClientIDMode="Static" Font-Bold="true"
                                    Text=""></asp:Label>
                            </th>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="3">
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
                                <span id="lblRimsize" class="spanCss-filter" onclick="filterClick(this)">Rim Size</span>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="3">
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
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <div class="divContent">
                        <div style="width: 100%; font-size: 12px;">
                            <div id="tr_Select_Platform" style="display: none;">
                                <table class="tableCss" border="1px" cellpadding="3">
                                    <tr>
                                        <th>
                                            <span class="spanCss">PlatForm</span>
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
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:LinkButton ID="lnkbtnApplyFilters" runat="server" ClientIDMode="Static" Text="SEARCH"
                        OnClick="btnApplyFilters_Click"></asp:LinkButton>
                </td>
                <td>
                    <asp:LinkButton ID="lnkbtnClearFilter" runat="server" ClientIDMode="Static" Text="CLEAR"
                        OnClick="btnClearFilter_Click"></asp:LinkButton>
                </td>
                <td>
                    <asp:LinkButton ID="lnkbtnDownload" runat="server" ClientIDMode="Static" Text="DOWNLOAD"
                        OnClick="btnDownload_Click"></asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td colspan="3">
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
    <asp:HiddenField ID="hdncustcode" runat="server" ClientIDMode="Static" Value="" />
    <asp:HiddenField ID="hdncusttype" runat="server" ClientIDMode="Static" Value="" />
    <asp:HiddenField ID="hdnfiletype" runat="server" ClientIDMode="Static" Value="" />
    <asp:HiddenField ID="hdnPlatform" runat="server" ClientIDMode="Static" Value="" />
    <asp:HiddenField ID="hdnBrand" runat="server" ClientIDMode="Static" Value="" />
    <asp:HiddenField ID="hdnSidewall" runat="server" ClientIDMode="Static" Value="" />
    <asp:HiddenField ID="hdnRimsize" runat="server" ClientIDMode="Static" Value="" />
    <asp:HiddenField ID="hdnTyretype" runat="server" ClientIDMode="Static" Value="" />
    <asp:HiddenField ID="hdnTyresize" runat="server" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        $(document).ready(function () { });
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
                $('td[id^="td"]:not(#td' + divFilt + ')').css('background-color', '#8abdc3');
                $('#td' + divFilt).css('background-color', 'antiquewhite');
                $('.filter-content:not(#div' + divFilt + ')').css({ 'display': 'none' });
                $('#div' + divFilt).slideToggle(1);
            }
            else {
                $('#div' + divFilt).slideToggle(1);
                $('#td' + divFilt).css('background-color', '#8abdc3');
            }
        }
        function btnFiltShow() {
            btnClearShow();
            $('#divGrid').css('display', 'none');
            $('#lnkbtnDownload').css('display', 'none');
            $('#lnkbtnApplyFilters').css('display', 'block');
        }
        function btnClearShow() {
            var i = 0;
            $("input[type=checkbox]:checked'").each(function () {
                i++;
            });
            $('#lnkbtnClearFilter').css('display', (i > 0 ? 'block' : 'none'));
        }
        function btnHide() {
            $('#lnkbtnApplyFilters').css('display', 'none');
            $('#divGrid').css('display', 'block');
            $('#lnkbtnDownload').css('display', 'block');
        }
    </script>
</asp:Content>
