<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="frmstockdata.aspx.cs" Inherits="COTS.frmstockdata" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="styles/GridviewScroll.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .tab
        {
            position: relative;
            overflow: hidden;
            margin-left: 5px;
            margin-top: 5px;
            width: 100%;
            font-weight: bold;
            font-size: 14px;
        }
        .tab a
        {
            background-color: #fafbd9;
            float: left;
            outline: none;
            cursor: pointer;
            padding: 10px 50px;
            color: #74777b;
            border-right: 1px solid #9fa5a2;
            border-bottom: 1px solid #9fa5a2;
            border-top-right-radius: 20px;
        }
        .tab a:hover
        {
            background-color: #ddd;
            border-bottom: 3px solid #9fa5a2;
        }
        .tab a.active
        {
            background-color: #f0f399;
            border-bottom: 3px solid #139c24;
            color: #138859;
        }
        .tabcontent
        {
            display: none;
            border: 1px solid #8e8e8e;
            margin: 5px;
            padding: 5px;
            border-radius: 5px;
        }
        hr.style1
        {
            height: 10px;
            border: 0;
            box-shadow: 0 10px 10px -10px #bb1313 inset;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="pageTitleHead">
        TYRE STOCK DATA
    </div>
    <div class="contPage">
        <div class="tab">
            <a class="tablinks" onclick="openDiv(event, 'divMatrix')" id="defaultOpen">Matrix</a>
            <a class="tablinks" onclick="openDiv(event, 'divList')">List</a>
        </div>
        <div id="divList" class="tabcontent">
            <table cellspacing="0" rules="all" border="1" style="background-color: #ffffff; width: 100%;
                border-color: #ffffff; border-collapse: separate;">
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblSolid" Font-Bold="true" Text="" ForeColor="#5d138"
                            Font-Size="15px"></asp:Label>
                        <asp:GridView runat="server" ID="gvSolidList" AutoGenerateColumns="false" Width="100%"
                            HeaderStyle-BackColor="#abf5f5" HeaderStyle-Font-Bold="true" HeaderStyle-Height="22px"
                            HeaderStyle-HorizontalAlign="Center">
                            <Columns>
                                <asp:BoundField DataField="config" HeaderText="PLATFORM" />
                                <asp:BoundField DataField="tyresize" HeaderText="TYRE SIZE" />
                                <asp:BoundField DataField="rimsize" HeaderText="RIM" />
                                <asp:BoundField DataField="tyretype" HeaderText="TYPE" />
                                <asp:BoundField DataField="brand" HeaderText="BRAND" />
                                <asp:BoundField DataField="sidewall" HeaderText="SIDEWALL" />
                                <asp:BoundField DataField="ProcessID" HeaderText="PROCESS CODE" />
                                <asp:BoundField DataField="StockCount" HeaderText="STOCK QTY" />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr class="style1" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblPobList" Font-Bold="true" Text="" ForeColor="#5d138"
                            Font-Size="15px"></asp:Label>
                        <asp:GridView runat="server" ID="gvPobList" AutoGenerateColumns="false" Width="100%"
                            HeaderStyle-BackColor="#baccf5" HeaderStyle-Font-Bold="true" HeaderStyle-Height="22px"
                            HeaderStyle-HorizontalAlign="Center">
                            <Columns>
                                <asp:BoundField DataField="config" HeaderText="PLATFORM" />
                                <asp:BoundField DataField="tyresize" HeaderText="TYRE SIZE" />
                                <asp:BoundField DataField="rimsize" HeaderText="RIM" />
                                <asp:BoundField DataField="tyretype" HeaderText="TYPE" />
                                <asp:BoundField DataField="brand" HeaderText="BRAND" />
                                <asp:BoundField DataField="sidewall" HeaderText="SIDEWALL" />
                                <asp:BoundField DataField="ProcessID" HeaderText="PROCESS CODE" />
                                <asp:BoundField DataField="StockCount" HeaderText="STOCK QTY" />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divMatrix" class="tabcontent">
            <table cellspacing="0" rules="all" border="1" style="background-color: #ffffff; width: 100%;
                border-color: #ffffff; border-collapse: separate;">
                <tr>
                    <td>
                        <asp:GridView ID="gvSolidStock" runat="server" AutoGenerateColumns="false" GridLines="None"
                            OnRowCreated="gvSolidStock_RowCreated">
                            <Columns>
                            </Columns>
                            <HeaderStyle CssClass="GridviewScrollHeader" />
                            <RowStyle CssClass="GridviewScrollItem" />
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"
                            Font-Bold="true" Font-Size="15px"></asp:Label>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="gvPobStock" runat="server" AutoGenerateColumns="false" GridLines="None"
                            OnRowCreated="gvPobStock_RowCreated">
                            <Columns>
                            </Columns>
                            <HeaderStyle CssClass="GridviewScrollHeader" />
                            <RowStyle CssClass="GridviewScrollItem" />
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <script src="scripts/jquerymin182.js" type="text/javascript"></script>
    <script src="scripts/jqueryui191.js" type="text/javascript"></script>
    <script src="scripts/gridviewScroll.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var $ = jQuery.noConflict();
        document.getElementById("defaultOpen").click();
        function openDiv(evt, divID) {
            var i, tabcontent, tablinks;
            tabcontent = document.getElementsByClassName("tabcontent");
            for (i = 0; i < tabcontent.length; i++) {
                tabcontent[i].style.display = "none";
            }

            tablinks = document.getElementsByClassName("tablinks");
            for (i = 0; i < tablinks.length; i++) {
                tablinks[i].className = tablinks[i].className.replace(" active", "");
            }

            document.getElementById(divID).style.display = "block";
            evt.currentTarget.className += " active";
        }
    </script>
</asp:Content>
