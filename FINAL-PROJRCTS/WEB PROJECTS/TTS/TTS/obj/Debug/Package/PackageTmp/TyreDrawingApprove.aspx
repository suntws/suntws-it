<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="TyreDrawingApprove.aspx.cs" Inherits="TTS.TyreDrawingApprove" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/TinyStyle.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/TinyboxScript.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        <asp:Label runat="server" ID="lblApproveHead" ClientIDMode="Static" Text=""></asp:Label>
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <asp:GridView runat="server" ID="gvDwgApprovePendingList" AutoGenerateColumns="false"
            Width="1080px" AlternatingRowStyle-BackColor="#f5f5f5" AllowPaging="true" PageSize="25"
            PagerStyle-Height="30px" PagerStyle-Font-Bold="true" PagerStyle-Font-Size="15px"
            PagerStyle-HorizontalAlign="Center" OnPageIndexChanging="gvDwgApprovePendingList_PageIndex"
            PagerStyle-VerticalAlign="Middle" RowStyle-Height="20px">
            <HeaderStyle Font-Bold="true" BackColor="#fefe8b" Height="20px" />
            <Columns>
                <asp:TemplateField HeaderText="CATEGORY">
                    <ItemTemplate>
                        <asp:Label ID="lblApprovecategory" runat="server" Text='<%# Eval("filecategory") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="config" HeaderText="PLATFORM" ItemStyle-Width="" />
                <asp:BoundField DataField="brand" HeaderText="BRAND" ItemStyle-Width="" />
                <asp:BoundField DataField="sidewall" HeaderText="SIDEWALL" ItemStyle-Width="" />
                <asp:BoundField DataField="tyretype" HeaderText="GRADE" ItemStyle-Width="" />
                <asp:BoundField DataField="tyresize" HeaderText="SIZE" ItemStyle-Width="" />
                <asp:BoundField DataField="rimwidth" HeaderText="RIM WIDTH" ItemStyle-Width="40px" />
                <asp:BoundField DataField="ETRTOREF" HeaderText="ETRTO REF" ItemStyle-Width="" />
                <asp:BoundField DataField="RIMTYPE" HeaderText="RIM TYPE" ItemStyle-Width="" />
                <asp:BoundField DataField="NoOfHoles" HeaderText="NO OF STUD HOLES" ItemStyle-Width="50px" />
                <asp:BoundField DataField="PCD" HeaderText="PCD" ItemStyle-Width="" />
                <asp:BoundField DataField="BOREDIA" HeaderText="BOREDIA" ItemStyle-Width="" />
                <asp:BoundField DataField="HoleDia" HeaderText="STUD HOLES DIA" ItemStyle-Width="50px" />
                <asp:TemplateField HeaderText="ACTION" ItemStyle-Width="40px">
                    <ItemTemplate>
                        <a onclick="TINY.box.show({iframe:'TyreDwgPopup.aspx?dwgid=<%# Eval("ID") %>&Status=<%# Eval("statuspage") %>&dwgstatus=<%# Eval("lnkText") %>&viewtype=<%# Eval("lnkremarks") %>',boxid:'frameless',width:1000,height:750,fixed:false,maskid:'bluemask',maskopacity:40,closejs:function(){}})"
                            style="cursor: pointer; text-decoration: underline; color: #082DEA; font-size: 10px;">
                            <%# Eval("lnkText") %></a>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
