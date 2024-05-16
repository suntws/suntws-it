<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="s3NetworkCount.aspx.cs" Inherits="TTS.s3NetworkCount" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div align="center" class="pageTitleHead">
        3S NETWORK COUNT</div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <div style="padding-left: 10px; padding-top: 10px;">
            <table>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>
                            <table cellspacing="0" rules="all" border="1" style="font-weight: bold; width: 1000px;
                                border-collapse: collapse;">
                                <tbody>
                                    <tr class="s3gvDashboardHead" style="background-color: #CAFAB7;">
                                        <th scope="col" style="background-color: #D6F5D6; width: 115px;">
                                            TOTAL INDIA
                                        </th>
                                        <th scope="col">
                                            3S Network Month/Year Activated
                                        </th>
                                        <th scope="col">
                                            3S Network Under Activation
                                        </th>
                                        <th scope="col">
                                            Month/Year Signed Under Probation
                                        </th>
                                        <th scope="col">
                                            Total 3S Network
                                        </th>
                                        <th scope="col">
                                            Month/Year Started Discussion
                                        </th>
                                        <th scope="col">
                                            Not Yet Identified
                                        </th>
                                    </tr>
                                    <tr style="line-height: 30px; font-weight: bold; text-align: center; font-size: 15px;">
                                        <td>
                                            <span id='totalCountCellHead'></span>
                                        </td>
                                        <td>
                                            <span id='totalCountCell1'></span>
                                        </td>
                                        <td>
                                            <span id='totalCountCell2'></span>
                                        </td>
                                        <td>
                                            <span id='totalCountCell3'></span>
                                        </td>
                                        <td>
                                            <span id='totalCountCell4'></span>
                                        </td>
                                        <td>
                                            <span id='totalCountCell5'></span>
                                        </td>
                                        <td>
                                            <span id='totalCountCell6'></span>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="line-height: 25px; padding-top: 20px;">
                        <asp:GridView runat="server" ID="gv_CountWestNorth" AutoGenerateColumns="false" Width="1000px"
                            HeaderStyle-BackColor="#BCDFF3" Font-Bold="true" RowStyle-BackColor="#F2F3F3"
                            HeaderStyle-CssClass="s3gvDashboardHead">
                            <Columns>
                                <asp:BoundField HeaderText="WEST & NORTH ZONE" DataField="MonthField" HeaderStyle-BackColor="#D6F5D6" />
                                <asp:BoundField HeaderText="3S Network Month/Year Activated" DataField="Activated" />
                                <asp:BoundField HeaderText="3S Network Under Activation" DataField="UnderActivation" />
                                <asp:BoundField HeaderText="Month/Year Signed Under Probation" DataField="SignedUnder" />
                                <asp:BoundField HeaderText="Total 3S Network" DataField="Total3s" />
                                <asp:BoundField HeaderText="Month/Year Started Discussion" DataField="StartedDiscussion" />
                                <asp:BoundField HeaderText="Not Yet Identified" DataField="NotYetIdentified" />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td style="line-height: 25px; padding-top: 15px;">
                        <asp:GridView runat="server" ID="gv_CountSouthEast" AutoGenerateColumns="false" Width="1000px"
                            HeaderStyle-BackColor="#BCDFF3" Font-Bold="true" RowStyle-BackColor="#F2F3F3"
                            HeaderStyle-CssClass="s3gvDashboardHead">
                            <Columns>
                                <asp:BoundField HeaderText="SOUTH & EAST ZONE" DataField="MonthField" HeaderStyle-BackColor="#D6F5D6" />
                                <asp:BoundField HeaderText="3S Network Month/Year Activated" DataField="Activated" />
                                <asp:BoundField HeaderText="3S Network Under Activation" DataField="UnderActivation" />
                                <asp:BoundField HeaderText="Month/Year Signed Under Probation" DataField="SignedUnder" />
                                <asp:BoundField HeaderText="Total 3S Network" DataField="Total3s" />
                                <asp:BoundField HeaderText="Month/Year Started Discussion" DataField="StartedDiscussion" />
                                <asp:BoundField HeaderText="Not Yet Identified" DataField="NotYetIdentified" />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <script type="text/javascript">
        function bind_RunningTotal() {
            prepareZoneWiseTotal('MainContent_gv_CountWestNorth');
            prepareZoneWiseTotal('MainContent_gv_CountSouthEast');
            prepareTotalIndia();

            PrepareBgColor_CountIncrementCell('MainContent_gv_CountWestNorth');
            PrepareBgColor_CountIncrementCell('MainContent_gv_CountSouthEast');
        }

        function prepareZoneWiseTotal(ctrl) {
            var gvRows = $('#' + ctrl + ' tr').length;
            for (var j = gvRows; j > 1; j--) {
                var curCol1 = $("#" + ctrl + " tr:nth-child(" + j + ")").find("td:eq(1)").html();
                var curCol2 = $("#" + ctrl + " tr:nth-child(" + j + ")").find("td:eq(2)").html();
                var curCol3 = $("#" + ctrl + " tr:nth-child(" + j + ")").find("td:eq(3)").html();
                var curCol4 = $("#" + ctrl + " tr:nth-child(" + j + ")").find("td:eq(4)").html();
                var curCol5 = $("#" + ctrl + " tr:nth-child(" + j + ")").find("td:eq(5)").html();
                var curCol6 = $("#" + ctrl + " tr:nth-child(" + j + ")").find("td:eq(6)").html();

                var k = parseInt(j) - 1;

                var prevCol1 = $("#" + ctrl + " tr:nth-child(" + k + ")").find("td:eq(1)").html();
                var prevCol2 = $("#" + ctrl + " tr:nth-child(" + k + ")").find("td:eq(2)").html();
                var prevCol3 = $("#" + ctrl + " tr:nth-child(" + k + ")").find("td:eq(3)").html();
                var prevCol4 = $("#" + ctrl + " tr:nth-child(" + k + ")").find("td:eq(4)").html();
                var prevCol5 = $("#" + ctrl + " tr:nth-child(" + k + ")").find("td:eq(5)").html();
                var prevCol6 = $("#" + ctrl + " tr:nth-child(" + k + ")").find("td:eq(6)").html();

                var count1 = parseInt(curCol1) + parseInt(prevCol1);
                var count2 = parseInt(curCol2) + parseInt(prevCol2);
                var count3 = parseInt(curCol3) + parseInt(prevCol3);
                var count4 = parseInt(curCol4) + parseInt(prevCol4);
                var count5 = parseInt(curCol5) + parseInt(prevCol5);
                var count6 = parseInt(curCol6) + parseInt(prevCol6);

                $("#" + ctrl + " tr:nth-child(" + k + ")").find("td:eq(1)").html(count1);
                $("#" + ctrl + " tr:nth-child(" + k + ")").find("td:eq(2)").html(count2);
                $("#" + ctrl + " tr:nth-child(" + k + ")").find("td:eq(3)").html(count3);
                $("#" + ctrl + " tr:nth-child(" + k + ")").find("td:eq(4)").html(count4);
                $("#" + ctrl + " tr:nth-child(" + k + ")").find("td:eq(5)").html(count5);
                $("#" + ctrl + " tr:nth-child(" + k + ")").find("td:eq(6)").html(count6);
            }
        }

        function prepareTotalIndia() {
            var d = new Date(), n = d.getMonth(), y = d.getFullYear();
            var monthNames = ["JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC"];
            var month_name = monthNames[n];

            var westnorth = "MainContent_gv_CountWestNorth tr:nth-child(2)";
            var southeast = "MainContent_gv_CountSouthEast tr:nth-child(2)";
            var cellCount1 = parseInt($("#" + westnorth).find("td:eq(1)").html()) + parseInt($("#" + southeast).find("td:eq(1)").html());
            var cellCount2 = parseInt($("#" + westnorth).find("td:eq(2)").html()) + parseInt($("#" + southeast).find("td:eq(2)").html());
            var cellCount3 = parseInt($("#" + westnorth).find("td:eq(3)").html()) + parseInt($("#" + southeast).find("td:eq(3)").html());
            var cellCount4 = parseInt($("#" + westnorth).find("td:eq(4)").html()) + parseInt($("#" + southeast).find("td:eq(4)").html());
            var cellCount5 = parseInt($("#" + westnorth).find("td:eq(5)").html()) + parseInt($("#" + southeast).find("td:eq(5)").html());
            var cellCount6 = parseInt($("#" + westnorth).find("td:eq(6)").html()) + parseInt($("#" + southeast).find("td:eq(6)").html());

            $('#totalCountCellHead').html(month_name + '-' + y);
            $('#totalCountCell1').html(cellCount1);
            $('#totalCountCell2').html(cellCount2);
            $('#totalCountCell3').html(cellCount3);
            $('#totalCountCell4').html(cellCount4);
            $('#totalCountCell5').html(cellCount5);
            $('#totalCountCell6').html(cellCount6);
        }

        function PrepareBgColor_CountIncrementCell(ctrl) {
            var gvRows = $('#' + ctrl + ' tr').length;
            for (var j = gvRows; j > 1; j--) {
                var curCol1 = $("#" + ctrl + " tr:nth-child(" + j + ")").find("td:eq(1)").html();
                var curCol2 = $("#" + ctrl + " tr:nth-child(" + j + ")").find("td:eq(2)").html();
                var curCol3 = $("#" + ctrl + " tr:nth-child(" + j + ")").find("td:eq(3)").html();
                var curCol4 = $("#" + ctrl + " tr:nth-child(" + j + ")").find("td:eq(4)").html();
                var curCol5 = $("#" + ctrl + " tr:nth-child(" + j + ")").find("td:eq(5)").html();
                var curCol6 = $("#" + ctrl + " tr:nth-child(" + j + ")").find("td:eq(6)").html();

                var k = parseInt(j) - 1;

                var prevCol1 = $("#" + ctrl + " tr:nth-child(" + k + ")").find("td:eq(1)").html();
                var prevCol2 = $("#" + ctrl + " tr:nth-child(" + k + ")").find("td:eq(2)").html();
                var prevCol3 = $("#" + ctrl + " tr:nth-child(" + k + ")").find("td:eq(3)").html();
                var prevCol4 = $("#" + ctrl + " tr:nth-child(" + k + ")").find("td:eq(4)").html();
                var prevCol5 = $("#" + ctrl + " tr:nth-child(" + k + ")").find("td:eq(5)").html();
                var prevCol6 = $("#" + ctrl + " tr:nth-child(" + k + ")").find("td:eq(6)").html();

                if (parseInt(curCol1) < parseInt(prevCol1))
                    $("#" + ctrl + " tr:nth-child(" + k + ")").find("td:eq(1)").css({ 'background-color': '#21EE3A' });

                else if (parseInt(curCol1) == parseInt(prevCol1))
                    $("#" + ctrl + " tr:nth-child(" + k + ")").find("td:eq(1)").css({ 'background-color': '#ff0000', 'color': '#fff' });

                if (parseInt(curCol2) < parseInt(prevCol2))
                    $("#" + ctrl + " tr:nth-child(" + k + ")").find("td:eq(2)").css({ 'background-color': '#21EE3A' });
                else if (parseInt(curCol2) == parseInt(prevCol2))
                    $("#" + ctrl + " tr:nth-child(" + k + ")").find("td:eq(2)").css({ 'background-color': '#ff0000', 'color': '#fff' });

                if (parseInt(curCol3) < parseInt(prevCol3))
                    $("#" + ctrl + " tr:nth-child(" + k + ")").find("td:eq(3)").css({ 'background-color': '#21EE3A' });
                else if (parseInt(curCol3) == parseInt(prevCol3))
                    $("#" + ctrl + " tr:nth-child(" + k + ")").find("td:eq(3)").css({ 'background-color': '#ff0000', 'color': '#fff' });

                if (parseInt(curCol4) < parseInt(prevCol4))
                    $("#" + ctrl + " tr:nth-child(" + k + ")").find("td:eq(4)").css({ 'background-color': '#21EE3A' });
                else if (parseInt(curCol4) == parseInt(prevCol4))
                    $("#" + ctrl + " tr:nth-child(" + k + ")").find("td:eq(4)").css({ 'background-color': '#ff0000', 'color': '#fff' });

                if (parseInt(curCol5) < parseInt(prevCol5))
                    $("#" + ctrl + " tr:nth-child(" + k + ")").find("td:eq(5)").css({ 'background-color': '#21EE3A' });
                else if (parseInt(curCol5) == parseInt(prevCol5))
                    $("#" + ctrl + " tr:nth-child(" + k + ")").find("td:eq(5)").css({ 'background-color': '#ff0000', 'color': '#fff' });

                if (parseInt(curCol6) < parseInt(prevCol6))
                    $("#" + ctrl + " tr:nth-child(" + k + ")").find("td:eq(6)").css({ 'background-color': '#21EE3A' });
                else if (parseInt(curCol6) == parseInt(prevCol6))
                    $("#" + ctrl + " tr:nth-child(" + k + ")").find("td:eq(6)").css({ 'background-color': '#ff0000', 'color': '#fff' });

            }
        }
    </script>
</asp:Content>
