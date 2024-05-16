<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="loadplanning.ascx.cs"
    Inherits="TTS.cargomanagement.loadplanning" %>
<style type="text/css">
    #tbCargoPlan th
    {
        background-color: #ccc;
        text-align: left;
    }
    
    .hide
    {
        display:none;
    }
</style>
<div style="width: 1080px;">
    <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label>
    <div id="divContent">
        <table>
            <tr>
                <td>
                    <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #000;
                        width: 1078px;" id="tbCargoPlan">
                        <tr>
                            <th>
                                CUSTOMER
                            </th>
                            <td>
                                <asp:DropDownList ID="ddlCustomer" ClientIDMode="Static" runat="server" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged"
                                    Width="400px" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <th>
                                ORIENT 1
                            </th>
                            <th>
                                ORIENT 2
                            </th>
                            <th>
                                ORIENT 3
                            </th>
                        </tr>
                        <tr>
                            <th>
                                ORDER
                            </th>
                            <td>
                                <asp:DropDownList ID="ddlOrder" ClientIDMode="Static" runat="server" OnSelectedIndexChanged="ddlOrder_SelectedIndexChanged"
                                    Width="300px" AutoPostBack="true">
                                </asp:DropDownList>
                                <asp:Label runat="server" ID="lblUserDetails" ClientIDMode="Static" Text=""></asp:Label>
                            </td>
                            <td rowspan="3">
                                <img src="../images/TyreOrient1.jpg" alt="orient 1" height="80px" width="80px" />
                            </td>
                            <td rowspan="3">
                                <img src="../images/TyreOrient2.jpg" alt="orient 2" height="80px" width="140px" />
                            </td>
                            <td rowspan="3">
                                <img src="../images/TyreOrient3.jpg" alt="orient 3" height="80px" width="120px" />
                            </td>
                        </tr>
                        <tr>
                            <th>
                                CONTAINER
                            </th>
                            <td>
                                <asp:Label runat="server" ID="lblContainerDimension" ClientIDMode="Static" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                SPECIAL INSTRUCTION
                            </th>
                            <td>
                                <asp:Label runat="server" ID="lblCargoSplIns" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="width: 1078px; float: left; margin-bottom: 5px; margin-top: 5px;">
                        <asp:GridView runat="server" ID="gv_CargoOrderList" AutoGenerateColumns="false" Width="1078px"
                            RowStyle-Height="22px">
                            <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" Height="22px" />
                            <Columns>
                                <asp:BoundField DataField="category" HeaderText="CATEGORY" HeaderStyle-Width="100px" />
                                <asp:BoundField DataField="Config" HeaderText="PLATFORM" HeaderStyle-Width="100px" />
                                <asp:BoundField DataField="brand" HeaderText="BRAND" HeaderStyle-Width="100px" />
                                <asp:BoundField DataField="sidewall" HeaderText="SIDEWALL" HeaderStyle-Width="100px" />
                                <asp:BoundField DataField="tyretype" HeaderText="TYPE" HeaderStyle-Width="60px" />
                                <asp:BoundField DataField="tyresize" HeaderText="TYRE SIZE" HeaderStyle-Width="150px" />
                                <asp:BoundField DataField="rimsize" HeaderText="RIM" HeaderStyle-Width="40px" />
                                <asp:BoundField DataField="quantity" HeaderText="QTY" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Right" />
                            </Columns>
                        </asp:GridView>
                        <div style="width: 1078px; float: right; margin-bottom: 10px; text-align: right;">
                            <asp:Label runat="server" ID="lblTotQTy" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="width: 1078px; float: left;">
                        <asp:Label runat="server" ID="lblGvHeadMsg" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                        <asp:GridView runat="server" ID="gvLoadedTyreList" AutoGenerateColumns="false" Font-Names="Arial"
                            Font-Size="12px"  HeaderStyle-BackColor="green" RowStyle-BackColor="#C2D69B"
                            HeaderStyle-Height="22px" HeaderStyle-ForeColor="white" HeaderStyle-Font-Size="12px" 
                            Width="100%" OnRowCreated="gvLoadedTyreList_RowCreated" OnRowDataBound="gvLoadedTyreList_RowDataBound" >
                            <Columns>
                                <asp:TemplateField HeaderText="S No" ItemStyle-Width="25">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="PLATFORM" DataField="Config" />
                                <asp:BoundField HeaderText="BRAND" DataField="Brand" />
                                <asp:BoundField HeaderText="SIDEWALL" DataField="Sidewall" />
                                <asp:BoundField HeaderText="TYRE TYPE" DataField="TyreType" />
                                <asp:BoundField HeaderText="TYRE SIZE" DataField="Tyresize" />
                                <asp:BoundField HeaderText="RIM" DataField="Rimsize" />
                                <asp:BoundField HeaderText="X" DataField="X_axis" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="Y" DataField="Y_axis" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="Z" DataField="Z_axis" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="LENGTH" DataField="Length" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="WIDTH" DataField="Width" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="HEIGHT" DataField="Height" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="QTY" DataField="Quantity" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="ORIENT" DataField="Orient" ItemStyle-HorizontalAlign="Center" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align:right; padding-right:10px">
                    <span id="lnkView3d" class="hide"> <a href="<%=(HttpContext.Current.Request.Url.AbsoluteUri).Substring(0,(HttpContext.Current.Request.Url.AbsoluteUri).IndexOf(HttpContext.Current.Request.Url.AbsolutePath))%>/cargomanagement/ThreeDRendering.aspx" target="_blank"> VIEW IN 3D </a></span>
                </td>
            </tr>
        </table>
    </div>
</div>
<script type="text/javascript">
    $("#lblPageHead").text("load planning");

    $("#lnkView3d").click(function (event) {

    });

    function ShowLinkTo3d(val)
    {
        if(val === 1)
        {
            $('#lnkView3d').removeClass("hide");
        }
        else if(val === 0) {
            $('#lnkView3d').addClass("hide");
        }
    }

    $(document).ready(function () {
        var gv = "#<%=gvLoadedTyreList.ClientID %>";
        var rows = $(gv).find("tr")
        rows.each(function (e) {
            var str = "";
        })
    });
</script>
