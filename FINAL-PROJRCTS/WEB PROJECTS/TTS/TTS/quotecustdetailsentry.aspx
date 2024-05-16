<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="quotecustdetailsentry.aspx.cs"
    Inherits="TTS.quotecustdetailsentry" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .tableAddress
        {
            border-collapse: collapse;
            border-color: #DDDADA;
            width: 1000px;
            float: left;
            line-height: 20px;
            color: #0D9FAC;
            font-size: 14px;
        }
        .tableAddress th
        {
            text-align: right;
            background-color: #f3e2e2;
            width: 135px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" style="margin: 30px auto 0px auto; width: 1010px;">
    <div style="width: 1000px;" align="center">
        <div runat="server" id="divMsgBox" clientidmode="Static">
            <asp:Label runat="server" ID="lblSuccessMsg" ClientIDMode="Static" Text="" Font-Bold="true"
                Font-Size="16px" ForeColor="Green"></asp:Label>
        </div>
        <div runat="server" id="divEntryBox" clientidmode="Static">
            <div style="font-weight: bold; font-size: 20px; color: #29571B;">
                Plese check & confirm below details are correct. If any changes required please
                do so.
            </div>
            <table cellspacing="0" rules="all" border="1" class="tableAddress" id="tbQuote" runat="server">
                <tr>
                    <td align="right">
                        <asp:Label runat="server" ID="lblQuoteNo" ClientIDMode="Static" Text="" Font-Bold="true"
                            Font-Size="16px" ForeColor="Green"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:Label runat="server" ID="lblQuoteReviseNo" ClientIDMode="Static" Text="" Font-Bold="true"
                            Font-Size="16px" ForeColor="Green"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <span style="line-height: 15px;">CUSTOMER NAME : </span>
                        <asp:TextBox runat="server" ID="txtCustName" ClientIDMode="Static" Text="" MaxLength="100"
                            Width="700px" ReadOnly="true"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <span style="line-height: 15px;">EMAIL-ID TO : </span>
                        <asp:TextBox runat="server" ID="txtQEmail" ClientIDMode="Static" Text="" MaxLength="100"
                            Width="350px" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td>
                        <span style="line-height: 15px;">CC : </span>
                        <asp:TextBox runat="server" ID="txtQcc" ClientIDMode="Static" Text="" MaxLength="100"
                            Width="350px" ReadOnly="true"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 500px;">
                        <table>
                            <tr>
                                <td colspan="2" style="text-decoration: underline; color: #BD0BB6;">
                                    BILLING TO
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    M/S.
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="txtBillCompanyName" ClientIDMode="Static" Text=""
                                        MaxLength="50" Width="300px" onblur="BindShipAddressTxt('txtBillCompanyName','txtShipCompanyName')"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    ADDRESS
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="txtBillAddress" ClientIDMode="Static" Text="" TextMode="MultiLine"
                                        Width="300px" Height="70px" onKeyUp="javascript:CheckCtrlLength(this, 499);"
                                        onChange="javascript:CheckCtrlLength(this, 499);" onblur="BindShipAddressTxt('txtBillAddress','txtShipAddress')"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    CITY
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="txtBillCity" ClientIDMode="Static" Text="" MaxLength="50"
                                        Width="300px" onblur="BindShipAddressTxt('txtBillCity','txtShipCity')"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    ZIP CODE
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="txtBillPincode" ClientIDMode="Static" Text="" MaxLength="50"
                                        Width="300px" onblur="BindShipAddressTxt('txtBillPincode','txtShipPincode')"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    STATE / CODE
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="txtBillState" ClientIDMode="Static" Text="" MaxLength="50"
                                        Width="300px" onblur="BindShipAddressTxt('txtBillState','txtShipState')"></asp:TextBox>
                                    &nbsp;/&nbsp;<asp:TextBox runat="server" ID="txtBillStateCode" ClientIDMode="Static"
                                        Text="" Width="300px" MaxLength="50" CssClass="txtCss" onblur="BindShipAddressTxt('txtBillStateCode','txtShipStateCode')"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    GSTIN No
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="txtBillGSTNo" ClientIDMode="Static" Text="" Width="300px"
                                        MaxLength="50" CssClass="txtCss" onblur="BindShipAddressTxt('txtBillGSTNo','txtShipGSTNo')"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    CONTACT PERSON
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="txtBillContactName" ClientIDMode="Static" Text=""
                                        Width="300px" MaxLength="50" CssClass="txtCss" onblur="BindShipAddressTxt('txtBillContactName','txtShipContactName')"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    CONTACT NO.
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="txtBillContactNo" ClientIDMode="Static" Text="" Width="300px"
                                        MaxLength="20" CssClass="txtCss" onblur="BindShipAddressTxt('txtBillContactNo','txtShipContactNo')"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:CheckBox runat="server" ID="chkSameAddress" ClientIDMode="Static" Text="If despatch address is the same as bill to address click here. "
                                        ForeColor="Black" onchange="chkConcept()" TextAlign="Left" Font-Bold="true" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="width: 500px; vertical-align: top;">
                        <table>
                            <tr>
                                <td colspan="2" style="text-decoration: underline; color: #DD700B;">
                                    DESPATCH TO
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    M/S.
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="txtShipCompanyName" ClientIDMode="Static" Text=""
                                        MaxLength="50" Width="300px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    ADDRESS
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="txtShipAddress" ClientIDMode="Static" Text="" TextMode="MultiLine"
                                        Width="300px" Height="70px" onKeyUp="javascript:CheckCtrlLength(this, 499);"
                                        onChange="javascript:CheckCtrlLength(this, 499);"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    CITY
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="txtShipCity" ClientIDMode="Static" Text="" MaxLength="50"
                                        Width="300px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    ZIP CODE
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="txtShipPincode" ClientIDMode="Static" Text="" MaxLength="50"
                                        Width="300px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    STATE / CODE
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="txtShipState" ClientIDMode="Static" Text="" MaxLength="50"
                                        Width="300px"></asp:TextBox>
                                    &nbsp;/&nbsp;<asp:TextBox runat="server" ID="txtShipStateCode" ClientIDMode="Static"
                                        Text="" Width="300px" MaxLength="50" CssClass="txtCss"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    GSTIN No
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="txtShipGSTNo" ClientIDMode="Static" Text="" Width="300px"
                                        MaxLength="50" CssClass="txtCss"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    CONTACT PERSON
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="txtShipContactName" ClientIDMode="Static" Text=""
                                        Width="300px" MaxLength="50" CssClass="txtCss"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    CONTACT NO.
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="txtShipContactNo" ClientIDMode="Static" Text="" Width="300px"
                                        MaxLength="20" CssClass="txtCss"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="width: 500px;">
                        COMMENTS :
                        <asp:TextBox runat="server" ID="txtCustComments" ClientIDMode="Static" Text="" TextMode="MultiLine"
                            Width="500px" Height="100px" onKeyUp="javascript:CheckCtrlLength(this, 499);"
                            onChange="javascript:CheckCtrlLength(this, 499);" BorderColor="Black"></asp:TextBox>
                    </td>
                    <td style="width: 500px; text-align: center;">
                        <asp:LinkButton ID="lnkQuotePdf" runat="server" Text="DOWNLOAD QUOTE" OnClick="lnkQuotePdf_Click"
                            Font-Size="16px" Font-Bold="true"></asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <asp:Button runat="server" ID="btnSave" ClientIDMode="Static" Text="SAVE" OnClientClick="javascript:return CtrlQuoteCustEntry();"
                            OnClick="btnSave_Click" />
                        <asp:Button runat="server" ID="btnClear" ClientIDMode="Static" Text="CLEAR" />
                    </td>
                </tr>
            </table>
            <table cellspacing="0" rules="all" border="1" class="tableAddress" id="tbProforma" runat="server">
                <tr>
                    <td>
                        <div id="divProformaPdf" runat="server" style="width: 988px; float: left;">
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label>
        </div>
        <asp:HiddenField runat="server" ID="hdnTtsUsername" ClientIDMode="Static" Value="" />
    </div>
    </form>
    <script type="text/javascript">
        function CheckCtrlLength(textBox, maxLength) {
            if (textBox.value.length > maxLength) {
                alert("Max characters allowed are " + maxLength);
                textBox.value = textBox.value.substr(0, maxLength);
            }
        }

        function chkConcept() {
            if (document.getElementById('chkSameAddress').checked) {
                document.getElementById('txtShipCompanyName').value = document.getElementById('txtBillCompanyName').value;
                document.getElementById('txtShipAddress').value = document.getElementById('txtBillAddress').value;
                document.getElementById('txtShipCity').value = document.getElementById('txtBillCity').value;
                document.getElementById('txtShipState').value = document.getElementById('txtBillState').value;
                document.getElementById('txtShipPincode').value = document.getElementById('txtBillPincode').value;
                document.getElementById('txtShipStateCode').value = document.getElementById('txtBillStateCode').value;
                document.getElementById('txtShipGSTNo').value = document.getElementById('txtBillGSTNo').value;
                document.getElementById('txtShipContactName').value = document.getElementById('txtBillContactName').value;
                document.getElementById('txtShipContactNo').value = document.getElementById('txtBillContactNo').value;
                document.getElementById('txtShipAddress').disabled = true;
                document.getElementById('txtShipCity').disabled = true;
                document.getElementById('txtShipState').disabled = true;
                document.getElementById('txtShipPincode').disabled = true;
                document.getElementById('txtShipStateCode').disabled = true;
                document.getElementById('txtShipGSTNo').disabled = true;
                document.getElementById('txtShipContactName').disabled = true;
                document.getElementById('txtShipContactNo').disabled = true;
            }
            else {
                document.getElementById('txtShipCompanyName').value = "";
                document.getElementById('txtShipAddress').value = "";
                document.getElementById('txtShipCity').value = "";
                document.getElementById('txtShipState').value = "";
                document.getElementById('txtShipPincode').value = "";
                document.getElementById('txtShipStateCode').value = "";
                document.getElementById('txtShipGSTNo').value = "";
                document.getElementById('txtShipContactName').value = "";
                document.getElementById('txtShipContactNo').value = "";
                document.getElementById('txtShipAddress').disabled = false;
                document.getElementById('txtShipCity').disabled = false;
                document.getElementById('txtShipState').disabled = false;
                document.getElementById('txtShipPincode').disabled = false;
                document.getElementById('txtShipStateCode').disabled = false;
                document.getElementById('txtShipGSTNo').disabled = false;
                document.getElementById('txtShipContactName').disabled = false;
                document.getElementById('txtShipContactNo').disabled = false;
            }
        }

        function BindShipAddressTxt(ctrlBillID, ctrlShipID) {
            if (document.getElementById('chkSameAddress').checked) {
                document.getElementById("" + ctrlShipID + "").value = document.getElementById("" + ctrlBillID + "").value;
            }
            else {
                document.getElementById("" + ctrlShipID + "").value = '';
            }
        }

        function CtrlQuoteCustEntry() {
            var errmsg = '';
            document.getElementById('lblErrMsg').innerHTML = '';
            if (document.getElementById('txtBillCompanyName').value.length == 0)
                errmsg += 'Enter Billing Name M/S.<br/>';
            if (document.getElementById('txtBillAddress').value.length == 0)
                errmsg += 'Enter billing address<br/>';
            if (document.getElementById('txtBillCity').value.length == 0)
                errmsg += 'Enter billing city<br/>';
            if (document.getElementById('txtBillState').value.length == 0)
                errmsg += 'Enter billing state<br/>';
            if (document.getElementById('txtBillPincode').value.length == 0)
                errmsg += 'Enter billing pincode<br/>';
            if (document.getElementById('txtShipCompanyName').value.length == 0)
                errmsg += 'Enter Shipping Name M/S.<br/>';
            if (document.getElementById('txtShipAddress').value.length == 0)
                errmsg += 'Enter shipping address<br/>';
            if (document.getElementById('txtShipCity').value.length == 0)
                errmsg += 'Enter shipping city<br/>';
            if (document.getElementById('txtShipState').value.length == 0)
                errmsg += 'Enter shipping state<br/>';
            if (document.getElementById('txtShipPincode').value.length == 0)
                errmsg += 'Enter shipping pincode<br/>';
            if (errmsg.length > 0) {
                document.getElementById('lblErrMsg').innerHTML = errmsg;
                return false;
            }
            else
                return true;
        }
    </script>
</body>
</html>
