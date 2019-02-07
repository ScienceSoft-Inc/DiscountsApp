using Microsoft.AppCenter.Crashes;
using System;
using System.Diagnostics;

namespace ScnDiscounts.Helpers
{
    public static class LoggerHelper
    {
        public static void WriteException(Exception ex)
        {
            Crashes.TrackError(ex);
            Debug.WriteLine(ex);
        }
    }
}
