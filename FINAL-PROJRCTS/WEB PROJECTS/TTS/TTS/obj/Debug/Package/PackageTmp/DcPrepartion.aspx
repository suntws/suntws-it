<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="DcPrepartion.aspx.cs" Inherits="TTS.DcPrepartion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
     <link href="Styles/pdistyle.css" rel="stylesheet" type="text/css" />
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/cotsScript.js" type="text/javascript"></script>
    <style type="text/css">


         th
        {
            text-align: center;
            font-weight: normal;
            background-color: #cccfff;
        }
        td
        {
            font-weight: bold;
            text-align: center;
        }
    </style>
   

<%--    <style type="text/css">

         .spanCss
        {
            color: Green;
            font-weight: bold;
            font-size: 14px;
            font-family: Times New Roman;
        }
       

        .tableCss {
            width: 1000px;
            height: 100px;
        }
       

        .auto-style1 {
            width: 1230px;
        }
       

        .auto-style2 {
            margin-right: 0px;
        }
       

        </style>--%>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center;" class="pageTitleHead">
        <asp:Label runat="server" ID="lblpageHeading" Text="MMNDCPREPARATION"></asp:Label>
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>

    
    <div class="contPage">
        <table  align="center" >
            <tr>
                <td>

                    
                    <asp:GridView ID="GRVIEWLALL" runat="server"   ClientIDMode="Static" AutoGenerateColumns="false"
                        Width="1070px" AlternatingRowStyle-BackColor="#ecf6ff"
                        RowStyle-Height="20px"
                          CssClass="auto-style2" Height="166px">
                        <HeaderStyle BackColor="#a1ccf3" Font-Bold="true" Height="22px" />
                        <Columns>
                            
                            <%--<asp:BoundField DataField="Dcno" HeaderText="DCNO"  />--%>
                            <asp:TemplateField HeaderText="Dcno">
                              <ItemTemplate>
                              <asp:Label ID="lblDcno" Text='<%# Eval("DCNO") %>' runat="server"></asp:Label>
                                  <asp:HiddenField ID="hdntid" Value='<%# Eval("tid") %>' runat="server" />
                                  

                                  <%--<asp:HiddenField ID="hdntid" Value="'<%# Eval("Dcno") %>'" runat="server" />--%>
                               </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="VehicleNo" >
                            <ItemTemplate>
                                <asp:Label ID="Lblvehicle" Text='<%# Eval("VehicleNo") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                                    </asp:TemplateField>
                               <%--<asp:BoundField DataField="VehicleNo" HeaderText="VechicleNo" />--%>
                            <asp:BoundField DataField="ScannedDate" HeaderText="ScannedDate" />
                             <asp:BoundField DataField="MOVEDDATE" HeaderText="MOVEDDATE" />
                            <asp:TemplateField HeaderText="ORDERQTY">
                                <ItemTemplate>
                                    <asp:Label ID="Lblorderqty" Text='<%# Eval("ORDERQTY") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                           <%-- <asp:BoundField DataField="ORDERQTY" HeaderText="ORDERQTY" />--%>
                            <asp:BoundField DataField="CurrentStatus" HeaderText="CurrentStatus" />
                            <asp:BoundField DataField="PLANT" HeaderText="PLANT" />
                            <asp:TemplateField HeaderText="ACTION" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                           
                                  <ItemTemplate>
                                        <asp:Button ID="viewlist" ClientIDMode="Static" runat="server"  TEXT='VIEWLIST'  OnClick="lnkProcessOrders_Click" />

                                         <%--OnClick="lnkProcessOrders_Click"--%>


                                     </ItemTemplate>
                            </asp:TemplateField>
                        
                           
                            
                            </Columns>

                    </asp:GridView>

                    
                    <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" Font-Bold="true"
                        Font-Size="12px" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td >

                    
                    &nbsp;</td>
                </tr>
            <%--</table>--%>
       <%-- </div>--%>
    
    


    <tr>
        <td>
        

            <div id="divbarcodelist" style="display: none;">
                <div style="text-align: center;">
                    <span class="Spancss">DCNO :</span>&nbsp;
                    <asp:Label ID="lblDCNO" runat="server" CssClass="Labelcss"></asp:Label>
                    <span class="Spancss">VEHICLENO :</span>&nbsp;
                            <asp:Label ID="lblSelectedVEHICLENO" runat="server" CssClass="Labelcss"></asp:Label>
                            <span class="Spancss">OrderQty :</span>&nbsp;
                            <asp:Label ID="lblSelectedOrderQty" runat="server" CssClass="Labelcss"></asp:Label>
                            <span class="Spancss">TotalWeight :</span>&nbsp;
                            <asp:Label ID="weight" runat="server" CssClass="Labelcss"></asp:Label>
                    </div>
                <hr />
                 <table align="center">
                            <tr>
                                <td>
                                    <asp:Button Text="BARCODE WISE" BorderStyle="None" ID="Button1" CssClass="Initial" runat="server"  OnClick="Tab_Click"  />
                                        
                                    <asp:Button Text="ITEM QTY WISE" BorderStyle="None" ID="Button2" CssClass="Initial"
                                        runat="server" 
                                    OnClick="Tab_Click" />
                                    <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                                        <asp:View ID="Tab1" runat="server">
                                              
                                            <asp:GridView runat="server" ID="gv_ScanedList" AutoGenerateColumns="false" Width="100%"
                                                HeaderStyle-BackColor="#c1dbf7" AllowPaging="true" OnPageIndexChanging="gv_ScanedList_PageIndex"
                                                PageSize="200" PagerStyle-Height="30px" PagerStyle-Font-Bold="true" PagerStyle-Font-Size="15px"
                                                PagerStyle-HorizontalAlign="Center" PagerStyle-VerticalAlign="Middle">
                                                <PagerSettings Mode="Numeric" Position="Bottom" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="PLATFORM">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblconfig" Text='<%# Eval("Config") %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="TYRE SIZE">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbltyresize" Text='<%# Eval("tyresize") %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="RIM SIZE">
                                                        <ItemTemplate>
                                                            <asp:Label ID="rimsize" Text='<%# Eval("rimsize") %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="TYRE TYPE">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbltyretype" Text='<%# Eval("tyretype") %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="BRAND">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblbrand" Text='<%# Eval("brand") %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="SIDEWALL">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSidewall" Text='<%# Eval("sidewall") %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="BARCODE">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblbarcode" Text='<%# Eval("barcode") %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:CheckBox runat="server" ID="checkAllChk" ClientIDMode="Static" />
                                                            ALL
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chk_selectQty" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>--%>
                                                </Columns>
                                            </asp:GridView>
                                            <%--<asp:Label runat="server" ID="lblDelSelectCount" ClientIDMode="Static" Text="" Font-Bold="true"
                                                Font-Size="14px"></asp:Label>--%>
                                            <asp:Button runat="server" ID="btnapprove" ClientIDMode="Static" Text="APPROVE FOR LOADING"
                                                CssClass="btn btn-success" 
                                                OnClick="btnapprove_click" />


                                                <asp:Button runat="server" ID="exportbtn" ClientIDMode="Static" Text="export to excel"
                                                CssClass="btn btn-success"  OnClick="exportclick"/>
 
                                                
                                                
                                                 <%--OnClientClick="javascript:return ctrlBtnDelete();" />--%>
                                 </asp:View>
                                        <asp:View ID="Tab2" runat="server">
                                            <asp:GridView runat="server" ID="gvScannedItemWise" Width="1070px" HeaderStyle-Font-Size="14px"
                                                HeaderStyle-BackColor="#eaf1f9" ClientIDMode="Static" ViewStateMode="Enabled">
                                            </asp:GridView>
                                        </asp:View>
                                    </asp:MultiView>
                                </td>
                            </tr>
                        </table>
                    </div>
            </td>
        </tr>
            </table>
        </div>

         <asp:HiddenField ID="hdnSelectedRow" Value="" runat="server" />   
    <asp:HiddenField ID="Hdndcid" runat="server" />

    <script type="text/javascript">

        $(function () {
            $('#checkAllChk').click(function () {
                if ($("[id*=gv_ScanedList_chk_selectQty_]").length > 0) {
                    if ($(this).attr('checked') == "checked")
                        $("[id*=gv_ScanedList_chk_selectQty_]").attr('checked', true)
                    else
                        $("[id*=gv_ScanedList_chk_selectQty_]").attr('checked', false)
                }
                else {
                    alert('No records');
                    $(this).attr('checked', false);
                }
                $('#lblDelSelectCount').html($("[id*=gv_ScanedList_chk_selectQty_]:checked").length + ' QTY SELECTED FOR ');
            });
            $("[id*=gv_ScanedList_chk_selectQty_]").click(function () {
                $('#lblDelSelectCount').html($("[id*=gv_ScanedList_chk_selectQty_]:checked").length + ' QTY SELECTED FOR ');
                $('#checkAllChk').attr('checked', false);
            });
        });

    </script>
                       
                           

            
        


          
    
    
    
</asp:Content>
