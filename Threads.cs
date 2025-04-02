using System;
using System.Threading;

class Program
{
    static void Main()
    {
        
        Console.WriteLine("Task 1: Threads with different priorities");
        Thread[] threads = new Thread[5];
        ThreadPriority[] priorities = { ThreadPriority.Lowest, ThreadPriority.BelowNormal, ThreadPriority.Normal, ThreadPriority.AboveNormal, ThreadPriority.Highest };
        
        for (int i = 0; i < 5; i++)
        {
            threads[i] = new Thread(ThreadTask) { Name = $"Thread {i + 1}" };
            threads[i].Priority = priorities[i];
            threads[i].Start();
        }

        Thread.Sleep(2000);
        Console.WriteLine("\nTask 2: Waiting for threads to complete");

        
        Thread t1 = new Thread(Task1);
        Thread t2 = new Thread(Task2);
        
        t1.Start();
        t2.Start();
        
        t1.Join();
        t2.Join();
        
        Console.WriteLine("Both threads have finished execution.");
        
        Thread.Sleep(1000);
        Console.WriteLine("\nTask 3: Using ThreadPool");
        
      
        ThreadPool.QueueUserWorkItem(_ => Task1());
        ThreadPool.QueueUserWorkItem(_ => Task2());
        ThreadPool.QueueUserWorkItem(_ => Task3());
        
        Thread.Sleep(2000);
        Console.WriteLine("\nTask 4: Passing multiple parameters to a thread");
        
        
        for (int i = 1; i <= 3; i++)
        {
            Thread thread = new Thread(ThreadFunc);
            thread.Start(new ThreadData { ID = i, SleepTime = i * 1000 });
        }
    }

    static void ThreadTask()
    {
        Console.WriteLine($"{Thread.CurrentThread.Name} with priority {Thread.CurrentThread.Priority} is running.");
    }

    static void Task1()
    {
        Console.WriteLine("Task1 is executing");
        Thread.Sleep(1000);
    }

    static void Task2()
    {
        Console.WriteLine("Task2 is executing");
        Thread.Sleep(1000);
    }

    static void Task3()
    {
        Console.WriteLine("Task3 is executing");
        Thread.Sleep(1000);
    }

    class ThreadData
    {
        public int ID { get; set; }
        public int SleepTime { get; set; }
    }

    static void ThreadFunc(object data)
    {
        ThreadData threadData = (ThreadData)data;
        Console.WriteLine($"Thread {threadData.ID} started, sleeping for {threadData.SleepTime}ms");
        Thread.Sleep(threadData.SleepTime);
        Console.WriteLine($"Thread {threadData.ID} finished");
    }
}
