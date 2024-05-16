<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="cotsRimSupplierMaster.aspx.cs" Inherits="TTS.cotsRimSupplierMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        table
        {
            background-color: #E4F7CF !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        <asp:Label runat="server" ID="lblPageTitle" ClientIDMode="Static" Text="RIM SUPPLIER MASTER"></asp:Label>
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table style="width: 100%; border-width: thin;" cellspacing="0" rules="all" class="tableCss">
            <tr>
                <td>
                    <table style="width: 100%;" cellspacing="0" rules="all" border="1" class="tableCss">
                        <tr>
                            <th>
                                SUPPLIER
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txt_suppliername" ClientIDMode="Static" Text="" Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                CONTACT PERSON
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txt_contactperson" ClientIDMode="Static" Text=""
                                    Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                CONTACT NUMBER
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txt_contactnumber" ClientIDMode="Static" Text=""
                                    Width="300px" onkeypress="if(event.keyCode<48 || event.keyCode>57)event.returnValue=false;"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
                <th>
                    ADDRESS
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txt_address" ClientIDMode="Static" Text="" Height="80px"
                        Width="300px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <table style="width: 100%;" cellspacing="0" rules="all" border="1" class="tableCss">
                        <tr>
                            <th>
                                EMAIL ID
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txt_emailid" ClientIDMode="Static" Text="" Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
                <td colspan="2">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button runat="server" ID="btnSave" ClientIDMode="Static" Text="SAVE" OnClick="btnSave_Click"
                        BackColor="#33CC33" />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button runat="server" ID="btnClear" ClientIDMode="Static" Text="CLEAR" OnClick="btnClear_Click"
                        BackColor="#FF6666" BorderColor="White" />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button runat="server" ID="btnDelete" ClientIDMode="Static" Text="SUSPEND" OnClick="btnDelete_Click"
                        BackColor="#6666FF" />
                </td>
            </tr>
        </table>
        <div hidden="visible">
            <asp:GridView runat="server" ID="dataGridView1" RowStyle-Height="25px"
                AutoGenerateColumns="false" CssClass="gridcss" 
                AutoGenerateSelectButton="True" 
                onselectedindexchanged="dataGridView1_SelectedIndexChanged">
                <Columns>
                    <asp:BoundField HeaderText="ID" DataField="ID" />
                    <asp:BoundField HeaderText="Supplier Name" DataField="SupplierName" />
                    <asp:BoundField HeaderText="Address" DataField="Address" />
                    <asp:BoundField HeaderText="Contact Person" DataField="ContactPerson" />
                    <asp:BoundField HeaderText="Contact Number" DataField="ContactNumber" />
                    <asp:BoundField HeaderText="Email-ID" DataField="EmailID" />
                </Columns>
            </asp:GridView>
        </div>
    </div>

</asp:Content>
