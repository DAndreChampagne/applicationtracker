namespace ApplicationTracker.Model;

public class Company {

    public int Id { get; set; }

    public string Name { get; set; }

    public int ContactId { get; set; }
    public Contact? Contact { get; set; }

}