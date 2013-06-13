<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="friends.aspx.cs" Inherits="NicePhotos.User.friends" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="listBoxContainer">
    <h3>Add Friend</h3>
    <div class="listBox" style="text-align:center;">
        <asp:TextBox ID="txtFriend" placeholder="Find by username" type="text" runat="server" CssClass="logininput" Width="80%" ></asp:TextBox>
        <div style="text-align:right;">
            <asp:Button ID="btnAddFriend" runat="server" Text="Add Friend" 
                CssClass="buttonAlternative" onclick="btnAddFriend_Click" />
        </div>
    </div>

    <h3>My Friends</h3>
    <div class="listBox">
        <asp:Repeater id="repFriends" runat="server">
            <SeparatorTemplate>
                <hr />
            </SeparatorTemplate>
            <headertemplate>
                <ul>
            </headertemplate>
            <itemtemplate>
                <li><%# Eval("Friend1") %></li>
            </itemtemplate>
            <footertemplate>
                </ul>
            </footertemplate>
        </asp:Repeater>
    </div>
</div>
</asp:Content>
