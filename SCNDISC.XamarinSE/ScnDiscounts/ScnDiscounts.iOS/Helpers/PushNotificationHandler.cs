using Foundation;
using Plugin.FirebasePushNotification;
using System.Collections.Generic;

namespace ScnDiscounts.iOS.Helpers
{
    public class PushNotificationHandler : DefaultPushNotificationHandler
    {
        public static IDictionary<string, object> GetParameters(NSDictionary data)
        {
            var result = new Dictionary<string, object>();

            var apsKey = new NSString("aps");
            var alertKey = new NSString("alert");

            foreach (var keyValuePair1 in data)
            {
                if (keyValuePair1.Key.Equals(apsKey))
                {
                    if (data.ValueForKey(apsKey) is NSDictionary nsDictionary)
                    {
                        foreach (var keyValuePair2 in nsDictionary)
                        {
                            if (keyValuePair2.Value is NSDictionary nsObject)
                            {
                                if (keyValuePair2.Key.Equals(alertKey))
                                {
                                    foreach (var keyValuePair3 in nsObject)
                                        result.Add($"aps.alert.{keyValuePair3.Key}", keyValuePair3.Value?.ToString());
                                }
                            }
                            else
                                result.Add($"aps.{keyValuePair2.Key}", keyValuePair2.Value?.ToString());
                        }
                    }
                }
                else
                    result.Add(keyValuePair1.Key?.ToString(), keyValuePair1.Value?.ToString());
            }

            return result;
        }
    }
}
