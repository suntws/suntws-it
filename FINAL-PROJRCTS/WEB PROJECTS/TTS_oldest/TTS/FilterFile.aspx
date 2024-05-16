<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="FilterFile.aspx.cs" Inherits="TTS.FilterFile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        /*table css */
        .tableCss1
        {
            width: 100%;
            background-color: #dcecfb;
            border-color: White;
            border-collapse: separate;
        }
        .tableCss1 th
        {
            width: 150px;
            font-weight: bold;
            text-align: center;
            background-color: #6ef0ee;
            color: #db4d67;
        }
        .tableCss1 td
        {
            font-size: 12px;
            text-align: left;
            padding-left: 4px;
        }
        
        /* label,span css -- for heading */
        .spanCss
        {
            font-size: 14px;
            font-style: italic;
            color: rgb(0, 102, 0);
            font-family: Times New Roman;
            font-weight: 600;
        }
        
        #divfilter
        {
            height: 550px;
            overflow: auto;
            position: relative;
            top: 5px;
            float: left;
        }
        .filter-block
        {
            margin-bottom: 1px;
            padding-left: 5px;
        }
        .filter-block ul
        {
            list-style: none;
            padding: 0px 0px 0px 10px;
            overflow-x: hidden;
            overflow-y: scroll;
        }
        .filter-block h4
        {
            position: relative;
            color: #000;
            text-transform: uppercase;
            font-weight: 400;
            font-size: 12px;
            cursor: pointer;
            height: 10px;
            text-decoration: underline;
            top: -5px;
            left: 0px;
        }
        .filter-block .checkbox-label
        {
            font-size: 12px;
            top: -3px;
            position: relative;
        }
        .filter-block input[type="checkbox"]
        {
            width: 16px;
            height: 16px;
            padding: 0px;
        }
        #divContent
        {
            overflow: auto;
            height: 550px;
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
        #gvStockDetails .paging:hover, tr:hover
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
        #ulTyreSize li
        {
            width: 250px;
        }
        #ulSidewall li
        {
            width: 200px;
        }
        .recCss
        {
            float: left;
            background-color: #075865;
            color: #fff;
            font-size: 15px;
            font-weight: bold;
            text-align: center;
            width: 870px;
        }
        .badge-success
        {
            background-color: #28a745;
        }
        .badge-secondary
        {
            background-color: #6c757d;
        }
        .badge-primary
        {
            background-color: #007bff;
        }
        .badge-warning
        {
            background-color: #ffc107;
        }
        .badge-info
        {
            background-color: #17a2b8;
        }
        .badge-light
        {
            background-color: #f1995b;
        }
        .badge-grade
        {
            background-color: #3cc4d1;
        }
        .badge-year
        {
            background-color: #f9f44f;
        }
        .badge-plant
        {
            background-color: #783de5;
        }
        span.badge
        {
            font-size: 12px;
            padding: 5px 6px 5px 6px;
            margin-top: 2px;
        }
        #btnApply
        {
            position: relative;
            left: 2px;
            background-color: #6edaf5;
            font-size: 12px;
            font-weight: bold;
            top: 1px;
        }
        #btnBack
        {
            position: relative;
            left: 2px;
            background-color: #f48cfa;
            font-size: 12px;
            font-weight: bold;
            display: block;
            top: 1px;
        }
        .style1
        {
            width: 70%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: center;" class="pageTitleHead">
        <asp:Label runat="server" ID="lblPageTitle" ClientIDMode="Static"></asp:Label>
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table id="mainTable" class="tableCss1" border="1">
            <tr>
                <td style="vertical-align: top;width:19%;">
                    <div id="divfilter">
                        <span style="text-align: center; width: 200px; height: 20px; float: left; font-weight: bold;
                            font-family: Times New Roman; font-size: 15px; background-color: #4293da; color: #fff;">
                            FILTERS</span> &nbsp;
                        <div class="filter-block">
                            <h4>
                                Platform</h4>
                            <ul class="filter-content" id="ulPlatform">
                            </ul>
                        </div>
                        <div class="filter-block">
                            <h4>
                                Brand</h4>
                            <ul class="filter-content" id="ulBrand">
                            </ul>
                        </div>
                        <div class="filter-block">
                            <h4>
                                Sidewall</h4>
                            <ul class="filter-content" id="ulSidewall">
                            </ul>
                        </div>
                        <div class="filter-block">
                            <h4>
                                Tyre Type
                            </h4>
                            <ul class="filter-content" id="ulType">
                            </ul>
                        </div>
                        <div class="filter-block">
                            <h4>
                                Tyre Size</h4>
                            <ul class="filter-content" id="ulTyreSize">
                            </ul>
                        </div>
                        <div class="filter-block">
                            <h4>
                                Rim Size
                            </h4>
                            <ul class="filter-content" id="ulRimSize">
                            </ul>
                        </div>
                        <div style="margin-top: 50px">
                            <span style="width: 120px; float: left; line-height: 50px; font-size: 20px; color: #51c91f;">
                                <asp:LinkButton runat="server" ID="lnkXlsDownload" ClientIDMode="Static" Text="DOWNLOAD"
                                    OnClick="lnkStockXls_Click" ForeColor="#51c91f"></asp:LinkButton>
                            </span>
                            <asp:ImageButton ID="lnkExport" runat="server" ImageUrl="~/images/imagexls.png" ClientIDMode="Static"
                                OnClick="btnStockXls_Click" Style="width: 48px; height: 48px; text-decoration: none;" /></div>
                    </div>
                </td>
                <td  style="width:81%;">
                    <div id="divContent">
                        <asp:ScriptManager runat="server" ID="scriptManager1">
                        </asp:ScriptManager>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <div style="width: 100%; font-size: 12px;">
                                    <div id="tr_Select_Platform" style="display: none;">
                                        <table class="tableCss" border="1px" cellpadding="3">
                                            <tr>
                                                <th style="width: 10%;">
                                                    <span class="spanCss">PlatForm</span>
                                                </th>
                                                <td style="width: 90%;">
                                                    <div id="div_platform">
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
                                                    <div id="div_brand">
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
                                                    <div id="div_sidewall">
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="tr_Select_Type" style="display: none;">
                                        <table class="tableCss" border="1px" cellpadding="3">
                                            <tr>
                                                <th style="width: 10%;">
                                                    <span class="spanCss">Type</span>
                                                </th>
                                                <td style="width: 90%;">
                                                    <div id="div_type">
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="tr_Select_Size" style="display: none;">
                                        <table class="tableCss" border="1px" cellpadding="3">
                                            <tr>
                                                <th style="width: 10%;">
                                                    <span class="spanCss">Size</span>
                                                </th>
                                                <td style="width: 90%;">
                                                    <div id="div_size">
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="tr_Select_Rim" style="display: none;">
                                        <table class="tableCss" border="1px" cellpadding="3">
                                            <tr>
                                                <th style="width: 10%;">
                                                    <span class="spanCss">Rim</span>
                                                </th>
                                                <td style="width: 95%;">
                                                    <div id="div_rim">
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <div style="width: 100%; font-size: 12px;" id="divApply">
                                    <table class="tableCss" border="1px" cellpadding="3">
                                        <tr>
                                            <th class="style1" id="thFiltertb">
                                                <span class="spanCss" style="position: relative;">Apply Filters</span>
                                            </th>
                                            <td style="width: 5%;" id="tdFiltertb">
                                                <asp:Button runat="server" ID="btnApply" ClientIDMode="Static" Text="APPLY" OnClick="btnApply_Click" />
                                            </td>
                                            <td style="width: 5%;">
                                                <asp:Button runat="server" ID="btnBack" ClientIDMode="Static" Text="BACK" OnClick="btnBack_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="divlblNoOfRec" style="width:870px">
                                    <asp:Label ID="lblNoOfRecords" Width="100%" Text="" runat="server" ClientIDMode="static" CssClass="recCss"></asp:Label></div>
                                <div id="divGrdview" style="width: 870px;height:499px;overflow:scroll; float: left;">
                                    <asp:GridView runat="server" AllowPaging="true" ID="gvStockDetails" PageSize="100"
                                        Width="100%" CssClass="gridcss" ClientIDMode="Static" PagerStyle-BackColor="#e4f7cf"
                                        PagerSettings-LastPageText="Last" PagerSettings-Mode="NumericFirstLast" PagerStyle-HorizontalAlign="Center"
                                        PagerStyle-Font-Size="16px" PagerStyle-ForeColor="#000" PagerStyle-CssClass="paging"
                                        ViewStateMode="Enabled" OnPageIndexChanging="gvStockList_PageIndex">
                                    </asp:GridView>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
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
    <asp:HiddenField ID="hdnconfig" runat="server" ClientIDMode="Static" EnableViewState="true" />
    <asp:HiddenField ID="hdnbrand" runat="server" ClientIDMode="Static" EnableViewState="true" />
    <asp:HiddenField ID="hdnsidewall" runat="server" ClientIDMode="Static" EnableViewState="true" />
    <asp:HiddenField ID="hdnrimsize" runat="server" ClientIDMode="Static" EnableViewState="true" />
    <asp:HiddenField ID="hdntyretype" runat="server" ClientIDMode="Static" EnableViewState="true" />
    <asp:HiddenField ID="hdnstocktype" runat="server" ClientIDMode="Static" EnableViewState="true" />
    <asp:HiddenField ID="hdntyresize" runat="server" ClientIDMode="Static" EnableViewState="true" />
    <script type="text/javascript">
        $(document).ready(function () {
            displayCategory();
            closeCategory();
            onclick_filters();
        });

        function displayCategory() {
            $('.filter-block h4').on('click', function () {
                if ($(this).siblings(".filter-content").css("display") == 'block')
                    $(this).siblings(".filter-content").slideToggle(1);
                else {
                    closeCategory();
                    $(this).toggleClass('closed').siblings('.filter-content').slideToggle(1);
                    $(this).toggleClass('closed').siblings('.filter-content').css({ "max-height": "300px" });
                }
            });
        }
        function onclick_filters() {
            $('input:checkbox').on('click', function () {
                $("#thFiltertb").css("visibility", 'visible');
                $("#tdFiltertb").css("visibility", 'visible');  
                $("#divGrdview").css("display", 'none');
                $("#lblNoOfRecords").css("visibility", 'hidden');
            });
        }

        function onclick_btnApply() {
            $("#btnApply").on('click', function () {
                $('#divGrdview').css('display', 'block');
                $("#lblNoOfRecords").css("visibility", "visible");
                $("#thFiltertb").css("visibility", "hidden");
                $("#tdFiltertb").css("visibility", "hidden");
            });  
        }

        function closeCategory() {
            $('.filter-content').each(function () {
                if ($(this).css("display") == 'block') $(this).slideToggle(1);
            });
        }

        function filterRecords() {

            $.ajax({ type: "POST", url: "BindRecords.aspx?type=getFilterReport&stocktype=" + $('#hdnstocktype').val() + "&qfyear=" + $("#hdnfyear").val() +
            "&qtyear=" + $("#hdntyear").val() + "&qfmonth=" + $("#hdnfmonth").val() + "&qtmonth=" + $("#hdntmonth").val() + "&qcustcode=" +
            $("#hdncustcode").val() + "&qcusttype=" + $("#hdncusttype").val() + "&qfiletype=" + $("#hdnfiletype").val(), contentType: "application/json; charset=utf-8",
            dataType: "json", success: function (data) {
                    var config = data.config;
                    var brand = data.brand;
                    var sidewall = data.sidewall;
                    var rimsize = data.rimsize;
                    var tyretype = data.tyretype;
                    var tyresize = data.tyresize;

                    var htmlElement = "";
                    for (var i = 0; i < config.length; i++) {
                        htmlElement += "<li> <input type='checkbox' data-filter='config' id='chkConfig_" + config[i].toString() + "' onclick='filterElementClick(this)' value=" + config[i].toString() + " /> <label class='checkbox-label' for='chkConfig_" + config[i].toString() + "' > " + config[i].toString() + "</label> </li>";
                    }
                    $("#ulPlatform").html(htmlElement);

                    htmlElement = "";
                    for (var i = 0; i < brand.length; i++) {
                        htmlElement += "<li> <input type='checkbox' data-filter='brand' id='chkBrand_" + brand[i].toString() + "' onclick='filterElementClick(this)' value=" + brand[i].toString() + " /> <label class='checkbox-label' for='chkBrand_" + brand[i].toString() + "' > " + brand[i].toString() + "</label> </li>";
                    }
                    $("#ulBrand").html(htmlElement);

                    htmlElement = "";
                    for (var i = 0; i < sidewall.length; i++) {
                        htmlElement += "<li> <input type='checkbox' data-filter='sidewall' id='chkSidewall_" + sidewall[i].toString() + "' onclick='filterElementClick(this)' value=" + sidewall[i].toString() + "/> <label class='checkbox-label' for='chkSidewall_" + sidewall[i].toString() + "' > " + sidewall[i].toString() + "</label> </li>";
                    }
                    $("#ulSidewall").html(htmlElement);

                    htmlElement = "";
                    for (var i = 0; i < tyresize.length; i++) {
                        htmlElement += "<li> <input type='checkbox' data-filter='tyresize' id='chkSize_" + tyresize[i].toString() + "' onclick='filterElementClick(this)' value=" + tyresize[i].toString() + "/> <label class='checkbox-label' for='chkSize_" + tyresize[i].toString() + "' > " + tyresize[i].toString() + "</label> </li>";
                    }
                    $("#ulTyreSize").html(htmlElement);

                    htmlElement = "";
                    for (var i = 0; i < rimsize.length; i++) {
                        htmlElement += "<li> <input type='checkbox' data-filter='rimsize' id='chkRim_" + rimsize[i].toString() + "' onclick='filterElementClick(this)' value=" + rimsize[i].toString() + "/> <label class='checkbox-label' for='chkRim_" + rimsize[i].toString() + "' > " + rimsize[i].toString() + "</label> </li>";
                    }
                    $("#ulRimSize").html(htmlElement);

                    htmlElement = "";
                    for (var i = 0; i < tyretype.length; i++) {
                        htmlElement += "<li> <input type='checkbox' data-filter='tyretype' id='chkType_" + tyretype[i].toString() + "' onclick='filterElementClick(this)' value=" + tyretype[i].toString() + "/> <label class='checkbox-label' for='chkType_" + tyretype[i].toString() + "' > " + tyretype[i].toString() + "</label> </li>";
                    }
                    $("#ulType").html(htmlElement);
                }
            });
        }
        function removeDoubleQuotes() {
            $(".badge").each(function () {
                var str = $(this).html();
                str = str.replace(/'/g, "");
                $(this).html(str);
            });
        }
        function filterElementClick(ele) {
            if (ele.checked == true)
                $("#hdn" + $(ele).attr("data-filter")).val($("#hdn" + $(ele).attr("data-filter")).val() + "'" + $(ele).siblings("label").text().trim() + "',");
            else
                $("#hdn" + $(ele).attr("data-filter")).val($("#hdn" + $(ele).attr("data-filter")).val().replace("'" + $(ele).siblings("label").text().trim() + "',", ""));
            bindCateg();
        }
        function bindCateg() {
            $("#div_platform").html(bindSel_Categories($("#hdnconfig").val(), "badge-success", "#tr_Select_Platform"));
            $("#div_brand").html(bindSel_Categories($("#hdnbrand").val(), "badge-secondary", "#tr_Select_Brand"));
            $("#div_sidewall").html(bindSel_Categories($("#hdnsidewall").val(), "badge-primary", "#tr_Select_Sidewall"));
            $("#div_type").html(bindSel_Categories($("#hdntyretype").val(), "badge-warning", "#tr_Select_Type"));
            $("#div_size").html(bindSel_Categories($("#hdntyresize").val(), "badge-info", "#tr_Select_Size"));
            $("#div_rim").html(bindSel_Categories($("#hdnrimsize").val(), "badge-light", "#tr_Select_Rim"));

            removeDoubleQuotes();
        }
        function bindSel_Categories(arrValue, cssClass, row) {
            var str = "";
            if (arrValue.length > 0) {
                var arr = arrValue.split(",");
                for (var i = 0; i < arr.length; i++) {
                    if (arr[i].length > 0)
                        str += "<span class='badge " + cssClass + "' style='border-radius: 20px;'>" + arr[i] + " </span> &nbsp;";
                }
                $(row).css("display", "block");
                return str;
            }
            else {
                $(row).css("display", "none");
                return "";
            }
        }
    </script>
</asp:Content>
