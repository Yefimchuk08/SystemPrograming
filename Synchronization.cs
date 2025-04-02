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
        Console.WriteLine("Starting text analysis...");
        string text = "Hello, world! This is a test.\nNew line here. Another sentence!";
        Stat textStat = new Stat();
        TextAnalyse(text, textStat);
        textStat.PrintStats();

        Console.WriteLine("\nTesting LockCounter...");
        LockCounter counter = new LockCounter();
        Parallel.For(0, 1000, _ => counter.UpdateFields());
        counter.PrintNumber();

        Console.WriteLine("\nTesting ThreadSafeCounter...");
        ThreadSafeCounter tsCounter = new ThreadSafeCounter();
        Parallel.For(0, 1000, _ => tsCounter.Increment());
        Console.WriteLine($"Final count: {tsCounter.GetValue()}");
    }

    static void TextAnalyse(string text, Stat stat)
    {
        string[] words = text.Split(new char[] { ' ', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        string[] lines = text.Split('\n');
        int punctuationCount = Regex.Matches(text, "[.,!?;:()]").Count;
        
        lock (stat)
        {
            stat.WordCount = words.Length;
            stat.LineCount = lines.Length;
            stat.PunctuationCount = punctuationCount;
        }
    }
}

class Stat
{
    public int WordCount { get; set; }
    public int LineCount { get; set; }
    public int PunctuationCount { get; set; }
    
    public void PrintStats()
    {
        Console.WriteLine($"Words: {WordCount}, Lines: {LineCount}, Punctuation: {PunctuationCount}");
    }
}

class LockCounter
{
    private int number = 0;
    private readonly object lockObject = new object();

    public void UpdateFields()
    {
        bool lockTaken = false;
        try
        {
            Monitor.Enter(lockObject, ref lockTaken);
            Interlocked.Increment(ref number);
        }
        finally
        {
            if (lockTaken) Monitor.Exit(lockObject);
        }
    }
    
    public void PrintNumber()
    {
        Console.WriteLine($"LockCounter number: {number}");
    }
}

class ThreadSafeCounter
{
    private int count = 0;
    private readonly ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim();

    public void Increment()
    {
        rwLock.EnterWriteLock();
        try
        {
            count++;
        }
        finally
        {
            rwLock.ExitWriteLock();
        }
    }
    
    public int GetValue()
    {
        rwLock.EnterReadLock();
        try
        {
            return count;
        }
        finally
        {
            rwLock.ExitReadLock();
        }
    }
}
