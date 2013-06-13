<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="albums.aspx.cs" EnableEventValidation="false" Inherits="NicePhotos.albums" %>
<%@ Register src="Controls/AlbumFaceControl.ascx" tagname="AlbumFaceControl" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
    $(document).ready(function () {
        $('.txtAlbumName').focus(function () {
            $('.addAlbumDetails').slideDown('fast', function () {
            });
        });
        $('.createbutton').click(function () {
            $('.addAlbumDetails').slideUp('fast', function () {
            });
        });
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="addAlbum" class="listBox" runat="server" style="width:30%;margin-left:auto;margin-right:auto;clear:both;overflow:auto;">
    <h3>Create A New Album...</h3>
    <!--<span class="buttonAlternative" style="cursor:pointer;border-radius:0px;margin-left:auto;margin-right:auto;"><asp:FileUpload id="fuFileUpload" runat="server" />&nbsp;&nbsp;
    <asp:Button ID="btnUpload" runat="server" Text="Upload" CssClass="button" 
        style="padding:0.7em 1em 0.7em 1em;margin:1px;" /></span>-->
    <div style="text-align:center;" id="createAlbumSection" runat="server">
        <asp:TextBox ID="txtAlbumName" placeholder="Album Name" CssClass="logininput txtAlbumName" type="text" runat="server"></asp:TextBox>
        &nbsp;&nbsp;

        <div class="addAlbumDetails" style="display:none;">
            <asp:DropDownList ID="ddlAvailabilityType" runat="server" CssClass="logininput" 
                DataTextField="TypeName" DataValueField="TypeId" 
                AppendDataBoundItems="True">
                <asp:ListItem Selected="True">~ Choose Availability ~</asp:ListItem>
            </asp:DropDownList>
            <br />
            <asp:Button ID="btnCreateAlbum" runat="server" CssClass="button creatbutton" Text="Create" 
                onclick="btnCreateAlbum_Click" />&nbsp;&nbsp;
            <a href="javascript:$('.addAlbumDetails').slideUp('fast', function () {});">Cancel</a>
        </div>
    </div>
</div>

<div style="clear:both;overflow:auto;">
    <asp:ListView ID="lsvAlbums" runat="server">
    <LayoutTemplate>
        <div id="albumListContainer">
            <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
        </div>
    </LayoutTemplate>

    <ItemTemplate>
        <div class="left">
            <uc1:AlbumFaceControl ID="AlbumFaceControl1" runat="server" AlbumId='<%# Eval("AlbumId") %>' />
        </div>
    </ItemTemplate>
    </asp:ListView>
</div>
</asp:Content>
