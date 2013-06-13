<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="payment.aspx.cs" Inherits="NicePhotos.User.payment" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div style="width:30%;margin-left:auto;margin-right:auto;">
<h3>Let's buy some credits...</h3>
<div class="listBox" style="clear:both;text-align:center;overflow:auto;margin:0 0 0 0;">
    <h3>1 Credit = 1 EUR</h3><br />
    <asp:TextBox ID="txtCredits" type="number" Text="1" runat="server"></asp:TextBox><br /><br />
    <asp:Button ID="btnBuy" CssClass="button" style="float:right;" runat="server" Text="Pay with PayPal!" OnClick="btnBuy_Click"
        OnClientClick="this.value='Processing...';this.style.background='#66CC99';this.trigger('click');" />
</div>
</div>
<br /><br /><br /><br /><br /><br />
</asp:Content>
