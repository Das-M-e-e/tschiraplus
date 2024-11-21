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
                CREATE TABLE IF NOT EXISTS Tasks (
                    TaskId TEXT PRIMARY KEY,
                    Title TEXT,
                    Description TEXT,
                    Status INTEGER,
                    Priority INTEGER,
                    CreationDate DATETIME,
                    DueDate DATETIME,
                    CompletionDate DATETIME,
                    SprintId TEXT,
                    ProjectId TEXT,
                    EstimatedTime TEXT,
                    ActualTimeSpent TEXT
                )
                
                CREATE TABLE IF NOT EXISTS Task (      /* a new task table because the other one is only for testing purposes */
                    TaskId TEXT PRIMARY KEY,
                    Title TEXT not null,
                    Description TEXT,
                    Status INTEGER,
                    Priority INTEGER,
                    CreationDate DATETIME,
                    DueDate DATETIME,
                    CompletionDate DATETIME,
                    Assignees TEXT references User (UserName),
                    Tags TEXT,
                    SprintId TEXT references Sprint(SprintId),
                    ProjectId TEXT references Project(ProjectId),
                    EstimatedTime TEXT,
                    ActualTimeSpent TEXT,
                    Attachements TEXT,
                    Comments TEXT,
                    Dependencies,
                )
                
                CREATE TABLE IF NOT EXISTS User (
                    UserId TEXT PRIMARY KEY,
                    Username TEXT not null,
                    Email TEXT not null,
                    PasswordHash TEXT not null,
                    ProfilePictureUrl TEXT,
                    CreatedAt DATETIME,
                    LastLogin DATETIME,
                    Status INTEGER,
                    Bio TEXT,
                    Settings TEXT,
                    Coins INTEGER,
                    Friends TEXT,
                    Notifications TEXT,
                )
                
                CREATE TABLE IF NOT EXISTS Project (
                    ProjectId TEXT PRIMARY KEY,
                    Name TEXT not null,
                    Description TEXT,
                    CreationDate DATETIME
                    DueDate DATETIME,
                    Status INTEGER,
                    Priority INTEGER,
                    Sprints TEXT,
                    Members TEXT not null,
                    Tasks TEXT,
                    Attachements TEXT,
                    OwnerId TEXT not null references User (UserId),
                    LastUpdated DATETIME
                ) 
                
                CREATE TABLE IF NOT EXISTS Attachment (
                    AttachmentId TEXT PRIMARY KEY,
                    FileName TEXT not null,
                    FileType TEXT not null,
                    FileSize TEXT not null,
                    FilePath TEXT not null,
                    UploadedBy TEXT not null,
                    UploadedDate DATETIME,
                    TaskId TEXT not null references Task (TaskId),
                    ProjectId TEXT not null references Project (ProjectId),
                    CommentId TEXT not null references Comment (CommentId),
                    Description TEXT not null,
                )
                
                CREATE TABLE IF NOT EXISTS Comment (
                    CommentId TEXT PRIMARY KEY,
                    Content TEXT not null,
                    CreatedDate DATETIME not null,
                    LastUpdatedTime DATETIME not null,
                    AuthorId TEXT not null references User (UserId),
                    ProjectId TEXT not null references Project (ProjectId),
                    TaskId TEXT not null references Task (TaskId),
                    ParentCommentId TEXT not null references Comment (CommentId),
                    Attachements TEXT,
                    isEdited TEXT not null,
                    isDeleted TEXT not null,
                )
               
                CREATE TABLE IF NOT EXISTS Notification (
                    NotificationId TEXT PRIMARY KEY,
                    Title TEXT not null,
                    Message TEXT not null,
                    Timestamp DATETIME not null,
                    isRead INTEGER,
                    Type TEXT not null,
                    Severity TEXT not null,
                    AuthorId TEXT not null references User (UserId),
                    RecipientId TEXT not null
                )
                
                CREATE TABLE IF NOT EXISTS TaskAssignment (
                    ProjectTaskAssignmentId TEXT PRIMARY KEY,
                    UserId TEXT not null references User(UserId),
                    ProjectId TEXT not null references Project(ProjectId),
                    TaskId TEXT not null references Task(TaskId),
                    AssignedDate DATETIME
                )
                
                CREATE TABLE IF NOT EXISTS UserProjectRole (
                    UserProjectRoleId TEXT PRIMARY KEY,
                    UserId TEXT not null references User (UserId),
                    ProjectId TEXT not null references Project (ProjectId),
                    Role TEXT not null,
                    AssignedDate DATETIME,
                    IsActive TEXT not null,
                    Permissions TEXT
                )
                
                CREATE TABLE IF NOT EXISTS UserSettings (
                    UserSettingsId TEXT PRIMARY KEY,
                    UserId TEXT not null references User (UserId),
                    Theme TEXT not null,
                    Language TEXT not null,
                    NotificationsEnabled TEXT not null,
                    EmailNotificationsEnabled TEXT not null,
                    ReceiveTaskReminders TEXT not null,
                    ShowToolTips TEXT not null,
                    Timezone TEXT not null,
                    DateFormat TEXT not null
                )
                
                CREATE TABLE IF NOT EXISTS Sprint (
                    SprintId TEXT PRIMARY KEY,
                    Name TEXT not null,
                    Description TEXT,
                    StartDate DATETIME,
                    EndDate DATETIME,
                    Status INTEGER,
                    ProjectId TEXT references Project(ProjectId),
                    Tasks TEXT,
                    CreationDate DATETIME,
                    LastUpdated DATETIME
                )
                ");
        
                
               
    }
    }