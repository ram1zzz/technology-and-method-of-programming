using MedicalSystem.Interfaces;
using MedicalSystem.Users;
using System;


namespace MedicalSystem.Notifier
{
    internal class EmailNotifier : INotifier
    {
        public void Send(Patient patient, string message)
        {
            Console.WriteLine($"Email на адрес {patient.Email}: {message}");
        }
    }
}
