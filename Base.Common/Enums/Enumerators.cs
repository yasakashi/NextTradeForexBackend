namespace Base.Common.Enums
{
    public enum RoleList
    {
        Admin = 1,
        Merchant = 2,
        Issuer = 3,
        Investor = 4,
        Customer = 5,
        User = 6
    }

    public enum WalletActionTypeList
    {
        DepositMoneyToAccount = 1, //واریز پول به حساب
        WithdrawMoneyFromTheAccount = 2 //برداشت پول از حساب
    }

    /// <summary>
    /// انواع کیف پول
    /// </summary>
    public enum WalletTypes
    {
        /// <summary>
        /// بدهکار - فقط واریز
        /// </summary>
        Debtor = 1,
        /// <summary>
        /// بستانکار - فقط برداشت
        /// </summary>
        Creditor = 2,
        /// <summary>
        ///  هر دو - واریز و برداشت
        /// </summary>
        Both = 3 
    }

    public enum UserServiceTypeList
    {
        Good = 1, //کالا
        Service = 2 //خدمات
    }

    ///نوع شمارش کالا/خدمات یا سرویسها
    public enum UserServiceUnitTypeList
    {
        Number = 1 //عدد
    }

    public enum TransactionResponseTypeList
    {
        Succeed = 1,
        Failed = 2
    }

    public enum FactorSatus
    {
        Register = 1,
        SendToIPG = 2,
        Paied = 3,
        PaymentRejected = 4
    }
    public enum ErrorType
    {
        Fanava = 8,
        Mart = 7,
        Behzendegi = 6,
        Ayantech = 5,
        Autorize = 4,
        Hasin = 3,
        Pasargad = 2,
        DBTrasaction = 1,
        Global = 0
    }

    /// <summary>
    /// درگاه های جابجای 
    /// </summary>
    public enum SitePaymentGatewayList
    {
        /// <summary>
        /// کیف پول 
        /// </summary>
        Wallet = 1,
        /// <summary>
        /// درگاه حصین
        /// </summary>
        Hasin = 2,
        /// <summary>
        /// درگاه بانک پاسارگاد
        /// </summary>
        Pasargad = 3
    }

    /// <summary>
    /// انواع تراکش بر روی کیف پول
    /// </summary>
    public enum WalletTransactionTypes
    {
        /// <summary>
        /// برداشت
        /// </summary>
        Withdraw = 1,
        /// <summary>
        /// واریز
        /// </summary>
        Deposit = 2
    }

    /// <summary>
    /// نوع ارز های کیف پول
    /// </summary>
    public enum CurrencyTypes
    { 
        /// <summary>
        /// ریال ایران
        /// </summary>
        IRR = 1,
        /// <summary>
        /// دلار آمریکا
        /// </summary>
        USD = 2,
        /// <summary>
        /// یورو
        /// </summary>
        EUR = 3
    }
    /// <summary>
    /// نوع شخص
    /// </summary>
    public enum PersonTypes
    {
        /// <summary>
        /// شخص حقیقی
        /// </summary>
        RealPerson = 1,
        /// <summary>
        /// شخص حقوقی
        /// </summary>
        Company = 2
    }
}
