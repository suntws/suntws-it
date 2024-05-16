<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="cotsftregister.aspx.cs" Inherits="TTS.cotsftregister" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .tableReq
        {
            border-collapse: collapse;
            border-color: #000;
            width: 1070px;
            line-height: 22px;
            margin-top: 5px;
        }
        .tableReq td:nth-child(odd)
        {
            width: 300px;
            background-color: #BFD7EA;
            text-align: left;
            padding-left: 10px;
            font-weight: bold;
        }
        .tableReq input[type="text"], select
        {
            background-color: #FAF5FB;
            border: 1px solid #000;
            margin-left: 10px;
        }
    </style>
    <link href="Styles/datemonthyear.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        <asp:Label runat="server" ID="lblHeadPlant" Text=""></asp:Label>
        FITMENT ORDER RECEIVED ENTRY</div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <div>
            <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #000;
                width: 1070px; line-height: 20px; margin-top: 5px;">
                <tr>
                    <td>
                        <table cellspacing="0" rules="all" border="1" class="tableReq">
                            <tr>
                                <td>
                                    CUSTOMER NAME
                                </td>
                                <td style="width: 410px;">
                                    <asp:DropDownList runat="server" ID="ddlCotsCustName" ClientIDMode="Static" Width="400px"
                                        OnSelectedIndexChanged="ddlCotsCustName_IndexChange" AutoPostBack="true" Font-Bold="true">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    USER ID
                                </td>
                                <td style="width: 250px;">
                                    <asp:DropDownList runat="server" ID="ddlLoginUserName" ClientIDMode="Static" Width="230px"
                                        OnSelectedIndexChanged="ddlLoginUserName_IndexChange" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    BILLING ADDRESS
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList runat="server" ID="ddlBillingAddress" ClientIDMode="Static" Width="800px"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlBillingAddress_IndexChange">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    GATE PASS / CHALLAN NO
                                </td>
                                <td>
                                    <asp:TextBox ID="txtGatePassNo" runat="server" ClientIDMode="Static" Width="300px"
                                        MaxLength="50" Text=""></asp:TextBox>
                                </td>
                                <td colspan="2" rowspan="4" style="background-color: #fff; vertical-align: top;">
                                    <asp:Label runat="server" ID="lblBillingAddress" ClientIDMode="Static" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    GATE PASS / CHALLAN DATE
                                </td>
                                <td>
                                    <asp:TextBox ID="txtGatePassDate" runat="server" ClientIDMode="Static" Width="80px"
                                        Text=""></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    GST NO
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTin" runat="server" ClientIDMode="Static" Width="150px" MaxLength="50"
                                        Text=""></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    CUSTOMER PAN NO
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPan" runat="server" ClientIDMode="Static" Width="150px" MaxLength="10"
                                        Text=""></asp:TextBox>
                                    <asp:Label runat="server" ID="lblerr" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #CE8686;
                            width: 1068px;">
                            <tr align="center">
                                <td style="width: 310px;">
                                    TYRE SIZE<asp:DropDownList runat="server" ID="ddlTyreSize" ClientIDMode="Static"
                                        Width="230px">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 70px;">
                                    QTY<asp:TextBox runat="server" ID="txtQty" ClientIDMode="Static" Text="" Width="30px"
                                        MaxLength="4" onkeypress="return isNumberWithoutDecimal(event)"></asp:TextBox>
                                </td>
                                <td style="width: 160px; text-align: center;">
                                    <div style="float: left; width: 110px; text-align: center;">
                                        <asp:Button ID="btnAddNextItem" runat="server" ClientIDMode="Static" Text="ADD NEXT ITEM"
                                            CssClass="btnactive" OnClientClick="javascript:return CtrlAddNextItem();" OnClick="btnAddNextItem_Click" />
                                    </div>
                                </td>
                                <td style="width: 500px; text-align: left; line-height: 15px;">
                                    <asp:Label runat="server" ID="lblErrMsg1" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="padding-left: 300px;">
                        <asp:GridView runat="server" ID="gvFtList" AutoGenerateColumns="false" Width="400px"
                            RowStyle-Height="20px" OnRowDeleting="gvFtList_RowDeleting" OnRowUpdating="gvFtList_RowUpdating"
                            OnRowCancelingEdit="gvFtList_RowCanceling" OnRowEditing="gvFtList_RowEditing">
                            <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" Height="22px" />
                            <Columns>
                                <asp:TemplateField HeaderText="TYRE SIZE" ItemStyle-Width="200px">
                                    <ItemTemplate>
                                        <asp:Label ID="lbltyresize" runat="server" ClientIDMode="Static" Text='<%#Eval("tyresize")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="QTY" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <%#Eval("itemqty")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <div style="background-color: #EEFD2D; border-bottom: #000; color: #000;">
                                            <asp:Label runat="server" ID="lblGItemQty" Text='<%#Eval("itemqty") %>'></asp:Label></div>
                                        <asp:TextBox runat="server" ID="txtGItemQty" onkeypress="return isNumberWithoutDecimal(event)"
                                            Width="50px" MaxLength="5" BackColor="#f9c232" Text='<%# Eval("itemqty") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ACTION" ItemStyle-Width="150px">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="lnkEdit" Text="EDIT" CommandName="Edit"></asp:LinkButton>
                                        <asp:LinkButton runat="server" ID="lnkDelete" Text="DELETE" CommandName="Delete"></asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Button runat="server" ID="btnUpdate" Text="Update" CommandName="Update" />
                                        <asp:Button runat="server" ID="btnCancel" Text="Cancel" CommandName="Cancel" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <div style="float: left; width: 240px; text-align: right;">
                            <asp:Label runat="server" ID="lblTotQTy" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div runat="server" clientidmode="Static" id="statusChangeDiv">
                            <div style="width: 1050px; float: left; padding-left: 15px;">
                                <span class="headCss">ANY COMMENTS / INSTRUCTION:</span>
                                <asp:TextBox runat="server" ID="txtComments" ClientIDMode="Static" Text="" TextMode="MultiLine"
                                    onKeyUp="javascript:CheckMaxLength(this, 999);" onChange="javascript:CheckMaxLength(this, 999);"
                                    Width="1000px" Height="60px"></asp:TextBox>
                            </div>
                            <div style="width: 1050px; float: left; text-align: center;">
                                <asp:Label runat="server" ID="lblerrmsg" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label>
                                <asp:Button runat="server" ID="btnSave" ClientIDMode="Static" CssClass="btnauthorize"
                                    Text="SAVE FITMENT ORDER" OnClientClick="javascript:return CtrlValidation();"
                                    OnClick="btnSave_Click" />
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <asp:HiddenField runat="server" ID="hdnCotsCustID" ClientIDMode="Static" Value="" />
        <asp:HiddenField runat="server" ID="hdnFullName" ClientIDMode="Static" Value="" />
        <asp:HiddenField runat="server" ID="hdnBillID" ClientIDMode="Static" Value="" />
        <asp:HiddenField runat="server" ID="hdntotalqty" ClientIDMode="Static" Value="" />
    </div>
    <script src="Scripts/datemonthyear.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#txtGatePassDate").datepicker({
                minDate: "-30D", maxDate: "+0D"
            }).keydown(function (e) {
                e.keyWhich = 0; e.keyCode = 0; e.preventDefault();
            });
            $("#ddlCotsCustName").change(function () {
                $('#hdnFullName').val($('#ddlCotsCustName option:selected').text());
            });
            $('#ddlLoginUserName').change(function () {
                $('#hdnCotsCustID').val($('#ddlLoginUserName option:selected').val());
            });
            $('#ddlBillingAddress').change(function () {
                $('#hdnBillID').val($('#ddlBillingAddress option:selected').val());
            });
            $('#txtPan').blur(function () {
                var err = '';
                $('#lblerr').html('');
                if ($(this).val().length == 10) {
                    var panPat = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;
                    if (panPat.test($(this).val()) == false)
                        err = 'Please enter valid pan number';
                }
                else if ($(this).val().length > 0 && $(this).val().length != 10)
                    err = 'Please enter the correct pan no';
                if (err.length > 0) {
                    $('#lblerr').html(err);
                    $(this).focus();
                }
            });
        });

        function CtrlValidation() {
            var errMsg = ''; $('#lblerrmsg').html('');
            if ($('#txtComments').val().length == 0)
                errMsg += 'Enter any comments / instruction<br/>';
            if (errMsg.length > 0) {
                $('#lblerrmsg').html(errMsg);
                return false;
            }
            else
                return true;
        }
        function CtrlAddNextItem() {
            var errMsg = ''; $('#lblErrMsg1').html('');
            if ($('#ddlCotsCustName option:selected').text() == 'Choose')
                errMsg += 'Choose customer name<br/>';
            if ($('#ddlLoginUserName option:selected').text() == 'Choose')
                errMsg += 'Choose user id<br/>';
            if ($('#ddlBillingAddress option:selected').text() == 'Choose')
                errMsg += 'Choose billing address<br/>';
            if ($('#txtGatePassNo').val().length == 0)
                errMsg += 'Enter gate pass no / challan no<br/>';
            if ($('#txtGatePassDate').val().length == 0)
                errMsg += 'Enter gate pass / challan date<br/>';
            if ($('#txtTin').val().length == 0)
                errMsg += 'Enter GST No<br/>';
            if ($('#ddlTyreSize option:selected').text() == 'Choose')
                errMsg += 'Choose tyre size<br/>';
            if ($('#txtQty').val().length == 0)
                errMsg += 'Enter item qty<br/>';
            else if (parseInt($('#txtQty').val()) == 0)
                errMsg += 'Enter correct qty<br/>';
            if (errMsg.length > 0) {
                $('#lblErrMsg1').html(errMsg);
                return false;
            }
            else
                return true;
        }
        function bind_errmsg(strErr) {
            $('#lblErrMsg1').html(strErr);
        }
    </script>
</asp:Content>
