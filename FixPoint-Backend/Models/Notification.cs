namespace FixPoint_Backend.Models;

public class Notification
{
    Guid ID { get; set; }
    Guid CaseID { get; set; }
    Guid? TechnicianID { get; set; }
    Guid? CustomerID { get; set; }
    string Text { get; set; }
    DateTime TimeStamp { get; set; }
    
    public Notification(Guid caseID, Guid? technicianID, Guid? customerID, string text, DateTime timeStamp)
    {
        ID = Guid.NewGuid();
        CaseID = caseID;
        TechnicianID = technicianID;
        CustomerID = customerID;
        Text = text;
        TimeStamp = timeStamp;
    }
}