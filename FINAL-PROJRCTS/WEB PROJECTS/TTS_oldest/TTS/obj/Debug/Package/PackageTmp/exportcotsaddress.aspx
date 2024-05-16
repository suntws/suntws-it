<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="exportcotsaddress.aspx.cs" Inherits="TTS.exportcotsaddress" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        <asp:Label runat="server" ID="lblAddressText" ClientIDMode="Static" Text=""></asp:Label>
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <div>
            <table cellspacing="0" rules="all" border="1" style="background-color: #ecf6ff; width: 100%;
                border-color: White;">
                <tr>
                    <td colspan="4" style="line-height: 15px;">
                        <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th class="spanCss">
                        Customer:
                    </th>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlCotsCustName" ClientIDMode="Static" Width="500px"
                            OnSelectedIndexChanged="ddlCotsCustName_IndexChange" AutoPostBack="true" CssClass="txtCss">
                        </asp:DropDownList>
                    </td>
                    <th class="spanCss">
                        User ID:
                    </th>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlLoginUserName" ClientIDMode="Static" Width="300px"
                            OnSelectedIndexChanged="ddlLoginUserName_IndexChange" AutoPostBack="true" CssClass="txtCss">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="billCtrlDiv" runat="server" clientidmode="Static">
                    <th class="spanCss">
                        Bill To:
                    </th>
                    <td colspan="3">
                        <asp:DropDownList runat="server" ID="ddlBillAddress" ClientIDMode="Static" Width="800px"
                            OnSelectedIndexChanged="ddlBillAddress_indexChange" AutoPostBack="true" CssClass="txtCss">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th class="spanCss">
                        Company Name<br />
                        <span style="font-size: 9px; float: left; line-height: 10px;">(For Invoice / Documents)</span>
                    </th>
                    <td>
                        <asp:TextBox runat="server" ID="txtCompanyName" ClientIDMode="Static" Text="" Width="500px"
                            CssClass="txtCss"></asp:TextBox>
                    </td>
                    <th class="spanCss">
                        Pincode
                    </th>
                    <td>
                        <asp:TextBox runat="server" ID="txtPincode" ClientIDMode="Static" Text="" Width="300px"
                            CssClass="txtCss"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th rowspan="3">
                        Address
                    </th>
                    <td rowspan="3">
                        <asp:TextBox runat="server" ID="txtAddress" ClientIDMode="Static" Text="" Width="300px"
                            TextMode="MultiLine" Height="70px" CssClass="txtCss"></asp:TextBox>
                    </td>
                    <th class="spanCss">
                        Attn :
                    </th>
                    <td>
                        <asp:TextBox runat="server" ID="txtAttn" ClientIDMode="Static" Text="" Width="300px"
                            CssClass="txtCss"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th class="spanCss">
                        Phone No.
                    </th>
                    <td>
                        <asp:TextBox runat="server" ID="txtPhone" ClientIDMode="Static" Text="" Width="300px"
                            CssClass="txtCss"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th class="spanCss">
                        Mobile No.
                    </th>
                    <td>
                        <asp:TextBox runat="server" ID="txtMobile" ClientIDMode="Static" Text="" Width="300px"
                            CssClass="txtCss"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th class="spanCss">
                        City
                    </th>
                    <td>
                        <asp:TextBox runat="server" ID="txtCity" ClientIDMode="Static" Text="" Width="300px"
                            CssClass="txtCss"></asp:TextBox>
                    </td>
                    <th class="spanCss">
                        Fax No.
                    </th>
                    <td>
                        <asp:TextBox runat="server" ID="txtFax" ClientIDMode="Static" Text="" Width="300px"
                            CssClass="txtCss"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th class="spanCss">
                        State
                    </th>
                    <td>
                        <asp:TextBox runat="server" ID="txtState" ClientIDMode="Static" Text="" Width="300px"
                            CssClass="txtCss"></asp:TextBox>
                    </td>
                    <th class="spanCss">
                        Email-ID
                    </th>
                    <td>
                        <asp:TextBox runat="server" ID="txtMail" ClientIDMode="Static" Text="" Width="300px"
                            CssClass="txtCss"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th class="spanCss">
                        Country
                    </th>
                    <td>
                        <asp:TextBox runat="server" ID="txtCountry" ClientIDMode="Static" Text="" Width="300px"
                            CssClass="txtCss"></asp:TextBox>
                    </td>
                    <td colspan="2" style="text-align: center;">
                        <asp:Button runat="server" ID="btnAddressSave" ClientIDMode="Static" Text="SAVE"
                            CssClass="btn btn-success" OnClientClick="javascript:return chkValidateAddress()"
                            OnClick="btnAddressSave_Click" />
                        <span class="btn btn-info" onclick="ctrlClearPage()">CLEAR</span>
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
                                    </div>
                                    <div>
                                        <asp:Label runat="server" ID="lblCountry" Text='<%# Eval("country")%>'></asp:Label>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Label runat="server" ID="lblZipcode" Text='<%# Eval("zipcode")%>'></asp:Label>
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
                                </div>
                            </ItemTemplate>
                        </asp:DataList>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdnCotsName" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnAddressID" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnBillID" ClientIDMode="Static" Value="0" />
    <asp:HiddenField runat="server" ID="hdnQueryType" ClientIDMode="Static" Value="0" />
    <asp:HiddenField runat="server" ID="hdnLoginName" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnCotsCustID" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        $(document).ready(function () {
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

            $(':text').bind('keydown', function (e) {
                if (e.keyCode == 13) {
                    e.preventDefault();
                    return false;
                }
            });
        });

        function chkValidateAddress() {
            var errmsg = '';
            var emailpattern = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
            $('#lblErrMsg').html('');
            if ($('#ddlCotsCustName option:selected').text() == 'Choose')
                errmsg += 'Choose customer full name<br/>';
            if ($('#ddlLoginUserName option:selected').text() == 'Choose')
                errmsg += 'Choose User Id<br/>';
            if ($('#hdnQueryType').val() == "1") {
                if ($('#ddlBillAddress option:selected').text() == 'Choose')
                    errmsg += 'Choose customer billing address<br/>';
            }
            if ($('#txtCompanyName').val().length == 0)
                errmsg += 'Enter Company Name<br/>';
            if ($('#txtCity').val().length == 0)
                errmsg += 'Enter city<br/>';
            if ($('#txtState').val().length == 0)
                errmsg += 'Enter State<br/>';
            if ($('#txtAttn').val().length == 0)
                errmsg += 'Enter Attn name<br/>';
            if ($('#txtMail').val().length > 0) {
                if (emailpattern.test($('#txtMail').val()) == false)
                    errmsg += 'Enter vaild email <br />';
            }
            if ($('#txtPhone').val().length == 0 && $('#txtMobile').val().length == 0)
                errmsg += 'Enter phone or mobile no.<br/>';
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
            $('#txtState').val('');
            $('#txtCountry').val('');
            $('#txtPincode').val('');
            $('#txtAttn').val('');
            $('#txtPhone').val('');
            $('#txtMobile').val('');
            $('#txtFax').val('');
            $('#txtMail').val('');
        }
    </script>
</asp:Content>
