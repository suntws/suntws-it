<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="requirementAnalysis.aspx.cs" Inherits="TTS.requirementAnalysis" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/datemonthyear.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .hide
        {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div align="center" class="pageTitleHead">
        REQUIREMENT ANALYSIS
    </div>
    <div>
        <asp:Label runat="server" ID="lblErrMsgcontent" ClientIDMode="Static" Text="" Font-Bold="true"
            Font-Size="20px" ForeColor="Red"></asp:Label>
    </div>
    <div id="displaycontent" class="contPage">
        <div>
            <div style="width: 1080px; padding-top: 10px;">
                <table style="border: 1px solid black; margin: 0px auto;" id="tblContent">
                    <tbody>
                        <tr>
                            <td>
                                <label>
                                    TITLE</label>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox runat="server" CssClass="editMode" ClientIDMode="Static" ID="txtTitle"></asp:TextBox>
                                <asp:Label runat="server" CssClass="viewMode" ClientIDMode="Static" ID="lblTitle"></asp:Label>
                                <asp:HiddenField runat="server" ClientIDMode="Static" ID="hdnId" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    MODULE NAME</label>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox runat="server" CssClass="editMode" ClientIDMode="Static" ID="txtModuleName"></asp:TextBox>
                                <asp:Label runat="server" CssClass="viewMode" ClientIDMode="Static" ID="lblModuleName"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    REQUIREMENT</label>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <textarea class="editMode" id="areaRequirement" style="width: 320px; height: 70px;
                                    overflow: auto; resize: none;"> </textarea>
                                <div class="viewMode" id="divRequirement" style="border: 1px solid rgba(0,0,0, 0.40);
                                    width: 320px; height: 70px; overflow: auto;">
                                </div>
                                <asp:HiddenField runat="server" ClientIDMode="static" ID="hdnRequirement" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    QUERY DATE</label>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox runat="server" CssClass="editMode" ClientIDMode="Static" ID="txtQueryDate"></asp:TextBox>
                                <asp:Label runat="server" CssClass="viewMode" ClientIDMode="Static" ID="lblQueryDate"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    DUE DATE</label>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox runat="server" CssClass="editMode" ClientIDMode="Static" ID="txtDueDate"></asp:TextBox>
                                <asp:Label runat="server" CssClass="viewMode" ClientIDMode="Static" ID="lblDueDate"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    PERCENTAGE</label>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox runat="server" CssClass="editMode" ClientIDMode="Static" ID="txtPercentage"></asp:TextBox>
                                <asp:Label runat="server" CssClass="viewMode" ClientIDMode="Static" ID="lblPercentage"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    PENDING TASK</label>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox runat="server" CssClass="editMode" ClientIDMode="Static" ID="txtPendingTask"></asp:TextBox>
                                <asp:Label runat="server" CssClass="viewMode" ClientIDMode="Static" ID="lblPendingTask"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    COMMUNICATION TYPE</label>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox runat="server" CssClass="editMode" ClientIDMode="Static" ID="txtCommunicationType"></asp:TextBox>
                                <asp:Label runat="server" CssClass="viewMode" ClientIDMode="Static" ID="lblCommunicationType"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    COMMUNICATED PERSON</label>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox runat="server" CssClass="editMode" ClientIDMode="Static" ID="txtCommunicatePerson"></asp:TextBox>
                                <asp:Label runat="server" CssClass="viewMode" ClientIDMode="Static" ID="lblCommunicatePerson"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:CheckBox runat="server" ID="chkIsCompleted" ClientIDMode="Static" Text="IS COMPLETED"
                                    Style="margin-left: 165px;" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" style="text-align: center;">
                                <div>
                                    <asp:Button runat="server" ID="btnAdd" ClientIDMode="Static" Text="ADD" OnClick="btnAdd_Click" />
                                    <asp:Button runat="server" ID="btnEdit" ClientIDMode="Static" Text="EDIT" />
                                    <asp:Button runat="server" ID="btnSave" ClientIDMode="Static" Text="SAVE" OnClick="btnAdd_Click" />
                                    <asp:Button runat="server" ID="btnCancel" ClientIDMode="Static" Text="CANCEL" OnClick="btnCancel_Click" />
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div style="width: 1080px;">
                <div style="padding: 5px; max-height: 150px; overflow: auto;">
                    <asp:GridView runat="server" ID="gvViewRequirements" ClientIDMode="Static" AutoGenerateColumns="false"
                        Font-Size="12px" AlternatingRowStyle-BackColor="#C2D69B" HeaderStyle-BackColor="green"
                        HeaderStyle-Height="22px" HeaderStyle-ForeColor="white" Font-Names="Arial" AllowPaging="false"
                        Width="100%">
                        <Columns>
                            <asp:TemplateField HeaderText="S_No" ItemStyle-Width="25">
                                <ItemTemplate>
                                    <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TITLE">
                                <ItemTemplate>
                                    <asp:Label ID="lblTitle" runat="server" Text='<%# Eval("Title")%>'></asp:Label>
                                    <asp:HiddenField ID="hdn_gv_Id" runat="server" ClientIDMode="Static" Value='<%# Eval("Id")%>' />
                                    <asp:HiddenField ID="hdn_gv_requirement" runat="server" ClientIDMode="Static" Value='<%# Eval("Requirement")%>' />
                                    <asp:HiddenField ID="hdn_gv_PendingTask" runat="server" ClientIDMode="Static" Value='<%# Eval("PendingTask")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Modulename" HeaderText="MODULE" />
                            <asp:BoundField DataField="QueryDate" HeaderText="QUERY DATE" />
                            <asp:BoundField DataField="CommunicationType" HeaderText="COMM. VIA" />
                            <asp:BoundField DataField="CommunicatedPerson" HeaderText="COMM. WITH" />
                            <asp:BoundField DataField="DueDate" HeaderText="DUE DATE" />
                            <asp:BoundField DataField="Percentage" HeaderText="COMPLETED %" />
                            <asp:BoundField DataField="IsCompleted" HeaderText="IS COMPLETED" />
                            <asp:TemplateField HeaderText="ACTION">
                                <ItemTemplate>
                                    <asp:Button ID="btnView" runat="server" OnClientClick="return getRecord(this)" Text="VIEW" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
    <script src="Scripts/datemonthyear.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#txtQueryDate").datepicker({
                minDate: "-10D", maxDate: "+0D"
            }).keydown(function (e) {
                e.keyWhich = 0; e.keyCode = 0; e.preventDefault();
            });

            $("#txtDueDate").datepicker({
                minDate: "+0D"
            }).keydown(function (e) {
                e.keyWhich = 0; e.keyCode = 0; e.preventDefault();
            });

            $("#areaRequirement").text($("#hdnRequirement").val());
            $("#divRequirement").text($("#hdnRequirement").val());

            $("#btnCancel").removeClass("hide");
            $("#btnSave").addClass("hide");
            $("#btnEdit").addClass("hide");

            $(".editMode").removeClass("hide")
            $(".viewMode").addClass("hide");
        });

        $("#btnEdit").click(function (event) {
            $("#btnCancel").removeClass("hide")
            $("#btnSave").removeClass("hide")
            $("#btnAdd").addClass("hide");
            $("#btnEdit").addClass("hide");
            $(".editMode").removeClass("hide")
            $(".viewMode").addClass("hide");
            event.preventDefault();
            return false;
        });


        $("#areaRequirement").change(function () {
            $("#hdnRequirement").val($("#areaRequirement").val());
        });

        function getRecord(ele) {
            $("#txtTitle").val($(ele).closest("tr").find("td").eq(1).text().trim());
            $("#hdnId").val($(ele).closest("tr").find("#hdn_gv_Id").val());
            $("#txtModuleName").val($(ele).closest("tr").find("td").eq(2).text().trim());
            $("#areaRequirement").val($(ele).closest("tr").find("#hdn_gv_requirement").val());
            $("#hdnRequirement").val($(ele).closest("tr").find("#hdn_gv_requirement").val());
            $("#txtQueryDate").val($(ele).closest("tr").find("td").eq(3).text().trim());
            $("#txtDueDate").val($(ele).closest("tr").find("td").eq(6).text().trim());
            $("#txtPercentage").val($(ele).closest("tr").find("td").eq(7).text().trim());
            $("#txtPendingTask").val($(ele).closest("tr").find("#hdn_gv_PendingTask").val());
            $("#txtCommunicationType").val($(ele).closest("tr").find("td").eq(4).text().trim());
            $("#txtCommunicatePerson").val($(ele).closest("tr").find("td").eq(5).text().trim());
            if ($(ele).closest("tr").find("td").eq(8).text().trim() == 1) $("#chkIsCompleted").attr("checked", "checked");
            else $("#chkIsCompleted").removeAttr("checked");
            $(".editMode").addClass("hide")
            $(".viewMode").removeClass("hide");
            $("#lblTitle").text($(ele).closest("tr").find("td").eq(1).text().trim());
            $("#lblModuleName").text($(ele).closest("tr").find("td").eq(2).text().trim());
            $("#divRequirement").html($(ele).closest("tr").find("#hdn_gv_requirement").val());
            $("#lblQueryDate").text($(ele).closest("tr").find("td").eq(3).text().trim());
            $("#lblDueDate").text($(ele).closest("tr").find("td").eq(6).text().trim());
            $("#lblPercentage").text($(ele).closest("tr").find("td").eq(7).text().trim());
            $("#lblPendingTask").text($(ele).closest("tr").find("#hdn_gv_PendingTask").val());
            $("#lblCommunicationType").text($(ele).closest("tr").find("td").eq(4).text().trim());
            $("#lblCommunicatePerson").text($(ele).closest("tr").find("td").eq(5).text().trim());
            $("#btnAdd").addClass("hide");
            $("#btnCancel").removeClass("hide");
            $("#btnSave").addClass("hide");
            $("#btnEdit").removeClass("hide");
            console.log(ele);
            return false;
        }
    </script>
</asp:Content>
