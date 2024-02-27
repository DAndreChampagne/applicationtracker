using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ApplicationTracker.Model;

public enum JobType {

    Unknown,

    [Display(Name="Full Time")]
    FullTime,

    [Display(Name="Contract")]
    Contract,
}

public enum ApplicationStatus {
    Considering,
    Applied,
    Pending,
    Declined,
    Accepted,

    [Display(Name="Rejected, no interview")]
    RejectedWithoutInterview,

    [Display(Name="Rejected, after interview")]
    RejectedAfterInterview,
}

public class Application {
    public int Id { get; set; }

    public Uri Link { get; set; }

    public int CompanyId { get; set; }
    public virtual Company? Company { get; set; }

    public int ContactId { get; set; }
    public virtual Contact? Contact { get; set; }
    
    public string Title { get; set; } = string.Empty;
    
    public JobType Type { get; set; }

    public string Location { get; set; } = string.Empty;

    [Display(Name="Req. Match %")]
    public int MatchPercent { get; set; } = 50;
    
    [Display(Name="Min Salary")]
    public double SalaryMin { get; set; } = 100000;
    
    [Display(Name="Max Salary")]
    public double SalaryMax { get; set; } = 250000;

    [Display(Name="Application Status")]
    public ApplicationStatus Status { get; set; } = ApplicationStatus.Applied;

    [Display(Name="Status Reason")]
    public string? ApplicationStatusReason { get; set; }

    [Display(Name="Date Applied")]
    public DateTime? DateApplied { get; set; }

    [UIHint("DateList")]
    [Display(Name="Follow up Dates")]
    public List<DateTime> FollowUps { get; set; } = new();

    [UIHint("StringList")]
    public List<string> Notes { get; set; } = new();

}
