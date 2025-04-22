using System;

unsafe struct Student
{
    public char* Name;
    public int Age;
}

class Program
{
    unsafe static void Main()
    {
        Console.WriteLine("=== Завдання 1: Пошук максимуму ===");
        int size1 = 5;
        int* arr1 = stackalloc int[size1] { 10, 25, 7, 40, 15 };
        int* maxPtr = FindMax(arr1, size1);
        Console.WriteLine($"Максимум: {*maxPtr}");

        Console.WriteLine("\n=== Завдання 2: Середнє значення ===");
        int size2 = 4;
        int* arr2 = stackalloc int[size2] { 10, 20, 30, 40 };
        float avg = CalcAverage(arr2, size2);
        Console.WriteLine($"Середнє: {avg}");

        Console.WriteLine("\n=== Завдання 3: Копіювання масиву ===");
        int size3 = 3;
        int* source = stackalloc int[size3] { 1, 2, 3 };
        int* destination = stackalloc int[size3];
        CopyArray(source, destination, size3);
        Console.Write("Скопійований масив: ");
        for (int i = 0; i < size3; i++)
            Console.Write($"{destination[i]} ");

        Console.WriteLine("\n\n=== Завдання 4: Структура Student ===");
        Student* students = stackalloc Student[3];

        string[] names = { "Ann", "Bob", "Eva" };
        int[] ages = { 18, 19, 20 };

        for (int i = 0; i < 3; i++)
        {
            char* namePtr = stackalloc char[names[i].Length + 1];
            for (int j = 0; j < names[i].Length; j++)
                namePtr[j] = names[i][j];
            namePtr[names[i].Length] = '\0';

            students[i].Name = namePtr;
            students[i].Age = ages[i];
        }

        for (int i = 0; i < 3; i++)
        {
            Console.Write($"Студент {i + 1}: ");
            char* p = students[i].Name;
            while (*p != '\0')
            {
                Console.Write(*p);
                p++;
            }
            Console.WriteLine($", Вік: {students[i].Age}");
        }

        Console.WriteLine("\n=== Завдання 5: Реверс масиву ===");
        int size5 = 6;
        int* arr5 = stackalloc int[size5] { 1, 2, 3, 4, 5, 6 };
        Console.Write("До реверсу: ");
        for (int i = 0; i < size5; i++) Console.Write($"{arr5[i]} ");

        ReverseArray(arr5, size5);
        Console.Write("\nПісля реверсу: ");
        for (int i = 0; i < size5; i++) Console.Write($"{arr5[i]} ");
    }

    unsafe static int* FindMax(int* arr, int size)
    {
        int* maxPtr = arr;
        for (int i = 1; i < size; i++)
            if (*(arr + i) > *maxPtr)
                maxPtr = arr + i;
        return maxPtr;
    }

    unsafe static float CalcAverage(int* arr, int size)
    {
        int sum = 0;
        for (int i = 0; i < size; i++)
            sum += *(arr + i);
        return (float)sum / size;
    }

    unsafe static void CopyArray(int* source, int* destination, int size)
    {
        for (int i = 0; i < size; i++)
            *(destination + i) = *(source + i);
    }

    unsafe static void ReverseArray(int* arr, int size)
    {
        int* start = arr;
        int* end = arr + size - 1;
        while (start < end)
        {
            int temp = *start;
            *start = *end;
            *end = temp;
            start++;
            end--;
        }
    }
}
