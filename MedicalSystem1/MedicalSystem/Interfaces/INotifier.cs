using MedicalSystem.Users;

namespace MedicalSystem.Interfaces
{
    internal interface INotifier
    {
        void Send(Patient patient, string message);
    }
}