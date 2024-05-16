<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="prospectpopup.aspx.cs"
    Inherits="TTS.prospectpopup" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="Scripts/ttsJS.js" type="text/javascript"></script>
    <script src="Scripts/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function attachDownload(fileLink, filename) {
            var a = document.createElement('a');
            a.href = fileLink;
            a.download = filename;
            document.body.appendChild(a);
            a.click();
            document.body.removeChild(a);
        }

        function SelectSingleReview() {
            $('#divreview').css({ 'display': 'block' });
            $('#divupdatemsg').css({ 'display': 'block' });
            $('#divFocusChange').css({ 'display': 'block' }); $('#divFlagChange').css({ 'display': 'block' });
            var strFcous = $('#hdnfocus').val();
            $('#txtFromFocus').val(strFcous);
            var strFlag = $('#hdnflag').val();
            $('#txtFromFlag').val(strFlag);
            $('#txtToFlag').val(strFlag == "On" ? "Off" : "On");
            $('#divErrMsg').html('');
            getFocusDetails();
            getCustSupplierDetails();
            getCustWiseLeadDetails('1', '6', '1', 'divPrevHistory');
            getpreviousfeedback('1', '6', '1', 'popup_feedback_box');
            LeadShow();
        }

        function getFocusDetails() {
            var custcode = $('#hdnProsCustCode').val();
            var txtFocus = $('#txtFromFocus').val();
            $.ajax({ type: "POST", url: "prospectreview.aspx/Get_Focus_Details", data: '{strCustCode:"' + custcode + '",strFocus:"' + txtFocus + '"}', contentType: "application/json; charset=utf-8", dataType: "json",
                success: OnSuccessFocusDetails,
                failure: function (response) { alert(response.d); },
                error: function (response) { alert(response.d); }
            });
        }
        function OnSuccessFocusDetails(response) {
            if (response.d.length > 0) { $('#ddlToFocus').html(response.d); }
            else { $('#ddlToFocus').html(''); }
        }

        function getCustSupplierDetails() {
            var custcode = $('#hdnProsCustCode').val();
            $.ajax({ type: "POST", url: "prospectreview.aspx/Get_Supplier_CustWise", data: '{strCustCode:"' + custcode + '"}', contentType: "application/json; charset=utf-8", dataType: "json",
                success: OnSuccessSupplierDetails,
                failure: function (response) { alert(response.d); },
                error: function (response) { alert(response.d); }
            });
        }

        function OnSuccessSupplierDetails(response) {
            if (response.d.length > 0) {
                $('#divPrevSupplier').html("<div style='float:left;width:550px;'>" + response.d + "</div>");
                $('#divhead').html($('.headname').html());
            }
            else { $('#divPrevSupplier').html(''); $('#divhead').html(''); }
        }
        function msgupdatetolead() {
            $('#popup_feedback_box').hide();
            $('#divErrMsg').html('').css({ 'color': '#ff0000', 'font-size': '12px' }); var updatemsg = $('#txtLeadUpdate').val(); var custcode = $('#hdnProsCustCode').val();
            if (custcode.length == 0)
                $('#divErrMsg').html('Choose any one customer');
            if (updatemsg.length == 0)
                $('#divErrMsg').html('Enter any update msg.');
            if (updatemsg != '' && custcode != '') {
                $.ajax({ type: "POST", url: "BindRecords.aspx?type=Insert_update_to_Lead&updatemsg=" + updatemsg + "&custcode=" + custcode + "", context: document.body,
                    success: function (data) {
                        if (data != '' && data != 'fail') {
                            $('#divErrMsg').html(data).css({ 'color': '#0C7C10', 'font-size': '16px' });
                            $('#txtLeadUpdate').val('');
                            getpreviousfeedback('1', '6', '1', 'popup_feedback_box');
                        }
                        else $('#divErrMsg').html(data).css({ 'color': '#ff0000', 'font-size': '12px' });
                    }
                });
            }
        }
        function closePopupOnly() {
            window.parent.TINY.box.hide();
            var url = $('#hdnURL').val();
            var qstr = $('#hdnqstr').val();
            if (url == "prospectreviewdom" && qstr != "1")
                window.parent.location.href = "prospectreviewdomestic.aspx?proscode=null";
            else if (url == "prospectleadstatus" && qstr != "1")
                window.parent.location.href = "prospectstatus.aspx?proscode=null";
            else if (url == "prospectreview" && qstr != "1")
                window.parent.location.href = "prospectreview.aspx?proscode=null";
            else
                window.parent.gridbind();
        }

        function btnfocuschange() {
            $('#divErrMsg').html('').css({ 'color': '#ff0000', 'font-size': '12px' });
            var custcode = $('#hdnProsCustCode').val();
            var strFocus = encodeURIComponent($('#ddlToFocus option:selected').text());
            if (custcode.length == 0)
                $('#divErrMsg').html('Choose any one customer');
            if (strFocus == 'Choose')
                $('#divErrMsg').html('Choose any one focus');
            if ($('#divErrMsg').html().length == 0) {
                $.ajax({ type: "POST", url: "BindRecords.aspx?type=update_to_ProspectFocus&custcode=" + custcode + "&changeFocus=" + $('#ddlToFocus option:selected').text() + "", context: document.body,
                    success: function (data) {
                        if (data != '' && data != 'fail') {
                            $('#focus').css({ 'display': 'none' }); $('#ddlToFocus option:selected').css({ 'disable': 'disable' });
                            $('#divErrMsg').html(data).css({ 'color': '#0C7C10', 'font-size': '16px' });
                        }
                        else $('#divErrMsg').html(data).css({ 'color': '#ff0000', 'font-size': '12px' });
                    }
                });
            }
        }

        function btnflagchange() {
            $('#divErrMsg').html('').css({ 'color': '#ff0000', 'font-size': '12px' });
            var custcode = $('#hdnProsCustCode').val();
            var strFlag = $('#txtToFlag').val();
            if (custcode.length == 0 || strFlag.length == 0)
                $('#divErrMsg').html('Choose any one customer');
            if ($('#divErrMsg').html().length == 0) {
                $.ajax({ type: "POST", url: "BindRecords.aspx?type=update_to_ProspectFlag&custcode=" + custcode + "&changeFlag=" + strFlag + "", context: document.body,
                    success: function (data) {
                        if (data != '' && data != 'fail') {
                            $('#divErrMsg').html(data).css({ 'color': '#0C7C10', 'font-size': '16px' });
                            $('#flag').css({ 'display': 'none' });
                        }
                        else
                            $('#divErrMsg').html(data).css({ 'color': '#ff0000', 'font-size': '12px' });
                    }
                });
            }
        }

        function getCustWiseLeadDetails(fromVal, toVal, fetchCount, bindCtrlID) {
            var custcode = $('#hdnProsCustCode').val();
            $.ajax({ type: "POST", url: "prospectdatabuild.aspx?type=LeadFetchRec&custcode=" + custcode + "&fromval=" + fromVal + "&toval=" + toVal + "&fetchcount=" + fetchCount + "", context: document.body,
                success: function (data) {
                    if (data != '') $('#' + bindCtrlID).html(data);
                    else $('#' + bindCtrlID).html('');
                }
            });
        }

        function getpreviousfeedback(fromVal, toVal, fetchCount, feedbackCtrlID) {
            var custcode = $('#hdnProsCustCode').val();
            $.ajax({ type: "POST", url: "prospectdatabuild.aspx?type=FeedbackFetchRec&custcode=" + custcode + "&fromval=" + fromVal + "&toval=" + toVal + "&fetchcount=" + fetchCount + "", context: document.body,
                success: function (data) {
                    if (data != '') $('#' + feedbackCtrlID).html(data);
                    else $('#' + feedbackCtrlID).html('');
                }
            });
        }
        function LeadShow() {
            $('#divPrevHistory').css({ 'display': 'block' }); $('#popup_feedback_box').css({ 'display': 'none' });
            $('#divleadshow').css({ 'background-color': '#BBBB4E' }); $('#divadminshow').css({ 'background-color': '#FFF' });
        }
        function AdminShow() {
            $('#popup_feedback_box').css({ 'display': 'block' }); $('#divPrevHistory').css({ 'display': 'none' });
            $('#divadminshow').css({ 'background-color': '#BBBB4E' }); $('#divleadshow').css({ 'background-color': '#FFF' });
        }
        $(document).ready(function () {
            blinkColor();
            $('#ddlCustFocus').change(function () { var strFlag = $('#ddlCustFocus option:selected').text(); FlagBgChange(strFlag); });
            $(':text').bind('keydown', function (e) {
                //on keydown for all textboxes
                if (e.target.className == "txtProspectCss") {
                    if (e.keyCode == 13) { //if this is enter key
                        e.preventDefault();
                        return false;
                    }
                }
            });
        });
        function blinkColor() { $('#divErrMsg').css({ 'background-color': '#2BE292', 'color': '#000' }); setTimeout("setblinkColor()", 1000) }

        function setblinkColor() { $('#divErrMsg').css({ 'background-color': '#384640', 'color': '#fff' }); setTimeout("blinkColor()", 1000) }

        function SelectSinglenew() {
            $('#divleadstatus').css({ 'display': 'block' });
            var strFlag = $('#hdnflag').val();
            FlagBgChange(strFlag);
            getFocusDetails();
            getCustSupplierDetails();
            getCustWiseLeadDetails('1', '6', '1', 'divPrevHistory');
            getpreviousfeedback('1', '6', '1', 'popup_feedback_box');
            LeadShow();
        }

        function FlagBgChange(strFlag) {
            $('#ddlCustFocus').find('option').remove();
            if (strFlag == 'On') {
                $('#ddlCustFocus').html("<option selected='selected' value='On'>On</option><option value='Off'>Off</option>");
                $('#ddlCustFocus').parent('div').css({ 'background-color': '#ff0000' })
            }
            else {
                $('#ddlCustFocus').html("<option selected='selected' value='Off'>Off</option><option value='On'>On</option>");
                $('#ddlCustFocus').parent('div').css({ 'background-color': '#2D920D' })
            }
        }

        function leadStatusInsert() {
            var custcode = $('#hdnProsCustCode').val();
            var emailpattern = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
            var errmsg = '';
            $('#divErrMsg').html('');
            if (custcode.length == 0)
                errmsg += 'Choose any one customer <br />';
            if ($('#txtContactPerson').val().length == 0)
                errmsg += 'Enter Contact Person<br />';
            if ($('#txtEmail').val().length > 0) {
                if (emailpattern.test($('#txtEmail').val()) == false)
                    errmsg += 'Enter valid E-Mail ID <br />';
            }
            if ($('#txtFeedBack').val().length == 0)
                errmsg += 'Enter any feedback <br />';
            if (errmsg.length > 0) {
                $('#divErrMsg').html(errmsg);
                return false;
            }
            else { return true; }
        }
    </script>
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
    <link href="Styles/prospectStyle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="contentText" style="background-color: #fff;">
        <table style="width: 960px;">
            <tr>
                <td style="text-align: right; line-height: 20px;">
                    <div style="background-color: #272461; width: 945px; float: left; text-align: center;
                        font-weight: bold; color: #fff;" id="divhead">
                    </div>
                    <img src="images/cancel.png" alt="CLOSE" onclick="closePopupOnly();" style="width: 15px;
                        cursor: pointer; padding-top: 2px;" />
                </td>
            </tr>
            <tr>
                <td>
                    <div style="float: left; line-height: 15px; border: 1px solid #000; background-color: #F6F3F3;">
                        <div id="divPrevSupplier" style="float: left; width: 550px;">
                        </div>
                        <div style="display: none;" id="divreview">
                            <div style="float: right; width: 380px; display: none; padding-right: 5px;" id="divupdatemsg">
                                <span class="headCss">Any message update to lead:</span>
                                <asp:TextBox ID="txtLeadUpdate" runat="server" ClientIDMode="Static" TextMode="MultiLine"
                                    Width="375px" Height="75px" Text="" onKeyUp="javascript:CheckMaxLength(this, 3999);"
                                    onChange="javascript:CheckMaxLength(this, 3999);"></asp:TextBox>
                                <span class="btnsave" onclick="msgupdatetolead();" style="margin-bottom: 5px; float: right;">
                                    Admin Feedback To Lead</span>
                            </div>
                            <hr style='width: 945px; float: left;' />
                            <div id="divFocusChange" style="float: left; width: 460px; display: none; margin-top: 5px;">
                                <span class="headCss">Focus change :</span> <span>From &nbsp;</span>
                                <asp:TextBox runat="server" ID="txtFromFocus" ClientIDMode="Static" Text="" Width="50px"
                                    Enabled="false"></asp:TextBox>
                                <span>To &nbsp;</span>
                                <asp:DropDownList runat="server" ID="ddlToFocus" ClientIDMode="Static" Width="85px">
                                </asp:DropDownList>
                                <span id="focus" class="btnsave" style="margin-bottom: 5px; padding-left: 20px;"
                                    onclick="btnfocuschange();">Save Focus</span>
                            </div>
                            <div id="divFlagChange" style="float: left; width: 460px; display: none; margin-top: 5px;">
                                <span class="headCss">Flag Change :</span> <span>From &nbsp;</span>
                                <asp:TextBox runat="server" ID="txtFromFlag" ClientIDMode="Static" Text="" Width="50px"
                                    Enabled="false"></asp:TextBox>
                                <span>To &nbsp;</span>
                                <asp:TextBox runat="server" ID="txtToFlag" ClientIDMode="Static" Text="" Width="50px"
                                    Enabled="false"></asp:TextBox>
                                <span id="flag" class="btnedit" style="margin-bottom: 5px; padding-left: 20px;" onclick="btnflagchange();">
                                    Save Flag</span>
                            </div>
                            <hr style='width: 945px; float: left;' />
                        </div>
                        <div style="float: left; width: 958px; display: none;" id="divleadstatus">
                            <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #000;
                                width: 958px; line-height: 15px;">
                                <tr>
                                    <th>
                                        COUNTRY
                                    </th>
                                    <th>
                                        CITY
                                    </th>
                                    <th>
                                        PORT
                                    </th>
                                    <th>
                                        FLAG
                                    </th>
                                    <th>
                                        FOCUS
                                    </th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCountry" runat="server" ClientIDMode="Static" Text=""></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCity" runat="server" ClientIDMode="Static" Text=""></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPort" runat="server" ClientIDMode="Static" Text=""></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblFlag" runat="server" ClientIDMode="Static" Text=""></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblfocus" runat="server" ClientIDMode="Static" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #000;
                                width: 958px; line-height: 12px;">
                                <tr>
                                    <td class="headCss" style="width: 120px;">
                                        Contact Person :
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtContactPerson" ClientIDMode="Static" Text="" Width="300px"
                                            CssClass="txtProspectCss"></asp:TextBox>
                                    </td>
                                    <td rowspan="5">
                                        <div class="headCss">
                                            Feedback:</div>
                                        <asp:TextBox runat="server" ID="txtFeedBack" ClientIDMode="Static" Text="" TextMode="MultiLine"
                                            Width="500px" Height="150px" CssClass="txtProspectCss" onKeyUp="javascript:CheckMaxLength(this, 3999);"
                                            onChange="javascript:CheckMaxLength(this, 3999);"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="headCss">
                                        E-Mail :
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtEmail" ClientIDMode="Static" Text="" Width="300px"
                                            CssClass="txtProspectCss"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="headCss">
                                        Phone No. 1 :
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtContact1" ClientIDMode="Static" Text="" Width="300px"
                                            CssClass="txtProspectCss"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="headCss">
                                        Phone No. 2 :
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtContact2" ClientIDMode="Static" Text="" Width="300px"
                                            CssClass="txtProspectCss"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="headCss">
                                        Web Address :
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtWebaddress" ClientIDMode="Static" Text="" Width="300px"
                                            CssClass="txtProspectCss"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="headCss">
                                        File Upload :
                                    </td>
                                    <td>
                                        <asp:FileUpload ID="FileUploadControl" runat="server" Width="300px" />
                                    </td>
                                    <td>
                                        <div style="background-color: #CE2626;">
                                            <span class="headCss" style="float: left; width: 50px; padding-left: 20px; color: #fff;">
                                                Flag :</span>
                                            <asp:DropDownList runat="server" ID="ddlCustFocus" ClientIDMode="Static" Width="80px">
                                                <asp:ListItem Text="Off" Value="Off"></asp:ListItem>
                                                <asp:ListItem Text="On" Value="On"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" style="text-align: center;">
                                        <asp:Button runat="server" ID="btnLeadSave" ClientIDMode="Static" CssClass="btnsave"
                                            Text="SAVE" OnClientClick="javascript:return leadStatusInsert();" OnClick="btnLeadSave_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <span id="divErrMsg" style="text-align: center; font-weight: bold; color: #ff0000;
                        float: left; width: 960px;"></span>
                </td>
            </tr>
            <tr>
                <td>
                    <div style='width: 905px; float: left; line-height: 15px; padding-left: 25px;'>
                        <div style='width: 450px; float: left; text-align: center; border: 1px solid black;'
                            id="divleadshow">
                            <span style='width: 450px; float: left; text-align: center; cursor: pointer;' onclick='LeadShow();'>
                                LEAD HISTORY</span>
                        </div>
                        <div style='width: 450px; float: left; text-align: center; border: 1px solid black'
                            id="divadminshow">
                            <span style='width: 450px; float: left; text-align: center; cursor: pointer;' onclick='AdminShow();'>
                                ADMIN HISTORY</span>
                        </div>
                        <div id="divPrevHistory" style="width: 902px; float: left; border: 1px solid #000;">
                        </div>
                        <div id="popup_feedback_box" style="width: 902px; float: left; border: 1px solid #000;">
                        </div>
                    </div>
                </td>
            </tr>
        </table>
        <asp:HiddenField runat="server" ID="hdnProsCustCode" ClientIDMode="Static" Value="" />
        <asp:HiddenField runat="server" ID="hdnqstr" ClientIDMode="Static" Value="" />
        <asp:HiddenField runat="server" ID="hdnfocus" ClientIDMode="Static" Value="" />
        <asp:HiddenField runat="server" ID="hdnflag" ClientIDMode="Static" Value="" />
        <asp:HiddenField runat="server" ID="hdnURL" ClientIDMode="Static" Value="" />
    </div>
    </form>
</body>
</html>
