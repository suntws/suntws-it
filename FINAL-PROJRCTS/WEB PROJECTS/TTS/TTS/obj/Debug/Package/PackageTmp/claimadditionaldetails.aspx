<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="claimadditionaldetails.aspx.cs" Inherits="TTS.claimadditionaldetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .claimImgSize
        {
            height: 300px;
            width: 300px;
            border: 0px;
        }
        .StencilFailureImgSize
        {
            height: 145px;
            width: 150px;
        }
        .parent
        {
            position: relative;
            display: inline-block;
            border: 1px solid #aba604;
            background-color: transparent;
            width: 150px;
            text-align: left;
        }
        
        .close
        {
            position: absolute;
            top: 0;
            right: 0;
            cursor: pointer;
            margin: 4px 2px;
        }
        .tbInfoCss
        {
            border-collapse: collapse;
            border-color: #000;
            width: 1068px;
            float: left;
        }
        .tbInfoCss th
        {
            background-color: #ccc;
            font-weight: bold;
            text-align: left;
            line-height: 20px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        CLAIM ADDITIONAL DETAILS ADD
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table style="width: 1070px;">
            <tr>
                <td>
                    <asp:GridView runat="server" ID="gvClaimAdditionalDetails" AutoGenerateColumns="false"
                        Width="1070px">
                        <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" Height="22px" />
                        <Columns>
                            <asp:BoundField DataField="custname" HeaderText="CUSTOMER NAME" ItemStyle-Width="220px" />
                            <asp:BoundField DataField="complaintno" HeaderText="COMPLAINT NO." ItemStyle-Width="60px" />
                            <asp:BoundField DataField="complaintdate" HeaderText="COMPLAINT DATE" ItemStyle-Width="60px" />
                            <asp:BoundField DataField="plant" HeaderText="PLANT" ItemStyle-Width="50px" />
                            <asp:BoundField DataField="Qty" HeaderText="QTY" ItemStyle-Width="40px" />
                            <asp:BoundField DataField="claimstatus" HeaderText="STATUS" ItemStyle-Width="250px" />
                            <asp:TemplateField HeaderText="ACTION" ItemStyle-Width="50px">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkClaimNo" runat="server" Text="Show List" OnClick="lnkClaimNo_Click" />
                                    <asp:HiddenField runat="server" ID="hdnClaimCustCode" Value='<%# Eval("custcode") %>' />
                                    <asp:HiddenField runat="server" ID="hdnstatusid" Value='<%# Eval("statusid") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="divclaimadddetails" style="width: 1068px; float: left;">
                        <div style="width: 1066px; float: left; border: 1px solid #000; background-color: #056442;
                            font-weight: bold; color: #fff; font-size: 15px;">
                            <div style="width: 525px; float: left; text-align: left; padding-right: 5px;">
                                <asp:Label runat="server" ID="lblClaimCustName" ClientIDMode="Static" Text=""></asp:Label>
                            </div>
                            <div style="width: 2px; float: left;">
                                <asp:Label runat="server" ID="lblClaim" ClientIDMode="Static" Text=""></asp:Label>
                            </div>
                            <div style="width: 520px; float: left; text-align: right; padding-left: 5px;">
                                <asp:Label runat="server" ID="lblClaimNo" ClientIDMode="Static" Text=""></asp:Label>
                            </div>
                        </div>
                        <div style="width: 1068px; float: left;">
                            <asp:GridView runat="server" ID="gvClaimItems" AutoGenerateColumns="false" Width="1068px"
                                RowStyle-Height="20px">
                                <HeaderStyle BackColor="#CACA55" Font-Bold="true" Height="22px" />
                                <Columns>
                                    <asp:BoundField DataField="brand" HeaderText="BRAND" ItemStyle-Width="100px" />
                                    <asp:BoundField DataField="tyresize" HeaderText="SIZE" ItemStyle-Width="160px" />
                                    <asp:BoundField DataField="stencilno" HeaderText="STENCIL NO." ItemStyle-Width="80px" />
                                    <asp:BoundField DataField="AdditionalStatus" HeaderText="STATUS" ItemStyle-Width="60px" />
                                    <asp:TemplateField HeaderText="REQUEST COMMENTS" ItemStyle-Width="200px">
                                        <ItemTemplate>
                                            <%# Eval("AdditionalStatus").ToString() == "QC REQUEST" || Eval("AdditionalStatus").ToString() == "UPDATED" ? ((string)Eval("AdditionalReq")).Replace("~", "<br/>") : ((string)Eval("AdditionalReq")).Replace("~", "<br/>")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ACTION" ItemStyle-Width="60px">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkClaimItem" runat="server" Text='<%# Eval("lnkTxt") %>' OnClick="lnkClaimStencil_Click"></asp:LinkButton>
                                            <asp:HiddenField runat="server" ID="hdnConfig" ClientIDMode="Static" Value='<%# Eval("config") %>' />
                                            <asp:HiddenField runat="server" ID="hdnTyreType" ClientIDMode="Static" Value='<%# Eval("tyretype") %>' />
                                            <asp:HiddenField runat="server" ID="hdnCustgvnType" ClientIDMode="Static" Value='<%# Eval("CustgvnType") %>' />
                                            <asp:HiddenField runat="server" ID="hdnComplaintDesc" ClientIDMode="Static" Value='<%# Eval("ClaimDescription") %>' />
                                            <asp:HiddenField runat="server" ID="hdnComplaint" ClientIDMode="Static" Value='<%# Eval("appstyle") %>' />
                                            <asp:HiddenField runat="server" ID="hdnOperatingCondition" ClientIDMode="Static"
                                                Value='<%# Eval("runninghours") %>' />
                                            <asp:HiddenField runat="server" ID="hdnAdditionalUpdateComments" ClientIDMode="Static"
                                                Value='<%# Eval("AdditionalUpdateComments") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div style="width: 1068px; float: left; text-align: left; margin-top: 5px;">
                            <asp:Label runat="server" ID="lblComplaintComments" ClientIDMode="Static" Text=""></asp:Label>
                        </div>
                        <div style="width: 1068px; float: left; margin-top: 5px; display: none;" id="divAddInfo">
                            <table cellspacing="0" rules="all" border="1" class="tbInfoCss">
                                <tr>
                                    <th>
                                        STENCIL NO.
                                    </th>
                                    <td>
                                        <asp:Label runat="server" ID="lblStencilNo" ClientIDMode="Static" Text=""></asp:Label>
                                    </td>
                                    <td rowspan="9" style="width: 300px;">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:GridView runat="server" ID="gvClaimImages" AutoGenerateColumns="false" AllowPaging="true"
                                                    PageSize="1" OnPageIndexChanging="gvClaimImages_PageIndex" PagerStyle-HorizontalAlign="Center"
                                                    PagerStyle-VerticalAlign="Middle">
                                                    <HeaderStyle CssClass="headerNone" />
                                                    <Columns>
                                                        <asp:TemplateField ItemStyle-CssClass="claimImgSize">
                                                            <ItemTemplate>
                                                                <a href='<%# Eval("ClaimImage") %>' class="claimImgSize" target="_blank">
                                                                    <img alt="Claim Images" src='<%# Eval("ClaimImage") %>' class="claimImgSize">
                                                                </a>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        BRAND
                                    </th>
                                    <td>
                                        <asp:Label runat="server" ID="lblBrand" ClientIDMode="Static" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        TYRE SIZE
                                    </th>
                                    <td>
                                        <asp:Label runat="server" ID="lblTyreSize" ClientIDMode="Static" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        TYRE TYPE
                                    </th>
                                    <td>
                                        <asp:Label runat="server" ID="lblTyreType" ClientIDMode="Static" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        COMPLAINT
                                    </th>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtComplaint" ClientIDMode="Static" Width="520px"
                                            CssClass="claimtxt" Height="70px" TextMode="MultiLine" onKeyUp="javascript:CheckMaxLength(this, 999);"
                                            onChange="javascript:CheckMaxLength(this, 999);"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        OPERATING CONDITION
                                    </th>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtOperatingCondition" ClientIDMode="Static" Width="520px"
                                            CssClass="claimtxt" Height="70px" TextMode="MultiLine" onKeyUp="javascript:CheckMaxLength(this, 999);"
                                            onChange="javascript:CheckMaxLength(this, 999);"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        STENCIL IMAGE
                                    </th>
                                    <td>
                                        <asp:FileUpload ID="up_Stencil" runat="server" ClientIDMode="Static" onchange="uploadImages('up_Stencil','uploadstencilimage')"
                                            accept=".png,.jpg,.jpeg,.gif,.bmp,.tif" />
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        FAILURE IMAGE
                                    </th>
                                    <td>
                                        <input type="file" multiple="multiple" id="up_Failure" onchange="uploadImages('up_Failure','uploadfailureimage')"
                                            accept=".png,.jpg,.jpeg,.gif,.bmp,.tif">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="vertical-align: top;">
                                        <asp:UpdatePanel ID="updpnl_StencilFailureImage" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:DataList runat="server" ID="gv_StencilFailure" RepeatColumns="5" RepeatDirection="Horizontal"
                                                    RepeatLayout="Table">
                                                    <ItemStyle VerticalAlign="Top" />
                                                    <ItemTemplate>
                                                        <div class="parent">
                                                            <img alt="Stencil/Failure Images" border="0" src='<%# Eval("ClaimImage") %>' class="StencilFailureImgSize"
                                                                id="img1" runat="server" />
                                                            <div>
                                                                <img alt="close" class="close" style="width: 15px; height: 15px;" src="images/cancel.png"
                                                                    id="img2" runat="server" onclick="closebuttonaction(this)" />
                                                                <asp:Label runat="server" ID="lblImgName" Text='<%# Eval("ClaimImageName") %>'></asp:Label>
                                                            </div>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:DataList>
                                                <asp:Button ID="btnTriggerGv" runat="server" ClientIDMode="static" Style="visibility: hidden;"
                                                    OnClick="btnTriggerGv_Click" />
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnTriggerGv" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        COMMENTS
                                    </th>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtComments" ClientIDMode="Static" Width="520px"
                                            CssClass="claimtxt" Height="70px" TextMode="MultiLine" onKeyUp="javascript:CheckMaxLength(this, 199);"
                                            onChange="javascript:CheckMaxLength(this, 199);"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" Font-Bold="true"
                                            ForeColor="Red"></asp:Label>
                                        <asp:Button runat="server" ID="btnSaveAdditionalDetails" ClientIDMode="Static" Text="SAVE ADDITIONAL DETAILS"
                                            OnClick="btnSaveAdditionalDetails_Click" OnClientClick="javascript:return CtrlSaveAdditional();"
                                            CssClass="btnactive" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div style="width: 1068px; float: left; margin-top: 5px; display: none;" id="divMovedFromAddInfo">
                            <span class="headCss" style="width: 1064px; float: left;">ANY COMMENTS:</span>
                            <asp:TextBox runat="server" ID="txtAdditionalComments" ClientIDMode="Static" Text=""
                                TextMode="MultiLine" Width="525px" Height="80px" onKeyUp="javascript:CheckMaxLength(this, 3999);"
                                onChange="javascript:CheckMaxLength(this, 3999);"></asp:TextBox>
                            <asp:Button runat="server" ID="btnMovedFromAdditionalDetails" ClientIDMode="Static"
                                CssClass="btnsave" Text="CLAIM ADDITIONAL DETAILS MOVE TO NEXT STAGE" OnClick="btnMovedFromAdditionalDetails_Click" />
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnCustCode" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnSelectIndex" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnselectedrow" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnStencilPlant" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnClaimStatus" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        function closebuttonaction(event) {
            var name = event.id;
            name = name.replace('_img2_', '_img1_');
            var id = $('#' + name).attr("src");
            $.ajax({ type: "POST", url: "BindRecords.aspx?type=delstencilFailure&path=" + id,
                success: function (data) {
                    if (data == '') { $('#btnTriggerGv').click(); }
                }
            });
        }
        function gotoClaimDiv(ctrlID) {
            $('#' + ctrlID).css({ 'display': 'block' });
            $("html, body").stop().animate({
                scrollLeft: $('#' + ctrlID).offset().left,
                scrollTop: $('#' + ctrlID).offset().top
            }, 1200);
        }
        function CtrlSaveAdditional() {
            $('#lblErrMsg').html('');
            var ErrMsg = '';
            if ($('#txtComplaint').val().length == 0)
                ErrMsg += 'Enter complaint<br/>';
            if ($('#txtOperatingCondition').val().length == 0)
                ErrMsg += 'Enter operating condition<br/>';
            if ($('#txtComments').val().length == 0)
                ErrMsg += 'Enter comments<br/>';
            if (ErrMsg.length > 0) {
                $('#lblErrMsg').html(ErrMsg);
                return false;
            }
            else
                return true;
        }

        function uploadImages(CtrlFileUpload, UploadHandler) {
            var fileUpload = $('#' + CtrlFileUpload).get(0);
            var files = fileUpload.files;
            var test = new FormData();
            for (var i = 0; i < files.length; i++) {
                test.append(files[i].name, files[i]);
            }
            $.ajax({
                url: UploadHandler + ".ashx", type: "POST", contentType: false, processData: false, data: test,
                success: function () {
                    $('#btnTriggerGv').click();
                },
                error: function () {
                    alert("File not support, Contact your admin");
                }
            });
        }
    </script>
</asp:Content>
