<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="makemodelsearch.aspx.cs" Inherits="TTS.makemodelsearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .mergecss
        {
            text-align: center !important;
            font-size: 12px;
            font-weight: bold;
        }
        .configCss
        {
            background-color: #63D9E9;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        MAKE MODEL SEARCH
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table>
            <tr>
                <td>
                    <div style="width: 1060px; float: left; border: 1px solid #000; padding-bottom: 5px;
                        padding-top: 10px; padding-left: 5px; display: none;">
                        <div style="width: 300px; float: left;">
                            <span class="headCss">CATEGORY</span>
                            <asp:DropDownList runat="server" ID="ddlModelCategory" ClientIDMode="Static" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlModelCategory_IndexChange" Width="200px">
                            </asp:DropDownList>
                        </div>
                        <div style="width: 350px; float: left;">
                            <asp:DropDownList runat="server" ID="ddlModelValue" ClientIDMode="Static" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlModelValue_IndexChange" Width="300px">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div style="width: 1060px; float: left; border: 1px solid #000; padding-bottom: 5px;
                        padding-top: 10px; padding-left: 5px;">
                        <div style="width: 400px; float: left;">
                            <span class="headCss">OEM</span>
                            <asp:DropDownList runat="server" ID="ddlOem" ClientIDMode="Static" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlOem_IndexChange" Width="300px">
                            </asp:DropDownList>
                        </div>
                        <div style="width: 400px; float: left;">
                            <span class="headCss">MODEL</span>
                            <asp:DropDownList runat="server" ID="ddlModel" ClientIDMode="Static" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlModel_IndexChange" Width="300px">
                            </asp:DropDownList>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView runat="server" ID="gvMakeModelSearchList" AutoGenerateColumns="false"
                        Width="100%" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#A4F9F5"
                        HeaderStyle-Font-Bold="true" RowStyle-Height="22px" AlternatingRowStyle-BackColor="#FCEFF9"
                        OnRowDataBound="gvMakeModelSearchList_RowDataBound" OnRowCreated="gvMakeModelSearchList_RowCreated">
                        <Columns>
                            <asp:BoundField DataField="Manufacture" ItemStyle-Width="150px" HeaderText="OEM" />
                            <asp:BoundField DataField="ModelID" ItemStyle-Width="80px" HeaderText="MODEL" />
                            <asp:BoundField DataField="Lift_Type" ItemStyle-Width="100px" HeaderText="TYPE" />
                            <asp:BoundField DataField="Lift_WeightKg" ItemStyle-Width="50px" HeaderText="WEIGHT KG" />
                            <asp:BoundField DataField="Lift_CapacityKg" ItemStyle-Width="50px" HeaderText="CAPACITY KG" />
                            <asp:BoundField DataField="Wheel_Tyresize_Front" ItemStyle-Width="90px" HeaderText="FRONT" />
                            <asp:BoundField DataField="Wheel_Tyresize_Rear" ItemStyle-Width="90px" HeaderText="REAR" />
                            <asp:BoundField DataField="Nos_Tyres_Front" ItemStyle-Width="40px" HeaderText="FRONT" />
                            <asp:BoundField DataField="Nos_Tyres_Rear" ItemStyle-Width="40px" HeaderText="REAR" />
                            <asp:BoundField DataField="TyreType" ItemStyle-Width="80px" HeaderText="TYRE TYPE" />
                            <asp:BoundField DataField="SunAvailability" ItemStyle-Width="100px" HeaderText="AVAILABILITY" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
