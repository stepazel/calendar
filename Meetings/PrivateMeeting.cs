namespace Calendar.Meetings;

public class PrivateMeeting
{
    public DateTime ScheduledAt { get; set; }
    
    public int DurationMinutes { get; set; }

    public string Title { get; set; } = "Schůzka";
}