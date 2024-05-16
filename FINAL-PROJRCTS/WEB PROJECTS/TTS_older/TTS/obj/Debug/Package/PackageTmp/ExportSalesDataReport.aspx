<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="ExportSalesDataReport.aspx.cs" Inherits="TTS.ExportSalesDataReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        #divfilterbar
        {
            width: 1072px;
            height: 66px;
            border: 1px solid black;
            position: relative;
            margin-top: 10px;
            margin-left: 3px;
        }
        #btnGetData
        {
            background-color: #4CAF50;
            border: none;
            color: white;
            padding: 5px 10px;
            text-align: center;
            text-decoration: none;
            display: inline-block;
            cursor: pointer;
            position: relative;
            font-size: 15px;
            font-weight: bold;
        }
        .section
        {
            float: left;
            padding-left: 5px;
            padding-right: 5px;
            position: relative;
            top: 5px;
        }
        
        .align-right
        {
            text-align: right;
        }
        #selGrade
        {
            width: 100px;
        }
        #selCustomerName
        {
            width: 220px;
        }
        #selConfig
        {
            width: 180px;
        }
        #selDuration
        {
            width: 155px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: center;" class="pageTitleHead">
        EXPORT SALES DATA MATRIX
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label>
        <div id="divfilterbar">
            <div style="width: 380px; float: left; border: 1px solid #000; height: 60px; margin-top: 2px;
                background-color: #fcf; margin-left: 5px; margin-right: 5px;">
                <div class="section" style="display: inline-block; width: 160px;">
                    <label>
                        RECORD OF THE YEAR
                    </label>
                    <br />
                    <select style="top: 5px; position: relative;" id="selDuration" name="Duration">
                        <option value="">--SELECT--</option>
                        <option value="1">APR'15 - MAR'16 </option>
                        <option value="2">APR'16 - MAR'17 </option>
                    </select>
                </div>
                <div class="section" style="width: 190px;">
                    <label for="selConfig">
                        PLATFORM
                    </label>
                    <br />
                    <select style="top: 5px; position: relative;" id="selConfig" name="Config">
                    </select>
                </div>
            </div>
            <div style="width: 380px; float: left; border: 1px solid #000; height: 60px; margin-top: 2px;
                background-color: #ccfffd;">
                <div class="section" style="width: 130px;">
                    <label for="selGrade">
                        GRADE
                    </label>
                    <br />
                    <select style="top: 5px; position: relative;" id="selGrade" name="Grade">
                    </select>
                </div>
                <div class="section" style="width: 220px;">
                    <label for="selCustomerName">
                        CUSTOMER NAME
                    </label>
                    <br />
                    <select style="top: 5px; position: relative;" id="selCustomerName" name="CustomerName">
                    </select>
                </div>
            </div>
            <div style="display: inline-block; width: 280px; height: 20px; padding-left: 5px;">
                <span style="width: inherit; text-align: center; position: relative; left: 5px; font-weight: bold;
                    font-size: 11px; color: #d80303;">YEAR & PLATFORM IS MANDATORY</span>
            </div>
            <div style="width: 280px; display: inline-block; height: 40px;">
                <div class="section" style="display: inline-block; left: 25px;">
                    <input type="button" value="GET DATA" id="btnGetData" onclick="javascript:return CtrlSalesMatrix();" />
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hdnDuration" runat="server" ClientIDMode="Static" EnableViewState="true" />
        <asp:HiddenField ID="hdnConfig" runat="server" ClientIDMode="Static" EnableViewState="true" />
        <asp:HiddenField ID="hdnGrade" runat="server" ClientIDMode="Static" EnableViewState="true" />
        <asp:HiddenField ID="hdnCustomerName" runat="server" ClientIDMode="Static" EnableViewState="true" />
    </div>
    <script type="text/javascript">
        function bindDDL(obj) {
            var Platform = obj.Platform;
            var Grade = obj.Grade;
            var CustomerName = obj.CustomerName;

            bindConfig(Platform);
            bindGrade(Grade);
            bindCustomer(CustomerName);
        }

        function bindCustomer(CustomerName) {
            if (CustomerName != "") {
                var htmlElement = "<option value=''>--SELECT--</option>";
                for (var k = 0; k < CustomerName.length; k++) {
                    htmlElement += "<option value='" + CustomerName[k].toString() + "'>" + CustomerName[k].toString() + "</option>";
                }
                $("#selCustomerName").html(htmlElement);
            }
            else
                $("#selCustomerName").html("");
        }

        function bindConfig(Platform) {
            if (Platform != "") {
                var htmlElement = "<option value=''>--SELECT--</option>";
                for (var i = 0; i < Platform.length; i++) {
                    htmlElement += "<option value='" + Platform[i].toString() + "'>" + Platform[i].toString() + "</option>";
                }
                $("#selConfig").html(htmlElement);
            }
            else
                $("#selConfig").html("");
        }

        function bindGrade(Grade) {
            if (Grade != "") {
                var htmlElement = "<option value=''>--SELECT--</option>";
                for (var j = 0; j < Grade.length; j++) {
                    htmlElement += "<option value='" + Grade[j].toString() + "'>" + Grade[j].toString() + "</option>";
                }
                $("#selGrade").html(htmlElement);
            }
            else
                $("#selGrade").html("");
        }

        $("#selGrade, #selCustomerName, #selConfig, #selDuration").change(function (ele) {
            if ($("#" + $(ele.srcElement).attr("id") + " option:selected").index() > 0)
                $("#hdn" + $(ele.srcElement).attr("name")).val($(ele.srcElement).val());
            else
                $("#hdn" + $(ele.srcElement).attr("name")).val("");

            if (ele.currentTarget.id != "selCustomerName") {
                var query = "";
                query = "duration=" + $("#selDuration option:selected").val() + "&grade=" + $("#selGrade option:selected").val() + "&config=" + $("#selConfig option:selected").val()
                $.ajax({ type: "POST", url: "ExportSalesDataReport.aspx?" + query, context: document.body, dataType: "json",
                    success: function (data) {
                        bindDdl(data, ele)
                    }
                });
            }
        });

        function bindDdl(obj, ele) {
            if (obj == null) return;
            var Platform = obj.Platform;
            var Grade = obj.Grade;
            var CustomerName = obj.CustomerName;

            if (ele.currentTarget.id == "selDuration") {
                bindConfig(Platform);
                bindGrade(Grade);
                bindCustomer(CustomerName);
            }
            else if (ele.currentTarget.id == "selConfig") {
                bindGrade(Grade);
                bindCustomer(CustomerName);
            }
            else if (ele.currentTarget.id == "selGrade")
                bindCustomer(CustomerName);
        }

        function CtrlSalesMatrix() {
            $('#lblErrMsg').html(''); var ErrMsg = '';
            if ($("#hdnDuration").val() == "")
                ErrMsg += "Choose duration</br>";
            if ($("#hdnConfig").val() == "")
                ErrMsg += "Choose Platform</br>";
            if (ErrMsg.length > 0) {
                $('#lblErrMsg').html(ErrMsg);
                return false;
            }
            else {
                window.open("exportsalesdatareview.aspx?duration=" + $("#hdnDuration").val() + "&config=" + $("#hdnConfig").val() + "&grade=" + $("#hdnGrade").val() + "&custname=" + $("#hdnCustomerName").val(), "_blank")
                return false;
            }
        }

    </script>
</asp:Content>
