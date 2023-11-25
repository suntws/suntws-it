<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="expscanrevise.aspx.cs" Inherits="TTS.expscanrevise" %>

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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: center" class="pageTitleHead">
        <asp:Label runat="server" ID="lblPageTitle" ClientIDMode="Static" Text=""></asp:Label>
        <asp:Label runat="server" ID="lblPageHead" ClientIDMode="Static" Text=""></asp:Label>
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div class="contPage">
        <table align="center">
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" Font-Bold="true"
                        Font-Size="12px" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView runat="server" ID="gv_PdiRevision" AutoGenerateColumns="false" Width="1070px"
                        HeaderStyle-BackColor="#c1dbf7" 
                        onselectedindexchanged="gv_PdiRevision_SelectedIndexChanged">
                        <Columns>
                            <asp:TemplateField HeaderText="CUSTOMER" ItemStyle-Width="250px">
                                <ItemTemplate>
                                    <asp:Label ID="lblCustomerName" Text='<%# Eval("custfullname") %>' runat="server"></asp:Label>
                                    <asp:HiddenField ID="hdnCustCode" Value='<%# Eval("custcode") %>' runat="server" />
                                    <asp:HiddenField ID="hdnPID" Value='<%# Eval("ID") %>' runat="server" />
                                    <asp:HiddenField ID="hdnOrderRefno" Value='<%# Eval("orderrefno") %>' runat="server" />
                                    <asp:HiddenField ID="hdn_OrderID" Value='<%# Eval("O_ID") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="WORK ORDER NO" ItemStyle-Width="250px">
                                <ItemTemplate>
                                    <asp:Label ID="lblWorkOrderNo" Text='<%# Eval("workorderno") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ORDER QTY" ItemStyle-Width="50px">
                                <ItemTemplate>
                                    <asp:Label ID="lblOrderQty" Text='<%# Eval("orderqty") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SCANNED QTY" ItemStyle-Width="50px">
                                <ItemTemplate>
                                    <asp:Label ID="lblScanedQty" Text='<%# Eval("ScanQty") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DELETE ACTION" ItemStyle-HorizontalAlign="Center"
                                ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:Button ID="btnViewOrderForDelete" runat="server" Text="View Order" CssClass="button"
                                        Width="80px" BackColor="#bb584f" Font-Bold="true" OnClick="btnViewOrderForDelete_click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="GIVE APPROVAL" ItemStyle-HorizontalAlign="Center"
                                ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:Button ID="btnViewOrderForApproval" runat="server" Text="APPROVE" CssClass="button"
                                        Width="80px" BackColor="#4caf50" Font-Bold="true" OnClick="btnViewOrderForApproval_click" /><br />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="EXCEL FILE" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="lnkPdiReport" Text="Download" OnClick="lnkPdiReport_Click"
                                        Font-Size="12px"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="div_DeleteRecord" style="display: none;">
                        <div style="text-align: center;">
                            <span class="Spancss">CustomerName :</span>&nbsp;
                            <asp:Label ID="lblSelectedCustomerName" runat="server" CssClass="Labelcss"></asp:Label>
                            <span class="Spancss">WorkOrderNo :</span>&nbsp;
                            <asp:Label ID="lblSelectedWorkOrderNo" runat="server" CssClass="Labelcss"></asp:Label>
                            <span class="Spancss">OrderQty :</span>&nbsp;
                            <asp:Label ID="lblSelectedOrderQty" runat="server" CssClass="Labelcss"></asp:Label>
                            <span class="Spancss">Scanned Qty :</span>&nbsp;
                            <asp:Label ID="lblSelectedScannedQty" runat="server" CssClass="Labelcss"></asp:Label>
                        </div>
                        <hr />
                        <table cellspacing="0" id="div_ddlSelect" rules="all" border="1" style="border-collapse: collapse;
                            border-color: #CE8686; width: 100%;">
                            <tr align="center" class="headCss" style="background-color: #EBEEED;">
                                <td class="tdhide">
                                    PLATFORM
                                </td>
                                <td class="tdhide">
                                    TYRE SIZE
                                </td>
                                <td class="tdhide">
                                    RIM SIZE
                                </td>
                                <td>
                                    TYRE TYPE
                                </td>
                                <td>
                                    BRAND
                                </td>
                                <td>
                                    SIDEWALL
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 200px">
                                    <asp:DropDownList ID="ddlPlatform" runat="server" ClientIDMode="Static" AutoPostBack="true"
                                        CssClass="form-control" Width="200px" OnSelectedIndexChanged="ddlPlatform_IndexChange">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 170px">
                                    <asp:DropDownList ID="ddlTyreSize" runat="server" ClientIDMode="Static" AutoPostBack="true"
                                        CssClass="form-control" Width="170px" OnSelectedIndexChanged="ddlTyreSize_IndexChange">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 210px">
                                    <asp:DropDownList ID="ddlRimSize" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="ddlRimSize_IndexChange"
                                        AutoPostBack="true" CssClass="form-control" Width="210px">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 150px">
                                    <asp:DropDownList ID="ddlTyretype" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="ddlTyretype_IndexChange"
                                        AutoPostBack="true" CssClass="form-control" Width="150px">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 150px">
                                    <asp:DropDownList ID="ddlBrand" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="ddlBrand_IndexChange"
                                        AutoPostBack="true" CssClass="form-control" Width="150px">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 170px">
                                    <asp:DropDownList ID="ddl_Sidewall" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="ddl_Sidewall_IndexChange"
                                        AutoPostBack="true" CssClass="form-control" Width="170px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                        <hr />
                        <table align="center">
                            <tr>
                                <td>
                                    <asp:Button Text="BARCODE WISE" BorderStyle="None" ID="Button1" CssClass="Initial"
                                        runat="server" OnClick="Tab_Click" />
                                    <asp:Button Text="ITEM QTY WISE" BorderStyle="None" ID="Button2" CssClass="Initial"
                                        runat="server" OnClick="Tab_Click" />
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
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:CheckBox runat="server" ID="checkAllChk" ClientIDMode="Static" />
                                                            ALL
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chk_selectQty" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <asp:Label runat="server" ID="lblDelSelectCount" ClientIDMode="Static" Text="" Font-Bold="true"
                                                Font-Size="14px"></asp:Label>
                                            <asp:Button runat="server" ID="btnDeletePdi" ClientIDMode="Static" Text="DELETE"
                                                CssClass="btn btn-danger" OnClick="btnDeletePdi_click" OnClientClick="javascript:return ctrlBtnDelete();" />
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
            <tr>
                <td>
                    <div id="div_PdiReject" style="display: none;">
                        <table cellspacing="0" rules="all" border="1" style="border: solid 1px #525252; border-collapse: collapse;
                            width: 100%; line-height: 25px;">
                            <tr>
                                <td colspan="8" class="pageTitleHead">
                                    ASSIGNED STECNIL PDI REJECTION
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    CUSTOMER
                                </th>
                                <td colspan="3">
                                    <asp:Label runat="server" ID="lblCustomer" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                                </td>
                                <th>
                                    ORDER REF NO
                                </th>
                                <td colspan="3">
                                    <asp:Label runat="server" ID="lblOrderNo" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    WORK ORDER
                                </th>
                                <td>
                                    <asp:Label runat="server" ID="lblWorkOrder" ClientIDMode="Static" Text="" Font-Bold="true"></asp:Label>
                                </td>
                                <th>
                                    TOT QTY
                                </th>
                                <td>
                                    <asp:Label runat="server" ID="lblTotQty" ClientIDMode="Static" Text="" Font-Bold="true"
                                        Font-Size="18px"></asp:Label>
                                </td>
                                <th>
                                    ASSIGN QTY
                                </th>
                                <td>
                                    <asp:Label runat="server" ID="lblEarmarkQty" ClientIDMode="Static" Text="" Font-Bold="true"
                                        Font-Size="18px"></asp:Label>
                                </td>
                                <th>
                                    UN-PDI QTY
                                </th>
                                <td>
                                    <asp:Label runat="server" ID="lblNotPdiQty" ClientIDMode="Static" Text="" Font-Bold="true"
                                        Font-Size="18px"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="8">
                                    <asp:GridView ID="GV_Condition" runat="server" AutoGenerateColumns="false" Width="100%"
                                        CssClass="gridcss">
                                        <Columns>
                                            <asp:BoundField HeaderText="PLATFORM" DataField="config" />
                                            <asp:BoundField HeaderText="TYRE SIZE" DataField="tyresize" />
                                            <asp:BoundField HeaderText="RIM" DataField="rimsize" />
                                            <asp:BoundField HeaderText="TYPE" DataField="tyretype" />
                                            <asp:BoundField HeaderText="BRAND" DataField="brand" />
                                            <asp:BoundField HeaderText="SIDEWALL" DataField="sidewall" />
                                            <asp:BoundField HeaderText="STENCIL NO" DataField="stencilno" />
                                            <asp:BoundField HeaderText="GRADE" DataField="grade" />
                                            <asp:BoundField HeaderText="DOM" DataField="yearofmanufacture" />
                                            <asp:BoundField HeaderText="PART" DataField="EarmarkPart" />
                                            <asp:BoundField HeaderText="ASSIGNED BY" DataField="EarmarkBy" />
                                            <asp:BoundField HeaderText="APPROVE BY" DataField="Earmark_PpcBy" />
                                            <asp:TemplateField HeaderText="REASON OF REJECTION">
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtReason" CssClass="form-control" MaxLength="50"
                                                        Width="300px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="8" style="text-align: right; padding-right: 120px;">
                                    <asp:Button ID="btnPdiRejectSave" runat="server" Text="SAVE REJECT REASON" ClientIDMode="Static"
                                        CssClass="btn btn-success" OnClick="btnPdiRejectSave_Click" OnClientClick="javascript:return cntrlSave();" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView runat="server" ID="gvUnmatchBarcode" ClientIDMode="Static" AutoGenerateColumns="true"
                        Width="100%" CssClass="gridcss">
                    </asp:GridView>
            </tr>
        </table>
    </div>
    <asp:HiddenField ID="hdnSelectedRow" Value="" runat="server" />
    <asp:HiddenField ID="hdnSelectedCustcode" Value="" runat="server" />
    <asp:HiddenField ID="hdnCurrentPID" Value="" runat="server" />
    <script type="text/javascript">
        function cntrlSave() {
            var count = 0;
            $('.form-control').each(function (index) {
                if ($(this).val() == "") {
                    $(this).css("border", "1px solid #ff0000");
                    count = 1;
                } else
                    $(this).css("border", "1px solid #000000");
            });
            if (count > 0) {
                alert("Enter Reason");
                return false;
            } else
                return true;
        }
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
        function ctrlBtnDelete() {
            if ($("[id*=gv_ScanedList_chk_selectQty_]:checked").length != 0)
                return true;
            else {
                alert("Choose atleast one barcode");
                return false;
            }
        }
    </script>
</asp:Content>
