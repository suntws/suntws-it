<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="typeHardnessRange.aspx.cs" Inherits="TTS.typeHardnessRange" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/cotsScript.js" type="text/javascript"></script>
    <style type="text/css">
        .tableHard
        {
            border-collapse: collapse;
            border-color: #000;
            width: 100%;
            line-height: 25px;
        }
        .tableHard th
        {
            background-color: #FFEEEC;
            text-align: right;
            width: 120px;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div align="center" class="pageTitleHead">
        TYPE HARDNESS RANGE</div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" class="tableHard">
            <tr>
                <th rowspan="2">
                    TYPE
                </th>
                <td rowspan="2">
                    <asp:DropDownList runat="server" ID="ddlType" ClientIDMode="Static" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlType_IndexChange" Width="150px" CssClass="form-control">
                    </asp:DropDownList>
                </td>
                <th rowspan="2">
                    HARDNESS RANGE
                </th>
                <th>
                    BASE FROM
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtFbase" Text="0" ClientIDMode="Static" Width="80px"
                        MaxLength="3" onkeypress="return isNumberWithoutDecimal(event)" CssClass="form-control"></asp:TextBox>
                </td>
                <th>
                    TO
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtTbase" Text="0" ClientIDMode="Static" Width="80px"
                        MaxLength="3" onkeypress="return isNumberWithoutDecimal(event)" CssClass="form-control"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    TREAD FROM
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtFtread" Text="0" ClientIDMode="Static" Width="80px"
                        MaxLength="3" onkeypress="return isNumberWithoutDecimal(event)" CssClass="form-control"></asp:TextBox>
                </td>
                <th>
                    TO
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtTtread" Text="0" ClientIDMode="Static" Width="80px"
                        MaxLength="3" onkeypress="return isNumberWithoutDecimal(event)" CssClass="form-control"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <div style="float: left; width: 500px; line-height: 15px; color: #ff0000; font-weight: bold;
                        text-align: right;" id="divErrMsg">
                    </div>
                </td>
                <td colspan="3" style="text-align: center;">
                    <asp:LinkButton runat="server" ID="lnkSave" CssClass="btnsave" ClientIDMode="Static"
                        OnClientClick="javascript:return RangeCtrlValidation();" OnClick="lnkSave_click"
                        Text="Save" Width="50px"></asp:LinkButton>
                </td>
                <td colspan="2" style="text-align: center;">
                    <span class="btnclear" id="lnkClear" style="cursor: pointer; width: 50px;">Clear</span>
                </td>
            </tr>
            <tr>
                <td colspan="3" style="line-height: 15px; vertical-align: top; padding-top: 5px;">
                    <asp:Label runat="server" ID="lblExists" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                    <asp:DataGrid runat="server" ID="grdExist" ClientIDMode="Static" Width="100%" AutoGenerateColumns="false"
                        HeaderStyle-BackColor="#49f5b5" ItemStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center">
                        <Columns>
                            <asp:BoundColumn DataField="TyreType" HeaderText="TYPE"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BaseFromRange" HeaderText="BASE FROM" ItemStyle-HorizontalAlign="Center">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="BaseToRange" HeaderText="BASE TO" ItemStyle-HorizontalAlign="Center">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="TreadFromRange" HeaderText="TREAD FROM" ItemStyle-HorizontalAlign="Center">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="TreadToRange" HeaderText="TREAD TO" ItemStyle-HorizontalAlign="Center">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="CreateDate" HeaderText="CREATED ON"></asp:BoundColumn>
                            <asp:BoundColumn DataField="UserName" HeaderText="CREATED BY"></asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
                <td colspan="5">
                    <asp:Label runat="server" ID="lblHistory" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                    <asp:DataGrid runat="server" ID="grdHistory" ClientIDMode="Static" Width="100%" AutoGenerateColumns="false"
                        HeaderStyle-BackColor="#83f549" ItemStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center">
                        <Columns>
                            <asp:BoundColumn DataField="TyreType" HeaderText="TYPE"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BaseFromRange" HeaderText="BASE FROM" ItemStyle-HorizontalAlign="Center">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="BaseToRange" HeaderText="BASE TO" ItemStyle-HorizontalAlign="Center">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="TreadFromRange" HeaderText="TREAD FROM" ItemStyle-HorizontalAlign="Center">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="TreadToRange" HeaderText="TREAD TO" ItemStyle-HorizontalAlign="Center">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="CreateDate" HeaderText="CREATED ON"></asp:BoundColumn>
                            <asp:BoundColumn DataField="UserName" HeaderText="CREATED BY"></asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#lnkClear').click(function () { window.location.href = window.location.href; });
        });

        function RangeCtrlValidation() {
            var msg = '';
            if ($('#grdExist tr:eq(1)').find('td:eq(1)').html() == $('#txtFbase').val() && $('#grdExist tr:eq(1)').find('td:eq(2)').html() == $('#txtTbase').val()
            && $('#grdExist tr:eq(1)').find('td:eq(3)').html() == $('#txtFtread').val() && $('#grdExist tr:eq(1)').find('td:eq(4)').html() == $('#txtTtread').val()) {
                msg += "Same hardness range is already exists<br />"
            }
            else {
                if ($('#ddlType option:selected').val() == "CHOOSE")
                    msg += "Choose tyre type<br />"
                if ($('#txtFbase').val() == "" || $('#txtTbase').val() == "")
                    msg += "Enter base hardness from/to range<br />";
                else if (parseInt($('#txtFbase').val()) > parseInt($('#txtTbase').val()))
                    msg += "Enter base [From] hardness range less then or equal to [To] range<br />";
                if ($('#txtFtread').val() == "" || $('#txtTtread').val() == "")
                    msg += "Enter tread hardness from/to range<br />";
                else if (parseInt($('#txtFtread').val()) > parseInt($('#txtTtread').val()))
                    msg += "Enter tread [From] hardness range less then or equal to [To] range<br />";
                if (parseInt($('#txtFbase').val()) == 0 && parseInt($('#txtTbase').val()) == 0 && parseInt($('#txtFtread').val()) == 0 && parseInt($('#txtTtread').val()) == 0)
                    msg += "Enter anyone [To] range greater then zero.<br />";
            }
            if (msg.length > 0) {
                $('#divErrMsg').html(msg);
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
