<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="claimregister1.aspx.cs" Inherits="TTS.claimregister1" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="styles/datemonthyear.css" rel="stylesheet" type="text/css" />
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .tbClaimReg
        {
            border-collapse: collapse;
            border-color: #605;
            margin-top: 10px;
            line-height: 20px;
            width: 100%;
        }
        .tbClaimReg th
        {
            background-color: #e1f8fb;
            text-align: left;
        }
        .req::after
        {
            content: "*";
            color: #f00;
            font-size: 18px;
        }
        .titleTD
        {
            background-color: #07868c;
            color: #fff;
            font-size: 14px;
            font-weight: bold;
        }
        .StencilFailureImgSize
        {
            height: 100px;
            width: 100px;
        }
        .parent
        {
            position: relative;
            display: inline-block;
            border: 1px;
            background-color: transparent;
            width: 400px;
            text-align: left;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div align="center" class="pageTitleHead">
        CLAIM REGISTER
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #000;
            width: 100%;">
            <tr>
                <td>
                    <asp:Label runat="server" ID="lbl1" ClientIDMode="Static" Text="CUSTOMER NAME" Font-Bold="true"
                        Font-Size="12px"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlClaimCustomer" AutoPostBack="True" Width="450"
                        ClientIDMode="Static" CssClass="form-control" OnSelectedIndexChanged="ddlClaimCustomer_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label runat="server" ID="lbl2" ClientIDMode="Static" Text="USER ID" Font-Bold="true"
                        Font-Size="12px"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlUserID" AutoPostBack="True" Width="250" ClientIDMode="Static"
                        CssClass="form-control" OnSelectedIndexChanged="ddlUserID_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:GridView runat="server" ID="gvClaimList" AutoGenerateColumns="false" Width="1100px"
                        OnRowDeleting="gvClaimList_RowDeleting" OnRowEditing="gvClaimList_RowEditing"
                        OnRowUpdating="gvClaimList_RowUpdating" OnRowCancelingEdit="gvClaimList_RowCancelingEdit">
                        <HeaderStyle BackColor="#D7F759" />
                        <Columns>
                            <asp:TemplateField HeaderText="BRAND" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:Label ID="lblbrand" runat="server" Text='<%# Eval("brand") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:HiddenField ID="lbledbrand" runat="server" Value='<%# Eval("brand") %>' />
                                    <asp:DropDownList runat="server" ID="ddledbrand" ClientIDMode="Static" CssClass="form-control">
                                    </asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SIZE" ItemStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lblTyreSize" runat="server" Text='<%# Eval("TyreSize") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:HiddenField ID="lbledTyreSize" runat="server" Value='<%# Eval("TyreSize") %>' />
                                    <asp:DropDownList runat="server" ID="ddledTyreSize" ClientIDMode="Static" CssClass="form-control">
                                    </asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TYPE" ItemStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lblTyreType" runat="server" Text='<%# Eval("TyreType") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:HiddenField ID="lbledTyreType" runat="server" Value='<%# Eval("TyreType") %>' />
                                    <asp:DropDownList runat="server" ID="ddledTyreType" ClientIDMode="Static" CssClass="form-control">
                                    </asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="STENCIL NO." ItemStyle-Width="80px">
                                <ItemTemplate>
                                    <asp:Label ID="lblStencilNo" runat="server" Text='<%# Eval("StencilNo") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:HiddenField ID="hdnstencilNo" runat="server" Value='<%# Eval("StencilNo") %>' />
                                    <asp:TextBox runat="server" ID="txtStencil" Text='<%# Eval("StencilNo") %>' ClientIDMode="Static"
                                        MaxLength="10" CssClass="form-control" Width="100px">></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="COMPLAINT" ItemStyle-Width="300px">
                                <ItemTemplate>
                                    <%#((string)Eval("AppStyle")).Replace("~", "<br/>")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox runat="server" ID="txtAppStyle" Text='<%#((string)Eval("AppStyle")).Replace("~", "\r\n")%>'
                                        ClientIDMode="Static" Width="300px" CssClass="form-control" Height="70px" TextMode="MultiLine"
                                        MaxLength="999"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="OPERATING CONDITION" ItemStyle-Width="300px">
                                <ItemTemplate>
                                    <%#((string)Eval("RunningHours")).Replace("~", "<br/>")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox runat="server" ID="txtRunningHours" Text='<%#((string)Eval("RunningHours")).Replace("~", "\r\n")%>'
                                        ClientIDMode="Static" Width="300px" CssClass="form-control" Height="70px" TextMode="MultiLine"
                                        MaxLength="999"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ACTION" ItemStyle-Width="120px">
                                <ItemTemplate>
                                    <asp:Button runat="server" ID="btnDelete" Text="Delete" CommandName="Delete" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <table cellspacing="0" cellpadding="4" rules="all" border="1" class="tbClaimReg">
                        <tr>
                            <td colspan="5" class="titleTD">
                                COMPLAINT TYRE DETAILS
                            </td>
                        </tr>
                        <tr>
                            <th class="req">
                                BRAND
                            </th>
                            <th class="req">
                                TYRE SIZE
                            </th>
                            <th>
                                TYRE TYPE
                            </th>
                            <th class="req">
                                STENCIL NO
                            </th>
                            <th>
                                TYRE POSITION
                            </th>
                        </tr>
                        <tr>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlClaimBrand" ClientIDMode="Static" Width="200px"
                                    CssClass="form-control" 
                                    onselectedindexchanged="ddlClaimBrand_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlClaimSize" ClientIDMode="Static" Width="250px"
                                    CssClass="form-control">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlTyreType" ClientIDMode="Static" Width="100px"
                                    CssClass="form-control">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtStencilNos" ClientIDMode="Static" MaxLength="12"
                                    CssClass="form-control" Width="100px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:RadioButtonList runat="server" ID="rdbTyrePosition" ClientIDMode="Static" Width="120px"
                                    RepeatColumns="2">
                                    <asp:ListItem Text="Drive" Value="Drive"></asp:ListItem>
                                    <asp:ListItem Text="Steer" Value="Steer"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" class="titleTD">
                                FORKLIFT / EQUIPMENT DETAILS
                            </td>
                        </tr>
                        <tr>
                            <th>
                                BRAND / MODEL
                            </th>
                            <th>
                                TYPE
                            </th>
                            <th>
                                DRIVE TYPE
                            </th>
                            <th>
                                OPERATING HOURS PER DAY
                            </th>
                            <th>
                                MAXIMUM TEMPERATURE
                            </th>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox runat="server" ID="txtForkliftBrand" ClientIDMode="Static" Text="" Width="200px"
                                    CssClass="form-control" MaxLength="50"></asp:TextBox>
                            </td>
                            <td>
                                <asp:RadioButtonList runat="server" ID="rdbForkliftType" ClientIDMode="Static" Width="150px"
                                    RepeatColumns="2">
                                    <asp:ListItem Text="3-Wheel" Value="3-Wheel"></asp:ListItem>
                                    <asp:ListItem Text="4-Wheel" Value="4-Wheel"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td>
                                <asp:RadioButtonList runat="server" ID="rdbForkliftDriveType" ClientIDMode="Static"
                                    Width="150px" RepeatColumns="2">
                                    <asp:ListItem Text="Single" Value="Single"></asp:ListItem>
                                    <asp:ListItem Text="Twin" Value="Twin"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtForkliftHours" ClientIDMode="Static" Width="60px"
                                    CssClass="form-control" onkeypress="return isNumberKey(event)" MaxLength="6"></asp:TextBox>
                            </td>
                            <td>
                                <div style="width: 65px; float: left; margin-top: 5px;">
                                    <asp:TextBox runat="server" ID="txtTemperature" ClientIDMode="Static" Text="" Width="60px"
                                        CssClass="form-control" onkeypress="return isNumberKey(event)" MaxLength="4"></asp:TextBox>
                                </div>
                                <div style="width: 100px; float: left;">
                                    <asp:RadioButtonList runat="server" ID="rdbTemperatureType" ClientIDMode="Static"
                                        Width="80px" RepeatColumns="2">
                                        <asp:ListItem Text="°C" Value="°C"></asp:ListItem>
                                        <asp:ListItem Text="°F" Value="°F"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                ANY ATTACHMENTS FITTED
                            </th>
                            <td colspan="4">
                                <div style="width: 100px; float: left;">
                                    <asp:RadioButtonList runat="server" ID="rdbAnyAttach" ClientIDMode="Static" Width="90px"
                                        RepeatColumns="2">
                                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                                <div style="width: 780px; float: left; margin-top: 3px;">
                                    If Yes, Details:
                                    <asp:TextBox runat="server" ID="txtAnyAttachDetails" ClientIDMode="Static" Text=""
                                        Width="660px" CssClass="form-control" MaxLength="100"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                FLOOR CONDITIONS
                            </th>
                            <td colspan="4">
                                <asp:TextBox runat="server" ID="txtFloorConditions" ClientIDMode="Static" Width="800px"
                                    CssClass="form-control" Height="35px" TextMode="MultiLine" MaxLength="999">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                DATE OF TYRE FITTED
                            </th>
                            <th>
                                DATE OF FAILURE
                            </th>
                            <th>
                                SERVICE HOURS
                            </th>
                            <th>
                                MAXIMUM LAOD CARRIED (kg)
                            </th>
                            <th>
                                PRESENT OUTSIDE DIAMETER (mm)
                            </th>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox runat="server" ID="txtTyreFittedDate" ClientIDMode="Static" Width="80px"
                                    CssClass="form-control"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtFailureDate" ClientIDMode="Static" Width="80px"
                                    CssClass="form-control"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtServiceHours" ClientIDMode="Static" Width="80px"
                                    CssClass="form-control" onkeypress="return isNumberKey(event)" MaxLength="6"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtMaximumLoadKg" ClientIDMode="Static" Width="80px"
                                    CssClass="form-control" onkeypress="return isNumberKey(event)" MaxLength="6"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtOutsideDiaMM" ClientIDMode="Static" Width="80px"
                                    CssClass="form-control" onkeypress="return isNumberKey(event)" MaxLength="6"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th class="req">
                                COMPLAINT DESCRIPTION
                            </th>
                            <td colspan="4">
                                <asp:TextBox runat="server" ID="txtComplaint" ClientIDMode="Static" Width="800px"
                                    CssClass="form-control" Height="35px" TextMode="MultiLine" MaxLength="999">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th style="vertical-align: top;">
                                <div style="width: 175px; float: left; line-height: 15px; background-color: #c10909;
                                    color: #fff;">
                                    <span style="color: #f00; background-color: #F5B7B1;">Note: </span>Each image file
                                    should be less then 5MB, Upload one image file for stencil no. and maximum three
                                    images for failure.
                                </div>
                            </th>
                            <td colspan="2" style="vertical-align: top;">
                                <div class="headCss req" style="width: 100px; float: left;">
                                    Stencil Image :
                                </div>
                                <div style="width: 415px; float: left;">
                                    <asp:FileUpload ID="up_Stencil" runat="server" ClientIDMode="Static" onchange="uploadImages('up_Stencil','uploadstencilimage')"
                                        accept=".png,.jpg,.jpeg,.gif,.bmp,.tif" />
                                </div>
                            </td>
                            <td colspan="2" style="vertical-align: top;">
                                <div class="headCss req" style="width: 100px; float: left;">
                                    Failure Image :
                                </div>
                                <div style="width: 415px; float: left;">
                                    <input type="file" id="up_Failure" onchange="uploadImages('up_Failure','uploadfailureimage')"
                                        accept=".png,.jpg,.jpeg,.gif,.bmp,.tif">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                UPLOAD IMAGE NAMES
                            </th>
                            <td colspan="4">
                                <asp:UpdatePanel ID="updpnl_StencilFailureImage" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:DataList runat="server" ID="gv_StencilFailure" RepeatColumns="2" RepeatDirection="Horizontal"
                                            RepeatLayout="Table">
                                            <ItemTemplate>
                                                <div class="parent">
                                                    <img alt="close" class="close" style="width: 15px; height: 15px;" src="images/cancel.png"
                                                        id="img2" runat="server" onclick="closebuttonaction(this)" />
                                                    <asp:Label runat="server" ID="lblImgName" Text='<%# Eval("ClaimImageName") %>'></asp:Label>
                                                    <asp:HiddenField runat="server" ID="hdn1" Value='<%# Eval("ClaimImage") %>' />
                                                </div>
                                            </ItemTemplate>
                                        </asp:DataList>
                                        <asp:Button ID="btnTriggerGv" runat="server" ClientIDMode="static" Style="visibility: hidden;"
                                            OnClick="btnTriggerGv_Click" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnTriggerGv" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <div style="text-align: center; width: 880px; float: left;">
                                    If you have another tyre failure
                                    <asp:Button runat="server" ID="btnAddMore" ClientIDMode="Static" Text="Add Next"
                                        CssClass="btn btn-info" OnClientClick="javascript:return CtrlClaimValidate();"
                                        OnClick="btnAddMore_Click" />
                                    tyre failure complaint
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" style="line-height: 15px; text-align: center;">
                                <asp:Label runat="server" ID="lblErrmsg" ClientIDMode="Static" Text="" ForeColor="Red"
                                    Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <div style="width: 880px; float: left; line-height: 20px;">
                                    <span class="headCss" style="width: 90px; float: left;">Any Comments</span>
                                    <asp:TextBox runat="server" ID="txtClaimComments" ClientIDMode="Static" Width="1070px"
                                        Height="60px" TextMode="MultiLine" MaxLength="999" CssClass="form-control">
                                    </asp:TextBox>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="text-align: left; padding-right: 10px;">
                                <span class="headCss">Ref No: </span><span style="color: #000; font-size: 9px;">(INVOICE
                                    / PO)</span>
                                <asp:TextBox runat="server" ID="txtCustomerRefNo" ClientIDMode="Static" Text="" Width="400px"
                                    CssClass="form-control" MaxLength="100"></asp:TextBox>
                            </td>
                            <td colspan="1" style="text-align: left; padding-left: 10px;">
                                <asp:Button runat="server" ID="btnSendClaimList" ClientIDMode="Static" Text="SEND CLAIM LIST"
                                    CssClass="btn btn-success" OnClick="btnSendClaimList_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnCustomer" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnBrand" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnTyreSize" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnTyreType" ClientIDMode="Static" Value="" />
    <script src="scripts/datemonthyear.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("input:radio").removeAttr("checked");
            $("input:text").val("");
            $("textarea").val("");
            $(':text').bind('keydown', function (e) { if (e.keyCode == 13) { e.preventDefault(); return false; } });
            $("#txtTyreFittedDate").datepicker({ minDate: "-730D", maxDate: "+0D" }).keydown(function (e) { e.keyWhich = 0; e.keyCode = 0; e.preventDefault(); });
            $("#txtFailureDate").datepicker({ minDate: "-30D", maxDate: "+0D" }).keydown(function (e) { e.keyWhich = 0; e.keyCode = 0; e.preventDefault(); });

            $('#ddlClaimCustomer').change(function () {
                $('#hdnCustomer').val($('#ddlClaimCustomer option:selected').text());
            });

            $('#ddlClaimBrand').change(function () {
                $('#hdnBrand').val(''); $('#ddlClaimSize').html(''); var strBrand = $("#ddlClaimBrand option:selected").text();
                if (strBrand != "CHOOSE") {
                    $.ajax({ type: "POST", url: "BindRecords.aspx?type=getClaimSize&Brand=" + strBrand + "", context: document.body,
                        success: function (data) { if (data != '') { $('#ddlClaimSize').html(data); $('#hdnBrand').val(strBrand); } }
                    });
                }
            });

            $('#ddlClaimSize').change(function () {
                $('#hdnTyreSize').val(''); var strSize = $("#ddlClaimSize option:selected").text(); var strBrand = $("#ddlClaimBrand option:selected").text();
                if (strSize != "CHOOSE") {
                    $.ajax({ type: "POST", url: "BindRecords.aspx?type=getClaimType&Brand=" + strBrand + "&Size=" + strSize + "", context: document.body,
                        success: function (data) { if (data != '') { $('#ddlTyreType').html(data); $('#hdnTyreSize').val(strSize); } }
                    });
                }
            });
            $('#ddlTyreType').change(function () {
                $('#hdnTyreType').val(''); var strType = $("#ddlTyreType option:selected").text();
                if (strType != "CHOOSE")
                    $('#hdnTyreType').val(strType);
            });
            $('#ddledbrand').change(function () {
                var strBrand = $("#ddledbrand option:selected").text();
                $.ajax({ type: "POST", url: "BindRecords.aspx?type=getClaimSize&Brand=" + strBrand + "", context: document.body,
                    success: function (data) { if (data != '') { $('#ddledTyreSize').html(data); $('#hdnBrand').val(strBrand); } }
                });
            });
            $('#ddledTyreSize').change(function () {
                $('#hdnTyreSize').val(''); var strSize = $("#ddledTyreSize option:selected").text(); var strBrand = $("#ddledbrand option:selected").text();
                if (strSize != "CHOOSE") {
                    $.ajax({ type: "POST", url: "BindRecords.aspx?type=getClaimType&Brand=" + strBrand + "&Size=" + strSize + "", context: document.body,
                        success: function (data) { if (data != '') { $('#ddledTyreType').html(data); $('#hdnTyreSize').val(strSize); } }
                    });
                }
            });
            $('#ddledTyreType').change(function () {
                $('#hdnTyreType').val(''); var strType = $("#ddledTyreType option:selected").text();
                if (strType != "CHOOSE")
                    $('#hdnTyreType').val(strType);
            });
            $('#txtStencilNos').blur(function () {
                var strstencil = $("#txtStencilNos").val(); var char1 = strstencil.charAt(0); var firstchar = char1.toUpperCase();
                if (firstchar == 'C' || firstchar == 'L' || firstchar == 'P') {
                    if (strstencil.length > 9) {
                        $.ajax({ type: "POST", url: "BindRecords.aspx?type=chkClaimstencil&claimstencilno=" + strstencil,
                            success: function (data) { if (data != '') { alert(data + ' ' + $("#txtStencilNos").val()); $("#txtStencilNos").val(''); } }
                        });
                    }
                }
            });
            $('#txtStencil').blur(function () {
                var strstencil = $("#txtStencil").val(); var char1 = strstencil.charAt(0); var firstchar = char1.toUpperCase();
                if (firstchar == 'C' || firstchar == 'L' || firstchar == 'P') {
                    if (strstencil.length > 9) {
                        $.ajax({ type: "POST", url: "BindRecords.aspx?type=chkClaimstencil&claimstencilno=" + strstencil,
                            success: function (data) { if (data != '') { alert(data + ' ' + $("#txtStencil").val()); $("#txtStencil").val(''); } }
                        });
                    }
                }
            });
        });
        function validateedit(lnk) {
            var row = lnk.parentNode.parentNode;
            var rowIndex = row.rowIndex - 1;
            var errMsg = ''; $('#lblErrMsg1').html('');
            if ($('#hdnBrand').val().length == 0 || $('#ddledbrand option:selected').text() == 'CHOOSE')
                errMsg += 'Choose Brand<br/>';
            if ($('#hdnTyreSize').val().length == 0 || $('#ddledTyreSize option:selected').text() == 'CHOOSE')
                errMsg += 'Choose Size<br/>';
            if ($('#txtStencil').val().length == 0)
                errMsg += 'Enter Stencil No.<br/>';
            else if ($('#txtStencil').val().length < 10)
                errMsg += 'Enter Proper Stencil No.<br/>';
            if ($('#txtAppStyle').val().length == 0)
                errMsg += 'Enter Complaint<br/>';
            if (errMsg.length > 0) {
                $('#lblErrmsg1').html(errMsg);
                return false;
            }
            else
                return true;
        }
        function CtrlClaimValidate() {
            $('#lblErrmsg').html(''); var errMsg = '';
            if ($('#hdnBrand').val().length == 0)
                errMsg += 'Choose Brand<br/>';
            if ($('#hdnTyreSize').val().length == 0)
                errMsg += 'Choose Size<br/>';
            if ($('#txtStencilNos').val().length == 0)
                errMsg += 'Enter Stencil No.<br/>';
            else if ($('#txtStencilNos').val().length < 10)
                errMsg += 'Enter Proper Stencil No.<br/>';
            if ($('#txtComplaint').val().length == 0)
                errMsg += 'Enter Complaint <br/>';
            if (errMsg.length > 0) {
                $('#lblErrmsg').html(errMsg);
                return false;
            }
            else
                return true;
        }

        function uploadImages(CtrlFileUpload, UploadHandler) {
            var fileUpload = $('#' + CtrlFileUpload).get(0);
            var files = fileUpload.files;
            var test = new FormData();
            for (var i = 0; i < files.length; i++) {
                test.append(files[i].name, files[i]);
            }
            $.ajax({
                url: UploadHandler + ".ashx", type: "POST", contentType: false, processData: false, data: test,
                success: function () { $('#btnTriggerGv').click(); },
                error: function () { alert("File not support, Contact your admin"); }
            });
        }
        function closebuttonaction(event) {
            var name = event.id;
            name = name.replace('_img2_', '_hdn1_');
            var id = $('#' + name).val();
            $.ajax({ type: "POST", url: "BindRecords.aspx?type=delstencilFailure&path=" + id,
                success: function (data) { if (data == '') { $('#btnTriggerGv').click(); } }
            });
        }
    </script>
</asp:Content>
