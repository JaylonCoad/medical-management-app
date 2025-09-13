namespace Physician;

public class Class1
{
    public string? Name { get; set; }
    public string? LicenseNumber { get; set; }
    public string? GraduationDate { get; set; } // multiple choice for months, enter a day, enter a year (1900 to 2025) -- should be in the form "06/10/2005"
    public List<string> Specializations { get; set; } = new(); // do i need the ?
}

// physician class will have data: name, license number, graduation date, and specializations
