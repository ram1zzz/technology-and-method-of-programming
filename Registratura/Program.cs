List<Patient> queue = new List<Patient>();
int lastTicket = 0;

while (true)
{
    Console.WriteLine();
    Console.WriteLine("1 - встать в очередь   2 - вызвать следующего   3 - табло   0 - выход");
    Console.Write("> ");
    string? command = Console.ReadLine();

    if (command == null || command == "0")
    {
        break;
    }
    else if (command == "1")
    {
        Console.Write("Имя: ");
        string name = Console.ReadLine() ?? "";
        lastTicket++;
        Patient patient = new Patient { Ticket = lastTicket, Name = name };
        queue.Add(patient);
        Console.WriteLine($"Ваш талон: {patient.Ticket}. Перед вами {queue.Count - 1} чел.");
    }
    else if (command == "2")
    {
        Patient next = queue[0];
        queue.RemoveAt(0);
        Console.WriteLine($"Талон {next.Ticket}, {next.Name}, пройдите в кабинет");
    }
    else if (command == "3")
    {
        Console.WriteLine($"В очереди {queue.Count} чел.:");
        foreach (Patient p in queue)
        {
            Console.WriteLine($"  талон {p.Ticket}: {p.Name}");
        }
    }
    else
    {
        Console.WriteLine("Нет такой команды");
    }
}