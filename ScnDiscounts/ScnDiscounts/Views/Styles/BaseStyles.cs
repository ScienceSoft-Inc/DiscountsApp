using Xamarin.Forms;

namespace ScnDiscounts.Views.Styles
{
	public class BaseStyles
	{
        public BaseStyles()
		{
			if (App.Current.Resources == null) 
            {
				App.Current.Resources = new ResourceDictionary ();
			}
			
			Resources = App.Current.Resources;

            Load();
		}

		protected ResourceDictionary Resources { get; private set; }

        public virtual void Load() {}
	}
}

