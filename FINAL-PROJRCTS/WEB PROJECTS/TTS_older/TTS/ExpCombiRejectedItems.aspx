<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="ExpCombiRejectedItems.aspx.cs" Inherits="TTS.ExpCombiRejectedItems" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/pdistyle.css" rel="stylesheet" type="text/css" />
    <link href="Styles/lightbox.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .divShow
        {
            width: 1060px;
            float: left;
            display: block;
        }
        .divHide
        {
            width: 1060px;
            float: left;
            display: none;
        }
        .hide
        {
            cursor: pointer;
            text-align: left;
            width: 15px;
            height: 15px;
            -webkit-transform: rotate(90deg);
        }
        .FadeIn
        {
            -webkit-transform: rotate(360deg);
        }
        .StatusMainDiv
        {
            width: 1060px;
            float: left;
            margin-bottom: 5px;
        }
        .StatusTitleDiv
        {
            float: left;
            text-align: left;
            width: 1035px;
            color: #000;
            font-size: 15px;
            font-weight: bold;
            line-height: 22px;
        }
        .StatusToggle
        {
            float: left;
            width: 20px;
            padding: 2px;
        }
        .headStyle
        {
            font-weight: bold;
            color: #27b9dd;
            float: left;
            width: 200px;
        }
        .valueStyle
        {
            width: 860px;
            float: left;
        }
        .commentDiv
        {
            width: 1060px;
            float: left;
            display: none;
        }
        #txtRejRemark
        {
            width: 97%;
            font-size: 18px;
            text-align: center;
            font-weight: bold;
            border-radius: 3px;
            padding: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: center;" class="pageTitleHead">
        <asp:Label runat="server" ID="lblPageTitle" ClientIDMode="Static"></asp:Label>
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div class="contPage">
        <asp:ScriptManager runat="server" ID="scriptManager1">
        </asp:ScriptManager>
        <table cellspacing="0" rules="all" border="1" style="background-color: #dcecfb; width: 100%;
            border-color: White; border-collapse: separate;">
            <tr>
                <td colspan="2">
                    <asp:GridView runat="server" ID="gvRejectedDet" AutoGenerateColumns="false" Width="100%"
                        HeaderStyle-BackColor="#FFCC00" RowStyle-Height="22px">
                        <Columns>
                            <asp:BoundField HeaderText="CUSTOMER" DataField="custfullname" />
                            <asp:BoundField HeaderText="WORK ORDER" DataField="workorderno" />
                            <asp:BoundField HeaderText="SENT QTY" DataField="Qty" />
                            <asp:BoundField HeaderText="REJECTED QTY" DataField="rejectedqty" />
                            <asp:BoundField HeaderText="REJECTED PLANT" DataField="Send_To" />
                            <asp:BoundField HeaderText="DC STATUS" DataField="rejectstatus" />
                            <asp:TemplateField HeaderText="DC NO">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hdnCustname" Value='<%# Eval("custfullname") %>' />
                                    <asp:HiddenField runat="server" ID="hdnWo" Value='<%# Eval("workorderno") %>' />
                                    <asp:HiddenField runat="server" ID="hdnDID" Value='<%# Eval("DC_Id") %>' />
                                    <asp:HiddenField runat="server" ID="hdnDPID" Value='<%# Eval("DC_PID") %>' />
                                    <asp:HiddenField runat="server" ID="hdnRejVal" Value='<%# Eval("DC_Status") %>' />
                                    <asp:HiddenField runat="server" ID="hdnRPlant" Value='<%# Eval("Send_To") %>' />
                                    <asp:LinkButton runat="server" ID="lnkDcno" OnClick="lnkDcno_Click" Text='<%# Eval("DC_No") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr style="text-align: center; line-height: 30px; font-size: 18px; background-color: #077305;
                color: #ffffff;">
                <td>
                    <asp:Label runat="server" ID="lblCustName" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblWorkorderNo" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:GridView runat="server" ID="gvRejectedlist" AutoGenerateColumns="false" Width="100%"
                        HeaderStyle-BackColor="#FFCC00" RowStyle-Height="22px">
                        <Columns>
                            <asp:BoundField HeaderText="PLATFORM" DataField="cplatform" />
                            <asp:BoundField HeaderText="TYRE SIZE" DataField="tyresize" />
                            <asp:BoundField HeaderText="RIM" DataField="rim" />
                            <asp:BoundField HeaderText="TYPE" DataField="tyretype" />
                            <asp:BoundField HeaderText="BRAND" DataField="brand" />
                            <asp:BoundField HeaderText="SIDEWALL" DataField="sidewall" />
                            <asp:BoundField HeaderText="PROCESSID" DataField="processid" />
                            <asp:BoundField HeaderText="STENCIL NO" DataField="stencilno" />
                            <asp:BoundField HeaderText="GRADE" DataField="grade" />
                            <asp:BoundField HeaderText="QUALITY STATUS" DataField="qstatus" />
                            <asp:BoundField HeaderText="COMMENTS" DataField="reason" />
                            <asp:BoundField HeaderText="REJECTED BY" DataField="RejectBy" />
                            <asp:BoundField HeaderText="REJECTED ON" DataField="RejectOn" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:GridView runat="server" ID="gvRejOpinionDet" AutoGenerateColumns="false" Width="1064px"
                        RowStyle-Height="22px">
                        <HeaderStyle CssClass="headerNone" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <div class='<%# Convert.ToInt32(Eval("DC_Status")) < 3 && Eval("ReceiverOpinion").ToString()==""? "divHide" : "divShow" %>'>
                                        <div class="StatusMainDiv">
                                            <div class="StatusToggle" style="background-color: #DCDADA;">
                                                <asp:Image ID="recOp" runat="server" src="images/collapse.gif" ClientIDMode="Static"
                                                    CssClass="hide FadeIn" />
                                            </div>
                                            <div class="StatusTitleDiv" style="background-color: #DCDADA;">
                                                RECEIVER'S OPINION
                                            </div>
                                            <div id="divrecOp" class="commentDiv">
                                                <div>
                                                    <span class="headStyle">COMMENTS :</span> <span class="valueStyle">
                                                        <%# Eval("Receiveropinion").ToString()%>
                                                    </span>
                                                </div>
                                                <div>
                                                    <span class="headStyle">BY :</span>
                                                    <%# Eval("Received_User").ToString()%>
                                                </div>
                                                <div>
                                                    <span class="headStyle">ON :</span>
                                                    <%# Eval("Received_On").ToString()%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class='<%# Convert.ToInt32(Eval("DC_Status")) < 4 && Eval("SenderOpinion").ToString()==""? "divHide" : "divShow" %>'>
                                        <div class="StatusMainDiv">
                                            <div class="StatusToggle" style="background-color: #DCDADA;">
                                                <asp:Image ID="senOp" runat="server" src="images/collapse.gif" ClientIDMode="Static"
                                                    CssClass="hide FadeIn" />
                                            </div>
                                            <div class="StatusTitleDiv" style="background-color: #DCDADA;">
                                                SENDER'S OPINION
                                            </div>
                                            <div id="divsenOp" class="commentDiv">
                                                <div>
                                                    <span class="headStyle">COMMENTS :</span> <span class="valueStyle">
                                                        <%# Eval("SenderOpinion").ToString()%>
                                                    </span>
                                                </div>
                                                <div>
                                                    <span class="headStyle">BY :</span>
                                                    <%# Eval("SenderOpinion_By").ToString()%>
                                                </div>
                                                <div>
                                                    <span class="headStyle">ON :</span>
                                                    <%# Eval("SenderOpinion_On").ToString()%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class='<%# Convert.ToInt32(Eval("DC_Status")) < 5 && Eval("ReceiverReopinion").ToString()==""? "divHide" : "divShow" %>'>
                                        <div class="StatusMainDiv">
                                            <div class="StatusToggle" style="background-color: #DCDADA;">
                                                <asp:Image ID="recReop" runat="server" src="images/collapse.gif" ClientIDMode="Static"
                                                    CssClass="hide FadeIn" />
                                            </div>
                                            <div class="StatusTitleDiv" style="background-color: #DCDADA;">
                                                RECEIVER'S REOPINION
                                            </div>
                                            <div id="divrecReop" class="commentDiv">
                                                <div>
                                                    <span class="headStyle">COMMENTS :</span> <span class="valueStyle">
                                                        <%# Eval("ReceiverReopinion").ToString()%>
                                                    </span>
                                                </div>
                                                <div>
                                                    <span class="headStyle">BY :</span>
                                                    <%# Eval("ReceiverReopinion_By").ToString()%>
                                                </div>
                                                <div>
                                                    <span class="headStyle">ON :</span>
                                                    <%# Eval("ReceiverReopinion_On").ToString()%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <table id="tblOpin" cellspacing="0" rules="all" border="1" style="background-color: #dcecfb;
                        width: 100%; border-color: White; border-collapse: separate; display: none;">
                        <tr>
                            <th style="width: 400px;">
                                <asp:Label runat="server" ID="lblVariOpinions" ClientIDMode="Static" Font-Bold="True"
                                    Font-Size="14px" Width="400px" Style="text-align: center;" ForeColor="Black"></asp:Label>
                            </th>
                            <td colspan="3">
                                <asp:RadioButtonList runat="server" ID="rdblRejOpinion" ClientIDMode="Static" RepeatColumns="2"
                                    RepeatDirection="Horizontal" Width="690px" Style="margin-left: 10px;">
                                    <asp:ListItem Text="OK" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="NOT OK" Value="0"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <th style="width: 300px;">
                    <asp:Label runat="server" ID="lblComment" ClientIDMode="Static" Font-Bold="True"
                        Text="COMMENT" Font-Size="16px" Width="350px" Style="text-align: center; display: none;"
                        ForeColor="Black"></asp:Label>
                </th>
                <td style="width: 600px;">
                    <asp:TextBox runat="server" ID="txtRejRemark" ClientIDMode="Static" TextMode="MultiLine" MaxLength="100"
                        Style="display: none;" Height="54px" Width="96%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: center;">
                    <asp:Button runat="server" ID="btnSave" ClientIDMode="Static" Text="SAVE OPINION"
                        OnClick="btnSave_Click" OnClientClick="javascript:return txtcommCheck();" CssClass="btn btn-success"
                        BackColor="#387509" Style="display: none; margin-left: 0px;" Width="267px" />
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnPID" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnDCID" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnDcStatusId" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnRejPlant" ClientIDMode="Static" Value="" />
    <script src="Scripts/lightbox.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".divShow:last").find('.commentDiv').fadeToggle('slow');
            $(".divShow:last").find('.hide').removeClass('FadeIn');
            $('#recOp').click(function () {
                $('#divrecOp').fadeToggle('slow');
                $(this).toggleClass('FadeIn');
                gotoClaimDiv('recOp');
            });
            $('#senOp').click(function () {
                $('#divsenOp').fadeToggle('slow');
                $(this).toggleClass('FadeIn');
                gotoClaimDiv('senOp');
            });
            $('#recReop').click(function () {
                $('#divrecReop').fadeToggle('slow');
                $(this).toggleClass('FadeIn');
                gotoClaimDiv('recReop');
            });
            $('#rdblRejOpinion').click(function () {
                if ($('input:radio[id*=rdblRejOpinion_]:checked').val() == "0") {
                    $('#lblComment').css('display', 'block');
                    $('#txtRejRemark').css('display', 'block');
                    $('#txtRejRemark').focus();
                    $('#btnSave').css('display', 'inline-block');
                }
                else {
                    $('#lblComment').css('display', 'none');
                    $('#txtRejRemark').css('display', 'none');
                    $('#btnSave').css('display', 'inline-block');
                }
            });
        });
        function gotoClaimDiv(ctrlID) {
            $("html, body").stop().animate({
                scrollLeft: $('#' + ctrlID).offset().left,
                scrollTop: $('#' + ctrlID).offset().top
            }, 1200);
        }
        function txtcommCheck() {
            if ($('#tblOpin').css('display') == 'block' && $('input:radio[id*=rdblRejOpinion_]:checked').val() == "0" && $('#txtRejRemark').val() == '') {
                alert('GIVE COMMENTS ABOUT YOUR OPINION.');
                $('#txtRejRemark').focus();
                return false;
            }
            else if ($('#tblOpin').css('display') == 'none' && $('#txtRejRemark').val() == '') {
                alert('GIVE COMMENTS ABOUT YOUR OPINION.');
                $('#txtRejRemark').focus();
                return false;
            } else { return true; }
        }
        function btnSaveshow() {
            $('#lblComment').css({ 'display': 'block' });
            $('#txtRejRemark').css({ 'display': 'block' });
            $('#btnSave').css({ 'display': 'inline-block' });
        }
        function hideActivities() {
            $('#tblOpin').css({ 'display': 'none' });
            $('#lblComment').css({ 'display': 'none' });
            $('#txtRejRemark').css({ 'display': 'none' });
            $('#btnSave').css({ 'display': 'none' });
        }
    </script>
</asp:Content>
