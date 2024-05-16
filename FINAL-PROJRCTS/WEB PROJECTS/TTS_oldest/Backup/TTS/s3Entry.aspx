<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="s3Entry.aspx.cs" Inherits="TTS._s3Entry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        3S NETWORK DATA ENTRY</div><div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">

        <div style="padding-left: 10px; padding-top: 10px;">
            <div id="divErrMsg">
            </div>
            <table id="s3NewCustomer" style="display: block;">
                <tr>
                    <td>
                        <div class="s3headDiv">
                            3s Network New Entry
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="line-height: 40px;">
                        <div style="width: 500px; float: left;">
                            <div>
                                <span class="headCss" style="width: 100px; float: left;">Name </span>
                                <asp:TextBox runat="server" ID="txt3sName" ClientIDMode="Static" Text="" Width="350px"></asp:TextBox>
                            </div>
                            <div>
                                <span class="headCss" style="width: 100px; float: left;">State </span>
                                <asp:DropDownList runat="server" ID="ddl3sState" ClientIDMode="Static" Width="350px"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddl3sState_IndexChanged">
                                </asp:DropDownList>
                                <span style="float: left; display: none; line-height: 10px;" id="divState"><span
                                    style="width: 100px; float: left; color: #ff0000;">Enter New State</span>
                                    <asp:TextBox runat="server" ID="txt3sState" ClientIDMode="Static" Text="" Width="350px"></asp:TextBox>
                                </span>
                            </div>
                            <div>
                                <span style="display: block;" id="div3sExistCity"><span class="headCss" style="width: 100px;
                                    float: left;">City </span>
                                    <asp:DropDownList runat="server" ID="ddl3sCity" ClientIDMode="Static" Width="350px"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddl3sCity_IndexChange">
                                    </asp:DropDownList>
                                </span><span style="float: left; display: none;" id="divCity"><span style="width: 100px;
                                    float: left; color: #ff0000;">Enter New City</span>
                                    <asp:TextBox runat="server" ID="txt3sCity" ClientIDMode="Static" Text="" Width="350px"></asp:TextBox>
                                </span>
                            </div>
                            <div>
                                <span class="headCss" style="width: 100px; float: left;">Zone </span>
                                <asp:TextBox runat="server" ID="txt3sZone" ClientIDMode="Static" Text="" Width="350px"></asp:TextBox>
                            </div>
                        </div>
                        <div style="width: 550px; float: left;">
                            <div>
                                <span class="headCss" style="float: left; width: 100px;">Category </span>
                                <asp:DropDownList runat="server" ID="ddl3sCategory" ClientIDMode="Static" Width="350px">
                                </asp:DropDownList>
                            </div>
                            <div>
                                <span class="headCss" style="float: left; width: 100px;">Spl Instruction </span>
                                <asp:TextBox runat="server" ID="txt3sComments" ClientIDMode="Static" Text="" Width="400px"
                                    TextMode="MultiLine" Height="100px"></asp:TextBox>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center; padding-left: 300px;">
                        <div style="width: 500px;">
                            <asp:Button runat="server" ID="btn3sSave" ClientIDMode="Static" Text="SAVE" CssClass="btnsave"
                                OnClientClick="javascript:return ctrl3sValidate();" OnClick="btn3sSave_Click" />
                            <span class="btnclear" style="margin-left: 50px;" onclick="btn3sNewEntryClear();">CLEAR</span>
                        </div>
                    </td>
                </tr>
            </table>
            <table id="s3ActivateChange" style="display: none;">
                <tr>
                    <td>
                        <div class="s3headDiv">
                            3s Network Activity Changes
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>
                            <div style="width: 500px; float: left;">
                                <asp:Repeater runat="server" ID="rpt3sCustDetails">
                                    <ItemTemplate>
                                        <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #07A714;
                                            margin-top: 20px; line-height: 35px; width: 400px;">
                                            <tr style="text-align: center; font-weight: bold; font-size: 15px; background-color: #E7FCA7;">
                                                <td style="text-align: left;">
                                                    <div>
                                                        <%# Eval("CustName")%>
                                                    </div>
                                                    <div>
                                                        <%# Eval("StateName")%>
                                                    </div>
                                                    <div>
                                                        <%# Eval("City")%>
                                                    </div>
                                                    <div>
                                                        <%# Eval("Zone")%>
                                                    </div>
                                                    <div>
                                                        <%# Eval("CategoryName")%>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                            <div style="width: 530px; float: left; line-height: 35px; padding-left: 20px;">
                                <div id="adminchangesdiv" style="display: none;">
                                    <div style="width: 500px; float: left;">
                                        <span class="headCss" style="width: 100px; float: left;">Category From</span>
                                        <asp:TextBox runat="server" ID="txt3sPrevCategory" Text="" ClientIDMode="Static"
                                            Width="350px" Enabled="false"></asp:TextBox>
                                    </div>
                                    <div style="width: 500px; float: left;">
                                        <span class="headCss" style="width: 100px; float: left;">To</span>
                                        <asp:DropDownList runat="server" ID="ddl3sChangedCategory" ClientIDMode="Static"
                                            Width="350px">
                                        </asp:DropDownList>
                                    </div>
                                    <div style="width: 500px; float: left;">
                                        <span class="headCss" style="width: 100px; float: left;">Tier</span>
                                        <asp:DropDownList runat="server" ID="ddl3sStatus" ClientIDMode="Static" Width="350px">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div style="width: 500px; float: left;">
                                    <span class="headCss" style="width: 100px; float: left;">Spl Instruction</span>
                                    <asp:TextBox runat="server" ID="txt3sChangedComments" Text="" ClientIDMode="Static"
                                        Width="390px" Height="100px" TextMode="MultiLine"></asp:TextBox>
                                </div>
                                <div style="width: 500px; float: left; text-align: center;">
                                    <asp:Button runat="server" ID="btn3sChange" ClientIDMode="Static" Text="SAVE" CssClass="btnactive"
                                        OnClientClick="javascript:return ctrl3sChangeValidate();" OnClick="btn3sChange_Click" />
                                    <span class="btnnull" style="margin-left: 50px; line-height: 20px;" onclick="gotoDashboard();">
                                        GO TO BACK</span>
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div style="width: 1050px; float: left; padding-left: 10px; padding-top: 20px; line-height: 30px;">
                            <asp:GridView runat="server" ID="gv_3sChangeHistory" AutoGenerateColumns="false"
                                AlternatingRowStyle-BackColor="#DDD5D5" Width="1000px" HeaderStyle-BackColor="#BCDFF3"
                                Font-Bold="true" RowStyle-BackColor="#F2F3F3">
                                <Columns>
                                    <asp:BoundField HeaderText="History Date" DataField="RecDate" ItemStyle-Width="100" />
                                    <asp:BoundField HeaderText="From Category" DataField="fromcategory" ItemStyle-Width="200" />
                                    <asp:BoundField HeaderText="To Category" DataField="tocategory" ItemStyle-Width="200" />
                                    <asp:BoundField HeaderText="Tier" DataField="statusname" ItemStyle-Width="300" />
                                    <asp:TemplateField HeaderText="Feedback" ItemStyle-Width="500">
                                        <ItemTemplate>
                                            <%# Bind_Feedback(Eval("Comments").ToString(), Eval("UserName").ToString())%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdnStateName" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnCityName" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnCustId" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnCustName" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnCategoryID" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnCustType" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        function makedivdisplay(qKey) {
            $('#s3NewCustomer').css({ 'display': 'none' });
            $('#s3ActivateChange').css({ 'display': 'none' });
            if (qKey == '0')
                $('#s3NewCustomer').css({ 'display': 'block' });
            else if (parseInt(qKey) > 0) {
                $('#s3ActivateChange').css({ 'display': 'block' });
                if ($('#hdnCustType').val() == 'admin')
                    $('#adminchangesdiv').css({ 'display': 'block' });
            }
        }

        function AddNew3sState() {
            $('#divState').css({ 'display': 'none' }); $('#divCity').css({ 'display': 'none' }); $('#div3sExistCity').css({ 'display': 'block' });
            $('#hdnStateName').val($("#ddl3sState option:selected").text())
            if ($('#ddl3sState option:selected').text() == 'Add New State') {
                $('#divState').css({ 'display': 'block' });
                $('#divCity').css({ 'display': 'block' });
                $('#hdnCityName').val('Add New City');
                $('#div3sExistCity').css({ 'display': 'none' });
            }
        }

        function AddNew3sCity() {
            $('#divCity').css({ 'display': 'none' });
            $('#hdnCityName').val($("#ddl3sCity option:selected").text())
            if ($('#ddl3sCity option:selected').text() == 'Add New City')
                $('#divCity').css({ 'display': 'block' });
        }

        function btn3sNewEntryClear() {
            window.location.href = window.location.href;
        }

        function gotoDashboard() {
            var pathname = window.location.href.toLowerCase();
            var splitval = pathname.split('/s3entry.aspx');
            window.location.href = splitval[0].toString() + '/s3dashboard.aspx';
        }

        function ctrl3sValidate() {
            $('#divErrMsg').html(''); var errmsg = '';
            if ($('#txt3sName').val().length == 0)
                errmsg += 'Enter Customer Name <br/>';
            if ($('#ddl3sState option:selected').text() == 'Choose' && $('#hdnStateName').val() != 'Add New State')
                errmsg += 'Choose State <br/>';
            else if ($('#hdnStateName').val() == 'Add New State') {
                if ($('#txt3sState').val().length == 0)
                    errmsg += 'Enter New State <br/>';
            }
            if ($('#ddl3sCity option:selected').text() == 'Choose' && $('#hdnCityName').val() != 'Add New City')
                errmsg += 'Choose City <br/>';
            else if ($('#hdnCityName').val() == 'Add New City') {
                if ($('#txt3sCity').val().length == 0)
                    errmsg += 'Enter New City <br/>';
            }
            if ($('#txt3sZone').val().length == 0)
                errmsg += 'Enter Zone <br/>';
            if ($('#ddl3sCategory option:selected').text() == 'Choose')
                errmsg += 'Choose Category <br/>';

            if (errmsg.length > 0) {
                $('#divErrMsg').html(errmsg);
                $('#divErrMsg').attr("style", "color:red");
                return false;
            }
            else
                return true;
        }

        function ctrl3sChangeValidate() {
            $('#divErrMsg').html('');
            errmsg = '';
            if ($('#txt3sPrevCategory').val().length == 0)
                errmsg += 'Invalid previous category<br/>';
            if ($('#ddl3sChangedCategory option:selected').text() == 'Choose')
                errmsg += 'Choose customer next category<br/>';

            if (errmsg.length > 0) {
                $('#divErrMsg').html(errmsg);
                $('#divErrMsg').attr("style", "color:red");
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
