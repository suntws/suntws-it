<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="containerorderentry.ascx.cs"
    Inherits="TTS.cargomanagement.containerorderentry" %>
<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
<div style="width: 1080px;">
    <div>
        <table width="1078px;">
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #CE8686;
                                width: 1065px;">
                                <tr align="center" class="headCss" style="background-color: #EBEEED;">
                                    <td>
                                        CATEGORY
                                    </td>
                                    <td>
                                        PLATFORM
                                    </td>
                                    <td>
                                        BRAND
                                    </td>
                                    <td>
                                        SIDEWALL
                                    </td>
                                    <td>
                                        TYPE
                                    </td>
                                    <td>
                                        SIZE
                                    </td>
                                    <td>
                                        RIM
                                    </td>
                                    <td>
                                        QTY
                                    </td>
                                    <td>
                                        WT
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlCategory" ClientIDMode="Static" Width="95px"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_IndexChange" CssClass="ddlExp">
                                            <asp:ListItem Text="Choose" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="SOLID" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="POB" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="PNEUMATIC" Value="3"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlPlatform" ClientIDMode="Static" Width="100px"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlPlatform_IndexChange" CssClass="ddlExp">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlBrand" ClientIDMode="Static" Width="100px"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlBrand_IndexChange" CssClass="ddlExp">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlSidewall" ClientIDMode="Static" Width="100px"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlSidewall_IndexChange" CssClass="ddlExp">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlType" ClientIDMode="Static" Width="80px"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlType_IndexChange" CssClass="ddlExp">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlSize" ClientIDMode="Static" Width="200px"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlSize_IndexChange" CssClass="ddlExp">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlRim" ClientIDMode="Static" Width="55px" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlRim_IndexChange" CssClass="ddlExp">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtQty" ClientIDMode="Static" Text="" Width="30px"
                                            MaxLength="4" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtFinishedWt" ClientIDMode="Static" Text="" Width="50px"
                                            onkeypress="return isNumberKey(event)" MaxLength="8"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="headCss">
                                        <input type="checkbox" id="chkrimassmbly" runat="server" clientidmode="Static" value="RIM ASSEMBLY"
                                            onclick="RimAssm_enabledisable();" />RIM ASSEMBLY
                                    </td>
                                    <td>
                                        <span>RIM QTY</span>
                                        <asp:TextBox runat="server" ID="txtRimQty" ClientIDMode="Static" Enabled="false"
                                            Text="" Width="40px" CssClass="rimassy" MaxLength="4" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                    </td>
                                    <td>
                                        <span>RIM WEIGHT</span>
                                        <asp:TextBox runat="server" ID="txtRimWt" ClientIDMode="Static" Enabled="false" Text=""
                                            Width="50px" CssClass="rimassy" onkeypress="return isNumberKey(event)" MaxLength="8"></asp:TextBox>
                                    </td>
                                    <td colspan="6">
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button runat="server" ID="btnAddNextItem" ClientIDMode="Static" Text="ADD NEXT ITEM"
                        CssClass="btnsave" OnClientClick="javascript:return CtrlAddNextItem();" OnClick="btnAddNextItem_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView runat="server" ID="gv_ManualOrderList" AutoGenerateColumns="false"
                        Width="1065px" RowStyle-Height="22px" OnRowDeleting="gv_ManualOrderList_RowDeleting"
                        OnRowUpdating="gv_ManualOrderList_RowUpdating" OnRowCancelingEdit="gv_ManualOrderList_RowCanceling"
                        OnRowEditing="gv_ManualOrderList_RowEditing">
                        <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" Height="22px" />
                        <Columns>
                            <asp:TemplateField HeaderText="CATEGORY" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <%#Eval("category")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PLATFORM" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <%#Eval("Config")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="BRAND" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <%#Eval("brand")%></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SIDEWALL" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <%#Eval("sidewall")%></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TYPE" ItemStyle-Width="60px">
                                <ItemTemplate>
                                    <%#Eval("tyretype") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TYRE SIZE" ItemStyle-Width="150px">
                                <ItemTemplate>
                                    <%#Eval("tyresize")%></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="RIM" ItemStyle-Width="40px">
                                <ItemTemplate>
                                    <%#Eval("rimsize")%>
                                    <asp:HiddenField ID="hdnprocessid" runat="server" Value='<%# Eval("processid") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="QTY" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <%#Eval("itemqty")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div style="background-color: #EEFD2D; border-bottom: #000; color: #000;">
                                        <asp:Label runat="server" ID="lblGItemQty" Text='<%#Eval("itemqty") %>'></asp:Label></div>
                                    <asp:TextBox runat="server" ID="txtGItemQty" onkeypress="return isNumberKey(event)"
                                        Width="50px" MaxLength="5" BackColor="#f9c232" Text='<%# Eval("itemqty") %>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="FWT" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <%# Eval("finishedwt") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="RIM QTY" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <%# Eval("Rimitemqty").ToString() == "0" ? "" : Eval("Rimitemqty").ToString()%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div style="background-color: #EEFD2D; border-bottom: #000; color: #000;">
                                        <asp:Label runat="server" ID="lblGRimQty" Text='<%#Eval("Rimitemqty") %>' Visible='<%# Eval("Rimitemqty").ToString() == "0" ? false : true%>'></asp:Label></div>
                                    <asp:TextBox runat="server" ID="txtGRimQty" onkeypress="return isNumberKey(event)"
                                        Width="40px" MaxLength="5" BackColor="#f9c232" Text='<%# Eval("Rimitemqty") %>'
                                        Visible='<%# Eval("Rimitemqty").ToString() == "0" ? false : true%>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="RIM FWT" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <%# Eval("Rimfinishedwt").ToString() == "0.00" ? "" : Eval("Rimfinishedwt").ToString()%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div style="background-color: #EEFD2D; border-bottom: #000; color: #000;">
                                        <asp:Label runat="server" ID="lblGRimFwt" Text='<%#Eval("Rimfinishedwt") %>' Visible='<%# Eval("Rimfinishedwt").ToString() == "0.00" ? false : true%>'></asp:Label></div>
                                    <asp:TextBox runat="server" ID="txtGRimFwt" onkeypress="return isNumberKey(event)"
                                        Width="70px" MaxLength="10" BackColor="#f9c232" Text='<%# Eval("Rimfinishedwt") %>'
                                        Visible='<%# Eval("Rimfinishedwt").ToString() == "0.00" ? false : true%>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TOTAL WEIGHT" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <%#Convert.ToInt32(Eval("itemqty")) * Convert.ToDouble(Eval("finishedwt")) + Convert.ToInt32(Eval("Rimitemqty")) * Convert.ToDouble(Eval("Rimfinishedwt"))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ACTION" ItemStyle-Width="70px">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="lnkEdit" Text="EDIT" CommandName="Edit" Font-Size="10px"></asp:LinkButton>
                                    <asp:LinkButton runat="server" ID="lnkDelete" Text="DELETE" CommandName="Delete"
                                        Font-Size="10px"></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Button runat="server" ID="btnUpdate" Text="Update" CommandName="Update" />
                                    <asp:Button runat="server" ID="btnCancel" Text="Cancel" CommandName="Cancel" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="float: left; width: 780px; text-align: right;">
                        <span class="headCss">TOTAL QTY:</span>
                        <asp:Label runat="server" ID="lblTotQTy" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                    </div>
                    <div style="float: left; width: 200px; text-align: right;">
                        <span class="headCss">TOTAL WT:</span>
                        <asp:Label runat="server" ID="lblTotWt" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #CE8686;
                        width: 1075px;">
                        <tr>
                            <td>
                                CUSTOMER
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlCargoCustomer" ClientIDMode="Static" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlCargoCustomer_IndexChange">
                                </asp:DropDownList>
                            </td>
                            <td rowspan="4">
                                SPECIAL INSTRUCTION:
                                <asp:TextBox runat="server" ID="txtCargoSplIns" ClientIDMode="Static" TextMode="MultiLine"
                                    Text="" Width="500px" Height="50px" onKeyUp="javascript:CheckMaxLength(this, 1000);"
                                    onChange="javascript:CheckMaxLength(this, 1000);"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                USER ID
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlCargoUserID" ClientIDMode="Static">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                ORDER REF NO.
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtCargoOrderNo" ClientIDMode="Static" Text=""></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                PLANT
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlCargoPlant" ClientIDMode="Static">
                                    <asp:ListItem Text="SLTL" Value="SLTL"></asp:ListItem>
                                    <asp:ListItem Text="SITL" Value="SITL"></asp:ListItem>
                                    <asp:ListItem Text="MMN" Value="MMN"></asp:ListItem>
                                    <asp:ListItem Text="PDK" Value="PDK"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblProcessID" ClientIDMode="Static" Text=""></asp:Label>
                    <asp:Button runat="server" ID="btnCargoOrderSave" ClientIDMode="Static" Text="ORDER SAVE"
                        OnClick="btnCargoOrderSave_Click" OnClientClick="javascript:return CtrlbtnCargoOrderSave();" />
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnLoadingQty" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnSizePosition" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnTypePosition" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        $(document).ready(function () {
            $(':text').bind('keydown', function (e) { if (e.keyCode == 13) { e.preventDefault(); return false; } });
            $('.ddlExp').change(function () {
                RimAssm_enabledisable();
            });
        });
        function CtrlAddNextItem() {
            var errMsg = ''; $('#lblErrMsg').html('');
            if ($('#ddlCategory option:selected').text() == 'Choose')
                errMsg += 'Choose category<br/>';
            if ($('#ddlPlatform option:selected').text() == 'Choose')
                errMsg += 'Choose platform<br/>';
            if ($('#ddlBrand option:selected').text() == 'Choose')
                errMsg += 'Choose brand<br/>';
            if ($('#ddlSidewall option:selected').text() == 'Choose')
                errMsg += 'Choose sidewall<br/>';
            if ($('#ddlType option:selected').text() == 'Choose')
                errMsg += 'Choose type<br/>';
            if ($('#ddlSize option:selected').text() == 'Choose')
                errMsg += 'Choose size<br/>';
            if ($('#ddlRim option:selected').text() == 'Choose')
                errMsg += 'Choose rim<br/>';

            if ($(chkrimassmbly).is(":checked")) {
                var qty = $('#txtQty').val();
                if (qty != $('#txtRimQty').val())
                    errMsg += 'Enter the rim qty less than r equal to ' + qty + '<br/>';
                if ($('#txtRimWt').val().length == 0)
                    errMsg += 'Enter assembly rim weight<br/>';
                if ($('#txtRimQty').val().length == 0)
                    errMsg += 'Enter assembly rim qty<br/>';
            }
            if ($('#txtQty').val().length == 0)
                errMsg += 'Enter item qty<br/>';
            if ($('#txtFinishedWt').val().length == 0)
                errMsg += 'Enter finished Wt.<br/>';
            if (errMsg.length > 0) {
                $('#lblErrMsg').html(errMsg);
                return false;
            }
            else
                return true;
        }

        function show_ManualOrderCtrl(ctrlID) {
            $('#' + ctrlID).show();
        }

        function bind_errmsg(strErr) {
            $('#lblErrMsg').html(strErr);
        }
        function RimAssm_enabledisable() {
            if ($(chkrimassmbly).is(":checked")) {
                $('.rimassy').attr({ 'disabled': false });
                $('#txtRimQty').val($('#txtQty').val());
            }
            else {
                $('.rimassy').attr({ 'disabled': true });
            }
        }

        function CtrlbtnCargoOrderSave() {
            var errMsg = ''; $('#lblErrMsg').html('');
            if ($('#ddlCargoCustomer option:selected').text() == 'CHOOSE')
                errMsg += 'Choose customer name<br/>';
            if ($('#ddlCargoUserID option:selected').text() == 'CHOOSE')
                errMsg += 'Choose customer user id<br/>';
            if ($('#txtCargoOrderNo').val().length == 0)
                errMsg += 'Enter order ref no.<br/>';
            if ($('#ddlCargoPlant option:selected').text() == 'CHOOSE')
                errMsg += 'Choose plant<br/>';
            if ($('#txtCargoSplIns').val().length == 0)
                errMsg += 'Enter specila instruction<br/>';
            if ($('#lblTotQTy').text().length == 0)
                errMsg += 'Fill the order items';
            if (errMsg.length > 0) {
                $('#lblErrMsg').html(errMsg);
                return false;
            }
            else
                return true;
        }
    </script>
</div>
