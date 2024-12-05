namespace Data.DatabaseConfig;

public class DatabaseInitializer
{
    private readonly PetaPocoConfig _dbConfig;

    public DatabaseInitializer(PetaPocoConfig dbConfig) //Konstroktor
    {
        _dbConfig = dbConfig;
    }

    public void InitializeDatabase() //Initialisiert die Datenbank und legt Tabellen an TODO (Müsste man nicht erst noch alle existierenden Tabellen löschen?)
    {
        using var db = _dbConfig.GetDatabase();
        db.Execute(@"
                CREATE TABLE IF NOT EXISTS Tasks (
                    TaskId TEXT PRIMARY KEY,
                    Title TEXT NOT NULL,
                    Description TEXT,
                    Status INTEGER NOT NULL,
                    Priority INTEGER,
                    CreationDate TIMESTAMP,  /* Timestamp because it converts the times into the Timezone in which the User is working from. */
                    DueDate TIMESTAMP,
                    CompletionDate TIMESTAMP,
                    SprintId TEXT,
                    ProjectId TEXT,
                    EstimatedTime TEXT,
                    ActualTimeSpent TEXT
                );
                
                CREATE TABLE IF NOT EXISTS Task (      /* a new task table because the other one is only for testing purposes */
                    TaskId TEXT PRIMARY KEY,
                    Title TEXT NOT NULL,
                    Description TEXT,
                    Status INTEGER,
                    Priority INTEGER,
                    CreationDate TIMESTAMP,
                    DueDate TIMESTAMP,
                    CompletionDate TIMESTAMP,
                    Assignees TEXT references User (UserName),
                    Tags TEXT,
                    SprintId TEXT references Sprint(SprintId),
                    ProjectId TEXT references Project(ProjectId),
                    EstimatedTime TEXT,
                    ActualTimeSpent TEXT,
                    Attachements TEXT,
                    Comments TEXT,
                    Dependencies TEXT
                );
                
                CREATE TABLE IF NOT EXISTS User (
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
                
                CREATE TABLE IF NOT EXISTS Project (
                    ProjectId TEXT PRIMARY KEY,
                    Name TEXT NOT NULL,
                    Description TEXT,
                    CreationDate TIMESTAMP,
                    DueDate TIMESTAMP,
                    Status INTEGER,
                    Priority INTEGER,
                    Sprints TEXT,
                    Members TEXT NOT NULL,
                    Tasks TEXT,
                    Attachements TEXT,
                    OwnerId TEXT references User (UserId),
                    LastUpdated TIMESTAMP
                );
                
                CREATE TABLE IF NOT EXISTS Attachment (
                    AttachmentId TEXT PRIMARY KEY,
                    FileName TEXT NOT NULL,
                    FileType TEXT NOT NULL,
                    FileSize TEXT NOT NULL,
                    FilePath TEXT NOT NULL,
                    UploadedBy TEXT NOT NULL,
                    UploadedDate TIMESTAMP,
                    TaskId TEXT references Task (TaskId),
                    ProjectId TEXT references Project (ProjectId),
                    CommentId TEXT references Comment (CommentId),
                    Description TEXT NOT NULL
                );
                
                CREATE TABLE IF NOT EXISTS Comment (
                    CommentId TEXT PRIMARY KEY,
                    Content TEXT NOT NULL,
                    CreatedDate TIMESTAMP NOT NULL,
                    LastUpdatedTime TIMESTAMP NOT NULL,
                    AuthorId TEXT references User (UserId),
                    ProjectId TEXT references Project (ProjectId),
                    TaskId TEXT references Task (TaskId),
                    ParentCommentId TEXT references Comment (CommentId),
                    Attachements TEXT,
                    isEdited TEXT NOT NULL,
                    isDeleted TEXT NOT NULL
                );
               
                CREATE TABLE IF NOT EXISTS Notification (
                    NotificationId TEXT PRIMARY KEY,
                    Title TEXT NOT NULL,
                    Message TEXT NOT NULL,
                    Timestamp TIMESTAMP NOT NULL,
                    isRead INTEGER,
                    Type TEXT NOT NULL,
                    Severity TEXT NOT NULL,
                    AuthorId TEXT references User (UserId),
                    RecipientId TEXT NOT NULL
                );
                
                CREATE TABLE IF NOT EXISTS TaskAssignment (
                    ProjectTaskAssignmentId TEXT PRIMARY KEY,
                    UserId TEXT references User(UserId),
                    ProjectId TEXT references Project(ProjectId),
                    TaskId TEXT references Task(TaskId),
                    AssignedDate TIMESTAMP NOT NULL
                );
                
                CREATE TABLE IF NOT EXISTS UserProjectRole (
                    UserProjectRoleId TEXT PRIMARY KEY,
                    UserId TEXT references User (UserId),
                    ProjectId TEXT references Project (ProjectId),
                    Role TEXT NOT NULL,
                    AssignedDate TIMESTAMP NOT NULL,
                    IsActive TEXT NOT NULL,
                    Permissions TEXT
                );
                
                CREATE TABLE IF NOT EXISTS UserSettings (
                    UserSettingsId TEXT PRIMARY KEY,
                    UserId TEXT references User (UserId),
                    Theme TEXT NOT NULL,
                    Language TEXT NOT NULL,
                    NotificationsEnabled TEXT NOT NULL,
                    EmailNotificationsEnabled TEXT NOT NULL,
                    ReceiveTaskReminders TEXT NOT NULL,
                    ShowToolTips TEXT NOT NULL,
                    Timezone TEXT NOT NULL,
                    DateFormat TEXT NOT NULL
                );
                
                CREATE TABLE IF NOT EXISTS Sprint (
                    SprintId TEXT PRIMARY KEY,
                    Name TEXT NOT NULL,
                    Description TEXT,
                    StartDate TIMESTAMP NOT NULL,
                    EndDate TIMESTAMP NOT NULL,
                    Status INTEGER,
                    ProjectId TEXT references Project(ProjectId),
                    Tasks TEXT,
                    CreationDate TIMESTAMP NOT NULL,
                    LastUpdated TIMESTAMP
                );

                ");
        
                
               
    }
    }