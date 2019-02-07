using System.Linq;
using System.Web.UI.WebControls;

namespace SCNDISC.Web.Admin.ServiceLayer.Extensions
{
    public static class UIExtensions
    {
        public static string[] GetSelectedValues(this CheckBoxList list)
        {
            return list.Items.Cast<ListItem>().Where(i => i.Selected).Select(i => i.Value).ToArray();
        }

        public static void SetSelectedValues(this CheckBoxList checkboxes, string[] values)
        {
            foreach (ListItem item in checkboxes.Items)
            {
                item.Selected = values.Contains(item.Value);
            }
        }

        public static void SelectAll(this CheckBoxList checkboxes)
        {
            foreach (ListItem item in checkboxes.Items)
            {
                item.Selected = true;
            }
        }

        public static bool CheckIfSelectedAll(this CheckBoxList checkboxes)
        {
            foreach (ListItem item in checkboxes.Items)
            {
                if (!item.Selected) return false;
            }

            return true;
        }
    }
}