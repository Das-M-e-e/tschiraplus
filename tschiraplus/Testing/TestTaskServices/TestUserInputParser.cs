using Core.Enums;
using Services.TaskServices;

namespace Testing.TestTaskServices;

[TestClass]
public class TestUserInputParser
{ 
    private UserInputParser _parser;
    
    [TestInitialize]
    public void Setup()
    {
        _parser = new UserInputParser();
    }
    [TestMethod]
    public void Parse_EmptyInput_ReturnsEmptyList()
    {
        var result = _parser.Parse("");

        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void Parse_WhitespaceInput_ReturnsEmptyList()
    {
        var result = _parser.Parse("   ");

        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void Parse_ValidSortCommand_ReturnsSortCommand()
    {
        var result = _parser.Parse("sort:Titel");

        Assert.AreEqual(1, result.Count);
        Assert.AreEqual(CommandType.Sort, result[0].Type);
        Assert.AreEqual("Titel", result[0].Parameters);
    }

    [TestMethod]
    public void Parse_ValidFilterCommand_ReturnsFilterCommand()
    {
        var result = _parser.Parse("filter:Status Offen");

        Assert.AreEqual(1, result.Count);
        Assert.AreEqual(CommandType.Filter, result[0].Type);
        Assert.AreEqual("Status Offen", result[0].Parameters);
    }

    [TestMethod]
    public void Parse_MultipleValidCommands_ReturnsAllCommands()
    {
        var result = _parser.Parse("sort:Titel;filter:Status Offen");

        Assert.AreEqual(2, result.Count);
        Assert.AreEqual(CommandType.Sort, result[0].Type);
        Assert.AreEqual("Titel", result[0].Parameters);
        Assert.AreEqual(CommandType.Filter, result[1].Type);
        Assert.AreEqual("Status Offen", result[1].Parameters);
    }
    [TestMethod]
    public void Parse_CommandWithExtraWhitespace_ParsesCorrectly()
    {
        var result = _parser.Parse("   sort:Titel   ");

        Assert.AreEqual(1, result.Count);
        Assert.AreEqual(CommandType.Sort, result[0].Type);
        Assert.AreEqual("Titel", result[0].Parameters);
    }

    [TestMethod]
    public void Parse_InvalidCommand_Ignored()
    {
        var result = _parser.Parse("invalid:Titel");

        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void Parse_InvalidCommandStructure_Ignored()
    {
        var result = _parser.Parse("sort:");

        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void Parse_MixedValidAndInvalidCommands_OnlyValidProcessed()
    {
        var result = _parser.Parse("sort:Titel;invalid:Test;filter:Status Offen");

        Assert.AreEqual(2, result.Count);
        Assert.AreEqual(CommandType.Sort, result[0].Type);
        Assert.AreEqual("Titel", result[0].Parameters);
        Assert.AreEqual(CommandType.Filter, result[1].Type);
        Assert.AreEqual("Status Offen", result[1].Parameters);
    }

    [TestMethod]
    public void Parse_DoubleSemicolons_ParsesCorrectly()
    {
        var result = _parser.Parse("sort:Titel;;filter:Status Offen");

        Assert.AreEqual(2, result.Count);
        Assert.AreEqual(CommandType.Sort, result[0].Type);
        Assert.AreEqual("Titel", result[0].Parameters);
        Assert.AreEqual(CommandType.Filter, result[1].Type);
        Assert.AreEqual("Status Offen", result[1].Parameters);
    }
    
}