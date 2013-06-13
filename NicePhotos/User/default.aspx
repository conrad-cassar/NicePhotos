<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="NicePhotos.User._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ul class="listBoxContainer">
        <li>
            <h3>Credits</h3>
            <div class="listBox">
                <span style="font-size:1.5em">Current Balance: <b>
                    <asp:Label ID="lblCurrentBalance" runat="server" Text="??"></asp:Label> credits</b></span>
                <div style="text-align:right;">
                    <a href="payment.aspx" style="text-decoration:none;" class="buttonAlternative">Buy More!</a>
                </div>
            </div>
        </li>
        
        <li>
            <h3>My Staff</h3>
            <div class="listBox" style="overflow-x:hidden;overflow-y:scroll;max-height:350px;">
                <h3>Albums</h3>
                    <asp:ListView ID="lsvAlbums" runat="server">
                        <LayoutTemplate>
                            <div id="albumListContainer">
                                <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
                            </div>
                        </LayoutTemplate>

                        <ItemTemplate>
                            <a href="../album.aspx?p=<%# Common.Utilities.EncryptTextForParameters("a=" + Eval("AlbumId").ToString()) %>"><%# Eval("AlbumName") %></a>
                            <br />
                        </ItemTemplate>
                    </asp:ListView>
                    <br />
                    <hr />
                    <br />
                    <h3>Bought Images</h3>
                    <asp:ListView ID="lsvBought" runat="server">
                        <LayoutTemplate>
                            <div id="albumListContainer">
                                <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
                            </div>
                        </LayoutTemplate>

                        <ItemTemplate>
                            <div style="border:none;border-bottom:1px dotted #bbb;padding:4px 4px 4px 4px;text-align:center;clear:both;">
                                <img width="150" height="150" src="../UserContent/<%# Eval("Album") %>/thumbs/<%# Eval("ImageId") %>.jpg" />
                                <asp:Button runat="server" OnClick="Download_Click" CommandArgument='<%# Eval("ImageId") %>'
                                 Text='Download' CssClass="buttonAlternative" style="float:right;margin:0 0 0 0;" />
                            </div>
                        </ItemTemplate>
                    </asp:ListView>
                <div id="divTotalBought" runat="server" style="padding:8px 8px 8px 8px;background-color:#ededed;text-align:center;">
                    All these costed you <b><asp:Label ID="lblTotalSpent" runat="server" Text="??"></asp:Label> credits</b>
                </div>
            </div>
        </li>
            
        <li>
            <h3>Earned Credits</h3>
            <div class="listBox" style="overflow-x:hidden;overflow-y:scroll;max-height:350px;">
                <asp:ListView ID="lsvEarnings" runat="server">
                <LayoutTemplate>
                    
                        <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
                </LayoutTemplate>
                <ItemTemplate>
                    <div style="border:0 0 0 0;border-bottom:1px dotted #999;">
                        <img src='../<%# Eval("ImageUrl") %>' width="50" height="50" />
                        Earned <b><%# Eval("EarnedCredits")%> credits</b>
                    </div>
                </ItemTemplate>
            </asp:ListView>
            </div>
        </li>

        <li>
            <h3>Update Personal Details</h3>
            <div class="listBox">
                <div style="width:150px;float:left;">Username </div><b><asp:Label ID="lblUsername" runat="server" Text="guest"></asp:Label></b><br />
                <div style="width:150px;float:left;">Email </div><asp:Label ID="lblEmail" runat="server" Text="guest"></asp:Label><br />
                <div style="width:150px;float:left;">Name </div><asp:TextBox ID="txtName" runat="server"></asp:TextBox><br />
                <div style="width:150px;float:left;">Surname </div><asp:TextBox ID="txtSurname" runat="server"></asp:TextBox><br />
                <div style="width:150px;float:left;">About Me </div><asp:TextBox ID="txtMe" TextMode="MultiLine" runat="server"></asp:TextBox><br />
                <div style="text-align:right;"><asp:Button ID="btnSaveDetails" runat="server" 
                        Text="Save" CssClass="buttonAlternative" onclick="btnSaveDetails_Click" /></div>
            </div>
        </li>

        <li>
            <h3>Change Password</h3>
            <div class="listBox">
                <div style="width:200px;float:left;">Old Password </div>
                <asp:TextBox ID="txtOldPass" runat="server" TextMode="Password"></asp:TextBox><br />
                <div style="width:200px;float:left;">New Password </div>
                <asp:TextBox ID="txtNewPass" runat="server" TextMode="Password"></asp:TextBox><br />
                <div style="width:200px;float:left;">Confirm Password </div>
                <asp:TextBox ID="txtConfPass" runat="server" TextMode="Password"></asp:TextBox><br />
                <div style="text-align:right;"><asp:Button ID="btnChangePass" runat="server" 
                        Text="Change Password" CssClass="buttonAlternative" 
                        onclick="btnChangePass_Click" /></div>
            </div>
        </li>
    </ul>
</asp:Content>
