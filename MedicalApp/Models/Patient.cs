namespace MedicalApi.Models;

public class Patient
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public DateOnly BirthDate { get; set; }
    public string Address { get; set; } = "";
    public string PhoneNumber { get; set; } = "";
}