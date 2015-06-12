using Xamarin.Forms;

namespace ScnDiscounts.Control
{
    public class LabelEx : RelativeLayout
    {
        public enum Labelkind 
        {
            lkLink
        };

        public LabelEx(Labelkind kind)
        {
            labelf = new Label();

            SetExtensions(kind);
        }

        public LabelEx(Label label, Labelkind kind)
        {
            labelf = label;
            
            SetExtensions(kind);
        }

        public string TextLabel
        {
            get { return _labelEx.Text; }
            set { _labelEx.Text = value; }
        }

        private Label _labelEx;
        private StackLayout labelStack;
        private Label labelf
        {
            get { return _labelEx; }
            set
            {
                _labelEx = value;
                
                labelStack = new StackLayout
                {
                    Children = { _labelEx }
                };

                Children.Add(labelStack,
                    Constraint.Constant(0),
                    Constraint.Constant(0));
            }
        }

        private void SetExtensions(Labelkind kind)
        {
            if (kind == Labelkind.lkLink)
            {
                var underLine = new BoxView
                {
                    Color = labelf.TextColor
                };
                _labelEx.PropertyChanged += (s, e) => { underLine.Color = labelf.TextColor; };

                Children.Add(underLine,
                  Constraint.RelativeToView(labelStack, (parent, sibling) =>
                  {
                      return sibling.X;
                  }),
                  Constraint.RelativeToView(labelStack, (parent, sibling) =>
                  {
                      return sibling.Y + sibling.Height;
                  }),
                  Constraint.RelativeToView(labelStack, (parent, sibling) =>
                  {
                      return sibling.X + sibling.Width;
                  }),
                  Constraint.RelativeToView(labelStack, (parent, sibling) =>
                  {
                       return sibling.Y + 2;
                  }));
            }
        }
    }
}
