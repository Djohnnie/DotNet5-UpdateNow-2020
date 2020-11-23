using System;
using System.Reflection;

namespace _03_SingleFileApplications
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Assembly.GetEntryAssembly().Location);
            Console.ReadKey();
        }
    }
}