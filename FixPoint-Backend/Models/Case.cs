namespace FixPoint_Backend.Models;

public class Case
{
    public Guid ID { get; set; }
    public Guid TechnicianFK { get; set; }
    public Guid CustomerFK { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
    public int Status { get; set; }
    public int Priority { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ExpectedDoneDate { get; set; }
    public string? Notes { get; set; }
    
    public Case(Guid technicianID, Guid customerID, string type, string description, 
        int status, int priority, DateTime createdDate, DateTime expectedDoneDate, string? notes)
    {
        ID = Guid.NewGuid();
        TechnicianFK = technicianID;
        CustomerFK = customerID;
        Type = type;
        Description = description;
        Status = status;
        Priority = priority;
        CreatedDate = createdDate;
        ExpectedDoneDate = expectedDoneDate;
        Notes = notes;
    }

    public Case()
    {
        
    }
    
    public Guid GetID()
    {
        return ID;
    }
}