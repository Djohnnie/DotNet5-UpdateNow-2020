using System;
using System.Runtime.InteropServices;

Console.WriteLine("Hello World!");
FromWhom();
Show.Excitement("Top-level programs can be brief", 8);

void FromWhom()
{
    Console.WriteLine($"From {RuntimeInformation.FrameworkDescription}");
}

internal class Show
{
    internal static void Excitement(string message, int levelOf)
    {
        Console.Write(message);

        for (int i = 0; i < levelOf; i++)
        {
            Console.Write("!");
        }

        Console.WriteLine();
    }
}