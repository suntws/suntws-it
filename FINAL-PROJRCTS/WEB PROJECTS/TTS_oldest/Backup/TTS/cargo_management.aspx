<%@ Page Title="" MasterPageFile="~/master.Master" Language="C#" AutoEventWireup="true" CodeBehind="cargo_management.aspx.cs" 
Inherits="TTS.cargo_management" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div align="center" class="pageTitleHead">
        <asp:Label runat="server" ID="lblPageHead" ClientIDMode="Static" Text=""></asp:Label>
     </div>
    <div id="displaycontent" class="contPage">
        <asp:PlaceHolder runat="server" ID="plhCargoManagement"></asp:PlaceHolder>
    </div>
</asp:Content>