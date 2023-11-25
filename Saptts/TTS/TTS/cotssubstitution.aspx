<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="cotssubstitution.aspx.cs" Inherits="TTS.cotssubstitution" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="Scripts/cotsScript.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        <asp:Label runat="server" ID="lblPageHead" ClientIDMode="Static" Text=""></asp:Label>
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="background-color: #dcecfb; width: 100%;
            border-color: White; border-collapse: separate;">
            <tr>
                <td>
                    <asp:GridView runat="server" ID="gvReviseOrderList" AutoGenerateColumns="false" Width="100%"
                        HeaderStyle-BackColor="#FFCC00" AlternatingRowStyle-BackColor="#f5f5f5">
                        <Columns>
                            <asp:TemplateField HeaderText="CUSTOMER NAME">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatusCustName" Text='<%# Eval("custfullname") %>'></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdnOrderCustCode" Value='<%# Eval("CustCode") %>' />
                                    <asp:HiddenField runat="server" ID="hdnOrderID" Value='<%# Eval("ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ORDER REF NO.">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOrderRefNo" Text='<%# Eval("OrderRefNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="WORK ORDER" DataField="workorderno" />
                            <asp:BoundField HeaderText="ORDERD DATE" DataField="CompletedDate" ItemStyle-Width="65px" />
                            <asp:BoundField HeaderText="LAST REVISED DATE" DataField="RevisedDate" ItemStyle-Width="65px" />
                            <asp:BoundField HeaderText="CUSTOMER DESIRED SHIP DATE" DataField="DesiredShipDate"
                                ItemStyle-Width="65px" />
                            <asp:BoundField HeaderText="ORDER QTY" DataField="itemqty" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Width="30px" />
                            <asp:BoundField HeaderText="PLANT" DataField="Plant" />
                            <asp:TemplateField HeaderText="STATUS">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatusText" Text='<%# Eval("StatusText") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ACTION">
                                <ItemTemplate>
                                    <div style="text-align: center; text-decoration: none;">
                                        <asp:LinkButton ID="lnkReviseBtn" runat="server" Text="SHOW ITEMS" OnClick="lnkReviseBtn_Click"
                                            Visible='<%# Eval("CustHoldStatus").ToString() == "True" ? false : true%>' />
                                        <span style="color: #ff0000; font-style: italic;">
                                            <%# Eval("CustHoldStatus").ToString() == "True" ? "CREDIT HOLD" : ""%></span>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="width: 100%; float: left; border: 1px solid #000; display: none; line-height: 20px;
                        margin-top: 10px; background-color: #F0E2F5; padding-top: 5px;" id="divStatusChange">
                        <div id="divOrderHead" style="width: 100%; float: left;">
                            <div style="width: 50%; float: left; text-align: left;">
                                <asp:Label runat="server" ID="lblCustName" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                            </div>
                            <div style="width: 50%; float: left; text-align: right;">
                                <asp:Label runat="server" ID="lblStausOrderRefNo" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label></div>
                        </div>
                        <div style="width: 100%; float: left; text-align: center;">
                            <asp:Label runat="server" ID="lblCurrentStatus" ClientIDMode="Static" Text="" Font-Bold="true"
                                Font-Size="15px"></asp:Label>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="line-height: 20px;">
                    <asp:GridView runat="server" ID="gvReviseItemList" AutoGenerateColumns="false" Width="100%"
                        AlternatingRowStyle-BackColor="#f5f5f5" 
                        onselectedindexchanged="gvReviseItemList_SelectedIndexChanged">
                        <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" Height="22px" />
                        <Columns>
                            <asp:TemplateField HeaderText="PLATFORM">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblConfig" Text='<%#Eval("config") %>'></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdnProcessid" Value='<%#Eval("processid") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TYRE SIZE">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblSize" Text='<%#Eval("tyresize") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="RIM">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblRim" Text='<%#Eval("rimsize") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TYPE">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblType" Text='<%#Eval("tyretype") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="BRAND">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblBrand" Text='<%#Eval("brand") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SIDEWALL">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblSidewall" Text='<%#Eval("sidewall") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="QTY">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblQty" Text='<%#Eval("itemqty") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ACTION" HeaderStyle-BackColor="LightGreen">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="lnkCommercial" Text="SUBSTITUTION" Font-Bold="true"
                                        OnClick="lnkCommercial_Click"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PLATFORM" HeaderStyle-BackColor="Cyan">
                                <ItemTemplate>
                                    <%#Eval("sPlatform")%>
                                    <br />
                                    <%#Eval("sPlatform1")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TYPE" HeaderStyle-BackColor="Cyan">
                                <ItemTemplate>
                                    <%#Eval("sType")%>
                                    <br />
                                    <%#Eval("sType1")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="BRAND" HeaderStyle-BackColor="Cyan">
                                <ItemTemplate>
                                    <%#Eval("sBrand")%>
                                    <br />
                                    <%#Eval("sBrand1")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SIDEWALL" HeaderStyle-BackColor="Cyan">
                                <ItemTemplate>
                                    <%#Eval("sSidewall")%>
                                    <br />
                                    <%#Eval("sSidewall1")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr id="divSubstitution" style="display: none;">
                <td>
                    <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #07A714;
                        line-height: 25px; width: 100%;">
                        <tr>
                            <td colspan="9" style="text-align: center; background-color: #210357; font-weight: bold;
                                color: #fff; font-size: 15px;">
                                SUBSTITUTION / LIQUIDATION CHANGES
                            </td>
                        </tr>
                        <tr style="text-align: center; background-color: #cccccc; font-weight: bold;">
                            <td>
                                PLATFORM
                            </td>
                            <td>
                                TYRE SIZE
                            </td>
                            <td>
                                RIM
                            </td>
                            <td>
                                TYPE
                            </td>
                            <td>
                                BRAND
                            </td>
                            <td>
                                SIDEWALL
                            </td>
                            <td>
                                PROCESS-ID
                            </td>
                        </tr>
                        <tr style="background-color: #E5E993;">
                            <td>
                                <asp:Label runat="server" ID="lblSubPlatform" ClientIDMode="Static" Text=""></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblSubSize" ClientIDMode="Static" Text=""></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblSubRim" ClientIDMode="Static" Text=""></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblSubType" ClientIDMode="Static" Text=""></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblSubBrand" ClientIDMode="Static" Text=""></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblSubSidewall" ClientIDMode="Static" Text=""></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblSubProcessID" ClientIDMode="Static" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr style="background-color: #b1ecf9;">
                            <td>
                                <asp:DropDownList runat="server" ID="ddlSPlatform" ClientIDMode="Static">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblSSize" ClientIDMode="Static" Text=""></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblSRim" ClientIDMode="Static" Text=""></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlSType" ClientIDMode="Static" 
                                    onselectedindexchanged="ddlSType_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlSBrand" ClientIDMode="Static">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlSSidewall" ClientIDMode="Static">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblAlterProcessID" ClientIDMode="Static" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr style="background-color: #ff9a9a;">
                            <td>
                                <asp:DropDownList runat="server" ID="ddlSPlatform1" ClientIDMode="Static">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblSSize1" ClientIDMode="Static" Text=""></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblSRim1" ClientIDMode="Static" Text=""></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlSType1" ClientIDMode="Static">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlSBrand1" ClientIDMode="Static">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlSSidewall1" ClientIDMode="Static">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblAlterProcessID1" ClientIDMode="Static" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8" style="text-align: center;">
                                <div id="splitErr" style="color: #f00;">
                                </div>
                                <div id="processidErr" style="color: #f00;">
                                </div>
                                <asp:Button runat="server" ID="btnCommercial" CssClass="btnsave" Text="SAVE COMMERCIAL DECISION"
                                    OnClick="btnCommercial_Click" OnClientClick="javascript:return CtrlbtnCommercialChk();" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnCotsCustCode" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnCotsSTDcode" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnselectedrow" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnAlterProcessID" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnAlterProcessID1" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnOID" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        $(document).ready(function () {
            blinkOrderHead();
            //First Substitution
            $('#ddlSType').change(function () {
                $('#processidErr').html(''); $('#splitErr').html('');
                if ($('#ddlSType option:selected').text() != "" && $('#ddlSType option:selected').text() != "Choose")
                    chkProcessID();
            });
            $('#ddlSBrand').change(function () {
                $('#processidErr').html(''); $('#splitErr').html('');
                if ($('#ddlSBrand option:selected').text() != "" && $('#ddlSBrand option:selected').text() != "Choose")
                    chkProcessID();
            });
            $('#ddlSPlatform').change(function () {
                $('#processidErr').html(''); $('#splitErr').html('');
                if ($('#ddlSPlatform option:selected').text() != "" && $('#ddlSPlatform option:selected').text() != "Choose")
                    chkProcessID();
            });
            $('#ddlSSidewall').change(function () {
                $('#processidErr').html(''); $('#splitErr').html('');
                if ($('#ddlSSidewall option:selected').text() != "" && $('#ddlSSidewall option:selected').text() != "Choose")
                    chkProcessID();
            });
            //Second Substitution
            $('#ddlSType1').change(function () {
                $('#processidErr').html(''); $('#splitErr').html('');
                if ($('#ddlSType1 option:selected').text() != "" && $('#ddlSType1 option:selected').text() != "Choose")
                    chkProcessID1();
            });
            $('#ddlSBrand1').change(function () {
                $('#processidErr').html(''); $('#splitErr').html('');
                if ($('#ddlSBrand1 option:selected').text() != "" && $('#ddlSBrand1 option:selected').text() != "Choose")
                    chkProcessID1();
            });
            $('#ddlSPlatform1').change(function () {
                $('#processidErr').html(''); $('#splitErr').html('');
                if ($('#ddlSPlatform1 option:selected').text() != "" && $('#ddlSPlatform1 option:selected').text() != "Choose")
                    chkProcessID1();
            });
            $('#ddlSSidewall1').change(function () {
                $('#processidErr').html(''); $('#splitErr').html('');
                if ($('#ddlSSidewall1 option:selected').text() != "" && $('#ddlSSidewall1 option:selected').text() != "Choose")
                    chkProcessID1();
            });
        });

        function chkProcessID() {
            $('#hdnAlterProcessID').val('')
            var sConfig = $('#ddlSPlatform option:selected').text();
            var sSize = $('#lblSSize').html();
            var sRim = $('#lblSRim').html();
            var sType = $('#ddlSType option:selected').text();
            var sBrand = $('#ddlSBrand option:selected').text();
            var sSidewall = $('#ddlSSidewall option:selected').text();
            $.ajax({ type: "POST", url: "BindRecords.aspx?type=ChkCommercialProcessID&config=" + sConfig + "&size=" + sSize + "&rim=" + sRim + "&tyretype=" + sType + "&brand=" + sBrand + "&sidewall=" + sSidewall + "&code=" + $('#hdnCotsSTDcode').val() + "", context: document.body,
                success: function (data) {
                    $('#hdnAlterProcessID').val('');
                    $('#lblAlterProcessID').html('');
                    if (data == 'Process-ID not available')
                        $('#processidErr').html(data);
                    else {
                        $('#hdnAlterProcessID').val(data);
                        $('#lblAlterProcessID').html(data);
                    }
                }
            });
        }

        function chkProcessID1() {
            $('#hdnAlterProcessID1').val('')
            var sConfig = $('#ddlSPlatform1 option:selected').text();
            var sSize = $('#lblSSize1').html();
            var sRim = $('#lblSRim1').html();
            var sType = $('#ddlSType1 option:selected').text();
            var sBrand = $('#ddlSBrand1 option:selected').text();
            var sSidewall = $('#ddlSSidewall1 option:selected').text();
            $.ajax({ type: "POST", url: "BindRecords.aspx?type=ChkCommercialProcessID&config=" + sConfig + "&size=" + sSize + "&rim=" + sRim + "&tyretype=" + sType + "&brand=" + sBrand + "&sidewall=" + sSidewall + "&code=" + $('#hdnCotsSTDcode').val() + "", context: document.body,
                success: function (data) {
                    $('#hdnAlterProcessID1').val('');
                    $('#lblAlterProcessID1').html('');
                    if (data == 'Process-ID not available')
                        $('#processidErr').html(data);
                    else {
                        $('#hdnAlterProcessID1').val(data);
                        $('#lblAlterProcessID1').html(data);
                    }
                }
            });
        }

        function gotoNewDiv(ctrlID) {
            if (ctrlID == 'divSubstitution')
                $('#divSubstitution').css({ 'display': 'block' });
            $("html, body").stop().animate({
                scrollLeft: $('#' + ctrlID).offset().left,
                scrollTop: $('#' + ctrlID).offset().top
            }, 1200);
        }

        function CtrlbtnCommercialChk() {
            $('#splitErr').html(''); var errMsg = '';
            if ($('#processidErr').html() == "Process-ID not available")
                errMsg += $('#processidErr').html() + '<br/>';
            if ($('#hdnAlterProcessID').val().length == 0 && $('#hdnAlterProcessID1').val().length == 0)
                errMsg += 'Data not changed<br/>';
            if (errMsg.length > 0) {
                $('#splitErr').html(errMsg);
                return false;
            }
            return true;
        }
    </script>
</asp:Content>
