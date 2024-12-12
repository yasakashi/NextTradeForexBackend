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
    public List<Stock_ManagementTeamDto>? managementteam { get; set; }
    public string? importantresearchnotes { get; set; }
    public string? chart { get; set; }
    public FinancialDataDto? financialdata_estturnoverus { get; set; }
    public FinancialDataDto? financialdata_estgrossprofit { get; set; }
    public FinancialDataDto? financialdata_estnetprofit { get; set; }
    public CurrentFinancialYearDto? currentfinancial_estturnoverus { get; set; }
    public CurrentFinancialYearDto? currentfinancial_estgrossprofit { get; set; }
    public CurrentFinancialYearDto? currentfinancial_estnetprofit { get; set; }
    public FinancialRatiosDto? workingcapotalratio { get; set; }
    public FinancialRatiosDto? quickratio { get; set; }
    public FinancialRatiosDto? earningpershareratio { get; set; }
    public FinancialRatiosDto? priceearninsratio { get; set; }
    public FinancialRatiosDto? earningpersdebttoequityratio { get; set; }
    public FinancialRatiosDto? returnonequityratio { get; set; }
    public string? briefdescriptionofratio { get; set; }
    public List<ProductAndServiceDto>? productsservices { get; set; }
}

public class FinancialDataDto
{
    public string? year1 { get; set; }
    public string? year2 { get; set; }
    public string? year3 { get; set; }
    public string? year4 { get; set; }
    public string? year5 { get; set; }
}
public class CurrentFinancialYearDto
{
    public string? Q1 { get; set; }
    public string? Q2 { get; set; }
    public string? Q3 { get; set; }
    public string? Q4 { get; set; }
}
public class FinancialRatiosDto
{
    public string? ratio { get; set; }
    public bool? analysis_isgood { get; set; }
}