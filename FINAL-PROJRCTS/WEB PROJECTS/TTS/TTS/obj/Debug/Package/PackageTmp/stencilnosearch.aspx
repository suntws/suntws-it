<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="stencilnosearch.aspx.cs"
    MasterPageFile="~/master.Master" Inherits="TTS.stencilnosearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .form-control
        {
            width: 170px;
            height: 30px;
            font-size: 20px;
            font-weight: bold;
            background-color: #fff;
            border: 1px solid #000;
            border-radius: 4px;
        }
        .btn
        {
            text-decoration: none;
            padding: 4px 10px;
            font-size: 14px;
            font-weight: bold;
            text-align: center;
            vertical-align: middle;
            cursor: pointer;
            border: 1px solid transparent;
            border-radius: 4px;
        }
        .btn-success
        {
            color: #fff;
            background-color: #5cb85c;
            border-color: #4cae4c;
        }
        .btn-success:hover
        {
            color: #fff;
            background-color: #449d44;
            border-color: #398439;
        }
        .tbl
        {
            width: 100%;
            border-color: #96c2e8;
            border-collapse: collapse;
            line-height: 30px;
            font-size: 14px;
        }
        .tbl th
        {
            font-weight: normal;
            text-align: right;
        }
        .tbl td
        {
            font-weight: bold;
            background-color: #b2f7f4;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: center;" class="pageTitleHead">
        STENCIL NO SEARCH
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <asp:ScriptManager runat="server" ID="ScriptManager1">
    </asp:ScriptManager>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="width: 100%; border-color: White;
            border-collapse: separate;">
            <tr style="width: 100%;">
                <th>
                    <span style="font-size: 20px; color: #1ba740; font-style: italic; text-align: right;">
                        Stencil No </span>
                    <asp:TextBox ID="txt_StencilNo" runat="server" ClientIDMode="Static" CssClass="form-control"
                        MaxLength="10"></asp:TextBox>
                    <asp:Button runat="server" ID="btnSearch" ClientIDMode="Static" Text="SEARCH" OnClick="btnSearch_Click"
                        CssClass="btn btn-success" />
                    <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"
                        Font-Bold="true"></asp:Label>
                </th>
            </tr>
            <tr style="width: 100%;">
                <td>
                    <asp:UpdatePanel ID="updatepanel1" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <asp:DataList runat="server" ID="dlStencilData" ClientIDMode="Static" RepeatColumns="2"
                                RepeatDirection="Horizontal" RepeatLayout="Table" Width="100%">
                                <ItemTemplate>
                                    <table cellspacing="0" rules="all" border="1" class="tbl">
                                        <tr>
                                            <th>
                                                PLATFORM
                                            </th>
                                            <td>
                                                <%# Eval("config")%>
                                            </td>
                                            <th>
                                                PROCESS ID
                                            </th>
                                            <td>
                                                <%# Eval("processid")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                TYRE SIZE
                                            </th>
                                            <td>
                                                <%# Eval("tyresize")%>
                                            </td>
                                            <th>
                                                STENCIL NO
                                            </th>
                                            <td>
                                                <%# Eval("StencilNo")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                RIM
                                            </th>
                                            <td>
                                                <%# Eval("rimsize")%>
                                            </td>
                                            <th>
                                                GARDE
                                            </th>
                                            <td>
                                                <%# Eval("grade")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                TYPE
                                            </th>
                                            <td>
                                                <%# Eval("tyretype")%>
                                            </td>
                                            <th>
                                                FWT
                                            </th>
                                            <td>
                                                <%# Eval("fwt")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                BRAND
                                            </th>
                                            <td>
                                                <%# Eval("brand")%>
                                            </td>
                                            <th>
                                                PLANT
                                            </th>
                                            <td style="background-color: #61fb45;">
                                                <%# Eval("plant")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                SIDEWALL
                                            </th>
                                            <td>
                                                <%# Eval("sidewall")%>
                                            </td>
                                            <th>
                                                LOCATION
                                            </th>
                                            <td>
                                                <%# Eval("location")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                DOM
                                            </th>
                                            <td>
                                                <%# Eval("DOM")%>
                                            </td>
                                            <th>
                                                WAREHOUSE LOCATION
                                            </th>
                                            <td>
                                                <%# Eval("warehouse_location")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                CUSTOMER
                                            </th>
                                            <td colspan="3" style="background-color: #61fb45;">
                                                <strong>
                                                    <%# Eval("custfullname")%></strong>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                <%# Eval("Qstring").ToString() == "CLAIM" ? "CLAIM" : "ORDER REF NO"%>
                                            </th>
                                            <td colspan="2">
                                                <%# Eval("orderrefno")%>
                                            </td>
                                            <td>
                                                <%# Eval("OrderDate")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                <%# Eval("Qstring").ToString() == "CLAIM" ? "CREDIT NOTE" : "PROFORMA REF"%>
                                            </th>
                                            <td colspan="2">
                                                <%# Eval("proformarefno")%>
                                            </td>
                                            <td>
                                                <%# Eval("ProformaDate")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                WORK ORDER
                                            </th>
                                            <td colspan="2">
                                                <%# Eval("workorderno")%>
                                            </td>
                                            <td>
                                                <%# Eval("WorkOrderDate")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                <%# Eval("Qstring").ToString() == "CLAIM" ? "SETTLEMENT" : "INVOICE"%>
                                            </th>
                                            <td colspan="2">
                                                <%# Eval("invoiceno")%>
                                            </td>
                                            <td>
                                                <%# Eval("InvoiceDate")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                DISPATCHED ON
                                            </th>
                                            <td colspan="3">
                                                <%# Eval("DispatchedDate")%>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:DataList>
                            <asp:Button ID="btnTrigger" runat="server" ClientIDMode="Static" Style="visibility: hidden;"
                                OnClick="btnTrigger_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        $(function () {
            $('#txt_StencilNo').focus();
            $('#txt_StencilNo').blur(function () {
                if ($(this).val().length == 10) {
                    $('#btnTrigger').trigger('click');
                }
            });
            $('#dlStencilData').ready(function () {
                if ($('.tbl').length > 1) {
                    $('#dlStencilData').css({ 'width': '100%' })
                }
                else {
                    $('#dlStencilData').css({ 'width': '50%' })
                }
            })
        });
    </script>
</asp:Content>
