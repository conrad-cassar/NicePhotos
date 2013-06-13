<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="imagereview.aspx.cs" Inherits="NicePhotos.Admin._imagereview" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="clear:both;width:60%;margin-left:auto;margin-right:auto;">
        <h3>Review Reported Images</h3>
        <asp:ListView ID="lsvReportedImages" runat="server">
        <LayoutTemplate>
            <div id="albumListContainer">
                <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
            </div>
        </LayoutTemplate>
        <ItemTemplate>
            <div style="border:none;border-bottom:1px dotted #bbb;padding:4px 4px 4px 4px;text-align:center;clear:both;">
                <img style="cursor:pointer;"
                onclick="var win=window.open('../UserContent/<%# Eval("Album") %>/<%# Eval("ImageId") %>.jpg', '_blank');win.focus();"
                width="180px" height="180px" src="../UserContent/<%# Eval("Album") %>/thumbs/<%# Eval("ImageId") %>.jpg" />
                <div style="float:right;text-align:right;">
                    <asp:Button runat="server" CommandArgument='<%# Eval("ImageId") %>' OnClick="Disapprove_Click"
                        Text='Set as Appropriate' CssClass="button" style="margin:0 0 4px 0;padding:3px 6px 3px 6px;" /><br />
                    <asp:Button runat="server" CommandArgument='<%# Eval("ImageId") %>' OnClick="Approve_Click"
                        Text='Delete Image' CssClass="button" style="margin:0 0 0 0;padding:3px 6px 3px 6px;" />
                </div>
            </div>
        </ItemTemplate>
        </asp:ListView>
    </div>
</asp:Content>
