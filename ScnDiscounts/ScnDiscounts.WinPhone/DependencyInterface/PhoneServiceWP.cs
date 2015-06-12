using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ScnDiscounts.DependencyInterface;
using ScnDiscounts.WinPhone.DependencyInterface;

[assembly: Dependency(typeof(PhoneServiceWP))]

namespace ScnDiscounts.WinPhone.DependencyInterface
{
    public class PhoneServiceWP : IPhoneService
    {
        public void DialNumber(string number, string name = "")
        {
            new PhoneCallTask { PhoneNumber = number, DisplayName = name }.Show();
        }
    }
}
