<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="containerdetail.ascx.cs"
    Inherits="TTS.cargomanagement.containerdetail" %>
<style type="text/css">
    #txtContainerType, #txtContainerQty
    {
        width: 95%;
        padding-top: 2px;
        margin-top: 2px;
    }
    #ddlOrder, #ddlCustomer
    {
        width: 95%;
        padding-top: 2px;
        margin-top: 2px;
    }
    .hide
    {
        display:none;
    }
</style>
<div style="width: 1080px;">
    <div style="margin: 10px;">
        <div style="margin-bottom:10px;">
            <asp:Label runat="server" ID="lblErrMsg" ClientIDMode="Static" Text="" ForeColor="Red"></asp:Label>
        </div>
        <div style="width: 100%">
            <div style="width: 40%; float: left">
                <label style="font-weight: bold;">
                    CUSTOMER
                </label>
                <br />
                <asp:DropDownList ID="ddlCustomer" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged"
                    AutoPostBack="true" runat="server" ClientIDMode="Static">
                </asp:DropDownList>
            </div>
            <div style="width: 20%; float: left">
                <label style="font-weight: bold;">
                    ORDER ID
                </label>
                <br />
                <asp:DropDownList ID="ddlOrder" runat="server" ClientIDMode="Static">
                </asp:DropDownList>
            </div>
            <div style="width: 18%; float: left">
                <label style="font-weight: bold;">
                    CONTAINER TYPE
                </label>
                <br />
                <asp:TextBox ID="txtContainerType" runat="server" list="dlContainerType" ClientIDMode="Static" />
                <datalist id="dlContainerType">
                        <asp:Literal ID="ltrlContainerType" runat="server" ClientIDMode="Static"></asp:Literal>
                    </datalist>
            </div>
            <div style="width: 10%; float: left">
                <label style="font-weight: bold;">
                    CONTAINER QTY
                </label>
                <br />
                <asp:TextBox ID="txtContainerQty" runat="server" ClientIDMode="Static" onkeypress="return isNumberKey(event)" />
            </div>
            <div style="width:10%;float:left;margin-left: 10px;margin-top: 15px;">
                <asp:Button ID="btnAddDimension" runat="server" ClientIDMode="Static" OnClick="btnAddDimension_Click" Text="Add Dimension" style="width:95%" />
            </div>
            <div style="clear: both">
            </div>
        </div>
        <div id="divDimensions" style="width:100%;margin-top:20px; text-align:center;line-height:30px;" class="hide">
            <table align="center" style="border:1px solid rgba(0,0,0,0.32)"  rules="all">
                <thead>
                    <tr>
                        <th>
                            PROPERTIES    
                        </th>
                        <th>
                            VALUES
                        </th>
                    </tr>
                </thead>
                <tbody style="text-align:left;text-indent:10px;">
                    <tr>
                        <th style="color:#d85314">
                            CONTAINER HEIGHT(mm)
                        </th>
                        <td>
                            <asp:TextBox ID="txtHeight" runat="server" ClientIDMode="Static" style="width:100px;" onkeypress="return isNumberKey(event)" ></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th style="color:#d85314">
                            CONTAINER WIDTH(mm)
                        </th>
                        <td>
                            <asp:TextBox ID="txtwidth" runat="server" ClientIDMode="Static" style="width:100px;" onkeypress="return isNumberKey(event)" ></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th style="color:#d85314">
                            CONTAINER LENGTH(mm)
                        </th>
                        <td>
                            <asp:TextBox ID="txtLength" runat="server" ClientIDMode="Static" style="width:100px;" onkeypress="return isNumberKey(event)" ></asp:TextBox>
                        </td>
                    </tr>
                    <%--<tr>
                        <th style="color:#d85314">
                            CONTAINER LOAD CAPACITY
                        </th>
                        <td>
                            <asp:TextBox ID="txtLoadCapacity" runat="server" ClientIDMode="Static" style="width:100px;" onkeypress="return isNumberKey(event)" ></asp:TextBox>
                        </td>
                    </tr>--%>
                </tbody>
            </table>
            <asp:Button ID="btnSave" runat="server" ClientIDMode="Static" Text="SAVE" OnClick="btnSave_Click" style="margin-top:10px;" />
        </div>
    </div>
</div>
<script type="text/javascript">
    $("#lblPageHead").text("container details");

    $("#btnAddDimension").click(function (event) {
        $("#lblErrMsg").text("");
        if ($("#ddlCustomer option:selected").index() < 1) { $("#lblErrMsg").text("select customer"); event.preventDefault(); return false; }
        if ($("#ddlOrder option:selected").index() < 1) { $("#lblErrMsg").text("select order ID"); event.preventDefault(); return false; }
        if ($("#txtContainerType").val() == "") { $("#lblErrMsg").text("select/enter container type"); event.preventDefault(); return false; }
        if ($("#txtContainerQty").val() == "") { $("#lblErrMsg").text("enter container quantity"); event.preventDefault(); return false; }
    });

    function isNumberKey(evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode;
        if (charCode != 46 && charCode > 31
            && (charCode < 48 || charCode > 57))
            return false;

        return true;
    }

    function showDiv() {
        $("#divDimensions").removeClass("hide");
    }

    $("#btnSave").click(function (event) {
        $("#lblErrMsg").text(""); 
        $("#divDimensions input[type='text']").each(function () {
            if ($(this).val() == "") { $(this).focus(); event.preventDefaul(); $("#lblErrMsg").text("Enter value"); return false; }
        });
    });
</script>
