<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="typegradeselector.aspx.cs" Inherits="TTS.typegradeselector" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div align="center" class="pageTitleHead">
        GRADE SELECTION TOOL
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage" style="height: auto;">
        <table>
            <tr>
                <td style="line-height: 25px;">
                    <div style="background-color: #def;" class="dlDivCss">
                        <asp:Label runat="server" ID="lblApplication" ClientIDMode="Static" Font-Bold="true"
                            Font-Size="16px" Font-Underline="true" ForeColor="#CCC"></asp:Label>
                        <asp:DataList runat="server" ID="dlAppHead" RepeatColumns="1" RepeatDirection="Vertical"
                            RepeatLayout="Table" Width="200px" OnSelectedIndexChanged="dlAppHead_IndexChange">
                            <ItemStyle CssClass="dlListMouse" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lnkAppHead" Text='<%# Eval("AppHead") %>' Font-Bold="true"
                                    Font-Size="14px" CommandName="Select" Font-Underline="false" ForeColor="#000"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:DataList>
                    </div>
                    <div style="background-color: #ADD572; display: none;" class="dlDivCss" id="divSubApp"
                        runat="server" clientidmode="Static">
                        <asp:Label runat="server" ID="lblSubApplication" ClientIDMode="Static" Font-Bold="true"
                            Font-Size="16px" Font-Underline="true" ForeColor="#CCC"></asp:Label>
                        <asp:DataList runat="server" ID="dlSubAppHead" RepeatColumns="1" RepeatDirection="Vertical"
                            RepeatLayout="Table" Width="200px" OnSelectedIndexChanged="dlSubAppHead_IndexChange">
                            <ItemStyle CssClass="dlListMouse" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lnkSubAppHead" Text='<%# Eval("SubHead") %>' Font-Bold="true"
                                    Font-Size="14px" CommandName="Select" Font-Underline="false" ForeColor="#000"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:DataList>
                    </div>
                    <div style="background-color: #A9CCB0; display: none;" class="dlDivCss" id="divProduct"
                        runat="server" clientidmode="Static">
                        <asp:Label runat="server" ID="lblProduct" ClientIDMode="Static" Font-Bold="true"
                            Font-Size="16px" Font-Underline="true" ForeColor="#CCC"></asp:Label>
                        <asp:DataList runat="server" ID="dlProduct" RepeatColumns="1" RepeatDirection="Vertical"
                            RepeatLayout="Table" Width="200px" OnSelectedIndexChanged="dlProduct_IndexChange">
                            <ItemStyle CssClass="dlListMouse" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lnkProductList" Text='<%# Eval("Product") %>'
                                    Font-Bold="true" Font-Size="14px" CommandName="Select" Font-Underline="false"
                                    ForeColor="#000"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:DataList>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="width: 1063px; float: left; border-bottom: 1px solid #000;">
                        <asp:GridView runat="server" ID="gvCategoryMasterList" AutoGenerateColumns="true"
                            Width="1063px" HeaderStyle-CssClass="gvHeadCss" RowStyle-CssClass="bottomborder">
                        </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="width: 1063px; float: left; text-align: center;">
                        <asp:Button runat="server" ID="btnResetPage" ClientIDMode="Static" Text="RESET" CssClass="btnsave"
                            OnClick="btnResetPage_Click" />
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="width: 1063px; float: left;">
                        <div style="width: 530px; float: left; font-size: 13px; font-weight: bold; color: #9B3BA8;">
                            <asp:PlaceHolder ID="PlaceHolderLabel" runat="server"></asp:PlaceHolder>
                            <asp:PlaceHolder ID="PlaceHolderHidden" runat="server"></asp:PlaceHolder>
                        </div>
                        <div style="width: 530px; float: left; font-weight: bold; font-size: 20px; color: #0C7E47;">
                            <asp:Label runat="server" ID="lblTypeList" ClientIDMode="Static" Text=""></asp:Label>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnApplication" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnSubApplication" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnProduct" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnAppIndex" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnSubIndex" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnProIndex" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        $(document).ready(function () {
            $('#MainContent_gvCategoryMasterList th').css({ 'width': '100px' });
            $('#MainContent_gvCategoryMasterList td').click(function () {
                var lblID = "MainContent_lbl_"; $('#lblTypeList').html('');
                var $cell = $(this); seltdCell = $cell.text(); seltdHead = $cell.closest('table').children(':first').find('th').eq($cell.index()).text();
                var cellCol = $cell.index() + 1; $(this).removeClass('tdmousemove'); var curClass = $(this).attr('class'); var selText = seltdCell;
                if (curClass == 'tdclick') {
                    $(this).removeClass('tdclick'); $('#' + lblID + seltdHead).html('');
                }
                else {
                    $("#MainContent_gvCategoryMasterList td:nth-child(" + cellCol + ")").removeClass('tdclick')
                    $(this).addClass('tdclick'); $('#' + lblID + seltdHead).html(selText);
                }
                bind_GradeList();
            });

            $('#MainContent_gvCategoryMasterList td').hover(function () { if ($(this).text().length > 1) { $(this).addClass('tdmousemove'); } });
            $('#MainContent_gvCategoryMasterList td').mouseout(function () { if ($(this).text().length > 1) { $(this).removeClass('tdmousemove'); } });
        });

        function bind_GradeList() {
            var conQry = '';
            $("span[id*='MainContent_lbl_']").each(function () {
                var SplitHead = this.id.replace('MainContent_lbl_', '');
                if ($(this).html().length > 0) { if (conQry.length > 0) { conQry += '~' + SplitHead + '|' + $(this).html(); } else { conQry = SplitHead + '|' + $(this).html(); } }
            });
            if (conQry.length > 0) {
                $.ajax({ type: "POST", url: "typegradeselector.aspx/get_GradeSelector_WebMethod", data: '{strQuery:"' + conQry + '"}', contentType: "application/json; charset=utf-8",
                    dataType: "json", success: OnSuccessGrade, failure: function (response) { alert(response.d); }, error: function (response) { alert(response.d); }
                });
            }
        }

        function OnSuccessGrade(response) {
            $('#lblTypeList').html('');
            var selGrade = ''; var xmlDoc = $.parseXML(response.d); var xml = $(xmlDoc); var listVals = xml.find("T1");
            if ($(listVals[0]).text() != "") {
                for (var j = 0; j < listVals.length; j++) {
                    if (selGrade.length > 0) { selGrade += " , " + $(listVals[j]).text().toLocaleUpperCase(); } else { selGrade = $(listVals[j]).text().toLocaleUpperCase(); }
                }
                $('#lblTypeList').html(selGrade);
            }
        }

        function datalistSelectIndexCss(ctrlID, itemRow) {
            $("#MainContent_" + ctrlID + " td:eq(" + itemRow + ")").addClass('tdclick')
        }
    </script>
</asp:Content>
