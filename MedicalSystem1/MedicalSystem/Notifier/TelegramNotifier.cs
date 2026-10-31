using System;
using MedicalSystem.Interfaces;
using MedicalSystem.Users;


namespace MedicalSystem.Notifier
{
    internal class TelegramNotifier : INotifier
    {
        public void Send(Patient patient, string message)
        {
            Console.WriteLine($"Telegram сообщение для {patient.Name} : {message}");
        }
    }
}