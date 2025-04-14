using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncExamples
{
    class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }

    class Program
    {
        // Task 1
        static async Task StartTimerAsync(int seconds, CancellationToken token)
        {
            for (int i = seconds; i >= 0; i--)
            {
                if (token.IsCancellationRequested)
                {
                    Console.WriteLine("⏹ Таймер зупинено!");
                    return;
                }

                Console.WriteLine($"⏳ Залишилось: {i} сек.");
                await Task.Delay(1000, token);
            }
            Console.WriteLine("✅ Таймер завершено!");
        }

        // Task 2
        static Task<ulong> CalculateFactorialAsync(int number) => Task.Run(() =>
        {
            ulong result = 1;
            for (uint i = 2; i <= number; i++)
                result *= i;
            return result;
        });

        // Task 3
        static async Task<Post> FetchPostAsync(int id)
        {
            using var client = new HttpClient();
            string url = $"https://jsonplaceholder.typicode.com/posts/{id}";
            var json = await client.GetStringAsync(url);
            return JsonSerializer.Deserialize<Post>(json);
        }

        // Task 4
        static async Task ScheduleTaskAsync(string taskName, int delaySeconds)
        {
            await Task.Delay(delaySeconds * 1000);
            Console.WriteLine($"🕒 Завдання '{taskName}' виконано через {delaySeconds} сек.");
        }

        // Task 5
        static async Task<string> FetchDataWithRetryAsync(string url, int maxRetries = 3)
        {
            using var client = new HttpClient();
            int attempt = 0;

            while (attempt < maxRetries)
            {
                try
                {
                    attempt++;
                    var result = await client.GetStringAsync(url);
                    return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"⚠️ Спроба {attempt} не вдалася: {ex.Message}");
                    if (attempt >= maxRetries) throw;
                    await Task.Delay(1000);
                }
            }
            return null;
        }

        static async Task Main()
        {
            Console.WriteLine("=== Завдання 1: Таймер з відміною ===");
            using var cts = new CancellationTokenSource();
            var timerTask = StartTimerAsync(10, cts.Token);

            _ = Task.Run(() =>
            {
                Console.ReadKey();
                cts.Cancel();
            });
            await timerTask;

            Console.WriteLine("\n=== Завдання 2: Факторіали ===");
            var factorialTasks = Enumerable.Range(10, 6)
                                           .Select(CalculateFactorialAsync)
                                           .ToArray();

            var factorialResults = await Task.WhenAll(factorialTasks);
            for (int i = 0; i < factorialResults.Length; i++)
                Console.WriteLine($"Факторіал {10 + i}! = {factorialResults[i]}");

            Console.WriteLine("\n=== Завдання 3: Завантаження JSON ===");
            var postIds = new[] { 1, 2, 3 };
            var postTasks = postIds.Select(FetchPostAsync).ToArray();
            var posts = await Task.WhenAll(postTasks);
            foreach (var post in posts)
                Console.WriteLine($"ID {post.Id}: {post.Title}");

            Console.WriteLine("\n=== Завдання 4: Планувальник задач ===");
            var taskList = new List<(string, int)>
            {
                ("Завдання A", 3),
                ("Завдання B", 5),
                ("Завдання C", 2)
            };
            var scheduledTasks = taskList.Select(t => ScheduleTaskAsync(t.Item1, t.Item2)).ToArray();
            await Task.WhenAll(scheduledTasks);

            Console.WriteLine("\n=== Завдання 5: Повторна спроба завантаження ===");
            string url = "https://jsonplaceholder.typicode.com/posts/1";
            try
            {
                string data = await FetchDataWithRetryAsync(url);
                Console.WriteLine($"✅ Дані отримано: {data.Substring(0, Math.Min(100, data.Length))}...");
            }
            catch
            {
                Console.WriteLine("❌ Всі спроби не вдалися.");
            }

            Console.WriteLine("\n🎉 Всі завдання виконані!");
        }
    }
}
