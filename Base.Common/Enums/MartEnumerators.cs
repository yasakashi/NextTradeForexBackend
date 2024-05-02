namespace Base.Common.Enums
{
    public class MartEnumerators
    {
        public static string LoginUrl { get { return "https://sharjmart.ir/Api/Login"; } }
        public static string GetDepositUrl { get { return "https://sharjmart.ir/Api/GetDepositRemaining"; } }
        public static string RegisterTopUpUrl { get { return "https://sharjmart.ir/Api/RegisterTopUp"; } }
        public static string StatusInquiryByOrderIdUrl { get { return "https://sharjmart.ir/Api/StatusInquiryByOrderId"; } }
        public static string StatusInquiryByOperatorRefNumUrl { get { return "https://sharjmart.ir/Api/StatusInquiryByOperatorRefNum"; } }
        public static string GetInternetPackageUrl { get { return "https://sharjmart.ir/Api/GetInternetPackages"; } }
        public static string GetMTNOfferPackageCategoryUrl { get { return "https://sharjmart.ir/Api/GetMTNOfferPackagesCategory"; } }
        public static string GetMTNOfferPackageUrl { get { return "https://sharjmart.ir/Api/GetMTNOfferPackages"; } }
    }
}
