using System;

class Program
{
    unsafe static void Main()
    {
        Console.WriteLine("=== Завдання 1: Об'єднання двох масивів ===");
        Console.Write("Введіть розмір масиву A: ");
        int m = int.Parse(Console.ReadLine());
        Console.Write("Введіть розмір масиву B: ");
        int n = int.Parse(Console.ReadLine());

        int* A = stackalloc int[m];
        int* B = stackalloc int[n];

        Console.WriteLine("Введіть елементи масиву A:");
        for (int i = 0; i < m; i++)
            A[i] = int.Parse(Console.ReadLine());

        Console.WriteLine("Введіть елементи масиву B:");
        for (int i = 0; i < n; i++)
            B[i] = int.Parse(Console.ReadLine());

        int total = m + n;
        int* C = stackalloc int[total];

        for (int i = 0; i < m; i++) C[i] = A[i];
        for (int i = 0; i < n; i++) C[m + i] = B[i];

        Console.Write("Об'єднаний масив: ");
        for (int i = 0; i < total; i++)
            Console.Write($"{C[i]} ");

        Console.WriteLine("\n\n=== Завдання 2: Видалення непарних ===");

        int* filtered = FilterEvenNumbers(C, total, out int newSize);
        Console.Write("Масив без непарних: ");
        for (int i = 0; i < newSize; i++)
            Console.Write($"{filtered[i]} ");

        Console.WriteLine("\n\nНатисніть Enter для завершення...");
        Console.ReadLine();
    }

    unsafe static int* FilterEvenNumbers(int* arr, int size, out int newSize)
    {
        newSize = 0;
        for (int i = 0; i < size; i++)
            if (arr[i] % 2 == 0) newSize++;

        int* result = stackalloc int[newSize];
        int index = 0;

        for (int i = 0; i < size; i++)
            if (arr[i] % 2 == 0)
                result[index++] = arr[i];

        return result;
    }
}
