<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="expscanpdi_1.aspx.cs" Inherits="TTS.expscanpdi_1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/scotsexport.css" rel="stylesheet" type="text/css" />
    <link href="Styles/pdistyle.css" rel="stylesheet" type="text/css" />
    <link href="Styles/TinyStyle.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: center" class="pageTitleHead">
        <asp:Label runat="server" ID="lblPageTitle" ClientIDMode="Static" Text=""></asp:Label>
        PDI INSPECTION THROUGH BARCODE SCAN
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div class="contPage">
        <asp:ScriptManager runat="server" ID="scriptManager1">
        </asp:ScriptManager>
        <table cellspacing="0" rules="all" border="1" style="border: solid 1px #525252; border-collapse: collapse;
            width: 100%;">
            <tr>
                <td colspan="5">
                    <table cellspacing="0" rules="all" border="1" class="tbMas">
                        <tr>
                            <th>
                                CUSTOMER
                            </th>
                            <td colspan="5">
                                <asp:Label runat="server" ID="lblCustomer" ClientIDMode="Static" Text=""></asp:Label>
                                <asp:HiddenField runat="server" ID="hdnCustCode" ClientIDMode="Static" Value="" />
                            </td>
                        </tr>
                        <tr>
                            <th>
                                ORDER REF NO
                            </th>
                            <td colspan="5">
                                <asp:Label runat="server" ID="lbl_OrderNo" ClientIDMode="Static" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                DOWNLOAD
                            </th>
                            <td colspan="5">
                                <asp:LinkButton runat="server" ID="lnkWorkOrder" ClientIDMode="Static" Text="" OnClick="lnkWorkOrder_CLick"></asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                W/O NO
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txt_OrderRefNo" ClientIDMode="Static" Text="" MaxLength="100"
                                    Width="150px" CssClass="form-control"></asp:TextBox>
                            </td>
                            <th>
                                ORDER QTY
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txt_OrderQty" ClientIDMode="Static" Text="" MaxLength="4"
                                    Enabled="false" Width="60px" CssClass="form-control" Font-Size="25px"></asp:TextBox>
                            </td>
                            <th>
                                INSPECTED QTY
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtInspectedQty" ClientIDMode="Static" Text="" MaxLength="4"
                                    Enabled="false" Width="60px" Font-Bold="true" CssClass="form-control" Font-Size="25px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <asp:UpdatePanel ID="updatePanel2" runat="server">
                        <ContentTemplate>
                            <div style="width: 100%; float: left;">
                                <table cellspacing="0" rules="all" border="1" class="tablePDI">
                                    <tr>
                                        <th>
                                            PLATFORM
                                        </th>
                                        <td>
                                            <asp:Label runat="server" ID="lblPlatform" ClientIDMode="Static" Text=""></asp:Label>
                                        </td>
                                        <th>
                                            BARCODE
                                        </th>
                                        <td>
                                            <div style="width: 250px; float: left;">
                                                <asp:TextBox runat="server" ID="txtBarcode" ClientIDMode="Static" Text="" MaxLength="21"
                                                    Width="200px" TabIndex="0" CssClass="form-control"></asp:TextBox></div>
                                            <div style="width: 100px; float: left;">
                                                <asp:Button runat="server" ID="btnBarcodeCheck" Text="CHECK" OnClick="btnBarcodeCheck_Click"
                                                    CssClass="btn btn-success" /></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            TYRE SIZE
                                        </th>
                                        <td>
                                            <asp:Label runat="server" ID="lblTyresize" ClientIDMode="Static" Text=""></asp:Label>
                                        </td>
                                        <th>
                                            BARCODE
                                        </th>
                                        <td>
                                            <asp:Label runat="server" ID="lblBarcode" ClientIDMode="Static" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            RIM
                                        </th>
                                        <td>
                                            <asp:Label runat="server" ID="lblRim" ClientIDMode="Static" Text=""></asp:Label>
                                        </td>
                                        <th>
                                            STENCIL
                                        </th>
                                        <td>
                                            <asp:Label runat="server" ID="lblStencil" ClientIDMode="Static" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            TYPE
                                        </th>
                                        <td>
                                            <asp:Label runat="server" ID="lblType" ClientIDMode="Static" Text=""></asp:Label>
                                        </td>
                                        <th>
                                            CONCESSION TYPE
                                        </th>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlReType" ClientIDMode="Static" CssClass="form-control">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            BRAND
                                        </th>
                                        <td>
                                            <asp:Label runat="server" ID="lblBrand" ClientIDMode="Static" Text=""></asp:Label>
                                        </td>
                                        <th>
                                            RE-BRAND
                                        </th>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlReBrand" ClientIDMode="Static" CssClass="form-control">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            SIDEWALL
                                        </th>
                                        <td>
                                            <asp:Label runat="server" ID="lblSidewall" ClientIDMode="Static" Text=""></asp:Label>
                                        </td>
                                        <th>
                                            RE-SIDEWALL
                                        </th>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlReSidewall" ClientIDMode="Static" CssClass="form-control">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            GRADE
                                        </th>
                                        <td>
                                            <asp:Label runat="server" ID="lblGrade" ClientIDMode="Static" Text=""></asp:Label>
                                        </td>
                                        <th>
                                            LOCATION
                                        </th>
                                        <td>
                                            <asp:Label runat="server" ID="lblLocation" ClientIDMode="Static" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div style="width: 100%; float: left;">
                                <table cellspacing="0" rules="all" border="1" class="tbMas">
                                    <tr>
                                        <th>
                                            DETECTOR CHECK
                                        </th>
                                        <td>
                                            <asp:RadioButtonList runat="server" ID="rdbDetector" ClientIDMode="Static" Width="95px"
                                                RepeatColumns="2" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="YES" Value="YES"></asp:ListItem>
                                                <asp:ListItem Text="NO" Value="NO"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <th>
                                            ID GAUGE CHECK
                                        </th>
                                        <td>
                                            <asp:RadioButtonList runat="server" ID="rdbGauge" ClientIDMode="Static" Width="100px"
                                                RepeatColumns="2" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="YES" Value="YES"></asp:ListItem>
                                                <asp:ListItem Text="NO" Value="NO"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <th>
                                            HARDNESS TREAD
                                        </th>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtTread" ClientIDMode="Static" Text="" MaxLength="3"
                                                Width="60px" CssClass="form-control" onkeypress="return isNumberWithoutDecimal(event)"></asp:TextBox>
                                        </td>
                                        <th>
                                            HARDNESS BASE
                                        </th>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtBase" ClientIDMode="Static" Text="" MaxLength="3"
                                                Width="60px" CssClass="form-control" onkeypress="return isNumberWithoutDecimal(event)"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            BEAD TYPE
                                        </th>
                                        <td>
                                            <asp:RadioButtonList runat="server" ID="rdbBeadType" ClientIDMode="Static" Width="100px"
                                                RepeatColumns="2" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="YES" Value="YES"></asp:ListItem>
                                                <asp:ListItem Text="NO" Value="NO"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <th style="line-height: 15px;">
                                            BEST QUALITY TEST(ONLY FOR POB)
                                        </th>
                                        <td>
                                            <asp:RadioButtonList runat="server" ID="rdbBestQuality" ClientIDMode="Static" Width="130px"
                                                RepeatColumns="2" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="OK" Value="OK"></asp:ListItem>
                                                <asp:ListItem Text="NOT OK" Value="NOT OK"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <th>
                                        Physical Barcode Check
                                        </th>
                                        <td>
                                        <asp:RadioButtonList runat="server" ID="Rbdphysical" ClientIDMode="Static" Width="130px"
                                                RepeatColumns="2" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="YES" Value="YES"></asp:ListItem>
                                                <asp:ListItem Text="NO" Value="NO"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        
                                        </td>
                                        <th>
                                            REMARKS
                                        </th>
                                        <td colspan="3">
                                            <asp:TextBox runat="server" ID="txtremarks" ClientIDMode="Static" Text="" TextMode="MultiLine"
                                                Width="300px" CssClass="form-control" MaxLength="499"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="8">
                                            <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"
                                                Font-Size="15px"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <asp:Button ID="btnTriggerScan" runat="server" ClientIDMode="static" OnClick="btnTriggerScan_Click"
                                    Style="visibility: hidden;" />
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnTriggerScan" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr style="height: 45px; text-align: center;">
                <td style="text-align: center;">
                    <span class="btn btn-info" id="All" onclick="showpdilist(this.id)">SHOW BARCODE LIST</span>
                </td>
                <td>
                </td>
                <td>
                    <asp:Label ID="lblNoOfRecords" Text="" runat="server" ClientIDMode="static" Font-Bold="true"
                        Font-Italic="true" Font-Size="22px" ForeColor="#543fca"></asp:Label>
                </td>
                <td>
                    <span class="btn btn-danger" onclick="ctrlClearPage();">CLEAR</span>
                </td>
                <td style="text-align: center;">
                    <asp:Button runat="server" ID="btnSaveItem" ClientIDMode="Static" Text="SAVE ITEM"
                        CssClass="btn btn-success" OnClick="btnSaveItem_Click" OnClientClick="javascript:return CtrlScanItemsave();" />
                    <asp:Label ID="lblSucessMsg" Text="" runat="server" ClientIDMode="static" Font-Bold="true"
                        Font-Italic="true" Font-Size="15px" ForeColor="Green"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <asp:GridView runat="server" ID="gvPdiItemList" AutoGenerateColumns="false" Width="100%"
                        AlternatingRowStyle-BackColor="#f5f5f5">
                        <HeaderStyle BackColor="#FEFE8B" Font-Bold="true" Height="22px" />
                        <Columns>
                            <asp:BoundField HeaderText="PLATFORM" DataField="config" />
                            <asp:BoundField HeaderText="TYRE SIZE" DataField="tyresize" />
                            <asp:BoundField HeaderText="RIM" DataField="rimsize" />
                            <asp:BoundField HeaderText="TYPE" DataField="tyretype" />
                            <asp:BoundField HeaderText="BRAND" DataField="brand" />
                            <asp:BoundField HeaderText="SIDEWALL" DataField="sidewall" />
                            <asp:BoundField HeaderText="QTY" DataField="itemqty" ItemStyle-HorizontalAlign="Right" />
                            <asp:TemplateField HeaderText="PLATFORM" HeaderStyle-BackColor="Cyan">
                                <ItemTemplate>
                                    <%#Eval("sPlatform")%>
                                    <br />
                                    <%#Eval("sPlatform1")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TYPE" HeaderStyle-BackColor="Cyan">
                                <ItemTemplate>
                                    <%#Eval("sType")%>
                                    <br />
                                    <%#Eval("sType1")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="BRAND" HeaderStyle-BackColor="Cyan">
                                <ItemTemplate>
                                    <%#Eval("sBrand")%>
                                    <br />
                                    <%#Eval("sBrand1")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SIDEWALL" HeaderStyle-BackColor="Cyan">
                                <ItemTemplate>
                                    <%#Eval("sSidewall")%>
                                    <br />
                                    <%#Eval("sSidewall1")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnPID" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnPlant" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnPdiFor" ClientIDMode="Static" Value="" />
    <script src="Scripts/TinyboxScript.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#txtBarcode').focus();
            $('#txtBarcode').blur(function (e) { $("#btnTriggerScan").trigger("click"); });
            $('input:text').bind('keydown', function (e) { if (e.keyCode == 13) { e.preventDefault(); return false; } });
        });

        function CtrlScanItemsave() {
            var errMsg = ''; $('#lblErrMsg').html('');
            if ($('#txt_OrderRefNo').val().length == 0)
                errMsg += 'Enter work order no<br/>';
            if ($('#lblBarcode').html().length == 0)
                errMsg += 'scan barcode label</br>';
            if ($('input:radio[id*=rdbDetector_]:checked').length == 0)
                errMsg += "choose metal detector check<br/>";
            if ($('input:radio[id*=rdbGauge_]:checked').length == 0)
                errMsg += "choose id gauge check<br/>";
            if ($('#txtTread').val().length == 0)
                errMsg += 'enter tread hardness</br>';
            if ($('#txtBase').val().length == 0)
                errMsg += 'enter base hardness</br>';
            if (errMsg.length > 0) {
                $('#lblErrMsg').html(errMsg);
                return false;
            }
            else {
                return true;
                $('#txtBarcode').focus();
            }
        }

        function bind_errmsg(strErr) {
            $('#lblErrMsg').html(strErr);
        }

        function ctrlClearPage() {
            window.location.href = window.location.href;
        }

        function showpdilist(clicked_id) {
            if ($('#hdnPID').val() != '') {
                TINY.box.show({ iframe: 'expscanpdidatashow.aspx?pid=' + $('#hdnPID').val() + '&plant=' + clicked_id + '&mtype=scanned',
                    boxid: 'frameless', width: 1106, height: 600, fixed: false, maskid: 'bluemask', maskopacity: 40, closejs: function () { }
                })
            }
            else
                alert('NO RECORDS');
        }

        function gotoPreviewDiv(ctrlID) {
            $('#' + ctrlID).css({ 'display': 'block' });
            $("html, body").stop().animate({ scrollLeft: $('#' + ctrlID).offset().left, scrollTop: $('#' + ctrlID).offset().top }, 1200);
        }
    </script>
</asp:Content>
