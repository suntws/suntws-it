<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="prospecthistory.aspx.cs" Inherits="TTS.prospecthistory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/prospectStyle.css" rel="stylesheet" type="text/css" />
    <link href="Styles/popupBox.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div align="center" class="pageTitleHead">
        SUPPLIER HISTORY</div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <div style="padding-left: 10px; line-height: 35px; padding-top: 50px;">
            <table>
                <tr>
                    <td style="float: left; width: 450px;">
                        <div style="float: left; width: 450px;">
                            <span class="headCss" style="width: 95px; float: left;">Customer Name</span>
                            <div style="float: left; width: 350px; line-height: 28px;" class="dropDivCss">
                                <asp:TextBox runat="server" ID="txtHistoryCust" Text="" CssClass="dropDownID" ClientIDMode="Static"
                                    Width="320px"></asp:TextBox>
                                <span class="dropdwonCustCss"></span>
                                <div id="popup_box_cust">
                                </div>
                            </div>
                        </div>
                        <div>
                            <span class="headCss" style="width: 95px; float: left;">Supplier Name</span>
                            <asp:DropDownList runat="server" ID="ddlSupplierName" ClientIDMode="Static" Width="350px">
                            </asp:DropDownList>
                            <span id="divNewSupply" style="display: none; padding-left: 100px;">
                                <asp:TextBox runat="server" ID="txtNewSupplier" ClientIDMode="Static" Width="350px"
                                    Text=""></asp:TextBox>
                            </span>
                        </div>
                        <div>
                            <span class="headCss" style="width: 95px; float: left;">Supply Year</span>From
                            <asp:TextBox runat="server" ID="txtFromYear" ClientIDMode="Static" Text="" Width="60px"
                                onkeypress="return isNumberKey(event)" MaxLength="4"></asp:TextBox>
                            To
                            <asp:TextBox runat="server" ID="txtToYear" ClientIDMode="Static" Text="" Width="60px"
                                onkeypress="return isNumberKey(event)" MaxLength="4"></asp:TextBox>
                        </div>
                        <div style="text-align: center;">
                            <span onclick="ctrlClearVal();" class="btnclear" style="line-height: 20px; margin-left: 20px;
                                margin-right: 30px;">Clear</span><asp:Button runat="server" ID="btnAddHistory" ClientIDMode="Static"
                                    Text="Add" OnClientClick="javascript:return ctrlHistoryValidate();" OnClick="btnAddHistory_click"
                                    CssClass="btnshow" />
                        </div>
                    </td>
                    <td style="vertical-align: top; padding-top: 10px;">
                        <div style="line-height: 20px;">
                            <asp:GridView runat="server" ID="gv_SupplierHistory" AutoGenerateColumns="false"
                                AlternatingRowStyle-BackColor="#f5f5f5" Width="480px" HeaderStyle-BackColor="#F3E756"
                                OnRowEditing="gv_RowEditing" OnRowUpdating="gv_RowUpdating" OnRowCancelingEdit="gv_RowCanceling">
                                <Columns>
                                    <asp:BoundField DataField="sup_name" HeaderText="SUPPLIER NAME" ItemStyle-Width="250px" />
                                    <asp:BoundField DataField="sup_from" HeaderText="FROM" ItemStyle-Width="60px" />
                                    <asp:TemplateField HeaderText="TO" ItemStyle-Width="60px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblSupTo" Text='<%# Eval("sup_to") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox runat="server" ID="txtSupTo" onkeypress="return isNumberKey(event)"
                                                Width="55px" MaxLength="4" BackColor="#f9c232" Text='<%# Eval("sup_to") %>'></asp:TextBox>
                                            <asp:HiddenField runat="server" ID="hdnSupSno" Value='<%# Eval("sno") %>' />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="200px">
                                        <HeaderTemplate>
                                            ACTION</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Button runat="server" ID="btnEdit" Text="Edit" CommandName="Edit" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Button runat="server" ID="btnUpdate" Text="Update" CommandName="Update" />
                                            <asp:Button runat="server" ID="btnCancel" Text="Cancel" CommandName="Cancel" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div style="font-weight: bold; color: #ff0000; float: left; line-height: 20px;" id="divErrmsg">
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdnHistorySno" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnProspectCode" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        $(document).ready(function () {
            $('#ddlSupplierName').change(function () {
                if ($("#ddlSupplierName option:selected").text() == "ADD NEW SUPPLIER")
                    $('#divNewSupply').show();
                else
                    $('#divNewSupply').hide();
            });

            $('.dropdwonCustCss').click(function () {
                loadUserWiseProspectCustName('popup_box_cust');
                loadPopupBox('popup_box_cust', 'txtHistoryCust');
            });

            liPossition = 0;
            $('#txtHistoryCust').keyup(function (e) {
                if ((e.keyCode == 40 || e.which == 40) || (e.keyCode == 38 || e.which == 38)) { // down arrow key code 
                    upanddownKey(e, 'popup_box_cust');
                } else if (e.keyCode == 13 || e.which == 13) { // enter key code 
                    popupEnterKey('popup_box_cust', 'txtHistoryCust');
                    if ($('#txtHistoryCust').val().length > 0) {
                        get_popupcustName($('#txtHistoryCust').val());
                    }
                }
                else {
                    var cname = $('#txtHistoryCust').val();
                    if (cname.length > 0) {
                        $.ajax({ type: "POST", url: "GetPopupRecords.aspx?type=getprospectUserWiselikecust&cname=" + cname + "", context: document.body,
                            success: function (data) {
                                if (data != '') {
                                    $('#popup_box_cust').html(data); $("div[id*='popup_box_cust'] ul li").first().addClass('current'); loadPopupBox('popup_box_cust', 'txtHistoryCust');
                                }
                                else {
                                    $('#popup_box_cust').html(''); $('#popup_box_cust').hide();
                                }
                            }
                        });
                    } else {
                        $('#popup_box_cust').html('');
                        $('#popup_box_cust').hide();
                    }
                    liPossition = 0;
                }
            });

            $('#popup_box_cust').hover(function () {
                popupHover('popup_box_cust', 'txtHistoryCust');
                if ($('#txtHistoryCust').val().length > 0 && $('#popup_box_cust').css("display") == "none") {
                    get_popupcustName($('#txtHistoryCust').val());
                }
            });
            // Customer PopUp End
        });

        function get_popupcustName(e) {
            var pathname = window.location.href;
            var splitval = pathname.split('?');
            window.location.href = splitval[0].toString() + "?custname=" + e;
            //unloadCustPopupBox();
        }

        function ctrlHistoryValidate() {
            var errmsg = '';
            $('#divErrmsg').html('');
            if ($("#txtHistoryCust").val().length == 0)
                errmsg += 'Choose customer name</br>';
            if ($("#ddlSupplierName option:selected").text() == "Choose")
                errmsg += 'Choose supplier name</br>';
            if ($("#ddlSupplierName option:selected").text() == "ADD NEW SUPPLIER") {
                if ($('#txtNewSupplier').val().length == 0)
                    errmsg += 'Enter new supplier name</br>';
            }
            //            if ($('#txtFromYear').val().length == 0 && $('#txtToYear').val().length == 0)
            //                errmsg += 'Enter any one year</br>';
            if ($('#txtFromYear').val().length == 4 && $('#txtToYear').val().length == 4) {
                if (parseInt($('#txtFromYear').val()) > parseInt($('#txtToYear').val()))
                    errmsg += 'From year should be less than To year';
            }
            if (errmsg.length > 0) {
                $('#divErrmsg').html(errmsg);
                return false;
            }
            else
                return true;
        }

        function ctrlClearVal() {
            $("#txtHistoryCust").val('');
            $("select#ddlSupplierName")[0].selectedIndex = $("#ddlSupplierName option").length - 1;
            $('#txtFromYear').val('');
            $('#txtToYear').val('');
            $('#MainContent_gv_SupplierHistory').remove();
            $('#btnAddHistory').val('Add');
            $('#divNewSupply').hide();
        }
    </script>
</asp:Content>
