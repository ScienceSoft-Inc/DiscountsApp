<%@ Page Language="C#" EnableEventValidation="false" ValidateRequest="false" UnobtrusiveValidationMode="None" AutoEventWireup="true" ClientIDMode="AutoID" CodeBehind="Default.aspx.cs" Inherits="SCNDISC.Web.Admin.Default" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Import Namespace="SCNDISC.Web.Admin.ServiceLayer" %>
<%@ Register TagPrefix="sc" TagName="Lang" Src="~/Controls/LanguageBar.ascx" %>
<%@ Register TagPrefix="sc" TagName="Contacts" Src="~/Controls/Contacts.ascx" %>
<%@ Register TagPrefix="sc" TagName="Categories" Src="~/Controls/Categories.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Скидон — бесплатный сервис скидок</title>
    <link href="Resources/favicon.ico" rel="icon" />

    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveUrl("Scripts/DataTables/jquery.dataTables.css") %>" />
    <link href="<%# Page.ResolveUrl("Scripts/jquery-ui-1.12.1.custom/jquery-ui.css") %>" rel="stylesheet" />

    <link rel="stylesheet" type="text/css" href="Resources/Styles.css?new8" />
    <link rel="stylesheet" type="text/css" href="Resources/Form.css?new8" />
</head>
<body>
    <form defaultbutton="uxFormSave" runat="server">
    <asp:ScriptManager runat="server">
        <Scripts>
            <asp:ScriptReference Path="Scripts/ui-localization.js?v=1" />
        </Scripts>
    </asp:ScriptManager>
        
        <div class="x-header">
            <asp:UpdatePanel UpdateMode="Conditional" runat="server">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="uxLangTips" />
                    <asp:AsyncPostBackTrigger ControlID="uxLangTipDetails" />
                    <asp:AsyncPostBackTrigger ControlID="uxLangTipCategories" />
                </Triggers>
                <ContentTemplate>
                    <div id="title" style="color: #0082dd; font-size: 28px; font-style: italic; margin-left: 12px; margin-top: 15px; position: absolute; z-index: 0;">
                        <asp:Literal ID="uxTitle" runat="server" meta:resourcekey="uxTitle"></asp:Literal>
                    </div>
                    <div class="x-filter-input">
                        <div style="width: 511px">
                            <asp:Panel ID="p" runat="server" DefaultButton="ApplyFilterNameBtn">
                                <asp:TextBox runat="server" ID="FilterName" meta:resourcekey="FilterName"></asp:TextBox>
                                <asp:Button ID="ApplyFilterNameBtn"
                                    class="x-form-open-map-btn"
                                    ToolTip="Искать"
                                    runat="server"
                                    CausesValidation="False"
                                    UseSubmitBehavior="false"
                                    OnClick="OnApplyFilter" />
                            </asp:Panel>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <div class="right-panel">
                <asp:LinkButton OnClick="OnLogout" ID="uxLogout" CausesValidation="False" ToolTip="Нажмите чтобы выйти" runat="server" CssClass="x-settings-btn" meta:resourcekey="uxLogoutResource1" />
                <div style="float: right;">
                    <asp:UpdatePanel UpdateMode="Conditional" runat="server">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="uxFormDelete" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="uxLangTipDetails" />
                            <asp:AsyncPostBackTrigger ControlID="uxLangTipCategories" />
                        </Triggers>
                        <ContentTemplate>
                            <sc:Lang ID="uxLangTips" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div style="float: right;">
                    <asp:UpdatePanel UpdateMode="Conditional" runat="server">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="uxLangTips" />
                            <asp:AsyncPostBackTrigger ControlID="uxLangTipDetails" />
                            <asp:AsyncPostBackTrigger ControlID="uxLangTipCategories" />
                        </Triggers>
                        <ContentTemplate>
                            <div class="dropdown">
                                <button onclick="toggleMenu(); return false;" class="dropbtn"><%= GetLocalResourceObject("menuBtnCaption") %></button>
                                <div id="dropdown-menu" class="dropdown-content">
                                    <a href="#" onclick="showManageCategories(true);"><%= GetLocalResourceObject("uxManageCategoriesResource1") %></a>
                                    <a href="#" onclick="showFeedBack();"><%= GetLocalResourceObject("feedbackMenuCaption") %></a>
                                    <a href="#" onclick="addPartner();"><%= GetLocalResourceObject("addPartner") %></a>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>

        <div class="x-body">
            <asp:UpdatePanel UpdateMode="Conditional" runat="server">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="uxLangTips" />
                    <asp:AsyncPostBackTrigger ControlID="uxLangTipDetails" />
                    <asp:AsyncPostBackTrigger ControlID="uxLangTipCategories" />
                    <asp:AsyncPostBackTrigger ControlID="uxFormDelete" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="ApplyFilterNameBtn" EventName="Click" />
                    <asp:PostBackTrigger ControlID="uxCategoryFormSave" />
                </Triggers>
                <ContentTemplate>
                    <div id="selectAll" class="x-form-category-item">
                        <ul class="x-form-types x-form-types-filter x-form-types-ru" style="width: 125px">
                            <li>
                                <input type="checkbox" id="chkbx_selectAll" checked="true" style="background-color: darkgray" runat="server" onclick="onFilterCategoryAllChanged(this)" />
                                <label for="chkbx_selectAll" style="background-color: darkgray; font-weight: bold"><%= GetLocalResourceObject("selectAll") %></label>
                            </li>
                        </ul>
                        <asp:CheckBoxList CssClass="x-form-types x-form-types-filter x-form-types-ru"
                            RepeatLayout="UnorderedList"
                            DataTextField="Names"
                            DataValueField="Id"
                            ID="CategoryFilter"
                            AutoPostBack="True"
                            OnSelectedIndexChanged="OnApplyFilter" runat="server">
                        </asp:CheckBoxList>
                    </div>

                    <asp:Repeater OnItemCommand="OnItemCommand" ID="uxTips_Ru" runat="server">
                        <ItemTemplate>
                            <asp:LinkButton CausesValidation="False" ToolTip="Нажмите для редактирования" ID="uxEditTip" CommandArgument='<%# Eval("Id") %>' CssClass="x-item" runat="server" meta:resourcekey="uxEditTipResource1">
                                <div style="<%# Eval("Image") == null ? "": "background-image: url(" + Page.ResolveUrl("GetImage.ashx") + "?id=" + Eval("Id") + ")" %>" class="x-item-image"></div>
                                <div style="<%# Eval("Icon") == null ? "": "background-image: url(" + Page.ResolveUrl("GetImage.ashx") + "?logo=true&id=" + Eval("Id") + ")" %>" class="x-item-logo"></div>
                                <div class="x-item-title">
                                    <%# Server.HtmlEncode((string) Eval("Name_RU")) %>
                                </div>
                                <div class="x-types-container">
                                    <asp:Repeater ID="childRepeater" DataSource='<%# ((TipForm) Container.DataItem).Tags %>' runat="server">
                                        <ItemTemplate>
                                            <div class="x-item-type" style='background-color: <%# Eval("Color") %>'><%# Eval("Name_RU") %></div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <div style="clear: both;"></div>
                                </div>
                                <div class="x-item-desc"><%# Server.HtmlEncode((string) Eval("Description_RU")) %></div>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Repeater Visible="False" OnItemCommand="OnItemCommand" ID="uxTips_Eng" runat="server">
                        <ItemTemplate>
                            <asp:LinkButton CausesValidation="False" ToolTip="Нажмите для редактирования" ID="uxEditTip" CommandArgument='<%# Eval("Id") %>' CssClass="x-item" runat="server" meta:resourcekey="uxEditTipResource3">
                                <div style="<%# Eval("Image") == null ? "": "background-image: url(" + Page.ResolveUrl("GetImage.ashx") + "?id=" + Eval("Id") + ")" %>" class="x-item-image"></div>
                                <div style="<%# Eval("Icon") == null ? "": "background-image: url(" + Page.ResolveUrl("GetImage.ashx") + "?logo=true&id=" + Eval("Id") + ")" %>" class="x-item-logo"></div>
                                <div class="x-item-title">
                                    <%# Server.HtmlEncode((string) Eval("Name_EN")) %>
                                </div>
                                <div class="x-types-container">
                                    <asp:Repeater ID="childRepeater" DataSource='<%# ((TipForm) Container.DataItem).Tags %>' runat="server">
                                        <ItemTemplate>
                                            <div class="x-item-type x-item-type-1" style='background-color: <%# Eval("Color") %>'><%# Eval("Name_EN") %></div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <div style="clear: both;"></div>
                                </div>
                                <div class="x-item-desc"><%# Server.HtmlEncode((string) Eval("Description_EN")) %></div>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:LinkButton CausesValidation="False" ToolTip="Нажмите чтобы добавить партнера" class="x-item-plus" OnClick="OnNewAdd" ID="uxAddNew" runat="server" meta:resourcekey="uxAddNewResource1"></asp:LinkButton>
                    <div style="clear: both"></div>
                    <asp:Panel class="x-empty-tips" ID="uxNullScreen" runat="server" meta:resourcekey="uxNullScreenResource1">
                        <div class="x-empty-tips-title"><%= GetLocalResourceObject("Welcome") %></div>
                        <div class="x-empty-tips-description">
                            <%= GetLocalResourceObject("Description") %>
                            <br />
                            <asp:Button runat="server" CausesValidation="False" ID="uxStartNew" OnClick="OnNewAdd" CssClass="x-primary-btn" Text="Начать Работу" meta:resourcekey="uxStartNewResource1" />
                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div id="uxHighlight" class="x-highlight"></div>

        <div id="uxCategoryForm" class="x-form">
            <asp:UpdatePanel UpdateMode="Conditional" runat="server">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="uxLangTips" />
                    <asp:AsyncPostBackTrigger ControlID="uxLangTipDetails" />
                    <asp:AsyncPostBackTrigger ControlID="uxTips_Ru" EventName="ItemCommand" />
                    <asp:AsyncPostBackTrigger ControlID="uxTips_Eng" EventName="ItemCommand" />
                    <asp:AsyncPostBackTrigger ControlID="uxCategoryFormAdd" />
                    <asp:PostBackTrigger ControlID="uxCategoryFormSave" />
                    <asp:PostBackTrigger ControlID="uxCategoryFormCancel" />
                </Triggers>
                <ContentTemplate>
                    <sc:Lang ID="uxLangTipCategories" runat="server" />
                    <div class="x-form-title x-form-category-title"><%= GetLocalResourceObject("Categories") %></div>
                    <sc:Categories runat="server" ID="uxCategories" OnCategoryDeleted="OnCategoryDeleted" />
                    <asp:Button CausesValidation="False" ID="uxCategoryFormAdd" OnClick="OnAddCategory" CssClass="x-form-action-btn x-add-category-btn" runat="server" meta:resourcekey="uxFormAddResource1" />
                    <div style="margin-top: 20px;">
                        <input id="uxCategoryFormDeleteQuestion" type="hidden" value="<%= GetLocalResourceObject("ConfirmDeleteCategory") %>" />
                        <asp:HiddenField runat="server" ID="uxCategoryFormDeleteList_Ru" />
                        <asp:HiddenField runat="server" ID="uxCategoryFormDeleteList_Eng" />
                        <asp:Button CausesValidation="False" ID="uxCategoryFormSave" OnClick="OnSaveCategories" OnClientClick="return confirmDeleteCategories();" class="x-primary-btn" runat="server" meta:resourcekey="uxFormSaveResource1" />
                        <asp:Button CausesValidation="False" ID="uxCategoryFormCancel" OnClick="OnCancelCategories" class="x-secondary-btn" runat="server" meta:resourcekey="uxFormCancelResource1" />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div id="uxForm" class="x-form">
            <asp:UpdatePanel UpdateMode="Conditional" runat="server">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="uxStartNew" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="uxAddNew" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="uxTips_Ru" EventName="ItemCommand" />
                    <asp:AsyncPostBackTrigger ControlID="uxTips_Eng" EventName="ItemCommand" />
                    <asp:AsyncPostBackTrigger ControlID="uxLangTips" />
                    <asp:AsyncPostBackTrigger ControlID="uxLangTipCategories" />
                    <asp:PostBackTrigger ControlID="uxFormSave" />
                </Triggers>
                <ContentTemplate>
                    <div class="x-form-title"><%= GetLocalResourceObject("Service") %></div>
                    <sc:Lang ID="uxLangTipDetails" runat="server" />
                    <asp:HiddenField ID="uxTipID" runat="server" />
                    <asp:Panel ID="uxServiceDetails_Ru" runat="server" meta:resourcekey="uxServiceDetails_RuResource1">
                        <div class="x-form-item">
                            <label class="x-form-side-label"><%= GetLocalResourceObject("Name") %></label>
                            <asp:TextBox ID="uxName_Ru" CssClass="x-form-service-field" runat="server" meta:resourcekey="uxName_RuResource1" />
                            <div style="clear: both"></div>
                        </div>
                        <div class="x-form-item">
                            <label class="x-form-side-label"><%= GetLocalResourceObject("ServiceDescription") %></label>
                            <asp:TextBox ID="uxDescription_Ru" TextMode="MultiLine" runat="server" meta:resourcekey="uxDescription_RuResource1" />
                            <div style="clear: both"></div>
                        </div>
                    </asp:Panel>
                    <asp:Panel Visible="False" ID="uxServiceDetails_Eng" runat="server" meta:resourcekey="uxServiceDetails_EngResource1">
                        <div class="x-form-item">
                            <label class="x-form-side-label"><%= GetLocalResourceObject("Name") %></label>
                            <asp:TextBox ID="uxName_Eng" CssClass="x-form-service-field" runat="server" meta:resourcekey="uxName_EngResource1" />
                            <div style="clear: both"></div>
                        </div>
                        <div class="x-form-item">
                            <label class="x-form-side-label"><%= GetLocalResourceObject("ServiceDescription") %></label>
                            <asp:TextBox ID="uxFormDecription_Eng" TextMode="MultiLine" runat="server" meta:resourcekey="uxFormDecription_EngResource1" />
                            <div style="clear: both"></div>
                        </div>
                    </asp:Panel>
                    <div class="x-form-category-item">
                        <label class="x-form-side-label"><%= GetLocalResourceObject("Category") %></label>
                        <asp:CheckBoxList class="x-form-types x-form-types-ru" RepeatLayout="UnorderedList" DataTextField="Names"
                            DataValueField="Id" ID="uxFormTypes" runat="server" meta:resourcekey="uxFormTypesResource1" />
                        <div style="clear: both"></div>
                    </div>
                    <div class="x-form-item">
                        <label class="x-form-side-label">Discount</label>
                        <asp:TextBox ID="uxFormDiscount"
                            CssClass="x-form-discount-field"
                            runat="server"
                            meta:resourcekey="uxFormDiscountResource1" />

                        <asp:DropDownList runat="server" CssClass="x-discount-type" ID="DiscountType">
                            <asp:ListItem Value="0" Text="%"></asp:ListItem>
                            <asp:ListItem Value="1" Text="Br"></asp:ListItem>
                        </asp:DropDownList>
                        <%--                        <label class="x-form-discount-label">%</label>--%>

                        <div style="clear: both"></div>
                        <div class="x-form-error-ct">
                            <asp:RegularExpressionValidator ID="uxNumberValidator" runat="server"
                                ControlToValidate="uxFormDiscount"
                                ValidationExpression="^[0-9]+$"
                                Display="Dynamic"
                                ErrorMessage="Ошибка ввода поле скидка. Поле должно быть числовым значением" meta:resourcekey="uxNumberValidatorResource1" />
                            <asp:RequiredFieldValidator ID="uxEmptyValidator" runat="server"
                                ControlToValidate="uxFormDiscount"
                                Display="Dynamic"
                                ErrorMessage="Ошибка ввода поле скидка. Поле должно быть числовым значением" meta:resourcekey="uxEmptyValidatorResource1"></asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <%--                        <label class="x-form-side-label"><%= GetLocalResourceObject("Url") %></label>--%>
                    <%--                        <asp:TextBox ID="uxFormUrl" runat="server" class="x-form-http-field" meta:resourcekey="uxFormUrlResource1" />--%>

                    <asp:Repeater runat="server" ID="uxWebUrls" OnItemCommand="OnRemoveWebAddressCommand" OnItemDataBound="OnWebAddressItemDataBound">
                        <ItemTemplate>
                            <div class="x-form-item">
                                <asp:HiddenField runat="server" ID="WebAddressId" Value='<%# Eval("Id") %>' />
                                <label class="x-form-side-label">URL# <%# Eval("Index") %> </label>

                                <asp:TextBox ID="uxWebUrl" runat="server" class="x-form-http-field" Text='<%# Eval("Url") %>'>' ></asp:TextBox>
                                <asp:DropDownList ID="WebAddressCategory" DataTextField="Name" DataValueField="Id" runat="server"></asp:DropDownList>
                                <asp:LinkButton CssClass="x-form-delete"
                                    CommandArgument='<%# Eval("Id") %>'
                                    ID="uxDeleteWebAddress"
                                    ToolTip="Удалить"
                                    meta:resourcekey="uxDeleteWebAddress"
                                    CausesValidation="False"
                                    runat="server" />
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>

                    <div class="x-form-item x-add-web-wrapper">
                        <asp:LinkButton CssClass="x-form-add x-add-web-url"
                            CommandName="AddWebAddress"
                            OnCommand="OnAddWebAddressCommand"
                            ToolTip="Добавить Web Адрес"
                            meta:resourcekey="AddWebAddress"
                            ID="uxAddWebAddress"
                            CausesValidation="False"
                            runat="server" />
                    </div>
                    <div class="x-form-item">
                        <label class="x-form-side-label"><%= GetLocalResourceObject("Comment") %></label>
                        <asp:TextBox ID="uxComment" TextMode="MultiLine" runat="server" />
                        <div style="clear: both"></div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:Panel ID="uxFormUploadBar" runat="server" meta:resourcekey="uxFormUploadBarResource1">
                <div class="x-form-title"><%= GetLocalResourceObject("Mdeia") %></div>
                <div class="x-form-item">
                    <asp:UpdatePanel ID="fileUploadTexts" runat="server" UpdateMode="Conditional">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="uxLangTips" />
                            <asp:AsyncPostBackTrigger ControlID="uxLangTipDetails" />
                            <asp:AsyncPostBackTrigger ControlID="uxLangTipCategories" />
                        </Triggers>
                        <ContentTemplate>
                            <label class="x-form-side-label"><%= GetLocalResourceObject("Files") %></label>
                            <asp:Label CssClass="x-form-action-btn x-form-file-upload" AssociatedControlID="uxFormAvatarUpload" runat="server" meta:resourcekey="LabelResource1">Выбрать Аватар</asp:Label>
                            <asp:Label CssClass="x-form-action-btn x-form-file-upload" AssociatedControlID="uxFormThemeUpload" runat="server" meta:resourcekey="LabelResource2">Выбрать Тему</asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:FileUpload ID="uxFormAvatarUpload" runat="server" meta:resourcekey="uxFormAvatarUploadResource1" />
                    <asp:FileUpload ID="uxFormThemeUpload" runat="server" meta:resourcekey="uxFormThemeUploadResource1" />
                    <div style="clear: both"></div>
                    <asp:UpdatePanel ID="picInstruction" runat="server" UpdateMode="Conditional">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="uxLangTips" />
                            <asp:AsyncPostBackTrigger ControlID="uxLangTipDetails" />
                            <asp:AsyncPostBackTrigger ControlID="uxLangTipCategories" />
                        </Triggers>
                        <ContentTemplate>
                            <div class="x-form-hint">
                                <%= GetLocalResourceObject("PicInstructions") %>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </asp:Panel>
            <asp:UpdatePanel UpdateMode="Conditional" runat="server">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="uxStartNew" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="uxAddNew" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="uxLangTips" />
                    <asp:AsyncPostBackTrigger ControlID="uxLangTipDetails" />
                    <asp:AsyncPostBackTrigger ControlID="uxLangTipCategories" />
                    <asp:AsyncPostBackTrigger ControlID="uxTips_Ru" EventName="ItemCommand" />
                    <asp:AsyncPostBackTrigger ControlID="uxTips_Eng" EventName="ItemCommand" />
                    <asp:PostBackTrigger ControlID="uxFormSave" />
                </Triggers>
                <ContentTemplate>
                    <sc:Contacts runat="server" ID="uxContacts" />
                    <asp:Button CausesValidation="False" ID="uxFormDelete" OnClick="OnDeleteTip" class="x-delete-btn" Text="Удалить" runat="server" meta:resourcekey="uxFormDeleteResource1" />
                    <asp:Button ID="uxFormSave" OnClick="OnSaveTip" class="x-primary-btn" Text="Сохранить" runat="server" meta:resourcekey="uxFormSaveResource1" />
                    <input type="button" class="x-secondary-btn" value="<%= GetLocalResourceObject("Cancel") %>" onclick=" new TipForm().show(false); " />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>

<div style="" id="feedbackDialog">
    <table id="feedback-table">
    </table>
</div>
</body>

<script type="text/javascript" src="https://code.jquery.com/jquery-1.11.3.min.js"></script>
<script src="<%# Page.ResolveUrl("Scripts/jquery-ui-1.12.1.custom/jquery-ui.js") %>"></script>
<script src="<%# Page.ResolveUrl("Scripts/moment.js") %>"></script>
<script type="text/javascript" charset="utf8" src="<%# Page.ResolveUrl("Scripts/DataTables/jquery.dataTables.js") %>"></script>
<script src="https://maps.googleapis.com/maps/api/js?key=xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"></script>
<script type="text/javascript">
    function fixHighlight() { $("#uxHighlight").css('height', document.body.scrollHeight + "px"); }
    Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(fixHighlight);

    function fixCategoryColor() {
        $('ul.x-form-types li').each(function () {
            var item = $(this);
            var color = item.find('label input:hidden').val();
            item.css('background-color', color);
        });
    }
    Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(fixCategoryColor);

    $(window).resize(fixHighlight);
    $(document.body).click(fixHighlight);

    function TipForm() {
        $("#uxFormPartners").change(function () { $("#uxFormUploadBar").toggle(!$(this).val()); });
    }

    TipForm.prototype.show = function (visibility) {
        $("#uxForm").toggle(visibility);
        $("#uxHighlight").toggle(visibility);
        $("#uxHighlight").css('height', document.body.scrollHeight + "px");
        $("#uxFormUploadBar").toggle(!$("#uxFormPartners").val());
        if (visibility) {
            $('html, body').animate({ scrollTop: 0 }, 'fast');
        }
    };

    function getCoordinatesByAddress(ruAddressID, enAddressID, coordinatesID) {
        var addresses = [ruAddressID, enAddressID];
        $.each(addresses, function () {
            var address = $(this.toString()).val();
            if (typeof address !== 'undefined') {
                var geoCoder = new window.google.maps.Geocoder();
                geoCoder.geocode({ 'address': address }, function (results, status) {
                    if (status === window.google.maps.GeocoderStatus.OK) {
                        var location = results[0].geometry.location;
                        $(coordinatesID).val(location.lat() + ', ' + location.lng());
                        return false;
                    }
                    else {
                        alert('Geocode was not successful for the following reason: ' + status);
                    }
                });
            }
        });
    }

    function localizedValue(key) {
        var lang = $("[name='uxLangTips$uxLang']").val();
        if (!lang) {
            lang = "en";
        }

        var value = localizationSet[lang][key];
        if (!value) {
            return "[" + key + "]";
        }

        return value;
    }

    function openMap(btn) {
        var coordinates = $(btn).parent().find(".x-form-coordinates").val();
        if (!coordinates) {
            return false;
        }

        var coordParts = coordinates.split(",");
        if (!coordParts || coordParts.length != 2) {
            return false;
        }

        var lat = coordParts[0];
        var lon = coordParts[1];
        var url = "http://maps.google.com/maps?z=12&t=m&q=loc:" + lat + "+" + lon;
        window.open(url, "_blank");

        return false;
    }

    function onFilterCategoryAllChanged(ctx) {
        $(".x-form-types-filter [type=checkbox]").prop("checked", ctx.checked);

        __doPostBack('CategoryFilter', '');

        return false;
    }

    function showManageCategories(show) {
        $("#uxCategoryForm").toggle(show);
        $("#uxHighlight").toggle(show);
    }

    function confirmDeleteCategories() {
        var lang = $('#uxLangTipCategories_uxLang').val();
        var question = $('#uxCategoryFormDeleteQuestion').val();
        var categories = $(lang == 'ru' ? '#uxCategoryFormDeleteList_Ru' : '#uxCategoryFormDeleteList_Eng').val();
        return categories == '' || confirm(question + '\n' + categories);
    }

    function addPartner() {
        __doPostBack('uxAddNew', '');

        return false;
    }

    function showFeedBack() {
        var table;

        var buttons = {};
        buttons[localizedValue("dialog.close")] = function () {
            $(this).dialog("close");
        }

        var closeKeyHandler = function (e) {
            if ((e.key == "c") || (e.key == "з")) {
                $('#feedbackDialog').dialog('close');
            }
        };

        $("#feedbackDialog").dialog({
            height: "600",
            width: 800,
            modal: true,
            responsive: true,
            title: localizedValue("Feedbacks"),
            open: function () {
                table = renderFeedbacks();
				$(document).bind("keypress", closeKeyHandler);
            },
            close: function() {
                table.destroy();
				$(document).unbind("keypress", closeKeyHandler);
            },
            buttons: buttons
        });

        //$("#feedbackDialog").removeClass("x-form");
    }

    function renderFeedbacks() {
        return $("#feedback-table").DataTable({
                    searching: false,
                    ordering: false,
                    lengthChange: false,
                    serverSide: true,
                    sScrollY: "auto",
                    scrollCollapse: true,
                    scroller: true,
                    processing: true,
                    autoWidth: true,
                    sDom: '<"top"iflp>rt<"clear">',
                    "language": {
                        "emptyTable": localizedValue("dataTable.emptyTable"),
                        "loadingRecords": localizedValue("dataTable.loadingRecords"),
                        "info": localizedValue("dataTable.info"),
                        "infoEmpty": localizedValue("dataTable.infoEmpty"),
                        "processing": localizedValue("dataTable.processing"),
                        "paginate": {
                            "first": localizedValue("dataTable.paginate.first"),
                            "last": localizedValue("dataTable.paginate.last"),
                            "next": localizedValue("dataTable.paginate.next"),
                            "previous": localizedValue("dataTable.paginate.previous")
                        },
                    },
                    ajax: '<%= WebApiUrl %>' + '/feedbacks',
                    aoColumns: [
                        {
                            data: 'Name',
                            sTitle: localizedValue("UserName"),
                            width: '120px',
                            render: function(data) {
                                return htmlEncode(data);
                            }
                        },
                        {
                            data: 'Message',
                            sTitle: localizedValue("Message"),
                            render: function(data) {
                                return htmlEncode(data);
                            }
                        },
                        {
                            data: 'Created',
                            sTitle: localizedValue("FeedbackDate"),
                            width: '140px',
                            render: function(data, type, row) {
                                if (type === "sort" || type === "type") {
                                    return data;
                                }

                                return moment(data).format("YYYY-MM-DD HH:mm");
                            }
                        }
                    ]
                });
    }

    function toggleMenu() {
        document.getElementById("dropdown-menu").classList.toggle("show");
        return false;
    }

    function htmlEncode(html) {
        return $('<div/>').text(html).html();
    }

// Close the dropdown if the user clicks outside of it
    window.onclick = function(event) {
        if (!event.target.matches('.dropbtn')) {

            var dropdowns = document.getElementsByClassName("dropdown-content");
            var i;
            for (i = 0; i < dropdowns.length; i++) {
                var openDropdown = dropdowns[i];
                if (openDropdown.classList.contains('show')) {
                    openDropdown.classList.remove('show');
                    return false;
                }
            }
        }
    }
    
</script>
</html>
