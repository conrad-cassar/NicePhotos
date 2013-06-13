<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AlbumFaceControl.ascx.cs" Inherits="NicePhotos.Controls.AlbumFaceControl" %>

<style type="text/css" scoped>
    .outbox
    {
        width:200px;
        height:137px;
        position:relative;
        padding:0px;
        margin:10px;
        clear:both;
        cursor:pointer;
    }
    .outbox img
    {
        min-width:100%;
        min-height:100%;
        max-width:150%;
    }
    .image
    {
        width:100px;
        height:100px;
        overflow:hidden;
        padding:0px;
        border:none;
        margin:0px;
        position:absolute;        
        border:4px solid #fff;
        outline:1px solid #999;
    }
    .one
    {
        top:5px;
        left:35px;
        z-index:3;
    }
    .two
    {
        top:10px;
        left:45px;
        z-index:2;
    }
    .three
    {
        top:15px;
        left:55px;
        z-index:1;
    }
    .imbDelete
    {
        width:10px;
        height:10px;
        top:10px;
        right:10px;
        position:absolute;
        z-index:4;
        display:none;
    }
    .outbox:hover .lblTitle
    {
        text-shadow: #990033 0.1em 0.1em 0.9em;
        color:#990033;
    }
    .album_title
    {
        height:25px;
        width:100%;
        /*background-color:#999;
        background-color:rgba(9,9,9,0.4);*/
        z-index:4;
        left:0px;
        bottom:0px;
        position:absolute;
        /*-webkit-border-bottom-right-radius: 5px;
        -webkit-border-bottom-left-radius: 5px;
        -moz-border-radius-bottomright: 5px;
        -moz-border-radius-bottomleft: 5px;
        border-bottom-right-radius: 5px;
        border-bottom-left-radius: 5px;*/
        padding-left:5px;
        overflow:hidden;
        vertical-align:middle;
        text-align:center;
    }
    .lblTitle
    {
        color:#0f0f0f;
        font-weight:800;
        text-shadow: black 0.1em 0.1em 0.9em;
        font-size:1.3em;
    }
</style>
<script type="text/javascript" src="../Scripts/jquery-1.9.1.min.js"></script>


<div class="outbox<%= AlbumId %> outbox" onclick="javascript:location='http://localhost:2381/album.aspx?p=<%= Common.Utilities.EncryptTextForParameters("a="+AlbumId) %>'">
    <div class="image one<%= AlbumId %> one">
        <asp:Image ID="imgOne" runat="server" ImageUrl="~/Images/blank_image.png" onerror="this.src='/Images/blank_image.png'" />
    </div>
    <div class="image two<%= AlbumId %> two">
        <asp:Image ID="imgTwo" runat="server" ImageUrl="~/Images/blank_image.png" onerror="this.src='/Images/blank_image.png'" />
    </div>
    <div class="image three<%= AlbumId %> three">
        <asp:Image ID="imgThree" runat="server" ImageUrl="~/Images/blank_image.png" onerror="this.src='/Images/blank_image.png'" />
    </div>
    <span class="imbDelete<%= AlbumId %> imbDelete">
        <asp:ImageButton ID="imbDelete" runat="server" 
            ImageUrl="~/Images/delete_icon.png" onclick="imbDelete_Click" />
    </span>
    <div class="album_title">
        <asp:Label ID="lblTitle" CssClass="lblTitle" runat="server" Text="untitled"></asp:Label>
    </div>
</div>

<script type="text/javascript">
    $(document).ready(function () {
        $('.outbox<%= AlbumId %>').hover(
        function () {
            $('.one<%= AlbumId %>').animate({
                left: '5px'
            });
            $('.two<%= AlbumId %>').animate({
                left: '43px',
                top: '20px'
            });
            $('.three<%= AlbumId %>').animate({
                left: '73px',
                top: '10px'
            }, 'slow');
            $('.imbDelete<%= AlbumId %>').show();
        }, function () {
            $('.one<%= AlbumId %>').animate({
                left: '35px'
            });
            $('.two<%= AlbumId %>').animate({
                left: '45px',
                top: '10px'
            });
            $('.three<%= AlbumId %>').animate({
                left: '55px',
                top: '15px'
            }, 'fast');
            $('.imbDelete<%= AlbumId %>').hide();
        });
    });
</script>