namespace Calendar.Meetings;

public class MeetingLink
{
    public Guid Id { get; set; }
    
    public int UserId { get; set; }
    
    public bool IsUsed { get; set; }
    
    public DateTime CreatedAt { get; set; }
}