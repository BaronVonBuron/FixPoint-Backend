namespace FixPoint_Backend.Models;

public class Case
{
    Guid ID { get; set; }
    Guid TechnicianID { get; set; }
    Guid CustomerID { get; set; }
    string Type { get; set; }
    string Description { get; set; }
    int Status { get; set; }
    int Priority { get; set; }
    DateTime CreateDate { get; set; }
    DateTime ExpectedDoneDate { get; set; }
    string? Notes { get; set; }
    
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
}