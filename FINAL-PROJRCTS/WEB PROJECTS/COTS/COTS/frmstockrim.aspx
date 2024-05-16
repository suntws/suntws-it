﻿<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="frmstockrim.aspx.cs" Inherits="COTS.frmstockrim" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #tdRimData th
        {
            text-align: right;
            font-weight: normal;
            padding-right: 10px;
            background-color: #f1f1f1;
        }
        #tdRimData td
        {
            text-align: left;
            font-weight: bold;
            padding-left: 10px;
            background-color: #b7ff9a;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div align="center" class="pageTitleHead">
        WHEEL STOCK DATA
    </div>
    <div class="contPage">
        <table cellspacing="0" rules="all" border="1" style="background-color: #ffffff; width: 100%;
            border-color: #cccbfb; border-collapse: separate; line-height: 25px;">
            <tr>
                <td colspan="2">
                    <asp:GridView runat="server" ID="gvRimStock" AutoGenerateColumns="false" Width="100%"
                        HeaderStyle-BackColor="#FEFE8B" HeaderStyle-Font-Bold="true" HeaderStyle-Height="22px"
                        HeaderStyle-HorizontalAlign="Center" OnDataBound="gvRimStock_DatBound">
                        <Columns>
                            <asp:TemplateField HeaderText="EDC NO" ItemStyle-Font-Bold="true" ItemStyle-Font-Size="15px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblEdcNo" ClientIDMode="Static" Text='<%# Eval("EDCNO") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="WHEEL SPECIFICATION (EDC REF)">
                                <ItemTemplate>
                                    <asp:FormView runat="server" ID="frmRimProcessID_Details" Width="100%">
                                        <ItemTemplate>
                                            <table cellspacing="0" rules="all" border="1" style="background-color: #ecf6ff; width: 100%;
                                                border-color: White; line-height: 20px;" id="tdRimData">
                                                <tr>
                                                    <th>
                                                        RIM SIZE
                                                    </th>
                                                    <td>
                                                        <%# Eval("Rimsize") %>
                                                    </td>
                                                    <th>
                                                        MOUNTING HOLES DIA
                                                    </th>
                                                    <td>
                                                        <%# Eval("MHdia")%>
                                                    </td>
                                                    <th>
                                                        FIXING HOLES DIA
                                                    </th>
                                                    <td>
                                                        <%# Eval("FHdia")%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        RIM TYPE
                                                    </th>
                                                    <td>
                                                        <%# Eval("RimType") %>
                                                        <%# Eval("NoofPiece").ToString() !="" ? " - ":"" %>
                                                        <%# Eval("NoofPiece") %>
                                                    </td>
                                                    <th>
                                                        MOUNTING HOLES TYPE
                                                    </th>
                                                    <td>
                                                        <%# Eval("MHtype")%>
                                                        <%# Eval("MHtype").ToString() == "COUNTERSINK SPHERICAL" && Eval("radius").ToString() != "" ? (" - RADIUS " + Eval("radius").ToString()) : ""%>
                                                        <%# Eval("MHtype").ToString() == "COUNTERSINK CONICAL" && Eval("angle").ToString() != "" ? (" - ANGLE " + Eval("angle").ToString()) : ""%>
                                                    </td>
                                                    <th>
                                                        FIXING HOLES TYPE
                                                    </th>
                                                    <td>
                                                        <%# Eval("FHtype")%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        TYRE CATEGORY
                                                    </th>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblTyreCategory" ClientIDMode="Static" Text='<%# Eval("TyreCategory") %>'></asp:Label>
                                                    </td>
                                                    <th>
                                                        DISC OFFSET
                                                    </th>
                                                    <td>
                                                        <%# Eval("DiscOffSet") %>
                                                    </td>
                                                    <th>
                                                        BORE DIA
                                                    </th>
                                                    <td>
                                                        <%# Eval("Boredia")%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        PILOTED
                                                    </th>
                                                    <td>
                                                        <%# Eval("Piloted") %>
                                                    </td>
                                                    <th>
                                                        DISC THICKNESS
                                                    </th>
                                                    <td>
                                                        <%# Eval("DiscThickness")%>
                                                    </td>
                                                    <th>
                                                        PAINTING COLOR
                                                    </th>
                                                    <td>
                                                        <%# Eval("PaintingColor")%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        NO. OF MOUNTING HOLES
                                                    </th>
                                                    <td>
                                                        <%# Eval("NoOfMH") %>
                                                    </td>
                                                    <th>
                                                        NO. OF FIXING HOLES
                                                    </th>
                                                    <td>
                                                        <%# Eval("NoofFH") %>
                                                    </td>
                                                    <th>
                                                        WALL THICKNESS
                                                    </th>
                                                    <td>
                                                        <%# Eval("WallThickness")%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        MOUNTING HOLES PCD
                                                    </th>
                                                    <td>
                                                        <%# Eval("MHpcd")%>
                                                    </td>
                                                    <th>
                                                        FIXING HOLES PCD
                                                    </th>
                                                    <td>
                                                        <%# Eval("FHpcd") %>
                                                    </td>
                                                    <th>
                                                        EDC-NO/ PROCESS-ID
                                                    </th>
                                                    <td>
                                                        <%# Eval("EDCNO") %>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </asp:FormView>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="STOCK QTY" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Bold="true"
                                ItemStyle-Font-Size="15px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblRimStockQty" ClientIDMode="Static" Text='<%# Eval("RimStock") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
