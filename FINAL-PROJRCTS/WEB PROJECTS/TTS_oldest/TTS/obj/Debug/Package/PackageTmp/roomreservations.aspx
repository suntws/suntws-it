<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="roomreservations.aspx.cs" Inherits="TTS.roomreservations" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/datemonthyear.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div align="center" class="pageTitleHead">
        ROOM RESERVATION</div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <div style="padding-left: 10px; padding-top: 10px;">
            <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #41ACE4;
                width: 99%;">
                <tr>
                    <td valign="top" style="width: 50%;">
                        <table cellspacing="5" cellpadding="5" rules="all" border="1" style="border-collapse: collapse;
                            border-color: #41ACE4; width: 100%;">
                            <tr>
                                <td>
                                    DATE
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtReserveDate" ClientIDMode="Static" Text="" Width="80px"></asp:TextBox>
                                </td>
                                <td colspan="2">
                                    TIME : &nbsp;&nbsp;&nbsp; From
                                    <asp:DropDownList runat="server" ID="ddlFromTime" AutoPostBack="true" Width="85px"
                                        ClientIDMode="Static" OnSelectedIndexChanged="ddlFromTime_IndexChange">
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;&nbsp; To
                                    <asp:DropDownList runat="server" ID="ddlToTime" Width="85px" ClientIDMode="Static">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    PURPOSE
                                </td>
                                <td colspan="3">
                                    <asp:TextBox runat="server" ID="txtPurpose" ClientIDMode="Static" Text="" Width="450px"
                                        MaxLength="99"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    REQUIRED SEATS
                                </td>
                                <td colspan="2">
                                    <asp:TextBox runat="server" ID="txtRequiredSeat" ClientIDMode="Static" Text="" Width="50px"
                                        MaxLength="2" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                </td>
                                <td align="center">
                                    <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"
                                        Font-Bold="true"></asp:Label>
                                    <asp:Button runat="server" ID="btnRoomReserveSave" ClientIDMode="Static" Text="SAVE"
                                        CssClass="btnsave" OnClientClick="javascript:return CtrlRoomReserved();" OnClick="btnRoomReserveSave_Click" />
                                    <span onclick="CtrlClearPage();" class="btnclear">CLEAR</span>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td valign="top">
                        <asp:GridView runat="server" ID="gvRoomMasterList" Width="100%" AutoGenerateColumns="false"
                            HeaderStyle-BackColor="#00CCAD">
                            <Columns>
                                <asp:TemplateField ItemStyle-Width="30px">
                                    <ItemTemplate>
                                        <asp:RadioButton ID="rdb1" runat="server" OnClick="javascript:SelectSingleReview(this.id)" />
                                        <asp:HiddenField runat="server" ID="hdn1" Value='<%# Eval("RoomID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ROOM NO" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblRoomNo" Text='<%# Eval("RoomNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ROOM NAME" ItemStyle-Width="300px">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblRoomName" Text='<%# Eval("RoomName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="MAX SEATS" ItemStyle-Width="70px">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblMaxSeat" Text='<%# Eval("MaxSeat") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="background-color: #424040; color: #fff; text-align: center;">
                        <span style="font-weight: bold; font-size: 20px;">RESERVED LIST </span>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:GridView runat="server" ID="gvRoomBookedList" AutoGenerateColumns="false" Width="100%"
                            HeaderStyle-BackColor="#B8ECA7" RowStyle-Height="25px">
                            <Columns>
                                <asp:BoundField DataField="ReservedDate" HeaderText="RESERVED DATE" ItemStyle-Width="50px" />
                                <asp:BoundField DataField="ReservedBy" HeaderText="RESERVED BY" ItemStyle-Width="80px" />
                                <asp:BoundField DataField="RoomNo" HeaderText="ROOM NO" ItemStyle-Width="40px" />
                                <asp:BoundField DataField="RoomName" HeaderText="ROOM NAME" ItemStyle-Width="200px" />
                                <asp:TemplateField HeaderText="TIME" ItemStyle-Width="120px">
                                    <ItemTemplate>
                                        <%# Eval("TimeFrom") %>
                                        &nbsp;-&nbsp;
                                        <%# Eval("TimeTo")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="RequiredSeat" HeaderText="REQUIRED SEATS" ItemStyle-Width="40px" />
                                <asp:BoundField DataField="Purpose" HeaderText="PURPOSE" ItemStyle-Width="250px" />
                                <asp:TemplateField HeaderText="ACTION" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <%# Bind_CancelledButton(Eval("ReservedDate").ToString(), Eval("ReservedBy").ToString(), Eval("RoomID").ToString(), Eval("FSno").ToString(), Eval("TSno").ToString())%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdnRoomId" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnRoomNo" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnRoomName" ClientIDMode="Static" Value="" />
    <script src="Scripts/datemonthyear.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#txtReserveDate").datepicker({
                minDate: "+1D", maxDate: "+60D"
            }).keydown(function (e) {
                e.keyWhich = 0; e.keyCode = 0; e.preventDefault();
            });
        });

        function CtrlRoomReserved() {
            $('#lblErrMsg').html('');
            var ErrMsg = '';
            if ($('#txtReserveDate').val().length == 0)
                ErrMsg += 'Choose date<br/>';
            if ($('#ddlFromTime option:selected').text() == 'CHOOSE')
                ErrMsg += 'Choose from time<br/>';
            if ($('#ddlToTime option:selected').text() == 'CHOOSE')
                ErrMsg += 'Choose to time<br/>';
            if ($('#txtPurpose').val().length == 0)
                ErrMsg += 'Enter purpose<br/>';
            if ($('#txtRequiredSeat').val().length == 0)
                ErrMsg += 'Enter required seats<br/>';
            if ($('input:radio[id*=MainContent_gvRoomMasterList_rdb1_]:checked').length == 0)
                ErrMsg += 'Choose anyone room<br/>';
            else if ($('input:radio[id*=MainContent_gvRoomMasterList_rdb1_]:checked').length > 0) {
                var lblMaxSeatID = $('input:radio[id*=MainContent_gvRoomMasterList_rdb1_]:checked').attr('id');
                var lblMaxSeat = lblMaxSeatID.replace('_rdb1_', '_lblMaxSeat_');
                if (parseInt($('#' + lblMaxSeat).html()) < parseInt($('#txtRequiredSeat').val()))
                    ErrMsg += 'Maximum ' + $('#' + lblMaxSeat).html() + ' seat only available<br/>';
                else {
                    var hdnMainID = $('input:radio[id*=MainContent_gvRoomMasterList_rdb1_]:checked').attr('id');
                    var hdnID = hdnMainID.replace('_rdb1_', '_hdn1_');
                    var lblRoomNo = hdnMainID.replace('_rdb1_', '_lblRoomNo_');
                    var lblRoomName = hdnMainID.replace('_rdb1_', '_lblRoomName_');
                    $('#hdnRoomId').val($('#' + hdnID).val());
                    $('#hdnRoomNo').val($('#' + lblRoomNo).html());
                    $('#hdnRoomName').val($('#' + lblRoomName).html());
                }
            }

            if (ErrMsg.length > 0) {
                $('#lblErrMsg').html(ErrMsg);
                return false;
            }
            else
                return true;
        }

        function CtrlClearPage() {
            window.location.href = window.location.href;
        }

        function SelectSingleReview(rdBtnID) {
            var rduser = $(document.getElementById(rdBtnID));
            rduser.closest('TR').addClass('SelectedRowStyle');
            rduser.checked = true;
            var list = rduser.closest('table').find("INPUT[type='radio']").not(rduser);
            list.attr('checked', false);
            list.closest('TR').removeClass('SelectedRowStyle');
        }

        function cancel_ReservedRoom(strRDate, strRName, strRoomID, strFSno, strTSno) {
            $.ajax({ type: "POST", url: "BindRecords.aspx?type=Cancel_RoomReserve&rDate=" + strRDate + "&rName=" + strRName + "&rRoom=" + strRoomID + "&rSno=" + strFSno + "&rTno=" + strTSno + "", context: document.body,
                success: function (data) {
                    if (data == 'SUCCESS') {
                        window.location.href = window.location.href;
                    }
                    else {
                        alert(data);
                    }
                }
            });
        }
    </script>
</asp:Content>
