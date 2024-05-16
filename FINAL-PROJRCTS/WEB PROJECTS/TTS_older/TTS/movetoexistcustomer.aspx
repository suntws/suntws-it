<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="movetoexistcustomer.aspx.cs" Inherits="TTS.movetoexistcustomer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/popupBox.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div align="center" class="pageTitleHead">
        PROSPECT MOVE TO EXIST CUSTOMER</div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <div style="padding-left: 10px; padding-top: 10px;">
            <table>
                <tr>
                    <td colspan="2" style="line-height: 30px;">
                        <div style="width: 1000px; float: left;">
                            <div style="float: left; width: 160px;">
                                <span class="headCss">Choose customer name:</span>
                            </div>
                            <div class="dropDivCss" style="width: 400px; line-height: 28px;">
                                <asp:TextBox runat="server" ID="txtName" Text="" CssClass="dropDownID" ClientIDMode="Static"
                                    Width="370px"></asp:TextBox>
                                <span class="dropdwonCustCss"></span>
                                <div id="popup_box_cust">
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="movetdCss">
                        <div class="movepgleft">
                            <span class="headCss">Name</span>
                        </div>
                        <div class="movepgright">
                            <asp:Label runat="server" ID="lblCustName" ClientIDMode="Static" Text="&nbsp;"></asp:Label>
                        </div>
                        <div class="movepgleft">
                            <span class="headCss">Address</span>
                        </div>
                        <div class="movepgright">
                            <asp:Label runat="server" ID="lblAddress" ClientIDMode="Static" Text="&nbsp;" CssClass="moveaddressCss"></asp:Label>
                        </div>
                        <div class="movepgleft">
                            <span class="headCss">Country</span>
                        </div>
                        <div class="movepgright">
                            <asp:Label runat="server" ID="lblCountry" ClientIDMode="Static" Text="&nbsp;"></asp:Label>
                        </div>
                        <div class="movepgleft">
                            <span class="headCss">City</span>
                        </div>
                        <div class="movepgright">
                            <asp:Label runat="server" ID="lblCity" ClientIDMode="Static" Text="&nbsp;"></asp:Label>
                        </div>
                        <div class="movepgleft">
                            <span class="headCss">Zipcode</span>
                        </div>
                        <div class="movepgright">
                            <asp:Label runat="server" ID="lblZipcode" ClientIDMode="Static" Text="&nbsp;"></asp:Label>
                        </div>
                        <div class="movepgleft">
                            <span class="headCss">E-Mail</span>
                        </div>
                        <div class="movepgright">
                            <asp:Label runat="server" ID="lblEmail" ClientIDMode="Static" Text="&nbsp;"></asp:Label>
                        </div>
                        <div class="movepgleft">
                            <span class="headCss">Web Address</span>
                        </div>
                        <div class="movepgright">
                            <asp:Label runat="server" ID="lblWebAddress" ClientIDMode="Static" Text="&nbsp;"></asp:Label>
                        </div>
                        <div class="movepgleft">
                            <span class="headCss">Phone No.</span>
                        </div>
                        <div class="movepgright">
                            <asp:Label runat="server" ID="lblPhone1" ClientIDMode="Static" Text="&nbsp;"></asp:Label>
                        </div>
                    </td>
                    <td class="movetdCss">
                        <div class="movepgleft">
                            <span class="headCss">Phone No2.</span>
                        </div>
                        <div class="movepgright">
                            <asp:Label runat="server" ID="lblPhone2" ClientIDMode="Static" Text="&nbsp;"></asp:Label>
                        </div>
                        <div class="movepgleft">
                            <span class="headCss">Type</span>
                        </div>
                        <div class="movepgright">
                            <asp:Label runat="server" ID="lblType" ClientIDMode="Static" Text="&nbsp;"></asp:Label>
                        </div>
                        <div class="movepgleft">
                            <span class="headCss">Prospect Code</span>
                        </div>
                        <div class="movepgright">
                            <asp:Label runat="server" ID="lblCode" ClientIDMode="Static" Text="&nbsp;"></asp:Label>
                        </div>
                        <div class="movepgleft">
                            <span class="headCss">Exist Cust Code</span>
                        </div>
                        <div class="movepgright">
                            <asp:Label runat="server" ID="lblGenreateCode" ClientIDMode="Static" Text="&nbsp;"></asp:Label>
                        </div>
                        <div class="movepgleft">
                            <span class="headCss">Contact Person</span>
                        </div>
                        <div class="movepgright">
                            <asp:Label runat="server" ID="lblContactPerson" ClientIDMode="Static" Text="&nbsp;"></asp:Label>
                        </div>
                        <div class="movepgleft">
                            <span class="headCss">Channel</span>
                        </div>
                        <div class="movepgright">
                            <asp:Label runat="server" ID="lblChannel" ClientIDMode="Static" Text="&nbsp;"></asp:Label>
                        </div>
                        <div class="movepgleft">
                            <span class="headCss">Price basis</span>
                        </div>
                        <div class="movepgright">
                            <asp:Label runat="server" ID="lblPriceBasis" ClientIDMode="Static" Text="&nbsp;"></asp:Label>
                        </div>
                        <div class="movepgleft">
                            <span class="headCss">Price Unit</span>
                        </div>
                        <div class="movepgright">
                            <asp:Label runat="server" ID="lblPriceUnit" ClientIDMode="Static" Text="&nbsp;"></asp:Label>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div style="width: 1040px; float: left; padding-bottom: 10px;">
                            <span class="headCss">Special Instruction :</span>
                            <asp:TextBox runat="server" ID="txtMoveCustSpl" ClientIDMode="Static" TextMode="MultiLine"
                                Height="100px" Width="1046px" Enabled="false" onKeyUp="javascript:CheckMaxLength(this, 3999);"
                                onChange="javascript:CheckMaxLength(this, 3999);"></asp:TextBox></div>
                        <div style="float: left; width: 1040px; padding-bottom: 10px;">
                            <div style="width: 550px; float: left; font-weight: bold; color: #000; padding-right: 35px;">
                                Do you want any update in the customer details please go to <span class="btnedit"
                                    onclick="gotoeditcustomer();">Edit customer master</span></div>
                            <div style="width: 300px; float: left;">
                                <asp:Button runat="server" ID="btnMoveToExist" ClientIDMode="Static" Text="Save To Exist Customer List"
                                    CssClass="btnsave" OnClientClick="javascript:return ctrlValidate();" OnClick="btnMoveToExist_Click" />
                            </div>
                            <div style="width: 150px; float: left;">
                                <span class="btnclear" onclick="ctrlMovePageClear();">Cancel</span>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdnTitleName" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        $(document).ready(function () {
            liPossition = 0;

            $('#txtName').keyup(function (e) {
                if ((e.keyCode == 40 || e.which == 40) || (e.keyCode == 38 || e.which == 38)) { // down arrow key code 
                    upanddownKey(e, 'popup_box_cust');
                } else if (e.keyCode == 13 || e.which == 13) { // enter key code 
                    popupEnterKey('popup_box_cust', 'txtName');
                    if ($('#txtName').val().length > 0) {
                        get_popupcustName($('#txtName').val());
                    }
                }
                else {
                    var cname = $('#txtName').val();
                    if (cname.length > 0) {
                        $.ajax({ type: "POST", url: "GetPopupRecords.aspx?type=getprospectlikecust&cname=" + cname + "", context: document.body,
                            success: function (data) {
                                if (data != '') {
                                    $('#popup_box_cust').html(data); $("div[id*='popup_box_cust'] ul li").first().addClass('current'); loadPopupBox('popup_box_cust', 'txtName');
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
                popupHover('popup_box_cust', 'txtName');
                if ($('#txtName').val().length > 0 && $('#popup_box_cust').css("display") == "none") {
                    get_popupcustName($('#txtName').val());
                }
            });
            // Customer PopUp End

            $('.dropdwonCustCss').click(function () {
                loadProspectCustName('popup_box_cust');
                loadPopupBox('popup_box_cust', 'txtName');
            });
        });

        $('body').click(function (e) {
            if ($('#popup_box_cust').is(':visible') == true && e.target.className != "dropdwonCustCss")
                unloadPopupBox('popup_box_cust');
        });

        function get_popupcustName(e) {
            var pathname = window.location.href;
            var splitval = pathname.split('?');
            window.location.href = splitval[0].toString() + "?custname=" + e;
            //unloadCustPopupBox();
        }

        function gotoeditcustomer() {
            var strcust = $('#lblCustName').html();
            if (strcust != '&nbsp;' && strcust != '') {
                var pathname = window.location.href;
                var splitval = pathname.split('/movetoexistcustomer.aspx');
                window.location.replace(splitval[0].toString() + "/editcustdetails.aspx?custname=" + strcust + "&custcategory=Prospect");
            }
            else
                alert('Please choose any one customer');
        }

        function ctrlMovePageClear() {
            var pathname = window.location.href;
            var splitval = pathname.split('?');
            window.location.replace(splitval[0].toString());
        }

        function ctrlValidate() {
            var strcust = $('#lblCustName').html();
            if (strcust != '&nbsp;' && strcust != '')
                return true;
            else {
                alert('Please choose any one customer');
                return false;
            }
        }
    </script>
</asp:Content>
