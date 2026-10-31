using MedicalSystem.Interfaces;
using MedicalSystem.Users;
using System;

namespace MedicalSystem.Notifier
{
    internal class SmsNotifier : INotifier
    {
        public void Send(Patient patient, string message)
        {
            Console.WriteLine($"SMS на номер {patient.PhoneNumber}: {message}");
        }
    } 
}


