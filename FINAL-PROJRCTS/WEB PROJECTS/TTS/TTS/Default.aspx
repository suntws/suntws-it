<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Master.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="TTS._Default" EnableViewState="false" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:Label runat="server" ID="lblSuccessMsg" ClientIDMode="Static" Text="" ForeColor="Green"
        Font-Bold="true" Font-Size="12px"></asp:Label>
</asp:Content>
