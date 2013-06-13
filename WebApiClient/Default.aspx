<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApiClient.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="margin-top:20px;clear:both;padding:10px 10px 10px 10px;">
        <h1>Web API Client</h1>
        <div style="float:left;width:49%;">
            <h4>All Albums</h4>
            <asp:GridView ID="grvAll" runat="server"></asp:GridView>
        </div>

        <div style="float:left;width:48%;padding-left:15px;">
            <h4>User Availability</h4>
            Available to <asp:TextBox ID="txtUsername" placeholder="Username" runat="server"></asp:TextBox>
            <asp:Button ID="btnFilter" runat="server" Text="GO" OnClick="btnFilter_Click" />
            <asp:GridView ID="grvFiltered" runat="server"></asp:GridView>
        </div>

    </div>
    </form>
</body>
</html>
