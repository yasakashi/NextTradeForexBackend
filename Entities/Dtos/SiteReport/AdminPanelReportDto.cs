using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos;

public class AdminPanelReportDto
{
    public decimal? pendingGroupApprovals { get; set; }
    public int? pendingTickets { get; set; }
    public decimal? pendingWithdrawalApprovals { get; set; }
    public decimal? pendingEmailActivation { get; set; }
    public decimal? pendingCourseApprovals { get; set; }
    public decimal? pendingChannelApprovals { get; set; }
    public decimal? companyIncom { get; set; }
    public decimal? royaltyIncom { get; set; }
    public decimal? dailyActivations { get; set; }
    public decimal? dailyTurnover { get; set; }
    public decimal? dailywithdrawals { get; set; }
    public decimal? dailyWithdrawalAmount { get; set; }
    public decimal? weeklyActivations { get; set; }
    public decimal? weeklyTurnover { get; set; }
    public decimal? weeklywithdrawals { get; set; }
    public decimal? weeklyWithdrawalAmount { get; set; }
    public decimal? monthlyActivations { get; set; }
    public decimal? monthlyTurnover { get; set; }
    public decimal? monthlywithdrawals { get; set; }
    public decimal? monthlyWithdrawalAmount { get; set; }
    public int? totalRegisteredUsers { get; set; }
    public int? activeUsers { get; set; }
    public int? inactiveUsers { get; set; }

}
