<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="currentdebosters.aspx.cs" Inherits="TTS.currentdebosters" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: center;" class="pageTitleHead">
        DOMESTIC DEBTORS MASTERS
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1" style="width: 100%; border-color: White;
            border-collapse: separate;">


            <tr>
                <td>
                    <asp:LinkButton ID="lnkbtnDownload" runat="server" ClientIDMode="Static" Text="DOWNLOAD"
                        OnClick="btnDownload_Click"></asp:LinkButton>
                    
                </td>
                </tr>
            <tr>
                <td>
                    <asp:GridView runat="server" ID="gvdebosters" AutoGenerateColumns="true" Width="300px"
                        CssClass="gridcss"></asp:GridView> 
                    
                </td>
                </tr>
            </table>
        </div>

</asp:Content>
