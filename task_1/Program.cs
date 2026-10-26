using System;
using System.Collections.Generic;
using System.Linq;

namespace CalorieCounter;

// ==========================================
// 1. ИНТЕРФЕЙСЫ
// ==========================================


/// Интерфейс для продуктов питания

public interface IFoodItem
{
    string Name { get; }
    double CaloriesPer100g { get; }

    // Метод расчета калорий на основе веса в граммах
    double CalculateCalories(double grams);
}


/// Интерфейс для сервиса учета калорий

public interface ITracker
{
    void ShowAvailableProducts();
    void AddProduct(Product product);
    void EatProduct(int productIndex, double grams);
    void ShowDailySummary();
}

// ==========================================
// 2. КЛАССЫ
// ==========================================

/// <summary>
/// Класс продукта, реализующий интерфейс IFoodItem
/// </summary>
public class Product : IFoodItem
{
    public string Name { get; set; }
    public double CaloriesPer100g { get; set; }

    public Product(string name, double caloriesPer100g)
    {
        Name = name;
        CaloriesPer100g = caloriesPer100g;
    }

    public double CalculateCalories(double grams)
    {
        return (CaloriesPer100g * grams) / 100.0;
    }

    public override string ToString()
    {
        return $"{Name} — {CaloriesPer100g} ккал / 100г";
    }
}

/// Класс для записи о съеденной порции еды

public class EatenFood
{
    public IFoodItem Food { get; set; }
    public double WeightInGrams { get; set; }

    // Вычисляемое свойство для итоговых калорий порции
    public double TotalCalories => Food.CalculateCalories(WeightInGrams);

    public EatenFood(IFoodItem food, double weightInGrams)
    {
        Food = food;
        WeightInGrams = weightInGrams;
    }

    public override string ToString()
    {
        return $"{Food.Name} ({WeightInGrams} г) — {TotalCalories:F1} ккал";
    }
}

/// <summary>
/// Класс трекера калорий, реализующий ITracker
/// </summary>
public class CalorieTracker : ITracker
{
    public double DailyLimit { get; set; }
    public List<Product> Products { get; set; }
    public List<EatenFood> MealsToday { get; set; }

    public CalorieTracker(double dailyLimit)
    {
        DailyLimit = dailyLimit;
        Products = new List<Product>();
        MealsToday = new List<EatenFood>();

        // Добавим несколько базовых продуктов для старта
        Products.Add(new Product("Яблоко", 52));
        Products.Add(new Product("Банан", 89));
        Products.Add(new Product("Куриное филе", 113));
        Products.Add(new Product("Гречка (вареная)", 110));
        Products.Add(new Product("Овсянка", 350));
        Products.Add(new Product("Творог 5%", 120));
    }

    // Вывод списка доступных продуктов
    public void ShowAvailableProducts()
    {
        Console.WriteLine("\n--- СПИСОК ПРОДУКТОВ ---");
        for (int i = 0; i < Products.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {Products[i]}");
        }
    }

    // Добавление нового продукта в базу
    public void AddProduct(Product product)
    {
        Products.Add(product);
        Console.WriteLine($"\nПродукт \"{product.Name}\" успешно добавлен в базу!");
    }

    // Запись съеденной порции
    public void EatProduct(int productIndex, double grams)
    {
        if (productIndex < 0 || productIndex >= Products.Count)
        {
            Console.WriteLine("Ошибка: неверный номер продукта.");
            return;
        }

        var product = Products[productIndex];
        var entry = new EatenFood(product, grams);
        MealsToday.Add(entry);

        Console.WriteLine($"\nДобавлено: {entry}");
    }

    // Вывод статистики за сегодня
    public void ShowDailySummary()
    {
        Console.WriteLine("\n================ СТАТИСТИКА ЗА СЕГОДНЯ ================");
        
        if (MealsToday.Count == 0)
        {
            Console.WriteLine("Сегодня еще ничего не съедено.");
        }
        else
        {
            Console.WriteLine("Съеденные продукты:");
            for (int i = 0; i < MealsToday.Count; i++)
            {
                Console.WriteLine($"  {i + 1}. {MealsToday[i]}");
            }
        }

        double totalConsumed = MealsToday.Sum(m => m.TotalCalories);
        double remaining = DailyLimit - totalConsumed;

        Console.WriteLine("------------------------------------------------------");
        Console.WriteLine($"Дневная норма: {DailyLimit:F0} ккал");
        Console.WriteLine($"Всего съедено: {totalConsumed:F1} ккал");

        if (remaining >= 0)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Осталось до нормы: {remaining:F1} ккал");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Превышение нормы на: {Math.Abs(remaining):F1} ккал!");
        }
        Console.ResetColor();
        Console.WriteLine("======================================================");
    }
}

// ==========================================
// 3. ТОЧКА ВХОДА (МЕНЮ)
// ==========================================

internal class Program
{
    private static void Main(string[] args)
    {
        // Создаем трекер с дневной нормой 2000 ккал
        ITracker tracker = new CalorieTracker(2000);

        bool running = true;
        while (running)
        {
            Console.WriteLine("\n=== СЧЕТЧИК КАЛОРИЙ ===");
            Console.WriteLine("1. Посмотреть съеденное за день");
            Console.WriteLine("2. Записать прием пищи");
            Console.WriteLine("3. Посмотреть базу продуктов");
            Console.WriteLine("4. Добавить новый продукт в базу");
            Console.WriteLine("5. Изменить дневную норму калорий");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите действие: ");

            string? choice = Console.ReadLine();
            if (choice == null) break;

            switch (choice)
            {
                case "1":
                    tracker.ShowDailySummary();
                    break;

                case "2":
                    EatFood(tracker);
                    break;

                case "3":
                    tracker.ShowAvailableProducts();
                    break;

                case "4":
                    AddNewProduct(tracker);
                    break;

                case "5":
                    ChangeLimit(tracker);
                    break;

                case "0":
                    running = false;
                    Console.WriteLine("До свидания!");
                    break;

                default:
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                    break;
            }
        }
    }

    // Метод для записи съеденного
    private static void EatFood(ITracker tracker)
    {
        tracker.ShowAvailableProducts();
        Console.Write("\nВведите номер продукта: ");
        if (!int.TryParse(Console.ReadLine(), out int index))
        {
            Console.WriteLine("Некорректный ввод номера.");
            return;
        }

        Console.Write("Введите вес в граммах: ");
        if (!double.TryParse(Console.ReadLine(), out double grams) || grams <= 0)
        {
            Console.WriteLine("Вес должен быть положительным числом.");
            return;
        }

        tracker.EatProduct(index - 1, grams);
    }

    // Метод для добавления нового продукта
    private static void AddNewProduct(ITracker tracker)
    {
        Console.Write("\nВведите название продукта: ");
        string? name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Название не может быть пустым.");
            return;
        }

        Console.Write("Введите калории на 100 грамм: ");
        if (!double.TryParse(Console.ReadLine(), out double calories) || calories < 0)
        {
            Console.WriteLine("Калории должны быть неотрицательным числом.");
            return;
        }

        tracker.AddProduct(new Product(name, calories));
    }

    // Метод для смены дневного лимита
    private static void ChangeLimit(ITracker tracker)
    {
        if (tracker is CalorieTracker ct)
        {
            Console.WriteLine($"\nТекущая норма: {ct.DailyLimit} ккал");
            Console.Write("Введите новую норму калорий: ");
            if (double.TryParse(Console.ReadLine(), out double newLimit) && newLimit > 0)
            {
                ct.DailyLimit = newLimit;
                Console.WriteLine($"Норма успешно изменена на {newLimit} ккал!");
            }
            else
            {
                Console.WriteLine("Некорректное значение.");
            }
        }
    }
}