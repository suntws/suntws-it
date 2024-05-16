<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="ProcessIdCreateRim.aspx.cs" Inherits="TTS.ProcessIdCreateRim" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        select, input[type="text"]
        {
            margin: 5px;
        }
        .recCss
        {
            float: left;
            background-color: #075865;
            color: #fff;
            font-size: 15px;
            font-weight: bold;
            text-align: center;
            width: 100%;
        }
        #tdRimData th
        {
            text-align: right;
            font-weight: normal;
            padding-right: 10px;
            background-color: #f1f1f1;
            color: #000000;
        }
        #tdRimData td
        {
            text-align: left;
            font-weight: bold;
            padding-left: 10px;
            background-color: #b7ff9a;
        }
        .lnkCss
        {
            color: #0000ef;
            cursor: pointer;
            text-decoration: underline;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: center;" class="pageTitleHead">
        CHECK & CREATE RIM PROCESS-ID
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="16px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="background-color: #ecf6ff; width: 100%;
            border-color: White;">
            <tr>
                <th>
                    RIM SIZE
                </th>
                <td>
                    <asp:DropDownList data="RIMSIZE" runat="server" ID="ddlRimSize" ui-Name="RIMSIZE"
                        Width="250" ClientIDMode="Static" CssClass="form-control">
                    </asp:DropDownList>
                    <asp:TextBox ID="txtRIMSIZE" Width="250px" runat="server" ClientIDMode="Static" CssClass="form-control"
                        MaxLength="20"></asp:TextBox>
                </td>
                <th>
                    NO. OF FIXING HOLES
                </th>
                <td>
                    <asp:DropDownList data="NOFH" runat="server" ID="ddlNOFH" ui-Name="NOFH" ClientIDMode="Static"
                        Width="250px" CssClass="form-control">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    RIM TYPE
                </th>
                <td>
                    <div>
                        <asp:DropDownList data="RIMTYPE" runat="server" ID="ddlRimtype" ui-Name="RIMTYPE"
                            Width="250px" ClientIDMode="Static" CssClass="form-control">
                            <asp:ListItem Text="--SELECT--" Value="--SELECT--"></asp:ListItem>
                            <asp:ListItem Text="SINGLE PIECE" Value="SINGLE PIECE"></asp:ListItem>
                            <asp:ListItem Text="SPLIT RIMS" Value="SPLIT RIMS"></asp:ListItem>
                            <asp:ListItem Text="MULTI PIECE" Value="MULTI PIECE"></asp:ListItem>
                            <asp:ListItem Text="MOULD-ON" Value="MOULD-ON"></asp:ListItem>
                            <asp:ListItem Text="POB WHEEL" Value="POB WHEEL"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div id="divrimsspecify" runat="server" clientidmode="Static" style="display: none;
                        float: left; background-color: #5F3212;">
                        <span style="padding-left: 10px; font-weight: bold; padding-top: 5px; color: #fff;">
                            NO OF PIECE</span>
                        <asp:DropDownList data="NOOFPIECE" runat="server" ID="ddlNoofpiece" ui-Name="NOOFPIECE"
                            ClientIDMode="Static" Width="250px" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </td>
                <th>
                    FIXING HOLES PCD
                </th>
                <td>
                    <asp:DropDownList data="FHPCD" runat="server" ID="ddlFPcd" ui-Name="FHPCD" ClientIDMode="Static"
                        Width="250px" CssClass="form-control">
                    </asp:DropDownList>
                    <asp:TextBox ID="txtFHPCD" Width="250px" runat="server" ClientIDMode="Static" CssClass="form-control"
                        Text="" MaxLength="6" onkeypress="return isNumberKey(event)"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    TYRE CATEGORY
                </th>
                <td>
                    <asp:DropDownList data="TYRETYPE" runat="server" ID="ddlTyreType" ui-Name="TYRETYPE"
                        ClientIDMode="Static" Width="250px" CssClass="form-control">
                        <asp:ListItem Text="--SELECT--" Value="--SELECT--"></asp:ListItem>
                        <asp:ListItem Text="SOLID" Value="SOLID"></asp:ListItem>
                        <asp:ListItem Text="PNEUMATIC" Value="PNEUMATIC"></asp:ListItem>
                        <asp:ListItem Text="SOLID & PNEUMATIC" Value="SOLID & PNEUMATIC"></asp:ListItem>
                        <asp:ListItem Text="POB" Value="POB"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <th>
                    FIXING HOLES DIA
                </th>
                <td>
                    <asp:DropDownList data="FHD" runat="server" ID="ddlFHD" ui-Name="FHD" ClientIDMode="Static"
                        Width="250px" CssClass="form-control">
                    </asp:DropDownList>
                    <asp:TextBox ID="txtFHD" Width="250px" runat="server" ClientIDMode="Static" CssClass="form-control"
                        MaxLength="4" onkeypress="return isNumberKey(event)"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    PILOTED
                </th>
                <td>
                    <asp:DropDownList data="PILOTED" runat="server" ID="ddlPiloted" ui-Name="PILOTED"
                        ClientIDMode="Static" Width="250px" CssClass="form-control">
                        <asp:ListItem Text="--SELECT--" Value="--SELECT--"></asp:ListItem>
                        <asp:ListItem Text="STUD" Value="STUD"></asp:ListItem>
                        <asp:ListItem Text="HUB" Value="HUB"></asp:ListItem>
                        <asp:ListItem Text="STUD & HUB" Value="STUD & HUB"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <th>
                    FIXING HOLES TYPE
                </th>
                <td>
                    <asp:DropDownList data="FHT" runat="server" ID="ddlFHT" ui-Name="FHT" ClientIDMode="Static"
                        Width="250px" CssClass="form-control">
                        <asp:ListItem Text="--SELECT--" Value="--SELECT--"></asp:ListItem>
                        <asp:ListItem Text="PLAIN" Value="PLAIN"></asp:ListItem>
                        <asp:ListItem Text="COUNTERSINK CONICAL" Value="COUNTERSINK CONICAL"></asp:ListItem>
                        <asp:ListItem Text="COUNTERSINK SPHERICAL" Value="COUNTERSINK SPHERICAL"></asp:ListItem>
                        <asp:ListItem Text="NONE" Value="NONE"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    NO. OF MOUNTING HOLES
                </th>
                <td>
                    <asp:DropDownList data="NOOFMH" runat="server" ID="ddlNoOfMH" ui-Name="NOOFMH" ClientIDMode="Static"
                        Width="250px" CssClass="form-control">
                    </asp:DropDownList>
                </td>
                <th>
                    BORE DIA
                </th>
                <td>
                    <asp:DropDownList data="BD" runat="server" ID="ddlBD" ui-Name="BD" ClientIDMode="Static"
                        Width="250px" CssClass="form-control">
                    </asp:DropDownList>
                    <asp:TextBox ID="txtBD" Width="250px" runat="server" ClientIDMode="Static" CssClass="form-control"
                        MaxLength="6" onkeypress="return isNumberKey(event)"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    MOUNTING HOLES PCD
                </th>
                <td>
                    <asp:DropDownList data="MHPCD" runat="server" ID="ddlMHPCD" ui-Name="MHPCD" ClientIDMode="Static"
                        Width="250px" CssClass="form-control">
                    </asp:DropDownList>
                    <asp:TextBox ID="txtMHPCD" Width="250px" runat="server" ClientIDMode="Static" CssClass="form-control"
                        MaxLength="6" onkeypress="return isNumberKey(event)"></asp:TextBox>
                </td>
                <th>
                    PAINTING COLOR
                </th>
                <td>
                    <asp:DropDownList data="PC" runat="server" ID="ddlPC" ui-Name="PC" ClientIDMode="Static"
                        Width="250px" CssClass="form-control">
                        <asp:ListItem Text="--SELECT--" Value="--SELECT--"></asp:ListItem>
                        <asp:ListItem Text="BLACK" Value="BLACK"></asp:ListItem>
                        <asp:ListItem Text="WHITE" Value="WHITE"></asp:ListItem>
                        <asp:ListItem Text="SILVER" Value="SILVER"></asp:ListItem>
                        <asp:ListItem Text="GRAY" Value="GRAY"></asp:ListItem>
                        <asp:ListItem Text="TRAFFIC WHITE RAL 9016" Value="TRAFFIC WHITE RAL 9016"></asp:ListItem>
                        <asp:ListItem Text="TRAFFIC WHITE RAL 9016 (PC)" Value="TRAFFIC WHITE RAL 9016 (PC)"></asp:ListItem>
                        <asp:ListItem Text="ORANGE" Value="ORANGE"></asp:ListItem>
                        <asp:ListItem Text="YELLOW" Value="YELLOW"></asp:ListItem>
                        <asp:ListItem Text="NONE" Value="NONE"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    MOUNTING HOLES DIA
                </th>
                <td>
                    <asp:DropDownList data="MHDIA" runat="server" ID="ddlMHDIA" ui-Name="MHDIA" ClientIDMode="Static"
                        Width="250px" CssClass="form-control">
                    </asp:DropDownList>
                    <asp:TextBox ID="txtMHDIA" Width="250px" runat="server" ClientIDMode="Static" CssClass="form-control"
                        MaxLength="4" onkeypress="return isNumberKey(event)"></asp:TextBox>
                </td>
                <th>
                    WALL THICKNESS
                </th>
                <td>
                    <asp:DropDownList data="WT" runat="server" ID="ddlWT" ui-Name="WT" ClientIDMode="Static"
                        Width="250px" CssClass="form-control">
                    </asp:DropDownList>
                    <asp:TextBox ID="txtWT" Width="250px" runat="server" ClientIDMode="Static" CssClass="form-control"
                        MaxLength="4" onkeypress="return isNumberKey(event)"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    MOUNTING HOLES TYPE
                </th>
                <td>
                    <div>
                        <asp:DropDownList data="mht" runat="server" ID="ddlMHT" ui-Name="RIMSIZE" Width="250px"
                            ClientIDMode="Static" CssClass="form-control">
                            <asp:ListItem Text="--SELECT--" Value="--SELECT--"></asp:ListItem>
                            <asp:ListItem Text="PLAIN" Value="PLAIN"></asp:ListItem>
                            <asp:ListItem Text="COUNTERSINK SPHERICAL" Value="COUNTERSINK SPHERICAL"></asp:ListItem>
                            <asp:ListItem Text="COUNTERSINK CONICAL" Value="COUNTERSINK CONICAL"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div id="divspherical" style="display: none; float: left; background-color: #61dc70;">
                        <span style="padding-left: 10px; font-weight: bold; padding-top: 5px; color: #fff;">
                            RADIUS</span>
                        <asp:DropDownList data="radius" runat="server" ID="ddlRadius" ui-Name="RADIUS" ClientIDMode="Static"
                            Width="250px" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div id="divconical" style="display: none; float: left; background-color: #b1ec3e;">
                        <span style="padding-left: 10px; font-weight: bold; padding-top: 5px; color: #fff;">
                            ANGLE</span>
                        <asp:DropDownList data="angle" runat="server" ID="ddlangle" ui-Name="ANGLE" ClientIDMode="Static"
                            Width="250px" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </td>
                <th>
                    EDC-NO/ PROCESS-ID
                </th>
                <td>
                    <div style="float: left; width: 100%">
                        <div style="float: left; width: 25%;">
                            <asp:TextBox runat="server" ClientIDMode="Static" ID="txtEdcNoProcessID" Width="80px"
                                MaxLength="5" CssClass="form-control" Font-Size="25px" onkeypress="return isNumberKey(event)"></asp:TextBox>
                        </div>
                        <div style="float: left; width: 8%; text-align: center; line-height: 40px;">
                            <asp:Label runat="server" ID="lblRevText" ClientIDMode="Static" Text="R" Font-Bold="true"
                                Font-Size="25px"></asp:Label>
                        </div>
                        <div style="float: left; width: 20%;">
                            <asp:TextBox runat="server" ClientIDMode="Static" ID="txtRevCount" Width="40px" MaxLength="2"
                                CssClass="form-control" Font-Size="25px" onkeypress="return isNumberKey(event)"></asp:TextBox>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <th>
                    DISC OFFSET
                </th>
                <td>
                    <asp:DropDownList data="DO" runat="server" ID="ddlDO" ui-Name="DO" ClientIDMode="Static"
                        Width="250px" CssClass="form-control">
                    </asp:DropDownList>
                    <asp:TextBox ID="txtDO" Width="250px" runat="server" ClientIDMode="Static" CssClass="form-control"
                        MaxLength="5" onkeypress="return isNumberKey(event)"></asp:TextBox>
                </td>
                <th>
                    WEIGHT
                </th>
                <td>
                    <asp:TextBox ID="txtRimWt" Width="90px" runat="server" ClientIDMode="Static" CssClass="form-control"
                        MaxLength="10" onkeypress="return isNumberKey(event)"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    DISC THICKNESS
                </th>
                <td>
                    <asp:DropDownList data="DT" runat="server" ID="ddlDT" ui-Name="DT" ClientIDMode="Static"
                        Width="250px" CssClass="form-control">
                    </asp:DropDownList>
                    <asp:TextBox ID="txtDT" Width="250px" runat="server" ClientIDMode="Static" CssClass="form-control"
                        MaxLength="4" onkeypress="return isNumberKey(event)"></asp:TextBox>
                </td>
                <th>
                    REVISION
                </th>
                <td>
                    <asp:Label runat="server" ID="lblRevision" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                    <span id="lnkSpanID" runat="server" clientidmode="Static" class="lnkCss" onclick="openrevision();">
                    </span>
                    <div id="div_revision" style="text-align: left; display: none;">
                        <asp:TextBox ID="txtRevision" Width="320px" runat="server" ClientIDMode="Static"
                            CssClass="form-control" MaxLength="80" Text=""></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2" rowspan="3" style="text-align: left;">
                    <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"
                        Font-Bold="true"></asp:Label>
                    <asp:Label runat="server" ID="lblSaveMsg" ClientIDMode="Static" Text="" ForeColor="Green"
                        Font-Bold="true"> </asp:Label>
                </td>
            </tr>
            <tr>
                <th>
                    DRAWING FILE
                </th>
                <td>
                    <div id="div_Dwg_Upload" style="text-align: left; display: none;">
                        <asp:FileUpload ID="FileUploadControl_Dwg" ClientIDMode="Static" runat="server" />
                        <asp:Button runat="server" ID="btnUpload" ClientIDMode="Static" Text="SAVE DWG FILE"
                            OnClick="btnUpload_Click" OnClientClick="javascript:return CtrlDwgValidate()"
                            CssClass="btnsave" />
                    </div>
                    <div id="div_Dwg_Clear" style="text-align: left; display: none;">
                        <asp:LinkButton runat="server" ID="lnkDwg" Text="DOWNLOAD" OnClick="lnkDwg_Click"></asp:LinkButton>
                        <%--<asp:Button runat="server" ID="btnDeleteDwg" ClientIDMode="Static" Text="DELETE"
                            OnClick="btnDelete_Click" CssClass="btn btn-danger" />--%>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: center;">
                    <asp:Button runat="server" ID="btnEDC" ClientIDMode="Static" Text="CREATE RIM PROCESS-ID"
                        Visible="true" CssClass="btnactive" OnClientClick="javascript:return CtrlEDCNORequestValidate()"
                        OnClick="btnSaveEDC_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:ScriptManager runat="server" ID="scriptManager1">
                    </asp:ScriptManager>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblNoOfRecords" Text="" runat="server" ClientIDMode="static" CssClass="recCss"></asp:Label>
                            <asp:GridView runat="server" ID="gv_RimProcessID" ClientIDMode="Static" CssClass="gridcss"
                                Width="100%" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="EDC NO" ItemStyle-Font-Bold="true" ItemStyle-Font-Size="15px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblEdcNo" ClientIDMode="Static" Text='<%# Eval("EDCNO") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
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
                                                        WEGIHT
                                                    </th>
                                                    <td>
                                                        <%# Eval("RimWt")%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        CREATED
                                                    </th>
                                                    <td colspan="5">
                                                        <%# Eval("UserName")%>
                                                        <%# Eval("CreatedDate")%>
                                                        <%# Eval("Usage")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:Button ID="btnTriggerGrid" runat="server" ClientIDMode="static" OnClick="btnTriggerGrid_Click"
                                Style="visibility: hidden;" />
                            <asp:HiddenField runat="server" ID="hdnRimEdcNo" ClientIDMode="Static" Value="" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnTriggerGrid" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#ddlRimSize').focus();
            $(':text').css("display", "none");
            $('#txtEdcNoProcessID,#txtRevCount,#txtRimWt,#txtRevision').css({ 'display': 'block' });
            $("select").change(function () {
                $('#divrimsspecify').css({ 'display': 'none' });
                if ($('#ddlRimtype').find('option:selected').text() == "MULTI PIECE")
                    $('#divrimsspecify').css({ 'display': 'block' });
                else
                    $("#ddlNoofpiece").val($("#ddlNoofpiece option:first").val());

                $('#divspherical,#divconical').css({ 'display': 'none' });
                if ($('#ddlMHT').find('option:selected').text() == "COUNTERSINK SPHERICAL") {
                    $('#divspherical').css({ 'display': 'block' });
                    $("#ddlangle").val($("#ddlangle option:first").val());
                }
                else if ($('#ddlMHT').find('option:selected').text() == "COUNTERSINK CONICAL") {
                    $('#divconical').css({ 'display': 'block' });
                    $("#ddlRadius").val($("#ddlRadius option:first").val());
                }
                else {
                    $("#ddlRadius").val($("#ddlRadius option:first").val());
                    $("#ddlangle").val($("#ddlangle option:first").val());
                }

                if ($("#" + this.id + " option:selected").text() == "ADD NEW " + $(this).attr("ui-Name").toString().toUpperCase())
                    $("#txt" + $(this).attr("data")).css("display", "block").focus();
                else
                    $("#txt" + $(this).attr("data")).val("").css("display", "none");
                $("#btnTriggerGrid").trigger("click");
            });
        });

        inputs = $('select').keypress(function (e) {
            if ((e.keycode == 13 || e.which == 13) && $("#" + this.id + " option:selected").text() != "--SELECT--") {
                if ($("#" + this.id + " option:selected").text() == "ADD NEW " + $(this).attr("ui-Name").toString().toUpperCase())
                    $("#txt" + $(this).attr("data")).css("display", "block").focus();
                else {
                    e.preventDefault();
                    var nextInput = inputs.get(inputs.index(this) + 1);
                    if (nextInput)
                        nextInput.focus();
                }
                $("#btnTriggerGrid").trigger("click");
            }
        });

        $(':text').keydown(function (e) {
            if ((e.keycode == 13 || e.which == 13) && $('#' + this.id).val() != '') {
                if (this.id == 'txtRIMSIZE') $('#ddlRimtype').focus();
                else if (this.id == 'txtMHPCD') $('#ddlMHDIA').focus();
                else if (this.id == 'txtMHDIA') $('#ddlMHT').focus();
                else if (this.id == 'txtDO') $('#ddlDT').focus();
                else if (this.id == 'txtDT') $('#NOFH').focus();
                else if (this.id == 'txtFHPCD') $('#ddlFHD').focus();
                else if (this.id == 'txtFHD') $('#ddlFHT').focus();
                else if (this.id == 'txtBD') $('#ddlPC').focus();
                else if (this.id == 'txtWT') $('#txtEdcNoProcessID').focus();
                else if (this.id == 'txtEdcNoProcessID') $('txtRevCount').focus();
                else if (this.id == 'txtRevCount') $('#txtRimWt').focus();
                else if (this.id == 'txtRimWt') $('#btnEDC').focus();
            }
        }).blur(function (e) { $('#' + this.id).val($('#' + this.id).val().toUpperCase()); });

        function CtrlEDCNORequestValidate() {
            var twodecPattern = /^[0-9][0-9]\.[0-9]{1}$/;
            var threedecPattern = /^[0-9][0-9][0-9]\.[0-9]{1}$/;
            var threedecTwoPattern = /^[0-9][0-9][0-9]\.[0-9][0-9]{1}$/;
            var numericPattern = /^[0-9]*$/;
            var errmsg = ''; $('#lblErrMsg').html('');
            if ($('#ddlRimSize option:selected').text() == '--SELECT--')
                errmsg += 'Choose rim size<br/>';
            else if ($('#ddlRimSize option:selected').text() == 'ADD NEW RIMSIZE' && $('#txtRIMSIZE').val().length == 0)
                errmsg += 'Enter rim size<br/>';
            if ($('#ddlRimtype option:selected').text() == '--SELECT--')
                errmsg += 'Choose rim type<br/>';
            else if ($('#ddlRimtype option:selected').text() == 'MULTI PIECE' && $('#ddlNoofpiece option:selected').text() == 'Choose')
                errmsg += 'Choose no of piece<br/>';
            if ($('#ddlTyreType option:selected').text() == '--SELECT--')
                errmsg += 'Choose tyre category<br/>';
            if ($('#ddlPiloted option:selected').text() == '--SELECT--')
                errmsg += 'Choose Piloted<br/>';
            if ($('#ddlNoOfMH option:selected').text() == '--SELECT--')
                errmsg += 'Choose no of mounting holes<br/>';
            if ($('#ddlMHPCD option:selected').text() == '--SELECT--')
                errmsg += 'Choose mounting holes PCD<br/>';
            else if ($('#ddlMHPCD option:selected').text() == 'ADD NEW MHPCD' && $('#txtMHPCD').val().length == 0)
                errmsg += 'Enter mounting holes PCD<br/>';
            else if ($('#ddlMHPCD option:selected').text() == 'ADD NEW MHPCD' && $('#txtMHPCD').val().length > 0) {
                if (threedecTwoPattern.test($('#txtMHPCD').val()) == false)
                    errmsg += 'Enter valid mounting holes PCD (000.00)<br/>';
            }
            if ($('#ddlMHDIA option:selected').text() == '--SELECT--')
                errmsg += 'Choose mounting holes DIA<br/>';
            else if ($('#ddlMHDIA option:selected').text() == 'ADD NEW MHDIA' && $('#txtMHDIA').val().length == 0)
                errmsg += 'Enter mounting holes DIA<br/>';
            else if ($('#ddlMHDIA option:selected').text() == 'ADD NEW MHDIA' && $('#txtMHDIA').val().length > 0) {
                if (twodecPattern.test($('#txtMHDIA').val()) == false)
                    errmsg += 'Enter valid mounting holes DIA (00.0)<br/>';
            }
            if ($('#ddlMHT option:selected').text() == '--SELECT--')
                errmsg += 'Choose mounting holes type<br/>';
            else if ($('#ddlMHT option:selected').text() == 'COUNTERSINK SPHERICAL' && $('#ddlRadius option:selected').text() == 'Choose')
                errmsg += 'Choose radius<br/>';
            else if ($('#ddlMHT option:selected').text() == 'COUNTERSINK CONICAL' && $('#ddlangle option:selected').text() == 'Choose')
                errmsg += 'Choose angle<br/>';
            if ($('#ddlDO option:selected').text() == '--SELECT--')
                errmsg += 'Choose disc offset<br/>';
            else if ($('#ddlDO option:selected').text() == 'ADD NEW DO' && $('#txtDO').val().length == 0)
                errmsg += 'Enter disc offset<br/>';
            else if ($('#ddlDO option:selected').text() == 'ADD NEW DO' && $('#txtDO').val().length > 0) {
                if (threedecPattern.test($('#txtDO').val()) == false)
                    errmsg += 'Enter valid disc offset (000.0)<br/>';
            }
            if ($('#ddlDT option:selected').text() == '--SELECT--')
                errmsg += 'Choose disc thickness<br/>';
            else if ($('#ddlDT option:selected').text() == 'ADD NEW DT' && $('#txtDT').val().length == 0)
                errmsg += 'Enter disc thickness<br/>';
            else if ($('#ddlDT option:selected').text() == 'ADD NEW DT' && $('#txtDT').val().length > 0) {
                if (twodecPattern.test($('#txtDT').val()) == false)
                    errmsg += 'Enter valid disc thickness (00.0)<br/>';
            }
            if ($('#ddlNOFH option:selected').text() == '--SELECT--')
                errmsg += 'Choose No of Fixing holes<br/>';
            if ($('#ddlFPcd option:selected').text() == '--SELECT--')
                errmsg += 'Choose Fixing holes PCD<br/>';
            else if ($('#ddlFPcd option:selected').text() == 'ADD NEW FHPCD' && $('#txtFHPCD').val().length == 0)
                errmsg += 'Enter Fixing holes PCD<br/>';
            else if ($('#ddlFPcd option:selected').text() == 'ADD NEW FHPCD' && $('#txtFHPCD').val().length > 0) {
                if (threedecPattern.test($('#txtFHPCD').val()) == false)
                    errmsg += 'Enter valid Fixing holes PCD (000.0)<br/>';
            }
            if ($('#ddlFHD option:selected').text() == '--SELECT--')
                errmsg += 'Choose Fixing holes DIA<br/>';
            else if ($('#ddlFHD option:selected').text() == 'ADD NEW FHD' && $('#txtFHD').val().length == 0)
                errmsg += 'Enter Fixing holes DIA<br/>';
            else if ($('#ddlFHD option:selected').text() == 'ADD NEW FHD' && $('#txtFHD').val().length > 0) {
                if (twodecPattern.test($('#txtFHD').val()) == false)
                    errmsg += 'Enter valid Fixing holes DIA (00.0)<br/>';
            }
            if ($('#ddlFHT option:selected').text() == '--SELECT--')
                errmsg += 'Choose Fixing holes Type<br/>';
            if ($('#ddlBD option:selected').text() == '--SELECT--')
                errmsg += 'Choose Bore DIA<br/>';
            else if ($('#ddlBD option:selected').text() == 'ADD NEW BD' && $('#txtBD').val().length == 0)
                errmsg += 'Enter Bore DIA<br/>';
            else if ($('#ddlBD option:selected').text() == 'ADD NEW BD' && $('#txtBD').val().length > 0) {
                if (threedecTwoPattern.test($('#txtBD').val()) == false)
                    errmsg += 'Enter valid bore DIA (000.00)<br/>';
            }
            if ($('#ddlPC option:selected').text() == '--SELECT--')
                errmsg += 'Choose painting color<br/>';
            if ($('#ddlWT option:selected').text() == '--SELECT--')
                errmsg += 'Choose wall thickness<br/>';
            else if ($('#ddlWT option:selected').text() == 'ADD NEW WT' && $('#txtWT').val().length == 0)
                errmsg += 'Enter wall thickness<br/>';
            else if ($('#ddlWT option:selected').text() == 'ADD NEW WT' && $('#txtWT').val().length > 0) {
                if (twodecPattern.test($('#txtWT').val()) == false)
                    errmsg += 'Enter valid wall thickness (00.0)<br/>';
            }
            if ($('#txtEdcNoProcessID').val().length == 0)
                errmsg += 'Enter EDC NO<br/>';
            else if ($('#txtEdcNoProcessID').val().length < 5)
                errmsg += 'Enter EDC NO in five digit<br/>';
            else if ($('#txtEdcNoProcessID').val().length > 0) {
                if (numericPattern.test($('#txtEdcNoProcessID').val()) == false)
                    errmsg += 'Enter only numeric in EDC-NO box (00000) <br/>';
            }
            if ($('#txtRevCount').val().length == 0)
                errmsg += 'Enter EDC Revision Count<br/>';
            else if ($('#txtRevCount').val().length < 2)
                errmsg += 'Enter EDC Revision Count in two digit<br/>';
            else if ($('#txtRevCount').val().length > 0) {
                if (numericPattern.test($('#txtRevCount').val()) == false)
                    errmsg += 'Enter only numeric in REV-NO box (00) <br/>';
            }
            if ($('#txtRimWt').val().length == 0)
                errmsg += 'Enter Weight<br/>';
            if ($('#lblRevision').html() == "REASON FOR REVISION") {
                if ($('#txtRevision').val().length == 0)
                    errmsg += 'Enter revision of reason';
            }

            if (errmsg.length > 0) {
                $('#lblErrMsg').html(errmsg);
                return false;
            }
            else
                return true;
        }

        function CtrlDwgValidate() {
            if ($('#hdnRimEdcNo').val().length == 0) {
                $('#lblErrMsg').html('Enter EDC NO<br/>');
                return false;
            }
            else
                return true;
        }

        function CtrlEnable() {
            $('#txtEdcNoProcessID,#txtRevCount,#txtRevision').val("");
            $('#lblRevision').html("NEW SPEC");
            $('#div_revision').css({ 'display': 'none' });
            $('#lnkSpanID').html("");
            $('#div_Dwg_Upload').css({ 'display': 'block' });
            $('#div_Dwg_Clear').css({ 'display': 'none' });
            $('#txtEdcNoProcessID').attr({ 'disabled': false });
            $('#txtRevCount').attr({ 'disabled': false });
            $('#btnEDC').css({ 'display': 'block' });
            if ($('#btnEDC').css('display') == 'block')
                $('#btnUpload').css({ 'display': 'none' });
        }
        function CtrlDisable() {
            $('#div_Dwg_Upload').css({ 'display': 'none' });
            $('#div_Dwg_Clear').css({ 'display': 'none' });
            $('#txtEdcNoProcessID').attr({ 'disabled': true });
            $('#txtRevCount').attr({ 'disabled': true });
            $('#btnEDC').css({ 'display': 'none' });
            if ($('#btnEDC').css('display') == 'none')
                $('#btnUpload').css({ 'display': 'block' });
        }
        function CtrlDwgUpload() {
            $('#div_Dwg_Upload').css({ 'display': 'block' });
            $('#div_Dwg_Clear').css({ 'display': 'none' });
        }
        function CtrlDwgView() {
            $('#div_Dwg_Upload').css({ 'display': 'none' });
            $('#div_Dwg_Clear').css({ 'display': 'block' });
        }
        function edc_change() {
            $("#lblRevision").html('');
            $('#lnkSpanID').html("CHANGE THE EDC REVISION NO. CLICK HERE");
            $('#div_revision').css({ 'display': 'none' });
        }
        function openrevision() {
            $('#div_revision').css({ 'display': 'block' });
            $("#lblRevision").html('REASON FOR REVISION');
            $('#btnEDC').css({ 'display': 'block' });
            $('#lnkSpanID').html("");
            $('#txtEdcNoProcessID,#txtRevCount').attr({ 'disabled': false });
            if ($('#hdnRimEdcNo').val().length > 0) {
                var strRimEdcNo = $('#hdnRimEdcNo').val();
                $('#txtEdcNoProcessID').val(strRimEdcNo.substring(0, 5));
                $('#txtRevCount').val(strRimEdcNo.substring(6, 8));
            }
        }
    </script>
</asp:Content>
