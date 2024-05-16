<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="PriceSheetCreation.aspx.cs" Inherits="TTS.PriceSheetCreation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
    <link href="Styles/GridviewScroll.css" rel="stylesheet" type="text/css" />
    <link href="Styles/datemonthyear.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        th
        {
            line-height: 15px;
            text-align: left;
            font-weight: normal;
        }
        td
        {
            font-weight: bold;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div align="center" class="pageTitleHead">
        EXPORT - LIST PRICE UPLOAD</div>
        <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Size="16px"
            ForeColor="Red"></asp:Label>
    </div>
    <div class="contPage">
        <table cellspacing="0" rules="all" border="1" style="background-color: #f5ffff; width: 100%;
            border-color: #d6d6d6; border-collapse: separate; height: 245px;">
            <tr>
                <th>
                    CUSTOMER
                </th>
                <td colspan="7">
                    <asp:DropDownList runat="server" ID="ddlCustomer" ClientIDMode="Static" CssClass="form-control"
                        AutoPostBack="true"  Width="400px" 
                        onselectedindexchanged="ddlCustomer_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    CATEGORY
                </th>
                <td>
                    <asp:DropDownList ID="ddl_Category" ClientIDMode="Static" CssClass="form-control"
                        AutoPostBack="true" runat="server" Width="120px" 
                        onselectedindexchanged="ddl_Category_SelectedIndexChanged" >
                    </asp:DropDownList>
                </td>
            
             <th>
                    PLATFORM
                </th>
                <td>
                    <asp:DropDownList ID="ddl_Platform" ClientIDMode="Static" CssClass="form-control"
                        AutoPostBack="true" runat="server" Width="120px" OnSelectedIndexChanged="ddl_Platform_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    PRICE SHEET REF NO
                </th>
                <td colspan="3">
                    <asp:TextBox runat="server" ID="txtPriceSheetRefNo" ClientIDMode="Static" CssClass="form-control"
                        Width="400px" MaxLength="50"></asp:TextBox>
                </td>
                <th>
                    RATES ID
                </th>
                <td>
                    <asp:TextBox ID="txtRatesId" runat="server" ClientIDMode="Static" Text="" Width="200px"
                        Height="25px" CssClass="form-control" MaxLength="50"></asp:TextBox>
                </td>
                <th>
                    END DATE
                </th>
                <td>
                    <asp:TextBox ID="txtEndDate" runat="server" ClientIDMode="Static" Text="" Width="200px"
                        Height="25px" CssClass="form-control"></asp:TextBox>
                </td>
            </tr>


            <tr>
            <td>
                   <asp:FileUpload ID="FileUpload1" runat="server"  ClientIDMode="Static" accept=".xls,.xlsx,.csv,.txt"
                         CssClass="btn btn-warning" />
                        </td>   
            </tr>
            
            
            
           
            
             
            <tr style="text-align: center;">
                <td colspan="8">
                   <asp:Button ID="btnXLUPLD" runat="server" Text="XL UPLOAD" 
                        CssClass="btn btn-success" BackColor="#2196f3" onclick="btnXLUPLD_Click"
                          />
                 </td>       
            </tr>
     </div>

     <script src="Scripts/datemonthyear.js" type="text/javascript"></script>
    <script src="Scripts/gridviewScroll1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#txtEndDate").datepicker({ minDate: "+1D", maxDate: "+120D" }).keydown(function (e) { e.keyWhich = 0; e.keyCode = 0; e.preventDefault(); });
        });
           
       
   </script>
</asp:Content>
