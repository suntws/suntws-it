<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="frmexpordergrade.aspx.cs" Inherits="COTS.frmexpordergrade" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div align="center" class="pageTitleHead">
        GRADE</div>
    <div class="contPage">
        <table style="width: 100%;">
            <tr>
                <td>
                    <div style="font-weight: bold; border: 1px solid #E71FB7; background-color: #FAD6D6;
                        width: 100%;" class="trMainDiv">
                        <div style="width: 455px; float: left;">
                            <div style="width: 95px; float: left; line-height: 25px;">
                                <span class="headCss">ORDER NO. :</span></div>
                            <div style="width: 350px; float: left;">
                                <asp:TextBox runat="server" ID="txtOrderRefNo" ClientIDMode="Static" Text="" CssClass="txtCss"
                                    Width="340px" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td style="background-color: #ccc; width: 200px; text-align: center; font-weight: bold;">
                                CATEGORY
                            </td>
                            <td style="background-color: #F4A460; width: 200px; text-align: center; font-weight: bold;">
                                PLATFORM
                            </td>
                            <td style="background-color: #c18ff0; width: 200px; text-align: center; font-weight: bold;">
                                BRAND
                            </td>
                            <td style="background-color: #ABEFDE; width: 200px; text-align: center; font-weight: bold;">
                                TYRE TYPE
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="divfulldata">
                        <asp:Label ID="lbl" runat="server" Text=""></asp:Label>
                    </div>
                </td>
            </tr>
            <tr>
                <td style='text-align: center;'>
                    <asp:Button ID="btnsave" runat="server" ClientIDMode="Static" OnClientClick="javascript:return ctrlValidation();"
                        Text="NEXT" CssClass="btnAuthorize" OnClick="btnsave_Click" />
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnCustCode" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnStdCustCode" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdndata" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        $(document).ready(function () {
            $('.divCategory').find('input').click(function (e) {
                $('.cssconfig').css({ "display": "none" });
                $('.cssbrand').css({ "display": "none" });
                $('.csstype').css({ "display": "none" });
                $('.cssconfig').find("input:radio").removeAttr("checked");
                $('.cssbrand').find('input:checkbox:not(:disabled)').removeAttr('checked');
                $('.csstype').find('input:checkbox:not(:disabled)').removeAttr('checked');
                var category = this.value;
                //  $('.cssconfig' + category).css({ "display": "block" });
                $('.cssconfig' + category).fadeIn(800);
            });
            $('.cssconfig').find('input').click(function (e) {
                $('.cssbrand').css({ "display": "none" });
                $('.csstype').css({ "display": "none" });
                $('.cssbrand').find('input:checkbox:not(:disabled)').removeAttr('checked');
                $('.csstype').find('input:checkbox:not(:disabled)').removeAttr('checked');
                var category = $('.divCategory').find('input:radio[id*=rdbSizeCategory_]:checked').val();
                var config = this.value;
                //  $('.cssbrand' + category + config).css({ "display": "block" });
                $('.cssbrand' + category + config).fadeIn(1000);
                var checked_checkboxes = $('.cssbrand').find('input:checkbox[id*=chkbrand"' + category + config + '"_"' + e + '"]:checked');
                checked_checkboxes.each(function () {
                    var brand = $(this).val();
                    $('.cssbrand' + category + config).find('input:checkbox[id*=chkbrand"' + category + config + '"_]:checked').each(function () {
                        if ($(this).val() == brand)
                            $('.csstype' + category + config + brand).fadeIn(500);
                    });
                });
            });
            $('.cssbrand').find('input').click(function (e) {
                $('.csstype').css({ "display": "none" });
                var category = $('.divCategory').find('input:radio[id*=rdbSizeCategory_]:checked').val();
                var config = $('.cssconfig').find('input:radio[id*=rdbConfig"' + category + '"_]:checked').val();
                var checked_checkboxes = $('.cssbrand').find('input:checkbox[id*=chkbrand"' + category + config + '"_"' + e + '"]:checked');

                checked_checkboxes.each(function () {
                    var brand = $(this).val();
                    $('.cssbrand' + category + config).find('input:checkbox[id*=chkbrand"' + category + config + '"_]:checked').each(function () {
                        if ($(this).val() == brand)
                            $('.csstype' + category + config + brand).fadeIn(500); // $('.csstype' + category + config + brand).css({ "display": "block" });
                    });
                });
                $(".cssbrand" + category + config + " :input:not(:checked)").each(function () {
                    var brand = $(this).val();
                    $('.csstype' + category + config + brand).find("input:checkbox").removeAttr("checked");
                });
            });
        });
        function disabledata(category, config, brand, type) {
            var checked_checkboxes = $('.cssbrand').find("input[name='brand" + category + config + "']")
            checked_checkboxes.each(function (e) {
                var data = $('#chkbrand' + category + config + '_' + e).val();
                if (data == brand) {
                    $('#chkbrand' + category + config + '_' + e).attr("disabled", true);
                    $('#chkbrand' + category + config + '_' + e).prop('checked', true);
                }
            });
            $('#chkTyretype' + "_" + category + "_" + config + "_" + brand + "_" + type).attr("disabled", true);
            $('#chkTyretype' + "_" + category + "_" + config + "_" + brand + "_" + type).prop('checked', true);
        }
        function ctrlValidation() {
            var ID = '';
            $('.csstype input:checked').each(function () {
                if (!$(this).is(':disabled')) {
                    if (ID == '')
                        ID = $(this).attr('id');
                    else
                        ID = ID + "~" + $(this).attr('id');
                }
            }); $('#hdndata').val(ID);
        }
    </script>
</asp:Content>
