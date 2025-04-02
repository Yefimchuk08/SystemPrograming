using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        RunTasks();
        TaskWithReturnValue();
        TaskWithExceptionHandling();
        FactorialTask();
        PrintNumbersTask();
    }

    static void RunTasks()
    {
        Task task1 = Task.Delay(1000).ContinueWith(_ => Console.WriteLine("Task 1 completed"));
        Task task2 = Task.Delay(2000).ContinueWith(_ => Console.WriteLine("Task 2 completed"));
        Task task3 = Task.Delay(3000).ContinueWith(_ => Console.WriteLine("Task 3 completed"));
        
        Task.WhenAll(task1, task2, task3).Wait();
        Console.WriteLine("All tasks completed");
    }

    static void TaskWithReturnValue()
    {
        Task<int> task = Task.Run(() => {
            Random rnd = new Random();
            int num = rnd.Next(1, 11);
            Console.WriteLine($"Generated number: {num}");
            return num;
        });

        int result = task.Result;
        if (result > 5)
        {
            Console.WriteLine($"Square: {result * result}");
        }
        else
        {
            Console.WriteLine("Too small number");
        }
    }

    static void TaskWithExceptionHandling()
    {
        Task task = Task.Run(() => throw new Exception("Something went wrong"));
        task.ContinueWith(t =>
        {
            if (t.IsFaulted)
                Console.WriteLine($"Error: {t.Exception?.InnerException?.Message}");
        }).Wait();
    }

    static void FactorialTask()
    {
        Task<long> factorialTask = Task.Run(() => Factorial(10));
        long result = factorialTask.Result;
        Console.WriteLine($"Factorial of 10: {result}");
    }

    static long Factorial(int n)
    {
        long fact = 1;
        for (int i = 2; i <= n; i++)
        {
            fact *= i;
        }
        return fact;
    }

    static void PrintNumbersTask()
    {
        Task printingTask = Task.Run(() =>
        {
            for (int i = 1; i <= 5; i++)
            {
                Console.WriteLine(i);
                Thread.Sleep(500);
            }
        });

        Console.WriteLine("Main thread is working...");
        printingTask.Wait();
    }
}
