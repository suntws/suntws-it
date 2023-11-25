<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="TyreDrawingRequest.aspx.cs" Inherits="TTS.TyreDrawingRequest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/TinyStyle.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/TinyboxScript.js" type="text/javascript"></script>
    <style type="text/css">
        .tableReq
        {
            border-collapse: collapse;
            border-color: #000;
            width: 1080px;
            line-height: 20px;
            margin-top: 5px;
        }
        .tableReq th:first-child
        {
            background-color: #E2F2FF;
            text-align: left;
            padding-left: 10px;
            width: 240px;
            font-weight: bold;
        }
        .tableReq input[type="text"], textarea, select, file
        {
            background-color: #F7E9C3;
            border: 1px solid #000;
            margin-left: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        DESIGN INPUT FORM</div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent">
        <table cellspacing="0" rules="all" border="1">
            <tr>
                <td>
                    <table cellspacing="0" rules="all" border="1" class="tableReq">
                        <tr>
                            <td colspan="2" style="text-align: center; font-size: 15px; font-weight: bold; background-color: #91E9F7;">
                                WHEEL SIZE / TYRE SIZE
                            </td>
                        </tr>
                        <tr>
                            <th>
                                TYRE SIZE
                            </th>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlTyreSize" ClientIDMode="Static" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlTyreSize_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                RIM WIDTH
                            </th>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlRimWidth" ClientIDMode="Static">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                TYRE TYPE
                            </th>
                            <td>
                                <asp:RadioButtonList ID="rdbTyreType" runat="server" ClientIDMode="Static" RepeatColumns="2"
                                    RepeatDirection="Horizontal">
                                    <asp:ListItem Text="SOLID" Value="SOLID"></asp:ListItem>
                                    <asp:ListItem Text="PNEUMATIC" Value="PNEUMATIC"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                WHEEL LOCATION ON THE VEHICLE
                            </th>
                            <td>
                                <asp:RadioButtonList ID="rdbWheelLocation" runat="server" ClientIDMode="Static" RepeatColumns="3"
                                    RepeatDirection="Horizontal">
                                    <asp:ListItem Text="FRONT" Value="FRONT"></asp:ListItem>
                                    <asp:ListItem Text="REAR" Value="REAR"></asp:ListItem>
                                    <asp:ListItem Text="FRONT & REAR" Value="FRONT & REAR"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                NO. OF WHEELS PER AXLE END
                            </th>
                            <td>
                                <asp:RadioButtonList ID="rdbWheelAxleEnd" runat="server" ClientIDMode="Static" RepeatColumns="2"
                                    RepeatDirection="Horizontal">
                                    <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                WHEEL APPLICATION
                            </th>
                            <td>
                                <asp:RadioButtonList ID="rdbWheelApp" runat="server" ClientIDMode="Static" RepeatColumns="3"
                                    RepeatDirection="Horizontal">
                                    <asp:ListItem Text="DRIVE" Value="DRIVE"></asp:ListItem>
                                    <asp:ListItem Text="STEER" Value="STEER"></asp:ListItem>
                                    <asp:ListItem Text="TRAIL" Value="TRAIL"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                RIMS TYPE
                            </th>
                            <td>
                                <div style="width: 625px; float: left;">
                                    <asp:RadioButtonList ID="rdbRims" runat="server" ClientIDMode="Static" RepeatColumns="5"
                                        RepeatDirection="Horizontal" Width="600px">
                                    </asp:RadioButtonList>
                                </div>
                                <div id="divrimsspecify" runat="server" clientidmode="Static" style="display: none;
                                    width: 180px; float: left; line-height: 27px; background-color: #5F3212; color: #fff;">
                                    <span style="padding-left: 10px;">NO OF PIECE</span>
                                    <asp:DropDownList runat="server" ID="ddlNoofpiece" ClientIDMode="Static">
                                    </asp:DropDownList>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                VEHICLE SPEED (kmph)
                            </th>
                            <td>
                                <asp:TextBox ID="txtVehicleSpeed" runat="server" Text="" ClientIDMode="Static" Width="150px"
                                    MaxLength="10" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: center; font-size: 15px; font-weight: bold; background-color: #91F7A2;">
                                WHEEL STUD DETAILS (FUNCTIONAL DETAILS)
                            </td>
                        </tr>
                        <tr>
                            <th>
                                PILOTED
                            </th>
                            <td>
                                <asp:RadioButtonList ID="rbdPiloted" runat="server" ClientIDMode="Static" RepeatColumns="3"
                                    RepeatDirection="Horizontal">
                                    <asp:ListItem Text="STUD" Value="STUD"></asp:ListItem>
                                    <asp:ListItem Text="HUB" Value="HUB"></asp:ListItem>
                                    <asp:ListItem Text="STUD & HUB" Value="STUD & HUB"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                NO OF STUD HOLES
                            </th>
                            <td>
                                <asp:RadioButtonList ID="rdbNoOfHole" runat="server" ClientIDMode="Static" RepeatColumns="8"
                                    RepeatDirection="Horizontal">
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <th style="line-height: 15px;">
                                STUD HOLES PCD<br />
                                (PITCH CIRCLE DIAMETER) (mm)
                            </th>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlPCD" ClientIDMode="Static" Width="150px">
                                </asp:DropDownList>
                                <asp:TextBox ID="txtPCD" runat="server" Text="" ClientIDMode="Static" Width="150px"
                                    MaxLength="10" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                STUD HOLES DIA (mm)
                            </th>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlHoleDia" ClientIDMode="Static" Width="150px">
                                </asp:DropDownList>
                                <asp:TextBox ID="txtHoleDia" runat="server" Text="" ClientIDMode="Static" Width="150px"
                                    MaxLength="10" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                STUD HOLES TYPE
                            </th>
                            <td>
                                <div style="width: 200px; float: left;">
                                    <asp:RadioButtonList ID="rdbHolesType" runat="server" ClientIDMode="Static" RepeatColumns="2"
                                        RepeatDirection="Horizontal">
                                        <asp:ListItem Text="PLAIN" Value="PLAIN"></asp:ListItem>
                                        <asp:ListItem Text="COUNTERSINK" Value="COUNTERSINK"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                                <div id="divcountersinkangle" runat="server" clientidmode="Static" style="display: none;
                                    width: 200px; float: left; background-color: #ccc;">
                                    <asp:RadioButtonList ID="rdbCountersink" runat="server" ClientIDMode="Static" RepeatColumns="2"
                                        RepeatDirection="Horizontal">
                                        <asp:ListItem Text="SPHERICAL" Value="SPHERICAL"></asp:ListItem>
                                        <asp:ListItem Text="CONICAL" Value="CONICAL"></asp:ListItem>
                                    </asp:RadioButtonList>
                                    <div id="divspherical" runat="server" clientidmode="Static" style="display: none;">
                                        <span style="padding-left: 10px;">RADIUS</span>
                                        <asp:DropDownList runat="server" ID="ddlRadius" ClientIDMode="Static">
                                        </asp:DropDownList>
                                    </div>
                                    <div id="divconical" runat="server" clientidmode="Static" style="display: none;">
                                        <span style="padding-left: 10px;">ANGLE</span>
                                        <asp:DropDownList runat="server" ID="ddlangle" ClientIDMode="Static">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                BORE DIA (mm)
                            </th>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlBOREDIA" ClientIDMode="Static" Width="150px">
                                </asp:DropDownList>
                                <asp:TextBox runat="server" ID="txtBoreDia" ClientIDMode="Static" Text="" Width="200px"
                                    MaxLength="10" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th style="line-height: 15px;">
                                ANY SPECIAL KEY / SLOT REQUIRED NEAR THE BORE
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtSpecialKey" ClientIDMode="Static" Text="" Width="200px"
                                    MaxLength="50"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                WHEEL STUD LENGTH (mm)
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtWheelStudLen" ClientIDMode="Static" Text="" Width="200px"
                                    MaxLength="10" onkeypress="return isNumberAndMinusKey(event)"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: center; font-size: 15px; font-weight: bold; background-color: #DBC4F3;">
                                WHEEL OTHER DETAILS
                            </td>
                        </tr>
                        <tr>
                            <th>
                                WHEEL WEIGHT (kg)
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtWheelWeight" ClientIDMode="Static" Text="" Width="200px"
                                    onkeypress="return isNumberAndMinusKey(event)" MaxLength="10"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th style="line-height: 15px;">
                                WHEEL PAINTING / PACKAGING REQUIREMENTS
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtDwgRemarks" ClientIDMode="Static" Text="" TextMode="MultiLine"
                                    Width="800px" Height="55px" onKeyUp="javascript:CheckMaxLength(this, 999);" onChange="javascript:CheckMaxLength(this, 999);"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th style="line-height: 15px;">
                                INFORMATION DERIVED FROM PAST
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtInfoDerived" ClientIDMode="Static" Text="" TextMode="MultiLine"
                                    Width="800px" Height="45px" onKeyUp="javascript:CheckMaxLength(this, 999);" onChange="javascript:CheckMaxLength(this, 999);"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th style="line-height: 15px;">
                                STATUTORY INFO
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtStatutory" ClientIDMode="Static" Text="" TextMode="MultiLine"
                                    Width="800px" Height="45px" onKeyUp="javascript:CheckMaxLength(this, 999);" onChange="javascript:CheckMaxLength(this, 999);"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                CUSTOMER
                            </th>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlCustomerSpecific" ClientIDMode="Static" Width="">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                CUSTOMER DRAWING FILE
                            </th>
                            <td>
                                <div style="margin-left: 10px; float: left; width: 500px;">
                                    <asp:FileUpload ID="fupTDS" runat="server" Width="500px" /></div>
                                <div id="divpdf" runat="server" style="float: left; display: none;">
                                    <a onclick="javascript:return popup();" style="cursor: pointer; text-decoration: underline;
                                        color: #082DEA;">VIEW</a></div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: center;">
                                <div id="divErrMsg" style="color: #f00; line-height: 15px; padding-left: 250px; text-align: left;">
                                </div>
                                <asp:Button runat="server" ID="btnDwgRequestSave" ClientIDMode="Static" Text="" CssClass="btnactive"
                                    OnClientClick="javascript:return CtrlDwgRequestValidate()" OnClick="btnDwgRequestSave_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="line-height: 12px;">
                    <asp:GridView runat="server" ID="gvDwgRequest" AutoGenerateColumns="false" Width="1078px"
                        AlternatingRowStyle-BackColor="#f5f5f5" AllowPaging="true" PagerStyle-Font-Bold="true"
                        PagerStyle-Font-Size="15px" PagerStyle-HorizontalAlign="Center" PagerStyle-VerticalAlign="Middle">
                        <HeaderStyle Font-Bold="true" BackColor="#fefe8b" />
                        <Columns>
                            <asp:TemplateField HeaderText="TYRE SIZE">
                                <ItemTemplate>
                                    <asp:Label ID="lblTyreSize" runat="server" Text='<%# Eval("TyreSize") %>'></asp:Label>
                                    <asp:HiddenField ID="hdnSlno" runat="server" Value='<%# Eval("Slno") %>' />
                                    <asp:HiddenField ID="hdnNoofPiece" runat="server" Value='<%# Eval("NoofPiece") %>' />
                                    <asp:HiddenField ID="hdnCountersink" runat="server" Value='<%# Eval("Countersink") %>' />
                                    <asp:HiddenField ID="hdnradius_angle" runat="server" Value='<%# Eval("radius_angle") %>' />
                                    <asp:HiddenField ID="hdnDwgRemarks" runat="server" Value='<%# Eval("DwgRemarks") %>' />
                                    <asp:HiddenField ID="hdnInfoDerived" runat="server" Value='<%# Eval("InfoDerivedFromPast") %>' />
                                    <asp:HiddenField ID="hdnStatutory" runat="server" Value='<%# Eval("StatutoryInfo") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="RIM WIDTH" ItemStyle-Width="40px">
                                <ItemTemplate>
                                    <asp:Label ID="lblRimWidth" runat="server" Text='<%# Eval("RimWidth") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TYRE TYPE">
                                <ItemTemplate>
                                    <asp:Label ID="lblTyreType" runat="server" Text='<%# Eval("TyreType") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="WHEEL LOCATION" HeaderStyle-CssClass="headerNone"
                                ItemStyle-CssClass="headerNone">
                                <ItemTemplate>
                                    <asp:Label ID="lblLocation" runat="server" Text='<%# Eval("Location") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="NO. OF WHEELS PER AXLE END" HeaderStyle-CssClass="headerNone"
                                ItemStyle-CssClass="headerNone">
                                <ItemTemplate>
                                    <asp:Label ID="lblAxelEnd" runat="server" Text='<%# Eval("AxelEnd") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="APPLICATION" HeaderStyle-CssClass="headerNone" ItemStyle-CssClass="headerNone">
                                <ItemTemplate>
                                    <asp:Label ID="lblWheelApp" runat="server" Text='<%# Eval("WheelApp") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="RIM TYPE">
                                <ItemTemplate>
                                    <asp:Label ID="lblRimType" runat="server" Text='<%# Eval("RimType") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="VEHICLE SPEED" HeaderStyle-CssClass="headerNone" ItemStyle-CssClass="headerNone">
                                <ItemTemplate>
                                    <asp:Label ID="lblVehicleSpeed" runat="server" Text='<%# Eval("VehicleSpeed") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PILOTED">
                                <ItemTemplate>
                                    <asp:Label ID="lblPiloted" runat="server" Text='<%# Eval("Piloted") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="MOUNTING HOLES" ItemStyle-Width="60px">
                                <ItemTemplate>
                                    <asp:Label ID="lblMountHoles" runat="server" Text='<%# Eval("MountHoles") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PCD">
                                <ItemTemplate>
                                    <asp:Label ID="lblPCD" runat="server" Text='<%# Eval("PCD") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="HOLES DIA">
                                <ItemTemplate>
                                    <asp:Label ID="lblMountHolesDia" runat="server" Text='<%# Eval("MountHolesDia") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="HOLES TYPE">
                                <ItemTemplate>
                                    <asp:Label ID="lblHolesType" runat="server" Text='<%# Eval("HolesType") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="BORE DIA">
                                <ItemTemplate>
                                    <asp:Label ID="lblBoreDia" runat="server" Text='<%# Eval("BoreDia") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SPECIAL KEY" HeaderStyle-CssClass="headerNone" ItemStyle-CssClass="headerNone">
                                <ItemTemplate>
                                    <asp:Label ID="lblSpecialKey" runat="server" Text='<%# Eval("SpecialKey") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="STUD LENGTH" HeaderStyle-CssClass="headerNone" ItemStyle-CssClass="headerNone">
                                <ItemTemplate>
                                    <asp:Label ID="lblWheelStudLen" runat="server" Text='<%# Eval("WheelStudLen") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="WHEEL WT" HeaderStyle-CssClass="headerNone" ItemStyle-CssClass="headerNone">
                                <ItemTemplate>
                                    <asp:Label ID="lblWheelWeight" runat="server" Text='<%# Eval("WheelWeight") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CUSTOMER">
                                <ItemTemplate>
                                    <asp:Label ID="lblCustomerName" runat="server" Text='<%# Eval("CustomerName") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ACTION" ItemStyle-Width="40px">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEdit" runat="server" Text="EDIT" OnClick="lnkEdit_Click" Font-Size="10px"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hdneditslno" runat="server" ClientIDMode="Static" Value="" />
        <asp:HiddenField ID="hdnurl" runat="server" ClientIDMode="Static" Value="" />
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#txtPCD').css({ 'display': 'none' });
            $('#txtBoreDia').css({ 'display': 'none' });
            $('#txtHoleDia').css({ 'display': 'none' });
            $(':text').bind('keydown', function (e) { if (e.keyCode == 13) { e.preventDefault(); return false; } });
            $('input:radio[id*=rdbRims_]').click(function () { $('#divrimsspecify').css({ 'display': 'none' }); if (this.id == 'rdbRims_2') $('#divrimsspecify').css({ 'display': 'block' }); });
            $('input:radio[id*=rdbHolesType_]').click(function () { $('#divcountersinkangle,#divspherical,#divconical').css({ 'display': 'none' }); $('#rdbCountersink_1,#rdbCountersink_2').removeAttr('checked'); if (this.id == 'rdbHolesType_1') $('#divcountersinkangle').css({ 'display': 'block' }); });
            $('input:radio[id*=rdbCountersink_]').click(function () {
                $('#divspherical,#divconical').css({ 'display': 'none' });
                if (this.id == 'rdbCountersink_0') $('#divspherical').css({ 'display': 'block' });
                else if (this.id == 'rdbCountersink_1') $('#divconical').css({ 'display': 'block' });
            });
            $('#ddlPCD').change(function () {
                if ($('#ddlPCD option:selected').text() == 'ADD NEW ENTRY')
                    $('#txtPCD').css({ 'display': 'block' });
                else
                    $('#txtPCD').css({ 'display': 'none' });
            });
            $('#ddlBOREDIA').change(function () {
                if ($('#ddlBOREDIA option:selected').text() == 'ADD NEW ENTRY')
                    $('#txtBoreDia').css({ 'display': 'block' });
                else
                    $('#txtBoreDia').css({ 'display': 'none' });
            });
            $('#ddlHoleDia').change(function () {
                if ($('#ddlHoleDia option:selected').text() == 'ADD NEW ENTRY')
                    $('#txtHoleDia').css({ 'display': 'block' });
                else
                    $('#txtHoleDia').css({ 'display': 'none' });
            });
        });
        function popup() {
            TINY.box.show({ iframe: 'TyreDwgPopup.aspx?dwgid=&Reqid=' + $('#hdnurl').val() + '&Status=', boxid: 'frameless', width: 1000, height: 620, fixed: false, maskid: 'bluemask', maskopacity: 40, closejs: function () { } })
            return false;
        }
        function CtrlDwgRequestValidate() {
            var errmsg = ''; $('#divErrMsg').html('');
            if ($('#ddlTyreSize option:selected').text() == 'Choose')
                errmsg += 'Choose tyre size<br/>';
            if ($('input:radio[id*=rdbTyreType_]:checked').length == 0)
                errmsg += 'Choose tyre type<br/>';
            if ($('input:radio[id*=rdbRims_]:checked').length == 0)
                errmsg += 'Choose rims type<br/>';
            else if ($('#rdbRims_2').attr('checked') == 'checked' && $('#ddlNoofpiece option:selected').text() == 'Choose')
                errmsg += 'Choose no of piece<br/>';
            if ($('input:radio[id*=rbdPiloted_]:checked').length == 0)
                errmsg += 'Choose piloted<br/>';
            if ($('input:radio[id*=rdbNoOfHole_]:checked').length == 0)
                errmsg += ' No. of mounting holes<br/>';
            if ($('#ddlPCD option:selected').text() == 'Choose') errmsg += 'Choose pcd<br/>';
            else if ($('#ddlPCD option:selected').text() == 'ADD NEW ENTRY' && $('#txtPCD').val().length == 0) errmsg += 'Enter pcd<br/>';
            if ($('#ddlHoleDia option:selected').text() == 'Choose') errmsg += 'Choose hole dia<br/>';
            else if ($('#ddlHoleDia option:selected').text() == 'ADD NEW ENTRY' && $('#txtHoleDia').val().length == 0) errmsg += 'Enter hole dia<br/>';
            if ($('input:radio[id*=rdbHolesType_]:checked').length == 0)
                errmsg += 'Choose mounting holes type<br/>';
            else if ($('#rdbHolesType_1').attr('checked') == 'checked') {
                if ($('input:radio[id*=rdbCountersink_]:checked').length == 0)
                    errmsg += 'Choose spherical/conical<br/>';
                else if ($('#rdbCountersink_0').attr('checked') == 'checked' && $('#ddlRadius option:selected').text() == 'Choose')
                    errmsg += 'Choose radius<br/>';
                else if ($('#rdbCountersink_1').attr('checked') == 'checked' && $('#ddlangle option:selected').text() == 'Choose')
                    errmsg += 'Choose angle<br/>';
            }
            if ($('#ddlBOREDIA option:selected').text() == 'Choose') errmsg += 'Choose bore dia<br/>';
            else if ($('#ddlBOREDIA option:selected').text() == 'ADD NEW ENTRY' && $('#txtBoreDia').val().length == 0) errmsg += 'Enter bore dia<br/>';
            //            if ($('#ddlCustomerSpecific option:selected').text() == 'Choose')
            //                errmsg += 'Enter customer specific<br/>';
            var xyz = document.getElementById('<%= fupTDS.ClientID %>');
            if (xyz.value != '') {
                var array = ['pdf']; var Extension = xyz.value.substring(xyz.value.lastIndexOf('.') + 1).toLowerCase();
                if (array.indexOf(Extension) <= -1)
                    errmsg += 'Please Upload only pdf extension file<br/>';
            }
            if (errmsg.length > 0) {
                $('#divErrMsg').html(errmsg);
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
