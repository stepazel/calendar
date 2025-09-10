using System.Data;
using Dapper;

namespace Calendar.Meetings;

public class MeetingService
{
    private readonly IDbConnection _db;

    public MeetingService(IDbConnection db)
    {
        _db = db;
    }

    public async Task<string> GenerateLink(int userId, string baseUri)
    {
        const string sql = "insert into MeetingLinks (UserId) output inserted.Id values (@UserId)";
        var linkGuid = await _db.QuerySingleAsync<Guid>(sql, new { UserId = userId });
        return $"{baseUri}/sjednani-schuzky/{linkGuid}";
    }
    
    public async Task<MeetingLink?> GetMeetingLinkAsync(Guid id)
    {
        const string sql = "SELECT * FROM MeetingLinks WHERE Id = @Id";
        return await _db.QueryFirstOrDefaultAsync<MeetingLink>(sql, new { Id = id });
    }
    
    public async Task<List<PrivateMeeting>> GetUserMeetingsAsync(int userId, DateTime month)
    {
        var start = new DateTime(month.Year, month.Month, 1);

        const string sql = @"
            SELECT m.ScheduledAt, m.DurationMinutes
            FROM Meetings m
            INNER JOIN MeetingLinks ml ON m.MeetingLinkId = ml.Id
            WHERE ml.UserId = @UserId AND m.ScheduledAt >= @Start";

        return (await _db.QueryAsync<PrivateMeeting>(sql, new { UserId = userId, Start = start })).ToList();
    }
    
    public async Task CreateMeetingAsync(Guid linkId, NewMeetingDto meeting)
    {
        const string sql = @"
            INSERT INTO Meetings (MeetingLinkId, ParticipantFirstName, ParticipantLastName, ParticipantEmail, ScheduledAt, DurationMinutes)
            VALUES (@LinkId, @ParticipantFirstName, @ParticipantLastName, @ParticipantEmail, @ScheduledAt, @DurationMinutes)";

        await _db.ExecuteAsync(sql, new
        {
            LinkId = linkId,
            meeting.ParticipantFirstName,
            meeting.ParticipantLastName,
            meeting.ParticipantEmail,
            meeting.ScheduledAt,
            meeting.DurationMinutes,
        });
    }

    public async Task MarkLinkAsUsedAsync(Guid linkId)
    {
        const string sql = "UPDATE MeetingLinks SET IsUsed = 1 WHERE Id = @Id";
        await _db.ExecuteAsync(sql, new { Id = linkId });
    }
}