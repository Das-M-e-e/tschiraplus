namespace Core;

//Nur ein absolutes Skellet von einer Klasse werde später noch mehr daran arbeiten
public class Task
{
    
    String titel { get; set; }
    String[] description { get; set; } //String Array um Zeilen-Umbrüche zu handeln
    
    /* Yet to implement
     Date Begin {get; set; };
     Date End
      Or somthing like that
    */

    public task(String titel, String[] description)
    {
        this.titel = titel;
        this.description = description;
    }
    
    
}