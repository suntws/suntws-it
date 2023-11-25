<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="Exp_ScanCombiTyresReceive.aspx.cs" Inherits="TTS.Exp_ScanCombiTyresReceive" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/pdistyle.css" rel="stylesheet" type="text/css" />
    <link href="Styles/TinyStyle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #txtRejRemark
        {
            height: 70px;
            width: 97%;
            font-size: 16px;
            text-align: center;
            font-weight: bold;
            border-radius: 3px;
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
                    <asp:GridView runat="server" ID="gvLoadCheckOrder" AutoGenerateColumns="false" Width="100%"
                        HeaderStyle-BackColor="#FFCC00" RowStyle-Height="22px">
                        <Columns>
                            <asp:BoundField HeaderText="CUSTOMER" DataField="custfullname" />
                            <asp:BoundField HeaderText="WORK ORDER" DataField="workorderno" />
                            <asp:BoundField HeaderText="COMBI ORDER QTY" DataField="orderqty" />
                            <asp:BoundField HeaderText="SENT QTY" DataField="Qty" />
                            <asp:BoundField HeaderText="SENT PLANT" DataField="PdiPlant" />
                            <asp:BoundField HeaderText="DC STATUS" DataField="rejectstatus" />
                            <asp:TemplateField HeaderText="DC NO">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hdnCustname" Value='<%# Eval("custfullname") %>' />
                                    <asp:HiddenField runat="server" ID="hdnWo" Value='<%# Eval("workorderno") %>' />
                                    <asp:HiddenField runat="server" ID="hdnDID" Value='<%# Eval("DC_Id") %>' />
                                    <asp:HiddenField runat="server" ID="hdnDPID" Value='<%# Eval("DC_PID") %>' />
                                    <asp:HiddenField runat="server" ID="hdnDNo" Value='<%# Eval("DC_No") %>' />
                                    <asp:HiddenField runat="server" ID="hdnSentqty" Value='<%# Eval("Qty") %>' />
                                    <asp:HiddenField runat="server" ID="hdnRejVal" Value='<%# Eval("DC_Status") %>' />
                                    <asp:LinkButton runat="server" ID="lnkPdiLoad" OnClick="lnkPdiLoad_Click" Text='<%# Eval("DC_No") %>'></asp:LinkButton>
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
                    <table id="tblBarcodeScan" class="tbMas" cellspacing="0" rules="all" border="1" style="display: none;">
                        <tr>
                            <th>
                                BARCODE
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtBarcode" ClientIDMode="Static" Text="" MaxLength="19"
                                    Width="180px" TabIndex="0" CssClass="form-control"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblBarcode" ClientIDMode="Static" Width="260px"></asp:Label>
                            </td>
                            <th>
                                SCANNED QTY
                            </th>
                            <td style="padding-top: 10px;">
                                <asp:Label runat="server" ID="lblLoadScanQty" ClientIDMode="Static" Text="0" Font-Bold="true"
                                    Font-Size="30px"></asp:Label>
                                &nbsp;<asp:Label runat="server" ID="Label1" ClientIDMode="Static" Text="/" Font-Bold="true"
                                    Font-Size="30px"></asp:Label>
                                &nbsp;<asp:Label runat="server" ID="lblQtytoLoad" ClientIDMode="Static" Text="" Font-Bold="true"
                                    Font-Size="30px" Width="80px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <div style="width: 100%;">
                                    <table id="tblqualityCheck" class="tbMas" cellspacing="0" rules="all" border="1"
                                        style="display: none;">
                                        <tr>
                                            <th style="width: 361px;">
                                                QUALITY
                                            </th>
                                            <td style="width: 330px;">
                                                <asp:RadioButtonList runat="server" ID="rdbQualityStatus" ClientIDMode="Static" RepeatColumns="2"
                                                    RepeatDirection="Horizontal" Width="431px">
                                                    <asp:ListItem Text="ACCEPT" Value="1" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="REJECT" Value="0"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td style="width: 300px;">
                                                <asp:Button runat="server" ID="btnBarcodeCheck" Text="CHECK & SAVE" OnClick="btnBarcodeCheck_Click"
                                                    OnClientClick="javascript:return cntrlLoadCkeck();" BackColor="#c5c013" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <table id="tblRejRemark" class="tbMas" cellspacing="0" rules="all" border="1" style="display: none;">
                                    <tr>
                                        <th style="width: 354px;">
                                            REASON FOR REJECTION
                                        </th>
                                        <td style="width: 740px;">
                                            <asp:TextBox runat="server" ID="txtRejRemark" ClientIDMode="Static" Text="" TextMode="MultiLine" MaxLength="100"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:TextBox runat="server" ID="txtLoadScanStatus" ClientIDMode="Static" Text=""
                                    CssClass="statusTxt"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:GridView runat="server" ID="gvBarcodelist" AutoGenerateColumns="false" Width="100%"
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
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <table id="tblRejOpinion" class="tbMas" cellspacing="0" rules="all" border="1" style="display: none;">
                        <tr>
                            <th style="width: 300px; text-align: center;">
                                REJECTED ITEMS
                            </th>
                            <td>
                                <asp:RadioButtonList runat="server" ID="rdblRejOpinion" ClientIDMode="Static" RepeatColumns="2"
                                    RepeatDirection="Horizontal" Width="690px" Style="margin-left: 10px;">
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label runat="server" ID="lblOpinFrmplant" ClientIDMode="Static" Font-Bold="True"
                                    Font-Size="14px" Width="100%" Style="text-align: center;" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: center;">
                    <asp:Button runat="server" ID="btnSave" ClientIDMode="Static" Text="INWARD STOCK TO WAREHOUSE"
                        OnClick="btnSave_Click" CssClass="btn btn-success" BackColor="#387509" Style="display: none;
                        margin-left: 0px;" Width="400px" />
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnPID" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnDCID" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnDCNo" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnRejlengthVal" ClientIDMode="Static" Value="" />
    <script type="text/javascript">
        $(document).ready(function () {
            $('#txtLoadScanStatus').keydown(function (e) {
                e.keyWhich = 0; e.keyCode = 0; e.preventDefault();
            });
            $('#rdbQualityStatus').click(function () {
                if ($('input:radio[id*=rdbQualityStatus_]:checked').val() == "0") {
                    $('#tblRejRemark').css('display', 'block');
                    $('#txtRejRemark').focus();
                }
                else { $('#tblRejRemark').css('display', 'none'); }
            });
            $('#rdblRejOpinion').click(function () {
                if ($('input:radio[id*=rdbQualityStatus_]:checked').length >= 0) {
                    $('#btnSave').css('display', 'inline-block');
                }
                else { $('#btnSave').css('display', 'none'); }
            });
        });
        function cntrlLoadCkeck() {
            if ($('input:radio[id*=rdbQualityStatus_]:checked').val() == "0" && $('#txtRejRemark').val() == "") {
                alert('PLEASE GIVE REASON FOR REJECTION.');
                $('#txtRejRemark').focus();
                return false;
            }
            else
                return true;
        }
        function endLoad() {
            $('#tblqualityCheck').css({ 'display': 'none' });
            $('#txtBarcode').attr('disabled', 'disabled');
        }
    </script>
</asp:Content>
