<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="claimregister.aspx.cs" Inherits="COTS.claimregister" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="styles/uploadify.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div align="center" class="pageTitleHead">
        CLAIM REGISTER
    </div>
    <div class="contPage">
        <div>
            <table>
                <tr>
                    <td>
                        <asp:GridView runat="server" ID="gvClaimList" AutoGenerateColumns="false" Width="1180px"
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
                                        <asp:DropDownList runat="server" ID="ddledbrand" ClientIDMode="Static">
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="SIZE" ItemStyle-Width="150px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTyreSize" runat="server" Text='<%# Eval("TyreSize") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:HiddenField ID="lbledTyreSize" runat="server" Value='<%# Eval("TyreSize") %>' />
                                        <asp:DropDownList runat="server" ID="ddledTyreSize" ClientIDMode="Static">
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="TYPE" ItemStyle-Width="150px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTyreType" runat="server" Text='<%# Eval("TyreType") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:HiddenField ID="lbledTyreType" runat="server" Value='<%# Eval("TyreType") %>' />
                                        <asp:DropDownList runat="server" ID="ddledTyreType" ClientIDMode="Static">
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
                                            MaxLength="12" CssClass="claimtxt" Width="100px">></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="COMPLAINT" ItemStyle-Width="300px">
                                    <ItemTemplate>
                                        <%#((string)Eval("AppStyle")).Replace("~", "<br/>")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox runat="server" ID="txtAppStyle" Text='<%#((string)Eval("AppStyle")).Replace("~", "\r\n")%>'
                                            ClientIDMode="Static" Width="300px" CssClass="claimtxt" Height="70px" TextMode="MultiLine"
                                            onKeyUp="javascript:CheckMaxLength(this, 999);" onChange="javascript:CheckMaxLength(this, 999);"></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="OPERATING CONDITION" ItemStyle-Width="300px">
                                    <ItemTemplate>
                                        <%#((string)Eval("RunningHours")).Replace("~", "<br/>")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox runat="server" ID="txtRunningHours" Text='<%#((string)Eval("RunningHours")).Replace("~", "\r\n")%>'
                                            ClientIDMode="Static" Width="300px" CssClass="claimtxt" Height="70px" TextMode="MultiLine"
                                            onKeyUp="javascript:CheckMaxLength(this, 999);" onChange="javascript:CheckMaxLength(this, 999);"></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ACTION" ItemStyle-Width="120px">
                                    <ItemTemplate>
                                        <asp:Button runat="server" ID="btnEdit" Text="Edit" CommandName="Edit" />
                                        <asp:Button runat="server" ID="btnDelete" Text="Delete" CommandName="Delete" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblErrmsg1" runat="server" ClientIDMode="Static" Text="" ForeColor="Red"
                                            Font-Bold="true"></asp:Label>
                                        <asp:Button runat="server" ID="btnUpdate" Text="Update" CommandName="Update" OnClientClick="return validateedit(this);"
                                            CausesValidation="true" />
                                        <asp:Button runat="server" ID="btnCancel" Text="Cancel" CommandName="Cancel" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #07A714;
                            margin-top: 10px; line-height: 25px; width: 1180px;">
                            <tr>
                                <td style="width: 100px; text-align: center;">
                                    <div class="headCss" style="background: #D8D6D6;">
                                        Brand
                                        <asp:Label ID="lblmdbrand" runat="server" Text="*" ForeColor="Red" Font-Bold="true"></asp:Label></div>
                                    <asp:DropDownList runat="server" ID="ddlClaimBrand" ClientIDMode="Static" Width="100px"
                                        CssClass="claimtxt">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 220px; text-align: center;">
                                    <div class="headCss" style="background: #D8D6D6;">
                                        Tyre Size
                                        <asp:Label ID="lblmdtyresize" runat="server" Text="*" ForeColor="Red" Font-Bold="true"></asp:Label></div>
                                    <asp:DropDownList runat="server" ID="ddlClaimSize" ClientIDMode="Static" Width="220px"
                                        CssClass="claimtxt">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 80px; text-align: center;">
                                    <div class="headCss" style="background: #D8D6D6;">
                                        Tyre Type</div>
                                    <asp:DropDownList runat="server" ID="ddlTyreType" ClientIDMode="Static" Width="80px"
                                        CssClass="claimtxt">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 100px; text-align: center;">
                                    <div class="headCss" style="background: #D8D6D6;">
                                        Stencil No.
                                        <asp:Label ID="lblmdstencilno" runat="server" Text="*" ForeColor="Red" Font-Bold="true"></asp:Label></div>
                                    <asp:TextBox runat="server" ID="txtStencilNos" ClientIDMode="Static" MaxLength="12"
                                        CssClass="claimtxt" Width="100px"></asp:TextBox>
                                </td>
                                <td rowspan="3" style="width: 510px; vertical-align: top;">
                                    <div style="line-height: 15px; width: 500px; float: left;">
                                        <span style="color: #f00;">Note: </span>Each image file should be less then 5MB,
                                        Upload one image file for stencil no. and maximum three images for failure.
                                    </div>
                                    <div style="width: 230px; float: left;">
                                        <span style="width: 90px; float: left;">Stencil Image :</span>
                                        <div style="padding: 1px; width: 175px; float: left;">
                                            <asp:FileUpload ID="FileUpload1" runat="server" />
                                        </div>
                                        <%--<a href="javascript:$('#<%=FileUpload1.ClientID%>').fileUploadStart()">Upload Stencil
                                            No. Images</a>--%>
                                    </div>
                                    <div style="width: 230px; float: left;">
                                        <span style="width: 90px; float: left;">Failure Image :</span>
                                        <div style="padding: 1px; width: 175px; float: left;">
                                            <asp:FileUpload ID="FileUpload2" runat="server" />
                                        </div>
                                        <%--<a href="javascript:$('#<%=FileUpload2.ClientID%>').fileUploadStart()">Upload Failure
                                            Images</a>--%>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <div style="line-height: 15px; width: 500px; float: right;">
                                        <span style="color: #f00;">Note: </span>Stencil no. available on the sides of tyre
                                        starts with 'C' or 'L' or 'P' with a digits.
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <div style="width: 648px; float: left; border-bottom: 1px solid #07A714; padding-bottom: 5px;">
                                        <span class="headCss" style="width: 120px; float: left;">Complaint
                                            <asp:Label ID="lblmdcomplaint" runat="server" Text="*" ForeColor="Red" Font-Bold="true"></asp:Label></span>
                                        <span style="width: 520px; float: left;">
                                            <asp:TextBox runat="server" ID="txtClaimApplication" ClientIDMode="Static" Width="520px"
                                                CssClass="claimtxt" Height="70px" TextMode="MultiLine" onKeyUp="javascript:CheckMaxLength(this, 999);"
                                                onChange="javascript:CheckMaxLength(this, 999);"></asp:TextBox>
                                        </span>
                                    </div>
                                    <div style="width: 640px; float: left; padding-bottom: 5px;">
                                        <span class="headCss" style="width: 120px; float: left;">Operating Condition</span>
                                        <span style="width: 520px; float: left;">
                                            <asp:TextBox runat="server" ID="txtClaimRunning" ClientIDMode="Static" Width="520px"
                                                CssClass="claimtxt" Height="70px" TextMode="MultiLine" onKeyUp="javascript:CheckMaxLength(this, 999);"
                                                onChange="javascript:CheckMaxLength(this, 999);"></asp:TextBox>
                                        </span>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <div style="text-align: center; width: 1180px; float: left;">
                                        If you have another tyre failure
                                        <asp:Button runat="server" ID="btnAddMore" ClientIDMode="Static" Text="Add Next"
                                            CssClass="btnSave" OnClientClick="javascript:return CtrlClaimValidate();" OnClick="btnAddMore_Click" />
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
                                    <div style="width: 1180px; float: left; line-height: 20px;">
                                        <span class="headCss" style="width: 90px; float: left;">Any Comments</span>
                                        <asp:TextBox runat="server" ID="txtClaimComments" ClientIDMode="Static" Width="1070px"
                                            Height="60px" TextMode="MultiLine" onKeyUp="javascript:CheckMaxLength(this, 3999);"
                                            onChange="javascript:CheckMaxLength(this, 3999);" CssClass="claimtxt"></asp:TextBox>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" style="text-align: right; padding-right: 10px;">
                                    <span class="headCss">Ref No: </span><span style="color: #000; font-size: 9px;">(INVOICE
                                        / PO)</span>
                                    <asp:TextBox runat="server" ID="txtCustomerRefNo" ClientIDMode="Static" Text="" Width="400px"
                                        CssClass="claimtxt" MaxLength="100"></asp:TextBox>
                                </td>
                                <td colspan="1" style="text-align: left; padding-left: 10px;">
                                    <asp:Button runat="server" ID="btnSendClaimList" ClientIDMode="Static" Text="SEND CLAIM LIST"
                                        CssClass="btnAuthorize" OnClick="btnSendClaimList_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdnBrand" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnTyreSize" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnTyreType" ClientIDMode="Static" Value="" />
    <script src="scripts/uploadify.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#ddlClaimBrand').change(function () {
                $('#hdnBrand').val(''); $('#ddlClaimSize').html('');
                var strBrand = $("#ddlClaimBrand option:selected").text();
                if (strBrand != "Choose") {
                    $.ajax({ type: "POST", url: "bindvalues.aspx?type=getClaimSize&Brand=" + strBrand + "", context: document.body,
                        success: function (data) {
                            if (data != '') {
                                $('#ddlClaimSize').html(data);
                                $('#hdnBrand').val(strBrand);
                            }
                        }
                    });
                }
            });

            $('#ddlClaimSize').change(function () {
                $('#hdnTyreSize').val('');
                var strSize = $("#ddlClaimSize option:selected").text();
                var strBrand = $("#ddlClaimBrand option:selected").text();
                if (strSize != "Choose") {
                    $.ajax({ type: "POST", url: "bindvalues.aspx?type=getClaimType&Brand=" + strBrand + "&Size=" + strSize + "", context: document.body,
                        success: function (data) {
                            if (data != '') {
                                $('#ddlTyreType').html(data);
                                $('#hdnTyreSize').val(strSize);
                            }
                        }
                    });
                }
            });
            $('#ddlTyreType').change(function () {
                $('#hdnTyreType').val('');
                var strType = $("#ddlTyreType option:selected").text();
                if (strType != "Choose")
                    $('#hdnTyreType').val(strType);
            });
            $('#ddledbrand').change(function () {
                var strBrand = $("#ddledbrand option:selected").text();
                $.ajax({ type: "POST", url: "bindvalues.aspx?type=getClaimSize&Brand=" + strBrand + "", context: document.body,
                    success: function (data) {
                        if (data != '') {
                            $('#ddledTyreSize').html(data);
                            $('#hdnBrand').val(strBrand);
                        }
                    }
                });
            });
            $('#ddledTyreSize').change(function () {
                $('#hdnTyreSize').val('');
                var strSize = $("#ddledTyreSize option:selected").text();
                var strBrand = $("#ddledbrand option:selected").text();
                if (strSize != "Choose") {
                    $.ajax({ type: "POST", url: "bindvalues.aspx?type=getClaimType&Brand=" + strBrand + "&Size=" + strSize + "", context: document.body,
                        success: function (data) {
                            if (data != '') {
                                $('#ddledTyreType').html(data);
                                $('#hdnTyreSize').val(strSize);
                            }
                        }
                    });
                }
            });
            $('#ddledTyreType').change(function () {
                $('#hdnTyreType').val('');
                var strType = $("#ddledTyreType option:selected").text();
                if (strType != "Choose")
                    $('#hdnTyreType').val(strType);
            });
            $('#txtStencilNos').blur(function () {
                var strstencil = $("#txtStencilNos").val();
                var char = strstencil.charAt(0);
                var firstchar = char.toUpperCase();
                if (firstchar == 'C' || firstchar == 'L' || firstchar == 'P') {
                    if (strstencil.length > 9) {
                        $.ajax({ type: "POST", url: "bindvalues.aspx?type=chkClaimstencil&claimstencilno=" + strstencil,
                            success: function (data) {
                                if (data != '') { alert(data + ' '+$("#txtStencilNos").val();); $("#txtStencilNos").val(''); }
                            }
                        });
                    }
                }
            });
            $('#txtStencil').blur(function () {
                var strstencil = $("#txtStencil").val();
                var char = strstencil.charAt(0);
                var firstchar = char.toUpperCase();
                if (firstchar == 'C' || firstchar == 'L' || firstchar == 'P') {
                    if (strstencil.length > 9) {
                        $.ajax({ type: "POST", url: "bindvalues.aspx?type=chkClaimstencil&claimstencilno=" + strstencil,
                            success: function (data) {
                                if (data != '') { alert(data+' ' +$("#txtStencil").val();); $("#txtStencil").val(''); }
                            }
                        });
                    }
                }
            });
        });
        function validateedit(lnk) {
            var row = lnk.parentNode.parentNode;
            var rowIndex = row.rowIndex - 1;
            var errMsg = ''; $('#lblErrMsg1').html('');
            if ($('#hdnBrand').val().length == 0 || $('#ddledbrand option:selected').text() == 'Choose')
                errMsg += 'Choose Brand<br/>';
            if ($('#hdnTyreSize').val().length == 0 || $('#ddledTyreSize option:selected').text() == 'Choose')
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
            else {
                return true;
            }
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
            if ($('#txtClaimApplication').val().length == 0)
                errMsg += 'Enter Complaint<br/>';
            if (errMsg.length > 0) {
                $('#lblErrmsg').html(errMsg);
                return false;
            }
            else {
                return true;
            }
        }

        var uploadedfiles = 0;
        var maxQueueSize = 3;
        var queueSize = 0;
        $(window).load(function () {
            $("#<%=FileUpload1.ClientID%>").fileUpload({
                'uploader': 'scripts/uploader.swf',
                'cancelImg': 'images/cancel.png',
                'buttonText': 'Choose Image',
                'script': 'uploadstencil.ashx',
                'folder': 'xml',
                'fileDesc': 'Image Files',
                'fileExt': '*.jpg;*.jpeg;*.gif;*.png',
                'multi': false,
                'auto': true,
                'sizeLimit': ((1024 * 1024) * 5), //1 MB
                'removeCompleted': false,
                onError: function (a, b, c, d) {
                    if (d.type === "File Size") {
                        $("#err").html('File: ' + c.name + ' Maximum ' + d.type + ' Limit: ' + Math.round(d.sizeLimit / 1024) + 'KB');
                    }
                },
                onCancel: function (a, b, c, d) {
                    $.ajax({ type: "POST", url: "bindvalues.aspx?type=delClaimImg&claimimgname=" + c.name,
                        success: function (data) {
                            if (data == '') { alert('Image deleted successfully'); }
                        }
                    });
                }
            });

            $("#<%=FileUpload2.ClientID%>").fileUpload({
                'uploader': 'scripts/uploader.swf',
                'cancelImg': 'images/cancel.png',
                'buttonText': 'Choose Images',
                'script': 'uploadhandler.ashx',
                'folder': 'xml',
                'fileDesc': 'Image Files',
                'fileExt': '*.jpg;*.jpeg;*.gif;*.png',
                'queueSizeLimit': 3,
                'multi': true,
                'auto': true,
                'sizeLimit': ((1024 * 1024) * 5), //1 MB
                onError: function (a, b, c, d) {
                    alert(d.type);
                    if (d.type === "File Size") {
                        $("#err").html('File: ' + c.name + ' Maximum ' + d.type + ' Limit: ' + Math.round(d.sizeLimit / 1024) + 'KB');
                    }
                },
                'onSelect': function (event, queueID) {
                    if (queueSize < maxQueueSize) {
                        queueSize++;
                        uploadedfiles++;
                    }
                    else {
                        return false;
                    }
                },
                'onCancel': function (a, b, c, d) {
                    queueSize--;
                    uploadedfiles--;
                    $.ajax({ type: "POST", url: "bindvalues.aspx?type=delClaimImg&claimimgname=" + c.name,
                        success: function (data) {
                            if (data == '') { alert('Image deleted successfully'); }
                        }
                    });
                }
            });
        });

    </script>
</asp:Content>
