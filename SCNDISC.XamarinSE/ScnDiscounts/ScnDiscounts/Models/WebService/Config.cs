namespace ScnDiscounts.Models.WebService
{
    public static class Config
    {
        public const string ServerAddress =
#if DEBUG
            @"https://localhost/SCNDISC.Dev.Server/";
#else
            @"https://localhost/SCNDISC.Server/";
#endif
    }
}
