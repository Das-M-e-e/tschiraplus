using Services.UserServices;

namespace Testing.TestTaskServices;

[TestClass]
[DoNotParallelize]
public class TestTokenStorageService
{
    private string _testFilePath;
    private TokenStorageService _tokenStorageService;

    [TestInitialize]
    public void Setup()
    {
        // Temporäre Datei für Tests setzen
        _testFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testAuthToken.txt");

        _tokenStorageService = new TokenStorageService(_testFilePath);
    }

    [TestCleanup]
    public void Cleanup()
    {
        // Testdatei nach jedem Test entfernen
        if (File.Exists(_testFilePath))
            File.Delete(_testFilePath);
    }
    [TestMethod]
    public void SaveToken_LoadToken_ReturnsSameToken()
    {
        // Arrange
        string originalToken = "ey123.fake.jwt.token";

        // Act
        _tokenStorageService.SaveToken(originalToken);
        string? loadedToken = _tokenStorageService.LoadToken();

        // Assert
        Assert.IsNotNull(loadedToken);
        Assert.AreEqual(originalToken, loadedToken);
    }

    [TestMethod]
    public void SaveToken_RemoveToken_FileShouldBeDeleted()
    {
        // Arrange
        _tokenStorageService.SaveToken("TestToken");

        // Act
        _tokenStorageService.RemoveToken();

        // Assert
        Assert.IsFalse(File.Exists(_testFilePath));
    }

    [TestMethod]
    public void LoadToken_WhenFileDoesNotExist_ReturnsNull()
    {
        // Act
        string? loadedToken = _tokenStorageService.LoadToken();

        // Assert
        Assert.IsNull(loadedToken);
    }

    [TestMethod]
    public void SaveToken_FileContentIsEncrypted()
    {
        // Arrange
        string token = "SensitiveToken123";

        // Act
        _tokenStorageService.SaveToken(token);
        string storedContent = File.ReadAllText(_testFilePath);

        // Assert
        Assert.IsFalse(storedContent.Contains(token)); // Token darf nicht im Klartext gespeichert sein
    }
    [TestMethod]
    public void SaveToken_OverwritePreviousToken_NewTokenIsSaved()
    {
        // Arrange
        string firstToken = "FirstToken";
        string secondToken = "SecondToken";

        // Act
        _tokenStorageService.SaveToken(firstToken);
        _tokenStorageService.SaveToken(secondToken);
        string? loadedToken = _tokenStorageService.LoadToken();

        // Assert
        Assert.IsNotNull(loadedToken);
        Assert.AreEqual(secondToken, loadedToken);
    }
    
}