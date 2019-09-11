using Xamarin.Forms;

namespace ScnDiscounts.Control
{
    public class RatingView : StackLayout
    {
        public static readonly BindableProperty MinimumValueProperty =
            BindableProperty.Create(nameof(MinimumValue), typeof(int), typeof(RatingView), 1,
                propertyChanged: MinimumValuePropertyChanged);

        public static readonly BindableProperty MaximumValueProperty =
            BindableProperty.Create(nameof(MaximumValue), typeof(int), typeof(RatingView), 5,
                propertyChanged: MaximumValuePropertyChanged);

        public static readonly BindableProperty ValueProperty =
            BindableProperty.Create(nameof(Value), typeof(int), typeof(RatingView), default, BindingMode.TwoWay,
                propertyChanged: ValuePropertyChanged);

        public static readonly BindableProperty RatedSourceProperty =
            BindableProperty.Create(nameof(RatedSource), typeof(ImageSource), typeof(RatingView),
                propertyChanged: RatedSourcePropertyChanged);

        public static readonly BindableProperty EmptySourceProperty =
            BindableProperty.Create(nameof(EmptySource), typeof(ImageSource), typeof(RatingView),
                propertyChanged: EmptySourcePropertyChanged);

        public new static readonly BindableProperty SpacingProperty =
            BindableProperty.Create(nameof(Spacing), typeof(double), typeof(RatingView),
                propertyChanged: SpacingPropertyChanged);

        public int MinimumValue
        {
            get => (int) GetValue(MinimumValueProperty);
            set => SetValue(MinimumValueProperty, value);
        }

        public int MaximumValue
        {
            get => (int) GetValue(MaximumValueProperty);
            set => SetValue(MaximumValueProperty, value);
        }

        public int Value
        {
            get => (int) GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public ImageSource RatedSource
        {
            get => (ImageSource) GetValue(RatedSourceProperty);
            set => SetValue(RatedSourceProperty, value);
        }

        public ImageSource EmptySource
        {
            get => (ImageSource) GetValue(EmptySourceProperty);
            set => SetValue(EmptySourceProperty, value);
        }

        public new double Spacing
        {
            get => (double) GetValue(SpacingProperty);
            set => SetValue(SpacingProperty, value);
        }

        public RatingView()
        {
            Orientation = StackOrientation.Horizontal;
            base.Spacing = 0;

            InitRating();
        }

        private void InitRating()
        {
            Children.Clear();

            for (var i = MinimumValue; i <= MaximumValue; i++)
            {
                var value = i;
                var view = new ContentView
                {
                    Content = new Image()
                };

                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += (sender, args) => Value = value;
                view.GestureRecognizers.Add(tapGestureRecognizer);

                Children.Add(view);
            }
        }

        private void RedrawRating()
        {
            for (var i = MinimumValue; i <= MaximumValue; i++)
            {
                var index = i - MinimumValue;
                var view = (ContentView) Children[index];
                var image = (Image) view.Content;
                image.Source = i <= Value ? RatedSource : EmptySource;
            }
        }

        private void UpdateSpacing()
        {
            var padding = Spacing / 2;

            for (var i = MinimumValue; i <= MaximumValue; i++)
            {
                var index = i - MinimumValue;
                var view = (ContentView) Children[index];
                view.Padding = new Thickness(i == MinimumValue ? 0 : padding, 0, i == MaximumValue ? 0 : padding, 0);
            }
        }

        private static void MinimumValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RatingView ratingView)
                ratingView.InitRating();
        }

        private static void MaximumValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RatingView ratingView)
                ratingView.InitRating();
        }

        private static void ValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RatingView ratingView)
                ratingView.RedrawRating();
        }

        private static void RatedSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RatingView ratingView)
                ratingView.RedrawRating();
        }

        private static void EmptySourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RatingView ratingView)
                ratingView.RedrawRating();
        }

        private static void SpacingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RatingView ratingView)
                ratingView.UpdateSpacing();
        }
    }
}