namespace Calendar.Meetings;

public class MeetingRequest
{
    public int DurationMinutes { get; set; }
    public DateTime StartTime { get; set; }
    public string ParticipantFirstName { get; set; } = string.Empty;
    public string ParticipantLastName { get; set; } = string.Empty;
}
