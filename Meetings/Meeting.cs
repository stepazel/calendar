namespace Calendar.Meetings;

public class Meeting
{
    public DateTime ScheduledAt { get; set; }
    public string ParticipantFirstName { get; set; }
    public string ParticipantLastName { get; set; }
    public int DurationMinutes { get; set; }
}