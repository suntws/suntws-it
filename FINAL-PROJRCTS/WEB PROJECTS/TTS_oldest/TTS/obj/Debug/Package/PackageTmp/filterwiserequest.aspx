<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="filterwiserequest.aspx.cs" Inherits="TTS.filterwiserequest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/pdistyle.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: center;" class="pageTitleHead">
        <asp:Label runat="server" ID="lblPageTitle" ClientIDMode="Static"></asp:Label>
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="divMainContent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="background-color: #dcecfb; width: 100%;
            border-color: White; border-collapse: separate;">
            <tr>
                <th colspan="2">
                    Period range can be chosen "From year and month" to next 12 months only.
                </th>
            </tr>
            <tr>
                <th>
                    PLANT
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="ddlPlant" ClientIDMode="Static" CssClass="form-control"
                        Width="150px" AutoPostBack="true" OnSelectedIndexChanged="ddlPlant_indexchanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    FROM YEAR
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="ddlFromYear" ClientIDMode="Static" CssClass="form-control"
                        Width="150px" AutoPostBack="true" OnSelectedIndexChanged="ddlFromYear_indexchanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    FROM MONTH
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="ddlFromMonth" ClientIDMode="Static" CssClass="form-control"
                        Width="150px" AutoPostBack="true" OnSelectedIndexChanged="ddlFromMonth_indexchanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    TO YEAR
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="ddlToYear" ClientIDMode="Static" CssClass="form-control"
                        Width="150px" AutoPostBack="true" OnSelectedIndexChanged="ddlToYear_indexchanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    TO MONTH
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="ddlToMonth" ClientIDMode="Static" CssClass="form-control"
                        Width="150px" AutoPostBack="true" OnSelectedIndexChanged="ddlToMonth_indexchanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    CUSTOMER
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="ddlCustName" ClientIDMode="Static" CssClass="form-control"
                        Width="300px" AutoPostBack="true" OnSelectedIndexChanged="ddlCustName_indexchanged">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $('[id^=ddl]').on("change", function () {
                getQueryVariable('qplant');
                getQueryVariable('qfyear');
                getQueryVariable('qfmonth');
                getQueryVariable('qtyear');
                getQueryVariable('qtmonth');
            });
        });

        function getQueryVariable(variable) {
            var query = window.location.search.substring(1);
            var vars = query.split('&');
            for (var i = 0; i < vars.length; i++) {
                var pair = vars[i].split('=');
                if (pair[0] == variable) {
                    pair[1] = '';
                }
            }
        }
    </script>
</asp:Content>
