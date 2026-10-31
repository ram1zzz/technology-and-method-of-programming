using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalSystem.Users
{
    internal class Patient
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; } 
        public string Email { get; set; } 
        public string BloodType { get; set; }

       


        public Patient(string name, string surname, string phoneNumber, string bloodType = "", string email = "")
        {
            Name = name;
            Surname = surname;
            PhoneNumber = phoneNumber;
            BloodType = bloodType;
            Email = email;
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"Patient Name: {Name} {Surname}");
            Console.WriteLine($"Phone Number: {PhoneNumber}");
            if (!string.IsNullOrEmpty(Email))
            {
                Console.WriteLine($"Email: {Email}");
            }
            if (!string.IsNullOrEmpty(BloodType))
            {
                Console.WriteLine($"Blood Type: {BloodType}");
            }
            Console.WriteLine();
        }
    }
}
