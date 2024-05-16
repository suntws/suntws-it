<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="claimtrack.aspx.cs" Inherits="COTS.claimtrack" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div align="center" class="pageTitleHead">
        TRACK MY COMPLAINT
    </div>
    <div class="contPage">
        <div>
            <table>
                <tr>
                    <td align="center">
                        <asp:GridView runat="server" ID="gvClaimTrackList" AutoGenerateColumns="false" Width="1180px"
                            RowStyle-Height="22px" AllowPaging="true" PageSize="10" PagerStyle-Height="30px"
                            PagerStyle-Font-Bold="true" PagerStyle-Font-Size="15px" PagerStyle-HorizontalAlign="Center"
                            PagerStyle-VerticalAlign="Middle" OnPageIndexChanging="gvClaimTrackList_PageIndex">
                            <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" Height="22px" />
                            <Columns>
                                <asp:BoundField DataField="complaintno" HeaderText="COMPLAINT NO" />
                                <asp:BoundField DataField="complaintdate" HeaderText="COMPLAINT DATE" />
                                <asp:BoundField DataField="Commets" HeaderText="CUSTOMER COMMENTS" />
                                <asp:BoundField DataField="CustomerRefNo" HeaderText="CUSTOMER REF NO" />
                                <asp:BoundField DataField="claimstatus" HeaderText="STATUS" />
                                <asp:TemplateField HeaderText="ACTION">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkClaimNo" runat="server" Text="Show List" OnClick="lnkClaimNo_Click" /></span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div style="width: 900px; float: left; padding-left: 130px; padding-right: 130px;">
                            <div style="width: 898px; float: left; border: 1px solid #000; background-color: #056442;
                                font-weight: bold; color: #fff; font-size: 15px; text-align: center;">
                                <asp:Label runat="server" ID="lblClaimNo" ClientIDMode="Static" Text=""></asp:Label>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="width: 1180px; float: left; text-align: center;">
                        <div style="border: 1px solid #000; margin-bottom: 5px; text-align: left;">
                            <asp:GridView runat="server" ID="gvClaimApproveItems" AutoGenerateColumns="false"
                                Width="1180px" RowStyle-Height="22px">
                                <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" Height="22px" HorizontalAlign="Center" />
                                <Columns>
                                    <asp:BoundField DataField="brand" HeaderText="BRAND" ItemStyle-Width="120px" />
                                    <asp:BoundField DataField="tyresize" HeaderText="TYRE SIZE" ItemStyle-Width="150px" />
                                    <asp:BoundField DataField="stencilno" HeaderText="STENCIL NO." ItemStyle-Width="100px" />
                                    <asp:TemplateField HeaderText="COMPLAINT" ItemStyle-Width="350px">
                                        <ItemTemplate>
                                            <%#((string)Eval("appstyle")).Replace("~", "<br/>")%></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="OPERATING CONDITION" ItemStyle-Width="350px">
                                        <ItemTemplate>
                                            <%#((string)Eval("RunningHours")).Replace("~", "<br/>")%></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="CrmStatus" HeaderText="CRM STATUS" ItemStyle-Width="100px" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
