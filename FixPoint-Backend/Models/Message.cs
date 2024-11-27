namespace FixPoint_Backend.Models;

public class Message
{
    public Guid ID { get; set; }
    public Guid CaseFK { get; set; }
    public Guid? TechnicianFK { get; set; }
    public Guid? CustomerFK { get; set; }
    public string Text { get; set; }
    public DateTime TimeStamp { get; set; }
    
    public Message(Guid caseID, Guid? technicianID, Guid? customerID, string text, DateTime timeStamp)
    {
        ID = Guid.NewGuid();
        CaseFK = caseID;
        TechnicianFK = technicianID;
        CustomerFK = customerID;
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