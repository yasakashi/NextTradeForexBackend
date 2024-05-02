using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Common.Enums
{
    public static class CanPayInfo
    {
        public static string Username { get { return "pod@pep.co.ir"; } }
        public static string Password { get { return "Mmpod@pep.co.ir2020"; } }
        public static string apikey { get { return "pod@fanap2020"; } }

        public static List<int> MCICerdit = new List<int>() { 10000, 20000, 50000, 100000, 200000, 500000 };

        public static List<int> MCI_ChargePriceList = new List<int>() { 10000, 20000, 50000, 100000, 200000, 500000 };
        public static List<int> MTN_WandefulTypeChargePriceList = new List<int>() { 50000, 100000, 200000};
        public static List<int> Ritel_WandefulTypeChargePriceList = new List<int>() {  20000, 50000, 100000, 200000, 500000 };
    }
    public enum CanPayInfo_MCI_ChargeType
    {
        /// <summary>
        /// عادی
        /// </summary>
        Normal = 0,
        /// <summary>
        /// طرح جوانان
        /// </summary>
        Young = 1,
        /// <summary>
        /// طرح بانوان
        /// </summary>
        Women = 2
    }

    public enum CanPayInfo_MTN_ChargeType
    {
        /// <summary>
        /// عادی
        /// </summary>
        Normal = 0,
        /// <summary>
        /// شگفت انگیز
        /// </summary>
        Wonderful = 1
    }

    public enum CanPayInfo_MTN_SimType
    {
        /// <summary>
        /// سیم کارت های اعتباری
        /// </summary>
        CriditSim = 0,
        /// <summary>
        /// سیم کارت دائمی
        /// </summary>
        PermanentSim = 1
    }

    public enum CanPayInfo_RITEL_ChargeType
    {
        /// <summary>
        /// عادی
        /// </summary>
        Normal = 0,
        /// <summary>
        /// شگفت انگیز
        /// </summary>
        Wonderful = 1
    }

    public enum CanPayInfo_OperatorList
    {
        MCI = 1,
        MTN = 2,
        Ritel =3
    }

    public enum CanPayInfo_MCI_Chargepackage_SimType
    {
        /// <summary>
        /// سیم کارت اعتباری
        /// </summary>
        Credit=1,
        /// <summary>
        /// سیم کارت دیتا
        /// </summary>
        Data = 2,
        /// <summary>
        /// سیم کارت دائمی
        /// </summary>
        Perminent = 3,
        /// <summary>
        /// اعتباری TD-LTE
        /// </summary>
        Credit_TDLTD = 4,
        /// <summary>
        /// دائمی TD-LTE
        /// </summary>
        Perminent_TDLTD = 5
    }
}
