using MedicalManagement.Models;
Console.WriteLine("Welcome to Jaylon's Medical Management App!\n");
PrintMenu();
var userChoice = Console.ReadLine();
Console.Clear();
var patients = new List<Patient>();
var physicians = new List<Physician>();
Dictionary<string, string> validRaceOptions = new() // using a dictionary for easy user input checking and storing into the Patient object
{
    { "A", "American Indian or Alaska Native" },
    { "B", "Asian" },
    { "C", "Black or African American" },
    { "D", "Native Hawaiian or Other Pacific Islander" },
    { "E", "White" },
    { "F", "Prefer not to say" },
};
Dictionary<string, string> validGenderOptions = new() // using a dictionary for easy user input checking and storing into the Patient object
{
    { "A", "Male" },
    { "B", "Female" },
    { "C", "Other" },
    { "D", "Prefer not to say" },
};
while (userChoice != "D" && userChoice != "d")
{
    if (userChoice == "A" || userChoice == "a")
    {
        Console.WriteLine("Creating a Patient...");
        Console.WriteLine("Enter Full Name: ");
        var name = CheckName();
        Console.WriteLine("Enter Address: ");
        var address = CheckEmptyInput("address");
        Console.WriteLine("Enter Birthday (MM/DD/YYYY): ");
        var input = Console.ReadLine();
        Console.Clear();
        DateOnly birthday = CheckDate(input, "birthday");
        RaceOptions();
        var race = Console.ReadLine();
        Console.Clear();
        while (string.IsNullOrWhiteSpace(race) || !validRaceOptions.ContainsKey(race.ToUpper())) // while the string the user gives us is NOT a key in the dictionary
        {
            Console.WriteLine("Invalid input. Please enter a valid race option.");
            RaceOptions();
            race = Console.ReadLine();
            Console.Clear();
        }
        GenderOptions();
        var gender = Console.ReadLine();
        Console.Clear();
        while (string.IsNullOrWhiteSpace(gender) || !validGenderOptions.ContainsKey(gender.ToUpper())) // while the string the user gives us is NOT a key in the dictionary
        {
            Console.WriteLine("Invalid input. Please enter a valid gender option.");
            GenderOptions();
            gender = Console.ReadLine();
            Console.Clear();
        }
        var patient = new Patient(name, validRaceOptions[race.ToUpper()], validGenderOptions[gender.ToUpper()], address, birthday);
        patients.Add(patient);
        Console.WriteLine("Patient created successfully!");
        Console.WriteLine(patient.ToString());
    }
    else if (userChoice == "B" || userChoice == "b")
    {
        Console.WriteLine("Creating a Physician...");
        Console.WriteLine("Enter Full Name: ");
        var name = CheckName();
        Console.WriteLine("Enter License Number: ");
        var licenseNumber = CheckEmptyInput("license number");
        Console.WriteLine("Enter graduation date: ");
        var input = Console.ReadLine();
        Console.Clear();
        DateOnly graduation = CheckDate(input, "graduation");
        Console.WriteLine("Enter your main specialization: ");
        var specialization = CheckEmptyInput("specialization");
        var physician = new Physician(name, licenseNumber, specialization, graduation);
        physicians.Add(physician);
        Console.WriteLine("Physician created successfully!");
        Console.WriteLine(physician.ToString());
    }
    else if (userChoice == "C" || userChoice == "c")
    {
        if (patients.Count == 0 || physicians.Count == 0) // if there are no patients or physicians we can't create an appointment
        {
            Console.WriteLine("Create a Patient and Physician first, try again.");
        }
        else
        {
            Console.WriteLine("Creating an Appointment...");
            var patientIds = patients.Select(p => p.Id).ToList();
            var physicianIds = physicians.Select(p => p.Id).ToList();
            PrintObjects(patients);
            Console.WriteLine("Choose a Patient (ID): ");
            var patientVerifiedID = Console.ReadLine();
            Console.Clear();
            while (string.IsNullOrEmpty(patientVerifiedID) || !patientIds.Contains(patientVerifiedID))
            {
                PrintObjects(patients);
                Console.WriteLine("Invalid input. Please enter an ID: ");
                patientVerifiedID = Console.ReadLine();
                Console.Clear();
            }
            PrintObjects(physicians);
            Console.WriteLine("Choose a Physician (ID): ");
            var physicianVerifiedID = Console.ReadLine();
            Console.Clear();
            while (string.IsNullOrEmpty(physicianVerifiedID) || !physicianIds.Contains(physicianVerifiedID))
            {
                PrintObjects(physicians);
                Console.WriteLine("Invalid input. Please enter an ID: ");
                physicianVerifiedID = Console.ReadLine();
                Console.Clear();
            }
            Patient? patient = patients.FirstOrDefault(p => p.Id == patientVerifiedID);
            Physician? physician = physicians.FirstOrDefault(p => p.Id == physicianVerifiedID);
            DateOnly earliestDate = DateOnly.FromDateTime(DateTime.Today.AddDays(7));
            DateOnly latestDate = earliestDate.AddMonths(1);
            Console.WriteLine($"Note: Appointments can only be made from {earliestDate} to {latestDate} and time from 8:00 (8:00AM) to 17:00 (5:00PM) only.");
            Console.WriteLine("Enter appointment date and time (MM/DD/YYYY HH:mm) (use military time): ");
            string? input = Console.ReadLine();
            Console.Clear();
            DateTime requestedTime;
            while (string.IsNullOrWhiteSpace(input) || !DateTime.TryParse(input, out requestedTime) || !physician!.IsValidAppointment(requestedTime))
            {
                Console.WriteLine($"Note: Appointments can only be made from {earliestDate} to {latestDate} and time from 8:00 (8:00AM) to 17:00 (5:00PM) only.");
                Console.WriteLine("Invalid input. Enter appointment date and time (MM/DD/YYYY HH:mm) (use military time): ");
                input = Console.ReadLine();
                Console.Clear();
            }
            var appointment = new Appointment(physician, patient!, requestedTime);
            physician.Appointments.Add(appointment);
            patient!.Appointments.Add(appointment);
            Console.WriteLine("Appointment created successfully!");
            Console.WriteLine(appointment.ToString());
            Physician.CreateNotes(patient!);
        }
    }
    else
    {
        Console.WriteLine("Invalid input. Try again.");
    }
    PrintMenu();
    userChoice = Console.ReadLine();
    Console.Clear();
}
static void PrintMenu()
{
    Console.WriteLine("Menu Options:\nA. Create a Patient\nB. Create a Physician\nC. Create an Appointment\nD. Quit\nEnter Choice: ");
}
static void RaceOptions()
{
    Console.WriteLine("Race Options:\nA. American Indian or Alaska Native\nB. Asian\nC. Black or African American\nD. Native Hawaiian or Other Pacific Islander\nE. White\nF. Prefer not to say\nEnter Choice: ");
}
static void GenderOptions()
{
    Console.WriteLine("Gender Options:\nA. Male\nB. Female\nC. Other\nD. Prefer not to say\nEnter Choice: ");
}
static DateOnly CheckDate(string? input, string context)
{
    DateOnly date;
    while (string.IsNullOrWhiteSpace(input) || !DateOnly.TryParse(input, out date) || date.Year < 1900 || date > DateOnly.FromDateTime(DateTime.Now))
    {
        Console.WriteLine($"Invalid {context} date. Please enter again (MM/dd/yyyy): ");
        input = Console.ReadLine();
        Console.Clear();
    }
    return date;
}
static string CheckName()
{
    var name = Console.ReadLine();
    Console.Clear();
    while (string.IsNullOrWhiteSpace(name) || int.TryParse(name, out int randomNum))
    {
        Console.WriteLine("Invalid input. Please enter a full name (string).\nEnter name: ");
        name = Console.ReadLine();
        Console.Clear();
    }
    return name;
}
static string CheckEmptyInput(string context)
{
    var input = Console.ReadLine();
    Console.Clear();
    while (string.IsNullOrWhiteSpace(input))
    {
        Console.WriteLine($"Invalid input. Please enter a valid {context}.\nEnter {context}: ");
        input = Console.ReadLine();
        Console.Clear();
    }
    return input;
}
static void PrintObjects<T>(List<T> list)
{
    foreach (var item in list)
    {
        Console.WriteLine(item?.ToString());
    }
}