<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="StockMatrixReport.aspx.cs" Inherits="TTS.StockMatrixReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        #divfilterbar
        {
            width: 1072px;
            height: 36px;
            border: 1px solid black;
            position: relative;
        }
        .section
        {
            float: left;
            padding-left: 5px;
            position: relative;
            top: 5px;
        }
        .section label
        {
            font-weight: bold;
        }
        #btnSearch
        {
            background-color: #4CAF50;
            border: none;
            color: white;
            padding: 3px 0px;
            text-align: center;
            text-decoration: none;
            display: inline-block;
            cursor: pointer;
            position: relative;
            font-size: 15px;
            font-weight: bold;
            width: 70px;
        }
        #divContent
        {
            width: 1080px;
            height: 520px;
            margin-top: 20px;
            border: 1px solid black;
        }
        #selGrade
        {
            width: 50px;
        }
        #selBrand
        {
            width: 120px;
        }
        #selSidewall
        {
            width: 130px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: center;" class="pageTitleHead">
        STOCK COUNT MATRIX REPORT
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table>
            <tr>
                <td>
                    <span style="width: inherit; position: relative; left: 10px; text-align: center;
                        font-weight: bold; font-size: 11px; color: #d80303; margin-top: 5px;">* PLANT &
                        PLATFORM ARE MANDATORY, REMAINING ARE OPTIONAL</span>
                    <div id="divfilterbar">
                        <div style="width: 360px; float: left; border: 1px solid #000; height: 30px; margin-top: 2px;
                            background-color: #fcf; margin-left: 2px; margin-right: 2px;">
                            <div class="section">
                                <label for="selPlant">
                                    PLANT
                                </label>
                                <select id="selPlant" name="stocktype">
                                </select>
                            </div>
                            <div class="section">
                                <label for="selConfig">
                                    PLATFORM
                                </label>
                                <select id="selConfig" name="config">
                                </select>
                            </div>
                        </div>
                        <div style="width: 600px; float: left; border: 1px solid #000; height: 30px; margin-top: 2px;
                            background-color: #ccfffd;">
                            <div class="section">
                                <label for="selGrade">
                                    GRADE
                                </label>
                                <select id="selGrade" name="grade">
                                </select>
                            </div>
                            <div class="section">
                                <label for="selBrand">
                                    BRAND
                                </label>
                                <select id="selBrand" name="brand">
                                </select>
                            </div>
                            <div class="section">
                                <label for="selSidewall">
                                    SIDEWALL
                                </label>
                                <select id="selSidewall" name="sidewall">
                                </select>
                            </div>
                        </div>
                        <div class="section">
                            <input type="button" id="btnSearch" value="SEARCH" />
                        </div>
                    </div>
                    <asp:HiddenField ID="hdnstocktype" runat="server" ClientIDMode="Static" EnableViewState="true" />
                    <asp:HiddenField ID="hdnconfig" runat="server" ClientIDMode="Static" EnableViewState="true" />
                    <asp:HiddenField ID="hdnbrand" runat="server" ClientIDMode="Static" EnableViewState="true" />
                    <asp:HiddenField ID="hdnsidewall" runat="server" ClientIDMode="Static" EnableViewState="true" />
                    <asp:HiddenField ID="hdngrade" runat="server" ClientIDMode="Static" EnableViewState="true" />
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        function bindDDL(obj) {
            if (obj != null) {
                var config = obj.config;
                var brand = obj.brand;
                var sidewall = obj.sidewall;
                var stocktype = obj.stocktype;
                var grade = obj.grade;

                bindStocktype(stocktype);
                bindConfig(config);
                bindGrade(grade);
                bindBrand(brand);
                bindSidewall(sidewall);
            }
        }

        function bindStocktype(stocktype) {
            if (stocktype != "") {
                var htmlElement = "<option value=''>--SELECT--</option>";
                for (var i = 0; i < stocktype.length; i++) {
                    htmlElement += "<option value='" + stocktype[i].toString() + "'>" + stocktype[i].toString() + "</option>";
                }
                $("#selPlant").html(htmlElement);
            }
            else
                $("#selPlant").html("");
        }

        function bindConfig(config) {
            if (config != "") {
                var htmlElement = "<option value=''>--SELECT--</option>";
                for (var j = 0; j < config.length; j++) {
                    htmlElement += "<option value='" + config[j].toString() + "'>" + config[j].toString() + "</option>";
                }
                $("#selConfig").html(htmlElement);
            }
            else
                $("#selConfig").html("");
        }
        function bindGrade(grade) {
            if (grade != "") {
                var htmlElement = "<option value=''>--SELECT--</option>";
                for (var i = 0; i < grade.length; i++) {
                    htmlElement += "<option value='" + grade[i].toString() + "'>" + grade[i].toString() + "</option>";
                }
                $("#selGrade").html(htmlElement);
            }
            else
                $("#selGrade").html("");
        }
        function bindBrand(brand) {
            if (brand != "") {
                var htmlElement = "<option value=''>--SELECT--</option>";
                for (var k = 0; k < brand.length; k++) {
                    htmlElement += "<option value='" + brand[k].toString() + "'>" + brand[k].toString() + "</option>";
                }
                $("#selBrand").html(htmlElement);
            }
            else
                $("#selBrand").html("");
        }
        function bindSidewall(sidewall) {
            if (sidewall != "") {
                var htmlElement = "<option value=''>--SELECT--</option>";
                for (var z = 0; z < sidewall.length; z++) {
                    htmlElement += "<option value='" + sidewall[z].toString() + "'>" + sidewall[z].toString() + "</option>";
                }
                $("#selSidewall").html(htmlElement);
            }
            else
                $("#selSidewall").html("");
        }

        $("#selSidewall, #selBrand, #selConfig, #selPlant, #selGrade").change(function (eve) {
            $("#divfilterbar").find("select").each(function (i, e) {
                if (i > $("#divfilterbar").find("select").index(eve.srcElement))
                    $($("#" + e.id + " option")[0]).prop("selected", "selected");
            });
            if ($("#" + $(eve.srcElement).attr("id") + " option:selected").index() > 0)
                $("#hdn" + $(eve.srcElement).attr("name")).val($(eve.srcElement).val());
            else
                $("#hdn" + $(eve.srcElement).attr("name")).val("");

            if (eve.currentTarget.id != "selSidewall") {
                var query = ""; // brand grade config stocktype
                query = "stocktype=" + $("#selPlant option:selected").val() + "&config=" + $("#selConfig option:selected").val() + "&grade=" +
                $("#selGrade option:selected").val() + "&brand=" + $("#selBrand option:selected").val()
                $.ajax({ type: "POST", url: "StockMatrixReport.aspx?" + query, context: document.body, dataType: "json", success: function (data) { bindDdl(data, eve) } });
            }
        });

        $("#btnSearch").click(function () {
            if ($("#hdnstocktype").val() != "" && $("#hdnconfig").val() != "") {
                window.open("StockMatrixReview.aspx?config=" + $("#hdnconfig").val() + "&plant=" + $("#hdnstocktype").val() + "&brand=" + $("#hdnbrand").val() +
                "&sdw=" + $("#hdnsidewall").val() + "&grade=" + $("#hdngrade").val(), "_blank");
                return false;
            }
            else
                alert('CHOOSE PLANT & PLATFORM');
        });

        function bindDdl(obj, ele) {
            if (obj != null) {
                var config = obj.config;
                var brand = obj.brand;
                var sidewall = obj.sidewall;
                var stocktype = obj.stocktype;
                var grade = obj.grade;

                if (ele.currentTarget.id == "selPlant") {
                    bindConfig(config);
                    bindGrade(grade);
                    bindBrand(brand);
                    bindSidewall(sidewall);
                }
                else if (ele.currentTarget.id == "selConfig") {
                    bindGrade(grade);
                    bindBrand(brand);
                    bindSidewall(sidewall);
                }
                else if (ele.currentTarget.id == "selGrade") {
                    bindBrand(brand);
                    bindSidewall(sidewall);
                }
                else if (ele.currentTarget.id == "selBrand")
                    bindSidewall(sidewall);
            }
        }
    </script>
</asp:Content>
