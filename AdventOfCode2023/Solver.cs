using AdventOfCode2023.Days;


namespace AdventOfCode2023;

public class Solver
{
    public void PrintAllAnswers()
    {
        var classes = GetClasses();

        foreach (var cls in classes)
        {
            if (Activator.CreateInstance(cls) is not Day instance)
            {
                continue;
            }

            PrintParts(instance);
        }
    }

    public void PrintDay(int day)
    {
        var classes = GetClasses();
        var targetDay = classes.FirstOrDefault(cls => cls.Name == $"{nameof(Day)}{day}");
        if (targetDay != null && Activator.CreateInstance(targetDay) is Day instance)
        {
            PrintParts(instance);
        }
    }

    private static IEnumerable<Type> GetClasses()
    {
        var baseClassType = typeof(Day);
        var enumerable = AppDomain.CurrentDomain.GetAssemblies()
                                  .SelectMany(assembly => assembly.GetTypes())
                                  .Where(type => type is
                                                 {
                                                     IsClass: true,
                                                     IsAbstract: false
                                                 }
                                                 && baseClassType.IsAssignableFrom(type));
        return enumerable;
    }

    private static void PrintParts(Day day)
    {
        Console.WriteLine(day.GetType().Name);
        Console.WriteLine($"Part 1 {day.Part1()}");
        Console.WriteLine($"Part 2 {day.Part2()}");
    }
}