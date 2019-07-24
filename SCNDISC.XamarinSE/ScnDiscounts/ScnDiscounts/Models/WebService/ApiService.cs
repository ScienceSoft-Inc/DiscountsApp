namespace ScnDiscounts.Models.WebService
{
    public static class ApiService
    {
        public const string ServerAddress =
#if DEBUG
            @"https://localhost/SCNDISC.Dev.Server";
#else
            @"https://localhost/SCNDISC.Server";
#endif

        private const string RequestPartnerLogo = "/discounts/{0}/logo";

        public static string GetPartnerLogoUrl(string documentId)
        {
            return $"{ServerAddress}{string.Format(RequestPartnerLogo, documentId)}";
        }
    }
}
