using Core.Models;

namespace Services.TaskServices;

public interface IUserInputParser
{
    List<CommandModel> Parse(string userinput);
}