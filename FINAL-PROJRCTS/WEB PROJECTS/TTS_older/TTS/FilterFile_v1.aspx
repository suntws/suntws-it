<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="FilterFile_v1.aspx.cs" Inherits="TTS.FilterFile_v1" %>

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
        }
        .tableCss td
        {
            text-align: left;
        }
        .tableCss1
        {
            background-color: #8abdc3;
            width: 100%;
            border-color: White;
            border-collapse: separate;
        }
        .tableCss1 td
        {
            text-align: center;
            width: 12.5%;
            border-radius: 20px;
        }
        .tableCss1 td:hover
        {
            background-color: #f9c3f8f2 !important;
        }
        
        /* label,span css -- for heading */
        .spanCss
        {
            font-size: 14px;
            font-style: italic;
            color: #006600;
            font-family: Times New Roman;
            font-weight: 600;
        }
        .spanCss-filter
        {
            font-size: 14px;
            color: Black;
            font-family: Times New Roman;
            font-weight: 600;
            width: 100%;
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
            font-size: 15px;
            font-weight: bold;
            text-align: center;
        }
        .badge-Platform
        {
            background-color: #91e4a4;
        }
        .badge-Brand
        {
            background-color: #b6bdc3;
        }
        .badge-Sidewall
        {
            background-color: #8ebae8;
        }
        .badge-Tyretype
        {
            background-color: #e1e404;
        }
        .badge-Tyresize
        {
            background-color: #61dff3;
        }
        .badge-Rimsize
        {
            background-color: #fcd7d7;
        }
        .badge-Grade
        {
            background-color: #b2f55b;
        }
        .badge-Year
        {
            background-color: #f0abf1;
        }
        span.badge
        {
            font-size: 12px;
            padding: 5px 6px 5px 6px;
            margin-top: 2px;
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
            background: #ffffff none repeat scroll 0 0;
            border: 1px solid #d48e65;
            color: #d48e65;
            outline: medium none;
            padding: 5px;
            text-decoration-line: none;
            font-weight: 800;
            text-align: center;
            float: left;
            display: block;
        }
        #lnkbtnApplyFilters:hover
        {
            background-color: #d48e65;
            color: #ffffff;
        }
        #lnkbtnClearFilter
        {
            
            background: #ffffff none repeat scroll 0 0;
            border: 1px solid #79ba92;
            color: #79ba92;
            outline: medium none;
            padding: 5px;
            text-align: center;
            text-decoration-line: none;
            font-weight: 800;
            float: left;
            width: 100px;
            display: none;
        }
        #lnkbtnClearFilter:hover
        {
            background-color: #79ba92;
            color: #ffffff;
        }
        #lnkbtnDownload
        {
            background: #ffffff none repeat scroll 0 0;
            border: 1px solid #fa98dd;
            color: #fa98dd;
            outline: medium none;
            padding: 5px;
            text-align: center;
            text-decoration-line: none;
            font-weight: 800;
            float: right;
            width: 100px;
            display: none;
        }
        #lnkbtnDownload:hover
        {
            background-color: #fa98dd;
            color: #ffffff;
        }
         #lnkbtnBack
        {
            background: #ffffff none repeat scroll 0 0;
            border: 1px solid #b8b6ff;
            color: #b8b6ff;
            outline: medium none;
            padding: 5px;
            text-align: center;
            text-decoration-line: none;
            font-weight: 800;
            float: right;
            width: 100px;
            display: block;
        }
        #lnkbtnBack:hover
        {
            background-color: #b8b6ff;
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
        <table class="tableCss" border="1">
            <tr>
                <td style="width: 100%; background-color: #4293da; padding-left: 43%;">
                    <span id="spanFilter" style="text-align: center; width: 150px; height: 20px; float: left;
                        font-weight: bold; font-family: Times New Roman; font-size: 15px; background-color: #4293da;
                        color: #fff; display: inline-block;">FILTERS</span>
                </td>
            </tr>
            <tr>
                <td>
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
                                    <span id="lblRimsize" class="spanCss-filter" onclick="filterClick(this)">Rim Size</span>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
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
                <td>
                    <div class="divContent">
                        <div style="width: 100%; font-size: 12px;">
                            <div id="tr_Select_Platform" style="display: none;">
                                <table class="tableCss" border="1px" cellpadding="3">
                                    <tr>
                                        <th style="width: 10%;">
                                            <span class="spanCss">PlatForm</span>
                                        </th>
                                        <td style="width: 90%;">
                                            <div id="div_Platform" runat="server">
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="tr_Select_Brand" style="display: none;">
                                <table class="tableCss" border="1px" cellpadding="3">
                                    <tr>
                                        <th style="width: 10%;">
                                            <span class="spanCss">Brand</span>
                                        </th>
                                        <td style="width: 90%;">
                                            <div id="div_Brand" runat="server">
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="tr_Select_Sidewall" style="display: none;">
                                <table class="tableCss" border="1px" cellpadding="3">
                                    <tr>
                                        <th style="width: 10%;">
                                            <span class="spanCss">Sidewall</span>
                                        </th>
                                        <td style="width: 90%;">
                                            <div id="div_Sidewall" runat="server">
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="tr_Select_Tyretype" style="display: none;">
                                <table class="tableCss" border="1px" cellpadding="3">
                                    <tr>
                                        <th style="width: 10%;">
                                            <span class="spanCss">Type</span>
                                        </th>
                                        <td style="width: 90%;">
                                            <div id="div_Tyretype" runat="server">
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="tr_Select_Tyresize" style="display: none;">
                                <table class="tableCss" border="1px" cellpadding="3">
                                    <tr>
                                        <th style="width: 10%;">
                                            <span class="spanCss">Size</span>
                                        </th>
                                        <td style="width: 90%;">
                                            <div id="div_Tyresize" runat="server">
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="tr_Select_Rimsize" style="display: none;">
                                <table class="tableCss" border="1px" cellpadding="3">
                                    <tr>
                                        <th style="width: 10%;">
                                            <span class="spanCss">Rim</span>
                                        </th>
                                        <td style="width: 90%;">
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
                    <div>
                        <table style="width: 100%; background-color: #dcecfb; border-collapse: collapse;
                            border-color: #dcecfb;" border="1px">
                            <tr>
                                <td style="width: 11%;">
                                    <asp:LinkButton ID="lnkbtnApplyFilters" runat="server" ClientIDMode="Static" Text="APPLY FILTERS"
                                        OnClick="btnApplyFilters_Click"></asp:LinkButton>
                                </td>
                                <td style="width: 69%;">
                                    <asp:LinkButton ID="lnkbtnClearFilter" runat="server" ClientIDMode="Static" Text="CLEAR"
                                        OnClick="btnClearFilter_Click"></asp:LinkButton>
                                </td>
                                <td style="width: 10%;">
                                    <asp:LinkButton ID="lnkbtnDownload" runat="server" ClientIDMode="Static" Text="DOWNLOAD"
                                        OnClick="btnDownload_Click"></asp:LinkButton>
                                </td>
                                <td style="width: 10%;">
                                <asp:LinkButton ID="lnkbtnBack" runat="server" ClientIDMode="Static" Text="BACK"
                                        OnClick="lnkbtnBack_Click"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="divContent">
                        <div id="divGrid" style="width: 100%; float: left;">
                            <asp:Label ID="lblNoOfRecords" runat="server" ClientIDMode="Static" Width="100%"
                                CssClass="recCss"></asp:Label>
                            <asp:GridView runat="server" AllowPaging="true" ID="gvStockDetails" PageSize="100"
                                Width="100%" CssClass="gridcss" ClientIDMode="Static" PagerStyle-BackColor="#e4f7cf"
                                PagerSettings-LastPageText="Last" PagerSettings-Mode="NumericFirstLast" PagerStyle-HorizontalAlign="Center"
                                PagerStyle-Font-Size="16px" PagerStyle-ForeColor="#000" PagerStyle-CssClass="paging"
                                ViewStateMode="Enabled" OnPageIndexChanging="gvStockList_PageIndex">
                            </asp:GridView>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
        <hr />
    </div>
    <asp:HiddenField ID="hdnfyear" runat="server" ClientIDMode="Static" EnableViewState="true" />
    <asp:HiddenField ID="hdnfmonth" runat="server" ClientIDMode="Static" EnableViewState="true" />
    <asp:HiddenField ID="hdntyear" runat="server" ClientIDMode="Static" EnableViewState="true" />
    <asp:HiddenField ID="hdntmonth" runat="server" ClientIDMode="Static" EnableViewState="true" />
    <asp:HiddenField ID="hdncustcode" runat="server" ClientIDMode="Static" EnableViewState="true" />
    <asp:HiddenField ID="hdncusttype" runat="server" ClientIDMode="Static" EnableViewState="true" />
    <asp:HiddenField ID="hdnfiletype" runat="server" ClientIDMode="Static" EnableViewState="true" />
    <asp:HiddenField ID="hdnPlatform" runat="server" ClientIDMode="Static" EnableViewState="true" />
    <asp:HiddenField ID="hdnBrand" runat="server" ClientIDMode="Static" EnableViewState="true" />
    <asp:HiddenField ID="hdnSidewall" runat="server" ClientIDMode="Static" EnableViewState="true" />
    <asp:HiddenField ID="hdnRimsize" runat="server" ClientIDMode="Static" EnableViewState="true" />
    <asp:HiddenField ID="hdnTyretype" runat="server" ClientIDMode="Static" EnableViewState="true" />
    <asp:HiddenField ID="hdnstocktype" runat="server" ClientIDMode="Static" EnableViewState="true" />
    <asp:HiddenField ID="hdnTyresize" runat="server" ClientIDMode="Static" EnableViewState="true" />
    <script type="text/javascript">
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
