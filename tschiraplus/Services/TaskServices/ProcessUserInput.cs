using Services.DTOs;

namespace Services.TaskServices;

public interface ProcessUserInput
{
    public IEnumerable<TaskDto> ProcessUserInput(string userinput, IEnumerable<TaskDto> tasks);
   
}