using Core.Enums;
using Core.Models;

namespace Services.TaskServices;

public class UserInputParser : IUserInputParser
{
    public List<CommandModel> Parse(string userInput)
    {
        var commands = new List<CommandModel>();
        if (string.IsNullOrWhiteSpace(userInput)) return commands;
        var instructions = userInput.Trim().Split(';', StringSplitOptions.RemoveEmptyEntries);
        foreach (var instruction in instructions)
        {
            var parts = instruction.Trim().Split(':', 2, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2) continue;
            
            var commandType = parts[0].Trim().ToLower();
            var parameters = parts[1].Trim();

            // CommandModel für sort und filter erstellen
            switch (commandType)
            {
                case "sort":
                    commands.Add(new CommandModel { Type = CommandType.Sort, Parameters = parameters });
                    break;
                case "filter":
                    commands.Add(new CommandModel { Type = CommandType.Filter, Parameters = parameters });
                    break;
            }
        }

        return commands;
    }
}