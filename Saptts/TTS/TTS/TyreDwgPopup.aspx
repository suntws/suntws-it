<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TyreDwgPopup.aspx.cs" Inherits="TTS.TyreDwgPopup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        #spinner1
        {
            position: fixed;
            left: 0px;
            top: 0px;
            width: 100%;
            height: 100%;
            z-index: 9999;
            background: url(images/progress.gif) 50% 50% no-repeat #ede9df;
        }
    </style>
    <script src="Scripts/ttsJS.js" type="text/javascript"></script>
    <script src="Scripts/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CtrlDwgApprove() {
            var errMsg = ''; $('#lblErrMsg').html('');
            if ($('#txtApproveRemarks').val().length == 0)
                errMsg += 'Enter approved remarks<br/>';
            if ($('input:radio[id*=rdbDwgApprovedStatus_]:checked').length == 0)
                errMsg += 'Choose approved status';
            if (errMsg.length > 0) {
                $('#lblErrMsg').html(errMsg);
                return false;
            }
            else
                return true;
        }
        function CtrlDwgremarks() {
            var errMsg = ''; $('#lblErrMsg').html('');
            if ($('#txtApproveRemarks').val().length == 0)
                errMsg += 'Enter approved remarks<br/>';
            if (errMsg.length > 0) {
                $('#lblErrMsg').html(errMsg);
                return false;
            }
            else
                return true;
        }
        function divnone(ctrl) { $('#' + ctrl).css({ "display": "none" }) }
        function divblock(ctrl) { $('#' + ctrl).css({ "display": "block" }) }
        function closePopup() {
            window.parent.TINY.box.hide();
            window.parent.location.href = 'TyreDrawingApprove.aspx?qid=' + $('#hdnStatus').val() + '&pid=' + $('#hdnStatusID').val();
        }

        function closePopupOnly() {
            window.parent.TINY.box.hide();
        }
        $(window).load(function () { $("#spinner1").css({ "display": "none" }); }) 
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="spinner1">
    </div>
    <div id="contentText" style="font-size: 12px;">
        <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #fff;
            width: 992px;">
            <tr>
                <td style="text-align: center;">
                    <div style="width: 965px; float: left; font-size: 15px; color: #AEAD25; font-weight: bold;">
                        <asp:Label runat="server" ID="lblApproveHeading" ClientIDMode="Static" Text=""></asp:Label>
                    </div>
                    <div style="width: 20px; float: right;">
                        <img src="images/cancel.png" alt="CLOSE" onclick="closePopupOnly();" style="width: 15px;
                            cursor: pointer;" />
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="divRemarksCtrl" runat="server">
                        <div id="divPdfDwg" runat="server" style="width: 988px; float: left;">
                        </div>
                        <div style="width: 805px; float: left; padding-top: 10px;">
                            <asp:Label runat="server" ID="lblremarks" ClientIDMode="Static" Text="" Font-Bold="true"
                                ForeColor="Green"></asp:Label>
                        </div>
                        <div id="divbutton" style="width: 988px; float: left; padding-top: 10px; display: none;">
                            <div id="divtext" style="width: 810px; float: left;">
                                REMARKS
                                <asp:TextBox runat="server" ID="txtApproveRemarks" ClientIDMode="Static" Text=""
                                    TextMode="MultiLine" Width="800px" Height="65px" onKeyUp="javascript:CheckMaxLength(this, 999);"
                                    onChange="javascript:CheckMaxLength(this, 999);"></asp:TextBox>
                                <asp:Label runat="server" ID="lblCustomer" ClientIDMode="Static" Text=""></asp:Label>
                                <asp:DropDownList runat="server" ID="ddlCustomer" ClientIDMode="Static">
                                </asp:DropDownList>
                            </div>
                            <div id="divApporove" style="width: 160px; float: left; text-align: center; padding-left: 10px;
                                display: none;">
                                <div style="width: 160px; float: left;">
                                    APPROVED STATUS
                                    <asp:RadioButtonList runat="server" ID="rdbDwgApprovedStatus" ClientIDMode="Static"
                                        RepeatColumns="2" Width="150px">
                                        <asp:ListItem Value="true" Text="OK"></asp:ListItem>
                                        <asp:ListItem Value="false" Text="NOT OK"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                                <div style="width: 160px; float: left;">
                                    <asp:Button runat="server" ID="btnApprove" Text="SAVE STATUS" OnClick="btnApprove_Click"
                                        OnClientClick="javascript:return CtrlDwgApprove()" BackColor="Green" />
                                </div>
                            </div>
                            <div id="divRemarks" style="width: 160px; float: left; text-align: center; padding-left: 10px;
                                display: none;">
                                <asp:Button runat="server" ID="btnremarks" Text="SAVE STATUS" OnClick="btnremarks_Click"
                                    OnClientClick="javascript:return CtrlDwgremarks()" BackColor="Green" />
                            </div>
                            <div>
                                <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblMsg" ClientIDMode="Static" Text="" ForeColor="Green"
                        Font-Bold="true"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView runat="server" ID="gvDwgApprovedHistory" AutoGenerateColumns="false"
                        Width="985px" AlternatingRowStyle-BackColor="#f5f5f5">
                        <HeaderStyle Font-Bold="true" BackColor="#fefe8b" />
                        <Columns>
                            <asp:TemplateField HeaderText="CATEGORY" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:Label ID="lblcategory" runat="server" Text='<%# Eval("Category") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="UPLOAD REMARKS">
                                <ItemTemplate>
                                    <%# Eval("UploadRemarks") %>
                                    <br />
                                    <%# Eval("UploadedDate") %>
                                    -
                                    <%# Eval("UploadedBy")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="EDC REMARKS 1">
                                <ItemTemplate>
                                    <%# Eval("EdcRemarks")%>
                                    <br />
                                    <%# Eval("EdcApprovedDate")%>
                                    <%# Eval("EdcApprovedBy")%>
                                    <%# Eval("EdcApprovedStatus").ToString() == "True" ? " - APPROVED" : Eval("EdcApprovedBy").ToString() == "" ? "WAITING" : "<span style='color:#f00;'> - NOT APPROVED</span>"%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="EDC REMARKS 2">
                                <ItemTemplate>
                                    <%# Eval("EdcRemarks1")%>
                                    <br />
                                    <%# Eval("EdcApprovedDate1")%>
                                    <%# Eval("EdcApprovedBy1")%>
                                    <%# Eval("EdcApprovedStatus1").ToString() == "True" ? " - APPROVED" : Eval("EdcApprovedBy1").ToString() == "" ? "WAITING" : "<span style='color:#f00;'> - NOT APPROVED</span>"%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="EDC REMARKS 3">
                                <ItemTemplate>
                                    <%# Eval("EdcRemarks2")%>
                                    <br />
                                    <%# Eval("EdcApprovedDate2")%>
                                    <%# Eval("EdcApprovedBy2")%>
                                    <%# Eval("EdcApprovedStatus2").ToString() == "True" ? " - APPROVED" : Eval("EdcApprovedBy2").ToString() == "" ? "WAITING" : "<span style='color:#f00;'> - NOT APPROVED</span>"%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CRM REMARKS">
                                <ItemTemplate>
                                    <%# Eval("CrmRemarks")%>
                                    <br />
                                    <%# Eval("CrmApprovedDate")%>
                                    <%# Eval("CrmApprovedBy")%>
                                    <%# Eval("CrmApprovedStatus").ToString() == "True" ? " - APPROVED" : Eval("CrmApprovedBy").ToString() == "" ? "WAITING" : "<span style='color:#f00;'> - NOT APPROVED</span>"%>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <div>
                        <asp:Label ID="lblALLOCATION" runat="server" ClientIDMode="Static" Text='' Font-Bold="true"
                            ForeColor="Green"></asp:Label>
                    </div>
                    <asp:GridView runat="server" ID="gvTyreSizeList" AutoGenerateColumns="false" Width="500px"
                        AlternatingRowStyle-BackColor="#f5f5f5" PagerStyle-Height="30px" PagerStyle-Font-Bold="true"
                        PagerStyle-Font-Size="15px" PagerStyle-HorizontalAlign="Center" PagerStyle-VerticalAlign="Middle">
                        <HeaderStyle Font-Bold="true" BackColor="#8BFEB8" />
                        <Columns>
                            <asp:TemplateField HeaderText="SIZE">
                                <ItemTemplate>
                                    <asp:Label ID="lblgtyresize" runat="server" Text='<%# Eval("TyreSize") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hdnStatus" runat="server" ClientIDMode="Static" Value="" />
        <asp:HiddenField ID="hdnStatusID" runat="server" ClientIDMode="Static" Value="" />
    </div>
    </form>
</body>
</html>
