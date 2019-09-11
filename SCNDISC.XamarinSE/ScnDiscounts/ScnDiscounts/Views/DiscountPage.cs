using ScnDiscounts.Control;
using ScnDiscounts.Helpers;
using ScnDiscounts.ViewModels;
using ScnDiscounts.Views.ContentUI;
using ScnDiscounts.Views.Styles;
using ScnSideMenu.Forms;
using ScnTitleBar.Forms;
using System;
using Xamarin.Forms;
using ImageButton = ScnTitleBar.Forms.ImageButton;

namespace ScnDiscounts.Views
{
    public class DiscountPage : SideBarPage
    {
        private DiscountViewModel viewModel => (DiscountViewModel) ViewModel;

        private DiscountContentUI contentUI => (DiscountContentUI) ContentUI;

        protected StackLayout StackByDistance;

        public DiscountPage()
            : base(typeof(DiscountViewModel), typeof(DiscountContentUI), PanelSetEnum.psRight)
        {
            BackgroundColor = MainStyles.StatusBarColor.FromResources<Color>();
            Content.BackgroundColor = MainStyles.MainLightBackgroundColor.FromResources<Color>();

            var loadingColor = MainStyles.LoadingColor.FromResources<Color>();
            LoadingActivityIndicator.Color = loadingColor;
            LoadingActivityText.TextColor = loadingColor;

            var appBar = new TitleBar(this, TitleBar.BarBtnEnum.bbBackRight)
            {
                BarColor = MainStyles.StatusBarColor.FromResources<Color>(),
                BtnBack =
                {
                    Source = contentUI.IconBack
                },
                BtnRight =
                {
                    Triggers =
                    {
                        new DataTrigger(typeof(ImageButton))
                        {
                            Binding = new Binding(nameof(DiscountViewModel.IsCustomFiltering)),
                            Value = true,
                            Setters =
                            {
                                new Setter
                                {
                                    Property = ImageButton.SourceProperty,
                                    Value = contentUI.IconMoreFilter
                                }
                            }
                        },
                        new DataTrigger(typeof(ImageButton))
                        {
                            Binding = new Binding(nameof(DiscountViewModel.IsCustomFiltering)),
                            Value = false,
                            Setters =
                            {
                                new Setter
                                {
                                    Property = ImageButton.SourceProperty,
                                    Value = contentUI.IconMore
                                }
                            }
                        }
                    }
                }
            };

            appBar.BtnRight.Click += BtnFilter_Click;

            InitSearchBar(appBar);
            InitFilterPanel();

            var listViewFooter = new ContentView
            {
                HeightRequest = 4
            };

            var discountListView = new ListView(ListViewCachingStrategy.RetainElement)
            {
                BackgroundColor = MainStyles.MainLightBackgroundColor.FromResources<Color>(),
                SeparatorVisibility = SeparatorVisibility.None,
                SelectionMode = ListViewSelectionMode.None,
                ItemTemplate = new DataTemplate(typeof(DiscountItemTemplate)),
                RowHeight = Functions.OnPlatform(125, 135),
                Header = new ContentView
                {
                    HeightRequest = 4
                },
                Footer = new ContentView
                {
                    Content = listViewFooter
                }
            };
            discountListView.SetBinding(ListView.ItemsSourceProperty, nameof(DiscountViewModel.DiscountItems));
            discountListView.SetBinding(IsVisibleProperty, nameof(DiscountViewModel.HasDiscountItems));
            discountListView.ItemSelected += (sender, args) => ((ListView) sender).SelectedItem = null;
            discountListView.ItemTapped += viewModel.OnDiscountItemTapped;

            var emptyLabel = new Label
            {
                Style = LabelStyles.EmptyListLabelStyle.FromResources<Style>(),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Text = contentUI.TxtEmptyList
            };
            emptyLabel.SetBinding(IsVisibleProperty, nameof(DiscountViewModel.HasNoDiscountItems));

            var safeAreaHelper = new SafeAreaHelper();
            safeAreaHelper.UseSafeArea(this, SafeAreaHelper.CustomSafeAreaFlags.Top);
            safeAreaHelper.UseSafeArea(appBar.BtnBack, SafeAreaHelper.CustomSafeAreaFlags.Left);
            safeAreaHelper.UseSafeArea(appBar.BtnRight, SafeAreaHelper.CustomSafeAreaFlags.Right);
            safeAreaHelper.UseSafeArea(RightPanel.Content, SafeAreaHelper.CustomSafeAreaFlags.Right);
            safeAreaHelper.UseSafeArea(discountListView, SafeAreaHelper.CustomSafeAreaFlags.Horizontal);
            safeAreaHelper.UseSafeArea(listViewFooter, SafeAreaHelper.CustomSafeAreaFlags.Bottom);
            safeAreaHelper.UseSafeArea(emptyLabel, SafeAreaHelper.CustomSafeAreaFlags.Horizontal);

            ContentLayout.Children.Add(appBar);
            ContentLayout.Children.Add(discountListView);
            ContentLayout.Children.Add(emptyLabel);
        }

        private void InitSearchBar(TitleBar appBar)
        {
            var seacrhBar = new SearchBarExtended
            {
                Style = LabelStyles.SearchBarStyle.FromResources<Style>(),
                VerticalOptions = LayoutOptions.Center,
                HeightRequest = Functions.OnPlatform(38, 30),
                Placeholder = contentUI.TxtSearchByText
            };
            seacrhBar.SetBinding(SearchBar.TextProperty, nameof(DiscountViewModel.SearchText));

            appBar.Content = seacrhBar;
        }

        private void InitFilterPanel()
        {
            SpeedAnimatePanel = 200;

            RightPanelWidthRequest = 400;

            RightSwipeSize = 30;
            TransparentSize = new Thickness(50, 50, 0, 0);

            RightPanel.BackgroundColor = MainStyles.MainBackgroundColor.FromResources<Color>();
            RightPanel.Opacity = 0.95;

            var rightPanelContent = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Spacing = 0
            };

            rightPanelContent.Children.Add(new BoxView
            {
                BackgroundColor = Color.Transparent,
                HeightRequest = 50
            });

            #region sort group title 

            var labelSortBy = new Label
            {
                VerticalOptions = LayoutOptions.Center,
                Style = LabelStyles.MenuStyle.FromResources<Style>(),
                Text = contentUI.TxtSortBy
            };

            var labelSortByContainer = new ContentView
            {
                BackgroundColor = MainStyles.StatusBarColor.FromResources<Color>(),
                Padding = new Thickness(App.IsMoreThan320Dpi ? 40 : 20, 0),
                HeightRequest = 40,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Content = labelSortBy
            };

            rightPanelContent.Children.Add(labelSortByContainer);

            #endregion

            #region sort by name

            var titleByName = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Spacing = 16,
                Children =
                {
                    new Image
                    {
                        VerticalOptions = LayoutOptions.Center,
                        Source = contentUI.IconSortByName
                    },
                    new Label
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.Center,
                        Text = contentUI.TxtSortByName,
                        Triggers =
                        {
                            new DataTrigger(typeof(Label))
                            {
                                Binding = new Binding(nameof(DiscountViewModel.IsSortByName)),
                                Value = true,
                                Setters =
                                {
                                    new Setter
                                    {
                                        Property = StyleProperty,
                                        Value = LabelStyles.MenuStyle.FromResources()
                                    }
                                }
                            },
                            new DataTrigger(typeof(Label))
                            {
                                Binding = new Binding(nameof(DiscountViewModel.IsSortByName)),
                                Value = false,
                                Setters =
                                {
                                    new Setter
                                    {
                                        Property = StyleProperty,
                                        Value = LabelStyles.MenuDisabledStyle.FromResources()
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var switchByName = new Switch
            {
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center,
                OnColor = MainStyles.SwitchColor.FromResources<Color>()
            };
            switchByName.SetBinding(Switch.IsToggledProperty, nameof(DiscountViewModel.IsSortByName));
            switchByName.SetBinding(InputTransparentProperty, nameof(DiscountViewModel.IsSortByName));

            var stackByName = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(App.IsMoreThan320Dpi ? 40 : 20, 2),
                Spacing = 0,
                Children =
                {
                    titleByName,
                    switchByName
                }
            };

            var tapGestureRecognizerByName = new TapGestureRecognizer();
            tapGestureRecognizerByName.Tapped += viewModel.SortByName_Tap;
            stackByName.GestureRecognizers.Add(tapGestureRecognizerByName);

            rightPanelContent.Children.Add(stackByName);

            #endregion

            #region sort by distance

            var titleByDistance = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Spacing = 16,
                Children =
                {
                    new Image
                    {
                        VerticalOptions = LayoutOptions.Center,
                        Source = contentUI.IconSortByDistance
                    },
                    new Label
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.Center,
                        Text = contentUI.TxtSortByDistance,
                        Triggers =
                        {
                            new DataTrigger(typeof(Label))
                            {
                                Binding = new Binding(nameof(DiscountViewModel.IsSortByDistance)),
                                Value = true,
                                Setters =
                                {
                                    new Setter
                                    {
                                        Property = StyleProperty,
                                        Value = LabelStyles.MenuStyle.FromResources()
                                    }
                                }
                            },
                            new DataTrigger(typeof(Label))
                            {
                                Binding = new Binding(nameof(DiscountViewModel.IsSortByDistance)),
                                Value = false,
                                Setters =
                                {
                                    new Setter
                                    {
                                        Property = StyleProperty,
                                        Value = LabelStyles.MenuDisabledStyle.FromResources()
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var switchByDistance = new Switch
            {
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center,
                OnColor = MainStyles.SwitchColor.FromResources<Color>()
            };
            switchByDistance.SetBinding(Switch.IsToggledProperty, nameof(DiscountViewModel.IsSortByDistance));
            switchByDistance.SetBinding(InputTransparentProperty, nameof(DiscountViewModel.IsSortByDistance));

            StackByDistance = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(App.IsMoreThan320Dpi ? 40 : 20, 2),
                Spacing = 0,
                Children =
                {
                    titleByDistance,
                    switchByDistance
                }
            };

            var tapGestureRecognizerByDistance = new TapGestureRecognizer();
            tapGestureRecognizerByDistance.Tapped += viewModel.SortByDistance_Tap;
            StackByDistance.GestureRecognizers.Add(tapGestureRecognizerByDistance);

            rightPanelContent.Children.Add(StackByDistance);

            #endregion

            #region sort by recent date

            var titleByRecentDate = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Spacing = 16,
                Children =
                {
                    new Image
                    {
                        VerticalOptions = LayoutOptions.Center,
                        Source = contentUI.IconSortByRecentDate
                    },
                    new Label
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.Center,
                        Text = contentUI.TxtSortByRecentDate,
                        Triggers =
                        {
                            new DataTrigger(typeof(Label))
                            {
                                Binding = new Binding(nameof(DiscountViewModel.IsSortByRecentDate)),
                                Value = true,
                                Setters =
                                {
                                    new Setter
                                    {
                                        Property = StyleProperty,
                                        Value = LabelStyles.MenuStyle.FromResources()
                                    }
                                }
                            },
                            new DataTrigger(typeof(Label))
                            {
                                Binding = new Binding(nameof(DiscountViewModel.IsSortByRecentDate)),
                                Value = false,
                                Setters =
                                {
                                    new Setter
                                    {
                                        Property = StyleProperty,
                                        Value = LabelStyles.MenuDisabledStyle.FromResources()
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var switchByRecentDate = new Switch
            {
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center,
                OnColor = MainStyles.SwitchColor.FromResources<Color>()
            };
            switchByRecentDate.SetBinding(Switch.IsToggledProperty, nameof(DiscountViewModel.IsSortByRecentDate));
            switchByRecentDate.SetBinding(InputTransparentProperty, nameof(DiscountViewModel.IsSortByRecentDate));

            var stackByRecentDate = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(App.IsMoreThan320Dpi ? 40 : 20, 2),
                Spacing = 0,
                Children =
                {
                    titleByRecentDate,
                    switchByRecentDate
                }
            };

            var tapGestureRecognizerByRecentDate = new TapGestureRecognizer();
            tapGestureRecognizerByRecentDate.Tapped += viewModel.SortByRecentDate_Tap;
            stackByRecentDate.GestureRecognizers.Add(tapGestureRecognizerByRecentDate);

            rightPanelContent.Children.Add(stackByRecentDate);

            #endregion

            rightPanelContent.Children.Add(new BoxView
            {
                BackgroundColor = Color.Transparent,
                HeightRequest = 30
            });

            App.RootPage.AddFilterPanelTo(rightPanelContent, viewModel);

            PanelChanged += (sender, args) =>
            {
                if (args.Panel == PanelAlignEnum.paRight && args.IsShow)
                    VerifyCurrentLocationAvailable();
            };

            RightPanel.Content = rightPanelContent;
        }

        private void BtnFilter_Click(object sender, EventArgs e)
        {
            IsShowRightPanel = !IsShowRightPanel;
        }

        private void VerifyCurrentLocationAvailable()
        {
            StackByDistance.IsVisible = LocationHelper.IsCurrentLocationAvailable;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            VerifyCurrentLocationAvailable();
        }
    }
}
