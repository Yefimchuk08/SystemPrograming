using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AsyncTraining
{
    class Program
    {
        // Task 1
        static async Task PrintNumbersAsync()
        {
            for (int i = 1; i <= 10; i++)
            {
                Console.WriteLine($"Число: {i}");
                await Task.Delay(1000);
            }
        }

        // Task 2
        static async Task<string> DownloadFileAsync(string fileName, int delay)
        {
            await Task.Delay(delay);
            if (fileName == "File2") throw new Exception($"❌ Помилка при завантаженні {fileName}");
            return $"✅ {fileName} завантажено за {delay / 1000} сек.";
        }

        // Task 4
        static async Task<int> CalculateSquareAsync(int number)
        {
            await Task.Delay(500);
            return number * number;
        }

        // Task 5
        static async Task<int> GetPageLengthAsync(string url)
        {
            using var client = new HttpClient();
            var html = await client.GetStringAsync(url);
            return html.Length;
        }

        static async Task Main()
        {
            Console.WriteLine("=== 🧪 Завдання 1: Асинхронний лічильник ===");
            await PrintNumbersAsync();

            Console.WriteLine("\n=== 🧪 Завдання 2-3: Завантаження файлів з обробкою помилок ===");

            var files = new List<(string, int)>
            {
                ("File1", 2000),
                ("File2", 3000), 
                ("File3", 1000)
            };

            var downloadTasks = files.Select(file => Task.Run(async () =>
            {
                try
                {
                    var result = await DownloadFileAsync(file.Item1, file.Item2);
                    Console.WriteLine(result);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"⚠️ {ex.Message}");
                }
            })).ToArray();

            await Task.WhenAll(downloadTasks);

            Console.WriteLine("\n=== 🧪 Завдання 4: Обчислення квадратів ===");
            var inputNumbers = new List<int>();

            for (int i = 0; i < 5; i++)
            {
                Console.Write($"Введи число {i + 1}: ");
                if (int.TryParse(Console.ReadLine(), out int num))
                    inputNumbers.Add(num);
                else
                    Console.WriteLine("❗ Це не число, пропускаємо!");
            }

            var squareTasks = inputNumbers.Select(CalculateSquareAsync).ToArray();
            var squares = await Task.WhenAll(squareTasks);

            for (int i = 0; i < squares.Length; i++)
                Console.WriteLine($"Квадрат {inputNumbers[i]} = {squares[i]}");

            Console.WriteLine("\n=== 🧪 Завдання 5: Завантаження з сайтів ===");
            var urls = new[]
            {
                "https://www.google.com",
                "https://www.bbc.com",
                "https://www.microsoft.com"
            };

            var lengthTasks = urls.Select(GetPageLengthAsync).ToArray();
            var lengths = await Task.WhenAll(lengthTasks);

            for (int i = 0; i < urls.Length; i++)
                Console.WriteLine($"🌐 {urls[i]} => {lengths[i]} символів");

            Console.WriteLine("\n🎉 Всі завдання завершені!");
        }
    }
}
