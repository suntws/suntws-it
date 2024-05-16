<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="cotsdomesticaddress.aspx.cs" Inherits="TTS.cotsdomesticaddress" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .tableCust
        {
            border-collapse: collapse;
            border-color: #868282;
            width: 100%;
            line-height: 20px;
            background-color: #d4f3fd;
        }
        .tableCust th
        {
            text-align: left;
            padding-left: 10px;
            width: 130px;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        DOMESTIC CUSTOMER&nbsp;
        <asp:Label runat="server" ID="lblAddressText" ClientIDMode="Static" Text=""></asp:Label>
        &nbsp;ADDRESS LIST</div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" class="tableCust" style="line-height: 20px;">
            <tr>
                <td colspan="4" style="line-height: 15px;">
                    <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <th>
                    CUSTOMER NAME
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="ddlCotsCustName" ClientIDMode="Static" Width="300px"
                        OnSelectedIndexChanged="ddlCotsCustName_IndexChange" AutoPostBack="true" CssClass="form-control">
                    </asp:DropDownList>
                </td>
                <th>
                    USER ID
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="ddlLoginUserName" ClientIDMode="Static" Width="300px"
                        OnSelectedIndexChanged="ddlLoginUserName_IndexChange" AutoPostBack="true" CssClass="form-control">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <div id="billCtrlDiv" runat="server" clientidmode="Static">
                        <table>
                            <tr>
                                <th style="width: 127px;">
                                    BILL TO
                                </th>
                                <td colspan="3">
                                    <asp:DropDownList runat="server" ID="ddlBillAddress" ClientIDMode="Static" Width="800px"
                                        OnSelectedIndexChanged="ddlBillAddress_indexChange" AutoPostBack="true" CssClass="form-control">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <th>
                    COMPANY NAME <span style="font-size: 9px; float: left; line-height: 10px;">(For Invoice/Documents)</span>
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtCompanyName" ClientIDMode="Static" Text="" Width="300px"
                        CssClass="form-control"></asp:TextBox>
                </td>
                <th>
                    ATTN
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtAttn" ClientIDMode="Static" Text="" Width="300px"
                        CssClass="form-control"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th rowspan="3">
                    ADDRESS
                </th>
                <td rowspan="3">
                    <asp:TextBox runat="server" ID="txtAddress" ClientIDMode="Static" Text="" Width="300px"
                        TextMode="MultiLine" Height="70px" CssClass="form-control"></asp:TextBox>
                </td>
                <th>
                    MOBILE NO.
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtMobile" ClientIDMode="Static" Text="" Width="300px"
                        CssClass="form-control" MaxLength="12"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    PHONE NO.
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtPhone" ClientIDMode="Static" Text="" Width="300px"
                        CssClass="form-control" MaxLength="12"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    FAX NO.
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtFax" ClientIDMode="Static" Text="" Width="300px"
                        CssClass="form-control"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    CITY
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtCity" ClientIDMode="Static" Text="" Width="300px"
                        CssClass="form-control"></asp:TextBox>
                </td>
                <th>
                    EMAIL-ID
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtMail" ClientIDMode="Static" Text="" Width="300px"
                        CssClass="form-control"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    STATE
                </th>
                <td>
                    <%--<asp:TextBox runat="server" ID="txtState" ClientIDMode="Static" Text="" Width="300px"
                        CssClass="form-control"></asp:TextBox>--%>
                    <asp:DropDownList runat="server" ID="ddlState" ClientIDMode="Static" Width="300px"
                        CssClass="form-control">
                    </asp:DropDownList>
                </td>
                <td colspan="2" rowspan="5">
                    <div style="float: left; border: 1px solid #868282; line-height: 15px;" id="divGST"
                        runat="server" clientidmode="Static">
                        <table style="">
                            <tr>
                                <th style="text-align: center;">
                                    GST VALUE
                                </th>
                            </tr>
                            <tr>
                                <td style="background-color: #F1CED5;">
                                    <div style="float: left;">
                                        <div style="float: left; background-color: #ccc; width: 100px;">
                                            <asp:CheckBox runat="server" ID="chkCGST" ClientIDMode="Static" Checked="false" Text="CGST %" /></div>
                                        <div id="divCGST" style="display: none; float: left;">
                                            <div style="float: left;">
                                                <asp:TextBox runat="server" ID="txtCGST" ClientIDMode="Static" Text="" Width="60px"
                                                    onkeypress="return isNumberKey(event)" MaxLength="5" CssClass="form-control"></asp:TextBox></div>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="background-color: #BCE0B8;">
                                    <div style="float: left;">
                                        <div style="float: left; background-color: #ccc; width: 100px;">
                                            <asp:CheckBox runat="server" ID="chkSGST" ClientIDMode="Static" Checked="false" Text="SGST %" /></div>
                                        <div id="divSGST" style="display: none; float: left;">
                                            <div style="float: left;">
                                                <asp:TextBox runat="server" ID="txtSGST" ClientIDMode="Static" Text="" Width="60px"
                                                    onkeypress="return isNumberKey(event)" MaxLength="5" CssClass="form-control"></asp:TextBox></div>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="background-color: #D2C9E8;">
                                    <div style="float: left;">
                                        <div style="float: left; background-color: #ccc; width: 100px;">
                                            <asp:CheckBox runat="server" ID="chkIGST" ClientIDMode="Static" Checked="false" Text="IGST %" /></div>
                                        <div id="divIGST" style="display: none; width: 100px; float: left;">
                                            <asp:TextBox runat="server" ID="txtIGST" ClientIDMode="Static" Text="" Width="60px"
                                                onkeypress="return isNumberKey(event)" MaxLength="5" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <th>
                    STATE CODE
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtStateCode" ClientIDMode="Static" Text="" Width="300px"
                        CssClass="form-control" MaxLength="2" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    COUNTRY
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtCountry" ClientIDMode="Static" Text="" Width="300px"
                        CssClass="form-control"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    PINCODE
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtPincode" ClientIDMode="Static" Text="" Width="300px"
                        CssClass="form-control" MaxLength="6"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    GST NO
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtGSTNo" ClientIDMode="Static" Text="" Width="300px"
                        CssClass="form-control" MaxLength="15"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <div style="width: 1000px; float: left; text-align: center;">
                        <asp:Button runat="server" ID="btnAddressSave" ClientIDMode="Static" Text="SAVE"
                            CssClass="btnsave" OnClientClick="javascript:return chkValidateAddress()" OnClick="btnAddressSave_Click" />
                        <span class="btnclear" onclick="ctrlClearPage()">CLEAR</span>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:DataList runat="server" ID="dlAddressList" RepeatColumns="2" RepeatDirection="Horizontal"
                        RepeatLayout="Table" OnSelectedIndexChanged="dlAddressList_SelectedIndexChanged"
                        AlternatingItemStyle-BackColor="#A1C9D3" ItemStyle-BackColor="#C9DAC9" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <div style="width: 500px; float: left; line-height: 18px;">
                                <div>
                                    <asp:Label runat="server" ID="lblCompanyName" Text='<%# Eval("CompanyName") %>' Font-Bold="true"></asp:Label>
                                </div>
                                <div>
                                    <asp:Label runat="server" ID="lblShippAddress" Text='<%# DataBinder.Eval(Container.DataItem, "shipaddress").ToString().Replace("~", "<br>")%>'></asp:Label>
                                </div>
                                <div>
                                    <asp:Label runat="server" ID="lblCity" Text='<%# Eval("city")%>'></asp:Label>
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Label runat="server" ID="lblState" Text='<%# Eval("statename")%>'></asp:Label>
                                    (<asp:Label runat="server" ID="lblStateCode" Text='<%# Eval("stateCode")%>'></asp:Label>)
                                </div>
                                <div>
                                    <asp:Label runat="server" ID="lblCountry" Text='<%# Eval("country")%>'></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label runat="server" ID="lblZipcode" Text='<%# Eval("zipcode")%>'></asp:Label>
                                    <br />
                                    <%# Eval("GST_No").ToString() != "" ? "GST NO: " : ""%>
                                    <asp:Label runat="server" ID="lblGSTNo" Text='<%# Eval("GST_No")%>'></asp:Label>
                                </div>
                                <div style="padding-top: 10px;">
                                    <asp:Label runat="server" ID="lblContact" Text='<%# Eval("contact_name")%>'></asp:Label>
                                </div>
                                <div>
                                    <asp:Label runat="server" ID="lblPhone" Text='<%# Eval("PhoneNo")%>'></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label runat="server" ID="lblmobile" Text='<%# Eval("mobile")%>'></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label runat="server" ID="lblfax" Text='<%# Eval("fax")%>'></asp:Label>
                                </div>
                                <div>
                                    <asp:Label runat="server" ID="lblEmail" Text='<%# Eval("EmailID")%>'></asp:Label>
                                    <span style="width: 120px; float: right;">
                                        <asp:HiddenField runat="server" ID="hdnSelectID" Value='<%# Eval("ID")%>' />
                                        <asp:LinkButton runat="server" ID="lnkDLAddress" Text="Edit Address Details" CommandName="Select"></asp:LinkButton></span>
                                </div>
                                <div style="width: 500px; float: left; padding-top: 5px; background-color: #E9B8B8;">
                                    <div style="width: 120px; float: left;">
                                        <%# Convert.ToString(DataBinder.Eval(Container.DataItem, "CGST").ToString()) != "0.00" ? "CGST % " : ""%>
                                        <asp:Label runat="server" ID="lblCGST" Text='<%# DataBinder.Eval(Container.DataItem, "CGST").ToString() !="0.00"? DataBinder.Eval(Container.DataItem, "CGST").ToString():"" %>'></asp:Label></div>
                                    <div style="width: 120px; float: left;">
                                        <%# Convert.ToString(DataBinder.Eval(Container.DataItem, "SGST").ToString()) != "0.00" ? "SGST % " : ""%>
                                        <asp:Label runat="server" ID="lblSGST" Text='<%# DataBinder.Eval(Container.DataItem, "SGST").ToString() !="0.00"? DataBinder.Eval(Container.DataItem, "SGST").ToString():"" %>'></asp:Label></div>
                                    <div style="width: 120px; float: left;">
                                        <%# Convert.ToString(DataBinder.Eval(Container.DataItem, "IGST").ToString()) != "0.00" ? "IGST % " : ""%>
                                        <asp:Label runat="server" ID="lblIGST" Text='<%# DataBinder.Eval(Container.DataItem, "IGST").ToString() !="0.00"? DataBinder.Eval(Container.DataItem, "IGST").ToString():"" %>'></asp:Label></div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:DataList>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnCotsName" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnAddressID" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnBillID" ClientIDMode="Static" Value="0" />
    <asp:HiddenField runat="server" ID="hdnQueryType" ClientIDMode="Static" Value="0" />
    <asp:HiddenField runat="server" ID="hdnLoginName" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnCotsCustID" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        $(document).ready(function () {
            $("input:checkbox[id*=chk]").click(function (e) {
                var ctrlID = e.target.id;
                if (ctrlID == "chkCGST")
                    chktxtEnableDisable('chkCGST', 'divCGST', 'txtCGST');
                if (ctrlID == "chkSGST")
                    chktxtEnableDisable('chkSGST', 'divSGST', 'txtSGST');
                if (ctrlID == "chkIGST")
                    chktxtEnableDisable('chkIGST', 'divIGST', 'txtIGST');
            });

            $('#ddlCotsCustName').change(function () {
                $('#hdnCotsName').val($('#ddlCotsCustName option:selected').text());
                AllCtrlMakeEmpty();
            });
            $('#ddlLoginUserName').change(function () {
                $('#hdnLoginName').val($('#ddlLoginUserName option:selected').text());
                $('#hdnCotsCustID').val($('#ddlLoginUserName option:selected').val());
                AllCtrlMakeEmpty();
            });
            $('#ddlBillAddress').change(function () {
                $('#hdnBillID').val($('#ddlBillAddress option:selected').val());
                AllCtrlMakeEmpty();
            });
            $('#ddlState').change(function () {
                $('#txtStateCode').val($('#ddlState option:selected').val());
            });

            $(':text').bind('keydown', function (e) {
                if (e.keyCode == 13) {
                    e.preventDefault();
                    return false;
                }
            });
        });

        function chktxtEnableDisable(chkID, divID, txtID) {
            if ($('#' + chkID).attr('checked') == "checked") {
                $('#' + divID).css({ 'display': 'block' });
                $('#' + txtID).focus();
            }
            else
                $('#' + divID).css({ 'display': 'none' });
        }

        function chkValidateAddress() {
            var errmsg = '';
            var emailpattern = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
            var numericPattern = /^[0-9]*$/;
            var gstPattern = /^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$/;
            $('#lblErrMsg').html('');
            if ($('#ddlCotsCustName option:selected').text() == 'Choose')
                errmsg += 'Choose customer full name<br/>';
            if ($('#hdnQueryType').val() == "1") {
                if ($('#ddlBillAddress option:selected').text() == 'Choose')
                    errmsg += 'Choose customer billing address<br/>';
            }
            if ($('#txtCompanyName').val().length == 0)
                errmsg += 'Enter Company Name<br/>';
            if ($('#txtCity').val().length == 0)
                errmsg += 'Enter city<br/>';
            if ($('#ddlState option:selected').text() == 'Choose')
                errmsg += 'Choose State<br/>';
            if ($('#txtStateCode').val().length == 0)
                errmsg += 'Enter State Code<br/>';

            if ($('#txtPincode').val().length == 0)
                errmsg += 'Enter Pincode<br/>';
            else if ($('#txtPincode').val().length < 6)
                errmsg += 'Enter Pincode in six digit<br/>';
            else if ($('#txtPincode').val().length == 6) {
                if (numericPattern.test($('#txtPincode').val()) == false)
                    errmsg += 'Enter only numeric in pincode box (000000) <br/>';
            }

            if ($('#txtGSTNo').val().length == 0)
                errmsg += 'Enter GST No<br/>';
            else if ($('#txtGSTNo').val() != 'NA' && $('#txtGSTNo').val().length != 15)
                errmsg += 'GST No length should be 15 digit<br/>';
            else if ($('#txtGSTNo').val().length == 15) {
                if (gstPattern.test($('#txtGSTNo').val()) == false)
                    errmsg += 'Enter valid Gst no in the box ([0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}) <br/>';
            }
            if ($('#txtAttn').val().length == 0)
                errmsg += 'Enter Attn name<br/>';
            if ($('#txtMail').val().length > 0) {
                if (emailpattern.test($('#txtMail').val()) == false)
                    errmsg += 'Enter vaild email <br />';
            }
            if ($('#txtMobile').val().length == 0)
                errmsg += 'Enter mobile no.<br/>';
            else if ($('#txtMobile').val().length > 0) {
                if ($('#txtMobile').val().length < 10)
                    errmsg += 'Enter Mobile no in minimum 10 digit<br/>';
                else if ($('#txtMobile').val().length >= 10) {
                    if (numericPattern.test($('#txtMobile').val()) == false)
                        errmsg += 'Enter only numeric in mobile no box (0-9) <br/>';
                }
            }
            if ($('#txtPhone').val().length > 0) {
                if ($('#txtPhone').val().length < 10)
                    errmsg += 'Enter Phone no in minimum 10 digit<br/>';
                else if ($('#txtPhone').val().length >= 10) {
                    if (numericPattern.test($('#txtPhone').val()) == false)
                        errmsg += 'Enter only numeric in phone no box (0-9) <br/>';
                }
            }

            if ($('#chkCGST').attr('checked') == 'checked' && $('#txtCGST').val().length == 0)
                errmsg += 'Enter CGST %<br />';
            if ($('#chkSGST').attr('checked') == 'checked' && $('#txtSGST').val().length == 0)
                errmsg += 'Enter SGST %<br />';
            if ($('#chkIGST').attr('checked') == 'checked' && $('#txtIGST').val().length == 0)
                errmsg += 'Enter IGST %<br />';
            if (errmsg.length > 0) {
                $('#lblErrMsg').html(errmsg);
                return false;
            }
            else
                return true;
        }

        function ctrlClearPage() {
            window.location.href = window.location.href;
        }

        function AllCtrlMakeEmpty() {
            $('#txtCompanyName').val('');
            $('#txtCompanyName').attr('disabled', false);
            $('#txtAddress').val('');
            $('#txtCity').val('');
            $('#ddlState option[value="Choose"]').attr("selected", "selected");
            $('#txtStateCode').val('');
            $('#txtGSTNo').val('');
            $('#txtCountry').val('');
            $('#txtPincode').val('');
            $('#txtAttn').val('');
            $('#txtPhone').val('');
            $('#txtMobile').val('');
            $('#txtFax').val('');
            $('#txtMail').val('');
            $('#txtCGST').val('');
            $('#txtSGST').val('');
            $('#txtIGST').val('');
            $('#hdnAddressID').val('');
            $('#chkCGST').attr('checked', false);
            $('#chkIGST').attr('checked', false);
            $('#chkSGST').attr('checked', false);
            $('#divCGST').css({ 'display': 'none' });
            $('#divIGST').css({ 'display': 'none' });
            $('#divSGST').css({ 'display': 'none' });
        }
    </script>
</asp:Content>
