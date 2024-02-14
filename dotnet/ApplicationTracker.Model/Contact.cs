namespace ApplicationTracker.Model;

public class Contact {
    public int Id { get; set; }


    public string Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public List<string> Notes { get; set; } = new();
    
}