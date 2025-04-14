using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace FinalWork_
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            RegistryKey user = Registry.CurrentUser;
            RegistryKey subKey = user.CreateSubKey(@"Software\SystemMonitor");

            //subKey.SetValue("LogEnabled", 0, RegistryValueKind.DWord);
            //subKey.SetValue("ParallelEnabled", 0, RegistryValueKind.DWord);
            //subKey.SetValue("MonitoringInterval", 0, RegistryValueKind.DWord);

            int interval = (int)subKey.GetValue("MonitoringInterval");
            if (interval > 0)
                Thread.Sleep(interval);

            Console.Write("Enter size of arr: ");
            int size = int.Parse(Console.ReadLine());
            int[] ints = ArrOfInts(size);

            int count = 0;
            if ((int)subKey.GetValue("ParallelEnabled") == 1)
            {
                Parallel.For(0, ints.Length, () => 0, (i, loop, localCount) =>
                {
                    return localCount + ints[i];
                },
                localCount => Interlocked.Add(ref count, localCount));
                Console.WriteLine("Progect goes with parallel");
            }
            else
            {
                for (int i = 0; i < ints.Length; i++)
                {
                    count += ints[i];
                }
            }

            Console.WriteLine($"Sum: {count}");

            if ((int)subKey.GetValue("LogEnabled") == 1)
            {
                Console.Write("Enter login: ");
                string login = Console.ReadLine();
                Console.Write("Enter password: ");
                string password = Console.ReadLine();


                await File.AppendAllTextAsync("monitor_log.txt", login + ": " + password + "\n");
            }
            else
            {
                Console.WriteLine("LogEnabled is FALSE — skipping logging.");
            }

            unsafe
            {
                int* data = stackalloc int[size];
                Random random = new Random();
                for (int i = 0; i < size; i++)
                {
                    data[i] = random.Next(500);
                }
                double avg = CalcAverage(data, size);
                Console.WriteLine($"Average: {avg:F2}");
            }
        }

        static int[] ArrOfInts(int size)
        {
            Random rnd = new Random();
            int[] arr = new int[size];
            for (int i = 0; i < size; i++)
            {
                arr[i] = rnd.Next(500);
            }
            return arr;
        }

        unsafe static double CalcAverage(int* data, int size)
        {
            long sum = 0;
            for (int i = 0; i < size; i++)
            {
                sum += data[i];
            }
            return (double)sum / size;
        }
    }
}
