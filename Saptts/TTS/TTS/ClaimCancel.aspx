<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="ClaimCancel.aspx.cs" Inherits="TTS.ClaimCancel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div align="center" class="pageTitleHead">
        <asp:Label runat="server" ID="lblTrackHead" ClientIDMode="Static" Text=""></asp:Label>
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <div style="padding-left: 5px;">
            <table style="width: 1070px;">
                <tr>
                    <td align="center">
                        <asp:GridView runat="server" ID="gvClaimTrackList" AutoGenerateColumns="false" Width="1065px"
                            RowStyle-Height="22px" OnDataBound="gvClaimTrackList_OnDataBound">
                            <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" Height="22px" />
                            <Columns>
                                <asp:BoundField DataField="custname" HeaderText="CUSTOMER NAME" ItemStyle-Width="200px" />
                                <asp:BoundField DataField="complaintno" HeaderText="COMPLAINT NO." ItemStyle-Width="80px" />
                                <asp:BoundField DataField="complaintdate" HeaderText="COMPLAINT DATE" ItemStyle-Width="80px" />
                                <asp:BoundField DataField="plant" HeaderText="PLANT" ItemStyle-Width="80px" />
                                <asp:BoundField DataField="Qty" HeaderText="QTY" ItemStyle-Width="60px" />
                                <asp:BoundField DataField="claimstatus" HeaderText="STATUS" ItemStyle-Width="200px" />
                                <asp:TemplateField HeaderText="ACTION" ItemStyle-Width="50px">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkClaimNo" runat="server" Text="Show List" OnClick="lnkClaimNo_Click" /></span>
                                        <asp:HiddenField runat="server" ID="hdnClaimCustCode" Value='<%# Eval("custcode") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div style="width: 1065px; float: left;">
                            <div style="width: 1064px; float: left; border: 1px solid #000; background-color: #056442;
                                font-weight: bold; color: #fff; font-size: 15px;">
                                <div style="width: 525px; float: left; text-align: left; padding-right: 5px;">
                                    <asp:Label runat="server" ID="lblClaimCustName" ClientIDMode="Static" Text=""></asp:Label>
                                </div>
                                <div style="width: 2px; float: left;">
                                    <asp:Label runat="server" ID="lblClaim" ClientIDMode="Static" Text=""></asp:Label>
                                </div>
                                <div style="width: 520px; float: left; text-align: right; padding-left: 5px;">
                                    <asp:Label runat="server" ID="lblClaimNo" ClientIDMode="Static" Text=""></asp:Label>
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td id="divgvclaimapproved" style="width: 1065px; float: left;">
                        <div style="border: 1px solid #000; margin-bottom: 5px; text-align: left;">
                            <asp:GridView runat="server" ID="gvClaimApproveItems" AutoGenerateColumns="false"
                                Width="1064px" RowStyle-Height="22px">
                                <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" Height="22px" HorizontalAlign="Center" />
                                <Columns>
                                    <asp:BoundField DataField="config" HeaderText="PLATFORM" ItemStyle-Width="95px" />
                                    <asp:TemplateField HeaderText="TYPE MENTIONED BY" ItemStyle-Width="90px">
                                        <ItemTemplate>
                                            <%# Eval("tyretype").ToString() == Eval("EdcType").ToString() ? (Eval("tyretype").ToString() == Eval("CustgvnType").ToString() ? Eval("tyretype").ToString() : (Eval("CustgvnType").ToString() != "" ? "<span class='headCss'>CUSTOMER : </span>" + Eval("CustgvnType").ToString() + "<br/>" : "") + "<span class='headCss'>QC : </span>" + Eval("tyretype").ToString() + "<br/>") : ((Eval("CustgvnType").ToString() != "" ? "<span class='headCss'>CUSTOMER : </span>" + Eval("CustgvnType").ToString() + "<br/>" : "") + "<span class='headCss'>QC : </span>" + Eval("tyretype").ToString() + "<br/>")%>
                                            <%#Eval("EdcType").ToString() != "" ? "<span class='headCss'>EDC : </span>" + Eval("EdcType").ToString() + "<br/>" : ""%>
                                            <%# (Eval("CrmType").ToString() == Eval("EdcType").ToString() && Eval("tyretype").ToString() == Eval("EdcType").ToString()) ? "" : (Eval("CrmType").ToString() != "" ? "<span class='headCss'>CRM : </span>" + Eval("CrmType").ToString() + "<br/>" : "")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="brand" HeaderText="BRAND" ItemStyle-Width="95px" />
                                    <asp:BoundField DataField="tyresize" HeaderText="TYRE SIZE" ItemStyle-Width="110px" />
                                    <asp:BoundField DataField="stencilno" HeaderText="STENCIL" ItemStyle-Width="70px" />
                                    <asp:TemplateField HeaderText="CONCLUSION" ItemStyle-Width="50px">
                                        <ItemTemplate>
                                            <%# Eval("ConclusionStatus")%><%# Eval("ConclusionStatus").ToString() == "Others" ? " - " + Eval("otherconclusion").ToString() : ""%>
                                            <asp:HiddenField ID="hdnstatusid" runat="server" Value='<%# Eval("statusid")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="CrmStatus" HeaderText="COMPLAINT" ItemStyle-Width="50px" />
                                    <asp:BoundField DataField="ClaimDescription" HeaderText="COMPLAINT DESC" ItemStyle-Width="50px" />
                                    <asp:TemplateField HeaderText="PLANT" ItemStyle-Width="50px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblplant" runat="server" Text='<%# Eval("assigntoqc")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div id="divcancelcomment" style="display: none; width: 1065px; float: left; border: 1px solid #000;
                            margin-bottom: 5px;">
                            <div style="width: 1063px; float: left; padding: 2px;">
                                <asp:Label ID="lblcancelComments" runat="server" Text="" />
                            </div>
                            <div style="width: 1063px; float: left; padding: 2px;">
                                <asp:Label ID="lblcanceluser" runat="server" Text="" />
                            </div>
                        </div>
                        <asp:Label ID="lblMsg" runat="server" ClientIDMode="Static" Text="" ForeColor="Red"
                            Font-Bold="true"></asp:Label>
                        <div id="divcancel" style="display: none; width: 1000px; float: left; padding: 6px">
                            <span class="headCss">CANCEL COMMENTS</span><asp:TextBox ID="txtCancelComments" runat="server"
                                TextMode="MultiLine" Text="" Width="1000px" Height="50px" onKeyUp="javascript:CheckMaxLength(this, 1999);"
                                onChange="javascript:CheckMaxLength(this, 1999);"></asp:TextBox>
                            <div style="text-align: center;">
                                <asp:Button ID="btnCancel" runat="server" Text="CANCEL CLAIM" CssClass="btnclear"
                                    OnClick="btnCancel_Click" /></div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <asp:HiddenField runat="server" ID="hdnCustCode" ClientIDMode="Static" Value="" />
    </div>
    <script type="text/javascript">
        function displayblock(ctrlID) { $('#' + ctrlID).css({ 'display': 'block' }); }
    </script>
</asp:Content>
