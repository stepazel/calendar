namespace Calendar.Meetings;

public class TimeSlot
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool IsAvailable { get; set; }
    public string Status { get; set; } = string.Empty; // "available", "occupied", "unavailable"
}
