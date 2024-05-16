<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="CustomerWiseSalesAnalysis.aspx.cs" Inherits="TTS.CustomerWiseSalesAnalysis" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
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
    </style>
    <div style="text-align: center;" class="pageTitleHead">
        SALES ANALYSIS
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="background-color: #dcecfb; width: 100%;
            border-color: White; border-collapse: separate;">
            <tr>
                <td>
                    CATEGORY
                    <asp:DropDownList runat="server" ID="ddlCategory" ClientIDMode="Static" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlCategory_IndexChange">
                        <asp:ListItem Text="CHOOSE" Value="0"></asp:ListItem>
                        <asp:ListItem Text="SOLID" Value="Solid"></asp:ListItem>
                        <asp:ListItem Text="POB" Value="Pob"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    YEAR
                    <asp:DropDownList runat="server" ID="ddlYear" ClientIDMode="Static" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlYear_IndexChange">
                    </asp:DropDownList>
                </td>
                <td>
                    TYRE TYPE
                    <asp:DropDownList runat="server" ID="ddlType" ClientIDMode="Static" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlType_IndexChange">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:CheckBoxList runat="server" ID="chkCustomerList" ClientIDMode="Static" RepeatColumns="2"
                        RepeatDirection="Horizontal">
                    </asp:CheckBoxList>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label>
                    <input type="button" value="GET DATA" id="btnGetData" onclick="javascript:return CtrlSalesMatrix();" />
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        $("[id*=chkCustomerList] input:checkbox").change(function () {
            var maxSelection = 10;
            if ($("[id*=chkCustomerList] input:checkbox:checked").length > maxSelection) {
                $(this).prop("checked", false);
                alert("Please select a maximum of " + maxSelection + " items.");
            }
        });

        function CtrlSalesMatrix() {
            $('#lblErrMsg').html(''); var ErrMsg = '';
            if ($("#ddlCategory option:selected").text() == "CHOOSE")
                ErrMsg += "Choose category</br>";
            if ($("#ddlYear option:selected").text() == "CHOOSE" || $("#ddlYear option:selected").text() == "")
                ErrMsg += "Choose year</br>";
            if ($("#ddlType option:selected").text() == "CHOOSE" || $("#ddlType option:selected").text() == "")
                ErrMsg += "Choose Tyre Type</br>";
            if ($("[id*=chkCustomerList] input:checked").length == 0)
                ErrMsg += "Choose customer";

            if (ErrMsg.length > 0) {
                $('#lblErrMsg').html(ErrMsg);
                return false;
            }
            else {
                var selectedValues = [];
                $("[id*=chkCustomerList] input:checked").each(function () { selectedValues.push($(this).val()); });
                window.open("customerwisesalesreportmatrix.aspx?Year=" + $("#ddlYear option:selected").text() + "&grade=" + $("#ddlType option:selected").text() +
                "&Category=" + $("#ddlCategory option:selected").text() + "&custname=" + selectedValues, "_blank")
                return false;
            }
        }
    </script>
</asp:Content>
