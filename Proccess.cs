using System;
using System.Diagnostics;
using System.Threading;

class Program
{
    static void Main()
    {
        Console.WriteLine("Starting notepad.exe...");
        Process notepadProcess = new Process();
        notepadProcess.StartInfo.FileName = "notepad.exe";
        notepadProcess.Start();
        
        Thread.Sleep(5000);
        
        if (!notepadProcess.HasExited)
        {
            Console.WriteLine("Notepad is still running. Press any key to close it.");
            Console.ReadKey();
            
            try
            {
                notepadProcess.Kill();
                Console.WriteLine("Notepad has been closed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error closing notepad: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Notepad has already been closed.");
        }
    }
}
