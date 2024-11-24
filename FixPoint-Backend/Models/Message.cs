namespace FixPoint_Backend.Models;

public class Message
{
    public Guid ID { get; set; }
    public Guid CaseID { get; set; }
    public Guid? TechnicianID { get; set; }
    public Guid? CustomerID { get; set; }
    public string Text { get; set; }
    public DateTime TimeStamp { get; set; }
    
    public Message(Guid caseID, Guid? technicianID, Guid? customerID, string text, DateTime timeStamp)
    {
        ID = Guid.NewGuid();
        CaseID = caseID;
        TechnicianID = technicianID;
        CustomerID = customerID;
        Text = text;
        TimeStamp = timeStamp;
    }
    
    public Message()
    {
        
    }
    
    public Guid GetID()
    {
        return ID;
    }
}