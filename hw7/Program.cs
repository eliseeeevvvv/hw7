using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        
        string filePath = "numbers.txt";

        if (!File.Exists(filePath))
        {
            Console.WriteLine("Файл numbers.txt не знайдено!");
            return;
        }

        List<int> numbers = File.ReadAllLines(filePath)
            .SelectMany(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            .Select(int.Parse)
            .ToList();


        // =========================
        // ЗАВДАННЯ 1
        // =========================

        Console.WriteLine("===== ЗАВДАННЯ 1 =====");

        int uniqueCount = numbers
            .AsParallel()
            .Distinct()
            .Count();

        Console.WriteLine($"Кількість унікальних значень: {uniqueCount}");


        // =========================
        // ЗАВДАННЯ 2
        // =========================

        Console.WriteLine();
        Console.WriteLine("===== ЗАВДАННЯ 2 =====");

        int maxIncreasingLength = GetMaxIncreasingSequence(numbers);

        Console.WriteLine(
            $"Довжина найбільшої зростаючої послідовності: {maxIncreasingLength}"
        );


        // =========================
        // ЗАВДАННЯ 3
        // =========================

        Console.WriteLine();
        Console.WriteLine("===== ЗАВДАННЯ 3 =====");

        int maxPositiveLength = GetMaxPositiveSequence(numbers);

        Console.WriteLine(
            $"Довжина найбільшої додатної послідовності: {maxPositiveLength}"
        );


        // =========================
        // ЗАВДАННЯ 4
        // =========================

        Console.WriteLine();
        Console.WriteLine("===== ЗАВДАННЯ 4 =====");

        Console.Write("Введіть початок діапазону: ");
        int start = int.Parse(Console.ReadLine());

        Console.Write("Введіть кінець діапазону: ");
        int end = int.Parse(Console.ReadLine());

        CreateMultiplicationTable(start, end);

        Console.WriteLine();
        Console.WriteLine("Таблицю множення записано у multiplication.txt");

        Console.ReadLine();
    }


    // =====================================================
    // ЗАВДАННЯ 2
    // Максимальна довжина зростаючої послідовності
    // =====================================================

    static int GetMaxIncreasingSequence(List<int> numbers)
    {
        if (numbers.Count == 0)
            return 0;

        
        List<int> starts = Enumerable.Range(0, numbers.Count)
            .AsParallel()
            .Where(i =>
                i == 0 ||
                numbers[i] <= numbers[i - 1])
            .ToList();

        int maxLength = starts
            .AsParallel()
            .Select(start =>
            {
                int length = 1;

                for (int i = start + 1; i < numbers.Count; i++)
                {
                    if (numbers[i] > numbers[i - 1])
                    {
                        length++;
                    }
                    else
                    {
                        break;
                    }
                }

                return length;
            })
            .Max();

        return maxLength;
    }


    // =====================================================
    // ЗАВДАННЯ 3
    // =====================================================

    static int GetMaxPositiveSequence(List<int> numbers)
    {
        if (numbers.Count == 0)
            return 0;

        
        List<int> starts = Enumerable.Range(0, numbers.Count)
            .AsParallel()
            .Where(i =>
                numbers[i] > 0 &&
                (i == 0 || numbers[i - 1] <= 0))
            .ToList();

        if (starts.Count == 0)
            return 0;

        int maxLength = starts
            .AsParallel()
            .Select(start =>
            {
                int length = 0;

                for (int i = start; i < numbers.Count; i++)
                {
                    if (numbers[i] > 0)
                    {
                        length++;
                    }
                    else
                    {
                        break;
                    }
                }

                return length;
            })
            .Max();

        return maxLength;
    }


    // =====================================================
    // ЗАВДАННЯ 4
    // =====================================================

    static void CreateMultiplicationTable(int start, int end)
    {
        if (start > end)
        {
            int temp = start;
            start = end;
            end = temp;
        }

        int count = end - start + 1;


        string[] results = new string[count];

        Parallel.For(start, end + 1, number =>
        {
            string result = "";

            for (int i = 1; i <= 10; i++)
            {
                result += $"{number}*{i} = {number * i}\n";
            }

            results[number - start] = result;
        });

        File.WriteAllText(
            "multiplication.txt",
            string.Join("", results)
        );
    }
}