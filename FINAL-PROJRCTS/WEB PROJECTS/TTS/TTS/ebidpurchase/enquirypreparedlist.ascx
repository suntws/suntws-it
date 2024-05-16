<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="enquirypreparedlist.ascx.cs"
    Inherits="TTS.ebidpurchase.enquirypreparedlist" %>
<style type="text/css">
    #gvEnquiries
    {
        width: 1078px;
        border: 1px solid tan;
        font-weight: bold;
    }
    
    #gvEnquiries th
    {
        background-color: #fefe8b;
        text-align: center;
        color: Black;
    }
    
    #divContent
    {
        padding-top: 2px;
        padding-left: 2px;
    }
</style>
<script type="text/javascript">
    function redirect(id) {
        document.location = "ebid_purchase.aspx?vid=5&enqid=" + id;
    }

    $(document).ready(function () {
        $("#lblPageHead").text("enquiry prepared list");
    });
</script>
<div id="divContent">
    <asp:GridView ID="gvEnquiries" ClientIDMode="Static" runat="server" AutoGenerateColumns="false">
        <Columns>
            <asp:BoundField DataField="S_NO" HeaderText="SNo." />
            <asp:BoundField DataField="EnqNo" HeaderText="ENQUIRY NUMBER" />
            <asp:BoundField DataField="EnqPreparedDate" HeaderText="PREPARED DATE" />
            <asp:BoundField DataField="EnqUsername" HeaderText="PREPARED USER" />
            <asp:BoundField DataField="NumberOfSupplier" HeaderText="NO OF SUPPLIERS" ItemStyle-HorizontalAlign="Right" />
            <asp:BoundField DataField="ExpiredDate" HeaderText="EXPIRY DATE"  />
            <asp:TemplateField HeaderText="ACTION" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <input type="button" value="EDIT" class="btnEdit" name="btnEdit" onclick='javascript:redirect(<%#Eval("EnqId")%>);' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</div>
