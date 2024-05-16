<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="CustomerDetails.aspx.cs" Inherits="TTS.CustomerDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/popupBox.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .tableCust
        {
            border-collapse: collapse;
            border-color: #000;
            width: 1080px;
            line-height: 25px;
        }
        .tableCust th
        {
            background-color: #FFEEEC;
            text-align: left;
            padding-left: 10px;
            width: 150px;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div align="center" class="pageTitleHead">
        ADD CUSTOMER DETAILS</div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" class="tableCust">
            <tr>
                <th>
                    Customer Category
                </th>
                <td>
                    <asp:RadioButtonList runat="server" ID="rdbCustCategory" ClientIDMode="Static" RepeatDirection="Horizontal"
                        Width="150px">
                        <asp:ListItem Text="Existing" Value="Existing" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Prospect" Value="Prospect"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <th>
                    Type
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="ddlType" CssClass="ddlCss" ClientIDMode="Static"
                        Width="150px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    Name
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtName" Text="" CssClass="txtCss" ClientIDMode="Static"
                        onkeypress="return splCharNotAllowed(event)" Width="350px"></asp:TextBox>
                    <span id="divName"></span>
                </td>
                <th>
                    Contact Person
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtContact" Text="" CssClass="txtCss" ClientIDMode="Static"
                        Width="250px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th rowspan="2">
                    Address
                </th>
                <td rowspan="2" style="line-height: 22px;">
                    <asp:TextBox runat="server" ID="txtAdd1" Text="" CssClass="txtCss" ClientIDMode="Static"
                        Width="350px"></asp:TextBox>
                    <asp:TextBox runat="server" ID="txtAdd2" Text="" CssClass="txtCss" ClientIDMode="Static"
                        Width="350px"></asp:TextBox>
                    <asp:TextBox runat="server" ID="txtAdd3" Text="" CssClass="txtCss" ClientIDMode="Static"
                        Width="350px"></asp:TextBox>
                </td>
                <th>
                    Phone
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtPhone" Text="" CssClass="txtCss" ClientIDMode="Static"
                        Width="250px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    Mobile
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtMobile" Text="" CssClass="txtCss" ClientIDMode="Static"
                        Width="250px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    Country
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="ddlCountry" CssClass="ddlCss" ClientIDMode="Static"
                        OnSelectedIndexChanged="ddlCountry_IndexChange" AutoPostBack="true">
                    </asp:DropDownList>
                    <span style="display: none; line-height: 10px;" id="divNewCountry"><span style="color: #ff0000;">
                        Enter New Country:</span>
                        <asp:TextBox runat="server" ID="txtNewCountry" ClientIDMode="Static" Width="300px"
                            Text=""></asp:TextBox>
                    </span>
                </td>
                <th>
                    Channel
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="ddlChannel" CssClass="ddlCss" ClientIDMode="Static"
                        Width="150px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    City
                </th>
                <td>
                    <span style="display: block;" id="divExistCity">
                        <asp:DropDownList runat="server" ID="ddlCity" CssClass="ddlCss" ClientIDMode="Static">
                        </asp:DropDownList>
                    </span><span style="display: none; line-height: 20px;" id="divNewCity"><span style="color: #ff0000;">
                        Enter New City:</span>
                        <asp:TextBox runat="server" ID="txtNewCity" ClientIDMode="Static" Width="300px" Text=""></asp:TextBox>
                    </span>
                </td>
                <th>
                    Price Unit
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="ddlPriceUnit" CssClass="ddlCss" ClientIDMode="Static">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    ZipCode
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtZip" Text="" CssClass="txtCss" ClientIDMode="Static"></asp:TextBox>
                </td>
                <th>
                    Price Basis
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="ddlPriceBasis" CssClass="ddlCss" ClientIDMode="Static"
                        Width="150px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    E-mail
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtEmail" Text="" CssClass="txtCss" ClientIDMode="Static"
                        Width="350px"></asp:TextBox>
                </td>
                <th>
                    Port
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtPort" Text="" CssClass="txtCss" ClientIDMode="Static"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    Web Address
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtWebAddress" Text="" CssClass="txtCss" ClientIDMode="Static"
                        Width="350px"></asp:TextBox>
                </td>
                <th>
                    Lead Source
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtLeadSource" Text="" CssClass="txtCss" ClientIDMode="Static"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:TextBox runat="server" ID="txtCustSpl" Text="Enter Special Instructions" CssClass="txtSplCss"
                        onKeyUp="javascript:CheckMaxLength(this, 3999);" onChange="javascript:CheckMaxLength(this, 3999);"
                        TextMode="MultiLine" ClientIDMode="Static"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div style="float: left; width: 500px; line-height: 15px; color: #ff0000;" id="divErrMsg"
                        runat="server" clientidmode="Static">
                    </div>
                </td>
                <td colspan="2">
                    <asp:LinkButton runat="server" ID="lnkSave" CssClass="btnsave" ClientIDMode="Static"
                        OnClientClick="javascript:return CustCtrlValidation();" OnClick="lnkSave_click"
                        Text="Save" Width="50px"></asp:LinkButton>
                    <span class="btnclear" id="lnkClear" style="cursor: pointer; width: 50px;">Clear</span>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnCustType" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnCustCategory" ClientIDMode="Static" Value="Exist" />
    <asp:HiddenField runat="server" ID="hdnCountryName" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnCityName" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnTitleName" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnCustCode" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        $(document).ready(function () {
            $('#txtName').focus();
            $('#txtCustSpl').blur(function () {
                if ($(this).val().trim().length == 0) {
                    $(this).val('Enter Special Instructions').css({ "border-color": '#000', "background-color": "#fff" });
                }
            }).focus(function () {
                if ($(this).val().trim().toLowerCase() == "enter special instructions") {
                    $(this).val('').css({ "border-color": '#000', "background-color": "#E4F7CF" });
                }
            })
            $('#txtCustSpl').trigger("blur");

            $('#lnkClear').click(function () {
                window.location.href = window.location.href;
            });

            $('#ddlType').change(function () {
                var strSplitType = '';
                var strcustType = $("#ddlType option:selected").text();
                if (strcustType.toLowerCase() == "end user")
                    strSplitType = "EU";
                else if (strcustType.toLowerCase() == "corporate")
                    strSplitType = "CP";
                else if (strcustType.toLowerCase() == "dealer")
                    strSplitType = "DE";
                else if (strcustType.toLowerCase() == "oem")
                    strSplitType = "OEM";
                else if (strcustType.toLowerCase() == "competitor")
                    strSplitType = "COM";
                else if (strcustType.toLowerCase() == "me")
                    strSplitType = "ME";
                $('#hdnCustType').val(strSplitType);
            });

            $('#txtName').focus(function () {
                $('#divName').html(''); $('#divErrMsg').html('');
            }).blur(function () {
                var cname = $('#txtName').val(); var errMsg = $('#divName').html();
                if (cname != '' && errMsg == '') {
                    $.ajax({ type: "POST", url: "UserValidation.aspx?type=chkcust&cname=" + cname + "&custtype=" + $('#hdnCustCategory').val(), context: document.body,
                        success: function (data) { if (data == "exists") { $('#divName').html('Name already exists').css({ "color": '#e5403a', "float": 'left' }); } }
                    });
                }
            });

            $('#rdbCustCategory_0').click(function () { $('#hdnCustCategory').val('Exist'); $('#txtName').val(''); $('#divName').html(''); $('#divErrMsg').html(''); });
            $('#rdbCustCategory_1').click(function () { $('#hdnCustCategory').val('Prospect'); $('#txtName').val(''); $('#divName').html(''); $('#divErrMsg').html(''); });
            $('#ddlCountry').change(function () { $('#hdnCountryName').val($('#ddlCountry option:selected').text()); if ($('#hdnCountryName').val() == 'Add New Country') { AddNewCountry(); return false } else { return true; } });
            $('#ddlCity').change(function () { $('#hdnCityName').val($("#ddlType option:selected").text()); AddNewCity(); });
        });

        function CustCtrlValidation() {
            var errMsg = $('#divName').html();
            $('#divErrMsg').html('');
            var emailpattern = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
            var intPattern = /^\d+$/; ;
            if (errMsg == '') {
                var msg = '';
                if ($('#txtName').val().length == 0)
                    msg += 'Enter customer name <br />';
                else if ($('#txtName').val().length > 0) {
                    var refvalue = $('#txtName').val();
                    if (~refvalue.indexOf("/") || ~refvalue.indexOf("&") || ~refvalue.indexOf("\\"))
                        errmsg += "Special characters(\,/,&) not allowed in order reference No.</br>";
                }
                if ($('#hdnCustCategory').val() == 'Exist') {
                    if ($('#txtAdd1').val().length == 0 || $('#txtAdd2').val().length == 0)
                        msg += 'Enter Address <br />';
                    if ($('#ddlType option:selected').text() == "CHOOSE")
                        msg += "Choose customer type<br/>";
                    if ($('#txtZip').val().length == 0)
                        msg += 'Enter ZipCode <br />';
                    if ($('#txtEmail').val().length == 0)
                        msg += 'Enter E-Mail ID <br />';
                    if ($('#txtContact').val().length == 0)
                        msg += 'Enter Contact Person <br />';
                    if ($('#txtMobile').val().length == 0)
                        msg += 'Enter Mobile No. <br />';
                    else if ($('#txtMobile').val().length > 0) {
                        var value = $('#txtMobile').val().replace(/^\s\s*/, '').replace(/\s\s*$/, '');
                        if (!intPattern.test(value))
                            msg += "Mobile No. must be numeric.<br/>";
                    }
                }
                if ($('#hdnCustCategory').val() == 'Prospect') {
                    if ($('#txtPort').val().length == 0)
                        msg += 'Enter port<br/>';
                    if ($('#txtLeadSource').val().length == 0)
                        msg += 'Enter lead source<br/>';
                }
                if ($('#txtEmail').val().length > 0) {
                    if (emailpattern.test($('#txtEmail').val()) == false)
                        msg += 'Enter valid E-Mail ID <br />';
                }
                if ($('#hdnCountryName').val() == 'Add New Country') {
                    if ($('#txtNewCountry').val().length == 0)
                        msg += 'Enter new country name<br />';
                }
                if ($('#hdnCityName').val() == 'Add New City') {
                    if ($('#txtNewCity').val().length == 0)
                        msg += 'Enter new city name<br />';
                }
                if (msg.length > 0) {
                    $('#divErrMsg').html(msg);
                    return false;
                }
                else
                    return true;
            }
            else {
                $('#divErrMsg').html($('#divName').html());
                return false;
            }
        }

        function NewProspectAdd() {
            $('#hdnCustCategory').val('Prospect');
        }
    </script>
</asp:Content>
