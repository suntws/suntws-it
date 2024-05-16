<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="frmexpitementry.aspx.cs" Inherits="COTS.frmexpitementry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="styles/GridviewScroll.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="pageTitleHead">
        FILL THE ORDER QTY
    </div>
    <div class="contPage">
        <table cellspacing="0" rules="all" border="1" style="background-color: #ffffff; width: 100%;
            border-color: #cccbfb; border-collapse: separate;">
            <tr>
                <th>
                    ORDER NO. :<asp:Label runat="server" ID="lblOrderRefNo" ClientIDMode="Static" Text=""
                        Font-Bold="true" ForeColor="#000CCC" Font-Size="15px"></asp:Label>
                </th>
                <td>
                    CURRENCY :
                    <asp:Label runat="server" ID="lblCurType" ClientIDMode="Static" Text="" Font-Bold="true"
                        ForeColor="#000CCC" Font-Size="15px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div id="divsolidentry">
                        <asp:GridView ID="gvSolid" runat="server" AutoGenerateColumns="false" GridLines="None"
                            OnRowCreated="gvSolid_RowCreated">
                            <Columns>
                            </Columns>
                            <HeaderStyle CssClass="GridviewScrollHeader" />
                            <RowStyle CssClass="GridviewScrollItem" />
                        </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div id="divpobentry">
                        <asp:GridView ID="gvPob" runat="server" AutoGenerateColumns="false" GridLines="None"
                            OnRowCreated="gvPob_RowCreated">
                            <Columns>
                            </Columns>
                            <HeaderStyle CssClass="GridviewScrollHeader" />
                            <RowStyle CssClass="GridviewScrollItem" />
                        </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr style="text-align: center;">
                <td>
                </td>
                <td>
                    <span id="tempInsert" class="btn btn-info" onclick="CompleteOrderEntryItems();" style="width: 150px;">
                        SAVE QTY ENTRY</span>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button runat="server" ID="btnComplete" ClientIDMode="Static" Text="CLICK FOR COMPLETE THE ORDER"
                                OnClick="btnComplete_Click" CssClass="btn btn-success" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnVirtualStr" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnCustCode" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnQType" ClientIDMode="Static" Value="" />
    <script src="scripts/jquerymin182.js" type="text/javascript"></script>
    <script src="scripts/jqueryui191.js" type="text/javascript"></script>
    <script src="scripts/gridviewScroll.min.js" type="text/javascript"></script>
    <script src="scripts/itemEntrySolid.js" type="text/javascript"></script>
    <script src="scripts/itemEntryPob.js" type="text/javascript"></script>
    <script type="text/javascript">
        var $ = jQuery.noConflict();
        $(document).ready(function () {
            gridviewSolidScroll();
            bind_AllSolidDesign();
            solidEntry();

            gridviewPobScroll();
            bind_AllPobDesign();
            PobEntry();
        });

        function gridviewSolidScroll() { $('#<%=gvSolid.ClientID %>').gridviewScroll({ width: 1180, height: 350, freezesize: 1, headerrowcount: 4 }); }
        function gridviewPobScroll() { $('#<%=gvPob.ClientID %>').gridviewScroll({ width: 1180, height: 350, freezesize: 1, headerrowcount: 4 }); }

        function CompleteOrderEntryItems() {
            var itemCount = 0;
            //SOLID ORDER
            $('#ContentPlaceHolder1_gvSolid tr').each(function (e) {
                var gvFreezeRow = parseInt(e) + 4;
                $('#ContentPlaceHolder1_gvSolid tr:eq(' + gvFreezeRow + ')').find("input[id*='ContentPlaceHolder1_gvSolid_txt_']").each(function (item) {
                    if ($(this).val().length > 0 && $(this).val() > 0) {
                        var id1 = this.id; var leftGvRow = parseInt(gvFreezeRow) - 3; var splitID = id1.split('_');
                        if (splitID.length == 8) {
                            var gvTypeVal = splitID[3].toString() + "_" + splitID[4].toString() + "_" + splitID[5].toString() + "_" + splitID[6].toString()
                            var gvSizeVal = $('#ContentPlaceHolder1_gvSolid_lbl_TyreSize_' + leftGvRow).html() + "_" + $('#ContentPlaceHolder1_gvSolid_lbl_RimSize_' + leftGvRow).html();
                            var gvExtraList = $(this).val() + "_Solid_" + $('#lblOrderRefNo').html();
                            var strorderjathagam = gvTypeVal + "_" + gvSizeVal + "_" + gvExtraList;
                            $.ajax({ type: "POST", url: "bindvalues.aspx", data: { orderjathagam: strorderjathagam }, context: document.body,
                                success: function (data) { if (data == 'success') { itemCount++; } }
                            });
                        }
                    }
                });
            });
            //POB ORDER
            $('#ContentPlaceHolder1_gvPob tr').each(function (e) {
                var gvFreezeRow = parseInt(e) + 4;
                $('#ContentPlaceHolder1_gvPob tr:eq(' + gvFreezeRow + ')').find("input[id*='ContentPlaceHolder1_gvPob_txt_']").each(function (item) {
                    if ($(this).val().length > 0 && $(this).val() > 0) {
                        var id1 = this.id; var leftGvRow = parseInt(gvFreezeRow) - 3; var splitID = id1.split('_');
                        if (splitID.length == 8) {
                            var gvTypeVal = splitID[3].toString() + "_" + splitID[4].toString() + "_" + splitID[5].toString() + "_" + splitID[6].toString()
                            var gvSizeVal = $('#ContentPlaceHolder1_gvPob_lbl_TyreSize_' + leftGvRow).html() + "_" + $('#ContentPlaceHolder1_gvPob_lbl_RimSize_' + leftGvRow).html();
                            var gvExtraList = $(this).val() + "_Pob_" + $('#lblOrderRefNo').html();
                            var strorderjathagam = gvTypeVal + "_" + gvSizeVal + "_" + gvExtraList;
                            $.ajax({ type: "POST", url: "bindvalues.aspx", data: { orderjathagam: strorderjathagam }, context: document.body,
                                success: function (data) { if (data == 'success') { itemCount++; } }
                            });
                        }
                    }
                });
            });
            //Move to profoma
            $('#tempInsert').fadeOut(1000); //.css({ "display": "none" });
            $('#btnComplete').fadeIn(5000); //.css({ "display": "block" }); 
        }

        function display_divs(displayVal, divType) {
            if (divType == "solid")
                $('#divsolidentry').css({ 'display': '' + displayVal + '' });
            if (divType == "pob")
                $('#divpobentry').css({ 'display': '' + displayVal + '' });
        }
    </script>
</asp:Content>
