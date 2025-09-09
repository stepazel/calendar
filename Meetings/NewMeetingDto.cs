namespace Calendar.Meetings;

public class NewMeetingDto
{
    public string ParticipantFirstName { get; set; } = string.Empty;
    public string ParticipantLastName { get; set; } = string.Empty;
    public string? ParticipantEmail { get; set; }
    public DateTime ScheduledAt { get; set; }
}