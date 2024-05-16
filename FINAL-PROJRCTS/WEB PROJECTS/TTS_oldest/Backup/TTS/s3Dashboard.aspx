<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="s3Dashboard.aspx.cs" Inherits="TTS.s3Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div align="center" class="pageTitleHead">
        3S NETWORK DETAILS</div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <div style="padding-left: 10px; padding-top: 10px;">
            <table>
                <tr>
                    <td>
                        <div style="width: 1000px; float: left;">
                            <div style="width: 180px; float: left;">
                                <span class="headCss">Zone</span>
                                <asp:DropDownList runat="server" ID="ddl3sZone" ClientIDMode="Static" Width="150px">
                                </asp:DropDownList>
                            </div>
                            <div style="width: 300px; float: left;">
                                <span class="headCss">State</span>
                                <asp:DropDownList runat="server" ID="ddl3sState" ClientIDMode="Static" Width="280px">
                                </asp:DropDownList>
                            </div>
                            <div style="width: 300px; float: left; display: none;">
                                <span class="headCss">City</span>
                                <asp:DropDownList runat="server" ID="ddl3sCity" ClientIDMode="Static" Width="280px">
                                </asp:DropDownList>
                            </div>
                            <div style="width: 300px; float: left; display: none;">
                                <span class="headCss">Category</span>
                                <asp:DropDownList runat="server" ID="ddl3sCategory" ClientIDMode="Static" Width="280px">
                                </asp:DropDownList>
                            </div>
                            <div style="width: 75px; float: left; margin-top: 10px;">
                                <asp:Button runat="server" ID="btn3sListShow" ClientIDMode="Static" CssClass="btnshow"
                                    Text="SHOW" OnClick="btn3sListShow_Click" />
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="line-height: 25px; padding-top: 15px;">
                        <asp:Label ID="lblErrMsg" runat="server" ClientIDMode="Static" ForeColor="Red" Text=""></asp:Label>
                        <asp:GridView runat="server" ID="gv_3sDashboard" AutoGenerateColumns="false" Width="1070px"
                            HeaderStyle-CssClass="s3gvDashboardHead" Font-Bold="true" RowStyle-BackColor="#F2F3F3"
                            OnSorting="gvReviewList_sorting" AllowSorting="true">
                            <%--OnDataBound="gv_3sDashboard_OnDataBound"--%>
                            <Columns>
                                <asp:BoundField HeaderText="Zone" DataField="Zone" SortExpression="Zone" />
                                <asp:BoundField HeaderText="State" DataField="StateName" SortExpression="StateName" />
                                <asp:BoundField HeaderText="City" DataField="City" SortExpression="City" />
                                <asp:TemplateField HeaderText="Customer" SortExpression="CustName" ItemStyle-Width="120px">
                                    <ItemTemplate>
                                        <%# Bind_CustName(Eval("CustName").ToString(),Eval("ID").ToString())%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tier" ItemStyle-Width="50px">
                                    <ItemTemplate>
                                        <%# Bind_3sTier(Eval("ID").ToString())%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="3S Network Activated">
                                    <ItemTemplate>
                                        <%# Bind_3sMonthYears(Eval("ID").ToString(), "12")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="3S Network<br/>under activation">
                                    <ItemTemplate>
                                        <%# Bind_3sMonthYears(Eval("ID").ToString(), "5")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Signed Under Probation">
                                    <ItemTemplate>
                                        <%# Bind_3sMonthYears(Eval("ID").ToString(), "9")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Started discussion">
                                    <ItemTemplate>
                                        <%# Bind_3sMonthYears(Eval("ID").ToString(), "7")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Not yet identified">
                                    <ItemTemplate>
                                        <%# Bind_3sMonthYears(Eval("ID").ToString(), "11")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Feedback" ItemStyle-Width="200px">
                                    <ItemTemplate>
                                        <%# Bind_Feedback(Eval("Comments").ToString(), Eval("UserName").ToString())%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <script type="text/javascript">
        function goto3sChangePage(s3CustID) {
            var pathname = window.location.href.toLowerCase();
            var splitval = pathname.split('/s3dashboard.aspx');
            window.location.href = splitval[0].toString() + '/s3entry.aspx?qKey=' + s3CustID;
        }
    </script>
</asp:Content>
