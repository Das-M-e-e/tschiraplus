using Core.Enums;
using TaskStatus = Core.Enums.TaskStatus;

namespace Core;

//Nur ein absolutes Skellet von einer Klasse werde später noch mehr daran arbeiten
public class Task
{
    
    String titel { get; set; }
    String[] description { get; set; } //String Array um Zeilen-Umbrüche zu handeln
    TaskPriority priority { get; set; }
    TaskStatus status { get; set; }
    
    /* Yet to implement
     Date Begin {get; set; };
     Date End
      Or somthing like that
    */

    public  Task(String titel, String[] description, TaskPriority priority, TaskStatus status)
    {
        this.titel = titel;
        this.description = description;
        this.priority = priority;
        this.status = status;
    }
    
    
}