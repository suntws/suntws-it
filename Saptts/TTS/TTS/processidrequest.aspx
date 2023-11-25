<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="processidrequest.aspx.cs" Inherits="TTS.processidrequest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .recCss
        {
            float: left;
            background-color: #075865;
            color: #fff;
            font-size: 15px;
            font-weight: bold;
            text-align: center;
            width: 100%;
        }
    </style>
    <script type="text/javascript" src="Scripts/ttsJS.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: center;" class="pageTitleHead">
        CHECK PROCESS-ID LIST & CREATE REQUEST FOR NEW PRODUCT
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="16px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="background-color: #ecf6ff; width: 100%;
            border-color: White;">
            <tr>
                <th>
                    CATEGORY
                </th>
                <th>
                    PLATFORM
                </th>
                <th>
                    BRAND
                </th>
                <th>
                    SIDEWALL
                </th>
                <th>
                    TYPE
                </th>
                <th>
                    TYRE SIZE
                </th>
                <th>
                    RIM
                </th>
            </tr>
            <tr>
                <td>
                    <asp:DropDownList data="SizeCategory" ui-Name="Size Category" ClientIDMode="Static"
                        runat="server" ID="ddlSizeCategory" CssClass="form-control" Width="100px">
                        <asp:ListItem Text="--SELECT--" Value="0"></asp:ListItem>
                        <asp:ListItem Text="SOLID" Value="1"></asp:ListItem>
                        <asp:ListItem Text="POB" Value="2"></asp:ListItem>
                        <asp:ListItem Text="PNEUMATIC" Value="3"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList data="Config" ID="ddlConfig" ui-Name="Platform" Width="150" runat="server"
                        ClientIDMode="Static" CssClass="form-control">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList data="Brand" ID="ddlBrand" ui-Name="Brand" Width="150" runat="server"
                        ClientIDMode="Static" CssClass="form-control">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList data="Sidewall" ID="ddlSidewall" ui-Name="Sidewall" Width="150"
                        runat="server" ClientIDMode="Static" CssClass="form-control">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList data="TyreType" ID="ddlTyreType" ui-Name="Type" Width="100" runat="server"
                        ClientIDMode="Static" CssClass="form-control">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList data="TyreSize" ID="ddlTyreSize" ui-Name="Tyre Size" Width="200"
                        runat="server" ClientIDMode="Static" CssClass="form-control">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList data="Rim" ID="ddlRim" Width="70" ui-Name="Rim" runat="server"
                        ClientIDMode="Static" CssClass="form-control">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr class="NoBorder" style="border: none">
                <td>
                </td>
                <td>
                    <asp:TextBox ID="txtConfig" Width="150px" runat="server" ClientIDMode="Static" CssClass="form-control"
                        onkeyup="nospaces(this)" MaxLength="50"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtBrand" Width="150px" runat="server" ClientIDMode="Static" CssClass="form-control"
                        onkeyup="nospaces(this)" MaxLength="50"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtSidewall" Width="150px" runat="server" ClientIDMode="Static"
                        CssClass="form-control" onkeyup="nospaces(this)" MaxLength="50"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtTyreType" Width="100px" runat="server" ClientIDMode="Static"
                        CssClass="form-control" onkeyup="nospaces(this)" MaxLength="20"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtTyreSize" Width="200px" runat="server" ClientIDMode="Static"
                        CssClass="form-control" onkeyup="nospaces(this)" MaxLength="50"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtRim" Width="50px" runat="server" ClientIDMode="Static" onkeypress="return isNumberKey(event)"
                        CssClass="form-control" onkeyup="nospaces(this)" MaxLength="8"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: right;">
                    <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"
                        Font-Bold="true"></asp:Label>
                    <asp:Label runat="server" ClientIDMode="Static" ID="lblProcessId" Style="font-size: 16px;
                        color: Green;"> </asp:Label>
                </td>
                <th>
                    FINISHED WT.
                </th>
                <td>
                    <asp:TextBox runat="server" ClientIDMode="Static" ID="txtFinishedWt" Width="70px"
                        MaxLength="8" onkeypress="return isNumberKey(event);" CssClass="form-control"></asp:TextBox>
                </td>
                <td colspan="3">
                    <div id="divNewProcessID" style="display: none;">
                        <div style="width: 425px; float: left;">
                            <span style="font-size: 10px; color: #860404; width: 150px; float: left; line-height: 25px;">
                                REQUIRED PLANT (OPTIONAL)</span>
                            <asp:RadioButtonList runat="server" ID="rdbProcessIDPlant" RepeatColumns="4" RepeatDirection="Horizontal"
                                Width="200px">
                                <asp:ListItem Text="MMN" Value="MMN"></asp:ListItem>
                                <asp:ListItem Text="SLTL" Value="LANKA"></asp:ListItem>
                                <asp:ListItem Text="SITL" Value="SITL"></asp:ListItem>
                                <asp:ListItem Text="PDK" Value="PDK"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                        <input type="button" value="CREATE REQUEST FOR NEW PROCESS-ID" id="btnCheck" class="btn btn-info" />
                    </div>
                </td>
            </tr>
            <tr style="background-color: #6fdc91;">
                <th style="text-align: right;">
                    SEARCH
                    <br />
                    PROCESS-ID DATA
                </th>
                <td>
                    <asp:TextBox ID="txtFindProcessID" Width="150px" runat="server" ClientIDMode="Static"
                        CssClass="form-control" onkeyup="nospaces(this)" MaxLength="8"></asp:TextBox>
                </td>
                <td>
                    <asp:Button runat="server" ID="btnFind" Text="FIND" OnClick="btnFind_Click" OnClientClick="javascript:return CtrlFindChk();"
                        CssClass="btn btn-info" BackColor="#e8af59" />
                </td>
                <th colspan="2" style="text-align: right;">
                    PLANT WISE AVAILABLE PROCESS-ID:
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="ddlProcessIDPlant" ClientIDMode="Static" AutoPostBack="true"
                        CssClass="form-control" OnSelectedIndexChanged="ddlProcessIDPlant_IndexChange"
                        Width="80px">
                        <asp:ListItem Text="CHOOSE" Value="CHOOSE" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="MMN" Value="MMN"></asp:ListItem>
                        <asp:ListItem Text="SLTL" Value="LANKA"></asp:ListItem>
                        <asp:ListItem Text="SITL" Value="SITL"></asp:ListItem>
                        <asp:ListItem Text="PDK" Value="PDK"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:ImageButton ID="lnkExport" runat="server" ImageUrl="~/images/imagexls.png" ClientIDMode="Static"
                        OnClick="btnStockXls_Click" Style="width: 48px; height: 48px; text-decoration: none;" />
                </td>
            </tr>
            <tr>
                <td colspan="7">
                    <asp:ScriptManager runat="server" ID="scriptManager1">
                    </asp:ScriptManager>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblNoOfRecords" Text="" runat="server" ClientIDMode="static" CssClass="recCss"></asp:Label>
                            <asp:GridView runat="server" ID="gvProcessIDDetails" BackColor="White" RowStyle-Height="20px"
                                Width="100%" ClientIDMode="Static" ViewStateMode="Enabled" CssClass="gridcss">
                            </asp:GridView>
                            <asp:Button ID="btnTriggerGrid" runat="server" ClientIDMode="static" OnClick="btnTriggerGrid_Click"
                                Style="visibility: hidden;" />
                            <div id="divEnableProcessID" style="display: none;">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" ID="lblEnableID" ClientIDMode="Static" Text="" ForeColor="#d22e5b"
                                                Font-Bold="true"></asp:Label>
                                        </td>
                                        <td>
                                            ENABLE TO :
                                        </td>
                                        <td>
                                            <asp:RadioButtonList runat="server" ID="rdoEnablePlant" RepeatColumns="5" RepeatDirection="Horizontal">
                                            </asp:RadioButtonList>
                                        </td>
                                        <td>
                                            <asp:Button runat="server" ID="btnEnablePlant" ClientIDMode="Static" OnClick="btnEnablePlant_Click"
                                                Text="PREOCESS-ID ENABLE" OnClientClick="javascript:return Ctrl_Validatae();"
                                                CssClass="btn btn-success" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnTriggerGrid" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        $('#ddlSizeCategory').focus();
        $(':text').css("display", "none");
        $('#txtFindProcessID').css("display", "block");
        $("select").change(function () {
            if (($("#" + this.id + " option:selected").text()) == "ADD NEW " + $(this).attr("ui-Name").toString().toUpperCase()) {
                $("#txt" + ($(this).attr("data"))).css("display", "block").focus();
            }
            else
                $("#txt" + ($(this).attr("data"))).val("").css("display", "none");
            $("#btnTriggerGrid").trigger("click");
            $('#txtFinishedWt').css("display", "block");
        });

        var inputs = $('select').keypress(function (e) {
            if ((e.keycode == 13 || e.which == 13) && $("#" + this.id + " option:selected").text() != "--SELECT--") {
                if ($("#" + this.id + " option:selected").text() == "ADD NEW " + $(this).attr("ui-Name").toString().toUpperCase())
                    $("#txt" + $(this).attr("data")).css("display", "block").focus();
                else {
                    e.preventDefault();
                    var nextInput = inputs.get(inputs.index(this) + 1);
                    if (nextInput)
                        nextInput.focus();
                    else if (this.id = 'ddlRim')
                        $('#txtFinishedWt').css("display", "block").focus();
                }
                $("#btnTriggerGrid").trigger("click");
            }
        });

        $(':text').keydown(function (e) {
            if ((e.keycode == 13 || e.which == 13) && $('#' + this.id).val() != '') {
                if (this.id == 'txtConfig') $('#ddlBrand').focus();
                else if (this.id == 'txtBrand') $('#ddlSidewall').focus();
                else if (this.id == 'txtSidewall') $('#ddlTyreType').focus();
                else if (this.id == 'txtTyreType') $('#ddlTyreSize').focus();
                else if (this.id == 'txtTyreSize') $('#ddlRim').focus();
                else if (this.id == 'txtRim') $('#txtFinishedWt').css("display", "block").focus();
                else if (this.id == 'txtFinishedWt') $('#btnCheck').focus();
            }
        }).blur(function (e) { $('#' + this.id).val($('#' + this.id).val()); }); ;

        $("#btnCheck").click(function (eve) {
            $("#lblErrMsg").text('');
            var valid = true;
            $("select").each(function (e) {
                if (this.id != 'ddlProcessIDPlant') {
                    if ($("#" + this.id + " option:selected").index() == 0) {
                        $("#lblErrMsg").text("Please choose " + $(this).attr("ui-Name").toString().toUpperCase());
                        $("#" + this.id).focus();
                        eve.preventDefault();
                        valid = false;
                        return false;
                    }
                    else if ($("#" + this.id).val() == "ADD NEW " + $(this).attr("ui-Name").toString().toUpperCase()) {
                        if ($("#txt" + $(this).attr("data")).val().trim() == "") {
                            $("#lblErrMsg").text("Please enter value for " + $(this).attr("ui-Name").toString().toUpperCase());
                            $("#txt" + $(this).attr("data")).focus();
                            eve.preventDefault();
                            valid = false;
                            return false;
                        }
                    }
                }
            });
            if (valid == true) {
                if ($('#txtFinishedWt').val().length == 0 || parseFloat($('#txtFinishedWt').val()) == 0) {
                    $("#lblErrMsg").text('Enter Proper Finished Wt.');
                    valid = false;
                    return false;
                }
                var plant = $('input:radio[id*=MainContent_rdbProcessIDPlant_]:checked').val();
                $.ajax({
                    url: "processidrequest.aspx/GetProcessId",
                    data: "{sizeCategory:\"" + $("#ddlSizeCategory option:selected").val() + "\"," +
                            "config:\"" + ($("#ddlConfig").val() == "ADD NEW " + $('#ddlConfig').attr("ui-Name").toString().toUpperCase() ? $("#txtConfig").val() : $("#ddlConfig option:selected").text()) + "\"," +
                            "brand:\"" + ($("#ddlBrand").val() == "ADD NEW " + $('#ddlBrand').attr("ui-Name").toString().toUpperCase() ? $("#txtBrand").val() : $("#ddlBrand option:selected").text()) + "\"," +
                            "sidewall:\"" + ($("#ddlSidewall").val() == "ADD NEW " + $('#ddlSidewall').attr("ui-Name").toString().toUpperCase() ? $("#txtSidewall").val() : $("#ddlSidewall option:selected").text()) + "\"," +
                            "type:\"" + ($("#ddlTyreType").val() == "ADD NEW " + $('#ddlTyreType').attr("ui-Name").toString().toUpperCase() ? $("#txtTyreType").val() : $("#ddlTyreType option:selected").text()) + "\"," +
                            "size:\"" + ($("#ddlTyreSize").val() == "ADD NEW " + $('#ddlTyreSize').attr("ui-Name").toString().toUpperCase() ? $("#txtTyreSize").val() : $("#ddlTyreSize option:selected").text()) + "\"," +
                            "rim:\"" + ($("#ddlRim").val() == "ADD NEW " + $('#ddlRim').attr("ui-Name").toString().toUpperCase() ? $("#txtRim").val() : $("#ddlRim option:selected").text()) + "\"," +
                            "finishedWt:\"" + $("#txtFinishedWt").val() + "\"," + "userName:\"" + $("#lblWelcome").text() + "\"," + "plant:\"" + plant + "\"" +
                            "}",
                    type: "POST",
                    contentType: "application/json",
                    dataType: "text",
                    success: function (rimResult) {
                        var jsonObj = JSON.parse(rimResult);
                        jsonObj = JSON.parse(jsonObj.d);
                        if (jsonObj.processid) {
                            $("#lblProcessId").text(jsonObj.processid);
                            $("#lblProcessId").css({ 'color': '#18d838' });
                        }
                        else if (jsonObj.newprocessid) {
                            $("#lblProcessId").text(jsonObj.newprocessid);
                            $("#lblProcessId").css({ 'color': '#d83c18' });
                        }
                    }
                });
            }
        });

        function Ctrl_Validatae() {
            if ($('input:radio[id*=MainContent_rdoEnablePlant_]:checked').length == 0) {
                alert('Choose plant');
                return false
            }
            else
                return true;
        }

        function nospaces(t) {
            if (t.value.match(/\s/g)) {
                $('#lblErrMsg').html('Not allowed to enter any spaces');
                t.value = t.value.replace(/\s/g, '');
            }
        }

        function CtrlFindChk() {
            $('#lblErrMsg').html('');
            if ($('#txtFindProcessID').val().length != 8) {
                $('#lblErrMsg').html('PROCESS-ID LENGTH SHOULD BE EQUAL TO 8');
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
