<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Categories.ascx.cs" Inherits="SCNDISC.Web.Admin.Controls.Categories" %>
<asp:Repeater runat="server" ID="uxCategories">
    <ItemTemplate>
        <asp:HiddenField runat="server" ID="CategoryId" Value='<%# Eval("Id") %>'/>
        <asp:Panel ID="uxCategoryForm_Ru" runat="server" CssClass="x-category-name-box">
            <div class="x-form-item">
                <label class="x-form-side-label"><%= GetLocalResourceObject("CategoryName") %></label>
                <asp:TextBox ID="uxCategoryName_Ru" CssClass="x-form-service-field" runat="server" Text='<%# Eval("Name_RU") %>' />
                <div style="clear: both"></div>
            </div>
        </asp:Panel>
        <asp:Panel ID="uxCategoryForm_Eng" runat="server" CssClass="x-category-name-box">
            <div class="x-form-item">
                <label class="x-form-side-label"><%= GetLocalResourceObject("CategoryName") %></label>
                <asp:TextBox ID="uxCategoryName_Eng" CssClass="x-form-service-field" runat="server" Text='<%# Eval("Name_EN") %>' />
                <div style="clear: both"></div>
            </div>
        </asp:Panel>
        <asp:Button CausesValidation="False" ID="uxDelete" CssClass="x-delete-btn x-delete-category-btn" runat="server" meta:resourcekey="uxDeleteResource1" OnCommand="uxDelete_OnCommand" CommandArgument='<%# Container.ItemIndex %>' />
        <div class="x-form-item x-category-color-box ">
            <input type="color" ID="uxCategoryColor" runat="server" class="x-form-color-field" value='<%# Eval("Color") %>' />
            <div style="clear: both"></div>
        </div>
        <hr class="separator"/>
    </ItemTemplate>
</asp:Repeater>
