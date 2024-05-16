<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="revisecargoentry.ascx.cs"
    Inherits="TTS.cargomanagement.revisecargoentry" %>
<div style="width: 1080px;">
    <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label>
    <div id="divContent">
        <table>
            <tr>
                <td>
                    <table cellspacing="0" rules="all" border="1" style="border-collapse: collapse; border-color: #000;
                        width: 1078px;" id="tbCargoPlan">
                        <tr>
                            <th>
                                CUSTOMER
                            </th>
                            <td>
                                <asp:DropDownList ID="ddlCustomer" ClientIDMode="Static" runat="server" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChange"
                                    Width="400px" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <th>
                                ORDER REF NO.
                            </th>
                            <asp:DropDownList ID="ddlOrderNo" ClientIDMode="Static" runat="server" OnSelectedIndexChanged="ddlOrderNo_SelectedIndexChange"
                                Width="150px" AutoPostBack="true">
                            </asp:DropDownList>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</div>
