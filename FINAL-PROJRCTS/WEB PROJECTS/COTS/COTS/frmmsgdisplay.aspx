<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="frmmsgdisplay.aspx.cs" Inherits="COTS.frmmsgdisplay" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div align="center" class="pageTitleHead">
        MESSAGE
    </div>
    <div class="contPage">
        <div style="padding-top: 30px; font-size: 25px;">
            <div style="text-align: center; display: none;" id="divpricesheetmsg" runat="server">
                <span style="color: #ff0000;">Sorry! Price sheet list not available to your account.</span><br />
                <br />
                <span>Please contact SUN TYRE & WHEEL SYSTEMS</span>
            </div>
            <div style="text-align: center; display: none;" id="divordercompletemsg" runat="server">
                <span style="color: #08AC1B;">
                    <asp:Label runat="server" ID="lblOrderNo" Text=""> </asp:Label>
                    - Order successfully sent to SUN TYRE & WHEEL SYSTEMS</span><br />
            </div>
            <div style="text-align: center; display: none;" id="divConfirmedMsg" runat="server">
                <span style="color: #08AC1B;">THANK YOU FOR YOUR PROFORMA CONFIRAMTION.</span><br />
            </div>
            <div style="text-align: center; display: none;" id="divClaimRegister" runat="server">
                <span style="color: #08AC1B;">Complaint registered successfully sent to SUN TYRE & WHEEL
                    SYSTEMS</span><br />
                <asp:Label runat="server" ID="lblComplaintNo" Text="" Font-Bold="true"> </asp:Label>
            </div>
        </div>
    </div>
</asp:Content>
