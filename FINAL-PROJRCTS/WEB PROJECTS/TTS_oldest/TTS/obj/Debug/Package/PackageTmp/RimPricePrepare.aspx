<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="RimPricePrepare.aspx.cs" Inherits="TTS.RimPricePrepare" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
    <link href="Styles/datemonthyear.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/cotsScript.js" type="text/javascript"></script>
    <style type="text/css">
        tr
        {
            height: 30px;
        }
        th
        {
            width: 270px;
            line-height: 15px;
            text-align: right;
            font-weight: normal;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center" class="pageTitleHead">
        <asp:Label runat="server" ID="lblPageTitle" ClientIDMode="Static" Text=""></asp:Label>
        RIM LIST PRICE ENTRY
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div class="contPage">
        <table cellspacing="0" rules="all" border="0" style="background-color: #C0FDBE; width: 100%;
            border-color: #C0FDBE; border-collapse: separate;">
            <tr>
                <th>
                    CUSTOMER
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="ddlCustomer" ClientIDMode="Static" AutoPostBack="true"
                        Width="400px" CssClass="form-control" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    EDCNO
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="ddl_Edcno" ClientIDMode="Static" AutoPostBack="true"
                        Width="150px" CssClass="form-control" OnSelectedIndexChanged="ddl_Edcno_IndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    WEIGHT
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txt_RimWeight" ClientIDMode="Static" CssClass="form-control"
                        onkeypress="return isNumberKey(event)" MaxLength="7" Width="100px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    PRICE
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txt_RimPrice" ClientIDMode="Static" CssClass="form-control"
                        onkeypress="return isNumberKey(event)" MaxLength="7" Width="100px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    VALIDITY DATE
                </th>
                <td>
                    <asp:TextBox ID="txt_ValDate" ClientIDMode="Static" runat="server" Width="150px"
                        ToolTip="Select Desired Shipping Date" CssClass="form-control"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="btnSaveRimPriceDetails" runat="server" Text="SAVE" ClientIDMode="Static"
                        CssClass="btn btn-success" Font-Bold="true" OnClientClick="javascript:return CntrlSave();"
                        OnClick="btnSaveRimPriceDetails_Click" />
                    <asp:Button ID="btn_clear" runat="server" Text="CLEAR" ClientIDMode="Static" CssClass="btn btn-danger"
                        Font-Bold="true" OnClick="btn_clear_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:GridView runat="server" ID="gvRimPrice" AutoGenerateColumns="false" Width="100%"
                        CssClass="gridcss">
                        <Columns>
                            <asp:BoundField DataField="EDCNO" HeaderText="EDC NO" />
                            <asp:BoundField DataField="RimWeight" HeaderText="WT" />
                            <asp:BoundField DataField="RimPrice" HeaderText="PRICE  " />
                            <asp:BoundField DataField="EndDate" HeaderText="END DATE" />
                            <asp:BoundField DataField="UserName" HeaderText="CREATED BY" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
    <script src="Scripts/datemonthyear.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#txt_ValDate").datepicker({ minDate: "+1D", maxDate: "+90D" }).keydown(function (e) { e.keyWhich = 0; e.keyCode = 0; e.preventDefault(); });
        });
        function CntrlSave() {
            var ErrMsg = "";
            $('#lblErrMsg1').html('');
            if ($("#ddlCustomer option:selected").text() == "CHOOSE")
                ErrMsg += 'Choose Customer Name \n';
            if ($("#ddl_Edcno option:selected").text() == "CHOOSE" || $("#ddl_Edcno option:selected").text() == "")
                ErrMsg += "Choose EDCNO \n"
            if ($('#txt_RimWeight').val() == "")
                ErrMsg += "Enter Rim Weight \n";
            if ($('#txt_RimPrice').val() == "")
                ErrMsg += "Enter Rim Price \n";
            if ($('#txt_ValDate').val() == "")
                ErrMsg += "Select Validity Date \n";
            if (ErrMsg.length > 0) {
                alert(ErrMsg);
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
