namespace Patient;

public class Class1 // these are all properties below, not to be confused with fields
{
    public string? Name { get; set; }
    public string? Race { get; set; } // multiple choice dropdown
    public string? Gender { get; set; } // multiple choice dropdown
    public string? Address { get; set; }
    public string? Birthdate { get; set; } // multiple choice for months, enter a day, enter a year (1900 to 2025) -- birthdate should be in the form "06/10/2005"
}
