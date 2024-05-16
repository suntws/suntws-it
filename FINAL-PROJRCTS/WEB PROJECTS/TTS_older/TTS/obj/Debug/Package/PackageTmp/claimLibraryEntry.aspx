<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="claimLibraryEntry.aspx.cs" Inherits="TTS.claimLibraryEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .tableReq
        {
            border-collapse: collapse;
            border-color: #000;
            width: 1080px;
            line-height: 20px;
            margin-top: 5px;
        }
        .tableReq th:first-child
        {
            background-color: #DEDEDE;
            text-align: left;
            padding-left: 10px;
            width: 240px;
            font-weight: bold;
        }
        .tableReq input[type="text"], textarea, select, file
        {
            background-color: #F7E9C3;
            border: 1px solid #000;
            margin-left: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div align="center" class="pageTitleHead">
        CLAIM LIBRARY FOR SOLID TYRES
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <table cellspacing="0" rules="all" border="1">
            <tr>
                <td>
                    <table cellspacing="0" rules="all" border="1" class="tableReq">
                        <tr>
                            <th>
                                COMPLAINT TYPE
                            </th>
                            <td>
                                <asp:DropDownList ID="ddlComplainttype" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="ddlComplainttype_SelectedIndexChanged"
                                    AutoPostBack="true" Width="600px">
                                </asp:DropDownList>
                                <asp:TextBox ID="txtComplaintType" runat="server" ClientIDMode="Static" Text="" Width="300px">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                APPEARANCE
                            </th>
                            <td>
                                <asp:TextBox ID="txtAppearance" runat="server" ClientIDMode="Static" Text="" TextMode="MultiLine"
                                    Width="800px" Height="50px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                SUSPECTED MANUFACTURING END
                            </th>
                            <td>
                                <asp:TextBox ID="txtManufacturingEnd" runat="server" ClientIDMode="Static" Text=""
                                    TextMode="MultiLine" Width="800px" Height="50px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                SUSPECTED CUSTOMER END
                            </th>
                            <td>
                                <asp:TextBox ID="txtCustomerEnd" runat="server" ClientIDMode="Static" Text="" TextMode="MultiLine"
                                    Width="800px" Height="50px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                ACTION
                            </th>
                            <td>
                                <asp:TextBox ID="txtAction" runat="server" ClientIDMode="Static" Text="" TextMode="MultiLine"
                                    Width="800px" Height="50px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                WARRANTY
                            </th>
                            <td>
                                <asp:TextBox ID="txtWarranty" runat="server" ClientIDMode="Static" Text="" TextMode="MultiLine"
                                    Width="800px" Height="50px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                UPLOAD IMAGE
                            </th>
                            <td>
                                <asp:FileUpload ID="Fupimage" runat="server" name="Fupimage[]" multiple="true" />:
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: center;">
                                <div style="width: 525px; float: left; text-align: right; padding-right: 20px;">
                                    <asp:Button ID="btnSave" ClientIDMode="Static" runat="server" Text="SAVE" CssClass="btnactive"
                                        OnClick="btnSave_Click" OnClientClick="javascript:return CtrlLibraryEntry();" /></div>
                                <div style="width: 525px; float: left; line-height: 15px; text-align: left;">
                                    <asp:Label ID="lblErrMsg" runat="server" ClientIDMode="Static" ForeColor="red" Text=""></asp:Label>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:DataList runat="server" ID="dtimglist" RepeatColumns="5" RepeatDirection="Horizontal"
                        RepeatLayout="Table" AlternatingItemStyle-BackColor="#A1C9D3" ItemStyle-BackColor="#C9DAC9"
                        ItemStyle-VerticalAlign="Top" Width="1078px">
                        <ItemTemplate>
                            <div id="temp" style="width: 200px; height: 200px; border: 1px solid #000; background-color: #F5F5F5;
                                margin: 5px;">
                                <asp:HiddenField ID="hdnurl" runat="server" ClientIDMode="Static" Value='<%# Eval("ImgUrl") %>' />
                                <div style="width: 200px; height: 200px;">
                                    <asp:Image runat="server" ID="thumbimg" ClientIDMode="Static" ImageUrl='<%# Eval("ImgUrl") %>'
                                        Width="200px" Height="200px" />
                                </div>
                                <div style="width: 100px; float: left; line-height: 15px; text-align: center;">
                                    <asp:LinkButton ID="lnkDelete" runat="server" ClientIDMode="Static" Text="DELETE"
                                        OnClick="lnkDelete_Click"></asp:LinkButton>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:DataList>
                </td>
            </tr>
            <tr>
                <td style="text-align: center;">
                    <asp:LinkButton ID="lnkPrevious" runat="server" ClientIDMode="Static" OnClick="lnkPrevious_Click">Previous</asp:LinkButton>
                    <asp:LinkButton ID="lnkNext" runat="server" ClientIDMode="Static" OnClick="lnkNext_Click">Next</asp:LinkButton>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField ID="hdnCount" runat="server" ClientIDMode="Static" Value='' />
    <asp:HiddenField ID="hdnID" runat="server" ClientIDMode="Static" Value='' />
    <script type="text/javascript">
        $(document).ready(function () {
            $('#ddlComplainttype').change(function () {
                if ($('#ddlComplainttype option:selected').text() == 'ADD NEW ENTRY')
                    $('#txtComplaintType').css({ 'display': 'block' });
                else
                    $('#txtComplaintType').css({ 'display': 'none' });
            });

        });
        function CtrlLibraryEntry() {
            var errMsg = ''; $('#lblErrMsg').html('');
            var dataListRef = document.getElementById('<%= dtimglist.ClientID %>');
            if (dataListRef == null) {
                var array = ["bmp", "gif", "png", "jpg", "jpeg", "tif"];
                var xyz = document.getElementById('<%= Fupimage.ClientID %>');
                if (xyz.files.length == 0)
                    errMsg += "Please upload any one failure images<br/>";
                else {
                    for (var i = 0; i < xyz.files.length; i++) {
                        var ext = xyz.files[i].name.substr(xyz.files[i].name.lastIndexOf('.') + 1).toLowerCase();
                        if (array.indexOf(ext) <= -1)
                            errMsg += "Please uipload only bmp, gif, png, jpg, tif, jpeg extension file<br/>";
                    }
                }
            }
            if ($('#ddlComplainttype option:selected').text() == 'Choose') errMsg += 'Choose complaint type<br/>';
            else if ($('#ddlComplainttype option:selected').text() == 'ADD NEW ENTRY' && $('#txtComplaintType').val().length == 0) errMsg += 'Enter complaint type<br/>';
            if ($('#txtAppearance').val().length == 0)
                errMsg += 'Enter Appearance<br/>';
            if ($('#txtManufacturingEnd').val().length == 0)
                errMsg += 'Enter manufacturing end<br/>';
            if ($('#txtCustomerEnd').val().length == 0)
                errMsg += 'Enter customer end<br/>';
            if ($('#txtAction').val().length == 0)
                errMsg += 'Enter action<br/>';
            if ($('#txtWarranty').val().length == 0)
                errMsg += 'Enter warranty<br/>';
            if (errMsg.length > 0) {
                $('#lblErrMsg').html(errMsg);
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
