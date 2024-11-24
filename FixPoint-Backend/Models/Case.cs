namespace FixPoint_Backend.Models;

public class Case
{
    public Guid ID { get; set; }
    public Guid TechnicianID { get; set; }
    public Guid CustomerID { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
    public int Status { get; set; }
    public int Priority { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime ExpectedDoneDate { get; set; }
    public string? Notes { get; set; }
    
    public Case(Guid technicianID, Guid customerID, string type, string description, 
        int status, int priority, DateTime createDate, DateTime expectedDoneDate, string? notes)
    {
        ID = Guid.NewGuid();
        TechnicianID = technicianID;
        CustomerID = customerID;
        Type = type;
        Description = description;
        Status = status;
        Priority = priority;
        CreateDate = createDate;
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