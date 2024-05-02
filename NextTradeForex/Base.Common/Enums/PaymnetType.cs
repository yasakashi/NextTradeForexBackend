namespace Base.Common.Enums
{
    public enum PaymentType
    {
        /// <summary>
        ///شارژ کیف پول
        /// </summary>
        WalletCharge = 1,
        /// <summary>
        /// پرداخت قبض
        /// </summary>
        BillPayment = 2,
        /// <summary>
        ///شارژ  موبایل
        /// </summary>
        MobileCharge = 3,
        /// <summary>
        /// انتقال موجوی بین کیف پول
        /// </summary>
        TransferWallet = 4,
        /// <summary>
        ///  خرید محصول
        /// </summary>
        BuyProduct = 5,
        /// <summary>
        ///  شارژ کیف پول بعد از درگاه پرداخت
        /// </summary>
        ChargingWalletAfterIPG = 6,
        /// <summary>
        ///  شارژ موبایل با شارز مارت
        /// </summary>
        MartMobileCharge = 31,
        /// <summary>
        ///  شارژ موبایل با شارز مارت با واسط درگاه پرداخت
        /// </summary>
        MartMobileChargeByIPG = 32,
        /// <summary>
        ///  خرید بسته های اینترنتی با شارز مارت با واسط درگاه پرداخت
        /// </summary>
        MartInternetPackageByIPG = 33,
        /// <summary>
        ///  استعلام قبض
        /// </summary>
        CheckBill = 21,
        /// <summary>
        ///  استعلام خلافی خودرو
        /// </summary>
        CheckDrivingOffense = 22,
        /// <summary>
        /// استعلام کدمیلی با موبایل
        /// </summary>
        CheckUserMobileNationalCode = 23,
        /// <summary>
        ///  واریز به حساب
        /// </summary>
        ChashOut = 100,

        /// <summary>
        ///  استعلام های بانکی
        /// </summary>
        CheckBankService = 50,
        /// <summary>
        ///  خرید یا فروش ارز دیجیتال
        /// </summary>
        DigitalCoin = 70
    }
}