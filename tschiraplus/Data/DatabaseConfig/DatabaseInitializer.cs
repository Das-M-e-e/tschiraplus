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
        // Todo: In Projects bei OwnerId wieder hinzufügen: references User (UserId)
        db.Execute(@"
                CREATE TABLE IF NOT EXISTS Projects (
                    ProjectId TEXT PRIMARY KEY,
                    Name TEXT NOT NULL,
                    Description TEXT,
                    CreationDate TIMESTAMP,
                    DueDate TIMESTAMP,
                    Status INTEGER,
                    Priority INTEGER,
                    OwnerId TEXT,
                    LastUpdated TIMESTAMP
                );

                CREATE TABLE IF NOT EXISTS Users (
                    UserId TEXT PRIMARY KEY,
                    Username TEXT NOT NULL,
                    Email TEXT NOT NULL,
                    PasswordHash TEXT NOT NULL,
                    ProfilePictureUrl TEXT,
                    CreatedAt TIMESTAMP,
                    LastLogin TIMESTAMP,
                    Status INTEGER,
                    Bio TEXT,
                    Settings TEXT,
                    Coins INTEGER,
                    Friends TEXT,
                    Notifications TEXT
                );

                CREATE TABLE IF NOT EXISTS LocalUserSettings (
                    LocalUserSettingsId TEXT PRIMARY KEY,
                    UserId TEXT,
                    AppLanguage TEXT,
                    AppTheme TEXT,
                    StartWithLastOpenedProject TEXT,
                    LastOpenedProjectId TEXT,
                    ShowToolTips TEXT
                );
        ");
    }

    public void InitializeProjectDatabase()
    {
        using var db = _dbConfig.GetDatabase();
        db.Execute(@"
                CREATE TABLE IF NOT EXISTS Tasks (
                    TaskId TEXT PRIMARY KEY,
                    Title TEXT NOT NULL,
                    Description TEXT,
                    Status INTEGER,
                    Priority INTEGER,
                    CreationDate TIMESTAMP,
                    DueDate TIMESTAMP,
                    CompletionDate TIMESTAMP,
                    Assignees TEXT,
                    Tags TEXT,
                    SprintId TEXT references Sprints(SprintId),
                    EstimatedTime TEXT,
                    ActualTimeSpent TEXT,
                    Attachments TEXT,
                    Comments TEXT,
                    Dependencies TEXT
                );

                CREATE TABLE IF NOT EXISTS Users (
                    UserId TEXT PRIMARY KEY,
                    Username TEXT NOT NULL,
                    Email TEXT NOT NULL,
                    PasswordHash TEXT NOT NULL,
                    ProfilePictureUrl TEXT,
                    CreatedAt TIMESTAMP,
                    LastLogin TIMESTAMP,
                    Status INTEGER,
                    Bio TEXT,
                    Settings TEXT,
                    Coins INTEGER,
                    Friends TEXT,
                    Notifications TEXT
                );
                
                CREATE TABLE IF NOT EXISTS Attachments (
                    AttachmentId TEXT PRIMARY KEY,
                    FileName TEXT NOT NULL,
                    FileType TEXT NOT NULL,
                    FileSize TEXT NOT NULL,
                    FilePath TEXT NOT NULL,
                    UploadedBy TEXT NOT NULL,
                    UploadedDate TIMESTAMP,
                    TaskId TEXT references Tasks (TaskId),
                    ProjectId TEXT references Projects (ProjectId),
                    CommentId TEXT references Comments (CommentId),
                    Description TEXT NOT NULL
                );
                
                CREATE TABLE IF NOT EXISTS Comments (
                    CommentId TEXT PRIMARY KEY,
                    Content TEXT NOT NULL,
                    CreatedDate TIMESTAMP NOT NULL,
                    LastUpdatedTime TIMESTAMP NOT NULL,
                    AuthorId TEXT references Users (UserId),
                    ProjectId TEXT references Projects (ProjectId),
                    TaskId TEXT references Tasks (TaskId),
                    ParentCommentId TEXT references Comments (CommentId),
                    isEdited TEXT NOT NULL,
                    isDeleted TEXT NOT NULL
                );
               
                CREATE TABLE IF NOT EXISTS Notifications (
                    NotificationId TEXT PRIMARY KEY,
                    Title TEXT NOT NULL,
                    Message TEXT NOT NULL,
                    Timestamp TIMESTAMP NOT NULL,
                    isRead INTEGER,
                    Type TEXT NOT NULL,
                    Severity TEXT NOT NULL,
                    AuthorId TEXT references Users (UserId),
                    RecipientId TEXT NOT NULL
                );
                
                CREATE TABLE IF NOT EXISTS ProjectTaskAssignments (
                    ProjectTaskAssignmentId TEXT PRIMARY KEY,
                    UserId TEXT references Users(UserId),
                    ProjectId TEXT references Projects(ProjectId),
                    TaskId TEXT references Tasks(TaskId),
                    AssignedDate TIMESTAMP NOT NULL
                );
                
                CREATE TABLE IF NOT EXISTS UserProjectRoles (
                    UserProjectRoleId TEXT PRIMARY KEY,
                    UserId TEXT references Users (UserId),
                    ProjectId TEXT references Projects (ProjectId),
                    Role TEXT NOT NULL,
                    AssignedDate TIMESTAMP NOT NULL,
                    IsActive TEXT NOT NULL,
                    Permissions TEXT
                );
                
                CREATE TABLE IF NOT EXISTS UserSettings (
                    UserSettingsId TEXT PRIMARY KEY,
                    UserId TEXT references Users (UserId),
                    Theme TEXT NOT NULL,
                    Language TEXT NOT NULL,
                    NotificationsEnabled TEXT NOT NULL,
                    EmailNotificationsEnabled TEXT NOT NULL,
                    ReceiveTaskReminders TEXT NOT NULL,
                    ShowToolTips TEXT NOT NULL,
                    Timezone TEXT NOT NULL,
                    DateFormat TEXT NOT NULL
                );
                
                CREATE TABLE IF NOT EXISTS Sprints (
                    SprintId TEXT PRIMARY KEY,
                    Name TEXT NOT NULL,
                    Description TEXT,
                    StartDate TIMESTAMP NOT NULL,
                    EndDate TIMESTAMP NOT NULL,
                    Status INTEGER,
                    ProjectId TEXT references Projects(ProjectId),
                    Tasks TEXT,
                    CreationDate TIMESTAMP NOT NULL,
                    LastUpdated TIMESTAMP
                );
        ");
    }
}