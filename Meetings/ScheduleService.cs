namespace Calendar.Meetings;

public class ScheduleService
{
    public List<TimeSlot> GenerateAvailableSlots(DateTime date, int durationMinutes, List<PrivateMeeting> existingMeetings)
    {
        var slots = new List<TimeSlot>();
        var currentTime = new DateTime(date.Year, date.Month, date.Day, 9, 0, 0);
        var endTime = new DateTime(date.Year, date.Month, date.Day, 16, 0, 0);

        while (currentTime.AddMinutes(durationMinutes) <= endTime)
        {
            var slot = new TimeSlot
            {
                StartTime = currentTime,
                EndTime = currentTime.AddMinutes(durationMinutes),
                IsAvailable = true,
                Status = "available"
            };

            if (existingMeetings.Any(meeting => IsTimeSlotOverlapping(slot, meeting)))
            {
                slot.IsAvailable = false;
                slot.Status = "occupied";
            }

            slots.Add(slot);
            currentTime = currentTime.AddMinutes(15);
        }

        return slots;
    }

    private static bool IsTimeSlotOverlapping(TimeSlot slot, PrivateMeeting meeting) =>
        slot.StartTime < meeting.ScheduledAt.AddMinutes(meeting.DurationMinutes + 15) &&
        slot.EndTime > meeting.ScheduledAt.AddMinutes(-15);
}
