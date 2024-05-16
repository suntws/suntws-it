<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="processidrimsizeassign.aspx.cs" Inherits="TTS.processidrimsizeassign" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
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
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: center;" class="pageTitleHead">
        TYRE SIZE ASSIGN BASED ON RIM SIZE
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="16px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="background-color: #ecf6ff; width: 100%;
            border-color: White;">
            <tr>
                <th style="text-align: right; padding-right: 10px;">
                    EDC-NO / PROCESS-ID
                </th>
                <td style="text-align: left; padding-left: 10px;">
                    <asp:DropDownList runat="server" ID="ddlEDCNO" ClientIDMode="Static" AutoPostBack="true"
                        Width="250px" OnSelectedIndexChanged="ddlEDCNO_SelectedIndexChanged" CssClass="form-control"
                        Font-Bold="true">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="2">
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
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:CheckBoxList runat="server" ID="chkTyreSize" ClientIDMode="Static" RepeatColumns="5"
                        RepeatDirection="Vertical" RepeatLayout="Table" Width="100%" BackColor="#fde7b1"
                        Font-Bold="true">
                    </asp:CheckBoxList>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: center;">
                    <asp:Button ID="btnSave" Text="ASSIGN LIST SAVE" runat="server" OnClick="btnSave_Click"
                        Visible="false" CssClass="btn btn-success" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:GridView runat="server" ID="gvAssignEdcTyreSize" AutoGenerateColumns="false"
                        Width="100%">
                        <Columns>
                            <asp:BoundField DataField="TyreSize" HeaderText="TYRE SIZE" />
                            <asp:BoundField DataField="RimSize" HeaderText="RIM" />
                            <asp:BoundField DataField="AssignDate" HeaderText="ASSIGNED ON" />
                            <asp:BoundField DataField="username" HeaderText="ASSIGN BY" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button runat="server" ID="btnDelete" ClientIDMode="Static" Text="DELETE" CssClass="btn btn-danger"
                                        OnClick="btnDelete_Click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
