<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="TyreDrawingEdit.aspx.cs" Inherits="TTS.TyreDrawingEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/TinyStyle.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/TinyboxScript.js" type="text/javascript"></script>
    <script src="Scripts/cotsScript.js" type="text/javascript"></script>
    <style type="text/css">
        #tbDwgEditMaster th
        {
            background-color: #FCD7D7;
            text-align: left;
        }
        #tbDwgEdit th
        {
            background-color: #E4F7CF;
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
        <div id="divmcont" style="display: none;">
            <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #CE8686;
                width: 1076px;" id="tbDwgEditMaster">
                <tr style="line-height: 20px;">
                    <th>
                        FILE CATEGORY
                    </th>
                    <td>
                        <asp:DropDownList ID="ddlMFileCategory" runat="server" ClientIDMode="Static">
                        </asp:DropDownList>
                    </td>
                    <th>
                        ETRTO REF
                    </th>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlMETRTOREF" ClientIDMode="Static">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th>
                        PLATFORM
                    </th>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlMPlatform" ClientIDMode="Static" Width="120px">
                        </asp:DropDownList>
                    </td>
                    <th>
                        RIM TYPE
                    </th>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlMRimType" ClientIDMode="Static">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th>
                        BRAND
                    </th>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlMBrand" ClientIDMode="Static" Width="120px">
                        </asp:DropDownList>
                    </td>
                    <th>
                        NO OF STUD HOLES
                    </th>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlMStudHoles" ClientIDMode="Static">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th>
                        SIDEWALL
                    </th>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlMSidewall" ClientIDMode="Static" Width="120px">
                        </asp:DropDownList>
                    </td>
                    <th>
                        PCD
                    </th>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlMPCD" ClientIDMode="Static">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th>
                        GRADE
                    </th>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlMGrade" ClientIDMode="Static" Width="100px">
                        </asp:DropDownList>
                    </td>
                    <th>
                        BOREDIA
                    </th>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlMBOREDIA" ClientIDMode="Static">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th>
                        SIZE
                    </th>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlMSize" ClientIDMode="Static" Width="200px">
                        </asp:DropDownList>
                    </td>
                    <th>
                        STUD HOLES DIA
                    </th>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlMStudHolesDia" ClientIDMode="Static">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th>
                        RIM WIDTH
                    </th>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlMRimWidth" ClientIDMode="Static" Width="60px">
                        </asp:DropDownList>
                    </td>
                    <td colspan="2">
                        <asp:Label runat="server" ID="lblMsg" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th>
                        CUSTOMER SPECIFIC
                    </th>
                    <td colspan="3">
                        <asp:DropDownList runat="server" ID="ddlMCustomerDwg" ClientIDMode="Static">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <div style="text-align: center; margin-top: 5px;">
                            <asp:Button ID="btnShow" runat="server" Text="SHOW DRAWING LIST" CssClass="btnshow"
                                ClientIDMode="Static" OnClick="btnShow_Click" /></div>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <asp:GridView runat="server" ID="gvDwgLibraryList" AutoGenerateColumns="false" Width="1076px"
                AlternatingRowStyle-BackColor="#f5f5f5" AllowPaging="true" PageSize="25" PagerStyle-Height="30px"
                PagerStyle-Font-Bold="true" PagerStyle-Font-Size="15px" PagerStyle-HorizontalAlign="Center"
                OnPageIndexChanging="gvDwgLibraryList_PageIndex" PagerStyle-VerticalAlign="Middle"
                RowStyle-Height="20px">
                <HeaderStyle Font-Bold="true" BackColor="#fefe8b" Height="20px" />
                <Columns>
                    <asp:TemplateField HeaderText="CATEGORY">
                        <ItemTemplate>
                            <asp:Label ID="lblgrfilecategory" runat="server" Text='<%# Eval("filecategory") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="PLATFORM">
                        <ItemTemplate>
                            <asp:Label ID="lblgrconfig" runat="server" Text='<%# Eval("config") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="BRAND">
                        <ItemTemplate>
                            <asp:Label ID="lblgrbrand" runat="server" Text='<%# Eval("brand") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="SIDEWALL">
                        <ItemTemplate>
                            <asp:Label ID="lblgrsidewall" runat="server" Text='<%# Eval("sidewall") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="GRADE">
                        <ItemTemplate>
                            <asp:Label ID="lblgrtyretype" runat="server" Text='<%# Eval("tyretype") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="SIZE">
                        <ItemTemplate>
                            <asp:Label ID="lblgrtyresize" runat="server" Text='<%# Eval("tyresize") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="RIM WIDTH">
                        <ItemTemplate>
                            <asp:Label ID="lblgrrimwidth" runat="server" Text='<%# Eval("rimwidth") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ETRTO REF">
                        <ItemTemplate>
                            <asp:Label ID="lblgrETRTOREF" runat="server" Text='<%# Eval("ETRTOREF") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="RIM TYPE">
                        <ItemTemplate>
                            <asp:Label ID="lblgrRIMTYPE" runat="server" Text='<%# Eval("RIMTYPE") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="NO OF STUD HOLES">
                        <ItemTemplate>
                            <asp:Label ID="lblgrNoOfHoles" runat="server" Text='<%# Eval("NoOfHoles") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="PCD">
                        <ItemTemplate>
                            <asp:Label ID="lblgrPCD" runat="server" Text='<%# Eval("PCD") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="BOREDIA">
                        <ItemTemplate>
                            <asp:Label ID="lblgrBOREDIA" runat="server" Text='<%# Eval("BOREDIA") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="STUD HOLES DIA">
                        <ItemTemplate>
                            <asp:Label ID="lblgrHoleDia" runat="server" Text='<%# Eval("HoleDia") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ACTION" ItemStyle-Width="60px">
                        <ItemTemplate>
                            <asp:HiddenField ID="hdnstrcustomerspecific" runat="server" ClientIDMode="Static"
                                Value='<%# Eval("CUSTOMERSPECIFIC") %>' />
                            <asp:HiddenField ID="hdnsrtname" runat="server" ClientIDMode="Static" Value='<%# Eval("PRODUCTDRAWING") %>' />
                            <asp:HiddenField ID="hdnstrPdfCount" runat="server" ClientIDMode="Static" Value='<%# Eval("pdfcount") %>' />
                            <%--<asp:HiddenField ID="hdnDwgApprove" runat="server" ClientIDMode="Static" Value='<%# Eval("DwgApprove") %>' />--%>
                            <asp:HiddenField ID="hdnfilecategory" runat="server" ClientIDMode="Static" Value='<%# Eval("filecategory") %>' />
                            <asp:HiddenField ID="hdnid" runat="server" ClientIDMode="Static" Value='<%# Eval("ID") %>' />
                            <asp:HiddenField runat="server" ID="hdnDrwaingNo" Value='<%# Eval("DrwaingNo") %>' />
                            <asp:HiddenField runat="server" ID="hdnRequestNo" Value='<%# Eval("RequestNo") %>' />
                            <asp:LinkButton ID="lnkEdit" runat="server" ClientIDMode="Static" Text="EDIT" OnClick="lnkEdit_Click"
                                Font-Size="10px"></asp:LinkButton>
                            &nbsp; <a onclick="TINY.box.show({iframe:'TyreDwgPopup.aspx?notapproveid=<%# Eval("ID") %>',boxid:'frameless',width:1000,height:750,fixed:false,maskid:'bluemask',maskopacity:40,closejs:function(){}})"
                                style="cursor: pointer; text-decoration: underline; color: #082DEA; font-size: 10px;">
                                VIEW</a>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <div id="Divcont" style="display: none;">
            <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #CE8686;
                width: 1076px;" id="tbDwgEdit">
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
                        <asp:DropDownList runat="server" ID="ddlCustomerDwg" ClientIDMode="Static" Width="">
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
                <tr>
                    <td colspan="4">
                        <asp:Label runat="server" ID="lblSAVEMSG" ClientIDMode="Static" Text="" ForeColor="GREEN"
                            Font-Bold="true"></asp:Label>
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
                    <td colspan="2" style="text-align: center;">
                        <asp:Button ID="btnOverWrite" runat="server" Text="OVERWRITE CURRENT DRAWING" CssClass="btnsave"
                            ClientIDMode="Static" OnClientClick="javascript:return CtrlAddNextItem();" OnClick="btnOverWrite_Click" />
                    </td>
                    <td colspan="2">
                        <div style="text-align: center; margin-top: 5px;">
                            <asp:Button ID="btnSave" runat="server" Text="SAVE" CssClass="btnshow" ClientIDMode="Static"
                                OnClientClick="javascript:return CtrlAddNextItem();" OnClick="btnSave_Click" /></div>
                    </td>
                </tr>
            </table>
        </div>
        <asp:HiddenField ID="hdnurl" runat="server" ClientIDMode="Static" Value="" />
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
        <asp:HiddenField ID="hdnID" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnRequestSlNo" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnqurstr" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnOfileName" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnOCategory" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnDwgCategory" runat="server" ClientIDMode="Static" />
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

        function CtrlAddNextItem() {
            var errMsg = ''; $('#lblErrMsg').html(''); $('#lblSAVEMSG').html('');
            if ($('#ddlFileCategory option:selected').text() == 'Choose' || $('#ddlFileCategory option:selected').text() == '') {
                errMsg += 'Choose File Category<br/>';
            } else {
                if ($('#ddlPlatform option:selected').text() == 'Choose' && $('#ddlBrand option:selected').text() == 'Choose' && $('#ddlSidewall option:selected').text() == 'Choose' && $('#ddlType option:selected').text() == 'Choose' && $('#ddlSize option:selected').text() == 'Choose' && $('#ddlRim option:selected').text() == 'Choose')
                    errMsg += 'Choose any one Platform, Brand, Sidewall, TyreType, Size, Rim<br/>';
                else {
                    if ($('#ddlETRTOREF option:selected').text() == 'Choose' && $('#ddlRIMTYPE option:selected').text() == 'Choose' && $('#ddlNoOfHoles option:selected').text() == 'Choose' && $('#ddlPCD option:selected').text() == 'Choose' && $('#ddlBOREDIA option:selected').text() == 'Choose' && $('#ddlHoleDia option:selected').text() == 'Choose')
                        errMsg += 'CHOOSE ETRTO REF, RIM TYPE, NO Of HOLES, PCD, BOREDIA, HOLE DIA<br/>';
                    else {
                        if ($('#ddlETRTOREF option:selected').text() == 'ADD NEW ENTRY' && $('#txtETRTOREF').val().length == 0)
                            errMsg += 'Enter ETRTO REF<br/>';
                        if ($('#ddlRIMTYPE option:selected').text() == 'ADD NEW ENTRY' && $('#txtRIMTYPE').val().length == 0)
                            errMsg += 'Enter RIM TYPE<br/>';
                        if ($('#ddlNoOfHoles option:selected').text() == 'ADD NEW ENTRY' && $('#txtNoOfHoles').val().length == 0)
                            errMsg += 'Enter No of Holes<br/>';
                        if ($('#ddlPCD option:selected').text() == 'ADD NEW ENTRY' && $('#txtPCD').val().length == 0)
                            errMsg += 'Enter PCD<br/>';
                        if ($('#ddlBOREDIA option:selected').text() == 'ADD NEW ENTRY' && $('#txtBOREDIA').val().length == 0)
                            errMsg += 'Enter BOREDIA<br/>';
                        if ($('#ddlHoleDia option:selected').text() == 'ADD NEW ENTRY' && $('#txtHoleDia').val().length == 0)
                            errMsg += 'Enter HOLE DIA<br/>'; else if ($('#hdnqurstr').val() == 'ntedit') {
                            var array = ['pdf']; var xyz = document.getElementById('<%= fupTDS.ClientID %>'); var Extension = xyz.value.substring(xyz.value.lastIndexOf('.') + 1).toLowerCase();
                            if (array.indexOf(Extension) <= -1) { document.getElementById('<%= fupTDS.ClientID %>').value = ''; errMsg += 'Please Upload only pdf extension file<br/>'; }
                        }
                    }
                    if ($('#txtDrwaingNo').val().length == 0)
                        errMsg += 'Enter Drawing No.<br/>';
                    else if ($('#txtDrwaingNo').val().length > 0) {
                        var refvalue = $('#txtDrwaingNo').val();
                        if (~refvalue.indexOf(",") || ~refvalue.indexOf("/") || ~refvalue.indexOf("\\") || ~refvalue.indexOf(":") || ~refvalue.indexOf("*") || ~refvalue.indexOf("?") ||
                            ~refvalue.indexOf("<") || ~refvalue.indexOf(">") || ~refvalue.indexOf("|") || ~refvalue.indexOf(";") || ~refvalue.indexOf("&") || ~refvalue.indexOf(".") ||
                            ~refvalue.indexOf("'"))
                            errMsg += "Special characters not allowed in drawing name & no.</br>";
                    }
                    if ($('#txtDwgRemarks').val().length == 0)
                        errMsg += 'Enter Remarks<br/>';
                }
            }
            if (errMsg.length > 0) {
                $('#lblErrMsg').html(errMsg);
                return false;
            }
            else
                return true;
        }

        function displayDiv(ctrl) {
            $('#' + ctrl).css({ 'display': 'block' });
            gotoPreviewDiv(ctrl);
        }

        function splCharNotAllowed(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode == 34 || charCode == 38 || charCode == 39 || charCode == 42 || charCode == 44 || charCode == 46 || charCode == 47 || charCode == 58 || charCode == 59 ||
            charCode == 60 || charCode == 62 || charCode == 63 || charCode == 92 || charCode == 124)
                return false;

            return true;
        }
    </script>
</asp:Content>
