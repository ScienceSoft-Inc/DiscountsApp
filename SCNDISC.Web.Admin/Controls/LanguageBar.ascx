<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LanguageBar.ascx.cs" Inherits="SCNDISC.Web.Admin.LanguageBar" %>
<asp:DropDownList ID="uxLang" OnSelectedIndexChanged="OnLanguageChanged" AutoPostBack="true" CssClass="x-lang-bar x-lang-bar-ru" runat="server">
    <asp:ListItem Value="ru" Text="Руc"></asp:ListItem>
    <asp:ListItem Value="en" Text="Eng"></asp:ListItem>
</asp:DropDownList>