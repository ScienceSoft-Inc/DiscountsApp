﻿using ScnDiscounts.Models;
using ScnDiscounts.Models.Data;
using ScnDiscounts.Views;
using ScnDiscounts.Views.ContentUI;
using ScnPage.Plugin.Forms;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ScnDiscounts.Helpers
{
    public static class ViewHelper
    {
        public static async void ClickAnimation(this View view, Action action = null)
        {
            await view.ScaleTo(0.95, 100, Easing.CubicOut);

            action?.Invoke();

            await view.ScaleTo(1, 100, Easing.CubicOut);
        }

        public static async Task OpenDetailPage(this BaseContentPage page, string documentId, bool isSilent = false)
        {
            if (page.IsOpenning)
                return;

            page.ViewModel.IsLoadActivity = true;
            await Task.Yield();

            DiscountDetailData discountDetailData;

            try
            {
                discountDetailData = AppData.Discount.Db.LoadDiscountDetail(documentId);
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteException(ex);

                discountDetailData = null;
            }

            if (discountDetailData == null)
            {
                page.ViewModel.IsLoadActivity = false;

                if (!isSilent)
                {
                    var discountContentUI = new DiscountContentUI();
                    await page.DisplayAlert(null, discountContentUI.TitleErrLoading, discountContentUI.TxtOk);
                }
            }
            else
            {
                Functions.SafeCall(() => page.OpenPage(new DiscountDetailPage(discountDetailData)));

                page.ViewModel.IsLoadActivity = false;
            }
        }
    }
}
