using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.Enums
{
    public static class ReferralPercents
    {
        public static  decimal GetLevelPercent(int level)
        {
            decimal LevelPercent = 0;
            switch (level)
            {
                case 1:
                    LevelPercent = (decimal)0.08;
                    break;
                case 2:
                    LevelPercent = (decimal)0.04;
                    break;
                case 3:
                    LevelPercent = (decimal)0.03;
                    break;
                case 4:
                    LevelPercent = (decimal)0.03;
                    break;
                case 5:
                    LevelPercent = (decimal)0.02;
                    break;
            };
            return LevelPercent;
        }

        public static decimal GetIndicatorPercent()
        {
            decimal LevelPercent =(decimal) 0.65;
            return LevelPercent;
        }
        public static decimal GetSitePercent()
        {
            decimal LevelPercent = (decimal)0.1;
            return LevelPercent;
        }
    }
}
