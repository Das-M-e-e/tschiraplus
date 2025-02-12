using Core.Enums;

namespace Core.Models;

public class CommandModel
{
    public CommandType Type { get; set; }
    public string Parameters { get; set; }
    
}