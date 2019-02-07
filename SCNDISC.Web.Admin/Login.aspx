<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SCNDISC.Web.Admin.Login" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title>Скидон — бесплатный сервис скидок</title>
        <link href="Resources/favicon.ico" rel="icon" />
        <link rel="stylesheet" type="text/css" href="Resources/Login.css?new" />
    </head>
    <body>
        <div class="x-login-box">
            <div class="x-company-logo">
                <asp:Literal ID="uxTitle" runat="server" meta:resourcekey="uxTitle"/>
            </div>
            <form id="form1" runat="server">
                <asp:TextBox ID="uxUserName" placeholder="Ваш логин" runat="server" meta:resourcekey="uxUserNameResource1" />
                <asp:TextBox ID="uxPassword" TextMode="Password" placeholder="Пароль" runat="server" meta:resourcekey="uxPasswordResource1" />
                <asp:Panel Visible="False" ID="uxError" CssClass="x-login-box-error" runat="server" ><asp:Literal meta:resourcekey="uxErrorResource1" runat="server"/></asp:Panel>
                <asp:Button OnClick="OnLoginClicked"  runat="server" text="Войти" meta:resourcekey="ButtonResource1" ></asp:Button>
            </form>
        </div>
    </body>
</html>