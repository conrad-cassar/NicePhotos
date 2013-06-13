<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Login.ascx.cs" Inherits="NicePhotos.Controls.Login" %>
<%@ Register src="CaptchaControl.ascx" tagname="CaptchaControl" tagprefix="uc1" %>
<div style="width:800px;height:auto;margin-left:auto;margin-right:auto;">
    <div style="float:left;width:49%;height:99%;margin:0 0 0 0;text-align:center;">
        <h3>
            <asp:Label ID="lblModeHeader" runat="server" Text=""></asp:Label>
            </h3>
        <asp:TextBox ID="txtUser" placeholder="Username" type="text" runat="server" CssClass="logininput"></asp:TextBox>
        <br />
        <asp:TextBox ID="txtPassword" type="password" placeholder="Password" runat="server" CssClass="logininput"></asp:TextBox>
        <br />
        <asp:TextBox ID="txtCaptchaInput" type="text" placeholder="Captcha Code Below" runat="server" CssClass="logininput"></asp:TextBox>
        <br />
        <uc1:CaptchaControl ID="CaptchaControl1" runat="server" />
        <br />
        <asp:Button ID="btnLogin" runat="server" Text="Log in" CssClass="button" 
            onclick="btnLogin_Click" />
        <asp:Button ID="btnResetPass" runat="server" Text="Save Password" 
            CssClass="button" onclick="btnResetPass_Click"  />
        <br />
        <asp:LinkButton ID="lnkForgot" runat="server" onclick="lnkForgot_Click">forgot password</asp:LinkButton>
    </div>

    <div style="float:left;width:49%;height:99%;margin:0 0 0 0;text-align:center;border-left:1px solid #bbb;" id="registerBox" runat="server">
        <h3>Register</h3>
        <asp:TextBox ID="txtNewUsername" placeholder="New Username" type="text" runat="server" CssClass="logininput"></asp:TextBox>
        <br />
        <asp:TextBox ID="txtNewEmail" placeholder="Your Email" type="email" runat="server" CssClass="logininput"></asp:TextBox>
        <br />
        <asp:TextBox ID="txtNewPassword" type="password" placeholder="New Password" runat="server" CssClass="logininput"></asp:TextBox>
        <br />
        <asp:TextBox ID="txtConfirmPassword" type="password" placeholder="Confirm Password" runat="server" CssClass="logininput"></asp:TextBox>
        <br />
        <asp:Button ID="btnRegister" runat="server" Text="Register" CssClass="button" 
            onclick="btnRegister_Click" />
    </div>
</div>