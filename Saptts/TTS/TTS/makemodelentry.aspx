<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="makemodelentry.aspx.cs" Inherits="TTS.makemodelentry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .mergecss
        {
            text-align: center !important;
            font-size: 12px;
            font-weight: bold;
        }
        .configCss
        {
            background-color: #63D9E9;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        MAKE MODEL ENTRY
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table>
            <tr>
                <td>
                    <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #000;
                        width: 100%; float: left; padding-left: 10px; line-height: 27px; font-weight: bold;">
                        <tr>
                            <td colspan="2">
                                OEM
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtEntryOem" ClientIDMode="Static" Text="" Width="300px"></asp:TextBox>
                            </td>
                            <td>
                                Wheel / Tyresize
                            </td>
                            <td colspan="2">
                                <div style="width: 250px; float: left;">
                                    Front
                                    <asp:TextBox runat="server" ID="txtSizeFront" ClientIDMode="Static" Text="" Width="200px"></asp:TextBox>
                                </div>
                                <div style="width: 250px; float: left;">
                                    Rear
                                    <asp:TextBox runat="server" ID="txtSizeRear" ClientIDMode="Static" Text="" Width="200px"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                Model
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtEntryModel" ClientIDMode="Static" Text="" Width="300px"></asp:TextBox>
                            </td>
                            <td>
                                Nos. Tyres
                            </td>
                            <td>
                                <div style="width: 100px; float: left;">
                                    Front
                                    <asp:TextBox runat="server" ID="txtNosFront" ClientIDMode="Static" Text="" Width="50px"
                                        onkeypress="return isNumberKey(event)" MaxLength="5"></asp:TextBox>
                                </div>
                                <div style="width: 100px; float: left;">
                                    Rear
                                    <asp:TextBox runat="server" ID="txtNosRear" ClientIDMode="Static" Text="" Width="50px"
                                        onkeypress="return isNumberKey(event)" MaxLength="5"></asp:TextBox></div>
                            </td>
                            <td>
                                Tyre Type<asp:TextBox runat="server" ID="txtEntryTyretype" ClientIDMode="Static"
                                    Text="" Width="240px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="2">
                                Lift
                            </td>
                            <td>
                                Type
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtEntryLiftType" ClientIDMode="Static" Text="" Width="300px"></asp:TextBox>
                            </td>
                            <td colspan="3">
                                SUN Availability
                                <asp:TextBox runat="server" ID="txtEntryAvailability" ClientIDMode="Static" Text=""
                                    Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Kg
                            </td>
                            <td>
                                <div style="width: 150px; float: left;">
                                    Weight
                                    <asp:TextBox runat="server" ID="txtEntryWeightKg" ClientIDMode="Static" Text="" Width="100px"></asp:TextBox>
                                </div>
                                <div style="width: 160px; float: left;">
                                    Capacity
                                    <asp:TextBox runat="server" ID="txtEntryCapacityKg" ClientIDMode="Static" Text=""
                                        Width="100px"></asp:TextBox>
                                </div>
                            </td>
                            <td colspan="3" align="right">
                                <asp:Button runat="server" ID="btnSaveNewModel" ClientIDMode="Static" Text="SAVE NEW MODEL DETAILS"
                                    CssClass="btnsave" OnClick="btnSaveNewModel_Click" OnClientClick="javascript:return CtrlbtnSaveNewModel();" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView runat="server" ID="gvMakeModelEntryList" AutoGenerateColumns="false"
                        Width="100%" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#A4F9F5"
                        HeaderStyle-Font-Bold="true" RowStyle-Height="22px" AlternatingRowStyle-BackColor="#FCEFF9"
                        OnRowDataBound="gvMakeModelEntryList_RowDataBound" OnRowCreated="gvMakeModelEntryList_RowCreated"
                        AllowPaging="true" OnPageIndexChanging="gvMakeModelEntryList_PageIndex" PageSize="100"
                        OnRowEditing="gvMakeModelEntryList_RowEditing" OnRowUpdating="gvMakeModelEntryList_RowUpdating"
                        OnRowCancelingEdit="gvMakeModelEntryList_RowCanceling" PagerStyle-Height="30px"
                        PagerStyle-Font-Bold="true" PagerStyle-Font-Size="15px" PagerStyle-HorizontalAlign="Center"
                        PagerStyle-VerticalAlign="Middle">
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="150px" HeaderText="OEM">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblManufacture" Text='<%# Eval("Manufacture") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="80px" HeaderText="MODEL">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblModel" Text='<%# Eval("ModelID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="100px" HeaderText="LIFT TYPE">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblLiftType" Text='<%# Eval("Lift_Type") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="50px" HeaderText="WEIGHT KG">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblLiftWeightKg" Text='<%# Eval("Lift_WeightKg") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="50px" HeaderText="CAPACITY KG">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblLiftCapacityKg" Text='<%# Eval("Lift_CapacityKg") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="90px" HeaderText="FRONT">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblLiftSizeFront" Text='<%# Eval("Wheel_Tyresize_Front") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="90px" HeaderText="REAR">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblLiftSizeRear" Text='<%# Eval("Wheel_Tyresize_Rear") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="40px" HeaderText="FRONT">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblNosFront" Text='<%# Eval("Nos_Tyres_Front") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="40px" HeaderText="REAR">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblNosRear" Text='<%# Eval("Nos_Tyres_Rear") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="80px" HeaderText="TYRE TYPE">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTyreType" Text='<%# Eval("TyreType") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox runat="server" ID="txtTyreType" Text='<%# Eval("TyreType") %>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="100px" HeaderText="AVAILABILITY">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblAvailable" Text='<%# Eval("SunAvailability") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox runat="server" ID="txtAvailable" Text='<%# Eval("SunAvailability") %>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="60px" HeaderText="ACTION">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="gvEdit" Text="EDIT" CommandName="Edit"></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton runat="server" ID="gvUpdate" Text="SAVE" CommandName="Update"></asp:LinkButton>
                                    <asp:LinkButton runat="server" ID="gvCancel" Text="CANCEL" CommandName="Cancel"></asp:LinkButton>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        function CtrlbtnSaveNewModel() {
            var ErrMsg = ''; $('#lblErrMsg').html('');
            if ($('#txtEntryOem').val().length == 0)
                ErrMsg += 'Enter OEM<br/>';
            if ($('#txtEntryModel').val().length == 0)
                ErrMsg += 'Enter Model<br/>';
            if ($('#txtEntryLiftType').val().length == 0 && $('#txtSizeFront').val().length == 0 && $('#txtSizeRear').val().length == 0)
                ErrMsg += 'Enter Lift Type / Wheel / Tyre Size<br/>';

            if (ErrMsg.length > 0) {
                $('#lblErrMsg').html(ErrMsg);
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
