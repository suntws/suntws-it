<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="roomreservemaster.aspx.cs" Inherits="TTS.roomreservemaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div align="center" class="pageTitleHead">
        ROOM RESERVATION USER LEVEL</div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <div style="padding-left: 10px; padding-top: 10px;">
            <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #41ACE4;
                width: 60%;" align="center">
                <tr>
                    <td valign="top">
                        <span class="headCss">USER NAME</span>
                        <asp:RadioButtonList runat="server" ID="rdbPrivilegeUserList" RepeatDirection="Horizontal"
                            AutoPostBack="true" OnSelectedIndexChanged="rdbPrivilegeUserList_IndexChange"
                            Width="150px" CellPadding="1" CellSpacing="1" RepeatColumns="1">
                        </asp:RadioButtonList>
                    </td>
                    <td valign="top">
                        <span class="headCss">ROOM NO - ROOM NAME</span>
                        <asp:CheckBoxList runat="server" ID="chkRoomList" ClientIDMode="Static" RepeatColumns="1"
                            RepeatDirection="Vertical" RepeatLayout="Table" Width="300px" CellPadding="1"
                            CellSpacing="1">
                        </asp:CheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="center">
                        <asp:Button runat="server" ID="btnRoomSave" ClientIDMode="Static" Text="SAVE" CssClass="btnsave"
                            OnClick="btnRoomSave_Click" OnClientClick="javascript:return CtrlbtnRoomSave();" />
                        &nbsp; &nbsp;&nbsp;&nbsp; <span class="btnclear" onclick="CtrClear();">CLEAR</span>
                    </td>
                    <td valign="top">
                        <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <script type="text/javascript">
        function CtrlbtnRoomSave() {
            var ErrMsg = ''; $('#lblErrMsg').html('');
            if ($('input:radio[id*=MainContent_rdbPrivilegeUserList_]:checked').length == 0)
                ErrMsg += 'Please select anyone username<br />';
            if ($('#chkRoomList input:checked').length == 0)
                ErrMsg += 'Choose any one room list<br/>';
            if (ErrMsg.length > 0) {
                $('#lblErrMsg').html(ErrMsg);
                return false;
            }
            else
                return true;
        }

        function CtrClear() {
            window.location.href = window.location.href;
        }
    </script>
</asp:Content>
