<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Contacts.ascx.cs" Inherits="SCNDISC.Web.Admin.Controls.Contacts" %>
<%@ Register TagPrefix="sc" TagName="Contact" Src="~/Controls/Contact.ascx" %>
<div class="x-form-title "><%= GetLocalResourceObject("Title") %>
    <div class="x-form-contacts-toolbar">
        <asp:LinkButton CssClass="x-form-delete" ID="uxRemove" OnClick="OnDeleteContact" CausesValidation="False" runat="server" meta:resourcekey="uxRemoveResource1" />
        <asp:LinkButton CssClass="x-form-add" ID="uxAdd" OnClick="OnAddContact" CausesValidation="False" runat="server" meta:resourcekey="uxAddResource1" />
        <div style="clear: both;"></div>
    </div>
</div>
<sc:Contact Visible="False" ID="uxContact1" runat="server"></sc:Contact>
<sc:Contact Visible="False" ID="uxContact2" runat="server"></sc:Contact>
<sc:Contact Visible="False" ID="uxContact3" runat="server"></sc:Contact>
<sc:Contact Visible="False" ID="uxContact4" runat="server"></sc:Contact>
<sc:Contact Visible="False" ID="uxContact5" runat="server"></sc:Contact>
<sc:Contact Visible="False" ID="uxContact6" runat="server"></sc:Contact>
<sc:Contact Visible="False" ID="uxContact7" runat="server"></sc:Contact>
<sc:Contact Visible="False" ID="uxContact8" runat="server"></sc:Contact>
<sc:Contact Visible="False" ID="uxContact9" runat="server"></sc:Contact>
<sc:Contact Visible="False" ID="uxContact10" runat="server"></sc:Contact>