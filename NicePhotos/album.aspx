<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="album.aspx.cs" Inherits="NicePhotos.album" %>
<%@ Register src="Controls/CaptchaControl.ascx" tagname="CaptchaControl" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<script type="text/javascript">
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div>
    <h3><asp:Label ID="lblAlbumName" runat="server" Text=""></asp:Label></h3>
    <div style="float:left;width:auto;clear:both;max-width:850px">
     <asp:ListView ID="lsvImages" runat="server">
        <LayoutTemplate>
                <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
        </LayoutTemplate>

        <ItemTemplate>
            <div class="left listBox" style=";width:320px;min-height:410px;margin:20px 20px 20px 20px;text-align:center;padding:10px 10px 10px 10px;overflow:hidden;">
                <div onclick="var win=window.open('UserContent/<%# Eval("Album") %>/<%# Eval("ImageId") %>.jpg', '_blank');win.focus();" style="cursor:pointer;border:1px solid #ddd;background-color:#efefef;width:300px;height:280px;margin-left:auto;margin-right:auto;">
                    <img src="UserContent/<%# Eval("Album") %>/<%# Eval("ImageId") %>.jpg" width="299px" height="279px" alt="<%# Eval("ImageName") %>" 
                    onerror="this.src='/Images/blank_image.png'" />
                    </div><br />
                    <h3><%# Eval("ImageName") %></h3>
                    <span>
                        <%# Eval("ImageDescruption") %>
                    </span>
                    <br />
                    <span>Price: <b><%# GetPresentableCost(Eval("Cost").ToString()) %></b></span>
                    <br />
                    <asp:Button runat="server" Text='<%# GetButtonNameByCost(Eval("Cost").ToString()) %>' 
                    CommandArgument='<%# Eval("ImageId") %>' CssClass="button" OnClick="buyPhoto_Click" />
                    <asp:ImageButton runat="server" CommandArgument='<%# Eval("ImageId") %>'
                    CssClass="button opequeButton" ImageUrl="~/Images/flag-icon.png" ImageAlign="Middle" OnClick="flagPhoto_Click" Visible="<%# !string.IsNullOrEmpty(Context.User.Identity.Name) %>"
                    style="margin:0 0 0 0;padding:5px 5px 5px 5px;float:right;background-color:#fefefe;border:1px solid #eee;" Width="20px" Height="20px" />

            </div>
        </ItemTemplate>
        </asp:ListView>
    </div>

    <div style="width:300px;float:right;">
        <div id="divAddPhoto" runat="server" style="overflow:hidden;">
            <h3>Add Photo...</h3>
            <div class="listBox" style="margin:0 0 0 0;text-align:center;overflow:auto;">
                <asp:FileUpload ID="fuImageUpload" runat="server" Width="250px" /><br />
                <span>
                    <asp:TextBox ID="txtPhotoTitle" runat="server" placeholder="Title" type="text" Width="180px"></asp:TextBox>
                </span><br />
                <span>
                    Photo Cost&nbsp;&nbsp;
                    <asp:TextBox ID="txtCredits" runat="server" placeholder=" .. in credits" step="0" type="number" Width="100px"></asp:TextBox>
                </span><br /> 
                <span>
                    Description<br />
                    <asp:TextBox ID="txtDesc" runat="server" placeholder="Describe the Image" 
                    step="0" type="number" Width="200px" Height="100px" TextMode="MultiLine"></asp:TextBox>
                </span><br />              
                
                <br />
                <div style="float:right;">
                    <asp:Button ID="btnUpload" runat="server" Text="Upload" 
                        CssClass="buttonAlternative" onclick="btnUpload_Click"  />
                </div>
            </div>
        </div>
        
        <br /><br />

        <div id="divFriendsList" runat="server">
            <h3>Friends who can see this...</h3>
            <div class="listBox" style="max-height:330px;overflow-x:hidden;overflow-y:scroll;margin:0 0 0 0;">
                <h3>
                    <asp:Label ID="lblFriendsError" runat="server" Text="ooh.. I wasn't able to grab your friends list :[" Visible="false"></asp:Label>
                </h3>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="cblFriendsToSee" EventName="SelectedIndexChanged" />
                </Triggers>
                <ContentTemplate>
                    <asp:CheckBoxList ID="cblFriendsToSee" runat="server" DataTextField="Friend1" 
                        DataTextFormatString="     {0}" DataValueField="Friend1" 
                        AutoPostBack="True" 
                        onselectedindexchanged="cblFriendsToSee_SelectedIndexChanged">
                        </asp:CheckBoxList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>

        <br /><br />

        <div id="divComments">
            <h3>Leave a Comment...</h3>
            <div class="listBox" style="margin:0 0 0 0;text-align:center;">
                <div style="border:0 0 0 0;border-bottom:1px dotted #999;clear:both;overflow:auto;">
                    <asp:TextBox ID="txtComment" runat="server" placeholder="Comment" 
                        TextMode="MultiLine"></asp:TextBox>
                    <br />
                    <asp:TextBox ID="txtCaptcha" runat="server" placeholder="Captcha Code"></asp:TextBox>
                    <br />
                    <uc1:CaptchaControl ID="CaptchaControl1" runat="server" />
                    <br />
                    <asp:Button ID="btnPostComment" runat="server" Text="Post" style="float:right;" 
                        CssClass="buttonAlternative" onclick="btnPostComment_Click" />
                </div>
                <div style="max-height:400px;overflow:auto;">
                    <asp:Repeater ID="rptComments" runat="server">
                        <HeaderTemplate>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div style="text-align:left;border:0 0 0 0;border-bottom:1px dotted #eee;overflow:auto;padding:1px 3px 3px 3px;margin:0px 0px 3px 0px;clear:both;">
                                <b><%# ConstructCommenter(DataBinder.Eval(Container.DataItem, "Username")) %></b><br />
                                <%# DataBinder.Eval(Container.DataItem, "Comment1")%>
                                <br />
                                <asp:ImageButton ID="btnFlag" Width="20px" Height="17px"
                                CausesValidation="false" style="float:right;" 
                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CommentId") %>'
                                runat="server" ImageUrl="~/Images/flag-icon.png" OnClick="flagClick" />
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
    </div>
</div>
</asp:Content>
