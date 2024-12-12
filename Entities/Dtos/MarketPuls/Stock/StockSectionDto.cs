using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos;
public class StockSectionDto
{
    public string? newstickernew { get; set; }
    public string? newstickerupdate { get; set; }
    public string? newstickerimportant { get; set; }
    public string? established { get; set; }
    public string? exchange { get; set; }
    public string? companytype { get; set; }
    public string? ownership { get; set; }
    public string? mainofficecountr { get; set; }
    public string? url { get; set; }
    public string? totalbranches { get; set; }
    public string? otherimportantlocation { get; set; }
    public string? overalllocations { get; set; }
    public string? servicesoffered { get; set; }
    public string? marketfocus { get; set; }
    public string? briefdescriptionofcompany { get; set; }
    public List<StockManagementTeamDto>? managementteam { get; set; }
    public string? importantresearchnotes { get; set; }
    public string? chart { get; set; }
    public StockFinancialDataDto? financialdata_estturnoverus { get; set; }
    public StockFinancialDataDto? financialdata_estgrossprofit { get; set; }
    public StockFinancialDataDto? financialdata_estnetprofit { get; set; }
    public StockCurrentFinancialYearDto? currentfinancial_estturnoverus { get; set; }
    public StockCurrentFinancialYearDto? currentfinancial_estgrossprofit { get; set; }
    public StockCurrentFinancialYearDto? currentfinancial_estnetprofit { get; set; }
    public StockFinancialRatiosDto? workingcapotalratio { get; set; }
    public StockFinancialRatiosDto? quickratio { get; set; }
    public StockFinancialRatiosDto? earningpershareratio { get; set; }
    public StockFinancialRatiosDto? priceearninsratio { get; set; }
    public StockFinancialRatiosDto? earningpersdebttoequityratio { get; set; }
    public StockFinancialRatiosDto? returnonequityratio { get; set; }
    public string? briefdescriptionofratio { get; set; }
    public List<StockProductAndServiceDto>? productsservices { get; set; }
    public List<StockInductryFocusDto>? industryfocuslist { get; set; }
}

public class StockFinancialDataDto
{
    public string? year1 { get; set; }
    public string? year2 { get; set; }
    public string? year3 { get; set; }
    public string? year4 { get; set; }
    public string? year5 { get; set; }
}
public class StockCurrentFinancialYearDto
{
    public string? Q1 { get; set; }
    public string? Q2 { get; set; }
    public string? Q3 { get; set; }
    public string? Q4 { get; set; }
}
public class StockFinancialRatiosDto
{
    public string? ratio { get; set; }
    public bool? analysis_isgood { get; set; }
}