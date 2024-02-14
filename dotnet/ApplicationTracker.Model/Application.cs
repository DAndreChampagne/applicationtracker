namespace ApplicationTracker.Model;

public enum JobType {
    Unknown,
    FullTime,
    Contract,
}

public enum ApplicationStatus {
    Considering,
    Applied,
    Pending,
    Declined,
    RejectedWithoutInterview,
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
    public int MatchPercent { get; set; } = 50;
    public double SalaryMin { get; set; } = 100000;
    public double SalaryMax { get; set; } = 250000;


    public ApplicationStatus Status { get; set; } = ApplicationStatus.Applied;
    public string? ApplicationStatusReason { get; set; }

    public DateTime? DateApplied { get; set; }
    public List<DateTime> FollowUps { get; set; } = new();

    public List<string> Notes { get; set; } = new();

}
