using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

class Program
{


    class User
    {
        public string Name { get; set; }
        public long Numbers { get; set; }
        public double Balance { get; set; }

        public User()
        {

        }
        public User(string n, long nu, double b)
        {
            Name = n;
            Numbers = nu;
            Balance = b;
        }
        public double GetBalance()
        {
            return Balance;
        }
        public void GetCash(int cash)
        {
            if (cash > Balance)
            {
                Console.WriteLine($"Oups {Name}. You don`t this sum. Try another sum!");
            }
            else
            {
                this.Balance = Balance - cash;
                Console.WriteLine($"Congratulation {Name}! You can get this: {cash}$. Your balance: {Balance}");
            }
        }
        public void SetCash(int cash)
        {
            this.Balance = Balance + cash;
            Console.WriteLine($"Congratulation {Name}! You can set this: {cash}$. Your balance: {Balance}");
        }
        static void Main()
        {
            // task 1
            Random rnd = new Random();
            List<int> list = new List<int>();
            Stopwatch sw = new Stopwatch();

            for (int i = 0; i < 10000000; i++)
            {
                list.Add(rnd.Next(100));
            }

            sw.Start();
            for (int i = 0; i < list.Count; i++)
            {
                int count = 0;
                count += list[i];
            }
            sw.Stop();
            Console.WriteLine($"for : {sw.ElapsedMilliseconds}ms");

            sw.Restart();
            Parallel.For (0, list.Count, i =>
            {
                int count = 0;
                count += list[i];
            }) ;
            sw.Stop();
            Console.WriteLine($"parallel for : {sw.ElapsedMilliseconds}ms");

            // task 2
            sw.Restart();
            foreach (int i in list)
            {
                int count = 0;
                count += i;
            }
            sw.Stop();
            Console.WriteLine($"foreach : {sw.ElapsedMilliseconds}ms");

            sw.Restart();
            Parallel.ForEach (list, num =>
            {
                int count = 0;
                count += num;
            });
            sw.Stop();
            Console.WriteLine($"parallel foreach : {sw.ElapsedMilliseconds}ms");

            // task 3
            List<int> list_of_num = GenereList();
            Parallel.Invoke(
                () => { Console.WriteLine("list of num: "); foreach (var i in list_of_num) { Console.WriteLine(i); } },
                () => Console.WriteLine($"count ints % 2 == 0: {CountInts(list_of_num)}"),
                () => Console.WriteLine($"average: {AVG(list_of_num)}"),
                () => Console.WriteLine($"max elem: {MaxElem(list_of_num)}")
            );
            //task 4
            string[] filepaths = { "task1.txt", "task2.txt", "task3.txt", "task4.txt", "task5.txt" };


            int[] task1 = { 1, 2, 3, 4, 5 };
            File.WriteAllText(filepaths[0], string.Join(" ", task1));


            int[] task2 = { 6, 7, 8, 9, 10 };
            File.WriteAllText(filepaths[1], string.Join(" ", task2));


            int[] task3 = { 11, 12, 13, 14, 15 };
            File.WriteAllText(filepaths[2], string.Join(" ", task3));


            int[] task4 = { 16, 17, 18, 19, 20 };
            File.WriteAllText(filepaths[3], string.Join(" ", task4));

            int[] task5 = { 21, 22, 23, 24, 25 };
            File.WriteAllText(filepaths[4], string.Join(" ", task5));

            Parallel.ForEach (filepaths, file =>
            {
                string content = File.ReadAllText(file);
                int count = content.Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
                Console.WriteLine($"{file}: {count} чисел");
            });

            //Task 5
            User user1 = new User("Dima", 123456789123, 250.0);
            User user2 = new User("Sasha", 32145685212, 350.0);
            User user3 = new User("Ivan", 789541236457, 450.0);
            User user4 = new User("Lev", 369258147741, 260.0);
            User user5 = new User("Misha", 111222333444, 750.0);
            Parallel.Invoke(
                () => Console.WriteLine($"{user1.Name}`s balance: {user1.GetBalance()}"),
                () => user1.GetCash(100),
                () => user1.SetCash(100),
                () => Console.WriteLine($"{user2.Name}`s balance: {user2.GetBalance()}") ,
                () => user2.GetCash(100),
                () => user2.SetCash(100),
                () => Console.WriteLine($"{user3.Name}`s balance:{user3.GetBalance()}"),
                () => user3.GetCash(100),
                () => user3.SetCash(100),
                () => Console.WriteLine($"{user4.Name}`s balance:{user4.GetBalance()}"),
                () => user4.GetCash(100),
                () => user4.SetCash(100),
                () => Console.WriteLine($"{user5.Name}`s balance:{user5.GetBalance()}"),
                () => user5.GetCash(100),
                () => user5.SetCash(100)



            );
        }

        static List<int> GenereList()
        {
            List<int> list = new List<int>();
            Random random = new Random();
            for (int i = 0; i < 100000; i++)
            {
                list.Add(random.Next(100000));
            }
            return list;
        }

        static int CountInts(List<int> list)
        {
            int count = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] % 2 == 0)
                {
                    count++;
                }
            }
            return count;
        }

        static double AVG(List<int> list)
        {
            double sum = 0;
            for (int i = 0; i < list.Count; i++)
            {
                sum += list[i];
            }
            return sum / list.Count;
        }

        static int MaxElem(List<int> list)
        {
            int max = list[0];
            for (int i = 1; i < list.Count; i++)
            {
                if (list[i] > max)
                {
                    max = list[i];
                }
            }
            return max;
        }
    }
}
