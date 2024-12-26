namespace Data.DatabaseConfig;

public class DatabaseInitializer
{
    private readonly PetaPocoConfig _dbConfig;

    public DatabaseInitializer(PetaPocoConfig dbConfig)
    {
        _dbConfig = dbConfig;
    }

    public void InitializeDatabase()
    {
        using var db = _dbConfig.GetDatabase();
        
        db.Execute(@"
                CREATE TABLE IF NOT EXISTS Users (
                    UserId TEXT PRIMARY KEY,
                    Username TEXT NOT NULL,
                    Email TEXT NOT NULL,
                    ProfilePictureUrl TEXT,
                    Bio TEXT,
                    IsActivated INTEGER NOT NULL,
                    Status INTEGER,
                    CreatedAt TIMESTAMP,
                    LastLogin TIMESTAMP
                );

                CREATE TABLE IF NOT EXISTS Projects (
                    ProjectId TEXT PRIMARY KEY,
                    OwnerId TEXT REFERENCES Users(UserId),
                    Name TEXT NOT NULL,
                    Description TEXT,
                    Status INTEGER,
                    Priority INTEGER,
                    CreationDate TIMESTAMP,
                    StartDate TIMESTAMP,
                    DueDate TIMESTAMP,
                    LastUpdated TIMESTAMP
                );

                CREATE TABLE IF NOT EXISTS Sprints (
                    SprintId TEXT PRIMARY KEY,
                    ProjectId TEXT REFERENCES Projects(ProjectId),
                    AuthorId TEXT REFERENCES Users(UserId),
                    Name TEXT NOT NULL,
                    Description TEXT,
                    Status INTEGER,
                    StartDate TIMESTAMP NOT NULL,
                    EndDate TIMESTAMP NOT NULL,
                    CreationDate TIMESTAMP NOT NULL,
                    LastUpdated TIMESTAMP
                );

                CREATE TABLE IF NOT EXISTS Tasks (
                    TaskId TEXT PRIMARY KEY,
                    ProjectId TEXT REFERENCES Projects(ProjectId),
                    SprintId TEXT REFERENCES Sprints(SprintId),
                    AuthorId TEXT REFERENCES Users(UserId),
                    Title TEXT NOT NULL,
                    Description TEXT,
                    Status INTEGER,
                    Priority INTEGER,
                    CreationDate TIMESTAMP,
                    DueDate TIMESTAMP,
                    CompletionDate TIMESTAMP,
                    LastUpdated TIMESTAMP,
                    EstimatedTime TEXT,
                    ActualTimeSpent TEXT
                );

                CREATE TABLE IF NOT EXISTS Attachments (
                    AttachmentId TEXT PRIMARY KEY,
                    ProjectId TEXT REFERENCES Projects(ProjectId),
                    TaskId TEXT REFERENCES Tasks(TaskId),
                    CommentId TEXT REFERENCES Comments(CommentId),
                    UploadedBy TEXT REFERENCES Users(UserId),
                    FileName TEXT NOT NULL,
                    FilePath TEXT NOT NULL,
                    Description TEXT NOT NULL,
                    FileSize INTEGER NOT NULL,
                    FileType INTEGER NOT NULL,
                    UploadedDate TIMESTAMP
                );
                
                CREATE TABLE IF NOT EXISTS Comments (
                    CommentId TEXT PRIMARY KEY,
                    ProjectId TEXT REFERENCES Projects(ProjectId),
                    TaskId TEXT REFERENCES Tasks(TaskId),
                    AuthorId TEXT REFERENCES Users(UserId),
                    ParentCommentId TEXT REFERENCES Comments(CommentId),
                    Content TEXT NOT NULL,
                    CreatedDate TIMESTAMP NOT NULL,
                    LastUpdatedTime TIMESTAMP NOT NULL,
                    isEdited INTEGER NOT NULL,
                    isDeleted INTEGER NOT NULL
                );
               
                CREATE TABLE IF NOT EXISTS Notifications (
                    NotificationId TEXT PRIMARY KEY,
                    AuthorId TEXT REFERENCES Users(UserId),
                    RecipientId TEXT REFERENCES Users(UserId),
                    Title TEXT,
                    Message TEXT NOT NULL,
                    Type INTEGER NOT NULL,
                    Severity INTEGER NOT NULL,
                    SentAt TIMESTAMP NOT NULL,
                    isRead INTEGER NOT NULL
                );

                CREATE TABLE IF NOT EXISTS Tags (
                    TagId TEXT PRIMARY KEY,
                    ProjectId TEXT REFERENCES Projects(ProjectId),
                    AuthorId TEXT REFERENCES Users(UserId),
                    Title TEXT NOT NULL,
                    Description TEXT,
                    ColorCode TEXT NOT NULL,
                    CreatedAt TIMESTAMP NOT NULL
                );
                
                CREATE TABLE IF NOT EXISTS UserSettings (
                    UserSettingsId TEXT PRIMARY KEY,
                    UserId TEXT REFERENCES Users(UserId),
                    NotificationsEnabled INTEGER NOT NULL,
                    EmailNotificationsEnabled INTEGER NOT NULL,
                    ReceiveTaskReminders INTEGER NOT NULL,
                    Timezone TEXT NOT NULL,
                    DateFormat TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS ProjectUsers (
                    ProjectUserId TEXT PRIMARY KEY,
                    ProjectId TEXT REFERENCES Projects(ProjectId),
                    UserId TEXT REFERENCES Users(UserId),
                    AssignedAt TIMESTAMP NOT NULL
                );

                CREATE TABLE IF NOT EXISTS UserTaskAssignments (
                    UserTaskAssignmentId TEXT PRIMARY KEY,
                    ProjectId TEXT REFERENCES Projects(ProjectId),
                    UserId TEXT REFERENCES Users(UserId),
                    TaskId TEXT REFERENCES Tasks(TaskId),
                    AssignedBy TEXT REFERENCES Users(UserId),
                    AssignedDate TIMESTAMP NOT NULL
                );
                
                CREATE TABLE IF NOT EXISTS UserProjectRoles (
                    UserProjectRoleId TEXT PRIMARY KEY,
                    ProjectId TEXT REFERENCES Projects(ProjectId),
                    UserId TEXT REFERENCES Users(UserId),
                    AssignedBy TEXT REFERENCES Users(UserId),
                    Role TEXT NOT NULL,
                    AssignedDate TIMESTAMP NOT NULL,
                    IsActive INTEGER NOT NULL
                );

                CREATE TABLE IF NOT EXISTS TaskTags (
                    TaskTagId TEXT PRIMARY KEY,
                    ProjectId TEXT REFERENCES Projects(ProjectId),
                    TaskId TEXT REFERENCES Tasks(TaskId),
                    TagId TEXT REFERENCES Tags(TagId),
                    AssignedBy TEXT REFERENCES Users(UserId),
                    AssignedAt TIMESTAMP NOT NULL
                );

                CREATE TABLE IF NOT EXISTS UserFriends (
                    UserFriendId TEXT PRIMARY KEY,
                    UserId TEXT REFERENCES Users(UserId),
                    FriendId TEXT REFERENCES Users(UserId),
                    BefriendedAt TIMESTAMP NOT NULL
                );
        ");
    }
}