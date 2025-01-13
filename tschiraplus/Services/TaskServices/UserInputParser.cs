using System.ComponentModel.Design;
using Core.Enums;
using Core.Models;

namespace Services.TaskServices;

public class UserInputParser : IUserInputParser
{
    public List<CommandModel> Parse(string userinput)
    {
        var commands = new List<CommandModel>(); //leereliste
        if (string.IsNullOrWhiteSpace(userinput))
            return commands;
        var instuctions = userinput.Split(';', StringSplitOptions.RemoveEmptyEntries); //Eingabetrenung
        foreach (var instruction in instuctions)
        {
            var parts = instruction.Trim().Split(':', 2);
            if (parts.Length < 2) continue;
            
            var commandType = parts[0].Trim().ToLower();
            var parameters = parts[1].Trim();

            // CommandModel für sort und filtererstellen
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
     
    
