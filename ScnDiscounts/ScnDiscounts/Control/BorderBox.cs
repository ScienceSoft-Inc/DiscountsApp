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

            borderLeft = new BoxView
            {
                BackgroundColor = BorderColor
            };

            borderTop = new BoxView
            {
                BackgroundColor = BorderColor
            };

            borderRight = new BoxView
            {
                BackgroundColor = BorderColor
            };

            borderBottom = new BoxView
            {
                BackgroundColor = BorderColor
            };

            borderBottomLeftPart = new BoxView
            {
                BackgroundColor = BorderColor
            };

            borderBottomRightPart = new BoxView
            {
                BackgroundColor = BorderColor
            };

            borderLabelLeftPart = new BoxView
            {
                BackgroundColor = BorderColor
            };

            borderLabelRightPart = new BoxView
            {
                BackgroundColor = BorderColor
            };

            boxLabel = new BoxView();
        }

        public Color _borderColor = Color.Transparent;
        public Color BorderColor
        {
            get { return _borderColor; }
            set 
            { 
                _borderColor = value;
                borderLeft.BackgroundColor = _borderColor;
                borderTop.BackgroundColor = _borderColor;
                borderRight.BackgroundColor = _borderColor;
                borderBottom.BackgroundColor = _borderColor;
                borderBottomLeftPart.BackgroundColor = _borderColor;
                borderBottomRightPart.BackgroundColor = _borderColor;
                borderLabelLeftPart.BackgroundColor = _borderColor;
                borderLabelRightPart.BackgroundColor = _borderColor;
            }
        }
        
        public int _borderWidth;
        public int BorderWidth 
        { 
            get { return _borderWidth; }
            set { _borderWidth = value; } 
        }

        private BorderTypeEnum BorderType;

        #region TagProperty
        public static readonly BindableProperty TagProperty =
            BindableProperty.Create<BorderBox, string>(p => p.Tag, "");

        public string Tag
        {
            get { return (string)GetValue(TagProperty); }
            set { SetValue(TagProperty, value); }
        }
        #endregion

        private TapGestureRecognizer _tapGesture;
        public TapGestureRecognizer TapGesture
        {
            get { return _tapGesture; }
            set 
            { 
                _tapGesture = value;
                _content.GestureRecognizers.Clear();
                _content.GestureRecognizers.Add(_tapGesture);
            }
        }

        private AbsoluteLayout baseLayout { get; set; }
        private BoxView borderLeft { get; set; }
        private BoxView borderTop { get; set; }
        private BoxView borderRight { get; set; }
        private BoxView borderBottom { get; set; }
        private BoxView borderBottomLeftPart { get; set; }
        private BoxView borderBottomRightPart { get; set; }
        private BoxView borderLabelLeftPart { get; set; }
        private BoxView borderLabelRightPart { get; set; }
        private BoxView boxLabel { get; set; }

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
                        /*AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.All);
                        AbsoluteLayout.SetLayoutBounds(this, new Rectangle(0f, 0f, 1f, 1f));
                        this.Children.Add(this);*/

                        Children.Add(borderLeft, Constraint.Constant(0), Constraint.Constant(0), Constraint.Constant(BorderWidth), Constraint.RelativeToParent(parent => { return parent.Height; }));

                        Children.Add(borderTop, Constraint.Constant(0), Constraint.Constant(0), Constraint.RelativeToParent(parent => { return parent.Width; }), Constraint.Constant(BorderWidth));

                        Children.Add(borderRight, Constraint.RelativeToParent(parent => { return parent.Width - BorderWidth; }), Constraint.Constant(0), Constraint.Constant(BorderWidth), Constraint.RelativeToParent(parent => { return parent.Height; }));

                        if (BorderType == BorderTypeEnum.btBound)
                        {
                            Children.Add(borderBottom, Constraint.Constant(0), Constraint.RelativeToParent(parent => { return parent.Height - BorderWidth; }), Constraint.RelativeToParent(parent => { return parent.Width; }), Constraint.Constant(BorderWidth));
                        } 
                        else if (BorderType == BorderTypeEnum.btLabel)
                        {
                            double labelWidth = 20;
                            double boxLabelWidth = Math.Sqrt((labelWidth * labelWidth)/2);
                            double smoothValue = 0.5;

                            Children.Add(borderBottomLeftPart, Constraint.Constant(0), Constraint.RelativeToParent(parent => { return parent.Height - BorderWidth; }), Constraint.RelativeToParent(parent => { return (parent.Width - labelWidth) / 2 + smoothValue; }), Constraint.Constant(BorderWidth));

                            Children.Add(borderBottomRightPart, Constraint.RelativeToParent(parent => { return (parent.Width + labelWidth) / 2 - smoothValue; }), Constraint.RelativeToParent(parent => { return parent.Height - BorderWidth; }), Constraint.RelativeToParent(parent => { return (parent.Width - labelWidth) / 2 + smoothValue; }), Constraint.Constant(BorderWidth));

                            Children.Add(boxLabel, Constraint.RelativeToParent(parent => { return (parent.Width - boxLabelWidth) / 2; }), Constraint.RelativeToParent(parent => { return (parent.Height - boxLabelWidth / 2) - BorderWidth; }), Constraint.Constant(boxLabelWidth), Constraint.Constant(boxLabelWidth));
                            boxLabel.Rotation = 45;
                            boxLabel.BackgroundColor = value.BackgroundColor;
                            boxLabel.Opacity = value.Opacity;

                            Children.Add(borderLabelLeftPart, Constraint.RelativeToParent(parent => { return (parent.Width - labelWidth) / 2; }), Constraint.RelativeToParent(parent => { return parent.Height - BorderWidth; }), Constraint.Constant(boxLabelWidth + smoothValue), Constraint.Constant(BorderWidth));
                            borderLabelLeftPart.AnchorX = 0;
                            borderLabelLeftPart.Rotation = 45;

                            Children.Add(borderLabelRightPart, Constraint.RelativeToParent(parent => { return ((parent.Width + labelWidth) / 2) - boxLabelWidth; }), Constraint.RelativeToParent(parent => { return parent.Height - BorderWidth; }), Constraint.Constant(boxLabelWidth), Constraint.Constant(BorderWidth));
                            borderLabelRightPart.AnchorX = 1;
                            borderLabelRightPart.Rotation = -45;

                        }
                    }
                }

                Children.Add(value, Constraint.Constant(BorderWidth), Constraint.Constant(BorderWidth), Constraint.RelativeToParent(parent => { return parent.Width - BorderWidth * 2; }), Constraint.RelativeToParent(parent => { return parent.Height - BorderWidth * 2; }));

                if (TapGesture != null)
                    value.GestureRecognizers.Add(TapGesture);
                
                _content = value;
            }
        }


    }
}
