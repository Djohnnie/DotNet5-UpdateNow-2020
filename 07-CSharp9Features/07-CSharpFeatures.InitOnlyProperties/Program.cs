using System;

PersonA pA = new();
pA.Name = "Hooyberghs";
pA.FirstName = "Johnny";

PersonB pB1 = new("Hooyberghs", "Johnny");
pB1.Name = "Janssans";
pB1.FirstName = "Jan";

PersonB pB2 = new()
{
    Name = "Hooyberghs",
    FirstName = "Johnny"
};

PersonC pC1 = new("Hooyberghs", "Johnny");
pC1.Name = "Janssans";
pC1.FirstName = "Jan";

PersonC pC2 = new()
{
    Name = "Hooyberghs",
    FirstName = "Johnny"
};

public class PersonA
{
    public string Name { get; set; }
    public string FirstName { get; set; }
}

public class PersonB
{
    public string Name { get; }
    public string FirstName { get; }

    public PersonB()
    {

    }

    public PersonB(string name, string firstName)
    {
        Name = name;
        FirstName = firstName;
    }
}

public class PersonC
{
    public string Name { get; init; }
    public string FirstName { get; init; }

    public PersonC()
    {

    }

    public PersonC(string name, string firstName)
    {
        Name = name;
        FirstName = firstName;
    }
}