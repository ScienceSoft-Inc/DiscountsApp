using Xamarin.Forms;

namespace ScnDiscounts.Views.Styles
{
	public class BaseStyles
	{
	    public BaseStyles()
		{
		    if (Application.Current.Resources == null)
		        Application.Current.Resources = new ResourceDictionary();

		    Resources = Application.Current.Resources;
		}

		protected ResourceDictionary Resources { get; }

	    public virtual void Load()
	    {
	    }
	}
}

