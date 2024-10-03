using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities.MarketPuls
{
    [Table("tblMarketPuls_Forex")]
    public class forex
    {
        (<id, uniqueidentifier,>
           ,<categoryid, bigint,>
           ,<createdatetime, datetime,>
           ,<creatoruserid, bigint,>
           ,<price, decimal (18,2),>
           ,<isvisible, bit,>
           ,<courseleveltypeId, int,>
           ,<coursetitle, nvarchar(3000),>
           ,<oneyeardescription, ntext,>
           ,<chartdescription, ntext,>
           ,<firstcountryheading, nvarchar(2000),>
           ,<firstcountrydescription, ntext,>
           ,<secondcountryheading, nvarchar(2000),>
           ,<secondcountrydescription, ntext,>
           ,<bottomdescription, ntext,>
           ,<maindescription, ntext,>
           ,<singlepagechartimage, nvarchar(3000),>
           ,<instrumentname, nvarchar(3000),>
           ,<fundamentalheading, nvarchar(3000),>
           ,<technicalheading, nvarchar(3000),>
           ,<marketsessiontitle, nvarchar(3000),>
           ,<marketsessionscript, ntext,>
           ,<marketsentimentstitle, nvarchar(3000),>
           ,<marketsentimentsscript, ntext,>
           ,<privatenotes, ntext,>
           ,<excerpt, ntext,>
           ,<author, nvarchar(500),>)
    }

    [Table("tblMarketPuls_Forex_SecondCountryDatas")]
    public class a
    {
        <id, uniqueidentifier,>
           ,<marketpulsforexid, uniqueidentifier,>
           ,<countries, nvarchar(700),>
           ,<centralbank, nvarchar(700),>
           ,<nickname, nvarchar(700),>
           ,<avragedaily, nvarchar(700),>
    }

    [Table("tblMarketPuls_Forex_FlexibleBlocks")]
    public class b
    { 
    <id, uniqueidentifier,>
           ,<marketpulsforexid, uniqueidentifier,>
           ,<MainTitle, nvarchar(700),>
           ,<countries, nvarchar(700),>
           ,<pairsthatcorrelate, nvarchar(700),>
           ,<highslows, nvarchar(700),>
           ,<pairtype, nvarchar(700),>
           ,<dailyavrage, nvarchar(700),>}
}

[Table("tblMarketPuls_Forex_FirstCountryDatas")]
public class b
{ 
<id, uniqueidentifier,>
           ,<marketpulsforexid, uniqueidentifier,>
           ,<countries, nvarchar(700),>
           ,<centralbank, nvarchar(700),>
           ,<nickname, nvarchar(700),>
           ,<avragedaily, nvarchar(700),>}
