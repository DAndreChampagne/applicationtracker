namespace ApplicationTracker.Model;

public enum JobType {
    Unknown,
    FullTime,
    Contract,
}

public class Application {
    public int Id { get; set; }

    public Uri Link { get; set; }

    public int CompanyId { get; set; }
    public virtual Company Company { get; set; } = new();

    
    public string Title { get; set; } = string.Empty;
    public List<Contact> Contacts { get; set; } = new();
    public JobType Type { get; set; }
    public string Location { get; set; } = string.Empty;
    public int MatchPercent { get; set; } = 50;
    public double SalaryMin { get; set; } = 100000;
    public double SalaryMax { get; set; } = 250000;

    public DateTime? DateApplied { get; set; }
    public List<DateTime> FollowUps { get; set; } = new();

    public List<string> Notes { get; set; } = new();

}
