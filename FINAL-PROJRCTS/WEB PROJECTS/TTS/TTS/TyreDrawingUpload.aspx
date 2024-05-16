<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="TyreDrawingUpload.aspx.cs" Inherits="TTS.TyreDrawingUpload" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/TinyStyle.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/TinyboxScript.js" type="text/javascript"></script>
    <style type="text/css">
        .divNone
        {
            display: none;
        }
        #tbDwgUpload th
        {
            background-color: #FCD7D7;
            text-align: left;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        <asp:Label runat="server" ID="lblhead" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px"></asp:Label>
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <div>
            <asp:GridView runat="server" ID="gvDwgRequest" AutoGenerateColumns="false" Width="1080px"
                AlternatingRowStyle-BackColor="#f5f5f5" RowStyle-Height="20px" CssClass="Click">
                <HeaderStyle Font-Bold="true" BackColor="#fefe8b" Height="20px" />
                <Columns>
                    <asp:TemplateField HeaderText="TYRE SIZE">
                        <ItemTemplate>
                            <asp:Label ID="lblTyreSize" runat="server" Text='<%# Eval("TyreSize") %>'></asp:Label>
                            <asp:HiddenField ID="hdnSlno" runat="server" Value='<%# Eval("Slno") %>' />
                            <asp:HiddenField ID="hdnDwgRemarks" runat="server" Value='<%# Eval("DwgRemarks") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="RIM WIDTH" ItemStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lblRimWidth" runat="server" Text='<%# Eval("RimWidth") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="TyreType" HeaderText="TYRE TYPE" />
                    <asp:BoundField DataField="Location" HeaderText="WHEEL LOCATION" HeaderStyle-CssClass="headerNone"
                        ItemStyle-CssClass="headerNone" />
                    <asp:BoundField DataField="AxelEnd" HeaderText="NO. OF WHEELS PER AXLE END" HeaderStyle-CssClass="headerNone"
                        ItemStyle-CssClass="headerNone" />
                    <asp:BoundField DataField="WheelApp" HeaderText="APPLICATION" HeaderStyle-CssClass="headerNone"
                        ItemStyle-CssClass="headerNone" />
                    <asp:TemplateField HeaderText="RIM TYPE">
                        <ItemTemplate>
                            <asp:HiddenField ID="hdnRimType" runat="server" Value='<%# Eval("RimType") %>' />
                            <%#Eval("RimType").ToString() == "MULTI PIECE" ? (Eval("RimType").ToString() + "-" + Eval("NoofPiece").ToString()) : Eval("RimType").ToString()%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="VehicleSpeed" HeaderText="VEHICLE SPEED" HeaderStyle-CssClass="headerNone"
                        ItemStyle-CssClass="headerNone" />
                    <asp:BoundField DataField="Piloted" HeaderText="PILOTED" />
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
                            <%#Eval("HolesType").ToString() == "COUNTERSINK" ? (Eval("HolesType").ToString() + "-" + Eval("Countersink").ToString() + "-" + Eval("radius_angle").ToString()) : Eval("HolesType").ToString()%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="BORE DIA">
                        <ItemTemplate>
                            <asp:Label ID="lblBoreDia" runat="server" Text='<%# Eval("BoreDia") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="SpecialKey" HeaderText="SPECIAL KEY" HeaderStyle-CssClass="headerNone"
                        ItemStyle-CssClass="headerNone" />
                    <asp:BoundField DataField="WheelStudLen" HeaderText="STUD LENGTH" HeaderStyle-CssClass="headerNone"
                        ItemStyle-CssClass="headerNone" />
                    <asp:BoundField DataField="WheelWeight" HeaderText="WHEEL WT" HeaderStyle-CssClass="headerNone"
                        ItemStyle-CssClass="headerNone" />
                    <asp:TemplateField HeaderText="CUSTOMER">
                        <ItemTemplate>
                            <asp:Label ID="lblCustomerName" runat="server" Text='<%# Eval("CustomerName") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ACTION" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEdit" runat="server" Text="UPLOAD NEW DWG" CssClass="rowclick"
                                OnClick="lnkEdit_Click" Font-Size="10px"></asp:LinkButton>
                            <div style='font-size: 9px; padding-top: 2px; background-color: #E4F7CF;' class='<%# Eval("DwgFile").ToString()!="True"?"divNone":"" %>'>
                                <a onclick="TINY.box.show({iframe:'TyreDwgPopup.aspx?dwgid=&Reqid=TYREDRAWINGCATALOG/DWGRequest/<%# Eval("Slno") %>.pdf&Status=',boxid:'frameless',width:1000,height:750,fixed:false,maskid:'bluemask',maskopacity:40,closejs:function(){}})"
                                    style="cursor: pointer; text-decoration: underline; color: #082DEA;">VIEW CUSTOMER
                                    DWG</a></div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <div style="width: 1060px; float: left; text-align: left; margin-top: 5px;">
                <asp:Label runat="server" ID="lblDwgRemarks" ClientIDMode="Static" Text=""></asp:Label>
            </div>
        </div>
        <div id="divsave" style="display: none;" runat="server" clientidmode="Static">
            <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #000;
                width: 1078px;">
                <tr>
                    <td colspan="4">
                        <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #CE8686;
                            width: 1076px; line-height: 20px;" id="tbDwgUpload">
                            <tr>
                                <th>
                                    FILE CATEGORY
                                </th>
                                <td>
                                    <asp:DropDownList ID="ddlFileCategory" runat="server" ClientIDMode="Static" Width="200px">
                                        <asp:ListItem Text="Choose" Value="Choose" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="TDS DRAWING" Value="TDSDRAWING"></asp:ListItem>
                                        <asp:ListItem Text="PRODUCTION DRAWING" Value="PRODUCTIONDRAWING"></asp:ListItem>
                                        <asp:ListItem Text="RIM DRAWING" Value="RIMDRAWING"></asp:ListItem>
                                        <asp:ListItem Text="ASSY. DRAWING" Value="ASSYDRAWING"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <th>
                                    ETRTO REF
                                </th>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlETRTOREF" ClientIDMode="Static" Width="150px">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtETRTOREF" runat="server" Text="" ClientIDMode="Static" Width="150px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    PLATFORM
                                </th>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlPlatform" ClientIDMode="Static" Width="200px">
                                    </asp:DropDownList>
                                </td>
                                <th>
                                    RIM TYPE
                                </th>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlRIMTYPE" ClientIDMode="Static" Width="150px">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtRIMTYPE" runat="server" Text="" ClientIDMode="Static" Width="150px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    BRAND
                                </th>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlBrand" ClientIDMode="Static" Width="200px">
                                    </asp:DropDownList>
                                </td>
                                <th>
                                    NO OF STUD HOLES
                                </th>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlNoOfHoles" ClientIDMode="Static" Width="150px">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtNoOfHoles" runat="server" Text="" ClientIDMode="Static" Width="150px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    SIDEWALL
                                </th>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlSidewall" ClientIDMode="Static" Width="200px">
                                    </asp:DropDownList>
                                </td>
                                <th>
                                    PCD
                                </th>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlPCD" ClientIDMode="Static" Width="150px">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtPCD" runat="server" Text="" ClientIDMode="Static" Width="150px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    GRADE
                                </th>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlType" ClientIDMode="Static" Width="120px">
                                    </asp:DropDownList>
                                </td>
                                <th>
                                    BOREDIA
                                </th>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlBOREDIA" ClientIDMode="Static" Width="150px">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtBOREDIA" runat="server" Text="" ClientIDMode="Static" Width="150px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    SIZE
                                </th>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlSize" ClientIDMode="Static" Width="200px">
                                    </asp:DropDownList>
                                </td>
                                <th>
                                    STUD HOLES DIA
                                </th>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlHoleDia" ClientIDMode="Static" Width="150px">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtHoleDia" runat="server" Text="" ClientIDMode="Static" Width="150px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    RIM WIDTH
                                </th>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlRim" ClientIDMode="Static" Width="80px">
                                    </asp:DropDownList>
                                </td>
                                <th>
                                    DRAWING NAME & NO.
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="txtDrwaingNo" ClientIDMode="Static" Text="" Width="200px"
                                        onkeypress="return splCharNotAllowed(event)"></asp:TextBox>
                                    <div style="font-size: 10px; line-height: 12px;">
                                        <span style="color: #f00;">NOTE: </span><span>Special Characters <b>( , / \ ' : * ?
                                            " < > | ; & .(dot) )</b> Not Allowed in Drawing Name & No.</span>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    CUSTOMER SPECIFIC
                                </th>
                                <td colspan="3">
                                    <asp:DropDownList runat="server" ID="ddlCustomerSpecific" ClientIDMode="Static" Width="">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    REMARKS
                                </th>
                                <td colspan="3">
                                    <asp:TextBox runat="server" ID="txtDwgRemarks" ClientIDMode="Static" Text="" TextMode="MultiLine"
                                        Width="900px" Height="65px" onKeyUp="javascript:CheckMaxLength(this, 999);" onChange="javascript:CheckMaxLength(this, 999);"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="line-height: 15px;">
                                    <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <div id="dtitem" style="float: left; text-align: center;">
                                        <asp:DataList runat="server" ID="dlExistDwg" RepeatColumns="1" RepeatDirection="Horizontal"
                                            RepeatLayout="Table" ItemStyle-VerticalAlign="Top">
                                            <ItemTemplate>
                                                <asp:HiddenField runat="server" ID="imgname" Value='<%# Eval("FILEURL") %>' />
                                                <div style="float: left;">
                                                    <asp:Label runat="server" ID="lblnamefile" Text='<%# Eval("FILEname1") %>'> </asp:Label>
                                                    <a href='<%# Eval("filename") %>' target="_blank">view</a>
                                                </div>
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <div id="UPLOADCONTROL">
                            <asp:Label ID="lbltds" runat="server" Text="DRAWING FILE"></asp:Label>
                            <asp:FileUpload ID="fupTDS" runat="server" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Label runat="server" ID="lblSAVEMSG" ClientIDMode="Static" Text="" ForeColor="GREEN"
                            Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center;">
                        <asp:Button ID="btnOverWrite" runat="server" Text="OVERWRITE CURRENT DRAWING" CssClass="btnsave"
                            ClientIDMode="Static" OnClientClick="javascript:return CtrlAddNextItem();" OnClick="btnOverWrite_Click" />
                    </td>
                    <td style="text-align: center;">
                        <asp:Button ID="btnSave" runat="server" Text="SAVE" CssClass="btnsave" ClientIDMode="Static"
                            OnClientClick="javascript:return CtrlAddNextItem();" OnClick="btnSave_Click" />
                    </td>
                    <td style="text-align: center;">
                        <asp:Button ID="btnclear" runat="server" class="btnclear" ClientIDMode="Static" Text="CLEAR"
                            OnClick="btnclear_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div>
        <asp:HiddenField ID="hdnorginalfilename" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdncountimg" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnfiles" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdndatatask" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnPATHSAVE" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnETRTOREF" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnRIMTYPE" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnNoOfHoles" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnPCD" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnBOREDIA" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnHoleDia" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnRequestNo" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnDwgCategory" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnselectedrow" runat="server" ClientIDMode="Static" />
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#txtETRTOREF').css({ 'display': 'none' });
            $('#txtRIMTYPE').css({ 'display': 'none' });
            $('#txtNoOfHoles').css({ 'display': 'none' });
            $('#txtPCD').css({ 'display': 'none' });
            $('#txtBOREDIA').css({ 'display': 'none' });
            $('#txtHoleDia').css({ 'display': 'none' });
        });
        $('#ddlETRTOREF').change(function () {
            if ($('#ddlETRTOREF option:selected').text() == 'ADD NEW ENTRY')
                $('#txtETRTOREF').css({ 'display': 'block' });
            else
                $('#txtETRTOREF').css({ 'display': 'none' });
        });
        $('#ddlRIMTYPE').change(function () {
            if ($('#ddlRIMTYPE option:selected').text() == 'ADD NEW ENTRY')
                $('#txtRIMTYPE').css({ 'display': 'block' });
            else
                $('#txtRIMTYPE').css({ 'display': 'none' });
        });
        $('#ddlNoOfHoles').change(function () {
            if ($('#ddlNoOfHoles option:selected').text() == 'ADD NEW ENTRY')
                $('#txtNoOfHoles').css({ 'display': 'block' });
            else
                $('#txtNoOfHoles').css({ 'display': 'none' });
        });
        $('#ddlPCD').change(function () {
            if ($('#ddlPCD option:selected').text() == 'ADD NEW ENTRY')
                $('#txtPCD').css({ 'display': 'block' });
            else
                $('#txtPCD').css({ 'display': 'none' });
        });
        $('#ddlBOREDIA').change(function () {
            if ($('#ddlBOREDIA option:selected').text() == 'ADD NEW ENTRY')
                $('#txtBOREDIA').css({ 'display': 'block' });
            else
                $('#txtBOREDIA').css({ 'display': 'none' });
        });
        $('#ddlHoleDia').change(function () {
            if ($('#ddlHoleDia option:selected').text() == 'ADD NEW ENTRY')
                $('#txtHoleDia').css({ 'display': 'block' });
            else
                $('#txtHoleDia').css({ 'display': 'none' });
        });

        function ddl_Select() {
            $('#dtitem').css({ 'display': 'none' });
            $('#btnOverWrite').css({ 'display': 'none' });
            $('#btnSave').css({ 'display': 'block', 'text-align': 'center' });
            $('#lblErrMsg').html('');
            $('#lblSAVEMSG').html('');
        }
        $('#ddlPlatform').change(function () { ddl_Select(); });
        $('#ddlBrand').change(function () { ddl_Select(); });
        $('#ddlSidewall').change(function () { ddl_Select(); });
        $('#ddlType').change(function () { ddl_Select(); });
        $('#ddlSize').change(function () { ddl_Select(); });
        $('#ddlRim').change(function () { ddl_Select(); });
        $('#ddlFileCategory').change(function () { ddl_Select(); });

        function CtrlAddNextItem() {
            var errMsg = ''; $('#lblErrMsg').html(''); $('#lblSAVEMSG').html('');
            if ($('#ddlFileCategory option:selected').text() == 'Choose' || $('#ddlFileCategory option:selected').text() == '') { errMsg += 'Choose File Category<br/>'; }
            else {
                if ($('#ddlPlatform option:selected').text() == 'Choose' && $('#ddlBrand option:selected').text() == 'Choose' && $('#ddlSidewall option:selected').text() == 'Choose' && $('#ddlType option:selected').text() == 'Choose' && $('#ddlSize option:selected').text() == 'Choose' && $('#ddlRim option:selected').text() == 'Choose')
                    errMsg += 'Choose any one Platform, Brand, Sidewall, TyreType, Size, Rim<br/>';
                else {
                    if ($('#ddlSize option:selected').text() == 'Choose')
                        errMsg += 'Choose TyreSize<br/>';
                    if ($('#ddlRim option:selected').text() == 'Choose')
                        errMsg += 'Choose Rim<br/>';
                    if ($('#ddlETRTOREF option:selected').text() == 'Choose' && $('#ddlRIMTYPE option:selected').text() == 'Choose' && $('#ddlNoOfHoles option:selected').text() == 'Choose' && $('#ddlPCD option:selected').text() == 'Choose' && $('#ddlBOREDIA option:selected').text() == 'Choose' && $('#ddlHoleDia option:selected').text() == 'Choose')
                        errMsg += 'CHOOSE ETRTO REF, RIM TYPE, NO Of HOLES, PCD, BOREDIA, HOLE DIA<br/>';
                    else {
                        if ($('#ddlETRTOREF option:selected').text() == 'ADD NEW ENTRY' && $('#txtETRTOREF').val().length == 0) errMsg += 'Enter ETRTO REF<br/>';
                        if ($('#ddlRIMTYPE option:selected').text() == 'ADD NEW ENTRY' && $('#txtRIMTYPE').val().length == 0) errMsg += 'Enter RIM TYPE<br/>';
                        if ($('#ddlNoOfHoles option:selected').text() == 'ADD NEW ENTRY' && $('#txtNoOfHoles').val().length == 0) errMsg += 'Enter No of Holes<br/>';
                        if ($('#ddlPCD option:selected').text() == 'ADD NEW ENTRY' && $('#txtPCD').val().length == 0) errMsg += 'Enter PCD<br/>';
                        if ($('#ddlBOREDIA option:selected').text() == 'ADD NEW ENTRY' && $('#txtBOREDIA').val().length == 0) errMsg += 'Enter BOREDIA<br/>';
                        if ($('#ddlHoleDia option:selected').text() == 'ADD NEW ENTRY' && $('#txtHoleDia').val().length == 0) errMsg += 'Enter HOLE DIA<br/>';
                        else {
                            var array = ['pdf']; var xyz = document.getElementById('<%= fupTDS.ClientID %>'); var Extension = xyz.value.substring(xyz.value.lastIndexOf('.') + 1).toLowerCase();
                            if (array.indexOf(Extension) <= -1) { document.getElementById('<%= fupTDS.ClientID %>').value = ''; errMsg += 'Please Upload only pdf extension file<br/>'; }
                        }
                        if ($('#txtDrwaingNo').val().length == 0) errMsg += 'Enter Drawing No.<br/>';
                        else if ($('#txtDrwaingNo').val().length > 0) {
                            var refvalue = $('#txtDrwaingNo').val();
                            if (~refvalue.indexOf(",") || ~refvalue.indexOf("/") || ~refvalue.indexOf("\\") || ~refvalue.indexOf(":") || ~refvalue.indexOf("*") || ~refvalue.indexOf("?") ||
                            ~refvalue.indexOf("<") || ~refvalue.indexOf(">") || ~refvalue.indexOf("|") || ~refvalue.indexOf(";") || ~refvalue.indexOf("&") || ~refvalue.indexOf(".") || ~refvalue.indexOf("'"))
                                errMsg += "Special characters not allowed in drawing name & no.</br>";
                        }
                        if ($('#txtDwgRemarks').val().length == 0) errMsg += 'Enter Remarks<br/>';
                    }
                }
            }
            if (errMsg.length > 0) { $('#lblErrMsg').html(errMsg); return false; } else return true;
        }

        function bind_errmsg(strErr) { $('#lblSAVEMSG').html(strErr); }

        function splCharNotAllowed(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode == 34 || charCode == 38 || charCode == 39 || charCode == 42 || charCode == 44 || charCode == 46 || charCode == 47 || charCode == 58 || charCode == 59 ||
            charCode == 60 || charCode == 62 || charCode == 63 || charCode == 92 || charCode == 124)
                return false;

            return true;
        }
    </script>
</asp:Content>
