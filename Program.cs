using System;
using System.Collections.Generic;
using System.Linq;

namespace Redox_Code_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> numbers = Enumerable.Range(1, 100).ToList();

            Console.WriteLine("Even numbers:");
            var evenNumbers = numbers.Where(n => n % 2 == 0);
            Console.WriteLine(string.Join(", ", evenNumbers));
            Console.WriteLine();

            Console.WriteLine("Numbers divisible by 3 or 5, but not both:");
            List<int> result = new List<int>();
            
            foreach (int number in numbers)
            {
                bool divisibleBy3 = number % 3 == 0;
                bool divisibleBy5 = number % 5 == 0;
                
                if (divisibleBy3 ^ divisibleBy5)
                {
                    result.Add(number);
                }
            }
            
            Console.WriteLine(string.Join(", ", result));
        }
    }
}
