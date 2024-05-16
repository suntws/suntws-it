<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="default.aspx.cs" Inherits="COTS._default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="styles/slideShow1.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="scripts/slideshowmin.js" type="text/javascript"></script>
    <script type="text/javascript">
        var $ = jQuery.noConflict();
        $(document).ready(function () {
            $('#dss').divSlideShow({ height: 630, width: 257, arrow: "split", loop: 20, slideContainerClass: "slide-container", separatorClass: "separator",
                controlClass: "control", leftArrowClass: "control", rightArrowClass: "control", controlActiveClass: "control-active", controlHoverClass: "control-hover",
                controlContainerClass: "control-container"
            });
        }); 
    </script>
    <div align="center" class="pageTitleHead">
        SCOTS
    </div>
    <div class="contPage">
        <div style="display: none;" id="div_Domestic" runat="server">
            <table cellspacing="0" rules="all" border="1" style="background-color: #ffffff; width: 100%;
                border-color: White; border-collapse: separate;">
                <tr>
                    <td rowspan="3">
                        <asp:TextBox runat="server" ID="txtLeftSide" ClientIDMode="Static" TextMode="MultiLine"
                            Width="580px" Height="500px" Enabled="false" CssClass="form-control"></asp:TextBox>
                    </td>
                    <td style="text-align: right;">
                        <asp:LinkButton runat="server" ID="lnkGuidePdfFile" Text="USER GUIDE" ClientIDMode="Static"
                            OnClick="lnkGuidePdfFile_Click"></asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox runat="server" ID="txtPayTerms" ClientIDMode="Static" TextMode="MultiLine"
                            Width="585px" Height="200px" Enabled="false" CssClass="form-control"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox runat="server" ID="txtInstruction" ClientIDMode="Static" TextMode="MultiLine"
                            Width="585px" Height="283px" Enabled="false" CssClass="form-control"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
        <div style="display: none;" id="div_Export" runat="server">
            <table cellspacing="0" rules="all" border="1" style="background-color: #ffffff; width: 100%;
                border-color: White; border-collapse: separate;">
                <tr>
                    <td style="width: 30%;">
                        <div class="slideMainDiv">
                            <div id='dss'>
                                <div class='slide'>
                                    <img src="imageslide/SLIDE_AERIAL.jpg" alt="AERIAL" /></div>
                                <div class='slide'>
                                    <img src="imageslide/SLIDE_AIRPORT.jpg" alt="AIRPORT" /></div>
                                <div class='slide'>
                                    <img src="imageslide/SLIDE_INDUSTRIAL.jpg" alt="INDUSTRIAL" /></div>
                                <div class='slide'>
                                    <img src="imageslide/SLIDE_PORTS_TRAILERS.jpg" alt="PORTS TRAILERS" /></div>
                                <div class='slide'>
                                    <img src="imageslide/SLIDE_PRESS_ONS.jpg" alt="PRESS ONS" /></div>
                                <div class='slide'>
                                    <img src="imageslide/SLIDE_SKID_STEER.jpg" alt="SKID STEER" /></div>
                                <div class='slide'>
                                    <img src="imageslide/SLIDE_SOLID_RESILIENTS.jpg" alt="SOLID RESILIENTS" /></div>
                            </div>
                        </div>
                    </td>
                    <td style="vertical-align: top;">
                        <asp:Label runat="server" ID="lblTitle" ClientIDMode="Static" Text="Active Order List"
                            Font-Bold="true" ForeColor="#fb8c4b" Font-Size="15px"></asp:Label>
                        <br />
                        <br />
                        <asp:GridView runat="server" ID="gv_OrderList" AutoGenerateColumns="false" Width="100%"
                            HeaderStyle-BackColor="#FFCC00" HeaderStyle-Height="25px" RowStyle-Height="24px">
                            <Columns>
                                <asp:BoundField DataField="OrderRefNo" HeaderText="REF NO" />
                                <asp:BoundField DataField="CompletedDate" HeaderText="ORDER DATE" />
                                <asp:BoundField DataField="RevisedDate" HeaderText="REVISED DATE" />
                                <asp:BoundField DataField="DesiredShipDate" HeaderText="DESIRED SHIP DATE" />
                                <asp:BoundField DataField="itemqty" HeaderText="QTY" ItemStyle-HorizontalAlign="Right" />
                                <asp:TemplateField HeaderText="ACTION" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:HiddenField runat="server" ID="hdnOrderID" ClientIDMode="Static" Value='<%# Eval("ID") %>' />
                                        <asp:LinkButton runat="server" ID="lnkStaus" ClientIDMode="Static" Text="TRACK" OnClick="lnkStaus_Click"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
