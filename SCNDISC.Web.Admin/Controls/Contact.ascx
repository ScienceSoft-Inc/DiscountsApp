<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Contact.ascx.cs" Inherits="SCNDISC.Web.Admin.Controls.Contact" %>
<div class="x-form-item ">
    <label class="x-form-side-label"><%= GetLocalResourceObject("Phone") %></label>
    <span class="x-form-number">1</span><asp:TextBox ID="uxFormPhone1" CssClass="x-form-phone" runat="server" meta:resourcekey="uxFormPhone1Resource1" />
    <span class="x-form-number">2</span><asp:TextBox ID="uxFormPhone2" CssClass="x-form-phone" runat="server" meta:resourcekey="uxFormPhone2Resource1" />
    <div style="clear: both"></div>
</div>
<div class="x-form-item ">
    <label class="x-form-side-label"><%= GetLocalResourceObject("Address") %></label>
    <asp:TextBox ID="uxFormAddress_RU" CssClass="x-form-address" runat="server" meta:resourcekey="uxFormAddress_RUResource1" />
    <asp:TextBox Visible="False" ID="uxFormAddress_EN" CssClass="x-form-address" runat="server" meta:resourcekey="uxFormAddress_ENResource1" />
    <input type="button" title="<%= GetLocalResourceObject("Hint") %>" class="x-form-action-btn x-gps-btn" value="&nbsp;" onclick="getCoordinatesByAddress(' #<%= uxFormAddress_RU.ClientID %>', '#<%= uxFormAddress_EN.ClientID %>', '#<%= uxFormPoint.ClientID %>' )" />
    <asp:TextBox ID="uxFormPoint" placeholder="00.000000, 00.000000" CssClass="x-form-coordinates" runat="server" meta:resourcekey="uxFormPointResource1" />
    
    <input type="button" title="<%= GetLocalResourceObject("CheckCoord") %>" class="x-form-open-map-btn" onclick="openMap(this)"/>

    <div style="clear: both"></div>
    <div class="x-form-error-ct">
        <asp:RegularExpressionValidator ControlToValidate="uxFormPoint"
                                        ValidationExpression="^[-+]?([1-8]?\d(\.\d+)?|90(\.0+)?),\s*[-+]?(180(\.0+)?|((1[0-7]\d)|([1-9]?\d))(\.\d+)?)$"
                                        Display="Dynamic"
                                        ErrorMessage="Ошибка ввода поле координаты. Поле должно являтся координатой, например 53.932160, 27.558136"
                                        runat="server" meta:resourcekey="RegularExpressionValidatorResource1" />
        <asp:RequiredFieldValidator ID="uxEmptyValidator" runat="server"
                                    ControlToValidate="uxFormPoint"
                                    Display="Dynamic"
                                    ErrorMessage="Ошибка ввода поле координаты. Поле должно являтся координатой, например 53.932160, 27.558136" meta:resourcekey="uxEmptyValidatorResource1"></asp:RequiredFieldValidator>
    </div>
</div>