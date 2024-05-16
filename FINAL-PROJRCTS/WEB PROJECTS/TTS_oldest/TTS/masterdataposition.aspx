<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="masterdataposition.aspx.cs"
    MasterPageFile="~/master.Master" Inherits="TTS.masterdataposition" %>

<asp:Content ID="headcontent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/datemonthyear.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        Type Ascending Position
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table style="width: 100%;">
            <tr>
                <td>
                    <asp:GridView ID="gvTypeDetails" runat="server" Width="1074px" AutoGenerateColumns="false"
                        OnRowDataBound="gvTypeDetails_RowDataBound" OnRowCancelingEdit="gvTypeDetails_RowCancelingEdit"
                        OnRowUpdating="gvTypeDetails_RowUpdating" OnRowEditing="gvTypeDetails_RowEditing">
                        <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" HorizontalAlign="Center" VerticalAlign="Middle" />
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="30px" HeaderText="TYPE">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblType" CssClass="rptLbl" Text='<%# Eval("type") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="30px" HeaderText="LIP">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblLip" CssClass="rptLbl" Text='<%# Eval("lipgum") %>'></asp:Label></ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList runat="server" ID="ddlLip" ClientIDMode="Static" CssClass="rptDdl">
                                    </asp:DropDownList>
                                    <asp:TextBox runat="server" ID="txtLip" ClientIDMode="Static" CssClass="rptTxt" Text='<%# Eval("lipgum") %>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="30px" HeaderText="LIP %">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblLipPer" CssClass="rptLbl" Text='<%# Eval("lipgumper") %>'></asp:Label></ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox runat="server" ID="txtLipPer" ClientIDMode="Static" CssClass="rptTxtNum"
                                        Text='<%# Eval("lipgumper") %>' onkeypress="return isNumberKey(event)" MaxLength="3"></asp:TextBox></EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="30px" HeaderText="BASE">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblBase" CssClass="rptLbl" Text='<%# Eval("base") %>'></asp:Label></ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList runat="server" ID="ddlBase" ClientIDMode="Static" CssClass="rptDdl">
                                    </asp:DropDownList>
                                    <asp:TextBox runat="server" ID="txtBase" ClientIDMode="Static" CssClass="rptTxt"
                                        Text='<%# Eval("base") %>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="30px" HeaderText="BASE %">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblBasePer" CssClass="rptLbl" Text='<%# Eval("baseper") %>'></asp:Label></ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox runat="server" ID="txtBasePer" ClientIDMode="Static" CssClass="rptTxtNum"
                                        Text='<%# Eval("baseper") %>' onkeypress="return isNumberKey(event)" MaxLength="3"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="30px" HeaderText="INTERFACE">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblInterface" CssClass="rptLbl" Text='<%# Eval("Interface") %>'></asp:Label></ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList runat="server" ID="ddlInterface" ClientIDMode="Static" CssClass="rptDdl">
                                    </asp:DropDownList>
                                    <asp:TextBox runat="server" ID="txtInterface" ClientIDMode="Static" CssClass="rptTxt"
                                        Text='<%#Eval("Interface") %>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="30px" HeaderText="INTERFACE %">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblInterfacePer" CssClass="rptLbl" Text='<%# Eval("InterfacePer") %>'></asp:Label></ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox runat="server" ID="txtInterfacePer" ClientIDMode="Static" CssClass="rptTxtNum"
                                        Text='<%# Eval("InterfacePer") %>' onkeypress="return isNumberKey(event)" MaxLength="3"></asp:TextBox></EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="30px" HeaderText="CENTER">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCenter" CssClass="rptLbl" Text='<%# Eval("center") %>'></asp:Label></ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList runat="server" ID="ddlCenter" ClientIDMode="Static" CssClass="rptDdl">
                                    </asp:DropDownList>
                                    <asp:TextBox runat="server" ID="txtCenter" ClientIDMode="Static" CssClass="rptTxt"
                                        Text='<%# Eval("center") %>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="30px" HeaderText="CENTER %">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCenterPer" CssClass="rptLbl" Text='<%# Eval("centerper") %>'></asp:Label></ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox runat="server" ID="txtCenterPer" ClientIDMode="Static" CssClass="rptTxtNum"
                                        Text='<%# Eval( "centerper") %>' onkeypress="return isNumberKey(event)" MaxLength="3"></asp:TextBox></EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="30px" HeaderText="TREAD">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTread" CssClass="rptLbl" Text='<%# Eval("tread") %>'></asp:Label></ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList runat="server" ID="ddlTread" ClientIDMode="Static" CssClass="rptDdl">
                                    </asp:DropDownList>
                                    <asp:TextBox runat="server" ID="txtTread" ClientIDMode="Static" CssClass="rptTxt"
                                        Text='<%# Eval("tread") %>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="30px" HeaderText="TREAD %">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTreadPer" CssClass="rptLbl" Text='<%# Eval("treadper") %>'></asp:Label></ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox runat="server" ID="txtTreadPer" ClientIDMode="Static" CssClass="rptTxtNum"
                                        Text='<%#Eval("treadper") %>' onkeypress="return isNumberKey(event)" MaxLength="3"></asp:TextBox></EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="30px" HeaderText="POSITION">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTypePosition" CssClass="rptLbl" Text='<%# Eval("TypePosition") %>'></asp:Label></ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox runat="server" ID="txtTypePosition" ClientIDMode="Static" CssClass="rptTxtNum"
                                        Text='<%# Eval("TypePosition") %>' onkeypress="return isNumberKey(event)" MaxLength="5"></asp:TextBox></EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="100px" HeaderText="DESCRIPTION">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTypeDesc" Text='<%# Eval("TypeDesc") %>'></asp:Label></ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox runat="server" ID="txtTypeDesc" ClientIDMode="Static" Width="150px"
                                        CssClass="rptTxtDesc" Text='<%# Eval("TypeDesc") %>' MaxLength="100"></asp:TextBox></EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="40px" HeaderText="ACTION">
                                <ItemTemplate>
                                    <asp:Button runat="server" ID="lnkEdit" Text="Edit" CommandName="Edit" /></ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Button runat="server" ID="lnkUpdate" Text="Update" CommandName="Update" OnClientClick="javascript:return calcPer(this);" />
                                    <asp:Button runat="server" ID="lnkCancel" Text="Cancel" CommandName="Cancel" /></EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#ddlLip').change(function () {
                $('#txtLip').css('display', 'none').val('');
                if ($('#ddlLip option:selected').text() != "CHOOSE" && $('#ddlLip option:selected').text() == "ADD NEW")
                    $('#txtLip').css('display', 'block');
                else if ($('#ddlLip option:selected').text() != "CHOOSE" && $('#ddlLip option:selected').text() != "ADD NEW")
                    $('#txtLip').val($('#ddlLip option:selected').text());
            });
            $('#ddlBase').change(function () {
                $('#txtBase').css('display', 'none').val('');
                if ($('#ddlBase option:selected').text() != "CHOOSE" && $('#ddlBase option:selected').text() == "ADD NEW")
                    $('#txtBase').css('display', 'block');
                else if ($('#ddlBase option:selected').text() != "CHOOSE" && $('#ddlBase option:selected').text() != "ADD NEW")
                    $('#txtBase').val($('#ddlBase option:selected').text());
            });
            $('#ddlCenter').change(function () {
                $('#txtCenter').css('display', 'none').val('');
                if ($('#ddlCenter option:selected').text() != "CHOOSE" && $('#ddlCenter option:selected').text() == "ADD NEW")
                    $('#txtCenter').css('display', 'block');
                else if ($('#ddlCenter option:selected').text() != "CHOOSE" && $('#ddlCenter option:selected').text() != "ADD NEW")
                    $('#txtCenter').val($('#ddlCenter option:selected').text());
            });
            $('#ddlTread').change(function () {
                $('#txtTread').css('display', 'none').val('');
                if ($('#ddlTread option:selected').text() != "CHOOSE" && $('#ddlTread option:selected').text() == "ADD NEW")
                    $('#txtTread').css('display', 'block');
                else if ($('#ddlTread option:selected').text() != "CHOOSE" && $('#ddlTread option:selected').text() != "ADD NEW")
                    $('#txtTread').val($('#ddlTread option:selected').text());
            });
            $('#ddlInterface').change(function () {
                $('#txtInterface').css('display', 'none').val('');
                if ($('#ddlInterface option:selected').text() != "CHOOSE" && $('#ddlInterface option:selected').text() == "ADD NEW")
                    $('#txtInterface').css('display', 'block');
                else if ($('#ddlInterface option:selected').text() != "CHOOSE" && $('#ddlInterface option:selected').text() != "ADD NEW")
                    $('#txtInterface').val($('#ddlInterface option:selected').text());
            });
        });
        function calcPer() {
            var ErrMsg = '';
            if ($('#ddlLip option:selected').text() == "CHOOSE" && $('#txtLipPer').val().length > 0)
                ErrMsg += 'choose lip code<br/>';
            else if ($('#ddlLip option:selected').text() != "CHOOSE" && $('#ddlLip option:selected').text() != "ADD NEW" && $('#txtLipPer').val().length == 0)
                ErrMsg += 'Enter lip %<br/>';
            else if ($('#ddlLip option:selected').text() == "ADD NEW" && $('#txtLip').val().length == 0)
                ErrMsg += 'Enter lip code<br/>';
            if ($('#ddlBase option:selected').text() == "CHOOSE" && $('#txtBasePer').val().length > 0)
                ErrMsg += 'choose base code<br/>';
            else if ($('#ddlBase option:selected').text() != "CHOOSE" && $('#ddlBase option:selected').text() != "ADD NEW" && $('#txtBasePer').val().length == 0)
                ErrMsg += 'Enter base %<br/>';
            else if ($('#ddlBase option:selected').text() == "ADD NEW" && $('#txtBase').val().length == 0)
                ErrMsg += 'Enter base code<br/>';
            if ($('#ddlInterface option:selected').text() == "CHOOSE" && $('#txtInterface').val().length > 0)
                ErrMsg += 'choose Interface code<br/>';
            else if ($('#ddlInterface option:selected').text() != "CHOOSE" && $('#ddlInterface option:selected').text() != "ADD NEW" && $('#txtInterface').val().length == 0)
                ErrMsg += 'Enter Interface %<br/>';
            else if ($('#ddlInterface option:selected').text() == "ADD NEW" && $('#txtInterface').val().length == 0)
                ErrMsg += 'Enter Interface code<br/>';
            if ($('#ddlCenter option:selected').text() == "CHOOSE" && $('#txtCenterPer').val().length > 0)
                ErrMsg += 'choose center code<br/>';
            else if ($('#ddlCenter option:selected').text() != "CHOOSE" && $('#ddlCenter option:selected').text() != "ADD NEW" && $('#txtCenterPer').val().length == 0)
                ErrMsg += 'Enter center %<br/>';
            else if ($('#ddlCenter option:selected').text() == "ADD NEW" && $('#txtCenter').val().length == 0)
                ErrMsg += 'Enter center code<br/>';
            if ($('#ddlTread option:selected').text() == "CHOOSE" && $('#txtTreadPer').val().length > 0)
                ErrMsg += 'choose tread code<br/>';
            else if ($('#ddlTread option:selected').text() != "CHOOSE" && $('#ddlTread option:selected').text() != "ADD NEW" && $('#txtTreadPer').val().length == 0)
                ErrMsg += 'Enter tread %<br/>';
            else if ($('#ddlTread option:selected').text() == "ADD NEW" && $('#txtTread').val().length == 0)
                ErrMsg += 'Enter tread code<br/>';
            if ($('#txtTypePosition').val().length == 0)
                ErrMsg += 'Enter Type Position Value<br/>'
            if ($('#txtTypePosition').val().length == 0)
                ErrMsg += 'Enter Type Position Value<br/>'

            var strlip = $("input[id*='txtLipPer']").val();
            var strbase = $("input[id*='txtBasePer']").val();
            var strcenter = $("input[id*='txtCenterPer']").val();
            var strtread = $("input[id*='txtTreadPer']").val();
            var strInterface = $("input[id*='InterfacePer']").val();
            strlip = strlip.length == 0 ? 0 : strlip;
            strbase = strbase.length == 0 ? 0 : strbase;
            strcenter = strcenter.length == 0 ? 0 : strcenter;
            strtread = strtread.length == 0 ? 0 : strtread;
            strInterface = strInterface.length == 0 ? 0 : strInterface;
            var totPer = parseInt(strlip) + parseInt(strbase) + parseInt(strcenter) + parseInt(strtread) + parseInt(strInterface);
            if (parseInt(totPer) < 100 || parseInt(totPer) > 100) {
                if (parseInt(totPer) != 0)
                    ErrMsg += 'Total percentage should be 100. Enter Correct Values'
            }
            if (ErrMsg.length > 0) {
                alert(ErrMsg);
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
