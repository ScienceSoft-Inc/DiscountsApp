using System;
using Xamarin.Forms;

namespace ScnDiscounts.Control
{
    public class BorderBox : RelativeLayout
    {
        public enum BorderTypeEnum { btNone, btBound, btLabel }

        public BorderBox(BorderTypeEnum borderType = BorderTypeEnum.btBound)
        {
            VerticalOptions = LayoutOptions.Center;
            HorizontalOptions = LayoutOptions.Center;
            BorderType = borderType;

            BorderLeft = new BoxView
            {
                BackgroundColor = BorderColor
            };

            BorderTop = new BoxView
            {
                BackgroundColor = BorderColor
            };

            BorderRight = new BoxView
            {
                BackgroundColor = BorderColor
            };

            BorderBottom = new BoxView
            {
                BackgroundColor = BorderColor
            };

            BorderBottomLeftPart = new BoxView
            {
                BackgroundColor = BorderColor
            };

            BorderBottomRightPart = new BoxView
            {
                BackgroundColor = BorderColor
            };

            BorderLabelLeftPart = new BoxView
            {
                BackgroundColor = BorderColor
            };

            BorderLabelRightPart = new BoxView
            {
                BackgroundColor = BorderColor
            };

            BoxLabel = new BoxView();
        }

        private Color _borderColor = Color.Transparent;

        public Color BorderColor
        {
            get => _borderColor;
            set 
            { 
                _borderColor = value;

                BorderLeft.BackgroundColor = _borderColor;
                BorderTop.BackgroundColor = _borderColor;
                BorderRight.BackgroundColor = _borderColor;
                BorderBottom.BackgroundColor = _borderColor;
                BorderBottomLeftPart.BackgroundColor = _borderColor;
                BorderBottomRightPart.BackgroundColor = _borderColor;
                BorderLabelLeftPart.BackgroundColor = _borderColor;
                BorderLabelRightPart.BackgroundColor = _borderColor;
            }
        }

        public int BorderWidth { get; set; }

        public BorderTypeEnum BorderType { get; }

        #region TagProperty

        public static readonly BindableProperty TagProperty =
            BindableProperty.Create(nameof(Tag), typeof(string), typeof(BorderBox), default(string));

        public string Tag
        {
            get => (string)GetValue(TagProperty);
            set => SetValue(TagProperty, value);
        }

        #endregion

        private TapGestureRecognizer _tapGesture;
        public TapGestureRecognizer TapGesture
        {
            get => _tapGesture;
            set 
            { 
                _tapGesture = value;
                _content.GestureRecognizers.Clear();
                _content.GestureRecognizers.Add(_tapGesture);
            }
        }

        private BoxView BorderLeft { get; }
        private BoxView BorderTop { get; }
        private BoxView BorderRight { get; }
        private BoxView BorderBottom { get; }
        private BoxView BorderBottomLeftPart { get; }
        private BoxView BorderBottomRightPart { get; }
        private BoxView BorderLabelLeftPart { get; }
        private BoxView BorderLabelRightPart { get; }
        private BoxView BoxLabel { get; }

        private View _content;

        public View Content
        {
            set
            {
                Children.Clear();

                if (BorderWidth > 0)
                {
                    if (BorderType != BorderTypeEnum.btNone)
                    {
                        Children.Add(BorderLeft, Constraint.Constant(0), Constraint.Constant(0),
                            Constraint.Constant(BorderWidth), Constraint.RelativeToParent(parent => parent.Height));

                        Children.Add(BorderTop, Constraint.Constant(0), Constraint.Constant(0),
                            Constraint.RelativeToParent(parent => parent.Width), Constraint.Constant(BorderWidth));

                        Children.Add(BorderRight, Constraint.RelativeToParent(parent => parent.Width - BorderWidth),
                            Constraint.Constant(0), Constraint.Constant(BorderWidth),
                            Constraint.RelativeToParent(parent => parent.Height));

                        if (BorderType == BorderTypeEnum.btBound)
                        {
                            Children.Add(BorderBottom, Constraint.Constant(0),
                                Constraint.RelativeToParent(parent => parent.Height - BorderWidth),
                                Constraint.RelativeToParent(parent => parent.Width), Constraint.Constant(BorderWidth));
                        }
                        else if (BorderType == BorderTypeEnum.btLabel)
                        {
                            const double labelWidth = 20;
                            const double smoothValue = 0.5;

                            var boxLabelWidth = Math.Sqrt(labelWidth * labelWidth / 2);

                            Children.Add(BorderBottomLeftPart, Constraint.Constant(0),
                                Constraint.RelativeToParent(parent => parent.Height - BorderWidth),
                                Constraint.RelativeToParent(parent => (parent.Width - labelWidth) / 2 + smoothValue),
                                Constraint.Constant(BorderWidth));

                            Children.Add(BorderBottomRightPart,
                                Constraint.RelativeToParent(parent => (parent.Width + labelWidth) / 2 - smoothValue),
                                Constraint.RelativeToParent(parent => parent.Height - BorderWidth),
                                Constraint.RelativeToParent(parent => (parent.Width - labelWidth) / 2 + smoothValue),
                                Constraint.Constant(BorderWidth));

                            Children.Add(BoxLabel,
                                Constraint.RelativeToParent(parent => (parent.Width - boxLabelWidth) / 2),
                                Constraint.RelativeToParent(parent => parent.Height - boxLabelWidth / 2 - BorderWidth),
                                Constraint.Constant(boxLabelWidth), Constraint.Constant(boxLabelWidth));
                            BoxLabel.Rotation = 45;
                            BoxLabel.BackgroundColor = value.BackgroundColor;
                            BoxLabel.Opacity = value.Opacity;

                            Children.Add(BorderLabelLeftPart,
                                Constraint.RelativeToParent(parent => (parent.Width - labelWidth) / 2),
                                Constraint.RelativeToParent(parent => parent.Height - BorderWidth),
                                Constraint.Constant(boxLabelWidth + smoothValue), Constraint.Constant(BorderWidth));
                            BorderLabelLeftPart.AnchorX = 0;
                            BorderLabelLeftPart.Rotation = 45;

                            Children.Add(BorderLabelRightPart,
                                Constraint.RelativeToParent(parent => (parent.Width + labelWidth) / 2 - boxLabelWidth),
                                Constraint.RelativeToParent(parent => parent.Height - BorderWidth),
                                Constraint.Constant(boxLabelWidth), Constraint.Constant(BorderWidth));
                            BorderLabelRightPart.AnchorX = 1;
                            BorderLabelRightPart.Rotation = -45;

                        }
                    }
                }

                Children.Add(value, Constraint.Constant(BorderWidth), Constraint.Constant(BorderWidth),
                    Constraint.RelativeToParent(parent => parent.Width - BorderWidth * 2),
                    Constraint.RelativeToParent(parent => parent.Height - BorderWidth * 2));

                if (TapGesture != null)
                    value.GestureRecognizers.Add(TapGesture);

                _content = value;
            }
        }
    }
}
