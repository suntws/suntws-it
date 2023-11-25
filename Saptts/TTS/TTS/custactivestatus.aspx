<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="custactivestatus.aspx.cs"
    Inherits="TTS.custactivestatus" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="Scripts/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $('#lnkStatus').click(function () {
                $('#errMsg').html(''); $('#lblMsg').html(''); if ($('#chkCustList input:checked').length > 0) { return true; }
                else { $('#errMsg').html('Please select atleast one customer').css({ "color": '#ff0000' }); return false; }
            })

            $('#txtFindCust').on('keyup', function () {
                var query = this.value; var regex = new RegExp('\\b' + query); $('[id^="chkCustList_"]').each(function (i, elem) {
                    if (regex.test($('#chkCustList_' + i).val().toLowerCase())) { $('[id^="chkCustList_' + i + '"]').css({ "display": "block", "width": "15px", "float": "left" }); $('[for^="chkCustList_' + i + '"]').css({ "display": "block" }); } else {$('[id^="chkCustList_' + i + '"]').css({ "display": "none", "width": "15px", "float": "left" })$('[for^="chkCustList_' + i + '"]').css({ "display": "none" });}
                });
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="padding-bottom: 10px; float: left; width: 780px; font-weight: bold; height: 25px;">
        <span class="headCss">Find Customer : </span>
        <asp:TextBox runat="server" ID="txtFindCust" ClientIDMode="Static" Text="" Width="250px"
            Height="22px" autocomplete="off"></asp:TextBox>
    </div>
    <div style="border: 1px solid #cccccc; min-height: 455px; float: left;">
        <asp:CheckBoxList runat="server" ID="chkCustList" ClientIDMode="Static" RepeatColumns="2"
            RepeatDirection="Vertical" RepeatLayout="Table" Width="790px" CellPadding="2"
            CellSpacing="2">
        </asp:CheckBoxList>
        <asp:Label runat="server" ID="lblMsg" ClientIDMode="Static" Text=""></asp:Label>
    </div>
    <div style="padding-top: 15px; height: 60px; padding-left: 315px;">
        <asp:LinkButton runat="server" ID="lnkStatus" OnClientClick="javascript:return chkCheck();"
            OnClick="lnkStatus_click" CssClass="btnsave"></asp:LinkButton>
        <span id="errMsg" style="color: #0BB104;"></span>
    </div>
    <asp:HiddenField runat="server" ID="hdnCustStatus" Value="" />
    </form>
</body>
</html>
