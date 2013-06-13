<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="NicePhotos.Admin._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div style="clear:both;">
    <div style="width:48%;float:left;border-right:1px dotted #aaa;border-bottom:1px dotted #aaa;padding-left:1%;">
        <h3>Menu Items Allocation</h3><br />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="cblMenuItem" EventName="SelectedIndexChanged" />
                </Triggers>
                <ContentTemplate>
                    <asp:DropDownList ID="ddlRoles" runat="server" DataTextField="RoleName" 
                        DataTextFormatString="{0}" DataValueField="RoleId" 
                        onselectedindexchanged="ddlRoles_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                        <br />

                    <asp:CheckBoxList ID="cblMenuItem" runat="server" DataTextField="ItemName" 
                        DataTextFormatString="{0}" DataValueField="ItemId" 
                        AutoPostBack="True" 
                        onselectedindexchanged="cblMenuItem_SelectedIndexChanged">
                        </asp:CheckBoxList>
                    </ContentTemplate>
                </asp:UpdatePanel>
        <br />
    </div>
    <div style="width:48%;float:right;border-left:1px dotted #aaa;border-bottom:1px dotted #aaa;padding-left:1%;">
        <h3>Roles Allocation</h3><br />
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>

                <fieldset style="text-align:center;">
                <legend>Load User</legend>
                    <asp:TextBox ID="txtUserToMod" runat="server" placeholder="Username" ></asp:TextBox>
                    <asp:Button ID="btnDoFind" runat="server" Text="Go" 
                        CssClass="buttonAlternative" onclick="btnDoFind_Click" />
                  </fieldset>
                  <br />
                <b style="font-size:1.5em;"><asp:Label ID="lblUsername" runat="server" Text=""></asp:Label></b>
                <br />

                <asp:CheckBoxList ID="cblRoles" runat="server" DataTextField="RoleName" 
                    DataTextFormatString="{0}" DataValueField="RoleId" 
                    AutoPostBack="True" 
                    onselectedindexchanged="cblRoles_SelectedIndexChanged" >
                    </asp:CheckBoxList>

            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
    </div>
</div>
</asp:Content>
