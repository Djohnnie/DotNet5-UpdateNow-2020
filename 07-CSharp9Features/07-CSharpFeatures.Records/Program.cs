using System;

A a = new();

PersonA pA = new()
{
    Name = "Hooyberghs",
    FirstName = "Johnny",
    A = a
};

PersonA pB = new()
{
    Name = "Hooyberghs",
    FirstName = "Johnny",
    A = a
};

Console.WriteLine(pA == pB);

PersonB pB = new PersonB("Hooyberghs", "Johnny");
pB.Name = "Janssens";
pB.FirstName = "Jan";

PersonC pC = new("Hooyberghs", "Johnny")
{
    Age = 35
};

PersonB personB = new();

PersonB person1 = new("Hooyberghs", "Johnny");
PersonB person2 = person1 with { FirstName = "Marina" };


public record PersonA
{
    public string Name { get; init; }
    public string FirstName { get; init; }
    public A A { get; init; }
}

public record PersonB(string Name, string FirstName);

public record PersonC(string Name, string FirstName)
{
    public byte? Age { get; init; }
}

public class A
{

}