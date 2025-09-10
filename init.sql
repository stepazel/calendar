create table Users
(
    Id    int identity
        primary key,
    Name  nvarchar(100) not null,
    Email nvarchar(255) not null
        unique
)

create table MeetingLinks
(
    Id        uniqueidentifier default newid()          not null
        primary key,
    UserId    int                                       not null
        constraint FK_MeetingLinks_Users
            references Users,
    IsUsed    bit              default 0                not null,
    CreatedAt datetime2        default sysutcdatetime() not null
)

create table Meetings
(
    Id                   int identity
        primary key,
    MeetingLinkId        uniqueidentifier                   not null
        constraint FK_Meetings_MeetingLinks
            references MeetingLinks,
    ScheduledAt          datetime2                          not null,
    ParticipantFirstName nvarchar(100)                      not null,
    ParticipantLastName  nvarchar(100)                      not null,
    ParticipantEmail     nvarchar(255)                      not null,
    CreatedAt            datetime2 default sysutcdatetime() not null,
    DurationMinutes      int
)


