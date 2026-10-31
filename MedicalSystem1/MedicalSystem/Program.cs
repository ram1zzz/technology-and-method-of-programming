using MedicalSystem.Users;
using MedicalSystem.Spravki;
using MedicalSystem.Interfaces;
using MedicalSystem.Buhgalteria;
using MedicalSystem.Notifier;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;


Patient patient = new Patient("Aktan", "Asanov", "+996 555 123 456","1-", "aktan@example.com");
Patient patient2 = new Patient("John", "Doe", "+996 777 666 555", "2+", "john@example.com");


List<Patient> Users = new List<Patient> { patient, patient2 };
foreach (var user in Users)
{
    user.DisplayInfo();
    Console.WriteLine();
}

Spravka83 spravka83 = new Spravka83
{
    SpravkaName = "Spravka 83",
    Patient = patient,
    Price = 500
};

SpravkaSanknijka spravkaSanknijka = new SpravkaSanknijka
{
    SpravkaName = "Spravka Sanknijka",
    Patient = patient,
    Price = 1000,
    AnalysisCount = 3
};

SpravkaSport spravkaSport = new SpravkaSport
{
    SpravkaName = "Spravka Sport",
    Patient = patient2,
    Price = 300,
    validMonths = 1,

};

List<ISpravka> spravkiList = new List<ISpravka> { spravka83, spravkaSanknijka, spravkaSport };
Console.WriteLine("Информация о справках: ");
foreach (var spravka in spravkiList)
{
    spravka.DisplayInfo();
    Console.WriteLine($"Total Price: {spravka.CalculatePrice()}");
    Console.WriteLine();
}

Console.WriteLine("Краткая информация о справках:");
foreach (var spravka in spravkiList)
{
    Console.WriteLine(spravka.GetShortInfo());
}

Kassa kassa = new Kassa();

decimal totalAmount = kassa.Total(spravkiList);

Console.WriteLine();
Console.WriteLine($"Total Amount in Kassa: {totalAmount}");
Console.WriteLine();

kassa.PrintCheck(spravkiList);
Console.WriteLine();

SmsNotifier notifier1 = new SmsNotifier();
EmailNotifier notifier2 = new EmailNotifier();
TelegramNotifier notifier3 = new TelegramNotifier();


Console.Write("Как уведомить пациента? 1 - SMS, 2 - Email, 3 - Telegram: ");
string? choice = Console.ReadLine();

if (choice == "1")
{
    notifier1.Send(patient, "Ваша справка готова");
}
else if (choice == "2")
{
    notifier2.Send(patient, "Ваша справка готова");
}
else if (choice == "3")
{
    notifier3.Send(patient, "Ваша справка готова");
}
else
{
    Console.WriteLine("Неверный выбор уведомления.");
}
