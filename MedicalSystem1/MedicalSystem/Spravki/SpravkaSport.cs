using System;
using System.Collections.Generic;
using System.Text;
using MedicalSystem.Interfaces;
using MedicalSystem.Users;

namespace MedicalSystem.Spravki
{
    internal class SpravkaSport : ISpravka
    {
        public string SpravkaName { get; set; }
        public Patient Patient { get; set; }
        public decimal Price { get; set; }
        public int validMonths { get; set; }
        public int ValidMonths { get ; set; }


        public void DisplayInfo()
        {
            Console.WriteLine($"Spravka Name: {SpravkaName}");
            Console.WriteLine($"Patient: {Patient.Name} {Patient.Surname}");
            Console.WriteLine($"Price: {Price}");
            Console.WriteLine($"Valid Months: {validMonths}");
        }

        public decimal CalculatePrice()
        {
            return Price;
        }

        public string GetShortInfo()
        {
             return $"{SpravkaName}: {CalculatePrice()}";
        }
    }
}
